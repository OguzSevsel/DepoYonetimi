using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Balya_Yerleştirme.Utilities.Utils;

namespace Balya_Yerleştirme.Models
{
    public partial class Item
    {
        [NotMapped]
        public RectangleF Rectangle { get; set; }
        [NotMapped]
        public RectangleF OriginalRectangle { get; set; }
        [NotMapped]
        public Pen Pen { get; set; } = new Pen(System.Drawing.Color.Blue,2);
        [NotMapped]
        public Point LocationofRect { get; set; }
        [NotMapped]
        public int drawingPanelMoveConst = 368 / 2;
        [NotMapped]
        MainForm Main { get; set; }
        [NotMapped]
        List<ItemReferencePoint> ReferencePoints { get; set; }
        

        public Item(float x, float y, float width, float height, float zoomlevel, MainForm main)
        {
            Rectangle = new RectangleF(x, y, width, height);
            OriginalRectangle = new RectangleF(x, y, width,height);
            Main = main;
            Zoomlevel = zoomlevel;
            LocationofRect = new Point((int)Rectangle.X, (int)Rectangle.Y);
            ReferencePoints = new List<ItemReferencePoint>();

            Main.ItemPlacementCancel += Item_ItemPlacementCancelEventHandler;
            Main.ItemPlacementToolStripButtonClicked += Item_ItemPlacementToolStripButtonClicked;
            Main.ItemPlacementContextMenuStripButtonClicked += Item_ItemPlacementContextMenuStripButtonClicked;
            Main.ExportToExcel += Item_ExportToExcelButtonClicked;
            Main.AddItemReferencePoint += Item_AddItemReferencePoint;
            Main.PLCBaglantisiniAyarlaButtonClicked += Item_PlcConnectionButton;
            Main.PLCBaglantisiPaneliniKapat += Item_MoveRight_Event;
            Main.ToolStripNesneYerlestirClicked += Item_MoveLeft_Event;
            Main.MoveRightEvent += Item_MoveRight_Event;
            Main.MoveLeftEvent += Item_MoveLeft_Event;
        }

        private void Item_MoveLeft_Event(object? sender, EventArgs e)
        {
            MoveLeft();
        }
        private void Item_MoveRight_Event(object? sender, EventArgs e)
        {
            MoveRight();
        }



        private void Item_PlcConnectionButton(object? sender, EventArgs e)
        {
            MoveLeft();
        }

        private void Item_AddItemReferencePoint(object? sender, EventArgs e)
        {
            MoveRight();
        }
        private void Item_ExportToExcelButtonClicked(object? sender, EventArgs e)
        {
            MoveRight();
        }
        private void Item_ItemPlacementContextMenuStripButtonClicked(object? sender, EventArgs e)
        {
            MoveLeft();
        }
        private void Item_ItemObtainToolStripButtonClicked(object? sender, EventArgs e)
        {
            MoveRight();
        }
        private void Item_ItemPlacementToolStripButtonClicked(object? sender, EventArgs e)
        {
            MoveLeft();
        }
        private void Item_ItemPlacementCancelEventHandler(object? sender, EventArgs e)
        {
            MoveRight();
        }
        private void Item_ItemPlacementCloseEventHandler(object? sender, EventArgs e)
        {
            MoveRight();
        }
        private void Item_ItemPlacementEventHandler(object? sender, EventArgs e)
        {
            MoveRight();
        }







        public void Draw(Graphics graphics, Pen pen)
        {
            graphics.DrawRectangle(pen, Rectangle);
            //graphics.FillRectangle(new SolidBrush(Color.AliceBlue), Rectangle);
            System.Drawing.Font font = new System.Drawing.Font("Arial", 6 * Zoomlevel);
            SolidBrush brush = new SolidBrush(System.Drawing.Color.Red);
            //sizeofQmark = graphics.MeasureString("?", font);
            //graphics.DrawString("?", font, brush, new PointF(Rectangle.Location.X + (Rectangle.Width / 2 - sizeofQmark.Width / 2), Rectangle.Location.Y + (Rectangle.Height / 2 - sizeofQmark.Height / 2)));
            foreach (var refpoint in ItemReferencePoints)
            {
                refpoint.Draw(graphics);
            }
            //DrawTag(graphics, CellEtiketi, Rectangle, font, brush);
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
            foreach (var refpoint in ItemReferencePoints)
            {
                refpoint.ApplyZoom(zoomlevel);
            }
        }
    }
}
