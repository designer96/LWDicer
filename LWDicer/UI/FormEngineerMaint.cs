using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LWDicer.Control;

namespace LWDicer.UI
{
    public partial class FormEngineerMaint : Form
    {
        private PageInfo PrevPage = null;
        private PageInfo AxisPage = null;
        private PageInfo IOPage = null;
        private BottomPageInfo NextBottomPage = null;

        public void SetPrevPage(PageInfo page)
        {
            PrevPage = page;
        }

        public void SetAxisOPPage(PageInfo page)
        {
            AxisPage = page;
        }

        public void SetIOPage(PageInfo page)
        {
            IOPage = page;

        }

        public void SetNextBottomPage(BottomPageInfo page)
        {
            NextBottomPage = page;
        }

        public FormEngineerMaint()
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

        private void BtnExit_Click(object sender, EventArgs e)
        {
            if (PrevPage == null)
            {
                return;
            }

            CMainFrame.MainFrame.MoveToPage(PrevPage);
            CMainFrame.MainFrame.MoveToBottomPage(NextBottomPage);
        }


        private void BtnAxisOP_Click(object sender, EventArgs e)
        {
            if (AxisPage == null)
            {
                return;
            }

            CMainFrame.MainFrame.MoveToPage(AxisPage);
        }

        private void BtnIOCheck_Click(object sender, EventArgs e)
        {
            if (IOPage == null)
            {
                return;
            }

            CMainFrame.MainFrame.MoveToPage(IOPage);
        }
    }
}
