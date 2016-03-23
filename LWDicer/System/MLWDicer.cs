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

using static LWDicer.Control.DEF_SerialPort;
using static LWDicer.Control.DEF_PolygonScanner;

namespace LWDicer.Control
{
    public class MLWDicer : MObject, IDisposable
    {

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
        public MMultiAxes_YMC m_AxHandler1;
        public MMultiAxes_YMC m_AxHandler2;
        public MMultiAxes_YMC m_AxCamera1;
        public MMultiAxes_YMC m_AxLaser1;

        // IO
        public IIO m_IO { get; private set; }

        // Cylinder
        public ICylinder m_UHandlerUDCyl;
        public ICylinder m_UHandlerUDCyl2;

        // Vacuum
        public IVacuum m_Stage1Vac;
        public IVacuum m_Stage2Vac;

        // Serial
        public ISerialPort m_PolygonComPort;

        // Scanner
        public IPolygonScanner[] m_Scanner = new IPolygonScanner[(int)EObjectScanner.MAX_OBJ];

        ///////////////////////////////////////////////////////////////////////
        // Mechanical Layer

        public MVision m_Vision { get; set; }

        ///////////////////////////////////////////////////////////////////////
        // Control Layer
        public MCtrlLoader m_ctrlLoader { get; private set; }
        public MCtrlPushPull m_ctrlPushPull { get; private set; }
        public MCtrlStage1 m_ctrlStage1 { get; private set; }

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

        public int Initialize(CMainFrame form1 = null)
        {
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
            m_AxHandler1.UpdateAxisStatus();
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

            // Stage2 Vacuum
            vacData = new CVacuumData();
            vacData.VacuumType = EVacuumType.SINGLE_VACUUM_WBLOW;
            vacData.Sensor[0] = iStage2_Vac_On;
            vacData.Solenoid[0] = oStage2_Vac_On;
            vacData.Solenoid[1] = oStage2_Vac_Off;

            m_SystemInfo.GetObjectInfo(151, out objInfo);
            CreateVacuum(objInfo, vacData, (int)EObjectVacuum.STAGE2, out m_Stage2Vac);

            // Polygon Scanner Serial Com Port
            m_SystemInfo.GetObjectInfo(30, out objInfo);
            CreatePolygonSerialPort(objInfo, out m_PolygonComPort);

            CPolygonIni PolygonIni = new CPolygonIni();
            m_SystemInfo.GetObjectInfo(200, out objInfo);
            CreatePolygonScanner(objInfo, PolygonIni, (int)EObjectScanner.SCANNER1, m_PolygonComPort);

            m_Scanner[0].LSEPortOpen();

            ////////////////////////////////////////////////////////////////////////
            // 2. Mechanical Layer
            ////////////////////////////////////////////////////////////////////////

            m_SystemInfo.GetObjectInfo(9, out objInfo);
            CreateVision(objInfo);

            CMainFrame.LWDicer.m_Vision.InitialLocalView(PRE__CAM, CMainFrame.MainFrame.m_FormManualOP.VisionView1.Handle);
           

            CMainFrame.LWDicer.m_Vision.LiveVideo(PRE__CAM);
            CMainFrame.LWDicer.m_Vision.LiveVideo(FINE_CAM);
            
            ////////////////////////////////////////////////////////////////////////
            // 3. Control Layer
            ////////////////////////////////////////////////////////////////////////
            m_SystemInfo.GetObjectInfo(351, out objInfo);
            CreateCtrlLoader(objInfo);

            m_SystemInfo.GetObjectInfo(352, out objInfo);
            CreateCtrlPushPull(objInfo);

            m_SystemInfo.GetObjectInfo(353, out objInfo);
            CreateCtrlStage1(objInfo);

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
            SetAllParameterToComponent();

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
            CYaskawaData data = m_DataManager.m_SystemData.YaskawaData;

            m_YMC = new MYaskawa(objInfo, refComp, data);
#if SIMULATION_MOTION
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
                initArray[i] = DEF_AXIS_NON_ID;
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

            // HANDLER1
            deviceNo = (int)EYMC_Device.HANDLER1;
            Array.Copy(initArray, axisList, initArray.Length);
            axisList[DEF_Y] = (int)EYMC_Axis.HANDLER1_Y;
            axisList[DEF_Z] = (int)EYMC_Axis.HANDLER1_Z;
            data = new CMultiAxesYMCData(deviceNo, axisList);

            m_SystemInfo.GetObjectInfo(261, out objInfo);
            m_AxHandler1 = new MMultiAxes_YMC(objInfo, refComp, data);

            // HANDLER2
            deviceNo = (int)EYMC_Device.HANDLER2;
            Array.Copy(initArray, axisList, initArray.Length);
            axisList[DEF_Y] = (int)EYMC_Axis.HANDLER2_Y;
            axisList[DEF_Z] = (int)EYMC_Axis.HANDLER2_Z;
            data = new CMultiAxesYMCData(deviceNo, axisList);

            m_SystemInfo.GetObjectInfo(262, out objInfo);
            m_AxHandler2 = new MMultiAxes_YMC(objInfo, refComp, data);

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

            data.Time = m_DataManager.m_SystemData.CylinderTimer[objIndex];
            pCylinder = new MCylinder(objInfo, m_IO, data);

            return iResult;
        }

        int CreateVacuum(CObjectInfo objInfo, CVacuumData data, int objIndex, out IVacuum pVacuum)
        {
            int iResult = SUCCESS;

            data.Time = m_DataManager.m_SystemData.VacuumTimer[objIndex];
            pVacuum = new MVacuum(objInfo, m_IO, data);

            return iResult;
        }

        void CreateVision(CObjectInfo objInfo)
        {
            CVisionData data = new CVisionData();

            m_Vision = new MVision(objInfo, data);
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
            data.bInSfaTest = false;
            data.bUseOnline = false;

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
            m_DataManager.m_SystemData.Scanner[objIndex] = PolygonIni;

            m_DataManager.m_SystemData.Scanner[objIndex].strIP = "192.168.1.161";
            m_DataManager.m_SystemData.Scanner[objIndex].strPort = "70";

            m_Scanner[objIndex] = new MPolygonScanner(objInfo, m_DataManager.m_SystemData.Scanner[objIndex], objIndex, m_ComPort);
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
            m_trsLoader.Stop();
            m_trsPushPull.Stop();
            m_trsStage1.Stop();
            m_trsAutoManager.Stop();
        }

        void SetAllParameterToComponent()
        {

        }

        void SetModelDataToComponent()
        {

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
    }
}
