using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO.Ports;

using System.Windows.Forms;

using LWDicer.UI;
using MotionYMC;

using static LWDicer.Control.DEF_System;
using static LWDicer.Control.DEF_Common;
using static LWDicer.Control.DEF_Error;
using static LWDicer.Control.DEF_IO;

using static LWDicer.Control.DEF_OpPanel;
using static LWDicer.Control.DEF_Thread;
using static LWDicer.Control.DEF_DataManager;

using static LWDicer.Control.DEF_Motion;
using static LWDicer.Control.DEF_Yaskawa;
using static LWDicer.Control.DEF_MultiAxesYMC;
using static LWDicer.Control.DEF_Cylinder;
using static LWDicer.Control.DEF_Vacuum;
using static LWDicer.Control.DEF_Vision;

using static LWDicer.Control.DEF_MeElevator;
using static LWDicer.Control.DEF_MeHandler;
using static LWDicer.Control.DEF_MeStage;
using static LWDicer.Control.DEF_SerialPort;
using static LWDicer.Control.DEF_PolygonScanner;

namespace LWDicer.Control
{
    public class MLWDicer : MObject, IDisposable
    {
        // static common data
        public static bool bUseOnline { get; private set; }
        public static bool bInSfaTest { get; private set; }

        ///////////////////////////////////////////////////////////////////////
        // Common Class
        public MSystemInfo m_SystemInfo { get; private set; }
        public MDataManager m_DataManager { get; private set; }

        ///////////////////////////////////////////////////////////////////////
        // Hardware Layer

        // Motion
        public MYaskawa m_YMC;

        // MultiAxes
        public MMultiAxes_YMC m_AxStage1;
        public MMultiAxes_YMC m_AxLoader;
        public MMultiAxes_YMC m_AxPushPull;
        public MMultiAxes_YMC m_AxCentering1;
        public MMultiAxes_YMC m_AxRotate1;
        public MMultiAxes_YMC m_AxCleanNozzle1;
        public MMultiAxes_YMC m_AxCoatNozzle1;
        public MMultiAxes_YMC m_AxCentering2;
        public MMultiAxes_YMC m_AxRotate2;
        public MMultiAxes_YMC m_AxCleanNozzle2;
        public MMultiAxes_YMC m_AxCoatNozzle2;
        public MMultiAxes_YMC m_AxUpperHandler;
        public MMultiAxes_YMC m_AxLowerHandler;
        public MMultiAxes_YMC m_AxCamera1;
        public MMultiAxes_YMC m_AxLaser1;

        // IO
        public IIO m_IO { get; private set; }

        // Cylinder
        public ICylinder m_UHandlerUDCyl;
        public ICylinder m_UHandlerUDCyl2;

        // Vacuum
        public IVacuum m_Stage1Vac;

        public IVacuum m_UHandlerSelfVac;
        public IVacuum m_LHandlerSelfVac;

        // Serial
        public ISerialPort m_PolygonComPort;

        // Scanner
        public IPolygonScanner[] m_Scanner = new IPolygonScanner[(int)EObjectScanner.MAX_OBJ];

        public MVisionSystem m_VisionSystem;
        public MVisionCamera[] m_VisionCamera;
        public MVisionView[] m_VisionView;



        // Mechanical Layer

        public MMeElevator m_MeElevator;            // Cassette Loader 용 Elevator
        public MMeHandler m_MeUpperHandler;         // UpperHandler of 2Layer
        public MMeHandler m_MeLowerHandler;         // LowerHandler of 2Layer
        public MMeStage m_MeStage;         // LowerHandler of 2Layer

        public MVision m_Vision { get; set; }

        ///////////////////////////////////////////////////////////////////////
        // Control Layer
        public MCtrlLoader m_ctrlLoader { get; private set; }
        public MCtrlPushPull m_ctrlPushPull { get; private set; }
        public MCtrlStage1 m_ctrlStage1 { get; private set; }
        public MCtrlHandler m_ctrlHandler { get; private set; }

        ///////////////////////////////////////////////////////////////////////
        // Process Layer
        public MTrsAutoManager m_trsAutoManager { get; private set; }
        public MTrsLoader m_trsLoader { get; private set; }
        public MTrsPushPull m_trsPushPull { get; private set; }
        public MTrsStage1 m_trsStage1 { get; private set; }

        public MLWDicer(CObjectInfo objInfo)
            : base(objInfo)
        {
        }

        ~MLWDicer()
        {
            Dispose();
        }

        public void Dispose()
        {
            // close handle

        }

        public CLoginData GetLogin()
        {
            return m_DataManager?.GetLogin();
        }

        public void SetLogin(CLoginData login)
        {
            m_DataManager?.SetLogin(login);
        }

        public void TestFunction()
        {

        }

        public int Initialize(CMainFrame form1 = null)
        {
            TestFunction();

            ////////////////////////////////////////////////////////////////////////
            // 0. Common Class
            ////////////////////////////////////////////////////////////////////////
            // init data file name
            CDBInfo dbInfo;
            InitDataFileNames(out dbInfo);
            CObjectInfo.DBInfo = dbInfo;
            MLog.DBInfo = dbInfo;

            CObjectInfo objInfo;
            m_SystemInfo = new MSystemInfo();

            // self set MLWDicer
            m_SystemInfo.GetObjectInfo(0, out objInfo);
            this.ObjInfo = objInfo;

            // DataManager
            m_SystemInfo.GetObjectInfo(1, out objInfo);
            m_DataManager = new MDataManager(objInfo, dbInfo);

            ////////////////////////////////////////////////////////////////////////
            // 1. Hardware Layer
            ////////////////////////////////////////////////////////////////////////

            // Motion
            m_SystemInfo.GetObjectInfo(2, out objInfo);
            CreateYMCBoard(objInfo);

            // MultiAxes
            CreateMultiAxes_YMC();
            m_AxUpperHandler.UpdateAxisStatus();
            // IO
            m_SystemInfo.GetObjectInfo(6, out objInfo);
            m_IO = new MIO_YMC(objInfo);
            m_IO.OutputOn(oUHandler_Self_Vac_On);

            // Cylinder
            // UHandlerUDCyl
            CCylinderData cylData = new CCylinderData();
            cylData.CylinderType = ECylinderType.UP_DOWN;
            cylData.SolenoidType = ESolenoidType.DOUBLE_SOLENOID;
            cylData.UpSensor[0] = iUHandler_Up1;
            cylData.DownSensor[0] = iUHandler_Down1;
            cylData.Solenoid[0] = oUHandler_Up1;
            cylData.Solenoid[1] = oUHandler_Down1;

            m_SystemInfo.GetObjectInfo(100, out objInfo);
            CreateCylinder(objInfo, cylData, (int)EObjectCylinder.UHANDLER_UD, out m_UHandlerUDCyl);

            // UHandlerUDCyl2
            cylData = new CCylinderData();
            cylData.CylinderType = ECylinderType.UP_DOWN;
            cylData.SolenoidType = ESolenoidType.DOUBLE_SOLENOID;
            cylData.UpSensor[0] = iUHandler_Up1;
            cylData.DownSensor[0] = iUHandler_Down2;
            cylData.Solenoid[0] = oUHandler_Up1;
            cylData.Solenoid[1] = oUHandler_Down2;

            m_SystemInfo.GetObjectInfo(101, out objInfo);
            CreateCylinder(objInfo, cylData, (int)EObjectCylinder.UHANDLER_UD2, out m_UHandlerUDCyl2);

            // Vacuum
            // Stage1 Vacuum
            CVacuumData vacData = new CVacuumData();
            vacData.VacuumType = EVacuumType.SINGLE_VACUUM_WBLOW;
            vacData.Sensor[0] = iStage1_Vac_On;
            vacData.Solenoid[0] = oStage1_Vac_On;
            vacData.Solenoid[1] = oStage1_Vac_Off;

            m_SystemInfo.GetObjectInfo(150, out objInfo);
            CreateVacuum(objInfo, vacData, (int)EObjectVacuum.STAGE1, out m_Stage1Vac);

            // UHandler Self Vacuum
            vacData = new CVacuumData();
            vacData.VacuumType = EVacuumType.SINGLE_VACUUM_WBLOW;
            vacData.Sensor[0] = iUHandler_Self_Vac_On;
            vacData.Solenoid[0] = oUHandler_Self_Vac_On;
            vacData.Solenoid[1] = oUHandler_Self_Vac_Off;

            m_SystemInfo.GetObjectInfo(151, out objInfo);
            CreateVacuum(objInfo, vacData, (int)EObjectVacuum.UHANDLER_SELF, out m_UHandlerSelfVac);

            // Polygon Scanner Serial Com Port
            m_SystemInfo.GetObjectInfo(30, out objInfo);
            CreatePolygonSerialPort(objInfo, out m_PolygonComPort);

            CPolygonIni PolygonIni = new CPolygonIni();
            m_SystemInfo.GetObjectInfo(200, out objInfo);
            CreatePolygonScanner(objInfo, PolygonIni, (int)EObjectScanner.SCANNER1, m_PolygonComPort);

            // Vision System
            m_SystemInfo.GetObjectInfo(40, out objInfo);
            CreateVisionSystem(objInfo);
            // Vision Camera
            m_SystemInfo.GetObjectInfo(42, out objInfo);
            CreateVisionCamera(objInfo);
            // Vision Display
            m_SystemInfo.GetObjectInfo(46, out objInfo);
            MVisionView(objInfo);

            ////////////////////////////////////////////////////////////////////////
            // 2. Mechanical Layer
            ////////////////////////////////////////////////////////////////////////

            // Cassette Loader
            m_SystemInfo.GetObjectInfo(310, out objInfo);
            CreateMeElevator(objInfo);

            // Handler
            m_SystemInfo.GetObjectInfo(318, out objInfo);
            CreateMeUpperHandler(objInfo);

            m_SystemInfo.GetObjectInfo(319, out objInfo);
            CreateMeLowerHandler(objInfo);

            // Stage
            m_SystemInfo.GetObjectInfo(325, out objInfo);
            CreateMeStage(objInfo);


            // Vision 
            m_SystemInfo.GetObjectInfo(340, out objInfo);
            CreateVision(objInfo);

            ////////////////////////////////////////////////////////////////////////
            // 3. Control Layer
            ////////////////////////////////////////////////////////////////////////
            m_SystemInfo.GetObjectInfo(351, out objInfo);
            CreateCtrlLoader(objInfo);

            m_SystemInfo.GetObjectInfo(352, out objInfo);
            CreateCtrlPushPull(objInfo);

            m_SystemInfo.GetObjectInfo(353, out objInfo);
            CreateCtrlStage1(objInfo);

            m_SystemInfo.GetObjectInfo(354, out objInfo);
            CreateCtrlHandler(objInfo);

            ////////////////////////////////////////////////////////////////////////
            // 4. Process Layer
            ////////////////////////////////////////////////////////////////////////
            m_SystemInfo.GetObjectInfo(401, out objInfo);
            CreateTrsLoader(objInfo);

            m_SystemInfo.GetObjectInfo(402, out objInfo);
            CreateTrsPushPull(objInfo);

            m_SystemInfo.GetObjectInfo(403, out objInfo);
            CreateTrsStage1(objInfo);

            m_SystemInfo.GetObjectInfo(400, out objInfo);
            CreateTrsAutoManager(objInfo);

            // temporary set windows
            m_trsLoader.SetWindows_Form1(form1);
            m_trsPushPull.SetWindows_Form1(form1);
            m_trsStage1.SetWindows_Form1(form1);
            m_trsAutoManager.SetWindows_Form1(form1);

            ////////////////////////////////////////////////////////////////////////
            // 5. Set Data
            ////////////////////////////////////////////////////////////////////////
            SetSystemDataToComponent();
            SetModelDataToComponent();
            SetPositionDataToComponent();

            ////////////////////////////////////////////////////////////////////////
            // 6. Start Thread & System
            ////////////////////////////////////////////////////////////////////////
            m_YMC.ThreadStart();

            SetThreadChannel();
            StartThreads();



            return SUCCESS;
        }

        void InitDataFileNames(out CDBInfo dbInfo)
        {
            dbInfo = new CDBInfo();
        }

        int CreateYMCBoard(CObjectInfo objInfo)
        {
            CYaskawaRefComp refComp = new CYaskawaRefComp();
            CYaskawaData data = new CYaskawaData();

            m_YMC = new MYaskawa(objInfo, refComp, data);
            m_YMC.SetMPMotionData(m_DataManager.SystemData_Axis.MPMotionData);

#if !SIMULATION_MOTION
            int iResult = m_YMC.OpenController();
            if (iResult != SUCCESS) return iResult;
#endif

            return SUCCESS;
        }

        int CreateMultiAxes_YMC()
            {
            CObjectInfo objInfo;
            CMutliAxesYMCRefComp refComp = new CMutliAxesYMCRefComp();
            CMultiAxesYMCData data;
            int deviceNo;
            int[] axisList = new int[DEF_MAX_COORDINATE];
            int[] initArray = new int[DEF_MAX_COORDINATE];
            for(int i = 0; i < DEF_MAX_COORDINATE; i++)
                {
                initArray[i] = DEF_AXIS_NONE_ID;
                }

            refComp.Motion = m_YMC;

            // Loader
            deviceNo = (int)EYMC_Device.LOADER;
            Array.Copy(initArray, axisList, initArray.Length);
            axisList[DEF_Z] = (int)EYMC_Axis.LOADER_Z;
            data = new CMultiAxesYMCData(deviceNo, axisList);

            m_SystemInfo.GetObjectInfo(251, out objInfo);
            m_AxLoader = new MMultiAxes_YMC(objInfo, refComp, data);

            // PushPull
            deviceNo = (int)EYMC_Device.PUSHPULL;
            Array.Copy(initArray, axisList, initArray.Length);
            axisList[DEF_Y] = (int)EYMC_Axis.PUSHPULL_Y;
            data = new CMultiAxesYMCData(deviceNo, axisList);

            m_SystemInfo.GetObjectInfo(252, out objInfo);
            m_AxPushPull = new MMultiAxes_YMC(objInfo, refComp, data);

            // C1_CENTERING
            deviceNo = (int)EYMC_Device.C1_CENTERING;
            Array.Copy(initArray, axisList, initArray.Length);
            axisList[DEF_T] = (int)EYMC_Axis.C1_CENTERING_T;
            data = new CMultiAxesYMCData(deviceNo, axisList);

            m_SystemInfo.GetObjectInfo(253, out objInfo);
            m_AxCentering1 = new MMultiAxes_YMC(objInfo, refComp, data);

            // C1_ROTATE
            deviceNo = (int)EYMC_Device.C1_ROTATE;
            Array.Copy(initArray, axisList, initArray.Length);
            axisList[DEF_T] = (int)EYMC_Axis.C1_CHUCK_ROTATE_T;
            data = new CMultiAxesYMCData(deviceNo, axisList);

            m_SystemInfo.GetObjectInfo(254, out objInfo);
            m_AxRotate1 = new MMultiAxes_YMC(objInfo, refComp, data);

            // C1_CLEAN_NOZZLE
            deviceNo = (int)EYMC_Device.C1_CLEAN_NOZZLE;
            Array.Copy(initArray, axisList, initArray.Length);
            axisList[DEF_T] = (int)EYMC_Axis.C1_CLEAN_NOZZLE_T;
            data = new CMultiAxesYMCData(deviceNo, axisList);

            m_SystemInfo.GetObjectInfo(255, out objInfo);
            m_AxCleanNozzle1 = new MMultiAxes_YMC(objInfo, refComp, data);

            // C1_COAT_NOZZLE
            deviceNo = (int)EYMC_Device.C1_COAT_NOZZLE;
            Array.Copy(initArray, axisList, initArray.Length);
            axisList[DEF_T] = (int)EYMC_Axis.C1_COAT_NOZZLE_T;
            data = new CMultiAxesYMCData(deviceNo, axisList);

            m_SystemInfo.GetObjectInfo(256, out objInfo);
            m_AxCoatNozzle1 = new MMultiAxes_YMC(objInfo, refComp, data);

            // C2_CENTERING
            deviceNo = (int)EYMC_Device.C2_CENTERING;
            Array.Copy(initArray, axisList, initArray.Length);
            axisList[DEF_T] = (int)EYMC_Axis.C2_CENTERING_T;
            data = new CMultiAxesYMCData(deviceNo, axisList);

            m_SystemInfo.GetObjectInfo(257, out objInfo);
            m_AxCentering2 = new MMultiAxes_YMC(objInfo, refComp, data);

            // C2_ROTATE
            deviceNo = (int)EYMC_Device.C2_ROTATE;
            Array.Copy(initArray, axisList, initArray.Length);
            axisList[DEF_T] = (int)EYMC_Axis.C2_CHUCK_ROTATE_T;
            data = new CMultiAxesYMCData(deviceNo, axisList);

            m_SystemInfo.GetObjectInfo(258, out objInfo);
            m_AxRotate2 = new MMultiAxes_YMC(objInfo, refComp, data);

            // C2_CLEAN_NOZZLE
            deviceNo = (int)EYMC_Device.C2_CLEAN_NOZZLE;
            Array.Copy(initArray, axisList, initArray.Length);
            axisList[DEF_T] = (int)EYMC_Axis.C2_CLEAN_NOZZLE_T;
            data = new CMultiAxesYMCData(deviceNo, axisList);

            m_SystemInfo.GetObjectInfo(259, out objInfo);
            m_AxCleanNozzle2 = new MMultiAxes_YMC(objInfo, refComp, data);

            // C2_COAT_NOZZLE
            deviceNo = (int)EYMC_Device.C2_COAT_NOZZLE;
            Array.Copy(initArray, axisList, initArray.Length);
            axisList[DEF_T] = (int)EYMC_Axis.C2_COAT_NOZZLE_T;
            data = new CMultiAxesYMCData(deviceNo, axisList);

            m_SystemInfo.GetObjectInfo(260, out objInfo);
            m_AxCoatNozzle2 = new MMultiAxes_YMC(objInfo, refComp, data);

            // UHANDLER
            deviceNo = (int)EYMC_Device.UHANDLER;
            Array.Copy(initArray, axisList, initArray.Length);
            axisList[DEF_X] = (int)EYMC_Axis.UHANDLER_X;
            axisList[DEF_Z] = (int)EYMC_Axis.UHANDLER_Z;
            data = new CMultiAxesYMCData(deviceNo, axisList);

            m_SystemInfo.GetObjectInfo(261, out objInfo);
            m_AxUpperHandler = new MMultiAxes_YMC(objInfo, refComp, data);

            // LHANDLER
            deviceNo = (int)EYMC_Device.LHANDLER;
            Array.Copy(initArray, axisList, initArray.Length);
            axisList[DEF_X] = (int)EYMC_Axis.LHANDLER_X;
            axisList[DEF_Z] = (int)EYMC_Axis.LHANDLER_Z;
            data = new CMultiAxesYMCData(deviceNo, axisList);

            m_SystemInfo.GetObjectInfo(262, out objInfo);
            m_AxLowerHandler = new MMultiAxes_YMC(objInfo, refComp, data);

            // CAMERA1
            deviceNo = (int)EYMC_Device.CAMERA1;
            Array.Copy(initArray, axisList, initArray.Length);
            axisList[DEF_Z] = (int)EYMC_Axis.CAMERA1_Z;
            data = new CMultiAxesYMCData(deviceNo, axisList);

            m_SystemInfo.GetObjectInfo(263, out objInfo);
            m_AxCamera1 = new MMultiAxes_YMC(objInfo, refComp, data);

            // LASER1
            deviceNo = (int)EYMC_Device.LASER1;
            Array.Copy(initArray, axisList, initArray.Length);
            axisList[DEF_Z] = (int)EYMC_Axis.LASER1_Z;
            data = new CMultiAxesYMCData(deviceNo, axisList);

            m_SystemInfo.GetObjectInfo(264, out objInfo);
            m_AxLaser1 = new MMultiAxes_YMC(objInfo, refComp, data);

            return SUCCESS;
        }

        int CreateCylinder(CObjectInfo objInfo, CCylinderData data, int objIndex, out ICylinder pCylinder)
        {
            int iResult = SUCCESS;

            data.Time = m_DataManager.SystemData_Cylinder.CylinderTimer[objIndex];
            pCylinder = new MCylinder(objInfo, m_IO, data);

            return iResult;
        }

        int CreateVacuum(CObjectInfo objInfo, CVacuumData data, int objIndex, out IVacuum pVacuum)
        {
            int iResult = SUCCESS;

            data.Time = m_DataManager.SystemData_Vacuum.VacuumTimer[objIndex];
            pVacuum = new MVacuum(objInfo, m_IO, data);

            return iResult;
        }

        int CreateVisionSystem(CObjectInfo objInfo)
        {
#if SIMULATION_VISION
                return SUCCESS;
#endif

            int iResult = 0;
            // Vision System 생성
            m_VisionSystem = new MVisionSystem(objInfo);
            // GigE Cam초기화 & MIL 초기화
            iResult = m_VisionSystem.Initialize();

            if (iResult != SUCCESS) return iResult;

            // GigE Camera 개수 확인
            int iGetCamNum = m_VisionSystem.GetCamNum();
            if (iGetCamNum != DEF_MAX_CAMERA_NO) return ERR_VISION_ERROR;

            return SUCCESS;
        }

        int CreateVisionCamera(CObjectInfo objInfo)
        {
#if SIMULATION_VISION
                return SUCCESS;
#endif
            m_VisionCamera = new MVisionCamera[DEF_MAX_CAMERA_NO];

            // Camera & View 를 생성함.
            for (int iIndex = 0; iIndex < DEF_MAX_CAMERA_NO; iIndex++)
            {
                // Camera를 생성함.
                m_VisionCamera[iIndex] = new MVisionCamera(objInfo);
                // Vision Library MIL
                m_VisionCamera[iIndex].SetMil_ID(m_VisionSystem.GetMilSystem());
                // Camera 초기화
                m_VisionCamera[iIndex].Initialize(iIndex, m_VisionSystem.GetSystem());
                
            }

            return SUCCESS;
        }
        int MVisionView(CObjectInfo objInfo)
        {
#if SIMULATION_VISION
                return SUCCESS;
#endif
            m_VisionView = new MVisionView[DEF_MAX_CAMERA_NO];

            // Camera & View 를 생성함.
            for (int iIndex = 0; iIndex < DEF_MAX_CAMERA_NO; iIndex++)
            {
                // Display View 생성함.
                m_VisionView[iIndex] = new MVisionView(objInfo);
                // Vision Library MIL
                m_VisionView[iIndex].SetMil_ID(m_VisionSystem.GetMilSystem());
                // Display 초기화
                m_VisionView[iIndex].Initialize(iIndex, m_VisionCamera[iIndex]);

            }
            return SUCCESS;
        }

        void CreateVision(CObjectInfo objInfo)
        {
#if !SIMULATION_VISION
            bool VisionHardwareCheck = true;
            if(m_VisionSystem.m_iResult != SUCCESS)
            {
                VisionHardwareCheck = false;
            }
            CVisionData data = new CVisionData();
            CVisionRefComp refComp = new CVisionRefComp();

            // 생성된 Vision System,Camera, View 를 RefComp로 연결
            refComp.System = m_VisionSystem;

            for (int iIndex = 0; iIndex < DEF_MAX_CAMERA_NO; iIndex++)
            {
                if(m_VisionCamera[iIndex].m_iResult != SUCCESS || m_VisionView[iIndex].m_iResult != SUCCESS)
                {
                    VisionHardwareCheck = false;
                    break;
                }
                refComp.Camera[iIndex] = m_VisionCamera[iIndex];
                refComp.View[iIndex]   = m_VisionView[iIndex];

                // Display와 Camera와 System을 연결
                refComp.Camera[iIndex].SelectView(refComp.View[iIndex]);
                refComp.System.SelectCamera(refComp.Camera[iIndex]);
                refComp.System.SelectView(refComp.View[iIndex]);
            }

            m_Vision = new MVision(objInfo, refComp, data);

            if(VisionHardwareCheck==false)
            {
                m_Vision.m_bSystemInit = false;
            }
            else
            {
                m_Vision.m_bSystemInit = true;
            }

            // Cam Live Set
            m_Vision.LiveVideo(PRE__CAM);
            m_Vision.LiveVideo(FINE_CAM);

            // Pattern Model Data Read & Apply
            CModelData pModelData;
            m_DataManager.ViewModelData("Default", out pModelData);
            m_DataManager.m_ModelData = pModelData;

            m_Vision.ReLoadPatternMark(PRE__CAM, PATTERN_A, m_DataManager.m_ModelData.MacroPatternA);
            m_Vision.ReLoadPatternMark(PRE__CAM, PATTERN_B, m_DataManager.m_ModelData.MacroPatternB);
            m_Vision.ReLoadPatternMark(FINE_CAM, PATTERN_A, m_DataManager.m_ModelData.MicroPatternA);
            m_Vision.ReLoadPatternMark(FINE_CAM, PATTERN_B, m_DataManager.m_ModelData.MicroPatternB);

#endif

        }

        void CreateCtrlStage1(CObjectInfo objInfo)
        {
            CCtrlStage1RefComp refComp = new CCtrlStage1RefComp();
            CCtrlStage1Data data = new CCtrlStage1Data();

            m_ctrlStage1 = new MCtrlStage1(objInfo, refComp, data);
        }

        void CreateCtrlLoader(CObjectInfo objInfo)
        {
            CCtrlLoaderRefComp refComp = new CCtrlLoaderRefComp();
            CCtrlLoaderData data = new CCtrlLoaderData();

            m_ctrlLoader = new MCtrlLoader(objInfo, refComp, data);
        }

        void CreateCtrlHandler(CObjectInfo objInfo)
        {
            CCtrlHandlerRefComp refComp = new CCtrlHandlerRefComp();
            CCtrlHandlerData data = new CCtrlHandlerData();

            m_ctrlHandler = new MCtrlHandler(objInfo, refComp, data);
        }

        void CreateCtrlPushPull(CObjectInfo objInfo)
        {
            CCtrlPushPullRefComp refComp = new CCtrlPushPullRefComp();
            CCtrlPushPullData data = new CCtrlPushPullData();

            m_ctrlPushPull = new MCtrlPushPull(objInfo, refComp, data);
        }

        void CreateTrsAutoManager(CObjectInfo objInfo)
        {
            CTrsAutoManagerRefComp refComp = new CTrsAutoManagerRefComp();
            refComp.trsLoader = m_trsLoader;
            refComp.trsPushPull = m_trsPushPull;

            CTrsAutoManagerData data = new CTrsAutoManagerData();

            m_trsAutoManager = new MTrsAutoManager(objInfo, TrsAutoManager, refComp, data);
        }

        void CreateTrsLoader(CObjectInfo objInfo)
        {
            CTrsLoaderRefComp refComp = new CTrsLoaderRefComp();
            refComp.ctrlLoader = m_ctrlLoader;

            CTrsLoaderData data = new CTrsLoaderData();

            m_trsLoader = new MTrsLoader(objInfo, TrsLoader, refComp, data);
        }

        void CreateTrsPushPull(CObjectInfo objInfo)
        {
            CTrsPushPullRefComp refComp = new CTrsPushPullRefComp();
            refComp.ctrlPushPull = m_ctrlPushPull;

            CTrsPushPullData data = new CTrsPushPullData();

            m_trsPushPull = new MTrsPushPull(objInfo, TrsLoader, refComp, data);
        }

        void CreateTrsStage1(CObjectInfo objInfo)
        {
            CTrsStage1RefComp refComp = new CTrsStage1RefComp();
            refComp.ctrlStage1 = m_ctrlStage1;

            CTrsStage1Data data = new CTrsStage1Data();

            m_trsStage1 = new MTrsStage1(objInfo, TrsLoader, refComp, data);
        }

        void CreatePolygonScanner(CObjectInfo objInfo, CPolygonIni PolygonIni, int objIndex, ISerialPort m_ComPort)
        {
            m_DataManager.SystemData_Scanner.Scanner[objIndex] = PolygonIni;

            m_DataManager.SystemData_Scanner.Scanner[objIndex].strIP = "192.168.1.161";
            m_DataManager.SystemData_Scanner.Scanner[objIndex].strPort = "70";

            m_Scanner[objIndex] = new MPolygonScanner(objInfo, m_DataManager.SystemData_Scanner.Scanner[objIndex], objIndex, m_ComPort);
        }

        void CreatePolygonSerialPort(CObjectInfo objInfo, out ISerialPort pComport)
        {
            // Polygon Scanner Serial Port 
            string PortName = "COM3";
            int BaudRate = 57600;
            Parity _Parity = Parity.None;
            int DataBits = 8;
            StopBits _StopBits = StopBits.One;

            CSerialPortData SerialCom = new CSerialPortData(PortName, BaudRate, _Parity, DataBits, _StopBits);

            pComport = new MSerialPort(objInfo, SerialCom);

        }

        void SetThreadChannel()
        {
            // AutoManager
            m_trsAutoManager.LinkThread(TrsSelfMessage, m_trsAutoManager);
            m_trsAutoManager.LinkThread(TrsLoader, m_trsLoader);
            m_trsAutoManager.LinkThread(TrsPushPull, m_trsPushPull);
            m_trsAutoManager.LinkThread(TrsStage1, m_trsStage1);

            // Loader
            m_trsLoader.LinkThread(TrsSelfMessage, m_trsLoader);
            m_trsLoader.LinkThread(TrsAutoManager, m_trsAutoManager);
            m_trsLoader.LinkThread(TrsPushPull, m_trsPushPull);
            m_trsLoader.LinkThread(TrsStage1, m_trsStage1);

            // PushPull
            m_trsPushPull.LinkThread(TrsSelfMessage, m_trsPushPull);
            m_trsPushPull.LinkThread(TrsAutoManager, m_trsAutoManager);
            m_trsPushPull.LinkThread(TrsLoader, m_trsLoader);
            m_trsPushPull.LinkThread(TrsStage1, m_trsStage1);

            // Stage1
            m_trsStage1.LinkThread(TrsSelfMessage, m_trsStage1);
            m_trsStage1.LinkThread(TrsAutoManager, m_trsAutoManager);
            m_trsStage1.LinkThread(TrsLoader, m_trsLoader);
            m_trsStage1.LinkThread(TrsPushPull, m_trsPushPull);

        }

        void StartThreads()
        {
            m_trsLoader.ThreadStart();
            m_trsPushPull.ThreadStart();
            m_trsStage1.ThreadStart();
            m_trsAutoManager.ThreadStart();
        }

        public void StopThreads()
        {
            m_trsLoader.ThreadStop();
            m_trsPushPull.ThreadStop();
            m_trsStage1.ThreadStop();
            m_trsAutoManager.ThreadStop();
        }

        public int SaveSystemData(CSystemData system = null, CSystemData_Axis systemAxis = null,
            CSystemData_Cylinder systemCylinder = null, CSystemData_Vacuum systemVacuum = null,
            CSystemData_Scanner systemScanner = null)
        {
            int iResult;

            // save
            iResult = m_DataManager.SaveSystemData(system, systemAxis, systemCylinder, systemVacuum, systemScanner);
            if (iResult != SUCCESS) return SUCCESS;

            // set
            SetSystemDataToComponent();

            return SUCCESS;
        }

        private void SetSystemDataToComponent()
        {
            m_DataManager.LoadSystemData();
            m_DataManager.LoadModelList();

            MLWDicer.bInSfaTest = m_DataManager.SystemData.UseInSfaTest;
            MLWDicer.bUseOnline = m_DataManager.SystemData.UseOnLineUse;

            // set system data to each component

            //////////////////////////////////////////////////////////////////
            // Hardware Layer

            //////////////////////////////////////////////////////////////////
            // Mechanical Layer

            // MeElevator
            CMeElevatorData meElevatorData;
            m_MeElevator.GetData(out meElevatorData);
            meElevatorData.ElevatorZone.SafetyPos = m_DataManager.SystemData.MAxSafetyPos.Elevator_Pos;
            m_MeElevator.SetData(meElevatorData);

            // MeHandler
            CMeHandlerData meHandlerData;
            m_MeUpperHandler.GetData(out meHandlerData);
            meHandlerData.HandlerZone.SafetyPos = m_DataManager.SystemData.MAxSafetyPos.UHandler_Pos;
            m_MeUpperHandler.SetData(meHandlerData);

            m_MeUpperHandler.GetData(out meHandlerData);
            meHandlerData.HandlerZone.SafetyPos = m_DataManager.SystemData.MAxSafetyPos.UHandler_Pos;
            m_MeUpperHandler.SetData(meHandlerData);

            //////////////////////////////////////////////////////////////////
            // Control Layer

            //////////////////////////////////////////////////////////////////
            // Process Layer

        }

        public int SaveModelData(CModelData modelData)
        {
            int iResult;

            // save
            iResult = m_DataManager.SaveModelData(modelData);
            if (iResult != SUCCESS) return SUCCESS;

            // set
            SetModelDataToComponent();

            return SUCCESS;
        }

        public void SetModelDataToComponent()
        {
            m_DataManager.ChangeModel(m_DataManager.SystemData.ModelName);

            // set model data to each component

            //////////////////////////////////////////////////////////////////
            // Hardware Layer

            //////////////////////////////////////////////////////////////////
            // Mechanical Layer

            // MMeHandler
            m_MeUpperHandler.SetCylUseFlag(m_DataManager.ModelData.MeUH_UseMainCylFlag,
                m_DataManager.ModelData.MeUH_UseSubCylFlag, m_DataManager.ModelData.MeUH_UseGuideCylFlag);
            m_MeUpperHandler.SetVccUseFlag(m_DataManager.ModelData.MeUH_UseVccFlag);

            m_MeLowerHandler.SetCylUseFlag(m_DataManager.ModelData.MeLH_UseMainCylFlag,
                m_DataManager.ModelData.MeLH_UseSubCylFlag, m_DataManager.ModelData.MeLH_UseGuideCylFlag);
            m_MeLowerHandler.SetVccUseFlag(m_DataManager.ModelData.MeLH_UseVccFlag);

            //////////////////////////////////////////////////////////////////
            // Control Layer

            //////////////////////////////////////////////////////////////////
            // Process Layer


        }

        public void SetPositionDataToComponent(EUnitObject unit = EUnitObject.ALL)
        {
            m_DataManager.LoadPositionData(true, unit);
            m_DataManager.LoadPositionData(false, unit);
            m_DataManager.GenerateModelPosition();

            CPositionData FixedPos = m_DataManager.FixedPos;
            CPositionData ModelPos = m_DataManager.ModelPos;
            CPositionData OffsetPos = m_DataManager.OffsetPos;

            // set position data to each component

            //////////////////////////////////////////////////////////////////
            // Hardware Layer

            //////////////////////////////////////////////////////////////////
            // Mechanical Layer

            // MMeHandler
            m_MeUpperHandler.SetHandlerPosition(FixedPos.UHandlerPos, ModelPos.UHandlerPos, OffsetPos.UHandlerPos);
            m_MeLowerHandler.SetHandlerPosition(FixedPos.LHandlerPos, ModelPos.LHandlerPos, OffsetPos.LHandlerPos);

            //////////////////////////////////////////////////////////////////
            // Control Layer

            //////////////////////////////////////////////////////////////////
            // Process Layer

        }

        public bool GetKeyPad(string StrCurrent, out string strModify)
        {
            FormKeyPad KeyPad = new FormKeyPad();
            KeyPad.SetValue(StrCurrent);
            KeyPad.ShowDialog();

            if (KeyPad.DialogResult == DialogResult.OK)
            {
                if (KeyPad.ModifyNo.Text == "")
                {
                    strModify = "0";
                }
                else
                {
                    strModify = KeyPad.ModifyNo.Text;
                }
            }
            else
            {
                strModify = StrCurrent;
                KeyPad.Dispose();
                return false;
            }
            KeyPad.Dispose();
            return true;
        }

        public bool GetKeyboard(out string strModify)
        {
            FormKeyBoard Keyboard = new FormKeyBoard();
            Keyboard.ShowDialog();

            if (Keyboard.DialogResult == DialogResult.OK)
            {
                strModify = Keyboard.PresentNo.Text;
            }
            else
            {
                strModify = "";
                Keyboard.Dispose();
                return false;
            }
            Keyboard.Dispose();
            return true;
        }

        public bool DisplayMsg(string strMsg)
        {
            FormMessageBox Msg = new FormMessageBox();
            Msg.SetText(strMsg);
            Msg.ShowDialog();

            if (Msg.DialogResult == DialogResult.OK)
            {
                Msg.Dispose();
                return true;
            }
            else
            {
                Msg.Dispose();
                return false;
            }
        }

        void CreateMeElevator(CObjectInfo objInfo)
        {
            CMeElevatorRefComp refComp = new CMeElevatorRefComp();
            CMeElevatorData data = new CMeElevatorData();

            refComp.IO = m_IO;
            refComp.AxElevator = m_AxLoader;            

            data.InDetectWafer = iUHandler_PanelDetect;

            data.ElevatorZone.UseSafetyMove[DEF_Z] = true;
            data.ElevatorZone.Axis[DEF_Z].ZoneAddr[(int)EHandlerZAxZone.SAFETY] = 111; // need updete io address

            m_MeElevator = new MMeElevator(objInfo, refComp, data);

        }

        void CreateMeUpperHandler(CObjectInfo objInfo)
        {
            CMeHandlerRefComp refComp = new CMeHandlerRefComp();
            CMeHandlerData data = new CMeHandlerData();

            refComp.IO = m_IO;
            refComp.AxHandler = m_AxUpperHandler;
            refComp.Vacuum[(int)EHandlerVacuum.SELF] = m_UHandlerSelfVac;

            data.HandlerType[DEF_X] = EHandlerType.AXIS;
            data.HandlerType[DEF_Z] = EHandlerType.AXIS;

            data.InDetectObject = iUHandler_PanelDetect;

            data.HandlerZone.UseSafetyMove[DEF_Z] = true;
            data.HandlerZone.Axis[DEF_Z].ZoneAddr[(int)EHandlerZAxZone.SAFETY] = 111; // need updete io address

            m_MeUpperHandler = new MMeHandler(objInfo, refComp, data);
        }

        void CreateMeLowerHandler(CObjectInfo objInfo)
        {
            CMeHandlerRefComp refComp = new CMeHandlerRefComp();
            CMeHandlerData data = new CMeHandlerData();

            refComp.IO = m_IO;
            refComp.AxHandler = m_AxLowerHandler;
            refComp.Vacuum[(int)EHandlerVacuum.SELF] = m_LHandlerSelfVac;

            data.HandlerType[DEF_X] = EHandlerType.AXIS;
            data.HandlerType[DEF_Z] = EHandlerType.AXIS;

            data.InDetectObject = iUHandler_PanelDetect;

            data.HandlerZone.UseSafetyMove[DEF_Z] = true;
            data.HandlerZone.Axis[DEF_Z].ZoneAddr[(int)EHandlerZAxZone.SAFETY] = 111; // need updete io address

            m_MeLowerHandler = new MMeHandler(objInfo, refComp, data);
        }

        void CreateMeStage(CObjectInfo objInfo)
        {
            CMeStageRefComp refComp = new CMeStageRefComp();
            CMeStageData data = new CMeStageData();

            refComp.IO = m_IO;
            refComp.AxStage = m_AxStage1;
            refComp.Vacuum[(int)EStageVacuum.SELF] = m_Stage1Vac;
            
            data.InDetectObject = iUHandler_PanelDetect;

            data.StageZone.UseSafetyMove[DEF_Z] = true;
            data.StageZone.Axis[DEF_Z].ZoneAddr[(int)EHandlerZAxZone.SAFETY] = 111; // need updete io address

            m_MeStage = new MMeStage(objInfo, refComp, data);
        }

    }
}
