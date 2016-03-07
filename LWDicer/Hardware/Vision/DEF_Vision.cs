using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using BGAPI;
using System.Configuration;
using Matrox.MatroxImagingLibrary;

namespace LWDicer.Control
{
    public class DEF_Vision
    {
        /************************************************************************/
        /*             Expansion S/W Module 사용 여부 설정                      */
        /************************************************************************/

        /** MIL Libray Module 사용 */
        //#define USE_MIL

        /** GMF(Geometry Model Finder) Module 사용 */
        //#define USE_MIL_G_MODULE

        /************************************************************************/
        /*                          Value Define                                */
        /************************************************************************/

        /** 설비 의존적 Data Value - User 설정 필요
         *  이하 부분은 이 곳의 형식에 따라, DefSystem.h 에서
         *  각 설비에 맞게 정의할 것.
         */
        /** ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ */
        /** Vision 의존적 Data Value - Value 변경 금지 */

        public static readonly string DEF_VISION_FILE              = ConfigurationManager.AppSettings["AppFilePath"] + "VisionData\\";
        public static readonly string DEF_IMAGE_LOG_FILE           = DEF_VISION_FILE+ "ImageLog\\";
        public static readonly string DEF_PATTERN_FILE             = DEF_VISION_FILE + "PatternFile\\";
        public static readonly string DEF_NGC_MARK_NAME_TEMPLATE   = "{0}\\Cam{1}_Mark{2}_NGC.mmo";
        /** ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ */

        /** Blob Analyze Algorithm */
        public const int DEF_BLOB_BY_DISTANCE = 0;
        public const int DEF_BLOB_BY_ONLY_AREA = 1;

        /** Camera 없음. */
        public const int DEF_NONE_CAMERA = -1;

        /** Mark 없음. */
        public const int DEF_NONE_MARK = -1;

        /** Camera 사용 숫자. */
        public const int DEF_MAX_CAMERA_NO = 2;

        public const int PRE__CAM = 0;
        public const int FINE_CAM = 1;
 

        /** Display Image Resolution */
        public const int DEF_IMAGE_SIZE_X = 1624;
        public const int DEF_IMAGE_SIZE_Y = 1228;

        public const int DEF_SEARCH_MIN_WIDTH   = 30;
        public const int DEF_SEARCH_MIN_HEIGHT  = 30;

        // Grid 간격
        //	: 정사각형 Grid 를 위해서 DEF_IMAGE_SIZE_X 와 DEF_IMAGE_SIZE_Y 의 공약수로 설정해야 한다.
        
        public const int DEF_GRID_GAP = 4;

        /** 하나의 Image 에 대한 한 번의 Search 작업에서 찾을 수 있는 최대 Pattern 수 */
        public const int DEF_MODEL_MAX_OCCURRENCES = 1;        

        // Hair Line Max,Min Width
        public const int DEF_HAIRLINE_MIN = 10;
        public const int DEF_HAIRLINE_NOR = 100;
        public const int DEF_HAIRLINE_MAX = 300;

        // Mark Max,Min Width & Height
        public const int DEF_MARK_WIDTH_MIN = 50;
        public const int DEF_MARK_WIDTH_NOR = 200;
        public const int DEF_MARK_WIDTH_MAX = 400;
        public const int DEF_MARK_HEIGHT_MIN = 50;
        public const int DEF_MARK_HEIGHT_NOR = 200;
        public const int DEF_MARK_HEIGHT_MAX = 400;

        /************************************************************************/
        /*                         Data Structure Define                        */
        /************************************************************************/

        public const int DEF_USE_SEARCH_MARK_NO = 3;
        public const int DEF_DEFAULT_ACCEP_THRESHOLD = 70;
        public const int DEF_DEFAULT_CERTAIN_THRESHOLD = 90;

        /// <summary>
        /// Pattern Mark의 종류
        /// 1. Pre Align Mark
        /// 2. Fine Align Mark-A
        /// 3. Fine Align Mark-B
        /// </summary>
        public enum EPatternMarkType
        {
            PRE_ALIGN_MARK,
            FINE_ALIGN_MARK_A,
            FINE_ALIGN_MARK_B,
        }
        // This structure is defined Vision Component Data list of Vision.
        //
        // @stereotype struct 

        public class CVisionData
        {
            // 설비에서 사용하는 Vision Board 개수
            public int m_iNumOfSystems;

            // AutoView 에서 사용하는 Vision Display View 의 개수 
            public int m_iNumOfViews;

            // Vision Model (Mark) Data Storage Directory Name 
            public string m_strModelFilePath;

            // Error Image Save 기능 사용 여부 
            public bool m_bSaveErrorImage;

        };

        
         // This structure is defined Vision Camera Data list of Vision.
         //
         // @stereotype struct 
         
        public class CCameraData
        {
            // Vision Grab 안정화 시간 : 단위 (ms) 
            public int m_iGrabSettlingTime;
            // Camera Change Time : 단위 (ms) 
            public int m_iCameraChangeTime;
            // Baumer Camera Info
            public BGAPIX_CameraInfo m_CamDeviceInfo;

        };

        public class CSearchData
        {
            /** Pattern File Path */
            public string m_strFilePath;

            /** Pattern File Name */
            public string m_strFileName;

            /** Model 등록 여부 */
            public bool m_bIsModel;

            /** MIL 에서 사용하는 Model ID (NGC) */
            public MIL_ID m_milModel;

            public MIL_ID m_ModelImage;

            /** MIL 에서 사용하는 Model ID (GMF) */
            public MIL_ID m_milGmfModel;

            /** Model Rectangle */
            public Rectangle m_rectModel;

            /** Search Area Pos. & Size */
            public Rectangle m_rectSearch;

            /** Reference Point */
            public Point m_pointReference;

            /** Acceptance Threshold */
            public double m_dAcceptanceThreshold;

            /** Certainty Threshold */
            public double m_dCertaintyThreshold;

            /** Search Angle : MIL 내부 bug 로 사용하면 Error 발생 */
            //	double m_dSearchAngle;

        };

        public class CResultData
        {
            /** Search 작업 성공 여부 */
            public bool m_bSearchSuccess;

            /** Vision 좌표계를 따르는 인식 좌표
             *  Display View 좌측 상단 좌표 : (0.0, 0.0)
             */
            public double m_dPixelX;
            public double m_dPixelY;

            // Score : 인식률 */
            public double m_dScore;

            // Search 작업 성공 결과 Model Pos. & Size */
            public Rectangle m_rectFindedModel;

            // Search Area Pos. & Size */
            public Rectangle m_rectSearch;

            // Search Result 를 저장할 문자열 */
            public string m_strResult;

            // Search 작업에 걸린 시간 
            public double m_dTime;

            // MIL 에서 사용하는 Search Result ID (NGC)
            public MIL_ID m_milResult;

            // MIL 에서 사용하는 Search Result ID (GMF)
            public MIL_ID m_milGMFResult;

        };

        public class CEdgeData
        {
            /** Search 작업 성공 여부 */
            public bool m_bSuccess;

            public int m_iEdgeNum;
            public double[] m_dPosX;
            public double[] m_dPosY;           

        };


        /************************************************************************/
        /*                          Error Code Define                           */
        /************************************************************************/

        /**
         * Error Code Define - Success...
         */
        public const int ERR_VISION_SUCCESS = 0;

        /** Error Code Reference */
        /** generateErrorCode() 를 사용해서 Error Code 를 생성하는 것은
         *  MVision Class 에서만 담당한다.
         *  나머지 Vision 관련 Class 에서는 아래 정의된 에러 코드를 사용한다.
         */

        public const int ERR_VISION_BOARD_NOT_INSTALLED = 1;
        // 104001 = Vision Board 가 설치되지 않았습니다.
        public const int ERR_VISION_ALLOCATION_FAILURE = 2;
        public const int ERR_VISION_REALLOCATION_ATTEMPT = 3;
        public const int ERR_VISION_NOT_LOCAL_VIEW_MODE = 4;
        // 104027 = Local View Mode 가 아닙니다.
        public const int ERR_VISION_ERROR = 5;
        public const int ERR_VISION_INVALID_CAMERA_NUMBER = 6;
        public const int ERR_VISION_GET_INVALID_VALUE = 7;
        // 104025 = 사용할 수 있는 범위 밖의 값입니다.
        // 104055 = 잘못된 Camera 번호입니다.
        // 104026 = OCR Calibration 에 실패했습니다.

        public const int ERR_VISION_IMAGE_BUFFER_ALLOCATION_FAILURE = 999;
        // 104002 = Vision Application Allocation 에 실패했습니다.
        // 104003 = Vision System Allocation 에 실패했습니다.
        // 104004 = Vision Digitizer (Camera) Allocation 에 실패했습니다.
        // 104005 = Vision Display (View) Allocation 에 실패했습니다.

        // 104006 = DCF File 을 찾을 수 없습니다.

        // 104007 = Vision Image Buffer Allocation 에 실패했습니다.
        // 104008 = Model Display 를 위한 Image Buffer Allocation 에 실패했습니다.
        // 104009 = GMF Model Masking 을 위한 Image Buffer Allocation 에 실패했습니다.
        // 104010 = OCR 인식을 위한 Image Buffer Allocation 에 실패했습니다.

        // 104015 = Pattern Matching Model Allocation 에 실패했습니다.
        // 104016 = Blob Analysis Model Allocation 에 실패했습니다.
        // 104017 = Edge Finder Model Allocation 에 실패했습니다.
        // 104018 = OCR Model Allocation 에 실패했습니다.

        //Recognition operations failure caused by Low Scores.
        public const int ERR_VISION_SEARCH_FAILURE = 345;
        // 104020 = Pattern Matching Model 인식에 실패했습니다.
        // 104022 = Edge 를 찾지 못했습니다.      

        // 104030 = Mark 번호 오류입니다.
        public const int ERR_VISION_INVALIDE_SEARCH_MARK_NO = 9999;
        // 104031 = Pattern Matching Model 번호가 아닙니다.

        public const int ERR_VISION_BAD_SIZE = 90;
        // 104040 = Search Area Size 가 부적절합니다.
        // 104041 = Model Area Size 가 부적절합니다.
        public const int ERR_VISION_INVALID_REFERENCE_POINT = 1;

        public const int ERR_VISION_FILE_READ_FAILURE = 1;
        public const int ERR_VISION_FILE_WRITE_FAILURE = 16;
        public const int ERR_BACKUP_FILE_WRITE_FAILURE = 10;

        public const int ERR_VISION_CAMERA_CHANGE = 80;
        // 104045 = Camera Change 에 실패했습니다.

        public const int ERR_VISION_INVALID_MODEL = 1000;
        public const int ERR_VISION_NOT_USE_MODEL = 1;
        public const int ERR_VISION_NO_MODEL = 1;
        // 104050 = 해당 번호로 등록된 Model 이 없습니다.


    }
}
