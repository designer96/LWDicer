using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

using static LWDicer.Control.DEF_Motion;

namespace LWDicer.Control
{
    class DEF_Motion
    {
        // 
        public const int DEF_MAX_AXIS_PER_SAXIS = 4;

        // Board에 따른 축 수
        public const int DEF_AXIS_NO_PER_BRD = 8;          // Motion Board가 Full-Size이면 8개

        // Common Value
        //const	int	DEF_SUCCESS					= 0;			// Function Execute Success

        // Board Value
        public const int DEF_NON_BOARD_TYPE = 0;           // Motion Board No-Use
        public const int DEF_MMC_BOARD_TYPE = 1;           // MMC Motion Board Use
        public const int DEF_MEI_BOARD_TYPE = 2;           // MEI Motion Board Use

        public const int DEF_0AXIS_BOARD = 0;          // 0축을 갖는 Motion Board
        public const int DEF_4AXIS_BOARD = 4;          // 4축을 갖는 Motion Board
        public const int DEF_8AXIS_BOARD = 8;          // 8축을 갖는 Motion Board

        public const int DEF_MAX_MOTION_BD = 8;            // Motion Board Max. Number
        public const int DEF_NON_MOTION_BD = 0;            // Motion Board No-Use Number
        public const int DEF_NON_MOTION_BD_ID = -1;            // Motion Board Non-Config

        public const int DEF_ALL_MOTION_BD_ID = -1;            // Motion Board ID ALL
        public const int DEF_MOTION_BD_ID1 = 0;            // Motion Board ID #1
        public const int DEF_MOTION_BD_ID2 = 1;            // Motion Board ID #2
        public const int DEF_MOTION_BD_ID3 = 2;            // Motion Board ID #3
        public const int DEF_MOTION_BD_ID4 = 3;            // Motion Board ID #4
        public const int DEF_MOTION_BD_ID5 = 4;            // Motion Board ID #5
        public const int DEF_MOTION_BD_ID6 = 5;            // Motion Board ID #6
        public const int DEF_MOTION_BD_ID7 = 6;            // Motion Board ID #7
        public const int DEF_MOTION_BD_ID8 = 7;            // Motion Board ID #8

        public const bool DEF_AUTO_CP_NON = false;     // 자동 가,감속 미사용
        public const bool DEF_AUTO_CP_USE = true;          // 자동 가,감속 사  용

        // 축 수
        public const int DEF_MAX_AXIS_NO = DEF_AXIS_NO_PER_BRD * DEF_MAX_MOTION_BD;
        public const int DEF_MIN_AXIS_NO = 0;

        public const int MAX_LENGTH_AXIS_NAME = 32;

        // Coordinate Value
        public const int DEF_MAX_COORDINATE = DEF_MAX_AXIS_NO; // Mulit Axes의 축 Max.

        public const int DEF_NO_COORDINATE = 0;            // Multi Axes의 축 구성 안함
        public const int DEF_ALL_COORDINATE = -1;          // Multi Axes의 모든 축 선택

        public const int DEF_AXIS_USE = 1;          // 축 사  용
        public const int DEF_AXIS_NON = 0;     // 축 미사용

        public const int DEF_AXIS_NON_NO = 0;          // 축 0개
        public const int DEF_AXIS_NON_ID = -1;         // 축 ID 미할당 (축 미구성)
        public const int DEF_AXIS_ALL_ID = -1;         // 축 전체 선택
                                                       //const	int	DEF_AXIS_BLANK_ID			= -2;			// 빈 축 ID (축 구성)

        // Move Priority Value ("1"이 우선순위 높음)
        public const int DEF_PRIORITY_NO = DEF_MAX_AXIS_NO;    // Multi Axes의 다축 이동 시 우선순위 사용 가능 개수
        public const int DEF_PRIORITY_NONE = -1;           // Multi Axes의 다축 이동 시 우선순위 1
        public const int DEF_PRIORITY_1 = 1;           // Multi Axes의 다축 이동 시 우선순위 1
        public const int DEF_PRIORITY_2 = 2;           // Multi Axes의 다축 이동 시 우선순위 2
        public const int DEF_PRIORITY_3 = 3;           // Multi Axes의 다축 이동 시 우선순위 3
        public const int DEF_PRIORITY_4 = 4;           // Multi Axes의 다축 이동 시 우선순위 4
        public const int DEF_PRIORITY_5 = 5;           // Multi Axes의 다축 이동 시 우선순위 5
        public const int DEF_PRIORITY_6 = 6;           // Multi Axes의 다축 이동 시 우선순위 6
        public const int DEF_PRIORITY_7 = 7;           // Multi Axes의 다축 이동 시 우선순위 7
        public const int DEF_PRIORITY_8 = 8;           // Multi Axes의 다축 이동 시 우선순위 8
        public const int DEF_PRIORITY_9 = 9;           // Multi Axes의 다축 이동 시 우선순위 9
        public const int DEF_PRIORITY_10 = 10;         // Multi Axes의 다축 이동 시 우선순위 10
        public const int DEF_PRIORITY_11 = 11;         // Multi Axes의 다축 이동 시 우선순위 11
        public const int DEF_PRIORITY_12 = 12;         // Multi Axes의 다축 이동 시 우선순위 12
        public const int DEF_PRIORITY_13 = 13;         // Multi Axes의 다축 이동 시 우선순위 13
        public const int DEF_PRIORITY_14 = 14;         // Multi Axes의 다축 이동 시 우선순위 14
        public const int DEF_PRIORITY_15 = 15;         // Multi Axes의 다축 이동 시 우선순위 15
        public const int DEF_PRIORITY_16 = 16;         // Multi Axes의 다축 이동 시 우선순위 16
        public const int DEF_PRIORITY_17 = 17;         // Multi Axes의 다축 이동 시 우선순위 17
        public const int DEF_PRIORITY_18 = 18;         // Multi Axes의 다축 이동 시 우선순위 18
        public const int DEF_PRIORITY_19 = 19;         // Multi Axes의 다축 이동 시 우선순위 19
        public const int DEF_PRIORITY_20 = 20;         // Multi Axes의 다축 이동 시 우선순위 20
        public const int DEF_PRIORITY_21 = 21;         // Multi Axes의 다축 이동 시 우선순위 21
        public const int DEF_PRIORITY_22 = 22;         // Multi Axes의 다축 이동 시 우선순위 22
        public const int DEF_PRIORITY_23 = 23;         // Multi Axes의 다축 이동 시 우선순위 23
        public const int DEF_PRIORITY_24 = 24;         // Multi Axes의 다축 이동 시 우선순위 24
        public const int DEF_PRIORITY_25 = 25;         // Multi Axes의 다축 이동 시 우선순위 25
        public const int DEF_PRIORITY_26 = 26;         // Multi Axes의 다축 이동 시 우선순위 26
        public const int DEF_PRIORITY_27 = 27;         // Multi Axes의 다축 이동 시 우선순위 27
        public const int DEF_PRIORITY_28 = 28;         // Multi Axes의 다축 이동 시 우선순위 28
        public const int DEF_PRIORITY_29 = 29;         // Multi Axes의 다축 이동 시 우선순위 29
        public const int DEF_PRIORITY_30 = 30;         // Multi Axes의 다축 이동 시 우선순위 30
        public const int DEF_PRIORITY_31 = 31;         // Multi Axes의 다축 이동 시 우선순위 31
        public const int DEF_PRIORITY_32 = 32;         // Multi Axes의 다축 이동 시 우선순위 32
        public const int DEF_PRIORITY_33 = 33;         // Multi Axes의 다축 이동 시 우선순위 33
        public const int DEF_PRIORITY_34 = 34;         // Multi Axes의 다축 이동 시 우선순위 34
        public const int DEF_PRIORITY_35 = 35;         // Multi Axes의 다축 이동 시 우선순위 35
        public const int DEF_PRIORITY_36 = 36;         // Multi Axes의 다축 이동 시 우선순위 36
        public const int DEF_PRIORITY_37 = 37;         // Multi Axes의 다축 이동 시 우선순위 37
        public const int DEF_PRIORITY_38 = 38;         // Multi Axes의 다축 이동 시 우선순위 38
        public const int DEF_PRIORITY_39 = 39;         // Multi Axes의 다축 이동 시 우선순위 39
        public const int DEF_PRIORITY_40 = 40;         // Multi Axes의 다축 이동 시 우선순위 40
        public const int DEF_PRIORITY_41 = 41;         // Multi Axes의 다축 이동 시 우선순위 41
        public const int DEF_PRIORITY_42 = 42;         // Multi Axes의 다축 이동 시 우선순위 42
        public const int DEF_PRIORITY_43 = 43;         // Multi Axes의 다축 이동 시 우선순위 43
        public const int DEF_PRIORITY_44 = 44;         // Multi Axes의 다축 이동 시 우선순위 44
        public const int DEF_PRIORITY_45 = 45;         // Multi Axes의 다축 이동 시 우선순위 45
        public const int DEF_PRIORITY_46 = 46;         // Multi Axes의 다축 이동 시 우선순위 46
        public const int DEF_PRIORITY_47 = 47;         // Multi Axes의 다축 이동 시 우선순위 47
        public const int DEF_PRIORITY_48 = 48;         // Multi Axes의 다축 이동 시 우선순위 48
        public const int DEF_PRIORITY_49 = 49;         // Multi Axes의 다축 이동 시 우선순위 49
        public const int DEF_PRIORITY_50 = 50;         // Multi Axes의 다축 이동 시 우선순위 50
        public const int DEF_PRIORITY_53 = 53;         // Multi Axes의 다축 이동 시 우선순위 53
        public const int DEF_PRIORITY_54 = 54;         // Multi Axes의 다축 이동 시 우선순위 54
        public const int DEF_PRIORITY_55 = 55;         // Multi Axes의 다축 이동 시 우선순위 55
        public const int DEF_PRIORITY_56 = 56;         // Multi Axes의 다축 이동 시 우선순위 56
        public const int DEF_PRIORITY_57 = 57;         // Multi Axes의 다축 이동 시 우선순위 57
        public const int DEF_PRIORITY_58 = 58;         // Multi Axes의 다축 이동 시 우선순위 58
        public const int DEF_PRIORITY_59 = 59;         // Multi Axes의 다축 이동 시 우선순위 59
        public const int DEF_PRIORITY_60 = 60;         // Multi Axes의 다축 이동 시 우선순위 60
        public const int DEF_PRIORITY_61 = 61;         // Multi Axes의 다축 이동 시 우선순위 61
        public const int DEF_PRIORITY_62 = 62;         // Multi Axes의 다축 이동 시 우선순위 62
        public const int DEF_PRIORITY_63 = 63;         // Multi Axes의 다축 이동 시 우선순위 63
        public const int DEF_PRIORITY_64 = 64;         // Multi Axes의 다축 이동 시 우선순위 64

        // Move 종류
        public const int DEF_MOVE_POSITION = 0;            // 사다리꼴 속도 Profile, 절대좌표 이동
        public const int DEF_SMOVE_POSITION = 1;           // S-Curve 속도 Profile, 절대좌표 이동
        public const int DEF_MOVE_DISTANCE = 2;            // 사다리꼴 속도 Profile, 상대거리 이동
        public const int DEF_SMOVE_DISTANCE = 3;           // S-Curve 속도 Profile, 상대거리 이동
        public const int DEF_TMOVE_POSITION = 4;           // 비대칭 사다리꼴 속도 Profile, 절대좌표 이동
        public const int DEF_TSMOVE_POSITION = 5;          // 비대칭 S-Curve 속도 Profile, 절대좌표 이동
        public const int DEF_TMOVE_DISTANCE = 6;           // 비대칭 사다리꼴 속도 Profile, 상대거리 이동
        public const int DEF_TSMOVE_DISTANCE = 7;          // 비대칭 S-Curve 속도 Profile, 상대거리 이동

        public const int DEF_2AXIS_MOVE = 2;
        public const int DEF_3AXIS_MOVE = 3;
        public const int DEF_4AXIS_MOVE = 4;

        public const int DEF_MAX_GROUP_NO = 8;
        public const int DEF_MIN_GROUP_NO = 1;

        // Stop 종류
        public const int DEF_STOP = 0;         // Stop
        public const int DEF_ESTOP = 1;            // E-Stop
        public const int DEF_VSTOP = 2;            // V-Stop

        // Limit Sensor 종류
        public const int DEF_HOME_SENSOR = 0;          // Home Sensor
        public const int DEF_POSITIVE_SENSOR = 1;          // Positive Sensor
        public const int DEF_NEGATIVE_SENSOR = 2;          // Negative Sensor

        // S/W Limit 종류
        public const bool DEF_POSITIVE_SW = true;          // S/W Positive Limit
        public const bool DEF_NEGATIVE_SW = false;     // S/W Negative Limit

        // Level 종류
        //#ifdef DEF_3_4LINE
        public const bool DEF_HIGH = true;         // High Level
                                                   //#else
                                                   //const	bool	DEF_HIGH				= false;			// High Level
                                                   //#endif

        public const bool DEF_LOW = false;     // Low Level

        // Event 종류
        public const int DEF_NO_EVENT = 0;         // ignore a condition
        public const int DEF_STOP_EVENT = 1;           // generate a stop event
        public const int DEF_E_STOP_EVENT = 2;         // generate an e_stop event
        public const int DEF_ABORT_EVENT = 3;          // disable PID control, and disable the amplifier

        // Motor 종류
        public const int DEF_SERVO_MOTOR = 0;          // 속도형 Servo
        public const int DEF_STEPPER = 1;          // 일반 Stepper
        public const int DEF_MICRO_STEPPER = 2;            // Micro Stepper 혹은 위치형 Servo

        // Feedback 종류
        public const int DEF_FB_ENCODER = 0;           // Encoder Feedback Device
        public const int DEF_FB_UNIPOLAR = 1;          // Unipolar Feedback Device
        public const int DEF_FB_BIPOLAR = 2;           // Bipolar Feedback Device

        // Control_Loop 종류
        public const bool DEF_OPEN_LOOP = false;       // Open Loop
        public const bool DEF_CLOSED_LOOP = true;          // Closed Loop

        // Control 방법
        public const bool DEF_V_CONTROL = false;       // 속도제어
        public const bool DEF_T_CONTROL = true;            // 토크제어

        //
        public const bool DEF_UNIPOLAR = false;
        public const bool DEF_BIPOLAR = true;

        // 적분모드 적용 사양
        public const int DEF_IN_STANDING = 0;          // 정지시만 적용
        public const int DEF_IN_ALWAYS = 1;            // 항상 적용

        // Pulse 종류
        public const int DEF_TWO_PULSE = 0;            // Two Pulse, CW+CCW
        public const int DEF_SIGN_PULSE = 1;           // Sign + Pulse

        // PC10 정의
        public const int DEF_PC_INDEXSEL_0 = 0;            // IndexSel 없음
        public const int DEF_PC_INDEXSEL_1 = 1;            // IndexSel 1축
        public const int DEF_PC_INDEXSEL_2 = 2;            // IndexSel 2축

        public const int DEF_PC_INDEX_MAX_NO = 8;          // Position Compare를 실시할 Index 최대 번호
        public const int DEF_PC_INDEX_MIN_NO = 1;

        public const bool DEF_PC_TRANSPARENT = false;      // Transparent Mode
        public const bool DEF_PC_LATCH = true;         // Latch Mode

        public const int DEF_PC_EQUAL = 1;         // Equal
        public const int DEF_PC_GT = 2;            // >
        public const int DEF_PC_LT = 3;            // <

        public const int DEF_PC_OUT_NON = 0;           // 축별 ON/OFF
        public const int DEF_PC_OUT_AND = 1;           // 두 축 AND
        public const int DEF_PC_OUT_OR = 2;            // 두 축 OR

        // I/O 정의
        public const int DEF_MAX_IO_PER_BOARD = 32;            // Board당 I/O Bit 수
        public const int DEF_MIN_IO_BIT = 0;
        public const bool DEF_IO_TYPE_IN = false;
        public const bool DEF_IO_TYPE_OUT = true;
        public const int DEF_MAX_IO_PORT = 3;
        public const int DEF_MIN_IO_PORT = 0;
        public const int DEF_MAX_ANALOG_CH = 4;
        public const int DEF_MIN_ANALOG_CH = 0;

        // I/O Interrupt
        public const int DEF_BOARD_ENABLE_MODE = 0;            // Board Interrup Enable Mode
        public const int DEF_STOP_EVENT_MODE = 1;          // Stop Event Interrup Mode
        public const int DEF_ESTOP_EVENT_MODE = 2;         // E-Stop Event Interrup Mode

        // In-Command 정의
        public const int DEF_INSEQUENCE = 0;           // in sequence (이동)
        public const int DEF_INMOTION = 1;         // in motion (속도 이동)
        public const int DEF_INPOSITION = 2;           // in position (위치)

        // Wait Done 정의
        public const bool DEF_MOTION_DONE = false;     // motion_done
        public const bool DEF_AXIS_DONE = true;            // axis_done

        // Current/Command 정의
        public const bool DEF_CURRENT_VAL = false;     // 현재값
        public const bool DEF_COMMAND_VAL = true;          // 명령값

        // ABS Motor 종류
        public const int DEF_SAMSUNGCSDJ = 1;          // 삼성 CSDJ, CSDJ + SERVO DRIVE
        public const int DEF_YASKAWA_SERVO_DRIVE = 2;          // YASKAWA SERVO DRIVE

        // Encoder Direction
        public const int DEF_CORD_CCW = 1;         // 반시계방향
        public const int DEF_CORD_CW = 0;          // 시계방향

        // Digital Filter Defines
        public const int DEF_GAIN_NUMBER = 5;          // elements expected get/set_filter(...)
        public const int DEF_GA_P = 0;         // proportional gain
        public const int DEF_GA_I = 1;         // integral gain
        public const int DEF_GA_D = 2;         // derivative gain-damping term
        public const int DEF_GA_F = 3;         // velocity feed forward
        public const int DEF_GA_ILIMIT = 4;            // integration summing limit

        // Sampling Rate
        public const int DEF_SAMPLING_4MSEC = 1;           // Samspling Rate 4msec
        public const int DEF_SAMPLING_2MSEC = 2;           // Samspling Rate 2msec
        public const int DEF_SAMPLING_1MSEC = 3;           // Samspling Rate 1msec

        // Analog Channel
        public const int DEF_ANALOG_IN_MAX_CH = 4;         // Analog Input Channel 최대값
        public const int DEF_ANALOG_IN_NON_CH = 0;         //

        // Lmit Vlaue
        public const int DEF_ACCEL_LIMIT = 25000;
        public const int DEF_VEL_LIMIT = 5000000;
        public const int DEF_POS_SW_LIMIT = 2147483640;
        public const int DEF_NEG_SW_LIMIT = -2147483640;
        public const int DEF_ERROR_LIMIT = 35000;
        public const int DEF_PULSE_RATIO = 255;
        public const double DEF_DEGREE = 1000.0;

        // Interpolation 관련
        public const int DEF_START_MOVE = 1;           // 동작 시작
        public const int DEF_MOVING = 2;           // 동작 중
        public const int DEF_END_MOVE = 3;         // 동작 완료

        // Position I/O
        public const int DEF_POSITION_IO_MAX_NO = 10;          //
        public const int DEF_POSITION_IO_MIN_NO = 1;           //
        public const int DEF_POSITION_IO_NON_NO = 0;           //

        // Spline Data
        public const int DEF_POSITION_DATA_MAX_NO = 30;
        public const int DEF_POSITION_DATA_MIN_NO = 1;
        public const int DEF_SPLINE_MOTION_MAX_NO = 20;
        public const int DEF_SPLINE_MOTION_MIN_NO = 1;
        public const int DEF_SPLINE_MOVE_PATH_MAX_NO = 500;
        public const int DEF_SPLINE_MOVE_PATH_MIN_NO = 1;

        // Event Source Status defines
        public const int DEF_ST_NONE = 0x0000;
        public const int DEF_ST_HOME_SWITCH = 0x0001;
        public const int DEF_ST_POS_LIMIT = 0x0002;
        public const int DEF_ST_NEG_LIMIT = 0x0004;
        public const int DEF_ST_AMP_FAULT = 0x0008;
        public const int DEF_ST_A_LIMIT = 0x0010;
        public const int DEF_ST_V_LIMIT = 0x0020;
        public const int DEF_ST_X_NEG_LIMIT = 0x0040;
        public const int DEF_ST_X_POS_LIMIT = 0x0080;
        public const int DEF_ST_ERROR_LIMIT = 0x0100;
        public const int DEF_ST_PC_COMMAND = 0x0200;
        public const int DEF_ST_OUT_OF_FRAMES = 0x0400;
        public const int DEF_ST_AMP_POWER_ONOFF = 0x0800;
        public const int DEF_ST_ABS_COMM_ERROR = 0x1000;
        public const int DEF_ST_INPOSITION_STATUS = 0x2000;
        public const int DEF_ST_RUN_STOP_COMMAND = 0x4000;
        public const int DEF_ST_COLLISION_STATE = 0x8000;

        // 원점복귀 Step
        public const int DEF_ORIGIN_START_STEP = 0;            // START
        public const int DEF_ORIGIN_FIRST_SET_STEP = 10;           // FIRST SETTING
        public const int DEF_ORIGIN_1ST_MOVE_STEP = 20;            // MOVE 1st
        public const int DEF_ORIGIN_2ND_MOVE_STEP = 30;            // MOVE 2nd
        public const int DEF_ORIGIN_3RD_MOVE_STEP = 40;            // MOVE 3rd
        public const int DEF_ORIGIN_4TH_MOVE_STEP = 50;            // MOVE 4th
        public const int DEF_ORIGIN_STOP_MOVE_STEP = 60;           // STOP MOVE
        public const int DEF_ORIGIN_LAST_SET_STEP = 70;            // LAST SETTING
        public const int DEF_ORIGIN_SET_ORIGIN_STEP = 80;          // SET ORIGIN
        public const int DEF_ORIGIN_ERROR_STEP = 999;          // ERROR
        public const int DEF_ORIGIN_FINISH_STEP = 1000;            // FINISH

        // 기타
        public const bool DEF_ENABLE = true;           // Enable
        public const bool DEF_DISABLE = false;     // Disable
        public const int ERR_MAX_ERROR_LEN = 80;           // maximum length for error massage string
        public const int DEF_DEFAULT_ORIGIN_WAIT_TIME = 60;        // 원점복귀 제한 시간 default 값
        public const bool DEF_COLLISION_SUB_POS = false;       // Collision Prevent 시 두 축 값의 차를 사용
        public const bool DEF_COLLISION_ADD_POS = true;            // Collision Prevent 시 두 축 값의 합을 사용
        public const bool DEF_COLLISION_LESSTHAN = false;      // Collision Prevent 시 기준값이 두 축 값다 작을 때 정지
        public const bool DEF_COLLISION_GREATTHAN = true;          // Collision Prevent 시 기준값이 두 축 값다 클 때 정지

        // MAX Frame
        public const int DEF_MAX_FRAME_NUM = 100;	// For use in Call frames_left Function

    }
    public struct SMotionAxis
    {
        /** Motor Type (0:속도형 Servo, 1:Stepper, 2:Micro Stepper or 위치형 Servo) - 2번만 지원 */
        int iMotorType;
        /** Loop Type (0:Open Loop, 1:Closed Loop) */
        bool bLoopType;
        /** Feedback Device Type (0:Encoder, 1:Unipolar, 2:Bipolar) */
        int iFeedbackType;
        /** 속도형 Servo 제어모드 (false:속도제어, true:토크제어) */
        bool bVServoControl;
        /** 속도형 Servo 출력모드 (true:Uni-Polar, false:Bi-Polar) */
        bool bVServoPolar;
        /** Stepper Pulse 분주비 */
        int iStepperPulseR;
        /** Stepper 전자기어비 */
        double dStepperEGear;
        /** Motor Pulse Type (0:Two-Pulse(CW+CCW), 1:Sign+Pulse) */
        bool bPulseType;
        /** Encoder 방향 () */
        bool bEncoderDir;
        /** 좌표 방향 () */
        bool bCoordinateDir;
        /** AMP Enable Level (true:HIGH, false:LOW) */
        bool bAmpEnableLevel;
        /** AMP Reset Level (true:HIGH, false:LOW) */
        bool bAmpResetLevel;
        /** AMP Fault Level (true:HIGH, false:LOW) */
        bool bAmpFaultLevel;
        /** In-Position Level (true:HIGH, false:LOW) */
        bool bInpositionLevel;
        /** In-Position Level Required (true, false) */
        bool bInpositionLevelRequired;
        /** Positive Sensor Level (true:HIGH, false:LOW) */
        bool bPositiveLevel;
        /** Negative Sensor Level (true:HIGH, false:LOW) */
        bool bNegativeLevel;
        /** Home Sensor Level (true:HIGH, false:LOW) */
        bool bHomeLevel;
        /** AMP Fault Event */
        int iAmpFaultEvent;
        /** Positive Sensor Event */
        int iPositiveEvent;
        /** Negative Sensor Event */
        int iNegativeEvent;
        /** Home Sensor Event */
        int iHomeEvent;
        /** Positive SW Limit */
        double dPositiveSWLimit;
        /** Negative SW Limit */
        double dNegativeSWLimit;
        /** Positive SW Limit Event */
        int iPositiveSWEvent;
        /** Negative SW Limit Event */
        int iNegativeSWEvent;
        /** In-Position Error */
        double dInpositionError;
    }

    /**
     * This structure is defined configuration of Motion boards.
     * You must use a object of this structure by static type,
     * because these are operated with same value.
     * 이 구조체는 Motion Board 구성 정보를 관리하는 구조체이다.
     * static type으로 개체를 만들어 개체간 동일 Data로 공유하도록 한다.
     *
     * @author ranian (ranian7@sfa.co.kr)
     * @version $Revision$
     *
     * @stereotype struct 
     */
    [StructLayout(LayoutKind.Sequential)]
    public struct SMotionBoard
    {
        // Motion Board - 초기 구성
        /** 장착된 Board 개수 (0:미구성, 1 ~ 8) */
        int iMaxBoardNo;                        // Initialize()에서 기록
                                                /** Motion Board DPRAM Address */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = DEF_MAX_MOTION_BD)]
        long[] rglAddress;     // Initialize()에서 기록
                                                /** 자동 가,감속 설정 여부 (true:자동) */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = DEF_MAX_MOTION_BD)]
        bool[] rgbAutoCP;      // SetAutoCP()에서 기록
                                                /** 원점복귀 대기 시간 (초단위) */
        //double	dOriginWaitTime;

        /** MMC-PC10 Option Board IndexSel */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = DEF_MAX_MOTION_BD)]
        int[] rgiPC10IndexSel;
        /** MMC-PC10 Option Board 사용 축 */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = DEF_MAX_MOTION_BD * 2)]
        int[][] rgiPC10Axis;
}

/**
 * This structure is defined configuration of single-axis.
 * 이 구조체는 축 1개에 대한 특성 정보를 관리하는 구조체이다.
 * 
 * Motor에 대한 특성 정보는 SMotionAxis에서 관리한다.
 *
 * @author ranian (ranian7@sfa.co.kr)
 * @version $Revision$
 *
 * @stereotype struct
 */
    [StructLayout(LayoutKind.Sequential)]
    public struct SAxis1
    {
        /** 축 ID (-1:미사용, 0 ~ 64) */
        int iAxisID;
        /** 축 이름 (최대 32문자) */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_LENGTH_AXIS_NAME)]
        char[] szName;
        /** 동시 이동 시 우선 순위 지정 (1 ~ 64, 0:미사용) */
        int iMovePriority;
        /** 원점복귀 동시 이동 시 우선 순위 지정 (1 ~ 64, 0:미사용) */
        int iOriginPriority;
        /** 원점위치 */
        double dHomePosition;
        /** S/W (-)방향 이동 제한 위치 */
        double dNegativeLimitPosition;
        /** S/W (+)방향 이동 제한 위치 */
        double dPositiveLimitPosition;
        /** 이동 속도 */
        double dMovingVelocity;
        /** 이동 가속도 */
        int iMovingAccelerate;
        /** 이동 감속도 */
        int iMovingDecelerate;
        /** Coarse 속도 */
        double dCoarseVelocity;
        /** Coarse 가속도 */
        int iCoarseAccelerate;
        /** Fine 속도 */
        double dFineVelocity;
        /** Fine 가속도 */
        int iFineAccelerate;
        /** Jog 속도 */
        double dJogVelocity;
        /** Jog Pitch */
        double dJogPitch;
        /** 축 이동방향 (true:+, false:-) */
        bool bSign;
        /** 축 원점복귀 진행방향(Coarse 속도 구간) (true:+, false:-) */
        bool bOriginDir;
        /** 축 원점복귀 진행방향(Fine 속도 구간) (TRUEL+, false:-) */
        bool bOriginFineDir;
        /** C상 사용여부 (true:사용함) */
        bool bCPhaseUse;
        /** 이동 값에 대한 Scale (default:1.0) */
        double dScale;
        /** 이동 지연 시간 */
        double dMoveTime;
        /** 이동후 안정화 시간 */
        double dMoveAfterTime;
        /** Tolerance - 위치 허용 오차 */
        double dTolerance;
        /** 원점복귀 대기 시간 (초단위) */
        double dOriginWaitTime;
    }

/**
 * This structure is defined configuration of origin thread parameter.
 * 이 구조체는 원점복귀 Thread에 전달할 인수에 대한 Data를 관리하는 구조체이다.
 * 
 * @author ranian (ranian7@sfa.co.kr)
 * @version $Revision$
 *
 * @stereotype struct
 */
    [StructLayout(LayoutKind.Sequential)]
    public struct SOriginThread
    {
        /** 원점복귀할 좌표, -1 = All Axis */
        int iCID;
        /** 원점복귀할 축 지정, iCoordinateID=-1일때만 사용
         * iCoordinateID가 -1이 아니면 사용안함 (null)
         * 배열구조에 사용하고자하는 축 위치에 true지정 */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = DEF_MAX_COORDINATE)]
        bool[] rgbUse;
        /** 원점복귀 시 이동할 지 여부, true=이동 포함 */
        bool[] bMoveOpt;
        /** 원점복귀하기 전 Limit Sensor Event 설정 값 */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = DEF_MAX_AXIS_NO)]
        int[] rgiPositiveLimit;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = DEF_MAX_AXIS_NO)]
        int[] rgiNegativeLimit;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = DEF_MAX_AXIS_NO)]
        int[] rgiHomeLimit;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = DEF_MAX_AXIS_NO)]
        bool[] rgbPositiveLevel;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = DEF_MAX_AXIS_NO)]
        bool[] rgbNegativeLevel;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = DEF_MAX_AXIS_NO)]
        bool[] rgbHomeLevel;
    }

    public interface IMultiAxes
    {
        /**
 * Motion Component를 초기화한다.
 *
 * 1. Motion Board 종류에 맞는 Motion Library 개체를 생성한다.
 * 2. 축 구성 개수를 설정한다.
 * 3. 축 정보를 설정한다.
 *
 * @param	iObjectID		: Object ID
 * @param	iBoardType		: Motion Board Type (1=MMC Board, 2=MEI board, 3=PPC Board, ...)
 * @param	iAxesNum		: 축 구성 개수
 * @param	*saxAxis		: 1축 구성 정보 (축 구성 수만큼 배열로 존재)
 */
        int Initialize(int iBoardType, int iAxesNum, ref SAxis1[] saxAxis);

        /**
         * 해당 축의 현재까지 쌓인 Frame을 Clear한다.
         *
         * @param   iCoordinateID   : 구성 축 배열 Index, -1 = All Axis
         * @return	Error Code		: 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID PRIORITY NUMBER (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int ClearFrames(int iCoordinateID);

        /**
         * MMC 보드의 비어있는 Frame 갯수를 돌려준다.
         *
         * @param   iCoordinateID   : 구성 축 배열 Index, -1 = All Axis
         * @return	Error Code		: 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID PRIORITY NUMBER (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int FramesLeft(int iCoordinateID, int[] piFrameNo);

        int SetStop(int iCoordinateID, int iType = DEF_STOP);

        /**
         * 축 구성 개수를 설정한다. (축 정보 설정과는 별개로 동작한다.)
         *
         * @param   iAxesNum        : 축 구성 개수 (0 ~ 64)
         * @return	Error Code		: 0 = SUCCESS
         *							  xx = INVALID AXES NUMBER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int SetAxesNumber(int iAxesNum);


        /**
         * 축 구성 개수를 읽는다.
         *
         * @param   *piAxesNum      : 축 구성 개수 (0 ~ 64)
         * @return	Error Code		: 0 = SUCCESS
         *							  xx = INVALID AXES NUMBER (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int GetAxesNumber(out int piAxesNum);


        /**
         * 축 이동 우선순위를 설정한다.
         * 축 정보에 우선순위 설정 후 우선순위별 축 ID를 재정렬한다.
         *
         * @param   iCoordinateID   : 구성 축 배열 Index, -1 = All Axis
         * @param   *pdPriority      : 축 이동 우선순위 (1 ~ 64), iCoordinateID=-1이면 배열로 구성
         * @return	Error Code		: 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID PRIORITY NUMBER (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int SetAxesMovePriority(int iCoordinateID, int[] pdPriority);


        /**
         * 축 이동 우선순위를 읽는다.
         *
         * @param   iCoordinateID   : 구성 축 배열 Index, -1 = All Axis
         * @param   *dpPriority     : 축 이동 우선순위 (1 ~ 64), iCoordinateID=-1이면 배열로 구성
         * @return	Error Code		: 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID PRIORITY NUMBER (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int GetAxesMovePriority(int iCoordinateID, out int[] pdPriority);


        /**
         * 축 원점복귀 이동 우선순위를 설정한다.
         * 축 정보에 우선순위 설정 후 우선순위별 축 ID를 재정렬한다.
         *
         * @param   iCoordinateID   : 구성 축 배열 Index, -1 = All Axis
         * @param   *pdPriority      : 축 원점복귀 이동 우선순위 (1 ~ 64), iCoordinateID=-1이면 배열로 구성
         * @return	Error Code		: 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID PRIORITY NUMBER (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int SetAxesOriginPriority(int iCoordinateID, int[] pdPriority);


        /**
         * 축 원점복귀 이동 우선순위를 읽는다.
         *
         * @param   iCoordinateID   : 구성 축 배열 Index, -1 = All Axis
         * @param   *dpPriority     : 축 원점복귀 이동 우선순위 (1 ~ 64), iCoordinateID=-1이면 배열로 구성
         * @return	Error Code		: 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID PRIORITY NUMBER (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int GetAxesOriginPriority(int iCoordinateID, out int[] pdPriority);


        /**
         * 축 1개 또는 구성된 축 모두에 대한 Data를 설정한다. (구조체)
         *
         * 1. 전체 축 모두 설정할 경우
         *	(1) 기 정보 영역이 존재하면 제거한다.
         *  (2) 영역을 새로 allocation한다.
         *	(3) 새 영역에 정보를 설정한다. 이때 축이 이미 사용되고 있으면 안된다.
         * 2. 축 하나만 설정할 경우
         *	(1) 축 정보 영역이 있어야 한다.
         *	(2) 축이 이미 사용되고 있으면 안된다.
         *
         * @param   iCoordinateID   : 구성 축 배열 Index, -1 = All Axis
         * @param   pax1Data        : 설정할 각 축의 설정 Data, iCoordinateID의 위치에 기록)
         * @return	Error Code		: 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  xx = USED AXIS ID (MULTIAXES)
         *							  xx = NO EXIST AXIS PARAMETER AREA (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int SetAxisData(int iCoordinateID, SAxis1[] ax1Data);


        /**
         * 축 1개 또는 구성된 축 모두에 대한 Data를 읽는다. (구조체)
         *
         * @param   iCoordinateID   : 구성 축 배열 Index, -1 = All Axis
         * @param   ax1Data[]       : 설정할 각 축의 설정 Data, iCoordinateID의 위치에 기록)
         * @return	Error Code		: 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int GetAxisData(int iCoordinateID, out SAxis1[] ax1Data);


        /** 
         * Board에 대한 자동 가, 감속 사용여부를 설정한다.
         *
         * @param   iBoardNo        : MMC Board 번호 0 ~ 7, -1 = All Board
         * @param   *pbAutoSet      : 자동 가,감속 설정여부, true : 수동, false : 자동, iBoardNo=-1이면 배열로 구성
         * @return	Error Code		: 0 = SUCCESS
         *							  xx = INVALID MOTION BOARD ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int SetAutoCP(int iBoardNo, bool[] pbAutoSet);


        /** 
          * Board에 대한 자동 가, 감속 사용여부를 읽는다.
          *
          * @param   iBoardNo        : MMC Board 번호 0 ~ 7, -1 = All Board
          * @param   *pbAutoSet      : 자동 가,감속 설정여부, true : 수동, false : 자동, iBoardNo=-1이면 배열로 구성
          * @return	Error Code		: 0 = SUCCESS
          *							  xx = INVALID MOTION BOARD ID (MULTIAXES)
          *							  xx = INVALID POINTER (MULTIAXES)
          *							  그 외 = 타 함수 Return Error
          */
        int GetAutoCP(int iBoardNo, out bool[] pbAutoSet);


        /** 
         * 축 1개 또는 구성된 축 모두에 대한 Axis ID를 설정한다.
         *
         * @param   iCoordinateID   : 구성 축 배열 Index, -1 = All Axis
         * @param   *piID           : 설정할 iAxisID, iCoordinateID=-1이면 배열로 제공
         * @return	Error Code		: 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int SetAxisID(int iCoordinateID, int[] piID);


        /** 
         * 축 1개 또는 구성된 축 모두에 대한 Axis ID를 읽는다.
         *
         * @param   iCoordinateID   : 구성 축 배열 Index, -1 = All Axis
         * @param   *piID           : 읽은 iAxisID, iCoordinateID=-1이면 배열로 제공
         * @return	Error Code		: 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int GetAxisID(int iCoordinateID, out int[] piID);


        /** 
         * 축 1개 또는 구성된 축 모두에 대한 Home 위치(원점복귀위치)를 설정한다.
         *
         * @param   iCoordinateID   : 구성 축 배열 Index, -1 = All Axis
         * @param   *pPosition      : 설정할 dHomePosition, iCoordinateID=-1이면 배열로 제공
         * @return	Error Code		: 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int SetHomePosition(int iCoordinateID, double[] pdPosition);


        /** 
         * 축 1개 또는 구성된 축 모두에 대한 Home 위치(원점복귀위치)를 읽는다.
         *
         * @param   iCoordinateID   : 구성 축 배열 Index, -1 = All Axis
         * @param   *pdPosition     : 읽은 dHomePosition, iCoordinateID=-1이면 배열로 제공
         * @return	Error Code		: 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int GetHomePosition(int iCoordinateID, out double[] pdPosition);


        /** 
         * 축 1개 또는 구성된 축 모두에 대한 Negative Limit 위치를 설정한다.
         *
         * @param   iCoordinateID   : 구성 축 배열 Index, -1 = All Axis
         * @param   *pdPosition     : 설정할 dNegativeLimit Position, iCoordinateID=-1이면 배열로 제공
         * @return	Error Code		: 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int SetNegativePosition(int iCoordinateID, double[] pdPosition);


        /** 
         * 축 1개 또는 구성된 축 모두에 대한 Negative Limit 위치를 읽는다.
         *
         * @param   iCoordinateID   : 구성 축 배열 Index, -1 = All Axis
         * @param   *pdPosition     : 읽은 dNegativeLimit Position, iCoordinateID=-1이면 배열로 제공
         * @return	Error Code		: 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int GetNegativePosition(int iCoordinateID, out double[] pdPosition);


        /** 
         * 축 1개 또는 구성된 축 모두에 대한 Positive Limit 위치를 설정한다.
         *
         * @param   iCoordinateID   : 구성 축 배열 Index, -1 = All Axis
         * @param   *pdPosition     : 설정할 dPositiveLimit Position, iCoordinateID=-1이면 배열로 제공
         * @return	Error Code		: 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int SetPositivePosition(int iCoordinateID, double[] pdPosition);


        /** 
         * 축 1개 또는 구성된 축 모두에 대한 Positive Limit 위치를 읽는다.
         *
         * @param   iCoordinateID   : 구성 축 배열 Index, -1 = All Axis
         * @param   *pdPosition     : 읽은 dPositiveLimit Position, iCoordinateID=-1이면 배열로 제공
         * @return	Error Code		: 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int GetPositivePosition(int iCoordinateID, out double[] pdPosition);


        /** 
         * 축 1개 또는 구성된 축 모두에 대한 Moving속도, 가속도를 설정한다.
         *
         * @param   iCoordinateID    : 구성 축 배열 Index, -1 = All Axis
         * @param   *pdVelocity      : 설정할 dMovingVelocity, iCoordinateID=-1이면 배열로 제공
         * @param   *piAccelerate    : 설정할 iMovingAccelerate, iCoordinateID=-1이면 배열로 제공
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int SetMovingVelocity(int iCoordinateID, double[] pdVelocity, int[] piAccelerate);


        /** 
         * 축 1개 또는 구성된 축 모두에 대한 Moving속도, 가속도를 읽는다.
         *
         * @param   iCoordinateID    : 구성 축 배열 Index, -1 = All Axis
         * @param   *pdVelocity      : 읽은 dMovingVelocity, iCoordinateID=-1이면 배열로 제공
         * @param   *piAccelerate    : 읽은 iMovingAccelerate, iCoordinateID=-1이면 배열로 제공
         * @return	Error Code	     : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int GetMovingVelocity(int iCoordinateID, out double[] pdVelocity, out int[] piAccelerate);


        /** 
         * 축 1개 또는 구성된 축 모두에 대한 Coarse속도, 가속도를 설정한다.
         *
         * @param   iCoordinateID    : 구성 축 배열 Index, -1 = All Axis
         * @param   *pdVelocity      : 설정할 dCoarseVelocity, iCoordinateID=-1이면 배열로 제공
         * @param   *piAccelerate    : 설정할 iCoarseAccelerate, iCoordinateID=-1이면 배열로 제공
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int SetCoarseVelocity(int iCoordinateID, double[] pdVelocity, int[] piAccelerate);


        /** 
         * 축 1개 또는 구성된 축 모두에 대한 Coarse속도, 가속도를 읽는다.
         *
         * @param   iCoordinateID    : 구성 축 배열 Index, -1 = All Axis
         * @param   *pdVelocity      : 읽은 dCoarseVelocity, iCoordinateID=-1이면 배열로 제공
         * @param   *piAccelerate    : 읽은 iCoarseAccelerate, iCoordinateID=-1이면 배열로 제공
         * @return	Error Code	     : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int GetCoarseVelocity(int iCoordinateID, out double[] pdVelocity, out int[] piAccelerate);


        /** 
         * 축 1개 또는 구성된 축 모두에 대한 Fine속도, 가속도를 설정한다.
         *
         * @param   iCoordinateID    : 구성 축 배열 Index, -1 = All Axis
         * @param   *pdVelocity      : 설정할 dFineVelocity, iCoordinateID=-1이면 배열로 제공
         * @param   *piAccelerate    : 설정할 iFineAccelerate, iCoordinateID=-1이면 배열로 제공
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int SetFineVelocity(int iCoordinateID, double[] pdVelocity, int[] piAccelerate);


        /** 
         * 축 1개 또는 구성된 축 모두에 대한 Fine속도, 가속도를 읽는다.
         *
         * @param   iCoordinateID    : 구성 축 배열 Index, -1 = All Axis
         * @param   *pdVelocity      : 읽은 dFineVelocity, iCoordinateID=-1이면 배열로 제공
         * @param   *piAccelerate    : 읽은 iFineAccelerate, iCoordinateID=-1이면 배열로 제공
         * @return	Error Code	     : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int GetFineVelocity(int iCoordinateID, out double[] pdVelocity, out int[] piAccelerate);


        /** 
         * 축 1개 또는 구성된 축 모두에 대한 Jog Move의 Pitch, 속도를 설정한다.
         *
         * @param   iCoordinateID    : 구성 축 배열 Index, -1 = All Axis
         * @param   *pdPitch         : 설정할 dJogPitch, iCoordinateID=-1이면 배열로 제공
         * @param   *pdVelocity      : 설정할 dJogVelocity, iCoordinateID=-1이면 배열로 제공
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int SetJogVelocity(int iCoordinateID, double[] pdPitch, double[] pdVelocity);


        /** 
         * 축 1개 또는 구성된 축 모두에 대한 Jog Move의 Pitch, 속도를 읽는다.
         *
         * @param   iCoordinateID    : 구성 축 배열 Index, -1 = All Axis
         * @param   *pdPitch         : 읽은 dJogPitch, iCoordinateID=-1이면 배열로 제공
         * @param   *pdVelocity      : 읽은 dJogVelocity, iCoordinateID=-1이면 배열로 제공
         * @return	Error Code	     : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int GetJogVelocity(int iCoordinateID, out double[] pdPitch, out double[] pdVelocity);


        /** 
         * 축 1개 또는 구성된 축 모두에 대한 Sign을 설정한다.
         *
         * @param   iCoordinateID    : 구성 축 배열 Index, -1 = All Axis
         * @param   *pbSign          : 설정할 bSign, iCoordinateID=-1이면 배열로 제공
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int SetSign(int iCoordinateID, bool[] pbSign);


        /** 
         * 축 1개 또는 구성된 축 모두에 대한 Sign을 읽는다.
         *
         * @param   iCoordinateID    : 구성 축 배열 Index, -1 = All Axis
         * @param   *pbSign          : 읽은 bSign, iCoordinateID=-1이면 배열로 제공
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int GetSign(int iCoordinateID, out bool[] pbSign);


        /** 
         * 축 1개 또는 구성된 축 모두에 대한 원점복귀 진행방향을 설정한다.
         *   Limit Sensor 구성에 따른 원점복귀 초기 진행방향을 설정할 수 있게 한다.
         *
         * @param   iCoordinateID    : 구성 축 배열 Index, -1 = All Axis
         * @param   *pbDir           : 설정할 bOriginDir, iCoordinateID=-1이면 배열로 제공
         *                                                (true : +방향, false : -방향)
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int SetOriginDir(int iCoordinateID, bool[] pbDir);


        /** 
         * 축 1개 또는 구성된 축 모두에 대한 원점복귀 진행방향을 읽는다.
         *   Limit Sensor 구성에 따른 원점복귀 초기 진행방향을 읽을 수 있게 한다.
         *
         * @param   iCoordinateID    : 구성 축 배열 Index, -1 = All Axis
         * @param   *pbDir           : 설정할 bOriginDir, iCoordinateID=-1이면 배열로 제공
         *                                                (true : +방향, false : -방향)
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int GetOriginDir(int iCoordinateID, out bool[] pbDir);


        /** 
         * 축 1개 또는 구성된 축 모두에 대한 원점복귀 진행(Fine구간)방향을 설정한다.
         *   Fine 속도 구간에서 초기 진행방향을 설정할 수 있게 한다.
         *
         * @param   iCoordinateID    : 구성 축 배열 Index, -1 = All Axis
         * @param   *pbDir           : 설정할 bOriginDir, iCoordinateID=-1이면 배열로 제공
         *                                                (true : +방향, false : -방향)
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int SetOriginFineDir(int iCoordinateID, bool[] pbDir);


        /** 
         * 축 1개 또는 구성된 축 모두에 대한 원점복귀 진행(Fine구간)방향을 읽는다.
         *   Fine 속도 구간에서 초기 진행방향을 읽을 수 있게 한다.
         *
         * @param   iCoordinateID    : 구성 축 배열 Index, -1 = All Axis
         * @param   *pbDir           : 설정할 bOriginDir, iCoordinateID=-1이면 배열로 제공
         *                                                (true : +방향, false : -방향)
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int GetOriginFineDir(int iCoordinateID, out bool[] pbDir);


        /** 
         * 축 1개 또는 구성된 축 모두에 대한 C상 사용여부를 설정한다.
         *
         * @param   iCoordinateID    : 구성 축 배열 Index, -1 = All Axis
         * @param   *pbUse           : 설정할 bCPhaseUse, iCoordinateID=-1이면 배열로 제공
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int SetCPhaseUse(int iCoordinateID, bool[] pbUse);


        /** 
         * 축 1개 또는 구성된 축 모두에 대한 C상 사용여부를 읽는다.
         *
         * @param   iCoordinateID    : 구성 축 배열 Index, -1 = All Axis
         * @param   *pbUse           : 읽은 bCPhaseUse, iCoordinateID=-1이면 배열로 제공
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int GetCPhaseUse(int iCoordinateID, out bool[] pbUse);


        /** 
         * 축 1개 또는 구성된 축 모두에 대한 Scale을 설정한다.
         *
         * @param   iCoordinateID    : 구성 축 배열 Index, -1 = All Axis
         * @param   *pdScale         : 설정할 dScale, iCoordinateID=-1이면 배열로 제공
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int SetScale(int iCoordinateID, double[] pdScale);


        /** 
         * 축 1개 또는 구성된 축 모두에 대한 Scale을 읽는다.
         *
         * @param   iCoordinateID    : 구성 축 배열 Index, -1 = All Axis
         * @param   *pdScale         : 읽은 dScale, iCoordinateID=-1이면 배열로 제공
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int GetScale(int iCoordinateID, out double[] pdScale);


        /** 
         * 축 이동 시 지연 시간을 설정한다.
         *
         * @param   iCoordinateID    : 구성 축 배열 Index, -1 = All Axis
         * @param   *pdTime          : 설정할 이동 지연 시간 (초단위), iCoordinateID=-1이면 배열로 제공
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int SetMoveTime(int iCoordinateID, double[] pdTime);


        /** 
         * 축 이동 시 지연 시간을 읽는다.
         *
         * @param   iCoordinateID    : 구성 축 배열 Index, -1 = All Axis
         * @param   *pdTime          : 설정된 이동 지연 시간 (초단위), iCoordinateID=-1이면 배열로 제공
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int GetMoveTime(int iCoordinateID, out double[] pdTime);


        /** 
         * 축 이동 후 안정화 시간을 설정한다.
         *
         * @param   iCoordinateID    : 구성 축 배열 Index, -1 = All Axis
         * @param   *pdTime          : 설정할 이동 후 안정화 시간 (초단위), iCoordinateID=-1이면 배열로 제공
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int SetMoveAfterTime(int iCoordinateID, double[] pdTime);


        /** 
         * 축 이동 후 안정화 시간을 읽는다.
         *
         * @param   iCoordinateID    : 구성 축 배열 Index, -1 = All Axis
         * @param   *pdTime          : 설정된 이동 후 안정화 시간 (초단위), iCoordinateID=-1이면 배열로 제공
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int GetMoveAfterTime(int iCoordinateID, out double[] pdTime);


        /** 
         * 축 위치 허용 오차를 설정한다.
         *
         * @param   iCoordinateID    : 구성 축 배열 Index, -1 = All Axis
         * @param   *pdTolerance     : 설정할 위치 허용 오차 (mm단위), iCoordinateID=-1이면 배열로 제공
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int SetTolerance(int iCoordinateID, double[] pdTolerance);


        /** 
         * 축 위치 허용 오차를 읽는다.
         *
         * @param   iCoordinateID    : 구성 축 배열 Index, -1 = All Axis
         * @param   *pdTolerance     : 설정된 위치 허용 오차 (mm단위), iCoordinateID=-1이면 배열로 제공
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int GetTolerance(int iCoordinateID, out double[] pdTolerance);


        /** 
         * 축 원점복귀 완료 대기 시간(초)을 설정한다.
         *
         * @param   iCoordinateID    : 구성 축 배열 Index, -1 = All Axis
         * @param   *pdTime          : 설정할 원점복귀 완료 대기 시간 (초 단위), iCoordinateID=-1이면 배열로 제공
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int SetOriginWaitTime(int iCoordinateID, double[] pdTime);


        /** 
         * 축 원점복귀 완료 대기 시간(초)을 읽는다.
         *
         * @param   iCoordinateID    : 구성 축 배열 Index, -1 = All Axis
         * @param   *pdTime          : 설정된 원점복귀 완료 대기 시간 (초 단위), iCoordinateID=-1이면 배열로 제공
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int GetOriginWaitTime(int iCoordinateID, out double[] pdTime);


        /**
         * 축이 원점복귀 됐는지 확인한다. (한개의 축 혹은 구성된 모든 축에 대해 가능)
         * 모든 축에 대한 원점복귀 확인 시 오류 Code는 전달되지 않는다.
         * 확인하고자 하는 축에 대해 오류 Code를 읽어봐야 한다.
         * 
         * @param   iCoordinateID    : 구성 축 배열 Index, -1 = All Axis
         * @param	*pbResult        : (OPTION = null) 축에 대한 원점복귀 여부 종합 상태
         * @param   *pbStatus        : (OPTION = null) 각 축마다 원점복귀 여부 읽기, iCoordinateID=-1이면 배열로 제공
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  xx = NOT RETURNED ORIGIN (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int IsOriginReturn(int iCoordinateID, out bool[] pbResult, out bool[] pbStatus);


        /**
         * 축 원점복귀 해제하기 (한개의 축 혹은 구성된 모든 축에 대해 가능)
         * 
         * @param   iCoordinateID    : 구성 축 배열 Index, -1 = All Axis
         * @param   *pbReturn        : (OPTION = null) 원점복귀 결과 읽기, iCoordinateID=-1이면 배열로 제공
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int ResetOrigin(int iCoordinateID, bool[] pbReturn = null);


        /**
         * 축 원점복귀 하기 (한개의 축 혹은 구성된 모든 축에 대해 가능)
         * 한번의 명령 수행 후 원점복귀가 완전히 종료된 후 다음 원점복귀를 수행할 수 있다. 만약 명령을 연달아 2번 이상 실행하게 되면 최종 명령만 수행하게 된다.
         * 
         * @param   iCoordinateID    : 구성 축 배열 Index, -1 = All Axis
         * @param	*pbUse           : 원점복귀할 축 지정, iCoordinateID=-1일때만 사용
         *                                                 iCoordinateID가 -1이 아니면 사용안함 (null)
         *                                                 배열구조에 사용하고자하는 축 위치에 true지정
         * @param	bMoveOpt         : 원점복귀 시 이동할 지 여부, true=이동 포함
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  xx = NOT EXECUTE ORIGIN RETURN THREAD (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int ReturnOrigin(int iCoordinateID, bool[] pbUse, bool bMoveOpt);


        /**
         * 축 원점복귀 강제 종료하기 (구성된 모든 축에 대해 동작 정지 명령 수행)
         * 
         * @return	Error Code		 : 0 = SUCCESS
         *							  그 외 = 타 함수 Return Error
         */
        int StopReturnOrigin();


        /**
         * 축의 현재좌표를 읽는다.
         * 
         * @param   iCoordinateID        : 구성 축 배열 Index, -1 = All Axis
         * @param   *pdCurrentPosition   : 현재 좌표값, iCoordinateID=-1이면 배열로 제공
         * @param   bType                : 읽을 위치 종류, false=실제위치, true=목표위치
         * @return	Error Code		     : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int GetCurrentPosition(int iCoordinateID, out double[] pdCurrentPosition, bool bType);


        /**
         * 축의 현재좌표를 설정한다.
         * 
         * @param   iCoordinateID        : 구성 축 배열 Index, -1 = All Axis
         * @param   *pdCurrentPosition   : 현재 좌표값, iCoordinateID=-1이면 배열로 제공
         * @param   bType                : 읽을 위치 종류, false=실제위치, true=목표위치
         * @return	Error Code		     : 0 = SUCCESS
         *								  xx = INVALID AXIS ID (MULTIAXES)
         *								  xx = INVALID POINTER (MULTIAXES)
         *								  그 외 = 타 함수 Return Error
         */
        int SetCurrentPosition(int iCoordinateID, double[] pdCurrentPosition, bool bType);


        /**
         * 축의 현재좌표와 특정좌표간의 수치에 의한 좌표차이를 비교한다.
         * 
         * @param   iCoordinateID        : 구성 축 배열 Index, -1 = All Axis
         * @param	bPosOpt              : 비교할 위치 종류, false=현재위치, true=Motion의 목표위치
         * @param   *pdTargetPosition    : 비교할 좌표값, iCoordinateID=-1이면 배열로 제공
         * @param   *pdPermission        : 비교허용 오차, iCoordinateID=-1이면 배열로 제공, null이면 내부 Tolerance값으로 비교한다.
         * @param   *pbJudge             : 비교결과, iCoordinateID=-1이면 배열로 제공
         * @param   *pdDeviation         : (OPTION = null) 비교 차이값, iCoordinateID=-1이면 배열로 제공
         * @return	Error Code		     : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int ComparePosition(int iCoordinateID, bool bPosOpt, double[] pdTargetPosition, double[] pdPermission,
                                     bool[] pbJudge, double[] pdDeviation = null);


        /**
         * 축 이동 (한개의 축 또는 여러 개(우선순위 이동)의 축에 대한 이동) - 이동 완료된 후 return
         * 
         * @param   iCoordinateID    : 이동할 축 지정, -1 = All Axis
         * @param   *pbMoveUse       : 이동할 축 지정, iCoordinateID=-1일때만 적용, 동시 이동할 축에 true 지정
         * @param   *pdPosition      : 이동할 위치, iCoordinateID=-1이면 배열로 제공
         * @param   *pdVelocity      : 이동할 속도, iCoordinateID=-1이면 배열로 제공, 0.0 or null = 지정된 속도 사용
         * @param   *piAccelerate    : 이동할 가속도, iCoordinateID=-1이면 배열로 제공, 0 or null = 지정된 가속도 사용	
         * @param   *piDecelerate    : 이동할 감속도, iCoordinateID=-1이면 배열로 제공, 0 or null = 지정된 감속도 사용	
         * @param	iMoveType        : 이동 Type, 0=사다리꼴 속도 Profile, 절대좌표 이동
         *										 1=S-Curve 속도 Profile, 절대좌표 이동
         *										 4=비대칭 사다리꼴 속도 Profile, 절대좌표 이동
         *										 5=비대칭 S-Curve 속도 Profile, 절대좌표 이동
         * @param   bPriority        : 우선순위 이동 사용 여부, true=사용, false=동시이동
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int Move(int iCoordinateID, bool[] pbMoveUse, double[] pdPosition, double[] pdVelocity,
                          int[] piAccelerate, int[] piDecelerate, int iMoveType, bool bPriority);


        /**
         * 축 이동 (한개의 축 또는 여러 개(우선순위 무시)의 축에 대한 이동) - 이동 명령 후 바로 return
         * 
         * @param   iCoordinateID    : 이동할 축 지정, -1 = All Axis
         * @param   *pbMoveUse       : 이동할 축 지정, iCoordinateID=-1일때만 적용, 동시 이동할 축에 true 지정
         * @param   *pdPosition      : 이동할 위치, iCoordinateID=-1이면 배열로 제공
         * @param   *pdVelocity      : 이동할 속도, iCoordinateID=-1이면 배열로 제공, 0.0 or null = 지정된 속도 사용
         * @param   *piAccelerate    : 이동할 가속도, iCoordinateID=-1이면 배열로 제공, 0 or null = 지정된 가속도 사용	
         * @param   *piDecelerate    : 이동할 감속도, iCoordinateID=-1이면 배열로 제공, 0 or null = 지정된 감속도 사용	
         * @param	iMoveType        : 이동 Type, 0=사다리꼴 속도 Profile, 절대좌표 이동
         *										 1=S-Curve 속도 Profile, 절대좌표 이동
         *										 4=비대칭 사다리꼴 속도 Profile, 절대좌표 이동
         *										 5=비대칭 S-Curve 속도 Profile, 절대좌표 이동
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int StartMove(int iCoordinateID, bool[] pbMoveUse, double[] pdPosition, double[] pdVelocity,
                               int[] piAccelerate, int[] piDecelerate, int iMoveType);


        /**
         * 축 이동 (한개의 축 또는 여러 개(우선순위 이동)의 축에 대한 상대위치 이동) - 이동 완료된 후 return
         * 
         * @param   iCoordinateID    : 이동할 축 지정, -1 = All Axis
         * @param   *pbMoveUse       : 이동할 축 지정, iCoordinateID=-1일때만 적용, 동시 이동할 축에 true 지정
         * @param   *pdDistance      : 이동할 거리, iCoordinateID=-1이면 배열로 제공
         * @param   *pdVelocity      : 이동할 속도, iCoordinateID=-1이면 배열로 제공, 0.0 or null = 지정된 속도 사용
         * @param   *piAccelerate    : 이동할 가속도, iCoordinateID=-1이면 배열로 제공, 0 or null = 지정된 가속도 사용	
         * @param   *piDecelerate    : 이동할 감속도, iCoordinateID=-1이면 배열로 제공, 0 or null = 지정된 감속도 사용	
         * @param	iMoveType        : 이동 Type, 2=사다리꼴 속도 Profile, 상대거리 이동
         *										 3=S-Curve 속도 Profile, 상대거리 이동
         *										 6=비대칭 사다리꼴 속도 Profile, 상대거리 이동
         *										 7=비대칭 S-Curve 속도 Profile, 상대거리 이동
         * @param   bPriority        : 우선순위 이동 사용 여부, true=사용, false=동시이동
         * @param	bClearOpt        : (OPTION=false) 이동 전과 후에 Encoder 값을 Clear하는 동작 사용 여부 (true:사용, false:미사용)
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int RMove(int iCoordinateID, bool[] pbMoveUse, double[] pdDistance, double[] pdVelocity,
                           int[] piAccelerate, int[] piDecelerate, int iMoveType, bool bPriority, bool bClearOpt = false);


        /**
         * 축 이동 (한개의 축 또는 여러 개(우선순위 무시)의 축에 대한 상대위치 이동) - 이동 명령 후 바로 return
         * 
         * @param   iCoordinateID    : 이동할 축 지정, -1 = All Axis
         * @param   *pbMoveUse       : 이동할 축 지정, iCoordinateID=-1일때만 적용, 동시 이동할 축에 true 지정
         * @param   *pdDistance      : 이동할 거리, iCoordinateID=-1이면 배열로 제공
         * @param   *pdVelocity      : 이동할 속도, iCoordinateID=-1이면 배열로 제공, 0.0 or null = 지정된 속도 사용
         * @param   *piAccelerate    : 이동할 가속도, iCoordinateID=-1이면 배열로 제공, 0 or null = 지정된 가속도 사용	
         * @param   *piDecelerate    : 이동할 감속도, iCoordinateID=-1이면 배열로 제공, 0 or null = 지정된 감속도 사용	
         * @param	iMoveType        : 이동 Type, 2=사다리꼴 속도 Profile, 상대거리 이동
         *										 3=S-Curve 속도 Profile, 상대거리 이동
         *										 6=비대칭 사다리꼴 속도 Profile, 상대거리 이동
         *										 7=비대칭 S-Curve 속도 Profile, 상대거리 이동
         * @param	bClearOpt        : (OPTION=false) 이동 전과 후에 Encoder 값을 Clear하는 동작 사용 여부 (true:사용, false:미사용)
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int StartRMove(int iCoordinateID, bool[] pbMoveUse, double[] pdDistance, double[] pdVelocity,
                                int[] piAccelerate, int[] piDecelerate, int iMoveType, bool bClearOpt = false);


        /**
         * 축 이동 (한개의 축에 대한 등속 이동, 등속 위치까진 가속 이동함) 
         * 
         * @param   iCoordinateID    : 이동할 축 지정, -1 허용안됨
         * @param   dVelocity        : 이동할 속도, 0.0 = 지정된 속도 사용
         * @param   iAccelerate      : 이동할 가속도, 0.0 = 지정된 가속도 사용
         * @param   bDir             : (OPTION=true) 이동할 방향, true:(+), false:(-), 생략하면 (+방향으로 이동
         *                             dVelocity에 값을 넣어주면 bDir은 생략해서 사용하면 된다.
         *                             이 경우는 dVelocity의 부호에 의해 이동 방향이 결정된다.
         *                             dVelocity에 0.0을 넣어 지정된 속도를 사용하는 경우는
         *                             bDir로 (+/-) 방향을 설정할 수 있다.
         *                             만약, dVelocity에 값을 넣은 경우 bDir을 설정을 하게 되면
         *                             지정된 dVelocuty, dAccelerate에 bDir이 반영되어 이동을 하게 된다.
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int VMove(int iCoordinateID, double dVelocity, int iAccelerate, bool bDir = true);


        /**
         * 축 이동 후 완료를 확인한다. (한개의 축 또는 여러 개의 축에 대한 완료 확인) 
         * 
         * @param   iCoordinateID    : 조회할 축 지정, -1 = All Axis
         * @param   *pbUse           : 이동 완료 확인할 축 지정, iCoordinateID=-1일때만 사용
         *                                                 iCoordinateID가 -1이 아니면 사용안함 (null)
         *                                                 배열구조에 사용하고자하는 축 위치에 true지정
         * @param   bSkipMode        : (OPTION=false) 위치 확인 대기, 이동 후 안정화 시간 지연 사용 여부
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  xx = TIMEOUT MOVE-TIME (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int Wait4Done(int iCoordinateID, bool[] pbUse, bool bSkipMode = false);


        /**
         * 축이 이동 완료되었는지 확인한다. (확인 후 바로 return한다.)
         * Motion 동작이 완료되었는지, Motion 동작과 In-Position되었는지 확인할 수 있는 기능을 제공한다.
         * 축 1개만 지정하여 확인할 수 있고, 모든 축을 다 확인할 수 있다.
         *
         * @param	iCoordinateID	: 좌표 Index (-1:All좌표)
         * @param	*pbDone			: 이동 완료 확인 결과 (true=이동 완료완료되었음, false=)이동완료 안되었음
         * @param	bMode			: (OPTION=false) 대기 종류, false=이동 및 속도이동 완료,
         *										 true=이동 및 속도이동 완료 & InPosition 범위내 이동 완료
         * @return	Error Code		: 0 = SUCCESS
         *							  그 외 = 타 함수 Return Error
         */
        int CheckDone(int iCoordinateID, out bool[] pbDone, bool bMode = false);


        /**
         * 직선보간 이동한다.
         * 
         * @param   iMaxAxNo         : 직선보간에 사용할 축 개수
         * @param   *pbUse           : 직선보간에 사용할 축 지정, 구성 축 개수만큼 배열 중 사용할 축에 true 지정
         * @param   iMaxPoint        : 직선보간 이동구간 개수
         * @param   *pdPosition      : 직선보간 이동구간 지정, (iMaxPoint X iMaxAxis)만큼 설정
         * @param   *pdVelocity      : 이동 시 속도, iMaxAxis만큼 설정
         * @param   *piAccelerate    : 이동 시 가속도, iMaxAxis만큼 설정
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int MoveSplineLine(int iMaxAxNo, bool[] pbUse, int iMaxPoint,
                                    double[] pdPosition, double[] pdVelocity, int[] piAccelerate);


        /**
         * 원호보간 이동한다.
         * 
         * @param   iMaxAxNo         : 원호보간에 사용할 축 개수
         * @param   *pbUse           : 원호보간에 사용할 축 지정, 구성 축 개수만큼 배열 중 사용할 축에 true 지정
         * @param   iMaxPoint        : 원호보간 이동구간 개수
         * @param   *pCenter         : 원호보간시 곡률 중심점 지정 (x, y), iMaxPoint만큼 설정
         * @param   *pdPosition      : 원호보간 이동구간 지정, (iMaxPoint X iMaxAxis)만큼 설정
         * @param   *pdVelocity      : 이동 시 속도, iMaxAxis만큼 설정
         * @param   *piAccelerate    : 이동 시 가속도, iMaxAxis만큼 설정
         * @param   *pbDir           : 원호보간 시 회전방향 설정, 1 = CIR_CCW (반시계방향), 0 = CIR_CW (시계방향),
         *											iMaxPoint만큼 설정
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int MoveSplineArc(int iMaxAxNo, bool[] pbUse, int iMaxPoint, double[] pCenter,
                                   double[] pdPosition, double[] pdVelocity, int[] piAccelerate, bool[] pbDir);


        /**
         * Jog Pitch에 의한 이동한다.
         * 
         * @param   iCoordinateID    : 이동할 축 지정, -1 허용안됨
         * @param   bDir             : 이동할 방향, true:(+), false:(-)
         * @param   dPitch           : (OPTION = 0.0) 이동할 거리, 0.0 = 지정된 Pitch거리 사용
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int JogMovePitch(int iCoordinateID, bool bDir, double dPitch = 0.0);


        /**
         * Jog Velocity에 의한 이동한다.
         * 
         * @param   iCoordinateID    : 이동할 축 지정, -1 허용안됨
         * @param   bDir             : 이동할 방향, true:(+), false:(-)
         * @param   dVelocity        : (OPTION = 0.0) 이동할 속도, 0.0 = 지정된 속도 사용
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int JogMoveVelocity(int iCoordinateID, bool bDir, double dVelocity = 0.0);


        /**
         * 축을 정지한다. (한개의 축 혹은 모든 축에 대한 정지)
         * 
         * @param   iCoordinateID    : 정지할 축 지정, -1 = All Axis
         * @param   *pbStatus      : (OPTION = null) 각 축의 Stop 상태, iCoordinateID=-1이면 배열로 구성
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int Stop(int iCoordinateID, out bool[] pbStatus);


        /**
         * 축을 등속이동에 대해 정지한다. (한개의 축 혹은 구성된 모든 축의 등속이동에 대한 정지)
         * 
         * @param   iCoordinateID    : 정지할 축 지정, -1 = All Axis
         * @param   *pbState         : (OPTION = null) 각 축의 VStop 상태, iCoordinateID=-1이면 배열로 구성
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int VStop(int iCoordinateID, out bool[] pbStatus);


        /**
         * 축을 비상정지한다. (한개의 축 혹은 구성된 모든 축에 대한 비상정지)
         * 
         * @param   iCoordinateID    : 정지할 축 지정, -1 = All Axis
         * @param   *pbStatus        : (OPTION = null) 각 축의 EStop 상태, iCoordinateID=-1이면 배열로 구성
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int EStop(int iCoordinateID, out bool[] pbStatus);


        /**
         * 축의 Servo를 On 한다. (한개의 축 혹은 구성된 모든 축에 대한 Servo On 수행)
         * 
         * @param   iCoordinateID    : Servo ON 할 축 지정, -1 = All Axis
         * @param   *pbStatus        : (OPTION = null) 각 축의 Servo ON 상태, iCoordinateID=-1이면 배열로 구성
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int ServoOn(int iCoordinateID, out bool[] pbStatus);


        /**
         * 축의 Servo를 Off 한다. (한개의 축 혹은 구성된 모든 축에 대한 Servo Off 수행)
         * 
         * @param   iCoordinateID    : Servo OFF 할 축 지정, -1 = All Axis
         * @param   *pbStatus        : (OPTION = null) 각 축의 Servo OFF 상태, iCoordinateID=-1이면 배열로 구성
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int ServoOff(int iCoordinateID, out bool[] pbStatus);


        /**
         * 축의 Home Sensor 상태를 읽는다. (한개의 축 혹은 구성된 모든 축에 대한 상태읽기)
         * 
         * @param   iCoordinateID    : Home Sensor 상태 읽을 축 지정, -1 = All Axis
         * @param   *pbStatus        : 각 축의 Home Sensor 상태, iCoordinateID=-1이면 배열로 구성
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int CheckHomeSensor(int iCoordinateID, out bool[] pbStatus);


        /**
         * 축의 Positive Sensor 상태를 읽는다. (한개의 축 혹은 구성된 모든 축에 대한 상태읽기)
         * 
         * @param   iCoordinateID    : Positive Sensor 상태 읽을 축 지정, -1 = All Axis
         * @param   *pbStatus        : 각 축의 Positive Sensor 상태, iCoordinateID=-1이면 배열로 구성
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int CheckPositiveSensor(int iCoordinateID, out bool[] pbStatus);


        /**
         * 축의 Negative Sensor 상태를 읽는다. (한개의 축 혹은 구성된 모든 축에 대한 상태읽기)
         * 
         * @param   iCoordinateID    : Negative Sensor 상태 읽을 축 지정, -1 = All Axis
         * @param   *pbStatus        : 각 축의 Negative Sensor 상태, iCoordinateID=-1이면 배열로 구성
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int CheckNegativeSensor(int iCoordinateID, out bool[] pbStatus);


        /**
         * 축의 Home Sensor에 대한 Event 및 Level을 설정한다. (한개의 축 혹은 구성된 모든 축에 대한 상태읽기)
         * 
         * @param   iCoordinateID    : Home Sensor Event/Limit 설정할 축 지정, -1 = All Axis
         * @param	*piLimit         : 동작할 Event, iCoordinateID=-1이면 배열로 구성
         * @param	*pbLevel         : 신호 Level, true=HIGH, FLASE=LOW, iCoordinateID=-1이면 배열로 구성
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int SetHomeSensorLimit(int iCoordinateID, int[] piLimit, bool[] pbLevel);


        /**
         * 축의 Home Sensor에 대한 Event를 설정한다. (한개의 축 혹은 구성된 모든 축에 대한 상태읽기)
         * 
         * @param   iCoordinateID    : Home Sensor Event/Limit 설정할 축 지정, -1 = All Axis
         * @param	*piLimit         : 동작할 Event, iCoordinateID=-1이면 배열로 구성
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int SetHomeSensorEvent(int iCoordinateID, int[] piLimit);


        /**
         * 축의 Home Sensor에 대한 Level을 설정한다. (한개의 축 혹은 구성된 모든 축에 대한 상태읽기)
         * 
         * @param   iCoordinateID    : Home Sensor Event/Limit 설정할 축 지정, -1 = All Axis
         * @param	*pbLevel         : 신호 Level, true=HIGH, FLASE=LOW, iCoordinateID=-1이면 배열로 구성
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int SetHomeSensorLevel(int iCoordinateID, bool[] pbLevel);


        /**
         * 축의 Positive Sensor에 대한 Event 및 Limit를 설정한다. (한개의 축 혹은 구성된 모든 축에 대한 상태읽기)
         * 
         * @param   iCoordinateID    : Positive Sensor Event/Limit 설정할 축 지정, -1 = All Axis
         * @param	*piLimit         : 동작할 Event, iCoordinateID=-1이면 배열로 구성
         * @param	*pbLevel         : 신호 Level, true=HIGH, FLASE=LOW, iCoordinateID=-1이면 배열로 구성
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int SetPositiveSensorLimit(int iCoordinateID, int[] piLimit, bool[] pbLevel);


        /**
         * 축의 Positive Sensor에 대한 Event를 설정한다. (한개의 축 혹은 구성된 모든 축에 대한 상태읽기)
         * 
         * @param   iCoordinateID    : Positive Sensor Event/Limit 설정할 축 지정, -1 = All Axis
         * @param	*piLimit         : 동작할 Event, iCoordinateID=-1이면 배열로 구성
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int SetPositiveSensorEvent(int iCoordinateID, int[] piLimit);


        /**
         * 축의 Positive Sensor에 대한 Limit를 설정한다. (한개의 축 혹은 구성된 모든 축에 대한 상태읽기)
         * 
         * @param   iCoordinateID    : Positive Sensor Event/Limit 설정할 축 지정, -1 = All Axis
         * @param	*pbLevel         : 신호 Level, true=HIGH, FLASE=LOW, iCoordinateID=-1이면 배열로 구성
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int SetPositiveSensorLevel(int iCoordinateID, bool[] pbLevel);


        /**
         * 축의 Negative Sensor에 대한 Event 및 Level를 설정한다. (한개의 축 혹은 구성된 모든 축에 대한 상태읽기)
         * 
         * @param   iCoordinateID    : Negative Sensor Event/Limit 설정할 축 지정, -1 = All Axis
         * @param	*piLimit         : 동작할 Event, iCoordinateID=-1이면 배열로 구성
         * @param	*pbLevel         : 신호 Level, true=HIGH, FLASE=LOW, iCoordinateID=-1이면 배열로 구성
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int SetNegativeSensorLimit(int iCoordinateID, int[] piLimit, bool[] pbLevel);


        /**
         * 축의 Negative Sensor에 대한 Event 설정한다. (한개의 축 혹은 구성된 모든 축에 대한 상태읽기)
         * 
         * @param   iCoordinateID    : Negative Sensor Event/Limit 설정할 축 지정, -1 = All Axis
         * @param	*piLimit         : 동작할 Event, iCoordinateID=-1이면 배열로 구성
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int SetNegativeSensorEvent(int iCoordinateID, int[] piLimit);


        /**
         * 축의 Negative Sensor에 대한 Level를 설정한다. (한개의 축 혹은 구성된 모든 축에 대한 상태읽기)
         * 
         * @param   iCoordinateID    : Negative Sensor Event/Limit 설정할 축 지정, -1 = All Axis
         * @param	*pbLevel         : 신호 Level, true=HIGH, FLASE=LOW, iCoordinateID=-1이면 배열로 구성
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int SetNegativeSensorLevel(int iCoordinateID, bool[] pbLevel);


        /**
         * 축의 상태(Source)를 읽는다. 
         * 
         * @param   iCoordinateID    : 상태 읽을 축 지정, -1 = All Axis
         * @param   *piSource        : 축 하나에 대한 상태 (Source), iCoordinateID=-1이면 배열로 구성
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int GetAxisStatus(int iCoordinateID, out int[] piSource);


        /**
         * 축의 상태(State)를 읽는다. 
         * 
         * @param   iCoordinateID    : 상태 읽을 축 지정, -1 = All Axis
         * @param   *piSource        : 축 하나에 대한 상태 (State), iCoordinateID=-1이면 배열로 구성
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int GetAxisState(int iCoordinateID, out int[] piState);


        /**
         * 축의 AMP Enable 상태를 읽는다. 
         * 
         * @param   iCoordinateID    : AMP 상태 읽을 축 지정, -1 = All Axis
         * @param   *pbEnable        : 축 하나에 대한 AMP상태, 혹은 모든 축의 AMP 상태 종합
         *							   모든축이 AMP Enable : true, 그외 : false
         * @param   *pbStatus        : (OPTION = null) 각 축의 AMP 상태, iCoordinateID=-1이면 배열로 구성
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int GetAmpEnable(int iCoordinateID, out bool[] pbEnable, out bool[] pbStatus);


        /**
         * 축의 AMP Fault 상태를 읽는다. 
         * 
         * @param   iCoordinateID    : AMP Fault 상태 읽을 축 지정, -1 = All Axis
         * @param   *pbFault         : 축 하나에 대한 AMP Fault상태, 혹은 모든 축의 AMP Fault 상태 종합,
         *							   축 하나라도 Fault 이면 : true, 축 모두 Fault 아니면 : false
         * @param   *pbStatus        : (OPTION = null) 각 축의 AMP Fault상태, iCoordinateID=-1이면 배열로 구성
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int GetAmpFault(int iCoordinateID, out bool[] pbFault, out bool[] pbStatus);


        /**
         * 축의 AMP Fault 상태를 Clear/Enable 한다. 
         * 
         * @param   iCoordinateID    : AMP Fault 상태 설정할 축 지정, -1 = All Axis
         * @param   *pbSet           : 각 축의 설정할 AMP Fault상태, true : Set, false : Reset, iCoordinateID=-1이면 배열로 구성
         * @param   *pbStatus        : (OPTION = null) 각 축의 AMP Fault Enable상태, iCoordinateID=-1이면 배열로 구성
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int SetAmpFault(int iCoordinateID, bool[] pbSet, bool[] pbStatus = null);


        /**
         * 원점 복귀 시 Encoder의 C상 펄스 이용 여부를 읽는다.
         *
         * @param   iCoordinateID	: 초기화할 축 지정, -1 = All Axis
         * @param	*pbIndexReq		: C상 펄스 사용 여부, true =Home Sensor와 Encoder의 Index Pulse를 동시 검출,
         *												  false=Home Sensor만 검출
         *												  iCoordinateID=-1이면 배열로 구성
         * @return	Error Code		: 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MOTIONLIB)
         *							  그 외 = 타 함수 Return Error
         */
        int GetIndexRequired(int iCoordinateID, out bool[] pbIndexReq);


        /**
         * 원점 복귀 시 Encoder의 C상 펄스 이용 여부를 설정한다.
         *
         * @param   iCoordinateID	: 초기화할 축 지정, -1 = All Axis
         * @param	*pbIndexReq		: C상 펄스 사용 여부, true =Home Sensor와 Encoder의 Index Pulse를 동시 검출,
         *												  false=Home Sensor만 검출
         *												  iCoordinateID=-1이면 배열로 구성
         * @return	Error Code		: 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MOTIONLIB)
         *							  그 외 = 타 함수 Return Error
         */
        int SetIndexRequired(int iCoordinateID, bool[] pbIndexReq);


        /**
         * 축의 상태를 초기화 한다. (한개의 축 혹은 구성된 모든 축에 대해 초기화)
         *  Clear Status & Clear Frames
         * 
         * @param   iCoordinateID    : 초기화할 축 지정, -1 = All Axis
         * @param   *pbStatus        : (OPTION = null) 각 축의 초기화 상태, iCoordinateID=-1이면 배열로 구성
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int ClearAxis(int iCoordinateID, out bool[] pbStatus);


        /**
         * 원점복귀 Step을 설정한다. (한개의 축)
         * 
         * @param   iCoordinateID    : 초기화할 축 지정, -1 = 허용안함
         * @param   iStep            : 설정값 (0:시작, 999:오류, 1000:완료, 그외:동작중)
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID ORIGIN STEP (<0) (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int SetOriginStep(int iCoordinateID, int iStep);


        /**
         * 원점복귀 Step을 읽는다. (한개의 축)
         * 
         * @param   iCoordinateID    : 초기화할 축 지정, -1 = 허용안함
         * @param   *piStep          : Step값 (0:시작, 999:오류, 1000:완료, 그외:동작중)
         * @param   *piPrevStep      : (OPTION=null) Error전 Step값 (0:시작, 999:오류, 1000:완료, 그외:동작중)
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  xx = INVALID POINTER (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int GetOriginStep(int iCoordinateID, out int[] piStep, out int[] piPrevStep);


        /**
         * 원점복귀 Error를 초기화한다. (한개의 축)
         * 
         * @param   iCoordinateID    : 초기화할 축 지정, -1 = 허용안함
         * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int ClearOriginError(int iCoordinateID);


        /**
         * 원점복귀 Error를 읽는다. (한개의 축)
         * 
         * @param   iCoordinateID    : 초기화할 축 지정, -1 = 허용안함
         * @param   *piError         : 발생한 오류 Code
        * @return	Error Code		 : 0 = SUCCESS
         *							  xx = INVALID AXIS ID (MULTIAXES)
         *							  그 외 = 타 함수 Return Error
         */
        int GetOriginError(int iCoordinateID, out int[] piError);

	/**
	 * 지정 축이 지정된 위치를 지날 때 지정 IO를 출력한다.
	 *
	 * @param   iCoordinateID    : 축 지정, -1 = 허용안함
	 * @param	siPosNum		: 위치 번호, 1 ~ 10
	 * @param	siIONum			: I/O 번호, 양의정수=ON, 음의정수=OFF
	 * @param	dPosition		: 지정 축의 위치값
	 * @param	bEncFlag		: Encoder Flag, false=내부위치 Counter 사용, true=외부 Encoder 사용
	 * @return	Error Code		: 0 = SUCCESS
	 *							  xx = INVALID POSITION IO NUMBER (MOTIONLIB)
	 *							  xx = INVALID AXIS ID (MOTIONLIB)
	 *							  그 외 = 타 함수 Return Error
	 */
	int PositionIoOnoff(int iCoordinateID, int iPosNum, int iIONum, double dPosition, int nEncFlag);

        /**
         * PositionIoOnOff()로 설정된 것을 해제한다.
         *
         * @param   iCoordinateID    : 축 지정, -1 = 허용안함
         * @param	siPosNum		: (OPTION=0) 위치 번호, 1 ~ 10, 0=모든 위치 해제
         * @return	Error Code		: 0 = SUCCESS
         *							  xx = INVALID POSITION IO NUMBER (MOTIONLIB)
         *							  그 외 = 타 함수 Return Error
         */
        int PositionIOClear(int iCoordinateID, int iPosNum = 0);

        int PositionCompare(int iCoordinateID, int iIndexNum, int iBitNo, double dPosition, bool bOutOn);

    }
}
