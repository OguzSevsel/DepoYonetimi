using Azure.Core.GeoJson;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using GUI_Library;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Balya_Yerleştirme.Utilities.Utils;

namespace Balya_Yerleştirme.Models
{
    public partial class Conveyor
    {
        [NotMapped]
        public RectangleF Rectangle { get; set; }
        [NotMapped]
        public RectangleF OriginalRectangle { get; set; }
        [NotMapped]
        public RectangleF SelectLayoutRectangle { get; set; }
        [NotMapped]
        MainForm ?Main { get; set; }
        [NotMapped]
        public Point LocationofRect { get; set; }
        [NotMapped]
        public int drawingPanelMoveConst = 368 / 2;
        [NotMapped]
        public LayoutOlusturma? layout { get; set; }
        [NotMapped]
        public bool isDragging { get; set; } = false;
        [NotMapped]
        public PointF DragStartPoint = new Point();
        [NotMapped]
        public Ambar Parent { get; set; }

        public Conveyor(float x, float y, float width, float height, MainForm ?main, LayoutOlusturma ?Layout, Ambar ambar)
        {
            Rectangle = new RectangleF(x, y, width, height);
            OriginalRectangle = new RectangleF(x, y, width, height);
            SelectLayoutRectangle = new RectangleF(x, y, width, height);

            Main = main;
            layout = Layout;
            Parent = ambar;
            LocationofRect = new Point((int)Rectangle.X, (int)Rectangle.Y);
        }

        public Conveyor Clone()
        {
            return new Conveyor
            {
                SelectLayoutRectangle = this.SelectLayoutRectangle,
                Rectangle = this.Rectangle,
                OriginalRectangle = this.OriginalRectangle,
                KareX = this.KareX,
                KareY = this.KareY,
                KareEni = this.KareEni,
                KareBoyu = this.KareBoyu,
                OriginalKareX = this.OriginalKareX,
                OriginalKareY = this.OriginalKareY,
                OriginalKareEni = this.OriginalKareEni,
                OriginalKareBoyu = this.OriginalKareBoyu,
                ConveyorId = this.ConveyorId,
                AmbarId = this.AmbarId,
                Ambar = this.Ambar,
                Parent = this.Parent,
                LocationofRect = this.LocationofRect,
                drawingPanelMoveConst = this.drawingPanelMoveConst,
                Main = this.Main,
                Zoomlevel = this.Zoomlevel,
                ConveyorAraligi = this.ConveyorAraligi,
                ConveyorBoyu = this.ConveyorBoyu,
                ConveyorEni = this.ConveyorEni,

                ConveyorReferencePoints = this.ConveyorReferencePoints.Select(d => d.Clone()).ToList(),
                // Clone other properties...
            };
        }

        public void Draw(Graphics graphics)
        {
            using (Pen pen = new Pen(System.Drawing.Color.Black, 3))
            {
                graphics.DrawRectangle(pen, Rectangle);
                //graphics.FillRectangle(new SolidBrush(System.Drawing.Color.AliceBlue), Rectangle);

                System.Drawing.Font font = new System.Drawing.Font("Arial", 10);
                System.Drawing.Brush brush = new SolidBrush(System.Drawing.Color.Red);

                //if (Rectangle.Width > Rectangle.Height)
                //{
                //    DrawHorizontalStringRepeated(graphics, "Conveyor", Rectangle, font, brush, 60);
                //}
                //else
                //{
                //    DrawVerticalStringRepeated(graphics, "Conveyor", Rectangle, font, brush, 60);
                //}
                foreach (var reff in ConveyorReferencePoints)
                {
                    reff.Draw(graphics);
                }
            }
        }



        public void OnMouseUp(MouseEventArgs e)
        {
            if (Main != null)
            {
                foreach (var reff in ConveyorReferencePoints)
                {
                    reff.OnMouseUp(e);
                    if (reff.isdragging == true)
                    {
                        reff.isdragging = false;
                    }
                }
            }
            
            if (layout != null)
            {
                layout.isMoving = false;
                isDragging = false;
            }
        }
        public void OnMouseDown(MouseEventArgs e)
        {
            if (Main != null)
            {
                Point scaledPoint = new Point((int)((e.X - Main.DrawingPanel.AutoScrollPosition.X)), 
                    (int)((e.Y - Main.DrawingPanel.AutoScrollPosition.Y)));

                if (e.Button == MouseButtons.Right)
                {
                    if (Rectangle.Contains(scaledPoint))
                    {
                        Main.Hide_infopanel();
                        //Main.ConveyorContextMenu.Show(Cursor.Position);
                        Main.DrawingPanel.Invalidate();
                    }
                    else
                    {
                        Main.DrawingPanel.Invalidate();
                    }
                }
                foreach (var reff in ConveyorReferencePoints)
                {
                    reff.OnMouseDown(e);
                }
            }
            
            if (layout != null)
            {
                Point scaledPoint = new Point((int)((e.X - layout.drawingPanel.AutoScrollPosition.X)), 
                    (int)((e.Y - layout.drawingPanel.AutoScrollPosition.Y)));

                if (e.Button == MouseButtons.Right)
                {
                    if (Rectangle.Contains(scaledPoint))
                    {
                        layout.menuProcess = false;
                        layout.selectedConveyor = this;
                        if (!layout.Manuel_Move)
                        {
                            layout.txt_Width.Text = $"{ConveyorEni}";
                            layout.txt_Height.Text = $"{ConveyorBoyu}";
                        }
                        layout.SelectNode(null,this, null);
                        layout.SelectedConveyorPen.Color = System.Drawing.Color.Blue;
                        layout.SelectedConveyorPen.Width = 3;
                        layout.SelectedConveyorEdgePen.Width = 3;
                        layout.conveyorMenuStrip.Show(Cursor.Position);

                        if (layout.LeftSide_LayoutPanel.Visible)
                        {
                            layout.SortFlowLayoutPanel(layout.layoutPanel_SelectedConveyor);
                            layout.Show_ConveyorMenus("Conveyor");
                        }

                        layout.drawingPanel.Invalidate();
                    }
                }

                if (e.Button == MouseButtons.Left)
                {
                    if (Rectangle.Contains(scaledPoint))
                    {
                        if (!layout.Fill_WareHouse && !layout.AddReferencePoint)
                        {
                            layout.menuProcess = false;
                            layout.selectedConveyor = this;
                            
                            if (!layout.Manuel_Move)
                            {
                                layout.txt_Width.Text = $"{ConveyorEni}";
                                layout.txt_Height.Text = $"{ConveyorBoyu}";
                            }
                            layout.SelectNode(null, this, null);
                            layout.SelectedConveyorPen.Color = System.Drawing.Color.Blue;
                            layout.SelectedConveyorPen.Width = 3;
                            layout.SelectedConveyorEdgePen.Width = 3;
                            layout.isMoving = true;
                            isDragging = true;
                            DragStartPoint = e.Location;
                            //holderRectangle = new RectangleF(Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);
                            if (layout.LeftSide_LayoutPanel.Visible)
                            {
                                layout.SortFlowLayoutPanel(layout.layoutPanel_SelectedConveyor);
                                layout.Show_ConveyorMenus("Conveyor");
                            }
                            layout.drawingPanel.Invalidate();
                        }
                    }
                    else
                    {
                        if (layout.AddReferencePoint)
                        {
                            layout.AddReferencePoint = false;

                            GVisual.HideControl(layout.Conveyor_Reference_Fixed_Panel, layout.drawingPanel);
                            GVisual.HideControl(layout.Conveyor_Reference_FixedorManuel_Panel, layout.drawingPanel);
                            GVisual.HideControl(layout.Conveyor_Reference_Sayisi_Paneli, layout.drawingPanel);
                        }
                    }
                    foreach (var reff in ConveyorReferencePoints)
                    {
                        reff.OnMouseDown(e);
                    }
                }
            }
        }
        public void OnMouseMove(MouseEventArgs e)
        {
            bool notCollidedwithConveyor = false;
            bool notCollidedwithDepo = false;
            if (layout != null)
            {
                Point scaledPoint = new Point((int)((e.X - layout.drawingPanel.AutoScrollPosition.X)),
                    (int)((e.Y - layout.drawingPanel.AutoScrollPosition.Y)));

                if (isDragging)
                {
                    float deltaX = e.X - DragStartPoint.X;
                    float deltaY = e.Y - DragStartPoint.Y;
                    DragStartPoint = e.Location;
                    //holderRectangle = GVisual.MoveRectangle(holderRectangle, deltaX, deltaY);
                    foreach (var conveyor in Parent.conveyors)
                    {
                        if (Parent.conveyors.Count > 1)
                        {
                            if (this != conveyor)
                            {
                                if (!Rectangle.IntersectsWith(conveyor.Rectangle))
                                {
                                    notCollidedwithConveyor = true;
                                }
                                else
                                {
                                    notCollidedwithConveyor = false;
                                    LocationofRect = new Point((int)Rectangle.X, (int)Rectangle.Y);
                                    if (layout != null && layout.Manuel_Move == false)
                                    {
                                        layout.UnchangedselectedConveyorRectangle = Rectangle;
                                    }
                                }
                            }
                        }
                        else
                        {
                            Rectangle = GVisual.MoveRectangle(Rectangle, deltaX, deltaY);
                            LocationofRect = new Point((int)Rectangle.X, (int)Rectangle.Y);
                            foreach (var reff in ConveyorReferencePoints)
                            {
                                reff.MoveRectangle(deltaX, deltaY);
                            }
                            ConstrainMovementWithinParent();
                            OriginalRectangle = Rectangle;
                            if (layout != null && layout.Manuel_Move == false)
                            {
                                layout.UnchangedselectedConveyorRectangle = Rectangle;
                            }
                        }
                    }
                    if (Parent.depolar.Count > 0)
                    {
                        foreach (var depo in Parent.depolar)
                        {
                            if (!Rectangle.IntersectsWith(depo.Rectangle))
                            {
                                notCollidedwithDepo = true;
                            }
                            else
                            {
                                notCollidedwithDepo = false;
                                LocationofRect = new Point((int)Rectangle.X, (int)Rectangle.Y);
                                if (layout != null && layout.Manuel_Move == false)
                                {
                                    layout.UnchangedselectedConveyorRectangle = Rectangle;
                                }
                            }
                        }
                    }
                    else
                    {
                        notCollidedwithDepo = true;
                    }

                    if (notCollidedwithConveyor == true && notCollidedwithDepo == true)
                    {
                        Rectangle = GVisual.MoveRectangle(Rectangle, deltaX, deltaY);
                        LocationofRect = new Point((int)Rectangle.X, (int)Rectangle.Y);
                        foreach (var reff in ConveyorReferencePoints)
                        {
                            reff.MoveRectangle(deltaX, deltaY);
                        }
                        ConstrainMovementWithinParent();
                        OriginalRectangle = Rectangle;
                        if (layout != null && layout.Manuel_Move == false)
                        {
                            layout.UnchangedselectedConveyorRectangle = Rectangle;
                        }
                    }
                }
            }
        }


        private void ConstrainMovementWithinParent()
        {
            if (Rectangle.Left < Parent.Rectangle.Left)
            {
                Rectangle = 
                    GVisual.MoveRectangleToPoint(Rectangle, Parent.Rectangle.Left, Rectangle.Y);
                LocationofRect = new Point((int)Rectangle.X, (int)Rectangle.Y);
                foreach (var reff in ConveyorReferencePoints)
                {
                    reff.MoveRectangleExact(Rectangle.X + reff.OriginalLocationInsideParent.X,
                        Rectangle.Y + reff.OriginalLocationInsideParent.Y);
                }
            }
            if (Rectangle.Top < Parent.Rectangle.Top)
            {
                Rectangle = 
                    GVisual.MoveRectangleToPoint(Rectangle, Rectangle.X, Parent.Rectangle.Top);
                LocationofRect = new Point((int)Rectangle.X, (int)Rectangle.Y);
                foreach (var reff in ConveyorReferencePoints)
                {
                    reff.MoveRectangleExact(Rectangle.X + reff.OriginalLocationInsideParent.X,
                        Rectangle.Y + reff.OriginalLocationInsideParent.Y);
                }
            }
            if (Rectangle.Right > Parent.Rectangle.Right)
            {
                Rectangle = 
                    GVisual.MoveRectangleToPoint(Rectangle, Parent.Rectangle.Right - Rectangle.Width, 
                    Rectangle.Y);
                LocationofRect = new Point((int)Rectangle.X, (int)Rectangle.Y);
                foreach (var reff in ConveyorReferencePoints)
                {
                    reff.MoveRectangleExact(Rectangle.X + reff.OriginalLocationInsideParent.X,
                        Rectangle.Y + reff.OriginalLocationInsideParent.Y);
                }
            }
            if (Rectangle.Bottom > Parent.Rectangle.Bottom)
            {
                Rectangle = 
                    GVisual.MoveRectangleToPoint(Rectangle, Rectangle.X, 
                    Parent.Rectangle.Bottom - Rectangle.Height);
                LocationofRect = new Point((int)Rectangle.X, (int)Rectangle.Y);
                foreach (var reff in ConveyorReferencePoints)
                {
                    reff.MoveRectangleExact(Rectangle.X + reff.OriginalLocationInsideParent.X,
                        Rectangle.Y + reff.OriginalLocationInsideParent.Y);
                }
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
            foreach (var refpoint in ConveyorReferencePoints)
            {
                refpoint.ApplyZoom(zoomlevel);
            }
        }
    }
}
