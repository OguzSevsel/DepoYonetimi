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
            panel_LayoutMenu = new Panel();
            InnerPanel1 = new Panel();
            InnerPanel = new Panel();
            LayoutPanel_MenuButtons = new FlowLayoutPanel();
            btn_Layout_Duzenle = new Button();
            btn_Sil = new Button();
            btn_Isim_Degistir = new Button();
            btn_Aciklama_Degistir = new Button();
            lbl_Layout_Name = new Krypton.Toolkit.KryptonWrapLabel();
            lbl_LayoutMenu_Title = new Krypton.Toolkit.KryptonWrapLabel();
            progressBarPanel.SuspendLayout();
            contextMenuStrip.SuspendLayout();
            panel_LayoutMenu.SuspendLayout();
            InnerPanel1.SuspendLayout();
            InnerPanel.SuspendLayout();
            LayoutPanel_MenuButtons.SuspendLayout();
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
            SelectLayoutPanel.Size = new Size(1560, 768);
            SelectLayoutPanel.TabIndex = 1;
            SelectLayoutPanel.Scroll += SelectLayoutPanel_Scroll;
            SelectLayoutPanel.MouseDown += SelectLayoutPanel_MouseDown;
            // 
            // progressBarPanel
            // 
            progressBarPanel.Controls.Add(kryptonWrapLabel1);
            progressBarPanel.Controls.Add(progressBar);
            progressBarPanel.Location = new Point(12, 844);
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
            // panel_LayoutMenu
            // 
            panel_LayoutMenu.BackColor = Color.LightCyan;
            panel_LayoutMenu.Controls.Add(InnerPanel1);
            panel_LayoutMenu.Controls.Add(lbl_LayoutMenu_Title);
            panel_LayoutMenu.Location = new Point(1246, 844);
            panel_LayoutMenu.Name = "panel_LayoutMenu";
            panel_LayoutMenu.Size = new Size(314, 768);
            panel_LayoutMenu.TabIndex = 4;
            // 
            // InnerPanel1
            // 
            InnerPanel1.Controls.Add(InnerPanel);
            InnerPanel1.Controls.Add(lbl_Layout_Name);
            InnerPanel1.Location = new Point(3, 85);
            InnerPanel1.Name = "InnerPanel1";
            InnerPanel1.Size = new Size(308, 680);
            InnerPanel1.TabIndex = 2;
            // 
            // InnerPanel
            // 
            InnerPanel.Controls.Add(LayoutPanel_MenuButtons);
            InnerPanel.Location = new Point(3, 93);
            InnerPanel.Name = "InnerPanel";
            InnerPanel.Size = new Size(302, 584);
            InnerPanel.TabIndex = 7;
            // 
            // LayoutPanel_MenuButtons
            // 
            LayoutPanel_MenuButtons.Controls.Add(btn_Layout_Duzenle);
            LayoutPanel_MenuButtons.Controls.Add(btn_Sil);
            LayoutPanel_MenuButtons.Controls.Add(btn_Isim_Degistir);
            LayoutPanel_MenuButtons.Controls.Add(btn_Aciklama_Degistir);
            LayoutPanel_MenuButtons.Location = new Point(14, 82);
            LayoutPanel_MenuButtons.Name = "LayoutPanel_MenuButtons";
            LayoutPanel_MenuButtons.Size = new Size(274, 421);
            LayoutPanel_MenuButtons.TabIndex = 2;
            // 
            // btn_Layout_Duzenle
            // 
            btn_Layout_Duzenle.FlatStyle = FlatStyle.Flat;
            btn_Layout_Duzenle.Location = new Point(3, 3);
            btn_Layout_Duzenle.Name = "btn_Layout_Duzenle";
            btn_Layout_Duzenle.Size = new Size(268, 99);
            btn_Layout_Duzenle.TabIndex = 2;
            btn_Layout_Duzenle.Text = "Layout'u Düzenle";
            btn_Layout_Duzenle.UseVisualStyleBackColor = true;
            btn_Layout_Duzenle.Click += btn_Layout_Duzenle_Click;
            // 
            // btn_Sil
            // 
            btn_Sil.FlatStyle = FlatStyle.Flat;
            btn_Sil.Location = new Point(3, 108);
            btn_Sil.Name = "btn_Sil";
            btn_Sil.Size = new Size(268, 99);
            btn_Sil.TabIndex = 3;
            btn_Sil.Text = "Sil";
            btn_Sil.UseVisualStyleBackColor = true;
            btn_Sil.Click += btn_Sil_Click;
            // 
            // btn_Isim_Degistir
            // 
            btn_Isim_Degistir.FlatStyle = FlatStyle.Flat;
            btn_Isim_Degistir.Location = new Point(3, 213);
            btn_Isim_Degistir.Name = "btn_Isim_Degistir";
            btn_Isim_Degistir.Size = new Size(268, 99);
            btn_Isim_Degistir.TabIndex = 4;
            btn_Isim_Degistir.Text = "İsmini Değiştir";
            btn_Isim_Degistir.UseVisualStyleBackColor = true;
            btn_Isim_Degistir.Click += btn_Isim_Degistir_Click;
            // 
            // btn_Aciklama_Degistir
            // 
            btn_Aciklama_Degistir.FlatStyle = FlatStyle.Flat;
            btn_Aciklama_Degistir.Location = new Point(3, 318);
            btn_Aciklama_Degistir.Name = "btn_Aciklama_Degistir";
            btn_Aciklama_Degistir.Size = new Size(268, 99);
            btn_Aciklama_Degistir.TabIndex = 5;
            btn_Aciklama_Degistir.Text = "Açıklamasını Değiştir";
            btn_Aciklama_Degistir.UseVisualStyleBackColor = true;
            btn_Aciklama_Degistir.Click += btn_Aciklama_Degistir_Click;
            // 
            // lbl_Layout_Name
            // 
            lbl_Layout_Name.AutoSize = false;
            lbl_Layout_Name.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl_Layout_Name.ForeColor = Color.Navy;
            lbl_Layout_Name.LabelStyle = Krypton.Toolkit.LabelStyle.AlternateControl;
            lbl_Layout_Name.Location = new Point(10, 21);
            lbl_Layout_Name.Name = "lbl_Layout_Name";
            lbl_Layout_Name.Size = new Size(288, 52);
            lbl_Layout_Name.StateCommon.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl_Layout_Name.StateCommon.TextColor = Color.Navy;
            lbl_Layout_Name.Text = "Layout İsmi";
            lbl_Layout_Name.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbl_LayoutMenu_Title
            // 
            lbl_LayoutMenu_Title.AutoSize = false;
            lbl_LayoutMenu_Title.Font = new Font("Microsoft Sans Serif", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl_LayoutMenu_Title.ForeColor = Color.Red;
            lbl_LayoutMenu_Title.LabelStyle = Krypton.Toolkit.LabelStyle.AlternateControl;
            lbl_LayoutMenu_Title.Location = new Point(13, 17);
            lbl_LayoutMenu_Title.Name = "lbl_LayoutMenu_Title";
            lbl_LayoutMenu_Title.Size = new Size(288, 52);
            lbl_LayoutMenu_Title.StateCommon.Font = new Font("Microsoft Sans Serif", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl_LayoutMenu_Title.StateCommon.TextColor = Color.Red;
            lbl_LayoutMenu_Title.Text = "Seçilen Layout";
            lbl_LayoutMenu_Title.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // SelectLayouts
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1584, 861);
            Controls.Add(panel_LayoutMenu);
            Controls.Add(progressBarPanel);
            Controls.Add(SelectLayoutPanel);
            Controls.Add(lbl_Layout_Sec_Title);
            Name = "SelectLayouts";
            Text = "Layout";
            progressBarPanel.ResumeLayout(false);
            contextMenuStrip.ResumeLayout(false);
            panel_LayoutMenu.ResumeLayout(false);
            InnerPanel1.ResumeLayout(false);
            InnerPanel.ResumeLayout(false);
            LayoutPanel_MenuButtons.ResumeLayout(false);
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
        private Panel panel_LayoutMenu;
        private Krypton.Toolkit.KryptonWrapLabel lbl_LayoutMenu_Title;
        private FlowLayoutPanel LayoutPanel_MenuButtons;
        private Button btn_Layout_Duzenle;
        private Button btn_Sil;
        private Button btn_Isim_Degistir;
        private Panel InnerPanel1;
        private Button btn_Aciklama_Degistir;
        private Krypton.Toolkit.KryptonWrapLabel lbl_Layout_Name;
        private Panel InnerPanel;
    }
}