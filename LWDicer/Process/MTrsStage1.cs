using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Diagnostics;

using static LWDicer.Control.DEF_Thread;
using static LWDicer.Control.DEF_Thread.ETrsStage1Step;
using static LWDicer.Control.DEF_Thread.EThreadMessage;
using static LWDicer.Control.DEF_Thread.ERunMode;
using static LWDicer.Control.DEF_Error;
using static LWDicer.Control.DEF_Common;

namespace LWDicer.Control
{
    public class CTrsStage1RefComp
    {
        public MWorkerThread worker1;
        public MWorkerThread worker3;
        public MCtrlStage1 ctrlStage1;

        public override string ToString()
        {
            return $"CTrsStage1RefComp : {ctrlStage1}";
        }
    }

    public class CTrsStage1Data
    {
        public bool bUseOnline;
        public bool bInSfaTest;
    }

    public class MTrsStage1 : MWorkerThread
    {
        private CTrsStage1RefComp m_RefComp;
        private CTrsStage1Data m_Data;

        // Message 변수
        bool m_bWorkbench_LoadRequest;
        bool m_bWorkbench_LoadComplete;

        bool m_bAuto_PanelSupplyStop;

        public MTrsStage1(CObjectInfo objInfo, int selfChannel, 
            CTrsStage1RefComp refComp, CTrsStage1Data data) 
             : base(objInfo, selfChannel)
        {
            m_RefComp = refComp;
            SetData(data);
        }

        public int SetData(CTrsStage1Data source)
        {
            m_Data = ObjectExtensions.Copy(source);
            return SUCCESS;
        }

        public int GetData(out CTrsStage1Data target)
        {
            target = ObjectExtensions.Copy(m_Data);

            return SUCCESS;
        }

        public void PrintRefComp()
        {

            Debug.WriteLine(m_RefComp);
        }

        public int Initialize()
        {
            // Do initialize
            InitializeMsg();
            InitializeInterface();

            // Do Action
            int iStep = (int)TRS_STAGE1_MOVETO_LOAD;

            // finally
            ThreadStep = iStep;

            return SUCCESS;
        }

        public int InitializeMsg()
        {
            m_bWorkbench_LoadComplete = false;
            m_bWorkbench_LoadRequest = false;

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
                // if need to change response for common message, then add case state here.
                default:
                    base.ProcessMsg(evnt);
                    break;

                case (int)MSG_WORKBENCH_STAGE1_LOAD_REQUEST: // Workbench가 Stage1에게 Panel을 Load하라고 요청
                    m_bWorkbench_LoadRequest = true;
                    m_bWorkbench_LoadComplete = false;
                    break;

                case (int)MSG_WORKBENCH_STAGE1_LOAD_COMPLETE: // Workbench가 Stage1에게 Panel의 진공을 흡착했음을 알림
                    m_bWorkbench_LoadRequest = false;
                    m_bWorkbench_LoadComplete = true;
                    break;

                case (int)MSG_PANEL_SUPPLY_STOP:
                    m_bAuto_PanelSupplyStop = true;
                    break;

                case (int)MSG_PANEL_SUPPLY_START:
                    m_bAuto_PanelSupplyStop = false;
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
                        //m_RefComp.ctrlStage1.SetAutoManual(OPERATION_MANUAL);
                        break;

                    case STS_ERROR_STOP: // Error Stop
                        break;

                    case STS_STEP_STOP: // Step Stop
                        break;

                    case STS_RUN_READY: // Run Ready
                        break;

                    case STS_CYCLE_STOP: // Cycle Stop
                        //if (ThreadStep == TRS_STAGE1_MOVETO_LOAD)
                        break;

                    case STS_RUN: // auto run
                        //m_RefComp.ctrlStage1.SetAutoManual(OPERATION_AUTO);

                        switch (ThreadStep)
                        {
                            case (int)TRS_STAGE1_MOVETO_LOAD:
                                if (m_bAuto_PanelSupplyStop) break;

                                PostMsg(TrsAutoManager, (int)MSG_STAGE_LOADING_END);

                                iResult = m_RefComp.ctrlStage1.MoveToLoadPos();
                                if (iResult != SUCCESS) { SendAlarmTo(iResult); break; }

                                SetStep((int)TRS_STAGE1_WAIT_MOVETO_LOAD);
                                break;

                            case (int)TRS_STAGE1_WAIT_MOVETO_LOAD:
                                if (m_bAuto_PanelSupplyStop) break;

                                SetStep((int)TRS_STAGE1_LOAD_PANEL);
                                break;

                            case (int)TRS_STAGE1_LOAD_PANEL: //2

                                PostMsg(TrsAutoManager, (int)MSG_PANEL_INPUT);
                                PostMsg(TrsAutoManager, (int)MSG_STAGE_LOADING_END);

                                SetStep((int)TRS_STAGE1_CAMERA_MARK_POS);
                                break;

                            case (int)TRS_STAGE1_UNLOAD_COMPLETE: //7
                                                             //				if(!m_bWorkbench_SafetyPos) break;

                                iResult = m_RefComp.ctrlStage1.MoveToWaitPos(false);
                                if (iResult != SUCCESS) { SendAlarmTo(iResult); break; }

                                //PostMsg(TrsWorkbench, MSG_STAGE1_WORKBENCH_SAFETY_POS);

                                SetStep((int)TRS_STAGE1_MOVETO_LOAD);
                                break;

                            default:
                                break;
                        }
                        break;

                    default:
                        break;
                }

                Sleep(ThreadSleepTime);
                //Debug.WriteLine(ToString() + " Thread running..");
            }

        }

    }
}
