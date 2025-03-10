﻿using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Balya_Yerleştirme.Utilities.Utils;

namespace Balya_Yerleştirme.Models
{
    public partial class ItemReferencePoint
    {
        [NotMapped]
        public RectangleF Rectangle { get; set; }
        [NotMapped]
        public RectangleF OriginalRectangle { get; set; }
        [NotMapped]
        public Pen Pen { get; set; } = new Pen(System.Drawing.Color.Blue, 2);
        [NotMapped]
        public Point LocationofRect { get; set; }
        [NotMapped]
        public int drawingPanelMoveConst = 368 / 2;
        [NotMapped]
        MainForm Main { get; set; }

        public ItemReferencePoint(float x, float y, float width, float height, float zoomlevel, 
            MainForm main) 
        {
            Rectangle = new RectangleF(x, y, width, height);
            OriginalRectangle = new RectangleF(x, y, width, height);
            LocationofRect = new Point((int)Rectangle.X, (int)Rectangle.Y);
            Zoomlevel = zoomlevel;
            Main = main;
        }

        public void Draw(Graphics graphics)
        {
            System.Drawing.Font font = new System.Drawing.Font("Arial", 6);
            SolidBrush brush = new SolidBrush(System.Drawing.Color.Red);
            graphics.FillEllipse(brush, Rectangle);
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
    }
}
