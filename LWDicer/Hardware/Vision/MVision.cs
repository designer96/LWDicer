using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using LWDicer.Control;
using static LWDicer.Control.DEF_Vision;
using static LWDicer.Control.DEF_Error;
using static LWDicer.Control.DEF_Common;
using Matrox.MatroxImagingLibrary;

namespace LWDicer.Control
{

    public class MVision : MObject,IVision,IDisposable
    {
        private bool m_bSystemInit;
        private bool m_bErrorPrint;
        private bool m_bSaveErrorImage;
        public int m_iCurrentViewNum { get; set; }
        public int m_iHairLineWidth { get; set; }
        public int m_iMarkROIWidth { get; set; }
        public int m_iMarkROIHeight { get; set; }

        private MVisionSystem m_pSystem;
        private MVisionCamera[] m_pCamera;
        private MVisionDisplay[] m_pView;

        private CVisionData m_Data;

        public MVision(CObjectInfo objInfo, CVisionData data)
            : base(objInfo)
        {

            SetData(data);

            m_bSystemInit = false;
            m_bErrorPrint       = false;
            m_bSaveErrorImage   = false;

            m_iHairLineWidth = DEF_HAIRLINE_NOR;
            m_iMarkROIWidth = DEF_MARK_WIDTH_NOR;
            m_iMarkROIHeight = DEF_MARK_HEIGHT_NOR;            

            Initialize(DEF_MAX_CAMERA_NO);
        }

        public void Dispose()
        {
            CloseVisionSystem();
        }

        ~MVision()
        {
            Dispose();
        }
        public int SetData(CVisionData source)
        {
            m_Data = ObjectExtensions.Copy(source);
            return SUCCESS;
        }
        public int Initialize(int iCameraNum)
        {

#if SIMULATION_VISION
                retrun SUCCESS;
#endif
            int iResult = 0;

            if (m_bSystemInit)
            {
                Debug.Write("\n====================================\n");
                Debug.Write("System 이미 초기화 되었습니다.");
                return ERR_VISION_ERROR;
            }

            Debug.Write("\n====================================\n");
            Debug.Write("System 초기화\n");
            m_pSystem = new MVisionSystem();

            // GigE Cam초기화 & MIL 초기화
            iResult=  m_pSystem.Initialize();

            if (iResult != SUCCESS) return ERR_VISION_ERROR;
            // MIL 의 Err Print하는 기능 Enable
            if (!m_bErrorPrint)
            {
                MIL.MappControl(MIL.M_ERROR, MIL.M_PRINT_DISABLE);
                MIL.MappControl(MIL.M_TRACE, MIL.M_PRINT_DISABLE);
                MIL.MappControl(MIL.M_PARAMETER, MIL.M_CHECK_DISABLE);
            }

            Debug.Write("\n====================================\n");
            Debug.Write("Camera 개수 확인\n");
            int iGetCamNum = m_pSystem.GetCamNum();

            if(iGetCamNum < iCameraNum)
            {
                Debug.Write("\n====================================\n");
                Debug.Write("연결된 카메라 수량이 맞지 않습니다.\n");

                // System을 해제한다.
                m_pSystem.freeSystems();

                return ERR_VISION_ERROR;
            }

            m_pCamera = new MVisionCamera[iCameraNum];
            m_pView = new MVisionDisplay[iCameraNum];
            Debug.Write("\n====================================\n");
            Debug.Write("Camera & View초기화\n");

            // Camera & View 를 생성함.
            for (int iIndex = 0; iIndex < iCameraNum; iIndex++) 
            {
                // Camera를 생성함.
                m_pCamera[iIndex] = new MVisionCamera();
                // Vision Library MIL
                m_pCamera[iIndex].SetMil_ID(m_pSystem.GetMilSystem());
                // Camera 초기화
                m_pCamera[iIndex].Initialize(iIndex, m_pSystem.GetSystem());                

                // Display View 생성함.
                m_pView[iIndex] = new MVisionDisplay();
                // Vision Library MIL
                m_pView[iIndex].SetMil_ID(m_pSystem.GetMilSystem());
                // Display 초기화
                m_pView[iIndex].Initialize(iIndex, m_pCamera[iIndex]);                

                // Display를 Camera와 System 연결
                ConnectCam(iIndex);
            }      
            
            Debug.Write("\n====================================\n");
            Debug.Write("System & Camera & View 생성 완료\n");

            m_bSystemInit = true;

            return SUCCESS;
        }

        public void CloseVisionSystem()
        {
#if SIMULATION_VISION
                retrun;
#endif
            // System의 상태 확인
            if (m_bSystemInit == false) return;

            // Camera Distroy
            for (int iIndex = 0; iIndex < DEF_MAX_CAMERA_NO; iIndex++)
            {
                HaltVideo(iIndex);
                m_pView[iIndex].FreeDisplay();
                m_pCamera[iIndex].FreeCamera();
            }
            
            // System Distroy
            m_pSystem.freeSystems();

            m_bSystemInit = false;
        }
        //━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━//
        //	Model Change 시, 기존 Model 에 대한 Vision Model Data 를 제거하고,
        //	새 Model 에 대한 Model Data 를 Load 한다.
        //
        public int ChangeModel(string strModelFilePath)
        {
            return LoadParameter(strModelFilePath);
        }

        //  파일에서 데이타를 로드 한다.	     
        public int LoadParameter(string strModelFilePath)
        {
        #if SIMULATION_VISION
                retrun SUCCESS;
        #endif
            int iResult;
            int i = 0;

            if (m_pSystem == null)
                // 104003 = Vision System Allocation 에 실패했습니다.
                return ERR_VISION_ERROR;

            if (m_pCamera == null)
                // 104006 = DCF File 을 찾을 수 없습니다.
                return ERR_VISION_ERROR;

            for (i = 0; i < DEF_MAX_CAMERA_NO; i++)
            {
                if (isValidCameraNo(i))
                {
                    iResult = m_pCamera[i].LoadCameraData(strModelFilePath);
                    if (iResult > 0) return iResult;
                }
                else
                {
                    continue;
                }

                // Model Mark Read
                m_pCamera[i].LoadSearchData(strModelFilePath);
            }

            return SUCCESS;
        }

        bool isValidCameraNo(int iCamNo)
        {
        #if SIMULATION_VISION
            retrun SUCCESS;
        #endif

            int i = 0;
            for (i = 0; i < DEF_MAX_CAMERA_NO; i++)
            {
                if (m_pCamera[i].GetCamID() == iCamNo)
                    return true;
            }
            
            return true;
        }
        bool isValidPatternMarkNo(int iModelNo)
        {
            if (iModelNo < 0 || iModelNo >= DEF_USE_SEARCH_MARK_NO)
                return false;

            return true;
        }

        // 생성된 Local View 를 제거한다.	     
        public int DestroyLocalView(int iCamNo)
        {
        #if SIMULATION_VISION
            retrun SUCCESS;
        #endif

            if (m_pView[iCamNo].IsLocalView())
            {
                m_pView[iCamNo].DestroyLocalView();
            }

            return SUCCESS;
        }

        // Local View 를 생성한다.	     
        public int InitialLocalView(int iCamNo, IntPtr pObject)
        {
#if SIMULATION_VISION
                retrun SUCCESS;
#endif
            if (m_bSystemInit == false) return ERR_VISION_ERROR;

            if (iCamNo > DEF_MAX_CAMERA_NO) return ERR_VISION_ERROR;

            // 설정할 객체의 Handle값이 다른 View에 있으면 빠져 나감.
            for (int iIndex=0;iIndex < DEF_MAX_CAMERA_NO; iIndex++)
            {
                // 바꿀 View와 현재 View를 비교함.
                if (m_pView[iIndex].GetViewHandle()== pObject)
                {
                    return ERR_VISION_ERROR;
                }
            }
            // View 를 Display로 등록한다.
            // View에 맞쳐 Zoom 설정을 한다
            // Mil의 SelectDisplayWindow 함수로 등록한다. 
            m_pView[iCamNo].SetDisplayWindow(pObject);
            m_iCurrentViewNum = iCamNo;

            return SUCCESS;
        }

        // 특정 Image를 표기할때 사용한다.
        public int InitialImageView(Rectangle RectSize, IntPtr pObject)
        {
#if SIMULATION_VISION
                retrun SUCCESS;
#endif
            
            // View 를 Display로 등록한다.
            // View에 맞쳐 Zoom 설정을 한다
            // Mil의 SelectDisplayWindow 함수로 등록한다. 
            m_pSystem.SetViewWindow(RectSize,pObject);

            return SUCCESS;
        }
        public void DisplayViewImage(MIL_ID image,IntPtr handle)
        {
#if SIMULATION_VISION
                retrun;
#endif
            m_pSystem.DisplayViewImage(image,handle);
        }

        // Grab Operation 을 수행한다.
        public void Grab(int iCamNo)
        {
#if SIMULATION_VISION
                retrun;
#endif
            m_pCamera[iCamNo].SetTrigger();
        }

        // Grab Buffer 로부터 해당 Camera 의 영상을 가져옴.	     
        public void GetGrabImage(int iViewNo)
        {
#if SIMULATION_VISION
                retrun;
#endif
            m_pView[iViewNo].GetImage();
        }
        public void DisplayMarkImage(int iViewNo, IntPtr pDisplayHandle )
        {
        #if SIMULATION_VISION
                retrun;
        #endif
            m_pView[iViewNo].GetMarkModelImage();
        }

        // Camera 와 View Window 를 연결한다.
        public void ConnectCam(int iCamNo)
        {
#if SIMULATION_VISION
            retrun;
#endif

            if (iCamNo > DEF_MAX_CAMERA_NO) return;

            // Callback에 Display View함수를 등록한다
            m_pCamera[iCamNo].SelectView(m_pView[iCamNo]);
            // System 에 Display를 연결한다.
            m_pSystem.SelectView(m_pView[iCamNo]);

        }

        // Write Vision Model Data
        public int WriteModelData(int iCamNo, int iModelNo)
        {
#if SIMULATION_VISION
            retrun SUCCESS;
#endif
            if (isValidPatternMarkNo(iModelNo))
            {
                if (m_pCamera[iCamNo].GetSearchData(iModelNo).m_bIsModel)
                    return m_pCamera[iCamNo].WriteSearchData(iModelNo);
                else
                    return ERR_VISION_ERROR;
            }
            return SUCCESS;
        }

        //
        public int SelectCamera(int iCamNo, int iViewNo)
        {
#if SIMULATION_VISION
            retrun SUCCESS;
#endif
            m_pView[iViewNo].SelectCamera(m_pCamera[iCamNo]);
            return SUCCESS;
        }

        //Check Enabled Model & Recognition Area	     
        public int CheckModel(int iCamNo, int iModelNo)
        {
#if SIMULATION_VISION
            retrun SUCCESS;
#endif
            if (isValidPatternMarkNo(iModelNo))
            {
                if (m_pCamera[iCamNo].GetSearchData(iModelNo).m_bIsModel)
                    return SUCCESS;
                else
                    return ERR_VISION_ERROR;
            }

            return SUCCESS;
        }

        // Delete Registered Model or Recognition Area
        public void DeleteMark(int iCamNo, int iModelNo)
        {
#if SIMULATION_VISION
                retrun;
#endif
            if (isValidPatternMarkNo(iModelNo))
            {
                m_pCamera[iCamNo].DeleteSearchModel(iModelNo);
            }
        }

        // Save Current Camera Image in file
	    // @param iCamNo : Camera Number
	    // @param iModelNo : Model Number
        public int SaveImage(int iCamNo, int iModelNo, double dScore)
        {
#if SIMULATION_VISION
                retrun SUCCESS;
#endif
            DateTime time = DateTime.Now;
            string strDirName = DEF_IMAGE_LOG_FILE + String.Format("{0:yyyy_MM_dd}",time); 

            if (!Directory.Exists(strDirName))
            {
                DirectoryInfo DirInfo = Directory.CreateDirectory(strDirName);
                if (!DirInfo.Exists)
                {
                    // 104060 = LogPicture 폴더를 생성할 수 없어 Image 를 저장할 수 없습니다.
                    return ERR_VISION_ERROR;
                }
            }

            string strFileName = String.Format("LogImage {0:HH.mm.ss.fff} [Cam{1:0}_Mark{2:0} Score_{3:0.00}].bmp",
                                               time, iCamNo + 1, iModelNo, dScore);

            m_pView[iCamNo].SaveImage(strDirName+"\\"+strFileName);

            return SUCCESS;

        }

        public int SaveModelImage(int iCamNo, int iModelNo, double dScore)
        {
#if SIMULATION_VISION
                retrun SUCCESS;
#endif
            DateTime time = DateTime.Now;
            string strDirName = DEF_IMAGE_LOG_FILE + String.Format("{0:yyyy_MM_dd}", time);

            if (!Directory.Exists(strDirName))
            {
                DirectoryInfo DirInfo = Directory.CreateDirectory(strDirName);
                if (!DirInfo.Exists)
                {
                    // 104060 = LogPicture 폴더를 생성할 수 없어 Image 를 저장할 수 없습니다.
                    return ERR_VISION_ERROR;
                }
            }

            string strFileName = String.Format("LogImage {0:HH.mm.ss.fff} [Cam{1:0}_Mark{2:0} Score_{3:0.00}].bmp",
                                               time, iCamNo + 1, iModelNo, dScore);

            m_pView[iCamNo].SaveModelImage(strDirName + "\\" + strFileName, iModelNo);

            return SUCCESS;

        }


        // Delete Old Error Image Files

        public int DeleteOldImageFiles()
        {
#if SIMULATION_VISION
                retrun SUCCESS;
#endif
            return SUCCESS;
        }

        // Enable or Disable "Save Error Image" Fuction.
	    
        public void EnableSaveErrorImage(bool bFlag = false)
        {
#if SIMULATION_VISION
                retrun;
#endif
            m_bSaveErrorImage = bFlag;
            //return 0;
        }

        // Get Grab Settling Time.
	    
        public int GetGrabSettlingTime(int iCamNo)
        {
            return SUCCESS;
        }

        // Set Grab Settling Time.
	 
        public void SetGrabSettlingTime(int iCamNo, int iGrabSettlingTime)
        {
            //return 0;
        }

        // Get Camera Change Time.	     
        public int GetCameraChangeTime(int iCamNo)
        {
            return SUCCESS;
        }

        // Set Camera Change Time.
        public void SetCameraChangeTime(int iCamNo, int iCameraChangeTime)
        {
            //return 0;
        }

        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        //     Related Pattern Matching Operations
         

        // Register NGC & GMF Model
        // @return boolean type : TRUE or FALSE
        // @param iCamNo : Camera Number
        // @param SelectMark : Model Mark Number
        // @param SearchArea : Search Area Rectangle
        // @param ModelArea : Model Area Rectangle
        // @param ReferencePoint : Reference Point

        public int RegisterPatternMark(int iCamNo,
                                       int iModelNo,
                                       ref Rectangle SearchArea,
                                       ref Rectangle ModelArea,
                                       ref Point ReferencePoint)
        {
#if SIMULATION_VISION
                retrun SUCCESS;
#endif
            // 모델 갯수 보다 큰 경우 Err
            if (iModelNo > DEF_USE_SEARCH_MARK_NO) return ERR_VISION_ERROR;
            // Search Size 확인 
            if (SearchArea.Width <= DEF_SEARCH_MIN_WIDTH ||
               SearchArea.Height <= DEF_SEARCH_MIN_HEIGHT ||
               SearchArea.Width > m_pCamera[iCamNo].m_CamPixelSize.Width ||
               SearchArea.Height > m_pCamera[iCamNo].m_CamPixelSize.Height)
            {
                return ERR_VISION_ERROR;
            }

            // 기존의 Mark 모델 Data를 연결함 (주소값으로 연결됨).
            CSearchData pSData = m_pCamera[iCamNo].GetSearchData(iModelNo);

            // 등록할 Mark의 Size 및 위치를 설정함.
            pSData.m_rectModel = ModelArea;
            pSData.m_rectSearch = SearchArea;
            pSData.m_pointReference = ReferencePoint;            

            // 기존에 등록된 모델이 있을 경우 삭제한다.
            if(pSData.m_milModel != MIL.M_NULL)
            {
                MIL.MpatFree(pSData.m_milModel);
                pSData.m_milModel = MIL.M_NULL;
                pSData.m_bIsModel = false;
            }

            // 설정한 Data로 Mark 모델을 등록한다.
            if(m_pSystem.RegisterMarkModel(iCamNo,ref pSData))
            {
                // Display Mark Rectagle
                
                pSData.m_bIsModel = true;
                return SUCCESS;
            }
            else
            {
                pSData.m_bIsModel = false;
                return ERR_VISION_ERROR;
            }       
                 
        }

        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        //     Related Vision View Display Operations


        //     Dispaly the registered Model Image or Model Area like below ;
        //     NGC Pattern Matching Model Image
        //     @return Error Code : 0 [SUCCESS] - Success, etc. - Error
        //    
        public int DisplayPatternImage(int iCamNo, int iModelNo)
        {
#if SIMULATION_VISION
            return SUCCESS;
#endif

            if (isValidPatternMarkNo(iModelNo))
            {
                CSearchData pSData = m_pCamera[iCamNo].GetSearchData(iModelNo);

                int iWidth = (int)(pSData.m_rectModel.Width);
                int iHeight = (int)(pSData.m_rectModel.Height);

                MIL_ID tmpModel = MIL.MbufChild2d(m_pView[iCamNo].GetLocalView(),
                                    (DEF_IMAGE_SIZE_X - iWidth) / 2,
                                    (DEF_IMAGE_SIZE_Y - iHeight) / 2,
                                    iWidth,
                                    iHeight,
                                    MIL.M_NULL);

                MIL.MpatDraw(MIL.M_DEFAULT,
                                    pSData.m_milModel,
                                    tmpModel,
                                    MIL.M_DRAW_BOX + MIL.M_DRAW_POSITION,
                                    MIL.M_DEFAULT,
                                    MIL.M_DEFAULT);

            }


            return SUCCESS;
        }
        // Reset Search Area
        // @param iCamNo : Camera Number
        // @param iModelNo : Model Mark Number
        // @param SArea : Search Area Rectangle

        public int SetSearchArea(int iCamNo, int iModelNo, ref Rectangle SArea)
        {
#if SIMULATION_VISION
                retrun SUCCESS;
#endif
            if (SArea.Width <= DEF_SEARCH_MIN_WIDTH || SArea.Height <= DEF_SEARCH_MIN_HEIGHT ||
                SArea.Width > m_pCamera[iCamNo].m_CamPixelSize.Width || SArea.Height > m_pCamera[iCamNo].m_CamPixelSize.Height)
                // 104040 = Search Area Size 가 부적절합니다.
                return ERR_VISION_ERROR;

            CSearchData pSData = m_pCamera[iCamNo].GetSearchData(iModelNo);
            pSData.m_rectSearch = SArea;

            MIL.MpatSetPosition(pSData.m_milModel,
                            pSData.m_rectSearch.Left,
                            pSData.m_rectSearch.Top,
                            pSData.m_rectSearch.Width,
                            pSData.m_rectSearch.Height);

            MIL.MpatPreprocModel(MIL.M_NULL, pSData.m_milModel, MIL.M_DEFAULT);

            return SUCCESS;
        }
        
          //Recognition Model Mark (NGC Algorithm)
	      //@return int type Error Code : 0 - SUCCESS, etc. - Error
	      //@param iCamNo : Camera Number
	      //@param iModelNo : Model Mark Number
	      //@param bUseGMF : True  - Use GMF(Geometry Model Finder)
	      //                 False - Not Use GMF(Geometry Model Finder) 
	    
        public int RecognitionPatternMark(int iCamNo, int iModelNo, out CResultData pPatResult, bool bUseGMF = false)
        {
#if SIMULATION_VISION
                retrun SUCCESS;
#endif

            int iResult = 0;
            CSearchData pSData = m_pCamera[iCamNo].GetSearchData(iModelNo);
            CResultData pSResult;// = m_pCamera[iCamNo].GetResultData(iModelNo);

            iResult = m_pSystem.SearchByNGC(iCamNo, pSData, out pSResult);

            if(iResult == SUCCESS)
            {
                pSResult.m_strResult = string.Format("-MK:{0} P_X:{1:0.00}  P_Y:{2:0.00}  Sc:{3:0.00}%% Tm:{4:0.0}ms",
                                                    iModelNo,
                                                    pSResult.m_dPixelX,
                                                    pSResult.m_dPixelY,
                                                    pSResult.m_dScore,
                                                    pSResult.m_dTime  *1000);
                if (pSResult.m_bSearchSuccess)
                    pSResult.m_strResult = "OK" + pSResult.m_strResult;
                else
                    pSResult.m_strResult = "NG" + pSResult.m_strResult;

            }
            else
            {
                pSResult.m_strResult = string.Format("Camera{0} : Model : {1} is Not Found! [sc:{2:0.00}%% Tm:{3:0.0}ms",
                                                    iCamNo,
                                                    iModelNo,
                                                    pSResult.m_dScore,
                                                    pSResult.m_dTime  *1000);
            }

            pPatResult = pSResult;

            // Pattern Search Fail시 Image 저장
            if (pSResult.m_bSearchSuccess==false)
            {
                if(m_bSaveErrorImage)
                {
                    iResult = SaveImage(iCamNo,1,90);
                    if(iResult != SUCCESS)
                    {
                        return ERR_VISION_ERROR;
                    }
                }
            }
           
            return SUCCESS;
        }

        public MIL_ID GetMarkImage(int iCamNo, int iModelNo)
        {
#if SIMULATION_VISION
                retrun MIL.M_NULL;
#endif
            CSearchData pSData = m_pCamera[iCamNo].GetSearchData(iModelNo);
            
            return pSData.m_milModel;
        }
        public void SearchMarkStop(int iCamNo)
        {
            m_pView[iCamNo].SearchMarkStop();
        }

          //Make a Mask Image & Apply the Mask Image to GMF Search Context
	      //@return int type Error Code : 0 - SUCCESS, etc. - Error
	      //@param iCamNo : Camera Number
	      //@param iModelNo : Model Mark Number
	      //@param MaskRect : Rectangle for masking
	      //@param ModelRect : GMF Model Rectangle
	      //@param bMakeEndFlag : TRUE - Stop making mask image & Apply the Mask Image to GMF Search Context
	      //                      FALSE - Continue making mask image
	     
        public int MaskImage(int iCamNo, int iModelNo, ref Rectangle MaskRect, ref Rectangle ModelRect, bool bMakeEndFlag)
        {
#if SIMULATION_VISION
                retrun SUCCESS;
#endif
            return SUCCESS;
        }

          // Return Pattern Matching Result : Reference Point X value
	      // @return double type : X coordinate
	      // @param iCamNo : Camera Number
	      // @param iModelNo : Model Mark Number
	     
        public double GetSearchResultX(int iCamNo, int iModelNo)
        {
#if SIMULATION_VISION
            return 0.0;
#endif
            if (!isValidPatternMarkNo(iModelNo))
                return 0.0;

            return m_pCamera[iCamNo].GetResultData(iModelNo).m_dPixelX;
        }

      //    Return Pattern Matching Result : Reference Point Y value
      //    @return double type : Y coordinate
      //    @param iCamNo : Camera Number
      //    @param iModelNo : Model Mark Number
 
        public double GetSearchResultY(int iCamNo, int iModelNo)
        {
#if SIMULATION_VISION
            return 0.0;
#endif
            if (!isValidPatternMarkNo(iModelNo))
                return 0.0;

            return m_pCamera[iCamNo].GetResultData(iModelNo).m_dPixelY;
        }

        public double GetSearchScore(int iCamNo, int iModelNo)
        {
#if SIMULATION_VISION
            return 0.0;
#endif

            if (!isValidPatternMarkNo(iModelNo))
                return 0.0;

            return m_pCamera[iCamNo].GetResultData(iModelNo).m_dScore;
        }


         // Return Pattern Matching Result Model Rectangle
         // @return CRect type : Finded Model
         // @param iCamNo : Camera Number
         // @param iModelNo : Model Mark Number
         
        public Rectangle GetFindedModelRect(int iCamNo, int iModelNo)
        {
#if SIMULATION_VISION
            return new Rectangle(0,0,0,0);
#endif
            if (!isValidPatternMarkNo(iModelNo))
                return new Rectangle(0, 0, 0, 0);

            return m_pCamera[iCamNo].GetResultData(iModelNo).m_rectFindedModel;
        }

         // Return Pattern Matching Search Area Rectangle
         // @return CRect type : Search Area
         // @param iCamNo : Camera Number
         // @param iModelNo : Model Mark Number
         
        public Rectangle GetSearchAreaRect(int iCamNo, int iModelNo)
        {
#if SIMULATION_VISION
            return new Rectangle(0,0,0,0);
#endif
            if (!isValidPatternMarkNo(iModelNo))
                return new Rectangle(0, 0, 0, 0);

            return m_pCamera[iCamNo].GetSearchData(iModelNo).m_rectSearch;
        }

        public double GetSearchAcceptanceThreshold(int iCamNo, int iModelNo)
        {
#if SIMULATION_VISION
            return 0.0;
#endif
            if (!isValidPatternMarkNo(iModelNo))
                return 0.0;

            return m_pCamera[iCamNo].GetSearchData(iModelNo).m_dAcceptanceThreshold;
        }

        public int SetSearchAcceptanceThreshold(int iCamNo, int iModelNo, double dValue)
        {
#if SIMULATION_VISION
            return SUCCESS;
#endif

            if (!isValidPatternMarkNo(iModelNo))
                return ERR_VISION_ERROR;

            if (dValue < 0.0 || dValue > 100.0)
                return ERR_VISION_ERROR;

            CSearchData pSData = m_pCamera[iCamNo].GetSearchData(iModelNo);
            if (pSData.m_milModel == MIL.M_NULL)
                return ERR_VISION_ERROR;

            pSData.m_dAcceptanceThreshold = dValue;
            MIL.MpatSetAcceptance(pSData.m_milModel, dValue);

            MIL_ID SourceImage = m_pView[iCamNo].GetImage();

            MIL.MpatPreprocModel(SourceImage, pSData.m_milModel, MIL.M_DEFAULT);

            return SUCCESS;
        }


         // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
         //     Related Edge Finder (Caliper Tool) Operations
         //
        public int FindEdge(int iCamNo, int iModelNo)
        {
#if SIMULATION_VISION
            return SUCCESS;
#endif
            m_pSystem.FindEdge(iCamNo);

            return SUCCESS;
        }

        public int SetEdgeFinderArea(int iCamNo, Point mPos, Size mSize, double dAng)
        {
#if SIMULATION_VISION
            return SUCCESS;
#endif
            mPos.X = m_pView[iCamNo].GetImageWidth() / 2;
            mPos.Y = m_pView[iCamNo].GetImageHeight() / 2;
            mSize.Width = 400;
            mSize.Height = 3;
            dAng = 135;

            m_pSystem.SetEdgeFindParameter(mPos,mSize,dAng);

            return SUCCESS;
        }
        public int SetEdgeFinderPolarity(int iCamNo, int iModelNo, double dPolarity)
        {
#if SIMULATION_VISION
            return SUCCESS;
#endif

            return SUCCESS;
        }
        public int SetEdgeFinderThreshold(int iCamNo, int iModelNo, double dThreshold)
        {
#if SIMULATION_VISION
            return SUCCESS;
#endif
            return SUCCESS;
        }
        public int SetEdgeFinderNumOfResults(int iCamNo, int iModelNo, double dNumOfResults)
        {
#if SIMULATION_VISION
            return SUCCESS;
#endif
            return SUCCESS;
        }
        public int SetEdgeFinderDirection(int iCamNo, int iModelNo, double dSearchDirection)
        {
#if SIMULATION_VISION
            return SUCCESS;
#endif
            return SUCCESS;
        }

        public double GetEdgeFinderPolarity(int iCamNo, int iModelNo)
        {
#if SIMULATION_VISION
            return SUCCESS;
#endif
            return 0.0;
        }
        public double GetEdgeFinderThreshold(int iCamNo, int iModelNo)
        {
#if SIMULATION_VISION
            return 0.0;
#endif
            return 0.0;
        }
        public double GetEdgeFinderNumOfResults(int iCamNo, int iModelNo)
        {
#if SIMULATION_VISION
            return 0.0;
#endif
            return 0.0;
        }
        public double GetEdgeFinderDirection(int iCamNo, int iModelNo)
        {
            return 0.0;
        }
                
        //Stop Live & Grap Image
	    //  @return 
	    //  @param iCamNo : Camera Number	       
        public void HaltVideo(int iCamNo)
        {
#if SIMULATION_VISION
            return;
#endif
            if (iCamNo > DEF_MAX_CAMERA_NO) return;

            m_pCamera[iCamNo].SetLive(false);            
        }

         // Start Live : Grab Continuous
	     // @return int type Error Code : 0 - SUCCESS
	     // @param iCamNo : Camera Number
	        
        public void LiveVideo(int iCamNo)
        {
#if SIMULATION_VISION
            return;
#endif
            if (iCamNo > DEF_MAX_CAMERA_NO) return;

            m_pCamera[iCamNo].SetLive(true);
            
        }

        // Clear Overlay Display
	    // @return
	    // @param iCamNo :
	    // @param bCenterLineFlag : TRUE - Draw Center Cross Mark when clear overlay display
	    //                          FALSE - Not Draw Center Cross Mark 
	        
        public void ClearOverlay(int iCamNo)
        {
#if SIMULATION_VISION
            return;
#endif
            if (iCamNo > DEF_MAX_CAMERA_NO) return;

            m_pView[iCamNo].ClearOverlay();
            //return 0;
        }

        // Draw Rectangle on the Overlay Display	        
        public void DrawOverlayAreaRect(int iCamNo, Rectangle rect)
        {
#if SIMULATION_VISION
            return;
#endif
            if (iCamNo > DEF_MAX_CAMERA_NO) return;
            m_pView[iCamNo].DrawBox(rect);
            //return 0;
        }

        // Draw Grid On the Overlay Display
        public void DrawOverlayGrid(int iCamNo)
        {
#if SIMULATION_VISION
            return;
#endif
            //return 0;
        }

        // Draw Line On the Overlay Display
        public void DrawOverlayLine(int iCamNo, Point ptStart, Point ptEnd, int color)
        {
#if SIMULATION_VISION
            return;
#endif
            if (iCamNo > DEF_MAX_CAMERA_NO) return;

            //return 0;
        }

        // Draw Cross Mark on the Overlay Display
        public void DrawOverlayCrossMark(int iCamNo, int iWidth, int iHeight,Point center)    //, int color = 1) ;
        {
#if SIMULATION_VISION
            return;
#endif

            if (iCamNo > DEF_MAX_CAMERA_NO) return;

           m_pView[iCamNo].DrawCrossMark(center,iWidth,iHeight);           
        }

        // Draw Text on the View Image Display
        public void DrawOverlayText(int iCamNo, string strText, Point pointText)
        {
#if SIMULATION_VISION
            return;
#endif

            if (iCamNo > DEF_MAX_CAMERA_NO) return;

            //return 0;
        }
        public void DrawOverLayHairLine(int iCamNo, int Width)
        {
#if SIMULATION_VISION
            return;
#endif

            // Hair Line의 제한값 설정
            if (Width < DEF_HAIRLINE_MIN && Width > DEF_HAIRLINE_MAX) return;

            m_iHairLineWidth = Width;

            m_pView[iCamNo].ClearOverlay();
            m_pView[iCamNo].DrawHairLine(Width);
        }
        
    }
}
