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
            public int[] AxesNo = new int[DEF_MAX_AXIS_PER_SAXIS];    // Yaskawa 축 번호, 미사용축은 EYMC_Axis.NULL 로 셋팅

            public CMultiAxesYMCData(int Length, int DeviceNo, int[] AxesNo)
            {
                this.DeviceNo = DeviceNo;
                Array.Copy(AxesNo, this.AxesNo, AxesNo.Length);
            }
        }

    }

    public class MMultiAxes_YMC : MObject
    {
        private CMutliAxesYMCRefComp m_RefComp;
        private CMultiAxesYMCData m_Data;

        private int[] MovePriority = new int[DEF_MAX_AXIS_PER_SAXIS];   // 축 이동시에 우선 순위
        private int[] OriginPriority = new int[DEF_MAX_AXIS_PER_SAXIS];   // 축 원점복귀시에 우선 순위
        private CServoStatus[] ServoStatus = new CServoStatus[DEF_MAX_AXIS_PER_SAXIS];

        public MMultiAxes_YMC(CObjectInfo objInfo, CMutliAxesYMCRefComp refComp, CMultiAxesYMCData data)
            : base(objInfo)
        {
            m_RefComp = refComp;
            SetData(data);
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

            int deviceNo = (iCoordID == DEF_ALL_COORDINATE) ? m_Data.DeviceNo : m_Data.AxesNo[iCoordID];
            iResult = m_RefComp.Motion.ServoMotionStop(deviceNo);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int SetAxesMovePriority(int iCoordID, params int[] iPriorities)
        {
            int iResult = SUCCESS;

            for (int i = 0; i < DEF_MAX_AXIS_PER_SAXIS; i++)
            {
                MovePriority[i] = DEF_PRIORITY_NONE;
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

            for (int i = 0; i < DEF_MAX_AXIS_PER_SAXIS; i++)
            {
                OriginPriority[i] = DEF_PRIORITY_NONE;
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

            if (iCoordID == DEF_ALL_COORDINATE)
            {
                dCurPos = new double[DEF_MAX_AXIS_PER_SAXIS];
                for (int i = 0; i < DEF_MAX_AXIS_PER_SAXIS; i++)
                {
                    if (m_Data.AxesNo[i] == (int)EYMC_Axis.NULL) continue;
                    dCurPos[i] = m_RefComp.Motion.ServoStatus[m_Data.AxesNo[i]].EncoderValue;
                }
            }
            else
            {
                dCurPos = new double[1];
                dCurPos[0] = m_RefComp.Motion.ServoStatus[m_Data.AxesNo[iCoordID]].EncoderValue;
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
            bool[] bPartUse = new bool[DEF_MAX_AXIS_PER_SAXIS];
            bool bPartMove;

            if (iCoordID == DEF_ALL_COORDINATE)
            {
                if (bUsePriority == true)
                {
                    for (i = 0; i < (int)EPriority.MAX; i++)
                    {
                        bPartMove = false;
                        for (j = 0; j < DEF_MAX_AXIS_PER_SAXIS; j++)
                            bPartUse[j] = false;

                        // 우선순위안의 축 모두 이동
                        for (j = 0; j < DEF_MAX_AXIS_PER_SAXIS; j++)
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
                            iResult = m_RefComp.Motion.MoveToPos(m_Data.AxesNo, bPartUse, dPosition, tempSpeed);
                            if (iResult != SUCCESS) return iResult;
                        }
                    }
                }
                else
                {
                    iResult = m_RefComp.Motion.MoveToPos(m_Data.AxesNo, bMoveUse, dPosition, tempSpeed);
                    if (iResult != SUCCESS) return iResult;
                }
            }
            else
            {
                iResult = m_RefComp.Motion.MoveToPos(m_Data.AxesNo[iCoordID], dPosition, tempSpeed);
                if (iResult != SUCCESS) return iResult;
            }

            return SUCCESS;
        }

        public int StartMove(int iCoordID, bool[] bMoveUse, double[] dPosition, CMotorSpeedData[] tempSpeed = null)
        {
            int iResult = SUCCESS;
            if (iCoordID == DEF_ALL_COORDINATE)
            {
                iResult = m_RefComp.Motion.StartMoveToPos(m_Data.AxesNo, bMoveUse, dPosition, tempSpeed);
                if (iResult != SUCCESS) return iResult;
            }
            else
            {
                iResult = m_RefComp.Motion.StartMoveToPos(m_Data.AxesNo[iCoordID], dPosition, tempSpeed);
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
                iResult = m_RefComp.Motion.Wait4Done(m_Data.AxesNo, bMoveUse);
            }
            else
            {
                int[] tAxes = new int[1] { m_Data.AxesNo[iCoordID] };
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
                iResult = m_RefComp.Motion.CheckMoveComplete(m_Data.AxesNo, out bDone);
            }
            else
            {
                int[] tAxes = new int[1] { m_Data.AxesNo[iCoordID] };
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
                iResult = m_RefComp.Motion.JogMoveStart(m_Data.AxesNo[iCoordID], bDir, false);
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
                iResult = m_RefComp.Motion.JogMoveStart(m_Data.AxesNo[iCoordID], bDir, true);
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
                iResult = m_RefComp.Motion.ServoMotionStop(m_Data.AxesNo[iCoordID]);
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
                iResult = m_RefComp.Motion.ServoOn(m_Data.AxesNo[iCoordID]);
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
                iResult = m_RefComp.Motion.ServoOff(m_Data.AxesNo[iCoordID]);
            }

            return SUCCESS;
        }

        public void UpdateAxisStatus()
        {
            for (int i = 0; i < m_Data.AxesNo.Length; i++)
            {
                ServoStatus[i] = ObjectExtensions.Copy(m_RefComp.Motion.ServoStatus[m_Data.AxesNo[i]]);
            }
        }

        private int GetCoordLength(int iCoordID)
        {
            int length = (iCoordID == DEF_ALL_COORDINATE) ? m_Data.AxesNo.Length : 1;

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
                iResult = m_RefComp.Motion.ResetAlarm(m_Data.AxesNo[iCoordID]);
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


        /**
         * 원점복귀된 축에 대해서 Event가 발생한 경우에 대해 점검 및 조치를 한다.
         * 
         * @param   iAxisID			: 축 ID (0 ~ 63)
         * @return	Error Code		: 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = HOME SWITCH AXIS SOURCE (MULTIAXES)
         *							  xx = POSITIVE LIMIT AXIS SOURCE (MULTIAXES)
         *							  xx = NEGATIVE LIMIT AXIS SOURCE (MULTIAXES)
         *							  xx = AMP FAULT AXIS SOURCE (MULTIAXES)
         *							  xx = ACCELERATE LIMIT AXIS SOURCE (MULTIAXES)
         *							  xx = VELOCITY LIMIT AXIS SOURCE (MULTIAXES)
         *							  xx = X NEGATIVE LIMIT AXIS SOURCE (MULTIAXES)
         *							  xx = X POSITIVE LIMIT AXIS SOURCE (MULTIAXES)
         *							  xx = ERROR LIMIT AXIS SOURCE (MULTIAXES)
         *							  xx = PC COMMAND AXIS SOURCE (MULTIAXES)
         *							  xx = OUT OF FRAMES AXIS SOURCE (MULTIAXES)
         *							  xx = AMP POWER ON OFF AXIS SOURCE (MULTIAXES)
         *							  xx = RUN STOP COMMAND AXIS SOURCE (MULTIAXES)
         *							  xx = COLLISION STATE AXIS SOURCE (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        public int checkAxisState(int iAxisID)
        {
            int iResult = SUCCESS;
            short siState;
            int i = 0;
            int iCoordID = DEF_AXIS_NON_ID;

            if ((iResult = m_RefComp.Motion.GetAxisState(iAxisID, &siState)) != SUCCESS)


                for (i = 0; i < m_iMaxAxis; i++)
                {
                    if (m_pSaxAxis[i].iAxisID == -1) continue;

                    if (m_pSaxAxis[i].iAxisID == iAxisID)
                        iCoordID = i;
                }

            if (iCoordID == DEF_AXIS_NON_ID)
                return generateErrorCode(ERR_MAXES_INVALID_AXIS_ID);

            if ((bOriginFlag[iCoordID] == true) && (siState != SUCCESS))
            {
                if ((iResult = m_RefComp.Motion.GetAxisSource(iAxisID, &siState)) != SUCCESS)


                    if (siState & DEF_ST_HOME_SWITCH)
                        return generateErrorCode(ERR_MAXES_HOME_SWITCH_AXIS_SOURCE);            // ErrorDisplay(ERROR_AXIS+axis_id+600);

                    else if (siState & DEF_ST_POS_LIMIT)
                        return generateErrorCode(ERR_MAXES_POSITIVE_LIMIT_AXIS_SOURCE);         // ErrorDisplay(ERROR_AXIS+axis_id+700);

                    else if (siState & DEF_ST_NEG_LIMIT)
                        return generateErrorCode(ERR_MAXES_NEGATIVE_LIMIT_AXIS_SOURCE);         // ErrorDisplay(ERROR_AXIS+axis_id+800);

                    else if (siState & DEF_ST_AMP_FAULT)
                    {
                        return generateErrorCode(ERR_MAXES_AMP_FAULT_AXIS_SOURCE);          // ErrorDisplay(ERROR_AXIS+axis_id+900);
                    }
                    else if (siState & DEF_ST_A_LIMIT)
                        return generateErrorCode(ERR_MAXES_ACCELERATE_LIMIT_AXIS_SOURCE);           // ErrorDisplay(ERROR_AXIS+axis_id+1000);

                    else if (siState & DEF_ST_V_LIMIT)
                        return generateErrorCode(ERR_MAXES_VELOCITY_LIMIT_AXIS_SOURCE);         // ErrorDisplay(ERROR_AXIS+axis_id+1100);

                    else if (siState & DEF_ST_X_NEG_LIMIT)
                        return generateErrorCode(ERR_MAXES_X_NEGATIVE_LIMIT_AXIS_SOURCE);           // ErrorDisplay(ERROR_AXIS+axis_id+1200);

                    else if (siState & DEF_ST_X_POS_LIMIT)
                        return generateErrorCode(ERR_MAXES_X_POSITIVE_LIMIT_AXIS_SOURCE);           // ErrorDisplay(ERROR_AXIS+axis_id+1300);

                    else if (siState & DEF_ST_ERROR_LIMIT)
                        return generateErrorCode(ERR_MAXES_ERROR_LIMIT_AXIS_SOURCE);            // ErrorDisplay(ERROR_AXIS+axis_id+1400);

                    else if (siState & DEF_ST_PC_COMMAND)
                        return generateErrorCode(ERR_MAXES_PC_COMMAND_AXIS_SOURCE);         // ErrorDisplay(ERROR_AXIS+axis_id+1500);

                    else if (siState & DEF_ST_OUT_OF_FRAMES)
                        return generateErrorCode(ERR_MAXES_OUT_OF_FRAMES_AXIS_SOURCE);          // ErrorDisplay(ERROR_AXIS+axis_id+1600);

                    else if (siState & DEF_ST_AMP_POWER_ONOFF)
                        return generateErrorCode(ERR_MAXES_AMP_POWER_ON_OFF_AXIS_SOURCE);           // ErrorDisplay(ERROR_AXIS+axis_id+1700);

                    else if (siState & DEF_ST_RUN_STOP_COMMAND)
                        return generateErrorCode(ERR_MAXES_RUN_STOP_COMMAND_AXIS_SOURCE);           // ErrorDisplay(ERROR_AXIS+axis_id+1800);

                    else if (siState & DEF_ST_COLLISION_STATE)
                        return generateErrorCode(ERR_MAXES_COLLISION_STATE_AXIS_SOURCE);            // ErrorDisplay(ERROR_AXIS+axis_id+1900);

                    else if (siState & DEF_ST_NONE)
                    {
                        if ((iResult = m_RefComp.Motion.ClearStatus(iAxisID)) != SUCCESS)


                            Sleep(400);
                        return SUCCESS;
                    }
                    else if (siState & DEF_ST_INPOSITION_STATUS)
                    {
                        if ((iResult = m_RefComp.Motion.ClearStatus(iAxisID)) != SUCCESS)


                            Sleep(400);
                        return SUCCESS;
                    }
                    else if (siState & DEF_ST_ABS_COMM_ERROR)
                    {
                        if ((iResult = m_RefComp.Motion.ClearStatus(iAxisID)) != SUCCESS)


                            Sleep(400);
                        return SUCCESS;
                    }
                    else
                        return generateErrorCode(ERR_MAXES_UNKNOWN_AXIS_SOURCE);                    // ErrorDisplay(ERROR_AXIS+axis_id+2000);
            }

            return SUCCESS;
        }

        /**
         * 이동할 위치가 SW Limit을 벗어나는지 확인한다.
         * 
         * @param   iCoordID   : 구성 축 배열 Index, -1 = 허용 안함
         * @param   dPosition		: 검사할 위치
         * @return	Error Code		: 0 = SUCCESS
         *							  xx = OVER SW POSITIVE LIMIT (MULTIAXES)
         *							  xx = OVER SW NEGATIVE LIMIT (MULTIAXES)
         */
        public int checkSWLimit(int iCoordID, double dPosition)
{
    if (dPosition > m_pSaxAxis[iCoordID].dPositiveLimitPosition)
        return generateErrorCode(ERR_MAXES_OVER_SW_POSITIVE_LIMIT);

    if (dPosition < m_pSaxAxis[iCoordID].dNegativeLimitPosition)
        return generateErrorCode(ERR_MAXES_OVER_SW_NAGATIVE_LIMIT);

    return SUCCESS;
}

/**
 * Error Code 생성하기
 *  +-----------+-------------------------+
 *  | Object ID | Error Code + Error Base |
 *  | (2 bytes) |        (2 bytes)        |
 *  +-----------+-------------------------+
 *
 * @param	iErrCode : 발생한 Error Code
 * @return	Error Code : Object ID (2bytes)와 Error Code + Error Base (2bytes)를 4bytes로 조합한 코드
 */
public int generateErrorCode(int iErrCode)
{
    int iResult = SUCCESS;

    // Error Code가 SUCCESS가 아니면 코드 생성
    if (iErrCode != SUCCESS)
        iResult = (m_iObjectID << 16) + (iErrCode + m_iErrorBase);
    // Error Code가 SUCCESS이면 SUCCESS return
    else
        iResult = SUCCESS;


}

/**
* 지정 축이 지정된 위치를 지날 때 지정 IO를 출력한다.
*
* @param   iCoordID    : 축 지정, -1 = 허용안함
* @param	siPosNum		: 위치 번호, 1 ~ 10
* @param	siIONum			: I/O 번호, 양의정수=ON, 음의정수=OFF
* @param	dPosition		: 지정 축의 위치값
* @param	bEncFlag		: Encoder Flag, false=내부위치 Counter 사용, true=외부 Encoder 사용
* @return	Error Code		: 0 = SUCCESS
*							  xx = INVALID POSITION IO NUMBER (MOTIONLIB)
*							  xx = INVALID AXIS ID (MOTIONLIB)
*							  그 외 = 타 함수 Return Error
*/
public int PositionIoOnoff(int iCoordID, short siPosNum, short siIONum, double dPosition, int nEncFlag)
{
    int iResult = SUCCESS;

    /** 축 상태 점검 */
    if ((iResult = checkAxisState(m_pSaxAxis[iCoordID].iAxisID)) != SUCCESS)


        double dSign = (m_pSaxAxis[iCoordID].bSign) ? 1.0 : -1.0;

    dPosition = dSign * m_pSaxAxis[iCoordID].dScale * dPosition;
    /*
        double dCurPos;
        GetCurPos (iCoordID, &dCurPos, false);
        dCurPos = dSign * m_pSaxAxis[iCoordID].dScale * dCurPos;
        if(dPosition >= 0)
        {
            if(dPosition < dCurPos)
                nEncFlag = 5;
            else nEncFlag = 3;
        } else
        {
            if(dPosition < dCurPos)
                nEncFlag = 3;
            else nEncFlag = 5;
        }*/
    /*
    1 encoder counter (Current Pos < minus sign Setting Pos)  || ( pluse sign Setting Pos < Current Pos)
    3 encoder counter (Current Pos > minus sign Setting Pos)  || ( pluse sign Setting Pos < Current Pos)
    5 encoder counter (Current Pos < minus sign Setting Pos)  || ( pluse sign Setting Pos > Current Pos)
    */

    iResult = m_RefComp.Motion.PositionIoOnoff(siPosNum, siIONum, m_pSaxAxis[iCoordID].iAxisID, dPosition, nEncFlag);
    if (iResult != SUCCESS) return iResult;


    return SUCCESS;
}

/**
* PositionIoOnOff()로 설정된 것을 해제한다.
*
* @param   iCoordID    : 축 지정, -1 = 허용안함
* @param	siPosNum		: (OPTION=0) 위치 번호, 1 ~ 10, 0=모든 위치 해제
* @return	Error Code		: 0 = SUCCESS
*							  xx = INVALID POSITION IO NUMBER (MOTIONLIB)
*							  그 외 = 타 함수 Return Error
*/
public int PositionIOClear(int iCoordID, short siPosNum)
{
    int iResult = SUCCESS;

    iResult = m_RefComp.Motion.PositionIOClear(m_pSaxAxis[iCoordID].iAxisID, siPosNum);
    if (iResult != SUCCESS) return iResult;


    return SUCCESS;
}

public int PositionCompare(int iCoordID, short siIndexNum, short siBitNo, double dPosition, bool bOutOn)
{
    int iResult = SUCCESS;

    /** 축 상태 점검 */
    if ((iResult = checkAxisState(m_pSaxAxis[iCoordID].iAxisID)) != SUCCESS)


        double dSign = (m_pSaxAxis[iCoordID].bSign) ? 1.0 : -1.0;

    dPosition = dSign * m_pSaxAxis[iCoordID].dScale * dPosition;

    int nLatchMode;
    if (bOutOn == true) nLatchMode = 1;
    else nLatchMode = 2;

    iResult = m_RefComp.Motion.PositionCompare(1, siIndexNum, siBitNo, m_pSaxAxis[iCoordID].iAxisID,
        m_pSaxAxis[iCoordID].iAxisID, nLatchMode, 1, 0, dPosition, 1);
    if (iResult != SUCCESS) return iResult;

    return SUCCESS;
}
    }
}
