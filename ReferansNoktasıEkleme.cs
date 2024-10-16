using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Balya_Yerleştirme.Utilities.Utils;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Balya_Yerleştirme
{
    public partial class ReferansNoktasıEkleme : Form
    {
        public RectangleF Rectangle { get; set; } = new RectangleF(100, 100, 100, 100);
        public Rectangle Top_Center_Rectangle { get; set; }
        public Rectangle Left_Center_Rectangle { get; set; }
        public Rectangle Right_Center_Rectangle { get; set; }
        public Rectangle Bottom_Center_Rectangle { get; set; }
        public Rectangle Center_Rectangle { get; set; }
        public bool Left_Center { get; set; } = false;
        public bool Top_Center { get; set; } = false;
        public bool Right_Center { get; set; } = false;
        public bool Bottom_Center { get; set; } = false;
        public bool Center { get; set; } = false;
        public bool isRedraw { get; set; } = false;
        public Pen pen = new Pen(Color.Red);

        public ReferansNoktasıEkleme()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            SetDoubleBuffered(RefDrawingPanel);
            //Rectangle = new RectangleF(100,100,100,100);
            Rectangle = CentertoRectangle(Rectangle, RefDrawingPanel.ClientRectangle);

            Top_Center_Rectangle = new Rectangle(4, 4, 4, 4);
            Left_Center_Rectangle = new Rectangle(4, 4, 4, 4);
            Right_Center_Rectangle = new Rectangle(4, 4, 4, 4);
            Bottom_Center_Rectangle = new Rectangle(4, 4, 4, 4);
            Center_Rectangle = new Rectangle(4, 4, 4, 4);
        }
        private void RefDrawingPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(this.BackColor);
            SolidBrush brush = new SolidBrush(Color.Blue);
            g.DrawRectangle(pen, Rectangle);
            Point top_center = GetMiddleOfTopEdge(Rectangle);
            Point right_center = GetMiddleOfRightEdge(Rectangle);
            Point left_center = GetMiddleOfLeftEdge(Rectangle);
            Point bottom_center = GetMiddleOfBottomEdge(Rectangle);
            Point center = GetCenter(Rectangle);

            Rectangle top_rect = new Rectangle(top_center.X, top_center.Y, 4, 4);
            Rectangle right_rect = new Rectangle(right_center.X, right_center.Y, 4, 4);
            Rectangle left_rect = new Rectangle(left_center.X, left_center.Y, 4, 4);
            Rectangle bottom_rect = new Rectangle(bottom_center.X, bottom_center.Y, 4, 4);
            Rectangle center_rect = new Rectangle(center.X, center.Y, 4, 4);

            if (Top_Center)
            {
                g.FillRectangle(brush, top_rect);
            }
            if (Bottom_Center)
            {
                g.FillRectangle(brush, bottom_rect);
            }
            if (Left_Center)
            {
                g.FillRectangle(brush, left_rect);
            }
            if (Right_Center)
            {
                g.FillRectangle(brush, right_rect);
            }
            if (Center)
            {
                g.FillRectangle(brush, center_rect);
            }
        }



        private Point GetCenter(RectangleF rect)
        {
            float centerX = (rect.Left + rect.Width / 2);
            float centerY = (rect.Top + rect.Height / 2);
            return new Point((int)centerX, (int)centerY);
        }
        private Point GetMiddleOfTopEdge(RectangleF rect)
        {
            float middleX = rect.Left + rect.Width / 2;
            float topY = rect.Top;
            return new Point((int)middleX, (int)topY);
        }
        private Point GetMiddleOfBottomEdge(RectangleF rect)
        {
            float middleX = rect.Left + rect.Width / 2;
            float bottomY = rect.Bottom;
            return new Point((int)middleX, (int)bottomY);
        }
        private Point GetMiddleOfLeftEdge(RectangleF rect)
        {
            float leftX = rect.Left;
            float middleY = rect.Top + rect.Height / 2;
            return new Point((int)leftX, (int)middleY);
        }
        private Point GetMiddleOfRightEdge(RectangleF rect)
        {
            float rightX = rect.Right;
            float middleY = rect.Top + rect.Height / 2;
            return new Point((int)rightX, (int)middleY);
        }



        private void check_Ust_Orta_CheckStateChanged(object sender, EventArgs e)
        {
            if (check_Ust_Orta.CheckState == CheckState.Checked)
            {
                Top_Center = true;
                RefDrawingPanel.Invalidate();
            }
            else
            {
                Top_Center = false;
                RefDrawingPanel.Invalidate();
            }
        }
        private void check_Sag_Orta_CheckStateChanged(object sender, EventArgs e)
        {
            if (check_Sag_Orta.CheckState == CheckState.Checked)
            {
                Right_Center = true;
                RefDrawingPanel.Invalidate();
            }
            else
            {
                Right_Center = false;
                RefDrawingPanel.Invalidate();
            }
        }
        private void check_Merkez_CheckStateChanged(object sender, EventArgs e)
        {
            if (check_Merkez.CheckState == CheckState.Checked)
            {
                Center = true;
                RefDrawingPanel.Invalidate();
            }
            else
            {
                Center = false;
                RefDrawingPanel.Invalidate();
            }
        }
        private void check_Alt_Orta_CheckStateChanged(object sender, EventArgs e)
        {
            if (check_Alt_Orta.CheckState == CheckState.Checked)
            {
                Bottom_Center = true;
                RefDrawingPanel.Invalidate();
            }
            else
            {
                Bottom_Center = false;
                RefDrawingPanel.Invalidate();
            }
        }
        private void check_Sol_Orta_CheckStateChanged(object sender, EventArgs e)
        {
            if (check_Sol_Orta.CheckState == CheckState.Checked)
            {
                Left_Center = true;
                RefDrawingPanel.Invalidate();
            }
            else
            {
                Left_Center = false;
                RefDrawingPanel.Invalidate();
            }
        }
    }
}
