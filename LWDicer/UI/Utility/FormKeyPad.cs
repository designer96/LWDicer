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
    public partial class FormKeyPad : Form
    {
        string strInput = "";
        string strCurrent = "";

        public FormKeyPad()
        {
            SetValue("");

            InitializeComponent();

            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);

            StartPosition = FormStartPosition.CenterScreen;
            MaximizeBox = false;
            MinimizeBox = false;
            FormBorderStyle = FormBorderStyle.Fixed3D;

            CGeneralUtils.AnimateEffect.AnimateWindow(this.Handle, 300, CGeneralUtils.AnimateEffect.AW_ACTIVATE | CGeneralUtils.AnimateEffect.AW_BLEND);
        }

        public void SetValue(string strValue)
        {
            strCurrent = strValue;
        }

        private void BtnNo_Click(object sender, EventArgs e)
        {
            double dTag = 0;
            Button Btn = sender as Button;

            dTag = Convert.ToDouble(Btn.Tag);

            strInput = strInput + Convert.ToDouble(dTag);

            UpdateDisplay(strInput);
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            int nNo = 0;

            if (strInput == "")
                return;

            nNo = strInput.Length - 1;
            strInput = strInput.Remove(nNo, 1);
            UpdateDisplay(strInput);
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            strInput = "";
            UpdateDisplay(strInput);
        }

        private void UpdateDisplay(string strNo)
        {
            ModifyNo.Text = strNo;
        }

        private void BtnComma_Click(object sender, EventArgs e)
        {
            if (strInput != "")
            {
                strInput = strInput + ".";
            }
        }

        private void BtnSign_Click(object sender, EventArgs e)
        {
            double nNo = 0;

            nNo = Convert.ToDouble(strInput);

            if (nNo > 0)
            {
                strInput = "-" + strInput;
            }
            else if (nNo < 0)
            {
                strInput = strInput.Replace("-", "");
            }

            UpdateDisplay(strInput);
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {

        }

        private void FormKeyPad_Load(object sender, EventArgs e)
        {
            PresentNo.Text = strCurrent;
            ModifyNo.Text = strCurrent;
            strInput = strCurrent;
        }
    }
}
