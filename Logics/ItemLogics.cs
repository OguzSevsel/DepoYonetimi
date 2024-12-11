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
        public RectangleF SelectLayoutRectangle { get; set; }
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
        [NotMapped]
        public Models.Cell Parent { get; set; }
        [NotMapped]
        public Models.Conveyor? ParentConveyor { get; set; }


        public Item(float x, float y, float width, float height, float zoomlevel, MainForm main)
        {
            Rectangle = new RectangleF(x, y, width, height);
            OriginalRectangle = new RectangleF(x, y, width,height);
            SelectLayoutRectangle = new RectangleF(x, y, width, height);

            Main = main;
            Zoomlevel = zoomlevel;
            LocationofRect = new Point((int)Rectangle.X, (int)Rectangle.Y);
            ReferencePoints = new List<ItemReferencePoint>();
        }
        public Item Clone()
        {
            return new Item
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
                LocationofRect = this.LocationofRect,
                drawingPanelMoveConst = this.drawingPanelMoveConst,
                Main = Main,
                Zoomlevel = Zoomlevel,
                ReferencePoints = this.ReferencePoints,
                Pen = this.Pen,
                CellId = this.CellId,
                ItemEtiketi = this.ItemEtiketi,
                ItemTuru = this.ItemTuru,
                ItemEni = this.ItemEni,
                ItemBoyu = this.ItemBoyu,
                ItemYuksekligi = this.ItemYuksekligi,
                ItemAgirligi = this.ItemAgirligi,
                ItemAciklamasi = this.ItemAciklamasi,
                Cm_X_Axis = this.Cm_X_Axis,
                Cm_Y_Axis = this.Cm_Y_Axis,
                Cm_Z_Axis = this.Cm_Z_Axis,

                // Clone other properties...
            };
        }

        public void Draw(Graphics graphics, Pen pen)
        {
            graphics.DrawRectangle(pen, Rectangle);
            //graphics.FillRectangle(new SolidBrush(Color.AliceBlue), Rectangle);
            System.Drawing.Font font = new System.Drawing.Font("Arial", 12 * Zoomlevel);
            SolidBrush brush = new SolidBrush(System.Drawing.Color.Red);
            string text = $"{Rectangle.X}";

            graphics.DrawString(text, font, brush, new PointF(Rectangle.X + Rectangle.Width, Rectangle.Y + (Rectangle.Height / 2)));

            //sizeofQmark = graphics.MeasureString("?", font);
            //graphics.DrawString("?", font, brush, new PointF(Rectangle.Location.X + (Rectangle.Width / 2 - sizeofQmark.Width / 2), Rectangle.Location.Y + (Rectangle.Height / 2 - sizeofQmark.Height / 2)));
            foreach (var refpoint in ItemReferencePoints)
            {
                refpoint.Draw(graphics);
            }
            //DrawTag(graphics, CellEtiketi, Rectangle, font, brush);
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
