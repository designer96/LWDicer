using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Messaging;
using System.Diagnostics;

using static LWDicer.Control.DEF_Common;

namespace LWDicer.Control
{
    public class MCmdTarget : MObject
    {
        private Queue<MEvent>  m_eventQ;

        private object _Lock = new object();

        public MCmdTarget(CObjectInfo objInfo) : base(objInfo)
        {
            m_eventQ = new Queue<MEvent>();
        }

        public int PostMsg(int msg, int wParam = 0, int lParam = 0)
		{ 
            return PostMsg(new MEvent(msg, wParam, lParam)); 
        }

        public int PostMsg(MEvent evnt)
        {
            lock(_Lock)
            {
                Debug.WriteLine("[EnQueue] " + evnt.ToString());

                m_eventQ.Enqueue(evnt);
            }

            return DEF_Error.SUCCESS;
        }

        public int SendMsg(int msg, int wParam = 0, int lParam = 0)
        { 
            return SendMsg(new MEvent(msg, wParam, lParam)); 
        }

        public int SendMsg(MEvent evnt)
        {
	        return ProcessMsg( evnt );
        }

        protected virtual int ProcessMsg(MEvent evnt)
        {
            Debug.WriteLine("Process " + evnt.ToString());

            return 0;
        }
        
        public virtual void CheckMsg(int nMsgCount = 2)
        {
            while (m_eventQ.Count > 0)
            {
                MEvent evnt = GetMsg();
                if (evnt == null)
                {
                    break;
                }

                ProcessMsg(evnt);
                if (--nMsgCount <= 0)
                {
                    break;
                }
            }
        }

        MEvent GetMsg() 
        {
            lock (_Lock)
            {
                try
                {
                    MEvent evnt = (MEvent)m_eventQ.Dequeue();
                    Debug.WriteLine("[DeQueue] " + evnt.ToString());
                    return evnt;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }

                return null;
            }
        }
    }
}
