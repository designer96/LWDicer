using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

using System.Windows.Forms;
using System.Drawing;
using LWDicer.UI;
using static LWDicer.Control.DEF_Vision;
using static LWDicer.Control.DEF_Error;
using Matrox.MatroxImagingLibrary;

namespace LWDicer.Control
{
    class MVisionModelData
    {
        private string m_strPath;
        private string m_strFileName;

        // System ID //
        private MIL_ID m_MilSystemID;

        public MVisionModelData(MIL_ID pSystemID, string strFileName, string strFilePath)
        {
            m_MilSystemID = pSystemID;
            m_strPath = strFilePath;
            m_strFileName = strFileName;
        }
        public MVisionModelData()
        {

        }

        public int DeleteSearchData(int iModelNo, ref CSearchData pSdata)
        {
            // Section Name //
            string strSection;
            strSection= string.Format("SEARCH_MODEL_DATA_%{0}", iModelNo);

            //String strModelDataFileName = pSdata.m_strFilePath + "\\" + pSdata.m_strFileName;
            //WritePrivateProfileSection(strSection, "", strModelDataFileName);

            return SUCCESS;
        }

        public int ReadCameraData(int iCamNo, ref CCameraData pData)
        {
            // Section Name 
            string strSection = string.Format("CAMERA_DATA");
            // Grab Settling Time //
            if (!GetIniValue(strSection, "GrabSettlingTime", out pData.m_iGrabSettlingTime))
            {
                pData.m_iGrabSettlingTime = 0;
                goto ERROR_OPERATION;
            }

            // Camera Change Time //
            if (!GetIniValue(strSection, "CameraChangeTime", out pData.m_iCameraChangeTime))
            {
                pData.m_iCameraChangeTime = 0;
                goto ERROR_OPERATION;
            }

            return SUCCESS;

        ERROR_OPERATION:
            // Msg로 PopUp으로 보내줌.
            return ERR_VISION_FILE_READ_FAILURE;
        }
        public int WriteCameraData(CCameraData pData)
        {
            // Section Name 
            string strSection = string.Format("CAMERA_DATA");
            // Grab Settling Time //
            if (!SetIniValue(strSection, "GrabSettlingTime", pData.m_iGrabSettlingTime))
            {
                goto ERROR_OPERATION;
            }

            // Camera Change Time //
            if (!SetIniValue(strSection, "CameraChangeTime", pData.m_iCameraChangeTime))
            {
                goto ERROR_OPERATION;
            }

            return SUCCESS;

        ERROR_OPERATION:
            // Msg로 PopUp으로 보내줌.
            return ERR_VISION_FILE_READ_FAILURE;
        }
        
        
        public int ReadSearchData(int iCamNo, int iModelNo, ref CSearchData pSData)
        {
            string strLoadFileName;
            string strSection;
            
            int iLeft;
            int iTop;
            int iWidth;
            int iHeight;
            
            if (iModelNo == 1)
                pSData = CMainFrame.LWDicer.m_DataManager.m_ModelData.MacroPatternA;
            if (iModelNo == 2)
                pSData = CMainFrame.LWDicer.m_DataManager.m_ModelData.MacroPatternB;

            pSData.m_bIsModel = false;

            // Section Name
            strSection = string.Format("SEARCH_MODEL_DATA_{0}", iModelNo);

            // ************************************************************************* //
            // Search Area 값 읽기
            if (!GetIniValue(strSection, "SearchAreaLeft", out iLeft))
            {
                // 처음으로 읽어 들인 데이터가 존재하지 않으면 모델이
                // 등록 되지 않았다고 간주한다.
                // 사용하지 않는 모델입니다.
                return ERR_VISION_NOT_USE_MODEL;
            }
            if (!GetIniValue(strSection, "SearchAreaTop", out iTop))
            {
                goto READING_ERROR_OPERATION;
            }

            if (!GetIniValue(strSection, "SearchAreaWidth", out iWidth))
            {
                goto READING_ERROR_OPERATION;
            }

            if (!GetIniValue(strSection, "SearchAreaHeight", out iHeight))
            {
                goto READING_ERROR_OPERATION;
            }

            // Search Area 크기 확인
            pSData.m_rectSearch = new Rectangle(iLeft, iTop, iWidth, iHeight);
            if (pSData.m_rectSearch.Left < 0 || pSData.m_rectSearch.Top < 0)
            {
                goto READING_ERROR_OPERATION;
            }
            else if (pSData.m_rectSearch.Right > DEF_IMAGE_SIZE_X
                    || pSData.m_rectSearch.Bottom > DEF_IMAGE_SIZE_Y)
            {
                goto READING_ERROR_OPERATION;
            }

            //// ************************************************************************* //
            //// 저장된 Pattern Model 읽어옴
            //strLoadFileName = string.Format(DEF_NGC_MARK_NAME_TEMPLATE,
            //                               /* pSData.m_strFilePath,*/ iCamNo, iModelNo);
            //MIL.MpatRestore(m_MilSystemID,strLoadFileName,ref pSData.m_milModel);
            //// Null이 경우에 Err
            //if(pSData.m_milModel == MIL.M_NULL) return ERR_VISION_FILE_READ_FAILURE;

            //// Restore Model Area
            //double dOffsetX = 0.0 , dOffsetY = 0.0;
            //double dWidth = 0.0 , dHeight = 0.0;

            //MIL.MpatInquire(pSData.m_milModel, MIL.M_ALLOC_OFFSET_X, ref dOffsetX);
            //MIL.MpatInquire(pSData.m_milModel, MIL.M_ALLOC_OFFSET_Y, ref dOffsetY);
            //if (dOffsetX < 0 || dOffsetX > DEF_IMAGE_SIZE_X) 
            //    return ERR_VISION_INVALID_MODEL;
            //else if(dOffsetY < 0 || dOffsetY > DEF_IMAGE_SIZE_Y)
            //    return ERR_VISION_INVALID_MODEL;

            //MIL.MpatInquire(pSData.m_milModel, MIL.M_ALLOC_SIZE_X, ref dWidth);
            //MIL.MpatInquire(pSData.m_milModel, MIL.M_ALLOC_SIZE_Y, ref dHeight);
            //if (dWidth < 0 || dHeight < 0)
            //    return ERR_VISION_INVALID_MODEL;
            //else if (dWidth > DEF_IMAGE_SIZE_X || dHeight > DEF_IMAGE_SIZE_Y)
            //    return ERR_VISION_INVALID_MODEL;
            //// Model Rectangle Size 설정
            //pSData.m_rectModel = new Rectangle((int)dOffsetX, (int)dOffsetY, (int)dWidth, (int)dHeight);

            //// Reference Point 설정
            //double dCenterX=0.0 , dCenterY=0.0;
            //MIL.MpatInquire(pSData.m_milModel, MIL.M_CENTER_X, ref dCenterX);
            //MIL.MpatInquire(pSData.m_milModel, MIL.M_CENTER_Y, ref dCenterY);

            //if ((pSData.m_rectModel.Left + dCenterX) < 0
            //    || (pSData.m_rectModel.Left + dCenterX) > DEF_IMAGE_SIZE_X
            //    || (pSData.m_rectModel.Top + dCenterY) < 0
            //    || (pSData.m_rectModel.Top + dCenterY) > DEF_IMAGE_SIZE_Y)
            //    return ERR_VISION_INVALID_REFERENCE_POINT;

            //pSData.m_pointReference.X = (int)dCenterX;
            //pSData.m_pointReference.Y = (int)dCenterY;

            //MIL.MpatInquire(pSData.m_milModel, MIL.M_ACCEPTANCE_THRESHOLD, ref pSData.m_dAcceptanceThreshold);
            //MIL.MpatInquire(pSData.m_milModel, MIL.M_CERTAINTY_THRESHOLD, ref pSData.m_dCertaintyThreshold);

            //MIL.MpatSetPosition(pSData.m_milModel, pSData.m_rectSearch.Left, pSData.m_rectSearch.Top, pSData.m_rectSearch.Width, pSData.m_rectSearch.Height);
            //MIL.MpatPreprocModel(MIL.M_NULL, pSData.m_milModel, MIL.M_DEFAULT);
           
            //pSData.m_bIsModel = true;
                        
            return SUCCESS;

            // ************************************************************************* //
            // Read Err처리 
            READING_ERROR_OPERATION:

            return ERR_VISION_FILE_READ_FAILURE;
        }
        
        private bool MakeBackUpFile()
        {
            string strSrcFile = m_strFileName;

            if (strSrcFile.EndsWith("dat") == false) return false;

            string strDestFile = strSrcFile + ".bak";
             
            //strSrcFile.

            return true;
        }
        //==============================================================================================//
        //
        //      INI file Read/Write
        //
        //==============================================================================================//

        // INI 값을 읽어 온다. 
        private bool GetIniValue(String Section, String Key, out int pValue)
        {
            StringBuilder temp = new StringBuilder(255);
            pValue = GetPrivateProfileString(Section, Key, "0", temp, 255, m_strPath);
            return true;
        }

        // INI 값을 셋팅
        private bool SetIniValue(String Section, String Key, int Value)
        {
            string strValue = string.Format("{0}", Value);
            WritePrivateProfileString(Section, Key, strValue, m_strPath);
            
            return true;
        }

        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileString(    // GetIniValue 를 위해
                                    String section,
                                    String key,
                                    String def,
                                    StringBuilder retVal,
                                    int size,
                                    String filePath);

        [DllImport("kernel32.dll")]
        private static extern long WritePrivateProfileString(  // SetIniValue를 위해
                                    String section,
                                    String key,
                                    String val,
                                    String filePath);
        [DllImport("kernel32.dll")]
        private static extern long WritePrivateProfileSection(
                                    String section,
                                    String val,
                                    String filePath);

    }
}
