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
    public class MPolygonScanner : MObject, IPolygonScanner
    {
        private ISerialPort m_COM;
        private CDBInfo m_DBInfo;

        protected string IPAddress;

        protected CPolygonParameter m_PolygonData;

        public MPolygonScanner(CObjectInfo objInfo, CPolygonParameter PolygonPara , CPolygonScannerData ScannerData, ISerialPort SerialPort)
            : base(objInfo)
        {
            m_DBInfo = new CDBInfo();

            LoadPolygonPara(PolygonPara);

            m_PolygonData = PolygonPara;

            m_COM = SerialPort;

            SetIPData(ScannerData);
        }

        public bool LoadPolygonPara(CPolygonParameter PolygonPara)
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
            PolygonPara.SeedClockFrequency = Convert.ToDouble(value);

            key = "RepetitionRate";
            value = GetValue(section, key, filepath);
            PolygonPara.RepetitionRate = Convert.ToDouble(value);

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
            PolygonPara.CrossScanMaxAccel = Convert.ToDouble(value);

            key = "EnCarSig";
            value = GetValue(section, key, filepath);
            PolygonPara.EnCarSig = Convert.ToInt16(value);

            key = "SwapCarSig";
            value = GetValue(section, key, filepath);
            PolygonPara.SwapCarSig = Convert.ToInt16(value);

            section = "Head Configuration";

            key = "SerialNumber";
            value = GetValue(section, key, filepath);
            PolygonPara.SerialNumber = Convert.ToDouble(value);

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

            return true;
        }

        public bool SavePolygonPara()
        {
            string section = "";
            string key = "";
            string value = "";
            string filepath = "";
            bool bRet = false;

            filepath = string.Format("{0:s}config.ini", m_DBInfo.ScannerLogDir);

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

        public CPolygonParameter GetPolygonPara()
        {
            return m_PolygonData;
        }


        /*------------------------------------------------------------------------------------
         * Date : 2016.02.24
         * Author : HSLEE
         * Function : SetPixelGridX(double pX)
         * Description : Polygon Scanner의 X Pixel Grid Size를 설정한다.
         *               사용자 입력 단위 - um    
         *               Scanner 단위 - m
         * Parameter : double pX - Scanline 에서 이웃한 두 pixel 사이 X축 거리
         ------------------------------------------------------------------------------------*/
        public void SetPixelGridX(double pX)
        {
            m_PolygonData.InScanResolution = 0.000001 * pX;
        }


        /*------------------------------------------------------------------------------------
         * Date : 2016.02.24
         * Author : HSLEE
         * Function : SetPixelGridY(double pY)
         * Description : Polygon Scanner의 Y Pixel Grid Size를 설정한다.
         *               사용자 입력 단위 - um    
         *               Scanner 단위 - m
         * Parameter : double pY - 이웃한 두 Scanline 사이 Y축 거리
         ------------------------------------------------------------------------------------*/
        public void SetPixelGridY(double pY)
        {
            m_PolygonData.CrossScanResolution = 0.000001 * pY;
        }


        /*------------------------------------------------------------------------------------
         * Date : 2016.02.24
         * Author : HSLEE
         * Function : SetBitMapColor(int  nColor)
         * Description : Polygon Scanner에서 처리하는 LaserPulse Exposure Bitmap의 색상 선택
         * Parameter : int  nColor - Scanline에서이웃한두pixel 사이X축거리
         *             BLACK = 0;  //0 = Black에서Laser On
         *             WHITE = 1;  //1 = White 에서Laser On
         ------------------------------------------------------------------------------------*/
        public void SetBitMapColor(int  nColor)
        {
            m_PolygonData.PixInvert = nColor;
        }


        /*------------------------------------------------------------------------------------
         * Date : 2016.02.24
         * Author : HSLEE
         * Function : SetSuperSync(double OffSet, int nNo)
         * Description : Scanner 내부 8개의 각 Mirror scanline의 시작위치 값의 미세 조정
         *               사용자 입력 단위 - um    
         *               Scanner 단위 - m
         * Parameter : double OffSet - 미세조정 OffSet Data
         *             int nNo -  Mirror No
         ------------------------------------------------------------------------------------*/
        public void SetSuperSync(double OffSet, int nNo)
        {
            switch(nNo)
            {
                case 0:
                    m_PolygonData.FacetFineDelay0 = 0.000001 * OffSet;
                    break;
                case 1:
                    m_PolygonData.FacetFineDelay1 = 0.000001 * OffSet;
                    break;
                case 2:
                    m_PolygonData.FacetFineDelay2 = 0.000001 * OffSet;
                    break;
                case 3:
                    m_PolygonData.FacetFineDelay3 = 0.000001 * OffSet;
                    break;
                case 4:
                    m_PolygonData.FacetFineDelay4 = 0.000001 * OffSet;
                    break;
                case 5:
                    m_PolygonData.FacetFineDelay5 = 0.000001 * OffSet;
                    break;
                case 6:
                    m_PolygonData.FacetFineDelay6 = 0.000001 * OffSet;
                    break;
                case 7:
                    m_PolygonData.FacetFineDelay7 = 0.000001 * OffSet;
                    break;
            }
        }

        /*------------------------------------------------------------------------------------
         * Date : 2016.02.24
         * Author : HSLEE
         * Function : SetStartOffset(double OffSet)
         * Description : 모든 Scanline의 X축 시작 위치를 조정
         *               사용자 입력 단위 - um    
         *               Scanner 단위 - m
         * Parameter : double OffSet
         ------------------------------------------------------------------------------------*/
        public void SetStartOffset(double OffSet)
        {
            m_PolygonData.InScanOffset = 0.000001 * OffSet;
        }


        /*------------------------------------------------------------------------------------
         * Date : 2016.02.24
         * Author : HSLEE
         * Function : SetSeedClock(double Frequency)
         * Description : Laser의 Seed Clock 주파수 설정
         *               사용자 입력 단위 - kHz    
         *               Scanner 단위 - Hz
         * Parameter : double Frequency - kHz 단위
         ------------------------------------------------------------------------------------*/
        public void SetSeedClock(double Frequency)
        {
            m_PolygonData.SeedClockFrequency = 1000 * Frequency;
        }


        /*------------------------------------------------------------------------------------
         * Date : 2016.02.24
         * Author : HSLEE
         * Function : SetRepRate(double Frequency)
         * Description : 가공에적용할펄스반복률(REP_RATE)설정
         *               사용자 입력 단위 - kHz    
         *               Scanner 단위 - Hz
         * Parameter : double Frequency - kHz 단위
         ------------------------------------------------------------------------------------*/
        public void SetRepRate(double Frequency)
        {
            m_PolygonData.RepetitionRate = 1000 * Frequency;
        }


        /*------------------------------------------------------------------------------------
         * Date : 2016.02.24
         * Author : HSLEE
         * Function : SetPixelWidth(double SeedClock, double RefRate)
         * Description : Laser SeedClock, Rep Rate 변경에 따른 Laser Pulse Width 변경
         *               사용자 입력 단위 - kHz    
         *               Scanner 단위 - Hz
         * Parameter : double SeedClock - 적용될 SeedClockFrequency
         *             double RepRate -  적용될 RepetitionRate
         ------------------------------------------------------------------------------------*/
        public void SetPixelWidth(double SeedClock, double RepRate)
        {
            m_PolygonData.PixelWidth = (int)((SeedClock / RepRate) / 2);
            m_PolygonData.PulsePickWidth = (int)(m_PolygonData.PixelWidth / 2);
        }

        /*------------------------------------------------------------------------------------
         * Date : 2016.02.24
         * Author : HSLEE
         * Function : SetLaserOP(int nOption)
         * Description : Laser 제어 방식 선택
         * Parameter : int nOption
         *             SuperSync=0,        // SuperSync 동작
         *             NotUsed,            // Not-used
         *             PulseOut,           // PULSE_OUT 출력만사용, SuperSync 중지
         *             NoneSeedClock,      // Trumph TruMicro Laser 전용(SEED CLOCK 미사용)
         *             InternalSeedClock   // AVIA 등나노초레이저용(내부SEED CLOCK 사용)
         ------------------------------------------------------------------------------------*/
        public void SetLaserOP(int nOption)
        {
            m_PolygonData.LaserOperationMode = nOption;
        }


        /*------------------------------------------------------------------------------------
         * Date : 2016.02.24
         * Author : HSLEE
         * Function : SetBufferTime(int nSec)
         * Description : Bitmap Uploading 시, exposure 하기전 대기 시간
         * Parameter : int nSec - 사용자 설정 시간 (sec)
         ------------------------------------------------------------------------------------*/
        public void SetBufferTime(int nSec)
        {
            m_PolygonData.JobStartBufferTime = nSec;
        }


        /*------------------------------------------------------------------------------------
         * Date : 2016.02.24
         * Author : HSLEE
         * Function : SetDummyBlankLine(int nScanLine)
         * Description : Stage 가속시의 충분한 Settle-time 을 위해 Dummy로 추가하는 scanline 수
         * Parameter : int nScanLine - 사용자 설정 Dummy Scan Line
         ------------------------------------------------------------------------------------*/
        public void SetDummyBlankLine(int nScanLine)
        {
            m_PolygonData.PrecedingBlankLines = nScanLine;
        }


        /*------------------------------------------------------------------------------------
         * Date : 2016.02.24
         * Author : HSLEE
         * Function : SetMotorBetweenJob(int nOption)
         * Description : Exposure 이후 polygon mirror 정지 여부 결정
         * Parameter : int nOption - SPIN or STOP
         ------------------------------------------------------------------------------------*/
        public void SetMotorBetweenJob(int nOption)
        {
            m_PolygonData.StopMotorBetweenJobs = nOption;
        }


        /*------------------------------------------------------------------------------------
         * Date : 2016.02.24
         * Author : HSLEE
         * Function : SetMotorStableTime(int nTime)
         * Description : speed-up 이후 exposure 시작 이전에 spinning 안정화를 위한 대기 시간
         * Parameter : int nTime - 미리초(ms) 단위로 입력 
         ------------------------------------------------------------------------------------*/
        public void SetMotorStableTime(int nTime)
        {
            m_PolygonData.MotorStableTime = nTime;
        }


        /*------------------------------------------------------------------------------------
         * Date : 2016.02.24
         * Author : HSLEE
         * Function : SetLeaveRatio(int nRatio)
         * Description : FacetFineDelayOffset 자동 설정 기능
         * Parameter : int nRatio
         ------------------------------------------------------------------------------------*/
        public void SetLeaveRatio(int nRatio)
        {
            m_PolygonData.InterleaveRatio = nRatio;
        }


        /*------------------------------------------------------------------------------------
         * Date : 2016.02.25
         * Author : HSLEE
         * Function : SetEncoderResol(double dResol)
         * Description : Stage Encoder 분해능 값 설정 / A상 edge 에서 B상 edge 까지 거리
         *               사용자 입력 단위 - um    
         *               Scanner 단위 - m
         * Parameter : double dResol
         ------------------------------------------------------------------------------------*/
        public void SetEncoderResol(double dResol)
        {
            m_PolygonData.CrossScanEncoderResol = 0.000001 * dResol;
        }


        /*------------------------------------------------------------------------------------
         * Date : 2016.02.25
         * Author : HSLEE
         * Function : SetMaxAccel(double dAcc)
         * Description : Stage start-up 과정의 최대 가속도 / Stage 관성 모멘트에 따라 설정
         *               사용자 입력 단위 - um    
         *               Scanner 단위 - m
         * Parameter : double dAcc
         ------------------------------------------------------------------------------------*/
        public void SetMaxAccel(double dAcc)
        {
            m_PolygonData.CrossScanMaxAccel = dAcc;
        }


        /*------------------------------------------------------------------------------------
         * Date : 2016.02.25
         * Author : HSLEE
         * Function : SetEnCarSig(double dAcc)
         * Description : Stage Control Encoder signal 출력여부
         * Parameter : int nSig
         *             0 = JOBSTART_N 미수신시작, Stageencoder 출력없음
         *             1 = JOBSTART_N 수신후시작, Stageencoder 출력있음
         ------------------------------------------------------------------------------------*/
        public void SetEnCarSig(int nSig)
        {
            m_PolygonData.EnCarSig = nSig;
        }


        /*------------------------------------------------------------------------------------
         * Date : 2016.02.25
         * Author : HSLEE
         * Function : SetSwapCarSig(double dAcc)
         * Description : Stage movement direction 선택
         * Parameter : int nSig
         *             0 = Bitmap이미지X축역상
         *             1 = Bitmap이미지X축역상없음
         ------------------------------------------------------------------------------------*/
        public void SetSwapCarSig(int nSig)
        {
            m_PolygonData.SwapCarSig = nSig;
        }


        /*------------------------------------------------------------------------------------
        * Date : 2016.02.25
        * Author : HSLEE
        * Function : SetSwapCarSig(double dAcc)
        * Description : exposure의 첫 scanline에 해당하는 facet 지정
        * Parameter : int nFaceTNo [ 0 ~ 7 ]
        ------------------------------------------------------------------------------------*/
        public void SetStartFacet(int nFaceTNo)
        {
            m_PolygonData.StartFacet = nFaceTNo;
        }


        /*------------------------------------------------------------------------------------
        * Date : 2016.02.25
        * Author : HSLEE
        * Function : SetAutoIncStartFacet(int nSig)
        * Description : exposure의 첫 scanline에 해당하는 facet를 새로운 가공시작시 Facet 자동증가 옵션
        * Parameter : int nFaceTNo 
        *             0 = Disable
        *             1 = Start Facet 자동증가
        ------------------------------------------------------------------------------------*/
        public void SetAutoIncStartFacet(int nSig)
        {
            m_PolygonData.AutoIncrementStartFacet = nSig;
        }


        /*------------------------------------------------------------------------------------
         * Date : 2016.02.24
         * Author : HSLEE
         * Function : SetIPData(CPolygonScannerData ScannerData)
         * Description : CPolygonScannerData Class 에서 받은 IP, Port를 만들어 FTP IP 생성
         * Parameter : CPolygonScannerData ScannerData
         *             string strIP - Scanner IP
         *             string strPort - Scanner Port
         ------------------------------------------------------------------------------------*/
        public void SetIPData(CPolygonScannerData ScannerData)
        {
             IPAddress = string.Format("ftp://{0:s}:{1:s}/", ScannerData.strIP, ScannerData.strPort);
        }


        /*------------------------------------------------------------------------------------
         * Date : 2016.02.24
         * Author : HSLEE
         * Function : GetIPData()
         * Description :  
         * Parameter : 
         * return : ex) IPAddress "ftp://192.168.22.60:21/"
         ------------------------------------------------------------------------------------*/
        public string GetIPData()
        {
            return IPAddress;
        }

        /*------------------------------------------------------------------------------------
         * Date : 2016.02.24
         * Author : HSLEE
         * Function : SendConfig(string strFile)
         * Description : Scanner에 Configure ini 파일전송 
         *               File Path = SFA\LWDicer\ScannerLog
         * Parameter : string strFile - 전송하고 자하는 ini File Name
         ------------------------------------------------------------------------------------*/
        public bool SendConfig(string strFile)
        {
            string strFTP = string.Format("{0:s}{1:s}.ini",GetIPData(),strFile); // ex) "ftp://192.168.22.60:21/configure.ini"

            string strPath = string.Format("{0:s}{1:s}.ini", m_DBInfo.ScannerLogDir,strFile);  // ex) "SFA\LWDicer\ScannerLog\configure.ini"

            if (SendFile(strPath, strFTP) == true)
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
         * Parameter : string strFile - 전송하고 자하는 BitMap File Name
         ------------------------------------------------------------------------------------*/
        public bool SendBitMap(string strFile)
        {
            string strFTP = string.Format("{0:s}{1:s}.bmp", GetIPData(), strFile); // ex) "ftp://192.168.22.60:21/BitMap.bmp"

            string strPath = string.Format("{0:s}{1:s}.bmp", m_DBInfo.ScannerLogDir, strFile); // ex) "SFA\LWDicer\ScannerLog\BitMap.bmp"

            if (SendFile(strPath, strFTP) == true)
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
        * Function : SendFile(string strPath, string strFTP)
        * Description : Scanner LSE Controller FTP Data 전송
        * Parameter : strPath - 전송하고 하는 파일 경로   ex)  SFA\LWDicer\ScannerLog\BitMap.bmp"
        *             strFTP - Controlller IP, Port, File Name 조합 ex) ex) "ftp://192.168.22.60:21/BitMap.bmp"
        ------------------------------------------------------------------------------------*/
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
