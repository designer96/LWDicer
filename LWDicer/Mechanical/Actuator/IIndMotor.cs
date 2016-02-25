using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using static LWDicer.Control.DEF_IndMotor;

namespace LWDicer.Control
{
    public class DEF_IndMotor
    {
        public const int ERR_INDMOTOR_LOG_NULL_POINTER = 1;
        public const int ERR_INDMOTOR_NULL_DATA        = 2;
        public const int ERR_INDMOTOR_INVALID_POINTER  = 3;
        public const int ERR_INDMOTOR_TIMEOUT          = 4;
        public const int ERR_INDMOTOR_NO_SWITCHCASE    = 5;

        public struct SIndMotorRefCompList
        {
        }

        public struct SIndMotorData
        {
            // Induction Motor를 제어하는 Solenoid의 I/O Address
            public int Solenoid;
        }

    }
    interface IIndMotor
    {
        int TurnIndMotor();

        int StopIndMotor();

        int AssignComponents(IIO pIO);
        int SetData(SIndMotorData source);
        int GetData(out SIndMotorData target);
    }
}
