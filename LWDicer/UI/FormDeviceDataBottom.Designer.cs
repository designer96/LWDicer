namespace LWDicer.UI
{
    partial class FormDeviceDataBottom
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDeviceDataBottom));
            this.BtnDraw = new System.Windows.Forms.Button();
            this.BtnClear = new System.Windows.Forms.Button();
            this.BtnLayout = new System.Windows.Forms.Button();
            this.ImagePolygon = new System.Windows.Forms.ImageList(this.components);
            this.BtnImageSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BtnDraw
            // 
            this.BtnDraw.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnDraw.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.BtnDraw.Image = ((System.Drawing.Image)(resources.GetObject("BtnDraw.Image")));
            this.BtnDraw.Location = new System.Drawing.Point(26, 14);
            this.BtnDraw.Name = "BtnDraw";
            this.BtnDraw.Size = new System.Drawing.Size(124, 61);
            this.BtnDraw.TabIndex = 431;
            this.BtnDraw.Text = " Draw";
            this.BtnDraw.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnDraw.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnDraw.UseVisualStyleBackColor = true;
            this.BtnDraw.Click += new System.EventHandler(this.BtnDraw_Click);
            // 
            // BtnClear
            // 
            this.BtnClear.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnClear.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.BtnClear.Image = ((System.Drawing.Image)(resources.GetObject("BtnClear.Image")));
            this.BtnClear.Location = new System.Drawing.Point(170, 14);
            this.BtnClear.Name = "BtnClear";
            this.BtnClear.Size = new System.Drawing.Size(124, 61);
            this.BtnClear.TabIndex = 432;
            this.BtnClear.Text = " Clear";
            this.BtnClear.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnClear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnClear.UseVisualStyleBackColor = true;
            this.BtnClear.Click += new System.EventHandler(this.BtnClear_Click);
            // 
            // BtnLayout
            // 
            this.BtnLayout.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnLayout.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.BtnLayout.Image = ((System.Drawing.Image)(resources.GetObject("BtnLayout.Image")));
            this.BtnLayout.Location = new System.Drawing.Point(314, 14);
            this.BtnLayout.Name = "BtnLayout";
            this.BtnLayout.Size = new System.Drawing.Size(124, 61);
            this.BtnLayout.TabIndex = 433;
            this.BtnLayout.Text = "  LayOut";
            this.BtnLayout.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnLayout.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnLayout.UseVisualStyleBackColor = true;
            this.BtnLayout.Click += new System.EventHandler(this.BtnLayout_Click);
            // 
            // ImagePolygon
            // 
            this.ImagePolygon.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImagePolygon.ImageStream")));
            this.ImagePolygon.TransparentColor = System.Drawing.Color.Transparent;
            this.ImagePolygon.Images.SetKeyName(0, "Led_Off.png");
            this.ImagePolygon.Images.SetKeyName(1, "Led_On.png");
            // 
            // BtnImageSave
            // 
            this.BtnImageSave.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnImageSave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.BtnImageSave.Image = ((System.Drawing.Image)(resources.GetObject("BtnImageSave.Image")));
            this.BtnImageSave.Location = new System.Drawing.Point(458, 14);
            this.BtnImageSave.Name = "BtnImageSave";
            this.BtnImageSave.Size = new System.Drawing.Size(124, 61);
            this.BtnImageSave.TabIndex = 434;
            this.BtnImageSave.Text = "  Image Save";
            this.BtnImageSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnImageSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnImageSave.UseVisualStyleBackColor = true;
            this.BtnImageSave.Click += new System.EventHandler(this.BtnImageSave_Click);
            // 
            // FormDeviceDataBottom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(1099, 168);
            this.Controls.Add(this.BtnImageSave);
            this.Controls.Add(this.BtnLayout);
            this.Controls.Add(this.BtnClear);
            this.Controls.Add(this.BtnDraw);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormDeviceDataBottom";
            this.Text = "FormDeviceDataBottom";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnDraw;
        private System.Windows.Forms.Button BtnClear;
        private System.Windows.Forms.Button BtnLayout;
        private System.Windows.Forms.ImageList ImagePolygon;
        private System.Windows.Forms.Button BtnImageSave;
    }
}