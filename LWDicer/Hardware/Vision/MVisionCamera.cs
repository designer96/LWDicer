using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using static LWDicer.Control.DEF_Vision;
using static LWDicer.Control.DEF_Error;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;

using BGAPI;
using Matrox.MatroxImagingLibrary;

namespace LWDicer.Control
{
    class MVisionCamera
    {
        private int m_iCamID;
        private int m_iResult;               

        private BGAPI.System m_CamSystem;
        private BGAPI.Camera m_Camera;
        private BGAPI.Image m_CamImage;
        private BGAPI_FeatureState m_CamState;
        private BGAPIX_TypeRangeINT m_CamExposure;
        private BGAPIX_CameraInfo m_CamDeviceInfo;
        private BGAPIX_CameraImageFormat m_CamImageInfo;

        private MIL_ID m_pMilSystemID;
        private MVisionDisplay m_pDisplay;

        public Size m_CamPixelSize;
        private CCameraData m_cCameraData;
        private CVisionPatternData[] m_rgsCSearchData;
        private CResultData[] m_rgsCResultData;
        private bool m_bLive;



        ///// Search Model Data
        //// MIL 에서 사용하는 Model ID (NGC)
        //public MIL_ID m_milModel = new MIL_ID();
        //// MIL 에서 영상 Display용
        //public MIL_ID m_ModelImage = new MIL_ID();
        //// MIL 에서 사용하는 Model ID (GMF)
        //public MIL_ID m_milGmfModel = new MIL_ID();

        public MVisionCamera()
        {
            int iMarkNum = (int)EPatternMarkType.ALIGN_MARK_COUNT;

            m_iCamID            = 0;
            m_iResult           = 0;
            m_bLive             = false;
            m_cCameraData       = new CCameraData();
            m_rgsCSearchData    = new CVisionPatternData[iMarkNum];
            m_rgsCResultData    = new CResultData[iMarkNum];

            for (int i = 0; i < iMarkNum; i++)
            {
                // Search Data Init
                m_rgsCSearchData[i] = new CVisionPatternData();
                m_rgsCSearchData[i].m_bIsModel = false;
                m_rgsCSearchData[i].m_dAcceptanceThreshold = DEF_DEFAULT_ACCEP_THRESHOLD;
                m_rgsCSearchData[i].m_dCertaintyThreshold  = DEF_DEFAULT_CERTAIN_THRESHOLD;

                // Result Data Init
                m_rgsCResultData[i] = new CResultData();
                m_rgsCResultData[i].m_bSearchSuccess = false;
                m_rgsCResultData[i].m_milResult = MIL.M_NULL;
                m_rgsCResultData[i].m_milGMFResult = MIL.M_NULL;
            }

            m_cCameraData.m_iGrabSettlingTime = 0;
            m_cCameraData.m_iCameraChangeTime = 0;
        }


        public int FreeCamera()
        {
            return 0;
        }
        
        /// <summary>
        /// Camera 초기화 진행함
        /// </summary>
        /// <param 카메라 번호="iCamNo"></param>
        /// <param 상위 GigE System="pSystem"></param>
        /// <returns></returns>
        public int Initialize(int iCamNo, BGAPI.System pSystem)
        {
            m_iCamID = iCamNo;
            // System 을 받아옴.
            m_CamSystem = pSystem;

            m_CamDeviceInfo = new BGAPI.BGAPIX_CameraInfo();
            m_CamImage         = new BGAPI.Image();

            // create camera   
            m_iResult = m_CamSystem.createCamera(iCamNo, ref m_Camera);
            if (m_iResult != BGAPI.Result.OK)            
            {
                //"System create camera failed!"
                return ERR_VISION_ERROR;
            }

            // get camera device information   
            m_iResult = m_Camera.getDeviceInformation(ref m_CamState, ref m_CamDeviceInfo);
            m_cCameraData.m_CamDeviceInfo = m_CamDeviceInfo;
            if (m_iResult != BGAPI.Result.OK)
            {   // "Camera get Device Information failed!"
                return ERR_VISION_ERROR;
            }

            // get camera Image information   
            m_iResult = m_Camera.getImageFormatDescription(1, ref m_CamImageInfo);
      
            if (m_iResult != BGAPI.Result.OK)
            {   // "Camera get Device Information failed!"
                return ERR_VISION_ERROR;
            }
            else
            {
                // Camera Pixel Size을 적용한다.
                m_CamPixelSize.Width = (int)(m_CamImageInfo.iScaleRoiX * m_CamImageInfo.iSizeX);
                m_CamPixelSize.Height = (int)(m_CamImageInfo.iScaleRoiY * m_CamImageInfo.iSizeY);
            }

            // camera open 
            m_iResult = m_Camera.open();
            if (m_iResult != BGAPI.Result.OK)
            {   //"Camera open failed!"
                return ERR_VISION_ERROR;
            }

            // image create 
            m_iResult = BGAPI.EntryPoint.createImage(ref m_CamImage);
            if (m_iResult != BGAPI.Result.OK)
            {   // Create Image failed
                return ERR_VISION_ERROR;
            }
            
            // camera & image connect 
            m_iResult = m_Camera.setImage(ref m_CamImage);
            if (m_iResult != BGAPI.Result.OK)
            {   // Camera set Image failed!
                return ERR_VISION_ERROR;
            }
                        
            return SUCCESS;
        }

        public void SetMil_ID(MIL_ID MilSystem)
        {
            m_pMilSystemID = MilSystem;
        }
        public int SelectView(MVisionDisplay m_Display)
        {
            // Display 객체를 받아온다
            m_pDisplay = m_Display;

            // CallBack 함수를 등록한다.
            m_iResult = m_Camera.registerNotifyCallback(this, m_pDisplay.ImageCallback);
            if (m_iResult != BGAPI.Result.OK)
            {   // Camera register Notify Callback failed!
                return ERR_VISION_ERROR;
            }

            return SUCCESS;
        }        

        public bool CheckImage()
        {
            return true;
        }


        public CCameraData GetCamDeviceInfo()
        {
            return m_cCameraData;
        }

        public BGAPI.Camera GetCamera()
        {
            return m_Camera;
        }

        public Size GetCameraPixelSize()
        {
            return m_CamPixelSize;
        }
        /// <summary>
        /// 현재 Image를 보내준다.
        /// </summary>
        /// <returns></returns>
        
        public int GetCamID()
        {
            return m_iCamID;
        }
        public void SetLive(bool bRun)
        {
            int iRes;
            if (bRun == true)
            {
                iRes = m_Camera.setStart(true);
                // Trigger Mode Off
                m_Camera.setTrigger(false);
            }

            else
            {
                iRes = m_Camera.setStart(false);
            }

            SetLiveFlag(bRun);

        }
        public void SetLiveFlag(bool bLive)
        {
            m_bLive = bLive;
        }
        public bool IsLive()
        {
            return m_bLive;
        }
        public void SetTrigger()
        {
            m_Camera.setTrigger(true);
            m_Camera.setTriggerSource(BGAPI_TriggerSource.BGAPI_TRIGGERSOURCE_SOFTWARE); 
            m_Camera.doTrigger();

        }
        public void MirrorImage()
        {
            //return 0;
        }

        public bool SetSearchData(int iModelNo, CSearchData pSearchData)
        {
            //m_rgsCSearchData[iModelNo] = pSearchData;
            m_rgsCSearchData[iModelNo].m_bIsModel = pSearchData.m_bIsModel;
            m_rgsCSearchData[iModelNo].m_dAcceptanceThreshold = pSearchData.m_dAcceptanceThreshold;
            m_rgsCSearchData[iModelNo].m_pointReference = pSearchData.m_pointReference;
            m_rgsCSearchData[iModelNo].m_rectModel = pSearchData.m_rectModel;
            m_rgsCSearchData[iModelNo].m_rectSearch = pSearchData.m_rectSearch;
            m_rgsCSearchData[iModelNo].m_strFileName = pSearchData.m_strFileName;
            m_rgsCSearchData[iModelNo].m_strFilePath = pSearchData.m_strFilePath;
            
            return true;
        }
        public CVisionPatternData GetSearchData(int iModelNo)
        { 
            return m_rgsCSearchData[iModelNo];
        }
        // Pattern Maching Result Data
        public CResultData GetResultData(int iModelNo)
        {
            return m_rgsCResultData[iModelNo];
        }

        public int LoadCameraData(string strModelFilePath)
        {
            int iResult;
            string strFileName;
            //	strFileName.Format("Cam_%d.dat", m_iCamID);
            strFileName= string.Format("VisionCamera_#{0}.dat", m_iCamID + 1);

            MVisionModelData cameraData= new MVisionModelData(m_pMilSystemID, strFileName, strModelFilePath);
            iResult = cameraData.ReadCameraData(m_iCamID, ref m_cCameraData);
            if (iResult != SUCCESS) return ERR_VISION_FILE_READ_FAILURE;

            return SUCCESS;            
        }

        public int WriteCameraData()
        {
            return SUCCESS;
        }
        public int LoadSearchData(string strModelFilePath)
        {
            string strFileName;
            strFileName = string.Format("VisionCamera_#{0}.dat", m_iCamID + 1);

            MVisionModelData modelData = new MVisionModelData(m_pMilSystemID, strFileName, strModelFilePath);

           // CResultData pRData;
            //for (int i = 0; i < DEF_USE_SEARCH_MARK_NO; i++)
            //{
            //    modelData.ReadSearchData(m_iCamID, i, ref m_rgsCSearchData[i]);
            //}

            return SUCCESS;
        }

        public int WriteSearchData(int iModelNo)
        {
            string strFileName;
            CSearchData pSData = m_rgsCSearchData[iModelNo];

            //strFileName= string.Format(DEF_NGC_MARK_NAME_TEMPLATE, pSData.m_strFilePath, m_iCamID, iModelNo);
            //MIL.MpatSave(strFileName, pSData.m_milModel);

            //MVisionModelData modelData = new MVisionModelData(m_pMilSystemID,
            //                                                  pSData.m_strFileName,
            //                                                  pSData.m_strFilePath);
            //modelData.WriteSearchData(iModelNo, ref pSData);

            return SUCCESS;
        }


        public void DeleteSearchModel(int iModelNo)
        {
            CVisionPatternData pSData = m_rgsCSearchData[iModelNo];

            if (pSData.m_milModel != MIL.M_NULL && pSData.m_bIsModel)
            {
                MIL.MpatFree(pSData.m_milModel);
                pSData.m_milModel = MIL.M_NULL;
                pSData.m_bIsModel = false;
            }

            pSData.m_pointReference = new Point(0, 0);
            //pSData.m_rectModel.SetRectEmpty();
            pSData.m_rectSearch = new Rectangle(3, 3, DEF_IMAGE_SIZE_X - 3, DEF_IMAGE_SIZE_Y - 3);
            pSData.m_dAcceptanceThreshold = 70.0;
            pSData.m_dCertaintyThreshold = 90.0;


            //MVisionModelData visionModelData = new MVisionModelData(m_pMilSystemID,
            //                                                        pSData.m_strFileName,
            //                                                        pSData.m_strFilePath);

            //visionModelData.DeleteSearchData(iModelNo, ref pSData);


            //string strFileName;
            //strFileName= string.Format(DEF_NGC_MARK_NAME_TEMPLATE, pSData.m_strFilePath, m_iCamID, iModelNo);

            //// File 이 존재하지 않으면 Fail ⇒ FALSE Return.            
            //File.Delete(strFileName);

        }

        public void RemoveModel()
        {
            //return 0;
        }


        public int GetGrabSettlingTime()
        {
            return m_cCameraData.m_iGrabSettlingTime;
        }
        public void SetGrabSettlingTime(int iGrabSettlingTime)
        {
            m_cCameraData.m_iGrabSettlingTime = iGrabSettlingTime;            
        }

        public int GetCameraChangeTime()
        {
            return m_cCameraData.m_iCameraChangeTime;
        }
        public void SetCameraChangeTime(int iCameraChangeTime)
        {
            m_cCameraData.m_iCameraChangeTime = iCameraChangeTime;            
        }
    }
}
