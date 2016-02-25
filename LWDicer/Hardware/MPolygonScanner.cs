using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

using System.Runtime.InteropServices;
using System.IO.Ports;

using static LWDicer.Control.DEF_Common;
using static LWDicer.Control.DEF_Error;
using static LWDicer.Control.DEF_PolygonScanner;
using static LWDicer.Control.DEF_DataManager;
using static LWDicer.Control.DEF_SerialPort;


namespace LWDicer.Control
{
    class MPolygonScanner : MObject, IPolygonScanner
    {
        public static ISerialPort m_COM;

        public MPolygonScanner(CObjectInfo objInfo, CPolygonParameter PolygonPara )
            : base(objInfo)
        {
            LoadPolygonPara(PolygonPara);

            InitializeSerial();

            LSEPortOpen();

        }

        public void LoadPolygonPara(CPolygonParameter PolygonPara)
        {
            string section = "";
            string key = "";
            string value = "";
            string filepath = @PATH_CONFIG_INI;

            section = "Job Settings";

            key = "InScanResolution";
            value = GetValue(section, key, filepath);
            PolygonPara.InScanResolution = Convert.ToDouble(value);

            key = "CrossScanResolution";
            value = GetValue(section, key, filepath);
            PolygonPara.CrossScanResolution = Convert.ToDouble(value);

            key = "InScanOffset";
            value = GetValue(section, key, filepath);
            PolygonPara.InScanOffset = Convert.ToDouble(value);

            key = "StopMotorBetweenJobs";
            value = GetValue(section, key, filepath);
            PolygonPara.StopMotorBetweenJobs = Convert.ToInt16(value);

            key = "PixInvert";
            value = GetValue(section, key, filepath);
            PolygonPara.PixInvert = Convert.ToInt16(value);

            key = "JobStartBufferTime";
            value = GetValue(section, key, filepath);
            PolygonPara.JobStartBufferTime = Convert.ToInt16(value);

            key = "PrecedingBlankLines";
            value = GetValue(section, key, filepath);
            PolygonPara.PrecedingBlankLines = Convert.ToInt16(value);

            section = "Laser Configuration";

            key = "LaserOperationMode";
            value = GetValue(section, key, filepath);
            PolygonPara.LaserOperationMode = Convert.ToInt16(value);

            key = "SeedClockFrequency";
            value = GetValue(section, key, filepath);
            PolygonPara.SeedClockFrequency = Convert.ToInt32(value);

            key = "RepetitionRate";
            value = GetValue(section, key, filepath);
            PolygonPara.RepetitionRate = Convert.ToInt32(value);

            key = "PulsePickWidth";
            value = GetValue(section, key, filepath);
            PolygonPara.PulsePickWidth = Convert.ToInt16(value);

            key = "PixelWidth";
            value = GetValue(section, key, filepath);
            PolygonPara.PixelWidth = Convert.ToInt16(value);

            key = "PulsePickAlgor";
            value = GetValue(section, key, filepath);
            PolygonPara.PulsePickAlgor = Convert.ToInt16(value);

            key = "UseCoaxIf";
            value = GetValue(section, key, filepath);
            PolygonPara.UseCoaxIf = Convert.ToInt16(value);

            section = "CrossScan Configuration";

            key = "CrossScanEncoderResol";
            value = GetValue(section, key, filepath);
            PolygonPara.CrossScanEncoderResol = Convert.ToDouble(value);

            key = "CrossScanMaxAccel";
            value = GetValue(section, key, filepath);
            PolygonPara.CrossScanMaxAccel = Convert.ToInt16(value);

            key = "EnCarSig";
            value = GetValue(section, key, filepath);
            PolygonPara.EnCarSig = Convert.ToInt16(value);

            key = "SwapCarSig";
            value = GetValue(section, key, filepath);
            PolygonPara.SwapCarSig = Convert.ToInt16(value);

            section = "Head Configuration";

            key = "SerialNumber";
            value = GetValue(section, key, filepath);
            PolygonPara.SerialNumber = Convert.ToInt32(value);

            key = "FThetaConstant";
            value = GetValue(section, key, filepath);
            PolygonPara.FThetaConstant = Convert.ToDouble(value);

            key = "ExposeLineLength";
            value = GetValue(section, key, filepath);
            PolygonPara.ExposeLineLength = Convert.ToDouble(value);

            key = "EncoderIndexDelay";
            value = GetValue(section, key, filepath);
            PolygonPara.EncoderIndexDelay = Convert.ToInt16(value);

            key = "FacetFineDelay0";
            value = GetValue(section, key, filepath);
            PolygonPara.FacetFineDelay0 = Convert.ToDouble(value);

            key = "FacetFineDelay1";
            value = GetValue(section, key, filepath);
            PolygonPara.FacetFineDelay1 = Convert.ToDouble(value);

            key = "FacetFineDelay2";
            value = GetValue(section, key, filepath);
            PolygonPara.FacetFineDelay2 = Convert.ToDouble(value);

            key = "FacetFineDelay3";
            value = GetValue(section, key, filepath);
            PolygonPara.FacetFineDelay3 = Convert.ToDouble(value);


            key = "FacetFineDelay4";
            value = GetValue(section, key, filepath);
            PolygonPara.FacetFineDelay4 = Convert.ToDouble(value);

            key = "FacetFineDelay5";
            value = GetValue(section, key, filepath);
            PolygonPara.FacetFineDelay5 = Convert.ToDouble(value);

            key = "FacetFineDelay6";
            value = GetValue(section, key, filepath);
            PolygonPara.FacetFineDelay6 = Convert.ToDouble(value);

            key = "FacetFineDelay7";
            value = GetValue(section, key, filepath);
            PolygonPara.FacetFineDelay7 = Convert.ToDouble(value);

            key = "InterleaveRatio";
            value = GetValue(section, key, filepath);
            PolygonPara.InterleaveRatio = Convert.ToInt16(value);

            key = "FacetFineDelayOffset0";
            value = GetValue(section, key, filepath);
            PolygonPara.FacetFineDelayOffset0 = Convert.ToDouble(value);

            key = "FacetFineDelayOffset1";
            value = GetValue(section, key, filepath);
            PolygonPara.FacetFineDelayOffset1 = Convert.ToDouble(value);

            key = "FacetFineDelayOffset2";
            value = GetValue(section, key, filepath);
            PolygonPara.FacetFineDelayOffset2 = Convert.ToDouble(value);

            key = "FacetFineDelayOffset3";
            value = GetValue(section, key, filepath);
            PolygonPara.FacetFineDelayOffset3 = Convert.ToDouble(value);

            key = "FacetFineDelayOffset4";
            value = GetValue(section, key, filepath);
            PolygonPara.FacetFineDelayOffset4 = Convert.ToDouble(value);

            key = "FacetFineDelayOffset5";
            value = GetValue(section, key, filepath);
            PolygonPara.FacetFineDelayOffset5 = Convert.ToDouble(value);

            key = "FacetFineDelayOffset6";
            value = GetValue(section, key, filepath);
            PolygonPara.FacetFineDelayOffset6 = Convert.ToDouble(value);

            key = "FacetFineDelayOffset7";
            value = GetValue(section, key, filepath);
            PolygonPara.FacetFineDelayOffset7 = Convert.ToDouble(value);

            key = "StartFacet";
            value = GetValue(section, key, filepath);
            PolygonPara.StartFacet = Convert.ToInt16(value);

            key = "AutoIncrementStartFacet";
            value = GetValue(section, key, filepath);
            PolygonPara.AutoIncrementStartFacet = Convert.ToInt16(value);

            section = "Polygon motor Configuration";

            key = "InternalMotorDriverClk";
            value = GetValue(section, key, filepath);
            PolygonPara.InternalMotorDriverClk = Convert.ToInt16(value);

            key = "MotorDriverType";
            value = GetValue(section, key, filepath);
            PolygonPara.MotorDriverType = Convert.ToInt16(value);

            key = "MotorSpeed";
            value = GetValue(section, key, filepath);
            PolygonPara.MotorSpeed = Convert.ToInt16(value);

            key = "SimEncSel";
            value = GetValue(section, key, filepath);
            PolygonPara.SimEncSel = Convert.ToInt16(value);

            key = "MinMotorSpeed";
            value = GetValue(section, key, filepath);
            PolygonPara.MinMotorSpeed = Convert.ToDouble(value);

            key = "MaxMotorSpeed";
            value = GetValue(section, key, filepath);
            PolygonPara.MaxMotorSpeed = Convert.ToDouble(value);

            key = "MotorEffectivePoles";
            value = GetValue(section, key, filepath);
            PolygonPara.MotorEffectivePoles = Convert.ToInt16(value);

            key = "SyncWaitTime";
            value = GetValue(section, key, filepath);
            PolygonPara.SyncWaitTime = Convert.ToInt16(value);

            key = "MotorStableTime";
            value = GetValue(section, key, filepath);
            PolygonPara.MotorStableTime = Convert.ToInt16(value);

            section = "Other Settings";

            key = "InterruptFreq";
            value = GetValue(section, key, filepath);
            PolygonPara.InterruptFreq = Convert.ToInt16(value);

            key = "HWDebugSelection";
            value = GetValue(section, key, filepath);
            PolygonPara.HWDebugSelection = Convert.ToInt16(value);

            key = "AutoRepeat";
            value = GetValue(section, key, filepath);
            PolygonPara.AutoRepeat = Convert.ToInt16(value);

            key = "PixAlwaysOn";
            value = GetValue(section, key, filepath);
            PolygonPara.PixAlwaysOn = Convert.ToInt16(value);

            key = "ExtCamTrig";
            value = GetValue(section, key, filepath);
            PolygonPara.ExtCamTrig = Convert.ToInt16(value);

            key = "EncoderExpo";
            value = GetValue(section, key, filepath);
            PolygonPara.EncoderExpo = Convert.ToInt16(value);

            key = "FacetTest";
            value = GetValue(section, key, filepath);
            PolygonPara.FacetTest = Convert.ToInt16(value);

            key = "SWTest";
            value = GetValue(section, key, filepath);
            PolygonPara.SWTest = Convert.ToInt16(value);

            key = "JobstartAutorepeat";
            value = GetValue(section, key, filepath);
            PolygonPara.JobstartAutorepeat = Convert.ToInt16(value);
        }

        public void SavePolygonPara(CPolygonParameter PolygonPara)
        {
            string section = "";
            string key = "";
            string value = "";
            string filepath = @PATH_CONFIG_INI;
            bool bRet = false;

            FileInfo fileinfo = new FileInfo(filepath);

            section = "Job Settings";

            key = "InScanResolution";
            value = string.Format("{0:F4}", PolygonPara.InScanResolution);
            bRet = SetValue(section, key, value, filepath);

            key = "CrossScanResolution";
            value = string.Format("{0:F4}", PolygonPara.CrossScanResolution);
            bRet = SetValue(section, key, value, filepath);

            key = "InScanOffset";
            value = Convert.ToString(PolygonPara.InScanOffset);
            bRet = SetValue(section, key, value, filepath);

            key = "StopMotorBetweenJobs";
            value = Convert.ToString(PolygonPara.StopMotorBetweenJobs);
            bRet = SetValue(section, key, value, filepath);

            key = "PixInvert";
            value = Convert.ToString(PolygonPara.PixInvert);
            bRet = SetValue(section, key, value, filepath);

            key = "JobStartBufferTime";
            value = Convert.ToString(PolygonPara.JobStartBufferTime);
            bRet = SetValue(section, key, value, filepath);

            key = "PrecedingBlankLines";
            value = Convert.ToString(PolygonPara.PrecedingBlankLines);
            bRet = SetValue(section, key, value, filepath);

            section = "Laser Configuration";

            key = "LaserOperationMode";
            value = Convert.ToString(PolygonPara.LaserOperationMode);
            bRet = SetValue(section, key, value, filepath);

            key = "SeedClockFrequency";
            value = Convert.ToString(PolygonPara.SeedClockFrequency);
            bRet = SetValue(section, key, value, filepath);

            key = "RepetitionRate";
            value = Convert.ToString(PolygonPara.RepetitionRate);
            bRet = SetValue(section, key, value, filepath);

            section = "CrossScan Configuration";

            key = "CrossScanEncoderResol";
            value = string.Format("{0:F7}", PolygonPara.CrossScanEncoderResol);
            bRet = SetValue(section, key, value, filepath);

            key = "EnCarSig";
            value = Convert.ToString(PolygonPara.EnCarSig);
            bRet = SetValue(section, key, value, filepath);

            key = "SwapCarSig";
            value = Convert.ToString(PolygonPara.SwapCarSig);
            bRet = SetValue(section, key, value, filepath);

            section = "Head Configuration";

            key = "InterleaveRatio";
            value = Convert.ToString(PolygonPara.InterleaveRatio);
            bRet = SetValue(section, key, value, filepath);

            key = "FacetFineDelayOffset0";
            value = string.Format("{0:F6}", PolygonPara.FacetFineDelayOffset0);
            bRet = SetValue(section, key, value, filepath);

            key = "FacetFineDelayOffset1";
            value = string.Format("{0:F6}", PolygonPara.FacetFineDelayOffset1);
            bRet = SetValue(section, key, value, filepath);

            key = "FacetFineDelayOffset2";
            value = string.Format("{0:F6}", PolygonPara.FacetFineDelayOffset2);
            bRet = SetValue(section, key, value, filepath);

            key = "FacetFineDelayOffset3";
            value = string.Format("{0:F6}", PolygonPara.FacetFineDelayOffset3);
            bRet = SetValue(section, key, value, filepath);

            key = "FacetFineDelayOffset4";
            value = string.Format("{0:F6}", PolygonPara.FacetFineDelayOffset4);
            bRet = SetValue(section, key, value, filepath);

            key = "FacetFineDelayOffset5";
            value = string.Format("{0:F6}", PolygonPara.FacetFineDelayOffset5);
            bRet = SetValue(section, key, value, filepath);

            key = "FacetFineDelayOffset6";
            value = string.Format("{0:F6}", PolygonPara.FacetFineDelayOffset6);
            bRet = SetValue(section, key, value, filepath);

            key = "FacetFineDelayOffset7";
            value = string.Format("{0:F6}", PolygonPara.FacetFineDelayOffset7);
            bRet = SetValue(section, key, value, filepath);

            key = "StartFacet";
            value = Convert.ToString(PolygonPara.StartFacet);
            bRet = SetValue(section, key, value, filepath);

            key = "AutoIncrementStartFacet";
            value = Convert.ToString(PolygonPara.AutoIncrementStartFacet);
            bRet = SetValue(section, key, value, filepath);

            section = "Polygon motor Configuration";

            key = "MotorStableTime";
            value = Convert.ToString(PolygonPara.MotorStableTime);
            bRet = SetValue(section, key, value, filepath);

        }

        public bool SendConfig(string strPath, string strFTP)
        {
            if (SendFile(strPath, strFTP) == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool SendBitMap(string strPath, string strFTP)
        {
            if(SendFile(strPath,strFTP) == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
       
        public void InitializeSerial()
        {
            string PortName = "COM1";
            int BaudRate = 115200;
            Parity _Parity = Parity.Even;
            int DataBits = 8;
            StopBits _StopBits = StopBits.One;

            CSerialPortData SerialCom = new CSerialPortData(PortName, BaudRate, _Parity, DataBits, _StopBits);

            CDBInfo dbInfo;
            dbInfo = new CDBInfo();

            MSystemInfo m_SystemInfo = new MSystemInfo();

            m_SystemInfo.GetObjectInfo(10, out objInfo);

            m_COM = new MSerialPort(objInfo, SerialCom);
        }

        public void LSEPortOpen()
        {
            m_COM.OpenPort();
        }

        public void LSEPortClose()
        {
            m_COM.ClosePort();
        }

        public int GetSerialData(out string Message)
        {
            string message = "";
            int QueueSize = 0;

            if (m_COM.IsOpened())
            {
                m_COM.ReceiveMessage(out message, out QueueSize);

                m_COM.ClearReceiveQue();

                Message = message;

                return SUCCESS;
            }
            else
            {
                Message = "";
                return ERR_SERIALPORT_RECEIVEDQUE_EMPTY;
            }
        }

        public String GetValue(String Section, String Key, String iniPath)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, iniPath);
            return temp.ToString();
        }

        public bool SetValue(String Section, String Key, String Value, String iniPath)
        {
            bool bRet = WritePrivateProfileString(Section, Key, Value, iniPath);
            return WritePrivateProfileString(Section, Key, Value, iniPath);
        }

        public bool SendFile(string strPath, string strFTP)
        {
            FtpWebRequest FTPUploader = (FtpWebRequest)WebRequest.Create(strFTP);

            FTPUploader.Method = WebRequestMethods.Ftp.UploadFile;

            FileInfo fileInfo = new FileInfo(strPath);
            FileStream fileStream = fileInfo.OpenRead();

            int bufferLength = 2048;
            byte[] buffer = new byte[bufferLength];

            try
            {
                Stream uploadStream = FTPUploader.GetRequestStream();
                int contentLength = fileStream.Read(buffer, 0, bufferLength);

                while (contentLength != 0)
                {
                    uploadStream.Write(buffer, 0, contentLength);
                    contentLength = fileStream.Read(buffer, 0, bufferLength);
                }

                uploadStream.Close();
                fileStream.Close();

                FTPUploader = null;

                return true;
            }
            catch
            {
                fileStream.Close();
                FTPUploader = null;

                return false;
            }
        }
    }
}
