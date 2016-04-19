using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static LWDicer.Control.DEF_Error;
using static LWDicer.Control.DEF_Common;
using static LWDicer.Control.DEF_MeElevator;
using static LWDicer.Control.DEF_Motion;
using static LWDicer.Control.DEF_IO;
using static LWDicer.Control.DEF_Vacuum;

namespace LWDicer.Control
{
    public class DEF_MeElevator
    {
        public const int ERR_Elevator_UNABLE_TO_USE_IO                           = 1;
        public const int ERR_Elevator_UNABLE_TO_USE_CYL                          = 2;
        public const int ERR_Elevator_UNABLE_TO_USE_VCC                          = 3;
        public const int ERR_Elevator_UNABLE_TO_USE_AXIS                         = 4;
        public const int ERR_Elevator_UNABLE_TO_USE_VISION                       = 5;
        public const int ERR_Elevator_NOT_ORIGIN_RETURNED                        = 6;
        public const int ERR_Elevator_INVALID_AXIS                               = 7;
        public const int ERR_Elevator_INVALID_PRIORITY                           = 8;
        public const int ERR_Elevator_NOT_SAME_POSITION                          = 20;
        public const int ERR_Elevator_UNABLE_TO_USE_POSITION                     = 21;
        public const int ERR_Elevator_MOVE_FAIL                                  = 22;
        public const int ERR_Elevator_EMPTY_SLOT_MOVE_FAIL                       = 23;
        public const int ERR_Elevator_CASSETTE_NOT_READY                         = 24;
        public const int ERR_Elevator_VACUUM_ON_TIME_OUT                         = 40;
        public const int ERR_Elevator_VACUUM_OFF_TIME_OUT                        = 41;
        public const int ERR_Elevator_INVALID_PARAMETER                          = 42;
        public const int ERR_Elevator_OBJECT_DETECTED_BUT_NOT_ABSORBED           = 43;
        public const int ERR_Elevator_OBJECT_NOT_DETECTED_BUT_NOT_RELEASED       = 44;


        public enum EElevatorPos
        {
            NONE = -1,
            BOTTOM = 0,
            LOAD,
            SLOT,
            TOP,
            MAX,
        }

        public enum EElevatorXAxZone
        {
            NONE = -1,
            SAFETY,
            MAX,
        }

        public enum EElevatorYAxZone
        {
            NONE = -1,
            SAFETY,
            MAX,
        }

        public enum EElevatorTAxZone
        {
            NONE = -1,
            SAFETY,
            MAX,
        }

        public enum EElevatorZAxZone
        {
            NONE = -1,
            BOTTOM = 0,
            LOAD,
            SLOT,
            TOP,
            SAFETY,
            MAX,
        }

        //===============================================================================
        //  Cassette Info
        public const int CASSETTE_MAX_SLOT_NUM = 20;
        public const int CASSETTE_DETECT_SENSOR_NUM = 4;
        public const double CASSETTE_DEFAULT_PITCH = 10.0;

        public enum ECassetteWaferInfo
        {
            NONE = -1,
            EMPTY   = 0,
            PRE_PROCESS,
            AFTER_PROCESS,
            MAX,
        }
        public enum ECassetteWaferType
        {
            NONE = -1,
            INCH_8 = 0,
            INCH_12,
            MAX,
        }

        public class CCassetteData
        {
            public int nSlotNum;
            public ECassetteWaferType nWaferType;
            public double dSlotPitch;
            public int[] nWaferData = new int[CASSETTE_MAX_SLOT_NUM];
        }

        //===============================================================================

        public class CMeElevatorRefComp
        {
            public IIO IO;        
            // MultiAxes
            public MMultiAxes_YMC AxElevator;
        }

        public class CMeElevatorData
        {
            // Cassette Info 
            public CCassetteData CassetteData = new CCassetteData();

            public int CurrentSlotNum = 0;

            // Detect Object Sensor Address
            public int InDetectWafer   = IO_ADDR_NOT_DEFINED;
            public int[] InDetectCassette = new int[CASSETTE_DETECT_SENSOR_NUM] 
                {IO_ADDR_NOT_DEFINED, IO_ADDR_NOT_DEFINED, IO_ADDR_NOT_DEFINED, IO_ADDR_NOT_DEFINED };
            
            // Physical check zone sensor. 원점복귀 여부와 상관없이 축의 물리적인 위치를 체크 및
            // 안전위치 이동 check
            public CMAxisZoneCheck ElevatorZone;

            public CMeElevatorData(CCassetteData CassetteData = null)
            {
                // Cassette Info Copy 
                if (CassetteData == null) // Cassette Data Init
                {   
                    this.CassetteData.nWaferType = ECassetteWaferType.INCH_12;
                    this.CassetteData.nSlotNum = CASSETTE_MAX_SLOT_NUM;
                    this.CassetteData.dSlotPitch = CASSETTE_DEFAULT_PITCH;
                    for (int i = 0; i < this.CassetteData.nWaferData.Length; i++)
                    {
                        this.CassetteData.nWaferData[i] = (int)ECassetteWaferInfo.NONE;
                    }
                }
                else  // Cassette Data Copy
                {
                    this.CassetteData = CassetteData;
                }

                ElevatorZone = new CMAxisZoneCheck((int)EElevatorXAxZone.MAX, (int)EElevatorYAxZone.MAX,
                    (int)EElevatorTAxZone.MAX, (int)EElevatorZAxZone.MAX);
            }
        }
    }
    
    public class MMeElevator : MMechanicalLayer
    {
        private CMeElevatorRefComp m_RefComp;
        private CMeElevatorData m_Data;

        // MovingObject
        private CMovingObject AxElevatorInfo = new CMovingObject((int)EElevatorPos.MAX);
    
        MTickTimer m_waitTimer = new MTickTimer();

        public MMeElevator(CObjectInfo objInfo, CMeElevatorRefComp refComp, CMeElevatorData data)
            : base(objInfo)
        {
            m_RefComp = refComp;
            SetData(data);
        }

        public int SetData(CMeElevatorData source)
        {
            m_Data = ObjectExtensions.Copy(source);
            return SUCCESS;
        }

        public int GetData(out CMeElevatorData target)
        {
            target = ObjectExtensions.Copy(m_Data);

            return SUCCESS;
        }

        public int SetElevatorPosition(CUnitPos FixedPos, CUnitPos ModelPos, CUnitPos OffsetPos)
        {
            AxElevatorInfo.SetPosition(FixedPos, ModelPos, OffsetPos);
            
            return SUCCESS;
        }        
        
        public int GetElevatorCurPos(out CPos_XYTZ pos)
        {
            m_RefComp.AxElevator.GetCurPos(out pos);
            return SUCCESS;
        }

        public void SetSlevatorSlotData(int nSlotNum, ECassetteWaferInfo WaferInfo)
        {
            m_Data.CassetteData.nWaferData[nSlotNum] = (int)WaferInfo;
        }
        public void GetSlevatorSlotData(int nSlotNum, out int nData)
        {
            nData = m_Data.CassetteData.nWaferData[nSlotNum];
        }

        /// <summary>
        /// sPos으로 이동하고, PosInfo를 iPos으로 셋팅한다. Backlash는 일단 차후로.
        /// </summary>
        /// <param name="sPos"></param>
        /// <param name="iPos"></param>
        /// <param name="bMoveFlag"></param>
        /// <param name="bUseBacklash"></param>
        /// <returns></returns>
        public int MoveElevatorPos(CPos_XYTZ sPos, int iPos, bool[] bMoveFlag = null, bool bUseBacklash = false,
            bool bUsePriority = false, int[] movePriority = null)
        {
            int iResult = SUCCESS;

            // safety check
            iResult = CheckForElevatorAxisMove();
            if (iResult != SUCCESS) return iResult;

            // assume move Z axis if bMoveFlag is null
            if(bMoveFlag == null)
            {
                bMoveFlag = new bool[DEF_MAX_COORDINATE] { false, false, false, true };
            }

            // Bottom Position으로 가는 것이면 Align Offset을 초기화해야 한다.
            if (iPos == (int)EElevatorPos.BOTTOM)
            {
                AxElevatorInfo.InitAlignOffset();
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
                    m_RefComp.AxElevator.SetAxesMovePriority(movePriority);
                }

                // move
                bMoveFlag[DEF_Z] = false;
                iResult = m_RefComp.AxElevator.Move(DEF_ALL_COORDINATE, bMoveFlag, dTargetPos, bUsePriority);
                if (iResult != SUCCESS)
                {
                    WriteLog("fail : move Elevator x y t axis", ELogType.Debug, ELogWType.Error);
                    return iResult;
                }
            }

            // 2. move Z Axis
            if (bMoveFlag[DEF_Z] == true)
            {
                bool[] bTempFlag = new bool[DEF_MAX_COORDINATE] { false, false, false, true };
                iResult = m_RefComp.AxElevator.Move(DEF_ALL_COORDINATE, bTempFlag, dTargetPos);
                if (iResult != SUCCESS)
                {
                    WriteLog("fail : move Elevator z axis", ELogType.Debug, ELogWType.Error);
                    return iResult;
                }
            }

            // set working pos
            if (iPos > (int)EElevatorPos.NONE)
            {
                AxElevatorInfo.PosInfo = iPos;
            }

            string str = $"success : move Elevator to pos:{iPos} {sPos.ToString()}";
            WriteLog(str, ELogType.Debug, ELogWType.Normal);

            return SUCCESS;
        }

        /// <summary>
        /// iPos 좌표로 선택된 축들을 이동시킨다.
        /// </summary>
        /// <param name="iPos">목표 위치</param>
        /// <param name="SlotNum">목표 Slow 위치</param>
        /// <param name="bUpdatedPosInfo">목표위치값을 update 할지의 여부</param>
        /// <param name="bMoveFlag">이동시킬 축 선택 </param>
        /// <param name="dMoveOffset">임시 옵셋값 </param>
        /// <param name="bUseBacklash"></param>
        /// <param name="bUsePriority">우선순위 이동시킬지 여부 </param>
        /// <param name="movePriority">우선순위 </param>
        /// <returns></returns>
        public int MoveElevatorPos(int iPos, int SlotNum =0, bool bUpdatedPosInfo = true, 
            bool[] bMoveFlag = null, double[] dMoveOffset = null, bool bUseBacklash = false,
            bool bUsePriority = false, int[] movePriority = null)
        {
            int iResult = SUCCESS;            

            // Load Position으로 가는 것이면 Align Offset을 초기화해야 한다.
            if (iPos == (int)EElevatorPos.LOAD)
            {
                AxElevatorInfo.InitAlignOffset();
            }
            // Slot Position으로 가는 것이면 Slot번호와 Pitch를 곱해서 Offset을 적용한다.
            if (iPos == (int)EElevatorPos.SLOT)
            {
                dMoveOffset[DEF_X] = 0.0;
                dMoveOffset[DEF_Y] = 0.0;
                dMoveOffset[DEF_T] = 0.0;
                dMoveOffset[DEF_Z] = (double)SlotNum * m_Data.CassetteData.dSlotPitch;

            }
            // 이동할 위치의 값을 읽어옴.
            CPos_XYTZ sTargetPos = AxElevatorInfo.GetTargetPos(iPos);
            if (dMoveOffset != null)
            {
                sTargetPos = sTargetPos + dMoveOffset;
            }

            if(bUpdatedPosInfo == false)
            {
                //iPos = (int)EElevatorPos.NONE;
                return GenerateErrorCode(ERR_Elevator_UNABLE_TO_USE_POSITION);
            }
            iResult = MoveElevatorPos(sTargetPos, iPos, bMoveFlag, bUseBacklash, bUsePriority, movePriority);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }
        
        /// <summary>
        /// Elevator를 LOAD, UNLOAD등의 목표위치로 이동시킬때에 좀더 편하게 이동시킬수 있도록 간편화한 함수
        /// Z축만 움직일 경우엔 Position Info를 업데이트 하지 않는다. 
        /// </summary>
        /// <param name="iPos"></param>
        /// <param name="bMoveAllAxis"></param>
        /// <param name="bMoveXYT"></param>
        /// <param name="bMoveZ"></param>
        /// <returns></returns>
        public int MoveElevatorPos(int iPos, int SlotNum=0, bool bMoveAllAxis=false, bool bMoveXYT=false, bool bMoveZ=true)
        {
            // 0. move all axis
            if (bMoveAllAxis)
            {
                bool[] bMoveFlag = new bool[DEF_MAX_COORDINATE] { true, true, true, true };
                return MoveElevatorPos(iPos, SlotNum, true, bMoveFlag);
            }

            // 1. move xyt only
            if (bMoveXYT)
            {
                bool[] bMoveFlag = new bool[DEF_MAX_COORDINATE] { true, true, true, false };
                return MoveElevatorPos(iPos, SlotNum, true, bMoveFlag);
            }

            // 2. move z only
            if (bMoveZ)
            {
                bool[] bMoveFlag = new bool[DEF_MAX_COORDINATE] { false, false, false, true };
                return MoveElevatorPos(iPos, SlotNum, false, bMoveFlag);
            }

            return SUCCESS;
        }

        public int MoveElevatorToBottomPos(bool bMoveAllAxis = false, bool bMoveXYT = false, bool bMoveZ = true)
        {
            int iPos = (int)EElevatorPos.BOTTOM;
            int iSlotNum = 0;
            
            return MoveElevatorPos(iPos, iSlotNum, bMoveAllAxis, bMoveXYT, bMoveZ);
        }

        public int MoveElevatorToLoadPos(bool bMoveAllAxis = false, bool bMoveXYT = false, bool bMoveZ = true)
        {
            int iPos = (int)EElevatorPos.LOAD;
            int iSlotNum = 0;
            return MoveElevatorPos(iPos, iSlotNum, bMoveAllAxis, bMoveXYT, bMoveZ);
        }

        public int MoveElevatorToSlotPos(int iSlotNum=0, bool bMoveAllAxis = false, bool bMoveXYT = false, bool bMoveZ = true)
        {
            int iPos = (int)EElevatorPos.SLOT;
            return MoveElevatorPos(iPos, iSlotNum, bMoveAllAxis, bMoveXYT, bMoveZ);
        }
        public int MoveElevatorToTopPos(bool bMoveAllAxis = false, bool bMoveXYT = false, bool bMoveZ = true)
        {
            int iPos = (int)EElevatorPos.TOP;
            int iSlotNum = 0;
            return MoveElevatorPos(iPos, iSlotNum,bMoveAllAxis, bMoveXYT, bMoveZ);
        }
        /// <summary>
        /// 다음 Slot으로 이동함. 
        /// </summary>
        /// <param name="bDirect"></param>
        /// <returns></returns>
        public int MoveElevatorNextSlot(bool bDirect=true)
        {
            bool bMoveAllAxis = false;
            bool bMoveXYT = false;
            bool bMoveZ = true;

            int nElevatorPos;
            int nSlotNum = 0;
            int nCurSlotNum = 0;
            
            // 현재 위치를 읽어옴
            GetElevatorPosInfo(out nElevatorPos, out nCurSlotNum);

            if (nElevatorPos != (int)EElevatorPos.SLOT)
                GenerateErrorCode(ERR_Elevator_UNABLE_TO_USE_POSITION);

            // 현재 위치한 Slot 번호를 대입한다.
            nSlotNum = nCurSlotNum;

            // 방향에 따라 +1 / -1을 함.
            if (bDirect) nSlotNum++;
            else nSlotNum--;

            return MoveElevatorPos(nElevatorPos, nSlotNum, bMoveAllAxis, bMoveXYT, bMoveZ);
        }

        /// <summary>
        /// Cassette의 Empty Slot을 차례로 아래부터 위 방향으로 이동한다.
        /// </summary>
        /// <returns></returns>
        public int MoveElevatorNextEmptySlot()
        {
            bool bMoveAllAxis = false;
            bool bMoveXYT = false;
            bool bMoveZ = true;

            int nResult = 0;
            int nElevatorPos = (int)EElevatorPos.SLOT;
            int nSlotNum = (int)ECassetteWaferInfo.NONE;
            int nCurSlotNum = 0;

            // Cassette의 Wafer Data를 아래부터 읽어 Empty Slot를 찾는다.
            for(int nNum=0; nNum < m_Data.CassetteData.nSlotNum; nNum++ )
            {
                if (m_Data.CassetteData.nWaferData[nNum] == (int)ECassetteWaferInfo.EMPTY) nSlotNum = nNum;
            }

            // 해당 Slot이 없을 경우 에러를 리턴함.
            if (nSlotNum == (int)ECassetteWaferInfo.NONE) GenerateErrorCode(ERR_Elevator_UNABLE_TO_USE_POSITION);

            // 해당 위치로 이동함.
            nResult = MoveElevatorPos(nElevatorPos, nSlotNum, bMoveAllAxis, bMoveXYT, bMoveZ);
            if(nResult != SUCCESS ) GenerateErrorCode(ERR_Elevator_MOVE_FAIL);

            Sleep(500);

            // Wafer Frame 감지 센서를 확인하여 Empty 여부를 확인한다.
            bool bStatus;
            nResult = m_RefComp.IO.IsOn(m_Data.InDetectWafer, out bStatus);

            // Input확인 동작 확인
            if (nResult != SUCCESS) GenerateErrorCode(ERR_Elevator_UNABLE_TO_USE_IO);
            // Wafer 유무 확인 ( Wafer 감지 센서 On이면 Err 리턴 )
            if(bStatus) GenerateErrorCode(ERR_Elevator_UNABLE_TO_USE_IO);

            return SUCCESS;
        }

        public int MoveElevatorNextProcessWaferSlot()
        {
            bool bMoveAllAxis = false;
            bool bMoveXYT = false;
            bool bMoveZ = true;

            int nResult = 0;
            int nElevatorPos = (int)EElevatorPos.SLOT;
            int nSlotNum = (int)ECassetteWaferInfo.NONE;
            int nCurSlotNum = 0;

            // Cassette의 Wafer Data를 아래부터 읽어 Empty Slot를 찾는다.
            for (int nNum = 0; nNum < m_Data.CassetteData.nSlotNum; nNum++)
            {
                if (m_Data.CassetteData.nWaferData[nNum] == (int)ECassetteWaferInfo.PRE_PROCESS) nSlotNum = nNum;
            }

            // 해당 Slot이 없을 경우 에러를 리턴함.
            if (nSlotNum == (int)ECassetteWaferInfo.NONE) GenerateErrorCode(ERR_Elevator_UNABLE_TO_USE_POSITION);

            // 해당 위치로 이동함.
            nResult = MoveElevatorPos(nElevatorPos, nSlotNum, bMoveAllAxis, bMoveXYT, bMoveZ);
            if (nResult != SUCCESS) GenerateErrorCode(ERR_Elevator_MOVE_FAIL);

            Sleep(500);

            // Wafer Frame 감지 센서를 확인하여 Empty 여부를 확인한다.
            bool bStatus;
            nResult = m_RefComp.IO.IsOn(m_Data.InDetectWafer, out bStatus);

            // Input확인 동작 확인
            if (nResult != SUCCESS) GenerateErrorCode(ERR_Elevator_UNABLE_TO_USE_IO);
            // Wafer 유무 확인 ( Wafer 감지 센서 Off이면 Err 리턴 )
            if (!bStatus) GenerateErrorCode(ERR_Elevator_UNABLE_TO_USE_IO);

            return SUCCESS;
        }

        public int SearchElevatorCassetteWafer()
        {
            bool bMoveAllAxis = false;
            bool bMoveXYT = false;
            bool bMoveZ = true;
            bool bStatus = false;

            int nResult = -1;
            int nElevatorPos = (int)EElevatorPos.SLOT;

            // Cassette 유무를 확인한다.


            // Slot 위치를 확인하며 Wafer의 유무를 확인한다.            
            for (int nNum = 0; nNum < m_Data.CassetteData.nSlotNum; nNum++)
            {
                // 해당 위치로 이동함.
                nResult = MoveElevatorPos(nElevatorPos, nNum, bMoveAllAxis, bMoveXYT, bMoveZ);
                if (nResult != SUCCESS) GenerateErrorCode(ERR_Elevator_MOVE_FAIL);

                // Wafer Frame 감지 센서를 확인하여 Empty 여부를 확인한다.               
                nResult = m_RefComp.IO.IsOn(m_Data.InDetectWafer, out bStatus);
                if (nResult != SUCCESS) return nResult;

                if (bStatus)
                {
                    m_Data.CassetteData.nWaferData[nNum] = (int)ECassetteWaferInfo.PRE_PROCESS;
                }
                else
                {
                    m_Data.CassetteData.nWaferData[nNum] = (int)ECassetteWaferInfo.EMPTY;
                }

                Sleep(100);

            }

            return SUCCESS;
        }
        /// <summary>
        /// 현재 위치와 목표위치의 위치차이 Tolerance check
        /// </summary>
        /// <param name="sPos"> 목표 위치값</param>
        /// <param name="bResult"></param>
        /// <param name="bCheck_TAxis"></param>
        /// <param name="bCheck_ZAxis"></param>
        /// <param name="bSkipError">위치가 틀릴경우 에러 보고할지 여부</param>
        /// <returns></returns>
        public int CompareElevatorPos(CPos_XYTZ sPos, out bool bResult, 
                        bool bCheck_XAxis, bool bCheck_YAxis, bool bCheck_TAxis, bool bSkipError = true)
        {
            int iResult = SUCCESS;

            bResult = false;

            // trans to array
            double[] dPos;
            sPos.TransToArray(out dPos);

            bool[] bJudge = new bool[DEF_MAX_COORDINATE];
            iResult = m_RefComp.AxElevator.ComparePosition(dPos, out bJudge, DEF_ALL_COORDINATE);
            if (iResult != SUCCESS) return iResult;

            // skip axis
            if (bCheck_XAxis == false) bJudge[DEF_X] = true;
            if (bCheck_YAxis == false) bJudge[DEF_Y] = true;
            if (bCheck_TAxis == false) bJudge[DEF_T] = true;            

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

                return GenerateErrorCode(ERR_Elevator_NOT_SAME_POSITION);
            }

            return SUCCESS;
        }

        public int CompareElevatorPos(int iPos, out bool bResult,out int nSlotNum, bool bSkipError = true)
        {
            int iResult = SUCCESS;

            bool bCheck_XAxis = false;
            bool bCheck_YAxis = false;
            bool bCheck_TAxis = false;

            bResult = false;
            nSlotNum = -1;

            CPos_XYTZ targetPos = AxElevatorInfo.GetTargetPos(iPos);
            if (iResult != SUCCESS) return iResult;

            iResult = CompareElevatorPos(targetPos, out bResult, bCheck_XAxis, bCheck_YAxis, bCheck_TAxis, bSkipError);
            if (iResult != SUCCESS) return iResult;

            // Slot Position Cals (Result가 true일 경우)
            if(iPos== (int)EElevatorPos.SLOT && bResult)
            {
                double dReferencePos = 0.0;
                CPos_XYTZ LoadPos = AxElevatorInfo.GetTargetPos((int)EUnitPos.LOAD);
                dReferencePos = targetPos.dZ - LoadPos.dZ;
                m_Data.CurrentSlotNum = (int)(dReferencePos / m_Data.CassetteData.dSlotPitch);

                nSlotNum = m_Data.CurrentSlotNum;
            }

            return SUCCESS;
        }

        public int GetElevatorPosInfo(out int posInfo, out int nSlotNum, bool bUpdatePos = true, bool bSkipError = false)
        {
            posInfo = (int)EElevatorPos.NONE;
            nSlotNum = -1;

            bool bStatus;
            int iResult = IsElevatorOrignReturn(out bStatus);
            if (iResult != SUCCESS) return iResult;

            // 실시간으로 자기 위치를 체크
            if(bUpdatePos)
            {
                for (int i = 0; i < (int)EElevatorPos.MAX; i++)
                {
                    CompareElevatorPos(i, out bStatus, out nSlotNum, bSkipError);
                    if (bStatus)
                    {
                        AxElevatorInfo.PosInfo = i;
                        break;
                    }
                }
            }

            posInfo = AxElevatorInfo.PosInfo;
            return SUCCESS;
        }

        public void SetElevatorPosInfo(int posInfo)
        {
            AxElevatorInfo.PosInfo = posInfo;
        }

        public int IsElevatorOrignReturn(out bool bStatus)
        {
            bool[] bAxisStatus;
            m_RefComp.AxElevator.IsOriginReturn(DEF_ALL_COORDINATE, out bStatus, out bAxisStatus);

            return SUCCESS;
        }

        public int IsObjectDetected(out bool bStatus)
        {
            int iResult = m_RefComp.IO.IsOn(m_Data.InDetectWafer, out bStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }
        
        ////////////////////////////////////////////////////////////////////////

        public int GetElevatorAxZone(int axis, out int curZone)
        {
            bool bStatus;
            curZone = (int)EElevatorXAxZone.NONE;
            for(int i = 0; i < (int)EElevatorXAxZone.MAX; i++)
            {
                if (m_Data.ElevatorZone.Axis[axis].ZoneAddr[i] == -1) continue; // if io is not allocated, continue;
                int iResult = m_RefComp.IO.IsOn(m_Data.ElevatorZone.Axis[axis].ZoneAddr[i], out bStatus);
                if (iResult != SUCCESS) return iResult;
                if (bStatus == true)
                {
                    curZone = i;
                    break;
                }
            }
            return SUCCESS;
        }

        public int IsElevatorAxisInSafetyZone(int axis, out bool bStatus)
        {
            bStatus = false;
            int curZone;
            int iResult = GetElevatorAxZone(axis, out curZone);
            if (iResult != SUCCESS) return iResult;

            switch(axis)
            {
                case DEF_X:
                    break;

                case DEF_Y:
                    if (curZone == (int)EElevatorYAxZone.SAFETY)
                    {
                        bStatus = true;
                    }
                    break;

                case DEF_T:
                    if (curZone == (int)EElevatorTAxZone.SAFETY)
                    {
                        bStatus = true;
                    }
                    break;

                case DEF_Z:
                    if (curZone == (int)EElevatorZAxZone.SAFETY)
                    {
                        bStatus = true;
                    }
                    break;
            }
            return SUCCESS;
        }

        public int CheckForElevatorAxisMove(bool bCheckVacuum = true)
        {
            bool bStatus;

            // check origin
            int nResult = IsElevatorOrignReturn(out bStatus);

            if (nResult != SUCCESS) return nResult;
            if(bStatus == false)
            {
                return GenerateErrorCode(ERR_Elevator_NOT_ORIGIN_RETURNED);
            }

            // Cassette 감지 센서 확인 (정위치 확인 or Cassette없음 Check)
            bool[] bCheckIO = new bool[CASSETTE_DETECT_SENSOR_NUM];

            bool bCassetteExist;
            bool bCassetteNone;
            nResult = CheckForElevatorCassetteExist(out bCassetteExist);
            if (nResult != SUCCESS) return nResult;

            nResult = CheckForElevatorCassetteNone(out bCassetteNone);
            if (nResult != SUCCESS) return nResult;

            // 전체가 On 이거나 Off되어야 동작 가능함.
            if (bCassetteExist || bCassetteNone)
            {
                return SUCCESS;
            }
            else
            {
                return GenerateErrorCode(ERR_Elevator_CASSETTE_NOT_READY);
            }
            
        }

        public int CheckForElevatorCassetteExist(out bool bExist)
        {
            bExist = false;
            int nResult = -1;

            // Wafer Frame 감지 센서 확인
            bool[] bCheckIO = new bool[CASSETTE_DETECT_SENSOR_NUM];

            nResult = m_RefComp.IO.IsOn(m_Data.InDetectCassette[0], out bCheckIO[0]) +
                      m_RefComp.IO.IsOn(m_Data.InDetectCassette[1], out bCheckIO[1]) +
                      m_RefComp.IO.IsOn(m_Data.InDetectCassette[2], out bCheckIO[2]) +
                      m_RefComp.IO.IsOn(m_Data.InDetectCassette[3], out bCheckIO[3]);

            if (nResult != SUCCESS) return nResult;

            // 전체가 On 일 경우에 True
            if (bCheckIO[0] && bCheckIO[1] && bCheckIO[2] && bCheckIO[3])
            {
                bExist = true;                
            }

            return SUCCESS;

        }

        public int CheckForElevatorCassetteNone(out bool bExist)
        {
            bExist = false;
            int nResult = -1;

            // Wafer Frame 감지 센서 확인
            bool[] bCheckIO = new bool[CASSETTE_DETECT_SENSOR_NUM];

            nResult = m_RefComp.IO.IsOn(m_Data.InDetectCassette[0], out bCheckIO[0]) +
                      m_RefComp.IO.IsOn(m_Data.InDetectCassette[1], out bCheckIO[1]) +
                      m_RefComp.IO.IsOn(m_Data.InDetectCassette[2], out bCheckIO[2]) +
                      m_RefComp.IO.IsOn(m_Data.InDetectCassette[3], out bCheckIO[3]);

            if (nResult != SUCCESS) return nResult;

            // 전체가 Off 일 경우에 True
            if (!bCheckIO[0] && !bCheckIO[1] && !bCheckIO[2] && !bCheckIO[3])
            {
                bExist = true;
            }

            return SUCCESS;

        }



    }
}
