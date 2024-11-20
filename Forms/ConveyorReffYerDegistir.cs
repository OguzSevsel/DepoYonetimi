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
using String_Library;
using static Balya_Yerleştirme.Utilities.Utils;

namespace Balya_Yerleştirme
{
    public partial class ConveyorReffYerDegistir : Form
    {
        public Conveyor Conveyor { get; set; }

        public ConveyorReffYerDegistir(Conveyor conveyor)
        {
            InitializeComponent();
            this.Conveyor = conveyor;
            InitializeReferencePoints(Conveyor);
        }

        public void InitializeReferencePoints(Conveyor conveyor)
        {
            Label Title = new Label();
            Title.AutoSize = true;
            Title.Text = "Referans Noktaları";
            Title.Location = new Point(this.Width / 2 - Title.Width / 2, 10);
            this.Controls.Add(Title);
            foreach (var reff in conveyor.ConveyorReferencePoints)
            {
                Panel panel = new Panel();
                panel.Size = new Size(230, 150);
                panel.BorderStyle = BorderStyle.Fixed3D;
                panel.Tag = reff;

                Label LabelLocationX = new Label();
                LabelLocationX.AutoSize = true;
                LabelLocationX.Text = "Referans noktasının\n x eksenindeki yeri (cm):";
                LabelLocationX.Location = new Point(10, 10);

                Label LabelLocationY = new Label();
                LabelLocationY.AutoSize = true;
                LabelLocationY.Text = "Referans noktasının\n y eksenindeki yeri (cm):";
                LabelLocationY.Location = new Point(LabelLocationX.Left, LabelLocationX.Bottom + 40);

                TextBox textBoxLocationX = new TextBox();
                textBoxLocationX.Size = new Size(50, textBoxLocationX.Height);
                textBoxLocationX.Text = $"{reff.LocationX}";
                textBoxLocationX.Name = "textBoxLocationX";
                textBoxLocationX.Location = new Point(LabelLocationX.Right + 50, LabelLocationX.Top + (LabelLocationX.Height / 2 - textBoxLocationX.Height / 2));

                TextBox textBoxLocationY = new TextBox();
                textBoxLocationY.Size = new Size(50, textBoxLocationY.Height);
                textBoxLocationY.Text = $"{reff.LocationY}";
                textBoxLocationY.Name = "textBoxLocationY";
                textBoxLocationY.Location = new Point(LabelLocationY.Right + 50, LabelLocationY.Top + (LabelLocationY.Height / 2 - textBoxLocationY.Height / 2));

                panel.Controls.Add(LabelLocationX);
                panel.Controls.Add(LabelLocationY);
                panel.Controls.Add(textBoxLocationX);
                panel.Controls.Add(textBoxLocationY);

                Panel.Controls.Add(panel);
            }
        }

        private void btn_Ref_Onayla_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();
            float Reff_X = -1;
            float Reff_Y = -1;
            foreach (Panel panel in Panel.Controls)
            {
                foreach (var textbox in panel.Controls)
                {
                    if (textbox is TextBox)
                    {
                        TextBox textBox = new TextBox();
                        textBox = (TextBox)textbox;
                        if (textBox.Name == "textBoxLocationX")
                        {

                            Reff_X = StrLib.ReplaceDotWithCommaReturnFloatButAllowZeros(textBox, errorProvider, "Bu alan boş bırakılamaz.", "Buraya bir sayı girmelisiniz.", "Buraya 0'dan daha küçük bir değer giremezsiniz.");

                            if (Reff_X > Conveyor.ConveyorEni * 100)
                            {
                                errorProvider.SetError(textBox, "Buraya Conveyor'un eninden daha büyük bir sayı giremezsiniz");
                            }
                        }
                        if (textBox.Name == "textBoxLocationY")
                        {
                            Reff_Y = StrLib.ReplaceDotWithCommaReturnFloatButAllowZeros(textBox, errorProvider, "Bu alan boş bırakılamaz.", "Buraya bir sayı girmelisiniz.", "Buraya 0'dan daha küçük bir değer giremezsiniz.");

                            if (Reff_Y > Conveyor.ConveyorBoyu * 100)
                            {
                                errorProvider.SetError(textBox, "Buraya Conveyor'un boyundan daha büyük bir sayı giremezsiniz");
                            }
                        }
                    }
                }
            }
            if (Reff_X != -1 && Reff_Y != -1 && !errorProvider.HasErrors)
            {
                this.DialogResult = DialogResult.OK;
            }
        }
    }
}
