using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Configuration;

using LWDicer.Control;
using LWDicer.UI;

using static LWDicer.Control.DEF_Thread;
using static LWDicer.Control.DEF_Thread.EThreadMessage;
using static LWDicer.Control.DEF_Thread.EWindowMessage;

using static LWDicer.Control.DEF_Error;
using static LWDicer.Control.DEF_Common;
using static LWDicer.Control.DEF_Cylinder;
using static LWDicer.Control.DEF_SerialPort;

//#pragma warning disable CS0219

namespace LWDicer.UI
{
    public partial class CMainFrame : Form
    {
        public static MLWDicer LWDicer = new MLWDicer(new CObjectInfo());

        public static CMainFrame MainFrame = null;

        private FormTop m_FormTop;

        private FormSubBottom m_FormSubBottom;

        private FormMain m_FormMain;
        private FormMainBottom m_FormMainBottom;

        private FormFullAuto m_FormFullAuto;
        private FormFullAuto_1 m_FormFullAuto_1;
        private FormFullAuto_2 m_FormFullAuto_2;
        private FormFullAuto_3 m_FormFullAuto_3;
        private FormFullAutoBottom m_FormFullAutoBottom;

        public FormManualOP m_FormManualOP;
        public FormManualOP_1 m_FormManualOP_1;
        public FormManualOP_2 m_FormManualOP_2;
        public FormManualOP_3 m_FormManualOP_3;
        public FormManualOPBottom m_FormManualOPBottom;

        private FormDeviceData m_FormDeviceData;
        private FormDeviceData_1 m_FormDeviceData_1;
        private FormDeviceData_2 m_FormDeviceData_2;
        private FormDeviceData_3 m_FormDeviceData_3;
        private FormDeviceDataBottom m_FormDeviceDataBottom;

        private FormEngineerMaint m_FormEngineerMaint;
        private FormAxisOperation m_AxisOperation;
        private FormIOCheck       m_IOCheck;
        private FormAxisParameter m_AxisParameter;
        private FormEngineerMaintBottom m_FormEngineerMaintBottom;

        private FormOperatorMaint m_FormOperatorMaint;
        private FormOperatorMaint_1 m_FormOperatorMaint_1;
        private FormOperatorMaint_2 m_FormOperatorMaint_2;
        private FormOperatorMaint_3 m_FormOperatorMaint_3;
        private FormOperatorMaintBottom m_FormOperatorMaintBottom;

        private FormMachineMaint m_FormMachineMaint;
        private FormMachineMaint_1 m_FormMachineMaint_1;
        private FormMachineMaint_2 m_FormMachineMaint_2;
        private FormMachineMaint_3 m_FormMachineMaint_3;
        private FormMachineMaintBottom m_FormMachineMaintBottom;

        private FormLaserMaint m_FormLaserMaint;
        private FormLaserMaint_1 m_FormLaserMaint_1;
        private FormLaserMaint_2 m_FormLaserMaint_2;
        private FormLaserMaint_3 m_FormLaserMaint_3;
        private FormLaserMaintBottom m_FormLaserMaintBottom;

        public List<KeyValuePair<PageInfo, Form>> FormMatchingList = new List<KeyValuePair<PageInfo, Form>>();
        public List<KeyValuePair<BottomPageInfo, Form>> BottomFormMatchingList = new List<KeyValuePair<BottomPageInfo, Form>>();

        public KeyValuePair<PageInfo, Form> CurrentPage;
        public KeyValuePair<BottomPageInfo, Form> BottomCurrentPage;

        public PageInfo PrevPage;
        public PageInfo PrevBottomPage;

        public CMainFrame()
        {

            InitializeLWDicer();

            InitializeComponent();

            InitializeForm();

            CreateForms();

            FormsAttachToMainContentsPanel();

            FormsAttachToBottomContentsPanel();

            PageCreationSetting();

            MainFrame = this;

            
        }

        public void ProcessMsg(MEvent evnt)
        {
            string msg = "Get Message from Control : " + evnt;
            Debug.WriteLine("===================================================");
            Debug.WriteLine(msg);

            m_FormFullAuto.ProcessMsg(evnt);
            switch (evnt.Msg)
            {
                case (int)WM_START_MANUAL_MSG:
                    break;
            }
        }

        protected virtual void InitializeForm()
        {
            Screen[] sc = System.Windows.Forms.Screen.AllScreens;

            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;

            if (sc.Length > 1)
            {
                this.DesktopLocation = new Point(DEF_UI.FORM_POS_X + sc[0].Bounds.Width, DEF_UI.FORM_POS_Y); // 다중 모니터
            }
            else
            {
                this.DesktopLocation = new Point(DEF_UI.FORM_POS_X, DEF_UI.FORM_POS_Y); // 기본 모니터
            }
            this.DesktopLocation = new Point(DEF_UI.FORM_POS_X, DEF_UI.FORM_POS_Y); // 기본 모니터

            this.Size = new Size(DEF_UI.FORM_SIZE_WIDTH, DEF_UI.FORM_SIZE_HEIGHT);
            this.IsMdiContainer = true;
            this.FormBorderStyle = FormBorderStyle.SizableToolWindow;

            PanelTop.Location = new Point(DEF_UI.TOP_POS_X, DEF_UI.TOP_POS_Y);
            PanelTop.Size = new Size(DEF_UI.TOP_SIZE_WIDTH, DEF_UI.TOP_SIZE_HEIGHT);

            MainUIPanel.Location = new Point(DEF_UI.MAIN_POS_X, DEF_UI.MAIN_POS_Y);
            MainUIPanel.Size = new Size(DEF_UI.MAIN_SIZE_WIDTH, DEF_UI.MAIN_SIZE_HEIGHT);

            PanelBottom.Location = new Point(DEF_UI.BOT_POS_X, DEF_UI.BOT_POS_Y);
            PanelBottom.Size = new Size(DEF_UI.BOT_SIZE_WIDTH, DEF_UI.BOT_SIZE_HEIGHT);

            PanelSubBottom.Location = new Point(DEF_UI.SUB_BOT_POS_X, DEF_UI.SUB_BOT_POS_Y);
            PanelSubBottom.Size = new Size(DEF_UI.SUB_BOT_WIDTH, DEF_UI.SUB_BOT_HEIGHT);

        }

        private void CreateForms()
        {
            m_FormTop = new FormTop();

            m_FormSubBottom = new FormSubBottom();

            m_FormMain = new FormMain();
            m_FormMainBottom = new FormMainBottom();

            m_FormFullAuto = new FormFullAuto();
            m_FormFullAuto_1 = new FormFullAuto_1();
            m_FormFullAuto_2 = new FormFullAuto_2();
            m_FormFullAuto_3 = new FormFullAuto_3();
            m_FormFullAutoBottom = new FormFullAutoBottom();

            m_FormManualOP = new FormManualOP();
            m_FormManualOP_1 = new FormManualOP_1();
            m_FormManualOP_2 = new FormManualOP_2();
            m_FormManualOP_3 = new FormManualOP_3();
            m_FormManualOPBottom = new FormManualOPBottom();

            m_FormDeviceData = new FormDeviceData();
            m_FormDeviceData_1 = new FormDeviceData_1();
            m_FormDeviceData_2 = new FormDeviceData_2();
            m_FormDeviceData_3 = new FormDeviceData_3();
            m_FormDeviceDataBottom = new FormDeviceDataBottom();

            m_FormEngineerMaint = new FormEngineerMaint();
            m_AxisOperation     = new FormAxisOperation();
            m_IOCheck           = new FormIOCheck();
            m_AxisParameter     = new FormAxisParameter();
            m_FormEngineerMaintBottom = new FormEngineerMaintBottom();

            m_FormOperatorMaint = new FormOperatorMaint();
            m_FormOperatorMaint_1 = new FormOperatorMaint_1();
            m_FormOperatorMaint_2 = new FormOperatorMaint_2();
            m_FormOperatorMaint_3 = new FormOperatorMaint_3();
            m_FormOperatorMaintBottom = new FormOperatorMaintBottom();

            m_FormMachineMaint = new FormMachineMaint();
            m_FormMachineMaint_1 = new FormMachineMaint_1();
            m_FormMachineMaint_2 = new FormMachineMaint_2();
            m_FormMachineMaint_3 = new FormMachineMaint_3();
            m_FormMachineMaintBottom = new FormMachineMaintBottom();

            m_FormLaserMaint = new FormLaserMaint();
            m_FormLaserMaint_1 = new FormLaserMaint_1();
            m_FormLaserMaint_2 = new FormLaserMaint_2();
            m_FormLaserMaint_3 = new FormLaserMaint_3();
            m_FormLaserMaintBottom = new FormLaserMaintBottom();
        }

        private void PageCreationSetting()
        {
            PageInfo PageInfoFormTop = new PageInfo();

            PageInfo PageInfoFormSubBottom = new PageInfo();

            //PageInfo PageInfoFormMain = new PageInfo();

            PageInfo PageInfoFormMain = new PageInfo() { BigClassifyingNumber = 0, SmallClassifyingNumber = 0 };

            BottomPageInfo PageInfoFormMainBottom = new BottomPageInfo();
            BottomPageInfo PageInfoFormFullAutoBottom = new BottomPageInfo();
            BottomPageInfo PageInfoFormManualOPBottom = new BottomPageInfo();
            BottomPageInfo PageInfoFormDeviceDataBottom = new BottomPageInfo();

            BottomPageInfo PageInfoFormEngineerMaintBottom = new BottomPageInfo();
            BottomPageInfo PageInfoFormOperatorMaintBottom = new BottomPageInfo();
            BottomPageInfo PageInfoFormMachineMaintBottom = new BottomPageInfo();
            BottomPageInfo PageInfoFormLaserMaintBottom = new BottomPageInfo();

            PageInfo PageInfoFormFullAuto = new PageInfo() { BigClassifyingNumber = 1, SmallClassifyingNumber = 1 };
            PageInfo PageInfoFormFullAuto_1 = new PageInfo() { BigClassifyingNumber = 1, SmallClassifyingNumber = 2 };
            PageInfo PageInfoFormFullAuto_2 = new PageInfo() { BigClassifyingNumber = 1, SmallClassifyingNumber = 3 };
            PageInfo PageInfoFormFullAuto_3 = new PageInfo() { BigClassifyingNumber = 1, SmallClassifyingNumber = 4 };

            PageInfo PageInfoFormManualOP = new PageInfo() { BigClassifyingNumber = 2, SmallClassifyingNumber = 1 };
            PageInfo PageInfoFormManualOP_1 = new PageInfo() { BigClassifyingNumber = 2, SmallClassifyingNumber = 2 };
            PageInfo PageInfoFormManualOP_2 = new PageInfo() { BigClassifyingNumber = 2, SmallClassifyingNumber = 3 };
            PageInfo PageInfoFormManualOP_3 = new PageInfo() { BigClassifyingNumber = 2, SmallClassifyingNumber = 4 };

            PageInfo PageInfoFormDeviceData = new PageInfo() { BigClassifyingNumber = 3, SmallClassifyingNumber = 1 };
            PageInfo PageInfoFormDeviceData_1 = new PageInfo() { BigClassifyingNumber = 3, SmallClassifyingNumber = 2 };
            PageInfo PageInfoFormDeviceData_2 = new PageInfo() { BigClassifyingNumber = 3, SmallClassifyingNumber = 3 };
            PageInfo PageInfoFormDeviceData_3 = new PageInfo() { BigClassifyingNumber = 3, SmallClassifyingNumber = 4 };

            PageInfo PageInfoEngineerMaint = new PageInfo() { BigClassifyingNumber = 4, SmallClassifyingNumber = 1 };
            PageInfo PageInfoAxisOperation = new PageInfo() { BigClassifyingNumber = 4, SmallClassifyingNumber = 2 };
            PageInfo PageInfoIOCheck       = new PageInfo() { BigClassifyingNumber = 4, SmallClassifyingNumber = 3 };
            PageInfo PageInfoAxisParameter = new PageInfo() { BigClassifyingNumber = 4, SmallClassifyingNumber = 4 };

            PageInfo PageInfoOperatorMaint = new PageInfo() { BigClassifyingNumber = 5, SmallClassifyingNumber = 1 };
            PageInfo PageInfoOperatorMaint_1 = new PageInfo() { BigClassifyingNumber = 5, SmallClassifyingNumber = 2 };
            PageInfo PageInfoOperatorMaint_2 = new PageInfo() { BigClassifyingNumber = 5, SmallClassifyingNumber = 3 };
            PageInfo PageInfoOperatorMaint_3 = new PageInfo() { BigClassifyingNumber = 5, SmallClassifyingNumber = 4 };

            PageInfo PageInfoMachineMaint = new PageInfo() { BigClassifyingNumber = 6, SmallClassifyingNumber = 1 };
            PageInfo PageInfoMachineMaint_1 = new PageInfo() { BigClassifyingNumber = 6, SmallClassifyingNumber = 2 };
            PageInfo PageInfoMachineMaint_2 = new PageInfo() { BigClassifyingNumber = 6, SmallClassifyingNumber = 3 };
            PageInfo PageInfoMachineMaint_3 = new PageInfo() { BigClassifyingNumber = 6, SmallClassifyingNumber = 4 };

            PageInfo PageInfoLaserMaint = new PageInfo() { BigClassifyingNumber = 7, SmallClassifyingNumber = 1 };
            PageInfo PageInfoLaserMaint_1 = new PageInfo() { BigClassifyingNumber = 7, SmallClassifyingNumber = 2 };
            PageInfo PageInfoLaserMaint_2 = new PageInfo() { BigClassifyingNumber = 7, SmallClassifyingNumber = 3 };
            PageInfo PageInfoLaserMaint_3 = new PageInfo() { BigClassifyingNumber = 7, SmallClassifyingNumber = 4 };

            m_FormMain.SetFullAutoPage(PageInfoFormFullAuto, PageInfoFormFullAutoBottom);
            m_FormMain.SetManualOPPage(PageInfoFormManualOP, PageInfoFormManualOPBottom);
            m_FormMain.SetDeviceDataPage(PageInfoFormDeviceData, PageInfoFormDeviceDataBottom);

            m_FormMain.SetOperatorMaintPage(PageInfoOperatorMaint, PageInfoFormOperatorMaintBottom);
            m_FormMain.SetMachineMaintPage(PageInfoMachineMaint, PageInfoFormMachineMaintBottom);
            m_FormMain.SetEngineerMaintPage(PageInfoEngineerMaint, PageInfoFormEngineerMaintBottom);
            m_FormMain.SetLaserMaintPage(PageInfoLaserMaint, PageInfoFormLaserMaintBottom);

            m_FormFullAuto.SetPrevPage(PageInfoFormMain);
            m_FormFullAuto.SetNextPage(PageInfoFormFullAuto_1);
            m_FormFullAuto.SetNextBottomPage(PageInfoFormMainBottom);

            m_FormFullAuto_1.SetPrevPage(PageInfoFormFullAuto);
            m_FormFullAuto_1.SetNextPage(PageInfoFormFullAuto_2);

            m_FormFullAuto_2.SetPrevPage(PageInfoFormFullAuto_1);
            m_FormFullAuto_2.SetNextPage(PageInfoFormFullAuto_3);

            m_FormFullAuto_3.SetPrevPage(PageInfoFormFullAuto_2);
            m_FormFullAuto_3.SetNextPage(null);

            m_FormManualOP.SetPrevPage(PageInfoFormMain);
            m_FormManualOP.SetNextPage(PageInfoFormManualOP_1);
            m_FormManualOP.SetNextBottomPage(PageInfoFormMainBottom);

            m_FormManualOP_1.SetPrevPage(PageInfoFormManualOP);
            m_FormManualOP_1.SetNextPage(PageInfoFormManualOP_2);

            m_FormManualOP_2.SetPrevPage(PageInfoFormManualOP_1);
            m_FormManualOP_2.SetNextPage(PageInfoFormManualOP_3);

            m_FormManualOP_3.SetPrevPage(PageInfoFormManualOP_2);
            m_FormManualOP_3.SetNextPage(null);

            m_FormDeviceData.SetPrevPage(PageInfoFormMain);
            m_FormDeviceData.SetNextPage(PageInfoFormDeviceData_1);
            m_FormDeviceData.SetNextBottomPage(PageInfoFormMainBottom);

            m_FormDeviceData_1.SetPrevPage(PageInfoFormDeviceData);
            m_FormDeviceData_1.SetNextPage(PageInfoFormDeviceData_2);

            m_FormDeviceData_2.SetPrevPage(PageInfoFormDeviceData_1);
            m_FormDeviceData_2.SetNextPage(PageInfoFormDeviceData_3);

            m_FormDeviceData_3.SetPrevPage(PageInfoFormDeviceData_2);
            m_FormDeviceData_3.SetNextPage(null);

            m_FormEngineerMaint.SetPrevPage(PageInfoFormMain);
            m_FormEngineerMaint.SetAxisOPPage(PageInfoAxisOperation);
            m_FormEngineerMaint.SetIOPage(PageInfoIOCheck);
            m_FormEngineerMaint.SetNextBottomPage(PageInfoFormMainBottom);

            m_AxisOperation.SetPrevPage(PageInfoEngineerMaint);
            m_AxisOperation.SetNextPage(PageInfoAxisParameter);
            m_AxisParameter.SetPrevPage(PageInfoAxisOperation);
            m_IOCheck.SetPrevPage(PageInfoEngineerMaint);

            m_FormOperatorMaint.SetPrevPage(PageInfoFormMain);
            m_FormOperatorMaint.SetNextPage(PageInfoOperatorMaint_1);
            m_FormOperatorMaint.SetNextBottomPage(PageInfoFormMainBottom);

            m_FormOperatorMaint_1.SetPrevPage(PageInfoOperatorMaint);
            m_FormOperatorMaint_1.SetNextPage(PageInfoOperatorMaint_2);

            m_FormOperatorMaint_2.SetPrevPage(PageInfoOperatorMaint_1);
            m_FormOperatorMaint_2.SetNextPage(PageInfoOperatorMaint_3);

            m_FormOperatorMaint_3.SetPrevPage(PageInfoOperatorMaint_2);
            m_FormOperatorMaint_3.SetNextPage(null);

            m_FormMachineMaint.SetPrevPage(PageInfoFormMain);
            m_FormMachineMaint.SetNextPage(PageInfoMachineMaint_1);
            m_FormMachineMaint.SetNextBottomPage(PageInfoFormMainBottom);

            m_FormMachineMaint_1.SetPrevPage(PageInfoMachineMaint);
            m_FormMachineMaint_1.SetNextPage(PageInfoMachineMaint_2);

            m_FormMachineMaint_2.SetPrevPage(PageInfoMachineMaint_1);
            m_FormMachineMaint_2.SetNextPage(PageInfoMachineMaint_3);

            m_FormMachineMaint_3.SetPrevPage(PageInfoMachineMaint_2);
            m_FormMachineMaint_3.SetNextPage(null);

            m_FormLaserMaint.SetPrevPage(PageInfoFormMain);
            m_FormLaserMaint.SetNextPage(PageInfoLaserMaint_1);
            m_FormLaserMaint.SetNextBottomPage(PageInfoFormMainBottom);

            m_FormLaserMaint_1.SetPrevPage(PageInfoLaserMaint);
            m_FormLaserMaint_1.SetNextPage(PageInfoLaserMaint_2);

            m_FormLaserMaint_2.SetPrevPage(PageInfoLaserMaint_1);
            m_FormLaserMaint_2.SetNextPage(PageInfoLaserMaint_3);

            m_FormLaserMaint_3.SetPrevPage(PageInfoLaserMaint_2);
            m_FormLaserMaint_3.SetNextPage(null);

            FormMatchingList.Add(new KeyValuePair<PageInfo, Form>(PageInfoFormTop, m_FormTop));
            FormMatchingList.Add(new KeyValuePair<PageInfo, Form>(PageInfoFormSubBottom, m_FormSubBottom));
            FormMatchingList.Add(new KeyValuePair<PageInfo, Form>(PageInfoFormMain, m_FormMain));

            FormMatchingList.Add(new KeyValuePair<PageInfo, Form>(PageInfoFormFullAuto, m_FormFullAuto));
            FormMatchingList.Add(new KeyValuePair<PageInfo, Form>(PageInfoFormFullAuto_1, m_FormFullAuto_1));
            FormMatchingList.Add(new KeyValuePair<PageInfo, Form>(PageInfoFormFullAuto_2, m_FormFullAuto_2));
            FormMatchingList.Add(new KeyValuePair<PageInfo, Form>(PageInfoFormFullAuto_3, m_FormFullAuto_3));

            FormMatchingList.Add(new KeyValuePair<PageInfo, Form>(PageInfoFormManualOP, m_FormManualOP));
            FormMatchingList.Add(new KeyValuePair<PageInfo, Form>(PageInfoFormManualOP_1, m_FormManualOP_1));
            FormMatchingList.Add(new KeyValuePair<PageInfo, Form>(PageInfoFormManualOP_2, m_FormManualOP_2));
            FormMatchingList.Add(new KeyValuePair<PageInfo, Form>(PageInfoFormManualOP_3, m_FormManualOP_3));

            FormMatchingList.Add(new KeyValuePair<PageInfo, Form>(PageInfoFormDeviceData, m_FormDeviceData));
            FormMatchingList.Add(new KeyValuePair<PageInfo, Form>(PageInfoFormDeviceData_1, m_FormDeviceData_1));
            FormMatchingList.Add(new KeyValuePair<PageInfo, Form>(PageInfoFormDeviceData_2, m_FormDeviceData_2));
            FormMatchingList.Add(new KeyValuePair<PageInfo, Form>(PageInfoFormDeviceData_3, m_FormDeviceData_3));

            FormMatchingList.Add(new KeyValuePair<PageInfo, Form>(PageInfoOperatorMaint, m_FormOperatorMaint));
            FormMatchingList.Add(new KeyValuePair<PageInfo, Form>(PageInfoOperatorMaint_1, m_FormOperatorMaint_1));
            FormMatchingList.Add(new KeyValuePair<PageInfo, Form>(PageInfoOperatorMaint_2, m_FormOperatorMaint_2));
            FormMatchingList.Add(new KeyValuePair<PageInfo, Form>(PageInfoOperatorMaint_3, m_FormOperatorMaint_3));

            FormMatchingList.Add(new KeyValuePair<PageInfo, Form>(PageInfoMachineMaint, m_FormMachineMaint));
            FormMatchingList.Add(new KeyValuePair<PageInfo, Form>(PageInfoMachineMaint_1, m_FormMachineMaint_1));
            FormMatchingList.Add(new KeyValuePair<PageInfo, Form>(PageInfoMachineMaint_2, m_FormMachineMaint_2));
            FormMatchingList.Add(new KeyValuePair<PageInfo, Form>(PageInfoMachineMaint_3, m_FormMachineMaint_3));

            FormMatchingList.Add(new KeyValuePair<PageInfo, Form>(PageInfoEngineerMaint, m_FormEngineerMaint));
            FormMatchingList.Add(new KeyValuePair<PageInfo, Form>(PageInfoAxisOperation, m_AxisOperation));
            FormMatchingList.Add(new KeyValuePair<PageInfo, Form>(PageInfoAxisParameter, m_AxisParameter));
            FormMatchingList.Add(new KeyValuePair<PageInfo, Form>(PageInfoIOCheck, m_IOCheck));


            FormMatchingList.Add(new KeyValuePair<PageInfo, Form>(PageInfoLaserMaint, m_FormLaserMaint));
            FormMatchingList.Add(new KeyValuePair<PageInfo, Form>(PageInfoLaserMaint_1, m_FormLaserMaint_1));
            FormMatchingList.Add(new KeyValuePair<PageInfo, Form>(PageInfoLaserMaint_2, m_FormLaserMaint_2));
            FormMatchingList.Add(new KeyValuePair<PageInfo, Form>(PageInfoLaserMaint_3, m_FormLaserMaint_3));

            BottomFormMatchingList.Add(new KeyValuePair<BottomPageInfo, Form>(PageInfoFormMainBottom, m_FormMainBottom));
            BottomFormMatchingList.Add(new KeyValuePair<BottomPageInfo, Form>(PageInfoFormFullAutoBottom, m_FormFullAutoBottom));
            BottomFormMatchingList.Add(new KeyValuePair<BottomPageInfo, Form>(PageInfoFormManualOPBottom, m_FormManualOPBottom));
            BottomFormMatchingList.Add(new KeyValuePair<BottomPageInfo, Form>(PageInfoFormDeviceDataBottom, m_FormDeviceDataBottom));

            BottomFormMatchingList.Add(new KeyValuePair<BottomPageInfo, Form>(PageInfoFormEngineerMaintBottom, m_FormEngineerMaintBottom));
            BottomFormMatchingList.Add(new KeyValuePair<BottomPageInfo, Form>(PageInfoFormOperatorMaintBottom, m_FormOperatorMaintBottom));
            BottomFormMatchingList.Add(new KeyValuePair<BottomPageInfo, Form>(PageInfoFormMachineMaintBottom, m_FormMachineMaintBottom));
            BottomFormMatchingList.Add(new KeyValuePair<BottomPageInfo, Form>(PageInfoFormLaserMaintBottom, m_FormLaserMaintBottom));

            // Top Page : 고정 Page
            CurrentPage = new KeyValuePair<PageInfo, Form>(PageInfoFormTop, m_FormTop);
            MoveToPage(CurrentPage.Key);

            // Sub Bottom Page : 고정 Page
            CurrentPage = new KeyValuePair<PageInfo, Form>(PageInfoFormSubBottom, m_FormSubBottom);
            MoveToPage(CurrentPage.Key);

            // MainFrame Page
            CurrentPage = new KeyValuePair<PageInfo, Form>(PageInfoFormMain, m_FormMain);
            PrevPage = PageInfoFormMain;
            MoveToPage(CurrentPage.Key);

            // MainFrame Bottom Page
            BottomCurrentPage = new KeyValuePair<BottomPageInfo, Form>(PageInfoFormMainBottom, m_FormMainBottom);
            MoveToBottomPage(BottomCurrentPage.Key);
        }

        public bool MoveToPage(PageInfo ChangePage)
        {
            if (CurrentPage.Key != null)
            {
                if (PrevPage != null)
                {
                    PrevPage.BigClassifyingNumber = CurrentPage.Key.BigClassifyingNumber;
                    PrevPage.SmallClassifyingNumber = CurrentPage.Key.SmallClassifyingNumber;
                }

                CurrentPage.Value.Hide();
            }

            KeyValuePair<PageInfo, Form> findCurrentPage;

            findCurrentPage = FormMatchingList.FirstOrDefault(listItem => listItem.Key == ChangePage);

            // 찾는 화면이 없을때
            if (findCurrentPage.Key == null)
            {
                MessageBox.Show("PageInfo 에 해당하는 화면이 없습니다.");

                return false;
            }

            findCurrentPage.Value.Show();

            CurrentPage = findCurrentPage;

            return true;
        }

        public bool MoveToBottomPage(BottomPageInfo ChangePage)
        {
            if (BottomCurrentPage.Key != null)
                BottomCurrentPage.Value.Hide();

            KeyValuePair<BottomPageInfo, Form> findCurrentPage;

            findCurrentPage = BottomFormMatchingList.FirstOrDefault(listItem => listItem.Key == ChangePage);

            // 찾는 화면이 없을때
            if (findCurrentPage.Key == null)
            {
                MessageBox.Show("PageInfo 에 해당하는 화면이 없습니다.");

                return false;
            }

            findCurrentPage.Value.Show();

            BottomCurrentPage = findCurrentPage;

            return true;
        }


        private void FormsAttachToBottomContentsPanel()
        {
            FormAttachToPanel(m_FormMainBottom, PanelBottom);
            FormAttachToPanel(m_FormFullAutoBottom, PanelBottom);
            FormAttachToPanel(m_FormManualOPBottom, PanelBottom);
            FormAttachToPanel(m_FormDeviceDataBottom, PanelBottom);

            FormAttachToPanel(m_FormEngineerMaintBottom, PanelBottom);
            FormAttachToPanel(m_FormOperatorMaintBottom, PanelBottom);
            FormAttachToPanel(m_FormMachineMaintBottom, PanelBottom);
            FormAttachToPanel(m_FormLaserMaintBottom, PanelBottom);
        }

        private void FormsAttachToMainContentsPanel()
        {
            FormAttachToPanel(m_FormTop, PanelTop);
            FormAttachToPanel(m_FormSubBottom, PanelSubBottom);

            FormAttachToPanel(m_FormMain, MainUIPanel);

            FormAttachToPanel(m_FormFullAuto, MainUIPanel);
            FormAttachToPanel(m_FormFullAuto_1, MainUIPanel);
            FormAttachToPanel(m_FormFullAuto_2, MainUIPanel);
            FormAttachToPanel(m_FormFullAuto_3, MainUIPanel);

            FormAttachToPanel(m_FormManualOP, MainUIPanel);
            FormAttachToPanel(m_FormManualOP_1, MainUIPanel);
            FormAttachToPanel(m_FormManualOP_2, MainUIPanel);
            FormAttachToPanel(m_FormManualOP_3, MainUIPanel);

            FormAttachToPanel(m_FormDeviceData, MainUIPanel);
            FormAttachToPanel(m_FormDeviceData_1, MainUIPanel);
            FormAttachToPanel(m_FormDeviceData_2, MainUIPanel);
            FormAttachToPanel(m_FormDeviceData_3, MainUIPanel);

            FormAttachToPanel(m_FormOperatorMaint, MainUIPanel);
            FormAttachToPanel(m_FormOperatorMaint_1, MainUIPanel);
            FormAttachToPanel(m_FormOperatorMaint_2, MainUIPanel);
            FormAttachToPanel(m_FormOperatorMaint_3, MainUIPanel);

            FormAttachToPanel(m_FormMachineMaint, MainUIPanel);
            FormAttachToPanel(m_FormMachineMaint_1, MainUIPanel);
            FormAttachToPanel(m_FormMachineMaint_2, MainUIPanel);
            FormAttachToPanel(m_FormMachineMaint_3, MainUIPanel);

            FormAttachToPanel(m_FormEngineerMaint, MainUIPanel);
            FormAttachToPanel(m_AxisOperation, MainUIPanel);
            FormAttachToPanel(m_IOCheck, MainUIPanel);
            FormAttachToPanel(m_AxisParameter, MainUIPanel);

            FormAttachToPanel(m_FormLaserMaint, MainUIPanel);
            FormAttachToPanel(m_FormLaserMaint_1, MainUIPanel);
            FormAttachToPanel(m_FormLaserMaint_2, MainUIPanel);
            FormAttachToPanel(m_FormLaserMaint_3, MainUIPanel);
        }

        private void FormAttachToPanel(Form TargetForm, Panel panel)
        {
            TargetForm.TopLevel = false;
            TargetForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            TargetForm.Dock = DockStyle.Fill;

            panel.Controls.Add(TargetForm);

            TargetForm.Hide();
        }

        public bool InitializeLWDicer()
        {
            int iResult = LWDicer.Initialize(MainFrame);
            if(iResult != SUCCESS)
            {
                // Show Error Message & 프로그램 종료?

            }
            return true;
        }

        private void CMainFrame_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Program 종료를 위해 Thread Kill
            LWDicer.StopThreads();

            LWDicer.m_Scanner[0].LSEPortClose();

            this.Dispose();
            this.Close();
        }
    }
    public class PageInfo
    {
        public int BigClassifyingNumber { get; set; }
        public int SmallClassifyingNumber { get; set; }
    }

    public class BottomPageInfo
    {
        public int BigClassifyingNumber { get; set; }
        public int SmallClassifyingNumber { get; set; }
    }

}
