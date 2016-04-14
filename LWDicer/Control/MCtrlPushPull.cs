using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using static LWDicer.Control.DEF_Error;
using static LWDicer.Control.DEF_Common;

namespace LWDicer.Control
{
    public class CCtrlPushPullRefComp
    {
        //public CCtrlPushPullRefComp()
        //{
        //}
        public override string ToString()
        {
            return $"CCtrlPushPullRefComp : ";
        }
    }

    public class CCtrlPushPullData
    {
    }

    public class MCtrlPushPull : MCtrlLayer
    {
        private CCtrlPushPullRefComp m_RefComp;
        private CCtrlPushPullData m_Data;

        public MCtrlPushPull(CObjectInfo objInfo,
            CCtrlPushPullRefComp refComp, CCtrlPushPullData data)
            : base(objInfo)
        {
            m_RefComp = refComp;
            SetData(data);
        }

        public int SetData(CCtrlPushPullData source)
        {
            m_Data = ObjectExtensions.Copy(source);
            return SUCCESS;
        }

        public int GetData(out CCtrlPushPullData target)
        {
            target = ObjectExtensions.Copy(m_Data);

            return SUCCESS;
        }

        public int IsWaferDetected(out bool bState)
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
