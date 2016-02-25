using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Data;
using System.Data.SQLite;
using System.Data.SQLite.Linq;
using System.IO;

using static LWDicer.Control.DEF_System;
using static LWDicer.Control.DEF_Common;
using static LWDicer.Control.DEF_Error;
using static LWDicer.Control.DEF_OpPanel;
using static LWDicer.Control.DEF_Thread;
using static LWDicer.Control.DEF_DataManager;

using static LWDicer.Control.DEF_Cylinder;
using static LWDicer.Control.DEF_Vacuum;

namespace LWDicer.Control
{
    public class DEF_DataManager
    {
        public const int ERR_DATA_MANAGER_FAIL_BACKUP_DB             = 1;
        public const int ERR_DATA_MANAGER_FAIL_DELETE_DB             = 2;
        public const int ERR_DATA_MANAGER_FAIL_DROP_TABLES           = 3;
        public const int ERR_DATA_MANAGER_FAIL_BACKUP_ROW            = 4;
        public const int ERR_DATA_MANAGER_FAIL_SAVE_LOGIN_HISTORY    = 5;
        public const int ERR_DATA_MANAGER_FAIL_SAVE_SYSTEM_DATA      = 6;
        public const int ERR_DATA_MANAGER_FAIL_LOAD_SYSTEM_DATA      = 7;
        public const int ERR_DATA_MANAGER_FAIL_SAVE_MODEL_DATA       = 8;
        public const int ERR_DATA_MANAGER_FAIL_LOAD_MODEL_DATA       = 9;
        public const int ERR_DATA_MANAGER_FAIL_SAVE_MODEL_LIST       = 10;
        public const int ERR_DATA_MANAGER_FAIL_LOAD_MODEL_LIST       = 11;
        public const int ERR_DATA_MANAGER_FAIL_SAVE_GENERAL_DATA     = 12;
        public const int ERR_DATA_MANAGER_FAIL_LOAD_GENERAL_DATA     = 13;

        public const int ERR_DATA_MANAGER_IO_DATA_FILE_NOT_EXIST = 1;
        public const int ERR_DATA_MANAGER_IO_DATA_FILE_CLOSE_FAILURE = 2;
        public const int ERR_DATA_MANAGER_CAMERA_NO_OUT_RANGE = 3;
        public const int ERR_DATA_MANAGER_TEACHING_INFO_FILE_NOT_EXIST = 4;
        public const int ERR_DATA_MANAGER_UNIT_INDEX_OUT_RANGE = 5;
        public const int ERR_DATA_MANAGER_POS_INDEX_OUT_RANGE = 6;
        public const int ERR_DATA_MANAGER_ARG_NULL_POINTER = 7;
        public const int ERR_DATA_MANAGER_POS_INFO_FILE_CLOSE_FAILURE = 8;
        public const int ERR_DATA_MANAGER_VACUUM_INDEX_OUT_RANGE = 9;
        public const int ERR_DATA_MANAGER_IO_NOT_INITIALIZED = 10;
        public const int ERR_DATA_MANAGER_MODEL_NAME_NULL = 11;
        public const int ERR_DATA_MANAGER_MODEL_FILE_NOT_EXIST = 12;
        public const int ERR_DATA_MANAGER_MODEL_CHANGE_FAIL = 13;
        public const int ERR_DATA_MANAGER_DELETE_CURRENT_MODEL_FAIL = 14;
        public const int ERR_DATA_MANAGER_COPY_SELF_MODEL_INCORRECT = 15;
        public const int ERR_DATA_MANAGER_CREATE_MODEL_FAIL = 16;
        public const int ERR_DATA_MANAGER_INVALID_FIXEDCOORD_UNIT_ID = 17;

        public const int ERR_SYSTEM_DATA_MANAGER_FILE_SAVE_FAIL = 20;
        public const int ERR_SYSTEM_DATA_MANAGER_NO_SECTIONNAME = 21;

        public const int ERR_MODEL_DATA_MAIN_MODEL_NAME_NULL = 31;
        public const int ERR_MODEL_DATA_MANAGER_NO_SECTIONNAME = 32;


        public const int DEF_MAX_SYSTEM_SECTION = 7;
        public const int DEF_MAX_FIXED_POSITION_SECTION = 14;
        public const int DEF_MAX_OFFSET_POSITION_SECTION = 14;
        public const int DEF_MAX_AXIS_NUM = 4;

        public class CSystemDataFileNames
        {
            public string SystemDataFile;
            public string LogDataFile;
            public string ProductDataFile;
            public string MotorDataFile;
            public string IndMotorActuatorDataFile;
            public string CylinderTimerDataFile;
            public string VacuumTimerDataFile;
            public string CalibrationDataFile;
            public string TeachingDataFile;
            public string StopCodeFile;
            public string UVLifeFile;
            public string UVCheckFile;
        }

        public class CModelDataFileNames
        {
            public string BaseDir;
            public string ModelName;
            public string PanelDataFile;
            public string FunctionDataFile;
            public string OffsetDataFile;
            public string TeachingDataFile;
        }

        public class CSystemData
        {
            //
            public string ModelName = "Default";

            //
            public string PassWord;     // Engineer Password

            // Timer
            public CCylinderTime[] CylinderTimer = new CCylinderTime[(int)EObjectCylinder.MAX_OBJ];
            public CVacuumTime[] VacuumTimer = new CVacuumTime[(int)EObjectVacuum.MAX_OBJ];


            // 아래는 아직 미정리 내역들

            public int SystemType;      // 작업변
            public bool UseSafetySensor;
            public DEF_Thread.ERunMode eOpModeStatus;
            public bool UseStepDisplay;
            public string LineControllerIP;
            public int LineControllerPort;
            public int MelsecChannelNo;
            public int MelsecStationNo;
            public int SystemLanguageSelect;
            public int VelocityMode;
            public double PanelBacklash;
            public bool UseOnLineUse;

            public bool UseInSfaTest;                // SFA 내에서 Test할때 쓰임
            public bool UseDisplayQuitButton;

            // Vision
            public bool UseVisionDisplay;
            public double VisionCenter_Offset_X;
            public double VisionCenter_Offset_Y;
            public bool UseAutoSearch_Panel;
            public double AutoSearchDistance_Panel;
            //	BOOL	bAutoSearch_SubMark;

            public bool UseAlignUseSubMark;  // sub 마크로 Align 할지 여부

            // Stage
            /** Workbench Unit에 진출입할때의 안전한 Y Position **/
            public double Stage1LoadPos_Y;
            public double Stage1UnloadPos_Y;
            public double Stage2LoadPos_Y;
            public double Stage2UnloadPos_Y;
            public double Stage3LoadPos_Y;
            public double Stage3UnloadPos_Y;

            /** Stage의 충돌 방지 봉때문에 Workbench에 간섭을 안주는 안전한 위치의 한계를 정한다. **/
            public double Stage1PlusSafetyLimit_X;
            public double Stage1MinusSafetyLimit_X;
            public double Stage2PlusSafetyLimit_X;
            public double Stage2MinusSafetyLimit_X;
            public double Stage3PlusSafetyLimit_X;
            public double Stage3MinusSafetyLimit_X;

            /** Stage가 회전중심축을 기준으로 180도 턴 했을때 정대칭이 되기 위한 오차. **/
            public double Stage1_Turn180_Offset_X;
            public double Stage1_Turn180_Offset_Y;

            /** Stage가 Workbench와 충돌하지 않는 안전한 Z축위치 **/
            public double Stage1_ZAxis_SafetyUpPos;
            public double Stage1_ZAxis_SafetyDownPos;
            public double Stage2_ZAxis_SafetyUpPos;
            public double Stage2_ZAxis_SafetyDownPos;
            public double Stage3_ZAxis_SafetyUpPos;
            public double Stage3_ZAxis_SafetyDownPos;

            // Dispenser
            public int TrashIntervalTime;           // Auto 아닌 모드에서 Trash할때까지의 wait time
            public int TrashTime;                   // 동작중이 아닐때 Trash 시간
            public int DispenserSpeed;              // 도포 속도
            public int DispenserAccel;              // 도포 속도
            public bool UseDispenserTrashOption;         // 자동Run시 도포전 토출 여부
            public bool UseDispenserRunWaitTrashOption;  // 자동 Run중 대기동작시.. 자동 토출여부
            public int TrashPerPanelCount;          // 자동 Run중 일정 횟수 판넬 생산후 토출기능 수행여부

            public bool UseRunTime_Do_Dispense;              // Run시 도포건 토출 여부
            public bool UseRunTime_Do_Cure;                  // Run시 경화기 사용 여부

            // Head
            public double Workbench_XHalf_Size;
            public double Workbench_YHalf_Size;

            public double NeedlesInterGap;  ///** 도포건 각 Needle 사이의 물리적인 거리 */

            public double Workbench_YShipt;       // SHead1 기준, 현재는 센터라 의미없음
            public double Workbench_XShipt;       // GHead 기준

            public double AfterFailLaser_DispensingHeight;    // Laser값 읽기 실패시 몇미리 떠서 갈지 설정함

            //public bool UseHead_UseLaserSensor[DEF_MAX_HEAD_NO];     // Laser센서 읽기 여부
            public double InterUVLEDDistance; // UVLED각각 채널 사이의 거리
            public double UVLightLowerLimit;      // UVLED Light Value Lower Limit
            public int nUVCheckPerCount;       // 생산중 몇매당 할지..
            public int nPanelCountForUVCheck;  // UV Check 후 생산된 수

            // UVLamp
            public bool UseUVLampRunCheckLight;          // UVLamp의 Run시 광량 체크
            public double UVLampRunLight;             // Run시의 권장 광량
            public double UVLampRunLightLimit;            // Run시의 광량 하한값
            public double UVLampLightControlLimit;        // 권장 광량의 오차 범위
            public bool UseUVLampUseAutoControlLight;        // 자동 광량 조절

            public double UVLampMaxTime;                  // 허용 램프 최대 수명


            // Pumping Job
            public int DoPumpingIntervalTime;       // 펌핑 잡 Interval
            public int DoPumpingTime;               // 펌핑 잡 총 동작 시간
            public int Pumping_OneShot_Interval;        // 매 일회 펌핑 동작당 대기시간

            public bool UseUseWorkbenchVacuum;   // Workbench Vacuum 사용 유무


            // Dispenser측, MMC에서 제어하는 cylinder time
            public double Head_Cyl_MovingTime;
            public double Head_Cyl_AfterOnTime;
            public double Head_Cyl_AfterOffTime;
            public double Head_Cyl_NoSensorWaitTime;

            public double Head_Gun_UV_InterGap;       // from Needle to UV End distance

            public bool UseCheck_Panel_Data;         // run time, check panel data
            public bool UseCheck_Panel_History;      // run time, check panel id History

            public bool UseVIPMode;

            // 2014.10.20 CF Align
            public bool UseUseCFAlign;       // CF Align 사용 여부
            public double CFAlignLimit;

            public CSystemData()
            {
                for(int i = 0; i < (int)EObjectCylinder.MAX_OBJ; i++)
                {
                    CylinderTimer[i] = new CCylinderTime();
                }

                for (int i = 0; i < (int)EObjectVacuum.MAX_OBJ; i++)
                {
                    VacuumTimer[i] = new CVacuumTime();
                }
            }
        }

        public class CProductData
        {
            public string Day_ModelName;
            public int Day_ModelProductQuantity;
            public string SW_ModelName;
            public int SW_ModelProductQuantity;
            public string GY_ModelName;
            public int GY_ModelProductQuantity;
            public int ProductQuantity_forOut;  // Out 되는 생산 수량
            public int ProductQuantity_forIn;       // In되는 생산 수량
        }

        public class CMotorParameter
        {
            public double CWSWLimit;
            public double CCWSWLimit;
            public double HomeFastVelocity;
            public double HomeSlowVelocity;
            public int HomeAccelerate;
            public double HomeOffset;
            public double JogPitch;
            public double JogVelocity;
            public double FastRunVelocity;
            public double RunVelocity;
            public double SlowRunVelocity;
            public int RunAccelerate;
            public double LimitTime;
            public double OriginLimitTime;
            public double StabilityTime;
            public double Tolerance;

        }

        public class CSimpleAxisTimer
        {
            public double TurningTime;
            public double SettlingTime;
            public double NoSenseMovingTime;
        }

        public class CCalibrationParameter
        {
            public int Move_Point_X;            // 2D Calibration을 하기 위한 X방향 이동 포인트 수
            public int Move_Point_Y;            // 2D Calibration을 하기 위한 Y방향 이동 포인트 수
            public double Move_Width_X;           // 2D Calibration을 하기 위한 X방향 이동 거리
            public double Move_Width_Y;           // 2D Calibration을 하기 위한 Y방향 이동 거리
            public bool UseComplete_Flag;            // 2D Calibration 수행 결과
            //public double PortingFactor[CALIB_FACTOR_NUMBER]; // 2D Calibration을 수행한 후 나온 Camera Factor 값

            public double FixedMoveX;         // 고정 좌표를 찾기 위해 첫번째 Mark 인식 후 이동할 X 방향 거리
            public double FixedMoveY;         // 고정 좌표를 찾기 위해 첫번째 Mark 인식 후 이동할 Y 방향 거리
            public double FixedMoveT;         // 고정 좌표를 찾기 위해 첫번째 Mark 인식 후 이동할 T 방향 거리

            public double FixedX;             // 계산된 고정 좌표 X 값
            public double FixedY;             // 계산된 고정 좌표 Y 값
        }

        public class CPanelMarkPos
        {
            public bool UseUseFlag;
            public double X;
            public double Y;
            public double Distance;
            public bool UseSubMarkUseFlag;
            public double Sub_Left_X;
            public double Sub_Right_X;
            public double Sub_Left_Y;
            public double Sub_Right_Y;
        }

        public class CWaferData
        {
            public double Size_X;
            public double Size_Y;
            public CPanelMarkPos FiduMarkXu = new CPanelMarkPos();
            //public SPanelMarkPos sFiduMarkXd;
            //public SPanelMarkPos sFiduMarkYl;
            //public SPanelMarkPos sFiduMarkYr;
            //public EInputDirection eInputDirection;
            //public EOutputDirection eOutputDirection;
            public double Thickness;
        }

        public class CLogParameter
        {
            public bool UseLogLevelTactTime;
            public bool UseLogLevelNormal;
            public bool UseLogLevelWarning;
            public bool UseLogLevelError;
            public int LogKeepingDay;

        }

        public class CModelHeader
        {
            // Header
            public string Name        = "Default";   // unique primary key
            public string Comment     = "Default Comment";
            public string Parent      = string.Empty;  // if == "", root
        }

        public class CModelData    // Model, Recipe
        {
            ///////////////////////////////////////////////////////////
            // Header
            public string Name = "Default";   // unique primary key

            ///////////////////////////////////////////////////////////
            // Wafer Data
            public CWaferData Wafer = new CWaferData();

            ///////////////////////////////////////////////////////////
            // Function Parameter
            public bool Use2Step_Use;

            // Dispenser
            public int uDispensingTwiceTime;
            public int uDisepnsingSpeed;
            public int uUVLEDSpeed;

            public double ThicknessValue; // 판넬 두께

	        public bool UseUHandler_ExtraVccUseFlag; // 2014.02.21 by ranian. Extra Vcc 추가
            public bool UseUHandler_WaitPosUseFlag; // 2014.02.21 by ranian. LP->UP 로 갈 때, WP 사용 여부
        }

    }

    public class MDataManager : MObject
    {
        private CLoginData m_Login = new CLoginData();
        CDBInfo m_DBInfo;

        public CSystemData m_SystemData { get; set; } = new CSystemData();
        public CModelData m_ModelData { get; set; } = new CModelData();
        public List<CModelHeader> m_ModelList { get; set; } = new List<CModelHeader>();

        public DEF_IO.CIOInfo[] m_InputArray { get; set; } = new DEF_IO.CIOInfo[DEF_IO.MAX_IO_INPUT];
        public DEF_IO.CIOInfo[] m_OutputArray { get; set; } = new DEF_IO.CIOInfo[DEF_IO.MAX_IO_OUTPUT];

        public MDataManager(CObjectInfo objInfo, CDBInfo dbInfo)
            : base(objInfo)
        {
            m_DBInfo = dbInfo;
            SetLogin(new CLoginData(), true);

            for(int i = 0; i < DEF_IO.MAX_IO_INPUT; i++)
            {
                m_InputArray[i] = new DEF_IO.CIOInfo(i+DEF_IO.INDEX_INPUT, DEF_IO.EIOType.DI);
            }
            for (int i = 0; i < DEF_IO.MAX_IO_OUTPUT; i++)
            {
                m_OutputArray[i] = new DEF_IO.CIOInfo(i+DEF_IO.INDEX_OUTPUT, DEF_IO.EIOType.DO);
            }

            TestFunction();

            LoadGeneralData();
            LoadSystemData();
            LoadModelList();
            ChangeModel();
        }

        public void TestFunction()
        {
            ///////////////////////////////////////
            CModelHeader header = new CModelHeader();
            m_ModelList.Add(header);

            for(int i = 0; i < 3; i++)
            {
                header = new CModelHeader();
                header.Name = $"Model{i}";
                header.Comment = $"Comment{i}";
                header.Parent = $"Parent{i}";
                m_ModelList.Add(header);
            }

            m_SystemData.CylinderTimer[0].SettlingTime1 = 1;
            m_SystemData.CylinderTimer[1].SettlingTime2 = 2;
            m_SystemData.Head_Cyl_AfterOffTime = 3.456;
            m_SystemData.LineControllerIP = "122,333,444";

            //SaveSystemData();
            //SaveModelList();
            //SaveModel();

            SaveGeneralData();

            ///////////////////////////////////////

            Type type = typeof(CSystemData);
            Dictionary<string, string> fieldBook = ObjectExtensions.ToStringDictionary(m_SystemData, type);

            CSystemData systemData2 = new CSystemData();
            ObjectExtensions.FromStringDicionary(systemData2, type, fieldBook);
        }

        public int BackupDB()
        {
            string[] dblist = new string[] { $"{m_DBInfo.DBConn}", $"{m_DBInfo.DBConn_Info}",
                $"{m_DBInfo.DBConn_DLog}", $"{m_DBInfo.DBConn_ELog}" };

            DateTime time = DateTime.Now;

            foreach(string source in dblist)
            {
                if (DBManager.BackupDB(source, time) == false)
                {
                    WriteLog("fail : backup db.", ELogType.Debug, ELogWriteType.Error);
                    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_BACKUP_DB);
                }
            }

            WriteLog("success : backup db.", ELogType.Debug);
            return SUCCESS;
        }

        public int DeleteDB()
        {
            string[] dblist = new string[] { $"{m_DBInfo.DBConn}", $"{m_DBInfo.DBConn_Backup}",
                $"{m_DBInfo.DBConn_Info}", $"{m_DBInfo.DBConn_DLog}", $"{m_DBInfo.DBConn_ELog}" };

            foreach(string source in dblist)
            {
                if (DBManager.DeleteDB(source) == false)
                {
                    WriteLog("fail : delete db.", ELogType.Debug, ELogWriteType.Error);
                    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_DELETE_DB);
                }
            }

            WriteLog("success : delete db.", ELogType.Debug);
            return SUCCESS;
        }

        public int SaveSystemData()
        {
            try
            {
                // SystemData
                string output = JsonConvert.SerializeObject(m_SystemData);

                if (DBManager.InsertRow(m_DBInfo.DBConn, m_DBInfo.TableSystem, "name", nameof(CSystemData), output,
                    true, m_DBInfo.DBConn_Backup) != true)
                {
                    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_SAVE_SYSTEM_DATA);
                }
            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_SAVE_SYSTEM_DATA);
            }

            WriteLog("success : save system data.", ELogType.SYSTEM, ELogWriteType.SAVE);
            return SUCCESS;
        }

        public int LoadSystemData()
        {
            CSystemData systemData = null;
            try
            {
                string output;

                // SystemData
                if (DBManager.SelectRow(m_DBInfo.DBConn, m_DBInfo.TableSystem, "name", nameof(CSystemData), out output) == true)
                {
                    systemData = JsonConvert.DeserializeObject<CSystemData>(output);
                }
                //else // Load 함수에서는 계속 읽기 위해서 Error 처리는 하지 않는다.
                //{
                //    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_SYSTEM_DATA);
                //}
            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_SYSTEM_DATA);
            }

            if(systemData != null)
            {
                m_SystemData = systemData;
                WriteLog("success : load system data.", ELogType.SYSTEM, ELogWriteType.LOAD);
            }
            return SUCCESS;
        }

        public int SaveModelList()
        {
            try
            {
                List<string> querys = new List<string>();
                string query;

                // 0. create table
                query = $"CREATE TABLE IF NOT EXISTS {m_DBInfo.TableModelHeader} (name string primary key, data string)";
                querys.Add(query);

                // 1. delete all
                query = $"DELETE FROM {m_DBInfo.TableModelHeader}";
                querys.Add(query);

                // 2. save model list
                string output;
                foreach(CModelHeader header in m_ModelList)
                {
                    output = JsonConvert.SerializeObject(header);
                    query = $"INSERT INTO {m_DBInfo.TableModelHeader} VALUES ('{header.Name}', '{output}')";
                    querys.Add(query);
                }

                // 3. execute query
                if (DBManager.ExecuteNonQuerys(m_DBInfo.DBConn, querys) != true)
                {
                    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_SAVE_MODEL_LIST);
                }
            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_SAVE_MODEL_LIST);
            }

            WriteLog($"success : save model list", ELogType.Debug);
            return SUCCESS;
        }

        public int LoadModelList()
        {
            try
            {
                string query;

                // 0. select table
                query = $"SELECT * FROM {m_DBInfo.TableModelHeader}";

                // 1. get table
                DataTable datatable;
                if(DBManager.GetTable(m_DBInfo.DBConn, query, out datatable) != true)
                {
                    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_MODEL_LIST);
                }

                // 2. delete list
                m_ModelList.Clear();

                // 3. get list
                foreach (DataRow row in datatable.Rows)
                {
                    string output = row["data"].ToString();
                    CModelHeader header = JsonConvert.DeserializeObject<CModelHeader>(output);
                    m_ModelList.Add(header);
                }
            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_MODEL_LIST);
            }

            WriteLog($"success : load model list", ELogType.Debug);
            return SUCCESS;
        }

        bool IsModelExist(string name)
        {
            foreach(CModelHeader header in m_ModelList)
            {
                if(header.Name == name)
                {
                    return true;
                }
            }
            return false;
        }

        public int SaveModel(CModelData modelData = null)
        {
            string name;
            try
            {
                string output;
                if(modelData == null)
                {
                    name = m_ModelData.Name;
                    output = JsonConvert.SerializeObject(m_ModelData);
                }
                else
                {
                    name = modelData.Name;
                    output = JsonConvert.SerializeObject(modelData);
                }

                if (DBManager.InsertRow(m_DBInfo.DBConn, m_DBInfo.TableModel, "name", name, output,
                    true, m_DBInfo.DBConn_Backup) != true)
                {
                    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_SAVE_MODEL_DATA);
                }
            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_SAVE_MODEL_DATA);
            }

            WriteLog($"success : save model [{name}].", ELogType.SYSTEM, ELogWriteType.SAVE);
            return SUCCESS;
        }

        public int ChangeModel(string name = "")
        {
            // 0. check exist
            if(string.IsNullOrEmpty(name))
            {
                name = m_SystemData.ModelName;
            }
            if(IsModelExist(name) == false)
            {
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_MODEL_DATA);
            }

            CModelData modelData = null;
            try
            {
                // 1. load model
                string output;
                if (DBManager.SelectRow(m_DBInfo.DBConn, m_DBInfo.TableModel, "name", name, out output) == true)
                {
                    modelData = JsonConvert.DeserializeObject<CModelData>(output);
                }
                else
                {
                    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_MODEL_DATA);
                }

                // 2. save system data
                string prev_model = m_SystemData.ModelName;
                m_SystemData.ModelName = name;
                int iResult = SaveSystemData();
                if(iResult != SUCCESS)
                {
                    m_SystemData.ModelName = prev_model;
                    return iResult;
                }
                
            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_MODEL_DATA);
            }

            // finally, set model data
            if(modelData != null)
            {
                m_ModelData = modelData;
                WriteLog($"success : change model [{m_ModelData.Name}].", ELogType.SYSTEM, ELogWriteType.LOAD);
            }
            return SUCCESS;
        }

        public int ViewModel(string name, out CModelData modelData)
        {
            modelData = new CModelData();
            // 0. check exist
            if (IsModelExist(name) == false)
            {
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_MODEL_DATA);
            }

            try
            {
                // 1. load model
                string output;
                if (DBManager.SelectRow(m_DBInfo.DBConn, m_DBInfo.TableModel, "name", name, out output) == true)
                {
                    modelData = JsonConvert.DeserializeObject<CModelData>(output);
                }
                else
                {
                    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_MODEL_DATA);
                }
            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_MODEL_DATA);
            }

            return SUCCESS;
        }

        public CLoginData GetLogin()
        {
            return m_Login;
        }

        public int SetLogin(CLoginData login, bool IsSystemStart = false)
        {
            if(IsSystemStart == false)
            {
                // Type과 사번이 같다면 동일 인물로 본다.
                if (login.Type == m_Login.Type && login.Number == m_Login.Number)
                {
                    return SUCCESS;
                }
            }

            // write login history
            string create_query = $"CREATE TABLE IF NOT EXISTS {m_DBInfo.TableLoginHistory} (logintime datetime, type string, number string, name string)";
            string query = $"INSERT INTO {m_DBInfo.TableLoginHistory} VALUES ('{DBManager.DateTimeSQLite(login.LoginTime)}', '{login.Type}', '{login.Number}', '{login.Name}')";

            if (DBManager.ExecuteNonQuerys(m_DBInfo.DBConn_ELog, create_query, query) == false)
            {
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_SAVE_LOGIN_HISTORY);
            }

            m_Login = login;
            DBManager.SetOperator(m_Login.Number, m_Login.Type.ToString());
            WriteLog($"login : {login}", ELogType.LOGIN, ELogWriteType.LOGIN);

            return SUCCESS;
        }

        public int LoadGeneralData()
        {
            int iResult;

            iResult = LoadIOList();
            //if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }
        public int LoadIOList()
        { 
            try
            {
                string query;

                // 0. select table
                query = $"SELECT * FROM {m_DBInfo.TableIO}";

                // 1. get table
                DataTable datatable;
                if (DBManager.GetTable(m_DBInfo.DBConn_Info, query, out datatable) != true)
                {
                    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_GENERAL_DATA);
                }

                // 2. delete list

                // 3. get list
                foreach (DataRow row in datatable.Rows)
                {
                    int index;
                    if(int.TryParse(row["name"].ToString(), out index))
                    {
                        string output = row["data"].ToString();
                        DEF_IO.CIOInfo ioInfo = JsonConvert.DeserializeObject<DEF_IO.CIOInfo>(output);
                        
                        if(index >= DEF_IO.INDEX_INPUT && index < DEF_IO.INDEX_INPUT)
                        {
                            m_InputArray[index - DEF_IO.INDEX_INPUT] = ioInfo;
                        } else if (index >= DEF_IO.INDEX_OUTPUT && index < DEF_IO.INDEX_END)
                        {
                            m_OutputArray[index - DEF_IO.INDEX_OUTPUT] = ioInfo;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_LOAD_GENERAL_DATA);
            }

            WriteLog($"success : load io list", ELogType.Debug);
            return SUCCESS;
        }

        public int SaveGeneralData()
        {
            int iResult;

            iResult = SaveIOList();
            //if (iResult != SUCCESS) return iResult;

            return SUCCESS;
        }

        public int SaveIOList()
        {
            try
            {
                List<string> querys = new List<string>();
                string query;

                // 0. create table
                query = $"CREATE TABLE IF NOT EXISTS {m_DBInfo.TableIO} (name string primary key, data string)";
                querys.Add(query);

                // 1. delete all
                query = $"DELETE FROM {m_DBInfo.TableIO}";
                querys.Add(query);

                // 2. save list
                string output;
                for (int i = 0; i < DEF_IO.MAX_IO_INPUT; i++)
                {
                    output = JsonConvert.SerializeObject(m_InputArray[i]);
                    query = $"INSERT INTO {m_DBInfo.TableIO} VALUES ('{i+DEF_IO.INDEX_INPUT}', '{output}')";
                    querys.Add(query);
                }
                for (int i = 0; i < DEF_IO.MAX_IO_OUTPUT; i++)
                {
                    output = JsonConvert.SerializeObject(m_OutputArray[i]);
                    query = $"INSERT INTO {m_DBInfo.TableIO} VALUES ('{i + DEF_IO.INDEX_OUTPUT}', '{output}')";
                    querys.Add(query);
                }

                // 3. execute query
                if (DBManager.ExecuteNonQuerys(m_DBInfo.DBConn_Info, querys) != true)
                {
                    return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_SAVE_GENERAL_DATA);
                }
            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_DATA_MANAGER_FAIL_SAVE_GENERAL_DATA);
            }

            WriteLog($"success : save io list", ELogType.Debug);
            return SUCCESS;
        }

    }
}
