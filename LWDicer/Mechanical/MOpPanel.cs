using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

using static LWDicer.Control.DEF_System;
using static LWDicer.Control.DEF_Common;
using static LWDicer.Control.DEF_Error;
using static LWDicer.Control.DEF_OpPanel;
using static LWDicer.Control.DEF_Thread;
using static LWDicer.Control.DEF_Motion;

namespace LWDicer.Control
{
    class DEF_OpPanel
    {
        public const int DEF_OPPANEL_NONE_PANEL_ID = 0;

        public const int DEF_OPPANEL_FRONT_PANEL_ID = 1;
        public const int DEF_OPPANEL_BACK_PANEL_ID = 2;

        // Jog 관련 define
        public const int DEF_OPPANEL_NO_JOGKEY = -1;
        public const int DEF_OPPANEL_JOG_X_KEY = 0;
        public const int DEF_OPPANEL_JOG_Y_KEY = 1;
        public const int DEF_OPPANEL_JOG_T_KEY = 2;
        public const int DEF_OPPANEL_JOG_Z_KEY = 3;

        // Tower Lamp 관련 define
        public const int DEF_OPPANEL_BUZZER_ALL = -1;
        public const int DEF_OPPANEL_BUZZER_K1 = 0;
        public const int DEF_OPPANEL_BUZZER_K2 = 1;
        public const int DEF_OPPANEL_BUZZER_K3 = 2;
        public const int DEF_OPPANEL_BUZZER_K4 = 3;

        // Max. Value
        public const int DEF_OPPANEL_MAX_JOG_LIST = 16;
        public const int DEF_OPPANEL_MAX_BUZZER_MODE = 4;
        public const int DEF_OPPANEL_MAX_DOOR_SENSOR = 7;
        public const int DEF_OPPANEL_MAX_ESTOP_RELAY_NO = 2;
        public const int DEF_OPPANEL_MAX_CP_TRIP_NO = 16;
        public const int DEF_OPPANEL_MAX_DOOR_GROUP = 1;
    }

    public class CPanelIOAddr
    {
        // Push Switch IO Address
        public int RunInputAddr;
        public int StopInputAddr;
        public int ResetInputAddr;
        // Teaching Pendant SStop IO Address
        public int TPStopInputAddr;

        // Switch LED IO Address
        public int RunOutputAddr;
        public int StopOutputAddr;
        public int ResetOutputAddr;

        // Jog +,- Switch IO Address
        public int XpInputAddr;
        public int XnInputAddr;
        public int YpInputAddr;
        public int YnInputAddr;
        public int TpInputAddr;
        public int TnInputAddr;
        public int ZpInputAddr;
        public int ZnInputAddr;
    }

    /// <summary>
    /// This structure is defined configuration of tower lamp.
    /// </summary>
    public class CTowerIOAddr
    {
        // Tower Lamp IO Address
        public int RedLampAddr;
        public int YellowLampAddr;
        public int GreenLampAddr;
        // Buzzer Operate IO Address
        public int[] BuzzerArray/* = new int[DEF_OPPANEL_MAX_BUZZER_MODE]*/;

        public CTowerIOAddr()
        {
            BuzzerArray = new int[DEF_OPPANEL_MAX_BUZZER_MODE];
        }
        
    }

    /// <summary>
    /// This structure is defined configuration of all op-panel.
    /// </summary>
    public class COpPanelIOAddr
    {
        // Panel IO Address Table 
        public CPanelIOAddr FrontPanel;
        public CPanelIOAddr BackPanel;

        // Tower Lamp IO Address Table 
        public CTowerIOAddr TowerLamp;

        // Touch Panel 선택 IO Address 
        public int TouchSelectAddr;

        // 판넬 마크용 외부모니터 선택 IO Address 
        public int MarkMoniterAddr1;
        public int MarkMoniterAddr2;

        // E-STOP Switch Status IO Address 
        public int[] EStopInputArray/* = new int[DEF_OPPANEL_MAX_ESTOP_RELAY_NO]*/;

        // 안전센서 (Door) IO Address 
        public int[,] SafeDoorArray/* = new int[DEF_OPPANEL_MAX_DOOR_GROUP, DEF_OPPANEL_MAX_DOOR_SENSOR]*/;

        // 안전센서 (Door) IO Option Flag : true일 때 IO 감지 안함 
        public bool[,] bSafeDoorFlagArray/* = new bool[DEF_OPPANEL_MAX_DOOR_GROUP, DEF_OPPANEL_MAX_DOOR_SENSOR]*/;

        // Main Air Check IO Address 
        public int MainAirAddr;

        // Sub Air Check IO Address 
        public int SubAirAddr;

        // Main Vacuum Check IO Address 
        public int MainVacuumAddr;

        // Sub Vacuum Check IO Address 
        public int SubVacuumAddr;

        // Main Vacuum Check IO Address 
        public int MainN2Addr;

        // CP Trip IO Address 
        public int[] CPTripArray/* = new int[DEF_OPPANEL_MAX_CP_TRIP_NO]*/;

        // Cleaner Detect IO Address 
        public int CleanerDetect1Addr;
        public int CleanerDetect2Addr;

        // EFD Ready Check IO Address 
        public int EFDReadyS1Addr;
        public int EFDReadyS2Addr;
        public int EFDReadyG1Addr;
        public int EFDReadyG2Addr;

        // DC Power Check IO Address 
        public int DcPowerAddr;

        public COpPanelIOAddr()
        {
            TowerLamp          = new CTowerIOAddr();

            EStopInputArray    = new int[DEF_OPPANEL_MAX_ESTOP_RELAY_NO];
            SafeDoorArray      = new int[DEF_OPPANEL_MAX_DOOR_GROUP, DEF_OPPANEL_MAX_DOOR_SENSOR];
            bSafeDoorFlagArray = new bool[DEF_OPPANEL_MAX_DOOR_GROUP, DEF_OPPANEL_MAX_DOOR_SENSOR];
            CPTripArray        = new int[DEF_OPPANEL_MAX_CP_TRIP_NO];
        }

    }

    /// <summary>
    /// This structure is defined one configuration of jog moving table.
    /// </summary>
    public class CJogMotion
    {
        // Jog로 움직일 Motion 대상
        public IMultiAxes m_plnkJog;

        // Jog Key로 움직일 Motion 축
        public int AxisIndex;
    }

    /// <summary>
    /// This structure is defined one configuration of jog moving table.
    /// </summary>
    public class CJogMotionTable
    {
        // Jog Key(X +/-)로 움직일 Motion 축
        public CJogMotion m_XKey;
        public CJogMotion m_YKey;
        public CJogMotion m_TKey;
        public CJogMotion m_ZKey;
    }

    /// <summary>
    /// This structure is defined all configuration of jog moving table.
    /// </summary>
    public class CJogTable
    {
        // Jog로 움직일 대상의 개수
        public int ListNo;

        // Jog로 움직일 Motion에 대한 정보 List
        public CJogMotionTable[] MotionArray/* = new CJogMotionTable[DEF_OPPANEL_MAX_JOG_LIST]*/;

        public CJogTable()
        {
            MotionArray = new CJogMotionTable[DEF_OPPANEL_MAX_JOG_LIST];

        }
    }


    public class COpPanelData
    {
        public bool bUseOnline;
        public bool bInSfaTest;
    }

    public class MOpPanel : MObject
    {
        private COpPanelData m_Data;

        COpPanelIOAddr m_IOAddrTable;
        EOperationMode m_eAutoManual;
        ERunMode m_eOpMode;

        CJogTable m_JogTable;

        //IIO* m_plnkIO;
        //IMotionLib* m_pMotionLib;


        // Unit 초기화 Flag
        bool[] m_bInitFlag = new bool[INIT_UNIT_MAX];

        // Switch Status (previous)
        bool m_bRunSWOld;
        bool m_bStopSWOld;
        bool m_bEStopSWOld;
        bool m_bResetSWOld;
        bool m_bDoorSWOld;
        bool m_bCPTripOld;
        bool m_bAirErrOld;

        // Jog Status (previous)
        bool m_bJogXpOld;
        bool m_bJogXnOld;
        bool m_bJogXOld; // Jog Key로 이동한 마지막 방향

        bool m_bJogYpOld;
        bool m_bJogYnOld;
        bool m_bJogYOld;

        bool m_bJogTpOld;
        bool m_bJogTnOld;
        bool m_bJogTOld;

        bool m_bJogZpOld;
        bool m_bJogZnOld;
        bool m_bJogZOld;

        public MOpPanel(CObjectInfo objInfo, COpPanelData data,
            COpPanelIOAddr sPanelIOAddr, CJogTable sJogTable)
            : base(objInfo)
        {
            SetData(data);
            m_IOAddrTable = sPanelIOAddr;
            m_JogTable = sJogTable;

            //m_plnkIO = pIO;

            m_JogTable.ListNo = 10;

            /** 사용하지 않는 정보에 대한 부분 초기화 */
            for (int i = m_JogTable.ListNo; i < DEF_OPPANEL_MAX_JOG_LIST; i++)
            {
                m_JogTable.MotionArray[i].m_XKey.m_plnkJog = null;
                m_JogTable.MotionArray[i].m_XKey.AxisIndex = DEF_OPPANEL_NO_JOGKEY;
                m_JogTable.MotionArray[i].m_YKey.m_plnkJog = null;
                m_JogTable.MotionArray[i].m_YKey.AxisIndex = DEF_OPPANEL_NO_JOGKEY;
                m_JogTable.MotionArray[i].m_TKey.m_plnkJog = null;
                m_JogTable.MotionArray[i].m_TKey.AxisIndex = DEF_OPPANEL_NO_JOGKEY;
                m_JogTable.MotionArray[i].m_ZKey.m_plnkJog = null;
                m_JogTable.MotionArray[i].m_ZKey.AxisIndex = DEF_OPPANEL_NO_JOGKEY;
            }

            // Unit의 초기화 Flag를 Reset해야 한다.
            for (int i = 0; i < INIT_UNIT_MAX; i++)
            {
                m_bInitFlag[i] = false;
            }

            /** Run SW 상태 (previous) */
            m_bRunSWOld = false;

            /** Stop SW 상태 (previous) */
            m_bStopSWOld = false;

            /** EStop SW 상태 (previous) */
            m_bEStopSWOld = false;

            /** Reset SW 상태 (previous) */
            m_bResetSWOld = false;

            /** Door Sensor 상태 (previous) */
            m_bDoorSWOld = false;

            /** CP Trip 상태 (previous) */
            m_bCPTripOld = false;

            /** Air Error 상태 (previous) */
            m_bAirErrOld = false;

            /** Jog X 로 이동한 마지막 방향 */
            m_bJogXOld = false;

            /** Jog Y 로 이동한 마지막 방향 */
            m_bJogYOld = false;

            /** Jog T 로 이동한 마지막 방향 */
            m_bJogTOld = false;

            /** Jog Z 로 이동한 마지막 방향 */
            m_bJogZOld = false;
        }

        public int SetData(COpPanelData source)
        {
            m_Data = ObjectExtensions.Copy(source);
            return SUCCESS;
        }

        public int GetData(out COpPanelData target)
        {
            target = ObjectExtensions.Copy(m_Data);

            return SUCCESS;
        }

        public void SetIOAddress(COpPanelIOAddr opIOAddress)
        {
            /** IO Address Table 설정 */
            m_IOAddrTable = opIOAddress;

            // 정상 동작 Log
            //	m_plogMng.WriteLog("IO Address Table을 설정하였습니다.", __FILE__, __LINE__);
        }

        public void GetIOAddress(ref COpPanelIOAddr pOpPanelIOAddr)
        {
            /** IO Address Table 전달 */
            pOpPanelIOAddr = m_IOAddrTable;

            // 정상 동작 Log
            //	m_plogMng.WriteLog("IO Address Table을 읽었습니다.", __FILE__, __LINE__);
        }

        public void SetJogTable(CJogTable sJogTable)
        {
            int i = 0;

            /** Jog 정보 설정 */
            m_JogTable = sJogTable;

            /** 사용하지 않는 정보에 대한 부분 초기화 */
            for (i = m_JogTable.ListNo; i < DEF_OPPANEL_MAX_JOG_LIST; i++)
            {
                m_JogTable.MotionArray[i].m_XKey.m_plnkJog = null;
                m_JogTable.MotionArray[i].m_XKey.AxisIndex = DEF_OPPANEL_NO_JOGKEY;
                m_JogTable.MotionArray[i].m_YKey.m_plnkJog = null;
                m_JogTable.MotionArray[i].m_YKey.AxisIndex = DEF_OPPANEL_NO_JOGKEY;
                m_JogTable.MotionArray[i].m_TKey.m_plnkJog = null;
                m_JogTable.MotionArray[i].m_TKey.AxisIndex = DEF_OPPANEL_NO_JOGKEY;
                m_JogTable.MotionArray[i].m_ZKey.m_plnkJog = null;
                m_JogTable.MotionArray[i].m_ZKey.AxisIndex = DEF_OPPANEL_NO_JOGKEY;
            }

            // 정상 동작 Log
            //	m_plogMng.WriteLog("Jog 구성 정보를 설정하였습니다.", __FILE__, __LINE__);
        }

        public void GetJogTable(out CJogTable psJogTable)
        {
            /** Jog 정보 설정 */
            psJogTable = m_JogTable;

            /** 사용하지 않는 정보에 대한 부분 초기화 */
            for (int i = m_JogTable.ListNo; i < DEF_OPPANEL_MAX_JOG_LIST; i++)
            {
                psJogTable.MotionArray[i].m_XKey.m_plnkJog = null;
                psJogTable.MotionArray[i].m_XKey.AxisIndex = DEF_OPPANEL_NO_JOGKEY;
                psJogTable.MotionArray[i].m_YKey.m_plnkJog = null;
                psJogTable.MotionArray[i].m_YKey.AxisIndex = DEF_OPPANEL_NO_JOGKEY;
                psJogTable.MotionArray[i].m_TKey.m_plnkJog = null;
                psJogTable.MotionArray[i].m_TKey.AxisIndex = DEF_OPPANEL_NO_JOGKEY;
                psJogTable.MotionArray[i].m_ZKey.m_plnkJog = null;
                psJogTable.MotionArray[i].m_ZKey.AxisIndex = DEF_OPPANEL_NO_JOGKEY;
            }

            // 정상 동작 Log
            //	m_plogMng.WriteLog("Jog 구성 정보를 읽었습니다.", __FILE__, __LINE__);
        }

        /**
        * Pitch 단위의 Jog 이동 동작을 수행한다.
        *
        * @param	iUnitIndex : Jog로 움직일 Motion에 대한 정보 Table의 Index
        * @param	iKey : 이동할 Jog Key 종류 (0:X, 1:Y, 2:T, 3:Z)
        * @param	bDir : 이동할 방향 (true: +, false: -)
        * @return	Error Code : 0=SUCCESS, 그외=Error
*/
        public int MoveJogPitch(int iUnitIndex, int iUnitIndex2, int iKey, bool bDir)
        {
            int iResult = SUCCESS;
            //bool bDone;
            //int iState;
            //int iSource;
            //int iCoordID;
            //int iEvent;
            //double dDistance = 1.0;
            //double dVelocity;
            //int iAccelerate;
            //int iDecelerate;

            ////  Unit Index 범위 점검
            //if ((iUnitIndex < 0) || (iUnitIndex > m_JogTable.ListNo))
            //    return GenerateErrorCode(ERR_OPPANEL_INVALID_JOG_UNIT_INDEX);

            ////  눌려진 Jog Key에 따라 동작
            //switch (iKey)
            //{
            //    //  X-key (Left/Right)
            //    case DEF_OPPANEL_JOG_X_KEY:
            //        //  X-key에 설정이 되어 있으면
            //        iCoordID = m_JogTable.MotionArray[iUnitIndex].m_XKey.AxisIndex;
            //        if (iCoordID == DEF_OPPANEL_NO_JOGKEY && iUnitIndex2 >= 0)
            //            iUnitIndex = iUnitIndex2;
            //        iCoordID = m_JogTable.MotionArray[iUnitIndex].m_XKey.AxisIndex;

            //        if (iCoordID > DEF_OPPANEL_NO_JOGKEY)
            //        {
            //            //  Motion 정지 확인
            //            if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_XKey.m_plnkJog.CheckDone(iCoordID, out bDone)) != SUCCESS)
            //                return iResult;

            //            if (bDone == true)
            //            {
            //                //  Pitch 단위 이동
            //                if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_XKey.m_plnkJog.JogMovePitch(iCoordID, bDir)) != SUCCESS)
            //                {
            //                    if (bDir != m_bJogXOld)     // 진행할 방향이 E-Stop 걸리게 된 방향과 반대이면
            //                    {
            //                        //  Motion 상태 확인
            //                        if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_XKey.m_plnkJog.GetAxisState(iCoordID, out iState)) != SUCCESS)
            //                        {
            //                            if (iState == DEF_E_STOP_EVENT)
            //                            {
            //                                if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_XKey.m_plnkJog.GetAxisStatus(iCoordID, out iSource)) != SUCCESS)
            //                                {
            //                                    if (iSource & DEF_ST_HOME_SWITCH)
            //                                    {
            //                                        //  E-Stop Event 해제
            //                                        iEvent = DEF_NO_EVENT;
            //                                        m_JogTable.MotionArray[iUnitIndex].m_XKey.m_plnkJog.SetHomeSensorEvent(iCoordID, out iEvent);

            //                                        //  E-Stop 벗어나는 방향으로 1mm 이동
            //                                        dDistance = bDir ? dDistance : -dDistance;
            //                                        m_JogTable.MotionArray[iUnitIndex].m_XKey.m_plnkJog.GetFineVelocity(iCoordID, out dVelocity, out iAccelerate);
            //                                        iDecelerate = iAccelerate;
            //                                        if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_XKey.m_plnkJog.RMove(iCoordID, null, out dDistance, out dVelocity, out iAccelerate, out iDecelerate, DEF_SMOVE_DISTANCE, false)) != SUCCESS)
            //                                            return iResult;

            //                                        //  E-Stop Event 설정
            //                                        iEvent = DEF_E_STOP_EVENT;
            //                                        m_JogTable.MotionArray[iUnitIndex].m_XKey.m_plnkJog.SetHomeSensorEvent(iCoordID, out iEvent);

            //                                        //  Pitch 단위 이동
            //                                        if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_XKey.m_plnkJog.JogMovePitch(iCoordID, bDir)) != SUCCESS)
            //                                            return iResult;
            //                                    }
            //                                    else if (iSource & DEF_ST_POS_LIMIT)
            //                                    {
            //                                        //  E-Stop Event 해제
            //                                        iEvent = DEF_NO_EVENT;
            //                                        m_JogTable.MotionArray[iUnitIndex].m_XKey.m_plnkJog.SetPositiveSensorEvent(iCoordID, out iEvent);

            //                                        //  E-Stop 벗어나는 방향으로 1mm 이동
            //                                        dDistance = bDir ? dDistance : -dDistance;
            //                                        m_JogTable.MotionArray[iUnitIndex].m_XKey.m_plnkJog.GetFineVelocity(iCoordID, out dVelocity, out iAccelerate);
            //                                        iDecelerate = iAccelerate;
            //                                        if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_XKey.m_plnkJog.RMove(iCoordID, null, out dDistance, out dVelocity, out iAccelerate, out iDecelerate, DEF_SMOVE_DISTANCE, false)) != SUCCESS)
            //                                            return iResult;

            //                                        //  E-Stop Event 설정
            //                                        iEvent = DEF_E_STOP_EVENT;
            //                                        m_JogTable.MotionArray[iUnitIndex].m_XKey.m_plnkJog.SetPositiveSensorEvent(iCoordID, out iEvent);

            //                                        //  Pitch 단위 이동
            //                                        if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_XKey.m_plnkJog.JogMovePitch(iCoordID, bDir)) != SUCCESS)
            //                                            return iResult;
            //                                    }
            //                                    else if (iSource & DEF_ST_NEG_LIMIT)
            //                                    {
            //                                        //  E-Stop Event 해제
            //                                        iEvent = DEF_NO_EVENT;
            //                                        m_JogTable.MotionArray[iUnitIndex].m_XKey.m_plnkJog.SetNegativeSensorEvent(iCoordID, out iEvent);

            //                                        //  E-Stop 벗어나는 방향으로 1mm 이동
            //                                        dDistance = bDir ? dDistance : -dDistance;
            //                                        m_JogTable.MotionArray[iUnitIndex].m_XKey.m_plnkJog.GetFineVelocity(iCoordID, out dVelocity, out iAccelerate);
            //                                        iDecelerate = iAccelerate;
            //                                        if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_XKey.m_plnkJog.RMove(iCoordID, null, out dDistance, out dVelocity, out iAccelerate, out iDecelerate, DEF_SMOVE_DISTANCE, false)) != SUCCESS)
            //                                            return iResult;

            //                                        //  E-Stop Event 설정
            //                                        iEvent = DEF_E_STOP_EVENT;
            //                                        m_JogTable.MotionArray[iUnitIndex].m_XKey.m_plnkJog.SetNegativeSensorEvent(iCoordID, out iEvent);

            //                                        //  Pitch 단위 이동
            //                                        if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_XKey.m_plnkJog.JogMovePitch(iCoordID, bDir)) != SUCCESS)
            //                                            return iResult;
            //                                    }
            //                                    else
            //                                        return iResult;
            //                                }
            //                                else
            //                                    return iResult;
            //                            }
            //                            else
            //                                return iResult;
            //                        }
            //                        else
            //                            return iResult;
            //                    }
            //                    else
            //                        return iResult;
            //                }
            //            }

            //            m_bJogXOld = bDir;

            //        }
            //        break;

            //    //  Y-key (For/Back)
            //    case DEF_OPPANEL_JOG_Y_KEY:
            //        //  Y-key에 설정이 되어 있으면
            //        iCoordID = m_JogTable.MotionArray[iUnitIndex].m_YKey.AxisIndex;
            //        if (iCoordID == DEF_OPPANEL_NO_JOGKEY && iUnitIndex2 >= 0)
            //            iUnitIndex = iUnitIndex2;
            //        iCoordID = m_JogTable.MotionArray[iUnitIndex].m_YKey.AxisIndex;

            //        if (iCoordID > DEF_OPPANEL_NO_JOGKEY)
            //        {
            //            //  Motion 정지 확인
            //            if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_YKey.m_plnkJog.CheckDone(iCoordID, out bDone)) != SUCCESS)
            //                return iResult;

            //            //  Pitch 단위 이동
            //            if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_YKey.m_plnkJog.JogMovePitch(iCoordID, bDir)) != SUCCESS)
            //            {
            //                if (bDir != m_bJogYOld)     // 진행할 방향이 E-Stop 걸리게 된 방향과 반대이면
            //                {
            //                    //  Motion 상태 확인
            //                    if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_YKey.m_plnkJog.GetAxisState(iCoordID, out iState)) != SUCCESS)
            //                    {
            //                        if (iState == DEF_E_STOP_EVENT)
            //                        {
            //                            if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_YKey.m_plnkJog.GetAxisStatus(iCoordID, out iSource)) != SUCCESS)
            //                            {
            //                                if (iSource & DEF_ST_HOME_SWITCH)
            //                                {
            //                                    //  E-Stop Event 해제
            //                                    iEvent = DEF_NO_EVENT;
            //                                    m_JogTable.MotionArray[iUnitIndex].m_YKey.m_plnkJog.SetHomeSensorEvent(iCoordID, out iEvent);

            //                                    //  E-Stop 벗어나는 방향으로 1mm 이동
            //                                    dDistance = bDir ? dDistance : -dDistance;
            //                                    m_JogTable.MotionArray[iUnitIndex].m_YKey.m_plnkJog.GetFineVelocity(iCoordID, out dVelocity, out iAccelerate);
            //                                    iDecelerate = iAccelerate;
            //                                    if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_YKey.m_plnkJog.RMove(iCoordID, null, out dDistance, out dVelocity, out iAccelerate, out iDecelerate, DEF_SMOVE_DISTANCE, false)) != SUCCESS)
            //                                        return iResult;

            //                                    //  E-Stop Event 설정
            //                                    iEvent = DEF_E_STOP_EVENT;
            //                                    m_JogTable.MotionArray[iUnitIndex].m_YKey.m_plnkJog.SetHomeSensorEvent(iCoordID, out iEvent);

            //                                    //  Pitch 단위 이동
            //                                    if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_YKey.m_plnkJog.JogMovePitch(iCoordID, bDir)) != SUCCESS)
            //                                        return iResult;
            //                                }
            //                                else if (iSource & DEF_ST_POS_LIMIT)
            //                                {
            //                                    //  E-Stop Event 해제
            //                                    iEvent = DEF_NO_EVENT;
            //                                    m_JogTable.MotionArray[iUnitIndex].m_YKey.m_plnkJog.SetPositiveSensorEvent(iCoordID, out iEvent);

            //                                    //  E-Stop 벗어나는 방향으로 1mm 이동
            //                                    dDistance = bDir ? dDistance : -dDistance;
            //                                    m_JogTable.MotionArray[iUnitIndex].m_YKey.m_plnkJog.GetFineVelocity(iCoordID, out dVelocity, out iAccelerate);
            //                                    iDecelerate = iAccelerate;
            //                                    if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_YKey.m_plnkJog.RMove(iCoordID, null, out dDistance, out dVelocity, out iAccelerate, out iDecelerate, DEF_SMOVE_DISTANCE, false)) != SUCCESS)
            //                                        return iResult;

            //                                    //  E-Stop Event 설정
            //                                    iEvent = DEF_E_STOP_EVENT;
            //                                    m_JogTable.MotionArray[iUnitIndex].m_YKey.m_plnkJog.SetPositiveSensorEvent(iCoordID, out iEvent);

            //                                    //  Pitch 단위 이동
            //                                    if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_YKey.m_plnkJog.JogMovePitch(iCoordID, bDir)) != SUCCESS)
            //                                        return iResult;
            //                                }
            //                                else if (iSource & DEF_ST_NEG_LIMIT)
            //                                {
            //                                    //  E-Stop Event 해제
            //                                    iEvent = DEF_NO_EVENT;
            //                                    m_JogTable.MotionArray[iUnitIndex].m_YKey.m_plnkJog.SetNegativeSensorEvent(iCoordID, out iEvent);

            //                                    //  E-Stop 벗어나는 방향으로 1mm 이동
            //                                    dDistance = bDir ? dDistance : -dDistance;
            //                                    m_JogTable.MotionArray[iUnitIndex].m_YKey.m_plnkJog.GetFineVelocity(iCoordID, out dVelocity, out iAccelerate);
            //                                    iDecelerate = iAccelerate;
            //                                    if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_YKey.m_plnkJog.RMove(iCoordID, null, out dDistance, out dVelocity, out iAccelerate, out iDecelerate, DEF_SMOVE_DISTANCE, false)) != SUCCESS)
            //                                        return iResult;

            //                                    //  E-Stop Event 설정
            //                                    iEvent = DEF_E_STOP_EVENT;
            //                                    m_JogTable.MotionArray[iUnitIndex].m_YKey.m_plnkJog.SetNegativeSensorEvent(iCoordID, out iEvent);

            //                                    //  Pitch 단위 이동
            //                                    if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_XKey.m_plnkJog.JogMovePitch(iCoordID, bDir)) != SUCCESS)
            //                                        return iResult;
            //                                }
            //                                else
            //                                    return iResult;
            //                            }
            //                            else
            //                                return iResult;
            //                        }
            //                        else
            //                            return iResult;
            //                    }
            //                    else
            //                        return iResult;
            //                }
            //                else
            //                    return iResult;
            //            }

            //            m_bJogYOld = bDir;
            //        }
            //        break;

            //    //  T-key (CW/CCW)
            //    case DEF_OPPANEL_JOG_T_KEY:
            //        //  T-key에 설정이 되어 있으면
            //        iCoordID = m_JogTable.MotionArray[iUnitIndex].m_TKey.AxisIndex;
            //        if (iCoordID == DEF_OPPANEL_NO_JOGKEY && iUnitIndex2 >= 0)
            //            iUnitIndex = iUnitIndex2;
            //        iCoordID = m_JogTable.MotionArray[iUnitIndex].m_TKey.AxisIndex;

            //        if (iCoordID > DEF_OPPANEL_NO_JOGKEY)
            //        {
            //            //  Motion 정지 확인
            //            if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_TKey.m_plnkJog.CheckDone(iCoordID, out bDone)) != SUCCESS)
            //                return iResult;

            //            //  Pitch 단위 이동
            //            if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_TKey.m_plnkJog.JogMovePitch(iCoordID, bDir)) != SUCCESS)
            //            {
            //                if (bDir != m_bJogTOld)     // 진행할 방향이 E-Stop 걸리게 된 방향과 반대이면
            //                {
            //                    //  Motion 상태 확인
            //                    if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_TKey.m_plnkJog.GetAxisState(iCoordID, out iState)) != SUCCESS)
            //                    {
            //                        if (iState == DEF_E_STOP_EVENT)
            //                        {
            //                            if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_TKey.m_plnkJog.GetAxisStatus(iCoordID, out iSource)) != SUCCESS)
            //                            {
            //                                if (iSource & DEF_ST_HOME_SWITCH)
            //                                {
            //                                    //  E-Stop Event 해제
            //                                    iEvent = DEF_NO_EVENT;
            //                                    m_JogTable.MotionArray[iUnitIndex].m_TKey.m_plnkJog.SetHomeSensorEvent(iCoordID, out iEvent);

            //                                    //  E-Stop 벗어나는 방향으로 1mm 이동
            //                                    dDistance = bDir ? dDistance : -dDistance;
            //                                    m_JogTable.MotionArray[iUnitIndex].m_TKey.m_plnkJog.GetFineVelocity(iCoordID, out dVelocity, out iAccelerate);
            //                                    iDecelerate = iAccelerate;
            //                                    if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_TKey.m_plnkJog.RMove(iCoordID, null, out dDistance, out dVelocity, out iAccelerate, out iDecelerate, DEF_SMOVE_DISTANCE, false)) != SUCCESS)
            //                                        return iResult;

            //                                    //  E-Stop Event 설정
            //                                    iEvent = DEF_E_STOP_EVENT;
            //                                    m_JogTable.MotionArray[iUnitIndex].m_TKey.m_plnkJog.SetHomeSensorEvent(iCoordID, out iEvent);

            //                                    //  Pitch 단위 이동
            //                                    if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_TKey.m_plnkJog.JogMovePitch(iCoordID, bDir)) != SUCCESS)
            //                                        return iResult;
            //                                }
            //                                else if (iSource & DEF_ST_POS_LIMIT)
            //                                {
            //                                    //  E-Stop Event 해제
            //                                    iEvent = DEF_NO_EVENT;
            //                                    m_JogTable.MotionArray[iUnitIndex].m_TKey.m_plnkJog.SetPositiveSensorEvent(iCoordID, out iEvent);

            //                                    //  E-Stop 벗어나는 방향으로 1mm 이동
            //                                    dDistance = bDir ? dDistance : -dDistance;
            //                                    m_JogTable.MotionArray[iUnitIndex].m_TKey.m_plnkJog.GetFineVelocity(iCoordID, out dVelocity, out iAccelerate);
            //                                    iDecelerate = iAccelerate;
            //                                    if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_TKey.m_plnkJog.RMove(iCoordID, null, out dDistance, out dVelocity, out iAccelerate, out iDecelerate, DEF_SMOVE_DISTANCE, false)) != SUCCESS)
            //                                        return iResult;

            //                                    //  E-Stop Event 설정
            //                                    iEvent = DEF_E_STOP_EVENT;
            //                                    m_JogTable.MotionArray[iUnitIndex].m_TKey.m_plnkJog.SetPositiveSensorEvent(iCoordID, out iEvent);

            //                                    //  Pitch 단위 이동
            //                                    if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_TKey.m_plnkJog.JogMovePitch(iCoordID, bDir)) != SUCCESS)
            //                                        return iResult;
            //                                }
            //                                else if (iSource & DEF_ST_NEG_LIMIT)
            //                                {
            //                                    //  E-Stop Event 해제
            //                                    iEvent = DEF_NO_EVENT;
            //                                    m_JogTable.MotionArray[iUnitIndex].m_TKey.m_plnkJog.SetNegativeSensorEvent(iCoordID, out iEvent);

            //                                    //  E-Stop 벗어나는 방향으로 1mm 이동
            //                                    dDistance = bDir ? dDistance : -dDistance;
            //                                    m_JogTable.MotionArray[iUnitIndex].m_TKey.m_plnkJog.GetFineVelocity(iCoordID, out dVelocity, out iAccelerate);
            //                                    iDecelerate = iAccelerate;
            //                                    if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_TKey.m_plnkJog.RMove(iCoordID, null, out dDistance, out dVelocity, out iAccelerate, out iDecelerate, DEF_SMOVE_DISTANCE, false)) != SUCCESS)
            //                                        return iResult;

            //                                    //  E-Stop Event 설정
            //                                    iEvent = DEF_E_STOP_EVENT;
            //                                    m_JogTable.MotionArray[iUnitIndex].m_TKey.m_plnkJog.SetNegativeSensorEvent(iCoordID, out iEvent);

            //                                    //  Pitch 단위 이동
            //                                    if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_TKey.m_plnkJog.JogMovePitch(iCoordID, bDir)) != SUCCESS)
            //                                        return iResult;
            //                                }
            //                                else
            //                                    return iResult;
            //                            }
            //                            else
            //                                return iResult;
            //                        }
            //                        else
            //                            return iResult;
            //                    }
            //                    else
            //                        return iResult;
            //                }
            //                else
            //                    return iResult;
            //            }

            //            m_bJogTOld = bDir;
            //        }
            //        break;

            //    //  Z-key (Up/Down)
            //    case DEF_OPPANEL_JOG_Z_KEY:
            //        //  Z-key에 설정이 되어 있으면
            //        iCoordID = m_JogTable.MotionArray[iUnitIndex].m_ZKey.AxisIndex;
            //        if (iCoordID == DEF_OPPANEL_NO_JOGKEY && iUnitIndex2 >= 0)
            //            iUnitIndex = iUnitIndex2;
            //        iCoordID = m_JogTable.MotionArray[iUnitIndex].m_ZKey.AxisIndex;

            //        if (iCoordID > DEF_OPPANEL_NO_JOGKEY)
            //        {
            //            //  Motion 정지 확인
            //            if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_ZKey.m_plnkJog.CheckDone(iCoordID, out bDone)) != SUCCESS)
            //                return iResult;

            //            //  Pitch 단위 이동
            //            if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_ZKey.m_plnkJog.JogMovePitch(iCoordID, bDir)) != SUCCESS)
            //            {
            //                if (bDir != m_bJogZOld)     // 진행할 방향이 E-Stop 걸리게 된 방향과 반대이면
            //                {
            //                    //  Motion 상태 확인
            //                    if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_ZKey.m_plnkJog.GetAxisState(iCoordID, out iState)) != SUCCESS)
            //                    {
            //                        if (iState == DEF_E_STOP_EVENT)
            //                        {
            //                            if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_ZKey.m_plnkJog.GetAxisStatus(iCoordID, out iSource)) != SUCCESS)
            //                            {
            //                                if (iSource & DEF_ST_HOME_SWITCH)
            //                                {
            //                                    //  E-Stop Event 해제
            //                                    iEvent = DEF_NO_EVENT;
            //                                    m_JogTable.MotionArray[iUnitIndex].m_ZKey.m_plnkJog.SetHomeSensorEvent(iCoordID, out iEvent);

            //                                    //  E-Stop 벗어나는 방향으로 1mm 이동
            //                                    dDistance = bDir ? dDistance : -dDistance;
            //                                    m_JogTable.MotionArray[iUnitIndex].m_ZKey.m_plnkJog.GetFineVelocity(iCoordID, out dVelocity, out iAccelerate);
            //                                    iDecelerate = iAccelerate;
            //                                    if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_ZKey.m_plnkJog.RMove(iCoordID, null, out dDistance, out dVelocity, out iAccelerate, out iDecelerate, DEF_SMOVE_DISTANCE, false)) != SUCCESS)
            //                                        return iResult;

            //                                    //  E-Stop Event 설정
            //                                    iEvent = DEF_E_STOP_EVENT;
            //                                    m_JogTable.MotionArray[iUnitIndex].m_ZKey.m_plnkJog.SetHomeSensorEvent(iCoordID, out iEvent);

            //                                    //  Pitch 단위 이동
            //                                    if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_ZKey.m_plnkJog.JogMovePitch(iCoordID, bDir)) != SUCCESS)
            //                                        return iResult;
            //                                }
            //                                else if (iSource & DEF_ST_POS_LIMIT)
            //                                {
            //                                    //  E-Stop Event 해제
            //                                    iEvent = DEF_NO_EVENT;
            //                                    m_JogTable.MotionArray[iUnitIndex].m_ZKey.m_plnkJog.SetPositiveSensorEvent(iCoordID, out iEvent);

            //                                    //  E-Stop 벗어나는 방향으로 1mm 이동
            //                                    dDistance = bDir ? dDistance : -dDistance;
            //                                    m_JogTable.MotionArray[iUnitIndex].m_ZKey.m_plnkJog.GetFineVelocity(iCoordID, out dVelocity, out iAccelerate);
            //                                    iDecelerate = iAccelerate;
            //                                    if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_ZKey.m_plnkJog.RMove(iCoordID, null, out dDistance, out dVelocity, out iAccelerate, out iDecelerate, DEF_SMOVE_DISTANCE, false)) != SUCCESS)
            //                                        return iResult;

            //                                    //  E-Stop Event 설정
            //                                    iEvent = DEF_E_STOP_EVENT;
            //                                    m_JogTable.MotionArray[iUnitIndex].m_ZKey.m_plnkJog.SetPositiveSensorEvent(iCoordID, out iEvent);

            //                                    //  Pitch 단위 이동
            //                                    if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_ZKey.m_plnkJog.JogMovePitch(iCoordID, bDir)) != SUCCESS)
            //                                        return iResult;
            //                                }
            //                                else if (iSource & DEF_ST_NEG_LIMIT)
            //                                {
            //                                    //  E-Stop Event 해제
            //                                    iEvent = DEF_NO_EVENT;
            //                                    m_JogTable.MotionArray[iUnitIndex].m_ZKey.m_plnkJog.SetNegativeSensorEvent(iCoordID, out iEvent);

            //                                    //  E-Stop 벗어나는 방향으로 1mm 이동
            //                                    dDistance = bDir ? dDistance : -dDistance;
            //                                    m_JogTable.MotionArray[iUnitIndex].m_ZKey.m_plnkJog.GetFineVelocity(iCoordID, out dVelocity, out iAccelerate);
            //                                    iDecelerate = iAccelerate;
            //                                    if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_ZKey.m_plnkJog.RMove(iCoordID, null, out dDistance, out dVelocity, out iAccelerate, out iDecelerate, DEF_SMOVE_DISTANCE, false)) != SUCCESS)
            //                                        return iResult;

            //                                    //  E-Stop Event 설정
            //                                    iEvent = DEF_E_STOP_EVENT;
            //                                    m_JogTable.MotionArray[iUnitIndex].m_ZKey.m_plnkJog.SetNegativeSensorEvent(iCoordID, out iEvent);

            //                                    //  Pitch 단위 이동
            //                                    if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_ZKey.m_plnkJog.JogMovePitch(iCoordID, bDir)) != SUCCESS)
            //                                        return iResult;
            //                                }
            //                                else
            //                                    return iResult;
            //                            }
            //                            else
            //                                return iResult;
            //                        }
            //                        else
            //                            return iResult;
            //                    }
            //                    else
            //                        return iResult;
            //                }
            //                else
            //                    return iResult;
            //            }

            //            m_bJogZOld = bDir;
            //        }
            //        break;

            //    default:
            //        return GenerateErrorCode(ERR_OPPANEL_INVALID_JOG_KEY_TYPE);
            //}

            return iResult;
        }

        /**
        * Velocity 단위의 Jog 이동 동작을 수행한다.
        *
        * @param	iUnitIndex : Jog로 움직일 Motion에 대한 정보 Table의 Index
        * @param	iKey : 이동할 Jog Key 종류 (0:X, 1:Y, 2:T, 3:Z)
        * @param	bDir : 이동할 방향 (true: +, false: -)
        * @return	Error Code : 0=SUCCESS, 그외=Error
*/
        public int MoveJogVelocity(int iUnitIndex, int iUnitIndex2, int iKey, bool bDir)
        {
            int iResult = SUCCESS;
            //bool bDone;
            //int iCoordID;

            //// Unit Index 범위 점검 
            //if ((iUnitIndex < 0) || (iUnitIndex > m_JogTable.ListNo))
            //    return GenerateErrorCode(ERR_OPPANEL_INVALID_JOG_UNIT_INDEX);

            //if (iUnitIndex2 == DEF_ManageOpPanel.DEF_MNGOPPANEL_JOG_NO_USE) // UnitIndex2가 안되어 있을때, 오류를 막기위해
            //    iUnitIndex2 = iUnitIndex;

            //// 눌려진 Jog Key에 따라 동작 
            //switch (iKey)
            //{
            //    // X-key (Left/Right) 
            //    case DEF_OPPANEL_JOG_X_KEY:
            //        // X-key에 설정이 되어 있으면 
            //        iCoordID = m_JogTable.MotionArray[iUnitIndex].m_XKey.AxisIndex;
            //        if (iCoordID == DEF_OPPANEL_NO_JOGKEY && iUnitIndex2 >= 0)
            //            iUnitIndex = iUnitIndex2;
            //        iCoordID = m_JogTable.MotionArray[iUnitIndex].m_XKey.AxisIndex;

            //        if (iCoordID > DEF_OPPANEL_NO_JOGKEY)
            //        {
            //            // Motion 정지 확인 
            //            if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_XKey.m_plnkJog.CheckDone(iCoordID, out bDone)) != SUCCESS)
            //                return iResult;

            //            // Motion이 정지한 상태이면 
            //            //if (bDone == true)
            //            //{
            //            // Pitch 단위 이동 
            //            if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_XKey.m_plnkJog.JogMoveVelocity(iCoordID, bDir)) != SUCCESS)
            //                return iResult;
            //            //}
            //        }
            //        break;

            //    // Y-key (For/Back) 
            //    case DEF_OPPANEL_JOG_Y_KEY:
            //        // Y-key에 설정이 되어 있으면 
            //        iCoordID = m_JogTable.MotionArray[iUnitIndex].m_YKey.AxisIndex;
            //        if (iCoordID == DEF_OPPANEL_NO_JOGKEY && iUnitIndex2 >= 0)
            //            iUnitIndex = iUnitIndex2;
            //        iCoordID = m_JogTable.MotionArray[iUnitIndex].m_YKey.AxisIndex;

            //        if (iCoordID > DEF_OPPANEL_NO_JOGKEY)
            //        {
            //            // Motion 정지 확인 
            //            if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_YKey.m_plnkJog.CheckDone(iCoordID, out bDone)) != SUCCESS)
            //                return iResult;

            //            // Motion이 정지한 상태이면 
            //            //if (bDone == true)
            //            //{
            //            // Pitch 단위 이동 
            //            if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_YKey.m_plnkJog.JogMoveVelocity(iCoordID, bDir)) != SUCCESS)
            //                return iResult;
            //            //}
            //        }
            //        break;

            //    // T-key (CW/CCW) 
            //    case DEF_OPPANEL_JOG_T_KEY:
            //        // T-key에 설정이 되어 있으면 
            //        iCoordID = m_JogTable.MotionArray[iUnitIndex].m_TKey.AxisIndex;
            //        if (iCoordID == DEF_OPPANEL_NO_JOGKEY && iUnitIndex2 >= 0)
            //            iUnitIndex = iUnitIndex2;
            //        iCoordID = m_JogTable.MotionArray[iUnitIndex].m_TKey.AxisIndex;

            //        if (iCoordID > DEF_OPPANEL_NO_JOGKEY)
            //        {
            //            // Motion 정지 확인 
            //            if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_TKey.m_plnkJog.CheckDone(iCoordID, out bDone)) != SUCCESS)
            //                return iResult;

            //            // Motion이 정지한 상태이면 
            //            //if (bDone == true)
            //            //{
            //            // Pitch 단위 이동 
            //            if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_TKey.m_plnkJog.JogMoveVelocity(iCoordID, bDir)) != SUCCESS)
            //                return iResult;
            //            //}
            //        }
            //        break;

            //    // Z-key (Up/Down) 
            //    case DEF_OPPANEL_JOG_Z_KEY:
            //        // Z-key에 설정이 되어 있으면 
            //        iCoordID = m_JogTable.MotionArray[iUnitIndex].m_ZKey.AxisIndex;
            //        if (iCoordID == DEF_OPPANEL_NO_JOGKEY && iUnitIndex2 >= 0)
            //            iUnitIndex = iUnitIndex2;
            //        iCoordID = m_JogTable.MotionArray[iUnitIndex].m_ZKey.AxisIndex;

            //        if (iCoordID > DEF_OPPANEL_NO_JOGKEY)
            //        {
            //            // Motion 정지 확인 
            //            if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_ZKey.m_plnkJog.CheckDone(iCoordID, out bDone)) != SUCCESS)
            //                return iResult;

            //            // Motion이 정지한 상태이면 
            //            //if (bDone == true)
            //            //{
            //            // Pitch 단위 이동 
            //            if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_ZKey.m_plnkJog.JogMoveVelocity(iCoordID, bDir)) != SUCCESS)
            //                return iResult;
            //            //}
            //        }
            //        break;

            //    default:
            //        return GenerateErrorCode(ERR_OPPANEL_INVALID_JOG_KEY_TYPE);
            //}

            return iResult;
        }

        /**
        * Jog로 이동한 것에 대한 정지 동작을 수행한다.
        *
        * @param	iUnitIndex : Jog로 움직일 Motion에 대한 정보 Table의 Index
        * @param	iKey : 정지할 Jog Key 종류 (0:X, 1:Y, 2:T, 3:Z)
        * @return	Error Code : 0=SUCCESS, 그외=Error
*/
        public int StopJog(int iUnitIndex, int iUnitIndex2, int iKey)
        {
            int iResult = SUCCESS;
            //bool bDone;
            //int iCoordID;

            //// Unit Index 범위 점검 
            //if ((iUnitIndex < 0) || (iUnitIndex > m_JogTable.ListNo))
            //    return GenerateErrorCode(ERR_OPPANEL_INVALID_JOG_UNIT_INDEX);

            //if (iUnitIndex2 == DEF_MNGOPPANEL_JOG_NO_USE) // UnitIndex2가 안되어 있을때, 오류를 막기위해
            //    iUnitIndex2 = iUnitIndex;

            //// 눌려진 Jog Key에 따라 동작 
            //switch (iKey)
            //{
            //    // X-key (Left/Right) 
            //    case DEF_OPPANEL_JOG_X_KEY:
            //        // X-key에 설정이 되어 있으면 
            //        iCoordID = m_JogTable.MotionArray[iUnitIndex].m_XKey.AxisIndex;
            //        if (iCoordID == DEF_OPPANEL_NO_JOGKEY && iUnitIndex2 >= 0)
            //            iUnitIndex = iUnitIndex2;
            //        iCoordID = m_JogTable.MotionArray[iUnitIndex].m_XKey.AxisIndex;

            //        if (iCoordID > DEF_OPPANEL_NO_JOGKEY)
            //        {
            //            // Motion 정지 확인 
            //            if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_XKey.m_plnkJog.CheckDone(iCoordID, out bDone)) != SUCCESS)
            //                return iResult;

            //            // Motion이 이동 중 상태이면 
            //            if (bDone == false)
            //            {
            //                // 이동 정지 
            //                if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_XKey.m_plnkJog.VStop(iCoordID)) != SUCCESS)
            //                    return iResult;
            //            }
            //        }
            //        break;

            //    // Y-key (For/Back) 
            //    case DEF_OPPANEL_JOG_Y_KEY:
            //        // Y-key에 설정이 되어 있으면 
            //        iCoordID = m_JogTable.MotionArray[iUnitIndex].m_YKey.AxisIndex;
            //        if (iCoordID == DEF_OPPANEL_NO_JOGKEY && iUnitIndex2 >= 0)
            //            iUnitIndex = iUnitIndex2;
            //        iCoordID = m_JogTable.MotionArray[iUnitIndex].m_YKey.AxisIndex;

            //        if (iCoordID > DEF_OPPANEL_NO_JOGKEY)
            //        {
            //            // Motion 정지 확인 
            //            if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_YKey.m_plnkJog.CheckDone(iCoordID, out bDone)) != SUCCESS)
            //                return iResult;

            //            // Motion이 이동 중 상태이면 
            //            if (bDone == false)
            //            {
            //                // 이동 정지 
            //                if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_YKey.m_plnkJog.VStop(iCoordID)) != SUCCESS)
            //                    return iResult;
            //            }
            //        }
            //        break;

            //    // T-key (CW/CCW) 
            //    case DEF_OPPANEL_JOG_T_KEY:
            //        // T-key에 설정이 되어 있으면 
            //        iCoordID = m_JogTable.MotionArray[iUnitIndex].m_TKey.AxisIndex;
            //        if (iCoordID == DEF_OPPANEL_NO_JOGKEY && iUnitIndex2 >= 0)
            //            iUnitIndex = iUnitIndex2;
            //        iCoordID = m_JogTable.MotionArray[iUnitIndex].m_TKey.AxisIndex;

            //        if (iCoordID > DEF_OPPANEL_NO_JOGKEY)
            //        {
            //            // Motion 정지 확인 
            //            if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_TKey.m_plnkJog.CheckDone(iCoordID, out bDone)) != SUCCESS)
            //                return iResult;

            //            // Motion이 이동 중 상태이면 
            //            if (bDone == false)
            //            {
            //                // 이동 정지 
            //                if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_TKey.m_plnkJog.VStop(iCoordID)) != SUCCESS)
            //                    return iResult;
            //            }
            //        }
            //        break;

            //    // Z-key (Up/Down) 
            //    case DEF_OPPANEL_JOG_Z_KEY:
            //        // Z-key에 설정이 되어 있으면 
            //        iCoordID = m_JogTable.MotionArray[iUnitIndex].m_ZKey.AxisIndex;
            //        if (iCoordID == DEF_OPPANEL_NO_JOGKEY && iUnitIndex2 >= 0)
            //            iUnitIndex = iUnitIndex2;
            //        iCoordID = m_JogTable.MotionArray[iUnitIndex].m_ZKey.AxisIndex;

            //        if (iCoordID > DEF_OPPANEL_NO_JOGKEY)
            //        {
            //            // Motion 정지 확인 
            //            if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_ZKey.m_plnkJog.CheckDone(iCoordID, out bDone)) != SUCCESS)
            //                return iResult;

            //            // Motion이 이동 중 상태이면 
            //            if (bDone == false)
            //            {
            //                // 이동 정지 
            //                if ((iResult = m_JogTable.MotionArray[iUnitIndex].m_ZKey.m_plnkJog.VStop(iCoordID)) != SUCCESS)
            //                    return iResult;
            //            }
            //        }
            //        break;

            //    default:
            //        return GenerateErrorCode(ERR_OPPANEL_INVALID_JOG_KEY_TYPE);
            //}

            return iResult;
        }

        /**
        * 모든 축들에 대해 원점복귀 동작을 정지한다.
        *
        * @return	Error Code : 0=SUCCESS, 그외=Error
*/
        public int StopAllReturnOrigin()
        {
            int i = 0;
            int iResult = SUCCESS;

            for (i = 0; i < m_JogTable.ListNo; i++)
            {
                if (m_JogTable.MotionArray[i].m_XKey.m_plnkJog != null)
                    iResult = m_JogTable.MotionArray[i].m_XKey.m_plnkJog.StopReturnOrigin();
                if (m_JogTable.MotionArray[i].m_YKey.m_plnkJog != null)
                    iResult = m_JogTable.MotionArray[i].m_YKey.m_plnkJog.StopReturnOrigin();
                if (m_JogTable.MotionArray[i].m_TKey.m_plnkJog != null)
                    iResult = m_JogTable.MotionArray[i].m_TKey.m_plnkJog.StopReturnOrigin();
                if (m_JogTable.MotionArray[i].m_ZKey.m_plnkJog != null)
                    iResult = m_JogTable.MotionArray[i].m_ZKey.m_plnkJog.StopReturnOrigin();
            }

            return iResult;
        }

        /**
        * 모든 축들에 대해 Servo AMP를 Enable한다.
        *
        * @return	Error Code : 0=SUCCESS, 그외=Error
*/
        public int OnAllServo()
        {
            int i = 0;
            int iResult = SUCCESS;

            //for (i = 0; i < DEF_MAX_MOTION_AXIS_NO; i++)
            //{
            //    // AMP Disable 
            //    if ((iResult = m_pMotionLib.SetAmpEnable(i, false)) != ERR_MOTION_SUCCESS)
            //        return iResult;
            //} // Next i
            //Sleep(10);
            //for (i = 0; i < DEF_MAX_MOTION_AXIS_NO; i++)
            //{
            //    // AMP Fault Reset 
            //    if ((iResult = m_pMotionLib.SetAmpFaultEnable(i, false)) != ERR_MOTION_SUCCESS)
            //        return iResult;
            //} // Next i
            //Sleep(10);
            //for (i = 0; i < DEF_MAX_MOTION_AXIS_NO; i++)
            //{
            //    // AMP Fault Set 
            //    if ((iResult = m_pMotionLib.SetAmpFaultEnable(i, true)) != ERR_MOTION_SUCCESS)
            //        return iResult;
            //} // Next i
            //Sleep(10);
            //for (i = 0; i < DEF_MAX_MOTION_AXIS_NO; i++)
            //{
            //    // Status Clear 
            //    if ((iResult = m_pMotionLib.ClearStatus(i)) != ERR_MOTION_SUCCESS)
            //        return iResult;
            //} // Next i
            //Sleep(10);
            //for (i = 0; i < DEF_MAX_MOTION_AXIS_NO; i++)
            //{
            //    // Frame Clear 
            //    if ((iResult = m_pMotionLib.ClearFrames(i)) != ERR_MOTION_SUCCESS)
            //        return iResult;
            //} // Next i
            //Sleep(10);
            //for (i = 0; i < DEF_MAX_MOTION_AXIS_NO; i++)
            //{
            //    // AMP Enable 
            //    if ((iResult = m_pMotionLib.SetAmpEnable(i, true)) != ERR_MOTION_SUCCESS)
            //        return iResult;
            //} // Next i

            return iResult;
        }

        /**
        * 모든 축들에 대해 Servo AMP를 Disable한다.
        *
        * @return	Error Code : 0=SUCCESS, 그외=Error
*/
        public int OffAllServo()
        {
            int i = 0;
            int iResult = SUCCESS;

            //for (i = 0; i < DEF_MAX_MOTION_AXIS_NO; i++)
            //{
            //    // AMP Disable 
            //    if ((iResult = m_pMotionLib.SetAmpEnable(i, false)) != ERR_MOTION_SUCCESS)
            //        return iResult;
            //} // Next i
            //Sleep(10);
            //for (i = 0; i < DEF_MAX_MOTION_AXIS_NO; i++)
            //{
            //    // AMP Fault Reset 
            //    if ((iResult = m_pMotionLib.SetAmpFaultEnable(i, false)) != ERR_MOTION_SUCCESS)
            //        return iResult;
            //} // Next i
            //Sleep(10);
            //for (i = 0; i < DEF_MAX_MOTION_AXIS_NO; i++)
            //{
            //    // AMP Fault Set 
            //    if ((iResult = m_pMotionLib.SetAmpFaultEnable(i, true)) != ERR_MOTION_SUCCESS)
            //        return iResult;
            //} // Next i
            //Sleep(10);
            //for (i = 0; i < DEF_MAX_MOTION_AXIS_NO; i++)
            //{
            //    // Status Clear 
            //    if ((iResult = m_pMotionLib.ClearStatus(i)) != ERR_MOTION_SUCCESS)
            //        return iResult;
            //} // Next i
            //Sleep(10);
            //for (i = 0; i < DEF_MAX_MOTION_AXIS_NO; i++)
            //{
            //    // Frame Clear 
            //    if ((iResult = m_pMotionLib.ClearFrames(i)) != ERR_MOTION_SUCCESS)
            //        return iResult;
            //} // Next i

            return iResult;
        }

        /**
        * 모든 축들에 대해 동작을 ESTOP 정지한다.
        *
        * @return	Error Code : 0=SUCCESS, 그외=Error
*/
        public int EStopAllAxis()
        {
            int i = 0;
            int iResult = SUCCESS;

            //for (i = 0; i < m_JogTable.ListNo; i++)
            //{
            //    if (m_JogTable.MotionArray[i].m_XKey.m_plnkJog != null)
            //    {
            //        iResult = m_JogTable.MotionArray[i].m_XKey.m_plnkJog.EStop(m_JogTable.MotionArray[i].m_XKey.AxisIndex);
            //        m_JogTable.MotionArray[i].m_XKey.m_plnkJog.ResetOrigin(m_JogTable.MotionArray[i].m_XKey.AxisIndex);
            //    }
            //    if (m_JogTable.MotionArray[i].m_YKey.m_plnkJog != null)
            //    {
            //        iResult = m_JogTable.MotionArray[i].m_YKey.m_plnkJog.EStop(m_JogTable.MotionArray[i].m_YKey.AxisIndex);
            //        m_JogTable.MotionArray[i].m_YKey.m_plnkJog.ResetOrigin(m_JogTable.MotionArray[i].m_YKey.AxisIndex);
            //    }
            //    if (m_JogTable.MotionArray[i].m_TKey.m_plnkJog != null)
            //    {
            //        iResult = m_JogTable.MotionArray[i].m_TKey.m_plnkJog.EStop(m_JogTable.MotionArray[i].m_TKey.AxisIndex);
            //        m_JogTable.MotionArray[i].m_TKey.m_plnkJog.ResetOrigin(m_JogTable.MotionArray[i].m_TKey.AxisIndex);
            //    }
            //    if (m_JogTable.MotionArray[i].m_ZKey.m_plnkJog != null)
            //    {
            //        iResult = m_JogTable.MotionArray[i].m_ZKey.m_plnkJog.EStop(m_JogTable.MotionArray[i].m_ZKey.AxisIndex);
            //        m_JogTable.MotionArray[i].m_ZKey.m_plnkJog.ResetOrigin(m_JogTable.MotionArray[i].m_ZKey.AxisIndex);
            //    }
            //}

            //// Unit의 초기화 Flag를 Reset해야 한다.
            //for (i = 0; i < INIT_UNIT_MAX; i++)
            //    m_bInitFlag[i] = false;

            return iResult;
        }

        /**
        * 모든 축들에 대해 동작을 정지한다.
        *
        * @return	Error Code : 0=SUCCESS, 그외=Error
*/
        public int StopAllAxis()
        {
            int i = 0;
            int iResult = SUCCESS;

            //for (i = 0; i < m_JogTable.ListNo; i++)
            //{
            //    if (m_JogTable.MotionArray[i].m_XKey.m_plnkJog != null)
            //        iResult = m_JogTable.MotionArray[i].m_XKey.m_plnkJog.VStop(DEF_ALL_COORDINATE);
            //    if (m_JogTable.MotionArray[i].m_YKey.m_plnkJog != null)
            //        iResult = m_JogTable.MotionArray[i].m_YKey.m_plnkJog.VStop(DEF_ALL_COORDINATE);
            //    if (m_JogTable.MotionArray[i].m_TKey.m_plnkJog != null)
            //        iResult = m_JogTable.MotionArray[i].m_TKey.m_plnkJog.VStop(DEF_ALL_COORDINATE);
            //    if (m_JogTable.MotionArray[i].m_ZKey.m_plnkJog != null)
            //        iResult = m_JogTable.MotionArray[i].m_ZKey.m_plnkJog.VStop(DEF_ALL_COORDINATE);
            //}

            return iResult;
        }

        /**
        * Unit의 초기화 Flag를 설정한다.
        *
        * @param	int iUnitIndex : 초기화 Flag 설정할 Unit의 Index (DefSystem.h에 정의되어 있음)
        * @param	bool bSts :  (OPTION=false) 설정할 Flag 값
        * @return	Error Code : 0=SUCCESS, 그외=Error
*/
        public int SetInitFlag(int iUnitIndex, bool bSts)
        {
            int iResult = SUCCESS;

            if ((iUnitIndex < 0) || (iUnitIndex > INIT_UNIT_MAX))
                return GenerateErrorCode(ERR_OPPANEL_INVALID_INIT_UNIT_INDEX);

            m_bInitFlag[iUnitIndex] = bSts;

            return iResult;
        }

        /**
        * Unit의 초기화 Flag를 읽는다.
        *
        * @param	int iUnitIndex : 초기화 Flag 설정할 Unit의 Index (DefSystem.h에 정의되어 있음)
        * @param	bool *pbSts :  초기화 Flag 값
        * @return	Error Code : 0=SUCCESS, 그외=Error
*/
        public int GetInitFlag(int iUnitIndex, out bool pbSts)
        {
            int iResult = SUCCESS;
            pbSts = false;

            if ((iUnitIndex < 0) || (iUnitIndex > INIT_UNIT_MAX))
                return GenerateErrorCode(ERR_OPPANEL_INVALID_INIT_UNIT_INDEX);

            pbSts = m_bInitFlag[iUnitIndex];

            return iResult;
        }

        /**
        * 시스템의 모든 Unit들이 초기화되어 있는지 확인한다.
        *
        * @param	bInitSts[] : (OPTION=null) 각 Unit별로 초기화 상태 (true:초기화되어 있음, false:아님)
        * @return	모든 Unit이 초기화되어 있으면 true, 아니면 false return
*/
        public bool CheckAllInit(out bool[] bInitSts)
        {
            bool bSts = false;
            bool bResult = true;

            bInitSts = new bool[INIT_UNIT_MAX];
            //bool bEStopSts;
            //GetEStopButtonStatus(&bEStopSts);
            //int i = 0;
            //for (i = 0; i < INIT_UNIT_MAX; i++)
            //{
            //    if (bEStopSts == true)
            //    {
            //        bResult = false;
            //        SetInitFlag(i, false);
            //    }
            //    else
            //    {
            //        GetInitFlag(i, out bSts);

            //        bResult &= bSts;

            //        if (bInitSts != null)
            //            bInitSts[i] = bSts;
            //    }
            //}

            return bResult;
        }

        /**
        * Unit의 원점복귀 Flag를 설정한다.
*/
        public void ResetAllOriginFlag()
        {
            int i = 0;

            for (i = 0; i < m_JogTable.ListNo; i++)
            {
                if (m_JogTable.MotionArray[i].m_XKey.m_plnkJog != null)
                    m_JogTable.MotionArray[i].m_XKey.m_plnkJog.ResetOrigin(m_JogTable.MotionArray[i].m_XKey.AxisIndex);
                if (m_JogTable.MotionArray[i].m_YKey.m_plnkJog != null)
                    m_JogTable.MotionArray[i].m_YKey.m_plnkJog.ResetOrigin(m_JogTable.MotionArray[i].m_YKey.AxisIndex);
                if (m_JogTable.MotionArray[i].m_TKey.m_plnkJog != null)
                    m_JogTable.MotionArray[i].m_TKey.m_plnkJog.ResetOrigin(m_JogTable.MotionArray[i].m_TKey.AxisIndex);
                if (m_JogTable.MotionArray[i].m_ZKey.m_plnkJog != null)
                    m_JogTable.MotionArray[i].m_ZKey.m_plnkJog.ResetOrigin(m_JogTable.MotionArray[i].m_ZKey.AxisIndex);
            }
        }

        /**
        * 축의 원점복귀 전체 상태를 읽는다.
        *
        * @param	bool *pbSts :  원점복귀 전체 상태
        * @return	Error Code : 0=SUCCESS, 그외=Error
*/
        public bool CheckAllOrigin(out bool[/*DEF_MAX_MOTION_AXIS_NO*/] bOriginSts)
        {
            bOriginSts = new bool[DEF_MAX_MOTION_AXIS_NO];
            return false;
            //int i = 0;
            //bool rgbResult[4] = { false, false, false, false };
            //bool bSts = true;
            //bool rgbUse[4] = { true, true, true, true };

            //// Motion MultiAxes Component - STAGE1
            //for (i = 0; i < 4; i++) rgbResult[i] = false;
            //m_JogTable.MotionArray[DEF_JOG_STAGE1].m_XKey.m_plnkJog.IsOriginReturn(-1, rgbUse, rgbResult);
            //if (bOriginSts != null)
            //    bOriginSts[DEF_AXIS_MMC_STAGE1_X] = rgbResult[DEF_X];
            //bSts &= rgbResult[DEF_X];
            //if (bOriginSts != null)
            //    bOriginSts[DEF_AXIS_MMC_STAGE1_Y] = rgbResult[DEF_Y];
            //bSts &= rgbResult[DEF_Y];
            //if (bOriginSts != null)
            //    bOriginSts[DEF_AXIS_MMC_STAGE1_T] = rgbResult[DEF_T];
            //bSts &= rgbResult[DEF_T];
            //if (bOriginSts != null)
            //    bOriginSts[DEF_AXIS_MMC_STAGE1_Z] = rgbResult[DEF_Z];
            //bSts &= rgbResult[DEF_Z];

            //// Motion MultiAxes Component - STAGE2
            //for (i = 0; i < 4; i++) rgbResult[i] = false;
            //m_JogTable.MotionArray[DEF_JOG_STAGE2].m_XKey.m_plnkJog.IsOriginReturn(-1, rgbUse, rgbResult);
            //if (bOriginSts != null)
            //    bOriginSts[DEF_AXIS_MMC_STAGE2_X] = rgbResult[DEF_X];
            //bSts &= rgbResult[DEF_X];
            //if (bOriginSts != null)
            //    bOriginSts[DEF_AXIS_MMC_STAGE2_Z] = rgbResult[DEF_Z];
            //bSts &= rgbResult[DEF_Z];

            //// Motion MultiAxes Component - STAGE3
            //for (i = 0; i < 4; i++) rgbResult[i] = false;
            //m_JogTable.MotionArray[DEF_JOG_STAGE3].m_YKey.m_plnkJog.IsOriginReturn(-1, rgbUse, rgbResult);
            //if (bOriginSts != null)
            //    bOriginSts[DEF_AXIS_MMC_STAGE3_Y] = rgbResult[DEF_Y];
            //bSts &= rgbResult[DEF_Y];
            //if (bOriginSts != null)
            //    bOriginSts[DEF_AXIS_MMC_STAGE3_T] = rgbResult[DEF_T];
            //bSts &= rgbResult[DEF_T];

            //// Motion MultiAxes Component - WORKBENCH1 
            //// 	for (i = 0; i < 4; i++) rgbResult[i] = false;
            //// 	m_JogTable.MotionArray[DEF_JOG_WORKBENCH].m_YKey.m_plnkJog.IsOriginReturn(0, null, rgbResult);
            //// 	if (bOriginSts != null)
            //// 		bOriginSts[DEF_AXIS_MMC_WORKBENCH_Y] = rgbResult[DEF_X];
            //// 	bSts &= rgbResult[DEF_X];

            //// Motion MultiAxes Component - SHEAD1 
            //for (i = 0; i < 4; i++) rgbResult[i] = false;
            //m_JogTable.MotionArray[DEF_JOG_SHEAD1].m_YKey.m_plnkJog.IsOriginReturn(-1, null, rgbResult);
            //if (bOriginSts != null)
            //    bOriginSts[DEF_AXIS_MMC_SHEAD1_Y] = rgbResult[DEF_Y];
            //bSts &= rgbResult[DEF_Y];
            //if (bOriginSts != null)
            //    bOriginSts[DEF_AXIS_MMC_SHEAD1_Z] = rgbResult[DEF_Z];
            //bSts &= rgbResult[DEF_Z];

            //// Motion MultiAxes Component - SHEAD2 
            //for (i = 0; i < 4; i++) rgbResult[i] = false;
            //m_JogTable.MotionArray[DEF_JOG_SHEAD2].m_YKey.m_plnkJog.IsOriginReturn(-1, null, rgbResult);
            //if (bOriginSts != null)
            //    bOriginSts[DEF_AXIS_MMC_SHEAD2_Y] = rgbResult[DEF_Y];
            //bSts &= rgbResult[DEF_Y];
            //if (bOriginSts != null)
            //    bOriginSts[DEF_AXIS_MMC_SHEAD2_Z] = rgbResult[DEF_Z];
            //bSts &= rgbResult[DEF_Z];

            //// Motion MultiAxes Component - GHEAD1 
            //for (i = 0; i < 4; i++) rgbResult[i] = false;
            //m_JogTable.MotionArray[DEF_JOG_GHEAD1].m_XKey.m_plnkJog.IsOriginReturn(-1, null, rgbResult);
            //if (bOriginSts != null)
            //    bOriginSts[DEF_AXIS_MMC_GHEAD1_X] = rgbResult[DEF_X];
            //bSts &= rgbResult[DEF_X];
            //if (bOriginSts != null)
            //    bOriginSts[DEF_AXIS_MMC_GHEAD1_Y] = rgbResult[DEF_Y];
            //bSts &= rgbResult[DEF_Y];
            //if (bOriginSts != null)
            //    bOriginSts[DEF_AXIS_MMC_GHEAD1_Z] = rgbResult[DEF_Z];
            //bSts &= rgbResult[DEF_Z];

            //// Motion MultiAxes Component - GHEAD2 
            //for (i = 0; i < 4; i++) rgbResult[i] = false;
            //m_JogTable.MotionArray[DEF_JOG_GHEAD2].m_XKey.m_plnkJog.IsOriginReturn(-1, null, rgbResult);
            //if (bOriginSts != null)
            //    bOriginSts[DEF_AXIS_MMC_GHEAD2_X] = rgbResult[DEF_X];
            //bSts &= rgbResult[DEF_X];
            //if (bOriginSts != null)
            //    bOriginSts[DEF_AXIS_MMC_GHEAD2_Y] = rgbResult[DEF_Y];
            //bSts &= rgbResult[DEF_Y];
            //if (bOriginSts != null)
            //    bOriginSts[DEF_AXIS_MMC_GHEAD2_Z] = rgbResult[DEF_Z];
            //bSts &= rgbResult[DEF_Z];

            //// Motion MultiAxes Component - CAMERA1 
            //for (i = 0; i < 4; i++) rgbResult[i] = false;
            //m_JogTable.MotionArray[DEF_JOG_CAMERA1].m_XKey.m_plnkJog.IsOriginReturn(0, null, rgbResult);
            //if (bOriginSts != null)
            //    bOriginSts[DEF_AXIS_MMC_CAMERA1_X] = rgbResult[DEF_X];
            //bSts &= rgbResult[DEF_X];

            //// Motion MultiAxes Component - CAMERA2 
            //for (i = 0; i < 4; i++) rgbResult[i] = false;
            //m_JogTable.MotionArray[DEF_JOG_CAMERA2].m_XKey.m_plnkJog.IsOriginReturn(0, null, rgbResult);
            //if (bOriginSts != null)
            //    bOriginSts[DEF_AXIS_MMC_CAMERA2_X] = rgbResult[DEF_X];
            //bSts &= rgbResult[DEF_X];

            //// Motion MultiAxes Component - UHANDLER 
            //for (i = 0; i < 4; i++) rgbResult[i] = false;
            //m_JogTable.MotionArray[DEF_JOG_UHANDLER].m_XKey.m_plnkJog.IsOriginReturn(0, null, rgbResult);
            //if (bOriginSts != null)
            //    bOriginSts[DEF_AXIS_MMC_UHANDLER_X] = rgbResult[DEF_X];
            //bSts &= rgbResult[DEF_X];

            //return bSts;
        }

        public int Initialize()
        {
            int iResult = SUCCESS;

            //if ((iResult = m_plnkIO.Initialize()) != SUCCESS)
            //{
            //    // 오류 동작 Log
            //    m_plogMng.WriteLog(DEF_MLOG_ERROR_LOG_LEVEL, "IO Component Object의 초기화에 실패하였습니다.", __FILE__, __LINE__);

            //    return iResult;
            //}

            //// 정상 동작 Log
            //m_plogMng.WriteLog("IO Component Object를 초기화하였습니다.", __FILE__, __LINE__);

            return SUCCESS;
        }

        public int GetStartButtonStatus(out bool pbStatus)
        {
            string str = "Start Button";
            return getPanelSwitchStatus(str, m_IOAddrTable.FrontPanel.RunInputAddr, m_IOAddrTable.BackPanel.RunInputAddr, out pbStatus);
        }

        public int GetStopButtonStatus(out bool pbStatus)
        {
            string str = "Stop Button";
            return getPanelSwitchStatus(str, m_IOAddrTable.FrontPanel.StopInputAddr, m_IOAddrTable.BackPanel.StopInputAddr, out pbStatus);
        }

        public int GetResetButtonStatus(out bool pbStatus)
        {
            string str = "Reset Button";
            return getPanelSwitchStatus(str, m_IOAddrTable.FrontPanel.ResetInputAddr, m_IOAddrTable.BackPanel.ResetInputAddr, out pbStatus);
        }

        /**
        * Teaching Pendant Stop Button의 상태를 읽는다.
        *
        * @param	*pbStatus : Teaching Pendant Stop Button 상태 (true : ON, false : OFF)
        * @return	Error Code : 0 = SUCCESS, 그외 = Error
*/
        public int GetTPStopButtonStatus(out bool pbStatus)
        {
            string str = "TP Stop Button";
            return getPanelSwitchStatus(str, m_IOAddrTable.FrontPanel.TPStopInputAddr, m_IOAddrTable.BackPanel.TPStopInputAddr, out pbStatus);
        }

        public int GetEStopButtonStatus(out bool pbStatus)
        {
            int iResult = SUCCESS;
            bool bStatus1 = false;
            bool bStatus2 = false;
            string strLogMessage;

            pbStatus = false;

            //// E-Stop Relay 1의 상태 읽기 
            //if ((iResult = m_plnkIO.IsOn(m_IOAddrTable.iEStopInputAddr[0], out bStatus1)) != SUCCESS)
            //{
            //    // 오류 동작 Log
            //    strLogMessage.Format("E-Stop Realy 1의 상태를 읽는데 실패했습니다.");
            //    m_plogMng.WriteLog(DEF_MLOG_ERROR_LOG_LEVEL, strLogMessage, __FILE__, __LINE__);

            //    return iResult;
            //}

            //// E-Stop Relay 2의 상태 읽기 
            //if ((iResult = m_plnkIO.IsOn(m_IOAddrTable.iEStopInputAddr[1], out bStatus2)) != SUCCESS)
            //{
            //    // 오류 동작 Log
            //    strLogMessage.Format("E-Stop Realy 2의 상태를 읽는데 실패했습니다.");
            //    m_plogMng.WriteLog(DEF_MLOG_ERROR_LOG_LEVEL, strLogMessage, __FILE__, __LINE__);

            //    return iResult;
            //}

            //// 하나라도 감지됐으면 true Return 
            //*pbStatus = bStatus1 || bStatus2;

            return SUCCESS;
        }

        public int GetJogYPlusButtonStatus(out bool pbStatus)
        {
            string str = "Jog Y(+) Button";
            return getPanelSwitchStatus(str, m_IOAddrTable.FrontPanel.YpInputAddr, m_IOAddrTable.BackPanel.YpInputAddr, out pbStatus);
        }

        public int GetJogYMinusButtonStatus(out bool pbStatus)
        {
            string str = "Jog Y(-) Button";
            return getPanelSwitchStatus(str, m_IOAddrTable.FrontPanel.YnInputAddr, m_IOAddrTable.BackPanel.YnInputAddr, out pbStatus);
        }

        public int GetJogXPlusButtonStatus(out bool pbStatus)
        {
            string str = "Jog X(+) Button";
            return getPanelSwitchStatus(str, m_IOAddrTable.FrontPanel.XpInputAddr, m_IOAddrTable.BackPanel.XpInputAddr, out pbStatus);
        }

        public int GetJogXMinusButtonStatus(out bool pbStatus)
        {
            string str = "Jog X(-) Button";
            return getPanelSwitchStatus(str, m_IOAddrTable.FrontPanel.XnInputAddr, m_IOAddrTable.BackPanel.XnInputAddr, out pbStatus);
        }

        public int GetJogZPlusButtonStatus(out bool pbStatus)
        {
            string str = "Jog Z(+) Button";
            return getPanelSwitchStatus(str, m_IOAddrTable.FrontPanel.ZpInputAddr, m_IOAddrTable.BackPanel.ZpInputAddr, out pbStatus);
        }

        public int GetJogZMinusButtonStatus(out bool pbStatus)
        {
            string str = "Jog Z(-) Button";
            return getPanelSwitchStatus(str, m_IOAddrTable.FrontPanel.ZnInputAddr, m_IOAddrTable.BackPanel.ZnInputAddr, out pbStatus);
        }

        public int GetJogTPlusButtonStatus(out bool pbStatus)
        {
            string str = "Jog T(+) Button";
            return getPanelSwitchStatus(str, m_IOAddrTable.FrontPanel.TpInputAddr, m_IOAddrTable.BackPanel.TpInputAddr, out pbStatus);
        }

        public int GetJogTMinusButtonStatus(out bool pbStatus)
        {
            string str = "Jog T(-) Button";
            return getPanelSwitchStatus(str, m_IOAddrTable.FrontPanel.TnInputAddr, m_IOAddrTable.BackPanel.TnInputAddr, out pbStatus);
        }

        public int GetSafeDoorStatus(out bool pbStatus, int iGroup = -1, int iIndex = -1)
        {
            int i, j;
            bool bResult;
            int iResult = SUCCESS;

            pbStatus = false;

            //// 센서 그룹  전체 확인 
            //if (iGroup == -1)
            //{
            //    for (i = 0; i < DEF_OPPANEL_MAX_DOOR_GROUP; i++)
            //    {
            //        if (iIndex == -1)
            //        {
            //            for (j = 0; j < DEF_OPPANEL_MAX_DOOR_SENSOR; j++)
            //            {
                            
            //                if (m_IOAddrTable.iSafeDoorAddr[i][j] != 0)
            //                {
            //                    if (m_IOAddrTable.rgbSafeDoorFlag[i][j] == true)
            //                    {
            //                        if ((iResult = m_plnkIO.IsOn(m_IOAddrTable.iSafeDoorAddr[i][j], out bResult)) != SUCCESS)
            //                            return iResult;

            //                        if (bResult == false)
            //                        {
            //                            *pbStatus = true;
            //                            return iResult;
            //                        }
            //                    }
            //                }
            //                // Sensor Address가 할당되어 있지 않으면 확인 중단 
            //                else
            //                    j = DEF_OPPANEL_MAX_DOOR_SENSOR;
            //            }
            //        }
            //        else
            //        {
                        
            //            if (m_IOAddrTable.iSafeDoorAddr[i][iIndex] != 0)
            //            {
            //                if (m_IOAddrTable.rgbSafeDoorFlag[i][iIndex] == true)
            //                {
            //                    if ((iResult = m_plnkIO.IsOn(m_IOAddrTable.iSafeDoorAddr[i][iIndex], out bResult)) != SUCCESS)
            //                        return iResult;

            //                    *pbStatus = !bResult;
            //                }
            //                else
            //                    *pbStatus = false;
            //            }
            //            else
            //                *pbStatus = false;
            //        }
            //    }
            //    *pbStatus = false;
            //}
            //// 센서 그룹 하나만 확인 
            //else
            //{
            //    if (iIndex == -1)
            //    {
            //        for (j = 0; j < DEF_OPPANEL_MAX_DOOR_SENSOR; j++)
            //        {
                        
            //            if (m_IOAddrTable.iSafeDoorAddr[iGroup][j] != 0)
            //            {
            //                if (m_IOAddrTable.rgbSafeDoorFlag[iGroup][j] == true)
            //                {
            //                    if ((iResult = m_plnkIO.IsOn(m_IOAddrTable.iSafeDoorAddr[iGroup][j], out bResult)) != SUCCESS)
            //                        return iResult;

            //                    if (bResult == false)
            //                    {
            //                        *pbStatus = true;
            //                        return iResult;
            //                    }
            //                }
            //            }
            //            // Sensor Address가 할당되어 있지 않으면 확인 중단 
            //            else
            //                j = DEF_OPPANEL_MAX_DOOR_SENSOR;
            //        }
            //    }
            //    else
            //    {
                    
            //        if (m_IOAddrTable.iSafeDoorAddr[iGroup][iIndex] != 0)
            //        {
            //            if (m_IOAddrTable.rgbSafeDoorFlag[iGroup][iIndex] == true)
            //            {
            //                if ((iResult = m_plnkIO.IsOn(m_IOAddrTable.iSafeDoorAddr[iGroup][iIndex], out bResult)) != SUCCESS)
            //                    return iResult;

            //                *pbStatus = !bResult;
            //            }
            //            else
            //                *pbStatus = false;
            //        }
            //        else
            //            *pbStatus = false;
            //    }
            //    *pbStatus = false;
            //}

            return iResult;
        }

        /**
        * CP Trip의 상태를 읽는다.
        *
        * @param	*pbStatus : CP Trip의 상태 (true : ON, false : OFF)
        * @param	iIndex : (OPTION=-1) 몇번째 센서인지 번호 (-1이면 전체 센서를 확인하여 하나라도 ON이면 결과를 ON으로 알린다.)
        * @return	Error Code : 0 = SUCCESS, 그외 = Error
*/
        public int GetCPTripStatus(out bool pbStatus, int iIndex)
        {
            int i = 0;
            bool bSts = false;
            bool bResult;
            int iResult = SUCCESS;

            pbStatus = false;

            //if (iIndex == -1)
            //{
            //    for (i = 0; i < DEF_OPPANEL_MAX_CP_TRIP_NO; i++)
            //    {
                    
            //        if (m_IOAddrTable.iCPTripAddr[i] != 0)
            //        {
            //            if ((iResult = m_plnkIO.IsOn(m_IOAddrTable.iCPTripAddr[i], out bResult)) != SUCCESS)
            //                return iResult;

            //            bSts = bSts || bResult;
            //        }
            //        /** Sensor Address가 할당되어 있지 않으면 확인 중단 */
            //        else
            //            break;
            //    }

            //    *pbStatus = bSts;
            //}
            //else
            //{
                
            //    if (m_IOAddrTable.iCPTripAddr[iIndex] != 0)
            //    {
            //        if ((iResult = m_plnkIO.IsOn(m_IOAddrTable.iCPTripAddr[iIndex], out bResult)) != SUCCESS)
            //            return iResult;

            //        *pbStatus = bResult;
            //    }
            //    else
            //        *pbStatus = false;
            //}

            return iResult;
        }

        /**
        * Air Error의 상태를 읽는다.
        *
        * @param	*pbStatus : Air Error의 상태 (true : ON, false : OFF)
        * @return	Error Code : 0 = SUCCESS, 그외 = Error
*/
        public int GetAirErrorStatus(out bool pbStatus)
        {
            pbStatus = false;
            return SUCCESS;

            //bool bSts = false;
            //int iResult = SUCCESS;

            //if (m_IOAddrTable.iMainAirAddr != 0)
            //{
            //    if ((iResult = m_plnkIO.IsOff(m_IOAddrTable.iMainAirAddr, out bSts)) != SUCCESS)
            //        return iResult;
            //}

            //pbStatus = bSts;

            //return iResult;
        }

        /**
        * DC POWER Error의 상태를 읽는다.
        *
        * @param	*pbStatus : DC PW Error의 상태 (true : ON, false : OFF)
        * @return	Error Code : 0 = SUCCESS, 그외 = Error
*/
        public int GetDcPWErrorStatus(out bool pbStatus)
        {
            pbStatus = false;
            return SUCCESS;

            //bool bSts = false;
            //int iResult = SUCCESS;

            //
            //if (m_IOAddrTable.iDcPowerAddr != 0)
            //{
            //    if ((iResult = m_plnkIO.IsOff(m_IOAddrTable.iDcPowerAddr, out bSts)) != SUCCESS)
            //        return iResult;
            //}

            //*pbStatus = bSts;

            //return iResult;
        }

        /**
        * EFD READY Error의 상태를 읽는다.
        *
        * @param	*pbStatus : EFD Error의 상태 (true : ON, false : OFF)
        * @return	Error Code : 0 = SUCCESS, 그외 = Error
*/
        public int GetEFDErrorStatus(out bool pbStatus)
        {
            pbStatus = false;
            // 2012.03.09 by ranian 여기는 그냥 이렇게 체크 안하도록 남겨둔다
            // 건 사용 여부에 따라서 MCtrlDispenser에서 에러 체크하도록.
            return SUCCESS;

            //bool bSts = false;
            //int iResult = SUCCESS;

            //if (m_IOAddrTable.iEFDReadyS1Addr != 0)
            //{
            //    if ((iResult = m_plnkIO.IsOff(m_IOAddrTable.iEFDReadyS1Addr, out bSts)) != SUCCESS)
            //        return iResult;
            //}
            //if (bSts == false)  // false 이면 비정상이므로 일단 하나만 안되도 안되므로
            //    return iResult;
            //if (m_IOAddrTable.iEFDReadyS2Addr != 0)
            //{
            //    if ((iResult = m_plnkIO.IsOff(m_IOAddrTable.iEFDReadyS2Addr, out bSts)) != SUCCESS)
            //        return iResult;
            //}
            //if (bSts == false)  // false 이면 비정상이므로 일단 하나만 안되도 안되므로
            //    return iResult;

            //if (m_IOAddrTable.iEFDReadyG1Addr != 0)
            //{
            //    if ((iResult = m_plnkIO.IsOff(m_IOAddrTable.iEFDReadyG1Addr, out bSts)) != SUCCESS)
            //        return iResult;
            //}
            //if (bSts == false)  // false 이면 비정상이므로 일단 하나만 안되도 안되므로
            //    return iResult;

            //if (m_IOAddrTable.iEFDReadyG2Addr != 0)
            //{
            //    if ((iResult = m_plnkIO.IsOff(m_IOAddrTable.iEFDReadyG2Addr, out bSts)) != SUCCESS)
            //        return iResult;
            //}
            //*pbStatus = bSts;

            //return iResult;
        }

        /**
        * Vacuum Error의 상태를 읽는다.
        *
        * @param	*pbStatus : Vacuum Error의 상태 (true : ON, false : OFF)
        * @return	Error Code : 0 = SUCCESS, 그외 = Error
*/
        public int GetVacuumErrorStatus(out bool pbStatus)
        {
            pbStatus = false;
            return SUCCESS;

            //bool bSts = false;
            //int iResult = SUCCESS;

            
            //if (m_IOAddrTable.iMainVacuumAddr != 0)
            //{
            //    if ((iResult = m_plnkIO.IsOff(m_IOAddrTable.iMainVacuumAddr, out bSts)) != SUCCESS)
            //        return iResult;
            //}

            //if (bSts == false)  // false 이면 비정상이므로 일단 하나만 안되도 안되므로
            //    return iResult;

            //if (m_IOAddrTable.iSubVacuumAddr != 0)
            //{
            //    if ((iResult = m_plnkIO.IsOff(m_IOAddrTable.iSubVacuumAddr, out bSts)) != SUCCESS)
            //        return iResult;
            //}

            //*pbStatus = bSts;

            //return iResult;
        }

        /**
        * N2 Error의 상태를 읽는다.
        *
        * @param	*pbStatus : N2 Error의 상태 (true : ON, false : OFF)
        * @return	Error Code : 0 = SUCCESS, 그외 = Error
*/
        public int GetN2ErrorStatus(out bool pbStatus)
        {
            bool bSts = false;
            int iResult = SUCCESS;

            
            if (m_IOAddrTable.MainN2Addr != 0)
            {
                //if ((iResult = m_plnkIO.IsOff(m_IOAddrTable.iMainN2Addr, out bSts)) != SUCCESS)
                //    return iResult;
            }

            pbStatus = bSts;

            return iResult;
        }

        /**
        * Cleaner Detect Error의 상태를 읽는다.
        *
        * @param	*pbStatus : Cleaner Detect Error의 상태 (true : ON, false : OFF)
        * @return	Error Code : 0 = SUCCESS, 그외 = Error
*/
        public int GetCleanerDetect1ErrorStatus(out bool pbStatus)
        {
            bool bSts = false;
            int iResult = SUCCESS;

            
            //if (m_IOAddrTable.iCleanerDetect1Addr != 0)
            //{
            //    if ((iResult = m_plnkIO.IsOff(m_IOAddrTable.iCleanerDetect1Addr, out bSts)) != SUCCESS)
            //        return iResult;
            //}

            pbStatus = bSts;

            return iResult;
        }

        /**
        * Cleaner Detect Error의 상태를 읽는다.
        *
        * @param	*pbStatus : Cleaner Detect Error의 상태 (true : ON, false : OFF)
        * @return	Error Code : 0 = SUCCESS, 그외 = Error
*/
        public int GetCleanerDetect2ErrorStatus(out bool pbStatus)
        {
            bool bSts = false;
            int iResult = SUCCESS;

            
            //if (m_IOAddrTable.iCleanerDetect2Addr != 0)
            //{
            //    if ((iResult = m_plnkIO.IsOff(m_IOAddrTable.iCleanerDetect2Addr, out bSts)) != SUCCESS)
            //        return iResult;
            //}

            pbStatus = bSts;

            return iResult;
        }

        /**
        * Motor AMP Fault 상태를 읽는다.
        *
        * @param	*pbStatus : Motor AMP Fault의 상태 (true : Fault, false : Normal)
        * @return	Error Code : 0 = SUCCESS, 그외 = Error
*/
        public int GetMotorAmpFaultStatus(out bool pbStatus)
        {
            int i = 0;
            int iResult = SUCCESS;
            bool bFault = false;

            pbStatus = true;

            //for (i = 0; i < m_JogTable.ListNo; i++)
            //{
            //    if (m_JogTable.MotionArray[i].m_XKey.m_plnkJog != null)
            //        iResult = m_JogTable.MotionArray[i].m_XKey.m_plnkJog.GetAmpFault(m_JogTable.MotionArray[i].m_XKey.AxisIndex, out bFault);
            //    if (bFault == true)
            //    {
            //        m_JogTable.MotionArray[i].m_XKey.m_plnkJog.ResetOrigin(m_JogTable.MotionArray[i].m_XKey.AxisIndex);
            //        return GenerateErrorCode(ERR_OPPANEL_AMP_FAULT);
            //    }

            //    if (m_JogTable.MotionArray[i].m_YKey.m_plnkJog != null)
            //        iResult = m_JogTable.MotionArray[i].m_YKey.m_plnkJog.GetAmpFault(m_JogTable.MotionArray[i].m_YKey.AxisIndex, out bFault);
            //    if (bFault == true)
            //    {
            //        m_JogTable.MotionArray[i].m_YKey.m_plnkJog.ResetOrigin(m_JogTable.MotionArray[i].m_YKey.AxisIndex);
            //        return GenerateErrorCode(ERR_OPPANEL_AMP_FAULT);
            //    }

            //    if (m_JogTable.MotionArray[i].m_TKey.m_plnkJog != null)
            //        iResult = m_JogTable.MotionArray[i].m_TKey.m_plnkJog.GetAmpFault(m_JogTable.MotionArray[i].m_TKey.AxisIndex, out bFault);
            //    if (bFault == true)
            //    {
            //        m_JogTable.MotionArray[i].m_TKey.m_plnkJog.ResetOrigin(m_JogTable.MotionArray[i].m_TKey.AxisIndex);
            //        return GenerateErrorCode(ERR_OPPANEL_AMP_FAULT);
            //    }

            //    if (m_JogTable.MotionArray[i].m_ZKey.m_plnkJog != null)
            //        iResult = m_JogTable.MotionArray[i].m_ZKey.m_plnkJog.GetAmpFault(m_JogTable.MotionArray[i].m_ZKey.AxisIndex, out bFault);
            //    if (bFault == true)
            //    {
            //        m_JogTable.MotionArray[i].m_ZKey.m_plnkJog.ResetOrigin(m_JogTable.MotionArray[i].m_ZKey.AxisIndex);
            //        return GenerateErrorCode(ERR_OPPANEL_AMP_FAULT);
            //    }
            //}

            //*pbStatus = false;

            return iResult;
        }

        /**
         * Motion Power Relay On/Off 상태를 읽는다.
         *
         * @param	*pbStatus : Motion Power Relay의 상태 (true : ON, false : OFF)
         * @return	Error Code : 0 = SUCCESS, 그외 = Error
         */
        public int GetMotionPowerRelayStatus(out bool pbStatus)
        {
            bool bSts1 = true, bSts2 = true;
            int iResult = SUCCESS;
            int iCount = 0;

            
            /*	
                for (i=0; i<DEF_OPPANEL_MAX_MOTION_RELAY_NO; i++)
                {
                    if (m_IOAddrTable.iMotionRelayAddr[i] != 0)
                    {
                        if ((iResult = m_plnkIO.IsOn(m_IOAddrTable.iMotionRelayAddr[i], out bSts1)) != SUCCESS)
                            return iResult;

                        bSts2 = bSts2 && bSts1;
                        iCount++;
                    }
                }

                if (iCount != 0)
                    *pbStatus = bSts2;
                else
                    *pbStatus = false;
            */
            pbStatus = true;
            return iResult;
        }

        /**
         * Motion Power Relay On/Off 를 설정한다.
         *
         * @param	bStatus : Motion Power Relay의 동작 (true : ON, false : OFF)
         * @return	Error Code : 0 = SUCCESS, 그외 = Error
         */
        public int SetMotionPowerRelayStatus(bool bStatus)
        {
            bool bSts1 = true, bSts2 = true;
            int iResult = SUCCESS;
            int iCount = 0;

            
            /*	for (i=0; i<DEF_OPPANEL_MAX_MOTION_RELAY_NO; i++)
                {
                    if (m_IOAddrTable.iMotionRelayAddr[i] != 0)
                    {
                        if (bStatus == true)
                        {
                            if ((iResult = m_plnkIO.OutputOn(m_IOAddrTable.iMotionRelayAddr[i])) != SUCCESS)
                                return iResult;
                        }
                        else
                        {
                            if ((iResult = m_plnkIO.OutputOff(m_IOAddrTable.iMotionRelayAddr[i])) != SUCCESS)
                                return iResult;
                        }
                    }
                }*/

            return iResult;
        }

        /**
         * Motion 이동 속도 Mode를 설정한다.
         *
         * @param	rgdVelocity[] : 설정할 Motion 속도 (배열 Index 순서는 MMC 축 ID 순서)
         */
        public void SetVelocityMode(double[/*DEF_MAX_MOTION_AXIS_NO*/] rgdVelocity)
        {
            //. Motion 속도 수정...
            int i = 0;
            int iResult = SUCCESS;
            bool bFault = false;
            int iAxisID;
            double dVel;
            int iAcc;

            //for (i = 0; i < m_JogTable.ListNo; i++)
            //{
            //    if (m_JogTable.MotionArray[i].m_XKey.m_plnkJog != null)
            //    {
            //        m_JogTable.MotionArray[i].m_XKey.m_plnkJog.GetAxisID(m_JogTable.MotionArray[i].m_XKey.AxisIndex, out iAxisID);
            //        m_JogTable.MotionArray[i].m_XKey.m_plnkJog.GetMovingVelocity(m_JogTable.MotionArray[i].m_XKey.AxisIndex, out dVel, out iAcc);
            //        dVel = rgdVelocity[iAxisID];
            //        iResult = m_JogTable.MotionArray[i].m_XKey.m_plnkJog.SetMovingVelocity(m_JogTable.MotionArray[i].m_XKey.AxisIndex, out dVel, out iAcc);
            //    }
            //    if (m_JogTable.MotionArray[i].m_YKey.m_plnkJog != null)
            //    {
            //        m_JogTable.MotionArray[i].m_YKey.m_plnkJog.GetAxisID(m_JogTable.MotionArray[i].m_YKey.AxisIndex, out iAxisID);
            //        m_JogTable.MotionArray[i].m_YKey.m_plnkJog.GetMovingVelocity(m_JogTable.MotionArray[i].m_YKey.AxisIndex, out dVel, out iAcc);
            //        dVel = rgdVelocity[iAxisID];
            //        iResult = m_JogTable.MotionArray[i].m_YKey.m_plnkJog.SetMovingVelocity(m_JogTable.MotionArray[i].m_YKey.AxisIndex, out dVel, out iAcc);
            //    }
            //    if (m_JogTable.MotionArray[i].m_TKey.m_plnkJog != null)
            //    {
            //        m_JogTable.MotionArray[i].m_TKey.m_plnkJog.GetAxisID(m_JogTable.MotionArray[i].m_TKey.AxisIndex, out iAxisID);
            //        m_JogTable.MotionArray[i].m_TKey.m_plnkJog.GetMovingVelocity(m_JogTable.MotionArray[i].m_TKey.AxisIndex, out dVel, out iAcc);
            //        dVel = rgdVelocity[iAxisID];
            //        iResult = m_JogTable.MotionArray[i].m_TKey.m_plnkJog.SetMovingVelocity(m_JogTable.MotionArray[i].m_TKey.AxisIndex, out dVel, out iAcc);
            //    }
            //    if (m_JogTable.MotionArray[i].m_ZKey.m_plnkJog != null)
            //    {
            //        m_JogTable.MotionArray[i].m_ZKey.m_plnkJog.GetAxisID(m_JogTable.MotionArray[i].m_ZKey.AxisIndex, out iAxisID);
            //        m_JogTable.MotionArray[i].m_ZKey.m_plnkJog.GetMovingVelocity(m_JogTable.MotionArray[i].m_ZKey.AxisIndex, out dVel, out iAcc);
            //        dVel = rgdVelocity[iAxisID];
            //        iResult = m_JogTable.MotionArray[i].m_ZKey.m_plnkJog.SetMovingVelocity(m_JogTable.MotionArray[i].m_ZKey.AxisIndex, out dVel, out iAcc);
            //    }
            //}
        }

        /**
         * Door Sensor 점검여부를 설정한다.
         *
         * @param	bFlag : 점검 여부 (true:점검, false:무시)
         * @param	iGroup : (OPTION=-1) Door Sensor Group 번호 (-1이면 모든 Group내 설정)
         * @param	iIndex : (OPTION=-1) Door Snesor Group 내 Index 번호 (-1이면 Group내 모든 Index 설정)
         */
        public void SetDoorCheckFlag(bool bFlag, int iGroup, int iIndex)
        {
            int i, j;
            int iResult = SUCCESS;

            //if (iGroup == -1)
            //{
            //    for (i = 0; i < DEF_OPPANEL_MAX_DOOR_GROUP; i++)
            //    {
            //        if (iIndex == -1)
            //        {
            //            for (j = 0; j < DEF_OPPANEL_MAX_DOOR_SENSOR; j++)
            //            {
                            
            //                if (m_IOAddrTable.iSafeDoorAddr[i][j] != 0)
            //                    m_IOAddrTable.rgbSafeDoorFlag[i][j] = bFlag;
            //                // Sensor Address가 할당되어 있지 않으면 확인 중단
            //                else
            //                    j = DEF_OPPANEL_MAX_DOOR_SENSOR;
            //            }
            //        }
            //        else
            //        {
                        
            //            if (m_IOAddrTable.iSafeDoorAddr[i][iIndex] != 0)
            //                m_IOAddrTable.rgbSafeDoorFlag[i][iIndex] = bFlag;
            //        }
            //    }
            //}
            //else
            //{
            //    if (iIndex == -1)
            //    {
            //        for (j = 0; j < DEF_OPPANEL_MAX_DOOR_SENSOR; j++)
            //        {
                        
            //            if (m_IOAddrTable.iSafeDoorAddr[iGroup][j] != 0)
            //                m_IOAddrTable.rgbSafeDoorFlag[iGroup][j] = bFlag;
            //            // Sensor Address가 할당되어 있지 않으면 확인 중단
            //            else
            //                j = DEF_OPPANEL_MAX_DOOR_SENSOR;
            //        }
            //    }
            //    else
            //    {
                    
            //        if (m_IOAddrTable.iSafeDoorAddr[iGroup][iIndex] != 0)
            //            m_IOAddrTable.rgbSafeDoorFlag[iGroup][iIndex] = bFlag;
            //    }
            //}
        }

        public int SetStartLamp(bool bStatus)
        {
            return setPanelLedStatus("Start LED", m_IOAddrTable.FrontPanel.RunOutputAddr, m_IOAddrTable.BackPanel.RunOutputAddr, bStatus);
        }

        public int SetStopLamp(bool bStatus)
        {
            return setPanelLedStatus("Stop LED", m_IOAddrTable.FrontPanel.StopOutputAddr, m_IOAddrTable.BackPanel.StopOutputAddr, bStatus);
        }

        public int SetResetLamp(bool bStatus)
        {
            return setPanelLedStatus("Reset LED", m_IOAddrTable.FrontPanel.ResetOutputAddr, m_IOAddrTable.BackPanel.ResetOutputAddr, bStatus);
        }

        public int SetTowerRedLamp(bool bStatus)
        {
            return setTowerLampStatus("RED Lamp", m_IOAddrTable.TowerLamp.RedLampAddr, bStatus);
        }

        public int SetTowerYellowLamp(bool bStatus)
        {
            return setTowerLampStatus("YELLOW Lamp", m_IOAddrTable.TowerLamp.YellowLampAddr, bStatus);
        }

        public int SetTowerGreenLamp(bool bStatus)
        {
            return setTowerLampStatus("GREEN Lamp", m_IOAddrTable.TowerLamp.GreenLampAddr, bStatus);
        }

        public int SetBuzzerStatus(int iMode, bool bStatus)
        {
            int i = 0;
            int iResult = SUCCESS;

            // Tower Lamp의 Buzzer에 대한 전체 출력 모드가 선택된 경우
            if (iMode == DEF_OPPANEL_BUZZER_ALL)
            {
                for (i = 0; i < DEF_OPPANEL_MAX_BUZZER_MODE; i++)
                {
                    if ((iResult = setTowerLampStatus("Buzzer", m_IOAddrTable.TowerLamp.BuzzerArray[i], bStatus)) != SUCCESS)
                        return iResult;
                }
            }
            // Tower Lamp의 Buzzer에 대한 하나의 출력 모드가 선택된 경우 
            else
            {
                if ((iResult = setTowerLampStatus("Buzzer", m_IOAddrTable.TowerLamp.BuzzerArray[iMode], bStatus)) != SUCCESS)
                    return iResult;
            }

            return iResult;
        }

        public int GetEnabledOpPanelID(out int piOpPanelID)
        {
            int iResult = SUCCESS;
            bool bStatus = false;
            string strLogMessage;

            piOpPanelID = DEF_OPPANEL_NONE_PANEL_ID;
            //if ((iResult = m_plnkIO.IsOn(m_IOAddrTable.iTouchSelectAddr, out bStatus)) != SUCCESS)
            //{
            //    // 오류 동작 Log
            //    strLogMessage.Format("Op Panel ID의 상태를 읽는데 실패했습니다.");
            //    m_plogMng.WriteLog(DEF_MLOG_ERROR_LOG_LEVEL, strLogMessage, __FILE__, __LINE__);

            //    piOpPanelID = DEF_OPPANEL_NONE_PANEL_ID;

            //    return iResult;
            //}
            //else
            //{
            //    // IO가 true이면 앞 Touch Panel
            //    if (bStatus == true)
            //        *piOpPanelID = DEF_OPPANEL_FRONT_PANEL_ID;
            //    // IO가 false이면 뒷 Touch Panel
            //    else
            //        *piOpPanelID = DEF_OPPANEL_BACK_PANEL_ID;
            //}

            return SUCCESS;
        }

        public int ChangeOpPanel(int iOpPanelID)
        {
            int iResult = SUCCESS;
            string strLogMessage;
            int iCurOpPanelID;

            // 현재 활성화되어 있는 Touch Panel을 알아온다.
            if ((iResult = GetEnabledOpPanelID(out iCurOpPanelID)) != SUCCESS)
                return iResult;

            // 전환할 Touch Panel이 현재 활성화된 Touch Panel과 일치하면 Pass
            if (iOpPanelID == iCurOpPanelID)
                return SUCCESS;

            //if (iOpPanelID == DEF_OPPANEL_FRONT_PANEL_ID)
            //{
            //    if ((iResult = m_plnkIO.OutputOn(m_IOAddrTable.TouchSelectAddr)) != SUCCESS)
            //    {
            //        // 오류 동작 Log
            //        strLogMessage.Format("앞면으로 Touch Panel 사용권 전환에 실패했습니다.");
            //        m_plogMng.WriteLog(DEF_MLOG_ERROR_LOG_LEVEL, strLogMessage, __FILE__, __LINE__);

            //        return iResult;
            //    }

            //    // 정상 동작 Log
            //    strLogMessage.Format("앞면으로 Touch Panel 사용권 전환하였습니다.");
            //    //		m_plogMng.WriteLog(strLogMessage, __FILE__, __LINE__);
            //}
            //else if (iOpPanelID == DEF_OPPANEL_BACK_PANEL_ID)
            //{
            //    if ((iResult = m_plnkIO.OutputOff(m_IOAddrTable.iTouchSelectAddr)) != SUCCESS)
            //    {
            //        // 오류 동작 Log
            //        strLogMessage.Format("뒷면으로 Touch Panel 사용권 전환에 실패했습니다.");
            //        m_plogMng.WriteLog(DEF_MLOG_ERROR_LOG_LEVEL, strLogMessage, __FILE__, __LINE__);

            //        return iResult;
            //    }

            //    // 정상 동작 Log
            //    strLogMessage.Format("뒷면으로 Touch Panel 사용권 전환하였습니다.");
            //    //		m_plogMng.WriteLog(strLogMessage, __FILE__, __LINE__);
            //}

            return SUCCESS;
        }

        public int GetOpPanelSelectSW(out int piStatus)
        {
            int iResult = SUCCESS;
            string strLogMessage;
            bool bStatus1, bStatus2, bStatus3, bStatus4;

            piStatus = DEF_OPPANEL_NONE_PANEL_ID;

            //if ((iResult = GetJogXMinusButtonStatus(out bStatus1)) != SUCCESS)
            //{
            //    // 오류 동작 Log
            //    //strLogMessage.Format("Touch Panel Select Switch(X-) 상태 읽기에 실패했습니다.");
            //    //m_plogMng.WriteLog(DEF_MLOG_ERROR_LOG_LEVEL, strLogMessage, __FILE__, __LINE__);

            //    piStatus = DEF_OPPANEL_NONE_PANEL_ID;

            //    return iResult;
            //}

            //if ((iResult = GetJogYPlusButtonStatus(out bStatus2)) != SUCCESS)
            //{
            //    // 오류 동작 Log
            //    strLogMessage.Format("Touch Panel Select Switch(Y+) 상태 읽기에 실패했습니다.");
            //    m_plogMng.WriteLog(DEF_MLOG_ERROR_LOG_LEVEL, strLogMessage, __FILE__, __LINE__);

            //    piStatus = DEF_OPPANEL_NONE_PANEL_ID;

            //    return iResult;
            //}

            //if ((iResult = GetJogXPlusButtonStatus(out bStatus3)) != SUCCESS)
            //{
            //    // 오류 동작 Log
            //    strLogMessage.Format("Touch Panel Select Switch(X+) 상태 읽기에 실패했습니다.");
            //    m_plogMng.WriteLog(DEF_MLOG_ERROR_LOG_LEVEL, strLogMessage, __FILE__, __LINE__);

            //    piStatus = DEF_OPPANEL_NONE_PANEL_ID;

            //    return iResult;
            //}

            //if ((iResult = GetJogYMinusButtonStatus(out bStatus4)) != SUCCESS)
            //{
            //    // 오류 동작 Log
            //    strLogMessage.Format("Touch Panel Select Switch(Y-) 상태 읽기에 실패했습니다.");
            //    m_plogMng.WriteLog(DEF_MLOG_ERROR_LOG_LEVEL, strLogMessage, __FILE__, __LINE__);

            //    piStatus = DEF_OPPANEL_NONE_PANEL_ID;

            //    return iResult;
            //}

            //// X(-)와 Y(+)가 동시에 눌려졌으면 - 앞면으로 전환 
            //if ((bStatus1 && bStatus2 && bStatus3 && bStatus4) == true)
            //{
            //    *piStatus = DEF_OPPANEL_NONE_PANEL_ID;

            //    // 정상 동작 Log
            //    strLogMessage.Format("Touch Panel 전환 Switch가 전부 눌렸습니다.");
            //    //		m_plogMng.WriteLog(strLogMessage, __FILE__, __LINE__);

            //    return SUCCESS;
            //}
            //// X(-)와 Y(+)가 동시에 눌려졌으면 - 앞면으로 전환 
            //else if ((bStatus1 && bStatus2) == true)
            //{
            //    *piStatus = DEF_OPPANEL_FRONT_PANEL_ID;

            //    // 정상 동작 Log
            //    strLogMessage.Format("앞면 Touch Panel 전환 Switch가 눌렸습니다.");
            //    //		m_plogMng.WriteLog(strLogMessage, __FILE__, __LINE__);

            //    return SUCCESS;
            //}
            //// X(+)와 Y(-)가 동시에 눌려졌으면 - 뒷면으로 전환 
            //else if ((bStatus3 && bStatus4) == true)
            //{
            //    *piStatus = DEF_OPPANEL_BACK_PANEL_ID;

            //    // 정상 동작 Log
            //    strLogMessage.Format("뒷면 Touch Panel 전환 Switch가 눌렸습니다.");
            //    //		m_plogMng.WriteLog(strLogMessage, __FILE__, __LINE__);

            //    return SUCCESS;
            //}
            //// 해당 사항 없을 때 
            //else
            //{
            //    *piStatus = DEF_OPPANEL_NONE_PANEL_ID;

            //    return SUCCESS;
            //}

            return SUCCESS;
        }

        /**
        * 앞, 뒷 Panel의 특정 Switch의 눌려진 상태를 확인한다.
        *
        * @param	strLogName : Log할 때 사용할 Switch 이름
        * @param	iFrontSWAddr : 앞 Panel의 Switch IO Address
        * @param	iBackSWAddr : 뒷 Panel의 Switch IO Address
        * @param	*pbStatus : 앞, 뒷 Panel의 Switch 눌려진 상태 (둘 중 하나라도 눌리면 true)
        * @return	Error Code : 0 = SUCCESS, 그외 = Error
*/
        public int getPanelSwitchStatus(string strLogName, int iFrontSWAddr, int iBackSWAddr, out bool pbStatus)
        {
            int iResult = SUCCESS;
            bool bFrontStatus = false;
            bool bBackStatus = false;
            string strLogMessage;

            //// 앞 Panel의 Button의 상태 읽기 
            //if ((iResult = m_plnkIO.IsOn(iFrontSWAddr, out bFrontStatus)) != SUCCESS)
            //{
            //    // 오류 동작 Log 
            //    strLogMessage.Format("앞 Panel의 %s의 상태를 읽는데 실패했습니다.", strLogName);
            //    m_plogMng.WriteLog(DEF_MLOG_ERROR_LOG_LEVEL, strLogMessage, __FILE__, __LINE__);

            //    return iResult;
            //}

            //// 뒷 Panel의 Button의 상태 읽기 
            //if ((iResult = m_plnkIO.IsOn(iBackSWAddr, out bBackStatus)) != SUCCESS)
            //{
            //    // 오류 동작 Log 
            //    strLogMessage.Format("뒷 Panel의 %s의 상태를 읽는데 실패했습니다.", strLogName);
            //    m_plogMng.WriteLog(DEF_MLOG_ERROR_LOG_LEVEL, strLogMessage, __FILE__, __LINE__);

            //    return iResult;
            //}

            pbStatus = bFrontStatus || bBackStatus;

            return SUCCESS;
        }

        /**
        * 앞, 뒷 Panel의 특정 LED의 동작을 설정한다.
        *
        * @param	strLogName : Log할 때 사용할 LED 이름
        * @param	iFrontSWAddr : 앞 Panel의 LED IO Address
        * @param	iBackSWAddr : 뒷 Panel의 LED IO Address
        * @param	bStatus : 설정할 앞, 뒷 Panel의 LED 동작상태 (앞, 뒷면 둘다 동작)
        * @return	Error Code : 0 = SUCCESS, 그외 = Error
*/
        public int setPanelLedStatus(string strLogName, int iFrontLedAddr, int iBackLedAddr, bool bStatus)
        {
            int iResult = SUCCESS;
            string strLogMessage;

            //if (bStatus == true)
            //{
            //    if ((iResult = m_plnkIO.OutputOn(iFrontLedAddr)) != SUCCESS)
            //    {
            //        // 오류 동작 Log
            //        strLogMessage.Format("앞 Panel의 %s를 ON하는데 실패했습니다.", strLogName);
            //        m_plogMng.WriteLog(DEF_MLOG_ERROR_LOG_LEVEL, strLogMessage, __FILE__, __LINE__);

            //        return iResult;
            //    }
            //    // 정상 동작 Log
            //    //.		strLogMessage.Format("앞 Panel의 %s를 ON하였습니다.", strLogName);
            //    //.		m_plogMng.WriteLog(strLogMessage, __FILE__, __LINE__);

            //    if ((iResult = m_plnkIO.OutputOn(iBackLedAddr)) != SUCCESS)
            //    {
            //        // 오류 동작 Log
            //        strLogMessage.Format("뒷 Panel의 %s를 ON하는데 실패했습니다.", strLogName);
            //        m_plogMng.WriteLog(DEF_MLOG_ERROR_LOG_LEVEL, strLogMessage, __FILE__, __LINE__);

            //        return iResult;
            //    }
            //    // 정상 동작 Log
            //    //.		strLogMessage.Format("뒷 Panel의 %s를 ON하였습니다.", strLogName);
            //    //.		m_plogMng.WriteLog(strLogMessage, __FILE__, __LINE__);
            //}
            //else
            //{
            //    if ((iResult = m_plnkIO.OutputOff(iFrontLedAddr)) != SUCCESS)
            //    {
            //        // 오류 동작 Log
            //        strLogMessage.Format("앞 Panel의 %s를 OFF하는데 실패했습니다.", strLogName);
            //        m_plogMng.WriteLog(DEF_MLOG_ERROR_LOG_LEVEL, strLogMessage, __FILE__, __LINE__);

            //        return iResult;
            //    }
            //    // 정상 동작 Log
            //    //.		strLogMessage.Format("앞 Panel의 %s를 OFF하였습니다.", strLogName);
            //    //.		m_plogMng.WriteLog(strLogMessage, __FILE__, __LINE__);

            //    if ((iResult = m_plnkIO.OutputOff(iBackLedAddr)) != SUCCESS)
            //    {
            //        // 오류 동작 Log
            //        strLogMessage.Format("뒷 Panel의 %s를 OFF하는데 실패했습니다.", strLogName);
            //        m_plogMng.WriteLog(DEF_MLOG_ERROR_LOG_LEVEL, strLogMessage, __FILE__, __LINE__);

            //        return iResult;
            //    }
            //    // 정상 동작 Log
            //    //.		strLogMessage.Format("뒷 Panel의 %s를 OFF하였습니다.", strLogName);
            //    //.		m_plogMng.WriteLog(strLogMessage, __FILE__, __LINE__);
            //}

            return SUCCESS;
        }

        /**
        * Tower Lamp의 Lamp, Buzzer의 동작을 설정한다.
        *
        * @param	strLogName : Log할 때 사용할 Tower Lamp 동작 요소 이름
        * @param	iTowerAddr : 앞 Panel의 LED IO Address
        * @param	bStatus : 설정할 Tower Lamp의 Lamp, Buzzer의 동작상태
        * @return	Error Code : 0 = SUCCESS, 그외 = Error
*/
        public int setTowerLampStatus(string strLogName, int iTowerAddr, bool bStatus)
        {
            int iResult = SUCCESS;
            string strLogMessage;

            //if (bStatus == true)
            //{
            //    if ((iResult = m_plnkIO.OutputOn(iTowerAddr)) != SUCCESS)
            //    {
            //        // 오류 동작 Log
            //        strLogMessage.Format("Tower Lamp의 %s를 ON하는데 실패했습니다.", strLogName);
            //        m_plogMng.WriteLog(DEF_MLOG_ERROR_LOG_LEVEL, strLogMessage, __FILE__, __LINE__);

            //        return iResult;
            //    }
            //}
            //else
            //{
            //    if ((iResult = m_plnkIO.OutputOff(iTowerAddr)) != SUCCESS)
            //    {
            //        // 오류 동작 Log
            //        strLogMessage.Format("Tower Lamp의 %s를 OFF하는데 실패했습니다.", strLogName);
            //        m_plogMng.WriteLog(DEF_MLOG_ERROR_LOG_LEVEL, strLogMessage, __FILE__, __LINE__);

            //        return iResult;
            //    }
            //}

            return SUCCESS;
        }

        /**
        * System의 Auto / Manual Mode를 반영한다.
        * Mode 변환이 생길때마다 TrsAutoManager에 의해 각 Control들의 Mode에 반영한다.
        *
        * @param	EAutoManual eAutoManual (반영하고자 하는 Auto/Manual Mode)
        * @return	void
*/
        public void SetAutoManual(EOperationMode eAutoManual)
        {
            m_eAutoManual = eAutoManual;
        }

        /**
        * System의 운전 Mode를 반영한다.
        * 화면에서 운전 Mode 변경 시 각 Control들의 운전 Mode에 반영한다.
        *
        * @param	ERunMode eOpMode (반영하고자 하는 운전 Mode)
        * @return	void
*/
        public void SetOpMode(ERunMode eOpMode)
        {
            m_eOpMode = eOpMode;
        }

        public int CheckAllHead_Tank_Empty(out bool pbEmptyAll, out bool pbEmptyPart)
        {
            int iResult = SUCCESS;
            bool bStatus1 = false;
            bool bStatus2 = false;

            pbEmptyAll = false;
            pbEmptyPart = false;

            int i = 0;
            for (i = 0; i < DEF_MAX_HEAD_NO; i++)
            {
                iResult = CheckHead_Tank_Empty(i, out bStatus1, out bStatus2);
                if (iResult != SUCCESS) return iResult;

                if (bStatus1 == true) pbEmptyAll = true;
                if (bStatus2 == true) pbEmptyPart = true;

            }

            return SUCCESS;
        }

        public int CheckHead_Tank_Empty(int nHeadNo, out bool pbEmptyAll, out bool pbEmptyPart)
        {
            int iResult = SUCCESS;

            bool[] bStatus = new bool[DEF_MAX_TANK_NO];

            pbEmptyAll = false;
            pbEmptyPart = false;
            bool bInit = false;

            //int i = 0;
            //for (i = 0; i < DEF_MAX_TANK_NO; i++)
            //{
            //    if (m_Data.m_bUseMaterialAlarmHead[nHeadNo][i] == false) continue;

            //    iResult = m_plnkIO.IsOn(m_Data.m_nTankEmpty[nHeadNo][i], out bStatus[i]);
            //    if (iResult != SUCCESS) return iResult;

            //    if (bInit == false)
            //    {
            //        bInit = true;
            //        *pbEmptyAll = true;
            //    }

            //    if (bStatus[i] == true) *pbEmptyPart = true;

            //    if (bStatus[i] == false) *pbEmptyAll = false;
            //}

            return SUCCESS;
        }

        public int GetAreaFrontBackStatus(out bool pbStatus)
        {
            int iResult = SUCCESS;

            //bool bStatus;

            //iResult = m_plnkIO.IsOn(iDoor_Front, out bStatus);
            //if (bStatus == false)
            //{
            //    *pbStatus = false;
            //    return iResult;
            //}

            //iResult = m_plnkIO.IsOn(iDoor_Side, out bStatus);
            //if (bStatus == false)
            //{
            //    *pbStatus = false;
            //    return iResult;
            //}

            //iResult = m_plnkIO.IsOn(iDoor_Back, out bStatus);
            //if (bStatus == false)
            //{
            //    *pbStatus = false;
            //    return iResult;
            //}

            pbStatus = true;
            return iResult;
        }

    }
}
