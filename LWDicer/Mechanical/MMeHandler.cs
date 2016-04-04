using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static LWDicer.Control.DEF_Error;
using static LWDicer.Control.DEF_Common;
using static LWDicer.Control.DEF_MeHandler;
using static LWDicer.Control.DEF_Motion;
using static LWDicer.Control.DEF_IO;
using static LWDicer.Control.DEF_Vacuum;

namespace LWDicer.Control
{
    public class DEF_MeHandler
    {
        public const int ERR_HANDLER_UNABLE_TO_USE_IO                = 1;
        public const int ERR_HANDLER_UNABLE_TO_USE_CYL               = 2;
        public const int ERR_HANDLER_UNABLE_TO_USE_VCC               = 3;
        public const int ERR_HANDLER_UNABLE_TO_USE_AXIS              = 4;
        public const int ERR_HANDLER_UNABLE_TO_USE_VISION            = 5;
        public const int ERR_HANDLER_NOT_ORIGIN_RETURNED             = 6;
        public const int ERR_HANDLER_INVALID_AXIS                    = 7;
        public const int ERR_HANDLER_INVALID_PRIORITY                = 8;
        public const int ERR_HANDLER_NOT_SAME_POSITION               = 9;
        public const int ERR_HANDLER_LCD_VCC_ABNORMALITY             = 10;
        public const int ERR_HANDLER_VACUUM_ON_TIME_OUT              = 11;
        public const int ERR_HANDLER_VACUUM_OFF_TIME_OUT             = 12;
        public const int ERR_HANDLER_INVALID_PARAMETER               = 13;

        public enum EHandlerType
        {
            NONE = -1,
            AXIS = 0,
            CYL,
        }

        public enum EHandlerVacuum
        {
            SELF,           // 자체 발생 진공
            FACTORY,        // 공장 진공
            OBJECT,         // LCD 패널의 PCB같은 걸 집는 용도
            EXTRA_SELF,     // 
            EXTRA_FACTORY,  //
            EXTRA_OBJECT,   //
            MAX,
        }

        public enum EHandlerPos
        {
            NONE = -1,
            WAIT = 0,
            TURN,
            LOAD,
            UNLOAD,
            LOAD_Z_UP,      // Loading Pos(x,y,t) + Z Up
            UNLOAD_Z_UP,    // Unloading Pos(x,y,t) + Z Up
            MAX,
        }

        public class CHandlerPos
        {
            public CPosition[] Pos = new CPosition[(int)EHandlerPos.MAX];

            public CHandlerPos()
            {
                for (int i = 0; i < Pos.Length; i++)
                {
                    Pos[i] = new CPosition();
                }
            }
        }

        public class CMeHandlerRefComp
        {
            public IIO IO;

            // Cylinder
            // 4축에 대응하는 방식 Upstr/Downstr, Front/Back, TurnCW/TurnCCW, Up/Down
            public ICylinder[] MainCyl = new ICylinder[DEF_MAX_COORDINATE];
            public ICylinder[] SubCyl = new ICylinder[DEF_MAX_COORDINATE];
            public ICylinder[] GuideCyl = new ICylinder[DEF_MAX_COORDINATE];

            // Vacuum
            public IVacuum[] Vacuum = new IVacuum[(int)EHandlerVacuum.MAX];

            // MultiAxes
            public MMultiAxes_YMC AxHandler;

            // Vision Object
            public IVision Vision;
        }

        public class CMeHandlerData
        {
            // Handler Type
            public EHandlerType[] HandlerType = new EHandlerType[DEF_MAX_COORDINATE];

            // Camera Number
            public int CamNo;

            // Detect Object Sensor Address
            public int InDetectObject   = IO_ADDR_NOT_DEFINED;

            // IO Address for manual control cylinder
            public int InUpCylinder     = IO_ADDR_NOT_DEFINED;
            public int InDownCylinder   = IO_ADDR_NOT_DEFINED;

            public int OutUpCylinder    = IO_ADDR_NOT_DEFINED;
            public int OutDownCylinder  = IO_ADDR_NOT_DEFINED;

            public CMeHandlerData(bool[] UseVccFlag = null, EHandlerType[] HandlerType = null)
            {
                if (HandlerType == null)
                {
                    for(int i = 0; i < this.HandlerType.Length; i++)
                    {
                        this.HandlerType[i] = EHandlerType.NONE;
                    }
                }
                else
                {
                    Array.Copy(HandlerType, this.HandlerType, HandlerType.Length);
                }
            }
        }
    }

    public class MMeHandler : MObject
    {
        private CMeHandlerRefComp m_RefComp;
        private CMeHandlerData m_Data;

        // Cylinder
        private bool[] UseMainCylFlag = new bool[DEF_MAX_COORDINATE];
        private bool[] UseSubCylFlag = new bool[DEF_MAX_COORDINATE];
        private bool[] UseGuideCylFlag = new bool[DEF_MAX_COORDINATE];

        // Vacuum
        private bool[] UseVccFlag = new bool[(int)EHandlerVacuum.MAX];

        private CHandlerPos HandlerPos = new CHandlerPos();
        private int HandlerPosInfo;
        private bool IsMarkAligned;
        private CPos_XYTZ AlignOffset = new CPos_XYTZ();

        MTickTimer m_waitTimer = new MTickTimer();

        public MMeHandler(CObjectInfo objInfo, CMeHandlerRefComp refComp, CMeHandlerData data)
            : base(objInfo)
        {
            m_RefComp = refComp;
            SetData(data);

            for (int i = 0; i < UseVccFlag.Length; i++)
            {
                UseVccFlag[i] = false;
            }

            HandlerPosInfo = (int)EHandlerPos.NONE;
        }

        public int SetData(CMeHandlerData source)
        {
            m_Data = ObjectExtensions.Copy(source);
            return SUCCESS;
        }

        public int GetData(out CMeHandlerData target)
        {
            target = ObjectExtensions.Copy(m_Data);

            return SUCCESS;
        }

        public int SetVccUseFlag(bool[] UseVccFlag = null)
        {
            if(UseVccFlag != null)
            {
                Array.Copy(UseVccFlag, this.UseVccFlag, UseVccFlag.Length);
            }
            return SUCCESS;
        }

        public int SetCylUseFlag(bool[] UseMainCylFlag = null, bool[] UseSubCylFlag = null, bool[] UseGuideCylFlag = null)
        {
            if(UseMainCylFlag != null)
            {
                Array.Copy(UseMainCylFlag, this.UseMainCylFlag, UseMainCylFlag.Length);
            }
            if (UseSubCylFlag != null)
            {
                Array.Copy(UseSubCylFlag, this.UseSubCylFlag, UseSubCylFlag.Length);
            }
            if (UseGuideCylFlag != null)
            {
                Array.Copy(UseGuideCylFlag, this.UseGuideCylFlag, UseGuideCylFlag.Length);
            }

            return SUCCESS;
        }

        public int Absorb(bool bSkipSensor)
        {
            bool bStatus;
            int iResult = SUCCESS;
            bool[] bWaitFlag = new bool[(int)EHandlerVacuum.MAX];
            CVacuumTime[] sData = new CVacuumTime[(int)EHandlerVacuum.MAX];
            bool bNeedWait = false;

            for (int i = 0; i < (int)EHandlerVacuum.MAX; i++)
            {
                if (UseVccFlag[i] == false) continue;

                m_RefComp.Vacuum[i].GetVacuumTime(out sData[i]);
                iResult = m_RefComp.Vacuum[i].IsOn(out bStatus);
                if (iResult != SUCCESS) return iResult;

                // 흡착되지 않은 상태라면 흡착시킴  
                if (bStatus == false)
                {
                    iResult = m_RefComp.Vacuum[i].On(true);
                    if (iResult != SUCCESS) return iResult;

                    bWaitFlag[i] = true;
                    bNeedWait = true;
                }

                Sleep(10);
            }

            if (bSkipSensor == true) return SUCCESS;

            m_waitTimer.StartTimer();
            while (bNeedWait)
            {
                bNeedWait = false;

                for (int i = 0; i < (int)EHandlerVacuum.MAX; i++)
                {
                    if (bWaitFlag[i] == false) continue;

                    iResult = m_RefComp.Vacuum[i].IsOn(out bStatus);
                    if (iResult != SUCCESS) return iResult;

                    if (bStatus == true) // if on
                    {
                        bWaitFlag[i] = false;
                        //Sleep(sData[i].OnSettlingTime * 1000);
                    }
                    else // if off
                    {
                        bNeedWait = true;
                        if (m_waitTimer.MoreThan(sData[i].TurningTime * 1000))
                        {
                            return GenerateErrorCode(ERR_HANDLER_VACUUM_ON_TIME_OUT);
                        }
                    }

                }
            }

            return SUCCESS;
        }

        public int Release(bool bSkipSensor)
        {
            bool bStatus;
            int iResult = SUCCESS;
            bool[] bWaitFlag = new bool[(int)EHandlerVacuum.MAX];
            CVacuumTime[] sData = new CVacuumTime[(int)EHandlerVacuum.MAX];
            bool bNeedWait = false;

            for (int i = 0; i < (int)EHandlerVacuum.MAX; i++)
            {
                if (UseVccFlag[i] == false) continue;

                m_RefComp.Vacuum[i].GetVacuumTime(out sData[i]);
                iResult = m_RefComp.Vacuum[i].IsOff(out bStatus);
                if (iResult != SUCCESS) return iResult;

                if (bStatus == false)
                {
                    iResult = m_RefComp.Vacuum[i].Off(true);
                    if (iResult != SUCCESS) return iResult;

                    bWaitFlag[i] = true;
                    bNeedWait = true;
                }

                Sleep(10);
            }

            if (bSkipSensor == true) return SUCCESS;

            m_waitTimer.StartTimer();
            while (bNeedWait)
            {
                bNeedWait = false;

                for (int i = 0; i < (int)EHandlerVacuum.MAX; i++)
                {
                    if (bWaitFlag[i] == false) continue;

                    iResult = m_RefComp.Vacuum[i].IsOff(out bStatus);
                    if (iResult != SUCCESS) return iResult;

                    if (bStatus == true) // if on
                    {
                        bWaitFlag[i] = false;
                        //Sleep(sData[i].OffSettlingTime * 1000);
                    }
                    else // if off
                    {
                        bNeedWait = true;
                        if (m_waitTimer.MoreThan(sData[i].TurningTime * 1000))
                        {
                            return GenerateErrorCode(ERR_HANDLER_VACUUM_OFF_TIME_OUT);
                        }
                    }

                }
            }

            return SUCCESS;
        }

        public int IsAbsorbed(out bool bStatus)
        {
            int iResult = SUCCESS;
            bStatus = false;
            bool bTemp;

            for (int i = 0; i < (int)EHandlerVacuum.MAX; i++)
            {
                if (UseVccFlag[i] == false) continue;

                iResult = m_RefComp.Vacuum[i].IsOn(out bTemp);
                if (iResult != SUCCESS) return iResult;

                if (bTemp == false) return SUCCESS;
            }

            bStatus = true;
            return SUCCESS;
        }

        public int IsReleased(out bool bStatus)
        {
            int iResult = SUCCESS;
            bStatus = false;
            bool bTemp;

            for (int i = 0; i < (int)EHandlerVacuum.MAX; i++)
            {
                if (UseVccFlag[i] == false) continue;

                iResult = m_RefComp.Vacuum[i].IsOff(out bTemp);
                if (iResult != SUCCESS) return iResult;

                if (bTemp == false) return SUCCESS;
            }

            bStatus = true;
            return SUCCESS;
        }

        public int SetHandlerPos(CHandlerPos HandlerPos)
        {
            this.HandlerPos = ObjectExtensions.Copy(HandlerPos);
            return SUCCESS;
        }

        public int SetHandlerPos(int iPos, CPosition pos)
        {
            HandlerPos.Pos[iPos] = ObjectExtensions.Copy(pos);
            return SUCCESS;
        }

        public int SetHandlerPos(CPosition[] pos)
        {
            if (pos.Length != HandlerPos.Pos.Length)
                return GenerateErrorCode(ERR_HANDLER_INVALID_PARAMETER);

            for (int i = 0; i < HandlerPos.Pos.Length; i++)
            {
                SetHandlerPos(i, pos[i]);
            }
            return SUCCESS;
        }

        public int GetHandlerPos(int iPos, out CPosition pos)
        {
            pos = ObjectExtensions.Copy(HandlerPos.Pos[iPos]);
            return SUCCESS;
        }

        public int GetHandlerPos(out CPosition[] pos)
        {
            pos = new CPosition[HandlerPos.Pos.Length];

            for (int i = 0; i < HandlerPos.Pos.Length; i++)
            {
                GetHandlerPos(i, out pos[i]);
            }
            return SUCCESS;
        }

        public int GetHandlerTargetPos(int iPos, out CPos_XYTZ pos)
        {
            pos = HandlerPos.Pos[iPos].GetTargetPos();
            return SUCCESS;
        }

        public int GetHandlerTargetPos(out CPos_XYTZ[] pos)
        {
            pos = new CPos_XYTZ[HandlerPos.Pos.Length];

            for (int i = 0; i < HandlerPos.Pos.Length; i++)
            {
                GetHandlerTargetPos(i, out pos[i]);
            }
            return SUCCESS;
        }

        public int GetHandlerCurPos(out CPos_XYTZ pos)
        {
            m_RefComp.AxHandler.GetCurPos(out pos);
            return SUCCESS;
        }

        public void InitAlignOffset()
        {
            IsMarkAligned = false;
            AlignOffset.Init();
            for(int i = 0; i < (int)EHandlerPos.MAX; i++)
            {
                HandlerPos.Pos[i].InitAlign();
            }
        }

        public void SetAlignOffset(CPos_XYTZ offset)
        {
            IsMarkAligned = true;
            AlignOffset = ObjectExtensions.Copy(offset);
            for (int i = 0; i < (int)EHandlerPos.MAX; i++)
            {
                HandlerPos.Pos[i].AlignOffset = ObjectExtensions.Copy(offset);
            }
        }

        public void GetAlignOffset(out CPos_XYTZ offset)
        {
            offset = ObjectExtensions.Copy(AlignOffset);
        }

        /// <summary>
        /// sPos으로 이동하고, PosInfo를 iPos으로 셋팅한다. Backlash는 일단 차후로.
        /// </summary>
        /// <param name="sPos"></param>
        /// <param name="iPos"></param>
        /// <param name="bMoveFlag"></param>
        /// <param name="bUseBacklash"></param>
        /// <returns></returns>
        public int MoveHandlerPos(CPos_XYTZ sPos, int iPos, bool[] bMoveFlag = null, bool bUseBacklash = false,
            bool bUsePriority = false, int[] movePriority = null)
        {
            int iResult = SUCCESS;

            // assume move all axis if bMoveFlag is null
            if(bMoveFlag == null)
            {
                bMoveFlag = new bool[DEF_MAX_COORDINATE] { true, true, true, true };
            }

            // Load Position으로 가는 것이면 Align Offset을 초기화해야 한다.
            if (iPos == (int)EHandlerPos.LOAD)
                InitAlignOffset();

            // trans to array
            double[] dPos;
            sPos.TransToArray(out dPos);

            // backlash
            if(bUseBacklash)
            {
                // 나중에 작업
            }

            // Move Z Axis
            bool[] bTempFlag = new bool[DEF_MAX_COORDINATE];
            if(bMoveFlag[DEF_Z] == true)
            {
                bTempFlag[DEF_Z] = true;
                iResult = m_RefComp.AxHandler.Move(DEF_ALL_COORDINATE, bTempFlag, dPos);
                if (iResult != SUCCESS)
                {
                    WriteLog("fail : move handler z axis", ELogType.Debug, ELogWType.Error);
                    return iResult;
                }
            }

            // Move X, Y, T
            if (bMoveFlag[DEF_X] == true || bMoveFlag[DEF_Y] == true || bMoveFlag[DEF_T] == true)
            {
                // set priority
                if(bUsePriority == true && movePriority != null)
                {
                    m_RefComp.AxHandler.SetAxesMovePriority(movePriority);
                }

                // move
                bMoveFlag[DEF_Z] = false;
                iResult = m_RefComp.AxHandler.Move(DEF_ALL_COORDINATE, bMoveFlag, dPos, bUsePriority);
                if (iResult != SUCCESS)
                {
                    WriteLog("fail : move handler x y t axis", ELogType.Debug, ELogWType.Error);
                    return iResult;
                }
            }

            // set working pos
            if (iPos > (int)EHandlerPos.NONE)
            {
                HandlerPosInfo = iPos;
            }

            string str = $"success : move handler to pos:{iPos} {sPos.ToString()}";
            WriteLog(str, ELogType.Debug, ELogWType.Normal);

            return SUCCESS;
        }

        /// <summary>
        /// iPos 좌표로 선택된 축들을 Offset을 더해서 이동한다.
        /// </summary>
        /// <param name="iPos"></param>
        /// <param name="bMoveFlag"></param>
        /// <param name="dMoveOffset"></param>
        /// <param name="bUseBacklash"></param>
        /// <returns></returns>
        public int MoveHandlerPos(int iPos, bool[] bMoveFlag = null, double[] dMoveOffset = null, bool bUseBacklash = false,
            bool bUsePriority = false, int[] movePriority = null)
        {
            int iResult = SUCCESS;

            CPos_XYTZ sTargetPos;
            GetHandlerTargetPos(iPos, out sTargetPos);
            if (dMoveOffset != null)
            {
                sTargetPos = sTargetPos + dMoveOffset;
            }

            iResult = MoveHandlerPos(sTargetPos, iPos, bMoveFlag, bUseBacklash, bUsePriority, movePriority);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        /// <summary>
        /// 현재 위치와 목표위치의 위치차이 Tolerance check
        /// </summary>
        /// <param name="sPos"></param>
        /// <param name="bResult"></param>
        /// <param name="bCheck_TAxis"></param>
        /// <param name="bCheck_ZAxis"></param>
        /// <param name="bSkipError">위치가 틀릴경우 에러 보고할지 여부</param>
        /// <returns></returns>
        public int CompareHandlerPos(CPos_XYTZ sPos, out bool bResult, bool bCheck_TAxis, bool bCheck_ZAxis, bool bSkipError = true)
        {
            int iResult = SUCCESS;

            bResult = false;

            // trans to array
            double[] dPos;
            sPos.TransToArray(out dPos);

            bool[] bJudge = new bool[DEF_MAX_COORDINATE];
            iResult = m_RefComp.AxHandler.ComparePosition(dPos, out bJudge, DEF_ALL_COORDINATE);
            if (iResult != SUCCESS) return iResult;

            // skip axis
            if (bCheck_TAxis == false) bJudge[DEF_T] = true;
            if (bCheck_ZAxis == false) bJudge[DEF_Z] = true;

            // error check
            bResult = true;
            foreach(bool bTemp in bJudge)
            {
                if (bTemp == false) bResult = false;
            }

            // skip error?
            if(bSkipError == false && bResult == false)
            {
                string str = $"Stage의 위치비교 결과 미일치합니다. Target Pos : {sPos.ToString()}";
                WriteLog(str, ELogType.Debug, ELogWType.Error);

                return GenerateErrorCode(ERR_HANDLER_NOT_SAME_POSITION);
            }

            return SUCCESS;
        }

        public int CompareHandlerPos(int iPos, out bool bResult, bool bCheck_TAxis, bool bCheck_ZAxis, bool bSkipError = true)
        {
            int iResult = SUCCESS;

            bResult = false;

            CPos_XYTZ targetPos;
            iResult = GetHandlerTargetPos(iPos, out targetPos);
            if (iResult != SUCCESS) return iResult;

            iResult = CompareHandlerPos(targetPos, out bResult, bCheck_TAxis, bCheck_ZAxis, bSkipError);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int GetHandlerPosInfo()
        {
            return HandlerPosInfo;
        }

        public void SetHandlerPosInfo(int posInfo)
        {
            HandlerPosInfo = posInfo;
        }

        public int IsHandlerOrignReturn(out bool bStatus)
        {
            bool[] bAxisStatus;
            m_RefComp.AxHandler.IsOriginReturn(DEF_ALL_COORDINATE, out bStatus, out bAxisStatus);

            return SUCCESS;
        }

        public int IsObjectDetected(out bool bStatus)
        {
            int iResult = m_RefComp.IO.IsOn(m_Data.InDetectObject, out bStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        ////////////////////////////////////////////////////////////////////////
        /// DEF_Z
        public int IsUp(out bool bStatus)
        {
            bStatus = false;
            int index = DEF_Z;

            if (UseMainCylFlag[index] == true)
            {
                if(m_RefComp.MainCyl[index] == null) return GenerateErrorCode(ERR_HANDLER_UNABLE_TO_USE_CYL);
                int iResult = m_RefComp.MainCyl[index].IsUp(out bStatus);
                if (iResult != SUCCESS) return iResult;
                if (bStatus == false) return SUCCESS;
            }
            if (UseSubCylFlag[index] == true)
            {
                if (m_RefComp.SubCyl[index] == null) return GenerateErrorCode(ERR_HANDLER_UNABLE_TO_USE_CYL);
                int iResult = m_RefComp.SubCyl[index].IsUp(out bStatus);
                if (iResult != SUCCESS) return iResult;
                if (bStatus == false) return SUCCESS;
            }

            return SUCCESS;
        }

        public int IsDown(out bool bStatus)
        {
            bStatus = false;
            int index = DEF_Z;

            if (UseMainCylFlag[index] == true)
            {
                if (m_RefComp.MainCyl[index] == null) return GenerateErrorCode(ERR_HANDLER_UNABLE_TO_USE_CYL);
                int iResult = m_RefComp.MainCyl[index].IsDown(out bStatus);
                if (iResult != SUCCESS) return iResult;
                if (bStatus == false) return SUCCESS;
            }
            if (UseSubCylFlag[index] == true)
            {
                if (m_RefComp.SubCyl[index] == null) return GenerateErrorCode(ERR_HANDLER_UNABLE_TO_USE_CYL);
                int iResult = m_RefComp.SubCyl[index].IsDown(out bStatus);
                if (iResult != SUCCESS) return iResult;
                if (bStatus == false) return SUCCESS;
            }

            return SUCCESS;
        }

        public int Up(bool bSkipSensor = false)
        {
            int index = DEF_Z;

            if (UseMainCylFlag[index] == true)
            {
                if (m_RefComp.MainCyl[index] == null) return GenerateErrorCode(ERR_HANDLER_UNABLE_TO_USE_CYL);
                int iResult = m_RefComp.MainCyl[index].Up(bSkipSensor);
                if (iResult != SUCCESS) return iResult;
            }
            if (UseSubCylFlag[index] == true)
            {
                if (m_RefComp.SubCyl[index] == null) return GenerateErrorCode(ERR_HANDLER_UNABLE_TO_USE_CYL);
                int iResult = m_RefComp.SubCyl[index].Up(bSkipSensor);
                if (iResult != SUCCESS) return iResult;
            }

            return SUCCESS;
        }

        public int Down(bool bSkipSensor = false)
        {
            int index = DEF_Z;

            if (UseMainCylFlag[index] == true)
            {
                if (m_RefComp.MainCyl[index] == null) return GenerateErrorCode(ERR_HANDLER_UNABLE_TO_USE_CYL);
                int iResult = m_RefComp.MainCyl[index].Down(bSkipSensor);
                if (iResult != SUCCESS) return iResult;
            }
            if (UseSubCylFlag[index] == true)
            {
                if (m_RefComp.SubCyl[index] == null) return GenerateErrorCode(ERR_HANDLER_UNABLE_TO_USE_CYL);
                int iResult = m_RefComp.SubCyl[index].Down(bSkipSensor);
                if (iResult != SUCCESS) return iResult;
            }
            
            return SUCCESS;
        }

        ////////////////////////////////////////////////////////////////////////
        /// DEF_X
        public int IsUpstr(out bool bStatus)
        {
            bStatus = false;
            int index = DEF_X;

            if (UseMainCylFlag[index] == true)
            {
                if (m_RefComp.MainCyl[index] == null) return GenerateErrorCode(ERR_HANDLER_UNABLE_TO_USE_CYL);
                int iResult = m_RefComp.MainCyl[index].IsUpstr(out bStatus);
                if (iResult != SUCCESS) return iResult;
                if (bStatus == false) return SUCCESS;
            }
            if (UseSubCylFlag[index] == true)
            {
                if (m_RefComp.SubCyl[index] == null) return GenerateErrorCode(ERR_HANDLER_UNABLE_TO_USE_CYL);
                int iResult = m_RefComp.SubCyl[index].IsUpstr(out bStatus);
                if (iResult != SUCCESS) return iResult;
                if (bStatus == false) return SUCCESS;
            }

            return SUCCESS;
        }

        public int IsDownstr(out bool bStatus)
        {
            bStatus = false;
            int index = DEF_X;

            if (UseMainCylFlag[index] == true)
            {
                if (m_RefComp.MainCyl[index] == null) return GenerateErrorCode(ERR_HANDLER_UNABLE_TO_USE_CYL);
                int iResult = m_RefComp.MainCyl[index].IsDownstr(out bStatus);
                if (iResult != SUCCESS) return iResult;
                if (bStatus == false) return SUCCESS;
            }
            if (UseSubCylFlag[index] == true)
            {
                if (m_RefComp.SubCyl[index] == null) return GenerateErrorCode(ERR_HANDLER_UNABLE_TO_USE_CYL);
                int iResult = m_RefComp.SubCyl[index].IsDownstr(out bStatus);
                if (iResult != SUCCESS) return iResult;
                if (bStatus == false) return SUCCESS;
            }

            return SUCCESS;
        }

        public int Upstr(bool bSkipSensor = false)
        {
            int index = DEF_X;

            if (UseMainCylFlag[index] == true)
            {
                if (m_RefComp.MainCyl[index] == null) return GenerateErrorCode(ERR_HANDLER_UNABLE_TO_USE_CYL);
                int iResult = m_RefComp.MainCyl[index].Upstr(bSkipSensor);
                if (iResult != SUCCESS) return iResult;
            }
            if (UseSubCylFlag[index] == true)
            {
                if (m_RefComp.SubCyl[index] == null) return GenerateErrorCode(ERR_HANDLER_UNABLE_TO_USE_CYL);
                int iResult = m_RefComp.SubCyl[index].Upstr(bSkipSensor);
                if (iResult != SUCCESS) return iResult;
            }

            return SUCCESS;
        }

        public int Downstr(bool bSkipSensor = false)
        {
            int index = DEF_X;

            if (UseMainCylFlag[index] == true)
            {
                if (m_RefComp.MainCyl[index] == null) return GenerateErrorCode(ERR_HANDLER_UNABLE_TO_USE_CYL);
                int iResult = m_RefComp.MainCyl[index].Downstr(bSkipSensor);
                if (iResult != SUCCESS) return iResult;
            }
            if (UseSubCylFlag[index] == true)
            {
                if (m_RefComp.SubCyl[index] == null) return GenerateErrorCode(ERR_HANDLER_UNABLE_TO_USE_CYL);
                int iResult = m_RefComp.SubCyl[index].Downstr(bSkipSensor);
                if (iResult != SUCCESS) return iResult;
            }

            return SUCCESS;
        }

        ////////////////////////////////////////////////////////////////////////
        /// DEF_Y
        public int IsFront(out bool bStatus)
        {
            bStatus = false;
            int index = DEF_Y;

            if (UseMainCylFlag[index] == true)
            {
                if (m_RefComp.MainCyl[index] == null) return GenerateErrorCode(ERR_HANDLER_UNABLE_TO_USE_CYL);
                int iResult = m_RefComp.MainCyl[index].IsFront(out bStatus);
                if (iResult != SUCCESS) return iResult;
                if (bStatus == false) return SUCCESS;
            }
            if (UseSubCylFlag[index] == true)
            {
                if (m_RefComp.SubCyl[index] == null) return GenerateErrorCode(ERR_HANDLER_UNABLE_TO_USE_CYL);
                int iResult = m_RefComp.SubCyl[index].IsFront(out bStatus);
                if (iResult != SUCCESS) return iResult;
                if (bStatus == false) return SUCCESS;
            }

            return SUCCESS;
        }

        public int IsBack(out bool bStatus)
        {
            bStatus = false;
            int index = DEF_Y;

            if (UseMainCylFlag[index] == true)
            {
                if (m_RefComp.MainCyl[index] == null) return GenerateErrorCode(ERR_HANDLER_UNABLE_TO_USE_CYL);
                int iResult = m_RefComp.MainCyl[index].IsBack(out bStatus);
                if (iResult != SUCCESS) return iResult;
                if (bStatus == false) return SUCCESS;
            }
            if (UseSubCylFlag[index] == true)
            {
                if (m_RefComp.SubCyl[index] == null) return GenerateErrorCode(ERR_HANDLER_UNABLE_TO_USE_CYL);
                int iResult = m_RefComp.SubCyl[index].IsBack(out bStatus);
                if (iResult != SUCCESS) return iResult;
                if (bStatus == false) return SUCCESS;
            }

            return SUCCESS;
        }

        public int Front(bool bSkipSensor = false)
        {
            int index = DEF_Y;

            if (UseMainCylFlag[index] == true)
            {
                if (m_RefComp.MainCyl[index] == null) return GenerateErrorCode(ERR_HANDLER_UNABLE_TO_USE_CYL);
                int iResult = m_RefComp.MainCyl[index].Front(bSkipSensor);
                if (iResult != SUCCESS) return iResult;
            }
            if (UseSubCylFlag[index] == true)
            {
                if (m_RefComp.SubCyl[index] == null) return GenerateErrorCode(ERR_HANDLER_UNABLE_TO_USE_CYL);
                int iResult = m_RefComp.SubCyl[index].Front(bSkipSensor);
                if (iResult != SUCCESS) return iResult;
            }

            return SUCCESS;
        }

        public int Back(bool bSkipSensor = false)
        {
            int index = DEF_Y;

            if (UseMainCylFlag[index] == true)
            {
                if (m_RefComp.MainCyl[index] == null) return GenerateErrorCode(ERR_HANDLER_UNABLE_TO_USE_CYL);
                int iResult = m_RefComp.MainCyl[index].Back(bSkipSensor);
                if (iResult != SUCCESS) return iResult;
            }
            if (UseSubCylFlag[index] == true)
            {
                if (m_RefComp.SubCyl[index] == null) return GenerateErrorCode(ERR_HANDLER_UNABLE_TO_USE_CYL);
                int iResult = m_RefComp.SubCyl[index].Back(bSkipSensor);
                if (iResult != SUCCESS) return iResult;
            }

            return SUCCESS;
        }
    }
}
