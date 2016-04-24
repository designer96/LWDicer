using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static LWDicer.Control.DEF_Thread;
using static LWDicer.Control.DEF_Error;
using static LWDicer.Control.DEF_Common;

namespace LWDicer.Control
{
    /// <summary>
    /// 해당 Layer class에서 공통된 특성을 나중에 따로 묶을수 있도록 define
    /// Abstract class 이기 때문에 refComp 혹은  data class는 define 하지 않는다.
    /// </summary>
    public abstract class MCtrlLayer : MObject
    {
        protected EAutoManual AutoManual;    // EAutoManual : AUTO, MANUAL
        protected EOpMode OpMode;            // EOpMode : NORMAL_RUN, PASS_RUN, DRY_RUN, REPAIR_RUN

        public MCtrlLayer(CObjectInfo objInfo) : base(objInfo)
        {
            AutoManual = EAutoManual.MANUAL;
            OpMode = EOpMode.NORMAL_RUN;
        }

        public void SetOperationMode(EOpMode mode)
        {
            OpMode = mode;
        }

        public void SetAutoManual(EAutoManual mode)
        {
            AutoManual = mode;
        }
    }
}
