using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MotionYMC;

using static LWDicer.Control.DEF_System;
using static LWDicer.Control.DEF_Common;
using static LWDicer.Control.DEF_Error;
using static LWDicer.Control.DEF_Yaskawa;
using static LWDicer.Control.DEF_IO;
using static LWDicer.Control.DEF_Motion;

namespace LWDicer.Control
{
    public class DEF_Yaskawa
    {
        public const int ERR_YASKAWA_INVALID_CONTROLLER                  = 1;
        public const int ERR_YASKAWA_FAIL_OPEN_YMC                       = 2;
        public const int ERR_YASKAWA_FAIL_SET_TIMEOUT                    = 3;
        public const int ERR_YASKAWA_FAIL_CHANGE_CONTROLLER              = 4;
        public const int ERR_YASKAWA_FAIL_CLEAR_ALL_AXIS                 = 5;
        public const int ERR_YASKAWA_FAIL_DECLARE_AXIS                   = 6;
        public const int ERR_YASKAWA_FAIL_DECLARE_DEVICE                 = 7;
        public const int ERR_YASKAWA_FAIL_SERVO_ON                       = 8;
        public const int ERR_YASKAWA_FAIL_SERVO_OFF                      = 9;
        public const int ERR_YASKAWA_FAIL_RESET_ALARM                    = 10;
        public const int ERR_YASKAWA_FAIL_GET_MOTION_PARAM               = 11;
        public const int ERR_YASKAWA_FAIL_SERVO_STOP                     = 12;
        public const int ERR_YASKAWA_SERVO_DETECTED_PLUS_LIMIT           = 13;
        public const int ERR_YASKAWA_SERVO_DETECTED_MINUS_LIMIT          = 14;
        public const int ERR_YASKAWA_FAIL_SERVO_MOVE_JOG                 = 15;
        public const int ERR_YASKAWA_FAIL_SERVO_MOVE_DRIVING_POSITIONING = 16;
        public const int ERR_YASKAWA_FAIL_SERVO_MOVE_HOME                = 17;
        public const int ERR_YASKAWA_FAIL_SERVO_GET_POS                  = 18;
        public const int ERR_YASKAWA_FAIL_GET_REGISTER_DATA_HANDLE       = 19;
        public const int ERR_YASKAWA_FAIL_GET_REGISTER_DATA              = 20;

        public const int MAX_MP_CPU = 4;    // pci board EA
        public const int MAX_MP_PORT = 2;   // ports per board
        public const int MP_AXIS_PER_PORT = 16; // physical axis per port
        public const int MP_AXIS_PER_CPU = MAX_MP_PORT * MP_AXIS_PER_PORT; // physical axis per cpu
        public const int MAX_MP_AXIS = MAX_MP_CPU * MAX_MP_PORT * MP_AXIS_PER_PORT;

        public const int UNIT_REF = 1000;// 0.001 Reference Unit( 1mm )

        public enum ERegister
        {
            S,  // System
            M,  // Data
            I,  // Input
            O,  // Output
            C,  // Constant
            D,  // D Register
        }

        public enum EData
        {
            B,  // bit
            W,  // int
            L,  // long int
            F,  // float
        }

        public enum EMPBoard
        {
            // cpu 갯수는 board의 갯수
            // port 1 : physical axis 1~16, logical axis 1~16
            // port 2 : physical axis 1~16 * 2EA, logical axis 1~32
            MP2100,     // cpu 1, port 1
            MP2100M,    // cpu 1, port 2
            MP2101,
            MP2101M,
            MP2101T,
            MP2101TM,   // cpu 1, port 2
        }

        public struct ServoStatusStruct
        {
            public double EncoderValue;
            public double Velocity;     //Servo 현재 속도
            public bool Ready;
            public bool Alarm;
            public bool Limit_N;
            public bool Limit_P;
            public bool Origin;
            public bool ServoOn;
            public int LoadFactor;
            public int AlarmCode;
        }

        public class CMPMotionData
        {
            // General
            public string Name; // Name of Axis
            public bool Exist;    // Use of Axis. if false, Axis not exist.

            // Speed
            public double MaxVelocity;                // Maximum feeding speed [reference unit/s]
            public double Acceleration;               // Acceleration [reference unit/s2], acceleration time constant [ms]
            public double Deceleration;               // Deceleration [reference unit/s2], deceleration time constant [ms]
            public double Velocity;                   // Feeding speed [reference unit/s], Offset speed

            // Home
            public UInt16 HomeMethod = (UInt16)CMotionAPI.ApiDefs.HMETHOD_INPUT_C;
            public UInt16 HomeDir = (UInt16)CMotionAPI.ApiDefs.DIRECTION_NEGATIVE; // Home Direction
            public double ApproachVelocity;             // Approach speed [reference unit/s], 원점복귀 접근 속도
            public double CreepVelocity;                // Creep speed [reference unit/s], C상 pulse rising -> falling 이동 속도
            public double HomeOffset;                   // C상 pulse falling후의 원점 복귀 offset

            // Jog Speed
            public double Jog_Acceleration;               // Acceleration [reference unit/s2], acceleration time constant [ms]
            public double Jog_Deceleration;               // Deceleration [reference unit/s2], deceleration time constant [ms]
            public double Jog_Fast_Velocity;
            public double Jog_Slow_Velocity;

            // below list is defined in MOTION_DATA of YMCMotion
            public Int16 CoordinateSystem;           // Coordinate system specified
            public Int16 MoveType;                   // Motion type
            public Int16 VelocityType;               // Speed type
            public Int16 AccDecType;                 // Acceleration type
            public Int16 FilterType;                 // Filter type
            public Int16 DataType;                   // Data type (0: immediate, 1: indirect designation)
            public Int32 FilterTime;                 // Filter time [ms]
            //public Int32 MaxVelocity;                // Maximum feeding speed [reference unit/s]
            //public Int32 Acceleration;               // Acceleration [reference unit/s2], acceleration time constant [ms]
            //public Int32 Deceleration;               // Deceleration [reference unit/s2], deceleration time constant [ms]
            //public Int32 Velocity;                   // Feeding speed [reference unit/s], Offset speed
            //public Int32 ApproachVelocity;           // Approach speed [reference unit/s]
            //public Int32 CreepVelocity;              // Creep speed [reference unit/s]

            public CMPMotionData()
            {
                // General
                Name = "Non Use";
                Exist = false;

                // Speed
                MaxVelocity = 100;
                Acceleration = 100;  //Acceleration time constant [ms] 
                Deceleration = 100;  // Deceleration time constant [ms]
                Velocity = 10;		// Speed [reference unit/s]					
                ApproachVelocity = 5;		// Speed [reference unit/s]					
                CreepVelocity = 1;      // Speed [reference unit/s]	

                Jog_Acceleration = Acceleration;
                Jog_Deceleration = Deceleration;
                Jog_Fast_Velocity = Jog_Slow_Velocity = Velocity;				

                // MOTION_DATA
                CoordinateSystem = (Int16)CMotionAPI.ApiDefs.WORK_SYSTEM;
                MoveType = (Int16)CMotionAPI.ApiDefs.MTYPE_RELATIVE;

                FilterTime = 10;                // Filter time [0.1 ms]
                VelocityType = (Int16)CMotionAPI.ApiDefs.VTYPE_UNIT_PAR;    // Speed [reference unit/s]
                AccDecType = (Int16)CMotionAPI.ApiDefs.ATYPE_UNIT_PAR;  // Time constant specified [ms] //ATYPE_TIME
                FilterType = (Int16)CMotionAPI.ApiDefs.FTYPE_S_CURVE;   // Moving average filter (simplified S-curve)
                DataType = 0;                                           // All parameters directly specified
            }
            
            public void GetMotionData(ref CMotionAPI.MOTION_DATA s)
            {
                // speed value를 UNIT_REF 적용해서 MOTION_DATA로 변환 
                s.CoordinateSystem = CoordinateSystem;
                s.MoveType         = MoveType;
                s.VelocityType     = VelocityType;
                s.AccDecType       = AccDecType;
                s.FilterType       = FilterType;
                s.DataType         = DataType;
                s.FilterTime       = FilterTime;
                s.MaxVelocity      = (int)MaxVelocity * UNIT_REF;
                s.Acceleration     = (int)Acceleration * UNIT_REF;
                s.Deceleration     = (int)Deceleration * UNIT_REF;
                s.Velocity         = (int)Velocity * UNIT_REF;
                s.ApproachVelocity = (int)ApproachVelocity * UNIT_REF;
                s.CreepVelocity    = (int)CreepVelocity * UNIT_REF;
            }

            public void GetMotionData_Jog(ref CMotionAPI.MOTION_DATA s, bool bJogFastMove = false)
            {
                GetMotionData(ref s);

                s.Acceleration = (int)Jog_Acceleration * UNIT_REF;
                s.Deceleration = (int)Jog_Deceleration * UNIT_REF;
                if (bJogFastMove == true)
                {
                    s.Velocity = (int)Jog_Fast_Velocity * UNIT_REF;
                }
                else
                {
                    s.Velocity = (int)Jog_Slow_Velocity * UNIT_REF;
                }
            }

            public void GetMotionData_Home(ref CMotionAPI.MOTION_DATA s, 
                out UInt16 Method, out UInt16 Dir, out CMotionAPI.POSITION_DATA Position)
            {
                GetMotionData(ref s);

                Method = this.HomeMethod;
                Dir = this.HomeDir;
                Position.PositionData = (int)HomeOffset * UNIT_REF;
                Position.DataType = (UInt16)CMotionAPI.ApiDefs.DATATYPE_IMMEDIATE;
            }

            public void SetMotionData(CMotionAPI.MOTION_DATA s)
            {
                // speed value를 UNIT_REF 적용해서 data로 변환 
                CoordinateSystem = s.CoordinateSystem;
                MoveType         = s.MoveType;
                VelocityType     = s.VelocityType;
                AccDecType       = s.AccDecType;
                FilterType       = s.FilterType;
                DataType         = s.DataType;
                FilterTime       = s.FilterTime;
                MaxVelocity      = s.MaxVelocity / UNIT_REF;
                Acceleration     = s.Acceleration / UNIT_REF;
                Deceleration     = s.Deceleration / UNIT_REF;
                Velocity         = s.Velocity / UNIT_REF;
                ApproachVelocity = s.ApproachVelocity / UNIT_REF;
                CreepVelocity    = s.CreepVelocity / UNIT_REF;
            }
    }

        public class CMPRackTable
        {
            public int RackNo = 1;
            public int SlotNo = 0;
            public int SubSlotNo = 3;
        }


        public class CYaskawaRefComp
        {

        }

        public class CMPBoard
        {
            public int CPUIndex;
            public EMPBoard Type;
            public int SlotLength;

            public CMPMotionData[] MotionData = new CMPMotionData[MP_AXIS_PER_CPU];

            // SVB, SVB-01, SVC, SVC-01
            public CMPRackTable[] SPort = new CMPRackTable[MAX_MP_PORT];
            public CMPRackTable VPort;  // SVR, Virtual Port

            public CMPBoard(int CPUIndex, EMPBoard Type, CMPMotionData[] motions)
            {
                this.CPUIndex = CPUIndex;
                this.Type = Type;
                switch (Type)
                {
                    case EMPBoard.MP2100:
                    case EMPBoard.MP2101:
                    case EMPBoard.MP2101T:
                        SPort[0].RackNo = 1;
                        SPort[0].SlotNo = 0;
                        SPort[0].SubSlotNo = 3;

                        VPort.RackNo = 1;
                        VPort.SlotNo = 0;
                        VPort.SubSlotNo = 4;

                        SlotLength = 1;
                        break;
                    case EMPBoard.MP2100M:
                    case EMPBoard.MP2101M:
                    case EMPBoard.MP2101TM:
                        SPort[0].RackNo = 1;
                        SPort[0].SlotNo = 0;
                        SPort[0].SubSlotNo = 3;

                        SPort[1].RackNo = 1;
                        SPort[1].SlotNo = 1;
                        SPort[1].SubSlotNo = 1;

                        VPort.RackNo = 1;
                        VPort.SlotNo = 0;
                        VPort.SubSlotNo = 4;

                        SlotLength = 2;
                        break;
                }

                for (int i = 0; i < motions.Length; i++)
                {
                    MotionData[i] = motions[i];
                }
            }

            public string GetMotionRegAddr(int servoNo)
            {
                // I/O W 8000 + (LineNo-1) x 800h + (AxisNo - 1) x 80h
                int addr = 0x8000 + (CPUIndex * 0x800) + servoNo * 0x80;
                string regAddr = addr.ToString("X4");
                return regAddr;
            }

            public void GetMotionData(int servoNo, ref CMotionAPI.MOTION_DATA s)
            {
                servoNo = servoNo % MP_AXIS_PER_CPU;
                MotionData[servoNo].GetMotionData(ref s);
            }

            public void GetMotionData_Jog(int servoNo, ref CMotionAPI.MOTION_DATA s, bool bJogFastMove = false)
            {
                servoNo = servoNo % MP_AXIS_PER_CPU;
                MotionData[servoNo].GetMotionData_Jog(ref s, bJogFastMove);
            }

            public void GetMotionData_Home(int servoNo, ref CMotionAPI.MOTION_DATA s,
                out UInt16 Method, out UInt16 Dir, out CMotionAPI.POSITION_DATA Position)
            {
                servoNo = servoNo % MP_AXIS_PER_CPU;
                MotionData[servoNo].GetMotionData_Home(ref s, out Method, out Dir, out Position);
            }
        }

        public class CYaskawaData
        {
            //
            public int CpuNo = 1;       // PCI 모드일때는 보드 숫자라고 생각하면 됨
            public int PortNo = 1;      // MP Background 프로그램에서 설정하는 communication port number

            public CMPBoard[] MPBoard = new CMPBoard[MAX_MP_CPU];

            public CYaskawaData()
            {

            }

            public CYaskawaData(int CpuNo, int PortNo, params CMPBoard[] boards)
            {
                this.CpuNo = CpuNo;
                this.PortNo = PortNo;

                MPBoard = new CMPBoard[boards.Length];
                for(int i = 0; i < boards.Length; i++)
                {
                    MPBoard[i] = boards[i];
                }
            }
        }
    }

    /// <summary>
    /// 우선 작성. ChangeController 함수는 만들어는 놨으나 호출은 고려하지 않고 우선 작성함
    /// 사용한 Yaskawa의 라이브러리는 아래와 같음
    /// English Version Released on September 20 2013
    /// Package version  :  Ver.2.00
    /// API DLL version  :  Ver.2.0.0.0
    /// Driver version     :  Ver.2.0.0.0
    /// Applicable firmware version v2.46 or later
    /// </summary>
    public class MYaskawa : MObject, IDisposable
    {
        private CYaskawaRefComp m_RefComp;
        private CYaskawaData m_Data;
        public int InstalledAxisNo; // System에 Install된 max axis

        public string LastHWMessage { get; private set; }

        MTickTimer m_waitTimer = new MTickTimer();
        UInt16 APITimeOut = 5000;

        UInt32[] m_hController = new UInt32[MAX_MP_CPU]; // Yaskawa controller handle
        UInt32[] m_hAxis = new UInt32[MAX_MP_AXIS];  // Axis handle
        UInt32[] m_hDevice = new UInt32[MAX_MP_AXIS];    // Device handle
        public ServoStatusStruct[] ServoStatus = new ServoStatusStruct[MAX_MP_AXIS];

        public MYaskawa(CObjectInfo objInfo, CYaskawaRefComp refComp, CYaskawaData data)
            : base(objInfo)
        {
            m_RefComp = refComp;
            SetData(data);
        }

        ~MYaskawa()
        {
            Dispose();
        }

        public void Dispose()
        {
            CloseController();

        }

        public int SetData(CYaskawaData source)
        {
            m_Data = ObjectExtensions.Copy(source);
            InstalledAxisNo = m_Data.CpuNo * MP_AXIS_PER_CPU;

            return SUCCESS;
        }

        public int GetData(out CYaskawaData target)
        {
            target = ObjectExtensions.Copy(m_Data);

            return SUCCESS;
        }

        private int GetDeviceLength(int deviceNo)
        {
            int length = 1;
            if (deviceNo < (int)EYMC_Device.STAGE1)
            {
                length = 1;
            }
            else
            {
                switch (deviceNo)
                {
                    case (int)EYMC_Device.STAGE1:
                        length = 3;
                        break;

                    case (int)EYMC_Device.LOADER:
                        length = 1;
                        break;

                    case (int)EYMC_Device.PUSHPULL:
                        length = 1;
                        break;

                    case (int)EYMC_Device.HANDLER:
                        length = 1;
                        break;
                }
            }

            return length;
        }

        private int GetDeviceWait(int deviceNo, out UInt16[] waits, ushort mode = (ushort)CMotionAPI.ApiDefs.POSITIONING_COMPLETED)
        {
            int length = GetDeviceLength(deviceNo);
            waits = new ushort[length];
            for (int i = 0; i < waits.Length; i++)
            {
                waits[i] = mode;
            }
            return SUCCESS;
        }

        private int GetDeviceAxis(int deviceNo, out UInt32[] hAxis)
        {
            int length = GetDeviceLength(deviceNo);
            hAxis = new UInt32[length];
            if (deviceNo < (int)EYMC_Device.STAGE1)
            {
                hAxis[deviceNo] = m_hAxis[deviceNo];
            }
            else
            {
                switch (deviceNo)
                {
                    case (int)EYMC_Device.STAGE1:
                        hAxis[0] = m_hAxis[(int)EYMC_Axis.STAGE1_X];
                        hAxis[1] = m_hAxis[(int)EYMC_Axis.STAGE1_X];
                        hAxis[2] = m_hAxis[(int)EYMC_Axis.STAGE1_X];
                        break;

                    case (int)EYMC_Device.LOADER:
                        hAxis[0] = m_hAxis[(int)EYMC_Axis.LOADER_Z];
                        break;

                    case (int)EYMC_Device.PUSHPULL:
                        break;

                    case (int)EYMC_Device.HANDLER:
                        break;
                }
            }

            return SUCCESS;
        }

        private int GetDeviceMotionData(int deviceNo, out CMotionAPI.MOTION_DATA[] MotionData)
        {
            int length = GetDeviceLength(deviceNo);
            MotionData = new CMotionAPI.MOTION_DATA[length];

            int boardNo = GetBoardIndex(deviceNo);
            if (deviceNo < (int)EYMC_Device.STAGE1)
            {
                m_Data.MPBoard[boardNo].GetMotionData(deviceNo, ref MotionData[0]);
            }
            else
            {
                switch (deviceNo)
                {
                    case (int)EYMC_Device.STAGE1:
                        boardNo = 0;
                        m_Data.MPBoard[boardNo].GetMotionData((int)EYMC_Axis.STAGE1_X, ref MotionData[0]);
                        m_Data.MPBoard[boardNo].GetMotionData((int)EYMC_Axis.STAGE1_Y, ref MotionData[1]);
                        m_Data.MPBoard[boardNo].GetMotionData((int)EYMC_Axis.STAGE1_T, ref MotionData[2]);
                        break;

                    case (int)EYMC_Device.LOADER:
                        boardNo = 0;
                        m_Data.MPBoard[boardNo].GetMotionData((int)EYMC_Axis.LOADER_Z, ref MotionData[0]);
                        break;

                    case (int)EYMC_Device.PUSHPULL:
                        break;

                    case (int)EYMC_Device.HANDLER:
                        break;
                }
            }

            return SUCCESS;
        }

        private int GetDeviceMotionData_Jog(int deviceNo, out CMotionAPI.MOTION_DATA[] MotionData, bool bJogFastMove = false)
        {
            int length = GetDeviceLength(deviceNo);
            MotionData = new CMotionAPI.MOTION_DATA[length];

            int boardNo = GetBoardIndex(deviceNo);
            if (deviceNo < (int)EYMC_Device.STAGE1)
            {
                m_Data.MPBoard[boardNo].GetMotionData_Jog(deviceNo, ref MotionData[0], bJogFastMove);
            }
            else
            {
                switch (deviceNo)
                {
                    case (int)EYMC_Device.STAGE1:
                        boardNo = 0;
                        m_Data.MPBoard[boardNo].GetMotionData_Jog((int)EYMC_Axis.STAGE1_X, ref MotionData[0], bJogFastMove);
                        m_Data.MPBoard[boardNo].GetMotionData_Jog((int)EYMC_Axis.STAGE1_Y, ref MotionData[1], bJogFastMove);
                        m_Data.MPBoard[boardNo].GetMotionData_Jog((int)EYMC_Axis.STAGE1_T, ref MotionData[2], bJogFastMove);
                        break;

                    case (int)EYMC_Device.LOADER:
                        boardNo = 0;
                        m_Data.MPBoard[boardNo].GetMotionData_Jog((int)EYMC_Axis.LOADER_Z, ref MotionData[0], bJogFastMove);
                        break;

                    case (int)EYMC_Device.PUSHPULL:
                        break;

                    case (int)EYMC_Device.HANDLER:
                        break;
                }
            }

            return SUCCESS;
        }

        private int GetDeviceMotionData_Home(int deviceNo, out CMotionAPI.MOTION_DATA[] MotionData,
            out UInt16[] Method, out UInt16[] Dir, out CMotionAPI.POSITION_DATA[] Position)
        {
            int length = GetDeviceLength(deviceNo);
            MotionData = new CMotionAPI.MOTION_DATA[length];
            Method = new UInt16[length];
            Dir = new UInt16[length];
            Position = new CMotionAPI.POSITION_DATA[length];

            int boardNo = GetBoardIndex(deviceNo);
            if (deviceNo < (int)EYMC_Device.STAGE1)
            {
                m_Data.MPBoard[boardNo].GetMotionData_Home(deviceNo, ref MotionData[0], out Method[0], out Dir[0], out Position[0]);
            }
            else
            {
                switch (deviceNo)
                {
                    case (int)EYMC_Device.STAGE1:
                        boardNo = 0;
                        m_Data.MPBoard[boardNo].GetMotionData_Home((int)EYMC_Axis.STAGE1_X, ref MotionData[0], out Method[0], out Dir[0], out Position[0]);
                        m_Data.MPBoard[boardNo].GetMotionData_Home((int)EYMC_Axis.STAGE1_Y, ref MotionData[1], out Method[1], out Dir[1], out Position[1]);
                        m_Data.MPBoard[boardNo].GetMotionData_Home((int)EYMC_Axis.STAGE1_T, ref MotionData[2], out Method[2], out Dir[2], out Position[2]);
                        break;

                    case (int)EYMC_Device.LOADER:
                        boardNo = 0;
                        m_Data.MPBoard[boardNo].GetMotionData_Home((int)EYMC_Axis.LOADER_Z, ref MotionData[0], out Method[0], out Dir[0], out Position[0]);
                        break;

                    case (int)EYMC_Device.PUSHPULL:
                        break;

                    case (int)EYMC_Device.HANDLER:
                        break;
                }
            }

            return SUCCESS;
        }

        private int GetDevicePositon(int deviceNo, out CMotionAPI.POSITION_DATA[] Position, double[] pos, 
            ushort type = (ushort)CMotionAPI.ApiDefs.DATATYPE_IMMEDIATE)
        {
            int length = GetDeviceLength(deviceNo);
            Position = new CMotionAPI.POSITION_DATA[length];
            for(int i = 0; i < length; i++)
            {
                Position[length].DataType = (ushort)type;
                Position[length].PositionData = (int)(pos[length] * UNIT_REF);
            }

            return SUCCESS;
        }

        private int GetBoardIndex(int servoNo)
        {
            return servoNo / MP_AXIS_PER_CPU;
        }


        public int OpenController(bool bServoOn = false)
        {
            // 0. init
            int iResult;
            UInt32 rc;
            CMotionAPI.COM_DEVICE ComDevice;

            // 1. Open Controller
            for (int i = 0; i < m_Data.CpuNo; i++)
            {
                // Sets the ymcOpenController parameters.		
                ComDevice.ComDeviceType = (UInt16)CMotionAPI.ApiDefs.COMDEVICETYPE_PCI_MODE;
                ComDevice.PortNumber = (UInt16)m_Data.PortNo;
                ComDevice.CpuNumber = Convert.ToUInt16(i + 1);    //cpuno;
                ComDevice.NetworkNumber = 0;
                ComDevice.StationNumber = 0;
                ComDevice.UnitNumber = 0;
                ComDevice.IPAddress = "";
                ComDevice.Timeout = APITimeOut;

                rc = CMotionAPI.ymcOpenController(ref ComDevice, ref m_hController[i]);
                if (rc != CMotionAPI.MP_SUCCESS)
                {
                    string str = String.Format("Error ymcOpenController Board {0} ErrorCode [ 0x{1} ]", i, rc.ToString("X"));
                    WriteLog(str, ELogType.Debug, ELogWType.Error, true);
                    return GenerateErrorCode(ERR_YASKAWA_FAIL_OPEN_YMC);
                }

                // Sets the motion API timeout. 		
                rc = CMotionAPI.ymcSetAPITimeoutValue(30000);
                if (rc != CMotionAPI.MP_SUCCESS)
                {
                    LastHWMessage = String.Format("Error ymcSetAPITimeoutValue ErrorCode [ 0x{0} ]", rc.ToString("X"));
                    WriteLog(LastHWMessage, ELogType.Debug, ELogWType.Error, true);
                    return GenerateErrorCode(ERR_YASKAWA_FAIL_SET_TIMEOUT);
                }

                iResult = ChangeController(i);
                if (iResult != SUCCESS) return iResult;

                // Deletes the axis handle that is held by the Machine Controller.
                rc = CMotionAPI.ymcClearAllAxes();
                if (rc != CMotionAPI.MP_SUCCESS)
                {
                    LastHWMessage = String.Format("Error ClearAllAxes  Board ErrorCode [ 0x{0} ]", rc.ToString("X"));
                    WriteLog(LastHWMessage, ELogType.Debug, ELogWType.Error, true);
                    return GenerateErrorCode(ERR_YASKAWA_FAIL_CLEAR_ALL_AXIS);
                }

                for (int j = 0; j < MP_AXIS_PER_CPU; j++)
                {
                    if (m_Data.MPBoard[i].SlotLength == 1 && j >= MP_AXIS_PER_PORT) break;
                    if (m_Data.MPBoard[i].MotionData[j].Exist == false) continue;

                    int port = 0;
                    if (j >= MP_AXIS_PER_PORT) port = 1;

                    // Logical ServoNo는 cpu마다 1~32 일까 아니면, 1~32, 33~64, 65~96 순서일까.
                    int logicalAxisNo = i * MP_AXIS_PER_CPU + j;
                    string axisName = m_Data.MPBoard[i].MotionData[j].Name;

                    UInt16 rackNo = (ushort)m_Data.MPBoard[i].SPort[port].RackNo;
                    UInt16 slotNo = (ushort)m_Data.MPBoard[i].SPort[port].SlotNo;
                    UInt16 subSlotNo = (ushort)m_Data.MPBoard[i].SPort[port].SubSlotNo;
                    // create axis handle
                    rc = CMotionAPI.ymcDeclareAxis(rackNo, slotNo, subSlotNo,
                        (UInt16)(j+1), (UInt16)(logicalAxisNo+1), (UInt16)CMotionAPI.ApiDefs.REAL_AXIS, 
                        axisName, ref m_hAxis[logicalAxisNo]);
                    if (rc != CMotionAPI.MP_SUCCESS)
                    {
                        LastHWMessage = String.Format("Error ymcDeclareAxis Board  ErrorCode [ 0x{0} ]", rc.ToString("X"));
                        WriteLog(LastHWMessage, ELogType.Debug, ELogWType.Error, true);
                        return GenerateErrorCode(ERR_YASKAWA_FAIL_DECLARE_AXIS);
                    }
                }
            }

            // create device handle
            for (int i = 0; i < (int)EYMC_Device.MAX; i++)
            {
                UInt32[] hAxis;
                iResult = GetDeviceAxis(i, out hAxis);
                //if (iResult != SUCCESS) return iResult;
                
                rc = CMotionAPI.ymcDeclareDevice((UInt16)GetDeviceLength(i), hAxis, ref m_hDevice[i]);
                if (rc != CMotionAPI.MP_SUCCESS)
                {
                    LastHWMessage = String.Format("Error ymcDeclareDevice  Board ErrorCode [ 0x{0} ]", rc.ToString("X"));
                    WriteLog(LastHWMessage, ELogType.Debug, ELogWType.Error, true);
                    return GenerateErrorCode(ERR_YASKAWA_FAIL_DECLARE_DEVICE);
                }

                // servo on
                if(bServoOn == true)
                {
                    rc = CMotionAPI.ymcServoControl(m_hDevice[i], (UInt16)CMotionAPI.ApiDefs.SERVO_ON, APITimeOut);
                    if (rc != CMotionAPI.MP_SUCCESS)
                    {
                        LastHWMessage = String.Format("Error ymcServoControl Board  ErrorCode [ 0x{0} ]", rc.ToString("X"));
                        WriteLog(LastHWMessage, ELogType.Debug, ELogWType.Error, true);
                        return GenerateErrorCode(ERR_YASKAWA_FAIL_SERVO_ON);
                    }
                }
            }
            return SUCCESS;
        }

        public int CloseController()
        {
            for (int i = 0; i < m_Data.CpuNo; i++)
            {
                if (m_hController[i] == 0) continue;
                uint rc = CMotionAPI.ymcCloseController(m_hController[i]);
                if (rc != CMotionAPI.MP_SUCCESS)
                {
                    LastHWMessage = String.Format("Error ymcCloseController Board {0} ErrorCode [ 0x{1} ]", i, rc.ToString("X"));
                    WriteLog(LastHWMessage, ELogType.Debug, ELogWType.Error, true);
                }
            }

            return SUCCESS;
        }

        private int ChangeController(int cpuIndex)
        {
            if (cpuIndex >= m_hController.Length || m_hController[cpuIndex] == 0)
            {
                return GenerateErrorCode(ERR_YASKAWA_INVALID_CONTROLLER);
            }

            UInt32 hCurrent = 0;
            uint rc = CMotionAPI.ymcGetController(ref hCurrent);
            if (rc != CMotionAPI.MP_SUCCESS)
            {
                LastHWMessage = String.Format("Error ymcGetController Board : ErrorCode [ 0x{0} ]", rc.ToString("X"));
                WriteLog(LastHWMessage, ELogType.Debug, ELogWType.Error, true);
                return GenerateErrorCode(ERR_YASKAWA_FAIL_CHANGE_CONTROLLER);
            }
            if (hCurrent == m_hController[cpuIndex]) return SUCCESS;

            rc = CMotionAPI.ymcSetController(m_hController[cpuIndex]);
            if (rc != CMotionAPI.MP_SUCCESS)
            {
                LastHWMessage = String.Format("Error ymcSetController Board : ErrorCode [ 0x{0} ]", rc.ToString("X"));
                WriteLog(LastHWMessage, ELogType.Debug, ELogWType.Error, true);
                return GenerateErrorCode(ERR_YASKAWA_FAIL_CHANGE_CONTROLLER);
            }

            return SUCCESS;
        }

        private int ChangeControllerByServo(int servoNo)
        {
            int cpuIndex = servoNo / (MP_AXIS_PER_CPU);

            return ChangeController(cpuIndex);
        }

        public void GetAllServoStatus()
        {
            for (int i = 0; i < InstalledAxisNo ; i++)
            {
                GetServoStatus(i);
            }

        }

        /// <summary>
        /// 속도 체크 필요함
        /// </summary>
        /// <param name="servoNo"></param>
        public void GetServoStatus(int servoNo)
        {
            UInt32 rc = 0;
            UInt32 returnValue = 0;

            int iResult = ChangeControllerByServo(servoNo);
            //if (iResult != SUCCESS) return iResult;

            ////Servo Position 값 Read 
            rc = CMotionAPI.ymcGetMotionParameterValue(m_hAxis[servoNo], (UInt16)CMotionAPI.ApiDefs.MONITOR_PARAMETER,
                    (UInt16)CMotionAPI.ApiDefs_MonPrm.SER_APOS, ref returnValue); //Machine coordinate system feedback position (APOS)
            if (rc == CMotionAPI.MP_SUCCESS)
                ServoStatus[servoNo].EncoderValue = (double)returnValue / UNIT_REF;

            //Servo 속도값 Read 
            rc = CMotionAPI.ymcGetMotionParameterValue(m_hAxis[servoNo], (UInt16)CMotionAPI.ApiDefs.MONITOR_PARAMETER,
                    (UInt16)CMotionAPI.ApiDefs_MonPrm.SER_FSPD, ref returnValue);
            if (rc == CMotionAPI.MP_SUCCESS)
                ServoStatus[servoNo].Velocity = (double)returnValue / UNIT_REF;

            //Servo Status Read 
            rc = CMotionAPI.ymcGetMotionParameterValue(m_hAxis[servoNo], (UInt16)CMotionAPI.ApiDefs.MONITOR_PARAMETER,    //110208
                    (UInt16)CMotionAPI.ApiDefs_MonPrm.SER_RUNSTS, ref returnValue);
            if (rc == CMotionAPI.MP_SUCCESS)
            {
                //Servo Ready
                ServoStatus[servoNo].Ready = Convert.ToBoolean((returnValue >> 3) & 0x1);
                //Servo On/Off
                ServoStatus[servoNo].ServoOn = Convert.ToBoolean((returnValue >> 1) & 0x1);
            }

            //Servo Alarm Read 
            rc = CMotionAPI.ymcGetMotionParameterValue(m_hAxis[servoNo], (UInt16)CMotionAPI.ApiDefs.MONITOR_PARAMETER,    //110208
                    (UInt16)CMotionAPI.ApiDefs_MonPrm.SER_ALARM, ref returnValue);
            if (rc == CMotionAPI.MP_SUCCESS)
            {
                //Servo Alarm
                ServoStatus[servoNo].Alarm = Convert.ToBoolean(returnValue != 0);
            }

            //UInt16[] reg_IW = new UInt16[1];
            //UInt32 numOfReadData = 0;

            ////============================================================================
            //// Gets the IW Register handle.	
            ////============================================================================
            //rc = CMotionAPI.ymcGetRegisterDataHandle("IW8000", ref g_hRegister_IW);
            //if (rc != CMotionAPI.MP_SUCCESS)
            //{
            //    MessageBox.Show(String.Format("Error ymcGetRegisterDataHandle IW ErrorCode [ 0x{0} ]", rc.ToString("X"));
            //    return;
            //}
            ////Motion Controller 상태 Read
            //rc = CMotionAPI.ymcGetRegisterData(g_hRegister_IW, 1, reg_IW, ref numOfReadData);
            //if (rc == CMotionAPI.MP_SUCCESS)
            //{
            //    if ((reg_IW[0] & 0x01) == 1)
            //        MotionStatus = 1;  //Ready
            //    else
            //        MotionStatus = 0;
            //}


            ////Warning 
            //rc = CMotionAPI.ymcGetMotionParameterValue(m_hAxis[servoNo], (UInt16)CMotionAPI.ApiDefs.MONITOR_PARAMETER,    //110208
            //     (UInt16)CMotionAPI.ApiDefs_MonPrm.SER_WARNING, ref returnValue);
            //if (rc == CMotionAPI.MP_SUCCESS)
            //{
            //    //Servo - Limit Sensor
            //    ServoStatus[servoNo].Limit_P = Convert.ToBoolean((returnValue >> 6) & 0x1);
            //    //Servo + Limit Sensor
            //    ServoStatus[servoNo].Limit_N = Convert.ToBoolean((returnValue >> 7) & 0x1);
            //}

            //rc = CMotionAPI.ymcGetMotionParameterValue(m_hAxis[servoNo], (UInt16)CMotionAPI.ApiDefs.MONITOR_PARAMETER,    //110208
            //     (UInt16)CMotionAPI.ApiDefs_MonPrm.SER_POSSTS, ref returnValue);
            //if (rc == CMotionAPI.MP_SUCCESS)
            //{
            //    //Servo Origin
            //    ServoStatus[servoNo].Origin = Convert.ToBoolean((returnValue >> 4) & 0x1);
            //}

            ////Servo 상태 Read
            //string registerName = "IW" + (8000 + servoNo * 0x80).ToString();
            //rc = CMotionAPI.ymcGetRegisterDataHandle(registerName, ref g_hRegister_IW);
            //if (rc != CMotionAPI.MP_SUCCESS)
            //{
            //    MessageBox.Show(String.Format("Error ymcGetRegisterDataHandle IW ErrorCode [ 0x{0} ]", rc.ToString("X"));
            //    return;
            //}
            //rc = CMotionAPI.ymcGetRegisterData(g_hRegister_IW, 1, reg_IW, ref numOfReadData);
            //if (rc == CMotionAPI.MP_SUCCESS)
            //{
            //    ushort servoValue = reg_IW[0];
            //    ////Servo Ready
            //    //ServoStatus[servoNo].Ready = Convert.ToBoolean((servoValue >> 2) & 0x1);
            //    ////Servo Alarm
            //    //ServoStatus[servoNo].Alarm = Convert.ToBoolean((servoValue >> 0) & 0x1);
            //    ////Servo - Limit Sensor
            //    //ServoStatus[servoNo].Limit_N = Convert.ToBoolean((servoValue >> 13) & 0x1);
            //    ////Servo + Limit Sensor
            //    //ServoStatus[servoNo].Limit_P = Convert.ToBoolean((servoValue >> 12) & 0x1);
            //    ////Servo Origin
            //    //ServoStatus[servoNo].Origin = Convert.ToBoolean((servoValue >> 6) & 0x1);
            //    ////Servo On/Off
            //    //ServoStatus[servoNo].ServoOn = Convert.ToBoolean((servoValue >> 3) & 0x1);
            // }

            // Servo Command Input Signal
            rc = CMotionAPI.ymcGetMotionParameterValue(m_hAxis[servoNo], (UInt16)CMotionAPI.ApiDefs.MONITOR_PARAMETER,
            (UInt16)40, ref returnValue);
            if (rc == CMotionAPI.MP_SUCCESS)
            {
                //Servo + Limit Sensor
                ServoStatus[servoNo].Limit_P = Convert.ToBoolean((returnValue >> 2) & 0x1);
                //Servo - Limit Sensor
                ServoStatus[servoNo].Limit_N = Convert.ToBoolean((returnValue >> 3) & 0x1);
                //Servo Origin
                ServoStatus[servoNo].Origin = Convert.ToBoolean((returnValue >> 4) & 0x1);
            }
        }

        public int ServoOn(int deviceNo)
        {
            UInt32 rc = 0;

            rc = CMotionAPI.ymcServoControl(m_hDevice[deviceNo], (UInt16)CMotionAPI.ApiDefs.SERVO_ON, APITimeOut);
            if (rc != CMotionAPI.MP_SUCCESS)
            {
                LastHWMessage = String.Format("Error ymcServoControl On/Off Board : ErrorCode [ 0x{0} ]", rc.ToString("X"));
                WriteLog(LastHWMessage, ELogType.Debug, ELogWType.Error, true);
                return GenerateErrorCode(ERR_YASKAWA_FAIL_SERVO_ON);
            }

            return SUCCESS;
        }

        public int ServoOff(int deviceNo)
        {
            UInt32 rc = 0;

            rc = CMotionAPI.ymcServoControl(m_hDevice[deviceNo], (UInt16)CMotionAPI.ApiDefs.SERVO_OFF, APITimeOut);
            if (rc != CMotionAPI.MP_SUCCESS)
            {
                LastHWMessage = String.Format("Error ymcServoControl On/Off Board : ErrorCode [ 0x{0} ]", rc.ToString("X"));
                WriteLog(LastHWMessage, ELogType.Debug, ELogWType.Error, true);
                return GenerateErrorCode(ERR_YASKAWA_FAIL_SERVO_ON);
            }

            return SUCCESS;
        }

        public int ResetAlarm()
        {
            UInt32 rc;

            //============================================================================
            // Clears all the Machine Controller alarms. 
            //============================================================================
            rc = CMotionAPI.ymcClearAlarm(0);
            if (rc != CMotionAPI.MP_SUCCESS)
            {
                //MessageBox.Show(String.Format("Error ymcClearAlarm ErrorCode [ 0x{0} ]", rc.ToString("X"));
                LastHWMessage = String.Format("Error ymcClearAlarm ErrorCode [ 0x{0} ]", rc.ToString("X"));
                WriteLog(LastHWMessage, ELogType.Debug, ELogWType.Error, true);
                return GenerateErrorCode(ERR_YASKAWA_FAIL_RESET_ALARM);
            }

            // servo off
            for (int i = 0; i < (int)EYMC_Device.MAX; i++)
            {
                // 실제 Servo Off 되어 있으나 ServoOn Command 가 살아 있을 경우 RESET 안되는 문제해결하기 위해
                bool ServoNotOn = IsServoNotOnReally(i);
                if (ServoNotOn)
                {
                    rc = CMotionAPI.ymcServoControl(m_hDevice[i], (UInt16)CMotionAPI.ApiDefs.SERVO_OFF, APITimeOut);
                    if (rc != CMotionAPI.MP_SUCCESS)
                    {
                        //MessageBox.Show(String.Format("Error ymcServoControl ReserAlarm ErrorCode [ 0x{0} ]", rc.ToString("X"));
                        LastHWMessage = String.Format("Error ymcServoControl ReserAlarm ErrorCode [ 0x{0} ]", rc.ToString("X"));
                        WriteLog(LastHWMessage, ELogType.Debug, ELogWType.Error, true);
                        return GenerateErrorCode(ERR_YASKAWA_FAIL_RESET_ALARM);
                    }
                }

            }

            //============================================================================
            // Clears the servo alarm. 
            //============================================================================
            for (int i = 0; i < InstalledAxisNo ; i++)
            {
                rc = CMotionAPI.ymcClearServoAlarm(m_hAxis[i]);
                if (rc != CMotionAPI.MP_SUCCESS)
                {
                    //MessageBox.Show(String.Format("Error ymcClearServoAlarm ErrorCode [ 0x{0} ]", rc.ToString("X"));
                    LastHWMessage = String.Format("Error ymcClearServoAlarm ErrorCode [ 0x{0} ]", rc.ToString("X"));
                    WriteLog(LastHWMessage, ELogType.Debug, ELogWType.Error, true);
                    return GenerateErrorCode(ERR_YASKAWA_FAIL_RESET_ALARM);
                }
            }

            return SUCCESS;
        }

        public int ChkHomeComplete(int servoNo, out bool bComplete)
        {
            return ChkMoveComplete(servoNo, out bComplete);
        }

        public int ChkMoveComplete(int servoNo, out bool bComplete)
        {
            UInt32 returnValue = 0;
            bComplete = false;

            UInt32 rc = CMotionAPI.ymcGetMotionParameterValue(m_hAxis[servoNo],
                                                              (UInt16)CMotionAPI.ApiDefs.MONITOR_PARAMETER,
                                                              (UInt16)CMotionAPI.ApiDefs_MonPrm.SER_POSSTS,
                                                              ref returnValue);
            if (rc != CMotionAPI.MP_SUCCESS)
            {
                //MessageBox.Show(String.Format("Error ymcGetMotionParameterValue ErrorCode [ 0x{0} ]", rc.ToString("X"));
                LastHWMessage = String.Format("Error ymcGetMotionParameterValue ErrorCode [ 0x{0} ]", rc.ToString("X"));
                WriteLog(LastHWMessage, ELogType.Debug, ELogWType.Error, true);
                return GenerateErrorCode(ERR_YASKAWA_FAIL_GET_MOTION_PARAM);
            }

            //Move 완료 확인 
            if (((returnValue >> (int)CMotionAPI.ApiDefs.POSITIONING_COMPLETED) & 0x1) == 1)
                bComplete = true;
            else bComplete = false;

            return SUCCESS;
        }

        public int ServoStop(int deviceNo)
        {
            ushort[] WaitForCompletion;
            GetDeviceWait(deviceNo, out WaitForCompletion);
            UInt32 rc = CMotionAPI.ymcStopJOG(m_hDevice[deviceNo], 0, "STOP", WaitForCompletion, 0);
            if (rc != CMotionAPI.MP_SUCCESS)
            {
                //MessageBox.Show(String.Format("Error ymcStopJOG ErrorCode [ 0x{0} ]", rc.ToString("X"));
                LastHWMessage = String.Format("Error ymcStopJOG ErrorCode [ 0x{0} ]", rc.ToString("X"));
                WriteLog(LastHWMessage, ELogType.Debug, ELogWType.Error, true);

                return GenerateErrorCode(ERR_YASKAWA_FAIL_SERVO_STOP);
            }

            return SUCCESS;
        }

        public int ServoMotionStop(int deviceNo, ushort mode = (ushort)CMotionAPI.ApiDefs.POSITIONING_COMPLETED)
        {
            CMotionAPI.MOTION_DATA[] MotionData;
            GetDeviceMotionData(deviceNo, out MotionData);
            ushort[] WaitForCompletion;
            GetDeviceWait(deviceNo, out WaitForCompletion, mode);

            UInt32 rc = CMotionAPI.ymcStopMotion(m_hDevice[deviceNo], MotionData, "STOP", WaitForCompletion, 0);
            if (rc != CMotionAPI.MP_SUCCESS)
            {
                //MessageBox.Show(String.Format("Error ymcStopJOG ErrorCode [ 0x{0} ]", rc.ToString("X"));
                LastHWMessage = String.Format("Error ymcStopJOG ErrorCode [ 0x{0} ]", rc.ToString("X"));
                WriteLog(LastHWMessage, ELogType.Debug, ELogWType.Error, true);
                return GenerateErrorCode(ERR_YASKAWA_FAIL_SERVO_STOP);
            }

            return SUCCESS;
        }
        // 

        public int MoveStartToJog(int deviceNo, bool jogDir, bool bJogFastMove = false)
        {
            // Jog함수는 multi axis device를 고려하지 않고 작성
            if (deviceNo >= (int)EYMC_Device.STAGE1) return SUCCESS;

            //============================================================================
            // Executes JOG operation.										
            //============================================================================
            // Motion data setting
            Int16[] Direction = new Int16[1];
            UInt16[] TimeOut = new UInt16[1] { APITimeOut };

            if (jogDir == JOG_DIR_POS)
            {
                //Jog +
                if (ServoStatus[deviceNo].Ready && ServoStatus[deviceNo].ServoOn)
                {
                    if (ServoStatus[deviceNo].Limit_P)
                    {
                        LastHWMessage = "Servo No[" + deviceNo + "] : + Limit";
                        WriteLog(LastHWMessage, ELogType.Debug, ELogWType.Warning, true);
                        return GenerateErrorCode(ERR_YASKAWA_SERVO_DETECTED_PLUS_LIMIT);
                    }
                    else
                    {
                        Direction[0] = (Int16)CMotionAPI.ApiDefs.DIRECTION_POSITIVE;
                    }
                }
            }
            else
            {
                //Jog -
                if (ServoStatus[deviceNo].Ready && ServoStatus[deviceNo].ServoOn)
                {
                    if (ServoStatus[deviceNo].Limit_N)
                    {
                        LastHWMessage = "Servo No[" + deviceNo + "] : - Limit";
                        WriteLog(LastHWMessage, ELogType.Debug, ELogWType.Warning, true);
                        return GenerateErrorCode(ERR_YASKAWA_SERVO_DETECTED_MINUS_LIMIT);
                    }
                    else
                    {
                        Direction[0] = (Int16)CMotionAPI.ApiDefs.DIRECTION_NEGATIVE;
                    }
                }
            }

            ushort TimeOut1 = 0;
            CMotionAPI.MOTION_DATA[] MotionData;
            GetDeviceMotionData_Jog(deviceNo, out MotionData, bJogFastMove);
            UInt32 rc = CMotionAPI.ymcMoveJOG(m_hDevice[deviceNo], MotionData, Direction, TimeOut, 0, "JOG", 0);
            if (rc != CMotionAPI.MP_SUCCESS)
            {
                LastHWMessage = String.Format("Error ymcMoveJOG ErrorCode [ 0x{0} ]", rc.ToString("X"));
                WriteLog(LastHWMessage, ELogType.Debug, ELogWType.Error, true);
                return GenerateErrorCode(ERR_YASKAWA_FAIL_SERVO_MOVE_JOG);
            }

            return SUCCESS;
        }

        public int MoveStartToPos(int deviceNo, double[] pos, ushort waitMode = (ushort)CMotionAPI.ApiDefs.COMMAND_STARTED)
        {
            return MoveToPos(deviceNo, pos, waitMode);

        }

        public int MoveToPos(int deviceNo, double[] pos, ushort waitMode = (ushort)CMotionAPI.ApiDefs.POSITIONING_COMPLETED)
        {
            // Position data setting
            CMotionAPI.POSITION_DATA[] PositionData;
            GetDevicePositon(deviceNo, out PositionData, pos, (UInt16)CMotionAPI.ApiDefs.DATATYPE_IMMEDIATE);
            CMotionAPI.MOTION_DATA[] MotionData;
            GetDeviceMotionData(deviceNo, out MotionData);
            ushort[] WaitForCompletion;
            GetDeviceWait(deviceNo, out WaitForCompletion, waitMode);

            UInt32 rc = CMotionAPI.ymcMoveDriverPositioning(m_hDevice[deviceNo], MotionData, PositionData, 0, "Move", WaitForCompletion, 0);
            if (rc != CMotionAPI.MP_SUCCESS)
            {
                //MessageBox.Show(String.Format("Error ymcMoveDriverPositioning ErrorCode [ 0x{0} ]",rc.ToString("X")));
                LastHWMessage = String.Format("Error ymcMoveDriverPositioning ErrorCode [ 0x{0} ]", rc.ToString("X"));
                WriteLog(LastHWMessage, ELogType.Debug, ELogWType.Error, true);
                return GenerateErrorCode(ERR_YASKAWA_FAIL_SERVO_MOVE_DRIVING_POSITIONING);
            }

            return SUCCESS;
        }
        
        public int MoveStartToHome(int deviceNo)
        {
            return HomePosition(deviceNo);
        }

        private int HomePosition(int deviceNo)
        {
            CMotionAPI.MOTION_DATA[] MotionData;
            ushort[] Method;
            ushort[] Dir;
            CMotionAPI.POSITION_DATA[] Position;
            GetDeviceMotionData_Home(deviceNo, out MotionData, out Method, out Dir, out Position);
            ushort[] WaitForCompletion;
            GetDeviceWait(deviceNo, out WaitForCompletion, (UInt16)CMotionAPI.ApiDefs.COMMAND_STARTED);

            UInt32 rc;
            rc = CMotionAPI.ymcMoveHomePosition(m_hDevice[deviceNo], MotionData, Position, Method, Dir, 0, null, WaitForCompletion, 0);
            if (rc != CMotionAPI.MP_SUCCESS)
            {
                //MessageBox.Show(String.Format("Error ymcMoveHomePositioning ErrorCode [ 0x{0} ]", rc.ToString("X"));
                LastHWMessage = String.Format("Error ymcMoveHomePositioning ErrorCode [ 0x{0} ]", rc.ToString("X"));
                WriteLog(LastHWMessage, ELogType.Debug, ELogWType.Error, true);
                return GenerateErrorCode(ERR_YASKAWA_FAIL_SERVO_MOVE_HOME);
            }

            return SUCCESS;
        }

        public int GetServoPos(int servoNo, out double pos)
        {
            pos = 0;
            UInt32 servoPosi = 0;
            UInt32 rc = CMotionAPI.ymcGetMotionParameterValue(m_hAxis[servoNo], (UInt16)CMotionAPI.ApiDefs.MONITOR_PARAMETER,
                 (UInt16)CMotionAPI.ApiDefs_MonPrm.SER_APOS, ref servoPosi);
            if (rc != CMotionAPI.MP_SUCCESS)
            {
                LastHWMessage = String.Format("Error ymcGetMotionParameterValue ErrorCode [ 0x{0} ]", rc.ToString("X"));
                WriteLog(LastHWMessage, ELogType.Debug, ELogWType.Error, true);
                return GenerateErrorCode(ERR_YASKAWA_FAIL_SERVO_GET_POS);
            }
            pos = servoPosi / UNIT_REF;

            return SUCCESS;
        }

        public bool IsServoNotOnReally(int servoNo)//140316 
        {

            UInt32 returnValue = 0;

            UInt32 rc = CMotionAPI.ymcGetMotionParameterValue(m_hAxis[servoNo],
                                                          (UInt16)CMotionAPI.ApiDefs.MONITOR_PARAMETER,
                                                          (UInt16)CMotionAPI.ApiDefs_MonPrm.SER_WARNING,
                                                          ref returnValue);

            return Convert.ToBoolean((returnValue >> 8) & 0x1);
        }
        /*
        public void ManualPacket()
        {
            UInt32 rc;
            UInt32 hRegister_ML;                     // Register data handle for ML register
            UInt32 hRegister_ML_Read;                     // Register data handle for ML register
            UInt32 hRegister_MB;                     // Register data handle for MB register

            Int32[] Reg_LongData = new Int32[59];      // L size register data storage variable ydh150717
            //UInt16[] Reg_ShortData = new UInt16[4];    // W or B size register data storage variable
            short Reg_ShortData = 0;                     // W or B size register data storage variable
            UInt32 Reg_IntData = 0;//ydh150803
            //UInt32 Reg_IntData_1214 = 0;
            UInt32[] Reg_IntData_1214 = new UInt32[5];

            //UInt32 Reg_IntData_1300 = 0;
            Int32[] Reg_LongData_1300 = new Int32[54];
            String cRegisterName_MB;                 // MB register name storage variable
            String cRegisterName_ML;                 // ML register name storage variable
            String cRegisterName_ML_1214;                 // ML register name storage variable
            String cRegisterName_ML_1300;                 // ML register name storage variable
            UInt32 RegisterDataNumber;               // Number of read-in registers
            UInt32 RegisterDataNumber_b;
            UInt32 RegisterDataNumber_1214;               // Number of read-in registers
            UInt32 RegisterDataNumber_1300;               // Number of read-in registers
            UInt32 ReadDataNumber;                   // Number of obtained registers
            UInt32 ReadDataNumber_b;                   // Number of obtained registers
            hRegister_ML = 0x00000000;
            hRegister_ML_Read = 0x00000000;
            hRegister_MB = 0x00000000;
            ReadDataNumber = 00000000;
            ReadDataNumber_b = 00000000;
            RegisterDataNumber_1214 = 0;
            RegisterDataNumber_1300 = 0;

            // MB Register
            cRegisterName_ML = "ML01000";   //ML Register 주소는 짝수로 설정해야함. 홀수일때 핸들에러 발생
            cRegisterName_MB = "MB000000";

            cRegisterName_ML_1214 = "ML01214";   //ydh150803 ML Register 주소는 짝수로 설정해야함. 홀수일때 핸들에러 발생
            cRegisterName_ML_1300 = "ML01300";
            #region MB 영역 read/write
            // MB Register 핸들
            rc = CMotionAPI.ymcGetRegisterDataHandle(cRegisterName_MB, ref hRegister_MB);
            if (rc != CMotionAPI.MP_SUCCESS)
            {
                //MessageBox.Show(String.Format("Error ymcGetRegisterDataHandle  MB ErrorCode [ 0x{0} ]", rc.ToString("X"));
                return GenerateErrorCode(ERR_YASKAWA_FAIL_GET_REGISTER_DATA_HANDLE);
                return;
            }

            // MB Register 읽기
            RegisterDataNumber_b = 1;
            rc = CMotionAPI.ymcGetRegisterData(hRegister_MB, RegisterDataNumber_b, ref Reg_ShortData, ref ReadDataNumber_b);
            if (rc != CMotionAPI.MP_SUCCESS)
            {
                //MessageBox.Show(String.Format("Error ymcGetRegisterData MB ErrorCode [ 0x{0} ]", rc.ToString("X"));
                return GenerateErrorCode(ERR_YASKAWA_FAIL_SET_REGISTER_DATA);
                return;
            }

            if (Convert.ToBoolean(Reg_ShortData))
                Reg_ShortData = 0;
            else
                Reg_ShortData = 1;

            // MB Register 쓰기 (HeartBit 용 PC <-> MP 간)
            RegisterDataNumber_b = 1;
            rc = CMotionAPI.ymcSetRegisterData(hRegister_MB, RegisterDataNumber_b, ref Reg_ShortData);
            if (rc != CMotionAPI.MP_SUCCESS)
            {
                //MessageBox.Show(String.Format("Error ymcSetRegisterData MB ErrorCode [ 0x{0} ]", rc.ToString("X"));
                return GenerateErrorCode(ERR_YASKAWA_FAIL_SET_REGISTER_DATA_FAIL);
                return;
            }
            #endregion

            //RegisterDataNumber = 0;

            #region ML 영역 read/write (ML1000)
            // ML Register 전송용 핸들
            rc = CMotionAPI.ymcGetRegisterDataHandle(cRegisterName_ML, ref hRegister_ML);
            if (rc != CMotionAPI.MP_SUCCESS)
            {
                //MessageBox.Show(String.Format("Error ymcGetRegisterDataHandle ML ErrorCode [ 0x{0} ]", rc.ToString("X"));
                return GenerateErrorCode(ERR_YASKAWA_FAIL_GET_REGISTER_DATA_HANDLE_FAIL);
                return;
            }

            // ML Register 수신data
            //rc = CMotionAPI.ymcGetRegisterData(hRegister_ML, RegisterDataNumber, ref Reg_IntData, ref ReadDataNumber);
            //if (rc != CMotionAPI.MP_SUCCESS)
            //{
            //    MessageBox.Show(String.Format("Error ymcGetRegisterData ML ErrorCode [ 0x{0} ]", rc.ToString("X"));
            //    return;
            //}

            //YDH150803 ML1214를 읽어 보여주자(GO CMD를 받은 시점부터의 이동거리 값을 읽어온다.MF1212 = MF1212 + IL8040(SER_FSPD)
            UInt32 GoCmdFromStartPos;
            GoCmdFromStartPos = Reg_IntData;

            // mode
            Reg_LongData[0] = (OHTInform.Inform.Mode);//ML1000

            // status
            Reg_LongData[1] = (OHTInform.OHTStatus);//ML1002

            //reset
            Reg_LongData[2] = (OHTInform.DrivingInform.Reset);//ML1004

            //비상정지
            Reg_LongData[3] = (UtilityClass.ValueToInt(OHTAlarm.EMFlag));//ML1006

            //정방향(0), 역방향 (1)
            Reg_LongData[4] = (OHTInform.DrivingInform.MoveDirection);//ML1008

            //command speed
            if (OHTInform.Inform.IsCleanVehicle)
            {
                // 청소차량 속도제한 설정
                if (OHTInform.DrivingInform.Velocity > CLEANVEHICLE_SPEED_LIMIT)
                {
                    Reg_LongData[5] = CLEANVEHICLE_SPEED_LIMIT;
                }
                else
                {
                    Reg_LongData[5] = (OHTInform.DrivingInform.Velocity);//ML1010
                }
            }
            else
            {
                Reg_LongData[5] = (OHTInform.DrivingInform.Velocity);//ML1010
            }

            // accel
            Reg_LongData[6] = (OHTInform.DrivingInform.Acceleration) * UNIT_REF;//ML1012

            //khc 151120 : 대인 감지할 경우 감속도 변경, 직진에서는 2단계만 감지해도 정지 수준으로 변경해야 함.
            if ((OHTDetect.DetectDir == (int)DetectDirection.STRAIGHT) && ((OHTInOut.X[DEF_IO.X_OBSTACLE_DETECT_STOP] == DEF_BIT.OFF) || TOHSMain.EStopFlag))
            {
                OHTInform.DrivingInform.Deceleration = 3000;
            }
            else if ((OHTDetect.DetectDir == (int)DetectDirection.STRAIGHT) && (OHTInOut.X[DEF_IO.X_OBSTACLE_DETECT_WARNING] == DEF_BIT.OFF))
            {
                OHTInform.DrivingInform.Deceleration = 2000;
            }
            else
            {
                OHTInform.DrivingInform.Deceleration = (int)ServoInform[(int)ServoList.DRIVING1].Deceleration;
            }
            // decel
            Reg_LongData[7] = (OHTInform.DrivingInform.Deceleration) * UNIT_REF;//ML1014

            // curve(1) / 직선(2)
            //int _tmp = (OHTInform.PositionFlag.SetPlcCurveMode) ? 1 : 0;   // 0 : 직선, 1: 90 도 커브, 2: 180 도 커브, 3: N Curve(90도 연속커브)
            Reg_LongData[8] = OHTInform.DrivingInform.CurCurveType;//ML1016

            //khc 151126 : auto 에서는 무조건 0을 써주자.
            if (OHTInform.Inform.Mode == (int)OHTInform.ModeList.AUTO) OHTInform.DrivingInform.DrivingAxis = 0;
            //Driving axis
            Reg_LongData[9] = (OHTInform.DrivingInform.DrivingAxis);////ML1018



            // 거리 reset request
            Reg_LongData[10] = (UtilityClass.ValueToInt(OHTInform.PositionFlag.PLCSyncReqFlag));//ML1020

            // 설정된 Creep speed
            Reg_LongData[11] = OHTInform.RunningSpeed.CreepSpeed;  //ML1022

            //Reg_LongData[12] = 5;

            //RegisterDataNumber = 13;

            for (int i = 12; i < 20; i++)
            {
                Reg_LongData[i] = 0;
            }
            // Distance to Goal
            Reg_LongData[20] = (OHTInform.PositionValue.FromStartToGoal);//ML1040


            // Distance to Juncture 1
            //Reg_LongData[21] = (OHTInform.FromStartToJunction[0].Dist);//ML1042
            //// Distance to Juncture 2
            //Reg_LongData[22] = (OHTInform.FromStartToJunction[1].Dist);//ML1044
            //// Distance to Juncture 3
            //Reg_LongData[23] = (OHTInform.FromStartToJunction[2].Dist);//ML1046
            //// Distance to Juncture 4
            //Reg_LongData[24] = (OHTInform.FromStartToJunction[3].Dist);//ML1048
            //// Distance to Curve 1
            //Reg_LongData[25] = (OHTInform.FromStartToCurve[0].Dist);//ML1050
            //// Distance to Curve 2
            //Reg_LongData[26] = (OHTInform.FromStartToCurve[1].Dist);//ML1052
            //// Distance to Curve 3
            //Reg_LongData[27] = (OHTInform.FromStartToCurve[2].Dist);//ML1054

            Reg_LongData[21] = (OHTInform.FromStartToCurve[0].Dist);//ML1042
            // Distance to Juncture 2
            Reg_LongData[22] = (OHTInform.FromStartToCurve[1].Dist);//ML1044
            // Distance to Juncture 3
            Reg_LongData[23] = (OHTInform.FromStartToCurve[2].Dist);//ML1046
            // Distance to Juncture 4
            Reg_LongData[24] = (OHTInform.FromStartToJunction[0].Dist);//ML1048
            // Distance to Curve 1
            Reg_LongData[25] = (OHTInform.FromStartToJunction[1].Dist);//ML1050
            // Distance to Curve 2
            Reg_LongData[26] = (OHTInform.FromStartToJunction[2].Dist);//ML1052
            // Distance to Curve 3
            Reg_LongData[27] = (OHTInform.FromStartToJunction[3].Dist);//ML1054

            //Reg_LongData[28] = (OHTInform.PositionValue.FromStartToCurve[3]);//ML1056

            // Speed For Stop ( 커브 감속 )
            Reg_LongData[30] = (VEHICLE_DEST_BCD_SEARCH_VELOCITY);//ML1060
            //// Speed For Juncture 1
            //Reg_LongData[31] = (OHTInform.RunningSpeed.JunctionRunningSpeed);//ML1062
            //// Speed For Juncture 2
            //Reg_LongData[32] = (OHTInform.RunningSpeed.JunctionRunningSpeed);//ML1064
            //// Speed For Juncture 3
            //Reg_LongData[33] = (OHTInform.RunningSpeed.JunctionRunningSpeed);//ML1066
            //// Speed For Juncture 4
            //Reg_LongData[34] = (OHTInform.RunningSpeed.JunctionRunningSpeed);//ML1068
            //// Speed For Curve 1
            ////Reg_LongData[35] = (VEHICLE_CURVE_MIRROR_SEARCH_VELOCITY);//ML1070
            //Reg_LongData[35] = (OHTInform.RunningSpeed.CurveRunningSpeed);
            //// Speed For Curve 2
            ////Reg_LongData[36] = (VEHICLE_CURVE_MIRROR_SEARCH_VELOCITY);//ML1072
            //Reg_LongData[36] = (OHTInform.RunningSpeed.CurveRunningSpeed);
            //// Speed For Curve 3
            ////Reg_LongData[37] = (VEHICLE_CURVE_MIRROR_SEARCH_VELOCITY);//ML1074
            //Reg_LongData[37] = (OHTInform.RunningSpeed.CurveRunningSpeed);

            // Speed For Juncture 1
            Reg_LongData[31] = (OHTInform.RunningSpeed.CurveRunningSpeed);//ML1062
            // Speed For Juncture 2
            Reg_LongData[32] = (OHTInform.RunningSpeed.CurveRunningSpeed);//ML1064
            // Speed For Juncture 3
            Reg_LongData[33] = (OHTInform.RunningSpeed.CurveRunningSpeed);//ML1066
            // Speed For Juncture 4
            Reg_LongData[34] = (OHTInform.RunningSpeed.JunctionRunningSpeed);//ML1068
            // Speed For Curve 1
            //Reg_LongData[35] = (VEHICLE_CURVE_MIRROR_SEARCH_VELOCITY);//ML1070
            Reg_LongData[35] = (OHTInform.RunningSpeed.JunctionRunningSpeed);
            // Speed For Curve 2
            //Reg_LongData[36] = (VEHICLE_CURVE_MIRROR_SEARCH_VELOCITY);//ML1072
            Reg_LongData[36] = (OHTInform.RunningSpeed.JunctionRunningSpeed);
            // Speed For Curve 3
            //Reg_LongData[37] = (VEHICLE_CURVE_MIRROR_SEARCH_VELOCITY);//ML1074
            Reg_LongData[37] = (OHTInform.RunningSpeed.JunctionRunningSpeed);

            // offset 거리
            Reg_LongData[50] = (OHTInform.OnPosiOffsetDist);//ML1100

            // 커브 돌때 속도
            Reg_LongData[51] = (OHTInform.RunningSpeed.CurveRunningSpeed);//ML1102

            // 대차감지속도1(감속시작)
            Reg_LongData[52] = (OHTDetect.DetectVelocity[0]);//ML1104

            // 대차감지속도2
            Reg_LongData[53] = (OHTDetect.DetectVelocity[1]);//ML1106

            // 대차감지속도3
            Reg_LongData[54] = (OHTDetect.DetectVelocity[2]);//ML1108

            // 대차감지속도4
            Reg_LongData[55] = (OHTDetect.DetectVelocity[3]);//ML1110

            // 대차감지속도5
            Reg_LongData[56] = (OHTDetect.DetectVelocity[4]);//ML1112

            // 대차감지속도(정지)
            Reg_LongData[57] = (0);//ML1114

            // 대인감지속도(감속시작)
            //Reg_LongData[58] = (OHTObstacle.DetectVelocity[1]);//ML1116
            Reg_LongData[58] = (0);//ML1116

            RegisterDataNumber = 59;

            rc = CMotionAPI.ymcSetRegisterData(hRegister_ML, RegisterDataNumber, ref Reg_LongData[0]);
            if (rc != CMotionAPI.MP_SUCCESS)
            {
                //MessageBox.Show(String.Format("Error ymcSetRegisterData ML ErrorCode [ 0x{0} ]", rc.ToString("X"));
                return GenerateErrorCode(ERR_YASKAWA_FAIL_SET_REGISTER_DATA_FAIL);
                return;
            }
            #endregion

            #region ML 영역 read/write (ML 1210)
            // ML Register 전송용 핸들
            rc = CMotionAPI.ymcGetRegisterDataHandle(cRegisterName_ML_1214, ref hRegister_ML_Read);
            if (rc != CMotionAPI.MP_SUCCESS)
            {
                //MessageBox.Show(String.Format("Error ymcGetRegisterDataHandle ML ErrorCode [ 0x{0} ]", rc.ToString("X"));
                return GenerateErrorCode(ERR_YASKAWA_FAIL_GET_REGISTER_DATA_HANDLE_FAIL);
                return;
            }
            RegisterDataNumber_1214 = 5;
            // ML Register 수신data
            rc = CMotionAPI.ymcGetRegisterData(hRegister_ML_Read, RegisterDataNumber_1214, ref Reg_IntData_1214[0], ref RegisterDataNumber_1214);
            if (rc != CMotionAPI.MP_SUCCESS)
            {
                // MessageBox.Show(String.Format("Error ymcGetRegisterData ML ErrorCode [ 0x{0} ]", rc.ToString("X"));
                return GenerateErrorCode(ERR_YASKAWA_FAIL_GET_REGISTER_DATA_FAIL);
                return;
            }

            //YDH150803 ML1214를 읽어 보여주자(GO CMD를 받은 시점부터의 이동거리 값을 읽어온다.MF1212 = MF1212 + IL8040(SER_FSPD)
            //UInt32 GoCmdFromStartPos;
            OHTInform.FromStartDist = (int)Reg_IntData_1214[0];  // ML1214

            // ML1216
            OHTInform.PlcSetCurve = (int)Reg_IntData_1214[1];
            // ML1218
            //= (int)Reg_IntData_1214[2]
            // ML1220
            //= (int)Reg_IntData_1214[3]
            // ML1222
            OHTInform.OverShootDist = (int)Reg_IntData_1214[4];  // ML1222


            #endregion

            #region ML 영역 read/write (ML 1300)
            // ML Register 전송용 핸들
            rc = CMotionAPI.ymcGetRegisterDataHandle(cRegisterName_ML_1300, ref hRegister_ML_Read);
            if (rc != CMotionAPI.MP_SUCCESS)
            {
                //MessageBox.Show(String.Format("Error ymcGetRegisterDataHandle ML ErrorCode [ 0x{0} ]", rc.ToString("X"));
                return GenerateErrorCode(ERR_YASKAWA_FAIL_GET_REGISTER_DATA_HANDLE_FAIL);
                return;
            }
            RegisterDataNumber_1300 = 53;
            // ML Register 수신data
            rc = CMotionAPI.ymcGetRegisterData(hRegister_ML_Read, RegisterDataNumber_1300, ref Reg_LongData_1300[0], ref RegisterDataNumber_1300);
            if (rc != CMotionAPI.MP_SUCCESS)
            {
                //MessageBox.Show(String.Format("Error ymcGetRegisterData ML ErrorCode [ 0x{0} ]", rc.ToString("X"));
                return GenerateErrorCode(ERR_YASKAWA_FAIL_GET_REGISTER_DATA_FAIL);
                return;
            }

            //YDH150803 ML1214를 읽어 보여주자(GO CMD를 받은 시점부터의 이동거리 값을 읽어온다.MF1212 = MF1212 + IL8040(SER_FSPD)
            //UInt32 GoCmdFromStartPos;
            //xxx = (int)Reg_LongData_1300[0];
            OHTInform.FdbSpeed = (int)Reg_LongData_1300[1];
            //  .... xxx = (int)Reg_LongData_1300[53];

            //khc 각 축별로 부하율
            ServoStatus[0].LoadFactor = (int)Reg_LongData_1300[2];
            ServoStatus[1].LoadFactor =
                (int)Reg_LongData_1300[17];
            ServoStatus[2].LoadFactor = (int)Reg_LongData_1300[32];
            ServoStatus[3].LoadFactor = (int)Reg_LongData_1300[47];

            ServoStatus[0].AlarmCode = (int)Reg_LongData_1300[7];  //ml1314
            ServoStatus[1].AlarmCode = (int)Reg_LongData_1300[22];  //ml1344

            if (Math.Abs(ServoStatus[0].LoadFactor) >= 250)
            {
                MainForm.LOG(LOGType.Warning, "(Front LoadFactor) BCD No : " + OHTBarcode.CurCtrlBcd.BCDNo.ToString() + " >>>> " + ServoStatus[0].LoadFactor.ToString() + OHTInform.LogggingDistanceInfo());
                MainForm.LOG(LOGType.Warning, "(Rear LoadFactor) BCD No : " + OHTBarcode.CurCtrlBcd.BCDNo.ToString() + " >>>> " + ServoStatus[1].LoadFactor.ToString() + OHTInform.LogggingDistanceInfo());
            }
            else if (Math.Abs(ServoStatus[1].LoadFactor) >= 200)
            {
                MainForm.LOG(LOGType.Warning, "(Front LoadFactor) BCD No : " + OHTBarcode.CurCtrlBcd.BCDNo.ToString() + " >>>> " + ServoStatus[0].LoadFactor.ToString() + OHTInform.LogggingDistanceInfo());
                MainForm.LOG(LOGType.Warning, "(Rear LoadFactor) BCD No : " + OHTBarcode.CurCtrlBcd.BCDNo.ToString() + " >>>> " + ServoStatus[1].LoadFactor.ToString() + OHTInform.LogggingDistanceInfo());
            }

            if ((ServoStatus[0].AlarmCode > 0 && (int)Reg_LongData_1300[6] > 0) && !TOHSMain.ServoAlarmCodeWriteLogFlag[0])
            {
                TOHSMain.ServoAlarmCodeWriteLogFlag[0] = true;
                MainForm.LOG(LOGType.Warning, "Front Servo Alarm Code :: " + ServoStatus[0].AlarmCode.ToString());
            }
            if ((ServoStatus[1].AlarmCode > 0 && (int)Reg_LongData_1300[21] > 0) && !TOHSMain.ServoAlarmCodeWriteLogFlag[1])
            {
                TOHSMain.ServoAlarmCodeWriteLogFlag[1] = true;
                MainForm.LOG(LOGType.Warning, "Rear Servo Alarm Code :: " + ServoStatus[1].AlarmCode.ToString());
            }
            #endregion

            //UInt32 rc;
            //UInt32 hRegister_ML;                     // Register data handle for ML register
            //UInt32 hRegister_MB;                     // Register data handle for MB register

            //Int32[] Reg_LongData = new Int32[10];      // L size register data storage variable
            ////UInt16[] Reg_ShortData = new UInt16[4];    // W or B size register data storage variable
            //short Reg_ShortData = 0;                     // W or B size register data storage variable
            //String cRegisterName_MB;                 // MB register name storage variable
            //String cRegisterName_ML;                 // ML register name storage variable
            //UInt32 RegisterDataNumber;               // Number of read-in registers
            //UInt32 ReadDataNumber;                   // Number of obtained registers
            //hRegister_ML = 0x00000000;
            //hRegister_MB = 0x00000000;
            //ReadDataNumber = 00000000;

            //// MB Register
            //cRegisterName_ML = "ML01000";   //ML Register 주소는 짝수로 설정해야함. 홀수일때 핸들에러 발생
            //cRegisterName_MB = "MB000000";

            //// MB Register 핸들
            //rc = CMotionAPI.ymcGetRegisterDataHandle(cRegisterName_MB, ref hRegister_MB);
            //if (rc != CMotionAPI.MP_SUCCESS)
            //{
            //    MessageBox.Show(String.Format("Error ymcGetRegisterDataHandle  MB ErrorCode [ 0x{0} ]", rc.ToString("X"));
            //    return;
            //}

            //// MB Register 읽기
            //RegisterDataNumber = 1;
            //rc = CMotionAPI.ymcGetRegisterData(hRegister_MB, RegisterDataNumber, ref Reg_ShortData, ref ReadDataNumber);
            //if (rc != CMotionAPI.MP_SUCCESS)
            //{
            //    MessageBox.Show(String.Format("Error ymcGetRegisterData MB ErrorCode [ 0x{0} ]", rc.ToString("X"));
            //    return;
            //}

            //if (Convert.ToBoolean(Reg_ShortData))
            //    Reg_ShortData = 0;
            //else
            //    Reg_ShortData = 1;

            //// MB Register 쓰기 (HeartBit 용 PC <-> MP 간)
            //RegisterDataNumber = 1;
            //rc = CMotionAPI.ymcSetRegisterData(hRegister_MB, RegisterDataNumber,ref Reg_ShortData);
            //if (rc != CMotionAPI.MP_SUCCESS)
            //{
            //    MessageBox.Show(String.Format("Error ymcSetRegisterData MB ErrorCode [ 0x{0} ]", rc.ToString("X"));
            //    return;
            //}

            //// ML Register 전송용 핸들
            //rc = CMotionAPI.ymcGetRegisterDataHandle(cRegisterName_ML, ref hRegister_ML);
            //if (rc != CMotionAPI.MP_SUCCESS)
            //{
            //    MessageBox.Show(String.Format("Error ymcGetRegisterDataHandle ML ErrorCode [ 0x{0} ]", rc.ToString("X"));
            //    return;
            //}

            //Reg_LongData[0] = (OHTInform.Inform.Mode);
            //Reg_LongData[1] = (OHTInform.OHTStatus);
            //Reg_LongData[2] = (OHTInform.DrivingInform.Reset);
            //Reg_LongData[3] = (UtilityClass.ValueToInt(OHTAlarm.EMFlag));
            //Reg_LongData[4] = (OHTInform.DrivingInform.MoveDirection);
            //Reg_LongData[5] = (OHTInform.DrivingInform.Velocity);
            //Reg_LongData[6] = (OHTInform.DrivingInform.Acceleration) * UNIT_REF;
            //Reg_LongData[7] = (OHTInform.DrivingInform.Deceleration) * UNIT_REF;
            //Reg_LongData[8] = (UtilityClass.ValueToInt(OHTInform.PositionFlag.InCurve));
            //Reg_LongData[9] = (OHTInform.DrivingInform.DrivingAxis);
            //Reg_LongData[10] = (OHTInform.DecelStopPosition.DestDist);  // Hyun 150619
            //RegisterDataNumber = 11;  // 10 -> 11 Hyun 150619

            //rc = CMotionAPI.ymcSetRegisterData(hRegister_ML, RegisterDataNumber, ref Reg_LongData[0]);
            //if (rc != CMotionAPI.MP_SUCCESS)
            //{
            //    MessageBox.Show(String.Format("Error ymcSetRegisterData ML ErrorCode [ 0x{0} ]", rc.ToString("X"));
            //    return;
            //}
        }
        */
        //public void GetInput(int startAddr, int ioAddr)
        //{
        //    unsafe
        //    {
        //        uint returnValue = 0;
        //        uint ustartAddr = 0;
        //        ustartAddr = Convert.ToUInt32(startAddr);
        //        Pci_Read16(ustartAddr , &returnValue);
        //        OHTInOut.TX16[ioAddr] = Convert.ToInt32(returnValue);

        //        OHTInOut.IOPoolX[ioAddr] = Convert.ToInt32(returnValue);//YDH150804 x값 배열명 변경
        //    }
        //}

        //public void SetOutput(int startAddr, int ioAddr)
        //{
        //    unsafe
        //    {
        //        uint setValue;
        //        uint ustartAddr = 0;
        //        ustartAddr = Convert.ToUInt32(startAddr);
        //        setValue = Convert.ToUInt32(OHTInOut.TY16[ioAddr]);

        //        setValue = Convert.ToUInt32(OHTInOut.IOPoolY[ioAddr]);//YDH150804 y값 배열명 변경

        //        Pci_Write16(ustartAddr, &setValue);
        //    }
        //}

    }
}
