using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

using static LWDicer.Control.DEF_Vision;
using static LWDicer.Control.DEF_Error;
using BGAPI;
using Matrox.MatroxImagingLibrary;

// Test Git 124
// Comment Insert
// Comment Insert


namespace LWDicer.Control
{
    class MVisionSystem
    {
        
        private int m_iSystemNo;
        private int m_iSystemIndex;
        private int m_iCheckCamNo;
        private int m_iResult;
        
        //=========================================================
        // System
        private MIL_ID m_MilApp;            // Application identifier.
        private MIL_ID m_MilSystem;          // System identifier.
        private MVisionDisplay[] m_pDisplay;

        private BGAPI.System m_System;
        private BGAPI_FeatureState m_State;

        //=========================================================
        // Image 
        private MIL_ID m_MilView = MIL.M_NULL;
        private MIL_ID m_MilViewImage = MIL.M_NULL;

        //=========================================================
        // Pattern Maching 
        private MIL_ID m_SearchResult = MIL.M_NULL;

        //=========================================================
        // Edge Find 
        private MIL_ID m_EdgeMaker = MIL.M_NULL;


        //static const MIL_DOUBLE ERROR_TEXT_POS_Y = 30.0;


        public MVisionSystem()
        {
            m_iSystemNo     =   0;
            m_iSystemIndex  =   0;
            m_iCheckCamNo   =   0;
            m_iResult       =   0;
            m_pDisplay = new MVisionDisplay[DEF_MAX_CAMERA_NO];
            m_MilApp = MIL.M_NULL;
            m_MilSystem = MIL.M_NULL;            
        }

        // Gig-E System을 초기화 한다.
        public int Initialize()
        {                        
            //====================================================================
            // Camera System 초기화  
            m_System = new BGAPI.System();
            m_State = new BGAPI.BGAPI_FeatureState();

            // check system.  
            m_iResult = BGAPI.EntryPoint.countSystems(ref m_iSystemNo);
            if (m_iResult != BGAPI.Result.OK)
            {   // BGAPI.EntryPoint.CountSystems failed
                return ERR_VISION_BOARD_NOT_INSTALLED;
            }
            // create system.  
            m_iResult = BGAPI.EntryPoint.createSystem(m_iSystemIndex, ref m_System);
            if (m_iResult != BGAPI.Result.OK)
            {   // BGAPI.EntryPoint.createSystems failed
                return ERR_VISION_BOARD_NOT_INSTALLED;
            }
            // open system     
            m_iResult = m_System.open();
            if (m_iResult != BGAPI.Result.OK)
            {   // System open failed
                return ERR_VISION_BOARD_NOT_INSTALLED;
            }

            // get camera num  
            m_iResult = m_System.countCameras(ref m_iCheckCamNo);
            if (m_iResult != BGAPI.Result.OK)           
            {   // System count cameras failed!
                return ERR_VISION_BOARD_NOT_INSTALLED;
            }

            //====================================================================
            // Library System 초기화  

            //MIL.MappAllocDefault(MIL.M_DEFAULT, ref m_MilApp, ref m_MilSystem, ref m_MilDisplay, MIL.M_NULL, MIL.M_NULL);

            MIL.MappAlloc(MIL.M_NULL, MIL.M_DEFAULT, ref m_MilApp);

            // Allocate a MIL system.
            MIL.MsysAlloc(MIL.M_DEFAULT, "M_DEFAULT", MIL.M_DEFAULT, MIL.M_DEFAULT, ref m_MilSystem);

            

            return SUCCESS;
        }

        // 현재 System의 주소를 리턴한다.
        public BGAPI.System GetSystem()
        {
            return m_System;
        }
        public MIL_ID GetMilSystem()
        {
            return m_MilSystem;
        }

        public void SelectView(MVisionDisplay m_Display)
        {
            m_pDisplay[m_Display.GetIdNum()] = m_Display;
        }

        // System에서 Read한 Cam의 개수를 리턴한다.
        public int GetCamNum()
        {
            return m_iCheckCamNo;
        }
        public void freeSystems()
        {
            // GigE Cam System 해제
            m_iResult = m_System.release();
            
            MIL.MsysFree(m_MilSystem);
            MIL.MappFree(m_MilApp);
        }

        public bool RegisterMarkModel(int iCam, ref CSearchData pSData)
        {
            // 0 위치를 화면의 중앙으로 설정함.
            pSData.m_rectModel.X = m_pDisplay[iCam].GetImageWidth() / 2;
            pSData.m_rectModel.Y = m_pDisplay[iCam].GetImageHeight() / 2;

            MIL_ID m_MilImage = m_pDisplay[iCam].GetImage();
            MIL_ID m_DisplayGraph = m_pDisplay[iCam].GetViewGraph();

            //Draw할 Rec을 생성한다.
            Rectangle pRec = new Rectangle(pSData.m_rectModel.X - pSData.m_rectModel.Width / 2,
                                           pSData.m_rectModel.Y - pSData.m_rectModel.Height / 2,
                                           pSData.m_rectModel.Width, pSData.m_rectModel.Height);

            // Allocate a normalized grayscale model.
            MIL.MpatAllocModel(m_MilSystem, m_MilImage, pRec.X, pRec.Y,
                               pRec.Width, pRec.Height, MIL.M_NORMALIZED, ref pSData.m_milModel);

            if (pSData.m_milModel == MIL.M_NULL) return false;

            MIL.MpatAllocResult(m_MilSystem, MIL.M_DEFAULT, ref m_SearchResult);

            // Set the search accuracy to high.
            MIL.MpatSetAccuracy(pSData.m_milModel, MIL.M_HIGH);

            MIL.MpatSetAcceptance(pSData.m_milModel, pSData.m_dAcceptanceThreshold);  // Acceptance Threshold Setting
            MIL.MpatSetCertainty(pSData.m_milModel, pSData.m_dAcceptanceThreshold);   // Set Certainty Threshold
            MIL.MpatSetCenter(pSData.m_milModel,                                      // Pattern Mark에서 Offset 설정함.
                              (double)pSData.m_pointReference.X,
                              (double)pSData.m_pointReference.Y);

            // Set the search model speed to high.
            MIL.MpatSetSpeed(pSData.m_milModel, MIL.M_HIGH);

            //================================================================================================
            //// Angle 설정 
            //MIL.MpatSetAngle(pSData.m_milModel, MIL.M_SEARCH_ANGLE_MODE, MIL.M_ENABLE);
            //MIL.MpatSetAngle(pSData.m_milModel, MIL.M_SEARCH_ANGLE_DELTA_NEG, 3.0);
            //MIL.MpatSetAngle(pSData.m_milModel, MIL.M_SEARCH_ANGLE_DELTA_POS, 3.0);
            //MIL.MpatSetAngle(pSData.m_milModel, MIL.M_SEARCH_ANGLE_ACCURACY, 0.25);
            //================================================================================================

            // Preprocess the model.
            MIL.MpatPreprocModel(m_MilImage, pSData.m_milModel, MIL.M_DEFAULT);

            // Draw a box around the model in the model image.
            
            MIL.MpatDraw(MIL.M_DEFAULT, pSData.m_milModel, m_DisplayGraph,
                         MIL.M_DRAW_BOX , MIL.M_DEFAULT, MIL.M_ORIGINAL);

            //MIL.MbufFree(m_MilImage);
            //MIL.MbufFree(m_DisplayGraph); 

            return true;

        }

        public int SetEdgeFindParameter(Point mPos, Size mSize,double dAng)
        {
            MIL_INT MARKER_TYPE = MIL.M_EDGE;
            double FIRST_EDGE_POLARITY =    (double)MIL.M_POSITIVE;
            double SECOND_EDGE_POLARITY =   (double)MIL.M_DEFAULT;
            double BOX_CENTER_POS_X =       (double) mPos.X;
            double BOX_CENTER_POS_Y =       (double) mPos.Y;
            double BOX_SIZE_X =             (double)mSize.Width;
            double BOX_SIZE_Y =             (double)mSize.Height;
            double BOX_ANGLE =              dAng;
            double SUB_REGIONS_NUMBER =     (double)MIL.M_DEFAULT;
            double NB_MARKERS =             (double)MIL.M_ALL;
            double EDGEVALUE_MIN =          (double)MIL.M_DEFAULT;

            // Edge Find 
            m_EdgeMaker = MIL.MmeasAllocMarker(m_MilSystem, MARKER_TYPE,  MIL.M_DEFAULT, MIL.M_NULL);

            MIL.MmeasSetMarker(m_EdgeMaker, MIL.M_POLARITY,              FIRST_EDGE_POLARITY, SECOND_EDGE_POLARITY);
            MIL.MmeasSetMarker(m_EdgeMaker, MIL.M_BOX_CENTER,            BOX_CENTER_POS_X, BOX_CENTER_POS_Y);
            MIL.MmeasSetMarker(m_EdgeMaker, MIL.M_BOX_SIZE,              BOX_SIZE_X, BOX_SIZE_Y);
            MIL.MmeasSetMarker(m_EdgeMaker, MIL.M_BOX_ANGLE,             BOX_ANGLE,MIL.M_NULL);
            MIL.MmeasSetMarker(m_EdgeMaker, MIL.M_SUB_REGIONS_NUMBER,    SUB_REGIONS_NUMBER, MIL.M_NULL);
            MIL.MmeasSetMarker(m_EdgeMaker, MIL.M_NUMBER,                NB_MARKERS, MIL.M_NULL);
            MIL.MmeasSetMarker(m_EdgeMaker, MIL.M_EDGEVALUE_MIN,         EDGEVALUE_MIN, MIL.M_NULL);

            return SUCCESS;
        }

        public int FindEdge(int iCam)
        {
            if (m_EdgeMaker == MIL.M_NULL) return ERR_VISION_ERROR;

            MIL_ID m_MilImage = m_pDisplay[iCam].GetImage();
            MIL_ID m_DisplayGraph = m_pDisplay[iCam].GetViewGraph();

            // Find the marker and compute all applicable measurements.
            MIL.MmeasFindMarker(MIL.M_DEFAULT, m_MilImage, m_EdgeMaker, MIL.M_DEFAULT);

           // m_pDisplay[iCam].ClearOverlay();
            MIL.MgraClear(MIL.M_DEFAULT, m_DisplayGraph);
            //MIL.MdispControl(m_MilImage, MIL.M_OVERLAY_CLEAR, MIL.M_DEFAULT);

            MIL.MgraColor(MIL.M_DEFAULT, MIL.M_COLOR_GREEN);
            MIL.MmeasDraw(MIL.M_DEFAULT, m_EdgeMaker, m_DisplayGraph, MIL.M_DRAW_SEARCH_REGION, MIL.M_DEFAULT, MIL.M_RESULT);
            //MIL.MgraColor(MIL.M_DEFAULT, MIL.M_COLOR_BLUE);
            //MIL.MmeasDraw(MIL.M_DEFAULT, m_EdgeMaker, m_DisplayGraph, MIL.M_DRAW_SEARCH_DIRECTION, MIL.M_DEFAULT, MIL.M_RESULT);
            

            // Edge 개수 확인
            MIL_INT FindEdgeNum = 0;
            MIL.MmeasGetResult(m_EdgeMaker, MIL.M_NUMBER + MIL.M_TYPE_MIL_INT, ref FindEdgeNum, MIL.M_NULL);
            if (FindEdgeNum >= 1)
            {
                //MIL.MgraColor(MIL.M_DEFAULT, MIL.M_COLOR_MAGENTA);
                //MIL.MmeasDraw(MIL.M_DEFAULT, m_EdgeMaker, m_DisplayGraph, MIL.M_DRAW_EDGES, MIL.M_DEFAULT, MIL.M_RESULT);
                MIL.MgraColor(MIL.M_DEFAULT, MIL.M_COLOR_RED);
                MIL.MmeasDraw(MIL.M_DEFAULT, m_EdgeMaker, m_DisplayGraph, MIL.M_DRAW_POSITION, MIL.M_DEFAULT, MIL.M_RESULT);

                double dPosX = 0.0;
                double dPosY = 0.0;
                MIL.MmeasGetResult(m_EdgeMaker, MIL.M_POSITION + MIL.M_EDGE_FIRST, ref dPosX, ref dPosY);

                return SUCCESS;
            }
            else
                return ERR_VISION_ERROR;
        }
        public int SearchByNGC(int iCam, CSearchData pSdata, out CResultData pRData)
        {
            
            MIL_ID m_MilImage = m_pDisplay[iCam].GetImage();
            MIL_ID m_DisplayGraph = m_pDisplay[iCam].GetViewGraph();

            CResultData pResult = new CResultData();
            Point RectOffset = new Point();            

            // Mark Search Timer Reset
            MIL.MappTimer(MIL.M_DEFAULT, MIL.M_TIMER_RESET + MIL.M_SYNCHRONOUS, MIL.M_NULL);
            // Mark Search Command
            MIL.MpatFindModel(m_MilImage, pSdata.m_milModel, m_SearchResult);
            // Mark Search Timer Check
            MIL.MappTimer(MIL.M_DEFAULT, MIL.M_TIMER_READ + MIL.M_SYNCHRONOUS, ref pResult.m_dTime);

            if (MIL.MpatGetNumber(m_SearchResult) == 1L)
            {
                // Display Mark Area
                MIL.MgraClear(MIL.M_DEFAULT, m_DisplayGraph);
                MIL.MpatDraw(MIL.M_DEFAULT, m_SearchResult, m_DisplayGraph, MIL.M_DRAW_BOX, MIL.M_DEFAULT, MIL.M_DEFAULT);

                //DisplaySearchResult();

                MIL.MpatGetResult(m_SearchResult, MIL.M_POSITION_X, ref pResult.m_dPixelX);
                MIL.MpatGetResult(m_SearchResult, MIL.M_POSITION_Y, ref pResult.m_dPixelY);
                MIL.MpatGetResult(m_SearchResult, MIL.M_SCORE, ref pResult.m_dScore);

                RectOffset.X = (int)pResult.m_dPixelX - pSdata.m_pointReference.X - pSdata.m_rectSearch.X;
                RectOffset.Y = (int)pResult.m_dPixelY - pSdata.m_pointReference.Y - pSdata.m_rectSearch.Y;

                pResult.m_rectFindedModel = pSdata.m_rectModel;
                pResult.m_rectFindedModel.Offset(RectOffset);
                pResult.m_rectSearch = pSdata.m_rectSearch;

                if (pResult.m_dScore > pSdata.m_dAcceptanceThreshold)
                {
                    pResult.m_bSearchSuccess = true;

                    // Result Data 전달
                    pRData = pResult;
                    return SUCCESS;
                }
            }

            // Search Data를 초기화 한다.
            pResult.m_bSearchSuccess = false;
            pResult.m_dPixelX = 0.0;
            pResult.m_dPixelY = 0.0;
            pResult.m_rectSearch = new Rectangle(0, 0, 0, 0);
            pResult.m_rectFindedModel = new Rectangle(0, 0, 0, 0);

            // Result Data 전달
            pRData = pResult;

            return ERR_VISION_SEARCH_FAILURE;

        }

        public void SetViewWindow(Rectangle RectImage, IntPtr pDisplayObject)
        {
            double ZoomX;
            double ZoomY;
            IntPtr m_ImageHandle = IntPtr.Zero;

            // Display하는 Panel의 사이즈를 읽어온다.
            Size DisplaySize = ContainerControl.FromHandle(pDisplayObject).Size;

            // Display Size에 맞게 Zoom를 설정한다.
            ZoomX = (double)DisplaySize.Width / (double)RectImage.Width;
            ZoomY = (double)DisplaySize.Height / (double)RectImage.Height;
            MIL.MdispZoom(m_MilView, ZoomX, ZoomY);

            // View Image 설정
            MIL.MbufAlloc2d(m_MilSystem, RectImage.Width, RectImage.Height,
                                MIL.M_UNSIGNED + 8, MIL.M_IMAGE + MIL.M_PROC + MIL.M_DISP,
                                ref m_MilViewImage);

            MIL.MbufClear(m_MilViewImage, 0);

            // 핸들값을 받아온다.
            m_ImageHandle = pDisplayObject;
            // Display Window를 설정한다.
            MIL.MdispSelectWindow(m_MilView, m_MilViewImage, m_ImageHandle);

        }

        public void ShowViewImage(MIL_ID pImage)
        {
            MIL.MbufCopy(pImage, m_MilViewImage);
        }
        public void DisplayViewImage(MIL_ID pImage, IntPtr pDisplayObject)
        {
            double ZoomX;
            double ZoomY;
            IntPtr m_ImageHandle = IntPtr.Zero;

            return;

            MIL_INT ImageWidth   = MIL.MbufInquire(pImage, MIL.M_SIZE_X, MIL.M_NULL);
            MIL_INT ImageHeight  = MIL.MbufInquire(pImage, MIL.M_SIZE_Y, MIL.M_NULL);
            
            // Display하는 Panel의 사이즈를 읽어온다.
            Size DisplaySize = ContainerControl.FromHandle(pDisplayObject).Size;

            // Display Size에 맞게 Zoom를 설정한다.
            ZoomX = (double)DisplaySize.Width / (double)ImageWidth;
            ZoomY = (double)DisplaySize.Height / (double)ImageHeight;
            MIL.MdispZoom(m_MilView, ZoomX, ZoomY);

            // View Image 설정
            MIL.MbufAlloc2d(m_MilSystem, ImageWidth,ImageHeight,
                                MIL.M_UNSIGNED + 8, MIL.M_IMAGE + MIL.M_PROC + MIL.M_DISP,
                                ref m_MilViewImage);

            MIL.MbufClear(m_MilViewImage, 0);

            // 핸들값을 받아온다.
            m_ImageHandle = pDisplayObject;
            // Display Window를 설정한다.
            MIL.MdispSelectWindow(m_MilView, m_MilViewImage, m_ImageHandle);

            MIL.MbufCopy(pImage, m_MilViewImage);

        }
        void Grab()
        {

        }

    }
}
