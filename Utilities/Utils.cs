using Balya_Yerleştirme.Models;
using Krypton.Toolkit;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Balya_Yerleştirme.Utilities
{
    public static class Utils
    {
        public static Point GetCenter(RectangleF rect)
        {
            float centerX = (rect.Left + rect.Width / 2);
            float centerY = (rect.Top + rect.Height / 2);
            return new Point((int)centerX, (int)centerY);
        }

        public static Point GetMiddleOfTopEdge(RectangleF rect)
        {
            float middleX = rect.Left + rect.Width / 2;
            float topY = rect.Top;
            return new Point((int)middleX, (int)topY);
        }

        public static Point GetMiddleOfBottomEdge(RectangleF rect)
        {
            float middleX = rect.Left + rect.Width / 2;
            float bottomY = rect.Bottom;
            return new Point((int)middleX, (int)bottomY);
        }

        public static Point GetMiddleOfLeftEdge(RectangleF rect)
        {
            float leftX = rect.Left;
            float middleY = rect.Top + rect.Height / 2;
            return new Point((int)leftX, (int)middleY);
        }

        public static Point GetMiddleOfRightEdge(RectangleF rect)
        {
            float rightX = rect.Right;
            float middleY = rect.Top + rect.Height / 2;
            return new Point((int)rightX, (int)middleY);
        }

        public static RectangleF resizeandPosition(float takenfromform_childwidth, 
            float takenfromform_childheight, float takenfromform_parentwidth, 
            float takenfromform_parentheight, RectangleF parentrect)
        {
            try
            {
                checked
                {
                    // Use long to prevent overflow
                    double ratioWidth = (double)takenfromform_childwidth / (double)takenfromform_parentwidth;
                    double ratioHeight = (double)takenfromform_childheight / (double)takenfromform_parentheight;

                    double depowidth = (double)((parentrect.Width * ratioWidth));
                    double depoheight = (double)((parentrect.Height * ratioHeight));

                    // Convert to int with overflow check
                    int newdepoalaniwidth = Convert.ToInt32(depowidth);
                    int newdepoalaniheight = Convert.ToInt32(depoheight);

                    int posX = (int)((parentrect.X + (parentrect.Width - newdepoalaniwidth) / 2));
                    int posY = (int)((parentrect.Y + (parentrect.Height - newdepoalaniheight) / 2));

                    return new RectangleF(posX, posY, newdepoalaniwidth, newdepoalaniheight);
                }
            }
            catch (OverflowException ex)
            {
                MessageBox.Show($"An overflow occurred: {ex.Message}", "Overflow Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return RectangleF.Empty; // Return an empty rectangle if overflow occurs
            }
        }

        public static RectangleF CentertoPanel(RectangleF rectangleToCenter, Panel panel)
        {
            float posX = (panel.Width - (rectangleToCenter.Width / 2));
            float posY = (panel.Height - (rectangleToCenter.Height / 2));

            return new RectangleF(posX, posY, rectangleToCenter.Width, rectangleToCenter.Height);
        }

        public static RectangleF CentertoRectangle(RectangleF rectangleToCenter, 
            RectangleF parentRect)
        {
            float posX = ((parentRect.X + (parentRect.Width - rectangleToCenter.Width) / 2));
            float posY = ((parentRect.Y + (parentRect.Height - rectangleToCenter.Height) / 2));

            return new RectangleF(posX, posY, rectangleToCenter.Width, rectangleToCenter.Height);
        }

        public static string CapitalizeFirstLetter(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            str = str.Trim();

            if (str.Length == 1)
            {
                return char.ToUpper(str[0], CultureInfo.CurrentCulture).ToString();
            }

            return char.ToUpper(str[0], CultureInfo.CurrentCulture) + str.Substring(1).ToLower(CultureInfo.CurrentCulture);
        }

        public static Point MoveControlInsidePanel(Control control, Panel panel, 
            string SolAlt_SolÜst_SağAlt_SağÜst_Merkez)
        {
            if (SolAlt_SolÜst_SağAlt_SağÜst_Merkez == "Sol Alt")
            {
                return new Point(2, panel.Height - control.Height - 2);
            }
            else if (SolAlt_SolÜst_SağAlt_SağÜst_Merkez == "Sol Üst")
            {
                return new Point(2, 2);
            }
            else if (SolAlt_SolÜst_SağAlt_SağÜst_Merkez == "Sağ Alt")
            {
                return new Point(panel.Width - control.Width - 2, panel.Height - control.Height - 2);
            }
            else if (SolAlt_SolÜst_SağAlt_SağÜst_Merkez == "Sağ Üst")
            {
                return new Point(panel.Width - control.Width - 2, 2);
            }
            else if (SolAlt_SolÜst_SağAlt_SağÜst_Merkez == "Merkez")
            {
                return new Point(panel.Width / 2 - control.Width / 2, panel.Height / 2 - control.Height / 2);
            }
            else
            {
                return Point.Empty;
            }
        }

        public static Point CenterControlInsidePanelX(Control control, Panel panel)
        {
            return new Point(panel.Width / 2 - control.Width / 2, control.Location.Y);
        }

        public static Point MoveControltoSidesofAnotherControl(Control movedControl, 
            Control referenceControl, int paddingBetweenControls, string Sol_Sağ_Alt_Üst)
        {
            if (Sol_Sağ_Alt_Üst == "Sol")
            {
                return new Point(referenceControl.Location.X - movedControl.Width - paddingBetweenControls,
                    referenceControl.Location.Y + (referenceControl.Height / 2 - movedControl.Height / 2));
            }
            else if (Sol_Sağ_Alt_Üst == "Sağ")
            {
                return new Point(referenceControl.Right + paddingBetweenControls,
                    referenceControl.Location.Y + (referenceControl.Height / 2 - movedControl.Height / 2));
            }
            else if (Sol_Sağ_Alt_Üst == "Alt")
            {
                return new Point(referenceControl.Location.X + (referenceControl.Width / 2 - movedControl.Width / 2),
                    referenceControl.Location.Y + referenceControl.Height + paddingBetweenControls);
            }
            else if (Sol_Sağ_Alt_Üst == "Üst")
            {
                return new Point(referenceControl.Location.X + (referenceControl.Width / 2 - movedControl.Width / 2),
                    referenceControl.Location.Y - movedControl.Height - paddingBetweenControls);
            }
            else
            {
                return Point.Empty;
            }
        }

        public static void CenterTwoControlInsidePanel(Control control, Control secondControl, 
            int Padding, Panel panel)
        {
            control.Location = new Point((panel.Width / 2 - control.Width) - Padding, (panel.Height / 2 - control.Height / 2));
            secondControl.Location = new Point(control.Right + Padding, control.Location.Y);
        }

        public static void CenterTwoControlHorizontallyInsidePanel(Control control, 
            Control secondControl, int Padding, Panel panel, int LocationY)
        {
            control.Location = new Point((panel.Width / 2 - control.Width - Padding), LocationY);
            secondControl.Location = new Point((panel.Width / 2 + Padding), LocationY);
        }

        public static void CenterThreeControlHorizontallyInsidePanel(Control control, 
            Control secondControl, Control thirdControl, int padding, Panel panel, 
            int LocationY)
        {
            int sidepadding = panel.Width - (control.Width + padding * 2 + secondControl.Width + thirdControl.Width);
            if (sidepadding > 0)
            {
                control.Location = new Point((sidepadding / 2), LocationY);
                secondControl.Location = new Point((control.Right + padding), LocationY);
                thirdControl.Location = new Point((secondControl.Right + padding), LocationY);
            }
        }

        public static void CenterTwoControlVerticallyInsidePanel(Control control, 
            Control secondControl, int Padding, Panel panel, int LocationX)
        {
            control.Location = new Point(LocationX - Padding, panel.Height / 2 - control.Height / 2);
            secondControl.Location = new Point(control.Right + Padding, control.Location.Y);
        }

        public static void CenterTwoControlVerticallyStackedInsidePanel(Control control, 
            Control secondControl, int Padding, Panel panel, int LocationX)
        {
            control.Location = 
                new Point(LocationX, panel.Height / 2 - control.Height - Padding);

            secondControl.Location = new Point(LocationX, panel.Height / 2 + Padding);
        }

        public static void CenterThreeControlVerticallyStackedInsidePanel(Control control, 
            Control secondControl, Control thirdControl, int Padding, Panel panel, 
            int LocationX)
        {
            control.Location = new Point(LocationX, panel.Height / 2 - control.Height * 3 - Padding);
            secondControl.Location = new Point(LocationX, control.Bottom + Padding);
            thirdControl.Location = new Point(LocationX, secondControl.Bottom + Padding);

        }

        public static void MainPanelMakeSmaller(Panel panel)
        {
            panel.Size = new Size(1513, 909);
            panel.Location = new Point(379, 91);
        }

        public static void MainPanelMakeBigger(Panel panel)
        {
            panel.Size = new Size(1880, 909);
            panel.Location = new Point(12, 91);
        }

        public static void MainPanelMakeSmallest(Panel panel)
        {
            panel.Size = new Size(1143, 909);
            panel.Location = new Point(379, 91);
        }

        public static void MainPanelMakeSmaller1(Panel panel)
        {
            panel.Size = new Size(1513, 909);
            panel.Location = new Point(12, 91);
        }

        public static void ShowSidePanel(Control ShowControl, Control ParentControl)
        {
            ShowControl.Visible = true;
            ShowControl.Enabled = true;
            ShowControl.Show();
            ParentControl.Controls.Add(ShowControl);
            ShowControl.Location = new Point(12, 108);
        }

        public static void HideSingleSidePanel(Control SidePanel, Control ParentPanel)
        {
            SidePanel.Hide();
            SidePanel.Enabled = false;
            SidePanel.Visible = false;
            ParentPanel.Controls.Remove(SidePanel);
        }

        public static void RemoveControlfromPanelAddItToAnotherPanel(Control control, 
            Panel removepanel, Panel addpanel)
        {
            removepanel.Controls.Remove(control);
            addpanel.Controls.Remove(control);
            control.Visible = true;
            control.Enabled = true;
            control.Show();
        }

        public static void CenterVertically(Control control, Panel panel)
        {
            control.Location = new Point(control.Location.X, panel.Height / 2 - control.Height / 2);
        }

        public static void CenterHorizontally(Control control, Panel panel)
        {
            control.Location = new Point(panel.Width / 2 - control.Width / 2, control.Location.Y);
        }

        public static RectangleF CenterVertically(RectangleF Rectangle, 
            RectangleF ParentRectangle)
        {
            Rectangle.Location = new Point((int)(Rectangle.Location.X), (int)(ParentRectangle.Location.Y + 
                (ParentRectangle.Height / 2 - Rectangle.Height / 2)));
            return Rectangle;
        }

        public static RectangleF CenterHorizontally(RectangleF Rectangle, 
            RectangleF ParentRectangle)
        {
            Rectangle.Location = new Point((int)(ParentRectangle.Location.X + 
                (ParentRectangle.Width / 2 - Rectangle.Width / 2)), (int)(Rectangle.Location.Y));

            return Rectangle;
        }

        public static void DrawVerticalStringRepeated
            (Graphics g, string text, RectangleF rect, Font font, Brush brush, int spacing)
        {
            GraphicsState state = g.Save();

            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            g.TranslateTransform(rect.Left, rect.Top);

            g.RotateTransform(90);

            float yOffset = -(rect.Width / 2);
            float xOffset = 0;

            float drawableHeight = rect.Height;

            SizeF textSize = g.MeasureString(text, font);

            while (xOffset + textSize.Height <= drawableHeight)
            {
                xOffset += textSize.Height + spacing;
                if (xOffset + textSize.Height < drawableHeight)
                {
                    g.DrawString(text, font, brush, xOffset, yOffset, format);
                }
            }

            g.Restore(state);
        }

        public static void DrawHorizontalStringRepeated
            (Graphics g, string text, RectangleF rect, Font font, Brush brush, int spacing)
        {
            GraphicsState state = g.Save();

            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            SizeF textSize = g.MeasureString(text, font);

            float xOffset = rect.Left;
            float yOffset = rect.Top + (rect.Height / 2);

            while (xOffset + textSize.Width <= rect.Right)
            {
                xOffset += textSize.Width + spacing;
                if (xOffset + textSize.Width < rect.Right)
                {
                    g.DrawString(text, font, brush, new PointF(xOffset, yOffset), format);
                }
            }

            g.Restore(state);
        }

        public static void PaintwithDashedPen(Rectangle zoomedRect, Graphics g)
        {
            using (Pen dashedPen = new Pen(Color.Green, 2))
            {
                dashedPen.DashStyle = DashStyle.Dash;

                g.DrawRectangle(dashedPen, zoomedRect);
            }
        }

        public static void SetDoubleBuffered(Control control)
        {
            System.Reflection.PropertyInfo doubleBufferPropertyInfo =
                typeof(Control).GetProperty("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance);

            doubleBufferPropertyInfo.SetValue(control, true, null);
        }

        public static void DeleteText(List<KryptonTextBox> textBoxes)
        {
            foreach (var txt in textBoxes)
            {
                txt.Clear();
            }
        }

        public static void ClearTextBoxes(Control control)
        {
            List<KryptonTextBox> list = new List<KryptonTextBox>();

            foreach (Control textbox in control.Controls)
            {
                if (textbox is KryptonTextBox)
                {
                    list.Add((KryptonTextBox)textbox);
                }
            }
            if (list.Count > 0)
            {
                DeleteText(list);
            }
        }

        public static void AddDepoToDatabase
            (int ambar_id, string depo_name, string depodescription, float depo_alani_eni,
            float depo_alani_boyu, int depo_alani_yuksekligi, float original_depo_size_width, 
            float original_depo_size_height, float kare_x, float kare_y, float kare_eni, 
            float kare_boyu, float original_kare_x, float original_kare_y, 
            float original_kare_eni, float original_kare_boyu, float zoomlevel,
            string itemDrop_StartLocation, string itemDrop_UpDown, 
            string itemDrop_LeftRight, int itemDrop_Stage1, int itemDrop_Stage2, 
            int yerlestirilme_Sirasi, float depo_alani_eni_cm,
            float depo_alani_boyu_cm, int column_count, int row_count, int current_column, int current_row,
            string item_turu, int asama1_itemsayisi, int asama2_toplamitemsayisi, int current_stage)
        {
            using (var context = new DBContext())
            {
                Depo depo = new Depo(ambar_id, depo_name, depodescription, depo_alani_eni, depo_alani_boyu, depo_alani_yuksekligi, original_depo_size_width,
                original_depo_size_height, kare_x, kare_y, kare_eni, kare_boyu, original_kare_x, original_kare_y, original_kare_eni, original_kare_boyu,
                zoomlevel, itemDrop_StartLocation, itemDrop_UpDown, itemDrop_LeftRight,
                itemDrop_Stage1, itemDrop_Stage2, yerlestirilme_Sirasi, depo_alani_eni_cm,
                depo_alani_boyu_cm, column_count, row_count, current_column, current_row, item_turu,
                asama1_itemsayisi, asama2_toplamitemsayisi, current_stage);

                context.Depos.Add(depo);
                context.SaveChanges();
            }
        }

        public static RectangleF ChangeRectangleLocation(RectangleF rectangle, float deltaX, 
            float deltaY)
        {
            rectangle = new RectangleF(rectangle.X + deltaX, rectangle.Y + deltaY, 
                                       rectangle.Width, rectangle.Height);
            return rectangle;
        }

        public static RectangleF ChangeRectangleLocationToAnExactPoint(RectangleF rectangle, 
            Point point)
        {
            rectangle = new RectangleF(point.X, point.Y, rectangle.Width, rectangle.Height);
            return rectangle;
        }

        public static RectangleF ChangeRectangleSize(RectangleF rectangle, float width, 
            float height)
        {
            rectangle = new RectangleF(rectangle.X, rectangle.Y, width, height);
            return rectangle;
        }

        public static RectangleF ChangeRectangleLocationAndSize
            (RectangleF rectangle, Point point, Size size)
        {
            rectangle = new RectangleF(point.X, point.Y, size.Width, size.Height);
            return rectangle;
        }

        public static RectangleF ResizeRectangleToRatioOfPanel
            (Panel drawingpanel, RectangleF rectangle)
        {
            double ratioX = (double)drawingpanel.Width / (double)rectangle.Width;
            double ratioY = (double)drawingpanel.Height / (double)rectangle.Height;
            double ratio = Math.Min(ratioX, ratioY);

            double newwidth = (rectangle.Width * ratio);
            double newheight = (rectangle.Height * ratio);

            int newHeight = Convert.ToInt32(newheight);
            int newWidth = Convert.ToInt32(newwidth);

            float posX = (drawingpanel.Width - newWidth) / 2;
            float posY = (drawingpanel.Height - newHeight) / 2;

            rectangle = new RectangleF(posX, posY, newWidth, newHeight);
            return rectangle;
        }

        public static (RectangleF,RectangleF,Point) ShiftLeft
            (RectangleF Rectangle, RectangleF OriginalRectangle, Point LocationofRect, 
            int drawingPanelMoveConst, float zoomlevel)
        {
            Rectangle = new RectangleF(Rectangle.X - (drawingPanelMoveConst), Rectangle.Y, Rectangle.Width, Rectangle.Height);
            Point point = new Point((int)(OriginalRectangle.X - (drawingPanelMoveConst)), (int)(OriginalRectangle.Y));
            OriginalRectangle = ChangeRectangleLocationToAnExactPoint(OriginalRectangle, point);
            LocationofRect = new Point((int)Rectangle.X, (int)Rectangle.Y);
            return (Rectangle, OriginalRectangle, LocationofRect);
        }

        public static (RectangleF, RectangleF, Point) ShiftRight
            (RectangleF Rectangle, RectangleF OriginalRectangle, Point LocationofRect, 
            int drawingPanelMoveConst, float zoomlevel)
        {
            Rectangle = new RectangleF(Rectangle.X + (drawingPanelMoveConst), Rectangle.Y, Rectangle.Width, Rectangle.Height);
            Point point = new Point((int)(OriginalRectangle.X + (drawingPanelMoveConst)), (int)(OriginalRectangle.Y));
            OriginalRectangle = ChangeRectangleLocationToAnExactPoint(OriginalRectangle, point);
            LocationofRect = new Point((int)Rectangle.X, (int)Rectangle.Y);
            return (Rectangle, OriginalRectangle, LocationofRect);
        }

        public static Color AdjustToGrayShade(int value)
        {
            value = Math.Clamp(value, 0, 255);

            return Color.FromArgb(value, value, value);
        }

        public static (float, float) ConvertRectanglesLocationtoCMInsideParentRectangle(
            RectangleF childRectangle, RectangleF parentRectangle,
            float parentAreaWidthMeters, float parentAreaHeightMeters, bool roundToInt)
        {
            float areaWidthCm = parentAreaWidthMeters * 100;
            float areaHeightCm = parentAreaHeightMeters * 100;

            float widthCmPx = areaWidthCm / parentRectangle.Width;
            float heightCmPx = areaHeightCm / parentRectangle.Height;

            float ChildCenterX = childRectangle.X + childRectangle.Width / 2;
            float ChildCenterY = childRectangle.Y + childRectangle.Height / 2;

            float childLocationInsideParentX = ChildCenterX - parentRectangle.X;
            float childLocationInsideParentY = ChildCenterY - parentRectangle.Y;

            float cmLocX = childLocationInsideParentX * widthCmPx;
            float cmLocY = childLocationInsideParentY * heightCmPx;

            double cmLocXRounded = Math.Round(cmLocX);
            double cmLocYRounded = Math.Round(cmLocY);

            if (roundToInt)
            {
                return ((int)cmLocXRounded, (int)cmLocYRounded);
            }
            else
            {
                double cmLocXRounded1 = Math.Round(cmLocX, 2);
                double cmLocYRounded1 = Math.Round(cmLocY, 2);
                return ((float)cmLocXRounded1, (float)cmLocYRounded1);
            }
        }


        public static PointF ResizeandMoveChildRectangleInsideParent(
            RectangleF parentRectangle, float ChildCmX, float ChildCmY, float ParentWidthCm,
            float ParentHeightCm, RectangleF childRectangle)
        {
            float ParentWidthCmPx = parentRectangle.Width / ParentWidthCm;
            float ParentHeightCmPx = parentRectangle.Height / ParentHeightCm;

            float CmX = ChildCmX * ParentWidthCmPx;
            float CmY = ChildCmY * ParentHeightCmPx;

            return new PointF((parentRectangle.X + CmX - (childRectangle.Width / 2)), 
                (parentRectangle.Y + CmY - (childRectangle.Height / 2)));
        }
    }
}
