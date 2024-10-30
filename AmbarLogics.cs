using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Balya_Yerleştirme.Utilities.Utils;

namespace Balya_Yerleştirme.Models
{
    public partial class Ambar
    {
        [NotMapped]
        public RectangleF Rectangle { get; set; }
        [NotMapped]
        public RectangleF OriginalRectangle { get; set; }
        [NotMapped]
        public RectangleF SelectLayoutRectangle { get; set; }
        [NotMapped]
        public List<Conveyor> conveyors { get; set; }
        [NotMapped]
        public List<Depo> depolar {  get; set; }
        [NotMapped]
        MainForm ?Main { get; set; }
        [NotMapped]
        public Point LocationofRect { get; set; }
        [NotMapped]
        public int drawingPanelMoveConst = 368 / 2;
        [NotMapped]
        public bool DrawMeters { get; set; } = false;
        [NotMapped]
        LayoutOlusturma ?layout { get; set; }


        public Ambar(float x, float y, float width, float height, MainForm ?main, LayoutOlusturma ?Layout) 
        { 
            Rectangle = new RectangleF(x, y, width, height);
            OriginalRectangle = new RectangleF(x, y, width, height);
            SelectLayoutRectangle = new RectangleF(x, y, width, height);

            Main = main;
            layout = Layout;
            conveyors = new List<Conveyor>();
            depolar = new List<Depo>();
            LocationofRect = new Point((int)Rectangle.X, (int)Rectangle.Y);

            if (Main != null)
            {
                Main.ItemPlacementCancel += Ambar_ItemPlacementCancelEventHandler;
                Main.ItemPlacementToolStripButtonClicked += Ambar_ItemPlacementToolStripButtonClicked;
                Main.ItemPlacementContextMenuStripButtonClicked += Ambar_ItemPlacementContextMenuStripButtonClicked;
                Main.ExportToExcel += Ambar_ExportToExcelButtonClicked;
                Main.AddItemReferencePoint += Ambar_AddItemReferencePoint;
                Main.PLCBaglantisiniAyarlaButtonClicked += MoveLeft_Event;
                Main.PLCBaglantisiPaneliniKapat += MoveRight_Event;
                Main.ToolStripNesneYerlestirClicked += MoveLeft_Event;
                Main.MoveRightEvent += MoveRight_Event;
                Main.MoveLeftEvent += MoveLeft_Event;
            }
        }

        private void MoveLeft_Event(object? sender, EventArgs e)
        {
            MoveLeft();
        }

        private void MoveRight_Event(object? sender, EventArgs e)
        {
            MoveRight();
        }

        private void Ambar_PlcConnectionButton(object? sender, EventArgs e)
        {
            MoveLeft();
        }

        private void Ambar_AddItemReferencePoint(object? sender, EventArgs e)
        {
            MoveRight();
        }
        private void Ambar_ExportToExcelButtonClicked(object? sender, EventArgs e)
        {
            MoveRight();
        }
        private void Ambar_ItemPlacementContextMenuStripButtonClicked(object? sender, EventArgs e)
        {
            MoveLeft();
        }
        private void Ambar_ItemObtainToolStripButtonClicked(object? sender, EventArgs e)
        {
            MoveRight();
        }
        private void Ambar_ItemPlacementToolStripButtonClicked(object? sender, EventArgs e)
        {
            MoveLeft();
        }
        private void Ambar_ItemPlacementCancelEventHandler(object? sender, EventArgs e)
        {
            MoveRight();
        }
        private void Ambar_ItemPlacementCloseEventHandler(object? sender, EventArgs e)
        {
            MoveRight();
        }

        private void Ambar_ItemPlacementEventHandler(object? sender, EventArgs e)
        {
            var values = ShiftRight(Rectangle, OriginalRectangle, LocationofRect, 
                drawingPanelMoveConst, Zoomlevel);
            Rectangle = values.Item1;
            OriginalRectangle = values.Item2;
            LocationofRect = values.Item3;
        }
        public void Draw(Graphics graphics)
        {
            using (Pen pen = new Pen(Color.Black, 2))
            {
                graphics.DrawRectangle(pen, Rectangle);

                if (DrawMeters)
                {
                    PointF AmbarBoyuTextLocation;
                    PointF AmbarEniTextLocation;
                    PointF UpLinePoint;
                    PointF DownLinePoint;
                    PointF LeftLinePoint;
                    PointF RightLinePoint;
                    int padding = 6;

                    string textAmbarEni = $"{AmbarEni}\nMetre";
                    string textAmbarBoyu = $"{AmbarBoyu}\nMetre";

                    Font font = new Font("Arial", 8 * Zoomlevel);
                    SolidBrush brush = new SolidBrush(Color.Red);

                    SizeF textAmbarEniSize = graphics.MeasureString(textAmbarEni, font);
                    SizeF textAmbarBoyuSize = graphics.MeasureString(textAmbarBoyu, font);

                    LeftLinePoint = new PointF(Rectangle.X + Rectangle.Width / 8 + padding * Zoomlevel,
                        Rectangle.Y + padding * Zoomlevel);

                    RightLinePoint =
                        new PointF(Rectangle.X + Rectangle.Width + padding * Zoomlevel - Rectangle.Width / 8,
                        Rectangle.Y + padding * Zoomlevel);

                    UpLinePoint =
                        new PointF(Rectangle.X + Rectangle.Width + padding * Zoomlevel,
                        Rectangle.Y + Rectangle.Height / 8);

                    DownLinePoint = new PointF(Rectangle.X + Rectangle.Width + padding * Zoomlevel,
                        Rectangle.Y + Rectangle.Height - Rectangle.Height / 8);

                    AmbarEniTextLocation = new PointF(Rectangle.X + Rectangle.Width / 2 - textAmbarEniSize.Width / 2,
                        RightLinePoint.Y + padding * Zoomlevel);

                    AmbarBoyuTextLocation =
                        new PointF(Rectangle.X + Rectangle.Width + 10 * Zoomlevel,
                       Rectangle.Y + Rectangle.Height / 2 - textAmbarBoyuSize.Height / 2);

                    graphics.DrawLine(new Pen(Color.Red), LeftLinePoint, RightLinePoint);
                    graphics.DrawLine(new Pen(Color.Red), UpLinePoint, DownLinePoint);
                    graphics.DrawString(textAmbarEni, font, brush, AmbarEniTextLocation);
                    graphics.DrawString(textAmbarBoyu, font, brush, AmbarBoyuTextLocation);
                }
            }
            foreach (var conveyor in conveyors)
            {
                conveyor.Draw(graphics);
            }
            foreach (var depo in depolar)
            {
                depo.Draw(graphics);
            }
        }

        public void OnMouseDown(MouseEventArgs e)
        {
            foreach (var depo in depolar)
            {
                depo.OnMouseDown(e);
            }
            foreach (var conveyor in conveyors)
            {
                conveyor.OnMouseDown(e);
            }
            if (layout != null)
            {
                if (!layout.Fill_WareHouse)
                {
                    Point scaledPoint =
                    new Point((int)((e.X - layout.drawingPanel.AutoScrollPosition.X)),
                    (int)((e.Y - layout.drawingPanel.AutoScrollPosition.Y)));

                    if (e.Button == MouseButtons.Right)
                    {
                        if (Rectangle.Contains(scaledPoint) &&
                            layout.selectedConveyor == null &&
                            layout.selectedDepo == null)
                        {
                            layout.SelectedAmbar = this;
                            layout.SelectedAmbarPen.Width = 3;
                            layout.SelectedAmbarPen.Color = System.Drawing.Color.Blue;
                            layout.ambarMenuStrip.Show(Cursor.Position);
                            layout.SelectNode(this, null, null);
                            if (layout.LeftSide_LayoutPanel.Visible)
                            {
                                layout.SortFlowLayoutPanel(layout.layoutPanel_Ambar);
                                layout.Show_AreaMenus("Alan");
                            }
                        }
                    }
                    if (e.Button == MouseButtons.Left)
                    {
                        if (Rectangle.Contains(scaledPoint) &&
                            layout.selectedConveyor == null &&
                            layout.selectedDepo == null)
                        {
                            layout.SelectedAmbar = this;
                            layout.SelectedAmbarPen.Width = 3;
                            layout.SelectedAmbarPen.Color = System.Drawing.Color.Blue;
                            layout.SelectNode(this, null, null);
                            if (layout.LeftSide_LayoutPanel.Visible)
                            {
                                layout.SortFlowLayoutPanel(layout.layoutPanel_Ambar);
                                layout.Show_AreaMenus("Alan");
                            }
                        }
                    }
                }
            }
        }
        public void OnMouseMove(MouseEventArgs e)
        {
            foreach (var depo in depolar)
            {
                depo.OnMouseMove(e);
            }
            foreach (var conveyor in conveyors)
            {
                conveyor.OnMouseMove(e);
            }
        }
        public void OnMouseUp(MouseEventArgs e)
        {
            foreach (var conveyor in conveyors)
            {
                conveyor.OnMouseUp(e);
            }
            foreach (var depo in depolar)
            {
                depo.OnMouseUp(e);
            }
        }
        public void OnMouseDoubleClick(MouseEventArgs e)
        {
            foreach (var depo in depolar)
            {
                depo.OnMouseDoubleClick(e);
            }
        }
        public void MoveLeft()
        {
            var values = ShiftLeft(Rectangle, OriginalRectangle, LocationofRect, 
                drawingPanelMoveConst, Zoomlevel);
            Rectangle = values.Item1;
            OriginalRectangle = values.Item2;
            LocationofRect = values.Item3;
        }
        public void MoveRight()
        {
            var values = ShiftRight(Rectangle, OriginalRectangle, LocationofRect, 
                drawingPanelMoveConst, Zoomlevel);
            Rectangle = values.Item1;
            OriginalRectangle = values.Item2;
            LocationofRect = values.Item3;
        }
        public Conveyor CreateConveyor(float x, float y, float width, float height, float metersChild_Width,
            float metersChild_Height, float metersParent_Width, float metersParent_Height)
        {
            Conveyor conveyor = new Conveyor(x,y,width,height, Main, layout, this);

            conveyor.Rectangle = resizeandPosition(metersChild_Width, metersChild_Height, metersParent_Width,
                metersParent_Height,Rectangle);

            conveyor.Rectangle = ChangeRectangleLocationToAnExactPoint(conveyor.Rectangle,
                new Point(700,200));

            conveyor.OriginalRectangle = conveyor.Rectangle;
            conveyor.LocationofRect = new Point((int)conveyor.Rectangle.X, (int)conveyor.Rectangle.Y);
            conveyors.Add(conveyor);

            return conveyor;
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
            foreach (var depo in depolar)
            {
                depo.ApplyZoom(zoomlevel);
            }
            foreach (var conveyor in conveyors)
            {
                conveyor.ApplyZoom(zoomlevel);
            }
        }
    }
}
