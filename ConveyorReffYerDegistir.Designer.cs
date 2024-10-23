namespace Balya_Yerleştirme
{
    partial class ConveyorReffYerDegistir
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
            Panel = new FlowLayoutPanel();
            btn_Ref_Vazgec = new Button();
            btn_Ref_Onayla = new Button();
            errorProvider = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            // 
            // Panel
            // 
            Panel.AutoScroll = true;
            Panel.Location = new Point(12, 46);
            Panel.Name = "Panel";
            Panel.Size = new Size(512, 286);
            Panel.TabIndex = 0;
            // 
            // btn_Ref_Vazgec
            // 
            btn_Ref_Vazgec.BackColor = Color.LightCyan;
            btn_Ref_Vazgec.DialogResult = DialogResult.Cancel;
            btn_Ref_Vazgec.FlatAppearance.BorderSize = 0;
            btn_Ref_Vazgec.FlatStyle = FlatStyle.Popup;
            btn_Ref_Vazgec.Font = new Font("Segoe UI", 10F);
            btn_Ref_Vazgec.Image = Resources.Resource1.Cancel;
            btn_Ref_Vazgec.Location = new Point(283, 339);
            btn_Ref_Vazgec.Name = "btn_Ref_Vazgec";
            btn_Ref_Vazgec.Size = new Size(94, 51);
            btn_Ref_Vazgec.TabIndex = 9;
            btn_Ref_Vazgec.TextAlign = ContentAlignment.MiddleLeft;
            btn_Ref_Vazgec.UseVisualStyleBackColor = false;
            // 
            // btn_Ref_Onayla
            // 
            btn_Ref_Onayla.BackColor = Color.LightCyan;
            btn_Ref_Onayla.FlatAppearance.BorderSize = 0;
            btn_Ref_Onayla.FlatStyle = FlatStyle.Popup;
            btn_Ref_Onayla.Font = new Font("Segoe UI", 10F);
            btn_Ref_Onayla.Image = Resources.Resource1.Done;
            btn_Ref_Onayla.Location = new Point(160, 339);
            btn_Ref_Onayla.Name = "btn_Ref_Onayla";
            btn_Ref_Onayla.Size = new Size(94, 51);
            btn_Ref_Onayla.TabIndex = 8;
            btn_Ref_Onayla.TextAlign = ContentAlignment.MiddleLeft;
            btn_Ref_Onayla.UseVisualStyleBackColor = false;
            btn_Ref_Onayla.Click += btn_Ref_Onayla_Click;
            // 
            // errorProvider
            // 
            errorProvider.ContainerControl = this;
            // 
            // ConveyorReffYerDegistir
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(536, 402);
            Controls.Add(btn_Ref_Vazgec);
            Controls.Add(btn_Ref_Onayla);
            Controls.Add(Panel);
            Name = "ConveyorReffYerDegistir";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Referans Noktalarının Yerlerini Değiştir";
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
        }

        #endregion
        public Button btn_Ref_Vazgec;
        public Button btn_Ref_Onayla;
        public FlowLayoutPanel Panel;
        public ErrorProvider errorProvider;
    }
}