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
            kryptonBorderEdge2 = new Krypton.Toolkit.KryptonBorderEdge();
            kryptonBorderEdge1 = new Krypton.Toolkit.KryptonBorderEdge();
            btn_ChangeLayoutDescription = new Button();
            txt_ChangeLayoutDescription = new TextBox();
            lbl_ChangeLayoutDescription = new Krypton.Toolkit.KryptonWrapLabel();
            btn_ChangeLayoutName = new Button();
            txt_ChangeLayoutName = new TextBox();
            InnerPanel = new Panel();
            kryptonBorderEdge3 = new Krypton.Toolkit.KryptonBorderEdge();
            LayoutPanel_MenuButtons = new FlowLayoutPanel();
            btn_Layout_Duzenle = new Button();
            btn_Sil = new Button();
            btn_Layout_Yukle = new Button();
            lbl_ChangeLayoutName = new Krypton.Toolkit.KryptonWrapLabel();
            lbl_LayoutMenu_Title = new Krypton.Toolkit.KryptonWrapLabel();
            errorProvider = new ErrorProvider(components);
            progressBarPanel.SuspendLayout();
            contextMenuStrip.SuspendLayout();
            panel_LayoutMenu.SuspendLayout();
            InnerPanel1.SuspendLayout();
            InnerPanel.SuspendLayout();
            LayoutPanel_MenuButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            // 
            // lbl_Layout_Sec_Title
            // 
            lbl_Layout_Sec_Title.AutoSize = false;
            lbl_Layout_Sec_Title.Font = new Font("Microsoft Sans Serif", 20F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lbl_Layout_Sec_Title.ForeColor = Color.Red;
            lbl_Layout_Sec_Title.LabelStyle = Krypton.Toolkit.LabelStyle.AlternateControl;
            lbl_Layout_Sec_Title.Location = new Point(595, 9);
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
            InnerPanel1.Controls.Add(kryptonBorderEdge2);
            InnerPanel1.Controls.Add(kryptonBorderEdge1);
            InnerPanel1.Controls.Add(btn_ChangeLayoutDescription);
            InnerPanel1.Controls.Add(txt_ChangeLayoutDescription);
            InnerPanel1.Controls.Add(lbl_ChangeLayoutDescription);
            InnerPanel1.Controls.Add(btn_ChangeLayoutName);
            InnerPanel1.Controls.Add(txt_ChangeLayoutName);
            InnerPanel1.Controls.Add(InnerPanel);
            InnerPanel1.Controls.Add(lbl_ChangeLayoutName);
            InnerPanel1.Location = new Point(3, 85);
            InnerPanel1.Name = "InnerPanel1";
            InnerPanel1.Size = new Size(308, 680);
            InnerPanel1.TabIndex = 2;
            // 
            // kryptonBorderEdge2
            // 
            kryptonBorderEdge2.Location = new Point(23, 3);
            kryptonBorderEdge2.Name = "kryptonBorderEdge2";
            kryptonBorderEdge2.Size = new Size(262, 1);
            kryptonBorderEdge2.Text = "kryptonBorderEdge2";
            // 
            // kryptonBorderEdge1
            // 
            kryptonBorderEdge1.Location = new Point(23, 101);
            kryptonBorderEdge1.Name = "kryptonBorderEdge1";
            kryptonBorderEdge1.Size = new Size(262, 1);
            kryptonBorderEdge1.Text = "kryptonBorderEdge1";
            // 
            // btn_ChangeLayoutDescription
            // 
            btn_ChangeLayoutDescription.FlatAppearance.BorderSize = 0;
            btn_ChangeLayoutDescription.FlatAppearance.MouseDownBackColor = Color.LightCyan;
            btn_ChangeLayoutDescription.FlatAppearance.MouseOverBackColor = Color.FromArgb(192, 255, 255);
            btn_ChangeLayoutDescription.FlatStyle = FlatStyle.Popup;
            btn_ChangeLayoutDescription.Image = Resources.Resource1.Checked_Checkbox;
            btn_ChangeLayoutDescription.Location = new Point(242, 217);
            btn_ChangeLayoutDescription.Name = "btn_ChangeLayoutDescription";
            btn_ChangeLayoutDescription.Size = new Size(60, 49);
            btn_ChangeLayoutDescription.TabIndex = 15;
            btn_ChangeLayoutDescription.UseVisualStyleBackColor = true;
            btn_ChangeLayoutDescription.Click += btn_ChangeLayoutDescription_Click;
            // 
            // txt_ChangeLayoutDescription
            // 
            txt_ChangeLayoutDescription.Location = new Point(7, 148);
            txt_ChangeLayoutDescription.Multiline = true;
            txt_ChangeLayoutDescription.Name = "txt_ChangeLayoutDescription";
            txt_ChangeLayoutDescription.ScrollBars = ScrollBars.Vertical;
            txt_ChangeLayoutDescription.Size = new Size(207, 187);
            txt_ChangeLayoutDescription.TabIndex = 14;
            txt_ChangeLayoutDescription.TextChanged += txt_ChangeLayoutDescription_TextChanged;
            // 
            // lbl_ChangeLayoutDescription
            // 
            lbl_ChangeLayoutDescription.AutoSize = false;
            lbl_ChangeLayoutDescription.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl_ChangeLayoutDescription.ForeColor = Color.Navy;
            lbl_ChangeLayoutDescription.LabelStyle = Krypton.Toolkit.LabelStyle.AlternateControl;
            lbl_ChangeLayoutDescription.Location = new Point(26, 112);
            lbl_ChangeLayoutDescription.Name = "lbl_ChangeLayoutDescription";
            lbl_ChangeLayoutDescription.Size = new Size(169, 33);
            lbl_ChangeLayoutDescription.StateCommon.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl_ChangeLayoutDescription.StateCommon.TextColor = Color.Navy;
            lbl_ChangeLayoutDescription.Text = "Layout Açıklaması";
            lbl_ChangeLayoutDescription.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btn_ChangeLayoutName
            // 
            btn_ChangeLayoutName.FlatAppearance.BorderSize = 0;
            btn_ChangeLayoutName.FlatAppearance.MouseDownBackColor = Color.LightCyan;
            btn_ChangeLayoutName.FlatAppearance.MouseOverBackColor = Color.FromArgb(192, 255, 255);
            btn_ChangeLayoutName.FlatStyle = FlatStyle.Popup;
            btn_ChangeLayoutName.Image = Resources.Resource1.Checked_Checkbox;
            btn_ChangeLayoutName.Location = new Point(242, 18);
            btn_ChangeLayoutName.Name = "btn_ChangeLayoutName";
            btn_ChangeLayoutName.Size = new Size(60, 49);
            btn_ChangeLayoutName.TabIndex = 11;
            btn_ChangeLayoutName.UseVisualStyleBackColor = true;
            btn_ChangeLayoutName.Click += btn_ChangeLayoutName_Click;
            // 
            // txt_ChangeLayoutName
            // 
            txt_ChangeLayoutName.Location = new Point(7, 49);
            txt_ChangeLayoutName.Name = "txt_ChangeLayoutName";
            txt_ChangeLayoutName.Size = new Size(207, 23);
            txt_ChangeLayoutName.TabIndex = 9;
            txt_ChangeLayoutName.TextChanged += txt_ChangeLayoutName_TextChanged;
            // 
            // InnerPanel
            // 
            InnerPanel.Controls.Add(kryptonBorderEdge3);
            InnerPanel.Controls.Add(LayoutPanel_MenuButtons);
            InnerPanel.Location = new Point(3, 353);
            InnerPanel.Name = "InnerPanel";
            InnerPanel.Size = new Size(302, 324);
            InnerPanel.TabIndex = 7;
            // 
            // kryptonBorderEdge3
            // 
            kryptonBorderEdge3.Location = new Point(20, 3);
            kryptonBorderEdge3.Name = "kryptonBorderEdge3";
            kryptonBorderEdge3.Size = new Size(262, 1);
            kryptonBorderEdge3.Text = "kryptonBorderEdge3";
            // 
            // LayoutPanel_MenuButtons
            // 
            LayoutPanel_MenuButtons.Controls.Add(btn_Layout_Duzenle);
            LayoutPanel_MenuButtons.Controls.Add(btn_Sil);
            LayoutPanel_MenuButtons.Controls.Add(btn_Layout_Yukle);
            LayoutPanel_MenuButtons.Location = new Point(14, 18);
            LayoutPanel_MenuButtons.Name = "LayoutPanel_MenuButtons";
            LayoutPanel_MenuButtons.Size = new Size(274, 288);
            LayoutPanel_MenuButtons.TabIndex = 2;
            // 
            // btn_Layout_Duzenle
            // 
            btn_Layout_Duzenle.FlatStyle = FlatStyle.Flat;
            btn_Layout_Duzenle.Location = new Point(3, 3);
            btn_Layout_Duzenle.Name = "btn_Layout_Duzenle";
            btn_Layout_Duzenle.Size = new Size(268, 90);
            btn_Layout_Duzenle.TabIndex = 2;
            btn_Layout_Duzenle.Text = "Layout'u Düzenle";
            btn_Layout_Duzenle.UseVisualStyleBackColor = true;
            btn_Layout_Duzenle.Click += btn_Layout_Duzenle_Click;
            // 
            // btn_Sil
            // 
            btn_Sil.FlatStyle = FlatStyle.Flat;
            btn_Sil.Location = new Point(3, 99);
            btn_Sil.Name = "btn_Sil";
            btn_Sil.Size = new Size(268, 90);
            btn_Sil.TabIndex = 3;
            btn_Sil.Text = "Sil";
            btn_Sil.UseVisualStyleBackColor = true;
            btn_Sil.Click += btn_Sil_Click;
            // 
            // btn_Layout_Yukle
            // 
            btn_Layout_Yukle.FlatStyle = FlatStyle.Flat;
            btn_Layout_Yukle.Location = new Point(3, 195);
            btn_Layout_Yukle.Name = "btn_Layout_Yukle";
            btn_Layout_Yukle.Size = new Size(268, 90);
            btn_Layout_Yukle.TabIndex = 4;
            btn_Layout_Yukle.Text = "Yükle";
            btn_Layout_Yukle.UseVisualStyleBackColor = true;
            btn_Layout_Yukle.Click += btn_Layout_Yukle_Click;
            // 
            // lbl_ChangeLayoutName
            // 
            lbl_ChangeLayoutName.AutoSize = false;
            lbl_ChangeLayoutName.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl_ChangeLayoutName.ForeColor = Color.Navy;
            lbl_ChangeLayoutName.LabelStyle = Krypton.Toolkit.LabelStyle.AlternateControl;
            lbl_ChangeLayoutName.Location = new Point(29, 16);
            lbl_ChangeLayoutName.Name = "lbl_ChangeLayoutName";
            lbl_ChangeLayoutName.Size = new Size(154, 30);
            lbl_ChangeLayoutName.StateCommon.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl_ChangeLayoutName.StateCommon.TextColor = Color.Navy;
            lbl_ChangeLayoutName.Text = "Layout İsmi";
            lbl_ChangeLayoutName.TextAlign = ContentAlignment.MiddleCenter;
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
            // errorProvider
            // 
            errorProvider.ContainerControl = this;
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
            InnerPanel1.PerformLayout();
            InnerPanel.ResumeLayout(false);
            InnerPanel.PerformLayout();
            LayoutPanel_MenuButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
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
        private Panel InnerPanel1;
        private Krypton.Toolkit.KryptonWrapLabel lbl_ChangeLayoutName;
        private Panel InnerPanel;
        private TextBox txt_ChangeLayoutName;
        private Button btn_ChangeLayoutName;
        private Krypton.Toolkit.KryptonWrapLabel lbl_ChangeLayoutDescription;
        private Button btn_ChangeLayoutDescription;
        private TextBox txt_ChangeLayoutDescription;
        private Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge1;
        private Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge2;
        private Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge3;
        private Button btn_Layout_Yukle;
        private ErrorProvider errorProvider;
    }
}