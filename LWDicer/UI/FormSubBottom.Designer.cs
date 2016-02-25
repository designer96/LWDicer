namespace LWDicer.UI
{
    partial class FormSubBottom
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSubBottom));
            this.BtnFullAuto = new Syncfusion.Windows.Forms.ButtonAdv();
            this.buttonAdv2 = new Syncfusion.Windows.Forms.ButtonAdv();
            this.CheckStage = new System.Windows.Forms.CheckBox();
            this.CheckDirect = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // BtnFullAuto
            // 
            this.BtnFullAuto.BackColor = System.Drawing.SystemColors.Control;
            this.BtnFullAuto.FlatAppearance.BorderSize = 5;
            this.BtnFullAuto.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnFullAuto.ForeColor = System.Drawing.Color.MidnightBlue;
            this.BtnFullAuto.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.BtnFullAuto.Location = new System.Drawing.Point(7, 5);
            this.BtnFullAuto.Name = "BtnFullAuto";
            this.BtnFullAuto.Size = new System.Drawing.Size(81, 78);
            this.BtnFullAuto.TabIndex = 4;
            this.BtnFullAuto.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // buttonAdv2
            // 
            this.buttonAdv2.BackColor = System.Drawing.SystemColors.Control;
            this.buttonAdv2.FlatAppearance.BorderSize = 5;
            this.buttonAdv2.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonAdv2.ForeColor = System.Drawing.Color.MidnightBlue;
            this.buttonAdv2.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonAdv2.Location = new System.Drawing.Point(90, 5);
            this.buttonAdv2.Name = "buttonAdv2";
            this.buttonAdv2.Size = new System.Drawing.Size(81, 78);
            this.buttonAdv2.TabIndex = 6;
            this.buttonAdv2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // CheckStage
            // 
            this.CheckStage.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.CheckStage.Appearance = System.Windows.Forms.Appearance.Button;
            this.CheckStage.Image = ((System.Drawing.Image)(resources.GetObject("CheckStage.Image")));
            this.CheckStage.Location = new System.Drawing.Point(7, 84);
            this.CheckStage.Name = "CheckStage";
            this.CheckStage.Size = new System.Drawing.Size(81, 78);
            this.CheckStage.TabIndex = 10;
            this.CheckStage.UseVisualStyleBackColor = true;
            // 
            // CheckDirect
            // 
            this.CheckDirect.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.CheckDirect.Appearance = System.Windows.Forms.Appearance.Button;
            this.CheckDirect.Image = ((System.Drawing.Image)(resources.GetObject("CheckDirect.Image")));
            this.CheckDirect.Location = new System.Drawing.Point(90, 84);
            this.CheckDirect.Name = "CheckDirect";
            this.CheckDirect.Size = new System.Drawing.Size(81, 78);
            this.CheckDirect.TabIndex = 11;
            this.CheckDirect.UseVisualStyleBackColor = true;
            // 
            // FormSubBottom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(178, 168);
            this.Controls.Add(this.CheckDirect);
            this.Controls.Add(this.CheckStage);
            this.Controls.Add(this.buttonAdv2);
            this.Controls.Add(this.BtnFullAuto);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormSubBottom";
            this.Text = "FormSubBottom";
            this.ResumeLayout(false);

        }

        #endregion

        private Syncfusion.Windows.Forms.ButtonAdv BtnFullAuto;
        private Syncfusion.Windows.Forms.ButtonAdv buttonAdv2;
        private System.Windows.Forms.CheckBox CheckStage;
        private System.Windows.Forms.CheckBox CheckDirect;
    }
}