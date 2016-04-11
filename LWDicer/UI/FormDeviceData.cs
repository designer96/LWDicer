using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Syncfusion.Windows.Forms.Grid;
using Syncfusion.Windows.Forms;

using LWDicer.UI;
using LWDicer.Control;

using static LWDicer.Control.DEF_PolygonScanner;

namespace LWDicer.UI
{
    public partial class FormDeviceData : Form
    {
        Point MousePoint;

        private PageInfo PrevPage = null;
        private PageInfo NextPage = null;
        private BottomPageInfo NextBottomPage = null;
        private BottomPageInfo PrevBottomPage = null;

        // 12 Inch -> 3:7  300mm Wafer UI Pixel Size X : 700, Y : 700
        // 8  Inch -> 2:5  200mm Wafer UI Pixel Size X : 500, Y : 500
        public double RATIO_12INCH = 2.3333;
        public double RATIO_8INCH = 2.5;

        public int SIZE_12INCH = 300;
        public int SIZE_8INCH  = 200;

        public int SHAPE_ROUND  = 0;
        public int SHAPE_SQUARE = 1;

        public float fPitch = 0;
        public float fOffsetX = 0;
        public float fOffsetY = 0;
        public int nShape = 0;
        public int nWaferSize = -1;
        public int nLineCount = 0;
        
        public Graphics m_Grapic;
        public Image Image;

        public LineData m_CutData = new LineData();


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

        public void SetPrevBottomPage(BottomPageInfo page)
        {
            PrevBottomPage = page;
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

            Image = new Bitmap(PicWafer.Width, PicWafer.Height);
            m_Grapic = Graphics.FromImage(Image);
            PicWafer.Image = Image;

            InitGrid();

            ClearPicture();
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
            CMainFrame.MainFrame.MoveToBottomPage(NextBottomPage);
        }

        private void PicWafer_MouseMove(object sender, MouseEventArgs e)
        {
            float fX = 0, fY = 0;
            int PicX = 0, PicY = 0;

            if (e.Button == MouseButtons.Left)
            {
                Graphics g = PicWafer.CreateGraphics();

                g.DrawLine(Pens.DarkBlue, MousePoint.X, MousePoint.Y, e.X, e.Y);

                MousePoint.X = e.X; MousePoint.Y = e.Y;
                g.Dispose();
            }

            if (nWaferSize == SIZE_12INCH)
            {
                PicX = 20;
                PicY = 20;

                fX = (float)((e.X - PicX) / RATIO_12INCH);
                fY = (float)((e.Y - PicY) / RATIO_12INCH);
            }

            if (nWaferSize == SIZE_8INCH)
            {
                PicX = 120;
                PicY = 120;

                fX = (float)((e.X - PicX) / RATIO_8INCH);
                fY = (float)((e.Y - PicY) / RATIO_8INCH);
            }

            PointXY.Text = string.Format("X : {0:f4}   Y : {1:f4}", fX, fY);
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            ClearPicture();
        }

        public void ShowLayout(bool bShow)
        {
            if (bShow == true)
            {
                LabelX.Show();
                LabelY.Show();
                PointXY.Show();

                DrawLayout();
            }
            else
            {
                LabelX.Hide();
                LabelY.Hide();
                PointXY.Hide();

                ClearLayout();
            }
        }

        public void DrawLayout()
        {
            int PicX = 0, PicY = 0;

            if (nWaferSize == SIZE_12INCH)
            {
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

            if (nWaferSize == SIZE_8INCH)
            {
                PicX = 120;
                PicY = 120;

                m_Grapic.DrawEllipse(Pens.Red, PicX, PicY, 500, 500);

                // 왼쪽 상단 코너
                m_Grapic.DrawLine(Pens.Red, PicX, PicY, PicX + 20, PicY);
                m_Grapic.DrawLine(Pens.Red, PicX, PicY, PicX, PicY + 20);

                // 왼쪽 하단 코너
                m_Grapic.DrawLine(Pens.Red, PicX, PicY + 480, PicX, PicY + 500);
                m_Grapic.DrawLine(Pens.Red, PicX, PicY + 500, PicX + 20, PicY + 500);

                // 오른쪽 상단 코너
                m_Grapic.DrawLine(Pens.Red, PicX + 480, PicY, PicX + 500, PicY);
                m_Grapic.DrawLine(Pens.Red, PicX + 500, PicY, PicX + 500, PicY + 20);

                // 오른쪽 하단 코너
                m_Grapic.DrawLine(Pens.Red, PicX + 480, PicY + 500, PicX + 500, PicY + 500);
                m_Grapic.DrawLine(Pens.Red, PicX + 500, PicY + 500, PicX + 500, PicY + 480);
            }

            PicWafer.Refresh();

        }

        public void ClearLayout()
        {
            int PicX = 0, PicY = 0;

            if (nWaferSize == SIZE_12INCH)
            {
                // 300 mm
                PicX = 20;
                PicY = 20;

                m_Grapic.DrawEllipse(Pens.White, PicX, PicY, 700, 700);

                // 왼쪽 상단 코너
                m_Grapic.DrawLine(Pens.White, PicX, PicY, PicX + 20, PicY);
                m_Grapic.DrawLine(Pens.White, PicX, PicY, PicX, PicY + 20);

                // 왼쪽 하단 코너
                m_Grapic.DrawLine(Pens.White, PicX, PicY + 680, PicX, PicY + 700);
                m_Grapic.DrawLine(Pens.White, PicX, PicY + 700, PicX + 20, PicY + 700);

                // 오른쪽 상단 코너
                m_Grapic.DrawLine(Pens.White, PicX + 680, PicY, PicX + 700, PicY);
                m_Grapic.DrawLine(Pens.White, PicX + 700, PicY, PicX + 700, PicY + 20);

                // 오른쪽 하단 코너
                m_Grapic.DrawLine(Pens.White, PicX + 680, PicY + 700, PicX + 700, PicY + 700);
                m_Grapic.DrawLine(Pens.White, PicX + 700, PicY + 700, PicX + 700, PicY + 680);
            }

            if (nWaferSize == SIZE_8INCH)
            {
                // 200 mm
                PicX = 120;
                PicY = 120;

                m_Grapic.DrawEllipse(Pens.White, PicX, PicY, 500, 500);

                // 왼쪽 상단 코너
                m_Grapic.DrawLine(Pens.White, PicX, PicY, PicX + 20, PicY);
                m_Grapic.DrawLine(Pens.White, PicX, PicY, PicX, PicY + 20);

                // 왼쪽 하단 코너
                m_Grapic.DrawLine(Pens.White, PicX, PicY + 480, PicX, PicY + 500);
                m_Grapic.DrawLine(Pens.White, PicX, PicY + 500, PicX + 20, PicY + 500);

                // 오른쪽 상단 코너
                m_Grapic.DrawLine(Pens.White, PicX + 480, PicY, PicX + 500, PicY);
                m_Grapic.DrawLine(Pens.White, PicX + 500, PicY, PicX + 500, PicY + 20);

                // 오른쪽 하단 코너
                m_Grapic.DrawLine(Pens.White, PicX + 480, PicY + 500, PicX + 500, PicY + 500);
                m_Grapic.DrawLine(Pens.White, PicX + 500, PicY + 500, PicX + 500, PicY + 480);
            }

            PicWafer.Refresh();
        }


        public void ClearPicture()
        {
            LabelX.Hide();
            LabelY.Hide();
            PointXY.Hide();

            m_Grapic.Clear(Color.White);

            PicWafer.Refresh();
        }


        private void GridDeviceData_CellClick(object sender, Syncfusion.Windows.Forms.Grid.GridCellClickEventArgs e)
        {
            int nCol = 0, nRow = 0;
            string StrCurrent = "", strModify = "";
            nCol = e.ColIndex;
            nRow = e.RowIndex;

            if (nCol == 0 || nRow == 0)
            {
                return;
            }

            StrCurrent = GridCutLine[nRow, nCol].Text;

            if (!CMainFrame.LWDicer.GetKeyPad(StrCurrent, out strModify))
            {
                return;
            }

            GridCutLine[nRow, nCol].Text = strModify;
        }

        private void InitGrid()
        {
            int i = 0, j = 0;

            // Cell Click 시 커서가 생성되지 않게함.
            GridCutLine.ActivateCurrentCellBehavior = GridCellActivateAction.None;

            // Header
            GridCutLine.Properties.RowHeaders = true;
            GridCutLine.Properties.ColHeaders = true;

            // Column,Row 개수
            GridCutLine.ColCount = 5;
            GridCutLine.RowCount = 0;

            // Column 가로 크기설정
            GridCutLine.ColWidths.SetSize(0, 40);
            GridCutLine.ColWidths.SetSize(1, 60);
            GridCutLine.ColWidths.SetSize(2, 60);
            GridCutLine.ColWidths.SetSize(3, 60);
            GridCutLine.ColWidths.SetSize(4, 60);
            GridCutLine.ColWidths.SetSize(5, 17);


            // Text Display
            GridCutLine[0, 0].Text = "Line";
            GridCutLine[0, 1].Text = "X1";
            GridCutLine[0, 2].Text = "Y1";
            GridCutLine[0, 3].Text = "X2";
            GridCutLine[0, 4].Text = "Y2";
            GridCutLine[0, 5].Text = "-";

            GridCutLine.GridVisualStyles = GridVisualStyles.Office2007Blue;
            GridCutLine.ResizeColsBehavior = 0;
            GridCutLine.ResizeRowsBehavior = 0;

            // Grid Display Update
            GridCutLine.Refresh();
        }

        private void LabelPitch_Click(object sender, EventArgs e)
        {
            int nPitch = 0;

            string StrCurrent = "", strModify = "";

            StrCurrent = LabelPitch.Text;

            if (!CMainFrame.LWDicer.GetKeyPad(StrCurrent, out strModify))
            {
                return;
            }

            LabelPitch.Text = strModify;

            fPitch = Convert.ToSingle(strModify);
        }


        public void DrawCutLine(LineData CutLineData)
        {
            int i = 0;

            double dPicX1 = 0, dPicY1 = 0, dPicX2 = 0, dPicY2 = 0;

            float PicX = 0;
            float PicY = 0;

            ClearLayout();

            if (nWaferSize == SIZE_12INCH)
            {
                PicX = 20;
                PicY = 20;

                for (i = 0; i < nLineCount; i++)
                {
                    dPicX1 = (CutLineData.fLineData[i, 0] * RATIO_12INCH) + PicX;
                    dPicY1 = (CutLineData.fLineData[i, 1] * RATIO_12INCH) + PicY;
                    dPicX2 = (CutLineData.fLineData[i, 2] * RATIO_12INCH) + PicX;
                    dPicY2 = (CutLineData.fLineData[i, 3] * RATIO_12INCH) + PicY;

                    SetDrawLine(dPicX1, dPicY1, dPicX2, dPicY2, 1, Color.Black);
                }
            }

            if (nWaferSize == SIZE_8INCH)
            {
                PicX = 120;
                PicY = 120;

                for (i = 0; i < nLineCount; i++)
                {
                    dPicX1 = (CutLineData.fLineData[i, 0] * RATIO_8INCH) + PicX;
                    dPicY1 = (CutLineData.fLineData[i, 1] * RATIO_8INCH) + PicY;
                    dPicX2 = (CutLineData.fLineData[i, 2] * RATIO_8INCH) + PicX;
                    dPicY2 = (CutLineData.fLineData[i, 3] * RATIO_8INCH) + PicY;

                    SetDrawLine(dPicX1, dPicY1, dPicX2, dPicY2, 1, Color.Black);
                }
            }
        }

        public void UpdateCutLine(int nShape, int nSize)
        {
            int i = 0, j = 0;
            double X1 = 0.0, X2 = 0.0, dPitch = 0.0;
            double dA = 0.0, dB = 0.0, dSum = 0.0;

            if (nSize == SIZE_12INCH)
            {
                dPitch = fPitch;

                nLineCount = (int)(SIZE_12INCH / dPitch);

                if (nShape == SHAPE_ROUND)
                {
                    for (i = 0; i < nLineCount; i++)
                    {
                        dA = Math.Pow((nWaferSize / 2) - dPitch, 2);
                        dB = Math.Pow((nWaferSize / 2), 2);
                        dSum = dB - dA;

                        X1 = (nSize / 2) - Math.Sqrt(dSum); // X1
                        X2 = ((Math.Sqrt(dSum)) * 2) + X1;  // X2

                        m_CutData.fLineData[i, 0] = Convert.ToSingle(string.Format("{0:f4}", X1 - fOffsetX));
                        m_CutData.fLineData[i, 1] = Convert.ToSingle(string.Format("{0:f4}", dPitch + fOffsetY)); // Y1
                        m_CutData.fLineData[i, 2] = Convert.ToSingle(string.Format("{0:f4}", X2 + fOffsetX));
                        m_CutData.fLineData[i, 3] = Convert.ToSingle(string.Format("{0:f4}", dPitch + fOffsetY)); // Y2

                        dPitch = dPitch + fPitch;
                    }
                }

                if (nShape == SHAPE_SQUARE)
                {
                    for (i = 0; i < nLineCount; i++)
                    {
                        m_CutData.fLineData[i, 0] = Convert.ToSingle(string.Format("{0:f4}", 0 - fOffsetX));
                        m_CutData.fLineData[i, 1] = Convert.ToSingle(string.Format("{0:f4}", dPitch + fOffsetY)); // Y1
                        m_CutData.fLineData[i, 2] = Convert.ToSingle(string.Format("{0:f4}", nSize + fOffsetX));
                        m_CutData.fLineData[i, 3] = Convert.ToSingle(string.Format("{0:f4}", dPitch + fOffsetY)); // Y2

                        dPitch = dPitch + fPitch;
                    }
                }
            }

            if (nSize == SIZE_8INCH)
            {
                dPitch = fPitch;

                nLineCount = (int)(SIZE_8INCH / dPitch);

                if (nShape == SHAPE_ROUND)
                {

                    for (i = 0; i < nLineCount; i++)
                    {
                        dA = Math.Pow((nWaferSize / 2) - dPitch, 2);
                        dB = Math.Pow((nWaferSize / 2), 2);
                        dSum = dB - dA;

                        X1 = (nSize / 2) - Math.Sqrt(dSum); // X1
                        X2 = ((Math.Sqrt(dSum)) * 2) + X1;  // X2

                        m_CutData.fLineData[i, 0] = Convert.ToSingle(string.Format("{0:f4}", X1 - fOffsetX));
                        m_CutData.fLineData[i, 1] = Convert.ToSingle(string.Format("{0:f4}", dPitch + fOffsetY)); // Y1
                        m_CutData.fLineData[i, 2] = Convert.ToSingle(string.Format("{0:f4}", X2 + fOffsetX));
                        m_CutData.fLineData[i, 3] = Convert.ToSingle(string.Format("{0:f4}", dPitch + fOffsetY)); // Y2

                        dPitch = dPitch + fPitch;
                    }
                }

                if (nShape == SHAPE_SQUARE)
                {
                    for (i = 0; i < nLineCount; i++)
                    {
                        m_CutData.fLineData[i, 0] = Convert.ToSingle(string.Format("{0:f4}", 0 - fOffsetX));
                        m_CutData.fLineData[i, 1] = Convert.ToSingle(string.Format("{0:f4}", dPitch + fOffsetY)); // Y1
                        m_CutData.fLineData[i, 2] = Convert.ToSingle(string.Format("{0:f4}", nSize + fOffsetX));
                        m_CutData.fLineData[i, 3] = Convert.ToSingle(string.Format("{0:f4}", dPitch + fOffsetY)); // Y2

                        dPitch = dPitch + fPitch;
                    }
                }
            }

            GridCutLine.RowCount = nLineCount + 1;

            for (i = 0; i < 5; i++)
            {
                for (j = 0; j < nLineCount + 1; j++)
                {
                    if (j == nLineCount + 1)
                    {
                        continue;
                    }

                    // Font Style - Bold
                    GridCutLine[j, 0].Font.Bold = true;
                    GridCutLine[0, i].Font.Bold = true;

                    GridCutLine[j, i].VerticalAlignment = GridVerticalAlignment.Middle;
                    GridCutLine[j, i].HorizontalAlignment = GridHorizontalAlignment.Center;
                }
            }

            for (i = 0; i < nLineCount + 1; i++)
            {
                if (i == nLineCount + 1)
                {
                    continue;
                }

                GridCutLine[i + 1, 1].BackColor = Color.FromArgb(255, 230, 255);
                GridCutLine[i + 1, 2].BackColor = Color.FromArgb(255, 230, 255);
                GridCutLine[i + 1, 3].BackColor = Color.FromArgb(230, 210, 255);
                GridCutLine[i + 1, 4].BackColor = Color.FromArgb(230, 210, 255);

                GridCutLine[i + 1, 1].Text = Convert.ToString(m_CutData.fLineData[i, 0]);
                GridCutLine[i + 1, 2].Text = Convert.ToString(m_CutData.fLineData[i, 1]);
                GridCutLine[i + 1, 3].Text = Convert.ToString(m_CutData.fLineData[i, 2]);
                GridCutLine[i + 1, 4].Text = Convert.ToString(m_CutData.fLineData[i, 3]);

                GridCutLine[i + 1, 5].CellType = GridCellTypeName.CheckBox;
                GridCutLine[i + 1, 5].CheckBoxOptions = new GridCheckBoxCellInfo("True", "False", "", true);
            }
        }

        public void SetDrawLine(double X1, double Y1, double X2, double Y2, float Width, Color LineColor)
        {
            Pen m_Pen = new Pen(LineColor, Width);

            m_Grapic.DrawLine(m_Pen, (float)X1, (float)Y1, (float)X2, (float)Y2);

            PicWafer.Refresh();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {



        }

        private void BtnInch_Click(object sender, EventArgs e)
        {
            string strText = "";
            Button Btn = sender as Button;

            strText = Btn.Text;

            SelectInch(strText);
        }

        private void SelectInch(string strSize)
        {
            if (strSize == "8 Inch")
            {
                Btn12Inch.Image = ImagePolygon.Images[0];
                Btn8Inch.Image = ImagePolygon.Images[1];
                nWaferSize = SIZE_8INCH;
            }

            if (strSize == "12 Inch")
            {
                Btn8Inch.Image = ImagePolygon.Images[0];
                Btn12Inch.Image = ImagePolygon.Images[1];
                nWaferSize = SIZE_12INCH;
            }
        }

        private void SelectShape(int nShape)
        {
            if(nShape == SHAPE_ROUND)
            {
                BtnCycle.BackColor = Color.LightPink;
                BtnSquare.BackColor = Color.Transparent;
            }

            if(nShape == SHAPE_SQUARE)
            {
                BtnCycle.BackColor = Color.Transparent;
                BtnSquare.BackColor = Color.LightPink;
            }
        }


        private void BtnShape_Click(object sender, EventArgs e)
        {
            Button Btn = sender as Button;

            if (Btn.Name == "BtnCycle")
            {
                nShape = SHAPE_ROUND;
                SelectShape(nShape);
            }

            if (Btn.Name == "BtnSquare")
            {
                nShape = SHAPE_SQUARE;
                SelectShape(nShape);
            }

            if (nWaferSize == SIZE_12INCH)
            {
                UpdateCutLine(nShape, SIZE_12INCH);
            }

            if (nWaferSize == SIZE_8INCH)
            {
                UpdateCutLine(nShape, SIZE_8INCH);
            }
        }

        private void LabelOffSetX_Click(object sender, EventArgs e)
        {
            int nPitch = 0;

            string StrCurrent = "", strModify = "";

            StrCurrent = LabelOffSetX.Text;

            if (!CMainFrame.LWDicer.GetKeyPad(StrCurrent, out strModify))
            {
                return;
            }

            LabelOffSetX.Text = strModify;
            fOffsetX = Convert.ToSingle(strModify);
        }

        private void LabelOffSetY_Click(object sender, EventArgs e)
        {
            int nPitch = 0;

            string StrCurrent = "", strModify = "";

            StrCurrent = LabelOffSetY.Text;

            if (!CMainFrame.LWDicer.GetKeyPad(StrCurrent, out strModify))
            {
                return;
            }

            LabelOffSetY.Text = strModify;
            fOffsetY = Convert.ToSingle(strModify);
        }

        private void PicWafer_MouseDown(object sender, MouseEventArgs e)
        {
            MousePoint.X = e.X;
            MousePoint.Y = e.Y;
        }

        private void GridCutLine_CheckBoxClick(object sender, GridCellClickEventArgs e)
        {
            int nRow = 0;

            nRow = e.RowIndex;

            float fX1 = 0, fY1 = 0, fX2 = 0, fY2 = 0, PicX = 0, PicY = 0;

            if (nRow == 0)
            {
                return;
            }

            if (nWaferSize == SIZE_12INCH)
            {
                PicX = 20;
                PicY = 20;

                fX1 = Convert.ToSingle(GridCutLine[nRow, 1].Text);
                fX1 = (float)(fX1 * RATIO_12INCH) + PicX;

                fY1 = Convert.ToSingle(GridCutLine[nRow, 2].Text);
                fY1 = (float)(fY1 * RATIO_12INCH) + PicY;

                fX2 = Convert.ToSingle(GridCutLine[nRow, 3].Text);
                fX2 = (float)(fX2 * RATIO_12INCH) + PicX;

                fY2 = Convert.ToSingle(GridCutLine[nRow, 4].Text);
                fY2 = (float)(fY2 * RATIO_12INCH) + PicY;
            }

            if (nWaferSize == SIZE_8INCH)
            {
                PicX = 120;
                PicY = 120;

                fX1 = Convert.ToSingle(GridCutLine[nRow, 1].Text);
                fX1 = (float)(fX1 * RATIO_8INCH) + PicX;

                fY1 = Convert.ToSingle(GridCutLine[nRow, 2].Text);
                fY1 = (float)(fY1 * RATIO_8INCH) + PicY;

                fX2 = Convert.ToSingle(GridCutLine[nRow, 3].Text);
                fX2 = (float)(fX2 * RATIO_8INCH) + PicX;

                fY2 = Convert.ToSingle(GridCutLine[nRow, 4].Text);
                fY2 = (float)(fY2 * RATIO_8INCH) + PicY;
            }


            if (GridCutLine[nRow, 5].CheckBoxOptions.FlatLook == true)
            {
                GridCutLine[nRow, 5].CheckBoxOptions.FlatLook = false;

                SetDrawLine(fX1,fY1,fX2,fY2,1,Color.White);
            }
            else
            {
                GridCutLine[nRow, 5].CheckBoxOptions.FlatLook = true;

                SetDrawLine(fX1, fY1, fX2, fY2, 1, Color.Black);
            }
        }

        public void ImageSave(string strPath)
        {
            Bitmap bmp = new Bitmap(PicWafer.Width, PicWafer.Height);
            PicWafer.DrawToBitmap(bmp, new Rectangle(0, 0, PicWafer.Width, PicWafer.Height));

            // 흑백색으로 구성된 단색 Bitmap 형식으로 변환해야함 [Scanner에서 단색 비트맵 인식]
            // BMP 파일 비트 수준 : 1
            // 1. 단색 비트맵을 저장을 위한 Bitmap 생성
            Bitmap SaveImage = new Bitmap(PicWafer.Width, PicWafer.Height, PixelFormat.Format1bppIndexed);

            // 2. 사용자가 입력한 Image Size에 해당하는 복사본을 만들위한 Rectangle 생성
            Rectangle rectangle = new Rectangle(0, 0, PicWafer.Width, PicWafer.Height);

            // 3. 원본 이미지에 단색 Bitmap 속성을 바꾼 복사본을 만든다.
            SaveImage = bmp.Clone(rectangle, PixelFormat.Format1bppIndexed);

            SaveImage.Save(strPath);
        }
    }
}
