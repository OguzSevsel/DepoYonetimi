namespace Balya_Yerleştirme.Forms
{
    partial class PLCPassword
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
            btn_PLC_Password_Onayla = new Krypton.Toolkit.KryptonButton();
            btn_PLC_Password_Vazgec = new Krypton.Toolkit.KryptonButton();
            txt_PLC_Password = new TextBox();
            lbl_PLC_Password_Title = new Krypton.Toolkit.KryptonWrapLabel();
            SuspendLayout();
            // 
            // btn_PLC_Password_Onayla
            // 
            btn_PLC_Password_Onayla.CornerRoundingRadius = 5F;
            btn_PLC_Password_Onayla.Location = new Point(101, 122);
            btn_PLC_Password_Onayla.Name = "btn_PLC_Password_Onayla";
            btn_PLC_Password_Onayla.Size = new Size(95, 56);
            btn_PLC_Password_Onayla.StateCommon.Back.Color1 = SystemColors.ActiveCaption;
            btn_PLC_Password_Onayla.StateCommon.Border.Color1 = SystemColors.ButtonShadow;
            btn_PLC_Password_Onayla.StateCommon.Border.Color2 = SystemColors.ButtonShadow;
            btn_PLC_Password_Onayla.StateCommon.Border.DrawBorders = Krypton.Toolkit.PaletteDrawBorders.Top | Krypton.Toolkit.PaletteDrawBorders.Bottom | Krypton.Toolkit.PaletteDrawBorders.Left | Krypton.Toolkit.PaletteDrawBorders.Right;
            btn_PLC_Password_Onayla.StateCommon.Border.Rounding = 5F;
            btn_PLC_Password_Onayla.StateCommon.Content.ShortText.Color1 = Color.FromArgb(0, 192, 0);
            btn_PLC_Password_Onayla.StateCommon.Content.ShortText.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_PLC_Password_Onayla.TabIndex = 2;
            btn_PLC_Password_Onayla.Values.Text = "Onayla";
            btn_PLC_Password_Onayla.Click += btn_PLC_Password_Onayla_Click;
            // 
            // btn_PLC_Password_Vazgec
            // 
            btn_PLC_Password_Vazgec.CornerRoundingRadius = 5F;
            btn_PLC_Password_Vazgec.Location = new Point(206, 122);
            btn_PLC_Password_Vazgec.Name = "btn_PLC_Password_Vazgec";
            btn_PLC_Password_Vazgec.Size = new Size(95, 56);
            btn_PLC_Password_Vazgec.StateCommon.Back.Color1 = SystemColors.ActiveCaption;
            btn_PLC_Password_Vazgec.StateCommon.Border.Color1 = SystemColors.ButtonShadow;
            btn_PLC_Password_Vazgec.StateCommon.Border.Color2 = SystemColors.ButtonShadow;
            btn_PLC_Password_Vazgec.StateCommon.Border.DrawBorders = Krypton.Toolkit.PaletteDrawBorders.Top | Krypton.Toolkit.PaletteDrawBorders.Bottom | Krypton.Toolkit.PaletteDrawBorders.Left | Krypton.Toolkit.PaletteDrawBorders.Right;
            btn_PLC_Password_Vazgec.StateCommon.Border.Rounding = 5F;
            btn_PLC_Password_Vazgec.StateCommon.Content.ShortText.Color1 = Color.Red;
            btn_PLC_Password_Vazgec.StateCommon.Content.ShortText.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_PLC_Password_Vazgec.TabIndex = 3;
            btn_PLC_Password_Vazgec.Values.Text = "Vazgeç";
            btn_PLC_Password_Vazgec.Click += btn_PLC_Password_Vazgec_Click;
            // 
            // txt_PLC_Password
            // 
            txt_PLC_Password.Location = new Point(101, 93);
            txt_PLC_Password.Name = "txt_PLC_Password";
            txt_PLC_Password.Size = new Size(200, 23);
            txt_PLC_Password.TabIndex = 4;
            // 
            // lbl_PLC_Password_Title
            // 
            lbl_PLC_Password_Title.AutoSize = false;
            lbl_PLC_Password_Title.Font = new Font("Microsoft Sans Serif", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl_PLC_Password_Title.ForeColor = Color.Blue;
            lbl_PLC_Password_Title.LabelStyle = Krypton.Toolkit.LabelStyle.AlternateControl;
            lbl_PLC_Password_Title.Location = new Point(19, 50);
            lbl_PLC_Password_Title.Name = "lbl_PLC_Password_Title";
            lbl_PLC_Password_Title.Size = new Size(364, 40);
            lbl_PLC_Password_Title.StateCommon.Font = new Font("Microsoft Sans Serif", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl_PLC_Password_Title.StateCommon.TextColor = Color.Blue;
            lbl_PLC_Password_Title.Text = "Lütfen Şifre Girin";
            lbl_PLC_Password_Title.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // PLCPassword
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(403, 229);
            Controls.Add(lbl_PLC_Password_Title);
            Controls.Add(txt_PLC_Password);
            Controls.Add(btn_PLC_Password_Vazgec);
            Controls.Add(btn_PLC_Password_Onayla);
            Name = "PLCPassword";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Şifre";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Krypton.Toolkit.KryptonButton btn_PLC_Password_Onayla;
        private Krypton.Toolkit.KryptonButton btn_PLC_Password_Vazgec;
        private TextBox txt_PLC_Password;
        private Krypton.Toolkit.KryptonWrapLabel lbl_PLC_Password_Title;
    }
}