namespace LWDicer.UI
{
    partial class FormLaserMaint
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLaserMaint));
            Syncfusion.Windows.Forms.Grid.GridRangeStyle gridRangeStyle13 = new Syncfusion.Windows.Forms.Grid.GridRangeStyle();
            Syncfusion.Windows.Forms.Grid.GridRangeStyle gridRangeStyle14 = new Syncfusion.Windows.Forms.Grid.GridRangeStyle();
            Syncfusion.Windows.Forms.Grid.GridRangeStyle gridRangeStyle15 = new Syncfusion.Windows.Forms.Grid.GridRangeStyle();
            Syncfusion.Windows.Forms.Grid.GridRangeStyle gridRangeStyle16 = new Syncfusion.Windows.Forms.Grid.GridRangeStyle();
            Syncfusion.Windows.Forms.Grid.GridRangeStyle gridRangeStyle17 = new Syncfusion.Windows.Forms.Grid.GridRangeStyle();
            Syncfusion.Windows.Forms.Grid.GridRangeStyle gridRangeStyle18 = new Syncfusion.Windows.Forms.Grid.GridRangeStyle();
            this.BtnExit = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnNext = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnConfigureSave = new System.Windows.Forms.Button();
            this.GridConfigure = new Syncfusion.Windows.Forms.Grid.GridControl();
            this.TitleEndX = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.TitleStartX = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.LabelPort = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            this.LabelIP = new Syncfusion.Windows.Forms.Tools.GradientLabel();
            ((System.ComponentModel.ISupportInitialize)(this.GridConfigure)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnExit
            // 
            this.BtnExit.Location = new System.Drawing.Point(1128, 663);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(125, 73);
            this.BtnExit.TabIndex = 2;
            this.BtnExit.Text = "Exit";
            this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // BtnNext
            // 
            this.BtnNext.Location = new System.Drawing.Point(1128, 584);
            this.BtnNext.Name = "BtnNext";
            this.BtnNext.Size = new System.Drawing.Size(125, 73);
            this.BtnNext.TabIndex = 9;
            this.BtnNext.Text = "Next";
            this.BtnNext.Click += new System.EventHandler(this.BtnNext_Click);
            // 
            // BtnConfigureSave
            // 
            this.BtnConfigureSave.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnConfigureSave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.BtnConfigureSave.Image = ((System.Drawing.Image)(resources.GetObject("BtnConfigureSave.Image")));
            this.BtnConfigureSave.Location = new System.Drawing.Point(1128, 507);
            this.BtnConfigureSave.Name = "BtnConfigureSave";
            this.BtnConfigureSave.Size = new System.Drawing.Size(125, 73);
            this.BtnConfigureSave.TabIndex = 12;
            this.BtnConfigureSave.Text = "  Save";
            this.BtnConfigureSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnConfigureSave.UseVisualStyleBackColor = true;
            this.BtnConfigureSave.Click += new System.EventHandler(this.BtnConfigureSave_Click);
            // 
            // GridConfigure
            // 
            this.GridConfigure.ActivateCurrentCellBehavior = Syncfusion.Windows.Forms.Grid.GridCellActivateAction.None;
            this.GridConfigure.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.GridConfigure.Font = new System.Drawing.Font("Tahoma", 9F);
            this.GridConfigure.Location = new System.Drawing.Point(30, 24);
            this.GridConfigure.Name = "GridConfigure";
            gridRangeStyle13.Range = Syncfusion.Windows.Forms.Grid.GridRangeInfo.Table();
            gridRangeStyle13.StyleInfo.Font.Bold = false;
            gridRangeStyle13.StyleInfo.Font.Facename = "Tahoma";
            gridRangeStyle13.StyleInfo.Font.Italic = false;
            gridRangeStyle13.StyleInfo.Font.Size = 9F;
            gridRangeStyle13.StyleInfo.Font.Strikeout = false;
            gridRangeStyle13.StyleInfo.Font.Underline = false;
            gridRangeStyle13.StyleInfo.Font.Unit = System.Drawing.GraphicsUnit.Point;
            gridRangeStyle14.Range = Syncfusion.Windows.Forms.Grid.GridRangeInfo.Table();
            gridRangeStyle14.StyleInfo.Font.Bold = false;
            gridRangeStyle14.StyleInfo.Font.Facename = "Tahoma";
            gridRangeStyle14.StyleInfo.Font.Italic = false;
            gridRangeStyle14.StyleInfo.Font.Size = 9F;
            gridRangeStyle14.StyleInfo.Font.Strikeout = false;
            gridRangeStyle14.StyleInfo.Font.Underline = false;
            gridRangeStyle14.StyleInfo.Font.Unit = System.Drawing.GraphicsUnit.Point;
            gridRangeStyle15.Range = Syncfusion.Windows.Forms.Grid.GridRangeInfo.Table();
            gridRangeStyle15.StyleInfo.Font.Bold = false;
            gridRangeStyle15.StyleInfo.Font.Facename = "Tahoma";
            gridRangeStyle15.StyleInfo.Font.Italic = false;
            gridRangeStyle15.StyleInfo.Font.Size = 9F;
            gridRangeStyle15.StyleInfo.Font.Strikeout = false;
            gridRangeStyle15.StyleInfo.Font.Underline = false;
            gridRangeStyle15.StyleInfo.Font.Unit = System.Drawing.GraphicsUnit.Point;
            gridRangeStyle16.Range = Syncfusion.Windows.Forms.Grid.GridRangeInfo.Table();
            gridRangeStyle16.StyleInfo.Font.Bold = false;
            gridRangeStyle16.StyleInfo.Font.Facename = "Tahoma";
            gridRangeStyle16.StyleInfo.Font.Italic = false;
            gridRangeStyle16.StyleInfo.Font.Size = 9F;
            gridRangeStyle16.StyleInfo.Font.Strikeout = false;
            gridRangeStyle16.StyleInfo.Font.Underline = false;
            gridRangeStyle16.StyleInfo.Font.Unit = System.Drawing.GraphicsUnit.Point;
            gridRangeStyle17.Range = Syncfusion.Windows.Forms.Grid.GridRangeInfo.Table();
            gridRangeStyle17.StyleInfo.Font.Bold = false;
            gridRangeStyle17.StyleInfo.Font.Facename = "Tahoma";
            gridRangeStyle17.StyleInfo.Font.Italic = false;
            gridRangeStyle17.StyleInfo.Font.Size = 9F;
            gridRangeStyle17.StyleInfo.Font.Strikeout = false;
            gridRangeStyle17.StyleInfo.Font.Underline = false;
            gridRangeStyle17.StyleInfo.Font.Unit = System.Drawing.GraphicsUnit.Point;
            gridRangeStyle18.Range = Syncfusion.Windows.Forms.Grid.GridRangeInfo.Table();
            gridRangeStyle18.StyleInfo.Font.Bold = false;
            gridRangeStyle18.StyleInfo.Font.Facename = "Tahoma";
            gridRangeStyle18.StyleInfo.Font.Italic = false;
            gridRangeStyle18.StyleInfo.Font.Size = 9F;
            gridRangeStyle18.StyleInfo.Font.Strikeout = false;
            gridRangeStyle18.StyleInfo.Font.Underline = false;
            gridRangeStyle18.StyleInfo.Font.Unit = System.Drawing.GraphicsUnit.Point;
            this.GridConfigure.RangeStyles.AddRange(new Syncfusion.Windows.Forms.Grid.GridRangeStyle[] {
            gridRangeStyle13,
            gridRangeStyle14,
            gridRangeStyle15,
            gridRangeStyle16,
            gridRangeStyle17,
            gridRangeStyle18});
            this.GridConfigure.SerializeCellsBehavior = Syncfusion.Windows.Forms.Grid.GridSerializeCellsBehavior.SerializeAsRangeStylesIntoCode;
            this.GridConfigure.Size = new System.Drawing.Size(1034, 707);
            this.GridConfigure.SmartSizeBox = false;
            this.GridConfigure.TabIndex = 11;
            this.GridConfigure.UseRightToLeftCompatibleTextBox = true;
            this.GridConfigure.CellClick += new Syncfusion.Windows.Forms.Grid.GridCellClickEventHandler(this.GridConfigure_CellClick);
            // 
            // TitleEndX
            // 
            this.TitleEndX.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.Goldenrod);
            this.TitleEndX.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.TitleEndX.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.TitleEndX.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.TitleEndX.ForeColor = System.Drawing.Color.Black;
            this.TitleEndX.Location = new System.Drawing.Point(1108, 91);
            this.TitleEndX.Name = "TitleEndX";
            this.TitleEndX.Size = new System.Drawing.Size(165, 32);
            this.TitleEndX.TabIndex = 27;
            this.TitleEndX.Text = "Port";
            this.TitleEndX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TitleStartX
            // 
            this.TitleStartX.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.LightSalmon);
            this.TitleStartX.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.TitleStartX.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.TitleStartX.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.TitleStartX.ForeColor = System.Drawing.Color.Black;
            this.TitleStartX.Location = new System.Drawing.Point(1108, 24);
            this.TitleStartX.Name = "TitleStartX";
            this.TitleStartX.Size = new System.Drawing.Size(165, 32);
            this.TitleStartX.TabIndex = 26;
            this.TitleStartX.Text = "IP Address";
            this.TitleStartX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LabelPort
            // 
            this.LabelPort.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.White);
            this.LabelPort.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.LabelPort.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LabelPort.ForeColor = System.Drawing.Color.Black;
            this.LabelPort.Location = new System.Drawing.Point(1109, 123);
            this.LabelPort.Name = "LabelPort";
            this.LabelPort.Size = new System.Drawing.Size(164, 32);
            this.LabelPort.TabIndex = 25;
            this.LabelPort.Text = "21";
            this.LabelPort.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.LabelPort.Click += new System.EventHandler(this.LabelPort_Click);
            // 
            // LabelIP
            // 
            this.LabelIP.BackgroundColor = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.White);
            this.LabelIP.BorderSides = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Right) 
            | System.Windows.Forms.Border3DSide.Bottom)));
            this.LabelIP.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LabelIP.ForeColor = System.Drawing.Color.Black;
            this.LabelIP.Location = new System.Drawing.Point(1109, 56);
            this.LabelIP.Name = "LabelIP";
            this.LabelIP.Size = new System.Drawing.Size(164, 32);
            this.LabelIP.TabIndex = 24;
            this.LabelIP.Text = "192.168.22.48";
            this.LabelIP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.LabelIP.Click += new System.EventHandler(this.LabelIP_Click);
            // 
            // FormLaserMaint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(1280, 754);
            this.Controls.Add(this.TitleEndX);
            this.Controls.Add(this.TitleStartX);
            this.Controls.Add(this.LabelPort);
            this.Controls.Add(this.LabelIP);
            this.Controls.Add(this.BtnConfigureSave);
            this.Controls.Add(this.GridConfigure);
            this.Controls.Add(this.BtnNext);
            this.Controls.Add(this.BtnExit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormLaserMaint";
            this.Text = "FormLaserMaint";
            this.Load += new System.EventHandler(this.FormLaserMaint_Load);
            ((System.ComponentModel.ISupportInitialize)(this.GridConfigure)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Syncfusion.Windows.Forms.ButtonAdv BtnExit;
        private Syncfusion.Windows.Forms.ButtonAdv BtnNext;
        private System.Windows.Forms.Button BtnConfigureSave;
        private Syncfusion.Windows.Forms.Grid.GridControl GridConfigure;
        private Syncfusion.Windows.Forms.Tools.GradientLabel TitleEndX;
        private Syncfusion.Windows.Forms.Tools.GradientLabel TitleStartX;
        private Syncfusion.Windows.Forms.Tools.GradientLabel LabelPort;
        private Syncfusion.Windows.Forms.Tools.GradientLabel LabelIP;
    }
}