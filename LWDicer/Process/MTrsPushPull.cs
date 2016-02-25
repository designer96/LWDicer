using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Diagnostics;

using static LWDicer.Control.DEF_Thread;
using static LWDicer.Control.DEF_Thread.ETrsPushPullStep;
using static LWDicer.Control.DEF_Thread.EThreadMessage;
using static LWDicer.Control.DEF_Thread.ERunMode;
using static LWDicer.Control.DEF_Error;
using static LWDicer.Control.DEF_Common;

namespace LWDicer.Control
{
    public class CTrsPushPullRefComp
    {
        public MCtrlPushPull ctrlPushPull;

        public override string ToString()
        {
            return $"CTrsPushPullRefComp : {ctrlPushPull}";
        }
    }

    public class CTrsPushPullData
    {
        public bool bUseOnline;
        public bool bInSfaTest;
    }

    public class MTrsPushPull : MWorkerThread
    {
        private CTrsPushPullRefComp m_RefComp;
        private CTrsPushPullData m_Data;

        // Message 변수
        bool m_bLoader_ReadyUnloading;
        bool m_bLoader_ReadyLoading;
        bool m_bLoader_AllWaferWorked;
        bool m_bLoader_StacksFull;

        bool m_bCleaner_ReadyLoading;
        bool m_bCleaner_StartLoading;
        bool m_bCleaner_CompleteLoading;
        bool m_bCleaner_ReadyUnloading;
        bool m_bCleaner_StartUnloading;
        bool m_bCleaner_CompleteUnloading;

        bool m_bCoater_ReadyLoading;
        bool m_bCoater_StartLoading;
        bool m_bCoater_CompleteLoading;
        bool m_bCoater_ReadyUnloading;
        bool m_bCoater_StartUnloading;
        bool m_bCoater_CompleteUnloading;

        bool m_bHandler_ReadyLoading;
        bool m_bHandler_StartLoading;
        bool m_bHandler_CompleteLoading;
        bool m_bHandler_ReadyUnloading;
        bool m_bHandler_StartUnloading;
        bool m_bHandler_CompleteUnloading;

        public MTrsPushPull(CObjectInfo objInfo, int selfChannel,
            CTrsPushPullRefComp refComp, CTrsPushPullData data)
             : base(objInfo, selfChannel)
        {
            m_RefComp = refComp;
            SetData(data);
        }

        public int SetData(CTrsPushPullData source)
        {
            m_Data = ObjectExtensions.Copy(source);
            return SUCCESS;
        }

        public int GetData(out CTrsPushPullData target)
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
            int iStep = (int)TRS_PUSHPULL_WAITFOR_MESSAGE;

            // finally
            ThreadStep = iStep;

            return SUCCESS;
        }

        public int InitializeMsg()
        {
            m_bLoader_ReadyUnloading = false;
            m_bLoader_ReadyLoading = false;
            m_bLoader_AllWaferWorked = false;
            m_bLoader_StacksFull = false;

            m_bCleaner_ReadyLoading = false;
            m_bCleaner_StartLoading = false;
            m_bCleaner_CompleteLoading = false;
            m_bCleaner_ReadyUnloading = false;
            m_bCleaner_StartUnloading = false;
            m_bCleaner_CompleteUnloading = false;

            m_bCoater_ReadyLoading = false;
            m_bCoater_StartLoading = false;
            m_bCoater_CompleteLoading = false;
            m_bCoater_ReadyUnloading = false;
            m_bCoater_StartUnloading = false;
            m_bCoater_CompleteUnloading = false;

            m_bHandler_ReadyLoading = false;
            m_bHandler_StartLoading = false;
            m_bHandler_CompleteLoading = false;
            m_bHandler_ReadyUnloading = false;
            m_bHandler_StartUnloading = false;
            m_bHandler_CompleteUnloading = false;

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

                case (int)MSG_LOADER_PUSHPULL_READY_UNLOADING:
                    m_bLoader_ReadyUnloading = true;
                    break;

                case (int)MSG_LOADER_PUSHPULL_READY_LOADING:
                    m_bLoader_ReadyLoading = true;
                    break;

                case (int)MSG_LOADER_PUSHPULL_ALL_WAFER_WORKED:
                    m_bLoader_AllWaferWorked = true;
                    break;

                case (int)MSG_LOADER_PUSHPULL_STACKS_FULL:
                    m_bLoader_StacksFull = true;
                    break;

                case (int)MSG_CLEANER_PUSHPULL_READY_LOADING:
                    m_bCleaner_ReadyLoading = true;
                    break;

                case (int)MSG_CLEANER_PUSHPULL_START_LOADING:
                    m_bCleaner_StartLoading = true;
                    break;

                case (int)MSG_CLEANER_PUSHPULL_COMPLETE_LOADING:
                    m_bCoater_CompleteLoading = true;
                    break;

                case (int)MSG_CLEANER_PUSHPULL_READY_UNLOADING:
                    m_bCleaner_ReadyUnloading = true;
                    break;

                case (int)MSG_CLEANER_PUSHPULL_START_UNLOADING:
                    m_bCleaner_StartUnloading = true;
                    break;

                case (int)MSG_CLEANER_PUSHPULL_COMPLETE_UNLOADING:
                    m_bCleaner_CompleteUnloading = true;
                    break;

                case (int)MSG_COATER_PUSHPULL_READY_LOADING:
                    m_bCoater_ReadyLoading = true;
                    break;

                case (int)MSG_COATER_PUSHPULL_START_LOADING:
                    m_bCoater_StartLoading = true;
                    break;

                case (int)MSG_COATER_PUSHPULL_COMPLETE_LOADING:
                    m_bCoater_CompleteLoading = true;
                    break;

                case (int)MSG_COATER_PUSHPULL_READY_UNLOADING:
                    m_bCoater_ReadyUnloading = true;
                    break;

                case (int)MSG_COATER_PUSHPULL_START_UNLOADING:
                    m_bCoater_StartUnloading = true;
                    break;

                case (int)MSG_COATER_PUSHPULL_COMPLETE_UNLOADING:
                    m_bCoater_CompleteUnloading = true;
                    break;

                case (int)MSG_HANDLER_PUSHPULL_READY_LOADING:
                    m_bHandler_ReadyLoading = true;
                    break;

                case (int)MSG_HANDLER_PUSHPULL_START_LOADING:
                    m_bHandler_StartLoading = true;
                    break;

                case (int)MSG_HANDLER_PUSHPULL_COMPLETE_LOADING:
                    m_bCoater_CompleteLoading = true;
                    break;

                case (int)MSG_HANDLER_PUSHPULL_READY_UNLOADING:
                    m_bHandler_ReadyUnloading = true;
                    break;

                case (int)MSG_HANDLER_PUSHPULL_START_UNLOADING:
                    m_bHandler_StartUnloading = true;
                    break;

                case (int)MSG_HANDLER_PUSHPULL_COMPLETE_UNLOADING:
                    m_bHandler_CompleteUnloading = true;
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
                        //m_RefComp.ctrlPushPull.SetAutoManual(OPERATION_MANUAL);
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
                        //m_RefComp.ctrlPushPull.SetAutoManual(OPERATION_AUTO);

                        switch (ThreadStep)
                        {
                            case (int)TRS_PUSHPULL_WAITFOR_MESSAGE:

                                // 0. check default status;

                                // 1. if detected wafer
                                m_RefComp.ctrlPushPull.IsWaferDetected(out bState);
                                if (bState)
                                {
                                    // 1.1 unload to loader
                                    SetStep((int)TRS_PUSHPULL_REQUEST_LOADER_LOADING);
                                    break;

                                    // 1.2 unload to cleaner
                                    SetStep((int)TRS_PUSHPULL_REQUEST_CLEANER_LOADING);
                                    break;

                                    // 1.3 unload to coater
                                    SetStep((int)TRS_PUSHPULL_REQUEST_COATER_LOADING);
                                    break;

                                    // 1.4 unload to handler
                                    SetStep((int)TRS_PUSHPULL_REQUEST_HANDLER_LOADING);
                                    break;
                                }
                                // 2. if not detected wafer
                                else
                                {
                                    // 2.1 load from loader
                                    SetStep((int)TRS_PUSHPULL_REQUEST_LOADER_UNLOADING);
                                    break;

                                    // 2.2 load from cleaner
                                    SetStep((int)TRS_PUSHPULL_REQUEST_CLEANER_UNLOADING);
                                    break;

                                    // 2.3 load from coater
                                    SetStep((int)TRS_PUSHPULL_REQUEST_COATER_UNLOADING);
                                    break;

                                    // 2.4 load from handler
                                    SetStep((int)TRS_PUSHPULL_REQUEST_HANDLER_UNLOADING);
                                    break;
                                }

                                break;

                                ///////////////////////////////////////////////////
                                // transfer wafer to loader from wafer
                            case (int)TRS_PUSHPULL_REQUEST_LOADER_LOADING:
                                PostMsg(TrsLoader, (int)MSG_PUSHPULL_LOADER_REQUEST_LOADING);

                                SetStep((int)TRS_PUSHPULL_WAITFOR_LOADER_READY_LOADING);
                                break;

                            case (int)TRS_PUSHPULL_WAITFOR_LOADER_READY_LOADING:
                                if (m_bLoader_ReadyLoading == false) break;
                                m_bLoader_ReadyLoading = false;

                                SetStep((int)TRS_PUSHPULL_WAITFOR_LOADER_COMPLETE_LOADING);
                                break;

                            case (int)TRS_PUSHPULL_START_UNLOADING_TO_LOADER:
                                // move to loader

                                PostMsg(TrsLoader, (int)MSG_PUSHPULL_LOADER_START_UNLOADING);

                                SetStep((int)TRS_PUSHPULL_WAITFOR_LOADER_COMPLETE_LOADING);
                                break;

                            case (int)TRS_PUSHPULL_WAITFOR_LOADER_COMPLETE_LOADING:
                                PostMsg(TrsLoader, (int)MSG_PUSHPULL_LOADER_REQUEST_LOADING);

                                SetStep((int)TRS_PUSHPULL_COMPLETE_UNLOADING_TO_LOADER);
                                break;

                            case (int)TRS_PUSHPULL_COMPLETE_UNLOADING_TO_LOADER:
                                PostMsg(TrsLoader, (int)MSG_PUSHPULL_LOADER_REQUEST_LOADING);

                                SetStep((int)TRS_PUSHPULL_WAITFOR_MESSAGE);
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
