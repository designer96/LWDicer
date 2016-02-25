using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using static LWDicer.Control.DEF_Error;
using static LWDicer.Control.MTickTimer.ETimeType;

namespace LWDicer.Control
{
    public class MTickTimer
    {
        public enum ETimeType
        {
            TIME_100NANOSECOND,
            TIME_MICROSECOND,
            TIME_MILLISECOND,
            TIME_SECOND,
            TIME_MINUTE,
            TIME_HOUR,
        }

        Stopwatch Timer;
        bool bIsTimerStarted;

        public MTickTimer()
        {
            Timer = new Stopwatch();
        }
        public int StartTimer()
        {
            Timer.Restart();
            //Timer.Start();
            bIsTimerStarted = true;

            return SUCCESS;
        }

        public int StopTimer()
        {
            //if (bIsTimerStarted == false) return;
            Timer.Stop();
            bIsTimerStarted = false;
            return SUCCESS;
        }

        public long GetElapsedTime(ETimeType type = TIME_MILLISECOND)
        {
            long gap = Timer.ElapsedTicks;

            switch(type)
            {
                case TIME_100NANOSECOND:
                    break;

                case TIME_MICROSECOND:
                    gap /= 10;
                    break;

                case TIME_MILLISECOND:
                    gap = Timer.ElapsedMilliseconds;
                    break;

                case TIME_SECOND:
                    gap = Timer.ElapsedMilliseconds / (1000);
                    break;

                case TIME_MINUTE:
                    gap = Timer.ElapsedMilliseconds / (1000 * 60);
                    break;

                case TIME_HOUR:
                    gap = Timer.ElapsedMilliseconds / (1000 * 60 * 60);
                    break;
            }
            return gap;
        }

        public bool LessThan(long CompareTime, ETimeType type = TIME_MILLISECOND)
        {
            long gap = GetElapsedTime(type);
            if (gap < CompareTime) return true;
            else return false;
        }

        public bool MoreThan(long CompareTime, ETimeType type = TIME_MILLISECOND)
        {
            long gap = GetElapsedTime(type);
            if (gap > CompareTime) return true;
            else return false;
        }

        public bool LessThan(double CompareTime, ETimeType type = TIME_MILLISECOND)
        {
            CompareTime = (long)CompareTime;
            long gap = GetElapsedTime(type);
            if (gap < CompareTime) return true;
            else return false;
        }

        public bool MoreThan(double CompareTime, ETimeType type = TIME_MILLISECOND)
        {
            CompareTime = (long)CompareTime;
            long gap = GetElapsedTime(type);
            if (gap > CompareTime) return true;
            else return false;
        }
    }
}
