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
            CEdgeData mEdgeData = new CEdgeData();

            CMainFrame.LWDicer.m_Vision.SetEdgeFinderArea(FINE_CAM);
            CMainFrame.LWDicer.m_Vision.FindEdge(FINE_CAM, ref mEdgeData);
        }

        private void btnSaveModelImage_Click(object sender, EventArgs e)
        {
            //CMainFrame.LWDicer.m_Vision.SaveImage(PRE__CAM, 1, 1);
            CMainFrame.LWDicer.m_Vision.SaveModelImage(FINE_CAM, 1);
        }

        private void btnMacroView_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_Vision.DestroyLocalView(FINE_CAM);
            CMainFrame.LWDicer.m_Vision.InitialLocalView(PRE__CAM, CMainFrame.MainFrame.m_FormManualOP.VisionView1.Handle);
        }

        private void btnMicroView_Click(object sender, EventArgs e)
        {
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

        private void btnMarkRegister_Click(object sender, EventArgs e)
        {
            Rectangle ptRecSearch = new Rectangle(0, 0, DEF_IMAGE_SIZE_X, DEF_IMAGE_SIZE_Y);
            Rectangle ptRecModel = new Rectangle(0, 0, 300,300);
            Point ptPointTemp = new Point(0, 0);

            CMainFrame.LWDicer.m_Vision.RegisterPatternMark(CMainFrame.LWDicer.m_Vision.m_iCurrentViewNum, 
                                                            1, ref ptRecSearch, ref ptRecModel, ref ptPointTemp);

        }

        private void btnMarkSearch_Click(object sender, EventArgs e)
        {
            CResultData pResult = new CResultData();

            CMainFrame.LWDicer.m_Vision.RecognitionPatternMark(CMainFrame.LWDicer.m_Vision.m_iCurrentViewNum, 1, out pResult);

            lblResult.Text = pResult.m_strResult;
        }

        private void btnShowModel_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_Vision.DisplayPatternImage(FINE_CAM, 1, VisionView2.Handle);
        }

        private void btnSaveImage_Click(object sender, EventArgs e)
        {
            CMainFrame.LWDicer.m_Vision.SaveImage();
        }
    }
}
