using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

using System.Runtime.InteropServices;
using System.IO.Ports;

using System.Diagnostics;

using static LWDicer.Control.DEF_System;
using static LWDicer.Control.DEF_Common;
using static LWDicer.Control.DEF_Error;
using static LWDicer.Control.DEF_PolygonScanner;
using static LWDicer.Control.DEF_DataManager;
using static LWDicer.Control.DEF_SerialPort;


namespace LWDicer.Control
{
    public class MPolygonScanner : MObject, IPolygonScanner
    {
        private ISerialPort m_COM;
        private CDBInfo m_DBInfo;

        protected string[] IPAddress = new string[(int)EObjectScanner.MAX_OBJ];

        protected string[] m_IP = new string[(int)EObjectScanner.MAX_OBJ];
        protected string[] m_Port = new string[(int)EObjectScanner.MAX_OBJ];

        protected CPolygonIni[] m_PolygonData = new CPolygonIni[(int)EObjectScanner.MAX_OBJ];

        protected ProcessStartInfo proInfo = new ProcessStartInfo();
        protected Process m_process = new Process();
        
        private Graphics m_Grapic;
        private Image Image;
        public PictureBox PicWafer;

        public MPolygonScanner(CObjectInfo objInfo, CPolygonIni PolygonPara , int scannerIndex, ISerialPort SerialPort)
            : base(objInfo)
        {
            m_DBInfo = new CDBInfo();

            LoadPolygonPara(scannerIndex, PolygonPara);

            m_PolygonData[scannerIndex] = PolygonPara;

            m_COM = SerialPort;

            SetScannerIP(scannerIndex,m_PolygonData[scannerIndex].strIP);

            SetScannerPort(scannerIndex, m_PolygonData[scannerIndex].strPort);

            PicWafer = new PictureBox();

            InitializeTFTP();

        }

        public bool LoadPolygonPara(int scannerIndex, CPolygonIni PolygonPara)
        {
            string section = "";
            string key = "";
            string value = "";
            string filepath = "";

            filepath = string.Format("{0:s}config.ini", m_DBInfo.ScannerLogDir);

            if(!File.Exists(filepath))
            {
                return false;
            }

            m_PolygonData[scannerIndex] = PolygonPara;
                
            section = "Job Settings";

            key = "InScanResolution";
            value = GetValue(section, key, filepath);
            m_PolygonData[scannerIndex].InScanResolution = Convert.ToDouble(value);

            key = "CrossScanResolution";
            value = GetValue(section, key, filepath);
            m_PolygonData[scannerIndex].CrossScanResolution = Convert.ToDouble(value);

            key = "InScanOffset";
            value = GetValue(section, key, filepath);
            m_PolygonData[scannerIndex].InScanOffset = Convert.ToDouble(value);

            key = "StopMotorBetweenJobs";
            value = GetValue(section, key, filepath);
            m_PolygonData[scannerIndex].StopMotorBetweenJobs = Convert.ToInt16(value);

            key = "PixInvert";
            value = GetValue(section, key, filepath);
            m_PolygonData[scannerIndex].PixInvert = Convert.ToInt16(value);

            key = "JobStartBufferTime";
            value = GetValue(section, key, filepath);
            m_PolygonData[scannerIndex].JobStartBufferTime = Convert.ToInt16(value);

            key = "PrecedingBlankLines";
            value = GetValue(section, key, filepath);
            m_PolygonData[scannerIndex].PrecedingBlankLines = Convert.ToInt16(value);

            section = "Laser Configuration";

            key = "LaserOperationMode";
            value = GetValue(section, key, filepath);
            m_PolygonData[scannerIndex].LaserOperationMode = Convert.ToInt16(value);

            key = "SeedClockFrequency";
            value = GetValue(section, key, filepath);
            m_PolygonData[scannerIndex].SeedClockFrequency = Convert.ToDouble(value);

            key = "RepetitionRate";
            value = GetValue(section, key, filepath);
            m_PolygonData[scannerIndex].RepetitionRate = Convert.ToDouble(value);

            key = "PulsePickWidth";
            value = GetValue(section, key, filepath);
            m_PolygonData[scannerIndex].PulsePickWidth = Convert.ToInt16(value);

            key = "PixelWidth";
            value = GetValue(section, key, filepath);
            m_PolygonData[scannerIndex].PixelWidth = Convert.ToInt16(value);

            key = "PulsePickAlgor";
            value = GetValue(section, key, filepath);
            m_PolygonData[scannerIndex].PulsePickAlgor = Convert.ToInt16(value);

            key = "UseCoaxIf";
            value = GetValue(section, key, filepath);
            m_PolygonData[scannerIndex].UseCoaxIf = Convert.ToInt16(value);

            section = "CrossScan Configuration";

            key = "CrossScanEncoderResol";
            value = GetValue(section, key, filepath);
            m_PolygonData[scannerIndex].CrossScanEncoderResol = Convert.ToDouble(value);

            key = "CrossScanMaxAccel";
            value = GetValue(section, key, filepath);
            m_PolygonData[scannerIndex].CrossScanMaxAccel = Convert.ToDouble(value);

            key = "EnCarSig";
            value = GetValue(section, key, filepath);
            m_PolygonData[scannerIndex].EnCarSig = Convert.ToInt16(value);

            key = "SwapCarSig";
            value = GetValue(section, key, filepath);
            m_PolygonData[scannerIndex].SwapCarSig = Convert.ToInt16(value);

            section = "Head Configuration";

            key = "SerialNumber";
            value = GetValue(section, key, filepath);
            m_PolygonData[scannerIndex].SerialNumber = Convert.ToDouble(value);

            key = "FThetaConstant";
            value = GetValue(section, key, filepath);
            m_PolygonData[scannerIndex].FThetaConstant = Convert.ToDouble(value);

            key = "ExposeLineLength";
            value = GetValue(section, key, filepath);
            m_PolygonData[scannerIndex].ExposeLineLength = Convert.ToDouble(value);

            key = "EncoderIndexDelay";
            value = GetValue(section, key, filepath);
            m_PolygonData[scannerIndex].EncoderIndexDelay = Convert.ToInt16(value);

            key = "FacetFineDelay0";
            value = GetValue(section, key, filepath);
            m_PolygonData[scannerIndex].FacetFineDelay0 = Convert.ToDouble(value);

            key = "FacetFineDelay1";
            value = GetValue(section, key, filepath);
            m_PolygonData[scannerIndex].FacetFineDelay1 = Convert.ToDouble(value);

            key = "FacetFineDelay2";
            value = GetValue(section, key, filepath);
            m_PolygonData[scannerIndex].FacetFineDelay2 = Convert.ToDouble(value);

            key = "FacetFineDelay3";
            value = GetValue(section, key, filepath);
            m_PolygonData[scannerIndex].FacetFineDelay3 = Convert.ToDouble(value);


            key = "FacetFineDelay4";
            value = GetValue(section, key, filepath);
            m_PolygonData[scannerIndex].FacetFineDelay4 = Convert.ToDouble(value);

            key = "FacetFineDelay5";
            value = GetValue(section, key, filepath);
            m_PolygonData[scannerIndex].FacetFineDelay5 = Convert.ToDouble(value);

            key = "FacetFineDelay6";
            value = GetValue(section, key, filepath);
            m_PolygonData[scannerIndex].FacetFineDelay6 = Convert.ToDouble(value);

            key = "FacetFineDelay7";
            value = GetValue(section, key, filepath);
            m_PolygonData[scannerIndex].FacetFineDelay7 = Convert.ToDouble(value);

            key = "InterleaveRatio";
            value = GetValue(section, key, filepath);
            m_PolygonData[scannerIndex].InterleaveRatio = Convert.ToInt16(value);

            key = "FacetFineDelayOffset0";
            value = GetValue(section, key, filepath);
            m_PolygonData[scannerIndex].FacetFineDelayOffset0 = Convert.ToDouble(value);

            key = "FacetFineDelayOffset1";
            value = GetValue(section, key, filepath);
            m_PolygonData[scannerIndex].FacetFineDelayOffset1 = Convert.ToDouble(value);

            key = "FacetFineDelayOffset2";
            value = GetValue(section, key, filepath);
            m_PolygonData[scannerIndex].FacetFineDelayOffset2 = Convert.ToDouble(value);

            key = "FacetFineDelayOffset3";
            value = GetValue(section, key, filepath);
            m_PolygonData[scannerIndex].FacetFineDelayOffset3 = Convert.ToDouble(value);

            key = "FacetFineDelayOffset4";
            value = GetValue(section, key, filepath);
            m_PolygonData[scannerIndex].FacetFineDelayOffset4 = Convert.ToDouble(value);

            key = "FacetFineDelayOffset5";
            value = GetValue(section, key, filepath);
            m_PolygonData[scannerIndex].FacetFineDelayOffset5 = Convert.ToDouble(value);

            key = "FacetFineDelayOffset6";
            value = GetValue(section, key, filepath);
            m_PolygonData[scannerIndex].FacetFineDelayOffset6 = Convert.ToDouble(value);

            key = "FacetFineDelayOffset7";
            value = GetValue(section, key, filepath);
            m_PolygonData[scannerIndex].FacetFineDelayOffset7 = Convert.ToDouble(value);

            key = "StartFacet";
            value = GetValue(section, key, filepath);
            m_PolygonData[scannerIndex].StartFacet = Convert.ToInt16(value);

            key = "AutoIncrementStartFacet";
            value = GetValue(section, key, filepath);
            m_PolygonData[scannerIndex].AutoIncrementStartFacet = Convert.ToInt16(value);

            section = "Polygon motor Configuration";

            key = "InternalMotorDriverClk";
            value = GetValue(section, key, filepath);
            m_PolygonData[scannerIndex].InternalMotorDriverClk = Convert.ToInt16(value);

            key = "MotorDriverType";
            value = GetValue(section, key, filepath);
            m_PolygonData[scannerIndex].MotorDriverType = Convert.ToInt16(value);

            key = "MotorSpeed";
            value = GetValue(section, key, filepath);
            m_PolygonData[scannerIndex].MotorSpeed = Convert.ToInt16(value);

            key = "SimEncSel";
            value = GetValue(section, key, filepath);
            m_PolygonData[scannerIndex].SimEncSel = Convert.ToInt16(value);

            key = "MinMotorSpeed";
            value = GetValue(section, key, filepath);
            m_PolygonData[scannerIndex].MinMotorSpeed = Convert.ToDouble(value);

            key = "MaxMotorSpeed";
            value = GetValue(section, key, filepath);
            m_PolygonData[scannerIndex].MaxMotorSpeed = Convert.ToDouble(value);

            key = "MotorEffectivePoles";
            value = GetValue(section, key, filepath);
            m_PolygonData[scannerIndex].MotorEffectivePoles = Convert.ToInt16(value);

            key = "SyncWaitTime";
            value = GetValue(section, key, filepath);
            m_PolygonData[scannerIndex].SyncWaitTime = Convert.ToInt16(value);

            key = "MotorStableTime";
            value = GetValue(section, key, filepath);
            m_PolygonData[scannerIndex].MotorStableTime = Convert.ToInt16(value);

            section = "Other Settings";

            key = "InterruptFreq";
            value = GetValue(section, key, filepath);
            m_PolygonData[scannerIndex].InterruptFreq = Convert.ToInt16(value);

            key = "HWDebugSelection";
            value = GetValue(section, key, filepath);
            m_PolygonData[scannerIndex].HWDebugSelection = Convert.ToInt16(value);

            key = "AutoRepeat";
            value = GetValue(section, key, filepath);
            m_PolygonData[scannerIndex].AutoRepeat = Convert.ToInt16(value);

            key = "PixAlwaysOn";
            value = GetValue(section, key, filepath);
            m_PolygonData[scannerIndex].PixAlwaysOn = Convert.ToInt16(value);

            key = "ExtCamTrig";
            value = GetValue(section, key, filepath);
            m_PolygonData[scannerIndex].ExtCamTrig = Convert.ToInt16(value);

            key = "EncoderExpo";
            value = GetValue(section, key, filepath);
            m_PolygonData[scannerIndex].EncoderExpo = Convert.ToInt16(value);

            key = "FacetTest";
            value = GetValue(section, key, filepath);
            m_PolygonData[scannerIndex].FacetTest = Convert.ToInt16(value);

            key = "SWTest";
            value = GetValue(section, key, filepath);
            m_PolygonData[scannerIndex].SWTest = Convert.ToInt16(value);

            key = "JobstartAutorepeat";
            value = GetValue(section, key, filepath);
            m_PolygonData[scannerIndex].JobstartAutorepeat = Convert.ToInt16(value);

            return true;
        }

        public bool SavePolygonPara(CPolygonIni m_PolygonData, string strFile)
        {
            string section = "";
            string key = "";
            string value = "";
            string filepath = "";
            bool bRet = false;

            filepath = string.Format("{0:s}{1:s}.ini", m_DBInfo.ScannerLogDir, strFile);

            if (!File.Exists(filepath))
            {
                return false;
            }

            FileInfo fileinfo = new FileInfo(filepath);

            section = "Job Settings";

            key = "InScanResolution";
            value = string.Format("{0:F6}", m_PolygonData.InScanResolution);
            bRet = SetValue(section, key, value, filepath);

            key = "CrossScanResolution";
            value = string.Format("{0:F6}", m_PolygonData.CrossScanResolution);
            bRet = SetValue(section, key, value, filepath);

            key = "InScanOffset";
            value = Convert.ToString(m_PolygonData.InScanOffset);
            bRet = SetValue(section, key, value, filepath);

            key = "StopMotorBetweenJobs";
            value = Convert.ToString(m_PolygonData.StopMotorBetweenJobs);
            bRet = SetValue(section, key, value, filepath);

            key = "PixInvert";
            value = Convert.ToString(m_PolygonData.PixInvert);
            bRet = SetValue(section, key, value, filepath);

            key = "JobStartBufferTime";
            value = Convert.ToString(m_PolygonData.JobStartBufferTime);
            bRet = SetValue(section, key, value, filepath);

            key = "PrecedingBlankLines";
            value = Convert.ToString(m_PolygonData.PrecedingBlankLines);
            bRet = SetValue(section, key, value, filepath);

            section = "Laser Configuration";

            key = "LaserOperationMode";
            value = Convert.ToString(m_PolygonData.LaserOperationMode);
            bRet = SetValue(section, key, value, filepath);

            key = "SeedClockFrequency";
            value = Convert.ToString(m_PolygonData.SeedClockFrequency);
            bRet = SetValue(section, key, value, filepath);

            key = "RepetitionRate";
            value = Convert.ToString(m_PolygonData.RepetitionRate);
            bRet = SetValue(section, key, value, filepath);

            key = "PulsePickWidth";
            value = Convert.ToString(m_PolygonData.PulsePickWidth);
            bRet = SetValue(section, key, value, filepath);

            key = "PixelWidth";
            value = Convert.ToString(m_PolygonData.PixelWidth);
            bRet = SetValue(section, key, value, filepath);

            section = "CrossScan Configuration";

            key = "CrossScanEncoderResol";
            value = string.Format("{0:F7}", m_PolygonData.CrossScanEncoderResol);
            bRet = SetValue(section, key, value, filepath);

            key = "EnCarSig";
            value = Convert.ToString(m_PolygonData.EnCarSig);
            bRet = SetValue(section, key, value, filepath);

            key = "SwapCarSig";
            value = Convert.ToString(m_PolygonData.SwapCarSig);
            bRet = SetValue(section, key, value, filepath);

            section = "Head Configuration";

            key = "InterleaveRatio";
            value = Convert.ToString(m_PolygonData.InterleaveRatio);
            bRet = SetValue(section, key, value, filepath);

            key = "FacetFineDelayOffset0";
            value = string.Format("{0:F6}", m_PolygonData.FacetFineDelayOffset0);
            bRet = SetValue(section, key, value, filepath);

            key = "FacetFineDelayOffset1";
            value = string.Format("{0:F6}", m_PolygonData.FacetFineDelayOffset1);
            bRet = SetValue(section, key, value, filepath);

            key = "FacetFineDelayOffset2";
            value = string.Format("{0:F6}", m_PolygonData.FacetFineDelayOffset2);
            bRet = SetValue(section, key, value, filepath);

            key = "FacetFineDelayOffset3";
            value = string.Format("{0:F6}", m_PolygonData.FacetFineDelayOffset3);
            bRet = SetValue(section, key, value, filepath);

            key = "FacetFineDelayOffset4";
            value = string.Format("{0:F6}", m_PolygonData.FacetFineDelayOffset4);
            bRet = SetValue(section, key, value, filepath);

            key = "FacetFineDelayOffset5";
            value = string.Format("{0:F6}", m_PolygonData.FacetFineDelayOffset5);
            bRet = SetValue(section, key, value, filepath);

            key = "FacetFineDelayOffset6";
            value = string.Format("{0:F6}", m_PolygonData.FacetFineDelayOffset6);
            bRet = SetValue(section, key, value, filepath);

            key = "FacetFineDelayOffset7";
            value = string.Format("{0:F6}", m_PolygonData.FacetFineDelayOffset7);
            bRet = SetValue(section, key, value, filepath);

            key = "StartFacet";
            value = Convert.ToString(m_PolygonData.StartFacet);
            bRet = SetValue(section, key, value, filepath);

            key = "AutoIncrementStartFacet";
            value = Convert.ToString(m_PolygonData.AutoIncrementStartFacet);
            bRet = SetValue(section, key, value, filepath);

            section = "Polygon motor Configuration";

            key = "MotorStableTime";
            value = Convert.ToString(m_PolygonData.MotorStableTime);
            bRet = SetValue(section, key, value, filepath);

            return true;
        }

        public CPolygonIni GetPolygonPara(int scannerIndex)
        {
            return m_PolygonData[scannerIndex];
        }


        /*------------------------------------------------------------------------------------
         * Date : 2016.02.24
         * Author : HSLEE
         * Function : SetPixelGridX(int scannerIndex, double pX)
         * Description : Polygon Scanner의 X Pixel Grid Size를 설정한다.
         *               사용자 입력 단위 - um    
         *               Scanner 단위 - m
         * Parameter : int scannerIndex - Scanner No.
         *             double pX - Scanline 에서 이웃한 두 pixel 사이 X축 거리
         ------------------------------------------------------------------------------------*/
        public void SetPixelGridX(int scannerIndex, double pX)
        {
            m_PolygonData[scannerIndex].InScanResolution = 0.000001 * pX;
        }


        /*------------------------------------------------------------------------------------
         * Date : 2016.02.24
         * Author : HSLEE
         * Function : SetPixelGridY(int scannerIndex, double pY)
         * Description : Polygon Scanner의 Y Pixel Grid Size를 설정한다.
         *               사용자 입력 단위 - um    
         *               Scanner 단위 - m
         * Parameter :  int scannerIndex - Scanner No.
         *              double pY - 이웃한 두 Scanline 사이 Y축 거리
         ------------------------------------------------------------------------------------*/
        public void SetPixelGridY(int scannerIndex, double pY)
        {
            m_PolygonData[scannerIndex].CrossScanResolution = 0.000001 * pY;
        }


        /*------------------------------------------------------------------------------------
         * Date : 2016.02.24
         * Author : HSLEE
         * Function : SetBitMapColor(int scannerIndex, int  nColor)
         * Description : Polygon Scanner에서 처리하는 LaserPulse Exposure Bitmap의 색상 선택
         * Parameter : int scannerIndex - Scanner No.
         *             int  nColor - Scanline에서이웃한두pixel 사이X축거리
         *             BLACK = 0;  //0 = Black에서Laser On
         *             WHITE = 1;  //1 = White 에서Laser On
         ------------------------------------------------------------------------------------*/
        public void SetBitMapColor(int scannerIndex, int nColor)
        {
            m_PolygonData[scannerIndex].PixInvert = nColor;
        }


        /*------------------------------------------------------------------------------------
         * Date : 2016.02.24
         * Author : HSLEE
         * Function : SetSuperSync(int scannerIndex, double OffSet, int nNo)
         * Description : Scanner 내부 8개의 각 Mirror scanline의 시작위치 값의 미세 조정
         *               사용자 입력 단위 - um    
         *               Scanner 단위 - m
         * Parameter : int scannerIndex - Scanner No.
         *             double OffSet - 미세조정 OffSet Data
         *             int nNo -  Mirror No
         ------------------------------------------------------------------------------------*/
        public void SetSuperSync(int scannerIndex, double OffSet, int nNo)
        {
            switch(nNo)
            {
                case 0:
                    m_PolygonData[scannerIndex].FacetFineDelayOffset0 = 0.000001 * OffSet;
                    break;
                case 1:
                    m_PolygonData[scannerIndex].FacetFineDelayOffset1 = 0.000001 * OffSet;
                    break;
                case 2:
                    m_PolygonData[scannerIndex].FacetFineDelayOffset2 = 0.000001 * OffSet;
                    break;
                case 3:
                    m_PolygonData[scannerIndex].FacetFineDelayOffset3 = 0.000001 * OffSet;
                    break;
                case 4:
                    m_PolygonData[scannerIndex].FacetFineDelayOffset4 = 0.000001 * OffSet;
                    break;
                case 5:
                    m_PolygonData[scannerIndex].FacetFineDelayOffset5 = 0.000001 * OffSet;
                    break;
                case 6:
                    m_PolygonData[scannerIndex].FacetFineDelayOffset6 = 0.000001 * OffSet;
                    break;
                case 7:
                    m_PolygonData[scannerIndex].FacetFineDelayOffset7 = 0.000001 * OffSet;
                    break;
            }
        }

        /*------------------------------------------------------------------------------------
         * Date : 2016.02.24
         * Author : HSLEE
         * Function : SetStartOffset(int scannerIndex, double OffSet)
         * Description : 모든 Scanline의 X축 시작 위치를 조정
         *               사용자 입력 단위 - um    
         *               Scanner 단위 - m
         * Parameter :   int scannerIndex - Scanner No.
         *               double OffSet
         ------------------------------------------------------------------------------------*/
        public void SetStartOffset(int scannerIndex, double OffSet)
        {
            m_PolygonData[scannerIndex].InScanOffset = 0.000001 * OffSet;
        }


        /*------------------------------------------------------------------------------------
         * Date : 2016.02.24
         * Author : HSLEE
         * Function : SetSeedClock(int scannerIndex, double Frequency)
         * Description : Laser의 Seed Clock 주파수 설정
         *               사용자 입력 단위 - kHz    
         *               Scanner 단위 - Hz
         * Parameter :  int scannerIndex - Scanner No.
         *              double Frequency - kHz 단위
         ------------------------------------------------------------------------------------*/
        public void SetSeedClock(int scannerIndex, double Frequency)
        {
            m_PolygonData[scannerIndex].SeedClockFrequency = 1000 * Frequency;
        }


        /*------------------------------------------------------------------------------------
         * Date : 2016.02.24
         * Author : HSLEE
         * Function : SetRepRate(int scannerIndex, double Frequency)
         * Description : 가공에적용할펄스반복률(REP_RATE)설정
         *               사용자 입력 단위 - kHz    
         *               Scanner 단위 - Hz
         * Parameter :  int scannerIndex - Scanner No.
         *              double Frequency - kHz 단위
         ------------------------------------------------------------------------------------*/
        public void SetRepRate(int scannerIndex, double Frequency)
        {
            m_PolygonData[scannerIndex].RepetitionRate = 1000 * Frequency;
        }


        /*------------------------------------------------------------------------------------
         * Date : 2016.02.24
         * Author : HSLEE
         * Function : SetPixelWidth(int scannerIndex, double SeedClock, double RefRate)
         * Description : Laser SeedClock, Rep Rate 변경에 따른 Laser Pulse Width 변경
         *               사용자 입력 단위 - kHz    
         *               Scanner 단위 - Hz
         * Parameter : int scannerIndex - Scanner No.
         *             double SeedClock - 적용될 SeedClockFrequency
         *             double RepRate -  적용될 RepetitionRate
         ------------------------------------------------------------------------------------*/
        public void SetPixelWidth(int scannerIndex, double SeedClock, double RepRate)
        {
            m_PolygonData[scannerIndex].PixelWidth = (int)((SeedClock / RepRate) / 2);
            m_PolygonData[scannerIndex].PulsePickWidth = (int)(m_PolygonData[scannerIndex].PixelWidth / 2);
        }

        /*------------------------------------------------------------------------------------
         * Date : 2016.02.24
         * Author : HSLEE
         * Function : SetLaserOP(int scannerIndex, int nOption)
         * Description : Laser 제어 방식 선택
         * Parameter : int nOption
         *             SuperSync=0,        // SuperSync 동작
         *             NotUsed,            // Not-used
         *             PulseOut,           // PULSE_OUT 출력만사용, SuperSync 중지
         *             NoneSeedClock,      // Trumph TruMicro Laser 전용(SEED CLOCK 미사용)
         *             InternalSeedClock   // AVIA 등나노초레이저용(내부SEED CLOCK 사용)
         ------------------------------------------------------------------------------------*/
        public void SetLaserOP(int scannerIndex, int nOption)
        {
            m_PolygonData[scannerIndex].LaserOperationMode = nOption;
        }


        /*------------------------------------------------------------------------------------
         * Date : 2016.02.24
         * Author : HSLEE
         * Function : SetBufferTime(int scannerIndex, int nSec)
         * Description : Bitmap Uploading 시, exposure 하기전 대기 시간
         * Parameter : int nSec - 사용자 설정 시간 (sec)
         ------------------------------------------------------------------------------------*/
        public void SetBufferTime(int scannerIndex, int nSec)
        {
            m_PolygonData[scannerIndex].JobStartBufferTime = nSec;
        }


        /*------------------------------------------------------------------------------------
         * Date : 2016.02.24
         * Author : HSLEE
         * Function : SetDummyBlankLine(int scannerIndex, int nScanLine)
         * Description : Stage 가속시의 충분한 Settle-time 을 위해 Dummy로 추가하는 scanline 수
         * Parameter : int nScanLine - 사용자 설정 Dummy Scan Line
         ------------------------------------------------------------------------------------*/
        public void SetDummyBlankLine(int scannerIndex, int nScanLine)
        {
            m_PolygonData[scannerIndex].PrecedingBlankLines = nScanLine;
        }


        /*------------------------------------------------------------------------------------
         * Date : 2016.02.24
         * Author : HSLEE
         * Function : SetMotorBetweenJob(int scannerIndex, int nOption)
         * Description : Exposure 이후 polygon mirror 정지 여부 결정
         * Parameter : int nOption - SPIN or STOP
         ------------------------------------------------------------------------------------*/
        public void SetMotorBetweenJob(int scannerIndex, int nOption)
        {
            m_PolygonData[scannerIndex].StopMotorBetweenJobs = nOption;
        }


        /*------------------------------------------------------------------------------------
         * Date : 2016.02.24
         * Author : HSLEE
         * Function : SetMotorStableTime(int scannerIndex, int nTime)
         * Description : speed-up 이후 exposure 시작 이전에 spinning 안정화를 위한 대기 시간
         * Parameter : int nTime - 미리초(ms) 단위로 입력 
         ------------------------------------------------------------------------------------*/
        public void SetMotorStableTime(int scannerIndex, int nTime)
        {
            m_PolygonData[scannerIndex].MotorStableTime = nTime;
        }


        /*------------------------------------------------------------------------------------
         * Date : 2016.02.24
         * Author : HSLEE
         * Function : SetLeaveRatio(int scannerIndex, int nRatio)
         * Description : FacetFineDelayOffset 자동 설정 기능
         * Parameter : int nRatio
         ------------------------------------------------------------------------------------*/
        public void SetLeaveRatio(int scannerIndex, int nRatio)
        {
            m_PolygonData[scannerIndex].InterleaveRatio = nRatio;
        }


        /*------------------------------------------------------------------------------------
         * Date : 2016.02.25
         * Author : HSLEE
         * Function : SetEncoderResol(int scannerIndex, double dResol)
         * Description : Stage Encoder 분해능 값 설정 / A상 edge 에서 B상 edge 까지 거리
         *               사용자 입력 단위 - um    
         *               Scanner 단위 - m
         * Parameter : double dResol
         ------------------------------------------------------------------------------------*/
        public void SetEncoderResol(int scannerIndex, double dResol)
        {
            m_PolygonData[scannerIndex].CrossScanEncoderResol = 0.000001 * dResol;
        }


        /*------------------------------------------------------------------------------------
         * Date : 2016.02.25
         * Author : HSLEE
         * Function : SetMaxAccel(int scannerIndex, double dAcc)
         * Description : Stage start-up 과정의 최대 가속도 / Stage 관성 모멘트에 따라 설정
         *               사용자 입력 단위 - um    
         *               Scanner 단위 - m
         * Parameter : double dAcc
         ------------------------------------------------------------------------------------*/
        public void SetMaxAccel(int scannerIndex, double dAcc)
        {
            m_PolygonData[scannerIndex].CrossScanMaxAccel = dAcc;
        }


        /*------------------------------------------------------------------------------------
         * Date : 2016.02.25
         * Author : HSLEE
         * Function : SetEnCarSig(int scannerIndex, double dAcc)
         * Description : Stage Control Encoder signal 출력여부
         * Parameter : int nSig
         *             0 = JOBSTART_N 미수신시작, Stageencoder 출력없음
         *             1 = JOBSTART_N 수신후시작, Stageencoder 출력있음
         ------------------------------------------------------------------------------------*/
        public void SetEnCarSig(int scannerIndex, int nSig)
        {
            m_PolygonData[scannerIndex].EnCarSig = nSig;
        }


        /*------------------------------------------------------------------------------------
         * Date : 2016.02.25
         * Author : HSLEE
         * Function : SetSwapCarSig(int scannerIndex, double dAcc)
         * Description : Stage movement direction 선택
         * Parameter : int nSig
         *             0 = Bitmap이미지X축역상
         *             1 = Bitmap이미지X축역상없음
         ------------------------------------------------------------------------------------*/
        public void SetSwapCarSig(int scannerIndex, int nSig)
        {
            m_PolygonData[scannerIndex].SwapCarSig = nSig;
        }


        /*------------------------------------------------------------------------------------
        * Date : 2016.02.25
        * Author : HSLEE
        * Function : SetSwapCarSig(int scannerIndex, double dAcc)
        * Description : exposure의 첫 scanline에 해당하는 facet 지정
        * Parameter : int nFaceTNo [ 0 ~ 7 ]
        ------------------------------------------------------------------------------------*/
        public void SetStartFacet(int scannerIndex, int nFaceTNo)
        {
            m_PolygonData[scannerIndex].StartFacet = nFaceTNo;
        }


        /*------------------------------------------------------------------------------------
        * Date : 2016.02.25
        * Author : HSLEE
        * Function : SetAutoIncStartFacet(int scannerIndex, int nSig)
        * Description : exposure의 첫 scanline에 해당하는 facet를 새로운 가공시작시 Facet 자동증가 옵션
        * Parameter : int nFaceTNo 
        *             0 = Disable
        *             1 = Start Facet 자동증가
        ------------------------------------------------------------------------------------*/
        public void SetAutoIncStartFacet(int scannerIndex, int nSig)
        {
            m_PolygonData[scannerIndex].AutoIncrementStartFacet = nSig;
        }


        /*------------------------------------------------------------------------------------
        * Date : 2016.03.03
        * Author : HSLEE
        * Function : SetScannerIP(int scannerIndex, string strIP)
        * Description : 
        * Parameter : int scannerIndex - Scanner No.
        *             string strIP - Scanner IP Address
        ------------------------------------------------------------------------------------*/
        public void SetScannerIP(int scannerIndex, string strIP)
        {
            m_IP[scannerIndex] = strIP;
        }

        /*------------------------------------------------------------------------------------
        * Date : 2016.03.03
        * Author : HSLEE
        * Function : SetScannerPort(int scannerIndex, string strPort)
        * Description : 
        * Parameter : int scannerIndex - Scanner No.
        *             string strPort - Scanner Port No.
        ------------------------------------------------------------------------------------*/
        public void SetScannerPort(int scannerIndex, string strPort)
        {
            m_Port[scannerIndex] = strPort;
        }

        /*------------------------------------------------------------------------------------
         * Date : 2016.02.24
         * Author : HSLEE
         * Function : GetIPData(int scannerIndex)
         * Description :  
         * Parameter : int scannerIndex - Scanner No.
         * return : ex) IPAddress "ftp://192.168.22.60:21/"
         ------------------------------------------------------------------------------------*/
        public string GetIPData(int scannerIndex)
        {
            return IPAddress[scannerIndex] = string.Format("ftp://{0:s}:{1:s}/", m_IP[scannerIndex], m_Port[scannerIndex]);
        }

        /*------------------------------------------------------------------------------------
         * Date : 2016.02.24
         * Author : HSLEE
         * Function : SendConfig(int scannerIndex, string strFile)
         * Description : Scanner에 Configure ini 파일전송 
         *               File Path = SFA\LWDicer\ScannerLog
         * Parameter :   int scannerIndex - Scanner No.
         *               string strFile - 전송하고 자하는 ini File Name
         ------------------------------------------------------------------------------------*/
        public bool SendConfig(int scannerIndex, string strFile)
        {
            string strFTP = string.Format("{0:s}{1:s}",GetIPData(scannerIndex),strFile); // ex) "ftp://192.168.22.60:21/configure.ini"

            string strPath = string.Format("{0:s}{1:s}", m_DBInfo.ScannerLogDir,strFile);  // ex) "SFA\LWDicer\ScannerLog\configure.ini"

            if (SendFTPFile(strPath, strFTP) == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /*------------------------------------------------------------------------------------
         * Date : 2016.02.24
         * Author : HSLEE
         * Function : SendBitMap(string strFile)
         * Description : Scanner에 Configure BitMap 파일전송 
         *               File Path = SFA\LWDicer\ScannerLog
         * Parameter :   int scannerIndex - Scanner No.
         *               string strFile - 전송하고 자하는 BitMap File Name
         ------------------------------------------------------------------------------------*/
        public bool SendBitMap(int scannerIndex, string strFile)
        {
            string strFTP = string.Format("{0:s}{1:s}", GetIPData(scannerIndex), strFile); // ex) "ftp://192.168.22.60:21/BitMap.bmp"

            string strPath = string.Format("{0:s}{1:s}", m_DBInfo.ScannerLogDir, strFile); // ex) "SFA\LWDicer\ScannerLog\BitMap.bmp"

            if (SendFTPFile(strPath, strFTP) == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /*------------------------------------------------------------------------------------
         * Date : 2016.02.24
         * Author : HSLEE
         * Function : LSEPortOpen()
         * Description : Scanner LSE Controller Serial COM Port Open
         *               m_COM - Hardware Layer ISerialPort 생성
         ------------------------------------------------------------------------------------*/
        public int LSEPortOpen()
        {
            return m_COM.OpenPort();
        }

        /*------------------------------------------------------------------------------------
         * Date : 2016.02.24
         * Author : HSLEE
         * Function : LSEPortClose()
         * Description : Scanner LSE Controller Serial COM Port Close
         *               m_COM - Hardware Layer ISerialPort 생성
         ------------------------------------------------------------------------------------*/
        public int LSEPortClose()
        {
            return m_COM.ClosePort();
        }


        /*------------------------------------------------------------------------------------
         * Date : 2016.02.24
         * Author : HSLEE
         * Function : GetSerialData(out string Message)
         * Description : Scanner LSE Controller 로 부터 Serial 통신으로 Data Read
         * Parameter : out string Message - Serial Port Event Handler로 부터 발생한 Data를 Line 별로 읽음
         ------------------------------------------------------------------------------------*/
        public int GetSerialData(out string Message)
        {
            string message = "";
            int QueueSize = 0;

            if (m_COM.IsOpened())
            {
                m_COM.ReceiveMessage(out message, out QueueSize);

                if(QueueSize != 0)
                {
                    Message = message;

                    return SUCCESS;
                }
                else
                {
                    Message = "";
                    return ERR_SERIALPORT_RECEIVEDQUE_EMPTY;
                }
            }
            else
            {
                Message = "";
                return ERR_SERIALPORT_OPENPORT_FAIL;
            }
        }

        /*------------------------------------------------------------------------------------
         * Date : 2016.02.24
         * Author : HSLEE
         * Function : GetValue(String Section, String Key, String iniPath)
         * Description : Text File Data Load 처리
         ------------------------------------------------------------------------------------*/
        public String GetValue(String Section, String Key, String iniPath)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, iniPath);
            return temp.ToString();
        }

        /*------------------------------------------------------------------------------------
         * Date : 2016.02.24
         * Author : HSLEE
         * Function : SetValue(String Section, String Key, String Value, String iniPath)
         * Description : Text File Data Save 처리
         ------------------------------------------------------------------------------------*/
        public bool SetValue(String Section, String Key, String Value, String iniPath)
        {
            bool bRet = WritePrivateProfileString(Section, Key, Value, iniPath);
            return WritePrivateProfileString(Section, Key, Value, iniPath);
        }

        /*------------------------------------------------------------------------------------
        * Date : 2016.02.24
        * Author : HSLEE
        * Function : SendFTPFile(string strPath, string strFTP)
        * Description : Scanner LSE Controller FTP Data 전송
        * Parameter : strPath - 전송하고 하는 파일 경로   ex)  SFA\LWDicer\ScannerLog\BitMap.bmp"
        *             strFTP - Controlller IP, Port, File Name 조합 ex) ex) "ftp://192.168.22.60:21/BitMap.bmp"
        ------------------------------------------------------------------------------------*/
        public bool SendFTPFile(string strPath, string strFTP)
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

        /*------------------------------------------------------------------------------------
        * Date : 2016.02.26
        * Author : HSLEE
        * Function : SetPicSize(int nX, int nY)
        * Description : BitMap Image를 그리기 위해 가로 세로 Picture Box Size를 생성
        * Parameter : nX - Image 가로 Size
        *             nY - Image 세로 Size
        ------------------------------------------------------------------------------------*/
        public void SetPicSize(int nX, int nY)
        {
            PicWafer.Width = nX;
            PicWafer.Height = nY;

            // BitMap Image 생성
            Image = new Bitmap(PicWafer.Width, PicWafer.Height);

            // 생성된 BitMap Image에 Graphic 속성을 생성
            m_Grapic = Graphics.FromImage(Image);

            // PictureBox Image 생성         
            PicWafer.Image = Image;

            // Background Color는 White
            m_Grapic.Clear(Color.White);
        }

        /// <summary>
        /// Date : 2016.03.11
        /// Author : HSLEE
        /// Function : InitializeTFTP()
        /// Description : Scanner LSE Controller TFTP Data을 위한 초기화 구문
        /// </summary>
        public void InitializeTFTP()
        {
            proInfo.FileName = @"cmd";

            proInfo.CreateNoWindow = true;

            proInfo.UseShellExecute = false;

            proInfo.RedirectStandardOutput = true;

            proInfo.RedirectStandardInput = true;

            proInfo.RedirectStandardError = true;

            m_process.StartInfo = proInfo;

        }

        /// <summary>
        /// Date : 2016.03.11
        /// Author : HSLEE
        /// Function : SendTFTPFile(string strIP, string strFTP)
        /// Description : Scanner LSE Controller TFTP Data 전송
        ///               ex) tftp -i 192.168.22.76 put t:\test.bmp
        /// </summary>
        /// <param name="strIP"></param>  : TFTP Server IP
        /// <param name="strFilePath"></param> : 전송하고자 하는 File Path
        public bool SendTFTPFile(string strIP, string strFilePath)
        {
            string strTFTP = string.Empty;

            strTFTP = string.Format("tftp -i {0:s} put {1:s}", strIP, strFilePath);

            m_process.Start();

            m_process.StandardInput.Write(strTFTP + Environment.NewLine);

            m_process.StandardInput.Close();

            if (m_process.WaitForExit(10000) == false)
            {
                m_process.Close();
                return false;
            }

            m_process.Close();
            return true;
        }


        /*------------------------------------------------------------------------------------
        * Date : 2016.02.26
        * Author : HSLEE
        * Function : SetDrawLine(float X1, float Y1, float X2, float Y2)
        * Description : 생성된 Image에 Line을 그린다.
        * Parameter : X1 - Line 시작 지점의 X Point
        *             Y1 - Line 시작 지점의 Y Point
        *             X2 - Line 끝 지점의 X Point      
        *             Y2 - Line 끝 지점의 Y Point
        *             Width - Line 굵기  
        ------------------------------------------------------------------------------------*/
        public void SetDrawLine(float X1, float Y1, float X2, float Y2, float Width)
        {
            Pen m_Pen = new Pen(Color.Black, Width);

            m_Grapic.DrawLine(m_Pen, (float)X1, (float)Y1, (float)X2, (float)Y2);
        }


        /*------------------------------------------------------------------------------------
        * Date : 2016.02.26
        * Author : HSLEE
        * Function : SaveImage(string strBMP)
        * Description : Bitmap Type Iamge 저장
        * Parameter : strBMP - Image 생성 하고자 하는 파일 이름
        ------------------------------------------------------------------------------------*/
        public void SaveImage(string strBMP)
        {
            string strFile = "";

            strFile = string.Format("{0:s}{1:s}.bmp", m_DBInfo.ScannerLogDir, strBMP);

            Bitmap bmp = new Bitmap(PicWafer.Width, PicWafer.Height);

            PicWafer.DrawToBitmap(bmp, new Rectangle(0, 0, PicWafer.Width, PicWafer.Height));

            // 흑백색으로 구성된 단색 Bitmap 형식으로 변환해야함 [Scanner에서 단색 비트맵 인식]
            // BMP 파일 비트 수준 : 1
            // 1. 단색 비트맵을 저장을 위한 Bitmap 생성
            Bitmap SaveImage = new Bitmap(PicWafer.Width, PicWafer.Height, PixelFormat.Format1bppIndexed);

            // 2. 사용자가 입력한 Image Size에 해당하는 복사본을 만들위한 Rectangle 생성
            Rectangle rectangle = new Rectangle(0, 0, PicWafer.Width, PicWafer.Height);

            // 3. 원본 이미지에 단색 Bitmap 속성을 바꾼 복사본을 만든다.
            SaveImage = bmp.Clone(rectangle, PixelFormat.Format1bppIndexed);

            SaveImage.Save(strFile);
        }


        /*------------------------------------------------------------------------------------
        * Date : 2016.02.26
        * Author : HSLEE
        * Function : DrawRound(LineData Data)
        * Description : Wafer 원형 모양의 Line
        * Parameter : LineData Data - Cut Line 가공 데이타
        ------------------------------------------------------------------------------------*/
        public void DrawRound(LineData Data)
        {
            int nCount = 0, i = 0;
            double X1 = 0.0, X2 = 0.0, dPitch = 0.0;
            double dA = 0.0, dB = 0.0, dSum = 0.0;

            nCount = Data.nLineCount;

            for (i = 0; i < nCount; i++)
            {
                dA = Math.Pow((Data.nWaferSize / 2) - dPitch, 2);
                dB = Math.Pow((Data.nWaferSize / 2), 2);
                dSum = dB - dA;

                X1 = (Data.nWaferSize / 2) - Math.Sqrt(dSum);   // X1
                X2 = ((Math.Sqrt(dSum)) * 2) + X1;              // X2

                Data.fLineData[i, 0] = Convert.ToSingle(string.Format("{0:f4}", X1));
                Data.fLineData[i, 2] = Convert.ToSingle(string.Format("{0:f4}", X2));

                dPitch = dPitch + Data.fPitch;
            }
        }


        /*------------------------------------------------------------------------------------
        * Date : 2016.02.26
        * Author : HSLEE
        * Function : DrawSquare(LineData Data)
        * Description : 정사각형 모양의 Line
        * Parameter : LineData Data - Cut Line 가공 데이타
        ------------------------------------------------------------------------------------*/
        public void DrawSquare(LineData Data)
        {
            float fPitch = 0;
            int i = 0;

            for (i = 0; i < Data.nLineCount; i++)
            {
                Data.fLineData[i, 0] = 0;                                                    // X1
                Data.fLineData[i, 1] = Convert.ToSingle(string.Format("{0:f4}", fPitch));    // Y1
                Data.fLineData[i, 2] = Data.nWaferSize;                                      // X2
                Data.fLineData[i, 3] = Convert.ToSingle(string.Format("{0:f4}", fPitch));    // Y2

                fPitch = Data.fPitch + fPitch;
            }
        }
    }
}
