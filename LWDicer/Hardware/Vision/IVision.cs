using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using static LWDicer.Control.DEF_Vision;
using Matrox.MatroxImagingLibrary;

namespace LWDicer.Control
{      
    interface IVision
    {
        //━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        //	Model Change 시, 기존 Model 에 대한 Vision Model Data 를 제거하고,
	    //  새 Model 에 대한 Model Data 를 Load 한다.
	     
        int ChangeModel(string strModelFilePath);

	    //  파일에서 데이타를 로드 한다.	     
	     int LoadParameter(string strModelFilePath);

	    // 생성된 Local View 를 제거한다.	     
	     int DestroyLocalView(int iCamNo);

	    // Local View 를 생성한다.	     
	     int InitialLocalView(int iCamNo, IntPtr pObject);
        

        // Grab Operation 을 수행한다.	     
        void Grab(int iCamNo);

        // Grab Buffer 로부터 해당 Camera 의 영상을 가져와 연결된 화면에 Display 한다.
        // 
        MIL_ID GetGrabImage(int iViewNo);

        // Camera 와 View Window 를 연결한다.
        //
        void ConnectCam(int iCamNo);

        // Write Vision Model Data


	     int SelectCamera(int iCamNo,int iViewNo);

        // Check Enabled Model & Recognition Area

        int CheckModel(int iCamNo, int iModelNo);

	    // Delete Registered Model or Recognition Area
	    void DeleteMark(int iCamNo, int iModelNo);

	    // Save Current Camera Image in file
	    // @param iCamNo : Camera Number
	    // @param iModelNo : Model Number
	     
	    int SaveImage(int iCamNo, int iModelNo, double dScore);

        int SaveModelImage(int iCamNo, string strPath, string strName);

        // Delete Old Error Image Files

        int DeleteOldImageFiles();

	    // Enable or Disable "Save Error Image" Fuction.
	     void EnableSaveErrorImage(bool bFlag = false);

	    // Get Grab Settling Time.	     
	     int GetGrabSettlingTime(int iCamNo);

	    // Set Grab Settling Time.	   
	     void SetGrabSettlingTime(int iCamNo, int iGrabSettlingTime);

	    // Get Camera Change Time.	     
	     int GetCameraChangeTime(int iCamNo);

	    // Set Camera Change Time.	     
	     void SetCameraChangeTime(int iCamNo, int iCameraChangeTime);

        int ReLoadPatternMark(int iCamNo, int iTypeNo, CSearchData pSData);
        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        //      Related Pattern Matching Operations

        //Register NGC & GMF Model
        //@return boolean type : TRUE or FALSE
        //@param iCameraNo : Camera Number
        //@param SelectMark : Model Mark Number
        //@param SearchArea : Search Area Rectangle
        //@param ModelArea : Model Area Rectangle
        //@param ReferencePoint : Reference Point

        int RegisterPatternMark(int iCameraNo,
                                 string strModel,
                                 int iTypeNo,                                        
                                 ref Rectangle SearchArea,
                                 ref Rectangle ModelArea,
                                 ref Point ReferencePoint);

	      //Reset Search Area
	      //@param iCamNo : Camera Number
	      //@param iModelNo : Model Mark Number
	      //@param SArea : Search Area Rectangle
	     
	     int SetSearchArea(int iCamNo, int iModelNo, ref Rectangle SArea);

	      //Recognition Model Mark (NGC Algorithm)
	      //@return int type Error Code : 0 - SUCCESS, etc. - Error
	      //@param iCamNo : Camera Number
	      //@param iModelNo : Model Mark Number
	      //@param bUseGMF : True  - Use GMF(Geometry Model Finder)
	      //                 False - Not Use GMF(Geometry Model Finder) 
	     
	     int RecognitionPatternMark(int iCameraNo, int iModelNo, out CResultData pPatResult, bool bUseGMF = false);

        
        /// <summary>
        /// Camera의 등록된 Search Data Read
        /// </summary>
        /// <param name="iCamNo"></param>
        /// <param name="iModelNo"></param>
        /// <returns></returns>
        CSearchData GetSearchData(int iCamNo, int iModelNo);


        /// <summary>
        /// Camera에 동록된 Pattern Image Read
        /// </summary>
        /// <param name="iCamNo"></param>
        /// <param name="iModelNo"></param>
        /// <returns></returns>
        MIL_ID GetPatternImage(int iCamNo, int iModelNo);

        //Make a Mask Image & Apply the Mask Image to GMF Search Context
        //@return int type Error Code : 0 - SUCCESS, etc. - Error
        //@param iCamNo : Camera Number
        //@param iModelNo : Model Mark Number
        //@param MaskRect : Rectangle for masking
        //@param ModelRect : GMF Model Rectangle
        //@param bMakeEndFlag : TRUE - Stop making mask image & Apply the Mask Image to GMF Search Context
        //                      FALSE - Continue making mask image
        int MaskImage(int iCamNo,int iModelNo,ref Rectangle MaskRect,ref Rectangle ModelRect, bool bMakeEndFlag);

	      //Return Pattern Matching Result : Reference Point X value
	      //@return double type : X coordinate
	      //@param iCamNo : Camera Number
	      //@param iModelNo : Model Mark Number
	     
	     double GetSearchResultX(int iCamNo, int iModelNo);

	      //Return Pattern Matching Result : Reference Point Y value
	      //@return double type : Y coordinate
	      //@param iCamNo : Camera Number
	      //@param iModelNo : Model Mark Number
	     
	     double GetSearchResultY(int iCamNo, int iModelNo);

          //Return Pattern Matching Result Model Rectangle
          //@return CRect type : Finded Model
          //@param iCamNo : Camera Number
          //@param iModelNo : Model Mark Number
         
        Rectangle GetFindedModelRect(int iCamNo, int iModelNo);

          //Return Pattern Matching Search Area Rectangle
          //@return CRect type : Search Area
          //@param iCamNo : Camera Number
          //@param iModelNo : Model Mark Number
         
        Rectangle GetSearchAreaRect(int iCamNo, int iModelNo);

	     double GetSearchAcceptanceThreshold(int iCamNo, int iModelNo);
	     int SetSearchAcceptanceThreshold(int iCamNo, int iModelNo, double dValue);
	     double GetSearchScore(int iCamNo, int iModelNo);

        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
          //Related Edge Finder (Caliper Tool) Operations
         
	     int FindEdge(int iCameraNo, ref CEdgeData pEdgeData);

	     int SetEdgeFinderPolarity(int iCameraNo, int iModelNo, double dPolarity);
	     int SetEdgeFinderThreshold(int iCameraNo, int iModelNo, double dThreshold);
	     int SetEdgeFinderNumOfResults(int iCameraNo, int iModelNo, double dNumOfResults);
	     int SetEdgeFinderDirection(int iCameraNo, int iModelNo, double dSearchDirection);

	     double GetEdgeFinderPolarity(int iCameraNo, int iModelNo);
	     double GetEdgeFinderThreshold(int iCameraNo, int iModelNo);
	     double GetEdgeFinderNumOfResults(int iCameraNo, int iModelNo);
	     double GetEdgeFinderDirection(int iCameraNo, int iModelNo);

        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        //      Related Vision View Display Operations
       

         //Dispaly the registered Model Image or Model Area like below ;
         //   		NGC Pattern Matching Model Image
         //   		GMF Model Edge information
         //   		Blob Area
         //   		OCR Reading Area
         //    @return Error Code : 0[SUCCESS] - Success, etc. - Error
            
         int DisplayPatternImage(int iCamNo, int iModelNo, IntPtr pHandle ) ;

	     //Stop Live & Grap Image
	     //    @return 
	     //    @param iCamNo : Camera Number
	        
	     void HaltVideo(int iCamNo) ;

         //Start Live : Grab Continuous
         //    @return int type Error Code : 0 - SUCCESS
         //    @param iCamNo : Camera Number

         void LiveVideo(int iCamNo) ;

	     //Clear Overlay Display
	     //    @return
	     //    @param iCamNo :
	     //    @param bCenterLineFlag : TRUE - Draw Center Cross Mark when clear overlay display
	     //                             FALSE - Not Draw Center Cross Mark 
	        
	     void ClearOverlay(int iCamNo) ;

	    // Draw Rectangle on the Overlay Display
	        
	        
	     void DrawOverlayAreaRect(int iCamNo, Rectangle rect) ;

	    // Draw Grid On the Overlay Display      
	     void DrawOverlayGrid(int iCamNo) ;

	    // Draw Line On the Overlay Display
	     void DrawOverlayLine(int iCamNo, Point ptStart, Point ptEnd, int color) ;

       // Draw Cross Mark on the Overlay Display
        void DrawOverlayCrossMark(int iCamNo, int iWidth, int iHeight, Point center);   //, int color = 1) ;

        // Draw Text on the View Image Display	      

        void DrawOverlayText(int iCamNo, string strText, Point pointText) ;


        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        //      Temporary Functions for Operating Test
                    
       // public void SetPortingFactor(int iCameraNo, SPortingFactorData sPortingData);
       
        //MToolSet ivtool;
    }
}
