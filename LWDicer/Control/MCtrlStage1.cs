using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using static LWDicer.Control.DEF_Error;
using static LWDicer.Control.DEF_Common;

namespace LWDicer.Control
{
    public class CCtrlStage1RefComp
    {
        //public CCtrlStage1RefComp()
        //{
        //}
        public override string ToString()
        {
            return $"CCtrlStage1RefComp : ";
        }
    }

    public class CCtrlStage1Data
    {
        public bool bUseOnline;
        public bool bInSfaTest;
    }

    public class MCtrlStage1 : MObject
    {
        private CCtrlStage1RefComp m_RefComp;
        private CCtrlStage1Data m_Data;

        public MCtrlStage1(CObjectInfo objInfo, 
            CCtrlStage1RefComp refComp, CCtrlStage1Data data)
            : base(objInfo)
        {
            m_RefComp = refComp;
            SetData(data);
        }

        public int SetData(CCtrlStage1Data source)
        {
            m_Data = ObjectExtensions.Copy(source);
            return SUCCESS;
        }

        public int GetData(out CCtrlStage1Data target)
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
