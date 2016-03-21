using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LWDicer.Control
{
    public interface IIO
    {
        /// <summary>
        /// Initialize
        /// </summary>
        /// <returns></returns>
        int Initialize();

        /// <summary>
        /// Incoming Buffer를 Update하고, Outgoing Buffer의 내용을 Physical I/O에 적용하는 IOThread를 Run한다.
        /// </summary>
        //void RunIOThread();

        /// <summary>
        /// Stop Communication and close driver handle
        /// </summary>
        /// <returns></returns>
        int Terminate();

        //////////////////////////////////////////////////
        // Get & Set Bit
        int GetBit(int addr, out bool bStatus);
        int IsOn(int addr, out bool bStatus);
        int IsOff(int addr, out bool bStatus);

        int SetBit(int addr, bool bStatus);
        int OutputOn(int addr);
        int OutputOff(int addr);
        int OutputToggle(int addr);

        int GetBit(string strAddr, out bool bStatus);
        int IsOn(string strAddr, out bool bStatus);
        int IsOff(string strAddr, out bool bStatus);

        int SetBit(string strAddr, bool bStatus);
        int OutputOn(string strAddr);
        int OutputOff(string strAddr);
        int OutputToggle(string strAddr);

        //////////////////////////////////////////////////
        // Get & Put value
        int GetInt16(int addr, out Int16 value);
        int GetInt32(int addr, out Int32 value);
        int GetFloat(int addr, out double value);

        int SetInt16(int addr, Int16 value);
        int SetInt32(int addr, Int32 value);
        int SetFloat(int addr, double value);
    }
}
