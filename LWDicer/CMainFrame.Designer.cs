namespace LWDicer.UI
{
    partial class CMainFrame
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.PanelSubBottom = new System.Windows.Forms.Panel();
            this.PanelTop = new System.Windows.Forms.Panel();
            this.PanelBottom = new System.Windows.Forms.Panel();
            this.MainUIPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // PanelSubBottom
            // 
            this.PanelSubBottom.Location = new System.Drawing.Point(1102, 876);
            this.PanelSubBottom.Name = "PanelSubBottom";
            this.PanelSubBottom.Size = new System.Drawing.Size(173, 143);
            this.PanelSubBottom.TabIndex = 18;
            // 
            // PanelTop
            // 
            this.PanelTop.Location = new System.Drawing.Point(0, 4);
            this.PanelTop.Name = "PanelTop";
            this.PanelTop.Size = new System.Drawing.Size(1275, 93);
            this.PanelTop.TabIndex = 17;
            // 
            // PanelBottom
            // 
            this.PanelBottom.Location = new System.Drawing.Point(1, 876);
            this.PanelBottom.Name = "PanelBottom";
            this.PanelBottom.Size = new System.Drawing.Size(1099, 143);
            this.PanelBottom.TabIndex = 16;
            // 
            // MainUIPanel
            // 
            this.MainUIPanel.Location = new System.Drawing.Point(0, 100);
            this.MainUIPanel.Name = "MainUIPanel";
            this.MainUIPanel.Size = new System.Drawing.Size(1275, 773);
            this.MainUIPanel.TabIndex = 15;
            // 
            // CMainFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 1024);
            this.Controls.Add(this.PanelSubBottom);
            this.Controls.Add(this.PanelTop);
            this.Controls.Add(this.PanelBottom);
            this.Controls.Add(this.MainUIPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CMainFrame";
            this.Text = "SFA Wafer Dicing";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CMainFrame_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel PanelSubBottom;
        private System.Windows.Forms.Panel PanelTop;
        private System.Windows.Forms.Panel PanelBottom;
        private System.Windows.Forms.Panel MainUIPanel;
    }
}

