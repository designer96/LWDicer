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
            LOAD = 0,
            WAIT,
            UNLOAD,
            //TURN,
            // Z Safety Up Position은 System Data 에서 관리하도록 한다.
            //LOAD_Z_UP,      // Loading Pos(x,y,t) + Z Up
            //UNLOAD_Z_UP,    // Unloading Pos(x,y,t) + Z Up
            MAX,
        }

        public enum EHandlerXAxZone
        {
            NONE = -1,
            LOAD,
            WAIT,
            UNLOAD,
            MAX,
        }

        public enum EHandlerZAxZone
        {
            NONE = -1,
            SAFETY_UP,
            MAX,
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

            // Physical check zone sensor
            public int[] XAxZoneAddr    = new int[(int)EHandlerXAxZone.MAX];
            public int[] ZAxZoneAddr    = new int[(int)EHandlerXAxZone.MAX];

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

        // MovingObject
        private CMovingObject AxHandlerInfo = new CMovingObject((int)EHandlerPos.MAX);

        // Cylinder
        private bool[] UseMainCylFlag = new bool[DEF_MAX_COORDINATE];
        private bool[] UseSubCylFlag = new bool[DEF_MAX_COORDINATE];
        private bool[] UseGuideCylFlag = new bool[DEF_MAX_COORDINATE];

        // Vacuum
        private bool[] UseVccFlag = new bool[(int)EHandlerVacuum.MAX];

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

        public int SetHandlerPosition(CUnitPos FixedPos, CUnitPos ModelPos, CUnitPos OffsetPos)
        {
            AxHandlerInfo.SetPosition(FixedPos, ModelPos, OffsetPos);
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

        public int GetHandlerCurPos(out CPos_XYTZ pos)
        {
            m_RefComp.AxHandler.GetCurPos(out pos);
            return SUCCESS;
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
            {
                AxHandlerInfo.InitAlignOffset();
            }

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
                AxHandlerInfo.PosInfo = iPos;
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

            CPos_XYTZ sTargetPos = AxHandlerInfo.GetTargetPos(iPos);
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

            CPos_XYTZ targetPos = AxHandlerInfo.GetTargetPos(iPos);
            if (iResult != SUCCESS) return iResult;

            iResult = CompareHandlerPos(targetPos, out bResult, bCheck_TAxis, bCheck_ZAxis, bSkipError);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int GetHandlerPosInfo(out int posInfo, bool bUpdatePos = true, bool bCheckZAxis = false)
        {
            posInfo = (int)EHandlerPos.NONE;
            bool bStatus;
            int iResult = IsHandlerOrignReturn(out bStatus);
            if (iResult != SUCCESS) return iResult;

            // 실시간으로 자기 위치를 체크
            if(bUpdatePos)
            {
                for (int i = 0; i < (int)EHandlerPos.MAX; i++)
                {
                    CompareHandlerPos(i, out bStatus, false, bCheckZAxis);
                    if (bStatus)
                    {
                        AxHandlerInfo.PosInfo = i;
                        break;
                    }
                }
            }

            posInfo = AxHandlerInfo.PosInfo;
            return SUCCESS;
        }

        public void SetHandlerPosInfo(int posInfo)
        {
            AxHandlerInfo.PosInfo = posInfo;
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

        public int GetHandlerXAxZone(out int curZone)
        {
            curZone = (int)EHandlerXAxZone.NONE;
            for(int i = 0; i < (int)EHandlerXAxZone.MAX; i++)
            {
                bool bStatus;
                int iResult = m_RefComp.IO.IsOn(m_Data.XAxZoneAddr[i], out bStatus);
                if (iResult != SUCCESS) return iResult;
                if (bStatus == true)
                {
                    curZone = i;
                    break;
                }
            }
            return SUCCESS;
        }

        public int GetHandlerZAxZone(out int curZone)
        {
            curZone = (int)EHandlerZAxZone.NONE;
            for (int i = 0; i < (int)EHandlerZAxZone.MAX; i++)
            {
                bool bStatus;
                int iResult = m_RefComp.IO.IsOn(m_Data.ZAxZoneAddr[i], out bStatus);
                if (iResult != SUCCESS) return iResult;
                if (bStatus == true)
                {
                    curZone = i;
                    break;
                }
            }
            return SUCCESS;
        }

        public int IsHandlerZInSafetyZone(out bool bStatus)
        {
            bStatus = false;
            int curZone;
            int iResult = GetHandlerZAxZone(out curZone);
            if (iResult != SUCCESS) return iResult;

            if(curZone == (int)EHandlerZAxZone.SAFETY_UP)
            {
                bStatus = true;
            }
            return SUCCESS;
        }

        public int CheckForHandlerMove()
        {
            int iResult = IsHandlerOrignReturn();

            return SUCCESS;
        }
    }
}
