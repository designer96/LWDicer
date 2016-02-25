using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using static LWDicer.Control.DEF_Common;
using static LWDicer.Control.DEF_Error;
using static LWDicer.Control.DEF_Vacuum;
using static LWDicer.Control.DEF_Vacuum.EVacuumTime;
using static LWDicer.Control.DEF_Vacuum.EVacuumType;

namespace LWDicer.Control
{
    public class MVacuum : MObject, IVacuum
    {
        IIO m_IO;
        CVacuumData m_Data;
        MTickTimer m_waitTimer = new MTickTimer();

        public MVacuum(CObjectInfo objInfo, IIO pIO, CVacuumData data) 
            : base(objInfo)
        {
            m_IO = pIO;

            SetData(data);
        }
 
        ~MVacuum()
        {
        }

        public int SetVacuumTime(CVacuumTime time)
        {
            m_Data.Time = time;
            return SUCCESS;
        }

        public int GetVacuumTime(out CVacuumTime time)
        {
            time = ObjectExtensions.Copy(m_Data.Time);
            return SUCCESS;
        }

        public int CompareIO(out bool pbVal)
        {
            int iRet;
            bool bVal1, bVal2;
            pbVal = false;

            switch (m_Data.VacuumType)
            {
                case SINGLE_VACUUM:
                case SINGLE_VACUUM_WBLOW:
                    if ((iRet = m_IO.IsOn(m_Data.Sensor[0], out bVal1)) != SUCCESS) return iRet;
                    if ((iRet = m_IO.IsOff(m_Data.Solenoid[0], out bVal2)) != SUCCESS) return iRet;
                    if (bVal1 && bVal2)
                    {
                        pbVal = false;
                        return SUCCESS;
                    }
                    else
                    {
                        if ((iRet = m_IO.IsOff(m_Data.Sensor[0], out bVal1)) != SUCCESS) return iRet;
                        if ((iRet = m_IO.IsOn(m_Data.Solenoid[0], out bVal2)) != SUCCESS) return iRet;
                        if (bVal1 && bVal2)
                        {
                            pbVal = false;
                            return SUCCESS;
                        }
                    }
                    pbVal = true;
                    return SUCCESS;
                    break;

                case REVERSE_SINGLE_VACUUM:
                    if ((iRet = m_IO.IsOn(m_Data.Sensor[0], out bVal1)) != SUCCESS) return iRet;
                    if ((iRet = m_IO.IsOn(m_Data.Solenoid[0], out bVal2)) != SUCCESS) return iRet;
                    if (bVal1 && bVal2)
                    {
                        pbVal = false;
                        return SUCCESS;
                    }
                    else
                    {
                        if ((iRet = m_IO.IsOff(m_Data.Sensor[0], out bVal1)) != SUCCESS) return iRet;
                        if ((iRet = m_IO.IsOff(m_Data.Solenoid[0], out bVal2)) != SUCCESS) return iRet;
                        if (bVal1 && bVal2)
                        {
                            pbVal = false;
                            return SUCCESS;
                        }
                    }
                    pbVal = true;
                    return SUCCESS;
                    break;

                case DOUBLE_VACUUM:
                case DOUBLE_VACUUM_WBLOW:

                    if ((iRet = m_IO.IsOn(m_Data.Sensor[0], out bVal1)) != SUCCESS) return iRet;
                    if ((iRet = m_IO.IsOff(m_Data.Solenoid[0], out bVal2)) != SUCCESS) return iRet;
                    if (bVal1 && bVal2)
                    {
                        pbVal = false;
                        return SUCCESS;
                    }
                    else
                    {
                        if ((iRet = m_IO.IsOff(m_Data.Sensor[0], out bVal1)) != SUCCESS) return iRet;
                        if ((iRet = m_IO.IsOn(m_Data.Solenoid[0], out bVal2)) != SUCCESS) return iRet;
                        if (bVal1 && bVal2)
                        {
                            pbVal = false;
                            return SUCCESS;
                        }
                        else
                        {
                            if ((iRet = m_IO.IsOn(m_Data.Sensor[1], out bVal1)) != SUCCESS) return iRet;
                            if ((iRet = m_IO.IsOff(m_Data.Solenoid[1], out bVal2)) != SUCCESS) return iRet;
                            if (bVal1 && bVal2)
                            {
                                pbVal = false;
                                return SUCCESS;
                            }
                            else
                            {
                                if ((iRet = m_IO.IsOff(m_Data.Sensor[1], out bVal1)) != SUCCESS) return iRet;
                                if ((iRet = m_IO.IsOn(m_Data.Solenoid[1], out bVal2)) != SUCCESS) return iRet;
                                if (bVal1 && bVal2)
                                {
                                    pbVal = false;
                                    return SUCCESS;
                                }
                            }
                        }
                    }

                    pbVal = true;
                    return SUCCESS;

                    break;
                case HETERO_DOUBLE_VACUUM:
                    //   if( (m_IO.IsOn(m_Data.Sensor[0]) && m_IO.IsOn(m_Data.Solenoid[0]))  || 
                    //       (!m_IO.IsOn(m_Data.Sensor[0]) && !m_IO.IsOn(m_Data.Solenoid[0])) ||
                    //       (m_IO.IsOn(m_Data.Sensor[1]) && !m_IO.IsOn(m_Data.Solenoid[1])) ||
                    //	      (!m_IO.IsOn(m_Data.Sensor[1]) && m_IO.IsOn(m_Data.Solenoid[1])))
                    //        return false;

                    if ((iRet = m_IO.IsOn(m_Data.Sensor[0], out bVal1)) != SUCCESS) return iRet;
                    if ((iRet = m_IO.IsOn(m_Data.Solenoid[0], out bVal2)) != SUCCESS) return iRet;
                    if (bVal1 && bVal2)
                    {
                        pbVal = false;
                        return SUCCESS;
                    }
                    else
                    {
                        if ((iRet = m_IO.IsOff(m_Data.Sensor[0], out bVal1)) != SUCCESS) return iRet;
                        if ((iRet = m_IO.IsOff(m_Data.Solenoid[0], out bVal2)) != SUCCESS) return iRet;
                        if (bVal1 && bVal2)
                        {
                            pbVal = false;
                            return SUCCESS;
                        }
                        else
                        {
                            if ((iRet = m_IO.IsOn(m_Data.Sensor[1], out bVal1)) != SUCCESS) return iRet;
                            if ((iRet = m_IO.IsOff(m_Data.Solenoid[1], out bVal2)) != SUCCESS) return iRet;
                            if (bVal1 && bVal2)
                            {
                                pbVal = false;
                                return SUCCESS;
                            }
                            else
                            {
                                if ((iRet = m_IO.IsOff(m_Data.Sensor[1], out bVal1)) != SUCCESS) return iRet;
                                if ((iRet = m_IO.IsOn(m_Data.Solenoid[1], out bVal2)) != SUCCESS) return iRet;
                                if (bVal1 && bVal2)
                                {
                                    pbVal = false;
                                    return SUCCESS;
                                }
                            }
                        }
                    }

                    pbVal = true;
                    return SUCCESS;

                    break;
                case REVERSE_DOUBLE_VACUUM:
                    //      if( (m_IO.IsOn(m_Data.Sensor[0]) && m_IO.IsOn(m_Data.Solenoid[0]))  ||
                    //		  (!m_IO.IsOn(m_Data.Sensor[0]) && !m_IO.IsOn(m_Data.Solenoid[0])) ||
                    //          (m_IO.IsOn(m_Data.Sensor[1]) && m_IO.IsOn(m_Data.Solenoid[1]))  ||
                    //	      (!m_IO.IsOn(m_Data.Sensor[1]) && !m_IO.IsOn(m_Data.Solenoid[1])))
                    //           return false;

                    if ((iRet = m_IO.IsOn(m_Data.Sensor[0], out bVal1)) != SUCCESS) return iRet;
                    if ((iRet = m_IO.IsOn(m_Data.Solenoid[0], out bVal2)) != SUCCESS) return iRet;
                    if (bVal1 && bVal2)
                    {
                        pbVal = false;
                        return SUCCESS;
                    }
                    else
                    {
                        if ((iRet = m_IO.IsOff(m_Data.Sensor[0], out bVal1)) != SUCCESS) return iRet;
                        if ((iRet = m_IO.IsOff(m_Data.Solenoid[0], out bVal2)) != SUCCESS) return iRet;
                        if (bVal1 && bVal2)
                        {
                            pbVal = false;
                            return SUCCESS;
                        }
                        else
                        {
                            if ((iRet = m_IO.IsOn(m_Data.Sensor[1], out bVal1)) != SUCCESS) return iRet;
                            if ((iRet = m_IO.IsOn(m_Data.Solenoid[1], out bVal2)) != SUCCESS) return iRet;
                            if (bVal1 && bVal2)
                            {
                                pbVal = false;
                                return SUCCESS;
                            }
                            else
                            {
                                if ((iRet = m_IO.IsOff(m_Data.Sensor[1], out bVal1)) != SUCCESS) return iRet;
                                if ((iRet = m_IO.IsOff(m_Data.Solenoid[1], out bVal2)) != SUCCESS) return iRet;
                                if (bVal1 && bVal2)
                                {
                                    pbVal = false;
                                    return SUCCESS;
                                }
                            }
                        }
                    }

                    pbVal = true;
                    return SUCCESS;

                    break;
                default:
                    break;
            }
            return GenerateErrorCode(ERR_VACUUM_NO_SWITCHCASE);
        }

        public int IsOn(out bool pbVal)
        {
            int iRet;
            bool bVal1, bVal2;
            pbVal = false;

            switch (m_Data.VacuumType)
            {
                case SINGLE_VACUUM:
                case SINGLE_VACUUM_WBLOW:
                case REVERSE_SINGLE_VACUUM:
                    return m_IO.IsOn(m_Data.Sensor[0], out pbVal);
                    break;
                case DOUBLE_VACUUM:
                case DOUBLE_VACUUM_WBLOW:
                case HETERO_DOUBLE_VACUUM:
                case REVERSE_DOUBLE_VACUUM:
                    if ((iRet = m_IO.IsOn(m_Data.Sensor[0], out bVal1)) != SUCCESS) return iRet;
                    if ((iRet = m_IO.IsOn(m_Data.Sensor[1], out bVal2)) != SUCCESS) return iRet;
                    if (bVal1 || bVal2) pbVal = true;
                    else pbVal = false;
                    return SUCCESS;
                    break;
                default:
                    break;
            }
            return GenerateErrorCode(ERR_VACUUM_NO_SWITCHCASE);
        }

        public int IsOff(out bool pbVal)
        {
            int iRet;
            bool bVal1, bVal2;
            pbVal = false;

            switch (m_Data.VacuumType)
            {
                case SINGLE_VACUUM:
                case SINGLE_VACUUM_WBLOW:
                case REVERSE_SINGLE_VACUUM:
                    return m_IO.IsOff(m_Data.Sensor[0], out pbVal);
                    break;
                case DOUBLE_VACUUM:
                case DOUBLE_VACUUM_WBLOW:
                case HETERO_DOUBLE_VACUUM:
                case REVERSE_DOUBLE_VACUUM:
                    if ((iRet = m_IO.IsOff(m_Data.Sensor[0], out bVal1)) != SUCCESS) return iRet;
                    if ((iRet = m_IO.IsOff(m_Data.Sensor[1], out bVal2)) != SUCCESS) return iRet;
                    if (bVal1 && bVal2) pbVal = true;
                    else pbVal = false;
                    return SUCCESS;
                    break;
                default:
                    break;
            }
            return GenerateErrorCode(ERR_VACUUM_NO_SWITCHCASE);
        }

        public int On(bool skip_sensor = false)
        {
            switch (m_Data.VacuumType)
            {
                case SINGLE_VACUUM:
                    m_IO.OutputOn(m_Data.Solenoid[0]);
                    break;
                case SINGLE_VACUUM_WBLOW:
                    m_IO.OutputOn(m_Data.Solenoid[0]);
                    //        m_IO.OutputOff(m_Data.Solenoid[1]);
                    break;
                case REVERSE_SINGLE_VACUUM:
                    m_IO.OutputOff(m_Data.Solenoid[0]);
                    break;
                case DOUBLE_VACUUM:
                case DOUBLE_VACUUM_WBLOW:
                    m_IO.OutputOn(m_Data.Solenoid[0]);
                    m_IO.OutputOn(m_Data.Solenoid[1]);
                    break;
                case HETERO_DOUBLE_VACUUM:
                    m_IO.OutputOff(m_Data.Solenoid[0]);
                    m_IO.OutputOn(m_Data.Solenoid[1]);
                    break;
                case REVERSE_DOUBLE_VACUUM:
                    m_IO.OutputOff(m_Data.Solenoid[0]);
                    m_IO.OutputOff(m_Data.Solenoid[1]);
                    break;
                default:
                    break;
            }

            if (skip_sensor)
            {
                Sleep(1);
                return SUCCESS;
            }

            return Wait4OnComplete();
        }

        public int Off(bool skip_sensor = false)
        {
            switch (m_Data.VacuumType)
            {
                case SINGLE_VACUUM:
                    m_IO.OutputOff(m_Data.Solenoid[0]);
                    Sleep((m_Data.Time.OffSettlingTime * 1000));           
                    break;
                case SINGLE_VACUUM_WBLOW:
                    m_IO.OutputOff(m_Data.Solenoid[0]);
                    m_IO.OutputOn(m_Data.Solenoid[1]);
                    Sleep((m_Data.Time.OffSettlingTime * 1000));           
                    m_IO.OutputOff(m_Data.Solenoid[1]);
                    break;
                case REVERSE_SINGLE_VACUUM:
                    m_IO.OutputOn(m_Data.Solenoid[0]);
                    break;
                case DOUBLE_VACUUM:
                    m_IO.OutputOff(m_Data.Solenoid[0]);
                    m_IO.OutputOff(m_Data.Solenoid[1]);
                    break;
                case DOUBLE_VACUUM_WBLOW:
                    m_IO.OutputOn(m_Data.Solenoid[2]);
                    m_IO.OutputOn(m_Data.Solenoid[3]);
                    Sleep((m_Data.Time.OffSettlingTime * 1000));           
                    break;
                case HETERO_DOUBLE_VACUUM:
                    m_IO.OutputOn(m_Data.Solenoid[0]);
                    m_IO.OutputOff(m_Data.Solenoid[1]);
                    break;
                case REVERSE_DOUBLE_VACUUM:
                    m_IO.OutputOn(m_Data.Solenoid[0]);
                    m_IO.OutputOn(m_Data.Solenoid[1]);
                    break;
                default:
                    break;
            }

            if (skip_sensor)
            {
                Sleep(1);
                return SUCCESS;
            }

            return Wait4OffComplete();
        }

        /**
        * Vacuum을 On신호만 해제한후, 파괴시키지는 않는다. 이 함수를 실행후, Off함수를 사용해 완전히 파괴시켜야한다.
        * @param int skip_sensor = false  (sol의 작동을 명령한 후에 sensor로 완료 Check할 것인가를 결정하는 변수임, true일 경우 sensor Check하지 않고 Skip함)
        * @return  0 if Vacuum operates right,  Error Number if error occurs
*/
        public int Off_Half(bool skip_sensor = false)
        {
            switch (m_Data.VacuumType)
            {
                case SINGLE_VACUUM:
                    m_IO.OutputOff(m_Data.Solenoid[0]);
                    break;
                case SINGLE_VACUUM_WBLOW:
                    m_IO.OutputOff(m_Data.Solenoid[0]);
                    break;
                case REVERSE_SINGLE_VACUUM:
                    m_IO.OutputOn(m_Data.Solenoid[0]);
                    break;
                case DOUBLE_VACUUM:
                    m_IO.OutputOff(m_Data.Solenoid[0]);
                    m_IO.OutputOff(m_Data.Solenoid[1]);
                    break;
                case DOUBLE_VACUUM_WBLOW:
                    m_IO.OutputOn(m_Data.Solenoid[2]);
                    m_IO.OutputOn(m_Data.Solenoid[3]);
                    break;
                case HETERO_DOUBLE_VACUUM:
                    m_IO.OutputOn(m_Data.Solenoid[0]);
                    m_IO.OutputOff(m_Data.Solenoid[1]);
                    break;
                case REVERSE_DOUBLE_VACUUM:
                    m_IO.OutputOn(m_Data.Solenoid[0]);
                    m_IO.OutputOn(m_Data.Solenoid[1]);
                    break;
                default:
                    break;
            }

            if (skip_sensor)
            {
                Sleep(1);
                return SUCCESS;
            }

            return Wait4OffComplete();
        }

        /*----------- MVacuum 동작 시작  -----------------------*/
        /**
         * Vacuum을 On시킨다.
         * @return  0 if Vacuum operates right,  Error Number if error occurs
         */
        public int StartOn()
        {
            return (On(true));
        }

        /**
         * Vacuum을 Off시킨다.
         * @return  0 if MVacuum operates right,  Error Number if error occurs
         */
        public int StartOff()
        {
            return (Off(true));
        }

        /*----------- MVacuum 동작완료시까지 Sleep  -----------------------*/
        /**
         * MVacuum On이 완료될때까지 기다린다.
         * @return  0 if MVacuum operates right,  Error Number if error occurs
         */
        public int Wait4OnComplete()
        {
            int iRet;
            bool bVal;

            m_waitTimer.StartTimer();

            while (true)
            {
                if ((iRet = IsOn(out bVal)) != SUCCESS) return iRet;
                if (bVal) break;

                if (m_waitTimer.MoreThan(m_Data.Time.TurningTime * 1000))
                {
                    switch (m_Data.VacuumType)
                    {
                        case SINGLE_VACUUM:
                            m_IO.OutputOff(m_Data.Solenoid[0]);
                            break;
                        case SINGLE_VACUUM_WBLOW:
                            m_IO.OutputOff(m_Data.Solenoid[0]);
                            m_IO.OutputOff(m_Data.Solenoid[1]);
                            break;
                        case REVERSE_SINGLE_VACUUM:
                            m_IO.OutputOn(m_Data.Solenoid[0]);
                            break;
                        case DOUBLE_VACUUM:
                        case DOUBLE_VACUUM_WBLOW:
                            m_IO.OutputOff(m_Data.Solenoid[0]);
                            m_IO.OutputOff(m_Data.Solenoid[1]);
                            break;
                        case HETERO_DOUBLE_VACUUM:
                            m_IO.OutputOn(m_Data.Solenoid[0]);
                            m_IO.OutputOff(m_Data.Solenoid[1]);
                            break;
                        case REVERSE_DOUBLE_VACUUM:
                            m_IO.OutputOn(m_Data.Solenoid[0]);
                            m_IO.OutputOn(m_Data.Solenoid[1]);
                            break;
                        default:
                            break;
                    }
                    return GenerateErrorCode(ERR_VACUUM_TIMEOUT);
                }
                Sleep(1);
            }

            Sleep((m_Data.Time.OnSettlingTime * 1000));

            return SUCCESS;
        }

        /**
         * MVacuum Off가 완료될때까지 기다린다.
         * @return  0 if MVacuum operates right,  Error Number if error occurs
         */
        public int Wait4OffComplete()
        {
            int iRet;
            bool bVal;

            m_waitTimer.StartTimer();

            while (true)
            {
                if ((iRet = IsOff(out bVal)) != SUCCESS) return iRet;
                if (bVal) break;
                if (m_waitTimer.MoreThan(m_Data.Time.TurningTime * 1000))
                {
                    switch (m_Data.VacuumType)
                    {
                        case SINGLE_VACUUM:
                            m_IO.OutputOff(m_Data.Solenoid[0]);
                            break;
                        case SINGLE_VACUUM_WBLOW:
                            m_IO.OutputOff(m_Data.Solenoid[0]);
                            m_IO.OutputOff(m_Data.Solenoid[1]);
                            break;
                        case REVERSE_SINGLE_VACUUM:
                            m_IO.OutputOn(m_Data.Solenoid[0]);
                            break;
                        case DOUBLE_VACUUM:
                            m_IO.OutputOff(m_Data.Solenoid[0]);
                            m_IO.OutputOff(m_Data.Solenoid[1]);
                            break;
                        case DOUBLE_VACUUM_WBLOW:
                            m_IO.OutputOff(m_Data.Solenoid[0]);
                            m_IO.OutputOff(m_Data.Solenoid[1]);
                            m_IO.OutputOff(m_Data.Solenoid[2]);
                            m_IO.OutputOff(m_Data.Solenoid[3]);
                            break;
                        case HETERO_DOUBLE_VACUUM:
                            m_IO.OutputOn(m_Data.Solenoid[0]);
                            m_IO.OutputOff(m_Data.Solenoid[1]);
                            break;
                        case REVERSE_DOUBLE_VACUUM:
                            m_IO.OutputOn(m_Data.Solenoid[0]);
                            m_IO.OutputOn(m_Data.Solenoid[1]);
                            break;
                        default:
                            break;
                    }
                    return GenerateErrorCode(ERR_VACUUM_TIMEOUT);
                }
                Sleep(1);
            }

            switch (m_Data.VacuumType)
            {
                case SINGLE_VACUUM_WBLOW:
                    m_IO.OutputOff(m_Data.Solenoid[0]);   
                    m_IO.OutputOff(m_Data.Solenoid[1]);
                    break;
                case DOUBLE_VACUUM_WBLOW:
                    m_IO.OutputOff(m_Data.Solenoid[0]);   
                    m_IO.OutputOff(m_Data.Solenoid[1]);
                    m_IO.OutputOff(m_Data.Solenoid[2]);
                    m_IO.OutputOff(m_Data.Solenoid[3]);
                    break;
                default:
                    Sleep((m_Data.Time.OffSettlingTime * 1000));    // Blow 아닐때만 안정화 기다리게 
                    break;
            }

            // Sleep((m_Data.Time.OffSettlingTime*1000));	// 삭제 : Blow 아닐때만 안정화 기다리게

            return SUCCESS;
        }

        /**
         * 모든 Solenoid를 Off한다.
         */
        public void OffSolenoid()
        {
            switch (m_Data.VacuumType)
            {
                case SINGLE_VACUUM:
                    m_IO.OutputOff(m_Data.Solenoid[0]);
                    break;
                case SINGLE_VACUUM_WBLOW:
                    m_IO.OutputOff(m_Data.Solenoid[0]);
                    m_IO.OutputOff(m_Data.Solenoid[1]);
                    break;
                case REVERSE_SINGLE_VACUUM:
                    m_IO.OutputOn(m_Data.Solenoid[0]);
                    break;
                case DOUBLE_VACUUM:
                    m_IO.OutputOff(m_Data.Solenoid[0]);
                    m_IO.OutputOff(m_Data.Solenoid[1]);
                    break;
                case DOUBLE_VACUUM_WBLOW:
                    m_IO.OutputOff(m_Data.Solenoid[0]);
                    m_IO.OutputOff(m_Data.Solenoid[1]);
                    m_IO.OutputOff(m_Data.Solenoid[2]);
                    m_IO.OutputOff(m_Data.Solenoid[3]);
                    break;
                case HETERO_DOUBLE_VACUUM:
                    m_IO.OutputOn(m_Data.Solenoid[0]);
                    m_IO.OutputOff(m_Data.Solenoid[1]);
                    break;
                case REVERSE_DOUBLE_VACUUM:
                    m_IO.OutputOn(m_Data.Solenoid[0]);
                    m_IO.OutputOn(m_Data.Solenoid[1]);
                    break;
                default:
                    break;
            }
            return;
        }

        /***************** Common Implementation *************************************/

        public int AssignComponents(IIO pIO)
        {
            Assert(pIO != null);

            m_IO = pIO;

            return SUCCESS;
        }

        public int SetData(CVacuumData source)
        {
            m_Data = ObjectExtensions.Copy(source);

            return SUCCESS;
        }

        public int GetData(out CVacuumData target)
        {
            target = ObjectExtensions.Copy(m_Data);

            return SUCCESS;
        }

    }
}
