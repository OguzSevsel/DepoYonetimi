namespace Balya_Yerleştirme
{
    partial class ConveyorReferansNoktasıEkleme
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConveyorReferansNoktasıEkleme));
            btn_Ref_Onayla = new Krypton.Toolkit.KryptonButton();
            btn_Ref_Vazgec = new Krypton.Toolkit.KryptonButton();
            drawingPanel = new Panel();
            btn_Ref_Point_Ekle = new Krypton.Toolkit.KryptonButton();
            lbl_Conveyor_Ref_Info = new Krypton.Toolkit.KryptonWrapLabel();
            lbl_Conveyor_Ref_Title = new Krypton.Toolkit.KryptonWrapLabel();
            SuspendLayout();
            // 
            // btn_Ref_Onayla
            // 
            btn_Ref_Onayla.CornerRoundingRadius = 20F;
            btn_Ref_Onayla.DialogResult = DialogResult.OK;
            btn_Ref_Onayla.Location = new Point(12, 322);
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
            btn_Ref_Onayla.StateNormal.Border.Color1 = Color.Lime;
            btn_Ref_Onayla.StateNormal.Border.DrawBorders = Krypton.Toolkit.PaletteDrawBorders.Top | Krypton.Toolkit.PaletteDrawBorders.Bottom | Krypton.Toolkit.PaletteDrawBorders.Left | Krypton.Toolkit.PaletteDrawBorders.Right;
            btn_Ref_Onayla.StateNormal.Border.Width = 2;
            btn_Ref_Onayla.StateNormal.Content.ShortText.Color1 = Color.Lime;
            btn_Ref_Onayla.StateNormal.Content.ShortText.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btn_Ref_Onayla.StateNormal.Content.ShortText.TextH = Krypton.Toolkit.PaletteRelativeAlign.Far;
            btn_Ref_Onayla.TabIndex = 6;
            btn_Ref_Onayla.Values.Text = "Onayla";
            // 
            // btn_Ref_Vazgec
            // 
            btn_Ref_Vazgec.CornerRoundingRadius = 20F;
            btn_Ref_Vazgec.DialogResult = DialogResult.Cancel;
            btn_Ref_Vazgec.Location = new Point(12, 416);
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
            btn_Ref_Vazgec.TabIndex = 8;
            btn_Ref_Vazgec.Values.Text = "Vazgeç";
            // 
            // drawingPanel
            // 
            drawingPanel.Location = new Point(142, 49);
            drawingPanel.Name = "drawingPanel";
            drawingPanel.Size = new Size(731, 455);
            drawingPanel.TabIndex = 15;
            drawingPanel.Paint += drawingPanel_Paint;
            drawingPanel.MouseClick += drawingPanel_MouseClick;
            // 
            // btn_Ref_Point_Ekle
            // 
            btn_Ref_Point_Ekle.CornerRoundingRadius = 20F;
            btn_Ref_Point_Ekle.Location = new Point(12, 228);
            btn_Ref_Point_Ekle.Name = "btn_Ref_Point_Ekle";
            btn_Ref_Point_Ekle.Size = new Size(124, 88);
            btn_Ref_Point_Ekle.StateCommon.Back.Color1 = Color.FromArgb(224, 224, 224);
            btn_Ref_Point_Ekle.StateCommon.Back.Image = (Image)resources.GetObject("btn_Ref_Point_Ekle.StateCommon.Back.Image");
            btn_Ref_Point_Ekle.StateCommon.Back.ImageStyle = Krypton.Toolkit.PaletteImageStyle.CenterLeft;
            btn_Ref_Point_Ekle.StateCommon.Border.Color1 = Color.Blue;
            btn_Ref_Point_Ekle.StateCommon.Border.DrawBorders = Krypton.Toolkit.PaletteDrawBorders.Top | Krypton.Toolkit.PaletteDrawBorders.Bottom | Krypton.Toolkit.PaletteDrawBorders.Left | Krypton.Toolkit.PaletteDrawBorders.Right;
            btn_Ref_Point_Ekle.StateCommon.Border.Rounding = 20F;
            btn_Ref_Point_Ekle.StateCommon.Border.Width = 2;
            btn_Ref_Point_Ekle.StateCommon.Content.Padding = new Padding(-1, -1, 10, -1);
            btn_Ref_Point_Ekle.StateCommon.Content.ShortText.Color1 = Color.Blue;
            btn_Ref_Point_Ekle.StateCommon.Content.ShortText.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btn_Ref_Point_Ekle.StateCommon.Content.ShortText.TextH = Krypton.Toolkit.PaletteRelativeAlign.Far;
            btn_Ref_Point_Ekle.StateNormal.Border.Color1 = Color.Blue;
            btn_Ref_Point_Ekle.StateNormal.Border.DrawBorders = Krypton.Toolkit.PaletteDrawBorders.Top | Krypton.Toolkit.PaletteDrawBorders.Bottom | Krypton.Toolkit.PaletteDrawBorders.Left | Krypton.Toolkit.PaletteDrawBorders.Right;
            btn_Ref_Point_Ekle.StateNormal.Border.Width = 2;
            btn_Ref_Point_Ekle.StateNormal.Content.ShortText.Color1 = Color.Blue;
            btn_Ref_Point_Ekle.StateNormal.Content.ShortText.TextH = Krypton.Toolkit.PaletteRelativeAlign.Far;
            btn_Ref_Point_Ekle.TabIndex = 16;
            btn_Ref_Point_Ekle.Values.Text = "Ekle";
            btn_Ref_Point_Ekle.Click += btn_Ref_Point_Ekle_Click;
            // 
            // lbl_Conveyor_Ref_Info
            // 
            lbl_Conveyor_Ref_Info.AutoSize = false;
            lbl_Conveyor_Ref_Info.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lbl_Conveyor_Ref_Info.ForeColor = Color.Blue;
            lbl_Conveyor_Ref_Info.LabelStyle = Krypton.Toolkit.LabelStyle.AlternateControl;
            lbl_Conveyor_Ref_Info.Location = new Point(12, 12);
            lbl_Conveyor_Ref_Info.Name = "lbl_Conveyor_Ref_Info";
            lbl_Conveyor_Ref_Info.Size = new Size(124, 213);
            lbl_Conveyor_Ref_Info.StateCommon.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lbl_Conveyor_Ref_Info.StateCommon.TextColor = Color.Blue;
            lbl_Conveyor_Ref_Info.Text = "Lütfen öncelikle ekle'ye tıklayın. Sonra yanda bulunan conveyor'a tıklayarak referans noktalarını ekleyebilirsiniz.";
            lbl_Conveyor_Ref_Info.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbl_Conveyor_Ref_Title
            // 
            lbl_Conveyor_Ref_Title.AutoSize = false;
            lbl_Conveyor_Ref_Title.Font = new Font("Microsoft Sans Serif", 20F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lbl_Conveyor_Ref_Title.ForeColor = Color.Red;
            lbl_Conveyor_Ref_Title.LabelStyle = Krypton.Toolkit.LabelStyle.AlternateControl;
            lbl_Conveyor_Ref_Title.Location = new Point(225, 9);
            lbl_Conveyor_Ref_Title.Name = "lbl_Conveyor_Ref_Title";
            lbl_Conveyor_Ref_Title.Size = new Size(564, 33);
            lbl_Conveyor_Ref_Title.StateCommon.Font = new Font("Microsoft Sans Serif", 20F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lbl_Conveyor_Ref_Title.StateCommon.TextColor = Color.Red;
            lbl_Conveyor_Ref_Title.Text = "Conveyor";
            lbl_Conveyor_Ref_Title.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // ConveyorReferansNoktasıEkleme
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(885, 516);
            Controls.Add(lbl_Conveyor_Ref_Title);
            Controls.Add(lbl_Conveyor_Ref_Info);
            Controls.Add(btn_Ref_Point_Ekle);
            Controls.Add(drawingPanel);
            Controls.Add(btn_Ref_Vazgec);
            Controls.Add(btn_Ref_Onayla);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "ConveyorReferansNoktasıEkleme";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Conveyor'a Referans Noktası Ekleme";
            ResumeLayout(false);
        }

        #endregion
        public Krypton.Toolkit.KryptonButton btn_Ref_Onayla;
        public Krypton.Toolkit.KryptonButton btn_Ref_Vazgec;
        public Krypton.Toolkit.KryptonButton btn_Ref_Point_Ekle;
        private Krypton.Toolkit.KryptonWrapLabel lbl_Conveyor_Ref_Info;
        private Krypton.Toolkit.KryptonWrapLabel lbl_Conveyor_Ref_Title;
        public Panel drawingPanel;
    }
}