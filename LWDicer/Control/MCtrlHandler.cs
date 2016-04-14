using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static LWDicer.Control.DEF_Error;
using static LWDicer.Control.DEF_Common;

namespace LWDicer.Control
{
    public class CCtrlHandlerRefComp
    {
        //public CCtrlHandlerRefComp()
        //{
        //}
        public override string ToString()
        {
            return $"MCtrlHandlerRefComp : ";
        }
    }

    public class CCtrlHandlerData
    {
    }

    public class MCtrlHandler : MObject
    {
        private CCtrlHandlerRefComp m_RefComp;
        private CCtrlHandlerData m_Data;

        public MCtrlHandler(CObjectInfo objInfo,
            CCtrlHandlerRefComp refComp, CCtrlHandlerData data)
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

        public int IsPanelDetected(out bool bState)
        {
            bState = false;
            return SUCCESS;
        }

        public int MoveToWaitPos(bool bObsolite)
        {
            return SUCCESS;
        }

        public int MoveToLoadPos()
        {
            return SUCCESS;
        }
    }
}
