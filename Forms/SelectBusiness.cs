using Balya_Yerleştirme.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GUI_Library;
using DocumentFormat.OpenXml.Drawing.Charts;

namespace Balya_Yerleştirme.Forms
{
    public partial class SelectBusiness : Form
    {
        #region Ui Operations Variables such as location and sizes


        public System.Drawing.Point rightPanelLocation = new System.Drawing.Point(1258, 70);
        public System.Drawing.Size MainPanelSmallSize { get; set; } = new System.Drawing.Size(1240, 768);
        public System.Drawing.Size MainPanelLargeSize { get; set; } = new System.Drawing.Size(1560, 768);

        public System.Drawing.Point Btn_ChangeLayoutDescLocation = new System.Drawing.Point(242, 217);

        public System.Drawing.Point Btn_ChangeLayoutNameLocation = new System.Drawing.Point(242, 18);

        public System.Drawing.Point SmallTitleLocation = new System.Drawing.Point(426, 9);

        public System.Drawing.Point LargeTitleLocation = new System.Drawing.Point(595, 9);


        #endregion

        
        
        #region Useful Objects


        public GroupBox? selectedGroupBox { get; set; } = null;
        MainForm main { get; set; }


        #endregion



        public SelectBusiness(MainForm main)
        {
            InitializeComponent();
            this.main = main;
            GVisual.HideControl(panel_CreateIsletme, this);
            GVisual.HideControl(panel_IsletmeMenu, this);

            GetIsletmeFromDB();
        }



        #region Initial Creation of GroupBoxes


        private void GetIsletmeFromDB()
        {
            using (var context = new DBContext())
            {
                var isletmeler = (from x in context.Isletme
                                  select x).ToList();

                if (isletmeler.Count > 0)
                {
                    foreach (var isletme in isletmeler)
                    {
                        var layouts = (from x in context.Layout
                                       where x.IsletmeID == isletme.IsletmeID
                                       select x).ToList();

                        CreateGroupBox(isletme, layouts, false);
                    }
                }
            }
        }
        private void CreateGroupBox(Isletme isletme, List<Models.Layout>? layouts, bool iscreating)
        {
            GroupBox groupBox = new GroupBox();
            groupBox.Size = new System.Drawing.Size(500, 500);
            
            groupBox.Font = new Font("Arial", 22);
            groupBox.Text = isletme.Name;
            groupBox.ForeColor = System.Drawing.Color.Blue;
            groupBox.Tag = isletme;

            ListBox layoutListBox = new ListBox();
            layoutListBox.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
            //layoutListBox.BorderStyle = BorderStyle.None;
            layoutListBox.BorderStyle = BorderStyle.FixedSingle;

            if (layouts != null)
            {
                foreach (var layout in layouts)
                {
                    layoutListBox.Items.Add(layout.Name);
                }
            }

            layoutListBox.ForeColor = System.Drawing.Color.Red;
            layoutListBox.Size = new System.Drawing.Size(400, 400);
            layoutListBox.Location = new Point(groupBox.Width / 2 - layoutListBox.Width / 2, groupBox.Height / 2 - layoutListBox.Height / 2 + 12);

            groupBox.Controls.Add(layoutListBox);

            if (iscreating)
            {
                EnlargeorShrinkInsidePanel(SelectBusinessPanel, groupBox, layoutListBox, MainPanelLargeSize, MainPanelSmallSize);
            }

            layoutListBox.MouseDown += (sender, e) => groupBox_MouseDown(sender, e);
            layoutListBox.MouseEnter += (sender, e) => groupBox_MouseEnter(sender, e);
            layoutListBox.MouseLeave += (sender, e) => groupBox_MouseLeave(sender, e);



            SelectBusinessPanel.Controls.Add(groupBox);

            groupBox.MouseDown += (sender, e) => groupBox_MouseDown(sender, e);
            groupBox.MouseEnter += (sender, e) => groupBox_MouseEnter(sender, e);
            groupBox.MouseLeave += (sender, e) => groupBox_MouseLeave(sender, e);
        }


        #endregion



        #region CreateIsletmeEventsandMethods


        //Button Events for Creating Isletme
        private void btn_CreateIsletme_Click(object sender, EventArgs e)
        {
            if (SelectBusinessPanel.Size == MainPanelLargeSize)
            {
                OpenRightSide(panel_CreateIsletme);
            }
            else
            {
                if (panel_IsletmeMenu.Visible)
                {
                    if (selectedGroupBox != null)
                    {
                        selectedGroupBox.ForeColor = System.Drawing.Color.Blue;
                        selectedGroupBox.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
                        selectedGroupBox = null;
                    }

                    CloseRightSide(panel_IsletmeMenu);
                    OpenRightSide(panel_CreateIsletme);
                }
                else
                {
                    CloseRightSide(panel_CreateIsletme);
                }
            }
        }
        private void btn_CreateIsletme_Olustur_Click(object sender, EventArgs e)
        {
            string isletme_name = txt_CreateIsletme_Name.Text;
            string isletme_description = txt_CreateIsletme_Description.Text;

            if (isletme_name.Length > 30)
            {
                errorProvider.SetError(txt_CreateIsletme_Name, "İşletmenin ismi 30 karakterden uzun olamaz.");
            }
            if (!errorProvider.HasErrors)
            {
                Isletme isletme = new Isletme(isletme_name, isletme_description, 0);

                using (var context = new DBContext())
                {
                    context.Isletme.Add(isletme);
                    context.SaveChanges();
                    isletme.IsletmeID = isletme.IsletmeID;
                }
                CreateGroupBox(isletme, null, true);
                CloseRightSide(panel_CreateIsletme);
                txt_CreateIsletme_Description.Clear();
                txt_CreateIsletme_Name.Clear();
            }
        }
        private void btn_CreateIsletme_Vazgec_Click(object sender, EventArgs e)
        {
            txt_CreateIsletme_Description.Clear();
            txt_CreateIsletme_Name.Clear();
            CloseRightSide(panel_CreateIsletme);
        }
        

        #endregion



        #region UIOperationEventsandMethods


        //Runtime Created Controls Events
        private void groupBox_MouseLeave(object sender, EventArgs e)
        {
            if (selectedGroupBox == null)
            {
                GroupBox gB = sender as GroupBox;
                ListBox lB = sender as ListBox;

                if (gB != null)
                {
                    gB.ForeColor = System.Drawing.Color.Blue;
                    gB.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
                }
                else if (lB != null)
                {
                    FindGroupBox(lB);
                }
            }
        }
        private void groupBox_MouseEnter(object sender, EventArgs e)
        {
            if (selectedGroupBox == null)
            {
                GroupBox gB = sender as GroupBox;
                ListBox lB = sender as ListBox;

                if (gB != null)
                {
                    gB.ForeColor = System.Drawing.Color.Red;
                    gB.BackColor = System.Drawing.Color.AliceBlue;
                }
                else if (lB != null)
                {
                    FindGroupBox(lB);
                }
            }
        }
        private void groupBox_MouseDown(object sender, MouseEventArgs e)
        {
            GroupBox gB = sender as GroupBox;
            ListBox lB = sender as ListBox;

            if (gB != null)
            {
                OpenRightSide(panel_IsletmeMenu);
                gB.ForeColor = System.Drawing.Color.Red;
                gB.BackColor = System.Drawing.Color.AliceBlue;
                selectedGroupBox = gB;
                AdjustOtherGroupboxesColors(selectedGroupBox);
                Isletme isletme = selectedGroupBox.Tag as Isletme;

                if (isletme != null)
                {
                    lbl_IsletmeMenu_Title.Text = isletme.Name;
                    txt_ChangeIsletmeDescription.Text = isletme.Description;
                    txt_ChangeIsletmeName.Text = isletme.Name;
                    GVisual.HideControl(btn_ChangeIsletmeDescription, InnerPanel1);
                    GVisual.HideControl(btn_ChangeIsletmeName, InnerPanel1);
                }
            }
            else if (lB != null)
            {
                OpenRightSide(panel_IsletmeMenu);
                selectedGroupBox = FindGroupBox(lB);
                AdjustOtherGroupboxesColors(selectedGroupBox);

                Isletme isletme = selectedGroupBox.Tag as Isletme;

                if (isletme != null)
                {
                    lbl_IsletmeMenu_Title.Text = isletme.Name;
                    txt_ChangeIsletmeDescription.Text = isletme.Description;
                    txt_ChangeIsletmeName.Text = isletme.Name;
                    GVisual.HideControl(btn_ChangeIsletmeDescription, InnerPanel1);
                    GVisual.HideControl(btn_ChangeIsletmeName, InnerPanel1);
                }
            }
        }
        private void SelectBusinessPanel_MouseDown(object sender, MouseEventArgs e)
        {
            bool isinside = false;
            foreach (System.Windows.Forms.Control control in SelectBusinessPanel.Controls)
            {
                if (control.ClientRectangle.Contains(e.Location))
                {
                    isinside = true;
                }
            }
            if (!isinside)
            {
                if (selectedGroupBox != null)
                {
                    selectedGroupBox.ForeColor = System.Drawing.Color.Blue;
                    selectedGroupBox.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
                    selectedGroupBox = null;

                    if (panel_IsletmeMenu.Visible)
                    {
                        CloseRightSide(panel_IsletmeMenu);
                    }
                    else if (panel_CreateIsletme.Visible)
                    {
                        CloseRightSide(panel_CreateIsletme);
                    }
                }
                else if (panel_CreateIsletme.Visible)
                {
                    CloseRightSide(panel_CreateIsletme);
                }
            }
        }



        //Utility Methods for UI
        private void OpenRightSide(System.Windows.Forms.Control showControl)
        {
            if (!showControl.Visible)
            {
                EnlargeorShrinkInsidePanel(SelectBusinessPanel, null, null, MainPanelLargeSize, MainPanelSmallSize);
            }
            
            GVisual.ChangeSize_of_Control(SelectBusinessPanel, MainPanelSmallSize);
            GVisual.ShowControl(showControl, this, rightPanelLocation);
        }
        private void CloseRightSide(System.Windows.Forms.Control hideControl)
        {
            if (hideControl.Visible)
            {
                EnlargeorShrinkInsidePanel(SelectBusinessPanel, null, null, MainPanelSmallSize, MainPanelLargeSize);
            }

            GVisual.HideControl(hideControl, this);
            GVisual.ChangeSize_of_Control(SelectBusinessPanel, MainPanelLargeSize);
        }
        private void EnlargeorShrinkInsidePanel(FlowLayoutPanel panel, GroupBox ?groupbox, ListBox ?listbox, System.Drawing.Size before, System.Drawing.Size after)
        {
            if (groupbox == null)
            {
                float widthRatio = (float)after.Width / before.Width;
                float heightRatio = (float)after.Height / before.Height;

                foreach (System.Windows.Forms.Control control in panel.Controls)
                {
                    int newWidth = (int)(control.Width * widthRatio);
                    int newHeight = (int)(control.Height * heightRatio);

                    control.Size = new System.Drawing.Size(newWidth, newHeight);

                    int newX = (int)(control.Location.X * widthRatio);
                    int newY = (int)(control.Location.Y * heightRatio);

                    control.Location = new System.Drawing.Point(newX, newY);

                    foreach (System.Windows.Forms.Control childcontrol in control.Controls)
                    {
                        int newWidth1 = (int)(childcontrol.Width * widthRatio);
                        int newHeight1 = (int)(childcontrol.Height * heightRatio);

                        childcontrol.Size = new System.Drawing.Size(newWidth1, newHeight1);

                        int newX1 = (int)(childcontrol.Location.X * widthRatio);
                        int newY1 = (int)(childcontrol.Location.Y * heightRatio);

                        childcontrol.Location = new System.Drawing.Point(newX1, newY1);
                    }
                }
            }
            else
            {
                float widthRatio = (float)after.Width / before.Width;
                float heightRatio = (float)after.Height / before.Height;
                int newWidth = (int)(groupbox.Width * widthRatio);
                int newHeight = (int)(groupbox.Height * heightRatio);
                groupbox.Size = new System.Drawing.Size(newWidth, newHeight);
                int newX = (int)(groupbox.Location.X * widthRatio);
                int newY = (int)(groupbox.Location.Y * heightRatio);
                groupbox.Location = new System.Drawing.Point(newX, newY);

                if (listbox != null)
                {
                    newWidth = (int)(listbox.Width * widthRatio);
                    newHeight = (int)(listbox.Height * heightRatio);
                    listbox.Size = new System.Drawing.Size(newWidth, newHeight);
                    newX = (int)(listbox.Location.X * widthRatio);
                    newY = (int)(listbox.Location.Y * heightRatio);
                    listbox.Location = new System.Drawing.Point(newX, newY);
                }
            }
        }
        private GroupBox? FindGroupBox(System.Windows.Forms.Control control)
        {
            foreach (var control1 in SelectBusinessPanel.Controls)
            {
                if (control1 is GroupBox)
                {
                    GroupBox groupBox = (GroupBox)control1;

                    if (groupBox.Controls.Contains(control))
                    {
                        groupBox.ForeColor = System.Drawing.Color.Red;
                        groupBox.BackColor = System.Drawing.Color.AliceBlue;
                        return groupBox;
                    }
                }
            }
            return null;
        }
        private void AdjustOtherGroupboxesColors(GroupBox groupbox)
        {
            foreach (System.Windows.Forms.Control control in SelectBusinessPanel.Controls)
            {
                if (control is GroupBox)
                {
                    GroupBox gb = (GroupBox)control;

                    if (control != groupbox)
                    {
                        if (gb.ForeColor == System.Drawing.Color.Red)
                        {
                            gb.ForeColor = System.Drawing.Color.Blue;
                            gb.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
                        }
                    }
                }
            }
        }

        #endregion



        #region ChangeIsletmeInfoandLoadIsletmeEvents


        //Button Events for Changing Isletme Name and Description
        private void btn_ChangeIsletmeName_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();
            if (selectedGroupBox != null)
            {
                string isletme_name = txt_ChangeIsletmeName.Text;
                if (isletme_name.Length > 30)
                {
                    errorProvider.SetError(txt_ChangeIsletmeName, "İşletme ismi 30 karakterden uzun olamaz.");
                }

                using (var context = new DBContext())
                {
                    var isletme = (from x in context.Isletme
                                   where x.Name == isletme_name
                                   select x).FirstOrDefault();

                    if (isletme != null)
                    {
                        errorProvider.SetError(txt_ChangeIsletmeName, "Aynı isimli bir işletme bulunuyor, lütfen başka bir isim verin.");
                    }
                }

                if (!errorProvider.HasErrors)
                {
                    Isletme isletme = (Isletme)selectedGroupBox.Tag;
                    
                    if (isletme != null)
                    {
                        using (var context = new DBContext())
                        {
                            var isletme1 = (from x in context.Isletme
                                            where x.IsletmeID == isletme.IsletmeID
                                            select x).FirstOrDefault();

                            if (isletme1 != null)
                            {
                                isletme1.Name = isletme_name;
                                isletme.Name = isletme_name;
                                selectedGroupBox.Text = isletme_name;
                                lbl_IsletmeMenu_Title.Text = isletme_name;
                                context.SaveChanges();
                                GVisual.HideControl(btn_ChangeIsletmeName, InnerPanel1);
                            }
                        }
                    }
                }
            }
        }
        private void btn_ChangeIsletmeDescription_Click(object sender, EventArgs e)
        {
            if (selectedGroupBox != null)
            {
                string isletme_description = txt_ChangeIsletmeDescription.Text;
                
                Isletme isletme = (Isletme)selectedGroupBox.Tag;

                if (isletme != null)
                {
                    using (var context = new DBContext())
                    {
                        var isletme1 = (from x in context.Isletme
                                        where x.IsletmeID == isletme.IsletmeID
                                        select x).FirstOrDefault();

                        if (isletme1 != null)
                        {
                            isletme1.Description = isletme_description;
                            isletme.Description = isletme_description;
                            context.SaveChanges();
                            GVisual.HideControl(btn_ChangeIsletmeDescription, InnerPanel1);
                        }
                    }
                }
            }
        }


        //TextChanged Events for changing isletme name and description
        private void txt_ChangeIsletmeName_TextChanged(object sender, EventArgs e)
        {
            if (this.Controls.Contains(panel_IsletmeMenu))
            {
                GVisual.ShowControl(btn_ChangeIsletmeName, InnerPanel1);
            }

            if (txt_ChangeIsletmeName.Text.Length == 0)
            {
                GVisual.HideControl(btn_ChangeIsletmeName, InnerPanel1);
            }
        }
        private void txt_ChangeIsletmeDescription_TextChanged(object sender, EventArgs e)
        {
            if (this.Controls.Contains(panel_IsletmeMenu))
            {
                GVisual.ShowControl(btn_ChangeIsletmeDescription, InnerPanel1);
            }

            if (txt_ChangeIsletmeDescription.Text.Length == 0)
            {
                GVisual.HideControl(btn_ChangeIsletmeDescription, InnerPanel1);
            }
        }


        //Button events for loading and deleting isletme
        private void btn_Isletme_Yukle_Click(object sender, EventArgs e)
        {
            if (selectedGroupBox != null)
            {
                Isletme isletme = selectedGroupBox.Tag as Isletme;

                if (isletme != null)
                {
                    if (main.Isletme != null)
                    {
                        if (main.Isletme.IsletmeID == isletme.IsletmeID && main.Isletme.Name == isletme.Name)
                        {

                        }
                        else
                        {
                            main.Isletme = isletme;
                            main.lbl_SelectedIsletme_Value.Text = isletme.Name;
                            main.lbl_SelectedLayout_Value.Text = "Seçilmedi";

                            if (main.ambar != null)
                            {
                                main.ambar = null;
                                main.DrawingPanel.Invalidate();
                            }
                        }
                    }
                    else
                    {
                        main.Isletme = isletme;
                        main.lbl_SelectedIsletme_Value.Text = isletme.Name;
                        main.lbl_SelectedLayout_Value.Text = "Seçilmedi";

                        if (main.ambar != null)
                        {
                            main.ambar = null;
                            main.DrawingPanel.Invalidate();
                        }
                    }
                    
                    this.Hide();
                    this.Close();
                }
            }
        }
        private void btn_Sil_Click(object sender, EventArgs e)
        {
            if (selectedGroupBox != null)
            {
                string isletme_name = txt_ChangeIsletmeName.Text;
                if (isletme_name.Length > 30)
                {
                    errorProvider.SetError(txt_ChangeIsletmeName, "İşletme ismi 30 karakterden uzun olamaz.");
                }
                if (!errorProvider.HasErrors)
                {

                    var result = MessageBox.Show($"{isletme_name} isimli işletmeyi silmek istiyor musunuz?", "Silmek istiyor musunuz?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        Isletme isletme = (Isletme)selectedGroupBox.Tag;

                        if (isletme != null)
                        {
                            using (var context = new DBContext())
                            {
                                var isletme1 = (from x in context.Isletme
                                                where x.IsletmeID == isletme.IsletmeID
                                                select x).FirstOrDefault();

                                if (isletme1 != null)
                                {
                                    context.Isletme.Remove(isletme1);
                                    context.SaveChanges();
                                    if (SelectBusinessPanel.Controls.Contains(selectedGroupBox))
                                    {
                                        SelectBusinessPanel.Controls.Remove(selectedGroupBox);
                                    }
                                    selectedGroupBox.Tag = null;
                                    selectedGroupBox = null;
                                    CloseRightSide(panel_IsletmeMenu);
                                }
                            }
                        }
                    }
                    else
                    {
                        selectedGroupBox.ForeColor = System.Drawing.Color.Blue;
                        selectedGroupBox.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
                        CloseRightSide(panel_IsletmeMenu);
                        selectedGroupBox = null;
                    }
                }
            }
        }


        #endregion
    }
}
