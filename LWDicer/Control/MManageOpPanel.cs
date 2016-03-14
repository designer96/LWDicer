using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

using static LWDicer.Control.DEF_Common;
using static LWDicer.Control.DEF_Error;
using static LWDicer.Control.DEF_ManageOpPanel;
using static LWDicer.Control.DEF_OpPanel;
using static LWDicer.Control.DEF_Thread;
using static LWDicer.Control.DEF_ManageOpPanel.ETowerLampMode;

namespace LWDicer.Control
{
    class DEF_ManageOpPanel
    {
        // Touch Panel 앞/뒷면 ID define
        public const int DEF_MNGOPPANEL_NONE_PANEL_ID = 0;
        public const int DEF_MNGOPPANEL_FRONT_PANEL_ID = 1;
        public const int DEF_MNGOPPANEL_BACK_PANEL_ID = 2;

        // Jog 관련 define
        public const int DEF_MNGOPPANEL_JOG_NO_USE = -1;

        public const double DEF_MNGOPPANEL_BLINK_RATE = 0.5;

        public enum ETowerLampMode
        {
            TOWER_STEPSTOP,           
            TOWER_START,              
            TOWER_RUN,                
            TOWER_STEPSTOP_ING,       
            TOWER_ERRORSTOP_ING,      
            TOWER_CYCLESTOP_ING,      
            TOWER_PARTSEMPTY,         
            TOWER_ERRORSTOP_NOBUZZER, 
            TOWER_PARTSEMPTY_NOBUZZER,
            TOWER_OP_CALL,            
            TOWER_RUN_PANEL_NO_EXIST, 
            TOWER_RUN_TRAFFIC_JAM,    
            TOWER_ESTOP_PRESSED,      
            TOWER_NCMC_BUZZER              
        }

    }

    public class MManageOpPanel : MObject
    {
        // Blink Rate
        double m_dBlinkRate;

        EOperationMode m_eAutoManual;
        ERunMode m_eOpMode;

        // SafeSensor Check 여부
        bool m_bSafeSensorUse;

        // Jog로 이동할 Motion에 대한 정보 Index
        int m_iJogIndex;
        int m_iJogIndexExtra;

        // Jog Key 이전 값 저장 용 변수
        int m_iPrevJogVal_X;
        int m_iPrevJogVal_Y;
        int m_iPrevJogVal_T;
        int m_iPrevJogVal_Z;

        bool m_bPrevDir_X;
        bool m_bPrevDir_Y;
        bool m_bPrevDir_T;
        bool m_bPrevDir_Z;

        // Tower Lamp Blink 동작을 위한 Timer
        MTickTimer m_BlinkTimer;

        // Blink Interval 계산을 위한 변수
        bool m_bBlinkState;

        // IO Check Dialog
        bool m_bIOCheck;

        bool m_bTrafficJam;
        bool m_bNoPanel;

        // Object Parameter
        MOpPanel m_OpPanel;

        public MManageOpPanel(CObjectInfo objInfo, MOpPanel pOpPanel)
            : base(objInfo)
        {
            m_OpPanel = pOpPanel;

            // Blink Rate 
            m_dBlinkRate = DEF_MNGOPPANEL_BLINK_RATE;

            // Jog로 이동할 Motion에 대한 정보 Index 
            m_iJogIndex = DEF_MNGOPPANEL_JOG_NO_USE;
            m_iJogIndexExtra = DEF_OPPANEL_NO_JOGKEY;

            m_bSafeSensorUse = false;

            // Blink Interval 계산을 위한 변수 
            m_bBlinkState = false;

            // Blink를 위한 Timer 시작 
            m_BlinkTimer.StartTimer();

            m_bIOCheck = false;

            // Jog Key 이전 값 저장 용 변수 
            m_iPrevJogVal_X = JOG_KEY_NON;
            m_iPrevJogVal_Y = JOG_KEY_NON;
            m_iPrevJogVal_T = JOG_KEY_NON;
            m_iPrevJogVal_Z = JOG_KEY_NON;

            m_bPrevDir_X = JOG_DIR_POS;
            m_bPrevDir_Y = JOG_DIR_POS;
            m_bPrevDir_T = JOG_DIR_POS;
            m_bPrevDir_Z = JOG_DIR_POS;

            m_bTrafficJam = false;

        }

        int Initialize()
        {
            return SUCCESS;
        }

        /**
         * System의 Auto / Manual Mode를 반영한다.
         * Mode 변환이 생길때마다 TrsAutoManager에 의해 각 Control들의 Mode에 반영한다.
         *
         * @param	EOperationMode eAutoManual (반영하고자 하는 Auto/Manual Mode)
         * @return	void
         */
        void SetAutoManual(EOperationMode eAutoManual)
        {
            m_eAutoManual = eAutoManual;
        }

        /**
         * System의 운전 Mode를 반영한다.
         * 화면에서 운전 Mode 변경 시 각 Control들의 운전 Mode에 반영한다.
         *
         * @param	ERunMode eOpMode (반영하고자 하는 운전 Mode)
         * @return	void
         */
        void SetOpMode(ERunMode eOpMode)
        {
            m_eOpMode = eOpMode;
        }

        /**
        * 안전센서 감지 여부를 설정한다. 
        *
        * @param	bSafeSensorUse : 설정할 안전센서 사용 여부 (TURE:사용, false:미사용)
*/
        void SetSafeSensorUse(bool bSafeSensorUse)
        {
            m_bSafeSensorUse = bSafeSensorUse;
        }

        /**
        * 안전센서 감지 여부를 Get한다. 
        *
        * @param	*pbSafeSensorUse : 설정할 안전센서 사용 여부 (TURE:사용, false:미사용)
*/
        void GetSafeSensorUse(out bool pbSafeSensorUse)
        {
            pbSafeSensorUse = m_bSafeSensorUse;
        }

        /**
         * Motion 이동 속도 Mode를 설정한다.
         *
         * @param	rgdVelocity[] : 설정할 Motion 속도 (배열 Index 순서는 MMC 축 ID 순서)
         */
        void SetVelocityMode(double[/*DEF_MAX_MOTION_AXIS*/] rgdVelocity)
        {
            m_OpPanel.SetVelocityMode(rgdVelocity);
        }

        /**
        * 자동운전 전의 운전 가능 상태를 읽는다.
        */
        int CheckBeforeAutoRun()
        {
#if SIMULATION_MOTION
            return SUCCESS;
#endif
            bool bStatus;
            bool bEmptyAll, bEmptyPart;
            bool bStatus1 = false;
            bool bStatus2 = false;
            int iResult = SUCCESS;

            // 1. 원점복귀 여부 확인 
            bool[] bOriginSts;
            if (m_OpPanel.CheckAllOrigin(out bOriginSts) == false)
                return GenerateErrorCode(ERR_MNGOPPANEL_NOT_ALL_ORIGIN);

            // 2. 초기화 여부 확인 
            bool[] bInitSts;
            if (m_OpPanel.CheckAllInit(out bInitSts) == false)
                return GenerateErrorCode(ERR_MNGOPPANEL_NOT_ALL_INIT);

            // 3. Tool CP 확인 
            //	if ((iResult = m_OpPanel.GetCPTripStatus(out bStatus);
            //		return iResult;
            //	if (bStatus == true)
            //		return GenerateErrorCode(ERR_MNGOPPANEL_CP_TRIP);

            // 4. Silicone 잔량 확인 
            if (m_eOpMode != ERunMode.MODE_DRY_RUN)
            {
                iResult = CheckSiliconeRemain(out bEmptyAll, out bEmptyPart);
                if (iResult != SUCCESS) return iResult;

                if (bEmptyAll == true)
                    return GenerateErrorCode(ERR_MNGOPPANEL_SILICONE_EMPTY_ALL);
            }

            // 5. Door Open 확인 
            if (m_bSafeSensorUse == true)
            {
                iResult = GetDoorSWStatus(out bStatus);
                if (iResult != SUCCESS) return iResult;
                if (bStatus == true)
                    return GenerateErrorCode(ERR_MNGOPPANEL_DOOR_OPEN);
            }

            // 6. AMP Fault 상태 확인 
            iResult = GetMotorAmpFaultStatus(out bStatus);
            if (iResult != SUCCESS) return iResult;
            if (bStatus == true)
                return GenerateErrorCode(ERR_MNGOPPANEL_AMP_FAULT);

            // 7. Air 확인 
            iResult = m_OpPanel.GetAirErrorStatus(out bStatus);
            if (iResult != SUCCESS) return iResult;
            if (bStatus == true)
                return GenerateErrorCode(ERR_MNGOPPANEL_MAIN_AIR_ERROR);

            // 8. Vacuum 확인 
            iResult = m_OpPanel.GetVacuumErrorStatus(out bStatus);
            if (iResult != SUCCESS) return iResult;
            if (bStatus == true)
                return GenerateErrorCode(ERR_MNGOPPANEL_MAIN_VACCUM_ERROR);

            // 9. EFD 확인 
            iResult = m_OpPanel.GetEFDErrorStatus(out bStatus);
            if (iResult != SUCCESS) return iResult;
            if (bStatus == true)
                return GenerateErrorCode(ERR_MNGOPPANEL_EFD_READY_ON_ERROR);

            // 10. DC POWER 확인 
            iResult = m_OpPanel.GetDcPWErrorStatus(out bStatus);
            if (iResult != SUCCESS) return iResult;
            if (bStatus == true)
                return GenerateErrorCode(ERR_MNGOPPANEL_DC_POWER_ON_ERROR);

#if DEF_NEW_CLEAN_SYSTEM
            // 9. N2 확인 
            //	iResult = m_OpPanel.GetN2ErrorStatus(out bStatus);
            //	if(iResult) return iResult;
            //	if (bStatus == true)
            //		return GenerateErrorCode(ERR_MNGOPPANEL_MAIN_N2_ERROR);

            // 10. Cleaner Detect 확인 
            //	iResult = m_OpPanel.GetCleanerDetect1ErrorStatus(out bStatus);
            //	if(iResult) return iResult;
            //	if (bStatus == true)
            //		return GenerateErrorCode(ERR_MNGOPPANEL_CLEANER_DETECT1_ERROR);

            //	iResult = m_OpPanel.GetCleanerDetect2ErrorStatus(out bStatus);
            //	if(iResult) return iResult;
            //	if (bStatus == true)
            //		return GenerateErrorCode(ERR_MNGOPPANEL_CLEANER_DETECT2_ERROR);
#endif //DEF_NEW_CLEAN_SYSTEM

            return 0;
        }

        /**
        * 자동운전 중의 운전 가능 상태를 읽는다.
        */
        int CheckAutoRun(out bool bEmptyPart)
        {
#if SIMULATION_MOTION
            bEmptyPart = false;
            return SUCCESS;
#endif
            bool bStatus;
            bool bEmptyAll;
            bool bStatus1 = false;
            bool bStatus2 = false;
            int iResult = SUCCESS;
            bEmptyPart = false;

            // 3. Tool CP 확인 
            //	if ((iResult = m_OpPanel.GetCPTripStatus(out bStatus);
            //		return iResult;
            //	if (bStatus == true)
            //		return GenerateErrorCode(ERR_MNGOPPANEL_CP_TRIP);

            // 4. Silicone 잔량 확인 
            if (m_eOpMode != ERunMode.MODE_DRY_RUN)
            {
                iResult = CheckSiliconeRemain(out bEmptyAll, out bEmptyPart);
                if (iResult != SUCCESS) return iResult;
                if (bEmptyAll == true)
                    return GenerateErrorCode(ERR_MNGOPPANEL_SILICONE_EMPTY_ALL);

            }

            // 5. Door Open 확인 
            if (m_bSafeSensorUse == true)
            {
                iResult = GetDoorSWStatus(out bStatus);
                if (iResult != SUCCESS) return iResult;
                if (bStatus == true)
                {
                    EStopAllAxis();
                    return GenerateErrorCode(ERR_MNGOPPANEL_DOOR_OPEN);
                }
            }

#if DEF_USE_AREA_SENSOR
            // 5.1 Area Sensor 확인 
            if (m_bSafeSensorUse == true)
            {
                iResult = GetAreaSWStatus(&bStatus1);
                if (iResult != SUCCESS) return iResult;

                if (false == bStatus1)
                {
                    EStopAllAxis();
                    return GenerateErrorCode(ERR_MNGOPPANEL_FRONT_BACK_AREA_SENSOR_DETECTED_ERROR);
                }

            }
#endif

            // 6. AMP Fault 상태 확인 
            iResult = GetMotorAmpFaultStatus(out bStatus);
            if (iResult != SUCCESS) return iResult;
            if (bStatus == true)
                return GenerateErrorCode(ERR_MNGOPPANEL_AMP_FAULT);

            // 7. Air 확인 
            iResult = m_OpPanel.GetAirErrorStatus(out bStatus);
            if (iResult != SUCCESS) return iResult;
            if (bStatus == true)
                return GenerateErrorCode(ERR_MNGOPPANEL_MAIN_AIR_ERROR);

            // 8. Vacuum 확인 
            iResult = m_OpPanel.GetVacuumErrorStatus(out bStatus);
            if (iResult != SUCCESS) return iResult;
            if (bStatus == true)
                return GenerateErrorCode(ERR_MNGOPPANEL_MAIN_VACCUM_ERROR);

#if DEF_NEW_CLEAN_SYSTEM
            // 9. N2 확인 
            //	iResult = m_OpPanel.GetN2ErrorStatus(out bStatus);
            //	if(iResult) return iResult;
            //	if (bStatus == true)
            //		return GenerateErrorCode(ERR_MNGOPPANEL_MAIN_N2_ERROR);
#endif
            return 0;
        }

        /**
        * Start Switch의 눌린 상태 읽어온다.
        *
        * @param	*pbStatus : Start Switch 눌린 상태 (0:OFF , 1:FRONT , 2:BACK)
        */
        int GetStartSWStatus(out bool pbStatus)
        {
            

            int iResult = SUCCESS;

            // Start Switch 눌린 상태 읽기
            iResult = m_OpPanel.GetStartButtonStatus(out pbStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        /**
        * Stop Switch의 눌린 상태 읽어온다.
        *
        * @param	*pbStatus : Stop Switch 눌린 상태 (0:OFF , 1:FRONT , 2:BACK)
        */
        int GetStopSWStatus(out bool pbStatus)
        {
            

            int iResult = SUCCESS;

            // Stop Switch 눌린 상태 읽기
            iResult = m_OpPanel.GetStopButtonStatus(out pbStatus);
            if (iResult != SUCCESS) return iResult;

            if (pbStatus == true)
            {
                //		StopAllReturnOrigin();
                //		StopAllAxis();
            }
            return SUCCESS;
        }

        /**
        * E-Stop Switch의 눌린 상태 읽어온다.
        *
        * @param	*pbStatus : E-Stop Switch 눌린 상태 (true:ON, false:OFF)
        */
        int GetEStopSWStatus(out bool pbStatus)
        {
            

            int iResult = SUCCESS;

            // E-Stop Switch 눌린 상태 읽기
            iResult = m_OpPanel.GetEStopButtonStatus(out pbStatus);
            if (iResult != SUCCESS) return iResult;

            if (pbStatus == true)
            {
                //		StopAllReturnOrigin();
                //		StopAllAxis();
            }
            return SUCCESS;
        }

        /**
        * Reset Switch의 눌린 상태 읽어온다.
        *
        * @param	*pbStatus : Reset Switch 눌린 상태 (0:OFF , 1:FRONT , 2:BACK)
        */
        int GetResetSWStatus(out bool pbStatus)
        {
            

            int iResult = SUCCESS;

            // Reset Switch 눌린 상태 읽기
            iResult = m_OpPanel.GetResetButtonStatus(out pbStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        /**
        * Door의 상태를 읽어온다.
        *
        * @param	*pbStatus : Door 상태 (true:CLOSE, false:OPEN)
        */
        int GetDoorSWStatus(out bool pbStatus)
        {
            

            int iResult = SUCCESS;

            // Door 열린 상태 읽기
            iResult = m_OpPanel.GetSafeDoorStatus(out pbStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        /**
        * Area의 상태를 읽어온다.
        *
        * @param	*pbStatus : Area 상태 (true:CLOSE, false:OPEN)
        */
        int GetAreaSWStatus(out bool pbStatus)
        {
            int iResult = SUCCESS;


            iResult = m_OpPanel.GetAreaFrontBackStatus(out pbStatus);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }


        /**
        * Motor AMP Fault 상태를 읽는다.
        *
        * @param	*pbStatus : Motor AMP Fault의 상태 (true : Fault, false : Normal)
        */
        int GetMotorAmpFaultStatus(out bool pbStatus)
        {
            return m_OpPanel.GetMotorAmpFaultStatus(out pbStatus);
        }

        /**
         * Motion Power Relay On/Off 를 설정한다.
         *
         * @param	bStatus : Motion Power Relay의 동작 (true : ON, false : OFF)
         */
        int SetMotionPowerRelayStatus(bool bStatus)
        {
            return m_OpPanel.SetMotionPowerRelayStatus(bStatus);
        }

        /**
         * OpPanel의 Switch 및 Tower Lamp의 On/Off를 지시한다.
         *
         * @param	iState : On/Off Mode
         *		   Mode                     Start	Step	TowerR	TowerY	TowerG	Buzzer
         *		1:Step Stop 완료 상태		  X		 O		   O	  X		  X		  X
         *		2:Start(Run Ready) 상태		  O		 X		   X	  O		  O		  X
         *		3:Run 상태					  O		 X		   X	  X		  O		  X
         *		4:Step Stop 진행 상태		  X		 O		   B	  X		  X		  X
         *		5:Error Stop 진행 상태		  O		 X		   B	  X		  B		  O
         *		6:Cycle Stop 진행 상태		  O		 X		   X	  X		  B		  X
         *		7:Parts Empty 상태			  O		 X		   X	  B		  B		  B
         *		  (Operator Call 상태)
         *		8:Error Stop 진행 상태		  O		 X		   B	  X		  B		  X
         *        (Buzzer No)
         *		9:Parts Empty 상태			  O		 X		   X	  B		  B		  B
         *		  (Buzzer No)
         *		10: 정체 상태				  0		 X		   O	  O		  O		  X
         */
        int SetOpPanel(ETowerLampMode towerLampMode)
        {
            int iResult = SUCCESS;

            if (m_bIOCheck) return iResult;

            // Blink Interval 계산 
            if (m_BlinkTimer.MoreThan(m_dBlinkRate * 1000) == true)
            {
                // Blink 반전 
                m_bBlinkState = !m_bBlinkState;
                // Timer 재기동 
                m_BlinkTimer.StartTimer();
            }

            iResult = m_OpPanel.SetResetLamp(false);
            if (iResult != SUCCESS) return iResult;

            // Switch LED 및 Tower Lamp의 On/Off Mode에 따라 동작 
            switch (towerLampMode)
            {
                // Step Stop 진행 상태 
                case TOWER_STEPSTOP_ING:
                    // Start Switch LED Off 
                    iResult = m_OpPanel.SetStartLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Step Stop Switch LED On 
                    iResult = m_OpPanel.SetStopLamp(true);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Red Lamp Blink 
                    iResult = m_OpPanel.SetTowerRedLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Yellow Lamp Off 
                    iResult = m_OpPanel.SetTowerYellowLamp(true);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Green Lamp Off 
                    iResult = m_OpPanel.SetTowerGreenLamp(true);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Buzzer Off 
                    iResult = m_OpPanel.SetBuzzerStatus(DEF_OPPANEL_BUZZER_ALL, false);
                    if (iResult != SUCCESS) return iResult;
                    break;

                // Step Stop 완료 상태 
                case TOWER_STEPSTOP:
                    // Start Switch LED Off 
                    iResult = m_OpPanel.SetStartLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Step Stop Switch LED On 
                    iResult = m_OpPanel.SetStopLamp(true);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Red Lamp On 
                    iResult = m_OpPanel.SetTowerRedLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Yellow Lamp Off 
                    iResult = m_OpPanel.SetTowerYellowLamp(true);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Green Lamp Off 
                    iResult = m_OpPanel.SetTowerGreenLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Buzzer Off 
                    iResult = m_OpPanel.SetBuzzerStatus(DEF_OPPANEL_BUZZER_ALL, false);
                    if (iResult != SUCCESS) return iResult;
                    break;

                // Cycle Stop 진행 상태 
                case TOWER_CYCLESTOP_ING:
                    // Start Switch LED On 
                    iResult = m_OpPanel.SetStartLamp(true);
                    if (iResult != SUCCESS) return iResult;
                    // Step Stop Switch LED Off 
                    iResult = m_OpPanel.SetStopLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Red Lamp Off 
                    iResult = m_OpPanel.SetTowerRedLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Yellow Lamp Off 
                    iResult = m_OpPanel.SetTowerYellowLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Green Lamp Blink 
                    iResult = m_OpPanel.SetTowerGreenLamp(m_bBlinkState);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Buzzer Off 
                    iResult = m_OpPanel.SetBuzzerStatus(DEF_OPPANEL_BUZZER_ALL, false);
                    if (iResult != SUCCESS) return iResult;
                    break;

                // Error Stop 진행 상태 
                case TOWER_ERRORSTOP_ING:
                    // Start Switch LED On 
                    iResult = m_OpPanel.SetStartLamp(true);
                    if (iResult != SUCCESS) return iResult;
                    // Step Stop Switch LED Off 
                    iResult = m_OpPanel.SetStopLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Red Lamp Blink 
                    iResult = m_OpPanel.SetTowerRedLamp(true);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Yellow Lamp Off 
                    iResult = m_OpPanel.SetTowerYellowLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Green Lamp Blink 
                    iResult = m_OpPanel.SetTowerGreenLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Buzzer On 
                    iResult = m_OpPanel.SetBuzzerStatus(DEF_OPPANEL_BUZZER_K2, true);
                    if (iResult != SUCCESS) return iResult;
                    break;

                // Error Stop 확인 상태 
                case TOWER_ERRORSTOP_NOBUZZER:
                    // Start Switch LED On 
                    iResult = m_OpPanel.SetStartLamp(true);
                    if (iResult != SUCCESS) return iResult;
                    // Step Stop Switch LED Off 
                    iResult = m_OpPanel.SetStopLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Red Lamp Blink 
                    iResult = m_OpPanel.SetTowerRedLamp(true);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Yellow Lamp Off 
                    iResult = m_OpPanel.SetTowerYellowLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Green Lamp Blink 
                    iResult = m_OpPanel.SetTowerGreenLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Buzzer On 
                    iResult = m_OpPanel.SetBuzzerStatus(DEF_OPPANEL_BUZZER_ALL, false);
                    if (iResult != SUCCESS) return iResult;
                    break;

                // Start (Run Ready) 상태 
                case TOWER_START:
                    // Start Switch LED On 
                    iResult = m_OpPanel.SetStartLamp(true);
                    if (iResult != SUCCESS) return iResult;
                    // Step Stop Switch LED Off 
                    iResult = m_OpPanel.SetStopLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Red Lamp Off 
                    iResult = m_OpPanel.SetTowerRedLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Yellow Lamp On 
                    iResult = m_OpPanel.SetTowerYellowLamp(true);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Green Lamp On 
                    iResult = m_OpPanel.SetTowerGreenLamp(true);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Buzzer Off 
                    iResult = m_OpPanel.SetBuzzerStatus(DEF_OPPANEL_BUZZER_ALL, false);
                    if (iResult != SUCCESS) return iResult;
                    break;

                // Run 상태 
                case TOWER_RUN:
                    // Start Switch LED On 
                    iResult = m_OpPanel.SetStartLamp(true);
                    if (iResult != SUCCESS) return iResult;
                    // Step Stop Switch LED Off 
                    iResult = m_OpPanel.SetStopLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Red Lamp Off 
                    iResult = m_OpPanel.SetTowerRedLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Yellow Lamp Off 
                    iResult = m_OpPanel.SetTowerYellowLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Green Lamp On 
                    iResult = m_OpPanel.SetTowerGreenLamp(true);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Buzzer Off 
                    iResult = m_OpPanel.SetBuzzerStatus(DEF_OPPANEL_BUZZER_ALL, false);
                    if (iResult != SUCCESS) return iResult;
                    break;

                // Run 상태 - but Panel non exist 
                case TOWER_RUN_PANEL_NO_EXIST:
                    // Start Switch LED On 
                    iResult = m_OpPanel.SetStartLamp(true);
                    if (iResult != SUCCESS) return iResult;
                    // Step Stop Switch LED Off 
                    iResult = m_OpPanel.SetStopLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Red Lamp Off 
                    iResult = m_OpPanel.SetTowerRedLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Yellow Lamp Blink 
                    iResult = m_OpPanel.SetTowerYellowLamp(true);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Green Lamp On 
                    iResult = m_OpPanel.SetTowerGreenLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Buzzer Off 
                    iResult = m_OpPanel.SetBuzzerStatus(DEF_OPPANEL_BUZZER_ALL, false);
                    if (iResult != SUCCESS) return iResult;
                    break;

                // Run 상태 - but Panel Traffic Jam 
                case TOWER_RUN_TRAFFIC_JAM:
                    // Start Switch LED On 
                    iResult = m_OpPanel.SetStartLamp(true);
                    if (iResult != SUCCESS) return iResult;
                    // Step Stop Switch LED Off 
                    iResult = m_OpPanel.SetStopLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Red Lamp Off 
                    iResult = m_OpPanel.SetTowerRedLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Yellow Lamp Blink 
                    iResult = m_OpPanel.SetTowerYellowLamp(true);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Green Lamp On 
                    iResult = m_OpPanel.SetTowerGreenLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Buzzer Off 
                    iResult = m_OpPanel.SetBuzzerStatus(DEF_OPPANEL_BUZZER_ALL, false);
                    if (iResult != SUCCESS) return iResult;
                    break;

                // Run 상태 - but Panel Traffic Jam 
                case TOWER_ESTOP_PRESSED:
                    // Start Switch LED On 
                    iResult = m_OpPanel.SetStartLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Step Stop Switch LED Off 
                    iResult = m_OpPanel.SetStopLamp(true);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Red Lamp Off 
                    iResult = m_OpPanel.SetTowerRedLamp(true);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Yellow Lamp Blink 
                    iResult = m_OpPanel.SetTowerYellowLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Green Lamp On 
                    iResult = m_OpPanel.SetTowerGreenLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Buzzer Off 
                    iResult = m_OpPanel.SetBuzzerStatus(DEF_OPPANEL_BUZZER_ALL, false);
                    if (iResult != SUCCESS) return iResult;
                    break;

                // Parts Empty 상태 
                case TOWER_PARTSEMPTY:
                    // Start Switch LED On 
                    // 		iResult = m_OpPanel.SetStartLamp(true);
                    // 		if(iResult) return iResult;
                    // Step Stop Switch LED Off 
                    // 		iResult = m_OpPanel.SetStopLamp(false);
                    // 		if(iResult) return iResult;
                    // Tower Lamp Red Lamp Off 
                    iResult = m_OpPanel.SetTowerRedLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Yellow Lamp Blink 
                    iResult = m_OpPanel.SetTowerYellowLamp(m_bBlinkState);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Green Lamp On 
                    iResult = m_OpPanel.SetTowerGreenLamp(true);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Buzzer On 
                    iResult = m_OpPanel.SetBuzzerStatus(DEF_OPPANEL_BUZZER_K4, true);
                    if (iResult != SUCCESS) return iResult;
                    break;

                // Parts Empty 상태 
                case TOWER_PARTSEMPTY_NOBUZZER:
                    // Start Switch LED On 
                    // 		iResult = m_OpPanel.SetStartLamp(true);
                    // 		if(iResult) return iResult;
                    // Step Stop Switch LED Off 
                    // 		iResult = m_OpPanel.SetStopLamp(false);
                    // 		if(iResult) return iResult;
                    // Tower Lamp Red Lamp Off 
                    iResult = m_OpPanel.SetTowerRedLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Yellow Lamp Blink 
                    iResult = m_OpPanel.SetTowerYellowLamp(m_bBlinkState);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Green Lamp On 
                    iResult = m_OpPanel.SetTowerGreenLamp(true);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Buzzer On 
                    iResult = m_OpPanel.SetBuzzerStatus(DEF_OPPANEL_BUZZER_ALL, false);
                    if (iResult != SUCCESS) return iResult;
                    break;

                // Line Controller로 부터 Op Call이 온 상태 
                case TOWER_OP_CALL:
                    // Start Switch LED On 
                    iResult = m_OpPanel.SetStartLamp(true);
                    if (iResult != SUCCESS) return iResult;
                    // Step Stop Switch LED Off 
                    iResult = m_OpPanel.SetStopLamp(false);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Red Lamp Blink 
                    iResult = m_OpPanel.SetTowerRedLamp(m_bBlinkState);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Yellow Lamp Blink 
                    iResult = m_OpPanel.SetTowerYellowLamp(m_bBlinkState);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Green Lamp Blink 
                    iResult = m_OpPanel.SetTowerGreenLamp(m_bBlinkState);
                    if (iResult != SUCCESS) return iResult;
                    // Tower Lamp Buzzer On 
                    iResult = m_OpPanel.SetBuzzerStatus(DEF_OPPANEL_BUZZER_K2, true);
                    if (iResult != SUCCESS) return iResult;
                    break;

                /** NSMC 상태 */
                case TOWER_NCMC_BUZZER:
                    if ((iResult = m_OpPanel.SetStartLamp(true)) != SUCCESS) return iResult;
                    if ((iResult = m_OpPanel.SetStopLamp(false)) != SUCCESS) return iResult;
                    if ((iResult = m_OpPanel.SetTowerRedLamp(m_bBlinkState)) != SUCCESS) return iResult;
                    if ((iResult = m_OpPanel.SetTowerYellowLamp(m_bBlinkState)) != SUCCESS) return iResult;
                    if ((iResult = m_OpPanel.SetTowerGreenLamp(m_bBlinkState)) != SUCCESS) return iResult;
                    if ((iResult = m_OpPanel.SetBuzzerStatus(DEF_OPPANEL_BUZZER_K2, true)) != SUCCESS) return iResult;
                    break;
                default:
                    return GenerateErrorCode(ERR_MNGOPPANEL_INVALID_SET_OPPANEL_STATE);
            }

            return SUCCESS;
        }

        /**
        * Jog Key 확인하여 해당 Jog 축을 이동/정지한다.
        */
        int MoveJog()
        {
            int iResult = SUCCESS;

            // Jog Key 값 변수
            bool bXpStatus = false;
            bool bXnStatus = false;
            bool bYpStatus = false;
            bool bYnStatus = false;
            bool bTpStatus = false;
            bool bTnStatus = false;
            bool bZpStatus = false;
            bool bZnStatus = false;

            // Jog Key Check 
            // X(+) Key Read 
            iResult = m_OpPanel.GetJogXPlusButtonStatus(out bXpStatus);
            if (iResult != SUCCESS) return iResult;
            // X(-) Key Read 
            iResult = m_OpPanel.GetJogXMinusButtonStatus(out bXnStatus);
            if (iResult != SUCCESS) return iResult;
            // Y(+) Key Read 
            iResult = m_OpPanel.GetJogYPlusButtonStatus(out bYpStatus);
            if (iResult != SUCCESS) return iResult;
            // Y(-) Key Read 
            iResult = m_OpPanel.GetJogYMinusButtonStatus(out bYnStatus);
            if (iResult != SUCCESS) return iResult;
            // T(+) Key Read 
            iResult = m_OpPanel.GetJogTPlusButtonStatus(out bTpStatus);
            if (iResult != SUCCESS) return iResult;
            // T(-) Key Read 
            iResult = m_OpPanel.GetJogTMinusButtonStatus(out bTnStatus);
            if (iResult != SUCCESS) return iResult;
            // Z(+) Key Read 
            iResult = m_OpPanel.GetJogZPlusButtonStatus(out bZpStatus);
            if (iResult != SUCCESS) return iResult;
            // Z(-) Key Read 
            iResult = m_OpPanel.GetJogZMinusButtonStatus(out bZnStatus);
            if (iResult != SUCCESS) return iResult;

            // 이동 모드이면 Jog 이동 
            if (m_iJogIndex > DEF_MNGOPPANEL_JOG_NO_USE)
            {
                // X(+) Pitch 이동 
                if (bXpStatus && !bXnStatus)
                {
                    // 처음 눌리는 것이면 
                    if (m_iPrevJogVal_X != JOG_KEY_POS)
                    {
                        m_iPrevJogVal_X = JOG_KEY_POS;
                        m_bPrevDir_X = JOG_DIR_POS;
                        //m_OpPanel.StopJog(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_X);
                    }
                    // X(+) 방향으로 Pitch 이동 실시 
                    m_OpPanel.MoveJogPitch(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_X, JOG_DIR_POS);
                }
                // X(-) Pitch 이동 
                else if (!bXpStatus && bXnStatus)
                {
                    // 처음 눌리는 것이면 
                    if (m_iPrevJogVal_X != JOG_KEY_POS)
                    {
                        m_iPrevJogVal_X = JOG_KEY_POS;
                        m_bPrevDir_X = JOG_DIR_NEG;
                        //m_OpPanel.StopJog(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_X);
                    }
                    // X(-) 방향으로 Pitch 이동 실시 
                    m_OpPanel.MoveJogPitch(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_X, JOG_DIR_NEG);
                }
                // X(+/-) Velocity 이동 
                else if (bXpStatus && bXnStatus)
                {
                    // 처음 눌리는 것이면 
                    if (m_iPrevJogVal_X != JOG_KEY_ALL)
                    {
                        m_iPrevJogVal_X = JOG_KEY_ALL;
                        //m_OpPanel.StopJog(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_X);
                    }
                    // X(+/) 방향으로 Velocity 이동 실시 
                    m_OpPanel.MoveJogVelocity(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_X, m_bPrevDir_X);
                }
                // 아무 것도 안 눌렸을 때 
                else
                {
                    // 처음 안 눌리는 것이면 
                    if (m_iPrevJogVal_X != JOG_KEY_NON)
                    {
                        m_iPrevJogVal_X = JOG_KEY_NON;
                        m_OpPanel.StopJog(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_X);
                    }
                }


                // Y(+) Pitch 이동 
                if (bYpStatus && !bYnStatus)
                {
                    // 처음 눌리는 것이면 
                    if (m_iPrevJogVal_Y != JOG_KEY_POS)
                    {
                        m_iPrevJogVal_Y = JOG_KEY_POS;
                        m_bPrevDir_Y = JOG_DIR_POS;
                        //m_OpPanel.StopJog(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_Y);
                    }
                    // Y(+) 방향으로 Pitch 이동 실시 
                    m_OpPanel.MoveJogPitch(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_Y, JOG_DIR_POS);
                }
                // Y(-) Pitch 이동 
                else if (!bYpStatus && bYnStatus)
                {
                    // 처음 눌리는 것이면 
                    if (m_iPrevJogVal_Y != JOG_KEY_POS)
                    {
                        m_iPrevJogVal_Y = JOG_KEY_POS;
                        m_bPrevDir_Y = JOG_DIR_NEG;
                        //m_OpPanel.StopJog(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_Y);
                    }
                    // Y(-) 방향으로 Pitch 이동 실시 
                    m_OpPanel.MoveJogPitch(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_Y, JOG_DIR_NEG);
                }
                // Y(+/-) Velocity 이동 
                else if (bYpStatus && bYnStatus)
                {
                    // 처음 눌리는 것이면 
                    if (m_iPrevJogVal_Y != JOG_KEY_ALL)
                    {
                        m_iPrevJogVal_Y = JOG_KEY_ALL;
                        //m_OpPanel.StopJog(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_Y);
                    }
                    // Y(+/) 방향으로 Velocity 이동 실시 
                    m_OpPanel.MoveJogVelocity(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_Y, m_bPrevDir_Y);
                }
                // 아무 것도 안 눌렸을 때 
                else
                {
                    // 처음 안 눌리는 것이면 
                    if (m_iPrevJogVal_Y != JOG_KEY_NON)
                    {
                        m_iPrevJogVal_Y = JOG_KEY_NON;
                        m_OpPanel.StopJog(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_Y);
                    }
                }


                // T(+) Pitch 이동 
                if (bTpStatus && !bTnStatus)
                {
                    // 처음 눌리는 것이면 
                    if (m_iPrevJogVal_T != JOG_KEY_POS)
                    {
                        m_iPrevJogVal_T = JOG_KEY_POS;
                        m_bPrevDir_T = JOG_DIR_POS;
                        //m_OpPanel.StopJog(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_T);
                    }
                    // T(+) 방향으로 Pitch 이동 실시 
                    m_OpPanel.MoveJogPitch(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_T, JOG_DIR_POS);
                }
                // T(-) Pitch 이동 
                else if (!bTpStatus && bTnStatus)
                {
                    // 처음 눌리는 것이면 
                    if (m_iPrevJogVal_T != JOG_KEY_POS)
                    {
                        m_iPrevJogVal_T = JOG_KEY_POS;
                        m_bPrevDir_T = JOG_DIR_NEG;
                        //m_OpPanel.StopJog(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_T);
                    }
                    // T(-) 방향으로 Pitch 이동 실시 
                    m_OpPanel.MoveJogPitch(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_T, JOG_DIR_NEG);
                }
                // T(+/-) Velocity 이동 
                else if (bTpStatus && bTnStatus)
                {
                    // 처음 눌리는 것이면 
                    if (m_iPrevJogVal_T != JOG_KEY_ALL)
                    {
                        m_iPrevJogVal_T = JOG_KEY_ALL;
                        //m_OpPanel.StopJog(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_T);
                    }
                    // T(+/) 방향으로 Velocity 이동 실시 
                    m_OpPanel.MoveJogVelocity(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_T, m_bPrevDir_T);
                }
                // 아무 것도 안 눌렸을 때 
                else
                {
                    // 처음 안 눌리는 것이면 
                    if (m_iPrevJogVal_T != JOG_KEY_NON)
                    {
                        m_iPrevJogVal_T = JOG_KEY_NON;
                        m_OpPanel.StopJog(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_T);
                    }
                }


                // Z(+) Pitch 이동 
                if (bZpStatus && !bZnStatus)
                {
                    // 처음 눌리는 것이면 
                    if (m_iPrevJogVal_Z != JOG_KEY_POS)
                    {
                        m_iPrevJogVal_Z = JOG_KEY_POS;
                        m_bPrevDir_Z = JOG_DIR_POS;
                        //				m_OpPanel.StopJog(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_Z);
                    }
                    // Z(+) 방향으로 Pitch 이동 실시 
                    m_OpPanel.MoveJogPitch(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_Z, JOG_DIR_POS);
                }
                // Z(-) Pitch 이동 
                else if (!bZpStatus && bZnStatus)
                {
                    // 처음 눌리는 것이면 
                    if (m_iPrevJogVal_Z != JOG_KEY_POS)
                    {
                        m_iPrevJogVal_Z = JOG_KEY_POS;
                        m_bPrevDir_Z = JOG_DIR_NEG;
                        //				m_OpPanel.StopJog(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_Z);
                    }
                    // Z(-) 방향으로 Pitch 이동 실시 
                    m_OpPanel.MoveJogPitch(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_Z, JOG_DIR_NEG);
                }
                // Z(+/-) Velocity 이동 
                else if (bZpStatus && bZnStatus)
                {
                    // 처음 눌리는 것이면 
                    if (m_iPrevJogVal_Z != JOG_KEY_ALL)
                    {
                        m_iPrevJogVal_Z = JOG_KEY_ALL;
                        //				m_OpPanel.StopJog(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_Z);
                    }
                    // Z(+/) 방향으로 Velocity 이동 실시 
                    m_OpPanel.MoveJogVelocity(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_Z, m_bPrevDir_Z);
                }
                // 아무 것도 안 눌렸을 때 
                else
                {
                    // 처음 안 눌리는 것이면 
                    if (m_iPrevJogVal_Z != JOG_KEY_NON)
                    {
                        m_iPrevJogVal_Z = JOG_KEY_NON;
                        m_OpPanel.StopJog(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_Z);
                    }
                }
            }
            // 이동 모드 아니면 Touch Panel 전환 점검 
            else
            {
                // X(-)와 Y(+)가 동시에 눌리면 
                if (bXnStatus && bYpStatus)
                {
                    // Touch Panel 앞면으로 전환 
                    iResult = m_OpPanel.ChangeOpPanel(DEF_MNGOPPANEL_FRONT_PANEL_ID);
                    if (iResult != SUCCESS) return iResult;
                }
                // X(+)와 Y(-)가 동시에 눌리면 
                else if (bXpStatus && bYnStatus)
                {
                    // Touch Panel 뒷면으로 전환 
                    iResult = m_OpPanel.ChangeOpPanel(DEF_MNGOPPANEL_BACK_PANEL_ID);
                    if (iResult != SUCCESS) return iResult;
                }
            }

            return SUCCESS;
        }

        /**
        * 모든 축들에 대해 원점복귀 동작을 정지한다.
        */
        int StopAllReturnOrigin()
        {
            return m_OpPanel.StopAllReturnOrigin();
        }

        /**
        * 모든 축들에 대해 Servo AMP를 Enable한다.
        */
        int OnAllServo()
        {
            return m_OpPanel.OnAllServo();
        }

        /**
        * 모든 축들에 대해 Servo AMP를 Disable한다.
        */
        int OffAllServo()
        {
            return m_OpPanel.OffAllServo();
        }

        /**
        * 모든 축들에 대해 동작을 정지한다.
        */
        int StopAllAxis()
        {
            return m_OpPanel.StopAllAxis();
        }

        /**
        * 모든 축들에 대해 동작을 E-STOP 정지한다.
        */
        int EStopAllAxis()
        {
            return m_OpPanel.EStopAllAxis();
        }

        /**
        * Tower Lamp Blink 속도 설정하기
        *
        * @param	dRate : (OPTION=0.5) 설정할 Blink 속도
*/
        void SetBlinkRate(double dRate)
        {
            m_dBlinkRate = dRate;
        }

        /**
        * Jog에 사용할 Unit Index를 설정한다.
        *
        * @param	iUnitIndex : (OPTION=-1) Jog에 사용할 Unit Index
*/
        void SetJogUnit(int iUnitIndex)
        {
            // Jog로 이동할 Motion에 대한 정보 Index
            m_iJogIndex = iUnitIndex;

            COpPanelIOAddr sOpPanelIO = new COpPanelIOAddr();
            m_OpPanel.GetIOAddress(ref sOpPanelIO);
            //if (m_iJogIndex == DEF_JOG_CAMERA1)
            //{
            //    sOpPanelIO.FrontPanel.XpInputAddr = iJog_X_Backward_SWFront;
            //    sOpPanelIO.FrontPanel.XnInputAddr = iJog_X_Forward_SWFront;

            //    sOpPanelIO.BackPanel.XpInputAddr = iJog_X_Backward_SWRear;
            //    sOpPanelIO.BackPanel.XnInputAddr = iJog_X_Forward_SWRear;
            //}
            //else
            //{
            //    sOpPanelIO.FrontPanel.XpInputAddr = iJog_X_Forward_SWFront;
            //    sOpPanelIO.FrontPanel.XnInputAddr = iJog_X_Backward_SWFront;

            //    sOpPanelIO.BackPanel.XpInputAddr = iJog_X_Forward_SWRear;
            //    sOpPanelIO.BackPanel.XnInputAddr = iJog_X_Backward_SWRear;
            //}
            m_OpPanel.SetIOAddress(sOpPanelIO);
        }

        /**
        * Jog에 사용할 Unit Index를 설정한다.
        *
        * @param	iUnitIndex : (OPTION=-1) Jog에 사용할 Unit Index
*/
        void SetJogUnitExtra(int iUnitIndex)
        {
            /** Jog로 이동할 Motion에 대한 정보 Index */
            /*	if (iUnitIndex != m_iJogIndex)
            {
            m_OpPanel.StopJog(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_X);
            m_OpPanel.StopJog(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_Y);
            m_OpPanel.StopJog(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_T);
            m_OpPanel.StopJog(m_iJogIndex, m_iJogIndexExtra, JOG_KEY_Z);
            //		m_OpPanel.StopAllAxis();
            }*/

            /** Jog로 이동할 Motion에 대한 정보 Index */
            m_iJogIndexExtra = iUnitIndex;
        }

        /** 
        * 설정된 Jog에 사용할 Unit Index를 읽는다.
        */
        int GetJogUnit()
        {
            /** Jog로 이동할 Motion에 대한 정보 Index */
            return m_iJogIndex;
        }

        /** 
        * 설정된 Jog에 사용할 Unit Index를 읽는다.
        */
        int GetJogUnitExtra()
        {
            /** Jog로 이동할 Motion에 대한 정보 Index */
            return m_iJogIndexExtra;
        }

        /**
        * Unit의 원점복귀 Flag를 설정한다.
*/
        void ResetAllOriginFlag()
        {
            m_OpPanel.ResetAllOriginFlag();
        }

        void ResetAllInitFlag()
        {
            int i = 0;
            for (i = 0; i < INIT_UNIT_MAX ; i++)
                m_OpPanel.SetInitFlag(i, false);
        }

        int CheckSiliconeRemain(out bool pbEmptyAll, out bool pbEmptyPart)
        {
            int iResult = SUCCESS;

            iResult = m_OpPanel.CheckAllHead_Tank_Empty(out pbEmptyAll, out pbEmptyPart);
            if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        void SetIOCheck(bool bCheck)
        {
            m_bIOCheck = bCheck;
        }

        int SetAlarmForAlignMsg(bool bSet)
        {
            int iResult = SUCCESS;
            m_bIOCheck = bSet;
            if (bSet == true)
            {
                // Tower Lamp Buzzer On
                iResult = m_OpPanel.SetBuzzerStatus(DEF_OPPANEL_BUZZER_K3, true);
                if (iResult != SUCCESS) return iResult;
            }

            return iResult;
        }

        bool SetMotionStatus()
        {
            //bool pbStatus;
            //if (m_OpPanel.SetTouchStatus(&pbStatus)) return true;
            //else return false;
            return true;
        }

        int CheckSafetyBeforeAxisMove()
        {
#if SIMULATION_MOTION
            return SUCCESS;
#endif

            bool bStatus;

            m_OpPanel.GetEStopButtonStatus(out bStatus);
            if (bStatus == true)
            {
                return GenerateErrorCode(ERR_MNGOPPANEL_EMERGENCY);
            }

            m_OpPanel.GetSafeDoorStatus(out bStatus);
            if (bStatus == true && m_bSafeSensorUse == true)
            {
                return GenerateErrorCode(ERR_MNGOPPANEL_DOOR_OPEN);
            }

            bool[] bOriginSts;
            bStatus = m_OpPanel.CheckAllOrigin(out bOriginSts);

            if (bStatus == false)
                return GenerateErrorCode(ERR_MNGOPPANEL_NOT_ALL_ORIGIN);

            return SUCCESS;
        }

        int CheckSafetyBeforeCylinderMove()
        {
#if SIMULATION_MOTION
            return SUCCESS;
#endif
            bool bStatus;

            //	m_OpPanel.GetEStopButtonStatus(out bStatus);
            //	if(bStatus == true)
            //	{
            //		return GenerateErrorCode(ERR_MNGOPPANEL_EMERGENCY);
            //	}

            m_OpPanel.GetSafeDoorStatus(out bStatus);
            if (bStatus == true && m_bSafeSensorUse == true)
            {
                return GenerateErrorCode(ERR_MNGOPPANEL_DOOR_OPEN);
            }

            return SUCCESS;
        }

        void SetTrafficJam(bool bSet)
        {
            m_bTrafficJam = bSet;
        }

        bool GetTrafficJam()
        {
            return m_bTrafficJam;
        }

        void SetNoPanel(bool bSet)
        {
            m_bNoPanel = bSet;
        }

        bool GetNoPanel()
        {
            return m_bNoPanel;
        }

    }
}
