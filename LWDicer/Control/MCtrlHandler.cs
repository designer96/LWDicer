using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static LWDicer.Control.DEF_Error;
using static LWDicer.Control.DEF_Common;
using static LWDicer.Control.DEF_Thread;
using static LWDicer.Control.DEF_Motion;
using static LWDicer.Control.DEF_MeHandler;
using static LWDicer.Control.DEF_CtrlHandler;

namespace LWDicer.Control
{
    public class DEF_CtrlHandler
    {
        public const int ERR_CTRLHANDLER_UNABLE_TO_USE_HANDLER                         = 1;
        public const int ERR_CTRLHANDLER_UNABLE_TO_USE_LOG                             = 2;
        public const int ERR_CTRLHANDLER_OBJECT_ABSORBED                               = 3;
        public const int ERR_CTRLHANDLER_OBJECT_NOT_ABSORBED                           = 4;
        public const int ERR_CTRLHANDLER_OBJECT_EXIST                                  = 5;
        public const int ERR_CTRLHANDLER_OBJECT_NOT_EXIST                              = 6;
        public const int ERR_CTRLHANDLER_CHECK_RUN_BEFORE_FAILED                       = 7;
        public const int ERR_CTRLHANDLER_CYLINDER_TIMEOUT                              = 8;
        public const int ERR_CTRLHANDLER_NOT_UP                                        = 9;
        public const int ERR_CTRLHANDLER_CANNOT_DETECT_POSINFO                         = 10;
        public const int ERR_CTRLHANDLER_PCB_DOOR_OPEN                                 = 11;
        public const int ERR_CTRLHANDLER_UHANDLER_IN_DOWN_AND_LHANDLER_IN_SAME_XZONE   = 12;
        public const int ERR_CTRLHANDLER_UHANDLER_NEED_DOWN_AND_LHANDLER_IN_SAME_XZONE = 13;
        public const int ERR_CTRLHANDLER_LHANDLER_NEED_MOVE_AND_UHANDLER_IN_DOWN       = 14;
        public const int ERR_CTRLHANDLER_XAX_POS_NOT_MATCH_ZONE                        = 15;

        /// <summary>
        /// Handler가 Upper/Lower 두 종류인데, 각각 Upper = LOAD, Lower = UNLOAD 용도로 사용
        /// </summary>
        public enum EHandlerIndex
        {
            LOAD_UPPER,     // Use UpperHandler for Loading
            UNLOAD_LOWER,   // Use LowerHandler for Unloading
        }

        public class CCtrlHandlerRefComp
        {
            // MeHandler
            public MMeHandler UpperHandler;
            public MMeHandler LowerHandler;

            // MeStage
            //public MMeStage1 Stage1;

            // MePushPull
            //public MMePushPull PushPull;

            public CCtrlHandlerRefComp()
            {
            }

            public override string ToString()
            {
                return $"CCtrlHandlerRefComp : ";
            }
        }

        public class CCtrlHandlerData
        {
            public CCtrlHandlerData()
            {

            }
        }
    }

    public class MCtrlHandler : MCtrlLayer
    {
        private CCtrlHandlerRefComp m_RefComp;
        private CCtrlHandlerData m_Data;

        public MCtrlHandler(CObjectInfo objInfo, CCtrlHandlerRefComp refComp, CCtrlHandlerData data)
            : base(objInfo)
        {
            m_RefComp = refComp;
            SetData(data);
        }

        public int SetData(CCtrlHandlerData source)
        {
            m_Data = ObjectExtensions.Copy(source);

            return SUCCESS;
        }

        public int GetData(out CCtrlHandlerData target)
        {
            target = ObjectExtensions.Copy(m_Data);

            return SUCCESS;
        }

        public MMeHandler GetHandler(EHandlerIndex index)
        {
            if(index == EHandlerIndex.LOAD_UPPER)
            {
                return m_RefComp.UpperHandler;
            }
            else
            {
                return m_RefComp.LowerHandler;
            }
        }

        public MMeHandler GetOtherHandler(EHandlerIndex index)
        {
            if (index == EHandlerIndex.LOAD_UPPER)
            {
                return m_RefComp.LowerHandler;
            }
            else
            {
                return m_RefComp.UpperHandler;
            }
        }

        public EHandlerIndex GetOtherIndex(EHandlerIndex index)
        {
            if (index == EHandlerIndex.LOAD_UPPER)
            {
                return EHandlerIndex.UNLOAD_LOWER;
            }
            else
            {
                return EHandlerIndex.LOAD_UPPER;
            }
        }

        public int IsObjectDetected(EHandlerIndex index, out bool bStatus)
        {
            bStatus = false;
            int iResult = GetHandler(index).IsObjectDetected(out bStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int IsAbsorbed(EHandlerIndex index, out bool bStatus)
        {
            bStatus = false;
            int iResult = GetHandler(index).IsAbsorbed(out bStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int IsReleased(EHandlerIndex index, out bool bStatus)
        {
            bStatus = false;
            int iResult = GetHandler(index).IsReleased(out bStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int Absorb(EHandlerIndex index, bool bSkipSensor = false)
        {
            int iResult = GetHandler(index).Absorb(bSkipSensor);
            return iResult;
        }

        public int Release(EHandlerIndex index, bool bSkipSensor = false)
        {
            int iResult = GetHandler(index).Absorb(bSkipSensor);
            return iResult;
        }

        /// <summary>
        /// Handler의 이동전에 Run Mode에 따라서, Object가 감지되어야 하는지 및 진공 여부를 체크
        /// </summary>
        /// <param name="index"></param>
        /// <param name="bPanelTransfer">Object가 있어야 되는지</param>
        /// <param name="bCheckAutoRun">AutoRun모드에서 Error 발생 여부</param>
        /// <returns></returns>
        private int CheckVacuum_forMove(EHandlerIndex index, bool bPanelTransfer, bool bCheckAutoRun = false)
        {
            int iResult = SUCCESS;
            bool bDetected, bAbsorbed;

            // check vacuum
            iResult = IsObjectDetected(index, out bDetected);
            if (iResult != SUCCESS) return iResult;

            iResult = IsAbsorbed(index, out bAbsorbed);
            if (iResult != SUCCESS) return iResult;

            if (bPanelTransfer)
            {
                if (bDetected == true && bAbsorbed == false)
                {
                    iResult = Absorb(index);
                    if (iResult != SUCCESS) return iResult;

                    bAbsorbed = true;
                }
            }

            // Panel이 있든 없든 상관없는 위치들, 가령 대기, 마크 등등의 위치를 위해서
            if (bCheckAutoRun == true) return SUCCESS;

            // check object exist when auto run
            if (AutoManual == EAutoManual.AUTO)
            {
                if (OpMode != EOpMode.DRY_RUN) // not dry run
                {
                    if (bDetected != bPanelTransfer)
                    {
                        if (bPanelTransfer)    // Panel이 있어야 할 상황일경우
                        {
                            WriteLog("CtrlHandler의 이동 전 조건을 정상적으로 확인하지 못함. OBJECT NOT EXIST", ELogType.Debug, ELogWType.Error);
                            return GenerateErrorCode(ERR_CTRLHANDLER_OBJECT_NOT_EXIST);
                        }
                        else
                        {
                            WriteLog("CtrlHandler의 이동 전 조건을 정상적으로 확인하지 못함. OBJECT EXIST", ELogType.Debug, ELogWType.Error);
                            return GenerateErrorCode(ERR_CTRLHANDLER_OBJECT_EXIST);
                        }
                    }
                }
                else // dry run
                {
                    if (bDetected || bAbsorbed)
                    {
                        WriteLog("CtrlHandler의 이동 전 조건을 정상적으로 확인하지 못함. OBJECT EXIST", ELogType.Debug, ELogWType.Error);
                        return GenerateErrorCode(ERR_CTRLHANDLER_OBJECT_EXIST);
                    }
                }
            }

            //	m_plogMng->WriteLog(DEF_MLOG_NORMAL_LOG_LEVEL, "Stage3의 이동 전 조건을 정상적으로 확인함: OK", __FILE__, __LINE__);
            return SUCCESS;
        }

        public int CheckHandlerPosition(EHandlerIndex index, out int curPos, 
            int firstCheckPos = (int)EHandlerPos.NONE, bool bCheck_ZAxis = true)
        {
            int iResult = SUCCESS;

            // Init
            curPos = (int)EHandlerPos.NONE;

            // 1. check first position
            if(firstCheckPos != (int)EHandlerPos.NONE)
            {
                bool bResult;
                iResult = GetHandler(index).CompareHandlerPos(firstCheckPos, out bResult, false, bCheck_ZAxis);
                if (iResult != SUCCESS) return iResult;
                if(bResult)
                {
                    curPos = firstCheckPos;
                    return SUCCESS;
                }
            }

            // Get Position Info
            iResult = GetHandler(index).GetHandlerPosInfo(out curPos, true, bCheck_ZAxis);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        private int CheckXAxMatchZone(EHandlerIndex index, int curPos, int curZone_X)
        {
            switch (curPos)
            {
                case (int)EHandlerPos.LOAD:
                    if (curZone_X != (int)EHandlerXAxZone.LOAD)
                        return GenerateErrorCode(ERR_CTRLHANDLER_XAX_POS_NOT_MATCH_ZONE);
                    break;
                case (int)EHandlerPos.WAIT:
                    if (curZone_X != (int)EHandlerXAxZone.WAIT)
                        return GenerateErrorCode(ERR_CTRLHANDLER_XAX_POS_NOT_MATCH_ZONE);
                    break;
                case (int)EHandlerPos.UNLOAD:
                    if (curZone_X != (int)EHandlerXAxZone.UNLOAD)
                        return GenerateErrorCode(ERR_CTRLHANDLER_XAX_POS_NOT_MATCH_ZONE);
                    break;
            }
            return SUCCESS;
        }

        public int CheckSafety_forMove(EHandlerIndex index, int nTargetPos, bool bPanelTransfer, bool bMozeZAxis)
        {
            int iResult = SUCCESS;

            // 0. init
            int curPos = (int)EHandlerPos.NONE;

            // 0.1 check vacuum
            iResult = CheckVacuum_forMove(index, bPanelTransfer);
            if (iResult != SUCCESS) return iResult;

            // 1 check object exist
            bool bObjectExist = false;
            iResult = IsObjectDetected(index, out bObjectExist);
            if (iResult != SUCCESS) return iResult;

            if(bPanelTransfer == true && bObjectExist == false)
            {
                return GenerateErrorCode(ERR_CTRLHANDLER_OBJECT_NOT_EXIST);
            }
            else if(bPanelTransfer == false && bObjectExist == true)
            {
                return GenerateErrorCode(ERR_CTRLHANDLER_OBJECT_EXIST);
            }

            // 2 get current pos
            // need to decide check position interlock..
            iResult = CheckHandlerPosition(index, out curPos, nTargetPos, false);
            if (iResult != SUCCESS) return iResult;
            if (curPos == (int)EHandlerPos.NONE)
                return GenerateErrorCode(ERR_CTRLHANDLER_CANNOT_DETECT_POSINFO);

            int other_curPos;
            iResult = CheckHandlerPosition(GetOtherIndex(index), out other_curPos, nTargetPos, false);
            if (iResult != SUCCESS) return iResult;
            if (other_curPos == (int)EHandlerPos.NONE)
                return GenerateErrorCode(ERR_CTRLHANDLER_CANNOT_DETECT_POSINFO);

            // 3. get current zone
            int curZone_X, other_curZone_X;
            iResult = GetHandler(index).GetHandlerAxZone(DEF_X, out curZone_X);
            if (iResult != SUCCESS) return iResult;
            iResult = GetOtherHandler(index).GetHandlerAxZone(DEF_X, out other_curZone_X);
            if (iResult != SUCCESS) return iResult;

            int curZone_Z, other_curZone_Z;
            iResult = GetHandler(index).GetHandlerAxZone(DEF_Z, out curZone_Z);
            if (iResult != SUCCESS) return iResult;
            iResult = GetOtherHandler(index).GetHandlerAxZone(DEF_Z, out other_curZone_Z);
            if (iResult != SUCCESS) return iResult;

            // 4. check curPos match cur zone
            iResult = CheckXAxMatchZone(index, curPos, curZone_X);
            if (iResult != SUCCESS) return iResult;

            iResult = CheckXAxMatchZone(GetOtherIndex(index), other_curPos, other_curZone_X);
            if (iResult != SUCCESS) return iResult;

            // 5. check interlock within handlers
            if (index == (int)EHandlerIndex.LOAD_UPPER) // Upper Handler
            {
                // check cur position, because uhandler may collide when z axis is in down pos.
                if(curZone_Z != (int)EHandlerZAxZone.SAFETY)
                {
                    if(curZone_X == other_curZone_X)
                    {
                        return GenerateErrorCode(ERR_CTRLHANDLER_UHANDLER_IN_DOWN_AND_LHANDLER_IN_SAME_XZONE);
                    }
                }

                // check target position,
                if(bMozeZAxis == true && 
                    (nTargetPos == (int)EHandlerPos.LOAD || nTargetPos == (int)EHandlerPos.UNLOAD))
                {
                    if(nTargetPos == other_curPos)
                    {
                        return GenerateErrorCode(ERR_CTRLHANDLER_UHANDLER_NEED_DOWN_AND_LHANDLER_IN_SAME_XZONE);
                    }
                }
            }
            else // Lower Handler
            {
                // check cur position, because uhandler may collide when z axis is in down pos.
                if (other_curZone_Z != (int)EHandlerZAxZone.SAFETY)
                {
                    if (curZone_X == other_curZone_X)
                    {
                        return GenerateErrorCode(ERR_CTRLHANDLER_UHANDLER_IN_DOWN_AND_LHANDLER_IN_SAME_XZONE);
                    }

                    // UHandler가 중간 지점 wait zone에서 down 되어있을경우, 못 움직인다.
                    if(other_curZone_X == (int)EHandlerXAxZone.WAIT)
                    {
                        return GenerateErrorCode(ERR_CTRLHANDLER_LHANDLER_NEED_MOVE_AND_UHANDLER_IN_DOWN);
                    }

                    // 목표 위치와 UHandler가 같은 지역일때
                    if (nTargetPos == other_curPos)
                    {
                        return GenerateErrorCode(ERR_CTRLHANDLER_LHANDLER_NEED_MOVE_AND_UHANDLER_IN_DOWN);
                    }
                }
            }

            return SUCCESS;
        }

        public int MoveToWaitPos(EHandlerIndex index, bool bPanelTransfer, bool bMoveXYT = true, bool bMoveZ = true, double dZMoveOffset = 0)
        {
            double[] dMoveOffset = new double[DEF_XYTZ];
            dMoveOffset[DEF_Z] = dZMoveOffset;

            int iResult = CheckSafety_forMove(index, (int)EHandlerPos.WAIT, bPanelTransfer, bMoveZ);
            if (iResult != SUCCESS) return iResult;

            iResult = GetHandler(index).MoveHandlerToWaitPos(bMoveXYT, bMoveZ, dMoveOffset);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int MoveToLoadPos(EHandlerIndex index, bool bPanelTransfer, bool bMoveXYT = true, bool bMoveZ = true, double dZMoveOffset = 0)
        {
            double[] dMoveOffset = new double[DEF_XYTZ];
            dMoveOffset[DEF_Z] = dZMoveOffset;

            int iResult = CheckSafety_forMove(index, (int)EHandlerPos.LOAD, bPanelTransfer, bMoveZ);
            if (iResult != SUCCESS) return iResult;

            iResult = GetHandler(index).MoveHandlerToLoadPos(bMoveXYT, bMoveZ, dMoveOffset);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int MoveToUnloadPos(EHandlerIndex index, bool bPanelTransfer, bool bMoveXYT = true, bool bMoveZ = true, double dZMoveOffset = 0)
        {
            double[] dMoveOffset = new double[DEF_XYTZ];
            dMoveOffset[DEF_Z] = dZMoveOffset;

            int iResult = CheckSafety_forMove(index, (int)EHandlerPos.UNLOAD, bPanelTransfer, bMoveZ);
            if (iResult != SUCCESS) return iResult;

            iResult = GetHandler(index).MoveHandlerToUnloadPos(bMoveXYT, bMoveZ, dMoveOffset);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

    }
}
