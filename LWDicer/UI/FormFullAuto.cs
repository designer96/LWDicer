using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LWDicer.Control;

using System.Diagnostics;
using System.Threading;
using System.Configuration;

using static LWDicer.Control.DEF_Thread;
using static LWDicer.Control.DEF_Thread.EThreadMessage;
using static LWDicer.Control.DEF_Thread.EWindowMessage;

using static LWDicer.Control.DEF_Common;
using static LWDicer.Control.DEF_Cylinder;
using static LWDicer.Control.DEF_SerialPort;

namespace LWDicer.UI
{
    public partial class FormFullAuto : Form
    {
        private PageInfo PrevPage = null;
        private PageInfo NextPage = null;
        private BottomPageInfo NextBottomPage = null;

        MTrsLoader worker1;
        MTrsPushPull worker2;
        MTrsStage1 worker3;

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

        public FormFullAuto()
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

        private void BtnNext_Click(object sender, EventArgs e)
        {
            if (NextPage == null)
            {
                return;
            }

            CMainFrame.MainFrame.MoveToPage(NextPage);
        }

        public void ProcessMsg(MEvent evnt)
        {
            string msg = "Get Message from Control : " + evnt;
            Debug.WriteLine("===================================================");
            Debug.WriteLine(msg);
            textBox2.Text = evnt.ToString();
            switch (evnt.Msg)
            {
                case (int)WM_START_MANUAL_MSG:
                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("[Hello-------------]");

            int nSleep = 10;
            int nParam = 0;
            //MCmdTarget cmd = new MCmdTarget(0, "Test", 1000);
            //             cmd.SendMsg(DEF_EventMsg.MSG_PROCESS_ALARM, nParam++, nParam++);
            //             Thread.Sleep(nSleep);
            //             cmd.PostMsg(DEF_EventMsg.MSG_PROCESS_ALARM, nParam++, nParam++);
            //             Thread.Sleep(nSleep);
            //             cmd.PostMsg(DEF_EventMsg.MSG_PROCESS_ALARM, nParam++, nParam++);
            //             Thread.Sleep(nSleep);
            //             cmd.PostMsg(DEF_EventMsg.MSG_PROCESS_ALARM, nParam++, nParam++);
            //             Thread.Sleep(nSleep);
            //             cmd.PostMsg(DEF_EventMsg.MSG_PROCESS_ALARM, nParam++, nParam++);
            //             Thread.Sleep(nSleep);
            // 
            //             cmd.CheckMsg();
            //             Thread.Sleep(nSleep);

            //CMainFrame.LWDicer.Initialize(CMainFrame.MainFrame);

            worker1 = CMainFrame.m_LWDicer.m_trsLoader;
            worker2 = CMainFrame.m_LWDicer.m_trsPushPull;
            worker3 = CMainFrame.m_LWDicer.m_trsStage1;

            worker1.PostMsg(TrsStage1, 912);
            Thread.Sleep(nSleep);
            worker1.PostMsg(TrsStage2, 913);
            Thread.Sleep(nSleep);

            worker2.PostMsg(TrsAutoManager, 921);
            Thread.Sleep(nSleep);
            worker2.PostMsg(TrsStage2, 923);
            Thread.Sleep(nSleep);

            worker3.PostMsg(TrsAutoManager, 931);
            Thread.Sleep(nSleep);
            worker3.PostMsg(TrsStage1, 932);
            Thread.Sleep(nSleep);

            worker1.PostMsg(TrsStage1, (int)MSG_MANUAL_CMD);
            Thread.Sleep(nSleep);
            worker1.PostMsg(TrsStage1, (int)MSG_START_RUN_CMD);
            Thread.Sleep(nSleep);
            worker1.PostMsg(TrsStage1, (int)MSG_START_CMD);
            Thread.Sleep(nSleep);
            worker1.PostMsg(TrsStage1, (int)MSG_ERROR_STOP_CMD);
            Thread.Sleep(1000);

            Debug.WriteLine("=================================");
            worker1.BroadcastMsg((int)MSG_STEP_STOP_CMD);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int msg;
            bool rtn = Int32.TryParse(textBox1.Text, out msg);
            if (rtn)
            {
                worker1?.SendMessageToMainWnd(msg);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            worker1?.WriteLog(textBox3.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            CMainFrame.m_LWDicer.m_DataManager.SaveSystemData();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            CMainFrame.m_LWDicer.m_DataManager.LoadSystemData();

        }

        private void button6_Click(object sender, EventArgs e)
        {
            //LWDicer.m_DataManager.DropTables(true);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            CLoginData login = new CLoginData();
            login.Type = ELoginType.OPERATOR;
            login.Number = DateTime.Now.Second.ToString();
            CMainFrame.m_LWDicer.m_DataManager.SetLogin(login);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            CLoginData login = new CLoginData();
            login.Type = ELoginType.MAKER;
            login.Number = DateTime.Now.Second.ToString();
            CMainFrame.m_LWDicer.m_DataManager.SetLogin(login);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string str = ConfigurationManager.AppSettings["AppFilePath"];
            MessageBox.Show(str);
        }
    }
}
