using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using LWDicer.UI;
using LWDicer.Control;

namespace LWDicer.UI
{
    public partial class FormDeviceData : Form
    {
        private PageInfo PrevPage = null;
        private PageInfo NextPage = null;
        private BottomPageInfo NextBottomPage = null;

        // 12 Inch -> 3:7  300mm Wafer UI Pixel Size X : 700, Y : 700
        // 8  Inch -> 2:5  200mm Wafer UI Pixel Size X : 500, Y : 500
        private double RATIO_12INCH = 2.3333;
        private double RATIO_8INCH = 2.5;

        public Graphics m_Grapic;
        public Image Image;

        public bool bLayout;


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

        public FormDeviceData()
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

            bLayout = true;
            ShowLayout();

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

        private void PicWafer_MouseMove(object sender, MouseEventArgs e)
        {
            float fX = 0, fY = 0;
            int PicX = 0, PicY = 0;

            PicX = 20;
            PicY = 20;

            fX = (float)((e.X - PicX) / RATIO_12INCH);
            fY = (float)((e.Y - PicY) / RATIO_12INCH);

            PointXY.Text = string.Format("X : {0:f4}   Y : {1:f4}", fX, fY);
        }

        private void DrawLayout()
        {
            int PicX = 0, PicY = 0, nCount = 0, i = 0;
            double X1 = 0, Y1 = 0, X2 = 0, Y2 = 0, OffSetX = 0, OffSetY = 0;

            Image = new Bitmap(PicWafer.Width, PicWafer.Height);
            m_Grapic = Graphics.FromImage(Image);
            PicWafer.Image = Image;

            PicX = 20;
            PicY = 20;

            m_Grapic.DrawEllipse(Pens.Red, PicX, PicY, 700, 700);

            // 왼쪽 상단 코너
            m_Grapic.DrawLine(Pens.Red, PicX, PicY, PicX + 20, PicY);
            m_Grapic.DrawLine(Pens.Red, PicX, PicY, PicX, PicY + 20);

            // 왼쪽 하단 코너
            m_Grapic.DrawLine(Pens.Red, PicX, PicY + 680, PicX, PicY + 700);
            m_Grapic.DrawLine(Pens.Red, PicX, PicY + 700, PicX + 20, PicY + 700);

            // 오른쪽 상단 코너
            m_Grapic.DrawLine(Pens.Red, PicX + 680, PicY, PicX + 700, PicY);
            m_Grapic.DrawLine(Pens.Red, PicX + 700, PicY, PicX + 700, PicY + 20);

            // 오른쪽 하단 코너
            m_Grapic.DrawLine(Pens.Red, PicX + 680, PicY + 700, PicX + 700, PicY + 700);
            m_Grapic.DrawLine(Pens.Red, PicX + 700, PicY + 700, PicX + 700, PicY + 680);
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            ClearLayout();
        }

        private void ShowLayout()
        {
            if (bLayout == true)
            {
                BtnLayout.Image = ImagePolygon.Images[1];
                bLayout = false;

                LabelX.Show();
                LabelY.Show();
                PointXY.Show();

                DrawLayout();
            }
            else
            {
                BtnLayout.Image = ImagePolygon.Images[0];
                bLayout = true;

                LabelX.Hide();
                LabelY.Hide();
                PointXY.Hide();

                ClearLayout();
            }
        }

        private void ClearLayout()
        {
            Image = new Bitmap(PicWafer.Width, PicWafer.Height);
            m_Grapic = Graphics.FromImage(Image);
            PicWafer.Image = Image;

            m_Grapic.Clear(Color.White);
        }

        private void BtnLayout_Click(object sender, EventArgs e)
        {
            ShowLayout();
        }
    }
}
