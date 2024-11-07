namespace Balya_Yerleştirme
{
    partial class ReferansNoktasıEkleme
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReferansNoktasıEkleme));
            check_Ust_Orta = new CheckBox();
            check_Sag_Orta = new CheckBox();
            check_Sol_Orta = new CheckBox();
            check_Alt_Orta = new CheckBox();
            check_Merkez = new CheckBox();
            btn_Ref_Onayla = new Krypton.Toolkit.KryptonButton();
            RefCheckboxPanel = new Panel();
            btn_Ref_Vazgec = new Krypton.Toolkit.KryptonButton();
            RefDrawingPanel = new Panel();
            RefCheckboxPanel.SuspendLayout();
            SuspendLayout();
            // 
            // check_Ust_Orta
            // 
            check_Ust_Orta.AutoSize = true;
            check_Ust_Orta.Location = new Point(27, 23);
            check_Ust_Orta.Name = "check_Ust_Orta";
            check_Ust_Orta.Size = new Size(69, 19);
            check_Ust_Orta.TabIndex = 0;
            check_Ust_Orta.Text = "Üst Orta";
            check_Ust_Orta.UseVisualStyleBackColor = true;
            check_Ust_Orta.CheckStateChanged += check_Ust_Orta_CheckStateChanged;
            // 
            // check_Sag_Orta
            // 
            check_Sag_Orta.AutoSize = true;
            check_Sag_Orta.Location = new Point(27, 67);
            check_Sag_Orta.Name = "check_Sag_Orta";
            check_Sag_Orta.Size = new Size(71, 19);
            check_Sag_Orta.TabIndex = 1;
            check_Sag_Orta.Text = "Sağ Orta";
            check_Sag_Orta.UseVisualStyleBackColor = true;
            check_Sag_Orta.CheckStateChanged += check_Sag_Orta_CheckStateChanged;
            // 
            // check_Sol_Orta
            // 
            check_Sol_Orta.AutoSize = true;
            check_Sol_Orta.Location = new Point(27, 199);
            check_Sol_Orta.Name = "check_Sol_Orta";
            check_Sol_Orta.Size = new Size(68, 19);
            check_Sol_Orta.TabIndex = 2;
            check_Sol_Orta.Text = "Sol Orta";
            check_Sol_Orta.UseVisualStyleBackColor = true;
            check_Sol_Orta.CheckStateChanged += check_Sol_Orta_CheckStateChanged;
            // 
            // check_Alt_Orta
            // 
            check_Alt_Orta.AutoSize = true;
            check_Alt_Orta.Location = new Point(27, 155);
            check_Alt_Orta.Name = "check_Alt_Orta";
            check_Alt_Orta.Size = new Size(67, 19);
            check_Alt_Orta.TabIndex = 3;
            check_Alt_Orta.Text = "Alt Orta";
            check_Alt_Orta.UseVisualStyleBackColor = true;
            check_Alt_Orta.CheckStateChanged += check_Alt_Orta_CheckStateChanged;
            // 
            // check_Merkez
            // 
            check_Merkez.AutoSize = true;
            check_Merkez.Location = new Point(27, 111);
            check_Merkez.Name = "check_Merkez";
            check_Merkez.Size = new Size(64, 19);
            check_Merkez.TabIndex = 4;
            check_Merkez.Text = "Merkez";
            check_Merkez.UseVisualStyleBackColor = true;
            check_Merkez.CheckStateChanged += check_Merkez_CheckStateChanged;
            // 
            // btn_Ref_Onayla
            // 
            btn_Ref_Onayla.CornerRoundingRadius = 20F;
            btn_Ref_Onayla.DialogResult = DialogResult.OK;
            btn_Ref_Onayla.Location = new Point(55, 259);
            btn_Ref_Onayla.Name = "btn_Ref_Onayla";
            btn_Ref_Onayla.Size = new Size(124, 88);
            btn_Ref_Onayla.StateCommon.Back.Color1 = Color.FromArgb(224, 224, 224);
            btn_Ref_Onayla.StateCommon.Back.Image = (Image)resources.GetObject("btn_Ref_Onayla.StateCommon.Back.Image");
            btn_Ref_Onayla.StateCommon.Back.ImageStyle = Krypton.Toolkit.PaletteImageStyle.CenterLeft;
            btn_Ref_Onayla.StateCommon.Border.Color1 = Color.Lime;
            btn_Ref_Onayla.StateCommon.Border.DrawBorders = Krypton.Toolkit.PaletteDrawBorders.Top | Krypton.Toolkit.PaletteDrawBorders.Bottom | Krypton.Toolkit.PaletteDrawBorders.Left | Krypton.Toolkit.PaletteDrawBorders.Right;
            btn_Ref_Onayla.StateCommon.Border.Rounding = 20F;
            btn_Ref_Onayla.StateCommon.Border.Width = 2;
            btn_Ref_Onayla.StateCommon.Content.Padding = new Padding(-1, -1, 5, -1);
            btn_Ref_Onayla.StateCommon.Content.ShortText.Color1 = Color.Lime;
            btn_Ref_Onayla.StateCommon.Content.ShortText.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btn_Ref_Onayla.StateCommon.Content.ShortText.TextH = Krypton.Toolkit.PaletteRelativeAlign.Far;
            btn_Ref_Onayla.TabIndex = 5;
            btn_Ref_Onayla.Values.Text = "Onayla";
            // 
            // RefCheckboxPanel
            // 
            RefCheckboxPanel.Controls.Add(check_Sol_Orta);
            RefCheckboxPanel.Controls.Add(check_Ust_Orta);
            RefCheckboxPanel.Controls.Add(check_Sag_Orta);
            RefCheckboxPanel.Controls.Add(check_Merkez);
            RefCheckboxPanel.Controls.Add(check_Alt_Orta);
            RefCheckboxPanel.Location = new Point(12, 12);
            RefCheckboxPanel.Name = "RefCheckboxPanel";
            RefCheckboxPanel.Size = new Size(124, 241);
            RefCheckboxPanel.TabIndex = 6;
            // 
            // btn_Ref_Vazgec
            // 
            btn_Ref_Vazgec.CornerRoundingRadius = 20F;
            btn_Ref_Vazgec.DialogResult = DialogResult.Cancel;
            btn_Ref_Vazgec.Location = new Point(245, 259);
            btn_Ref_Vazgec.Name = "btn_Ref_Vazgec";
            btn_Ref_Vazgec.Size = new Size(124, 88);
            btn_Ref_Vazgec.StateCommon.Back.Color1 = Color.FromArgb(224, 224, 224);
            btn_Ref_Vazgec.StateCommon.Back.Image = (Image)resources.GetObject("btn_Ref_Vazgec.StateCommon.Back.Image");
            btn_Ref_Vazgec.StateCommon.Back.ImageStyle = Krypton.Toolkit.PaletteImageStyle.CenterLeft;
            btn_Ref_Vazgec.StateCommon.Border.Color1 = Color.Red;
            btn_Ref_Vazgec.StateCommon.Border.DrawBorders = Krypton.Toolkit.PaletteDrawBorders.Top | Krypton.Toolkit.PaletteDrawBorders.Bottom | Krypton.Toolkit.PaletteDrawBorders.Left | Krypton.Toolkit.PaletteDrawBorders.Right;
            btn_Ref_Vazgec.StateCommon.Border.Rounding = 20F;
            btn_Ref_Vazgec.StateCommon.Border.Width = 2;
            btn_Ref_Vazgec.StateCommon.Content.Padding = new Padding(-1, -1, 3, -1);
            btn_Ref_Vazgec.StateCommon.Content.ShortText.Color1 = Color.Red;
            btn_Ref_Vazgec.StateCommon.Content.ShortText.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btn_Ref_Vazgec.StateCommon.Content.ShortText.TextH = Krypton.Toolkit.PaletteRelativeAlign.Far;
            btn_Ref_Vazgec.TabIndex = 7;
            btn_Ref_Vazgec.Values.Text = "Vazgeç";
            // 
            // RefDrawingPanel
            // 
            RefDrawingPanel.Location = new Point(142, 12);
            RefDrawingPanel.Name = "RefDrawingPanel";
            RefDrawingPanel.Size = new Size(271, 241);
            RefDrawingPanel.TabIndex = 8;
            RefDrawingPanel.Paint += RefDrawingPanel_Paint;
            // 
            // ReferansNoktasıEkleme
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(425, 359);
            Controls.Add(RefDrawingPanel);
            Controls.Add(btn_Ref_Onayla);
            Controls.Add(btn_Ref_Vazgec);
            Controls.Add(RefCheckboxPanel);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "ReferansNoktasıEkleme";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Balyalara Referans Noktaları Ekleme";
            RefCheckboxPanel.ResumeLayout(false);
            RefCheckboxPanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        public CheckBox check_Ust_Orta;
        public CheckBox check_Sag_Orta;
        public CheckBox check_Sol_Orta;
        public CheckBox check_Alt_Orta;
        public CheckBox check_Merkez;
        public Krypton.Toolkit.KryptonButton btn_Ref_Onayla;
        public Panel RefCheckboxPanel;
        public Krypton.Toolkit.KryptonButton btn_Ref_Vazgec;
        public Panel RefDrawingPanel;
    }
}