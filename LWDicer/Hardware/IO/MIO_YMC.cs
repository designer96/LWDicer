using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using MotionYMC;

using static LWDicer.Control.DEF_Common;
using static LWDicer.Control.DEF_Error;
using static LWDicer.Control.DEF_IO;

namespace LWDicer.Control
{
    public class MIO_YMC : MObject, IIO
    {
        UInt32 m_hController; // Yaskawa controller handle

        MTickTimer m_waitTimer = new MTickTimer();
        Thread m_hThread;   // Thread Handle

        public enum EYMCRegisterType
        {
            S,  // System
            M,  // Data
            I,  // Input
            O,  // Output
            C,  // Constant
            D,  // D Register
        }

        public enum EYMCDataType
        {
            B,  // bit
            W,  // int
            L,  // long int
            F,  // float
        }

        public MIO_YMC(CObjectInfo objInfo, UInt32 hController = 0)
            : base(objInfo)
        {
            m_hController = hController;
        }

        ~MIO_YMC()
        {
        }

        /// <summary>
        /// Initialize
        /// </summary>
        /// <returns></returns>
        public int Initialize()
        {
            return SUCCESS;
        }

        public int ThreadStart()
        {
            m_hThread = new Thread(ThreadProcess);
            m_hThread.Start();

            return DEF_Error.SUCCESS;
        }

        public int ThreadStop()
        {
            m_hThread.Abort();

            return DEF_Error.SUCCESS;
        }

        public void ThreadProcess()
        {
            while (true)
            {
#if SIMULATION_MOTION
                GetAllServoStatus();
#endif

                Sleep(DEF_Thread.ThreadSleepTime);
            }
        }

        /// <summary>
        /// Stop Communication and close driver handle
        /// </summary>
        /// <returns></returns>
        public int Terminate()
        {
            return SUCCESS;
        }

        int GetRegisterDataHandle(int addr, EYMCDataType type, out uint hDataHandle)
        {
            Debug.Assert(addr >= INPUT_ORIGIN && addr <= OUTPUT_END);
            hDataHandle = 0;
#if SIMULATION_IO
            return SUCCESS;
#endif

            string registerName;
            // register type
            if (addr < OUTPUT_ORIGIN) registerName = EYMCRegisterType.I.ToString();
            else registerName = EYMCRegisterType.O.ToString();

            // data type
            registerName += type.ToString();

            // address 계산 부분
            registerName += "0000";

            // get handle
            uint rc = CMotionAPI.ymcGetRegisterDataHandle(registerName, ref hDataHandle);
            if (rc != CMotionAPI.MP_SUCCESS)
            {
                string str = $"Error ymcGetRegisterDataHandle ML \nErrorCode [ 0x{rc.ToString("X")} ]";
                WriteLog(str, ELogType.Debug, ELogWType.Error, true);
                return GenerateErrorCode(ERR_YMC_FAIL_GET_DATA_HANDLE);
            }

            return SUCCESS;
        }

        int GetRegisterData(int addr, EYMCDataType type, uint RegisterDataNumber, out Int16[] Reg_ShortData, out Int32[] Reg_LongData)
        {
            Debug.Assert(addr >= INPUT_ORIGIN && addr <= OUTPUT_END);
            Reg_ShortData = new Int16[RegisterDataNumber];
            Reg_LongData = new Int32[RegisterDataNumber];
#if SIMULATION_IO
            return SUCCESS;
#endif

            uint hDataHandle;
            int iResult = GetRegisterDataHandle(addr, type, out hDataHandle);
            if (iResult != SUCCESS) return iResult;

            UInt32 ReadDataNumber = 0;                 // Number of obtained registers
            //UInt32 RegisterDataNumber = 1;             // Number of read-in registers
            //Int16[] Reg_ShortData = new Int16[1];      // W or B size register data storage variable
            //Int32[] Reg_LongData = new Int32[1];       // L size register data storage variable
            uint rc;

            if(type == EYMCDataType.B || type == EYMCDataType.W)
            {
                rc = CMotionAPI.ymcGetRegisterData(hDataHandle, RegisterDataNumber, Reg_ShortData, ref ReadDataNumber);
            }
            else
            {
                rc = CMotionAPI.ymcGetRegisterData(hDataHandle, RegisterDataNumber, Reg_LongData, ref ReadDataNumber);
            }

            if (rc != CMotionAPI.MP_SUCCESS)
            {
                string str = $"Error ymcGetRegisterData MB \nErrorCode [ 0x{rc.ToString("X")} ]";
                WriteLog(str, ELogType.Debug, ELogWType.Error, true);
                return GenerateErrorCode(ERR_YMC_FAIL_GET_DATA);
            }

            return SUCCESS;
        }

        int SetRegisterData(int addr, EYMCDataType type, uint RegisterDataNumber, Int16[] Reg_ShortData, Int32[] Reg_LongData)
        {
            Debug.Assert(addr >= INPUT_ORIGIN && addr <= OUTPUT_END);
#if SIMULATION_IO
            return SUCCESS;
#endif

            uint hDataHandle;
            int iResult = GetRegisterDataHandle(addr, type, out hDataHandle);
            if (iResult != SUCCESS) return iResult;

            UInt32 ReadDataNumber = 0;                 // Number of obtained registers
            //UInt32 RegisterDataNumber = 1;             // Number of read-in registers
            //Int16[] Reg_ShortData = new Int16[1];      // W or B size register data storage variable
            //Int32[] Reg_LongData = new Int32[1];       // L size register data storage variable
            uint rc;

            if (type == EYMCDataType.B || type == EYMCDataType.W)
            {
                rc = CMotionAPI.ymcSetRegisterData(hDataHandle, RegisterDataNumber, Reg_ShortData);
            }
            else
            {
                rc = CMotionAPI.ymcSetRegisterData(hDataHandle, RegisterDataNumber, Reg_LongData);
            }

            if (rc != CMotionAPI.MP_SUCCESS)
            {
                string str = $"Error ymcSetRegisterData MB \nErrorCode [ 0x{rc.ToString("X")} ]";
                WriteLog(str, ELogType.Debug, ELogWType.Error, true);
                return GenerateErrorCode(ERR_YMC_FAIL_SET_DATA);
            }

            return SUCCESS;
        }

        //////////////////////////////////////////////////
        // Get & Set Bit
        public int GetBit(int addr, out bool bStatus)
        {
            Debug.Assert(addr >= INPUT_ORIGIN && addr <= OUTPUT_END);
            bStatus = false;

            Int16[] Reg_ShortData;      // W or B size register data storage variable
            Int32[] Reg_LongData;       // L size register data storage variable
            int iResult = GetRegisterData(addr, EYMCDataType.B, 1, out Reg_ShortData, out Reg_LongData);
            if (iResult != SUCCESS) return iResult;

            if (Reg_ShortData[0] == TRUE) bStatus = true;
            else bStatus = false;

            return SUCCESS;
        }

        public int IsOn(int addr, out bool bStatus)
        {
            bStatus = false;
            bool bTemp;
            int iResult = GetBit(addr, out bTemp);
            if (iResult != SUCCESS) return iResult;

            if (bTemp == true) bStatus = true;
            else bStatus = false;

            return SUCCESS;
        }
        public int IsOff(int addr, out bool bStatus)
        {
            bStatus = false;
            bool bTemp;
            int iResult = GetBit(addr, out bTemp);
            if (iResult != SUCCESS) return iResult;

            if (bTemp == false) bStatus = true;
            else bStatus = false;

            return SUCCESS;
        }

        public int SetBit(int addr, bool bStatus)
        {
            Debug.Assert(addr >= INPUT_ORIGIN && addr <= OUTPUT_END);

            UInt32 RegisterDataNumber = 1;             // Number of read-in registers
            Int16[] Reg_ShortData = new Int16[RegisterDataNumber];  // W or B size register data storage variable
            Int32[] Reg_LongData = new Int32[RegisterDataNumber];       // L size register data storage variable

            if (bStatus == true) Reg_ShortData[0] = TRUE;
            else Reg_ShortData[0] = FALSE;

            int iResult = SetRegisterData(addr, EYMCDataType.B, RegisterDataNumber, Reg_ShortData, Reg_LongData);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int OutputOn(int addr)
        {
            int iResult = SetBit(addr, true);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }
        public int OutputOff(int addr)
        {
            int iResult = SetBit(addr, false);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int OutputToggle(int addr)
        {
            bool bStatus;
            int iResult = IsOn(addr, out bStatus);
            if (iResult != SUCCESS) return iResult;

            bStatus = !bStatus;
            iResult = SetBit(addr, bStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int GetBit(string strAddr, out bool bStatus)
        {
            return GetBit(CUtils.IntTryParse(strAddr), out bStatus);
        }
        public int IsOn(string strAddr, out bool bStatus)
        {
            return IsOn(CUtils.IntTryParse(strAddr), out bStatus);
        }
        public int IsOff(string strAddr, out bool bStatus)
        {
            return IsOff(CUtils.IntTryParse(strAddr), out bStatus);
        }

        public int SetBit(string strAddr, bool bStatus)
        {
            return SetBit(CUtils.IntTryParse(strAddr), bStatus);
        }
        public int OutputOn(string strAddr)
        {
            return OutputOn(CUtils.IntTryParse(strAddr));
        }
        public int OutputOff(string strAddr)
        {
            return OutputOff(CUtils.IntTryParse(strAddr));
        }
        public int OutputToggle(string strAddr)
        {
            return OutputToggle(CUtils.IntTryParse(strAddr));
        }

        //////////////////////////////////////////////////
        // Get & Put value
        public int GetInt16(int addr, out Int16 value)
        {
            Debug.Assert(addr >= INPUT_ORIGIN && addr <= OUTPUT_END);
            value = 0;

            Int16[] Reg_ShortData;      // W or B size register data storage variable
            Int32[] Reg_LongData;       // L size register data storage variable
            int iResult = GetRegisterData(addr, EYMCDataType.W, 1, out Reg_ShortData, out Reg_LongData);
            if (iResult != SUCCESS) return iResult;

            value = Reg_ShortData[0];

            return SUCCESS;
        }

        public int GetInt32(int addr, out Int32 value)
        {
            Debug.Assert(addr >= INPUT_ORIGIN && addr <= OUTPUT_END);
            value = 0;

            Int16[] Reg_ShortData;      // W or B size register data storage variable
            Int32[] Reg_LongData;       // L size register data storage variable
            int iResult = GetRegisterData(addr, EYMCDataType.L, 1, out Reg_ShortData, out Reg_LongData);
            if (iResult != SUCCESS) return iResult;

            value = Reg_LongData[0];

            return SUCCESS;
        }

        public int GetFloat(int addr, out double value)
        {
            // 나중에 구현
            value = 0;

            return GenerateErrorCode(ERR_YMC_NOT_SUPPORT_FUNCTION);
            return SUCCESS;
        }

        public int SetInt16(int addr, Int16 value)
        {
            Debug.Assert(addr >= INPUT_ORIGIN && addr <= OUTPUT_END);

            UInt32 RegisterDataNumber = 1;             // Number of read-in registers
            Int16[] Reg_ShortData = new Int16[RegisterDataNumber];  // W or B size register data storage variable
            Int32[] Reg_LongData = new Int32[RegisterDataNumber];       // L size register data storage variable

            Reg_ShortData[0] = value;

            int iResult = SetRegisterData(addr, EYMCDataType.W, RegisterDataNumber, Reg_ShortData, Reg_LongData);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int SetInt32(int addr, Int32 value)
        {
            Debug.Assert(addr >= INPUT_ORIGIN && addr <= OUTPUT_END);

            UInt32 RegisterDataNumber = 1;             // Number of read-in registers
            Int16[] Reg_ShortData = new Int16[RegisterDataNumber];  // W or B size register data storage variable
            Int32[] Reg_LongData = new Int32[RegisterDataNumber];       // L size register data storage variable

            Reg_LongData[0] = value;

            int iResult = SetRegisterData(addr, EYMCDataType.L, RegisterDataNumber, Reg_ShortData, Reg_LongData);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int SetFloat(int addr, double value)
        {
            // 나중에 구현

            return GenerateErrorCode(ERR_YMC_NOT_SUPPORT_FUNCTION);
            return SUCCESS;
        }

    }
}
