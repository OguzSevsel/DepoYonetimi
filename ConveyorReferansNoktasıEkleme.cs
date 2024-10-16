using Balya_Yerleştirme.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Balya_Yerleştirme.Utilities.Utils;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Balya_Yerleştirme
{
    public partial class ConveyorReferansNoktasıEkleme : Form
    {
        public Conveyor conveyor {  get; set; }
        public Panel panel { get; set; }
        public RectangleF Rectangle { get; set; }
        public RectangleF OriginalRectangle { get; set; }
        public Pen Pen { get; set; } = new Pen(Color.Blue, 2);
        public PointF LocationofRect { get; set; }
        public int drawingPanelMoveConst = 346 / 2;
        public RectangleF PointRectangle {  get; set; }
        public List<RectangleF> PointRectangles { get; set; }
        public List<Models.ConveyorReferencePoint> ConveyorReffs { get; set; } = new List<Models.ConveyorReferencePoint>();

        MainForm Main { get; set; }
        public bool isEkle { get; set; } = false;

        public ConveyorReferansNoktasıEkleme(Conveyor Conveyor, Panel Panel)
        {
            InitializeComponent();
            this.MaximizeBox = false;
            conveyor = Conveyor;
            panel = Panel;
            PointRectangle = new RectangleF(0,0,4,4);
            PointRectangles = new List<RectangleF>();
            Rectangle = conveyor.OriginalRectangle;
            Rectangle = ResizetoNewPanel(Rectangle);
            Rectangle = CentertoRectangle(Rectangle, drawingPanel.ClientRectangle);
            drawingPanel.Invalidate();
        }
        private RectangleF ResizetoNewPanel(RectangleF rectangle)
        {
            float widthRatio = panel.Width / drawingPanel.Width;
            float heightRatio = panel.Height / drawingPanel.Height;

            float x = rectangle.X / widthRatio;
            float y = rectangle.Y / heightRatio;
            float width = rectangle.Width / widthRatio;
            float height = rectangle.Height / heightRatio;

            if (width > height * 4)
            {
                rectangle = new RectangleF(x, y, width, height * 4);
                return rectangle;
            }
            if (height > width * 4)
            {
                rectangle = new RectangleF(x, y, width * 4, height);
                return rectangle;
            }
            else
            {
                rectangle = new RectangleF(x, y, width, height);
                return rectangle;
            }
        }
        
        private void drawingPanel_MouseClick(object sender, MouseEventArgs e)
        {
            if (isEkle)
            {
                if (Rectangle.Contains(e.Location))
                {
                    if (PointRectangles.Count < 10)
                    {
                        PointRectangle = new RectangleF(e.Location.X, e.Location.Y, 6, 6);
                        PointRectangles.Add(PointRectangle);
                        drawingPanel.Invalidate();
                    }
                    else
                    {
                        MessageBox.Show("10'dan fazla referans noktasını aynı anda ekleyemezsiniz.");
                    }
                }
                else
                {
                    MessageBox.Show("Lütfen conveyor'a tıklayın");
                }
            }
        }
        private void drawingPanel_Paint(object sender, PaintEventArgs e)
        {
            float locX = 0;
            float locY = 0;
            Graphics g = e.Graphics;
            Pen.Color = Color.Red;
            g.DrawRectangle(Pen, Rectangle);
            foreach (var rect in PointRectangles)
            {
                g.FillEllipse(new SolidBrush(Color.Red), rect);
            }
        }

        private void btn_Ref_Point_Ekle_Click(object sender, EventArgs e)
        {
            if (isEkle)
            {
                isEkle = false;
                drawingPanel.BorderStyle = BorderStyle.None;
                btn_Ref_Point_Ekle.Text = "Ekle";
                btn_Ref_Point_Ekle.StateCommon.Back.Image = Resources.Resource1.Add;
                btn_Ref_Onayla.Enabled = true;
                btn_Ref_Vazgec.Enabled = true;
                btn_Ref_Onayla.StateCommon.Border.Color1 = Color.Lime;
                btn_Ref_Vazgec.StateCommon.Border.Color1 = Color.Red;
                btn_Ref_Onayla.StateCommon.Content.ShortText.Color1 = Color.Lime;
                btn_Ref_Vazgec.StateCommon.Content.ShortText.Color1 = Color.Red;
            }
            else
            {
                isEkle = true;
                drawingPanel.BorderStyle = BorderStyle.Fixed3D;
                btn_Ref_Point_Ekle.Text = "Tamamla";
                btn_Ref_Point_Ekle.StateCommon.Back.Image = null;
                btn_Ref_Onayla.Enabled = false;
                btn_Ref_Vazgec.Enabled = false;
                btn_Ref_Onayla.StateCommon.Border.Color1 = Color.LightGray;
                btn_Ref_Vazgec.StateCommon.Border.Color1 = Color.LightGray;
                btn_Ref_Onayla.StateCommon.Content.ShortText.Color1 = Color.LightGray;
                btn_Ref_Vazgec.StateCommon.Content.ShortText.Color1 = Color.LightGray;
            }
        }
        
        private void Hide_Onayla_Button()
        {
            btn_Ref_Onayla.Enabled = false;
            btn_Ref_Onayla.Visible = false;
            btn_Ref_Onayla.Hide();
            this.Controls.Remove(btn_Ref_Onayla);
        }
    }
}
