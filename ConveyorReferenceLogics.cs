﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using static Balya_Yerleştirme.Utilities.Utils;

namespace Balya_Yerleştirme.Models
{
    public partial class ConveyorReferencePoint
    {
        [NotMapped]
        public RectangleF Rectangle { get; set; }
        [NotMapped]
        public RectangleF OriginalRectangle { get; set; }
        [NotMapped]
        public RectangleF SelectLayoutRectangle { get; set; }
        [NotMapped]
        public Pen Pen { get; set; } = new Pen(Color.Blue, 2);
        [NotMapped]
        public Point LocationofRect { get; set; }
        [NotMapped]
        public int drawingPanelMoveConst = 368 / 2;
        [NotMapped]
        MainForm ?Main { get; set; }
        [NotMapped]
        public bool isdragging { get; set; } = false;
        [NotMapped]
        public Point DragStartPoint { get; set; }
        [NotMapped]
        public Conveyor ParentConveyor { get; set; }
        [NotMapped]
        public LayoutOlusturma ?Layout { get; set; }
        [NotMapped]
        public PointF OriginalLocationInsideParent { get; set; }
        [NotMapped]
        public bool Info { get; set; } = false;
        [NotMapped]
        public float LocationX { get; set; }
        [NotMapped]
        public float LocationY { get; set; }
        [NotMapped]
        public string FixedPointLocation { get; set; } = string.Empty;



        public ConveyorReferencePoint(float x, float y, float width, float height, float zoomlevel, MainForm ?main, Conveyor parentConveyor, LayoutOlusturma ?layout)
        {
            Rectangle = new RectangleF(x, y, width, height);
            OriginalRectangle = new RectangleF(x, y, width, height);
            SelectLayoutRectangle = new RectangleF(x, y, width, height);

            Zoomlevel = zoomlevel;
            Main = main;
            Layout = layout;
            ParentConveyor = parentConveyor;

            if (Main != null)
            {
                Main.ItemPlacementCancel += ConveyorRefs_ItemPlacementCancelEventHandler;
                Main.ItemPlacementToolStripButtonClicked += ConveyorRefs_ItemPlacementToolStripButtonClicked;
                Main.ItemPlacementContextMenuStripButtonClicked += ConveyorRefs_ItemPlacementContextMenuStripButtonClicked;
                Main.ExportToExcel += ConveyorRefs_ExportToExcelButtonClicked;
                Main.AddItemReferencePoint += ConveyorRefs_AddItemReferencePoint;
                Main.PLCBaglantisiniAyarlaButtonClicked += ConveyorRefs_PlcConnectionButton;
                Main.PLCBaglantisiPaneliniKapat += ConveyorRefs_MoveRight_Event;
                Main.ToolStripNesneYerlestirClicked += ConveyorRefs_MoveLeft_Event;
                Main.MoveRightEvent += ConveyorRefs_MoveRight_Event;
                Main.MoveLeftEvent += ConveyorRefs_MoveLeft_Event;
            }
        }

        private void ConveyorRefs_MoveLeft_Event(object? sender, EventArgs e)
        {
            MoveLeft();
        }

        private void ConveyorRefs_MoveRight_Event(object? sender, EventArgs e)
        {
            MoveRight();
        }

        private void ConveyorRefs_PlcConnectionButton(object? sender, EventArgs e)
        {
            MoveLeft();
        }
        private void ConveyorRefs_AddItemReferencePoint(object? sender, EventArgs e)
        {
            MoveRight();
        }
        private void ConveyorRefs_ExportToExcelButtonClicked(object? sender, EventArgs e)
        {
            MoveRight();
        }
        private void ConveyorRefs_ItemPlacementContextMenuStripButtonClicked(object? sender, EventArgs e)
        {
            MoveLeft();
        }
        private void ConveyorRefs_ItemObtainToolStripButtonClicked(object? sender, EventArgs e)
        {
            MoveRight();
        }
        private void ConveyorRefs_ItemPlacementToolStripButtonClicked(object? sender, EventArgs e)
        {
            MoveLeft();
        }
        private void ConveyorRefs_ItemPlacementCancelEventHandler(object? sender, EventArgs e)
        {
            MoveRight();
        }
        private void ConveyorRefs_ItemPlacementCloseEventHandler(object? sender, EventArgs e)
        {
            MoveRight();
        }
        private void ConveyorRefs_ItemPlacementEventHandler(object? sender, EventArgs e)
        {
            MoveRight();
        }





        public void Draw(Graphics graphics)
        {
            Font font = new Font("Arial", 12);
            SolidBrush brush = new SolidBrush(Color.Red);
            graphics.FillEllipse(brush, Rectangle);

            if (Layout != null)
            {
                if (Info)
                {
                    SolidBrush infobrush = new SolidBrush(Color.Blue);

                    string X = $"X: {LocationX} cm";
                    string Y = $"Y: {LocationY} cm";

                    SizeF sizeX = graphics.MeasureString(X, font);
                    SizeF sizeY = graphics.MeasureString(Y, font);

                    PointF PointX = new PointF(ParentConveyor.Rectangle.Right, ParentConveyor.Rectangle.Y +
                        (ParentConveyor.Rectangle.Height / 2 - sizeX.Height / 2));
                    PointF PointY = new PointF(PointX.X, PointX.Y + sizeX.Height);

                    graphics.DrawString(X, font, infobrush, PointX);
                    graphics.DrawString(Y, font, infobrush, PointY);
                }
            }
        }
        public void OnMouseDown(MouseEventArgs e)
        {
            if (Layout != null)
            {
                Point scaledPoint = new Point((int)((e.X - Layout.drawingPanel.AutoScrollPosition.X)),
                    (int)((e.Y - Layout.drawingPanel.AutoScrollPosition.Y)));

                if (Rectangle.Contains(scaledPoint))
                {
                    Info = true;
                    Layout.drawingPanel.Invalidate();
                }
                else
                {
                    Info = false;
                    Layout.drawingPanel.Invalidate();
                }
            }
        }
        public void OnMouseMove(MouseEventArgs e)
        {
            
        }
        public void OnMouseUp(MouseEventArgs e)
        {
            
        }





        private void ConstrainMovementWithinParent()
        {
            // Constrain DepoAlani within its parent bounds
            if (Rectangle.Left < ParentConveyor.Rectangle.Left)
            {
                MoveRectangleExact(ParentConveyor.Rectangle.Left, Rectangle.Y);
            }
            if (Rectangle.Top < ParentConveyor.Rectangle.Top)
            {
                MoveRectangleExact(Rectangle.X, ParentConveyor.Rectangle.Top);
            }
            if (Rectangle.Right > ParentConveyor.Rectangle.Right)
            {
                MoveRectangleExact(ParentConveyor.Rectangle.Right - Rectangle.Width, Rectangle.Y);
            }
            if (Rectangle.Bottom > ParentConveyor.Rectangle.Bottom)
            {
                MoveRectangleExact(Rectangle.X, ParentConveyor.Rectangle.Bottom - Rectangle.Height);
            }
        }
        public bool ContainsRectangle(RectangleF rectangle)
        {
            return Rectangle.Contains(rectangle);
        }
        public bool Contains(PointF point)
        {
            return Rectangle.Contains(point);
        }
        public void MoveRectangle(float deltaX, float deltaY)
        {
            Rectangle = new RectangleF(Rectangle.X + deltaX, Rectangle.Y + deltaY, Rectangle.Width, Rectangle.Height);
            OriginalRectangle = Rectangle;
        }
        public void MoveRectangleExact(float deltaX, float deltaY)
        {
            Rectangle = new RectangleF(deltaX, deltaY, Rectangle.Width, Rectangle.Height);
            OriginalRectangle = Rectangle;
        }
        public void ChangeRectangleSize(float width, float height)
        {
            Rectangle = new RectangleF(Rectangle.X, Rectangle.Y, width, height);
        }
        public void ChangeRectangleLocatioonandSize(float x, float y, float width, float height)
        {
            Rectangle = new RectangleF(x, y, width, height);
        }
        public void ChangeOriginalRectangleLocation(float x, float y)
        {
            OriginalRectangle = new RectangleF(x, y, OriginalRectangle.Width, OriginalRectangle.Height);
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
    }
}
