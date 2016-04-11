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
 
        public const int PATTERN_A = 0;
        public const int PATTERN_B = 1;


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

        public const int DEF_USE_SEARCH_MARK_NO = 2;
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
            ALIGN_MARK_A,
            ALIGN_MARK_B,
            ALIGN_MARK_COUNT
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

        // Vision DB 데이터
        public class CSearchData
        {
            //Pattern File Path
            public string m_strFilePath;
            //Pattern File Name
            public string m_strFileName;
            // Model 등록 여부 
            public bool m_bIsModel;
            // Model Rectangle 
            public Rectangle m_rectModel;
            // Search Area Pos. & Size 
            public Rectangle m_rectSearch;
            // Reference Point 
            public Point m_pointReference;
            // Acceptance Threshold
            public double m_dAcceptanceThreshold;
            // Certainty Threshold
            public double m_dCertaintyThreshold;

        };

        // Vision Pattern Data
        // DB Data + Mil Data임
        public class CVisionPatternData : CSearchData
        {
            /// Search Model Data
            // MIL 에서 사용하는 Model ID (NGC)
            public MIL_ID m_milModel = new MIL_ID();
            // MIL 에서 영상 Display용
            public MIL_ID m_ModelImage = new MIL_ID();
            // MIL 에서 사용하는 Model ID (GMF)
            public MIL_ID m_milGmfModel = new MIL_ID();
        }

        public class CResultData
        {
            //  Search 작업 성공 여부
            public bool m_bSearchSuccess;

            //  Vision 좌표계를 따르는 인식 좌표
            //  Display View 좌측 상단 좌표 : (0.0, 0.0)          
            public double m_dPixelX;
            public double m_dPixelY;

            // Score : 인식률 
            public double m_dScore;

            // Search 작업 성공 결과 Model Pos. & Size 
            public Rectangle m_rectFindedModel;

            // Search Area Pos. & Size 
            public Rectangle m_rectSearch;

            // Search Result 를 저장할 문자열 
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

            // Search Result 를 저장할 문자열 
            public string m_strResult;

        };

        public class CVisionRefComp
        {
            public MVisionSystem System;
            public MVisionCamera[] Camera = new MVisionCamera[DEF_MAX_CAMERA_NO];
            public MVisionView[] View = new MVisionView[DEF_MAX_CAMERA_NO];
        };

        /************************************************************************/
        /*                          Error Code Define                           */
        /************************************************************************/

        // Error Define
        public const int ERR_VISION_SYSTEM_FAIL                 = 10;
        public const int ERR_VISION_SYSTEM_CHECK_FAIL           = 11;
        public const int ERR_VISION_SYSTEM_CREATE_FAIL          = 12;
        public const int ERR_VISION_SYSTEM_OPEN_FAIL            = 13;
        public const int ERR_VISION_SYSTEM_CHECK_CAM_FAIL       = 14;  

        public const int ERR_VISION_CAMERA_FAIL                 = 20;
        public const int ERR_VISION_CAMERA_NON_USEFUL           = 20;
        public const int ERR_VISION_CAMERA_CREATE_FAIL          = 21;
        public const int ERR_VISION_CAMERA_GET_INFO_FAIL        = 22;
        public const int ERR_VISION_CAMERA_CONNECT_FAIL         = 23;
        public const int ERR_VISION_CAMERA_IMAGE_SIZE_FAIL      = 23;
        public const int ERR_VISION_CAMERA_CREATE_IMAGE_FAIL    = 24;
        public const int ERR_VISION_CAMERA_GET_IMAGE_FAIL       = 25;
        public const int ERR_VISION_CAMERA_SET_CALLBACK_FAIL    = 26;

        public const int ERR_VISION_PATTERN_NONE                = 30;
        public const int ERR_VISION_PATTERN_NUM_OVER            = 31;
        public const int ERR_VISION_PATTERN_SEARCH_FAIL         = 32;
        public const int ERR_VISION_PATTERN_REG_FAIL            = 33;
        public const int ERR_VISION_EDGE_SEARCH_FAIL            = 34;
        public const int ERR_VISION_SEARCH_SIZE_OVER            = 35;

        public const int ERR_VISION_PARAMETER_UNFIT             = 40;
        public const int ERR_VISION_FOLDER_FAIL                 = 50;

        public const int ERR_VISION_SUCCESS = 0;
        public const int ERR_VISION_ERROR = 5;



    }
}
