using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LWDicer.Control;

using Syncfusion.Windows.Forms.Grid;
using Syncfusion.Windows.Forms;
using Syncfusion.Windows.Forms.Tools;

using Excel = Microsoft.Office.Interop.Excel;

using static LWDicer.Control.DEF_Yaskawa;

namespace LWDicer.UI
{
    public partial class FormAxisParameter : Form
    {
        private PageInfo PrevPage = null;

        public void SetPrevPage(PageInfo page)
        {
            PrevPage = page;
        }

        public FormAxisParameter()
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

            InitGrid();

            UpdateScreen();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            if (PrevPage == null)
            {
                return;
            }

            CMainFrame.MainFrame.MoveToPage(PrevPage);
        }

        private void InitGrid()
        {
            int i = 0, j = 0, nCol = 0, nRow = 0;

            // Cell Click 시 커서가 생성되지 않게함.
            GridMotorPara.ActivateCurrentCellBehavior = GridCellActivateAction.None;

            // Header
            GridMotorPara.Properties.RowHeaders = true;
            GridMotorPara.Properties.ColHeaders = true;

            nCol = 29;
            nRow = 19;

            // Column,Row 개수
            GridMotorPara.ColCount = nCol;
            GridMotorPara.RowCount = nRow;

            // Column 가로 크기설정
            for (i = 0; i < nCol + 1; i++)
            {
                GridMotorPara.ColWidths.SetSize(i, 120);
            }

            GridMotorPara.ColWidths.SetSize(0, 40);
            GridMotorPara.ColWidths.SetSize(1, 170);

            for (i = 0; i < nRow + 1; i++)
            {
                GridMotorPara.RowHeights[i] = 34;

            }

            for (i = 0; i < nRow; i++)
            {
                GridMotorPara[i + 1, 0].Text = string.Format("#{0:d}", i + 1);
            }


            // Text Display
            GridMotorPara[0, 0].Text = "No.";
            GridMotorPara[0, 1].Text = "Axis Name";
            GridMotorPara[0, 2].Text = "Manual Slow Vel";
            GridMotorPara[0, 3].Text = "Manual Fast Vel";
            GridMotorPara[0, 4].Text = "Auto Slow Vel";
            GridMotorPara[0, 5].Text = "Auto Fast Vel";
            GridMotorPara[0, 6].Text = "Jog Slow Vel";
            GridMotorPara[0, 7].Text = "Jog Fast Vel";
            GridMotorPara[0, 8].Text = "Manual Fast Acc";
            GridMotorPara[0, 9].Text = "Manual Slow Acc";
            GridMotorPara[0, 10].Text = "Auto Slow Acc";
            GridMotorPara[0, 11].Text = "Auto Fast Vcc";
            GridMotorPara[0, 12].Text = "Jog Slow Vcc";
            GridMotorPara[0, 13].Text = "Jog Fast Vcc";
            GridMotorPara[0, 14].Text = "Manual Fast Dec";
            GridMotorPara[0, 15].Text = "Manual Slow Dec";
            GridMotorPara[0, 16].Text = "Auto Slow Dec";
            GridMotorPara[0, 17].Text = "Auto Fast Dec";
            GridMotorPara[0, 18].Text = "Jog Slow Dec";
            GridMotorPara[0, 19].Text = "Jog Fast Dec";
            GridMotorPara[0, 20].Text = "S/W P Limit";
            GridMotorPara[0, 21].Text = "S/W N Limit";
            GridMotorPara[0, 22].Text = "Move Limit";
            GridMotorPara[0, 23].Text = "After Move";
            GridMotorPara[0, 24].Text = "Origin Limit";
            GridMotorPara[0, 25].Text = "Home Method";
            GridMotorPara[0, 26].Text = "Home Dir";
            GridMotorPara[0, 27].Text = "Home Fast Speed";
            GridMotorPara[0, 28].Text = "Home Slow Speed";
            GridMotorPara[0, 29].Text = "Home Offset";

            GridMotorPara[1, 1].Text = "LIFTER";
            GridMotorPara[2, 1].Text = "CLAMPER FEEDER";
            GridMotorPara[3, 1].Text = "CENTERING 1";
            GridMotorPara[4, 1].Text = "CENTERING 2";
            GridMotorPara[5, 1].Text = "CHUCK ROTATE[SC1]";
            GridMotorPara[6, 1].Text = "CLEANING NOZZLE[SC1]";
            GridMotorPara[7, 1].Text = "COATING NOZZLE[SC1]";
            GridMotorPara[8, 1].Text = "CHUCK ROTATE[SC2]";
            GridMotorPara[9, 1].Text = "CLEANING NOZZLE[SC2]";
            GridMotorPara[10, 1].Text = "COATING NOZZLE[SC2]";
            GridMotorPara[11, 1].Text = "TR HAND 1 Z";
            GridMotorPara[12, 1].Text = "TR HAND 1 Y";
            GridMotorPara[13, 1].Text = "TR HAND 2 Z";
            GridMotorPara[14, 1].Text = "TR HAND 2 Y";
            GridMotorPara[15, 1].Text = "STAGE Y";
            GridMotorPara[16, 1].Text = "STAGE X";
            GridMotorPara[17, 1].Text = "STAGE R";
            GridMotorPara[18, 1].Text = "SCANNER Z";
            GridMotorPara[19, 1].Text = "CAMERA Z";

            for (i = 0; i < nCol + 1; i++)
            {
                for (j = 0; j < nRow + 1; j++)
                {
                    // Font Style - Bold
                    GridMotorPara[j, i].Font.Bold = true;

                    GridMotorPara[j, i].VerticalAlignment = GridVerticalAlignment.Middle;
                    GridMotorPara[j, i].HorizontalAlignment = GridHorizontalAlignment.Center;
                }
            }

            for (i = 0; i < nRow; i++)
            {
                GridMotorPara[i + 1, 1].BackColor = Color.FromArgb(230, 210, 255);
            }

            GridMotorPara.GridVisualStyles = GridVisualStyles.Office2007Blue;
            GridMotorPara.ResizeColsBehavior = 0;
            GridMotorPara.ResizeRowsBehavior = 0;

            // Grid Display Update
            GridMotorPara.Refresh();
        }

        private void UpdateScreen()
        {
            int i = 0;

            for (i = 0; i < 19; i++)
            {
                // Speed
                GridMotorPara[i + 1, 2].Text = Convert.ToString(CMainFrame.LWDicer.m_DataManager.SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.MANUAL_SLOW].Vel);
                GridMotorPara[i + 1, 3].Text = Convert.ToString(CMainFrame.LWDicer.m_DataManager.SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.MANUAL_FAST].Vel);
                GridMotorPara[i + 1, 4].Text = Convert.ToString(CMainFrame.LWDicer.m_DataManager.SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.AUTO_SLOW].Vel);
                GridMotorPara[i + 1, 5].Text = Convert.ToString(CMainFrame.LWDicer.m_DataManager.SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.AUTO_FAST].Vel);
                GridMotorPara[i + 1, 6].Text = Convert.ToString(CMainFrame.LWDicer.m_DataManager.SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.JOG_SLOW].Vel);
                GridMotorPara[i + 1, 7].Text = Convert.ToString(CMainFrame.LWDicer.m_DataManager.SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.JOG_FAST].Vel);

                // Acc
                GridMotorPara[i + 1, 8].Text = Convert.ToString(CMainFrame.LWDicer.m_DataManager.SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.MANUAL_SLOW].Acc);
                GridMotorPara[i + 1, 9].Text = Convert.ToString(CMainFrame.LWDicer.m_DataManager.SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.MANUAL_FAST].Acc);
                GridMotorPara[i + 1, 10].Text = Convert.ToString(CMainFrame.LWDicer.m_DataManager.SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.AUTO_SLOW].Acc);
                GridMotorPara[i + 1, 11].Text = Convert.ToString(CMainFrame.LWDicer.m_DataManager.SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.AUTO_FAST].Acc);
                GridMotorPara[i + 1, 12].Text = Convert.ToString(CMainFrame.LWDicer.m_DataManager.SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.JOG_SLOW].Acc);
                GridMotorPara[i + 1, 13].Text = Convert.ToString(CMainFrame.LWDicer.m_DataManager.SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.JOG_FAST].Acc);

                // Dec
                GridMotorPara[i + 1, 14].Text = Convert.ToString(CMainFrame.LWDicer.m_DataManager.SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.MANUAL_SLOW].Dec);
                GridMotorPara[i + 1, 15].Text = Convert.ToString(CMainFrame.LWDicer.m_DataManager.SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.MANUAL_FAST].Dec);
                GridMotorPara[i + 1, 16].Text = Convert.ToString(CMainFrame.LWDicer.m_DataManager.SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.AUTO_SLOW].Dec);
                GridMotorPara[i + 1, 17].Text = Convert.ToString(CMainFrame.LWDicer.m_DataManager.SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.AUTO_FAST].Dec);
                GridMotorPara[i + 1, 18].Text = Convert.ToString(CMainFrame.LWDicer.m_DataManager.SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.JOG_SLOW].Dec);
                GridMotorPara[i + 1, 19].Text = Convert.ToString(CMainFrame.LWDicer.m_DataManager.SystemData_Axis.MPMotionData[i].Speed[(int)EMotorSpeed.JOG_FAST].Dec);

                // S/W Limit
                GridMotorPara[i + 1, 20].Text = Convert.ToString(CMainFrame.LWDicer.m_DataManager.SystemData_Axis.MPMotionData[i].PosLimit.Plus);
                GridMotorPara[i + 1, 21].Text = Convert.ToString(CMainFrame.LWDicer.m_DataManager.SystemData_Axis.MPMotionData[i].PosLimit.Minus);

                // Limit Time
                GridMotorPara[i + 1, 22].Text = Convert.ToString(CMainFrame.LWDicer.m_DataManager.SystemData_Axis.MPMotionData[i].TimeLimit.tMoveLimit);
                GridMotorPara[i + 1, 23].Text = Convert.ToString(CMainFrame.LWDicer.m_DataManager.SystemData_Axis.MPMotionData[i].TimeLimit.tSleepAfterMove);
                GridMotorPara[i + 1, 24].Text = Convert.ToString(CMainFrame.LWDicer.m_DataManager.SystemData_Axis.MPMotionData[i].TimeLimit.tOriginLimit);

                // Home Option
                GridMotorPara[i + 1, 25].Text = Convert.ToString(CMainFrame.LWDicer.m_DataManager.SystemData_Axis.MPMotionData[i].OriginData.Method);
                GridMotorPara[i + 1, 26].Text = Convert.ToString(CMainFrame.LWDicer.m_DataManager.SystemData_Axis.MPMotionData[i].OriginData.Dir);
                GridMotorPara[i + 1, 27].Text = Convert.ToString(CMainFrame.LWDicer.m_DataManager.SystemData_Axis.MPMotionData[i].OriginData.FastSpeed);
                GridMotorPara[i + 1, 28].Text = Convert.ToString(CMainFrame.LWDicer.m_DataManager.SystemData_Axis.MPMotionData[i].OriginData.SlowSpeed);
                GridMotorPara[i + 1, 29].Text = Convert.ToString(CMainFrame.LWDicer.m_DataManager.SystemData_Axis.MPMotionData[i].OriginData.HomeOffset);
            }
        }

        private void GridMotorPara_CellClick(object sender, GridCellClickEventArgs e)
        {
            int nCol = 0, nRow = 0;
            string StrCurrent = "", strModify = "";

            nCol = e.ColIndex;
            nRow = e.RowIndex;

            if (nCol == 0 || nCol == 1 || nRow == 0)
            {
                return;
            }

            StrCurrent = GridMotorPara[nRow, nCol].Text;

            if (!CMainFrame.LWDicer.GetKeyPad(StrCurrent, out strModify))
            {
                return;
            }

            GridMotorPara[nRow, nCol].Text = strModify;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            int i = 0, j = 0;
            string[,] strPara = new string[19, 28];

            if (!CMainFrame.LWDicer.DisplayMsg("Motor Data를 저장 하시겠습니까?"))
            {
                return;
            }

            for (i = 0; i < 19; i++)
            {
                for (j = 0; j < 28; j++)
                {
                    strPara[i, j] = GridMotorPara[i + 1, j + 2].Text;
                }
            }

            CMainFrame.LWDicer.m_DataManager.SaveExcelSystemData(strPara);

        }
    }
}
