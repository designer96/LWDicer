using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Diagnostics;

using static LWDicer.Control.DEF_Vision;
using static LWDicer.Control.DEF_Error;
using static LWDicer.Control.DEF_Common;
using BGAPI;
using Matrox.MatroxImagingLibrary;

namespace LWDicer.Control
{
    public class MVisionView: MObject
    {
        private MVisionCamera m_pCamera;
        private int m_iViewID;
        private int m_iResult;
        private bool m_bLocal;
        private double dGrabInterval = 0.0;
        
        private Byte[] m_ImgBits;
        private IntPtr m_ImageBuffer;
        private IntPtr m_ImageHandle;

        private int m_CameraWidth;
        private int m_CameraHeight;
        
        private Rectangle m_recImage;
        private PictureBox m_Picture;
        
        // Mil System & Display
        private MIL_ID m_pMilSystemID;
        private MIL_ID m_MilDisplay;
        private MIL_ID m_MilImage;        
        private MIL_ID m_MarkModel;
        private MIL_ID m_ImgText = MIL.M_DEFAULT;
        private MIL_ID m_MilOverlay = MIL.M_NULL;
        private MIL_ID m_MilOverLayID = MIL.M_NULL;
        
        private MIL_ID GraphicList = MIL.M_NULL;
        private MIL_INT m_ImageWidth, m_ImageHeight;

        // Overlay 관련 변수
        private IntPtr m_hCustomDC = IntPtr.Zero;
        private Graphics m_DrawGraph;
        private Pen m_DrawPen;    
        
        private Point m_ptDrawStart;
        private Point m_ptDrawEnd;

        public MVisionView(CObjectInfo objInfo) : base(objInfo)
        {
            m_iViewID   = 0;
            m_Picture   = new PictureBox();

            m_ImageHandle = new IntPtr();
            m_ImageBuffer = new IntPtr();

            m_MilImage = MIL.M_NULL;
            m_MilDisplay = MIL.M_NULL;

            m_DrawPen = new Pen(Color.LightGreen);
            m_DrawPen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
            m_ptDrawStart = new Point(0, 0);
            m_ptDrawEnd = new Point(0, 0);

        }

        public int Initialize(int iViewNo, MVisionCamera pCamera)
        {
            // Num 설정
            m_iViewID = iViewNo;

            // Mil Displayt 할당
            MIL.MdispAlloc(m_pMilSystemID, MIL.M_DEFAULT, "M_DEFAULT", MIL.M_WINDOWED, ref m_MilDisplay);

            // Camera Select
            if(SelectCamera(pCamera)==SUCCESS) return SUCCESS;
            else return GenerateErrorCode(ERR_VISION_CAMERA_CREATE_FAIL);


        }
        
        public int GetIdNum()
        {
            return m_iViewID;
        }
        public void SetMil_ID(MIL_ID MilSystem)
        {
            m_pMilSystemID  = MilSystem;
        }
        public int SelectCamera(MVisionCamera pCamera)
        {
            if (pCamera == null)  return GenerateErrorCode(ERR_VISION_CAMERA_NON_USEFUL);

            Size CameraPixelSize;

            // View의 Camera 주소에 객체를 대입한다.
            m_pCamera = pCamera;

            // Camera Pixel Size 대입
            CameraPixelSize = m_pCamera.GetCameraPixelSize();
            m_CameraWidth = CameraPixelSize.Width;
            m_CameraHeight = CameraPixelSize.Height;

            if (m_CameraWidth == 0 || m_CameraHeight == 0) return GenerateErrorCode(ERR_VISION_CAMERA_IMAGE_SIZE_FAIL);

            // image byte 변수
            m_ImgBits = new Byte[m_CameraWidth * m_CameraHeight];

            // set source image size Rect size 
            m_recImage.X = 0;
            m_recImage.Y = 0;
            m_recImage.Width = m_CameraWidth;
            m_recImage.Height = m_CameraHeight;            

            // MIL Buffer 초기화
            if(m_MilImage != MIL.M_NULL)
            {
                MIL.MbufFree(m_MilImage);
            }
            MIL.MbufAlloc2d(m_pMilSystemID, m_recImage.Width, m_recImage.Height,
                                MIL.M_UNSIGNED + 8, MIL.M_IMAGE + MIL.M_PROC + MIL.M_DISP,
                                ref m_MilImage);

            MIL.MbufClear(m_MilImage, 0);

            return SUCCESS;
        }

        /// <summary>
        /// Call Back 함수로 Image가 새로 들어올때 마다 Event를 발생시킴.
        /// </summary>
        /// <param Camera="callBackOwner"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        public int ImageCallback(object callBackOwner, ref BGAPI.Image image)
        {            
            // get image to IntPtr   
            image.get(ref m_ImageBuffer);   
            // Copy to Byte[]
            Marshal.Copy(m_ImageBuffer, m_ImgBits, 0, m_CameraWidth * m_CameraHeight);
            // MIL Buffer에 Copy
            MIL.MbufPut(m_MilImage, m_ImgBits);
            MIL.MbufControl(m_MilImage, MIL.M_MODIFIED, MIL.M_DEFAULT);

            // Timer 확인
            MIL.MappTimer(MIL.M_DEFAULT, MIL.M_TIMER_READ, ref dGrabInterval);
            
            String strResult = String.Format(" Scan Time : {0:0.00} ", dGrabInterval);
            // Timer Reset
            MIL.MappTimer(MIL.M_DEFAULT, MIL.M_TIMER_RESET, ref dGrabInterval);
            // new image 호출 (Image Grab 지령)
            m_pCamera.GetCamera().setImage(ref image);

            return SUCCESS;
        }

        private void DisplaySearchResult()
        {
            double XOrg = 0.0;              // Original model position.
            double YOrg = 0.0;
            double x = 0.0;                 // Model position.
            double y = 0.0;
            double ErrX = 0.0;              // Model error position.
            double ErrY = 0.0;
            double Score = 0.0;             // Model correlation score.

            // Read results and draw a box around the model occurrence.
            //MIL.MpatGetResult(m_SearchResult, MIL.M_POSITION_X, ref x);
            //MIL.MpatGetResult(m_SearchResult, MIL.M_POSITION_Y, ref y);
            //MIL.MpatGetResult(m_SearchResult, MIL.M_SCORE, ref Score);

            // Calculate the position errors in X and Y and inquire original model position.
            ErrX = Math.Abs((m_ImageWidth/2) - x);
            ErrY = Math.Abs((m_ImageHeight/2) - y);

            String strResult = "";

            strResult = String.Format(" Pos X : {0:0.00} \n Pos Y : {1:0.00} \n Score : {2:0.00}", ErrX, ErrY, Score);            

            DrawString(strResult, new PointF(100, 800));

        }

        public static long GrabEnd()
        {
            return 0;
        }

        public void FreeGraphicsContext()
        {

        }
        public void AllocGraphicsContext()
        {

        }

        public bool SaveImage(MIL_ID pImage , string strPath)
        {
            MIL_ID pSaveImage = MIL.M_NULL;
            // Inquire overlay size.
            MIL_INT iWidth  = MIL.MbufInquire(pImage, MIL.M_SIZE_X, MIL.M_NULL);
            MIL_INT iHeight = MIL.MbufInquire(pImage, MIL.M_SIZE_Y, MIL.M_NULL);

            MIL.MbufAlloc2d(m_pMilSystemID, iWidth, iHeight,
                                MIL.M_UNSIGNED + 8, MIL.M_IMAGE + MIL.M_PROC + MIL.M_DISP,
                                ref pSaveImage);

            MIL.MbufCopy(pImage, pSaveImage);

            MIL.MbufExport(strPath, MIL.M_BMP, pSaveImage);

            return true;
        }

        /// <summary>
        ///  현재 Grab하는 Image를 저장함.
        /// </summary>
        /// <param name="strPath"></param>
        /// <returns></returns>
        public bool SaveImage(string strPath)
        {
            MIL_ID pSaveImage = MIL.M_NULL;
            MIL.MbufAlloc2d(m_pMilSystemID, m_recImage.Width, m_recImage.Height,
                                MIL.M_UNSIGNED + 8, MIL.M_IMAGE + MIL.M_PROC + MIL.M_DISP,
                                ref pSaveImage);

            MIL.MbufCopy(m_MilImage, pSaveImage);

            MIL.MbufExport(strPath, MIL.M_BMP, pSaveImage);

            MIL.MbufFree(pSaveImage);
            
            return true;
        }

        /// <summary>
        /// SaveModelImage: Model Image를 저장함.
        /// </summary>
        /// <param name="strPath"></param>
        /// <param name="iModelNo"></param>
        /// <returns></returns>
        public bool SaveModelImage(string strPath,int iModelNo)
        {
            CVisionPatternData pSData = m_pCamera.GetSearchData(iModelNo);
           
            if (pSData.m_ModelImage == MIL.M_NULL) return false;
            MIL.MbufExport(strPath, MIL.M_BMP, pSData.m_ModelImage);
            
            return true;
        }

        /// <summary>
        /// MIL Buffer를 Panel에 영상을 Display함
        /// </summary>
        /// <param name="pImage" :  MIL Buffer 이미지></param>
        /// <param name="pHandle" : Panel의 Handle값 ></param>
        public void DisplayImage(MIL_ID pImage, IntPtr pHandle)
        {
            // Image Size Read           
            int iWidth = MIL.MbufInquire(pImage, MIL.M_SIZE_X, MIL.M_NULL);
            int iHeight = MIL.MbufInquire(pImage, MIL.M_SIZE_Y, MIL.M_NULL);
            // Image Size Check
            if (iWidth == 0 || iHeight == 0) return;

            Rectangle RecImage = new Rectangle(0, 0, iWidth, iHeight);

            // Byte 생성
            Byte[] ImgBits;
            ImgBits = new Byte[iWidth * iHeight];

            // Bitmap 생성
            Bitmap Bitmap = new Bitmap(iWidth, iHeight,
                                        PixelFormat.Format8bppIndexed);
            // Pallette 생성
            ColorPalette Palette;
            Palette = Bitmap.Palette;
            for (int i = 0; i < 256; i++)
            {
                Palette.Entries[i] = Color.FromArgb(255, i, i, i);
            }

            Bitmap.Palette = Palette;
            // BitmapData 생성
            BitmapData BmpData = Bitmap.LockBits(RecImage,
                                            ImageLockMode.WriteOnly,
                                            PixelFormat.Format8bppIndexed);
            // MIL이미지를  Byte 변환
            MIL.MbufGet(pImage, ImgBits);

            // Byte to Bitmap
            Marshal.Copy(ImgBits, 0, BmpData.Scan0, iWidth * iHeight);
            Bitmap.UnlockBits(BmpData);

            // Display할 객체의 Size를 읽기
            int Width = System.Windows.Forms.Control.FromHandle(pHandle).Width;
            int Height = System.Windows.Forms.Control.FromHandle(pHandle).Height;

            Rectangle RecHandle = new Rectangle(0, 0, Width, Height);

            // Graph로 Bmp를 Display함
            System.Drawing.Graphics graph;
            graph = System.Drawing.Graphics.FromHwnd(pHandle);
            graph.DrawImage(Bitmap, RecHandle, RecImage, GraphicsUnit.Pixel);

        }
        
        public MIL_ID GetImage()
        {
            return m_MilImage;
        }
        public MIL_ID GetViewGraph()
        {
            return GraphicList;
        }
        public int GetImageWidth()
        {
            return (int)m_ImageWidth;
        }
        public int GetImageHeight()
        {
            return (int)m_ImageHeight;
        }
        public MIL_ID GetMarkModelImage()
        {
            return m_MarkModel;
        }
        public void DrawLine(Point ptStart, Point ptEnd, Pen pPen)
        {            
            double dStartX = (double)ptStart.X;
            double dStartY = (double)ptStart.Y;
            double dEndX = (double)ptEnd.X; 
            double dEndY = (double)ptEnd.Y;

            MIL.MgraLine(m_MilOverLayID, m_MilOverlay, dStartX, dStartY, dEndX, dEndY);
        }

        public void GraphDrawLine(Point ptStart, Point ptEnd, Pen pPen)
        {
            double dStartX = (double)ptStart.X;
            double dStartY = (double)ptStart.Y;
            double dEndX = (double)ptEnd.X;
            double dEndY = (double)ptEnd.Y;

            m_DrawGraph.DrawLine(pPen,ptStart,ptEnd);
        }
        
        public void DrawCrossMark(Point Center, int Width, int Height)
        {
            // Overlay DC를 가져 온다
            if (GetOverlayDC() == false) return;

            //==================================================
            // Pen Type 설정
            m_DrawPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            m_DrawPen.Color = Color.Red;
            m_DrawPen.Width = 4;

            //==================================================
            // 중심 라인 Draw

            // 0 위치를 화면의 중앙으로 설정함.
            Center.X = m_ImageWidth / 2;
            Center.Y = m_ImageHeight / 2;

            // Width 라인 Draw
            m_ptDrawStart.X = Center.X - Width / 2;
            m_ptDrawStart.Y = Center.Y;
            m_ptDrawEnd.X = Center.X + Width / 2;
            m_ptDrawEnd.Y = Center.Y;
            GraphDrawLine(m_ptDrawStart, m_ptDrawEnd, m_DrawPen);

            // Hight 라인 Draw
            m_ptDrawStart.X = Center.X;
            m_ptDrawStart.Y = Center.Y - Height/2;
            m_ptDrawEnd.X = Center.X;
            m_ptDrawEnd.Y = Center.Y + Height / 2;
            GraphDrawLine(m_ptDrawStart, m_ptDrawEnd, m_DrawPen);

            // Overlay 화면 갱신
            UpdataOverlay();
        }
        

        public void DrawBox(Rectangle recBox)
        {
            // Overlay DC를 가져 온다
            if (GetOverlayDC() == false) return;

            //==================================================
            // Pen Type 설정
            m_DrawPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            m_DrawPen.Color = Color.Red;
            m_DrawPen.Width = 4;
            
            // 0 위치를 화면의 중앙으로 설정함.
            recBox.X = m_ImageWidth / 2;
            recBox.Y = m_ImageHeight / 2;

            //Draw할 Rec을 생성한다.
            Rectangle pRec = new Rectangle(recBox.X - recBox.Width / 2, recBox.Y - recBox.Height / 2, recBox.Width, recBox.Height);           

            m_DrawGraph.DrawRectangle(m_DrawPen, pRec);

            // Overlay 화면 갱신
            UpdataOverlay();
        }

        public void DrawString( string pStr, PointF pPos)
        {
            // Overlay DC를 가져 온다
            if (GetOverlayDC() == false) return;

            SolidBrush Brush = new SolidBrush(Color.Red);
            Font OverlayFont = new Font(FontFamily.GenericSansSerif, 40, FontStyle.Bold);
            if (m_DrawGraph == null)
            {
                UpdataOverlay();
                return;
            }
            m_DrawGraph.DrawString(pStr, OverlayFont, Brush, pPos);
            // Overlay 화면 갱신
            UpdataOverlay();            
        }
        public void DrawGrid()
        {
            // Pen Type 설정
            MIL.MgraColor(m_MilOverLayID, MIL.M_COLOR_GREEN);

            m_DrawPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;

            // 첫째 라인 Draw
            m_ptDrawStart.X = 0;
            m_ptDrawStart.Y = (int)(m_ImageHeight/2);
            m_ptDrawEnd.X = (int)m_ImageWidth;
            m_ptDrawEnd.Y = (int)(m_ImageHeight/2);

            DrawLine(m_ptDrawStart,m_ptDrawEnd, m_DrawPen);

            // 둘째 라인 Draw
            m_ptDrawStart.X = (int)(m_ImageWidth / 2);
            m_ptDrawStart.Y = 0;
            m_ptDrawEnd.X = (int)m_ImageWidth/2;
            m_ptDrawEnd.Y = (int)(m_ImageHeight);
            DrawLine(m_ptDrawStart, m_ptDrawEnd, m_DrawPen);
            
        }        

        public void DrawHairLine(int Width)
        {
            if (Width < DEF_HAIRLINE_MIN) return;
            if (Width > DEF_HAIRLINE_MAX) return;

            // Overlay DC를 가져 온다
            if (GetOverlayDC() == false) return;

            //==================================================
            // Pen Type 설정
            //MIL.MgraColor(m_MilOverLayID, MIL.M_COLOR_GREEN);
            m_DrawPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            //m_DrawPen.DashCap = System.Drawing.Drawing2D.DashCap.Round;
            m_DrawPen.Color = Color.Red;
            m_DrawPen.Width = 3;

            //==================================================
            // 중심 라인 Draw            
            m_ptDrawStart.X = 0;
            m_ptDrawStart.Y = ((int)m_ImageHeight / 2);
            m_ptDrawEnd.X = (int)m_ImageWidth;
            m_ptDrawEnd.Y = ((int)m_ImageHeight / 2);
            GraphDrawLine(m_ptDrawStart, m_ptDrawEnd, m_DrawPen);
            

            //==================================================
            // Pen Type 설정
            m_DrawPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            //m_DrawPen.DashCap = System.Drawing.Drawing2D.DashCap.Round;
            m_DrawPen.DashPattern = new float[] { 5.0F, 2.0F, 1.0F, 2.0F };
            m_DrawPen.Color = Color.LightGreen;
            m_DrawPen.Width = 4;

            //==================================================
            // 점선 라인 Draw

            // 첫째 라인 Draw
            m_ptDrawStart.X = 0;
            m_ptDrawStart.Y = ((int)m_ImageHeight / 2) - Width;
            m_ptDrawEnd.X = (int)m_ImageWidth;
            m_ptDrawEnd.Y = ((int)m_ImageHeight / 2) - Width;
            GraphDrawLine(m_ptDrawStart, m_ptDrawEnd, m_DrawPen);

            // 둘째 라인 Draw
            m_ptDrawStart.X = 1;
            m_ptDrawStart.Y = ((int)m_ImageHeight / 2) + Width;
            m_ptDrawEnd.X = (int)m_ImageWidth;
            m_ptDrawEnd.Y = ((int)m_ImageHeight / 2) + Width;
            GraphDrawLine(m_ptDrawStart, m_ptDrawEnd, m_DrawPen);

            // Overlay 화면 갱신
            UpdataOverlay();
        }
       
        public void SetDisplayWindow(IntPtr pDisplayObject)
        {
            double ZoomX;
            double ZoomY;
            // Display하는 Panel의 사이즈를 읽어온다.
            Size DisplaySize = ContainerControl.FromHandle(pDisplayObject).Size;

            // Display Size에 맞게 Zoom를 설정한다.
            ZoomX = (double)DisplaySize.Width  / (double)m_recImage.Width;
            ZoomY = (double)DisplaySize.Height / (double)m_recImage.Height;
            MIL.MdispZoom(m_MilDisplay, ZoomX, ZoomY);

            // 핸들값을 받아온다.
            m_ImageHandle = pDisplayObject;
            // Display Window를 설정한다.
            MIL.MdispSelectWindow(m_MilDisplay, m_MilImage, m_ImageHandle);

            SetLocalOverlay(true);

            // 기존 Overlay Clear
            ClearOverlay();

            m_bLocal = true;

        }

        
        public void DisplayView()
        {               
            //if (m_Bitmap == null) return;
            if (m_ImageHandle == IntPtr.Zero) return;
       
        }
        
        public bool IsLocalView()
        {
            // 할당된 Picture Handle이 없을 경우 
            if (m_Picture.Handle == IntPtr.Zero)
                return false;
            else
                return true;
        }
        public void GetLocalOverlay()
        {

        }
        public MIL_ID GetLocalView()
        {
            return m_MilImage;
        }

        public bool CheckMilBufferClear()
        {
            if (m_MilImage != MIL.M_NULL)
            {
                m_MilImage = MIL.M_NULL;
                return false;
            }
            if (m_MilDisplay != MIL.M_NULL)
            {
                m_MilDisplay = MIL.M_NULL;
                return false;
            }

            return true;
        }
        public int FreeModelImage()
        {
            return 0;
        }
        public void SetModelViewFlag(bool bModelView)
        {

        }
        public bool IsModelView()
        {
            return true;
        }
        public void ClearOverlay()
        {
            // Clear the overlay to transparent.
            MIL.MgraClear(MIL.M_DEFAULT, GraphicList);
            MIL.MdispControl(m_MilDisplay, MIL.M_OVERLAY_CLEAR, MIL.M_DEFAULT);
        }
        public bool GetOverlayDC()
        {
            // Create a device context to draw in the overlay buffer with GDI.
            MIL.MbufControl(m_MilOverlay, MIL.M_DC_ALLOC, MIL.M_DEFAULT);
            // Overlay DC 얻기
            m_hCustomDC = (IntPtr)MIL.MbufInquire(m_MilOverlay, MIL.M_DC_HANDLE, MIL.M_NULL);
            if (m_hCustomDC.Equals(IntPtr.Zero)) return false;
            // 아래 코드로 MIL 해제시 문제가 발생함.
            // MIL.MappControl(MIL.M_DEFAULT, MIL.M_ERROR, MIL.M_PRINT_ENABLE);
            m_DrawGraph = Graphics.FromHdc(m_hCustomDC);

            return true;
        }
        public void UpdataOverlay()
        {
            //   // Delete device context.
            MIL.MbufControl(m_MilOverlay, MIL.M_DC_FREE, MIL.M_DEFAULT);
            //   // Signal MIL that the overlay buffer was modified.
            MIL.MbufControl(m_MilOverlay, MIL.M_MODIFIED, MIL.M_DEFAULT);
        }

        public void FreeLocalOverlay()
        {
            
        }
        public int SetLocalOverlay(bool milSystem)
        {
            // Prepare overlay buffer.
            //***************************
            // Enable the display of overlay annotations.
            MIL.MdispControl(m_MilDisplay, MIL.M_OVERLAY, MIL.M_ENABLE);

            // Inquire the overlay buffer associated with the display.
            MIL.MdispInquire(m_MilDisplay, MIL.M_OVERLAY_ID, ref m_MilOverlay);

            // Clear the overlay to transparent.
            MIL.MdispControl(m_MilDisplay, MIL.M_OVERLAY_CLEAR, MIL.M_DEFAULT);

            // Disable the overlay display update to accelerate annotations.
            MIL.MdispControl(m_MilDisplay, MIL.M_OVERLAY_SHOW, MIL.M_DISABLE);

            // Inquire overlay size.
            m_ImageWidth = MIL.MbufInquire(m_MilOverlay, MIL.M_SIZE_X, MIL.M_NULL);
            m_ImageHeight = MIL.MbufInquire(m_MilOverlay, MIL.M_SIZE_Y, MIL.M_NULL);

            // Draw MIL overlay annotations.
            //*********************************
            // Set the graphic text background to transparent.
            MIL.MgraControl(m_ImgText, MIL.M_BACKGROUND_MODE, MIL.M_TRANSPARENT);

            // Re-enable the overlay display after all annotations are done.
            MIL.MdispControl(m_MilDisplay, MIL.M_OVERLAY_SHOW, MIL.M_ENABLE);

            m_MilOverLayID = MIL.MgraAlloc(m_pMilSystemID, MIL.M_NULL);
            
            // Draw GDI color overlay annotation.
            //***********************************
            // The inquire might not be supported
            MIL.MappControl(MIL.M_DEFAULT, MIL.M_ERROR, MIL.M_PRINT_DISABLE);

            // Allocate a graphic list to hold the subpixel annotations to draw.
            MIL.MgraAllocList(m_pMilSystemID, MIL.M_DEFAULT, ref GraphicList);

            // Associate the graphic list to the display for annotations.
            MIL.MdispControl(m_MilDisplay, MIL.M_ASSOCIATED_GRAPHIC_LIST_ID, GraphicList);

            MIL.MgraColor(MIL.M_DEFAULT, MIL.M_COLOR_GREEN);            

            return 0;
        }

        
        public void DestroyLocalView()
        {
            // Picture Handle값 초기화
            m_ImageHandle = IntPtr.Zero;

            MIL.MdispSelect(m_MilDisplay,MIL.M_NULL);

            m_bLocal = false;

        }

        public IntPtr GetViewHandle()
        {
            return m_ImageHandle;
        }
        public void GetDisplay(int iCamNo)
        {

        }
        public int CreatViews(long lZoomFactor)
        {
            return 0;
        }

        public void FreeDisplay()
        {
            DestroyLocalView();

            m_hCustomDC = IntPtr.Zero;

            m_ImgText = MIL.M_NULL;
            m_MilOverlay = MIL.M_NULL;
            //m_SearchResult = MIL.M_NULL;
            m_MarkModel = MIL.M_NULL;
            m_MilOverLayID = MIL.M_NULL;
            m_MilDisplay = MIL.M_NULL;

            MIL.MgraClear(MIL.M_DEFAULT, GraphicList);
            MIL.MgraFree(GraphicList);
            MIL.MbufFree(m_MilOverlay);
            MIL.MbufFree(m_MilOverLayID);
            //MIL.MbufFree(m_SearchResult);
            MIL.MbufFree(m_MarkModel);
            MIL.MbufFree(m_ImgText);
            MIL.MbufFree(m_MilImage);
            MIL.MdispFree(m_MilDisplay);
        }
    }
}
