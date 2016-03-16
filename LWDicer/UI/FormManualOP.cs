using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using LWDicer.Control;
using LWDicer.UI;
using static LWDicer.Control.DEF_Vision;

namespace LWDicer.UI
{
    public partial class FormManualOP : Form
    {
        private PageInfo PrevPage = null;
        private PageInfo NextPage = null;

        private BottomPageInfo NextBottomPage = null;

        public void SetPrevPage(PageInfo page)
        {
            PrevPage = page;
        }

        public void SetNextPage(PageInfo page)
        {
            NextPage = page;
        }
        public void SetNextBottomPage(BottomPageInfo page)
        {
            NextBottomPage = page;
        }

        public FormManualOP()
        {
            InitializeComponent();

            InitializeForm();
        }

        protected virtual void InitializeForm()
        {
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.DesktopLocation = new Point(DEF_UI.MAIN_POS_X, DEF_UI.MAIN_POS_Y);
            this.Size = new Size(DEF_UI.MAIN_SIZE_WIDTH, DEF_UI.MAIN_SIZE_HEIGHT);
            this.FormBorderStyle = FormBorderStyle.None;            
        }

        private void BtnNext_Click(object sender, EventArgs e)
        {
            if (NextPage == null)
            {
                return;
            }

            CMainFrame.MainFrame.MoveToPage(NextPage);
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            if (PrevPage == null)
            {
                return;
            }

            CMainFrame.MainFrame.MoveToPage(PrevPage);
            CMainFrame.MainFrame.MoveToBottomPage(NextBottomPage);
        }

        private void btnEdgeFind_Click(object sender, EventArgs e)
        {
            int iCamNo = CMainFrame.LWDicer.m_Vision.m_iCurrentViewNum;

            CEdgeData mEdgeData = new CEdgeData();

            CMainFrame.LWDicer.m_Vision.SetEdgeFinderArea(iCamNo);
            CMainFrame.LWDicer.m_Vision.FindEdge(iCamNo, ref mEdgeData);

            lblResult.Text = mEdgeData.m_strResult;
        }

        private void btnSaveModelImage_Click(object sender, EventArgs e)
        {
            int iCamNo = CMainFrame.LWDicer.m_Vision.m_iCurrentViewNum;
            //CMainFrame.LWDicer.m_Vision.SaveModelImage(iCamNo, 1);
        }

        private void btnMacroView_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_Vision.DestroyLocalView(FINE_CAM);
            CMainFrame.LWDicer.m_Vision.DestroyLocalView(PRE__CAM);
            CMainFrame.LWDicer.m_Vision.InitialLocalView(PRE__CAM, CMainFrame.MainFrame.m_FormManualOP.VisionView1.Handle);
            
        }

        private void btnMicroView_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_Vision.DestroyLocalView(FINE_CAM);
            CMainFrame.LWDicer.m_Vision.DestroyLocalView(PRE__CAM);
            CMainFrame.LWDicer.m_Vision.InitialLocalView(FINE_CAM, CMainFrame.MainFrame.m_FormManualOP.VisionView1.Handle);
        }

        private void btnShowHairLine_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_Vision.ClearOverlay();
            CMainFrame.LWDicer.m_Vision.DrawOverLayHairLine(DEF_HAIRLINE_NOR);
        }

        private void btnMarkROI_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_Vision.ClearOverlay();
            Point ptPoint = new Point(0, 0);
            Rectangle ptRec = new Rectangle(0, 0, 300,300);
            CMainFrame.LWDicer.m_Vision.DrawOverlayCrossMark(ptRec.Width, ptRec.Height, ptPoint);
            CMainFrame.LWDicer.m_Vision.DrawOverlayAreaRect( ptRec);
        }

        private void btnMarkRegisterA_Click(object sender, EventArgs e)
        {
            int iCamNo = CMainFrame.LWDicer.m_Vision.m_iCurrentViewNum;
            int iCamWidth = CMainFrame.LWDicer.m_Vision.GetCameraPixelSize(iCamNo).Width;
            int iCamHeight = CMainFrame.LWDicer.m_Vision.GetCameraPixelSize(iCamNo).Height;

            Rectangle ptRecSearch = new Rectangle(0, 0, iCamWidth, iCamHeight);
            Rectangle ptRecModel = new Rectangle(0, 0, 300,300);
            Point ptPointTemp = new Point(0, 0);

            string strModel = CMainFrame.LWDicer.m_DataManager.m_ModelData.Name;
            CMainFrame.LWDicer.m_Vision.RegisterPatternMark(iCamNo,strModel, PATTERN_A, ref ptRecSearch, ref ptRecModel, ref ptPointTemp);
            
            if (iCamNo == PRE__CAM)
                CMainFrame.LWDicer.m_DataManager.m_ModelData.MacroPatternA = CMainFrame.LWDicer.m_Vision.GetSearchData(iCamNo, PATTERN_A);
            if (iCamNo == FINE_CAM)
                CMainFrame.LWDicer.m_DataManager.m_ModelData.MicroPatternA = CMainFrame.LWDicer.m_Vision.GetSearchData(iCamNo, PATTERN_A);


            //CMainFrame.LWDicer.m_DataManager.SaveModelList();
            CMainFrame.LWDicer.m_DataManager.ChangeModel();

        }

        private void btnMarkRegisterB_Click(object sender, EventArgs e)
        {
            int iCamNo = CMainFrame.LWDicer.m_Vision.m_iCurrentViewNum;
            int iCamWidth = CMainFrame.LWDicer.m_Vision.GetCameraPixelSize(iCamNo).Width;
            int iCamHeight = CMainFrame.LWDicer.m_Vision.GetCameraPixelSize(iCamNo).Height;

            Rectangle ptRecSearch = new Rectangle(0, 0, iCamWidth, iCamHeight);
            Rectangle ptRecModel = new Rectangle(0, 0, 300, 300);
            Point ptPointTemp = new Point(0, 0);

            string strModel = CMainFrame.LWDicer.m_DataManager.m_ModelData.Name;
            CMainFrame.LWDicer.m_Vision.RegisterPatternMark(iCamNo, strModel, PATTERN_B, ref ptRecSearch, ref ptRecModel, ref ptPointTemp);
            
            if (iCamNo == PRE__CAM)
                CMainFrame.LWDicer.m_DataManager.m_ModelData.MacroPatternB = CMainFrame.LWDicer.m_Vision.GetSearchData(iCamNo, PATTERN_B);
            if (iCamNo == FINE_CAM)
                CMainFrame.LWDicer.m_DataManager.m_ModelData.MicroPatternB = CMainFrame.LWDicer.m_Vision.GetSearchData(iCamNo, PATTERN_B);


            //CMainFrame.LWDicer.m_DataManager.SaveModelList();
            CMainFrame.LWDicer.m_DataManager.ChangeModel();
        }

        private void btnMarkSearchA_Click(object sender, EventArgs e)
        {
            CResultData pResult = new CResultData();

            CMainFrame.LWDicer.m_Vision.RecognitionPatternMark(CMainFrame.LWDicer.m_Vision.m_iCurrentViewNum, PATTERN_A, out pResult);

            lblResult.Text = pResult.m_strResult;
        }

        private void btnMarkSearchB_Click(object sender, EventArgs e)
        {
            CResultData pResult = new CResultData();

            CMainFrame.LWDicer.m_Vision.RecognitionPatternMark(CMainFrame.LWDicer.m_Vision.m_iCurrentViewNum, PATTERN_B, out pResult);

            lblResult.Text = pResult.m_strResult;
        }
  
        private void btnShowModelA_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_Vision.DisplayPatternImage(CMainFrame.LWDicer.m_Vision.m_iCurrentViewNum, PATTERN_A, VisionView2.Handle);
        }

        private void btnShowModelB_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_Vision.DisplayPatternImage(CMainFrame.LWDicer.m_Vision.m_iCurrentViewNum, PATTERN_B, VisionView2.Handle);
        }

        private void btnSaveImage_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_Vision.SaveImage();
        }

        private void btnShowImage_Click(object sender, EventArgs e)
        {
            if (btnShowImage.Tag.Equals("1"))
            {
                btnShowImage.Tag = "2";
                CMainFrame.LWDicer.m_Vision.DisplayViewImage(CMainFrame.LWDicer.m_Vision.GetGrabImage(CMainFrame.LWDicer.m_Vision.m_iCurrentViewNum), View1.Handle);
                return;
            }
            if (btnShowImage.Tag.Equals("2"))
            {
                btnShowImage.Tag = "3";
                CMainFrame.LWDicer.m_Vision.DisplayViewImage(CMainFrame.LWDicer.m_Vision.GetGrabImage(CMainFrame.LWDicer.m_Vision.m_iCurrentViewNum), View2.Handle);
                return;
            }
            if (btnShowImage.Tag.Equals("3"))
            {
                btnShowImage.Tag = "4";
                CMainFrame.LWDicer.m_Vision.DisplayViewImage(CMainFrame.LWDicer.m_Vision.GetGrabImage(CMainFrame.LWDicer.m_Vision.m_iCurrentViewNum), View3.Handle);
                return;
            }
            if (btnShowImage.Tag.Equals("4"))
            {
                btnShowImage.Tag = "5";
                CMainFrame.LWDicer.m_Vision.DisplayViewImage(CMainFrame.LWDicer.m_Vision.GetGrabImage(CMainFrame.LWDicer.m_Vision.m_iCurrentViewNum), View4.Handle);
                return;
            }
            if (btnShowImage.Tag.Equals("5"))
            {
                btnShowImage.Tag = "1";
                CMainFrame.LWDicer.m_Vision.DisplayViewImage(CMainFrame.LWDicer.m_Vision.GetGrabImage(CMainFrame.LWDicer.m_Vision.m_iCurrentViewNum), View5.Handle);
                return;
            }
        }

        private void FormManualOP_Load(object sender, EventArgs e)
        {

        }

    }
}
