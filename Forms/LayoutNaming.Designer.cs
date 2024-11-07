namespace Balya_Yerleştirme
{
    partial class LayoutNaming
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
            lbl_Layout_Title = new Krypton.Toolkit.KryptonWrapLabel();
            lbl_Layout_Isim = new Krypton.Toolkit.KryptonWrapLabel();
            lbl_Layout_Aciklama = new Krypton.Toolkit.KryptonWrapLabel();
            txt_Layout_Isim = new TextBox();
            txt_Layout_Aciklama = new TextBox();
            btn_Layout_Onayla = new Krypton.Toolkit.KryptonButton();
            btn_Layout_Vazgeç = new Krypton.Toolkit.KryptonButton();
            errorProvider = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            // 
            // lbl_Layout_Title
            // 
            lbl_Layout_Title.AutoSize = false;
            lbl_Layout_Title.Font = new Font("Microsoft Sans Serif", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lbl_Layout_Title.ForeColor = Color.Red;
            lbl_Layout_Title.LabelStyle = Krypton.Toolkit.LabelStyle.AlternateControl;
            lbl_Layout_Title.Location = new Point(12, 9);
            lbl_Layout_Title.Name = "lbl_Layout_Title";
            lbl_Layout_Title.Size = new Size(508, 57);
            lbl_Layout_Title.StateCommon.Font = new Font("Microsoft Sans Serif", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lbl_Layout_Title.StateCommon.TextColor = Color.Red;
            lbl_Layout_Title.Text = "Layout'un İsmini ve Açıklamasını Girin";
            lbl_Layout_Title.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbl_Layout_Isim
            // 
            lbl_Layout_Isim.AutoSize = false;
            lbl_Layout_Isim.Font = new Font("Microsoft Sans Serif", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lbl_Layout_Isim.ForeColor = Color.Navy;
            lbl_Layout_Isim.LabelStyle = Krypton.Toolkit.LabelStyle.AlternateControl;
            lbl_Layout_Isim.Location = new Point(69, 76);
            lbl_Layout_Isim.Name = "lbl_Layout_Isim";
            lbl_Layout_Isim.Size = new Size(141, 36);
            lbl_Layout_Isim.StateCommon.Font = new Font("Microsoft Sans Serif", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lbl_Layout_Isim.StateCommon.TextColor = Color.Navy;
            lbl_Layout_Isim.Text = "İsim: ";
            lbl_Layout_Isim.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbl_Layout_Aciklama
            // 
            lbl_Layout_Aciklama.AutoSize = false;
            lbl_Layout_Aciklama.Font = new Font("Microsoft Sans Serif", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lbl_Layout_Aciklama.ForeColor = Color.Navy;
            lbl_Layout_Aciklama.LabelStyle = Krypton.Toolkit.LabelStyle.AlternateControl;
            lbl_Layout_Aciklama.Location = new Point(69, 185);
            lbl_Layout_Aciklama.Name = "lbl_Layout_Aciklama";
            lbl_Layout_Aciklama.Size = new Size(141, 36);
            lbl_Layout_Aciklama.StateCommon.Font = new Font("Microsoft Sans Serif", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lbl_Layout_Aciklama.StateCommon.TextColor = Color.Navy;
            lbl_Layout_Aciklama.Text = "Açıklama: ";
            lbl_Layout_Aciklama.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txt_Layout_Isim
            // 
            txt_Layout_Isim.Location = new Point(216, 79);
            txt_Layout_Isim.Name = "txt_Layout_Isim";
            txt_Layout_Isim.Size = new Size(244, 23);
            txt_Layout_Isim.TabIndex = 5;
            // 
            // txt_Layout_Aciklama
            // 
            txt_Layout_Aciklama.Location = new Point(216, 119);
            txt_Layout_Aciklama.Multiline = true;
            txt_Layout_Aciklama.Name = "txt_Layout_Aciklama";
            txt_Layout_Aciklama.Size = new Size(244, 168);
            txt_Layout_Aciklama.TabIndex = 6;
            // 
            // btn_Layout_Onayla
            // 
            btn_Layout_Onayla.CornerRoundingRadius = 5F;
            btn_Layout_Onayla.Location = new Point(95, 298);
            btn_Layout_Onayla.Name = "btn_Layout_Onayla";
            btn_Layout_Onayla.Size = new Size(127, 50);
            btn_Layout_Onayla.StateCommon.Back.Color1 = Color.Lime;
            btn_Layout_Onayla.StateCommon.Back.Color2 = Color.Lime;
            btn_Layout_Onayla.StateCommon.Back.Image = Resources.Resource1.Checked_Checkbox;
            btn_Layout_Onayla.StateCommon.Back.ImageAlign = Krypton.Toolkit.PaletteRectangleAlign.Control;
            btn_Layout_Onayla.StateCommon.Back.ImageStyle = Krypton.Toolkit.PaletteImageStyle.CenterMiddle;
            btn_Layout_Onayla.StateCommon.Border.Color1 = Color.Lime;
            btn_Layout_Onayla.StateCommon.Border.DrawBorders = Krypton.Toolkit.PaletteDrawBorders.Top | Krypton.Toolkit.PaletteDrawBorders.Bottom | Krypton.Toolkit.PaletteDrawBorders.Left | Krypton.Toolkit.PaletteDrawBorders.Right;
            btn_Layout_Onayla.StateCommon.Border.Rounding = 5F;
            btn_Layout_Onayla.TabIndex = 7;
            btn_Layout_Onayla.Values.Text = "";
            btn_Layout_Onayla.Click += btn_Layout_Onayla_Click;
            // 
            // btn_Layout_Vazgeç
            // 
            btn_Layout_Vazgeç.CornerRoundingRadius = 5F;
            btn_Layout_Vazgeç.DialogResult = DialogResult.Cancel;
            btn_Layout_Vazgeç.Location = new Point(304, 298);
            btn_Layout_Vazgeç.Name = "btn_Layout_Vazgeç";
            btn_Layout_Vazgeç.Size = new Size(127, 50);
            btn_Layout_Vazgeç.StateCommon.Back.Color1 = Color.Red;
            btn_Layout_Vazgeç.StateCommon.Back.Color2 = Color.Red;
            btn_Layout_Vazgeç.StateCommon.Back.Image = Resources.Resource1.CancelRed;
            btn_Layout_Vazgeç.StateCommon.Back.ImageAlign = Krypton.Toolkit.PaletteRectangleAlign.Control;
            btn_Layout_Vazgeç.StateCommon.Back.ImageStyle = Krypton.Toolkit.PaletteImageStyle.CenterMiddle;
            btn_Layout_Vazgeç.StateCommon.Border.Color1 = Color.Lime;
            btn_Layout_Vazgeç.StateCommon.Border.DrawBorders = Krypton.Toolkit.PaletteDrawBorders.Top | Krypton.Toolkit.PaletteDrawBorders.Bottom | Krypton.Toolkit.PaletteDrawBorders.Left | Krypton.Toolkit.PaletteDrawBorders.Right;
            btn_Layout_Vazgeç.StateCommon.Border.Rounding = 5F;
            btn_Layout_Vazgeç.TabIndex = 8;
            btn_Layout_Vazgeç.Values.Text = "";
            // 
            // errorProvider
            // 
            errorProvider.ContainerControl = this;
            // 
            // LayoutNaming
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(528, 357);
            Controls.Add(btn_Layout_Vazgeç);
            Controls.Add(btn_Layout_Onayla);
            Controls.Add(txt_Layout_Aciklama);
            Controls.Add(txt_Layout_Isim);
            Controls.Add(lbl_Layout_Aciklama);
            Controls.Add(lbl_Layout_Isim);
            Controls.Add(lbl_Layout_Title);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "LayoutNaming";
            StartPosition = FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Krypton.Toolkit.KryptonWrapLabel lbl_Layout_Title;
        private Krypton.Toolkit.KryptonWrapLabel lbl_Layout_Isim;
        private Krypton.Toolkit.KryptonWrapLabel lbl_Layout_Aciklama;
        private Krypton.Toolkit.KryptonButton btn_Layout_Vazgeç;
        private ErrorProvider errorProvider;
        public TextBox txt_Layout_Isim;
        public TextBox txt_Layout_Aciklama;
        public Krypton.Toolkit.KryptonButton btn_Layout_Onayla;
    }
}