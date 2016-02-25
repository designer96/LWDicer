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
    public partial class FormTop : Form
    {
        public static FormTop TopMenu = null;

        public FormTop()
        {
            InitializeComponent();

            InitializeForm();

            TopMenu = this;
        }

        protected virtual void InitializeForm()
        {
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.DesktopLocation = new Point(DEF_UI.TOP_POS_X, DEF_UI.TOP_POS_Y);
            this.Size = new Size(DEF_UI.TOP_SIZE_WIDTH, DEF_UI.TOP_SIZE_HEIGHT);
            this.FormBorderStyle = FormBorderStyle.None;

            tmFormTop.Interval = 1000;
            tmFormTop.Enabled = true;
            tmFormTop.Start();
        }

        private void tmFormTop_Tick(object sender, EventArgs e)
        {
            TextTime.Text = DateTime.Now.ToString("yyyy-MM-dd [ddd] <tt> HH:mm:ss");
        }

        public void SetMessage(string strMsg)
        {
            TextMessage.Text = strMsg;
        }

    }
}
