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
    public partial class FormSubBottom : Form
    {
        public FormSubBottom()
        {
            InitializeComponent();
        }

        protected virtual void InitializeForm()
        {
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.DesktopLocation = new Point(DEF_UI.SUB_BOT_POS_X, DEF_UI.SUB_BOT_POS_Y);
            this.Size = new Size(DEF_UI.SUB_BOT_WIDTH, DEF_UI.SUB_BOT_HEIGHT);
            this.FormBorderStyle = FormBorderStyle.None;
        }
    }
}
