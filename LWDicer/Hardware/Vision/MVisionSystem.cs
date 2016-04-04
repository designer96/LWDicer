using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

using static LWDicer.Control.DEF_Vision;
using static LWDicer.Control.DEF_Error;
using BGAPI;
using Matrox.MatroxImagingLibrary;


namespace LWDicer.Control
{
    public class MVisionSystem
    {        
        private int m_iSystemNo;
        private int m_iSystemIndex;
        private int m_iCheckCamNo;
        private int m_iResult;
        
        //=========================================================
        // System
        private MIL_ID m_MilApp;            // Application identifier.
        private MIL_ID m_MilSystem;          // System identifier.
        private MVisionCamera[] m_pCamera;
        private MVisionDisplay[] m_pDisplay;

        private BGAPI.System m_System;
        private BGAPI_FeatureState m_State;

        //=========================================================
        // Image 
        private MIL_ID m_MilView = MIL.M_NULL;

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
            m_pCamera = new MVisionCamera[DEF_MAX_CAMERA_NO];
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

        public void SelectCamera(MVisionCamera m_Camera)
        {
            m_pCamera[m_Camera.GetCamID()] = m_Camera;
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


        public int ReloadModel(int iCamNo, ref CVisionPatternData pSData)
        {
            if (pSData.m_bIsModel == false) return ERR_VISION_ERROR;

            MIL_ID m_MilImage = MIL.M_NULL;
            // Image Load...
            string strLoadFileName = pSData.m_strFilePath + pSData.m_strFileName;  
            MIL.MbufRestore(strLoadFileName, m_MilSystem, ref m_MilImage);

            //Draw할 Rec을 생성한다.
            Rectangle pRec = new Rectangle(pSData.m_rectModel.X - pSData.m_rectModel.Width / 2,
                                           pSData.m_rectModel.Y - pSData.m_rectModel.Height / 2,
                                           pSData.m_rectModel.Width, pSData.m_rectModel.Height);

            // Allocate a normalized grayscale model.
            MIL.MpatAllocModel(m_MilSystem, m_MilImage, pRec.X, pRec.Y,
                               pRec.Width, pRec.Height, MIL.M_NORMALIZED, ref pSData.m_milModel);

            // Model Image Save (Image View Save용)
            MIL.MbufAlloc2d(m_MilSystem, pRec.Width, pRec.Height, MIL.M_UNSIGNED + 8, MIL.M_IMAGE + MIL.M_PROC + MIL.M_DISP,
                                ref pSData.m_ModelImage);
            MIL.MbufCopyColor2d(m_MilImage, pSData.m_ModelImage, MIL.M_ALL_BANDS, pRec.X, pRec.Y,
                               MIL.M_ALL_BANDS, 0, 0, pRec.Width, pRec.Height);

            if (pSData.m_milModel == MIL.M_NULL) return ERR_VISION_ERROR;

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

            // Preprocess the model.
            MIL.MpatPreprocModel(m_MilImage, pSData.m_milModel, MIL.M_DEFAULT);            

            return SUCCESS;

        }

        public bool RegisterMarkModel(int iCamNo, ref CVisionPatternData pSData)
        {
            // 0 위치를 화면의 중앙으로 설정함.
            pSData.m_rectModel.X = m_pDisplay[iCamNo].GetImageWidth() / 2;
            pSData.m_rectModel.Y = m_pDisplay[iCamNo].GetImageHeight() / 2;

            MIL_ID m_MilImage = m_pDisplay[iCamNo].GetImage();
            MIL_ID m_DisplayGraph = m_pDisplay[iCamNo].GetViewGraph();

            //Draw할 Rec을 생성한다.
            Rectangle pRec = new Rectangle(pSData.m_rectModel.X - pSData.m_rectModel.Width / 2,
                                           pSData.m_rectModel.Y - pSData.m_rectModel.Height / 2,
                                           pSData.m_rectModel.Width, pSData.m_rectModel.Height);

            // Allocate a normalized grayscale model.
            MIL.MpatAllocModel(m_MilSystem, m_MilImage, pRec.X, pRec.Y,
                               pRec.Width, pRec.Height, MIL.M_NORMALIZED, ref pSData.m_milModel);

            // Model Image Save (Image View Save용)
            MIL.MbufAlloc2d(m_MilSystem, pRec.Width, pRec.Height, MIL.M_UNSIGNED + 8, MIL.M_IMAGE + MIL.M_PROC + MIL.M_DISP,
                                ref pSData.m_ModelImage);
            MIL.MbufCopyColor2d(m_MilImage, pSData.m_ModelImage, MIL.M_ALL_BANDS, pRec.X, pRec.Y,
                               MIL.M_ALL_BANDS,0,0, pRec.Width, pRec.Height);            

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

            // Save Image Bitmap

            return true;

        }

        /// <summary>
        /// SetEdgeFindParameter
        /// Edge검출 영역 설정
        /// </summary>
        /// <param name="mPos"></param>
        /// <param name="dWidth"></param>
        /// <param name="dHeight"></param>
        /// <param name="dAng"></param>
        /// <returns></returns>
        public int SetEdgeFindParameter(Point mPos, double dWidth,double dHeight,double dAng)
        {
            MIL_INT MARKER_TYPE = MIL.M_EDGE;
            double FIRST_EDGE_POLARITY =    (double)MIL.M_POSITIVE;
            double SECOND_EDGE_POLARITY =   (double)MIL.M_DEFAULT;
            double BOX_CENTER_POS_X =       (double) mPos.X;
            double BOX_CENTER_POS_Y =       (double) mPos.Y;
            double BOX_SIZE_X =             dWidth;
            double BOX_SIZE_Y =             dHeight;
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

        /// <summary>
        /// Edge를 찾음
        /// dPosX,dPosY에 검출된 값을 보내줌
        /// </summary>
        /// <param name="iCam"></param>
        /// <param name="dPosX"></param>
        /// <param name="dPosY"></param>
        /// <returns></returns>
        public int FindEdge(int iCam, ref CEdgeData pEdgeData)
        {
            // Edge 검출 설정이 되어 있는지를 확인함.
            if (m_EdgeMaker == MIL.M_NULL) return ERR_VISION_ERROR;

            // 검출할 영상 Image를 가져온다.
            MIL_ID m_MilImage = m_pDisplay[iCam].GetImage();
            // 결과를 Display할 Overlay를 가져온다.
            MIL_ID m_DisplayGraph = m_pDisplay[iCam].GetViewGraph();

            // Find the marker and compute all applicable measurements.
            // Edge검출 명령 실행
            MIL.MmeasFindMarker(MIL.M_DEFAULT, m_MilImage, m_EdgeMaker, MIL.M_DEFAULT);

            // Overlay Clear
            MIL.MgraClear(MIL.M_DEFAULT, m_DisplayGraph);

            // Edge 검출 영역을  Overlay에 표시함
            MIL.MgraColor(MIL.M_DEFAULT, MIL.M_COLOR_GREEN);
            MIL.MmeasDraw(MIL.M_DEFAULT, m_EdgeMaker, m_DisplayGraph, MIL.M_DRAW_SEARCH_REGION, MIL.M_DEFAULT, MIL.M_RESULT);
            
            // Edge 개수 확인
            MIL_INT FindEdgeNum = 0;
            MIL.MmeasGetResult(m_EdgeMaker, MIL.M_NUMBER + MIL.M_TYPE_MIL_INT, ref FindEdgeNum, MIL.M_NULL);
            // Edge 검출 되었을 경우
            if (FindEdgeNum >= 1)
            {                
                pEdgeData.m_bSuccess = true;
                pEdgeData.m_iEdgeNum = FindEdgeNum;
                pEdgeData.m_dPosX = new double[FindEdgeNum];
                pEdgeData.m_dPosY = new double[FindEdgeNum];

                // 검출된 Edge를 Overlay에 표시함.
                MIL.MgraColor(MIL.M_DEFAULT, MIL.M_COLOR_RED);
                MIL.MmeasDraw(MIL.M_DEFAULT, m_EdgeMaker, m_DisplayGraph, MIL.M_DRAW_POSITION, MIL.M_DEFAULT, MIL.M_RESULT);
                MIL.MmeasGetResult(m_EdgeMaker, MIL.M_POSITION + MIL.M_EDGE_FIRST, ref pEdgeData.m_dPosX[0], ref pEdgeData.m_dPosY[0]);

                return SUCCESS;
            }
            else
            {
                pEdgeData.m_bSuccess = false;
                //pEdgeData.m_dPosX = 0.0;
                //pEdgeData.m_dPosY = 0.0;
                pEdgeData.m_iEdgeNum = 0;
                return ERR_VISION_ERROR;
            }
                
        }
        /// <summary>
        /// Pattern Maching으로 Mark의 위치를 검색함
        /// </summary>
        /// <param name="iCam"></param>
        /// <param name="pSdata"></param>
        /// <param name="pRData"></param>
        /// <returns></returns>
        public int SearchByNGC(int iCamNo, CVisionPatternData pSdata, out CResultData pRData)
        {
            
            MIL_ID m_MilImage = m_pDisplay[iCamNo].GetImage();
            MIL_ID m_DisplayGraph = m_pDisplay[iCamNo].GetViewGraph();

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
                //MIL.MgraClear(MIL.M_DEFAULT, m_DisplayGraph);
                m_pDisplay[iCamNo].ClearOverlay();
                MIL.MgraColor(MIL.M_DEFAULT, MIL.M_COLOR_GREEN);
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

    }
}
