using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Diagnostics;

using static LWDicer.Control.DEF_Thread;
using static LWDicer.Control.DEF_Thread.EThreadMessage;
using static LWDicer.Control.DEF_Thread.EWindowMessage;
using static LWDicer.Control.DEF_Thread.ERunMode;
using static LWDicer.Control.DEF_Error;
using static LWDicer.Control.DEF_Common;

namespace LWDicer.Control
{
    public class CTrsAutoManagerRefComp
    {
        public MCtrlLoader ctrlLoader;

        public MTrsLoader trsLoader;
        public MTrsPushPull trsPushPull;

        //public CTrsAutoManagerRefComp(MCtrlLoader ctrlLoader)
        //{
        //    this.ctrlLoader = ctrlLoader;
        //}
        public override string ToString()
        {
            return $"CTrsAutoManagerRefComp : {ctrlLoader}";
        }
    }

    public class CTrsAutoManagerData
    {
    }

    public class MTrsAutoManager : MWorkerThread
    {
        private CTrsAutoManagerRefComp m_RefComp;
        private CTrsAutoManagerData m_Data;

        // Mode
        // Part Empty - Exchange Mode
        bool m_bExchangeMode;

        // Buzzer On Mode
        bool m_bBuzzerMode;

        // Line controller Op call Mode
        bool m_bOpCallMode;

        // Error Display를 위한 Mode
        bool m_bErrDispMode;

        // Thread가 해당 상태로 전환했는지 확인하기 위한 테이블
        //  Thread에게 명령을 내려 보내기전에 Clear 한다. 
        int[] m_ThreadStatusArray = new int[MAX_THREAD_CHANNEL];

        // switch status
        bool m_bStartPressed;
        bool m_bStepStopPressed;
        bool m_bResetPressed;
        bool m_bEStopPressed;
        bool m_bDoorOpened;

        // Run 도중에 Error가 이미 감지됐음
        bool m_bIsErrorDetected_WhenRun;

        // Alarm Logging을 위한 Path 지정
        string m_strLogPath;
        // Alarm 정보를 읽어오기 위한 Path 지정
        string m_strDataPath;

        // Tower Lamp 상태
        int m_iTowerLampSts;

        // Alram 처리 상태
        bool m_bAlarmProcFlag;

        MTickTimer m_ResetTimer = new MTickTimer();
        MTickTimer m_NoPanelTimer = new MTickTimer();
        MTickTimer m_DeivceIDCheck = new MTickTimer(); //NSMC

        int m_iOldEqState;
        int m_iOldEqProcessState;
        bool m_bInitState;          // 원점잡기나, 초기화 실행하고 있는 상태이면 TRUE
        bool m_bLC_PM_Mode;         // LC에서 PM 명령이 온 상태면 TRUE
        bool m_bLC_NORMAL_Mode;     // LC에서 PM 명령이 와서 실행된 후 Normal 명령이 왔을 때 TRUE
        bool m_bCurrent_PM_Mode;        // 현재 PM 모드 상태인지
        string m_strPM_Code;
        bool m_bFaultState;//1208

        // BUFFER PANEL LOADING
        bool m_bStage_PanelLoading; // 런중 버퍼 판넬 로딩시 안전신호 온

        // UNLOAD HANDLER PANEL LOADING
        bool m_bUnloadHandler_PanelUnloading;   // Unload Handler가 Panel을 Unloading중인가를 나타내는 Flag

        bool m_bPanelExist_InFacility;  // 현재 설비에 Panel이 존재함.
        bool m_bNoPanel_TowerLamp_Flag; // 현재 설비에 Panel이 없음.
        bool m_bTimerStarFalg;

        // Clean System
        bool m_bForcedPumpingJob;       // 강제적으로 Pumping Job 실행, while(true){do always;}

	    bool m_DoingOriginReturn;	// 원점 복귀중인지의 flag

        // Message 변수
        bool m_bPushPull_RequestUnloading;
        bool m_bPushPull_StartLoading;
        bool m_bPushPull_CompleteLoading;
        bool m_bPushPull_RequestLoading;
        bool m_bPushPull_StartUnloading;
        bool m_bPushPull_CompleteUnloading;

        bool m_bAuto_RequestLoadCassette;
        bool m_bAuto_RequestUnloadCassette;

        bool m_bSupplyCassette = false;
        bool m_bSupplyWafer = false;


        public MTrsAutoManager(CObjectInfo objInfo, int selfChannel,
            CTrsAutoManagerRefComp refComp, CTrsAutoManagerData data)
            : base(objInfo, selfChannel)
        {
            m_RefComp = refComp;
            SetData(data);
        }

        public int SetData(CTrsAutoManagerData source)
        {
            m_Data = ObjectExtensions.Copy(source);
            return SUCCESS;
        }

        public int GetData(out CTrsAutoManagerData target)
        {
            target = ObjectExtensions.Copy(m_Data);

            return SUCCESS;
        }

        int InitializeAllThread()
        {
            int iResult = SUCCESS;

            iResult = m_RefComp.trsLoader.Initialize();
            if (iResult != SUCCESS) return iResult;
            //m_RefComp.m_pOpPanel.SetInitFlag(INIT_UNIT_LOADER, true);


            return iResult;
        }

        public int InitializeMsg()
        {
            m_bPushPull_RequestUnloading = false;
            m_bPushPull_StartLoading = false;
            m_bPushPull_CompleteLoading = false;
            m_bPushPull_RequestLoading = false;
            m_bPushPull_StartUnloading = false;
            m_bPushPull_CompleteUnloading = false;

            m_bAuto_RequestLoadCassette = false;
            m_bAuto_RequestUnloadCassette = false;

            return SUCCESS;
        }

        private int InitializeInterface()
        {

            return SUCCESS;
        }

        protected override int ProcessMsg(MEvent evnt)
        {
            Debug.WriteLine($"{ToString()} received message : {evnt}");
            switch (evnt.Msg)
            {
                case (int)MSG_MANUAL_CMD:
                    SetRunStatus(STS_MANUAL);

                    PostMsg(TrsAutoManager, (int)MSG_MANUAL_CNF);
                    break;

                case (int)MSG_MANUAL_CNF:
                    SetThreadStatus(evnt.Sender, STS_MANUAL); // 메세지를 보낸 Thread를 STS_MANUAL 상태로 놓는다.
                    if (CheckAllThreadStatus(STS_MANUAL))       // 모든 Thread가 STS_MANUAL 상태인지 확인한다.
                    {
                        SetSystemStatus(STS_MANUAL);
                        m_bExchangeMode = false;
                        m_bErrDispMode = false;
                        m_bBuzzerMode = false;
                        // m_bAlarmProcFlag = false;

                        //setVelocityMode(VELOCITY_MODE_SLOW);	// Manual일 때 느린 속도로 이동

                        SendMessageToMainWnd((int)WM_START_MANUAL_MSG);

                        //m_RefComp.m_pManageOpPanel->SetAutoManual(OPERATION_MANUAL);
                    }
                    break;


                case (int)MSG_START_RUN_CMD:

                    OnStartRun();

                    PostMsg(TrsAutoManager, (int)MSG_START_RUN_CNF);
                    break;

                case (int)MSG_START_CMD:
                    if (RunStatus == STS_RUN_READY || RunStatus == STS_STEP_STOP ||
                        RunStatus == STS_ERROR_STOP)
                    {
                        SetRunStatus(STS_RUN);

                        PostMsg(TrsAutoManager, (int)MSG_START_CNF);
                    }
                    break;

                case (int)MSG_ERROR_STOP_CMD:
                    SetRunStatus(STS_ERROR_STOP);

                    PostMsg(TrsAutoManager, (int)MSG_ERROR_STOP_CNF);
                    break;

                case (int)MSG_STEP_STOP_CMD:
                    if (RunStatus == STS_STEP_STOP || RunStatus == STS_ERROR_STOP)
                    {
                        SetRunStatus(STS_MANUAL);
                    }
                    else
                    {
                        SetRunStatus(STS_STEP_STOP);
                    }

                    PostMsg(TrsAutoManager, (int)MSG_STEP_STOP_CNF);
                    break;

                case (int)MSG_CYCLE_STOP_CMD:
                    SetRunStatus(STS_CYCLE_STOP);
                    PostMsg(TrsAutoManager, (int)MSG_CYCLE_STOP_CNF);
                    break;

                case (int)MSG_PROCESS_ALARM:
                    //if (AfxGetApp()->GetMainWnd() != NULL)
                    //{
                    //    if (((CMainFrame*)AfxGetApp()->GetMainWnd())->m_pErrorDlg == NULL)
                    //        return processAlarm(evnt);  // process에서 올라온 알람메세지의 처리
                    //}
                    break;

                case (int)MSG_START_CASSETTE_SUPPLY:
                    m_bSupplyCassette = true;
                    break;

                case (int)MSG_STOP_CASSETTE_SUPPLY:
                    m_bSupplyCassette = false;
                    break;

                case (int)MSG_START_WAFER_SUPPLY:
                    m_bSupplyWafer = true;
                    break;

                case (int)MSG_STOP_WAFER_SUPPLY:
                    m_bSupplyWafer = false;
                    break;

            }
            return 0;
        }

        public override void ThreadProcess()
        {
            int iResult = SUCCESS;
            bool bState = false;

            while (true)
            {
                // if thread has been suspended
                if (IsAlive == false)
                {
                    Sleep(ThreadSuspendedTime);
                    continue;
                }

                // check message from other thread
                CheckMsg(1);

                switch (RunStatus)
                {
                    case STS_MANUAL: // Manual Mode
                        //m_RefComp.ctrlAutoManager.SetAutoManual(OPERATION_MANUAL);
                        break;

                    case STS_ERROR_STOP: // Error Stop
                        break;

                    case STS_STEP_STOP: // Step Stop
                        break;

                    case STS_RUN_READY: // Run Ready
                        break;

                    case STS_CYCLE_STOP: // Cycle Stop
                        //if (ThreadStep == TRS_AUTOMANAGER_MOVETO_LOAD)
                        break;

                    case STS_RUN: // auto run
                        //m_RefComp.ctrlAutoManager.SetAutoManual(OPERATION_AUTO);

                        switch (ThreadStep)
                        {
                            default:
                                break;
                        }
                        break;
                }

                Sleep(ThreadSleepTime);
                //Debug.WriteLine(ToString() + " Thread running..");
            }

        }

        void SetThreadStatus(int iIndex, int iStatus)
        {
            m_ThreadStatusArray[iIndex] = iStatus;
        }

        void ClearAllThreadStatus()
        {
            for (int iIndex = 1; iIndex <= GetThreadsCount(); iIndex++)
            {
                m_ThreadStatusArray[iIndex] = -1;
            }
        }

        bool CheckAllThreadStatus(int iStatus)
        {
            for (int iIndex = 1; iIndex <= GetThreadsCount() ; iIndex++)
            {
                if (iIndex == TrsAutoManager) continue;

                if (m_ThreadStatusArray[iIndex] != iStatus)
                {
                    return false;
                }
            }

            return true;
        }

        void SetSystemStatus(int iStatus)
        {
            if (SetRunStatus(iStatus) == false) return;

            bool bStatus;

            if (RunStatus == STS_RUN)
            {
                // 설비가 Live 상태임을 알리는 oUpper_Alive 신호는 On
                //m_RefComp.m_pC_InterfaceCtrl->SendInterfaceOnMsg(PRE_EQ, oUpper_Alive);
                //m_RefComp.m_pC_InterfaceCtrl->SendInterfaceOnMsg(NEXT_EQ, oLower_Alive);
            }
            else
            {
                if (RunStatus_Old == STS_RUN)
                {
                    //// 설비가 Live 상태임을 알리는 oUpper_Alive 신호는 On
                    //m_RefComp.m_pC_InterfaceCtrl->SendInterfaceOffMsg(PRE_EQ, oUpper_Alive);
                    //m_RefComp.m_pC_InterfaceCtrl->SendInterfaceOffMsg(NEXT_EQ, oLower_Alive);

                    ////kshong Door Interlock
                    //m_RefComp.m_pManageOpPanel->GetDoorSWStatus(&bStatus);
                    //if (bStatus)
                    //{
                    //    m_RefComp.m_pC_InterfaceCtrl->SendInterfaceOffMsg(PRE_EQ, oUpper_SI_DoorOpen); //B접
                    //    m_RefComp.m_pC_InterfaceCtrl->SendInterfaceOffMsg(NEXT_EQ, oLower_SI_DoorOpen); //B접
                    //}
                    //else
                    //{
                    //    m_RefComp.m_pC_InterfaceCtrl->SendInterfaceOnMsg(PRE_EQ, oUpper_SI_DoorOpen); //B접
                    //    m_RefComp.m_pC_InterfaceCtrl->SendInterfaceOnMsg(NEXT_EQ, oLower_SI_DoorOpen); //B접
                    //}
                }
            }
        }

        void SendMsg_To_MainWindow(int nMsg, int wParam = 0, int lParam = 0)
        {
            //// 2010.09.29 by ranian
            //// EqState를 메세지 처리하면서 자원낭비로 계속 들어오는것보다 차라리 ontimer에서하도록 함
            //if (nMsg == WM_DISP_EQ_STATE || nMsg == WM_DISP_EQ_PROC_STATE)
            //    return;

            //if (AfxGetApp()->GetMainWnd() != NULL)
            //    SendMessage(AfxGetApp()->GetMainWnd()->GetSafeHwnd(), nMsg, wParam, lParam);
        }

    }
}
