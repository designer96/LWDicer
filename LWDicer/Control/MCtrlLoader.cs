using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using static LWDicer.Control.DEF_Error;
using static LWDicer.Control.DEF_Common;

namespace LWDicer.Control
{
    public class CCtrlLoaderRefComp
    {
        //public CCtrlLoaderRefComp()
        //{
        //}
        public override string ToString()
        {
            return $"CCtrlLoaderRefComp : ";
        }
    }

    public class CCtrlLoaderData
    {
        public bool bUseOnline;
        public bool bInSfaTest;
    }

    public class MCtrlLoader : MObject
    {
        private CCtrlLoaderRefComp m_RefComp;
        private CCtrlLoaderData m_Data;

        public MCtrlLoader(CObjectInfo objInfo,
            CCtrlLoaderRefComp refComp, CCtrlLoaderData data)
            : base(objInfo)
        {
            m_RefComp = refComp;
            SetData(data);
        }

        public int SetData(CCtrlLoaderData source)
        {
            m_Data = ObjectExtensions.Copy(source);
            
            return SUCCESS;
        }

        public int GetData(out CCtrlLoaderData target)
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
