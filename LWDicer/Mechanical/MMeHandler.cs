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
        public const int ERR_HANDLER_UNABLE_TO_USE_IO                           = 1;
        public const int ERR_HANDLER_UNABLE_TO_USE_CYL                          = 2;
        public const int ERR_HANDLER_UNABLE_TO_USE_VCC                          = 3;
        public const int ERR_HANDLER_UNABLE_TO_USE_AXIS                         = 4;
        public const int ERR_HANDLER_UNABLE_TO_USE_VISION                       = 5;
        public const int ERR_HANDLER_NOT_ORIGIN_RETURNED                        = 6;
        public const int ERR_HANDLER_INVALID_AXIS                               = 7;
        public const int ERR_HANDLER_INVALID_PRIORITY                           = 8;
        public const int ERR_HANDLER_NOT_SAME_POSITION                          = 9;
        public const int ERR_HANDLER_LCD_VCC_ABNORMALITY                        = 10;
        public const int ERR_HANDLER_VACUUM_ON_TIME_OUT                         = 11;
        public const int ERR_HANDLER_VACUUM_OFF_TIME_OUT                        = 12;
        public const int ERR_HANDLER_INVALID_PARAMETER                          = 13;
        public const int ERR_HANDLER_OBJECT_DETECTED_BUT_NOT_ABSORBED           = 14;
        public const int ERR_HANDLER_OBJECT_NOT_DETECTED_BUT_NOT_RELEASED       = 15;

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

        public enum EHandlerYAxZone
        {
            NONE = -1,
            SAFETY,
            MAX,
        }

        public enum EHandlerTAxZone
        {
            NONE = -1,
            SAFETY,
            MAX,
        }

        public enum EHandlerZAxZone
        {
            NONE = -1,
            SAFETY,
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

            // Physical check zone sensor. 원점복귀 여부와 상관없이 축의 물리적인 위치를 체크 및
            // 안전위치 이동 check
            public CMAxisZoneCheck HandlerZone;

            public CMeHandlerData(EHandlerType[] HandlerType = null)
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

                HandlerZone = new CMAxisZoneCheck((int)EHandlerXAxZone.MAX, (int)EHandlerYAxZone.MAX,
                    (int)EHandlerTAxZone.MAX, (int)EHandlerZAxZone.MAX);
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

        public int MoveHandlerToSafetyPos(int axis)
        {
            int iResult = SUCCESS;
            string str;
            // 0. safety check
            iResult = CheckForHandlerAxisMove();
            if (iResult != SUCCESS) return iResult;

            // 0.1 trans to array
            double[] dPos = new double[1] { m_Data.HandlerZone.SafetyPos.GetAt(axis) };

            // 0.2 set use flag
            bool[] bTempFlag = new bool[1] { true };

            // 1. Move
            iResult = m_RefComp.AxHandler.Move(axis, bTempFlag, dPos);
            if (iResult != SUCCESS)
            {
                str = $"fail : move handler to safety pos [axis={axis}]";
                WriteLog(str, ELogType.Debug, ELogWType.Error);
                return iResult;
            }

            str = $"success : move handler to safety pos [axis={axis}";
            WriteLog(str, ELogType.Debug, ELogWType.Normal);

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

            // safety check
            iResult = CheckForHandlerAxisMove();
            if (iResult != SUCCESS) return iResult;

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
            double[] dTargetPos;
            sPos.TransToArray(out dTargetPos);

            // backlash
            if(bUseBacklash)
            {
                // 나중에 작업
            }

            // 1. move Z Axis to Safety Up. but when need to move z axis only, don't need to move z to safety pos
            if (bMoveFlag[DEF_X] == false && bMoveFlag[DEF_Y] == false && bMoveFlag[DEF_T] == false
                && bMoveFlag[DEF_Z] == true)
            {
                ;
            } else
            {
                bool bStatus;
                iResult = IsHandlerAxisInSafetyZone(DEF_Z, out bStatus);
                if (iResult != SUCCESS) return iResult;
                if (bStatus == false)
                {
                    iResult = MoveHandlerToSafetyPos(DEF_Z);
                    if (iResult != SUCCESS) return iResult;
                }
            }

            // 2. move X, Y, T
            if (bMoveFlag[DEF_X] == true || bMoveFlag[DEF_Y] == true || bMoveFlag[DEF_T] == true)
            {
                // set priority
                if(bUsePriority == true && movePriority != null)
                {
                    m_RefComp.AxHandler.SetAxesMovePriority(movePriority);
                }

                // move
                bMoveFlag[DEF_Z] = false;
                iResult = m_RefComp.AxHandler.Move(DEF_ALL_COORDINATE, bMoveFlag, dTargetPos, bUsePriority);
                if (iResult != SUCCESS)
                {
                    WriteLog("fail : move handler x y t axis", ELogType.Debug, ELogWType.Error);
                    return iResult;
                }
            }

            // 3. move Z Axis
            if (bMoveFlag[DEF_Z] == true)
            {
                bool[] bTempFlag = new bool[DEF_MAX_COORDINATE] { false, false, false, true };
                iResult = m_RefComp.AxHandler.Move(DEF_ALL_COORDINATE, bTempFlag, dTargetPos);
                if (iResult != SUCCESS)
                {
                    WriteLog("fail : move handler z axis", ELogType.Debug, ELogWType.Error);
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
        /// iPos 좌표로 선택된 축들을 이동시킨다.
        /// </summary>
        /// <param name="iPos">목표 위치</param>
        /// <param name="bUpdatedPosInfo">목표위치값을 update 할지의 여부</param>
        /// <param name="bMoveFlag">이동시킬 축 선택 </param>
        /// <param name="dMoveOffset">임시 옵셋값 </param>
        /// <param name="bUseBacklash"></param>
        /// <param name="bUsePriority">우선순위 이동시킬지 여부 </param>
        /// <param name="movePriority">우선순위 </param>
        /// <returns></returns>
        public int MoveHandlerPos(int iPos, bool bUpdatedPosInfo = true, bool[] bMoveFlag = null, double[] dMoveOffset = null, bool bUseBacklash = false,
            bool bUsePriority = false, int[] movePriority = null)
        {
            int iResult = SUCCESS;

            CPos_XYTZ sTargetPos = AxHandlerInfo.GetTargetPos(iPos);
            if (dMoveOffset != null)
            {
                sTargetPos = sTargetPos + dMoveOffset;
            }

            if(bUpdatedPosInfo == false)
            {
                iPos = (int)EHandlerPos.NONE;
            }
            iResult = MoveHandlerPos(sTargetPos, iPos, bMoveFlag, bUseBacklash, bUsePriority, movePriority);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        /// <summary>
        /// Handler Z축을 안전 Up 위치로 이동
        /// </summary>
        /// <returns></returns>
        public int MoveHandlerToSafetyUp()
        {
            return MoveHandlerToSafetyPos(DEF_Z);
        }

        /// <summary>
        /// Handler를 LOAD, UNLOAD등의 목표위치로 이동시킬때에 좀더 편하게 이동시킬수 있도록 간편화한 함수
        /// Z축만 움직일 경우엔 Position Info를 업데이트 하지 않는다. 
        /// </summary>
        /// <param name="iPos"></param>
        /// <param name="bMoveAllAxis"></param>
        /// <param name="bMoveXYT"></param>
        /// <param name="bMoveZ"></param>
        /// <returns></returns>
        public int MoveHandlerPos(int iPos, bool bMoveAllAxis, bool bMoveXYT, bool bMoveZ)
        {
            // 0. move all axis
            if (bMoveAllAxis)
            {
                return MoveHandlerPos(iPos);
            }

            // 1. move xyt only
            if (bMoveXYT)
            {
                bool[] bMoveFlag = new bool[DEF_MAX_COORDINATE] { true, true, true, false };
                return MoveHandlerPos(iPos, true, bMoveFlag);
            }

            // 2. move z only
            if (bMoveXYT)
            {
                bool[] bMoveFlag = new bool[DEF_MAX_COORDINATE] { false, false, false, true };
                return MoveHandlerPos(iPos, false, bMoveFlag);
            }

            return SUCCESS;
        }

        public int MoveHandlerToLoadPos(bool bMoveAllAxis = true, bool bMoveXYT = false, bool bMoveZ = false)
        {
            int iPos = (int)EHandlerPos.LOAD;

            return MoveHandlerPos(iPos, bMoveAllAxis, bMoveXYT, bMoveZ);
        }

        public int MoveHandlerToUnloadPos(bool bMoveAllAxis = true, bool bMoveXYT = false, bool bMoveZ = false)
        {
            int iPos = (int)EHandlerPos.UNLOAD;

            return MoveHandlerPos(iPos, bMoveAllAxis, bMoveXYT, bMoveZ);
        }

        public int MoveHandlerToWaitPos(bool bMoveAllAxis = true, bool bMoveXYT = false, bool bMoveZ = false)
        {
            int iPos = (int)EHandlerPos.WAIT;

            return MoveHandlerPos(iPos, bMoveAllAxis, bMoveXYT, bMoveZ);
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
        public int IsCylUp(out bool bStatus, int index = DEF_Z)
        {
            int iResult;
            bStatus = false;

            if (UseMainCylFlag[index] == true)
            {
                if(m_RefComp.MainCyl[index] == null) return GenerateErrorCode(ERR_HANDLER_UNABLE_TO_USE_CYL);
                iResult = m_RefComp.MainCyl[index].IsUp(out bStatus);
                if (iResult != SUCCESS) return iResult;
                if (bStatus == false) return SUCCESS;
            }
            if (UseSubCylFlag[index] == true)
            {
                if (m_RefComp.SubCyl[index] == null) return GenerateErrorCode(ERR_HANDLER_UNABLE_TO_USE_CYL);
                iResult = m_RefComp.SubCyl[index].IsUp(out bStatus);
                if (iResult != SUCCESS) return iResult;
                if (bStatus == false) return SUCCESS;
            }

            return SUCCESS;
        }

        public int IsCylDown(out bool bStatus, int index = DEF_Z)
        {
            int iResult;
            bStatus = false;

            if (UseMainCylFlag[index] == true)
            {
                if (m_RefComp.MainCyl[index] == null) return GenerateErrorCode(ERR_HANDLER_UNABLE_TO_USE_CYL);
                iResult = m_RefComp.MainCyl[index].IsDown(out bStatus);
                if (iResult != SUCCESS) return iResult;
                if (bStatus == false) return SUCCESS;
            }
            if (UseSubCylFlag[index] == true)
            {
                if (m_RefComp.SubCyl[index] == null) return GenerateErrorCode(ERR_HANDLER_UNABLE_TO_USE_CYL);
                iResult = m_RefComp.SubCyl[index].IsDown(out bStatus);
                if (iResult != SUCCESS) return iResult;
                if (bStatus == false) return SUCCESS;
            }

            return SUCCESS;
        }

        public int CylUp(bool bSkipSensor = false, int index = DEF_Z)
        {
            // check for safety
            int iResult = CheckForHandlerCylMove();
            if (iResult != SUCCESS) return iResult;

            if (UseMainCylFlag[index] == true)
            {
                if (m_RefComp.MainCyl[index] == null) return GenerateErrorCode(ERR_HANDLER_UNABLE_TO_USE_CYL);
                iResult = m_RefComp.MainCyl[index].Up(bSkipSensor);
                if (iResult != SUCCESS) return iResult;
            }
            if (UseSubCylFlag[index] == true)
            {
                if (m_RefComp.SubCyl[index] == null) return GenerateErrorCode(ERR_HANDLER_UNABLE_TO_USE_CYL);
                iResult = m_RefComp.SubCyl[index].Up(bSkipSensor);
                if (iResult != SUCCESS) return iResult;
            }

            return SUCCESS;
        }

        public int CylDown(bool bSkipSensor = false, int index = DEF_Z)
        {
            // check for safety
            int iResult = CheckForHandlerCylMove();
            if (iResult != SUCCESS) return iResult;

            if (UseMainCylFlag[index] == true)
            {
                if (m_RefComp.MainCyl[index] == null) return GenerateErrorCode(ERR_HANDLER_UNABLE_TO_USE_CYL);
                iResult = m_RefComp.MainCyl[index].Down(bSkipSensor);
                if (iResult != SUCCESS) return iResult;
            }
            if (UseSubCylFlag[index] == true)
            {
                if (m_RefComp.SubCyl[index] == null) return GenerateErrorCode(ERR_HANDLER_UNABLE_TO_USE_CYL);
                iResult = m_RefComp.SubCyl[index].Down(bSkipSensor);
                if (iResult != SUCCESS) return iResult;
            }
            
            return SUCCESS;
        }

        ////////////////////////////////////////////////////////////////////////
        /// DEF_X
        public int IsCylUpstr(out bool bStatus)
        {
            bStatus = false;
            return IsCylUp(out bStatus, DEF_X);
        }

        public int IsCylDownstr(out bool bStatus)
        {
            bStatus = false;
            return IsCylDown(out bStatus, DEF_X);
        }

        public int CylUpstr(bool bSkipSensor = false)
        {
            return CylUp(bSkipSensor, DEF_X);
        }

        public int CylDownstr(bool bSkipSensor = false)
        {
            return CylDown(bSkipSensor, DEF_X);
        }

        ////////////////////////////////////////////////////////////////////////
        /// DEF_Y
        public int IsCylFront(out bool bStatus)
        {
            bStatus = false;
            return IsCylUp(out bStatus, DEF_Y);
        }

        public int IsCylBack(out bool bStatus)
        {
            bStatus = false;
            return IsCylDown(out bStatus, DEF_Y);
        }

        public int CylFront(bool bSkipSensor = false)
        {
            return CylUp(bSkipSensor, DEF_Y);
        }

        public int CylBack(bool bSkipSensor = false)
        {
            return CylDown(bSkipSensor, DEF_Y);
        }

        ////////////////////////////////////////////////////////////////////////
        /// DEF_T
        public int IsCylCW(out bool bStatus)
        {
            bStatus = false;
            return IsCylUp(out bStatus, DEF_T);
        }

        public int IsCylCCW(out bool bStatus)
        {
            bStatus = false;
            return IsCylDown(out bStatus, DEF_T);
        }

        public int CylCW(bool bSkipSensor = false)
        {
            return CylUp(bSkipSensor, DEF_T);
        }

        public int CylCCW(bool bSkipSensor = false)
        {
            return CylDown(bSkipSensor, DEF_T);
        }

        ////////////////////////////////////////////////////////////////////////

        public int GetHandlerAxZone(int axis, out int curZone)
        {
            bool bStatus;
            curZone = (int)EHandlerXAxZone.NONE;
            for(int i = 0; i < (int)EHandlerXAxZone.MAX; i++)
            {
                if (m_Data.HandlerZone.Axis[axis].ZoneAddr[i] == -1) continue; // if io is not allocated, continue;
                int iResult = m_RefComp.IO.IsOn(m_Data.HandlerZone.Axis[axis].ZoneAddr[i], out bStatus);
                if (iResult != SUCCESS) return iResult;
                if (bStatus == true)
                {
                    curZone = i;
                    break;
                }
            }
            return SUCCESS;
        }

        public int IsHandlerAxisInSafetyZone(int axis, out bool bStatus)
        {
            bStatus = false;
            int curZone;
            int iResult = GetHandlerAxZone(axis, out curZone);
            if (iResult != SUCCESS) return iResult;

            switch(axis)
            {
                case DEF_X:
                    break;

                case DEF_Y:
                    if (curZone == (int)EHandlerYAxZone.SAFETY)
                    {
                        bStatus = true;
                    }
                    break;

                case DEF_T:
                    if (curZone == (int)EHandlerTAxZone.SAFETY)
                    {
                        bStatus = true;
                    }
                    break;

                case DEF_Z:
                    if (curZone == (int)EHandlerZAxZone.SAFETY)
                    {
                        bStatus = true;
                    }
                    break;
            }
            return SUCCESS;
        }

        public int CheckForHandlerAxisMove(bool bCheckVacuum = true)
        {
            bool bStatus;

            // check origin
            int iResult = IsHandlerOrignReturn(out bStatus);
            if (iResult != SUCCESS) return iResult;
            if(bStatus == false)
            {
                return GenerateErrorCode(ERR_HANDLER_NOT_ORIGIN_RETURNED);
            }

            // check object
            iResult = CheckForHandlerCylMove();
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int CheckForHandlerCylMove(bool bCheckVacuum = true)
        {
            bool bStatus;

            // check object
            int iResult = IsObjectDetected(out bStatus);
            if (iResult != SUCCESS) return iResult;
            if (bStatus)
            {
                IsAbsorbed(out bStatus);
                if (iResult != SUCCESS) return iResult;
                if (bStatus == false)
                {
                    return GenerateErrorCode(ERR_HANDLER_OBJECT_DETECTED_BUT_NOT_ABSORBED);
                }
            }
            else
            {
                IsReleased(out bStatus);
                if (iResult != SUCCESS) return iResult;
                if (bStatus == false)
                {
                    return GenerateErrorCode(ERR_HANDLER_OBJECT_NOT_DETECTED_BUT_NOT_RELEASED);
                }
            }

            return SUCCESS;
        }

    }
}
