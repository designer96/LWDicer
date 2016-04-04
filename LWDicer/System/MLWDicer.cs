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

using static LWDicer.Control.DEF_Cylinder;
using static LWDicer.Control.DEF_Vacuum;
using static LWDicer.Control.DEF_Vision;

using static LWDicer.Control.DEF_SerialPort;
using static LWDicer.Control.DEF_PolygonScanner;

namespace LWDicer.Control
{
    public class MLWDicer : MObject, IDisposable
    {
        public const int ERR_SYSTEM_FAIL_OPEN_YMC = 1;
        public const int ERR_SYSTEM_FAIL_SET_TIMEOUT = 2;

        // Common Class
        public MSystemInfo m_SystemInfo { get; private set; }
        public MDataManager m_DataManager { get; private set; }

        // Hardware Layer
        public UInt32[] m_hYMC = new UInt32[DEF_MAX_YMC_BOARD];

        public IIO m_IO { get; private set; }

        public ICylinder m_UHandlerUDCyl;
        public ICylinder m_UHandlerUDCyl2;

        public IVacuum m_Stage1Vac;
        public IVacuum m_Stage2Vac;

        public ISerialPort m_PolygonComPort;

        public IPolygonScanner[] m_Scanner = new IPolygonScanner[(int)EObjectScanner.MAX_OBJ];

        // Mechanical Layer

        public MVision m_Vision { get; set; }

        // Control Layer
        public MCtrlLoader m_ctrlLoader { get; private set; }
        public MCtrlPushPull m_ctrlPushPull { get; private set; }
        public MCtrlStage1 m_ctrlStage1 { get; private set; }

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

            // yaskawa
            for (int i = 0; i < DEF_MAX_YMC_BOARD; i++)
            {
                if (m_hYMC[i] == 0) continue;
                uint rc = CMotionAPI.ymcCloseController(m_hYMC[i]);
                if (rc != CMotionAPI.MP_SUCCESS)
                {
                    string str = String.Format("Error ymcCloseController Board {0} \nErrorCode [ 0x{1} ]", i, rc.ToString("X"));
                    WriteLog(str, ELogType.Debug, ELogWType.Error, true);
                    MessageBox.Show(str);
                }
            }
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

            // YMC
            CreateYMCBoard();

            // IO
            m_SystemInfo.GetObjectInfo(2, out objInfo);
            m_IO = new MIO_YMC(objInfo, m_hYMC[0]);
            m_IO.OutputOn(oUHandler_Self_Vac_On);

            // Motion

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
            m_SystemInfo.GetObjectInfo(10, out objInfo);
            CreatePolygonSerialPort(objInfo, out m_PolygonComPort);

            CPolygonIni PolygonIni = new CPolygonIni();
            m_SystemInfo.GetObjectInfo(200, out objInfo);
            CreatePolygonScanner(objInfo, PolygonIni, (int)EObjectScanner.SCANNER1, m_PolygonComPort);

            ////////////////////////////////////////////////////////////////////////
            // 2. Mechanical Layer
            ////////////////////////////////////////////////////////////////////////

            // Vision 
            m_SystemInfo.GetObjectInfo(320, out objInfo);
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
            SetThreadChannel();
            StartThreads();

            return SUCCESS;
        }

        void InitDataFileNames(out CDBInfo dbInfo)
        {
            dbInfo = new CDBInfo();
        }

        int CreateYMCBoard()
        {
#if SIMULATION_MOTION
            return SUCCESS;
#endif
            UInt32 rc;
            CMotionAPI.COM_DEVICE ComDevice;

            for(int i = 0; i < DEF_MAX_YMC_BOARD; i++)
            {
                //============================================================================ To Contents of Processing
                // Sets the ymcOpenController parameters.		
                //============================================================================
                ComDevice.ComDeviceType = (UInt16)CMotionAPI.ApiDefs.COMDEVICETYPE_PCI_MODE;
                ComDevice.PortNumber = Convert.ToUInt16(i+1);
                ComDevice.CpuNumber = 1;    //cpuno;
                ComDevice.NetworkNumber = 0;
                ComDevice.StationNumber = 0;
                ComDevice.UnitNumber = 0;
                ComDevice.IPAddress = "";
                ComDevice.Timeout = 10000;

                rc = CMotionAPI.ymcOpenController(ref ComDevice, ref m_hYMC[i]);
                if (rc != CMotionAPI.MP_SUCCESS)
                {
                    string str = String.Format("Error ymcOpenController Board {0} \nErrorCode [ 0x{1} ]", i, rc.ToString("X"));
                    WriteLog(str, ELogType.Debug, ELogWType.Error, true);
                    MessageBox.Show(str);
                    return GenerateErrorCode(ERR_SYSTEM_FAIL_OPEN_YMC);
                }

                //============================================================================ To Contents of Processing
                // Sets the motion API timeout. 		
                //============================================================================
                rc = CMotionAPI.ymcSetAPITimeoutValue(30000);
                if (rc != CMotionAPI.MP_SUCCESS)
                {
                    string str = String.Format("Error ymcSetAPITimeoutValue \nErrorCode [ 0x{0} ]", rc.ToString("X"));
                    WriteLog(str, ELogType.Debug, ELogWType.Error, true);
                    MessageBox.Show(str);
                    return GenerateErrorCode(ERR_SYSTEM_FAIL_SET_TIMEOUT);
                }
            }
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

            CMainFrame.LWDicer.m_Vision.InitialLocalView(PRE__CAM, CMainFrame.MainFrame.m_FormManualOP.VisionView1.Handle);
            CMainFrame.LWDicer.m_Vision.LiveVideo(PRE__CAM);
            CMainFrame.LWDicer.m_Vision.LiveVideo(FINE_CAM);

            CModelData pModelData;
            CMainFrame.LWDicer.m_DataManager.ViewModel("Default", out pModelData);
            CMainFrame.LWDicer.m_DataManager.m_ModelData = pModelData;

            CMainFrame.LWDicer.m_Vision.ReLoadPatternMark(PRE__CAM, PATTERN_A, CMainFrame.LWDicer.m_DataManager.m_ModelData.MacroPatternA);
            CMainFrame.LWDicer.m_Vision.ReLoadPatternMark(PRE__CAM, PATTERN_B, CMainFrame.LWDicer.m_DataManager.m_ModelData.MacroPatternB);
            CMainFrame.LWDicer.m_Vision.ReLoadPatternMark(FINE_CAM, PATTERN_A, CMainFrame.LWDicer.m_DataManager.m_ModelData.MicroPatternA);
            CMainFrame.LWDicer.m_Vision.ReLoadPatternMark(FINE_CAM, PATTERN_B, CMainFrame.LWDicer.m_DataManager.m_ModelData.MicroPatternB);
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

            m_DataManager.m_SystemData.Scanner[objIndex].strIP = "192.168.22.60";
            m_DataManager.m_SystemData.Scanner[objIndex].strPort = "21";
            
            m_Scanner[objIndex] = new MPolygonScanner(objInfo, m_DataManager.m_SystemData.Scanner[objIndex], objIndex, m_ComPort);
        }

        void CreatePolygonSerialPort(CObjectInfo objInfo, out ISerialPort pComport)
        {
            // Polygon Scanner Serial Port 
            string PortName = "COM1";
            int BaudRate = 115200;
            Parity _Parity = Parity.Even;
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
            m_trsLoader.Start();
            m_trsPushPull.Start();
            m_trsStage1.Start();
            m_trsAutoManager.Start();
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
