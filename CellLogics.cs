using GUI_Library;
using Microsoft.Identity.Client;
using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using static Balya_Yerleştirme.Utilities.Utils;

namespace Balya_Yerleştirme.Models
{
    public partial class Cell
    {
        [NotMapped]
        public RectangleF Rectangle { get; set; }
        [NotMapped]
        public RectangleF OriginalRectangle { get; set; }
        [NotMapped]
        public RectangleF SelectLayoutRectangle { get; set; }
        [NotMapped]
        public Point LocationofRect { get; set; }
        [NotMapped]
        public Pen HoverPen = new Pen(Color.DarkGray);
        [NotMapped]
        MainForm ?Main { get; set; }
        [NotMapped]
        public bool Itemplacement { get; set; } = false;
        [NotMapped]
        public bool ItemplacementContextMenuStripButton { get; set; } = false;
        [NotMapped]
        public bool ItemObtainContextMenuStripButton { get; set; } = false;
        [NotMapped]
        public bool itemremoval { get; set; } = false;
        [NotMapped]
        public int drawingPanelMoveConst = 368 / 2;
        [NotMapped]
        public List<Models.Item> items {  get; set; }
        [NotMapped]
        List<ToolStripItem> ContextMenuItems {  get; set; }
        [NotMapped]
        public bool drawtag { get; set; }
        [NotMapped]
        public ToolStripItem? toolitem {  get; set; }
        [NotMapped]
        public ToolStripItem? toolitem1 { get; set; }
        [NotMapped]
        public Depo Parent { get; set; }
        [NotMapped]
        public LayoutOlusturma ?Layout { get; set; }
       

        public Cell(float x, float y, float width, float height, MainForm ?main, Depo parent, LayoutOlusturma ?layout)
        {
            Rectangle = new RectangleF(x, y, width, height);
            OriginalRectangle = new RectangleF(x, y, width, height);
            SelectLayoutRectangle = new RectangleF(x, y, width, height);

            Main = main;
            LocationofRect = new Point((int)Rectangle.X, (int)Rectangle.Y);
            Parent = parent;
            Layout = layout;
            items = new List<Models.Item>();
            ContextMenuItems = new List<ToolStripItem>();
            HoverPen.DashStyle = DashStyle.DashDot;

            if (Main != null)
            {
                foreach (ToolStripItem item in main.Balya_Context_Menu_Strip.Items)
                {
                    ContextMenuItems.Add(item);
                }

                toolitem = Main.Balya_Context_Menu_Strip.Items["referansNoktasıEkleToolStripMenuItem"];
                toolitem1 = Main.Balya_Context_Menu_Strip.Items["balyalarınReferansNoktalarınıSilToolStripMenuItem"];

                Main.ItemPlacementCancel += Cell_ItemPlacementCancelEventHandler;
                Main.ItemPlacementToolStripButtonClicked += Cell_ItemPlacementToolStripButtonClicked;
                Main.ItemPlacementContextMenuStripButtonClicked += Cell_ItemPlacementContextMenuStripButtonClicked;
                Main.ExportToExcel += Cell_ExportToExcelButtonClicked;
                Main.AddItemReferencePoint += Cell_AddItemReferencePoint;

                Main.PLCBaglantisiniAyarlaButtonClicked += Cell_PlcConnectionButton;
                Main.PLCBaglantisiPaneliniKapat += Cell_MoveRight_Event;
                Main.ToolStripNesneYerlestirClicked += Cell_MoveLeft_Event;
                Main.MoveRightEvent += Cell_MoveRight_Event;
                Main.MoveLeftEvent += Cell_MoveLeft_Event;
            }
        }

        private void Cell_MoveLeft_Event(object? sender, EventArgs e)
        {
            MoveLeft();
        }

        private void Cell_MoveRight_Event(object? sender, EventArgs e)
        {
            MoveRight();
        }

        private void Cell_PlcConnectionButton(object? sender, EventArgs e)
        {
            MoveLeft();
        }

        private void Cell_AddItemReferencePoint(object? sender, EventArgs e)
        {
            MoveRight();
        }
        private void Cell_ExportToExcelButtonClicked(object? sender, EventArgs e)
        {
            MoveRight();
        }
        private void Cell_ItemObtainContextMenuStripButtonClicked(object? sender, 
            EventArgs e)
        {
            
        }
        private void Cell_ItemPlacementContextMenuStripButtonClicked(object? sender, 
            EventArgs e)
        {
            MoveLeft();
        }
        private void Cell_ItemObtainToolStripButtonClicked(object? sender, EventArgs e)
        {
            MoveRight();
            itemremoval = true;
        }
        private void Cell_ItemPlacementToolStripButtonClicked(object? sender, EventArgs e)
        {
            MoveLeft();
        }
        private void Cell_ItemPlacementCancelEventHandler(object? sender, EventArgs e)
        {
            MoveRight();
        }
        private void Cell_ItemPlacementCloseEventHandler(object? sender, EventArgs e)
        {
            MoveRight();
        }
        private void Cell_ItemPlacementEventHandler(object? sender, EventArgs e)
        {
            MoveRight();
        }


        private void ConstrainMovementWithinParent()
        {
            if (Rectangle.Left < Parent.Rectangle.Left)
            {
                Rectangle = GVisual.MoveRectangleToPoint(Rectangle, Parent.Rectangle.Left, Rectangle.Y);
            }
            if (Rectangle.Top < Parent.Rectangle.Top)
            {
                Rectangle = GVisual.MoveRectangleToPoint(Rectangle, Rectangle.X, Parent.Rectangle.Top);
            }
            if (Rectangle.Right > Parent.Rectangle.Right)
            {
                Rectangle = GVisual.MoveRectangleToPoint(Rectangle, Parent.Rectangle.Right - Rectangle.Width, Rectangle.Y);
            }
            if (Rectangle.Bottom > Parent.Rectangle.Bottom)
            {
                Rectangle = GVisual.MoveRectangleToPoint(Rectangle, Rectangle.X, Parent.Rectangle.Bottom - Rectangle.Height);
            }
        }


        public void Draw(Graphics graphics)
        {
            RectangleF rect = new RectangleF();
            int counter = 0;
            float totalitemyuksekligi = 0;
            Pen pen = new Pen(Color.Red, 2);
            graphics.DrawRectangle(HoverPen, Rectangle);
            Font font = new Font("Arial", 8 * Zoomlevel);
            SolidBrush brush = new SolidBrush(Color.Red);
            //string column = $"C: {Column}";
            //string row = $"R: {Row}";
            //graphics.DrawString(column, font, brush, new PointF(Rectangle.X, Rectangle.Y));
            //graphics.DrawString(row, font, brush, new PointF(Rectangle.X, Rectangle.Y + 20));
            if (drawtag == true)
            {
                DrawTag(graphics, CellEtiketi, Rectangle, font, brush);
            }
            else
            {
                foreach (var item in items)
                {
                    totalitemyuksekligi += item.ItemYuksekligi;
                    counter++;
                    rect = item.Rectangle;
                }

                if (items.Count > 0)
                {
                    string text = $"{counter}";
                    SizeF textSize = graphics.MeasureString(text, font);
                    graphics.DrawString(text, font, brush, new PointF(rect.X +
                        (rect.Width / 2 - textSize.Width / 2),
                        rect.Y + (rect.Height / 2 - textSize.Height / 2)));
                }
            }

            

            foreach (var x in items)
            {
                pen = adjustItemRectangleColor(pen, x.ItemYuksekligi, CellYuksekligi);

                x.Draw(graphics, pen);
            }
        }
        public Pen adjustItemRectangleColor(Pen pen, int item_yuksekligi, int Cell_yuksekligi)
        {
            int kapasite = Cell_yuksekligi / item_yuksekligi;
            
            if (items.Count == kapasite)
            {
                pen.Color = Color.Red;
            }
            else if (items.Count == kapasite - 1)
            {
                pen.Color = AdjustToGrayShade(10);
            }
            else if (items.Count == kapasite - 2)
            {
                pen.Color = AdjustToGrayShade(80);
            }
            else if (items.Count == kapasite - 3)
            {
                pen.Color = AdjustToGrayShade(150);
            }
            else
            {
                pen.Color = AdjustToGrayShade(220);
            }
            return pen;
        }
        public void OnMouseDown(MouseEventArgs e)
        {
            if (Main != null && Layout == null)
            {
                Point scaledPoint = new Point((int)((e.X - Main.DrawingPanel.AutoScrollPosition.X)), (int)((e.Y - Main.DrawingPanel.AutoScrollPosition.Y)));

                if (e.Button == MouseButtons.Right)
                {
                    if (Rectangle.Contains(scaledPoint))
                    {
                        Main.Hide_infopanel();
                        HoverPen.Color = Color.Red;
                        //ShowContextMenu(e.Location);
                        if (items.Count > 0)
                        {
                            foreach (var item in ContextMenuItems)
                            {
                                Main.Balya_Context_Menu_Strip.Items.Add(item);
                            }
                            foreach (var item in items)
                            {
                                if (item.Rectangle.Contains(scaledPoint))
                                {
                                    if (item.ItemReferencePoints.Count > 0)
                                    {
                                        Main.Balya_Context_Menu_Strip.Show(Cursor.Position);
                                    }
                                    else
                                    {
                                        Main.Balya_Context_Menu_Strip.Items.Remove(toolitem1);
                                        Main.Balya_Context_Menu_Strip.Show(Cursor.Position);
                                    }
                                }
                            }
                        }
                        else if (items.Count == 0)
                        {
                            Main.Balya_Context_Menu_Strip.Items.Remove(toolitem);
                            Main.Balya_Context_Menu_Strip.Items.Remove(toolitem1);
                            Main.Balya_Context_Menu_Strip.Show(Cursor.Position);
                        }
                        Main.DrawingPanel.Invalidate();
                        //itemremoval = true;
                    }
                    else
                    {
                        HoverPen.Color = Color.DarkGray;
                        Main.DrawingPanel.Invalidate();
                    }
                }
                else if (e.Button == MouseButtons.Left)
                {
                    if (items.Count == 0)
                    {
                        Main.Hide_infopanel();
                    }
                }
            }
        }
        public void OnMouseMove(MouseEventArgs e)
        {
            if (Main != null && Layout == null)
            {
                Point scaledPoint = new Point((int)((e.X - Main.DrawingPanel.AutoScrollPosition.X)), (int)((e.Y - Main.DrawingPanel.AutoScrollPosition.Y)));

                Item balya = new Item();
                if (Rectangle.Contains(scaledPoint))
                {
                    if (items.Count > 0)
                    {
                        foreach (var item in items)
                        {
                            if (item == items.Last())
                            {
                                balya = item;
                            }
                        }
                        
                        Main.Show_infopanel(balya.ItemEtiketi, balya.ItemAgirligi, CellEtiketi, balya.ItemAciklamasi, Parent.DepoName);
                        if (e.Location.Y + Main.infopanel.Height > 950)
                        {
                            Main.infopanel.Location = new Point((int)(e.Location.X - Main.infopanel.Width - 5),
                                (int)(e.Location.Y - Main.infopanel.Height - 5));
                        }
                        else
                        {
                            Main.infopanel.Location = new Point((int)(e.Location.X + 10), (int)(e.Location.Y + 10));
                        }
                    }
                    else
                    {
                        Main.Hide_infopanel();
                    }
                    Main.DrawingPanel.Invalidate();
                }
            }
        }
        public void OnMouseUp(MouseEventArgs e)
        {
            
        }
        public void OnMouseDoubleClick(MouseEventArgs e)
        {
            if (Main != null)
            {
                Point scaledPoint = new Point((int)((e.X - Main.DrawingPanel.AutoScrollPosition.X)),
                (int)((e.Y - Main.DrawingPanel.AutoScrollPosition.Y)));
                if (items.Count > 0)
                {
                    if (Rectangle.Contains(scaledPoint))
                    {
                        InfoForm info = new InfoForm(items, this);
                        info.Show();
                    }
                }
            }
        }



        public void DeleteItem()
        {
            List<Item> newitems = new List<Item>();
            toplam_Nesne_Yuksekligi = 0;

            if (itemremoval)
            {
                foreach (var item in items)
                {
                    if (item == items.Last())
                    {
                        newitems.Add(item);
                    }
                }
                foreach (var item in newitems)
                {
                    if (item.ItemId != 0)
                    {
                        using (var context = new DBContext())
                        {
                            var item1 = (from x in context.Items
                                         where x.ItemId == item.ItemId
                                         select x).FirstOrDefault();

                            if (item1 != null)
                            {
                                context.Items.Remove(item1);
                                context.SaveChanges();
                            }
                        }
                    }
                    items.Remove(item);
                    itemremoval = false;
                    Main.DrawingPanel.Invalidate();
                }
                newitems.Clear();
                toplam_Nesne_Yuksekligi = NesneYuksekligi * items.Count;
            }
        }
        public void AdjustRectangletozoomlevel(float zoomlevel)
        {
            Zoomlevel = zoomlevel;
            // Calculate zoomed rectangle
            Rectangle = new RectangleF(
                (int)(Rectangle.X * zoomlevel),
                (int)(Rectangle.Y * zoomlevel),
                (int)(Rectangle.Width * zoomlevel),
                (int)(Rectangle.Height * zoomlevel));
        }
        public void MoveRectangle(float deltaX, float deltaY)
        {
            Rectangle = new RectangleF(Rectangle.X + deltaX, Rectangle.Y + deltaY, Rectangle.Width, Rectangle.Height);
            LocationofRect = new Point((int)Rectangle.X, (int)Rectangle.Y);
        }
        public void ChangeOriginalRectangleLocation(float x, float y)
        {
            OriginalRectangle = new RectangleF(OriginalRectangle.X + x, OriginalRectangle.Y + y, OriginalRectangle.Width, OriginalRectangle.Height);
        }
        public void DrawTag(Graphics g, string tag, RectangleF rect, Font font, Brush brush)
        {
            // Define a format to center the text
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            // Draw the tag in the center of the cell
            g.DrawString(tag, font, brush, rect, format);
        }
        public void MoveLeft()
        {
            var values = ShiftLeft(Rectangle, OriginalRectangle, LocationofRect, drawingPanelMoveConst, Zoomlevel);
            Rectangle = values.Item1;
            OriginalRectangle = values.Item2;
            LocationofRect = values.Item3;
        }
        public void MoveRight()
        {
            var values = ShiftRight(Rectangle, OriginalRectangle, LocationofRect, drawingPanelMoveConst, Zoomlevel);
            Rectangle = values.Item1;
            OriginalRectangle = values.Item2;
            LocationofRect = values.Item3;
        }
        public void ApplyZoom(float zoomlevel)
        {
            Zoomlevel = zoomlevel;
            RectangleF zoomedRect = new RectangleF(
                (float)(OriginalRectangle.X * zoomlevel),
                (float)(OriginalRectangle.Y * zoomlevel),
                (float)(OriginalRectangle.Width * zoomlevel),
                (float)(OriginalRectangle.Height * zoomlevel));

            if (zoomlevel <= 1.0f)
            {
                Rectangle = OriginalRectangle;
                LocationofRect = new Point((int)Rectangle.X, (int)Rectangle.Y);
            }

            if (zoomlevel > 1.0f)
            {
                Rectangle = zoomedRect;
                LocationofRect = new Point((int)Rectangle.X, (int)Rectangle.Y);
            }
            // Apply zoom to child objects if any
            foreach (var item in items)
            {
                item.ApplyZoom(zoomlevel);
            }
        }
    }
}
