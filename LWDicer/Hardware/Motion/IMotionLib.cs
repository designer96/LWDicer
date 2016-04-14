using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static LWDicer.Control.DEF_Motion;

namespace LWDicer.Control
{
    public class DEF_Motion
    {
        // 
        public const int DEF_MAX_AXIS_PER_SAXIS = 4;    // DEF_XYTZ

        // Board에 따른 축 수
        public const int DEF_AXIS_NO_PER_BRD    = 8; // Motion Board가 Full-Size이면 8개

        // Board Value
        public const int DEF_NON_BOARD_TYPE     = 0; // Motion Board No-Use
        public const int DEF_MMC_BOARD_TYPE     = 1; // MMC Motion Board Use
        public const int DEF_MEI_BOARD_TYPE     = 2; // MEI Motion Board Use

        public const int DEF_0AXIS_BOARD        = 0; // 0축을 갖는 Motion Board
        public const int DEF_4AXIS_BOARD        = 4; // 4축을 갖는 Motion Board
        public const int DEF_8AXIS_BOARD        = 8; // 8축을 갖는 Motion Board

        public const int DEF_MAX_MOTION_BD      = 8; // Motion Board Max. Number
        public const int DEF_NON_MOTION_BD      = 0; // Motion Board No-Use Number
        public const int DEF_NON_MOTION_BD_ID   = -1;// Motion Board Non-Config

        public const int DEF_ALL_MOTION_BD_ID   = -1;// Motion Board ID ALL
        public const int DEF_MOTION_BD_ID1      = 0; // Motion Board ID #1
        public const int DEF_MOTION_BD_ID2      = 1; // Motion Board ID #2
        public const int DEF_MOTION_BD_ID3      = 2; // Motion Board ID #3
        public const int DEF_MOTION_BD_ID4      = 3; // Motion Board ID #4
        public const int DEF_MOTION_BD_ID5      = 4; // Motion Board ID #5
        public const int DEF_MOTION_BD_ID6      = 5; // Motion Board ID #6
        public const int DEF_MOTION_BD_ID7      = 6; // Motion Board ID #7
        public const int DEF_MOTION_BD_ID8      = 7; // Motion Board ID #8

        public const bool DEF_AUTO_CP_NON       = false; // 자동 가,감속 미사용
        public const bool DEF_AUTO_CP_USE       = true;  // 자동 가,감속 사  용

        // 축 수
        public const int DEF_MAX_AXIS_NO        = DEF_AXIS_NO_PER_BRD * DEF_MAX_MOTION_BD;
        public const int DEF_MIN_AXIS_NO        = 0;

        // Coordinate Value
        public const int DEF_MAX_COORDINATE     = DEF_XYTZ; // Multi Axes의 축 Max.
        public const int DEF_ALL_COORDINATE     = -1; // Multi Axes의 모든 축 선택

        //public const int DEF_AXIS_USE           = 1;  // 축 사  용
        //public const int DEF_AXIS_NON           = 0;  // 축 미사용

        //public const int DEF_AXIS_NON_NO        = 0;  // 축 0개
        //public const int DEF_AXIS_ALL_ID        = -1; // 축 전체 선택
        public const int DEF_AXIS_NONE_ID        = -1; // 축 ID 미할당 (축 미구성)

        // X,Y,T 코드 정의
        public const int DEF_X = 0;
        public const int DEF_Y = 1;
        public const int DEF_T = 2;
        public const int DEF_Z = 3;
        public const int DEF_XYTZ = 4;

        // Left / Right 코드 정의
        public const int DEF_LEFT = 0;
        public const int DEF_RIGHT = 1;

        // UP / Down 코드 정의
        public const int DEF_UP = 0;
        public const int DEF_DOWN = 1;

        // 좌표 종류를 정의 한다.
        public const int POSITION_FIXED = 0;
        public const int POSITION_OFFSET = 1;
        public const int POSITION_MODEL = 2;
        public const int POSITION_MARKOFFSET = 3;
        public const int POSITION_MAX = 4;

        // Limit Sensor Check 상태
        public const int DEF_NONE_SENSOR_CHECK = 0;
        public const int DEF_NEG_SENSOR_CHECK = 1;
        public const int DEF_POS_SENSOR_CHECK = 2;
        public const int DEF_HOME_SENSOR_CHECK = 3;

        // Move Priority Value ("0"이 우선순위 높음)
        public enum EPriority
        {
            NONE = -1,
            P_0 = 0,
            P_1,
            P_2,
            P_3,
            P_4,
            P_5,
            P_6,
            P_7,
            P_8,
            P_9,
            P_10,
            P_11,
            P_12,
            P_13,
            P_14,
            P_15,
            P_16,
            P_17,
            P_18,
            P_19,
            P_20,
            P_21,
            P_22,
            P_23,
            P_24,
            P_25,
            P_26,
            P_27,
            P_28,
            P_29,
            P_30,
            P_31,
            P_32,
            P_33,
            P_34,
            P_35,
            P_36,
            P_37,
            P_38,
            P_39,
            P_40,
            P_41,
            P_42,
            P_43,
            P_44,
            P_45,
            P_46,
            P_47,
            P_48,
            P_49,
            P_50,
            P_51,
            P_52,
            P_53,
            P_54,
            P_55,
            P_56,
            P_57,
            P_58,
            P_59,
            P_60,
            P_61,
            P_62,
            P_63,
            MAX,
        }

        // Move 종류
        public const int DEF_MOVE_POSITION   = 0; // 사다리꼴 속도 Profile, 절대좌표 이동
        public const int DEF_SMOVE_POSITION  = 1; // S-Curve 속도 Profile, 절대좌표 이동
        public const int DEF_MOVE_DISTANCE   = 2; // 사다리꼴 속도 Profile, 상대거리 이동
        public const int DEF_SMOVE_DISTANCE  = 3; // S-Curve 속도 Profile, 상대거리 이동
        public const int DEF_TMOVE_POSITION  = 4; // 비대칭 사다리꼴 속도 Profile, 절대좌표 이동
        public const int DEF_TSMOVE_POSITION = 5; // 비대칭 S-Curve 속도 Profile, 절대좌표 이동
        public const int DEF_TMOVE_DISTANCE  = 6; // 비대칭 사다리꼴 속도 Profile, 상대거리 이동
        public const int DEF_TSMOVE_DISTANCE = 7; // 비대칭 S-Curve 속도 Profile, 상대거리 이동

        public const int DEF_2AXIS_MOVE      = 2;
        public const int DEF_3AXIS_MOVE      = 3;
        public const int DEF_4AXIS_MOVE      = 4;

        public const int DEF_MAX_GROUP_NO    = 8;
        public const int DEF_MIN_GROUP_NO    = 1;

        // Stop 종류
        public const int DEF_STOP            = 0; // Stop
        public const int DEF_ESTOP           = 1; // E-Stop
        public const int DEF_VSTOP           = 2; // V-Stop

        // Limit Sensor 종류
        public const int DEF_HOME_SENSOR     = 0; // Home Sensor
        public const int DEF_POSITIVE_SENSOR = 1; // Positive Sensor
        public const int DEF_NEGATIVE_SENSOR = 2; // Negative Sensor

        // S/W Limit 종류
        public const bool DEF_POSITIVE_SW    = true;  // S/W Positive Limit
        public const bool DEF_NEGATIVE_SW    = false; // S/W Negative Limit

        // Level 종류
        //#ifdef DEF_3_4LINE
        public const bool DEF_HIGH = true;         // High Level
                                                   //#else
                                                   //const	bool	DEF_HIGH				= false;			// High Level
                                                   //#endif

        public const bool DEF_LOW = false;     // Low Level

        // Event 종류
        public const int DEF_NO_EVENT                 = 0; // ignore a condition
        public const int DEF_STOP_EVENT               = 1; // generate a stop event
        public const int DEF_E_STOP_EVENT             = 2; // generate an e_stop event
        public const int DEF_ABORT_EVENT              = 3; // disable PID control, and disable the amplifier

        // Motor 종류
        public const int DEF_SERVO_MOTOR              = 0; // 속도형 Servo
        public const int DEF_STEPPER                  = 1; // 일반 Stepper
        public const int DEF_MICRO_STEPPER            = 2; // Micro Stepper 혹은 위치형 Servo

        // Feedback 종류
        public const int DEF_FB_ENCODER               = 0; // Encoder Feedback Device
        public const int DEF_FB_UNIPOLAR              = 1; // Unipolar Feedback Device
        public const int DEF_FB_BIPOLAR               = 2; // Bipolar Feedback Device

        // Control_Loop 종류
        public const bool DEF_OPEN_LOOP               = false; // Open Loop
        public const bool DEF_CLOSED_LOOP             = true;  // Closed Loop

        // Control 방법
        public const bool DEF_V_CONTROL               = false; // 속도제어
        public const bool DEF_T_CONTROL               = true;  // 토크제어

        //
        public const bool DEF_UNIPOLAR                = false;
        public const bool DEF_BIPOLAR                 = true;

        // 적분모드 적용 사양
        public const int DEF_IN_STANDING              = 0; // 정지시만 적용
        public const int DEF_IN_ALWAYS                = 1; // 항상 적용

        // Pulse 종류
        public const int DEF_TWO_PULSE                = 0; // Two Pulse, CW+CCW
        public const int DEF_SIGN_PULSE               = 1; // Sign + Pulse

        // PC10 정의
        public const int DEF_PC_INDEXSEL_0            = 0; // IndexSel 없음
        public const int DEF_PC_INDEXSEL_1            = 1; // IndexSel 1축
        public const int DEF_PC_INDEXSEL_2            = 2; // IndexSel 2축

        public const int DEF_PC_INDEX_MAX_NO          = 8; // Position Compare를 실시할 Index 최대 번호
        public const int DEF_PC_INDEX_MIN_NO          = 1;

        public const bool DEF_PC_TRANSPARENT          = false; // Transparent Mode
        public const bool DEF_PC_LATCH                = true;  // Latch Mode

        public const int DEF_PC_EQUAL                 = 1; // Equal
        public const int DEF_PC_GT                    = 2; // >
        public const int DEF_PC_LT                    = 3; // <

        public const int DEF_PC_OUT_NON               = 0; // 축별 ON/OFF
        public const int DEF_PC_OUT_AND               = 1; // 두 축 AND
        public const int DEF_PC_OUT_OR                = 2; // 두 축 OR

        // I/O 정의
        public const int DEF_MAX_IO_PER_BOARD         = 32; // Board당 I/O Bit 수
        public const int DEF_MIN_IO_BIT               = 0;
        public const bool DEF_IO_TYPE_IN              = false;
        public const bool DEF_IO_TYPE_OUT             = true;
        public const int DEF_MAX_IO_PORT              = 3;
        public const int DEF_MIN_IO_PORT              = 0;
        public const int DEF_MAX_ANALOG_CH            = 4;
        public const int DEF_MIN_ANALOG_CH            = 0;

        // I/O Interrupt
        public const int DEF_BOARD_ENABLE_MODE        = 0; // Board Interrup Enable Mode
        public const int DEF_STOP_EVENT_MODE          = 1; // Stop Event Interrup Mode
        public const int DEF_ESTOP_EVENT_MODE         = 2; // E-Stop Event Interrup Mode

        // In-Command 정의
        public const int DEF_INSEQUENCE               = 0; // in sequence (이동)
        public const int DEF_INMOTION                 = 1; // in motion (속도 이동)
        public const int DEF_INPOSITION               = 2; // in position (위치)

        // Wait Done 정의
        public const bool DEF_MOTION_DONE             = false; // motion_done
        public const bool DEF_AXIS_DONE               = true;  // axis_done

        // Current/Command 정의
        public const bool DEF_CURRENT_VAL             = false; // 현재값
        public const bool DEF_COMMAND_VAL             = true;  // 명령값

        // ABS Motor 종류
        public const int DEF_SAMSUNGCSDJ              = 1; // 삼성 CSDJ, CSDJ + SERVO DRIVE
        public const int DEF_YASKAWA_SERVO_DRIVE      = 2; // YASKAWA SERVO DRIVE

        // Encoder Direction
        public const int DEF_CORD_CCW                 = 1; // 반시계방향
        public const int DEF_CORD_CW                  = 0; // 시계방향

        // Digital Filter Defines
        public const int DEF_GAIN_NUMBER              = 5; // elements expected get/set_filter(...)
        public const int DEF_GA_P                     = 0; // proportional gain
        public const int DEF_GA_I                     = 1; // integral gain
        public const int DEF_GA_D                     = 2; // derivative gain-damping term
        public const int DEF_GA_F                     = 3; // velocity feed forward
        public const int DEF_GA_ILIMIT                = 4; // integration summing limit

        // Sampling Rate
        public const int DEF_SAMPLING_4MSEC           = 1; // Samspling Rate 4msec
        public const int DEF_SAMPLING_2MSEC           = 2; // Samspling Rate 2msec
        public const int DEF_SAMPLING_1MSEC           = 3; // Samspling Rate 1msec

        // Analog Channel
        public const int DEF_ANALOG_IN_MAX_CH         = 4; // Analog Input Channel 최대값
        public const int DEF_ANALOG_IN_NON_CH         = 0; //

        // Lmit Vlaue
        public const int DEF_ACCEL_LIMIT              = 25000;
        public const int DEF_VEL_LIMIT                = 5000000;
        public const int DEF_POS_SW_LIMIT             = 2147483640;
        public const int DEF_NEG_SW_LIMIT             = -2147483640;
        public const int DEF_ERROR_LIMIT              = 35000;
        public const int DEF_PULSE_RATIO              = 255;
        public const double DEF_DEGREE                = 1000.0;

        // Interpolation 관련
        public const int DEF_START_MOVE               = 1; // 동작 시작
        public const int DEF_MOVING                   = 2; // 동작 중
        public const int DEF_END_MOVE                 = 3; // 동작 완료

        // Position I/O
        public const int DEF_POSITION_IO_MAX_NO       = 10; //
        public const int DEF_POSITION_IO_MIN_NO       = 1;  //
        public const int DEF_POSITION_IO_NON_NO       = 0;  //

        // Spline Data
        public const int DEF_POSITION_DATA_MAX_NO     = 30;
        public const int DEF_POSITION_DATA_MIN_NO     = 1;
        public const int DEF_SPLINE_MOTION_MAX_NO     = 20;
        public const int DEF_SPLINE_MOTION_MIN_NO     = 1;
        public const int DEF_SPLINE_MOVE_PATH_MAX_NO  = 500;
        public const int DEF_SPLINE_MOVE_PATH_MIN_NO  = 1;

        // Event Source Status defines
        public const int DEF_ST_NONE                  = 0x0000;
        public const int DEF_ST_HOME_SWITCH           = 0x0001;
        public const int DEF_ST_POS_LIMIT             = 0x0002;
        public const int DEF_ST_NEG_LIMIT             = 0x0004;
        public const int DEF_ST_AMP_FAULT             = 0x0008;
        public const int DEF_ST_A_LIMIT               = 0x0010;
        public const int DEF_ST_V_LIMIT               = 0x0020;
        public const int DEF_ST_X_NEG_LIMIT           = 0x0040;
        public const int DEF_ST_X_POS_LIMIT           = 0x0080;
        public const int DEF_ST_ERROR_LIMIT           = 0x0100;
        public const int DEF_ST_PC_COMMAND            = 0x0200;
        public const int DEF_ST_OUT_OF_FRAMES         = 0x0400;
        public const int DEF_ST_AMP_POWER_ONOFF       = 0x0800;
        public const int DEF_ST_ABS_COMM_ERROR        = 0x1000;
        public const int DEF_ST_INPOSITION_STATUS     = 0x2000;
        public const int DEF_ST_RUN_STOP_COMMAND      = 0x4000;
        public const int DEF_ST_COLLISION_STATE       = 0x8000;

        // 원점복귀 Step
        public const int DEF_ORIGIN_START_STEP        = 0;  // START
        public const int DEF_ORIGIN_FIRST_SET_STEP    = 10; // FIRST SETTING
        public const int DEF_ORIGIN_1ST_MOVE_STEP     = 20; // MOVE 1st
        public const int DEF_ORIGIN_2ND_MOVE_STEP     = 30; // MOVE 2nd
        public const int DEF_ORIGIN_3RD_MOVE_STEP     = 40; // MOVE 3rd
        public const int DEF_ORIGIN_4TH_MOVE_STEP     = 50; // MOVE 4th
        public const int DEF_ORIGIN_STOP_MOVE_STEP    = 60; // STOP MOVE
        public const int DEF_ORIGIN_LAST_SET_STEP     = 70; // LAST SETTING
        public const int DEF_ORIGIN_SET_ORIGIN_STEP   = 80; // SET ORIGIN
        public const int DEF_ORIGIN_ERROR_STEP        = 999; // ERROR
        public const int DEF_ORIGIN_FINISH_STEP       = 1000; // FINISH

        // 기타
        public const bool DEF_ENABLE                  = true;  // Enable
        public const bool DEF_DISABLE                 = false; // Disable
        public const int ERR_MAX_ERROR_LEN            = 80;    // maximum length for error massage string
        public const int DEF_DEFAULT_ORIGIN_WAIT_TIME = 60;    // 원점복귀 제한 시간 default 값
        public const bool DEF_COLLISION_SUB_POS       = false; // Collision Prevent 시 두 축 값의 차를 사용
        public const bool DEF_COLLISION_ADD_POS       = true;  // Collision Prevent 시 두 축 값의 합을 사용
        public const bool DEF_COLLISION_LESSTHAN      = false; // Collision Prevent 시 기준값이 두 축 값다 작을 때 정지
        public const bool DEF_COLLISION_GREATTHAN     = true;  // Collision Prevent 시 기준값이 두 축 값다 클 때 정지

        // MAX Frame
        public const int DEF_MAX_FRAME_NUM            = 100;   // For use in Call frames_left Function

        /// <summary>
        /// Axis attribute class
        /// </summary>
        public class CMotionAxis
        {
            /** Motor Type (0:속도형 Servo, 1:Stepper, 2:Micro Stepper or 위치형 Servo) - 2번만 지원 */
            public int iMotorType;
            /** Loop Type (0:Open Loop, 1:Closed Loop) */
            public bool bLoopType;
            /** Feedback Device Type (0:Encoder, 1:Unipolar, 2:Bipolar) */
            public int iFeedbackType;
            /** 속도형 Servo 제어모드 (false:속도제어, true:토크제어) */
            public bool bVServoControl;
            /** 속도형 Servo 출력모드 (true:Uni-Polar, false:Bi-Polar) */
            public bool bVServoPolar;
            /** Stepper Pulse 분주비 */
            public int iStepperPulseR;
            /** Stepper 전자기어비 */
            public double dStepperEGear;
            /** Motor Pulse Type (0:Two-Pulse(CW+CCW), 1:Sign+Pulse) */
            public bool bPulseType;
            /** Encoder 방향 () */
            public bool bEncoderDir;
            /** 좌표 방향 () */
            public bool bCoordinateDir;
            /** AMP Enable Level (true:HIGH, false:LOW) */
            public bool bAmpEnableLevel;
            /** AMP Reset Level (true:HIGH, false:LOW) */
            public bool bAmpResetLevel;
            /** AMP Fault Level (true:HIGH, false:LOW) */
            public bool bAmpFaultLevel;
            /** In-Position Level (true:HIGH, false:LOW) */
            public bool bInpositionLevel;
            /** In-Position Level Required (true, false) */
            public bool bInpositionLevelRequired;
            /** Positive Sensor Level (true:HIGH, false:LOW) */
            public bool bPositiveLevel;
            /** Negative Sensor Level (true:HIGH, false:LOW) */
            public bool bNegativeLevel;
            /** Home Sensor Level (true:HIGH, false:LOW) */
            public bool bHomeLevel;
            /** AMP Fault Event */
            public int iAmpFaultEvent;
            /** Positive Sensor Event */
            public int iPositiveEvent;
            /** Negative Sensor Event */
            public int iNegativeEvent;
            /** Home Sensor Event */
            public int iHomeEvent;
            /** Positive SW Limit */
            public double dPositiveSWLimit;
            /** Negative SW Limit */
            public double dNegativeSWLimit;
            /** Positive SW Limit Event */
            public int iPositiveSWEvent;
            /** Negative SW Limit Event */
            public int iNegativeSWEvent;
            /** In-Position Error */
            public double dInpositionError;
        }

        /// <summary>
        /// MotionBoard for MMC, MMI
        /// </summary>
        public class CMBType_MMC
        {
            // Motion Board - 초기 구성 장착된 Board 개수 (0:미구성, 1 ~ 8)
            public int iMaxBoardNo;                        // Initialize()에서 기록
                                                           /** Motion Board DPRAM Address */
            public long[] lAddress = new long[DEF_MAX_MOTION_BD];     // Initialize()에서 기록
                                                                      /** 자동 가,감속 설정 여부 (true:자동) */
            public bool[] bAutoCP = new bool[DEF_MAX_MOTION_BD];      // SetAutoCP()에서 기록
                                                                      /** 원점복귀 대기 시간 (초단위) */
            //double	dOriginWaitTime;

            /** MMC-PC10 Option Board IndexSel */
            public int[] iPC10IndexSel = new int[DEF_MAX_MOTION_BD];
            /** MMC-PC10 Option Board 사용 축 */
            public int[,] iPC10Axis = new int[DEF_MAX_MOTION_BD, 2];
        }

        /// <summary>
        /// Motor에 대한 특성 정보
        /// </summary>
        public class CAxis1
        {
            /** 축 ID (-1:미사용, 0 ~ 64) */
            public int iAxisID;
            /** 축 이름 (최대 32문자) */
            string Name;
            /** 동시 이동 시 우선 순위 지정 (1 ~ 64, 0:미사용) */
            public int iMovePriority;
            /** 원점복귀 동시 이동 시 우선 순위 지정 (1 ~ 64, 0:미사용) */
            public int iOriginPriority;
            /** 원점위치 */
            public double dHomePosition;
            /** S/W (-)방향 이동 제한 위치 */
            public double dNegativeLimitPosition;
            /** S/W (+)방향 이동 제한 위치 */
            public double dPositiveLimitPosition;
            /** 이동 속도 */
            public double dMovingVelocity;
            /** 이동 가속도 */
            public int iMovingAccelerate;
            /** 이동 감속도 */
            public int iMovingDecelerate;
            /** Coarse 속도 */
            public double dCoarseVelocity;
            /** Coarse 가속도 */
            public int iCoarseAccelerate;
            /** Fine 속도 */
            public double dFineVelocity;
            /** Fine 가속도 */
            public int iFineAccelerate;
            /** Jog 속도 */
            public double dJogVelocity;
            /** Jog Pitch */
            public double dJogPitch;
            /** 축 이동방향 (true:+, false:-) */
            public bool bSign;
            /** 축 원점복귀 진행방향(Coarse 속도 구간) (true:+, false:-) */
            public bool bOriginDir;
            /** 축 원점복귀 진행방향(Fine 속도 구간) (TRUEL+, false:-) */
            public bool bOriginFineDir;
            /** C상 사용여부 (true:사용함) */
            public bool bCPhaseUse;
            /** 이동 값에 대한 Scale (default:1.0) */
            public double dScale;
            /** 이동 지연 시간 */
            public double dMoveTime;
            /** 이동후 안정화 시간 */
            public double dMoveAfterTime;
            /** Tolerance - 위치 허용 오차 */
            public double dTolerance;
            /** 원점복귀 대기 시간 (초단위) */
            public double dOriginWaitTime;
        }

        /// <summary>
        /// 원점복귀 Thread에 전달할 인수에 대한 Data를 관리
        /// </summary>
        public class COriginThread
        {
            /** 원점복귀할 좌표, -1             = All Axis */
            public int iCID;
            /** 원점복귀할 축 지정, iCoordinateID=-1일때만 사용
             * iCoordinateID가 -1이 아니면 사용안함 (null)
             * 배열구조에 사용하고자하는 축 위치에 true지정 */
            public bool[] bUse = new bool[DEF_MAX_COORDINATE];
            /** 원점복귀 시 이동할 지 여부, true    =이동 포함 */
            public bool bMoveOpt;
            /** 원점복귀하기 전 Limit Sensor Event 설정 값 */
            public int[] iPositiveLimit = new int[DEF_MAX_AXIS_NO];
            public int[] iNegativeLimit = new int[DEF_MAX_AXIS_NO];
            public int[] iHomeLimit = new int[DEF_MAX_AXIS_NO];
            public bool[] bPositiveLevel = new bool[DEF_MAX_AXIS_NO];
            public bool[] bNegativeLevel = new bool[DEF_MAX_AXIS_NO];
            public bool[] bHomeLevel = new bool[DEF_MAX_AXIS_NO];
        }

        /// <summary>
        /// 원점복귀 여부와 상관없이 해당 축의 물리적인 위치를 체크하기 위한 센서들을 관리하는 class
        /// </summary>
        public class COneAxisZoneCheck
        {
            private int Length;          // Zone
            public int[] ZoneAddr;     // Sensor Address, if -1, not use

            public COneAxisZoneCheck(int Length)
            {
                if (Length < 1) Length = 1;
                this.Length = Length;
                ZoneAddr = new int[Length];
                for (int i = 0; i < Length; i++)
                {
                    ZoneAddr[i] = -1;
                }
            }
        }

        public class CMAxisZoneCheck
        {
            public COneAxisZoneCheck[] Axis = new COneAxisZoneCheck[DEF_XYTZ];
            public bool[] UseSafetyMove = new bool[DEF_XYTZ];  // Axis Moving시에 SafetyPos으로 이동후에 목표위치로 이동하는 옵션
            public CPos_XYTZ SafetyPos;    // 안전 위치. 비간섭 position


            public CMAxisZoneCheck(int XLength, int YLength, int TLength, int ZLength)
            {
                Axis[DEF_X] = new COneAxisZoneCheck(XLength);
                Axis[DEF_Y] = new COneAxisZoneCheck(YLength);
                Axis[DEF_T] = new COneAxisZoneCheck(TLength);
                Axis[DEF_Z] = new COneAxisZoneCheck(ZLength);
            }

            public CMAxisZoneCheck(int[] ZoneLength)
            {
                for (int i = 0; i < DEF_XYTZ; i++)
                {
                    Axis[i] = new COneAxisZoneCheck(ZoneLength[i]);
                }
            }

            public CMAxisZoneCheck(COneAxisZoneCheck[] Axis)
            {
                for (int i = 0; i < DEF_XYTZ; i++)
                {
                    Axis[i] = ObjectExtensions.Copy(Axis[i]);
                }
            }
        }
    }

    interface IMotionLib
    {
        /**
         * 축 사용 여부 설정 (이미 사용중인 축은 사용으로 설정 불가)
         *
         * 축 사용 여부 설정은 Motor Parameter 설정과는 무관하며, 사용자에 의해 설정이 되어야 한다.
         *
         * @param	iAxisID		: 축 ID
         * @param	bState			: 사용 여부 (true=사용, false=미사용)
         */
        int SetUseAxis(int iAxisID, bool bState);

        /**
         * 축 사용 여부 읽기
         *
         * @param	iAxisID		: 축 ID
         * @param	bState		: 사용 여부 (true=사용, false=미사용)
         */
        int GetUseAxis(int iAxisID, out bool bState);

        /**
         * Motor Parameter 설정 (Board 초기화 후 사용 가능)
         *
         * 전달된 CMotionAxis의 내용으로 Motion을 설정한다. (축 1개 단위로 설정)
         *
         * 1. Board가 초기화 안되어 있으면 Error Return
         * 2. 축 AMP Enable(Servo ON)이면 Disable(OFF) 후 작업 진행
         * 3. Motor 종류 설정
         * 4. Feedback Device 설정
         * 5. Loop 형태 설정
         * 6. Motor 종류에 따라
         *		6.1 속도형 Servo의 경우
         *			제어 모드와 출력 형태 설정
         *		6.2 일반 Stepper의 경우
         *			펄스 분주비와 전자기어비 설정
         *		6.3 Micro Stepper 혹은 위치형 Servo의 경우
         *			펄스 분주비(default:8)와 전자기어비(default:1.0) 설정
         * 7. 출력 펄스 형태 설정
         * 8. Encoder와 좌표 방향 설정
         * 9. AMP Enable, Fault, Reset 설정
         * 10. Home, Positive, Negative Sensor 설정
         * 11. Positive, Negative S/W Limit 설정
         * 12. In-Position 설정
         * 13. 축 AMP Enable(Servo ON)이었으면 Enable(ON) 설정
         *
         * @param	iAxisID		: 축 ID, -1=허용안됨
         * @param	mAx				: Motor Parameter
         * @param	bBootOpt		: (OPTION=false) boot file에 기록할지 여부, true=boot file에 기록
         */
        int SetMotorParam(int iAxisID, CMotionAxis mAx, bool bBootOpt = false);

        /**
         * Motor Parameter 읽기
         *
         * @param	iAxisID		: 축 ID, -1=All Motor
         * @param	mAx			: Motor Parameter, iAxisID=-1이면 배열 구조로 구성
         */
        int GetMotorParam(int iAxisID, out CMotionAxis mAx);
        int GetMotorParam(out CMotionAxis[] mAx);


        /**
         * Motion Board Parameter 설정
         *
         * @param	MotionBd		: Motion Board Parameter
         */
        int SetBoardParam(CMBType_MMC MotionBd);

        /**
         * Motion Board Parameter 읽기
         *
         * @param	mBd			: Motion Board Parameter
         */
        int GetBoardParam(out CMBType_MMC mBd);

        /**
         * 구성된 Board 개수 읽기
         *
         * @param	iBdNum		: Board 구성 개수
         */
        int GetBoardNum(out int iBdNum);

        /**
         * Motion Board 초기화
         * 
         * 1. Motion Board 초기화가 수행된 경우
         * 	(1) Board 구성 수와 Board DPRAM Address가 같은 경우
         * 		→ Board 초기화 과정 Pass
         * 	(2) Board 구성 수가 다른 경우
         * 		→ Board 초기화 오류 처리
         * 	(3) Board DPRAM Address가 다른 경우
         * 		→ Board 초기화 오류 처리
         * 
         * 2. Motion Board 초기화가 수행되지 않은 경우
         * 	→ Board 초기화 수행
         *
         * @param   iBdNum			: Motion Board 수 (1 ~ 4)
         * @param	lAddress		: Board DPRAM Address (Board 수만큼)
         */
        int Initialize(int iBdNum, out long lAddress);

        /**
         * Motion Board 초기화 여부 읽기
         *
         * @param	bInit			: Board 초기화 여부, true=초기화됐음
         */
        int GetBoardInit(out bool bInit);

        /**
         * Motion Board의 제어 축 수를 돌려준다.
         *
         * @param   iBdNum			: Motion Board ID (0 ~ 7), -1=All Board
         * @param	iAxes		: Board에 구성된 제어 축 수
         */
        int GetAxes(int iBdNum, out int iAxes);

        /**
         * 직선, 원, 원호등의 동작을 수행할 각 좌표계의 축을 정의
         * 같은 Board의 축으로 구성해야 한다.
         *
         * @param   iAxNum			: 축 수 (1 ~ 8)
         * @param	iMapArray	: 축 ID 배열 (같은 Board안의 축 ID이어야 한다.)
         */
        int MapAxes(int iAxNum, out int iMapArray);

        /**
         * 직선, 원, 원호등의 동작을 수행할 각 좌표계의 축을 정의
         * Motion 프로그램 지연과 축 다음 동작 실행 지연 2가지를 제공한다.
         * 지연시간은 1msec 단위이며 0보다 커야 한다.
         *
         * @param	iAxis			: 축 ID (-1 ~ 63), -1=프로그램지연
         * @param	lDuration		: 지연시간 (1msec단위)
         */
        int Dwell(int iAxis, out long lDuration);

        /**
         * I/O Bit가 지정된 상태로 될 때까지 해당 축의 다음 동작 실행을 지연한다.
         * I/O Bit No는 Board 구성 상태에 따라 64개 단위로 변동된다.
         *  (Board#1 : 0 ~ 63, Board #2 : 64 ~ 63, Board #3 : 64 ~ 95, Board #4 : 96 ~ 127)
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param   iBitNo			: I/O Bit No
         * @param	bState			: I/O Bit 상태
         */
        int IOTrigger(int iAxis, int iBitNo, bool bState);

        /**
         * 축 명령 수행에 대한 상태를 돌려준다.
         * 명령 수행 완료된 상태이면 SUCCESS를 Return한다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param   iType			: 조회 종류, 0=이동명령, 1=속도명령, 2=InPosition 범위내 여부
         */
        int InCommand(int iAxis, int iType);

        /**
         * 직선, 원, 원호등의 동작이 완료되었는지 여부를 돌려준다.
         * MapAxes()에서 설정한 축들에 대해 확인한다.
         *
         * @param	bStatus		: 완료 여부
         */
        int AllDone(out bool bStatus);

        /**
         * 축의 동작 완료될 때까지 대기한다.
         * 이동 및 속도 이동 완료 확인 또는 더불어 In-Position 여부까지 확인한다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	iMode			: 대기 종류, 0=이동 및 속도이동 완료,
         *										 1=이동 및 속도이동 완료 & InPosition 범위내 이동 완료
         */
        int WaitDone(int iAxis, int iMode);

        /**
         * 지정 축이 동작 완료될 때까지 기다린다.
         * @param	iAxis			: 축 ID (0 ~ 63) 혹은 축 수 (1 ~ 64)
         */
        int WaitForDone(int iAxis);
        int WaitForDone(int[] arrayAxis);


        /**
         * 지정 축의 AMP Fault를 Clear하거나 Fault Port를 Enable 상태로 지정한다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	bState			: 지정할 상태, false=Clear, true=Enable
         */
        int SetAmpFaultEnable(int iAxis, bool bState);

        /**
         * 축 모든 현재 상태를 한꺼번에 읽는다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	iState		: 상태, (axis_source, in_sequence, get_com_velocity, get_act_velocity,
         *									 motion_done, in_position, axis_done 결과값)
         * @param	lStatus		: 상태, (get_io 결과값)
         * @param	dStatus		: 상태, (get_position, get_command, get_error 결과값)
         */
        int GetAllStatus(int iAxis, out int iStatus, out long lStatus, out double dStatus);

        /**
         * 축 현재 상태를 읽는다.
         * 상태는 bit 조합으로 구성되어 있다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	iState		: 상태, 각 상태는 bit 조합으로 구성된다.
         */
        int GetAxisSource(int iAxis, out int iState);

        /**
         * 축의 센서(Home, Positive, Negative) 상태를 읽는다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	iType			: 센서 종류, 0=Home, 1=Positive, 2=Negative
         * @param	bState		: 센서 상태, true=Active, false=No Active
         */
        int GetSensorStatus(int iAxis, int iType, out bool bState);

        /**
         * AMP Disable/Enable 상태를 읽는다. (Servo ON/OFF)
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	bState		: AMP Enable 상태, true=Enable, false=Disable
         */
        int GetAmpEnable(int iAxis, out bool bState);

        /**
         * AMP Enable의 Active Level을 읽는다.
         * boot file 또는 실행중인 memory에서 읽을 수 있다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	bLevel		: Enable Level, true=HIGH, false=LOW
         * @param	bBootOpt		: (OPTION=false) boot file에서 읽을지 여부, true=boot file에서 읽음
         */
        int GetAmpEnableLevel(int iAxis, out bool bLevel, bool bBootOpt = false);

        /**
         * 축의 현재 Event 발생 상태를 읽는다.
         *
         *		NO_EVENT		0		Event 발생없이 정상 동작
         *		STOP_EVENT		1		stop_rate로 감속하면서 정지
         *		E_STOP_EVENT	2		e_stop_rate로 감속하면서 정지
         *		ABORT_EVENT		3		AMP disable 상태
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	iState		: Event 내역, 0=NO EVENT, 1=STOP EVENT, 2=ESTOP EVENT, 3=ABORT EVENT
         */
        int GetAxisState(int iAxis, out int iState);

        /**
         * Board의 Position Latch 여부를 읽는다.
         *
         * @param   iBdNum			: Board ID (0 ~ 7)
         * @param	bState		: Position Latch 여부, true=Latch상태
         */
        int GetAxisLatchStatus(int iBdNum, out bool bState);

        /**
         * 축에 발생된 Event를 해제하고, 다음 명령부터 실행한다.
         * Event 발생 후에는 항상 Event를 해제해 주어야 한다.
         * ABORT_EVENT 발생 시에는 Event 해제 후 AMP가 Disable 상태이므로 다시 Enable해주어야 한다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         */
        int ClearStatus(int iAxis);

        /**
         * 축의 Frame Buffer를 Clear한다.
         * 축별 최대 50개의 Frame의 내용을 Clear한다. 단 현재 실행중인 명령은 계속 수행된다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         */
        int ClearFrames(int iAxis);

        /**
         * 축의 비어있는 Interpolation Frame 개수를 돌려준다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	iFrameNo		: Frame 개수
         */
        int FramesInterpolation(int iAxis, out int iFrameNo);

        /**
         * 축의 비어있는 Frame 개수를 돌려준다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	iFrameNo		: 비어있는 Frame 개수
         */
        int FramesLeft(int iAxis, out int iFrameNo);

        /**
         * 해당 Board의 Latch 상태를 지정하고, Latch Status를 False로 만들거나, S/W적으로 Position을 Latch한다.
         *
         * @param   iBdNum			: Board ID (0 ~ 7)
         * @param	bType			: Latch 종류, false=S/W Position Latch, true=Board Latch Enable/Disable 지정
         * @param	bState			: (OPTION=false) bType=true인 경우 Enable/Disable 지정
         */
        int Latch(int iBdNum, bool bType, bool bState = false);

        /**
         * 지정 축의 Latch된 Position을 돌려준다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	dPosition		: Latch된 Position
         */
        int GetLatchedPosition(int iAxis, out double dPosition);

        /**
         * 동작중 목표위치를 재지정할 때 사용한다. (원, 원호 동작중에는 적용되지 않는다.)
         *
         * @param   iLen			: 축 수
         * @param	iAxes		: 축 ID 배열
         * @param	dDist		: 위치 보정값
         * @param	iAccel		: 이동 가,감속 구간
         */
        int CompensationPos(int iLen, out int iAxes, out double dDist, out int iAccel);

        /**
         * Board DPRAM Address를 읽는다.
         *
         * @param   iBdNum			: Board ID (0 ~ 7)
         * @param	lAddr			: DPRAM Address
         */
        int GetDpramAddress(int iBdNum, out long lAddr);

        /**
         * 절대치 Motor의 Type을 읽는다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	iType		: Motor 종류, 1=삼성CSDJ, CSDJ+SERVO DRIVE, 2=YASKAWA SERVO DRIVE
         */
        int GetAbsEncoderType(int iAxis, out int iType);

        /**
         * 축의 이동 최고속도와 가,감속 구간값의 제한값을 읽는다.
         * boot file 또는 실행중인 memory에서 읽을 수 있다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	dVelocity		: 이동 최고속도, 1 ~ 2047000 coutn/sec
         * @param	iAccel		: 가,감속 구간값, 1 ~ 200, 10msec 단위
         * @param	bBootOpt		: (OPTION=false) boot file에서 읽을지 여부, true=boot file에서 읽음
         */
        int GetVelLimit(int iAxis, out double dVelocity, out int iAccel, bool bBootOpt = false);

        /**
         * AMP Drive에 Fault 발생 상태를 읽는다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	bStatus		: AMP Fault 상태를 읽는다. true=FAULT, false=NORMAL
         */
        int GetAmpFaultStatus(int iAxis, out bool bStatus);

        /**
         * AMP Drive에 Fault 발생 시 동작할 Event를 읽는다.
         * boot file 또는 실행중인 memory에서 읽을 수 있다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	iAction		: 동작할 Event, NO EVENT, STOP EVENT, ESTOP EVENT, ABORT EVENT
         * @param	bBootOpt		: (OPTION=false) boot file에서 읽을지 여부, true=boot file에서 읽음
         */
        int GetAmpFaultEvent(int iAxis, out int iAction, bool bBootOpt = false);

        /**
         * AMP Enable의 Active Level을 읽는다.
         * boot file 또는 실행중인 memory에서 읽을 수 있다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	bLevel		: Enable 신호 Level, true=HIGH, false=LOW
         * @param	bBootOpt		: (OPTION=false) boot file에서 읽을지 여부, true=boot file에서 읽음
         */
        int GetAmpEanbleLevel(int iAxis, out bool bLevel, bool bBootOpt = false);

        /**
         * AMP Fault의 Active Level을 읽는다.
         * boot file 또는 실행중인 memory에서 읽을 수 있다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	bLevel		: Fault 신호 Level, true=HIGH, false=LOW
         * @param	bBootOpt		: (OPTION=false) boot file에서 읽을지 여부, true=boot file에서 읽음
         */
        int GetAmpFaultLevel(int iAxis, out bool bLevel, bool bBootOpt = false);

        /**
         * AMP Reset의 Active Level을 읽는다.
         * boot file 또는 실행중인 memory에서 읽을 수 있다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	bLevel		: Reset 신호 Level, true=HIGH, false=LOW
         * @param	bBootOpt		: (OPTION=false) boot file에서 읽을지 여부, true=boot file에서 읽음
         */
        int GetAmpResetLevel(int iAxis, out bool bLevel, bool bBootOpt = false);

        /**
         * 지정 축의 AMP Drive의 Resolution을 읽는다.
         * boot file 또는 실행중인 memory에서 읽을 수 있다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	iiResolution	: AMP Resolution, default=2500 pulse/rev
         * @param	bBootOpt		: (OPTION=false) boot file에서 읽을지 여부, true=boot file에서 읽음
         */
        int GetAmpResolution(int iAxis, out int iResolution, bool bBootOpt = false);

        /**
         * 지정 축의 분주비에 대한 분자값, 분모값을 읽는다.
         * boot file 또는 실행중인 memory에서 읽을 수 있다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	iRatioA		: Encoder 분주비 분자값
         * @param	iRatioB		: Encoder 분주비 분모값
         * @param	bBootOpt		: (OPTION=false) boot file에서 읽을지 여부, true=boot file에서 읽음
         */
        int GetEncoderRatio(int iAxis, out int iRatioA, out int iRatioB, bool bBootOpt = false);

        /**
         * 지정 축이 회전/직선운동하는 무한회전 축인지 읽는다.
         * boot file 또는 실행중인 memory에서 읽을 수 있다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	bStatus		: 무한회전 축 설정여부
         * @param	bType			: 운동 종류, false=직선, true=회전
         * @param	bBootOpt		: (OPTION=false) boot file에서 읽을지 여부, true=boot file에서 읽음
         */
        int GetEndlessAx(int iAxis, out bool bStatus, bool bType, bool bBootOpt = false);

        /**
         * 무한회전 축의 움직이는 영역을 읽는다.
         * boot file 또는 실행중인 memory에서 읽을 수 있다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	dRange		: 이동 영역
         * @param	bBootOpt		: (OPTION=false) boot file에서 읽을지 여부, true=boot file에서 읽음
         */
        int GetEndlessRange(int iAxis, out double dRange, bool bBootOpt = false);

        /**
         * 축의 위치결정 완료값과 위치결정 시 신호 Level을 읽는다.
         * boot file 또는 실행중인 memory에서 읽을 수 있다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	pdInPosition	: 위치 결정값
         * @param	bLevel		: 신호 Level, true=HIGH, false=LOW
         * @param	bBootOpt		: (OPTION=false) boot file에서 읽을지 여부, true=boot file에서 읽음
         */
        int GetInPosition(int iAxis, out double dInPosition, out bool bLevel, bool bBootOpt = false);

        /**
         * 지정 축의 InPosition 신호 사용여부를 읽는다.
         * boot file 또는 실행중인 memory에서 읽을 수 있다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	pbReq			: 사용 여부, true=사용
         * @param	bBootOpt		: (OPTION=false) boot file에서 읽을지 여부, true=boot file에서 읽음
         */
        int GetInpositionRequired(int iAxis, out bool bReq, bool bBootOpt = false);

        /**
         * 축의 위치오차 제한값과 Event를 읽는다.
         * boot file 또는 실행중인 memory에서 읽을 수 있다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	dLimit		: 위치오차 제한값, 최대 35000 count
         * @param	iAction		: 위치오차 Event, NO EVENT, ESTOP EVENT, ABORT EVENT
         * @param	bBootOpt		: (OPTION=false) boot file에서 읽을지 여부, true=boot file에서 읽음
         */
        int GetErrorLimit(int iAxis, out double dLimit, out int iAction, bool bBootOpt = false);

        /**
         * 원점 복귀 시 Encoder의 C상 펄스 이용 여부를 읽는다.
         * boot file 또는 실행중인 memory에서 읽을 수 있다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	bIndexReq		: C상 펄스 사용 여부, true=Home Sensor와 Encoder의 Index Pulse를 동시 검출,
         *												  false=Home Sensor만 검출
         * @param	bBootOpt		: (OPTION=false) boot file에서 읽을지 여부, true=boot file에서 읽음
         */
        int GetIndexRequired(int iAxis, out bool bIndexReq, bool bBootOpt = false);

        /**
         * I/O 8점에 대한  입,출력 모드를 읽는다.
         * boot file 또는 실행중인 memory에서 읽을 수 있다.
         *
         * @param   iBdNum			: Board ID (0 ~ 7)
         * @param   bMode			: 입, 출력 모드, true=출력, false=입력
         * @param	bBootOpt		: (OPTION=false) boot file에서 읽을지 여부, true=boot file에서 읽음
         */
        int GetIOMode(int iBdNum, out bool bMode, bool bBootOpt = false);

        /**
         * Home, +/- 방향 Limit Switch Active시 동작할 Event와 신호 Level을 읽는다.
         * boot file 또는 실행중인 memory에서 읽을 수 있다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param   iType			: Sensor 종류, 0=Home, 1=Positive, 2=Negative
         * @param   iLimit		: 동작할 Event
         * @param	bLevel		: 신호 Level, true=HIGH, false=LOW
         * @param	bBootOpt		: (OPTION=false) boot file에서 읽을지 여부, true=boot file에서 읽음
         */
        int GetSensorLimit(int iAxis, int iType, out int iLimit, out bool bLevel, bool bBootOpt = false);

        /**
         * +/- 방향으로 Motor가 이동할 수 있는 제한 위치값과 그 위치값에 도달했을 때 적용할 Event를 읽는다.
         * boot file 또는 실행중인 memory에서 읽을 수 있다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	bType			: 이동 방향, true=Positive, false=Negative
         * @param	dPosition		: 제한 위치값
         * @param   iLimit		: 적용할 Event
         * @param	bBootOpt		: (OPTION=false) boot file에서 읽을지 여부, true=boot file에서 읽음
         */
        int GetSWLimit(int iAxis, bool bType, out double dPosition, out int iLimit, bool bBootOpt = false);

        /**
         * 해당 축이 어떤 Motor로 제어하는 축으로 지정되어 있는지 읽는다.
         * boot file 또는 실행중인 memory에서 읽을 수 있다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	iType		: Motor 종류, 0=속도형Servo, 1=일반Stepper, 2=MicroStepper 혹은 위치형Servo
         * @param	bBootOpt		: (OPTION=false) boot file에서 읽을지 여부, true=boot file에서 읽음
         */
        int GetMotorType(int iAxis, out int iType, bool bBootOpt = false);

        /**
         * 해당 축의 Feedback 장치와 Loop 형태를 읽는다.
         * boot file 또는 실행중인 memory에서 읽을 수 있다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param   iDevice		: Feedback 장치, 0=Encoder입력, 1=0~10volt입력, 2=-10~10volt입력
         * @param   bLoop			: Loop 형태, false=Open Loop, true=Closed Loop
         * @param	bBootOpt		: (OPTION=false) boot file에서 읽을지 여부, true=boot file에서 읽음
         */
        int GetAxisProperty(int iAxis, out int iDevice, out bool bLoop, bool bBootOpt = false);

        /**
         * 해당 축의 Pulse 분주비와 전자기어비를 읽는다.
         * boot file 또는 실행중인 memory에서 읽을 수 있다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param   iPgratio		: Pulse 분주비
         * @param   dEgratio		: 전자기어비
         * @param	bBootOpt		: (OPTION=false) boot file에서 읽을지 여부, true=boot file에서 읽음
         */
        int GetRatioProperty(int iAxis, out int iPgratio, out double dEgratio, bool bBootOpt = false);

        /**
         * 속도형 Servo의 설정을 읽는다.
         * boot file 또는 실행중인 memory에서 읽을 수 있다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param   bControl		: 제어모드, false=속도제어, true=위치제어
         * @param   bPolar		: Analog 출력 종류, false=UNIPOLAR, true=BIPOLER
         * @param	bBootOpt		: (OPTION=false) boot file에서 읽을지 여부, true=boot file에서 읽음
         */
        int GetVServoProperty(int iAxis, out bool bControl, out bool bPolar, bool bBootOpt = false);

        /**
         * 지정 축의 Pulse 출력 형태를 읽는다.
         * boot file 또는 실행중인 memory에서 읽을 수 있다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param   bMode			: Pulse 출력 형태, false=Two Pulse(CW+CCW), true=Sign+Pulse
         * @param	bBootOpt		: (OPTION=false) boot file에서 읽을지 여부, true=boot file에서 읽음
         */
        int GetStepMode(int iAxis, out bool bMode, bool bBootOpt = false);

        /**
         * 지정 축의 Encoder 입력 방향과 좌표 방향을 읽는다.
         * boot file 또는 실행중인 memory에서 읽을 수 있다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param   bEncDir		: Encoder 입력 방향, false=ENCO_CW(시계방향, - Count)
         *												 true =ENCO_CCW(반시계방향, + Count)
         * @param   bCoorDir		: 좌표방향, false=CORD_CW(시계방향, + 좌표 이동)
         *										true =CORD_CCW(반시계방향, - 좌표 이동)
         * @param	bBootOpt		: (OPTION=false) boot file에서 읽을지 여부, true=boot file에서 읽음
         */
        int GetEncoderDirection(int iAxis, out bool bEncDir, out bool bCoorDir, bool bBootOpt = false);

        /**
         * 지정된 축의 STOP EVENT, ESTOP EVENT 수행 시 감속 시간을 읽는다.
         * boot file 또는 실행중인 memory에서 읽을 수 있다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	bType			: 정지 종류, false=STOP, true=E-STOP
         * @param   iRate		: 감속 시간
         * @param	bBootOpt		: (OPTION=false) boot file에서 읽을지 여부, true=boot file에서 읽음
         */
        int GetStopRate(int iAxis, bool bType, out int iRate, bool bBootOpt = false);

        /**
         * 동기제어시 적용되는 보상 Gain값을 읽는다.
         *
         * MMC Library : get_sync_gain()
         *
         * @param   lCoeff		: 보상 Gain 값
         */
        int GetSyncGain(out long lCoeff);

        /**
         * 해당 축의 속도 또는 위치에 대한 PID & FF Gain값들을 읽는다.
         * boot file 또는 실행중인 memory에서 읽을 수 있다.
         *
         * MMC Library : get_gain(), fget_gain(), get_v_gain(), fget_v_gain()
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	bVelType		: 위치/속도 종류 지정, false=위치, true=속도
         * @param   lGain			: Gain 값 배열, 배열인수위치는 아래와 같다.
         *								0=GA_P, 1=GA_I, 2=GA_D, 3=GA_F, 4=GA_LIMIT, 5=GAIN_MUNBER
         * @param	bBootOpt		: (OPTION=false) boot file에서 읽을지 여부, true=boot file에서 읽음
         */
        int GetGain(int iAxis, bool bVelType, out long lGain, bool bBootOpt = false);

        /**
         * 해당 축의 적분제어 시 적분제어 모드를 읽는다.
         * boot file 또는 실행중인 memory에서 읽을 수 있다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	bType			: 제어모드, false=위치, true=속도
         * @param   bMode			: 적분제어 모드, false=정지시적용, true=항상적용
         * @param	bBootOpt		: (OPTION=false) boot file에서 읽을지 여부, true=boot file에서 읽음
         */
        int GetIntegration(int iAxis, bool bType, out bool bMode, bool bBootOpt = false);

        /**
         * 속도지령 혹은 토크 지령에 대해 Low Pass Filter 혹은 Notch Filter에 대한 Filter 값을 읽는다.
         * boot file 또는 실행중인 memory에서 읽을 수 있다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	bCommandType	: 지령 종류, false=속도(Position), true=토크(Velocity)
         * @param	bFilterType		: Filter 종류, false=LowPass, true=Notch
         * @param   dFilter		: Filter 값
         * @param	bBootOpt		: (OPTION=false) boot file에 저장 여부, true=boot file에 저장
         */
        int GetFilter(int iAxis, bool bCommandType, bool bFilterType, out double dFilter,
                                bool bBootOpt = false);

        /**
         * 해당 축의 동작 중 속도를 읽는다. (명령 값과 실제 값)
         * boot file 또는 실행중인 memory에서 읽을 수 있다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	bType			: 속도 종류, false=실제속도값, true=속도명령값
         * @param   iPulse		: 속도의 Pulse값
         */
        int GetVelocity(int iAxis, bool bType, out int iPulse);

        /**
         * 지정된 Board의 축별 동작여부를 읽는다.
         *
         *		b7	b6	b5	b4	b3	b2	b1	b0
         *		축8	축7	축6	축5	축4	축3	축2	축1
         *
         *		bit = true : 동작 금지
         *		bit = false : 동작 가능
         *
         * @param   iBdNum			: Board ID (0 ~ 7)
         * @param	iState		: 축별 동작 여부, bit가 한 축 (b0=축1, b1=축2, ...), true=정지, false=동작
         */
        int GetAxisRunStop(int iBdNum, out bool bState);

        /**
         * 축의 실제위치 및 목표위치를 읽는다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	bType			: 위치 종류, false=실제위치, true=목표위치
         * @param	dPosition		: bType=false이면, 지정할 실제위치
         *							  bType=true 이면, 지정할 목표위치
         */
        int GetPosition(int iAxis, bool bType, out double dPosition);

        /**
         * Motor의 지령치 RPM이나 실제 RPM을 읽는다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	bType			: RMP 종류, false=실제RPM, true=지령치RPM
         * @param   iRpm			: RPM값
         */
        int GetRpm(int iAxis, bool bType, out int iRpm);

        /**
         * Board별 Sampling Rate를 읽는다.
         * boot file 또는 실행중인 memory에서 읽을 수 있다.
         *
         * @param   iBdNum			: Board ID (0 ~ 7)
         * @param   iTime		: Sampling Rate, msec단위 (1=4msec, 2=2msec, 3=1msec만 지원)
         * @param	bBootOpt		: (OPTION=false) boot file에서 읽을지 여부, true=boot file에서 읽음
         */
        int GetControlTimer(int iBdNum, out int iTime, bool bBootOpt = false);

        /**
         * 축의 목표위치와 실제위치의 차이값인 위치오차를 읽는다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param   dError		: 위치오차, (목표위치-실제위치)
         */
        int GetError(int iAxis, out double dError);

        /**
         * 특정 축의 Encoder Feedback Data를 빠르게 읽어들일 때 사용 (50usec 주기 Update)
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	bStatus		: 설정 여부
         */
        int GetFastReadEncoder(int iAxis, out bool bStatus);

        /**
         * 해당 축의 Analog Offset 값을 읽는다.
         * boot file 또는 실행중인 memory에서 읽을 수 있다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param   iOffset		: Analog Offer, +/-2048, +/-64767
         * @param	bBootOpt		: (OPTION=false) boot file에서 읽을지 여부, true=boot file에서 읽음
         */
        int GetAnalogOffset(int iAxis, out int iOffset, bool bBootOpt = false);

        /**
         * 입, 출력 Port의 64bit Data를 읽는다.
         *
         * @param   iPort			: 입, 출력 Port 번호 (0 ~ 3, Board 구성 개수에 따라 변동)
         * @param	bType			: 입, 출력 종류, false=입력, true=출력
         * @param   lValue		: 64bit Data
         */
        int GetIO(int iPort, bool bType, out long lValue);

        /**
         * 지정된 Analog 입/출력의 12/16bit Data 값을 읽는다.
         *
         * @param   iChannel		: Analog 입력 채널 수(0 ~ 7) 혹은 출력 축 ID(0 ~ 63)
         * @param	bType			: 입, 출력 종류, false=입력, true=출력
         * @param   iValue		: bType=false이면 Analog 입력 값, -2048 ~ +2047
         *							  bType=true이면 Analog 출력 값, +/-2048, +/-64767
         */
        int GetAnalog(int iChannel, bool bType, out int iValue);

        /**
         * Board의 충돌방지 기능의 사용여부를 읽는다.
         *
         * @param   iBdNum			: Board ID (0 ~ 7)
         * @param   bMode			: 사용여부, true=사용
         */
        int GetCollisionPreventFlag(int iBdNum, out bool bMode);

        /**
         * 동기제어 여부를 읽는다.
         *
         * @param	bState		: 지정 여부, true=지정
         */
        int GetSyncControl(out bool bState);

        /**
         * Master축과 Slave축의 실제위치를 읽는다.
         *
         * @param   dMasterPos	: Master 축 위치
         * @param   dSlavePos		: Slave 축 위치
         */
        int GetSyncPosition(out double dMasterPos, out double dSlavePos);

        /**
         * 보드별로 I/O Interrupt를 Enable/Diable하거나, I/O Interrupt 발생 시
         * STOP-EVENT나 E-STOP-EVENT를 지정축에 발생할지 여부를 지정한다.
         * boot file 또는 실행중인 memory에 저장할 수 있다.
         *
         * @param   iID			: Board (0 ~ 7) 혹은 축 ID (0 ~ 63)
         * @param   iType			: 종류, 0=Board Enable/Disable, 1=STOP EVENT지정, 2=ESTOP EVENT지정
         * @param	bState			: 설정, true =Enable지정,  STOP EVENT/ESTOP EVENT지정,
         *									false=Disable지정, STOP EVENT/ESTOP EVENT미지정
         * @param	bBootOpt		: (OPTION=false) boot file에 저장 여부, true=boot file에 저장
         */
        int IOInterrupt(int iID, int iType, bool bState, bool bBootOpt = false);

        /**
         * I/O Interrupt 발생 시 PC쪽으로 Interrupt를 발생시킬지 여부를 지정한다.
         * boot file 또는 실행중인 memory에 저장할 수 있다.
         *
         * @param   iBdNum			: Board ID (0 ~ 7)
         * @param	bState			: 발생 여부, true=발생
         * @param	bBootOpt		: (OPTION=false) boot file에 저장 여부, true=boot file에 저장
         */
        int IOInterruptPCIRQ(int iBdNum, bool bState, bool bBootOpt = false);

        /**
         * PC Interrupt 발생 시 end of interrupt 신호를 발생시킨다.
         *
         * @param   iBdNum			: Board ID (0 ~ 7)
         */
        int IOInterruptPCIRQ_EOI(int iBdNum);

        /**
         * 지정 축의 PID 제어 여부를 지정한다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	bState			: PID 제어 여부, false=PID제어 미실시, Analog 출력 0volt,
         *											 true =PID제어 실시
         */
        int SetController(int iAxis, bool bState);

        /**
         * AMP Disable/Enable 상태를 설정한다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	bState			: AMP Enable 상태, true=Enable
         */
        int SetAmpEnable(int iAxis, bool bState);

        /**
         * 축의 이동 최고속도와 가,감속 구간값의 Limit를 지정한다. (boot file에 자동 저장)
         *
         * MMC Library : set_accel_limit(), set_vel_limit()
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	dVelocity		: 이동속도, 1 ~ 2047000 count/rev
         * @param   iAccel			: 가,감속구간값, 1 ~ 200, 10msec단위
         */
        int SetVelLimit(int iAxis, double dVelocity, int iAccel);

        /**
         * AMP Drive에 Fault 발생 시 동작할 Event를 설정한다.
         * boot file 또는 실행중인 memory에 저장할 수 있다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param   iAction		: 동작할 Event, NO EVENT, STOP EVENT, ESTOP EVENT, ABORT EVENT
         * @param	bBootOpt		: (OPTION=false) boot file에 저장 여부, true=boot file에 저장
         */
        int SetAmpFaultEvent(int iAxis, int iAction, bool bBootOpt = false);

        /**
         * AMP Enable의 Active Level을 지정한다.
         * boot file 또는 실행중인 memory에 저장할 수 있다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	bLevel			: Enable Level, true=HIGH, false=LOW
         * @param	bBootOpt		: (OPTION=false) boot file에 저장 여부, true=boot file에 저장
         */
        int SetAmpEnableLevel(int iAxis, bool bLevel, bool bBootOpt = false);

        /**
         * AMP Fault의 Active Level을 지정한다.
         * boot file 또는 실행중인 memory에 저장할 수 있다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	bLevel			: Fault Level, true=HIGH, false=LOW
         * @param	bBootOpt		: (OPTION=false) boot file에 저장 여부, true=boot file에 저장
         */
        int SetAmpFaultLevel(int iAxis, bool bLevel, bool bBootOpt = false);

        /**
         * AMP Reset의 Active Level을 지정한다.
         * boot file 또는 실행중인 memory에 저장할 수 있다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	bLevel			: Reset Level, true=HIGH, false=LOW
         * @param	bBootOpt		: (OPTION=false) boot file에 저장 여부, true=boot file에 저장
         */
        int SetAmpResetLevel(int iAxis, bool bLevel, bool bBootOpt = false);

        /**
         * 지정 축의 AMP Drive의 Resolution을 설정한다.
         * boot file 또는 실행중인 memory에 저장할 수 있다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param   iResolution	: AMP Resolution, default=2500 pulse/rev
         * @param	bBootOpt		: (OPTION=false) boot file에 저장 여부, true=boot file에 저장
         */
        int SetAmpResolution(int iAxis, int iResolution, bool bBootOpt = false);

        /**
         * 지정 축의 분주비에 대한 분자값, 분모값을 설정한다.
         * boot file 또는 실행중인 memory에 저장할 수 있다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param   iRatioA		: Encoder 분주비 분자값
         * @param   iRatioB		: Encoder 분주비 분모값
         * @param	bBootOpt		: (OPTION=false) boot file에 저장 여부, true=boot file에 저장
         */
        int SetEncoderRatio(int iAxis, int iRatioA, int iRatioB, bool bBootOpt = false);

        /**
         * 지정 축을 회전/직선운동하는 무한회전 축으로 설정한다.
         * boot file 또는 실행중인 memory에 저장할 수 있다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	bStatus			: 무한회전 축 설정여부
         * @param   iResolution	: Motor 1회전당 Pulse수
         * @param	bType			: 운동 종류, false=직선, true=회전
         * @param	bBootOpt		: (OPTION=false) boot file에 저장 여부, true=boot file에 저장
         */
        int SetEndlessAx(int iAxis, bool bStatus, int iResolution, bool bType,
                                   bool bBootOpt = false);

        /**
         * 무한회전 축의 움직이는 영역을 설정한다.
         * boot file 또는 실행중인 memory에 저장할 수 있다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	dRange			: 이동 영역
         * @param	bBootOpt		: (OPTION=false) boot file에 저장 여부, true=boot file에 저장
         */
        int SetEndlessRange(int iAxis, double dRange, bool bBootOpt = false);

        /**
         * 원점 복귀 시 Encoder의 C상 펄스 이용 여부를 설정한다.
         * boot file 또는 실행중인 memory에 저장할 수 있다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	bIndexReq		: C상 펄스 사용 여부, true =Home Sensor와 Encoder의 Index Pulse를 동시 검출,
         *												  false=Home Sensor만 검출
         * @param	bBootOpt		: (OPTION=false) boot file에 저장 여부, true=boot file에 저장
         */
        int SetIndexRequired(int iAxis, bool bIndexReq, bool bBootOpt = false);

        /**
         * 해당 축을 해당 Motor 종류로 제어하는 축으로 지정한다.
         * boot file 또는 실행중인 memory에 저장할 수 있다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param   iType			: Motor 종류, 0=속도형Servo, 1=일반Stepper, 2=MicroStepper 혹은 위치형Servo
         * @param	bBootOpt		: (OPTION=false) boot file에 저장 여부, true=boot file에 저장
         */
        int SetMotorType(int iAxis, int iType, bool bBootOpt = false);

        /**
         * 해당 축의 Feedback 장치와 Loop 형태를 지정한다.
         * boot file 또는 실행중인 memory에 저장할 수 있다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param   iDevice		: Feedback 장치, 0=ENCODER, 1=0~10volt입력, 2=-10~10volt입력
         * @param	bLoop			: Loop 형태, false=Open Loop, true=Closed Loop
         * @param	bBootOpt		: (OPTION=false) boot file에 저장 여부, true=boot file에 저장
         */
        int SetAxisProperty(int iAxis, int iDevice, bool bLoop, bool bBootOpt = false);

        /**
         * 해당 축의 분주비와 전자기어비를 지정한다.
         * boot file 또는 실행중인 memory에 저장할 수 있다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param   iPgratio		: Pulse 분주비, default=8
         * @param	dEgratio		: 전자기어비, default=1.0
         * @param	bBootOpt		: (OPTION=false) boot file에 저장 여부, true=boot file에 저장
         */
        int SetRatioProperty(int iAxis, int iPgratio, double dEgratio, bool bBootOpt = false);

        /**
         * 속도형 Servo의 설정을 지정한다.
         * boot file 또는 실행중인 memory에 저장할 수 있다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	bControl		: 제어모드, false=속도제어, true=토크제어
         * @param	bPolar			: Analog 출력 종류,  true=UNIPOLAR, false=BIPOLER
         * @param	bBootOpt		: (OPTION=false) boot file에 저장 여부, true=boot file에 저장
         */
        int SetVServoProperty(int iAxis, bool bControl, bool bPolar, bool bBootOpt = false);

        /**
         * 지정 축의 Pulse 출력 형태를 지정한다.
         * boot file 또는 실행중인 memory에 저장할 수 있다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	bMode			: Pulse 출력 형태, false=Two Pulse(CW+CCW), true=Sign+Pulse
         * @param	bBootOpt		: (OPTION=false) boot file에 저장 여부, true=boot file에 저장
         */
        int SetStepMode(int iAxis, bool bMode, bool bBootOpt = false);

        /**
         * 지정 축의 Encoder 입력 방향과 좌표 방향을 지정한다.
         * boot file 또는 실행중인 memory에 저장할 수 있다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	bEncDir			: Encoder 입력 방향, false=ENCO_CW(시계방향, - count),
         *												 true =ENCO_CCW(반시계방향, + count)
         * @param	bCoorDir		: 좌표방향, false=CORD_CW(시계방향, +좌표이동),
         *										true =CORD_CCW(반시계방향, -좌표이동)
         * @param	bBootOpt		: (OPTION=false) boot file에 저장 여부, true=boot file에 저장
         */
        int SetEncoderDirection(int iAxis, bool bEncDir, bool bCoorDir, bool bBootOpt = false);

        /**
         * I/O 8점에 대한  입,출력 모드를 지정한다.
         * boot file 또는 실행중인 memory에 저장할 수 있다.
         *
         * @param   iBdNum			: Board ID (0 ~ 7)
         * @param	bMode			: 입, 출력 모드, true=출력, false=입력
         * @param	bBootOpt		: (OPTION=false) boot file에 저장 여부, true=boot file에 저장
         */
        int SetIOMode(int iBdNum, bool bMode, bool bBootOpt = false);

        /**
         * 축의 위치결정 완료값과 위치결정 시 신호 Level을 지정한다.
         * boot file 또는 실행중인 memory에 저장할 수 있다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	dInPosition		: 위치 결정값
         * @param	bLevel			: 신호 Level, true=HIGH, false=LOW
         * @param	bBootOpt		: (OPTION=false) boot file에 저장 여부, true=boot file에 저장
         */
        int SetInPosition(int iAxis, double dInPosition, bool bLevel, bool bBootOpt = false);

        /**
         * 지정 축의 InPosition 신호 사용여부를 설정한다.
         * boot file 또는 실행중인 memory에 저장할 수 있다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	bReq			: 사용 여부, true=사용
         * @param	bBootOpt		: (OPTION=false) boot file에 저장 여부, true=boot file에 저장
         */
        int SetInpositionRequired(int iAxis, bool bReq, bool bBootOpt = false);

        /**
         * 축의 위치오차 Limit값과 Event를 지정한다.
         * boot file 또는 실행중인 memory에 저장할 수 있다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	dLimit			: 위치오차 Limit값, 최대 35000 count
         * @param   iAction		: 위치오차 Event, NO EVENT, STOP EVENT, ESTOP EVENT
         * @param	bBootOpt		: (OPTION=false) boot file에 저장 여부, true=boot file에 저장
         */
        int SetErrorLimit(int iAxis, double dLimit, int iAction, bool bBootOpt = false);

        /**
         * 지정된 축의 STOP EVENT, ESTOP EVENT 수행 시 감속 시간을 설정한다.
         * boot file 또는 실행중인 memory에 저장할 수 있다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	bType			: 정지 종류, false=STOP, true=ESTOP
         * @param   iRate			: 감속 시간, default=10
         * @param	bBootOpt		: (OPTION=false) boot file에 저장 여부, true=boot file에 저장
         */
        int SetStopRate(int iAxis, bool bType, int iRate, bool bBootOpt = false);

        /**
         * Home, +/- 방향 Limit Switch Active시 동작할 Event와 신호 Level을 지정한다.
         * boot file 또는 실행중인 memory에 저장할 수 있다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param   iType			: Sensor 종류, 0=Home, 1=Positive, 2=Negative
         * @param   iLimit			: 동작할 Event
         * @param	bLevel			: 신호 Level, true=HIGH, FLASE=LOW
         * @param	bBootOpt		: (OPTION=false) boot file에 저장 여부, true=boot file에 저장
         */
        int SetSensorLimit(int iAxis, int iType, int iLimit, bool bLevel, bool bBootOpt = false);

        /**
         * Home, +/- 방향 Limit Switch Active시 동작할 Event를 지정한다.
         * boot file 또는 실행중인 memory에 저장할 수 있다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param   iType			: Sensor 종류, 0=Home, 1=Positive, 2=Negative
         * @param   iLimit			: 동작할 Event
         * @param	bBootOpt		: (OPTION=false) boot file에 저장 여부, true=boot file에 저장
         */
        int SetSensorEvent(int iAxis, int iType, int iLimit, bool bBootOpt = false);

        /**
         * Home, +/- 방향 Limit Switch Active시 동작할 신호 Level을 지정한다.
         * boot file 또는 실행중인 memory에 저장할 수 있다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param   iType			: Sensor 종류, 0=Home, 1=Positive, 2=Negative
         * @param	bLevel			: 신호 Level, true=HIGH, FLASE=LOW
         * @param	bBootOpt		: (OPTION=false) boot file에 저장 여부, true=boot file에 저장
         */
        int SetSensorLevel(int iAxis, int iType, bool bLevel, bool bBootOpt = false);

        /**
         * +/- 방향으로 Motor가 이동할 수 있는 Limit 위치값과 그 위치값에 도달했을 때 적용될 Event를 지정한다.
         * boot file 또는 실행중인 memory에 저장할 수 있다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	bType			: 방향, false=Negative, true=Positive
         * @param	dPosition		: 제한 위치값, +/-2147483647
         * @param   iLimit			: 적용될 Event
         * @param	bBootOpt		: (OPTION=false) boot file에 저장 여부, true=boot file에 저장
         */
        int SetSWLimit(int iAxis, bool bType, double dPosition, int iLimit, bool bBootOpt = false);

        /**
         * 해당 축의 속도 또는 위치에 대한 PID & FF Gain 값들을 지정한다.
         * boot file 또는 실행중인 memory에 저장할 수 있다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	bVelType		: 위치/속도 종류 지정, false=위치, true=속도
         * @param   lGain			: Gain 값 배열, 배열인수 위치는 아래와 같다.
         *								0=GA_P, 1=GA_I, 2=GA_D, 3=GA_F, 4=GA_ILIMIT, 5=GAIN_NUMBER
         * @param	bBootOpt		: (OPTION=false) boot file에 저장 여부, true=boot file에 저장
         */
        int SetGain(int iAxis, bool bVelType, out long lGain, bool bBootOpt = false);

        /**
         * 해당 축의 적분 제어 시 적분 제어 모드를 지정한다.
         * boot file 또는 실행중인 memory에 저장할 수 있다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	bType			: 제어모드, false=위치, true=속도
         * @param	bMode			: 적분제어 모드, false=항상적용, true=정지시적용
         * @param	bBootOpt		: (OPTION=false) boot file에 저장 여부, true=boot file에 저장
         */
        int SetIntegration(int iAxis, bool bType, bool bMode, bool bBootOpt = false);

        /**
         * 속도지령 혹은 토크 지령에 대해 Low Pass Filter 혹은 Notch Filter에 대한 Filter 값을 설정한다.
         * boot file 또는 실행중인 memory에 저장할 수 있다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	bCommandType	: 지령 종류, false=속도(Position), true=토크(Velocity)
         * @param	bFilterType		: Filter 종류, false=LowPass, true=Notch
         * @param	dFilter			: Filter 값
         * @param	bBootOpt		: (OPTION=false) boot file에 저장 여부, true=boot file에 저장
         */
        int SetFilter(int iAxis, bool bCommandType, bool bFilterType, double dFilter,
                                bool bBootOpt = false);

        /**
         * 지정된 Board의 축별 동작 여부를 설정한다.
         *
         *		b7	b6	b5	b4	b3	b2	b1	b0
         *		축8	축7	축6	축5	축4	축3	축2	축1
         *
         *		bit = true : 동작 금지
         *		bit = false : 동작 가능
         *
         * @param   iBdNum			: Board ID (0 ~ 7)
         * @param   iState			: 축별 동작 여부, bit가 한 축 (b0=축1, b1=축2, ...), true=정지, false=동작
         */
        int SetAxisRunStop(int iBdNum, int iState);

        /**
         * 지정 I/O bit를 HIGH(1)/LOW(0) 상태로 만든다.
         *
         *		 Board수	  I/O Bit 범위
         *			1			0  ~ 63
         *			2			64 ~ 63
         *			3			64 ~ 95
         *			4			96 ~ 127
         *
         * @param   iBitNo			: 지정할 I/O Bit 번호 (장착된 Board의 수량에 따라 달라짐)
         * @param	bValue			: 지정할 값, (true, false)
         */
        int SetBit(int iBitNo, bool bValue);

        /**
         * 64bit의 I/O Data를 출력 Port를 통해 내보낸다.
         *
         * @param   iPort			: 출력 Port 번호 (0 ~ 3, Board 구성 개수에 따라 변동)
         * @param	lValue			: 출력 값
         */
        int SetIO(int iPort, long lValue);

        /**
         * 12/16 bit Analog 출력전압을 내보낸다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param   iValue			: 출력할 전압 값, +/-2048, +/-64767
         */
        int SetDacOut(int iAxis, int iValue);

        /**
         * 해당 축의 Analog Offset값을 설정한다.
         * boot file 또는 실행중인 memory에 저장할 수 있다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param   iOffset		: Analog OFfset, +/-2048, +/-64767
         * @param	bBootOpt		: (OPTION=false) boot file에 저장 여부, true=boot file에 저장
         */
        int SetAnalogOffset(int iAxis, int iOffset, bool bBootOpt = false);

        /**
         * 지정 축의 출력전압의 범위를 설정한다.
         * boot file 또는 실행중인 memory에 저장할 수 있다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param   iLimit			: A출력 전압 범위, 0 ~ 64767
         * @param	bBootOpt		: (OPTION=false) boot file에 저장 여부, true=boot file에 저장
         */
        int SetAnalogLimit(int iAxis, int iLimit, bool bBootOpt = false);

        /**
         * 축의 실제 위치 및 목표 위치를 지정한다. (AMP Disable 상태에서 수행하는게 좋다.)
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	bType			: 위치 종류, false=실제위치, true=목표위치
         * @param	dPosition		: bType=false이면 지정할 실제위치, bType=true이면 지정할 목표위치
         */
        int SetPosition(int iAxis, bool bType, double dPosition);

        /**
         * 특정 축의 Encoder Feedback Data를 빠르게 읽어들일 때 사용 (50usec 주기 Update)
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	bStatus			: 설정 여부, true=설정
         */
        int SetFastReadEncoder(int iAxis, bool bStatus);

        /**
         * 사용자가 Motion 관련 S/W를 자체 개발하여 시스템을 동작시킬 수 있도록 지원해주는 기능
         *
         * @param   iLen			: 제어대상 축 수, Board의 제어 축수와 일치시킨다. 8축 Board => 8, 4축 Board => 4
         * @param	iAxes		: 제어대상 축 ID를 배열구조로 설정
         * @param   lDelt			: 매 Sampling Time(10msec)당 위치증가분 Data
         * @param   iFlag			: 속도 Profile의 시작과 끝을 알려주는데 사용, 1=동작시작, 2=동작중, 3=동작완료
         */
        int SetInterpolation(int iLen, out int iAxes, out long lDelt, int iFlag);

        /**
         * 충돌방지 기능을 사용할 Mastr/Slave축 및 충돌방지 거리 및 조건 (+, -, >, <)을 설정한다.
         *
         * @param   iMasterAx		: Master 축 ID (0 ~ 63)
         * @param   iSlaveAx		: Slave 축 ID (0 ~ 63)
         * @param	bAddSub			: 오차 계산, false=(Master현재위치-Slave현재위치),
         *										 true=(Master현재위치+Slave현재위치)
         * @param	bNonEqual		: 비교, false=(dPosition < bAddSub결과치),
         *									true=(dPosition > bAddSub결과치)
         * @param	dPosition		: 충돌 방지 거리
         */
        int SetCollisionPrevent(int iMasterAx, int iSlaveAx,
                                          bool bAddSub, bool bNonEqual, double dPosition);

        /**
         * 충돌방지 기능의 사용여부를 설정한다.
         *
         * @param   iBdNum			: Board ID (0 ~ 7)
         * @param	bMode			: 사용 여부, true=사용
         */
        int SetCollisionPreventFlag(int iBdNum, bool bMode);

        /**
         * Board DPRAM Address를 설정한다.
         *
         * @param   iBdNum			: Board ID (0 ~ 7)
         * @param	lAddr			: DPRAM Address
         */
        int SetDpramAddress(int iBdNum, long lAddr);

        /**
         * 절대치 Motor의 Type을 지정한다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param   iType			: Motor 종류, 1=삼성CSDJ, CSDJ+SERVO DRIVE, 2=YASKAWA SERVO DRIVE
         */
        int SetAbsEncoderType(int iAxis, int iType);

        /**
         * 절대치 Motor를 설정한다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         */
        int SetAbsEncoder(int iAxis);

        /**
         * Servo Linear Flag 상태를 설정한다.
         * boot file 또는 실행중인 memory에 저장할 수 있다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param   iFlag			: Servo Linear Flag 상태
         * @param	bBootOpt		: (OPTION=false) boot file에 저장 여부, true=boot file에 저장
         */
        int SetServoLinearFlag(int iAxis, int iFlag, bool bBootOpt = false);

        /**
         * 동기제어 여부를 지정한다.
         *
         * @param	bState			: 지정 여부, true=동기제어 실행
         */
        int SetSyncControl(bool bState);

        /**
         * 동기제어할 Master축과 Slave축을 지정한다.
         *
         * @param   iMasterAx		: Master 축 ID (0 ~ 63)
         * @param   iSlaveAx		: Slave 축 ID (0 ~ 63)
         */
        int SetSyncMapAxes(int iMasterAx, int iSlaveAx);

        /**
         * 동기제어시 적용되는 보상 Gain값을 지정한다.
         * boot file 또는 실행중인 memory에 저장할 수 있다.
         *
         * @param   iCoeff			: 보상 Gain 값
         * @param	bBootOpt		: (OPTION=false) boot file에 저장 여부, true=boot file에 저장
         */
        int SetSyncGain(int iCoeff, bool bBootOpt = false);

        /**
         * Board별 Sampling Rate를 설정한다.
         * boot file 또는 실행중인 memory에 저장할 수 있다.
         *
         * @param   iBdNum			: Board ID (0 ~ 7)
         * @param   iTime			: Sampling Time(msec 단위) (1=4msec, 2=2msec, 3=1msec만 지원)
         * @param	bBootOpt		: (OPTION=false) boot file에 저장 여부, true=boot file에 저장
         */
        int SetControlTimer(int iBdNum, int iTime, bool bBootOpt = false);

        /**
         * PositionIoOnOff()로 설정된 것을 해제한다.
         *
         * @param   iPosNum		: (OPTION=0) 위치 번호, 1 ~ 10, 0=모든 위치 해제
         */
        int PositionIOClear(int iAxis, int iPosNum = 0);

        /**
         * 지정 축이 지정된 위치를 지날 때 지정 IO를 출력한다.
         *
         * @param   iPosNum		: 위치 번호, 1 ~ 10
         * @param   iIONum			: I/O 번호, 양의정수=ON, 음의정수=OFF
         * @param	iAxis			: 축 ID
         * @param	dPosition		: 지정 축의 위치값
         * @param	bEncFlag		: Encoder Flag, false=내부위치 Counter 사용, true=외부 Encoder 사용
         */
        int PositionIoOnoff(int iPosNum, int iIONum, int iAxis, double dPosition, int nEncFlag);

        /**
         * 직선 동작 혹은 직선, 원, 원호등의 동작 시 속도와 가,감속도를 지정한다.
         *
         * @param	dVelocity		: 속도
         * @param   iAccel			: 가,감속도
         */
        int SetMoveSpeed(double dVelocity, int iAccel);

        /**
         * 자동 가,감속 기능의 사용여부를 지정한다.
         *
         * @param   iBdNum			: Board ID (0 ~ 7)
         * @param	bState			: 사용여부, false=사용
         */
        int SetSplAutoOff(int iBdNum, bool bState);

        /**
         * 자동 가,감속 기능의 사용여부를 읽는다.
         * Library에 제공되는 함수가 없는 관계로 설정 Data에서 읽어온다.
         *
         * @param   iBdNum			: Board ID (0 ~ 7)
         * @param	bState		: 사용여부, TURE=사용
         */
        int GetSplAutoOff(int iBdNum, out bool bState);

        /**
         * 축 이동을 정지한다.
         * 일반정지, 비상정지, 속도이동정지를 제공한다.
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param   iType			: 정지 종류, 0=STOP, 1=ESTOP, 2=VSTOP
         */
        int SetStop(int iAxis, int iType);

        /**
         * 1축 속도 Profile 이동 (축 1개 단위만 이동 가능함)
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	dPosition		: 이동할 위치, 혹은 상대거리
         * @param	dVelocity		: 이동 속도
         * @param   iAccel			: 이동 가속도
         * @param   iDecel			: 이동 감속도 (비대칭(t) Type만 적용)
         * @param   iType			: 이동 Type, 0=사다리꼴 속도 Profile, 절대좌표 이동
         *										 1=S-Curve 속도 Profile, 절대좌표 이동
         *										 2=사다리꼴 속도 Profile, 상대거리 이동
         *										 3=S-Curve 속도 Profile, 상대거리 이동
         *										 4=비대칭 사다리꼴 속도 Profile, 절대좌표 이동
         *										 5=비대칭 S-Curve 속도 Profile, 절대좌표 이동
         *										 6=비대칭 사다리꼴 속도 Profile, 상대거리 이동
         *										 7=비대칭 S-Curve 속도 Profile, 상대거리 이동
         * @param	bWaitOpt		: (OPTION=false) 이동 완료 대기 여부, true=이동완료될때까지대기
         */
        int Move(int iAxis, double dPosition, double dVelocity, int iAccel,
                           int iDecel, int iType, bool bWaitOpt = false);

        /**
         * 다축 속도 Profile 동시 이동 (상대거리 이동은 지원하지 않는다.)
         *
         *		|----------------> siLen = n <----------------|
         *		+---------+---------+---------+-----+---------+
         *		| 축 ID#1 | 축 ID#2 | 축 ID#3 | ... | 축 ID#n |
         *		+---------+---------+---------+-----+---------+
         *		| 위치 #1 | 위치 #2 | 위치 #3 | ... | 위치 #n |
         *		+---------+---------+---------+-----+---------+
         *		| 속도 #1 | 속도 #2 | 속도 #3 | ... | 속도 #n |
         *		+---------+---------+---------+-----+---------+
         *		| 가속 #1 | 가속 #2 | 가속 #3 | ... | 가속 #n |
         *		+---------+---------+---------+-----+---------+
         *		| 감속 #1 | 감속 #2 | 감속 #3 | ... | 감속 #n |
         *		+---------+---------+---------+-----+---------+
         *
         * @param   iLen			: 축 수, >0
         * @param	iAxis		: 축 ID (축 수 만큼 존재)
         * @param	dPosition		: 이동할 위치, 혹은 상대거리 (축 수 만큼 존재)
         * @param	dVelocity		: 이동 속도 (축 수 만큼 존재)
         * @param	iAccel		: 이동 가속도 (축 수 만큼 존재)
         * @param   iDecel		: 이동 감속도 (축 수 만큼 존재)
         * @param   iType			: 이동 Type, 0=사다리꼴 속도 Profile, 절대좌표 이동
         *										 1=S-Curve 속도 Profile, 절대좌표 이동
         *										 4=비대칭 사다리꼴 속도 Profile, 절대좌표 이동
         *										 5=비대칭 S-Curve 속도 Profile, 절대좌표 이동
         * @param	bWaitOpt		: (OPTION=false) 이동 완료 대기 여부, true=이동완료될때까지대기
         */
        int MoveAll(int iLen, out int iAxes, out double dPosition, out double dVelocity,
                              out int iAccel, out int iDecel, int iType, bool bWaitOpt = false);

        /**
         * 지정된 n축이 주어진 좌표값만큼 직선이동을 한다. (다른 Board의 축 사용 불가)
         *  MapAxes(), SetMoveSpped()에서 지정된 축들이 이동한다.
         *
         *		|----------------> siLen = n <----------------|
         *		+---------+---------+---------+-----+---------+
         *		| 위치 #1 | 위치 #2 | 위치 #3 | ... | 위치 #n |
         *		+---------+---------+---------+-----+---------+
         *
         * @param   iLen			: 축 수, 2 <= siLen <= 8
         * @param	dPosition		: 이동할 좌표값 (축 수 만큼 존재)
         * @param   iType			: 이동 Type, 0=사다리꼴 속도 Profile, 절대좌표 이동
         *										 1=S-Curve 속도 Profile, 절대좌표 이동
         */
        int MoveN(int iLen, out double dPosition, int iType);

        /**
         * 지정된 n축이 주어진 좌표값만큼 직선이동을 한다. (다른 Board의 축 사용 가능)
         *
         *		|-------------------> siLen <-----------------|
         *		+---------+---------+---------+-----+---------+
         *		| 축 ID#1 | 축 ID#2 | 축 ID#3 | ... | 축 ID#n |
         *		+---------+---------+---------+-----+---------+
         *		| 위치 #1 | 위치 #2 | 위치 #3 | ... | 위치 #n |
         *		+---------+---------+---------+-----+---------+
         *		+------+--------+
         *		| 속도 | 가속도 |
         *		+------+--------+
         *
         * @param   iLen			: 축 수, >=2
         * @param	iAxes		: 축 ID 배열
         * @param	dPosition		: 이동할 좌표값 (축 수 만큼 존재)
         * @param   iType			: 이동 Type, 0=사다리꼴 속도 Profile, 절대좌표 이동
         *										 1=S-Curve 속도 Profile, 절대좌표 이동
         * @param	dVelocity		: 이동 속도
         * @param   iAccel			: 이동 가속도
         */
        int MoveNAx(int iLen, out int iAxes, out double dPosition, int iType,
                              double dVelocity, int iAccel);

        /**
         * 지정된 그룹의 축들이 주어진 좌표값만큼 직선이동을 한다.
         *
         *		|----------------> siLen = n <----------------|
         *		+---------+---------+---------+-----+---------+
         *		| 축 ID#1 | 축 ID#2 | 축 ID#3 | ... | 축 ID#n |
         *		+---------+---------+---------+-----+---------+
         *		| 위치 #1 | 위치 #2 | 위치 #3 | ... | 위치 #n |
         *		+---------+---------+---------+-----+---------+
         *		+------+--------+
         *		| 속도 | 가속도 |
         *		+------+--------+
         *
         * @param   iGrpNum		: Group 번호, 1 ~ 4
         * @param   iLen			: 축 수, 2 <= siLen <= 4
         * @param	iAxes		: 축 ID 배열 (축 수 만큼 존재)
         * @param	dPosition		: 이동할 좌표값 (축 수 만큼 존재)
         * @param   iType			: 이동 Type, 0=사다리꼴 속도 Profile, 절대좌표 이동
         *										 1=S-Curve 속도 Profile, 절대좌표 이동
         * @param	dVelocity		: 이동 속도
         * @param   iAccel			: 이동 가속도
         */
        int MoveNAxGr(int iGrpNum, int iLen, out int iAxes, out double dPosition,
                                int iType, double dVelocity, int iAccel);

        /**
         * 가속 후 등속 이동한다. (축 1개 단위로만 동작 가능하다.)
         *
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	dVelocity		: 이동 속도
         * @param   iAccel			: 이동 가속도
         */
        int VMove(int iAxis, double dVelocity, int iAccel);

        /**
         * 현재 위치에서 주어진 2/3차원 평면상의 좌표점까지 가,감속하면서 원호 CP Motion으로 이동한다.
         *  (다른 Board의 축 사용 불가)
         *  MapAxes(), SetMoveSpped()에서 지정된 축들이 이동한다.
         *
         *		|---------------> siAxNum = n <---------------|
         *		+---------+---------+---------+-----+---------+
         *		| 위치 #1 | 위치 #2 | 위치 #3 | ... | 위치 #n |
         *		+---------+---------+---------+-----+---------+
         *		+-----------+-----------+------+--------+----------+
         *		| 회전중심X | 회전중심Y | 속도 | 가속도 | 회전방향 |
         *		+-----------+-----------+------+--------+----------+
         *
         * @param   iAxNum			: 축 수, 2=2축, 3=3축
         * @param	dCenterX		: 회전 중심 X좌표
         * @param	dCenterY		: 회전 중심 Y좌표
         * @param   dPoint		: 이동할 좌표, 2축이면 2차원배열, 3축이면 3차원배열
         * @param	dVelocity		: 이동 속도 (0.0=SetMoveSpeed()에서 지정한 속도 사용)
         * @param   iAccel			: 이동 가속도 (0=SetMoveSpeed()에서 지정한 가,감속도 사용)
         * @param	bDir			: 회전방향, false=CIR_CW(시계방향), true=CIR_CCW(반시계방향)
         */
        int SplArcMove(int iAxNum, double dCenterX, double dCenterY,
                                 out double dPoint, double dVelocity, int iAccel, bool bDir);

        /**
         * 현재 위치에서 주어진 좌표점까지 가,감속하면서 원호 CP Motion으로 이동한다.
         *  (다른 Board의 축 사용 가능)
         *
         *		|----------------> siLen = n <----------------|
         *		+---------+---------+---------+-----+---------+
         *		| 축 ID#1 | 축 ID#2 | 축 ID#3 | ... | 축 ID#n |
         *		+---------+---------+---------+-----+---------+
         *		| 위치 #1 | 위치 #2 | 위치 #3 | ... | 위치 #n |
         *		+---------+---------+---------+-----+---------+
         *		+-----------+-----------+------+--------+----------+
         *		| 회전중심X | 회전중심Y | 속도 | 가속도 | 회전방향 |
         *		+-----------+-----------+------+--------+----------+
         *
         * @param   iLen			: 축 수 (>= 2)
         * @param	iAxes		: 축 ID 배열 (축 수만큼 존재)
         * @param   iCenterX		: 회전 중심 X좌표
         * @param   iCenterY		: 회전 중심 Y좌표
         * @param   dPoint		: 이동할 좌표 (축 수 만큼 존재)
         * @param	dVelocity		: 이동 속도 (0.0=SetMoveSpeed()에서 지정한 속도 사용)
         * @param   iAccel			: 이동 가속도 (0=SetMoveSpeed()에서 지정한 가,감속도 사용)
         * @param	bDir			: 회전방향, false=CIR_CW(시계방향), true=CIR_CCW(반시계방향)
         */
        int SplArcMoveNax(int iLen, out int iAxes, double dCenterX, double dCenterY,
                                    out double dPoint, double dVelocity, int iAccel, bool bDir);

        /**
         * 현재 위치에서 주어진 2/3차원 평면상의 좌표점까지 가,감속하면서 직선 CP Motion으로 이동한다.
         *  (다른 Board의 축 사용 불가)
         *  MapAxes(), SetMoveSpped()에서 지정된 축들이 이동한다.
         *
         *		|---------------> siAxNum = n <---------------|
         *		+---------+---------+---------+-----+---------+
         *		| 위치 #1 | 위치 #2 | 위치 #3 | ... | 위치 #n |
         *		+---------+---------+---------+-----+---------+
         *		+------+--------+
         *		| 속도 | 가속도 |
         *		+------+--------+
         *
         * @param   iAxNum			: 축 수, 2=2축, 3=3축
         * @param   dPoint		: 이동할 좌표, 2축이면 2차원배열, 3축이면 3차원배열
         * @param	dVelocity		: 이동 속도 (0.0=SetMoveSpeed()에서 지정한 속도 사용)
         * @param   iAccel			: 이동 가속도 (0=SetMoveSpeed()에서 지정한 가,감속도 사용)
         */
        int SplLineMoveN(int iAxNum, out double dPoint, double dVelocity, int iAccel);

        /**
         * 현재 위치에서 주어진 좌표점까지 가,감속하면서 직선 CP Motion으로 이동한다.
         *  (다른 Board의 축 사용 가능)
         *
         *		|----------------> siLen = n <----------------|
         *		+---------+---------+---------+-----+---------+
         *		| 축 ID#1 | 축 ID#2 | 축 ID#3 | ... | 축 ID#n |
         *		+---------+---------+---------+-----+---------+
         *		| 위치 #1 | 위치 #2 | 위치 #3 | ... | 위치 #n |
         *		+---------+---------+---------+-----+---------+
         *		+------+--------+
         *		| 속도 | 가속도 |
         *		+------+--------+
         *
         * @param   iLen			: 축 수
         * @param	iAxes		: 축 ID 배열
         * @param   dPoint		: 이동할 좌표 (축 수 만큼 존재)
         * @param	dVelocity		: 이동 속도, 0.0=기지정된 속도로 이동
         * @param   iAccel			: 이동 가속도, 0=기지정된 가속도로 이동
         */
        int SplLineMoveNax(int iLen, out int iAxes, out double dPoint, double dVelocity, int iAccel);

        /**
         * 원, 원호 이동 시 원주속도를 지정한다.
         *
         * @param	dDegree			: 원주속도, 0 < dDegree < 1000.0
         */
        int SetArcDivision(double dDegree);

        /**
         * 주어진 중심에서 지정된 각도만큼 원호를 그리며 동작을 수행한다.
         *  (다른 Board의 축 사용 불가)
         *  MapAxes(), SetMoveSpped()에서 지정된 축들이 이동한다.
         *
         * @param   iCenterX		: 회전 중심 X좌표
         * @param   iCenterY		: 회전 중심 Y좌표
         * @param	dAngle			: 회전 각도
         */
        int Arc2(double dXCenter, double dYCenter, double dAngle);

        /**
         * 주어진 중심에서 지정된 각도만큼 원호를 그리며 동작을 수행한다.
         *  (다른 Board의 축 사용 불가)
         *
         * @param   iAxis1			: 축1 ID (0 ~ 63)
         * @param   iAxis2			: 축2 ID (0 ~ 63)
         * @param   iCenterX		: 회전 중심 X좌표
         * @param   iCenterY		: 회전 중심 Y좌표
         * @param	dAngle			: 회전 각도
         * @param	dVelocity		: 이동 속도
         * @param   iAccel			: 이동 가속도
         */
        int Arc2Ax(int iAxis1, int iAxis2, double dXCenter, double dYCenter,
                             double dAngle, double dVelocity, int iAccel);

        /**
         * 주어진 사각형의 가로와 세로의 길이를 이용하여 현재위치에서 상대이동을 하면서 CP Motion으로 사각형을 그린다.
         *
         *			+--------------------+ pdPoint (X, Y)
         *			|					 |
         *			|					 |
         *			|					 |
         *			|					 |
         *			|					 |
         *			+--------------------+
         *		현재위치
         *
         * @param   iAxis1			: 축1 ID (0 ~ 63)
         * @param   iAxis2			: 축2 ID (0 ~ 63)
         * @param   dPoint		: 현재위치와 대각선방향의 X, Y 좌표
         * @param	dVelocity		: 이동 속도
         * @param   iAccel			: 이동 가속도
         */
        int RectMove(int iAxis1, int iAxis2, out double dPoint,
                               double dVelocity, int iAccel);

        /**
         * 현재위치에서 주어진 위치를 경유하면서 CP Motion으로 이동한다.
         *
         *				  |----------------> siLen = n <----------------|
         *		+---------+---------+---------+---------+-----+---------+
         *		| 축 ID#1 | 위치 #1 | 위치 #2 | 위치 #3 | ... | 위치 #n |
         *		+---------+---------+---------+---------+-----+---------+
         *		| 축 ID#2 | 위치 #1 | 위치 #2 | 위치 #3 | ... | 위치 #n |
         *		+---------+---------+---------+---------+-----+---------+
         *		| 축 ID#3 | 위치 #1 | 위치 #2 | 위치 #3 | ... | 위치 #n |
         *		+---------+---------+---------+---------+-----+---------+
         *		+------+--------+
         *		| 속도 | 가속도 |
         *		+------+--------+
         *
         * @param   iLen			: 위치 Data 개수, 최대 30
         * @param   iAxis1			: 축1 ID (0 ~ 63)
         * @param   iAxis2			: 축2 ID (0 ~ 63)
         * @param   iAxis3			: 축3 ID (0 ~ 63)
         * @param   dPointX		: X좌표 배열 (위치 Data 개수만큼 존재)
         * @param   dPointY		: Y좌표 배열 (위치 Data 개수만큼 존재)
         * @param   dPointZ		: Z좌표 배열 (위치 Data 개수만큼 존재)
         * @param	dVelocity		: 이동 속도
         * @param   iAccel			: 이동 가속도
         */
        int SplMove(int iLen, int iAxis1, int iAxis2, int iAxis3,
                              out double dPointX, out double dPointY, out double dPointZ,
                              double dVelocity, int iAccel);

        /**
         * SplMoveX()에 필요한 위치경로를 설정한다.
         *
         *				  |----------------> siLen = n <----------------|
         *		+---------+---------+---------+---------+-----+---------+
         *		| 축 ID#1 | 위치 #1 | 위치 #2 | 위치 #3 | ... | 위치 #n |
         *		+---------+---------+---------+---------+-----+---------+
         *		| 축 ID#2 | 위치 #1 | 위치 #2 | 위치 #3 | ... | 위치 #n |
         *		+---------+---------+---------+---------+-----+---------+
         *		| 축 ID#3 | 위치 #1 | 위치 #2 | 위치 #3 | ... | 위치 #n |
         *		+---------+---------+---------+---------+-----+---------+
         *		+------+--------+
         *		| 속도 | 가속도 |
         *		+------+--------+
         *
         * @param   iSplNum		: Spline Motion 번호, 1 ~ 20
         * @param   iLen			: 이동 경로 수, 1 ~ 500
         * @param   iAxis1			: 축1 ID (0 ~ 63)
         * @param   iAxis2			: 축2 ID (0 ~ 63)
         * @param   iAxis3			: 축3 ID (0 ~ 63)
         * @param   dPoint1		: 1좌표 배열
         * @param   dPoint2		: 2좌표 배열
         * @param   dPoint3		: 3좌표 배열
         * @param	dVelocity		: 이동 속도
         * @param   iAccel			: 이동 가속도
         */
        int SplMoveData(int iSplNum, int iLen, int iAxis1, int iAxis2,
                                  int iAxis3, out double dPoint1, out double dPoint2,
                                  out double dPoint3, double dVelocity, int iAccel);

        /**
         * 지정 3축이 Spline Motion으로 SplMoveData()에서 지정한 위치를 경유하면서 연속 이동한다.
         *
         * @param   iSplNum		: Spline Motion 번호, 1 ~ 20
         * @param   iAxis1			: 축1 ID (0 ~ 63)
         * @param   iAxis2			: 축2 ID (0 ~ 63)
         * @param   iAxis3			: 축3 ID (0 ~ 63)
         */
        int SplMovex(int iSplNum, int iAxis1, int iAxis2, int iAxis3);

        /**
         * 각 Board별 ROM Version을 읽는다.
         *
         * @param   iBdNum			: Board ID (0 ~ 7)
         * @param   iVersion		: ROM Version, 101 => 1.01
         */
        int VersionCheck(int iBdNum, out int iVersion);

        /**
         * 해당 Error Code의 Error Message를 반환한다.
         *
         * @param   iCode			: Error Code
         * @param	msg			: Error Message, ERR_MAX_ERROR_LEN(80)보다 크거나 같아야 한다.
         */
        int ErrorMessage(int iCode, out string msg);

        /**
         * Position Compare Board를 초기화한다. (축 2를 사용할 때는 같은 Board의 축이어야 한다.)
         *
         * @param   iIndexSel		: Position Compare할 축, 1 ~ 2
         * @param   iAxis1			: 축1 ID (0 ~ 63), 동일 Board의 축이어야 한다.
         * @param   iAxis2			: 축2 ID (0 ~ 63), 동일 Board의 축이어야 한다. (siIndexSel=2일 때만 적용)
         */
        int PositionCompareInit(int iIndexSel, int iAxis1, int iAxis2);

        /**
         * Position Compare를 설정한다. (Standard Type)
         *
         * @param   iIndexSel		: Position Compare시 사용할 축 수, 1 ~ 2
         * @param   iIndexNum		: Position Compare를 실시할 Index 번호, 1 ~ 8
         * @param   iBitNo			: 출력할 I/O Bit 번호, 0 ~ 63
         * @param   iAxis1			: 축1 ID (0 ~ 63), 동일 Board의 축이어야 한다.
         * @param   iAxis2			: 축2 ID (0 ~ 63), 동일 Board의 축이어야 한다. (siIndexSel=2일 때만 적용)
         * @param	bLatch			: I/O 출력모드, false=Transparent Mode, true=Latch Mode
         * @param   iFunction		: Position Compare에 사용할 부등호, 1="=", 2=">", 3="<"
         * @param   iOutMode		: 지정 I/O의 출력모드, 0=축별 ON/OFF, 1=두축 AND, 2=두축 OR
         * @param	dPosition		: Position Compare에 사용될 위치 Data (> 0.0)
         * @param	lTime			: I/O 출력 시간, Transparent Mode일때만 적용, 40usec단위,  최대 5.38sec
         */
        int PositionCompare(int iIndexSel, int iIndexNum, int iBitNo, int iAxis1,
                                      int iAxis2, int iLatch, int iFunction, int iOutMode,
                                      double dPosition, long lTime);

        /**
         * Position Compare를 설정한다. (Interval Type)
         *
         * @param	bDir			: Position Compare시 +방향으로 이동시 동작시킬 것인지 -방향으로 동작시킬 것인 설정
         *							   false="+", true="-"
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param   iBitNo			: 출력할 I/O Bit 번호 (0 ~ 63)
         * @param	dStartPos		: I/O가 동작될 최초의 Position 값
         * @param	dLimitPos		: I/O가 동작될 마지막 Position 값
         * @param	lInterval		: I/O가 반복될 간격을 펄스수 단위로 지정
         * @param	lTime			: I/O 출력이 지속될 시간, 40sec 단위
         */
        int PositionCompareInterval(bool bDir, int iAxis, int iBitNo, double dStartPos, double dLimitPos,
                                      long lInterval, long lTime);

        /**
         * Position Compare 동작을 할 것인지 여부를 설정한다.
         *
         * @param   iBdNum			: Board ID (0 ~ 7)
         * @param	bFlag			: false=Position Compare 동작 Disable, true=Enable
         */
        int PositionCompareEnable(int iBdNum, bool bFlag);

        /**
         * Position Compare의 Index를 초기화한다.
         *
         * @param   iBdNum			: Board ID (0 ~ 7)
         * @param   iIndexSel		: 항상 "1"로 설정
         */
        int PositionCompareClear(int iBdNum, int iIndexSel);

        /**
         * Position Compare 설정된 축의 Encoder 값을 읽어낸다.
         *
         * @param   iIndexSel		: 항상 "1"로 설정
         * @param	iAxis			: 축 ID (0 ~ 63)
         * @param	dPosition		: Encoder 값
         */
        int PositionCompareRead(int iIndexSel, int iAxis, out double dPosition);

        int PositionCompareReset(int iBdNum);
    }
}
