namespace Balya_Yerleştirme
{
    partial class SelectLayouts
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
            components = new System.ComponentModel.Container();
            lbl_Layout_Sec_Title = new Krypton.Toolkit.KryptonWrapLabel();
            SelectLayoutPanel = new FlowLayoutPanel();
            progressBarPanel = new Panel();
            kryptonWrapLabel1 = new Krypton.Toolkit.KryptonWrapLabel();
            progressBar = new ProgressBar();
            contextMenuStrip = new ContextMenuStrip(components);
            layoutuSilToolStripMenuItem = new ToolStripMenuItem();
            timer = new System.Windows.Forms.Timer(components);
            progressBarPanel.SuspendLayout();
            contextMenuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // lbl_Layout_Sec_Title
            // 
            lbl_Layout_Sec_Title.AutoSize = false;
            lbl_Layout_Sec_Title.Font = new Font("Microsoft Sans Serif", 20F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lbl_Layout_Sec_Title.ForeColor = Color.Red;
            lbl_Layout_Sec_Title.LabelStyle = Krypton.Toolkit.LabelStyle.AlternateControl;
            lbl_Layout_Sec_Title.Location = new Point(426, 9);
            lbl_Layout_Sec_Title.Name = "lbl_Layout_Sec_Title";
            lbl_Layout_Sec_Title.Size = new Size(394, 58);
            lbl_Layout_Sec_Title.StateCommon.Font = new Font("Microsoft Sans Serif", 20F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lbl_Layout_Sec_Title.StateCommon.TextColor = Color.Red;
            lbl_Layout_Sec_Title.Text = "Layout Seçin";
            lbl_Layout_Sec_Title.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // SelectLayoutPanel
            // 
            SelectLayoutPanel.AutoScroll = true;
            SelectLayoutPanel.Location = new Point(12, 70);
            SelectLayoutPanel.Name = "SelectLayoutPanel";
            SelectLayoutPanel.Size = new Size(1240, 599);
            SelectLayoutPanel.TabIndex = 1;
            SelectLayoutPanel.Scroll += SelectLayoutPanel_Scroll;
            // 
            // progressBarPanel
            // 
            progressBarPanel.Controls.Add(kryptonWrapLabel1);
            progressBarPanel.Controls.Add(progressBar);
            progressBarPanel.Location = new Point(331, 675);
            progressBarPanel.Name = "progressBarPanel";
            progressBarPanel.Size = new Size(1228, 588);
            progressBarPanel.TabIndex = 0;
            // 
            // kryptonWrapLabel1
            // 
            kryptonWrapLabel1.AutoSize = false;
            kryptonWrapLabel1.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            kryptonWrapLabel1.ForeColor = Color.FromArgb(30, 57, 91);
            kryptonWrapLabel1.LabelStyle = Krypton.Toolkit.LabelStyle.AlternateControl;
            kryptonWrapLabel1.Location = new Point(458, 178);
            kryptonWrapLabel1.Name = "kryptonWrapLabel1";
            kryptonWrapLabel1.Size = new Size(313, 59);
            kryptonWrapLabel1.StateCommon.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            kryptonWrapLabel1.Text = "Yükleniyor...";
            kryptonWrapLabel1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // progressBar
            // 
            progressBar.Location = new Point(458, 265);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(313, 59);
            progressBar.TabIndex = 0;
            // 
            // contextMenuStrip
            // 
            contextMenuStrip.Font = new Font("Segoe UI", 9F);
            contextMenuStrip.Items.AddRange(new ToolStripItem[] { layoutuSilToolStripMenuItem });
            contextMenuStrip.Name = "contextMenuStrip";
            contextMenuStrip.Size = new Size(136, 26);
            // 
            // layoutuSilToolStripMenuItem
            // 
            layoutuSilToolStripMenuItem.Name = "layoutuSilToolStripMenuItem";
            layoutuSilToolStripMenuItem.Size = new Size(135, 22);
            layoutuSilToolStripMenuItem.Text = "Layout'u Sil";
            layoutuSilToolStripMenuItem.Click += layoutuSilToolStripMenuItem_Click;
            // 
            // timer
            // 
            timer.Interval = 150;
            timer.Tick += timer_Tick;
            // 
            // SelectLayouts
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1264, 681);
            Controls.Add(progressBarPanel);
            Controls.Add(SelectLayoutPanel);
            Controls.Add(lbl_Layout_Sec_Title);
            Name = "SelectLayouts";
            Text = "SelectLayouts";
            progressBarPanel.ResumeLayout(false);
            contextMenuStrip.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Krypton.Toolkit.KryptonWrapLabel lbl_Layout_Sec_Title;
        private FlowLayoutPanel SelectLayoutPanel;
        private ContextMenuStrip contextMenuStrip;
        private ToolStripMenuItem layoutuSilToolStripMenuItem;
        private Panel progressBarPanel;
        private Krypton.Toolkit.KryptonWrapLabel kryptonWrapLabel1;
        public ProgressBar progressBar;
        private System.Windows.Forms.Timer timer;
    }
}