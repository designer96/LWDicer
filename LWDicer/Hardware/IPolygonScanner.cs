using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

using static LWDicer.Control.DEF_PolygonScanner;

namespace LWDicer.Control
{
    public class DEF_PolygonScanner
    {
        public static readonly String PATH_APPLICATION = "T:/SFA/LWDicer/";
        public static readonly String PATH_SYSTEM = PATH_APPLICATION + "System/";
        public static readonly String PATH_DATA = PATH_APPLICATION + "Data/";
        public static readonly String PATH_IMAGE = PATH_APPLICATION + "Image/";
        public static readonly String PATH_PROGRAM = PATH_APPLICATION + "Program/";
        public static readonly String PATH_LOG = PATH_APPLICATION + "Log/";
        public static readonly String PATH_CONFIG = PATH_APPLICATION + "Configure/";

        public static readonly String PATH_CONFIG_INI = PATH_CONFIG + "config.ini";


        [DllImport("kernel32.dll")]
        public static extern int GetPrivateProfileString(
        String section,
        String key,
        String def,
        StringBuilder retVal,
        int size,
        String filePath);

        [DllImport("kernel32.dll")]
        public static extern bool WritePrivateProfileString(
            String section,
            String key,
            String val,
            String filePath);

        public class PolygonScannerData
        {
            public PolygonScannerData()
            {

            }
        }

        public class CPolygonParameter
        {
            /* [Job Settings] */
            public double InScanResolution;     // USER ENABLE
            public double CrossScanResolution;  // USER ENABLE
            public double InScanOffset;         // USER ENABLE
            public int StopMotorBetweenJobs;    // USER ENABLE
            public int PixInvert;               // USER ENABLE
            public int JobStartBufferTime;      // USER ENABLE
            public int PrecedingBlankLines;     // USER ENABLE

            /* [Laser Configuration] */
            public int LaserOperationMode;
            public Int32 SeedClockFrequency;      // USER ENABLE
            public Int32 RepetitionRate;          // USER ENABLE
            public int PulsePickWidth;
            public int PixelWidth;
            public int PulsePickAlgor;
            public int UseCoaxIf;

            /* [CrossScan Configuration] */
            public double CrossScanEncoderResol;// USER ENABLE
            public int CrossScanMaxAccel;       // USER ENABLE  
            public int EnCarSig;                // USER ENABLE
            public int SwapCarSig;              // USER ENABLE

            /* [Head Configuration] */
            public Int32 SerialNumber;
            public double FThetaConstant;
            public double ExposeLineLength;
            public int EncoderIndexDelay;
            public double FacetFineDelay0;
            public double FacetFineDelay1;
            public double FacetFineDelay2;
            public double FacetFineDelay3;
            public double FacetFineDelay4;
            public double FacetFineDelay5;
            public double FacetFineDelay6;
            public double FacetFineDelay7;
            public int InterleaveRatio;         // USER ENABLE
            public double FacetFineDelayOffset0;// USER ENABLE
            public double FacetFineDelayOffset1;// USER ENABLE
            public double FacetFineDelayOffset2;// USER ENABLE
            public double FacetFineDelayOffset3;// USER ENABLE
            public double FacetFineDelayOffset4;// USER ENABLE
            public double FacetFineDelayOffset5;// USER ENABLE
            public double FacetFineDelayOffset6;// USER ENABLE
            public double FacetFineDelayOffset7;// USER ENABLE
            public int StartFacet;              // USER ENABLE
            public int AutoIncrementStartFacet; // USER ENABLE

            /* [Polygon motor Configuration] */
            public int InternalMotorDriverClk;
            public int MotorDriverType;
            public int MotorSpeed;
            public int SimEncSel;
            public double MinMotorSpeed;
            public double MaxMotorSpeed;
            public int MotorEffectivePoles;
            public int SyncWaitTime;
            public int MotorStableTime;         // USER ENABLE

            /* [Other Settings] */
            public int InterruptFreq;
            public int HWDebugSelection;
            public int AutoRepeat;
            public int PixAlwaysOn;
            public int ExtCamTrig;
            public int EncoderExpo;
            public int FacetTest;
            public int SWTest;
            public int JobstartAutorepeat;
        }

    }
    public interface IPolygonScanner
    {
        int GetSerialData(out string Message);

        void LoadPolygonPara(CPolygonParameter PolygonPara);

        void SavePolygonPara(CPolygonParameter PolygonPara);

        bool SendConfig(string strPath, string strFTP);

        bool SendBitMap(string strPath, string strFTP);
    }
}
