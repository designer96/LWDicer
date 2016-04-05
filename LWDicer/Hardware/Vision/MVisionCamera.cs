using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;

using static LWDicer.Control.DEF_Vision;
using static LWDicer.Control.DEF_Error;
using static LWDicer.Control.DEF_Common;

using BGAPI;
using Matrox.MatroxImagingLibrary;

namespace LWDicer.Control
{
    public class MVisionCamera: MObject
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
        private MVisionView m_pDisplay;

        public Size m_CamPixelSize;
        private CCameraData m_cCameraData;
        private CVisionPatternData[] m_rgsCSearchData;
        private CResultData[] m_rgsCResultData;
        private bool m_bLive;
        
        public MVisionCamera(CObjectInfo objInfo): base(objInfo)
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
                return GenerateErrorCode(ERR_VISION_CAMERA_CREATE_FAIL);
            }

            // get camera device information   
            m_iResult = m_Camera.getDeviceInformation(ref m_CamState, ref m_CamDeviceInfo);
            m_cCameraData.m_CamDeviceInfo = m_CamDeviceInfo;
            if (m_iResult != BGAPI.Result.OK)
            {   // "Camera get Device Information failed!"
                return GenerateErrorCode(ERR_VISION_CAMERA_GET_INFO_FAIL);
            }

            // get camera Image information   
            m_iResult = m_Camera.getImageFormatDescription(1, ref m_CamImageInfo);
      
            if (m_iResult != BGAPI.Result.OK)
            {   // "Camera get Device Information failed!"
                return GenerateErrorCode(ERR_VISION_CAMERA_GET_INFO_FAIL);
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
                return GenerateErrorCode(ERR_VISION_CAMERA_CONNECT_FAIL);
            }

            // image create 
            m_iResult = BGAPI.EntryPoint.createImage(ref m_CamImage);
            if (m_iResult != BGAPI.Result.OK)
            {   // Create Image failed
                return GenerateErrorCode(ERR_VISION_CAMERA_CREATE_IMAGE_FAIL);
            }
            
            // camera & image connect 
            m_iResult = m_Camera.setImage(ref m_CamImage);
            if (m_iResult != BGAPI.Result.OK)
            {   // Camera set Image failed!
                return GenerateErrorCode(ERR_VISION_CAMERA_GET_IMAGE_FAIL);
            }
                        
            return SUCCESS;
        }

        /// <summary>
        /// SetMil_ID : MIL ID를 카메라와 연결함
        /// </summary>
        /// <param name="MilSystem": System에서 생성된 MIL ID></param>
        public void SetMil_ID(MIL_ID MilSystem)
        {
            m_pMilSystemID = MilSystem;
        }

        /// <summary>
        /// SelectView : View와 카메라와 연결함
        /// </summary>
        /// <param name="m_Display"></param>
        /// <returns></returns>
        public int SelectView(MVisionView m_Display)
        {
            // Display 객체를 받아온다
            m_pDisplay = m_Display;

            // CallBack 함수를 등록한다.
            m_iResult = m_Camera.registerNotifyCallback(this, m_pDisplay.ImageCallback);
            if (m_iResult != BGAPI.Result.OK)
            {   // Camera register Notify Callback failed!
                return GenerateErrorCode(ERR_VISION_CAMERA_SET_CALLBACK_FAIL);
            }

            return SUCCESS;
        }

        /// <summary>
        /// GetCamDeviceInfo : 카메라 정보를 반환한다.
        /// </summary>
        /// <returns></returns>
        public CCameraData GetCamDeviceInfo()
        {
            return m_cCameraData;
        }
        /// <summary>
        /// GetCamera : 카메라 객체를 반환한다.
        /// </summary>
        /// <returns></returns>
        public BGAPI.Camera GetCamera()
        {
            return m_Camera;
        }
        /// <summary>
        /// GetCameraPixelSize: 카메라 영상의 가로/세로 사이즈를 반환한다
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// SetLive: 카메라의 Live 상태를 지령한다.
        /// </summary>
        /// <param name="bRun"></param>
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
        /// <summary>
        /// SetLiveFlag : 카메라 Live 상태 Bit를 설정한다.
        /// </summary>
        /// <param name="bLive"></param>
        public void SetLiveFlag(bool bLive)
        {
            m_bLive = bLive;
        }

        /// <summary>
        /// IsLive : 카메라의 Live 상태를 반환한다.
        /// </summary>
        /// <returns></returns>
        public bool IsLive()
        {
            return m_bLive;
        }

        /// <summary>
        /// SetTrigger: 카메라의 Trigger를 설정한다. (SW 방식)
        /// </summary>
        public void SetTrigger()
        {
            m_Camera.setTrigger(true);
            m_Camera.setTriggerSource(BGAPI_TriggerSource.BGAPI_TRIGGERSOURCE_SOFTWARE); 
            m_Camera.doTrigger();

        }
        /// <summary>
        /// MirrorImage: 영상의 이미지를 반전한다.
        /// </summary>
        public void MirrorImage()
        {
            // $$$ 기능 추가
            //return 0;
        }

        /// <summary>
        /// SetSearchData: Search할 Data를 카메라 내부로 할당한다.
        /// </summary>
        /// <param name="iModelNo"></param>
        /// <param name="pSearchData"></param>
        /// <returns></returns>
        public bool SetSearchData(int iModelNo, CSearchData pSearchData)
        {
            m_rgsCSearchData[iModelNo].m_bIsModel = pSearchData.m_bIsModel;
            m_rgsCSearchData[iModelNo].m_dAcceptanceThreshold = pSearchData.m_dAcceptanceThreshold;
            m_rgsCSearchData[iModelNo].m_pointReference = pSearchData.m_pointReference;
            m_rgsCSearchData[iModelNo].m_rectModel = pSearchData.m_rectModel;
            m_rgsCSearchData[iModelNo].m_rectSearch = pSearchData.m_rectSearch;
            m_rgsCSearchData[iModelNo].m_strFileName = pSearchData.m_strFileName;
            m_rgsCSearchData[iModelNo].m_strFilePath = pSearchData.m_strFilePath;
            
            return true;
        }
        /// <summary>
        /// GetSearchData: 할당된 Search Data를 반환한다.
        /// </summary>
        /// <param name="iModelNo"></param>
        /// <returns></returns>
        public CVisionPatternData GetSearchData(int iModelNo)
        { 
            return m_rgsCSearchData[iModelNo];
        }
        /// <summary>
        /// GetResultData: Pattern Search한 결과를 반환한다.
        /// </summary>
        /// <param name="iModelNo"></param>
        /// <returns></returns>
        public CResultData GetResultData(int iModelNo)
        {
            return m_rgsCResultData[iModelNo];
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
            pSData.m_rectSearch = new Rectangle(3, 3, DEF_IMAGE_SIZE_X - 3, DEF_IMAGE_SIZE_Y - 3);
            pSData.m_dAcceptanceThreshold = 70.0;
            pSData.m_dCertaintyThreshold = 90.0;

            // $$$ DB에 저장 기능 추가

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
