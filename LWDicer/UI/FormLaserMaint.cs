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

using static LWDicer.Control.DEF_PolygonScanner;

namespace LWDicer.UI
{
    public partial class FormLaserMaint : Form
    {
        private PageInfo PrevPage = null;
        private PageInfo NextPage = null;
        private BottomPageInfo NextBottomPage = null;

        private CPolygonIni m_Polygon = null;

        private int scannerIndex;

        public string strBMPFileName;
        public string strCFGFileName;

        Queue<string> m_ReceivedQueue = new Queue<string>();

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

        public FormLaserMaint()
        {
            InitializeComponent();

            scannerIndex = 0;

            InitTabControl();

            InitializeForm();

            TmrScannerTest.Enabled = true;
            TmrScannerTest.Interval = 100;
            TmrScannerTest.Stop();
        }
        protected virtual void InitializeForm()
        {
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.DesktopLocation = new Point(DEF_UI.MAIN_POS_X, DEF_UI.MAIN_POS_Y);
            this.Size = new Size(DEF_UI.MAIN_SIZE_WIDTH, DEF_UI.MAIN_SIZE_HEIGHT);
            this.FormBorderStyle = FormBorderStyle.None;
        }

        private void InitTabControl()
        {
            TabCtlLaserMaint.TabStyle = typeof(Syncfusion.Windows.Forms.Tools.TabRendererVS2008);

            TabCtlLaserMaint.TabPages[0].Text = "Configure Data";
            TabCtlLaserMaint.TabPages[1].Text = "Scanner Test";

            TabCtlLaserMaint.Padding = new Point(6,5);
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

        private void InitGrid()
        {
            int i = 0, j = 0;

            // Cell Click 시 커서가 생성되지 않게함.
            GridConfigure.ActivateCurrentCellBehavior = GridCellActivateAction.None;

            // Header
            GridConfigure.Properties.RowHeaders = true;
            GridConfigure.Properties.ColHeaders = true;

            // Column,Row 개수
            GridConfigure.ColCount = 3;
            GridConfigure.RowCount = 25;

            // Column 가로 크기설정
            GridConfigure.ColWidths.SetSize(0, 200);
            GridConfigure.ColWidths.SetSize(1, 80);
            GridConfigure.ColWidths.SetSize(2, 100);
            GridConfigure.ColWidths.SetSize(3, 650);

            for (i = 0; i < 26; i++)
            {
                GridConfigure.RowHeights[i] = 27;
            }

            for (i = 0; i < 25; i++)
            {
                GridConfigure[i + 1, 1].BackColor = Color.FromArgb(230, 210, 255);
                GridConfigure[i + 1, 3].BackColor = Color.FromArgb(255, 230, 255);
            }

            // Text Display
            GridConfigure[0, 0].Text = "Parameter";
            GridConfigure[0, 1].Text = "Unit";
            GridConfigure[0, 2].Text = "Data";
            GridConfigure[0, 3].Text = "Description";

            GridConfigure[1, 0].Text = "InScanResolution";
            GridConfigure[2, 0].Text = "CrossScanResolution";
            GridConfigure[3, 0].Text = "InScanOffset";
            GridConfigure[4, 0].Text = "StopMotorBetweenJobs";
            GridConfigure[5, 0].Text = "PixInvert";
            GridConfigure[6, 0].Text = "JobStartBufferTime";
            GridConfigure[7, 0].Text = "PrecedingBlankLines";
            GridConfigure[8, 0].Text = "SeedClockFrequency";
            GridConfigure[9, 0].Text = "RepetitionRate";
            GridConfigure[10, 0].Text = "CrossScanEncoderResol";
            GridConfigure[11, 0].Text = "CrossScanMaxAccel";
            GridConfigure[12, 0].Text = "EnCarSig";
            GridConfigure[13, 0].Text = "SwapCarSig";
            GridConfigure[14, 0].Text = "InterLeaveRatio";
            GridConfigure[15, 0].Text = "FacetFineDelayOffset0";
            GridConfigure[16, 0].Text = "FacetFineDelayOffset1";
            GridConfigure[17, 0].Text = "FacetFineDelayOffset2";
            GridConfigure[18, 0].Text = "FacetFineDelayOffset3";
            GridConfigure[19, 0].Text = "FacetFineDelayOffset4";
            GridConfigure[20, 0].Text = "FacetFineDelayOffset5";
            GridConfigure[21, 0].Text = "FacetFineDelayOffset6";
            GridConfigure[22, 0].Text = "FacetFineDelayOffset7";
            GridConfigure[23, 0].Text = "StartFacet";
            GridConfigure[24, 0].Text = "AutoIncrementStartFacet";
            GridConfigure[25, 0].Text = "MotorStableTime";

            GridConfigure[1, 1].Text = "[u]";
            GridConfigure[2, 1].Text = "[u]";
            GridConfigure[3, 1].Text = "[u]";
            GridConfigure[4, 1].Text = "[-]";
            GridConfigure[5, 1].Text = "[-]";
            GridConfigure[6, 1].Text = "[sec]";
            GridConfigure[7, 1].Text = "[-]";
            GridConfigure[8, 1].Text = "[kHz]";
            GridConfigure[9, 1].Text = "[kHz]";
            GridConfigure[10, 1].Text = "[u]";
            GridConfigure[11, 1].Text = "[m/s^2]";
            GridConfigure[12, 1].Text = "[-]";
            GridConfigure[13, 1].Text = "[-]";
            GridConfigure[14, 1].Text = "[-]";
            GridConfigure[15, 1].Text = "[u]";
            GridConfigure[16, 1].Text = "[u]";
            GridConfigure[17, 1].Text = "[u]";
            GridConfigure[18, 1].Text = "[u]";
            GridConfigure[19, 1].Text = "[u]";
            GridConfigure[20, 1].Text = "[u]";
            GridConfigure[21, 1].Text = "[u]";
            GridConfigure[22, 1].Text = "[u]";
            GridConfigure[23, 1].Text = "[-]";
            GridConfigure[24, 1].Text = "[-]";
            GridConfigure[25, 1].Text = "[ms]";

            GridConfigure[1, 3].Text = "[Job Settings] Scanline에서 이웃한 두 pixel 사이 X축 거리";
            GridConfigure[2, 3].Text = "[Job Settings] 이웃한 두 Scanline 사이 Y축 거리";
            GridConfigure[3, 3].Text = "[Job Settings] 모든 Scanline의 X축 시작 위치를 조정";
            GridConfigure[4, 3].Text = "[Job Settings] Exposure 이후 polygonmirror 정지 여부 결정";
            GridConfigure[5, 3].Text = "[Job Settings] LaserPulse Exposure 되는 Bitmap의 색상 선택";
            GridConfigure[6, 3].Text = "[Job Settings] Bitmap Uploading 시, exposure 하기전 대기 시간";
            GridConfigure[7, 3].Text = "[Job Settings] Stage 가속시의 충분한 Settle-time을 위한 Dummy scanline 수";
            GridConfigure[8, 3].Text = "[Laser Configuration] Laser의 Seed Clock 주파수 설정";
            GridConfigure[9, 3].Text = "[Laser Configuration] 가공에 적용할 펄스 반복률(REP_RATE) 설정";
            GridConfigure[10, 3].Text = "[CrossScan Configuration] StageEncoder 분해능 값 설정";
            GridConfigure[11, 3].Text = "[CrossScan Configuration] Stagestart-up 과정의 최대 가속도";
            GridConfigure[12, 3].Text = "[CrossScan Configuration] Stage Control Encoder signal 출력 여부";
            GridConfigure[13, 3].Text = "[CrossScan Configuration] Stage movement direction 선택";
            GridConfigure[14, 3].Text = "[Head Configuration] FacetFineDelayOffset 자동 설정 기능";
            GridConfigure[15, 3].Text = "[Head Configuration] 각 scanline의 시작 위치 값의 미세 조정";
            GridConfigure[16, 3].Text = "[Head Configuration] 각 scanline의 시작 위치 값의 미세 조정";
            GridConfigure[17, 3].Text = "[Head Configuration] 각 scanline의 시작 위치 값의 미세 조정";
            GridConfigure[18, 3].Text = "[Head Configuration] 각 scanline의 시작 위치 값의 미세 조정";
            GridConfigure[19, 3].Text = "[Head Configuration] 각 scanline의 시작 위치 값의 미세 조정";
            GridConfigure[20, 3].Text = "[Head Configuration] 각 scanline의 시작 위치 값의 미세 조정";
            GridConfigure[21, 3].Text = "[Head Configuration] 각 scanline의 시작 위치 값의 미세 조정";
            GridConfigure[22, 3].Text = "[Head Configuration] 각 scanline의 시작 위치 값의 미세 조정";
            GridConfigure[23, 3].Text = "[Head Configuration] exposure의 첫 scanline에 해당하는 facet 지정";
            GridConfigure[24, 3].Text = "[Head Configuration] 새로운job started 마다 StartFacet 값 증가";
            GridConfigure[25, 3].Text = "[Polygon motor Configuration] speed-up 이후 exposure 시작 이전에 spinning 안정화 대기 시간";


            for (i = 0; i < 4; i++)
            {
                for (j = 0; j < 26; j++)
                {
                    // Font Style - Bold
                    GridConfigure[j, i].Font.Bold = true;

                    GridConfigure[j, i].VerticalAlignment = GridVerticalAlignment.Middle;

                    if (i != 3)
                    {
                        GridConfigure[j, i].HorizontalAlignment = GridHorizontalAlignment.Center;
                    }
                }
            }

            GridConfigure.GridVisualStyles = GridVisualStyles.Office2007Blue;
            GridConfigure.ResizeColsBehavior = 0;
            GridConfigure.ResizeRowsBehavior = 0;

            GetPolygonPara(scannerIndex);

            // Grid Display Update
            GridConfigure.Refresh();
        }

        private CPolygonIni GetPolygonPara(int objIndex)
        {
            return m_Polygon = CMainFrame.LWDicer.m_Scanner[objIndex].GetPolygonPara(objIndex);
        }

        private void UpdateScreen(CPolygonIni m_PolygonPara)
        {
            // User Enable Para
            GridConfigure[1, 2].Text = string.Format("{0:f}", m_PolygonPara.InScanResolution*1000000);
            GridConfigure[2, 2].Text = string.Format("{0:f}", m_PolygonPara.CrossScanResolution*1000000);
            GridConfigure[3, 2].Text = string.Format("{0:f}", m_PolygonPara.InScanOffset * 1000000);
            GridConfigure[4, 2].Text = Convert.ToString(m_PolygonPara.StopMotorBetweenJobs);
            GridConfigure[5, 2].Text = Convert.ToString(m_PolygonPara.PixInvert);
            GridConfigure[6, 2].Text = Convert.ToString(m_PolygonPara.JobStartBufferTime);
            GridConfigure[7, 2].Text = Convert.ToString(m_PolygonPara.PrecedingBlankLines);

            GridConfigure[8, 2].Text = string.Format("{0:f}", m_PolygonPara.SeedClockFrequency / 1000);
            GridConfigure[9, 2].Text = string.Format("{0:f}", m_PolygonPara.RepetitionRate / 1000);

            GridConfigure[10, 2].Text = string.Format("{0:f}", m_PolygonPara.CrossScanEncoderResol * 1000000);

            GridConfigure[11, 2].Text = Convert.ToString(m_PolygonPara.CrossScanMaxAccel);
            GridConfigure[12, 2].Text = Convert.ToString(m_PolygonPara.EnCarSig);
            GridConfigure[13, 2].Text = Convert.ToString(m_PolygonPara.SwapCarSig);
            GridConfigure[14, 2].Text = Convert.ToString(m_PolygonPara.InterleaveRatio);
            GridConfigure[15, 2].Text = string.Format("{0:f}", m_PolygonPara.FacetFineDelayOffset0 * 1000000);
            GridConfigure[16, 2].Text = string.Format("{0:f}", m_PolygonPara.FacetFineDelayOffset1 * 1000000);
            GridConfigure[17, 2].Text = string.Format("{0:f}", m_PolygonPara.FacetFineDelayOffset2 * 1000000);
            GridConfigure[18, 2].Text = string.Format("{0:f}", m_PolygonPara.FacetFineDelayOffset3 * 1000000);
            GridConfigure[19, 2].Text = string.Format("{0:f}", m_PolygonPara.FacetFineDelayOffset4 * 1000000);
            GridConfigure[20, 2].Text = string.Format("{0:f}", m_PolygonPara.FacetFineDelayOffset5 * 1000000);
            GridConfigure[21, 2].Text = string.Format("{0:f}", m_PolygonPara.FacetFineDelayOffset6 * 1000000);
            GridConfigure[22, 2].Text = string.Format("{0:f}", m_PolygonPara.FacetFineDelayOffset7 * 1000000);
            GridConfigure[23, 2].Text = Convert.ToString(m_PolygonPara.StartFacet);
            GridConfigure[24, 2].Text = Convert.ToString(m_PolygonPara.AutoIncrementStartFacet);
            GridConfigure[25, 2].Text = Convert.ToString(m_PolygonPara.MotorStableTime);
        }

        private void BtnConfigureSave_Click(object sender, EventArgs e)
        {

            if (!CMainFrame.LWDicer.DisplayMsg("Polygon Data를 저장 하시겠습니까?"))
            {
                return;
            }

            CMainFrame.LWDicer.m_Scanner[scannerIndex].SetPixelGridX(scannerIndex, Convert.ToDouble(GridConfigure[1, 2].Text));
            CMainFrame.LWDicer.m_Scanner[scannerIndex].SetPixelGridY(scannerIndex, Convert.ToDouble(GridConfigure[2, 2].Text));
            CMainFrame.LWDicer.m_Scanner[scannerIndex].SetStartOffset(scannerIndex, Convert.ToDouble(GridConfigure[3, 2].Text));
            CMainFrame.LWDicer.m_Scanner[scannerIndex].SetMotorBetweenJob(scannerIndex, Convert.ToInt16(GridConfigure[4, 2].Text));
            CMainFrame.LWDicer.m_Scanner[scannerIndex].SetBitMapColor(scannerIndex, Convert.ToInt16(GridConfigure[5, 2].Text));
            CMainFrame.LWDicer.m_Scanner[scannerIndex].SetBufferTime(scannerIndex, Convert.ToInt16(GridConfigure[6, 2].Text));
            CMainFrame.LWDicer.m_Scanner[scannerIndex].SetDummyBlankLine(scannerIndex, Convert.ToInt16(GridConfigure[7, 2].Text));
            CMainFrame.LWDicer.m_Scanner[scannerIndex].SetSeedClock(scannerIndex, Convert.ToDouble(GridConfigure[8, 2].Text));
            CMainFrame.LWDicer.m_Scanner[scannerIndex].SetRepRate(scannerIndex, Convert.ToDouble(GridConfigure[9, 2].Text));
            CMainFrame.LWDicer.m_Scanner[scannerIndex].SetEncoderResol(scannerIndex, Convert.ToDouble(GridConfigure[10, 2].Text));
            CMainFrame.LWDicer.m_Scanner[scannerIndex].SetMaxAccel(scannerIndex, Convert.ToDouble(GridConfigure[11, 2].Text));
            CMainFrame.LWDicer.m_Scanner[scannerIndex].SetEnCarSig(scannerIndex, Convert.ToInt16(GridConfigure[12, 2].Text));
            CMainFrame.LWDicer.m_Scanner[scannerIndex].SetSwapCarSig(scannerIndex, Convert.ToInt16(GridConfigure[13, 2].Text));
            CMainFrame.LWDicer.m_Scanner[scannerIndex].SetLeaveRatio(scannerIndex, Convert.ToInt16(GridConfigure[14, 2].Text));
            CMainFrame.LWDicer.m_Scanner[scannerIndex].SetSuperSync(scannerIndex, Convert.ToDouble(GridConfigure[15, 2].Text), Facet0);
            CMainFrame.LWDicer.m_Scanner[scannerIndex].SetSuperSync(scannerIndex, Convert.ToDouble(GridConfigure[16, 2].Text), Facet1);
            CMainFrame.LWDicer.m_Scanner[scannerIndex].SetSuperSync(scannerIndex, Convert.ToDouble(GridConfigure[17, 2].Text), Facet2);
            CMainFrame.LWDicer.m_Scanner[scannerIndex].SetSuperSync(scannerIndex, Convert.ToDouble(GridConfigure[18, 2].Text), Facet3);
            CMainFrame.LWDicer.m_Scanner[scannerIndex].SetSuperSync(scannerIndex, Convert.ToDouble(GridConfigure[19, 2].Text), Facet4);
            CMainFrame.LWDicer.m_Scanner[scannerIndex].SetSuperSync(scannerIndex, Convert.ToDouble(GridConfigure[20, 2].Text), Facet5);
            CMainFrame.LWDicer.m_Scanner[scannerIndex].SetSuperSync(scannerIndex, Convert.ToDouble(GridConfigure[21, 2].Text), Facet6);
            CMainFrame.LWDicer.m_Scanner[scannerIndex].SetSuperSync(scannerIndex, Convert.ToDouble(GridConfigure[22, 2].Text), Facet7);
            CMainFrame.LWDicer.m_Scanner[scannerIndex].SetStartFacet(scannerIndex, Convert.ToInt16(GridConfigure[23, 2].Text));
            CMainFrame.LWDicer.m_Scanner[scannerIndex].SetAutoIncStartFacet(scannerIndex, Convert.ToInt16(GridConfigure[24, 2].Text));
            CMainFrame.LWDicer.m_Scanner[scannerIndex].SetMotorStableTime(scannerIndex, Convert.ToInt16(GridConfigure[25, 2].Text));

            CMainFrame.LWDicer.m_Scanner[scannerIndex].SetScannerIP(scannerIndex, LabelIP.Text);
            CMainFrame.LWDicer.m_Scanner[scannerIndex].SetScannerPort(scannerIndex, LabelPort.Text);
            
            CMainFrame.LWDicer.m_DataManager.m_SystemData.Scanner[scannerIndex] = GetPolygonPara(scannerIndex);

            CMainFrame.LWDicer.m_Scanner[scannerIndex].SavePolygonPara(CMainFrame.LWDicer.m_DataManager.m_SystemData.Scanner[scannerIndex], "config");

            UpdateScreen(m_Polygon);
        }

        private void FormLaserMaint_Load(object sender, EventArgs e)
        {
            InitGrid();

            UpdateScreen(m_Polygon);
        }

        private void GridConfigure_CellClick(object sender, GridCellClickEventArgs e)
        {
            int nCol = 0, nRow = 0;
            string StrCurrent = "", strModify = "";

            nCol = e.ColIndex;
            nRow = e.RowIndex;

            if (nCol != 2 || nRow == 0)
            {
                return;
            }

            StrCurrent = GridConfigure[nRow, nCol].Text;

            if (!CMainFrame.LWDicer.GetKeyPad(StrCurrent, out strModify))
            {
                return;
            }

            GridConfigure[nRow, nCol].Text = strModify;
        }


        private void LabelIP_Click(object sender, EventArgs e)
        {
            string strModify = "";

            if (!CMainFrame.LWDicer.GetKeyboard(out strModify))
            {
                return;
            }

            LabelIP.Text = strModify;

            CMainFrame.LWDicer.m_DataManager.m_SystemData.Scanner[scannerIndex].strIP = LabelIP.Text;

        }

        private void LabelPort_Click(object sender, EventArgs e)
        {
            string StrCurrent = "", strModify = "";

            StrCurrent = LabelPort.Text;

            if (!CMainFrame.LWDicer.GetKeyPad(StrCurrent, out strModify))
            {
                return;
            }

            LabelPort.Text = strModify;

            CMainFrame.LWDicer.m_DataManager.m_SystemData.Scanner[scannerIndex].strPort = LabelPort.Text;
        }

        private void TabCtlLaserMaint_Click(object sender, EventArgs e)
        {
            int nIndex = 0;

            TabControlAdv Tab = sender as TabControlAdv;

            nIndex = Tab.SelectedTab.TabIndex;

            if(nIndex == 1)
            {
                BtnConfigureSave.Show();
                TmrScannerTest.Stop();

            }else if(nIndex == 2)
            {
                BtnConfigureSave.Hide();
                TmrScannerTest.Start();
            }
        }

        private void BtnLoadFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenIniFile = new OpenFileDialog();
            OpenIniFile.Filter = "TXT Text|*.ini";
            OpenIniFile.InitialDirectory = "T:\\SFA\\LWDicer\\ScannerLog";

            if (OpenIniFile.ShowDialog() == DialogResult.OK)
            {
                if (OpenIniFile.FileName.Length > 0)
                {
                    foreach (string filename in OpenIniFile.FileNames)
                    {
                        this.TextConfigureFile.Text = filename;
                        strCFGFileName = OpenIniFile.SafeFileName;
                    }
                }
            }

            OpenIniFile.Dispose();
        }

        private void BtnUploadFile_Click(object sender, EventArgs e)
        {
            string strPath = string.Empty;

            strPath = string.Format(@"{0:s}", @TextConfigureFile.Text);

            CMainFrame.LWDicer.m_Scanner[scannerIndex].SendTFTPFile(LabelIP.Text, strPath);
        }

        private void TmrScannerTest_Tick(object sender, EventArgs e)
        {
            string strText = "";
            string strMsg = "";

            if (CMainFrame.LWDicer.m_Scanner[scannerIndex].GetSerialData(out strText) == DEF_Error.SUCCESS)
            {
                if (strText != "")
                {
                    strMsg = strText;

                    TextComLog.Text = TextComLog.Text + strMsg + "\r\n";

                }
            }
        }


        private void BtnLoadBitmap_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenBmpFile = new OpenFileDialog();
            OpenBmpFile.Filter = "Bitmap Image|*.bmp";
            OpenBmpFile.InitialDirectory = "T:\\SFA\\LWDicer\\ScannerLog";

            if (OpenBmpFile.ShowDialog() == DialogResult.OK)
            {
                if (OpenBmpFile.FileName.Length > 0)
                {
                    foreach (string filename in OpenBmpFile.FileNames)
                    {
                        TextBitmapFile.Text = filename;
                        strBMPFileName = OpenBmpFile.SafeFileName;
                    }
                }
            }

            OpenBmpFile.Dispose();
        }

        private void BtnStreaming_Click(object sender, EventArgs e)
        {
            string strPath = string.Empty;

            strPath = string.Format(@"{0:s}", TextBitmapFile.Text);

            CMainFrame.LWDicer.m_Scanner[scannerIndex].SendTFTPFile(LabelIP.Text, strPath);
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            TextComLog.Text = "";
        }
    }
}
