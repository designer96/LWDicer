using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Diagnostics;

using static LWDicer.Control.DEF_Thread;
using static LWDicer.Control.DEF_Thread.ETrsLoaderStep;
using static LWDicer.Control.DEF_Thread.EThreadMessage;
using static LWDicer.Control.DEF_Thread.ERunMode;
using static LWDicer.Control.DEF_Error;
using static LWDicer.Control.DEF_Common;

namespace LWDicer.Control
{
    public class CTrsLoaderRefComp
    {
        public MCtrlLoader ctrlLoader;

        public override string ToString()
        {
            return $"CTrsLoaderRefComp : {ctrlLoader}";
        }
    }

    public class CTrsLoaderData
    {
        public bool bUseOnline;
        public bool bInSfaTest;
    }

    public class MTrsLoader : MWorkerThread
    {
        private CTrsLoaderRefComp m_RefComp;
        private CTrsLoaderData m_Data;

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

        public MTrsLoader(CObjectInfo objInfo, int selfChannel,
            CTrsLoaderRefComp refComp, CTrsLoaderData data)
             : base(objInfo, selfChannel)
        {
            m_RefComp = refComp;
            SetData(data);
        }

        public int SetData(CTrsLoaderData source)
        {
            m_Data = ObjectExtensions.Copy(source);
            return SUCCESS;
        }

        public int GetData(out CTrsLoaderData target)
        {
            target = ObjectExtensions.Copy(m_Data);

            return SUCCESS;
        }

        public int Initialize()
        {
            // Do initialize
            InitializeMsg();
            InitializeInterface();

            // Do Action
            int iStep = (int)TRS_LOADER_WAITFOR_MESSAGE;

            // finally
            ThreadStep = iStep;

            return SUCCESS;
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
                // if need to change response for common message, then add case state here.
                default:
                    base.ProcessMsg(evnt);
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

                case (int)MSG_PUSHPULL_LOADER_REQUEST_UNLOADING:
                    m_bPushPull_RequestUnloading = true;
                    break;

                case (int)MSG_PUSHPULL_LOADER_START_LOADING:
                    m_bPushPull_StartLoading = true;
                    break;

                case (int)MSG_PUSHPULL_LOADER_COMPLETE_LOADING:
                    m_bPushPull_CompleteLoading = true;
                    break;

                case (int)MSG_PUSHPULL_LOADER_REQUEST_LOADING:
                    m_bPushPull_RequestLoading = true;
                    break;

                case (int)MSG_PUSHPULL_LOADER_START_UNLOADING:
                    m_bPushPull_StartUnloading = true;
                    break;

                case (int)MSG_PUSHPULL_LOADER_COMPLETE_UNLOADING:
                    m_bPushPull_CompleteUnloading = true;
                    break;

                case (int)MSG_AUTO_LOADER_REQUEST_LOAD_CASSETTE:
                    m_bAuto_RequestLoadCassette = true;
                    break;

                case (int)MSG_AUTO_LOADER_REQUEST_UNLOAD_CASSETTE:
                    m_bAuto_RequestUnloadCassette = true;
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
                        //m_RefComp.ctrlLoader.SetAutoManual(OPERATION_MANUAL);
                        break;

                    case STS_ERROR_STOP: // Error Stop
                        break;

                    case STS_STEP_STOP: // Step Stop
                        break;

                    case STS_RUN_READY: // Run Ready
                        break;

                    case STS_CYCLE_STOP: // Cycle Stop
                        //if (ThreadStep == TRS_LOADER_MOVETO_LOAD)
                        break;

                    case STS_RUN: // auto run
                        //m_RefComp.ctrlLoader.SetAutoManual(OPERATION_AUTO);

                        switch (ThreadStep)
                        {
                            case (int)TRS_LOADER_WAITFOR_MESSAGE:

                                // 0. check wafer cassette status;

                                // 1. response to loading cassette
                                if (m_bAuto_RequestLoadCassette && m_bSupplyCassette)
                                {
                                    SetStep((int)TRS_LOADER_READY_LOAD_CASSETTE);
                                    break;
                                }

                                // 2. response to unloading cassette
                                if (m_bAuto_RequestUnloadCassette)
                                {
                                    SetStep((int)TRS_LOADER_READY_UNLOAD_CASSETTE);
                                    break;
                                }

                                // 3. response to loading wafer
                                if (m_bPushPull_RequestLoading)
                                {
                                    SetStep((int)TRS_LOADER_READY_LOADING_WAFER);
                                    break;
                                }

                                // 4. response to unloading wafer
                                if (m_bPushPull_RequestUnloading && m_bSupplyWafer)
                                {
                                    SetStep((int)TRS_LOADER_READY_UNLOADING_WAFER);
                                    break;
                                }
                                break;

                            case (int)TRS_LOADER_READY_LOAD_CASSETTE:

                                SetStep((int)TRS_LOADER_WAITFOR_CASSETTE_LOADED);
                                break;

                            case (int)TRS_LOADER_WAITFOR_CASSETTE_LOADED:

                                SetStep((int)TRS_LOADER_LOAD_CASSETTE);
                                break;

                            case (int)TRS_LOADER_LOAD_CASSETTE:

                                SetStep((int)TRS_LOADER_CHECK_STACK_OF_CASSETTE);
                                break;

                            case (int)TRS_LOADER_CHECK_STACK_OF_CASSETTE:

                                SetStep((int)TRS_LOADER_WAITFOR_MESSAGE);
                                break;

                            case (int)TRS_LOADER_READY_UNLOAD_CASSETTE:

                                SetStep((int)TRS_LOADER_WAITFOR_CASSETTE_REMOVED);
                                break;

                            case (int)TRS_LOADER_WAITFOR_CASSETTE_REMOVED:

                                SetStep((int)TRS_LOADER_WAITFOR_MESSAGE);
                                break;

                            case (int)TRS_LOADER_READY_UNLOADING_WAFER:

                                SetStep((int)TRS_LOADER_WAITFOR_PUSHPULL_START_LOADING);
                                break;

                            case (int)TRS_LOADER_WAITFOR_PUSHPULL_START_LOADING:

                                SetStep((int)TRS_LOADER_UNLOAD_WAFER);
                                break;

                            case (int)TRS_LOADER_UNLOAD_WAFER:

                                SetStep((int)TRS_LOADER_WAITFOR_PUSHPULL_COMPLETE_LOADING);
                                break;

                            case (int)TRS_LOADER_WAITFOR_PUSHPULL_COMPLETE_LOADING:

                                SetStep((int)TRS_LOADER_WAITFOR_MESSAGE);
                                break;

                            case (int)TRS_LOADER_READY_LOADING_WAFER:

                                SetStep((int)TRS_LOADER_WAITFOR_PUSHPULL_START_UNLOADING);
                                break;

                            case (int)TRS_LOADER_WAITFOR_PUSHPULL_START_UNLOADING:

                                SetStep((int)TRS_LOADER_LOAD_WAFER);
                                break;

                            case (int)TRS_LOADER_LOAD_WAFER:

                                SetStep((int)TRS_LOADER_WAITFOR_PUSHPULL_COMPLETE_UNLOADING);
                                break;

                            case (int)TRS_LOADER_WAITFOR_PUSHPULL_COMPLETE_UNLOADING:

                                SetStep((int)TRS_LOADER_WAITFOR_MESSAGE);
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
