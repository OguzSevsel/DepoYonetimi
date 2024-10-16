namespace Balya_Yerleştirme
{
    partial class DepodakiMallariGoruntule
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            panel1 = new Panel();
            DepoDataPanel = new Panel();
            DepoDataGridView = new DataGridView();
            lbl_Nesne_Gotuntule_Title = new Krypton.Toolkit.KryptonWrapLabel();
            panel1.SuspendLayout();
            DepoDataPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)DepoDataGridView).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.Silver;
            panel1.Controls.Add(DepoDataPanel);
            panel1.Controls.Add(lbl_Nesne_Gotuntule_Title);
            panel1.Location = new Point(12, 12);
            panel1.Name = "panel1";
            panel1.Size = new Size(1240, 657);
            panel1.TabIndex = 0;
            // 
            // DepoDataPanel
            // 
            DepoDataPanel.Controls.Add(DepoDataGridView);
            DepoDataPanel.Location = new Point(160, 183);
            DepoDataPanel.Name = "DepoDataPanel";
            DepoDataPanel.Size = new Size(918, 372);
            DepoDataPanel.TabIndex = 2;
            // 
            // DepoDataGridView
            // 
            DepoDataGridView.AllowUserToAddRows = false;
            DepoDataGridView.AllowUserToDeleteRows = false;
            DepoDataGridView.AllowUserToResizeColumns = false;
            DepoDataGridView.AllowUserToResizeRows = false;
            DepoDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DepoDataGridView.Location = new Point(215, 14);
            DepoDataGridView.Name = "DepoDataGridView";
            DepoDataGridView.ReadOnly = true;
            DepoDataGridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            DepoDataGridView.RowsDefaultCellStyle = dataGridViewCellStyle1;
            DepoDataGridView.ShowEditingIcon = false;
            DepoDataGridView.Size = new Size(527, 344);
            DepoDataGridView.TabIndex = 1;
            DepoDataGridView.VisibleChanged += DataGridView_VisibleChanged;
            // 
            // lbl_Nesne_Gotuntule_Title
            // 
            lbl_Nesne_Gotuntule_Title.AutoSize = false;
            lbl_Nesne_Gotuntule_Title.Font = new Font("Microsoft Sans Serif", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lbl_Nesne_Gotuntule_Title.ForeColor = Color.Red;
            lbl_Nesne_Gotuntule_Title.LabelStyle = Krypton.Toolkit.LabelStyle.AlternateControl;
            lbl_Nesne_Gotuntule_Title.Location = new Point(484, 117);
            lbl_Nesne_Gotuntule_Title.Name = "lbl_Nesne_Gotuntule_Title";
            lbl_Nesne_Gotuntule_Title.Size = new Size(270, 53);
            lbl_Nesne_Gotuntule_Title.StateCommon.Font = new Font("Microsoft Sans Serif", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lbl_Nesne_Gotuntule_Title.StateCommon.TextColor = Color.Red;
            lbl_Nesne_Gotuntule_Title.Text = "Depodaki Nesneler";
            lbl_Nesne_Gotuntule_Title.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // DepodakiMallariGoruntule
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1264, 681);
            Controls.Add(panel1);
            Name = "DepodakiMallariGoruntule";
            Text = "Nesneleri Görüntüle";
            panel1.ResumeLayout(false);
            DepoDataPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)DepoDataGridView).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Krypton.Toolkit.KryptonWrapLabel lbl_Nesne_Gotuntule_Title;
        private Panel DepoDataPanel;
        private DataGridView DepoDataGridView;
    }
}