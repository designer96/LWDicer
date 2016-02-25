using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using static LWDicer.Control.DEF_Vacuum;

namespace LWDicer.Control
{
    public class DEF_Vacuum
    {
        public const int DEF_MAX_VACUUM_SENSOR = 4;
        public const int DEF_MAX_VACUUM_SOLENOID = 4;

        public const int ERR_VACUUM_LOG_NULL_POINTER = 1;
        public const int ERR_VACUUM_NULL_DATA = 2;
        public const int ERR_VACUUM_INVALID_POINTER = 3;
        public const int ERR_VACUUM_TIMEOUT = 4;
        public const int ERR_VACUUM_NO_SWITCHCASE = 5;

        public enum EVacuumTime
        {
            TURNING_TIME, ON_SETTLING_TIME, OFF_SETTLING_TIME
        };

        public enum EVacuumType
        {
            SINGLE_VACUUM,
            SINGLE_VACUUM_WBLOW,
            DOUBLE_VACUUM,
            DOUBLE_VACUUM_WBLOW,
            HETERO_DOUBLE_VACUUM,
            REVERSE_DOUBLE_VACUUM,
            REVERSE_SINGLE_VACUUM
        };

        /** Vacuum Component가 가지는 Mechanical Component List */
        public class CVacuumRefCompList
        {
        }

        public class CVacuumTime
        {
            // Vacuum On-Off 하는데 걸리는 최대 시간
            public double TurningTime;

            //	Vacuum이 Turn-Off 된 후 안정화되는데 필요한 시간
            public double OffSettlingTime;

            // Vacuum이 Turn-On 된 후 안정화되는데 필요한 시간
            public double OnSettlingTime;
        }

        public class CVacuumData
        {
            // @link aggregation Vacuum Type
            public EVacuumType VacuumType;

            // Vacuum을 제어하는 Solenoid들의 I/O Address들에 대한 포인터
            public int[] Solenoid = new int[DEF_MAX_VACUUM_SOLENOID];

            // Vacuum의 흡착 상태를 확인하는 Sensor들의 I/O Address들에 대한 포인터
            public int[] Sensor = new int[DEF_MAX_VACUUM_SOLENOID];

            public CVacuumTime Time = new CVacuumTime();

            public CVacuumData()
            {
            }

        }
    }

    public interface IVacuum
    {
        int SetVacuumTime(CVacuumTime time);

        int GetVacuumTime(out CVacuumTime time);

        /*----------- Vacuum 상태 확인  -----------------------*/
        /**
         * 현재 Vacuum 출력상태와  I/O의 출력 상태를 비교한다.
         * @param	pbStatus : TRUE = OK, false = NOT OK 
         * @return  0 = SUCCESS, 그외 IO Device Error Code
         */
        int CompareIO(out bool pbStatus);

        /**
         * Vacuum이 On 되었는지 확인한다.
         * @param	pbStatus : TRUE = OK, false = NOT OK 
         * @return  0 = SUCCESS, 그외 IO Device Error Code
         */
        int IsOn(out bool pbStatus);

        /**
         * Vacuum이 Off 되었는지 확인한다.
         * @param	pbStatus : TRUE = OK, false = NOT OK 
         * @return  0 = SUCCESS, 그외 IO Device Error Code
         */
        int IsOff(out bool pbStatus);

        /*----------- Vacuum 동작  -----------------------*/
        /**
         * Vacuum을 On시킨다.
         * @param bool skip_sensor = false  (sol의 작동을 명령한 후에 sensor로 완료 Check할 것인가를 결정하는 변수임, TRUE일 경우 sensor Check하지 않고 Skip함)
         * @return  0 if Vacuum operates right,  Error Number if error occurs
         */
        int On(bool skip_sensor = false);

        /**
         * Vacuum을 Off시킨다.
         * @param bool skip_sensor = false  (sol의 작동을 명령한 후에 sensor로 완료 Check할 것인가를 결정하는 변수임, TRUE일 경우 sensor Check하지 않고 Skip함)
         * @return  0 if Vacuum operates right,  Error Number if error occurs
         */
        int Off(bool skip_sensor = false);

        /**
         * Vacuum을 On신호만 해제한후, 파괴시키지는 않는다. 이 함수를 실행후, Off함수를 사용해 완전히 파괴시켜야한다.
         * @param bool skip_sensor = false  (sol의 작동을 명령한 후에 sensor로 완료 Check할 것인가를 결정하는 변수임, TRUE일 경우 sensor Check하지 않고 Skip함)
         * @return  0 if Vacuum operates right,  Error Number if error occurs
         */
        int Off_Half(bool skip_sensor = false);
        /*----------- Vacuum 동작 시작  -----------------------*/
        /**
         * Vacuum을 On시킨다.
         * @return  0 if Vacuum operates right,  Error Number if error occurs
         */
        int StartOn();

        /**
         * Vacuum을 Off시킨다.
         * @return  0 if Vacuum operates right,  Error Number if error occurs
         */
        int StartOff();

        /*----------- Vacuum 동작완료시까지 Sleep  -----------------------*/
        /**
         * Vacuum On이 완료될때까지 기다린다.
         * @return  0 if Vacuum operates right,  Error Number if error occurs
         */
        int Wait4OnComplete();

        /**
         * Vacuum Off가 완료될때까지 기다린다.
         * @return  0 if Vacuum operates right,  Error Number if error occurs
         */
        int Wait4OffComplete();

        /**
         * 모든 Solenoid를 Off한다.
         */
        void OffSolenoid();

        //----------- Component 공통  -----------------------
        int AssignComponents(IIO pIO);
        int SetData(CVacuumData source);
        int GetData(out CVacuumData target);

    }
}
