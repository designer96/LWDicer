namespace LWDicer.UI
{
    partial class FormEngineerMaint
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEngineerMaint));
            this.BtnExit = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnIOCheck = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnAxisOP = new Syncfusion.Windows.Forms.ButtonAdv();
            this.SuspendLayout();
            // 
            // BtnExit
            // 
            this.BtnExit.Location = new System.Drawing.Point(1128, 663);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(125, 73);
            this.BtnExit.TabIndex = 1;
            this.BtnExit.Text = "Exit";
            this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // BtnIOCheck
            // 
            this.BtnIOCheck.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnIOCheck.ForeColor = System.Drawing.Color.MidnightBlue;
            this.BtnIOCheck.Image = ((System.Drawing.Image)(resources.GetObject("BtnIOCheck.Image")));
            this.BtnIOCheck.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.BtnIOCheck.Location = new System.Drawing.Point(312, 115);
            this.BtnIOCheck.Name = "BtnIOCheck";
            this.BtnIOCheck.Size = new System.Drawing.Size(163, 152);
            this.BtnIOCheck.TabIndex = 11;
            this.BtnIOCheck.Text = "\r\nI/O Check";
            this.BtnIOCheck.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.BtnIOCheck.Click += new System.EventHandler(this.BtnIOCheck_Click);
            // 
            // BtnAxisOP
            // 
            this.BtnAxisOP.BackColor = System.Drawing.SystemColors.Control;
            this.BtnAxisOP.FlatAppearance.BorderSize = 5;
            this.BtnAxisOP.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnAxisOP.ForeColor = System.Drawing.Color.MidnightBlue;
            this.BtnAxisOP.Image = ((System.Drawing.Image)(resources.GetObject("BtnAxisOP.Image")));
            this.BtnAxisOP.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.BtnAxisOP.Location = new System.Drawing.Point(108, 115);
            this.BtnAxisOP.Name = "BtnAxisOP";
            this.BtnAxisOP.Size = new System.Drawing.Size(163, 152);
            this.BtnAxisOP.TabIndex = 10;
            this.BtnAxisOP.Text = "\r\nAxis Operation";
            this.BtnAxisOP.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.BtnAxisOP.Click += new System.EventHandler(this.BtnAxisOP_Click);
            // 
            // FormEngineerMaint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(1280, 754);
            this.Controls.Add(this.BtnIOCheck);
            this.Controls.Add(this.BtnAxisOP);
            this.Controls.Add(this.BtnExit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormEngineerMaint";
            this.Text = "FormEngineerMaint";
            this.ResumeLayout(false);

        }

        #endregion

        private Syncfusion.Windows.Forms.ButtonAdv BtnExit;
        private Syncfusion.Windows.Forms.ButtonAdv BtnIOCheck;
        private Syncfusion.Windows.Forms.ButtonAdv BtnAxisOP;
    }
}