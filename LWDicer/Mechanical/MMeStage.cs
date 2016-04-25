using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static LWDicer.Control.DEF_Error;
using static LWDicer.Control.DEF_Common;
using static LWDicer.Control.DEF_MeStage;
using static LWDicer.Control.DEF_Motion;
using static LWDicer.Control.DEF_IO;
using static LWDicer.Control.DEF_Vacuum;

namespace LWDicer.Control
{
    public class DEF_MeStage
    {
        #region Data Define

        // Error Define
        public const int ERR_Stage_UNABLE_TO_USE_IO                         = 1;
        public const int ERR_Stage_UNABLE_TO_USE_CYL                        = 2;
        public const int ERR_Stage_UNABLE_TO_USE_VCC                        = 3;
        public const int ERR_Stage_UNABLE_TO_USE_AXIS                       = 4;
        public const int ERR_Stage_UNABLE_TO_USE_VISION                     = 5;
        public const int ERR_Stage_NOT_ORIGIN_RETURNED                      = 6;
        public const int ERR_Stage_INVALID_AXIS                             = 7;
        public const int ERR_Stage_INVALID_PRIORITY                         = 8;
        public const int ERR_Stage_NOT_SAME_POSITION                        = 20;
        public const int ERR_Stage_UNABLE_TO_USE_POSITION                   = 21;
        public const int ERR_Stage_MOVE_FAIL                                = 22;
        public const int ERR_Stage_READ_CURRENT_POSITION                    = 23;
        public const int ERR_Stage_CASSETTE_NOT_READY                       = 24;
        public const int ERR_Stage_VACUUM_ON_TIME_OUT                       = 40;
        public const int ERR_Stage_VACUUM_OFF_TIME_OUT                      = 41;
        public const int ERR_Stage_INVALID_PARAMETER                        = 42;
        public const int ERR_Stage_OBJECT_DETECTED_BUT_NOT_ABSORBED         = 43;
        public const int ERR_Stage_OBJECT_NOT_DETECTED_BUT_NOT_RELEASED     = 44;

        // System Define
        public const int WAFER_CLAMP_CYL_1                                  = 0;
        public const int WAFER_CLAMP_CYL_2                                  = 1;
        public const int WAFER_CLAMP_CYL_NUM                                = 2;

        public enum EStageCtlMode
        {
            LASER =0,
            PC
        }
        public enum EStageVacuum
        {
            SELF,           // 자체 발생 진공
            FACTORY,        // 공장 진공
            OBJECT,         // LCD 패널의 PCB같은 걸 집는 용도
            EXTRA_SELF,     // 
            EXTRA_FACTORY,  //
            EXTRA_OBJECT,   //
            MAX,
        }

        public enum EStagePos
        {
            NONE = -1,
            LOAD = 0,
            WAIT,
            UNLOAD,
            EDGE_ALIGN_1,           // EDGE Detect "0"도 위치
            EDGE_ALIGN_2,           // EDGE Detect "90"도 위치
            EDGE_ALIGN_3,           // EDGE Detect "180"도 위치
            EDGE_ALIGN_4,           // EDGE Detect "270"도 위치
            MACRO_ALIGN,            // MACRO Align "A" Mark 위치
            MICRO_ALIGN,           // MICRO Align "A" Mark 위치
            MICRO_ALIGN_TURN,     // MICRO Align Turn 후 "A" Mark 위치
            LASER_PROCESS,          // Laser Cutting할 첫 위치 (가로 방향)
            LASER_PROCESS_TURN,     // Laser Cutting할 첫 위치 (세로 방향)
            MAX,
        }

        public enum EStageXAxZone
        {
            NONE = -1,
            LOAD,
            WAIT,
            UNLOAD,
            MAX,
        }

        public enum EStageYAxZone
        {
            NONE = -1,
            LOAD,
            WAIT,
            UNLOAD,
            MAX,
        }

        public enum EStageTAxZone
        {
            NONE = -1,
            LOAD,
            WAIT,
            UNLOAD,
            MAX,
        }

        public enum EStageZAxZone
        {
            NONE = -1,
            SAFETY,
            MAX,
        }

        public class CMeStageRefComp
        {
            public IIO IO;

            // Cylinder (Wafer Clamp 1,2)
            public ICylinder[] MainCyl = new ICylinder[WAFER_CLAMP_CYL_NUM];

            // Vacuum
            public IVacuum[] Vacuum = new IVacuum[(int)EStageVacuum.MAX];

            // MultiAxes
            public MMultiAxes_YMC AxStage;
        }

        public class CMeStageData
        {
            // Index Move Length
            public double IndexWidth = 0.0;
            public double IndexHeight = 0.0;
            public double AlignMarkWidthLen = 0.0;
            public double AlignMarkWidthRatio = 0.0;
            public double VisionLaserDistance = 0.0;

            // Detect Object Sensor Address
            public int InDetectObject   = IO_ADDR_NOT_DEFINED;

            // IO Address for manual control cylinder
            public int InClampOpen1     = IO_ADDR_NOT_DEFINED;
            public int InClampClose1    = IO_ADDR_NOT_DEFINED;
            public int InClampOpen2     = IO_ADDR_NOT_DEFINED;            
            public int InClampClose2    = IO_ADDR_NOT_DEFINED;

            public int OutClampOpen1    = IO_ADDR_NOT_DEFINED;
            public int OutClampClose1   = IO_ADDR_NOT_DEFINED;
            public int OutClampOpen2    = IO_ADDR_NOT_DEFINED;
            public int OutClampClose2   = IO_ADDR_NOT_DEFINED;

            // Physical check zone sensor. 원점복귀 여부와 상관없이 축의 물리적인 위치를 체크 및
            // 안전위치 이동 check 
            public CMAxisZoneCheck StageZone;

            public CMeStageData()
            {
                StageZone = new CMAxisZoneCheck((int)EStageXAxZone.MAX, (int)EStageYAxZone.MAX,
                    (int)EStageTAxZone.MAX, (int)EStageZAxZone.MAX);
            }
            
        }

        #endregion
    }

    public class MMeStage : MMechanicalLayer
    {
        private CMeStageRefComp m_RefComp;
        private CMeStageData m_Data;

        // MovingObject
        private CMovingObject AxStageInfo = new CMovingObject((int)EStagePos.MAX);

        // Cylinder
        private bool[] UseMainCylFlag   = new bool[WAFER_CLAMP_CYL_NUM];
        private bool[] UseSubCylFlag    = new bool[WAFER_CLAMP_CYL_NUM];
        private bool[] UseGuideCylFlag  = new bool[WAFER_CLAMP_CYL_NUM];

        // Vacuum
        private bool[] UseVccFlag = new bool[(int)EStageVacuum.MAX];

        MTickTimer m_waitTimer = new MTickTimer();

        public MMeStage(CObjectInfo objInfo, CMeStageRefComp refComp, CMeStageData data)
            : base(objInfo)
        {
            m_RefComp = refComp;
            SetData(data);

            for (int i = 0; i < UseVccFlag.Length; i++)
            {
                UseVccFlag[i] = false;
            }
        }

        // Data & Flag 설정
        #region Data & Flag 설정
        public int SetData(CMeStageData source)
        {
            m_Data = ObjectExtensions.Copy(source);
            return SUCCESS;
        }

        public int GetData(out CMeStageData target)
        {
            target = ObjectExtensions.Copy(m_Data);

            return SUCCESS;
        }

        public int SetStagePosition(CUnitPos FixedPos, CUnitPos ModelPos, CUnitPos OffsetPos)
        {
            AxStageInfo.SetPosition(FixedPos, ModelPos, OffsetPos);
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

        #endregion

        // Air 흡착 관련 
        #region Air Vaccum 동작
        public int Absorb(bool bSkipSensor)
        {
            bool bStatus;
            int iResult = SUCCESS;
            bool[] bWaitFlag = new bool[(int)EStageVacuum.MAX];
            CVacuumTime[] sData = new CVacuumTime[(int)EStageVacuum.MAX];
            bool bNeedWait = false;

            for (int i = 0; i < (int)EStageVacuum.MAX; i++)
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

                for (int i = 0; i < (int)EStageVacuum.MAX; i++)
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
                            return GenerateErrorCode(ERR_Stage_VACUUM_ON_TIME_OUT);
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
            bool[] bWaitFlag = new bool[(int)EStageVacuum.MAX];
            CVacuumTime[] sData = new CVacuumTime[(int)EStageVacuum.MAX];
            bool bNeedWait = false;

            for (int i = 0; i < (int)EStageVacuum.MAX; i++)
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

                for (int i = 0; i < (int)EStageVacuum.MAX; i++)
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
                            return GenerateErrorCode(ERR_Stage_VACUUM_OFF_TIME_OUT);
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

            for (int i = 0; i < (int)EStageVacuum.MAX; i++)
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

            for (int i = 0; i < (int)EStageVacuum.MAX; i++)
            {
                if (UseVccFlag[i] == false) continue;

                iResult = m_RefComp.Vacuum[i].IsOff(out bTemp);
                if (iResult != SUCCESS) return iResult;

                if (bTemp == false) return SUCCESS;
            }

            bStatus = true;
            return SUCCESS;
        }

        //===============================================================================================

        #endregion
        
        // Stage Servo 구동
        #region Stage Move 동작

        public int GetStageCurPos(out CPos_XYTZ pos)
        {
            m_RefComp.AxStage.GetCurPos(out pos);
            return SUCCESS;
        }

        public int MoveStageToSafetyPos(int axis)
        {
            int iResult = SUCCESS;
            string str;
            // 0. safety check
            iResult = CheckForStageAxisMove();
            if (iResult != SUCCESS) return iResult;

            // 0.1 trans to array
            double[] dPos = new double[1] { m_Data.StageZone.SafetyPos.GetAt(axis) };

            // 0.2 set use flag
            bool[] bTempFlag = new bool[1] { true };

            // 1. Move
            iResult = m_RefComp.AxStage.Move(axis, bTempFlag, dPos);
            if (iResult != SUCCESS)
            {
                str = $"fail : move Stage to safety pos [axis={axis}]";
                WriteLog(str, ELogType.Debug, ELogWType.Error);
                return iResult;
            }

            str = $"success : move Stage to safety pos [axis={axis}";
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
        public int MoveStagePos(CPos_XYTZ sPos, int iPos, bool[] bMoveFlag = null, bool bUseBacklash = false,
            bool bUsePriority = false, int[] movePriority = null)
        {
            int iResult = SUCCESS;

            // safety check
            iResult = CheckForStageAxisMove();
            if (iResult != SUCCESS) return iResult;

            // assume move all axis if bMoveFlag is null
            if(bMoveFlag == null)
            {
                // Stage는 Z축이 없다 (X,Y,T축)
                bMoveFlag = new bool[DEF_MAX_COORDINATE] { true, true, true, false };
            }

            // Load / Unload Position으로 가는 것이면 Align Offset을 초기화해야 한다.
            if (iPos == (int)EStagePos.LOAD || iPos == (int)EStagePos.UNLOAD)
            {
                AxStageInfo.InitAlignOffset();
            }

            // trans to array
            double[] dTargetPos;
            sPos.TransToArray(out dTargetPos);

            // backlash
            if(bUseBacklash)
            {
                // 나중에 작업
            }

            // 1. move X, Y, T
            if (bMoveFlag[DEF_X] == true || bMoveFlag[DEF_Y] == true || bMoveFlag[DEF_T] == true)
            {
                // set priority
                if(bUsePriority == true && movePriority != null)
                {
                    m_RefComp.AxStage.SetAxesMovePriority(movePriority);
                }

                // move
                bMoveFlag[DEF_Z] = false;
                iResult = m_RefComp.AxStage.Move(DEF_ALL_COORDINATE, bMoveFlag, dTargetPos, bUsePriority);
                if (iResult != SUCCESS)
                {
                    WriteLog("fail : move Stage x y t axis", ELogType.Debug, ELogWType.Error);
                    return iResult;
                }
            }

            // 2. move Z Axis
            if (bMoveFlag[DEF_Z] == true)
            {
                bool[] bTempFlag = new bool[DEF_MAX_COORDINATE] { false, false, false, true };
                iResult = m_RefComp.AxStage.Move(DEF_ALL_COORDINATE, bTempFlag, dTargetPos);
                if (iResult != SUCCESS)
                {
                    WriteLog("fail : move Stage z axis", ELogType.Debug, ELogWType.Error);
                    return iResult;
                }
            }

            // set working pos
            if (iPos > (int)EStagePos.NONE)
            {
                AxStageInfo.PosInfo = iPos;
            }

            string str = $"success : move Stage to pos:{iPos} {sPos.ToString()}";
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
        public int MoveStagePos(int iPos, bool bUpdatedPosInfo = true, bool[] bMoveFlag = null, double[] dMoveOffset = null, bool bUseBacklash = false,
            bool bUsePriority = false, int[] movePriority = null)
        {
            int iResult = SUCCESS;

            // Load / Unload Position으로 가는 것이면 Align Offset을 초기화해야 한다.
            if (iPos == (int)EStagePos.LOAD || iPos == (int)EStagePos.UNLOAD)
            {
                AxStageInfo.InitAlignOffset();
            }

            CPos_XYTZ sTargetPos = AxStageInfo.GetTargetPos(iPos);

            if (dMoveOffset != null)
            {
                sTargetPos = sTargetPos + dMoveOffset;
            }

            if(bUpdatedPosInfo == false)
            {
                iPos = (int)EStagePos.NONE;
            }
            iResult = MoveStagePos(sTargetPos, iPos, bMoveFlag, bUseBacklash, bUsePriority, movePriority);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int MoveStageIndexPos(int iAxis, double dMoveLength,  bool bUseBacklash = false)
        {
            int iResult = SUCCESS;

            // 이동 Position 선택
            int iPos = (int)EStagePos.NONE;

            bool[] bMoveFlag = new bool[DEF_MAX_COORDINATE] { false, false, false, false };           
            bool bUsePriority = false;
            int[] movePriority = null;

            // 현재 위치를 읽어옴 (Command 값을 사용하는 것이 좋을 듯)
            CPos_XYTZ sTargetPos;

            iResult = GetStageCurPos(out sTargetPos);
            if (iResult != SUCCESS) GenerateErrorCode(ERR_Stage_READ_CURRENT_POSITION);
            // Index 거리를 해당 축에 더하여 거리를 산출함.

            if (iAxis == DEF_X)
            {
                bMoveFlag[DEF_X] = true;
                sTargetPos.dX += dMoveLength;
            }
            if (iAxis == DEF_Y)
            {
                bMoveFlag[DEF_Y] = true;
                sTargetPos.dY += dMoveLength;
            }
            if (iAxis == DEF_T)
            {
                bMoveFlag[DEF_T] = true;
                sTargetPos.dT += dMoveLength;
            }
            
            iResult = MoveStagePos(sTargetPos, iPos, bMoveFlag, bUseBacklash, bUsePriority, movePriority);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int MoveStageIndexPlus(int iAxis)
        {
            bool[] bMoveFlag = new bool[DEF_MAX_COORDINATE] { false, false, false, false };
            double dMoveLength = 0.0;

            if (iAxis == DEF_X)
            {
                bMoveFlag[DEF_X] = true;
                dMoveLength = m_Data.IndexWidth;
            }
            if (iAxis == DEF_Y)
            {
                bMoveFlag[DEF_Y] = true;
                dMoveLength = m_Data.IndexHeight;
            }

            // + 방향으로 이동
            MoveStageIndexPos(iAxis, dMoveLength);

            return SUCCESS;
        }

        public int MoveStageIndexMinus(int iAxis)
        {
            bool[] bMoveFlag = new bool[DEF_MAX_COORDINATE] { false, false, false, false };
            double dMoveLength = 0.0;

            if (iAxis == DEF_X)
            {
                bMoveFlag[DEF_X] = true;
                dMoveLength = m_Data.IndexWidth;
            }
            if (iAxis == DEF_Y)
            {
                bMoveFlag[DEF_Y] = true;
                dMoveLength = m_Data.IndexHeight;
            }

            // - 방향으로 이동
            MoveStageIndexPos(iAxis, -dMoveLength);

            return SUCCESS;
        }

        /// <summary>
        /// Stage를 LOAD, UNLOAD등의 목표위치로 이동시킬때에 좀더 편하게 이동시킬수 있도록 간편화한 함수
        /// Z축만 움직일 경우엔 Position Info를 업데이트 하지 않는다. 
        /// </summary>
        /// <param name="iPos"></param>
        /// <param name="bMoveAllAxis"></param>
        /// <param name="bMoveXYT"></param>
        /// <param name="bMoveZ"></param>
        /// <returns></returns>
        public int MoveStagePos(int iPos, bool bMoveAllAxis, bool bMoveXYT, bool bMoveZ)
        {
            // 0. move all axis
            if (bMoveAllAxis)
            {
                return MoveStagePos(iPos);
            }

            // 1. move xyt only
            if (bMoveXYT)
            {
                bool[] bMoveFlag = new bool[DEF_MAX_COORDINATE] { true, true, true, false };
                return MoveStagePos(iPos, true, bMoveFlag);
            }

            // 2. move z only
            if (bMoveZ)
            {
                bool[] bMoveFlag = new bool[DEF_MAX_COORDINATE] { false, false, false, true };
                return MoveStagePos(iPos, false, bMoveFlag);
            }

            return SUCCESS;
        }

        public int MoveStageToLoadPos(bool bMoveAllAxis = false, bool bMoveXYT = true, bool bMoveZ = false)
        {
            int iPos = (int)EStagePos.LOAD;

            return MoveStagePos(iPos, bMoveAllAxis, bMoveXYT, bMoveZ);
        }

        public int MoveStageToUnloadPos(bool bMoveAllAxis = false, bool bMoveXYT = true, bool bMoveZ = false)
        {
            int iPos = (int)EStagePos.UNLOAD;

            return MoveStagePos(iPos, bMoveAllAxis, bMoveXYT, bMoveZ);
        }

        public int MoveStageToWaitPos(bool bMoveAllAxis = false, bool bMoveXYT = true, bool bMoveZ = false)
        {
            int iPos = (int)EStagePos.WAIT;

            return MoveStagePos(iPos, bMoveAllAxis, bMoveXYT, bMoveZ);
        }

        public int MoveStageToEdgeAlignPos1(bool bMoveAllAxis = false, bool bMoveXYT = true, bool bMoveZ = false)
        {
            int iPos = (int)EStagePos.EDGE_ALIGN_1;

            return MoveStagePos(iPos, bMoveAllAxis, bMoveXYT, bMoveZ);
        }

        public int MoveStageToEdgeAlignPos2(bool bMoveAllAxis = false, bool bMoveXYT = true, bool bMoveZ = false)
        {
            int iPos = (int)EStagePos.EDGE_ALIGN_2;

            return MoveStagePos(iPos, bMoveAllAxis, bMoveXYT, bMoveZ);
        }

        public int MoveStageToEdgeAlignPos3(bool bMoveAllAxis = false, bool bMoveXYT = true, bool bMoveZ = false)
        {
            int iPos = (int)EStagePos.EDGE_ALIGN_3;

            return MoveStagePos(iPos, bMoveAllAxis, bMoveXYT, bMoveZ);
        }

        public int MoveStageToEdgeAlignPos4(bool bMoveAllAxis = false, bool bMoveXYT = true, bool bMoveZ = false)
        {
            int iPos = (int)EStagePos.EDGE_ALIGN_4;

            return MoveStagePos(iPos, bMoveAllAxis, bMoveXYT, bMoveZ);
        }

        public int MoveStageToEdgeMacroAlignA(bool bMoveAllAxis = false, bool bMoveXYT = true, bool bMoveZ = false)
        {
            int iPos = (int)EStagePos.MACRO_ALIGN;

            return MoveStagePos(iPos, bMoveAllAxis, bMoveXYT, bMoveZ);
        }

        public int MoveStageToEdgeMacroAlignB(bool bMoveAllAxis = false, bool bMoveXYT = true, bool bMoveZ = false)
        {
            int iResult = -1;
            // Mark A 위치로 이동
            iResult = MoveStageToEdgeMacroAlignA();
            if (iResult != SUCCESS) return iResult;

            // 수평으로 Align Mark 거리 만큼 이동함.
            double dMoveDistance = m_Data.AlignMarkWidthLen;

            iResult = MoveStageIndexPos(DEF_Y, dMoveDistance);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }
        

        public int MoveStageToEdgeMicroAlignA(bool bMoveAllAxis = false, bool bMoveXYT = true, bool bMoveZ = false)
        {
            int iPos = (int)EStagePos.MICRO_ALIGN;

            return MoveStagePos(iPos, bMoveAllAxis, bMoveXYT, bMoveZ);
        }
        
        public int MoveStageToEdgeMicroAlignTurn(bool bMoveAllAxis = false, bool bMoveXYT = true, bool bMoveZ = false)
        {
            int iPos = (int)EStagePos.MICRO_ALIGN_TURN;

            return MoveStagePos(iPos, bMoveAllAxis, bMoveXYT, bMoveZ);
        }

        
        #endregion

        // 모드 변경 및 Align Data Set
        #region Control Mode & Align Data Set
        public void SetStageCtlMode(int nCtlMode)
        {
            if(nCtlMode == (int)EStageCtlMode.LASER)
            {
                // ACS Buffer에 모드 변경 Program을 작성... Buffer Call로 변경함.
            }

            if (nCtlMode == (int)EStageCtlMode.PC)
            {
                // ACS Buffer에 모드 변경 Program을 작성... Buffer Call로 변경함.
            }

        }
        public int SetAlignData(CPos_XYTZ offset)
        {
            int iResult;
            CPos_XYTZ curPos;

            // 현재 Align Offset 값을 읽어옴
            iResult = GetStageCurPos(out curPos);
            if (iResult != SUCCESS) return iResult;
            // AlignData 적용
            curPos += offset;

            // AlignOffet 적용
            AxStageInfo.SetAlignOffset(curPos);

            return SUCCESS;
        }

        public void SetAlignDataInit()
        {
            AxStageInfo.InitAlignOffset();
        }

        #endregion

        // Stage Pos Data 확인 및 비교
        #region Stage Pos Data

        /// <summary>
        /// 현재 위치와 목표위치의 위치차이 Tolerance check
        /// </summary>
        /// <param name="sPos"></param>
        /// <param name="bResult"></param>
        /// <param name="bCheck_TAxis"></param>
        /// <param name="bCheck_ZAxis"></param>
        /// <param name="bSkipError">위치가 틀릴경우 에러 보고할지 여부</param>
        /// <returns></returns>
        public int CompareStagePos(CPos_XYTZ sPos, out bool bResult, bool bCheck_TAxis, bool bCheck_ZAxis, bool bSkipError = true)
        {
            int iResult = SUCCESS;

            bResult = false;

            // trans to array
            double[] dPos;
            sPos.TransToArray(out dPos);

            bool[] bJudge = new bool[DEF_MAX_COORDINATE];
            iResult = m_RefComp.AxStage.ComparePosition(dPos, out bJudge, DEF_ALL_COORDINATE);
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

                return GenerateErrorCode(ERR_Stage_NOT_SAME_POSITION);
            }

            return SUCCESS;
        }

        public int CompareStagePos(int iPos, out bool bResult, bool bCheck_TAxis, bool bCheck_ZAxis, bool bSkipError = true)
        {
            int iResult = SUCCESS;

            bResult = false;

            CPos_XYTZ targetPos = AxStageInfo.GetTargetPos(iPos);
            if (iResult != SUCCESS) return iResult;

            iResult = CompareStagePos(targetPos, out bResult, bCheck_TAxis, bCheck_ZAxis, bSkipError);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int GetStagePosInfo(out int posInfo, bool bUpdatePos = true, bool bCheckZAxis = false)
        {
            posInfo = (int)EStagePos.NONE;
            bool bStatus;
            int iResult = IsStageOrignReturn(out bStatus);
            if (iResult != SUCCESS) return iResult;

            // 실시간으로 자기 위치를 체크
            if(bUpdatePos)
            {
                for (int i = 0; i < (int)EStagePos.MAX; i++)
                {
                    CompareStagePos(i, out bStatus, false, bCheckZAxis);
                    if (bStatus)
                    {
                        AxStageInfo.PosInfo = i;
                        break;
                    }
                }
            }

            posInfo = AxStageInfo.PosInfo;
            return SUCCESS;
        }

        public void SetStagePosInfo(int posInfo)
        {
            AxStageInfo.PosInfo = posInfo;
        }

        public int IsStageOrignReturn(out bool bStatus)
        {
            bool[] bAxisStatus;
            m_RefComp.AxStage.IsOriginReturn(DEF_ALL_COORDINATE, out bStatus, out bAxisStatus);

            return SUCCESS;
        }

        #endregion

        // Stage Wafer Clamp 동작
        #region Wafer Clamp

        /// Cylinder
        public int IsCylUp(out bool bStatus, int index = DEF_Z)
        {
            int iResult;
            bStatus = false;

            if (UseMainCylFlag[index] == true)
            {
                if (m_RefComp.MainCyl[index] == null) return GenerateErrorCode(ERR_Stage_UNABLE_TO_USE_CYL);
                iResult = m_RefComp.MainCyl[index].IsUp(out bStatus);
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
                if (m_RefComp.MainCyl[index] == null) return GenerateErrorCode(ERR_Stage_UNABLE_TO_USE_CYL);
                iResult = m_RefComp.MainCyl[index].IsDown(out bStatus);
                if (iResult != SUCCESS) return iResult;
                if (bStatus == false) return SUCCESS;
            }


            return SUCCESS;
        }

        public int CylUp(bool bSkipSensor = false, int index = DEF_Z)
        {
            // check for safety
            int iResult = CheckForStageCylMove();
            if (iResult != SUCCESS) return iResult;

            if (UseMainCylFlag[index] == true)
            {
                if (m_RefComp.MainCyl[index] == null) return GenerateErrorCode(ERR_Stage_UNABLE_TO_USE_CYL);
                iResult = m_RefComp.MainCyl[index].Up(bSkipSensor);
                if (iResult != SUCCESS) return iResult;
            }


            return SUCCESS;
        }

        public int CylDown(bool bSkipSensor = false, int index = DEF_Z)
        {
            // check for safety
            int iResult = CheckForStageCylMove();
            if (iResult != SUCCESS) return iResult;

            if (UseMainCylFlag[index] == true)
            {
                if (m_RefComp.MainCyl[index] == null) return GenerateErrorCode(ERR_Stage_UNABLE_TO_USE_CYL);
                iResult = m_RefComp.MainCyl[index].Down(bSkipSensor);
                if (iResult != SUCCESS) return iResult;
            }


            return SUCCESS;
        }

        ////////////////////////////////////////////////////////////////////////
        /// Wafer Clamp
        public int IsClampOpen(out bool bStatus)
        {
            int iResult = 0;
            bStatus = false;
            // Cylinder #1
            iResult = IsCylUp(out bStatus, WAFER_CLAMP_CYL_1);
            if (iResult != SUCCESS) return iResult;
            // Cylinder #2
            iResult = IsCylUp(out bStatus, WAFER_CLAMP_CYL_2);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int IsClampClose(out bool bStatus)
        {
            int iResult = 0;
            bStatus = false;
            // Cylinder #1
            iResult = IsCylDown(out bStatus, WAFER_CLAMP_CYL_1);
            if (iResult != SUCCESS) return iResult;
            // Cylinder #2
            iResult = IsCylDown(out bStatus, WAFER_CLAMP_CYL_2);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int ClampOpen(bool bSkipSensor = false)
        {
            int iResult = 0;
            // Cylinder #1
            iResult = CylUp(bSkipSensor, WAFER_CLAMP_CYL_1);
            if (iResult != SUCCESS) return iResult;
            // Cylinder #2
            iResult = CylUp(bSkipSensor, WAFER_CLAMP_CYL_2);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int ClampClose(bool bSkipSensor = false)
        {
            int iResult = 0;
            // Cylinder #1
            iResult = CylDown(bSkipSensor, WAFER_CLAMP_CYL_1);
            if (iResult != SUCCESS) return iResult;
            // Cylinder #2
            iResult = CylDown(bSkipSensor, WAFER_CLAMP_CYL_2);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        ////////////////////////////////////////////////////////////////////////
        #endregion

        // Interlock 조건 확인
        #region Interlock 확인
        public int IsObjectDetected(out bool bStatus)
        {
            int iResult = m_RefComp.IO.IsOn(m_Data.InDetectObject, out bStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        /// <summary>
        /// Stage Z축을 안전 Up 위치로 이동
        /// </summary>
        /// <returns></returns>
        public int MoveStageToSafetyUp()
        {
            int nResult = -1;

            nResult = MoveStageToSafetyPos(DEF_X);
            if (nResult != SUCCESS) return nResult;
            nResult = MoveStageToSafetyPos(DEF_Y);
            if (nResult != SUCCESS) return nResult;
            nResult = MoveStageToSafetyPos(DEF_T);
            if (nResult != SUCCESS) return nResult;

            return SUCCESS;
        }


        public int GetStageAxZone(int axis, out int curZone)
        {
            bool bStatus;
            curZone = (int)EStageXAxZone.NONE;
            for (int i = 0; i < (int)EStageXAxZone.MAX; i++)
            {
                if (m_Data.StageZone.Axis[axis].ZoneAddr[i] == -1) continue; // if io is not allocated, continue;
                int iResult = m_RefComp.IO.IsOn(m_Data.StageZone.Axis[axis].ZoneAddr[i], out bStatus);
                if (iResult != SUCCESS) return iResult;
                if (bStatus == true)
                {
                    curZone = i;
                    break;
                }
            }
            return SUCCESS;
        }

        public int IsStageAxisInSafetyZone(int axis, out bool bStatus)
        {
            bStatus = false;
            int curZone;
            int iResult = GetStageAxZone(axis, out curZone);
            if (iResult != SUCCESS) return iResult;

            switch (axis)
            {
                case DEF_X:
                    break;

                case DEF_Y:
                    break;

                case DEF_T:
                    break;

                case DEF_Z:
                    if (curZone == (int)EStageZAxZone.SAFETY)
                    {
                        bStatus = true;
                    }
                    break;
            }
            return SUCCESS;
        }

        public int CheckForStageAxisMove(bool bCheckVacuum = true)
        {
            bool bStatus;

            // check origin
            int iResult = IsStageOrignReturn(out bStatus);
            if (iResult != SUCCESS) return iResult;
            if (bStatus == false)
            {
                return GenerateErrorCode(ERR_Stage_NOT_ORIGIN_RETURNED);
            }

            // check object
            iResult = CheckForStageCylMove();
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        

        public int CheckForStageCylMove(bool bCheckVacuum = true)
        {
            bool bStatus=true;

            // 조건없이 True

            //// check object
            //int iResult = IsObjectDetected(out bStatus);
            //if (iResult != SUCCESS) return iResult;
            //if (bStatus)
            //{
            //    IsAbsorbed(out bStatus);
            //    if (iResult != SUCCESS) return iResult;
            //    if (bStatus == false)
            //    {
            //        return GenerateErrorCode(ERR_Stage_OBJECT_DETECTED_BUT_NOT_ABSORBED);
            //    }
            //}
            //else
            //{
            //    IsReleased(out bStatus);
            //    if (iResult != SUCCESS) return iResult;
            //    if (bStatus == false)
            //    {
            //        return GenerateErrorCode(ERR_Stage_OBJECT_NOT_DETECTED_BUT_NOT_RELEASED);
            //    }
            //}

            return SUCCESS;
        }

        #endregion

    }
}
