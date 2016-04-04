namespace LWDicer.UI
{
    partial class FormDeviceData
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDeviceData));
            this.BtnLayout = new System.Windows.Forms.Button();
            this.BtnClear = new System.Windows.Forms.Button();
            this.PointXY = new System.Windows.Forms.TextBox();
            this.LabelY = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.LabelX = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.PicWafer = new System.Windows.Forms.PictureBox();
            this.BtnNext = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnExit = new Syncfusion.Windows.Forms.ButtonAdv();
            this.ImagePolygon = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.PicWafer)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnLayout
            // 
            this.BtnLayout.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnLayout.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.BtnLayout.Image = ((System.Drawing.Image)(resources.GetObject("BtnLayout.Image")));
            this.BtnLayout.Location = new System.Drawing.Point(872, 686);
            this.BtnLayout.Name = "BtnLayout";
            this.BtnLayout.Size = new System.Drawing.Size(104, 61);
            this.BtnLayout.TabIndex = 42;
            this.BtnLayout.Text = "  LayOut";
            this.BtnLayout.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnLayout.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnLayout.UseVisualStyleBackColor = true;
            this.BtnLayout.Click += new System.EventHandler(this.BtnLayout_Click);
            // 
            // BtnClear
            // 
            this.BtnClear.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnClear.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.BtnClear.Image = ((System.Drawing.Image)(resources.GetObject("BtnClear.Image")));
            this.BtnClear.Location = new System.Drawing.Point(766, 686);
            this.BtnClear.Name = "BtnClear";
            this.BtnClear.Size = new System.Drawing.Size(104, 61);
            this.BtnClear.TabIndex = 41;
            this.BtnClear.Text = " Clear";
            this.BtnClear.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnClear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnClear.UseVisualStyleBackColor = true;
            this.BtnClear.Click += new System.EventHandler(this.BtnClear_Click);
            // 
            // PointXY
            // 
            this.PointXY.BackColor = System.Drawing.Color.White;
            this.PointXY.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.PointXY.Enabled = false;
            this.PointXY.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.PointXY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.PointXY.Location = new System.Drawing.Point(515, 10);
            this.PointXY.Name = "PointXY";
            this.PointXY.Size = new System.Drawing.Size(187, 15);
            this.PointXY.TabIndex = 37;
            // 
            // LabelY
            // 
            this.LabelY.BackgroundColor = new Syncfusion.Drawing.BrushInfo();
            this.LabelY.BorderAppearance = System.Windows.Forms.BorderStyle.None;
            this.LabelY.BorderColor = System.Drawing.Color.Transparent;
            this.LabelY.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.LabelY.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LabelY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.LabelY.Image = ((System.Drawing.Image)(resources.GetObject("LabelY.Image")));
            this.LabelY.Location = new System.Drawing.Point(23, 65);
            this.LabelY.Name = "LabelY";
            this.LabelY.Size = new System.Drawing.Size(10, 95);
            this.LabelY.TabIndex = 40;
            this.LabelY.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LabelX
            // 
            this.LabelX.BackgroundColor = new Syncfusion.Drawing.BrushInfo();
            this.LabelX.BorderAppearance = System.Windows.Forms.BorderStyle.None;
            this.LabelX.BorderColor = System.Drawing.Color.Transparent;
            this.LabelX.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.LabelX.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LabelX.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.LabelX.Image = ((System.Drawing.Image)(resources.GetObject("LabelX.Image")));
            this.LabelX.Location = new System.Drawing.Point(71, 10);
            this.LabelX.Name = "LabelX";
            this.LabelX.Size = new System.Drawing.Size(113, 15);
            this.LabelX.TabIndex = 39;
            this.LabelX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PicWafer
            // 
            this.PicWafer.BackColor = System.Drawing.Color.White;
            this.PicWafer.Cursor = System.Windows.Forms.Cursors.Cross;
            this.PicWafer.Location = new System.Drawing.Point(18, 7);
            this.PicWafer.Name = "PicWafer";
            this.PicWafer.Size = new System.Drawing.Size(740, 740);
            this.PicWafer.TabIndex = 38;
            this.PicWafer.TabStop = false;
            this.PicWafer.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PicWafer_MouseMove);
            // 
            // BtnNext
            // 
            this.BtnNext.Location = new System.Drawing.Point(1138, 584);
            this.BtnNext.Name = "BtnNext";
            this.BtnNext.Size = new System.Drawing.Size(125, 73);
            this.BtnNext.TabIndex = 36;
            this.BtnNext.Text = "Next";
            this.BtnNext.Click += new System.EventHandler(this.BtnNext_Click);
            // 
            // BtnExit
            // 
            this.BtnExit.Location = new System.Drawing.Point(1138, 663);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(125, 73);
            this.BtnExit.TabIndex = 35;
            this.BtnExit.Text = "Exit";
            this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // ImagePolygon
            // 
            this.ImagePolygon.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImagePolygon.ImageStream")));
            this.ImagePolygon.TransparentColor = System.Drawing.Color.Transparent;
            this.ImagePolygon.Images.SetKeyName(0, "Led_Off.png");
            this.ImagePolygon.Images.SetKeyName(1, "Led_On.png");
            // 
            // FormDeviceData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(1280, 754);
            this.Controls.Add(this.BtnLayout);
            this.Controls.Add(this.BtnClear);
            this.Controls.Add(this.PointXY);
            this.Controls.Add(this.LabelY);
            this.Controls.Add(this.LabelX);
            this.Controls.Add(this.PicWafer);
            this.Controls.Add(this.BtnNext);
            this.Controls.Add(this.BtnExit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormDeviceData";
            this.Text = "FormDeviceData";
            ((System.ComponentModel.ISupportInitialize)(this.PicWafer)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnLayout;
        private System.Windows.Forms.Button BtnClear;
        private System.Windows.Forms.TextBox PointXY;
        private Syncfusion.Windows.Forms.Tools.GradientLabel LabelY;
        private Syncfusion.Windows.Forms.Tools.GradientLabel LabelX;
        private System.Windows.Forms.PictureBox PicWafer;
        private Syncfusion.Windows.Forms.ButtonAdv BtnNext;
        private Syncfusion.Windows.Forms.ButtonAdv BtnExit;
        private System.Windows.Forms.ImageList ImagePolygon;
    }
}