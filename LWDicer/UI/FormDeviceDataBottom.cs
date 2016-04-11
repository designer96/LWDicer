using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LWDicer.Control;

using static LWDicer.Control.DEF_Common;

namespace LWDicer.UI
{
    public partial class FormDeviceDataBottom : Form
    {
        public bool bLayout;
        private CDBInfo m_DBInfo;

        public FormDeviceDataBottom()
        {
            InitializeComponent();

            InitializeForm();

            m_DBInfo = new CDBInfo();
        }
        protected virtual void InitializeForm()
        {
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.DesktopLocation = new Point(DEF_UI.BOT_POS_X, DEF_UI.BOT_POS_Y);
            this.Size = new Size(DEF_UI.BOT_SIZE_WIDTH, DEF_UI.BOT_SIZE_HEIGHT);
            this.FormBorderStyle = FormBorderStyle.None;

            bLayout = true;
            BtnLayout.Image = ImagePolygon.Images[0];
        }

        private void BtnDraw_Click(object sender, EventArgs e)
        {
            CMainFrame.MainFrame.m_FormDeviceData.DrawCutLine(CMainFrame.MainFrame.m_FormDeviceData.m_CutData);
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            CMainFrame.MainFrame.m_FormDeviceData.ClearPicture();
        }

        private void BtnLayout_Click(object sender, EventArgs e)
        {
            if (bLayout == true)
            {
                BtnLayout.Image = ImagePolygon.Images[1];
                bLayout = false;

                CMainFrame.MainFrame.m_FormDeviceData.ShowLayout(true);
            }
            else
            {
                BtnLayout.Image = ImagePolygon.Images[0];
                bLayout = true;

                CMainFrame.MainFrame.m_FormDeviceData.ShowLayout(false);
            }
        }

        private void BtnImageSave_Click(object sender, EventArgs e)
        {
            string strFile = "";

            SaveFileDialog SaveImageFile = new SaveFileDialog();

            SaveImageFile.Title = "이미지 파일저장";
            SaveImageFile.OverwritePrompt = true;
            SaveImageFile.Filter = "Bitmap Image|*.bmp";

            SaveImageFile.InitialDirectory = m_DBInfo.ScannerLogDir;
            SaveImageFile.RestoreDirectory = true;

            DialogResult result = SaveImageFile.ShowDialog();

            if (result == DialogResult.OK)
            {
                strFile = SaveImageFile.FileName;
                CMainFrame.MainFrame.m_FormDeviceData.ImageSave(strFile);
            }
        }
    }
}
