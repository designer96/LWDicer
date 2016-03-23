using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MotionYMC;

using static LWDicer.Control.DEF_System;
using static LWDicer.Control.DEF_Common;
using static LWDicer.Control.DEF_Error;
using static LWDicer.Control.DEF_Yaskawa;
using static LWDicer.Control.DEF_IO;
using static LWDicer.Control.DEF_Motion;
using static LWDicer.Control.DEF_MultiAxesYMC;

namespace LWDicer.Control
{
    public class DEF_MultiAxesYMC
    {
        public const int ERR_MAXES_YMC_INVALID_AXIS_ID = 1;
        public const int ERR_MAXES_YMC_INVALID_AXIS_NUMBER = 2;

        public class CMutliAxesYMCRefComp
        {
            public MYaskawa Motion;
        }

        public class CMultiAxesYMCData
        {
            public int DeviceNo;    // Yaskawa Device 번호 for controll by device group
            public int[] AxisList = new int[DEF_MAX_COORDINATE];    // Yaskawa 축 번호, 미사용축은 EYMC_Axis.NULL 로 셋팅

            public CMultiAxesYMCData(int DeviceNo, int[] AxisList)
            {
                this.DeviceNo = DeviceNo;
                Array.Copy(AxisList, this.AxisList, AxisList.Length);
            }
        }

    }

    public class MMultiAxes_YMC : MObject
    {
        private CMutliAxesYMCRefComp m_RefComp;
        private CMultiAxesYMCData m_Data;

        private int[] MovePriority = new int[DEF_MAX_COORDINATE];   // 축 이동시에 우선 순위
        private int[] OriginPriority = new int[DEF_MAX_COORDINATE];   // 축 원점복귀시에 우선 순위
        private CServoStatus[] ServoStatus = new CServoStatus[DEF_MAX_COORDINATE];

        public MMultiAxes_YMC(CObjectInfo objInfo, CMutliAxesYMCRefComp refComp, CMultiAxesYMCData data)
            : base(objInfo)
        {
            m_RefComp = refComp;
            SetData(data);

            for(int i = 0; i < DEF_MAX_COORDINATE; i++)
            {
                ServoStatus[i] = new CServoStatus();
            }
        }

        public int SetData(CMultiAxesYMCData source)
        {
            m_Data = ObjectExtensions.Copy(source);

            return SUCCESS;
        }

        public int GetData(out CMultiAxesYMCData target)
        {
            target = ObjectExtensions.Copy(m_Data);

            return SUCCESS;
        }

        public int SetStop(int iCoordID = DEF_ALL_COORDINATE, short siType = DEF_STOP)
        {
            int iResult = SUCCESS;

            int deviceNo = (iCoordID == DEF_ALL_COORDINATE) ? m_Data.DeviceNo : m_Data.AxisList[iCoordID];
            iResult = m_RefComp.Motion.ServoMotionStop(deviceNo);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int SetAxesMovePriority(int iCoordID, params int[] iPriorities)
        {
            int iResult = SUCCESS;

            for (int i = 0; i < DEF_MAX_COORDINATE; i++)
            {
                MovePriority[i] = (int)EPriority.NONE;
            }

            if (iCoordID == DEF_ALL_COORDINATE)
            {
                for (int i = 0; i < iPriorities.Length; i++)
                {
                    MovePriority[i] = iPriorities[i];
                }
            }
            else
            {
                MovePriority[iCoordID] = iPriorities[0];
            }

            return SUCCESS;
        }

        public int SetAxesOriginPriority(int iCoordID, params int[] iPriorities)
        {
            int iResult = SUCCESS;

            for (int i = 0; i < DEF_MAX_COORDINATE; i++)
            {
                OriginPriority[i] = (int)EPriority.NONE;
            }

            if (iCoordID == DEF_ALL_COORDINATE)
            {
                for (int i = 0; i < iPriorities.Length; i++)
                {
                    OriginPriority[i] = iPriorities[i];
                }
            }
            else
            {
                OriginPriority[iCoordID] = iPriorities[0];
            }

            return SUCCESS;
        }

        /// <summary>
        /// 축의 현재좌표를 읽는다.
        /// </summary>
        public int GetCurPos(int iCoordID, out double[] dCurPos)
        {
            int iResult = SUCCESS;

            UpdateAxisStatus();
            if (iCoordID == DEF_ALL_COORDINATE)
            {
                dCurPos = new double[DEF_MAX_COORDINATE];
                for (int i = 0; i < DEF_MAX_COORDINATE; i++)
                {
                    if (m_Data.AxisList[i] == DEF_AXIS_NON_ID) continue;
                    dCurPos[i] = ServoStatus[i].EncoderValue;
                }
            }
            else
            {
                dCurPos = new double[1];
                dCurPos[0] = ServoStatus[iCoordID].EncoderValue;
            }


            return SUCCESS;
        }

        /// <summary>
        /// Move 함수, 전체축 이동일 경우엔 bMoveUse와 Priority를 이용하여 순차 이동, 선택 이동 가능
        /// </summary>
        /// <param name="iCoordID"></param>
        /// <param name="bMoveUse"></param>
        /// <param name="dPosition"></param>
        /// <param name="bUsePriority"></param>
        /// <param name="tempSpeed"></param>
        /// <returns></returns>
        public int Move(int iCoordID, bool[] bMoveUse, double[] dPosition, bool bUsePriority = false,
            CMotorSpeedData[] tempSpeed = null)
        {
            int iResult = SUCCESS;
            int i, j, k;
            int iAxisID;
            int iAxisCount = 0;
            bool[] bPartUse = new bool[DEF_MAX_COORDINATE];
            bool bPartMove;

            if (iCoordID == DEF_ALL_COORDINATE)
            {
                // call api by axis group
                if (bUsePriority == true)
                {
                    for (i = 0; i < (int)EPriority.MAX; i++)
                    {
                        bPartMove = false;
                        for (j = 0; j < DEF_MAX_COORDINATE; j++)
                            bPartUse[j] = false;

                        // 우선순위안의 축 모두 이동
                        for (j = 0; j < DEF_MAX_COORDINATE; j++)
                        {
                            if (bMoveUse[j] == false) continue;
                            if (MovePriority[j] == i)
                            {
                                bPartMove = true;
                                bPartUse[j] = true;
                            }
                        }

                        if (bPartMove == true)
                        {
                            iResult = m_RefComp.Motion.MoveToPos(m_Data.AxisList, bPartUse, dPosition, tempSpeed);
                            if (iResult != SUCCESS) return iResult;
                        }
                    }
                }
                else
                {
                    iResult = m_RefComp.Motion.MoveToPos(m_Data.AxisList, bMoveUse, dPosition, tempSpeed);
                    if (iResult != SUCCESS) return iResult;
                }
            }
            else
            {
                // call api by each axis(one device)
                iResult = m_RefComp.Motion.MoveToPos(m_Data.AxisList[iCoordID], dPosition, tempSpeed);
                if (iResult != SUCCESS) return iResult;
            }

            return SUCCESS;
        }

        public int StartMove(int iCoordID, bool[] bMoveUse, double[] dPosition, CMotorSpeedData[] tempSpeed = null)
        {
            int iResult = SUCCESS;
            if (iCoordID == DEF_ALL_COORDINATE)
            {
                // call api by axis group
                iResult = m_RefComp.Motion.StartMoveToPos(m_Data.AxisList, bMoveUse, dPosition, tempSpeed);
                if (iResult != SUCCESS) return iResult;
            }
            else
            {
                // call api by each axis(one device)
                iResult = m_RefComp.Motion.StartMoveToPos(m_Data.AxisList[iCoordID], dPosition, tempSpeed);
                if (iResult != SUCCESS) return iResult;
            }

            return SUCCESS;
        }

        /// <summary>
        /// 상대위치 이동 함수
        /// </summary>
        /// <param name="iCoordID"></param>
        /// <param name="bMoveUse"></param>
        /// <param name="dPosition"></param>
        /// <param name="bUsePriority"></param>
        /// <param name="tempSpeed"></param>
        /// <returns></returns>
        public int RMove(int iCoordID, bool[] bMoveUse, double[] dPosition, bool bUsePriority = false,
    CMotorSpeedData[] tempSpeed = null)
        {
            int iResult = SUCCESS;

            double[] dCurPos;
            iResult = GetCurPos(iCoordID, out dCurPos);
            if (iResult != SUCCESS) return iResult;
            Array.Copy(dCurPos, dPosition, dCurPos.Length);

            iResult = Move(iCoordID, bMoveUse, dPosition, bUsePriority, tempSpeed);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int StartRMove(int iCoordID, bool[] bMoveUse, double[] dPosition, CMotorSpeedData[] tempSpeed = null)
        {
            int iResult = SUCCESS;

            double[] dCurPos;
            iResult = GetCurPos(iCoordID, out dCurPos);
            if (iResult != SUCCESS) return iResult;
            Array.Copy(dCurPos, dPosition, dCurPos.Length);

            iResult = StartMove(iCoordID, bMoveUse, dPosition, tempSpeed);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        /// <summary>
        /// 등속 이동 함수. 
        /// </summary>
        /// <param name="iCoordID"></param>
        /// <param name="dVelocity"></param>
        /// <param name="iAccelerate"></param>
        /// <param name="bDir"></param>
        /// <returns></returns>
        public int VMove(int iCoordID, double dVelocity, int iAccelerate, bool bDir)
        {
            // 나중에 구현 

            return SUCCESS;
        }

        /// <summary>
        /// 축 이동 완료 체크
        /// </summary>
        /// <param name="iCoordID"></param>
        /// <param name="bMoveUse"></param>
        /// <returns></returns>
        public int Wait4Done(int iCoordID, bool[] bMoveUse)
        {
            int iResult = SUCCESS;

            if (iCoordID == DEF_ALL_COORDINATE)
            {
                iResult = m_RefComp.Motion.Wait4Done(m_Data.AxisList, bMoveUse);
            }
            else
            {
                int[] tAxes = new int[1] { m_Data.AxisList[iCoordID] };
                iResult = m_RefComp.Motion.Wait4Done(tAxes, bMoveUse);
            }

            return iResult;
        }

        /// <summary>
        /// 축이 이동 완료되었는지 확인한다.
        /// </summary>
        /// <param name="iCoordID"></param>
        /// <param name="bDone"></param>
        /// <returns></returns>
        public int CheckDone(int iCoordID, out bool[] bDone)
        {
            int iResult = SUCCESS;

            if (iCoordID == DEF_ALL_COORDINATE)
            {
                iResult = m_RefComp.Motion.CheckMoveComplete(m_Data.AxisList, out bDone);
            }
            else
            {
                int[] tAxes = new int[1] { m_Data.AxisList[iCoordID] };
                iResult = m_RefComp.Motion.CheckMoveComplete(tAxes, out bDone);
            }

            return iResult;

        }

        public int JogMovePitch(int iCoordID, bool bDir, double dPitch)
        {
            int iResult = SUCCESS;

            if (iCoordID == DEF_ALL_COORDINATE)
            {
                return GenerateErrorCode(ERR_MAXES_YMC_INVALID_AXIS_ID);
            }
            else
            {
                iResult = m_RefComp.Motion.JogMoveStart(m_Data.AxisList[iCoordID], bDir, false);
            }

            return iResult;
        }

        public int JogMoveVelocity(int iCoordID, bool bDir, double dVelocity)
        {
            int iResult = SUCCESS;

            if (iCoordID == DEF_ALL_COORDINATE)
            {
                return GenerateErrorCode(ERR_MAXES_YMC_INVALID_AXIS_ID);
            }
            else
            {
                iResult = m_RefComp.Motion.JogMoveStart(m_Data.AxisList[iCoordID], bDir, true);
            }

            return iResult;
        }

        public int EStop(int iCoordID)
        {
            int iResult = SUCCESS;

            if (iCoordID == DEF_ALL_COORDINATE)
            {
                iResult = m_RefComp.Motion.ServoMotionStop(m_Data.DeviceNo);
            }
            else
            {
                iResult = m_RefComp.Motion.ServoMotionStop(m_Data.AxisList[iCoordID]);
            }

            return SUCCESS;
        }

        public int ServoOn(int iCoordID)
        {
            int iResult = SUCCESS;

            if (iCoordID == DEF_ALL_COORDINATE)
            {
                iResult = m_RefComp.Motion.ServoOn(m_Data.DeviceNo);
            }
            else
            {
                iResult = m_RefComp.Motion.ServoOn(m_Data.AxisList[iCoordID]);
            }

            return SUCCESS;
        }

        public int ServoOff(int iCoordID)
        {
            int iResult = SUCCESS;

            if (iCoordID == DEF_ALL_COORDINATE)
            {
                iResult = m_RefComp.Motion.ServoOff(m_Data.DeviceNo);
            }
            else
            {
                iResult = m_RefComp.Motion.ServoOff(m_Data.AxisList[iCoordID]);
            }

            return SUCCESS;
        }

        public void UpdateAxisStatus()
        {
            for (int i = 0; i < DEF_MAX_COORDINATE; i++)
            {
                if (m_Data.AxisList[i] == DEF_AXIS_NON_ID) continue;
                ServoStatus[i] = ObjectExtensions.Copy(m_RefComp.Motion.ServoStatus[m_Data.AxisList[i]]);
            }
        }

        private int GetCoordLength(int iCoordID)
        {
            int length = (iCoordID == DEF_ALL_COORDINATE) ? m_Data.AxisList.Length : 1;

            return length;
        }

        public int CheckHomeSensor(int iCoordID, out bool[] bStatus)
        {
            UpdateAxisStatus();
            bStatus = new bool[GetCoordLength(iCoordID)];
            for (int i = 0; i < GetCoordLength(iCoordID); i++)
            {
                bStatus[i] = ServoStatus[i].DetectHomeSensor;
            }
            
            return SUCCESS;
        }

        public int CheckPositiveSensor(int iCoordID, out bool[] bStatus)
        {
            UpdateAxisStatus();
            bStatus = new bool[GetCoordLength(iCoordID)];
            for (int i = 0; i < GetCoordLength(iCoordID); i++)
            {
                bStatus[i] = ServoStatus[i].DetectPlusSensor;
            }

            return SUCCESS;
        }

        public int CheckNegativeSensor(int iCoordID, out bool[] bStatus)
        {
            UpdateAxisStatus();
            bStatus = new bool[GetCoordLength(iCoordID)];
            for (int i = 0; i < GetCoordLength(iCoordID); i++)
            {
                bStatus[i] = ServoStatus[i].DetectMinusSensor;
            }

            return SUCCESS;
        }

        public int GetAmpEnable(int iCoordID, out bool[] bStatus)
        {
            UpdateAxisStatus();
            bStatus = new bool[GetCoordLength(iCoordID)];
            for (int i = 0; i < GetCoordLength(iCoordID); i++)
            {
                if (ServoStatus[i].Alarm == false && ServoStatus[i].Ready == true)
                    bStatus[i] = true;
                else bStatus[i] = false;
            }

            return SUCCESS;
        }

        public int GetAmpFault(int iCoordID, out bool[] bStatus)
        {
            GetAmpEnable(iCoordID, out bStatus);
            for (int i = 0; i < GetCoordLength(iCoordID); i++)
            {
                bStatus[i] = !bStatus[i];
            }

            return SUCCESS;
        }

        /// <summary>
        /// 축의 상태를 초기화 한다. (한개의 축 혹은 구성된 모든 축에 대해 초기화)
        /// </summary>
        /// <param name="iCoordID"></param>
        /// <returns></returns>
        public int ClearAxis(int iCoordID)
        {
            int iResult = SUCCESS;

            if (iCoordID == DEF_ALL_COORDINATE)
            {
                iResult = m_RefComp.Motion.ResetAlarm(m_Data.DeviceNo);
            }
            else
            {
                iResult = m_RefComp.Motion.ResetAlarm(m_Data.AxisList[iCoordID]);
            }

            return SUCCESS;
        }

        public int GetServoAlarm(int iCoordID, out bool[] alarm, out int[] alarmCode)
        {
            UpdateAxisStatus();
            alarm = new bool[GetCoordLength(iCoordID)];
            alarmCode = new int[GetCoordLength(iCoordID)];
            for (int i = 0; i < GetCoordLength(iCoordID); i++)
            {
                alarm[i] = ServoStatus[i].Alarm;
                alarmCode[i] = ServoStatus[i].AlarmCode;
            }

            return SUCCESS;
        }
    }
}
