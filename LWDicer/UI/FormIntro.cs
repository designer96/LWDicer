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
    public partial class FormIntro : Form
    {
        public FormIntro()
        {
            InitializeComponent();
            StatusBar.Enabled = true;
            LabelStatus.Enabled = true;
        }

        public void SetStatus(string strText, int nProgress)
        {
            StatusBar.Value = nProgress;
            LabelStatus.Text = strText;
        }

        private void FormIntro_Load(object sender, EventArgs e)
        {
            CUtils.AnimateEffect.AnimateWindow(this.Handle, 1500, CUtils.AnimateEffect.AW_ACTIVATE | CUtils.AnimateEffect.AW_BLEND);
        }
    }
}
