using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LWDicer.Control;

using Syncfusion.Windows.Forms.Tools;
using Syncfusion.Drawing;

namespace LWDicer.UI
{
    public partial class FormAxisOperation : Form
    {
        private PageInfo PrevPage = null;
        private PageInfo NextPage = null;

        private GradientLabel[] MotorNo = new GradientLabel[19];
        private GradientLabel[] MotorPos = new GradientLabel[19];
        private GradientLabel[] MotorServo = new GradientLabel[19];
        private GradientLabel[] MotorNLimit = new GradientLabel[19];
        private GradientLabel[] MotorHome = new GradientLabel[19];
        private GradientLabel[] MotorPLimit = new GradientLabel[19];
        private GradientLabel[] MotorAlarm = new GradientLabel[19];

        private int nSelMotor;


        public void SetPrevPage(PageInfo page)
        {
            PrevPage = page;
        }

        public void SetNextPage(PageInfo page)
        {
            NextPage = page;
        }

        public FormAxisOperation()
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

            nSelMotor = -1;

            ResouceMapping();

            TmrMotor.Enabled = true;
            TmrMotor.Interval = 100;
            TmrMotor.Stop();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            if (PrevPage == null)
            {
                return;
            }

            CMainFrame.MainFrame.MoveToPage(PrevPage);
        }

        private void BtnServoOn_Click(object sender, EventArgs e)
        {
            if (GetdMotor() == -1)
            {
                return;
            }



        }

        private void BtnServoOff_Click(object sender, EventArgs e)
        {
            if (GetdMotor() == -1)
            {
                return;
            }


        }

        private void BtnServoOrigin_Click(object sender, EventArgs e)
        {
            if (GetdMotor() == -1)
            {
                return;
            }


        }

        private void ResouceMapping()
        {
            MotorNo[0] = Motor1; MotorNo[1] = Motor2; MotorNo[2] = Motor3; MotorNo[3] = Motor4; MotorNo[4] = Motor5;
            MotorNo[5] = Motor6; MotorNo[6] = Motor7; MotorNo[7] = Motor8; MotorNo[8] = Motor9; MotorNo[9] = Motor10;
            MotorNo[10] = Motor11; MotorNo[11] = Motor12; MotorNo[12] = Motor13; MotorNo[13] = Motor14; MotorNo[14] = Motor15;
            MotorNo[15] = Motor16; MotorNo[16] = Motor17; MotorNo[17] = Motor18; MotorNo[18] = Motor19;

            MotorPos[0] = Motor1_Pos; MotorPos[1] = Motor2_Pos; MotorPos[2] = Motor3_Pos; MotorPos[3] = Motor4_Pos; MotorPos[4] = Motor5_Pos;
            MotorPos[5] = Motor6_Pos; MotorPos[6] = Motor7_Pos; MotorPos[7] = Motor8_Pos; MotorPos[8] = Motor9_Pos; MotorPos[9] = Motor10_Pos;
            MotorPos[10] = Motor11_Pos; MotorPos[11] = Motor12_Pos; MotorPos[12] = Motor13_Pos; MotorPos[13] = Motor14_Pos; MotorPos[14] = Motor15_Pos;
            MotorPos[15] = Motor16_Pos; MotorPos[16] = Motor17_Pos; MotorPos[17] = Motor18_Pos; MotorPos[18] = Motor19_Pos;

            MotorServo[0] = Motor1_Servo; MotorServo[1] = Motor2_Servo; MotorServo[2] = Motor3_Servo; MotorServo[3] = Motor4_Servo; MotorServo[4] = Motor5_Servo;
            MotorServo[5] = Motor6_Servo; MotorServo[6] = Motor7_Servo; MotorServo[7] = Motor8_Servo; MotorServo[8] = Motor9_Servo; MotorServo[9] = Motor10_Servo;
            MotorServo[10] = Motor11_Servo; MotorServo[11] = Motor12_Servo; MotorServo[12] = Motor13_Servo; MotorServo[13] = Motor14_Servo; MotorServo[14] = Motor15_Servo;
            MotorServo[15] = Motor16_Servo; MotorServo[16] = Motor17_Servo; MotorServo[17] = Motor18_Servo; MotorServo[18] = Motor19_Servo;

            MotorNLimit[0] = Motor1_N_Limit; MotorNLimit[1] = Motor2_N_Limit; MotorNLimit[2] = Motor3_N_Limit; MotorNLimit[3] = Motor4_N_Limit; MotorNLimit[4] = Motor5_N_Limit;
            MotorNLimit[5] = Motor6_N_Limit; MotorNLimit[6] = Motor7_N_Limit; MotorNLimit[7] = Motor8_N_Limit; MotorNLimit[8] = Motor9_N_Limit; MotorNLimit[9] = Motor10_N_Limit;
            MotorNLimit[10] = Motor11_N_Limit; MotorNLimit[11] = Motor12_N_Limit; MotorNLimit[12] = Motor13_N_Limit; MotorNLimit[13] = Motor14_N_Limit; MotorNLimit[14] = Motor15_N_Limit;
            MotorNLimit[15] = Motor16_N_Limit; MotorNLimit[16] = Motor17_N_Limit; MotorNLimit[17] = Motor18_N_Limit; MotorNLimit[18] = Motor19_N_Limit;

            MotorHome[0] = Motor1_Home; MotorHome[1] = Motor2_Home; MotorHome[2] = Motor3_Home; MotorHome[3] = Motor4_Home; MotorHome[4] = Motor5_Home;
            MotorHome[5] = Motor6_Home; MotorHome[6] = Motor7_Home; MotorHome[7] = Motor8_Home; MotorHome[8] = Motor9_Home; MotorHome[9] = Motor10_Home;
            MotorHome[10] = Motor11_Home; MotorHome[11] = Motor12_Home; MotorHome[12] = Motor13_Home; MotorHome[13] = Motor14_Home; MotorHome[14] = Motor15_Home;
            MotorHome[15] = Motor16_Home; MotorHome[16] = Motor17_Home; MotorHome[17] = Motor18_Home; MotorHome[18] = Motor19_Home;

            MotorPLimit[0] = Motor1_P_Limit; MotorPLimit[1] = Motor2_P_Limit; MotorPLimit[2] = Motor3_P_Limit; MotorPLimit[3] = Motor4_P_Limit; MotorPLimit[4] = Motor5_P_Limit;
            MotorPLimit[5] = Motor6_P_Limit; MotorPLimit[6] = Motor7_P_Limit; MotorPLimit[7] = Motor8_P_Limit; MotorPLimit[8] = Motor9_P_Limit; MotorPLimit[9] = Motor10_P_Limit;
            MotorPLimit[10] = Motor11_P_Limit; MotorPLimit[11] = Motor12_P_Limit; MotorPLimit[12] = Motor13_P_Limit; MotorPLimit[13] = Motor14_P_Limit; MotorPLimit[14] = Motor15_P_Limit;
            MotorPLimit[15] = Motor16_P_Limit; MotorPLimit[16] = Motor17_P_Limit; MotorPLimit[17] = Motor18_P_Limit; MotorPLimit[18] = Motor19_P_Limit;

            MotorAlarm[0] = Motor1_Alarm; MotorAlarm[1] = Motor2_Alarm; MotorAlarm[2] = Motor3_Alarm; MotorAlarm[3] = Motor4_Alarm; MotorAlarm[4] = Motor5_Alarm;
            MotorAlarm[5] = Motor6_Alarm; MotorAlarm[6] = Motor7_Alarm; MotorAlarm[7] = Motor8_Alarm; MotorAlarm[8] = Motor9_Alarm; MotorAlarm[9] = Motor10_Alarm;
            MotorAlarm[10] = Motor11_Alarm; MotorAlarm[11] = Motor12_Alarm; MotorAlarm[12] = Motor13_Alarm; MotorAlarm[13] = Motor14_Alarm; MotorAlarm[14] = Motor15_Alarm;
            MotorAlarm[15] = Motor16_Alarm; MotorAlarm[16] = Motor17_Alarm; MotorAlarm[17] = Motor18_Alarm; MotorAlarm[18] = Motor19_Alarm;
        }

        private void Motor_Click(object sender, EventArgs e)
        {
            int nNo = 0, i = 0;

            GradientLabel MotorLabel = sender as GradientLabel;

            nNo = Convert.ToInt16(MotorLabel.Tag);

            for (i = 0; i < 19; i++)
            {
                MotorNo[i].BackgroundColor = new BrushInfo(Color.White);
            }

            MotorLabel.BackgroundColor = new BrushInfo(Color.LightSeaGreen);

            SetMotor(nNo);
        }

        private void TmrMotor_Tick(object sender, EventArgs e)
        {
            int i = 0;
            double dPos = 0;

            for (i = 0; i < 19; i++)
            {
                // 1.Encoder Position
                MotorPos[i].Text = string.Format("{0:f4}", dPos);

                // 2.Servo On, Off
                MotorServo[i].Text = "Off";

                // 3.- Limit Sensor
                MotorNLimit[i].Text = "Off";

                // 4.Home Sensor
                MotorHome[i].Text = "Off";

                // 5.+Limit Sensor
                MotorPLimit[i].Text = "Off";

                // 6.Driver Alarm
                MotorAlarm[i].Text = "ER0001";
            }
        }

        private void SetMotor(int nMotorNo)
        {
            nSelMotor = nMotorNo;
        }

        private int GetdMotor()
        {
            return nSelMotor;
        }

        private void FormAxisOperation_Enter(object sender, EventArgs e)
        {
            TmrMotor.Start();
        }

        private void FormAxisOperation_Leave(object sender, EventArgs e)
        {
            TmrMotor.Stop();
        }

        private void BtnMotorPara_Click(object sender, EventArgs e)
        {
            if (NextPage == null)
            {
                return;
            }

            CMainFrame.MainFrame.MoveToPage(NextPage);
        }
    }
}
