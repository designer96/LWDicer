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
    public partial class FormKeyBoard : Form
    {
        string strInput = "";

        public FormKeyBoard()
        {
            SetValue("");

            InitializeComponent();

            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.Fixed3D;

            MaximizeBox = false;
            MinimizeBox = false;

            CUtils.AnimateEffect.AnimateWindow(this.Handle, 300, CUtils.AnimateEffect.AW_ACTIVATE | CUtils.AnimateEffect.AW_BLEND);
        }

        public void SetValue(string strValue)
        {
            strInput = strValue;
        }

        private void BtnNo_Click(object sender, EventArgs e)
        {
            double dTag = 0;
            Button Btn = sender as Button;

            dTag = Convert.ToDouble(Btn.Tag);

            strInput = strInput + Convert.ToDouble(dTag);

            UpdateDisplay(strInput);
        }
        private void btn_Char_Click(object sender, EventArgs e)
        {
            Button Btn = sender as Button;
            strInput = strInput + Btn.Text;
            UpdateDisplay(strInput);

        }

        private void UpdateDisplay(string strNo)
        {
            PresentNo.Text = strNo;
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
    }
}
