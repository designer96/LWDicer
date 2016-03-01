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
            Syncfusion.Windows.Forms.Grid.GridRangeStyle gridRangeStyle1 = new Syncfusion.Windows.Forms.Grid.GridRangeStyle();
            Syncfusion.Windows.Forms.Grid.GridRangeStyle gridRangeStyle2 = new Syncfusion.Windows.Forms.Grid.GridRangeStyle();
            Syncfusion.Windows.Forms.Grid.GridRangeStyle gridRangeStyle3 = new Syncfusion.Windows.Forms.Grid.GridRangeStyle();
            Syncfusion.Windows.Forms.Grid.GridRangeStyle gridRangeStyle4 = new Syncfusion.Windows.Forms.Grid.GridRangeStyle();
            this.BtnExit = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnNext = new Syncfusion.Windows.Forms.ButtonAdv();
            this.BtnConfigureSave = new System.Windows.Forms.Button();
            this.GridConfigure = new Syncfusion.Windows.Forms.Grid.GridControl();
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
            this.BtnConfigureSave.Location = new System.Drawing.Point(1125, 507);
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
            gridRangeStyle1.Range = Syncfusion.Windows.Forms.Grid.GridRangeInfo.Table();
            gridRangeStyle1.StyleInfo.Font.Bold = false;
            gridRangeStyle1.StyleInfo.Font.Facename = "Tahoma";
            gridRangeStyle1.StyleInfo.Font.Italic = false;
            gridRangeStyle1.StyleInfo.Font.Size = 9F;
            gridRangeStyle1.StyleInfo.Font.Strikeout = false;
            gridRangeStyle1.StyleInfo.Font.Underline = false;
            gridRangeStyle1.StyleInfo.Font.Unit = System.Drawing.GraphicsUnit.Point;
            gridRangeStyle2.Range = Syncfusion.Windows.Forms.Grid.GridRangeInfo.Table();
            gridRangeStyle2.StyleInfo.Font.Bold = false;
            gridRangeStyle2.StyleInfo.Font.Facename = "Tahoma";
            gridRangeStyle2.StyleInfo.Font.Italic = false;
            gridRangeStyle2.StyleInfo.Font.Size = 9F;
            gridRangeStyle2.StyleInfo.Font.Strikeout = false;
            gridRangeStyle2.StyleInfo.Font.Underline = false;
            gridRangeStyle2.StyleInfo.Font.Unit = System.Drawing.GraphicsUnit.Point;
            gridRangeStyle3.Range = Syncfusion.Windows.Forms.Grid.GridRangeInfo.Table();
            gridRangeStyle3.StyleInfo.Font.Bold = false;
            gridRangeStyle3.StyleInfo.Font.Facename = "Tahoma";
            gridRangeStyle3.StyleInfo.Font.Italic = false;
            gridRangeStyle3.StyleInfo.Font.Size = 9F;
            gridRangeStyle3.StyleInfo.Font.Strikeout = false;
            gridRangeStyle3.StyleInfo.Font.Underline = false;
            gridRangeStyle3.StyleInfo.Font.Unit = System.Drawing.GraphicsUnit.Point;
            gridRangeStyle4.Range = Syncfusion.Windows.Forms.Grid.GridRangeInfo.Table();
            gridRangeStyle4.StyleInfo.Font.Bold = false;
            gridRangeStyle4.StyleInfo.Font.Facename = "Tahoma";
            gridRangeStyle4.StyleInfo.Font.Italic = false;
            gridRangeStyle4.StyleInfo.Font.Size = 9F;
            gridRangeStyle4.StyleInfo.Font.Strikeout = false;
            gridRangeStyle4.StyleInfo.Font.Underline = false;
            gridRangeStyle4.StyleInfo.Font.Unit = System.Drawing.GraphicsUnit.Point;
            this.GridConfigure.RangeStyles.AddRange(new Syncfusion.Windows.Forms.Grid.GridRangeStyle[] {
            gridRangeStyle1,
            gridRangeStyle2,
            gridRangeStyle3,
            gridRangeStyle4});
            this.GridConfigure.SerializeCellsBehavior = Syncfusion.Windows.Forms.Grid.GridSerializeCellsBehavior.SerializeAsRangeStylesIntoCode;
            this.GridConfigure.Size = new System.Drawing.Size(1034, 707);
            this.GridConfigure.SmartSizeBox = false;
            this.GridConfigure.TabIndex = 11;
            this.GridConfigure.UseRightToLeftCompatibleTextBox = true;
            this.GridConfigure.CellClick += new Syncfusion.Windows.Forms.Grid.GridCellClickEventHandler(this.GridConfigure_CellClick);
            // 
            // FormLaserMaint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(1280, 754);
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
    }
}