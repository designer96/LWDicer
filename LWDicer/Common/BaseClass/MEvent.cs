using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LWDicer.Control
{
    public class MEvent
    {
        public int Msg { get; set; }
        public int lParam { get; set; }
        public int wParam { get; set; }
        public int Sender { get; set; }

        private DateTime MsgTime;
        private int Index;

        private static int stCounter = 0;
        private object _Lock = new object();

        public MEvent(int Msg, int lParam, int wParam, int Sender = -1) // -1 : unknown
        {
            lock(_Lock)
            {
                this.Msg = Msg;
                this.lParam = lParam;
                this.wParam = wParam;
                this.Sender = Sender;
                MsgTime = DateTime.Now;

                if (stCounter >= Int32.MaxValue) stCounter = 0;
                Index = stCounter++;
            }
        }

        public override string ToString()
        {
            return $"[Event] Idx_{Index}, Msg : {Msg}, lParam : {lParam}, wParam : { wParam}, Sender : {Sender}, Created : {MsgTime.ToString("yyyy-MM-dd HH:mm:ss.ffff")}";
        }
    }
}
