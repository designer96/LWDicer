using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace LWDicer.Control
{
    public static class CGeneralUtils
    {
        public static class AnimateEffect
        {
            public const int AW_HOR_POSITIVE = 0X1;
            public const int AW_HOR_NEGATIVE = 0X2;
            public const int AW_VER_POSITIVE = 0X4;
            public const int AW_VER_NEGATIVE = 0X8;
            public const int AW_CENTER = 0X10;
            public const int AW_HIDE = 0X10000;
            public const int AW_ACTIVATE = 0X20000;
            public const int AW_SLIDE = 0X40000;
            public const int AW_BLEND = 0X80000;

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern int AnimateWindow(IntPtr hwand, int dwTime, int dwFlags);
        }

    }
}
