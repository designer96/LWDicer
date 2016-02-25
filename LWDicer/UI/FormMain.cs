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
    public partial class FormMain : Form
    {
        private PageInfo FullAutoPage = null;
        private BottomPageInfo FullAutoBottomPage = null;

        private PageInfo ManualOPPage = null;
        private BottomPageInfo ManualOPBottomPage = null;

        private PageInfo DeviceDataPage = null;
        private BottomPageInfo DeviceDataBottomPage = null;

        private PageInfo LaserMaintPage = null;
        private BottomPageInfo LaserMaintBottomPage = null;

        private PageInfo OperatorMaintPage = null;
        private BottomPageInfo OperatorMaintBottomPage = null;

        private PageInfo MachineMaintPage = null;
        private BottomPageInfo MachineMaintBottomPage = null;

        private PageInfo EngineerMaintPage = null;
        private BottomPageInfo EngineerMaintBottomPage = null;


        public FormMain()
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

        public void SetFullAutoPage(PageInfo page, BottomPageInfo Bottompage)
        {
            FullAutoPage = page;
            FullAutoBottomPage = Bottompage;
        }

        public void SetManualOPPage(PageInfo page, BottomPageInfo Bottompage)
        {
            ManualOPPage = page;
            ManualOPBottomPage = Bottompage;
        }

        public void SetDeviceDataPage(PageInfo page, BottomPageInfo Bottompage)
        {
            DeviceDataPage = page;
            DeviceDataBottomPage = Bottompage;
        }

        public void SetOperatorMaintPage(PageInfo page, BottomPageInfo Bottompage)
        {
            OperatorMaintPage = page;
            OperatorMaintBottomPage = Bottompage;
        }
        public void SetMachineMaintPage(PageInfo page, BottomPageInfo Bottompage)
        {
            MachineMaintPage = page;
            MachineMaintBottomPage = Bottompage;
        }

        public void SetEngineerMaintPage(PageInfo page, BottomPageInfo Bottompage)
        {
            EngineerMaintPage = page;
            EngineerMaintBottomPage = Bottompage;
        }

        public void SetLaserMaintPage(PageInfo page, BottomPageInfo Bottompage)
        {
            LaserMaintPage = page;
            LaserMaintBottomPage = Bottompage;
        }

        private void BtnFullAuto_Click(object sender, EventArgs e)
        {
            CMainFrame.MainFrame.MoveToPage(FullAutoPage);
            CMainFrame.MainFrame.MoveToBottomPage(FullAutoBottomPage);
        }

        private void BtnManualOP_Click(object sender, EventArgs e)
        {
            CMainFrame.MainFrame.MoveToPage(ManualOPPage);
            CMainFrame.MainFrame.MoveToBottomPage(ManualOPBottomPage);
        }

        private void BtnDeviceData_Click(object sender, EventArgs e)
        {
            CMainFrame.MainFrame.MoveToPage(DeviceDataPage);
            CMainFrame.MainFrame.MoveToBottomPage(DeviceDataBottomPage);
        }

        private void BtnLaserMaint_Click(object sender, EventArgs e)
        {
            CMainFrame.MainFrame.MoveToPage(LaserMaintPage);
            CMainFrame.MainFrame.MoveToBottomPage(LaserMaintBottomPage);
        }

        private void BtnOPMaint_Click(object sender, EventArgs e)
        {
            CMainFrame.MainFrame.MoveToPage(OperatorMaintPage);
            CMainFrame.MainFrame.MoveToBottomPage(OperatorMaintBottomPage);
        }

        private void BtnMachineMaint_Click(object sender, EventArgs e)
        {
            CMainFrame.MainFrame.MoveToPage(MachineMaintPage);
            CMainFrame.MainFrame.MoveToBottomPage(MachineMaintBottomPage);
        }

        private void BtnEnginnerMaint_Click(object sender, EventArgs e)
        {
            CMainFrame.MainFrame.MoveToPage(EngineerMaintPage);
            CMainFrame.MainFrame.MoveToBottomPage(EngineerMaintBottomPage);
        }
    }
}
