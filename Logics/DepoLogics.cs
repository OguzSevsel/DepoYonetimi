using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Balya_Yerleştirme.Utilities.Utils;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DocumentFormat.OpenXml.Spreadsheet;
using GUI_Library;
using String_Library;
using DocumentFormat.OpenXml.Vml.Office;
using System.Diagnostics;

namespace Balya_Yerleştirme.Models
{
    public partial class Depo
    {
        [NotMapped]
        public List<Cell> gridmaps { get; set; }
        [NotMapped]
        public SizeF OriginalDepoSize { get; set; }
        [NotMapped]
        public RectangleF Rectangle {  get; set; }
        [NotMapped]
        public RectangleF OriginalRectangle { get; set; }
        [NotMapped]
        public RectangleF SelectLayoutRectangle { get; set; }
        [NotMapped]
        MainForm Main { get; set; }
        [NotMapped]
        public Point LocationofRect { get; set; }
        [NotMapped]
        public int drawingPanelMoveConst = 368 / 2;
        [NotMapped]
        public bool DrawMeters { get; set; } = false;
        [NotMapped]
        public LayoutOlusturma ?layout { get; set; }
        [NotMapped]
        public bool isDragging { get; set; } = false;
        [NotMapped]
        public Point DragStartPoint = new Point();
        //[NotMapped]
        //public RectangleF holderRectangle = new RectangleF();
        [NotMapped]
        public Ambar Parent { get; set; }
        [NotMapped]
        public float depo_alani_x { get; set; }
        [NotMapped]
        public float depo_alani_y { get; set; }
        [NotMapped]
        public string itemDrop_Stage1 { get; set; } = string.Empty;
        [NotMapped]
        public string itemDrop_Stage2 { get; set; } = string.Empty;
        [NotMapped]
        public float nesneEni { get; set; }
        [NotMapped]
        public float nesneBoyu { get; set; }
        [NotMapped]
        public int nesneYuksekligi { get; set; }
        [NotMapped]
        public float Cm_Width { get; set; }
        [NotMapped]
        public float Cm_Height { get; set; }


        public Depo(float x, float y, float width, float height, float zoomlevel, MainForm ?main, LayoutOlusturma ?Layout, Ambar ambar) 
        { 
            Rectangle = new RectangleF(x, y, width, height);
            OriginalRectangle = new RectangleF(x, y, width, height);
            SelectLayoutRectangle = new RectangleF(x, y, width, height);

            LocationofRect = new Point((int)Rectangle.X, (int)Rectangle.Y);
            Zoomlevel = zoomlevel;
            gridmaps = new List<Cell>();
            Parent = ambar;
            Main = main;
            layout = Layout;
            depo_alani_x = Rectangle.X;
            depo_alani_y = Rectangle.Y;

            if (layout != null)
            {
                layout.izgaraHaritasiOlustur += Depo_IzgaraHaritasiOlustur;
            }
            
        }
        public Depo Clone()
        {
            return new Depo
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
                AmbarId = this.AmbarId,
                Ambar = this.Ambar,
                DepoName = this.DepoName,
                DepoDescription = this.DepoDescription,
                DepoAlaniEni = this.DepoAlaniEni,
                DepoAlaniBoyu = this.DepoAlaniBoyu,
                DepoAlaniYuksekligi = this.DepoAlaniYuksekligi,
                OriginalDepoSizeWidth = this.OriginalDepoSizeWidth,
                OriginalDepoSizeHeight = this.OriginalDepoSizeHeight,
                itemDrop_StartLocation = this.itemDrop_StartLocation,
                itemDrop_UpDown = this.itemDrop_UpDown,
                itemDrop_LeftRight = this.itemDrop_LeftRight,
                asama1_Yuksekligi = this.asama1_Yuksekligi,
                asama2_Yuksekligi = this.asama2_Yuksekligi,
                Yerlestirilme_Sirasi = this.Yerlestirilme_Sirasi,
                Depo_Alani_Boyu_Cm = this.Depo_Alani_Boyu_Cm,
                Depo_Alani_Eni_Cm = this.Depo_Alani_Eni_Cm,
                ColumnCount = this.ColumnCount,
                RowCount = this.RowCount,
                currentColumn = this.currentColumn,
                currentRow = this.currentRow,
                ItemTuru = this.ItemTuru,
                asama1_ItemSayisi = this.asama1_ItemSayisi,
                asama2_ToplamItemSayisi = this.asama2_ToplamItemSayisi,
                currentStage = this.currentStage,
                Cm_Width = this.Cm_Width,
                Cm_Height = this.Cm_Height,
                depo_alani_x = this.depo_alani_x,
                depo_alani_y = this.depo_alani_y,
                nesneEni = this.nesneEni,
                nesneBoyu = this.nesneBoyu,
                nesneYuksekligi = this.nesneYuksekligi,
                OriginalDepoSize = this.OriginalDepoSize,
                drawingPanelMoveConst = this.drawingPanelMoveConst,
                itemDrop_Stage1 = this.itemDrop_Stage1,
                itemDrop_Stage2 = this.itemDrop_Stage2,
                Main = this.Main,
                Parent = this.Parent,
                layout = this.layout,


                gridmaps = this.gridmaps.Select(d => d.Clone()).ToList(),

                // Clone other properties...
            };
        }

        public void Depo_IzgaraHaritasiOlustur(object? sender, EventArgs e)
        {
            if (layout != null)
            {
                if (gridmaps.Count == 0)
                {
                    float dikey_kenar_boslugu =
                    StrLib.ReplaceDotWithCommaReturnFloat(layout.txt_Dikey_Kenar_Boslugu,
                    layout.errorProvider, "Bu alan boş bırakılamaz.",
                    "Lütfen buraya bir sayı giriniz.", "Buraya 0 ya da daha küçük bir değer giremezsiniz.");

                    float yatay_kenar_boslugu =
                        StrLib.ReplaceDotWithCommaReturnFloat(layout.txt_Yatay_Kenar_Boslugu,
                        layout.errorProvider, "Bu alan boş bırakılamaz.",
                        "Lütfen buraya bir sayı giriniz.", "Buraya 0 ya da daha küçük bir değer giremezsiniz.");

                    CreateGridMapMenuItem(layout.total_Cell_Width,
                        layout.total_Cell_Height, dikey_kenar_boslugu, yatay_kenar_boslugu,
                        layout.nesne_Eni, layout.nesne_Boyu, layout.nesne_Yuksekligi);

                    nesneYuksekligi = layout.nesne_Yuksekligi;
                    nesneEni = layout.nesne_Eni;
                    nesneBoyu = layout.nesne_Boyu;
                }
            }
        }
       

        public void Draw(Graphics graphics)
        {
            using (Pen pen = new Pen(System.Drawing.Color.MidnightBlue, 3))
            {
                graphics.DrawRectangle(pen, Rectangle);
                //graphics.FillRectangle(new SolidBrush(System.Drawing.Color.AliceBlue), Rectangle);

                if (layout != null)
                {
                    string textDepoAdi = $"{DepoName}";

                    System.Drawing.Font font = new System.Drawing.Font("Arial", 8 * Zoomlevel);
                    SolidBrush brush = new SolidBrush(System.Drawing.Color.Red);

                    graphics.DrawString(textDepoAdi, font, brush, Rectangle.Location);
                }

                if (DrawMeters)
                {
                    PointF DepoBoyuTextLocation;
                    PointF UpLinePoint;
                    PointF DownLinePoint;
                    int padding = 4;

                    System.Drawing.Font font = new System.Drawing.Font("Arial", 8 * Zoomlevel);
                    SolidBrush brush = new SolidBrush(System.Drawing.Color.Red);

                    string text = $"{DepoAlaniEni}\nMetre";
                    string textDepoBoyu = $"{DepoAlaniBoyu}\nMetre";

                    SizeF textSize = graphics.MeasureString(text, font);
                    SizeF textDepoBoyuSize = graphics.MeasureString(textDepoBoyu, font);

                    PointF LeftLinePoint = new PointF(Rectangle.Location.X + Rectangle.Width / 8,
                        Rectangle.Location.Y - padding * Zoomlevel);
                    PointF RightLinePoint = new PointF(Rectangle.Location.X + Rectangle.Width - Rectangle.Width / 8,
                        Rectangle.Location.Y - padding * Zoomlevel);

                    PointF TextLocation =
                        new PointF(((LeftLinePoint.X - (Rectangle.Width / 8) + (Rectangle.Width / 2)) - textSize.Width / 2),
                        LeftLinePoint.Y - textSize.Height);

                    if (Main != null)
                    {
                        if (OriginalRectangle.Location.X < Main.DrawingPanel.Width / 2)
                        {
                            UpLinePoint = new PointF(Rectangle.Location.X - padding * Zoomlevel,
                            Rectangle.Location.Y + Rectangle.Height / 8);
                            DownLinePoint = new PointF(Rectangle.Location.X - padding * Zoomlevel,
                                Rectangle.Location.Y + Rectangle.Height - Rectangle.Height / 8);

                            DepoBoyuTextLocation = new PointF(Rectangle.Location.X - padding * Zoomlevel - textDepoBoyuSize.Width,
                            Rectangle.Location.Y + Rectangle.Height / 2 - textDepoBoyuSize.Height / 2);
                        }
                        else
                        {
                            UpLinePoint = new PointF(Rectangle.Location.X + Rectangle.Width + padding * Zoomlevel,
                            Rectangle.Location.Y + Rectangle.Height / 8);

                            DownLinePoint = new PointF(Rectangle.Location.X + Rectangle.Width + padding * Zoomlevel,
                                Rectangle.Location.Y + Rectangle.Height - Rectangle.Height / 8);

                            DepoBoyuTextLocation =
                           new PointF(Rectangle.Location.X + Rectangle.Width + padding * Zoomlevel,
                          Rectangle.Location.Y + Rectangle.Height / 2 - textDepoBoyuSize.Height / 2);
                        }

                        graphics.DrawLine(new Pen(System.Drawing.Color.Red), LeftLinePoint, RightLinePoint);
                        graphics.DrawLine(new Pen(System.Drawing.Color.Red), UpLinePoint, DownLinePoint);
                        graphics.DrawString(text, font, brush, TextLocation);
                        graphics.DrawString(textDepoBoyu, font, brush, DepoBoyuTextLocation);
                    }
                }
            }
            foreach (var cell in gridmaps)
            {
                cell.Draw(graphics);
            }
        }
        public void OnMouseDown(MouseEventArgs e)
        {
            if (layout != null)
            {
                Point scaledPoint = 
                    new Point((int)((e.X - layout.drawingPanel.AutoScrollPosition.X)),
                    (int)((e.Y - layout.drawingPanel.AutoScrollPosition.Y)));

                if (e.Button == MouseButtons.Right)
                {
                    if (Rectangle.Contains(scaledPoint))
                    {
                        if (!layout.Manuel_Move)
                        {
                            layout.txt_Width.Text = $"{DepoAlaniEni}";
                            layout.txt_Height.Text = $"{DepoAlaniBoyu}";
                        }
                        if (!layout.Fill_WareHouse)
                        {
                            layout.menuProcess = false;
                            layout.selectedDepo = this;

                            layout.SelectedDepoPen.Color = System.Drawing.Color.Blue;
                            layout.SelectedDepoPen.Width = 3;
                            layout.SelectedDepoEdgePen.Width = 3;
                            layout.colCount = ColumnCount;
                            layout.rowCount = RowCount;
                            layout.MenuStrip.Show(Cursor.Position);
                            if (layout.LeftSide_LayoutPanel.Visible)
                            {
                                layout.Show_DepoMenus("Depo");
                                layout.SortFlowLayoutPanel(layout.LayoutPanel_SelectedDepo);
                            }
                            layout.SelectNode(null, null, this);
                        }
                    }
                }
                else if (e.Button == MouseButtons.Left)
                {
                    if (Rectangle.Contains(scaledPoint))
                    {
                        if (!layout.MovingParameter && !layout.Fill_WareHouse && !layout.Manuel_Move)
                        {
                            layout.menuProcess = false;
                            layout.selectedDepo = this;
                            layout.isMoving = true;
                            layout.SelectedDepoPen.Width = 3;
                            layout.SelectedDepoEdgePen.Width = 3;
                            layout.SelectedDepoPen.Color = System.Drawing.Color.Blue;
                            isDragging = true;
                            DragStartPoint = e.Location;
                            if (layout.LeftSide_LayoutPanel.Visible)
                            {
                                layout.Show_DepoMenus("Depo");
                                layout.SortFlowLayoutPanel(layout.LayoutPanel_SelectedDepo);
                            }
                            layout.SelectNode(null, null, this);
                        }
                    }
                }
                layout.drawingPanel.Invalidate();
            }
            foreach (var cell in gridmaps)
            {
                cell.OnMouseDown(e);
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
                    float deltaX = (float)(e.X - DragStartPoint.X);
                    float deltaY = (float)(e.Y - DragStartPoint.Y);

                    DragStartPoint = e.Location;

                    //holderRectangle = GVisual.MoveRectangle(holderRectangle, deltaX, deltaY);

                    foreach (var depo in Parent.depolar)
                    {
                        if (Parent.depolar.Count > 1)
                        {
                            if (this != depo)
                            {
                                if (!Rectangle.IntersectsWith(depo.Rectangle))
                                {
                                    notCollidedwithDepo = true;
                                }
                            }
                        }
                        else
                        {
                            MoveRectangle(deltaX, deltaY);
                        }
                    }

                    if (Parent.conveyors.Count > 0)
                    {
                        foreach (var conveyor in Parent.conveyors)
                        {
                            if (!Rectangle.IntersectsWith(conveyor.Rectangle))
                            {
                                notCollidedwithConveyor = true;
                            }
                        }
                    }
                    else
                    {
                        notCollidedwithConveyor = true;
                    }

                    if (notCollidedwithConveyor == true && notCollidedwithDepo == true)
                    {
                        MoveRectangle(deltaX, deltaY);
                    }
                }
            }
            foreach (var cell in gridmaps)
            {
                cell.OnMouseMove(e);
            }
        }
        private void MoveRectangle(float deltaX, float deltaY)
        {
            Rectangle = GVisual.MoveRectangle(Rectangle, deltaX, deltaY);
            LocationofRect = new Point((int)Rectangle.X, (int)Rectangle.Y);
            depo_alani_x = Rectangle.X;
            depo_alani_y = Rectangle.Y;
            foreach (var cell in gridmaps)
            {
                cell.Rectangle = GVisual.MoveRectangle(cell.Rectangle, deltaX, deltaY);
                cell.OriginalRectangle = GVisual.MoveRectangle(cell.OriginalRectangle, deltaX, deltaY);
                cell.LocationofRect = new Point((int)cell.Rectangle.X, (int)cell.Rectangle.Y);
            }
            ConstrainMovementWithinParent();
            OriginalRectangle = Rectangle;
        }
        private void ConstrainMovementWithinParent()
        {
            if (Rectangle.Left <= Parent.Rectangle.Left)
            {
                Rectangle = 
                    GVisual.MoveRectangleToPoint(Rectangle, Parent.Rectangle.X, Rectangle.Y);
                LocationofRect = new Point((int)Rectangle.X, (int)Rectangle.Y);
                SyncGridMap();
            }
            if (Rectangle.Top <= Parent.Rectangle.Top)
            {
                Rectangle = 
                    GVisual.MoveRectangleToPoint(Rectangle, Rectangle.X, Parent.Rectangle.Y);
                LocationofRect = new Point((int)Rectangle.X, (int)Rectangle.Y);
                SyncGridMap();
            }
            if (Rectangle.Right >= Parent.Rectangle.Right)
            {
                Rectangle = 
                    GVisual.MoveRectangleToPoint(Rectangle, Parent.Rectangle.Right - Rectangle.Width, Rectangle.Y);
                LocationofRect = new Point((int)Rectangle.X, (int)Rectangle.Y);
                SyncGridMap();
            }
            if (Rectangle.Bottom >= Parent.Rectangle.Bottom)
            {
                Rectangle = 
                    GVisual.MoveRectangleToPoint(Rectangle, Rectangle.X, Parent.Rectangle.Bottom - Rectangle.Height);
                LocationofRect = new Point((int)Rectangle.X, (int)Rectangle.Y);
                SyncGridMap();
            }
            if (Rectangle.Left <= Parent.Rectangle.Left && Rectangle.Top <= Parent.Rectangle.Top)
            {
                Rectangle =
                    GVisual.MoveRectangleToPoint(Rectangle, Parent.Rectangle.X, Parent.Rectangle.Top);
                LocationofRect = new Point((int)Rectangle.X, (int)Rectangle.Y);
                SyncGridMap();
            }
        }
        private void SyncGridMap()
        {
            foreach (var cell in gridmaps)
            {
                cell.Rectangle =
                    GVisual.MoveRectangleToPoint(cell.Rectangle, (cell.Rectangle.X + Rectangle.X - depo_alani_x),
                    (cell.Rectangle.Y + Rectangle.Y - depo_alani_y));
                float x = cell.Rectangle.X;
                float y = cell.Rectangle.Y;
                cell.OriginalRectangle = GVisual.MoveRectangleToPoint(cell.OriginalRectangle, (float)x, (float)y);
                cell.LocationofRect = new Point((int)cell.Rectangle.X, (int)cell.Rectangle.Y);

                var Location = ConvertRectanglesLocationtoCMInsideParentRectangle(cell.OriginalRectangle,
                    Parent.OriginalRectangle, Parent.AmbarEni, Parent.AmbarBoyu, true);

                cell.cell_Cm_X = Location.Item1;
                cell.cell_Cm_Y = Location.Item2;
            }
            depo_alani_x = Rectangle.X;
            depo_alani_y = Rectangle.Y;
            
            OriginalRectangle = Rectangle;
        }
        public void OnMouseUp(MouseEventArgs e)
        {
            if (layout != null)
            {
                layout.isMoving = false;
                isDragging = false;
            }
        }
        public void OnMouseDoubleClick(MouseEventArgs e)
        {
            foreach (var cell in gridmaps)
            {
                cell.OnMouseDoubleClick(e);
            }
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
            foreach (var cell in gridmaps)
            {
                cell.ApplyZoom(zoomlevel);
            }
        }

        public void CreateGridMapMenuItem(float izgaraEni, float izgaraBoyu,
            float dikey_kenar_boslugu, float yatay_kenar_boslugu, float nesne_eni,
            float nesne_boyu, int nesne_yuksekligi)
        {
            float izgaraEniMeters = (float)izgaraEni / 100.0f;
            float izgaraBoyuMeters = (float)izgaraBoyu / 100.0f;

            float PXX = (float)OriginalDepoSize.Width / (float)DepoAlaniEni;
            float PXY = (float)OriginalDepoSize.Height / (float)DepoAlaniBoyu;

            float px = Math.Min(PXX, PXY);

            float izgaraEniPX = px * izgaraEniMeters;
            float izgaraBoyuPX = px * izgaraBoyuMeters;

            //float izgaraEniPX = px * izgaraEniMeters;
            //float izgaraBoyuPX = px * izgaraBoyuMeters;

            nesneEni = nesne_eni;
            nesneBoyu = nesne_boyu;
            nesneYuksekligi = nesne_yuksekligi;

            if (gridmaps.Count == 0)
            {
                CreateGrid(izgaraEniPX, izgaraBoyuPX, OriginalDepoSize.Width, 
                    OriginalDepoSize.Height, izgaraEni, izgaraBoyu, nesne_eni,
                    nesne_boyu, nesne_yuksekligi, dikey_kenar_boslugu, yatay_kenar_boslugu);
            } 
            else
            {
                gridmaps.Clear();
                CreateGrid(izgaraEniPX, izgaraBoyuPX, OriginalDepoSize.Width,
                    OriginalDepoSize.Height, izgaraEni, izgaraBoyu, nesne_eni,
                    nesne_boyu, nesne_yuksekligi, dikey_kenar_boslugu, yatay_kenar_boslugu);
            }
        }
        public void CreateGrid(float izgaraEniPx, float izgaraBoyuPx, float depoAlanininEni, 
            float depoAlanininBoyu, float izgaraEni, float izgaraBoyu, float nesne_eni,
            float nesne_boyu, int nesne_yuksekligi, float dikey_kenar_boslugu, float yatay_kenar_boslugu)
        {
            int counter = 0;
            int columnCount = (int)Math.Floor((float)OriginalDepoSize.Width / izgaraEniPx);
            int rowCount = (int)Math.Floor((float)OriginalDepoSize.Height / izgaraBoyuPx);
            ColumnCount = columnCount;
            RowCount = rowCount;

            for (int row = 0; row < rowCount; row++)
            {
                string rowLetter = GetRowLetter(row);
                for (int col = 0; col < columnCount; col++)
                {
                    float x = (float)((float)col * izgaraEniPx);
                    float y = (float)((float)row * izgaraBoyuPx);
                    int x1 = Convert.ToInt32(x);
                    int y1 = Convert.ToInt32(y);
                    int izgaraEni1 = Convert.ToInt32(izgaraEniPx);
                    int izgaraBoyu1 = Convert.ToInt32(izgaraBoyuPx);

                    float deponunyüksekliğicm = DepoAlaniYuksekligi;

                    Cell cell = new Cell(OriginalRectangle.X + x,
                        OriginalRectangle.Y + y, izgaraEniPx, izgaraBoyuPx, Main, this, layout);

                    cell.OriginalRectangle = new RectangleF(OriginalRectangle.X + x,
                        OriginalRectangle.Y + y, izgaraEniPx, izgaraBoyuPx);

                    cell.DepoId = DepoId;
                    cell.CellId = 0;

                    cell.LocationofRect = new Point((int)cell.Rectangle.X, (int)cell.Rectangle.Y);

                    cell.CellEni = izgaraEni;
                    cell.CellBoyu = izgaraBoyu;
                    cell.KareX = cell.Rectangle.X;
                    cell.KareY = cell.Rectangle.Y;
                    cell.KareEni = cell.Rectangle.Width;
                    cell.KareBoyu = cell.Rectangle.Height;
                    cell.OriginalKareX = cell.OriginalRectangle.X;
                    cell.OriginalKareY = cell.OriginalRectangle.Y;
                    cell.OriginalKareEni = cell.OriginalRectangle.Width;
                    cell.OriginalKareBoyu = cell.OriginalRectangle.Height;
                    cell.Zoomlevel = Zoomlevel;
                    cell.CellEni = izgaraEni;
                    cell.CellBoyu = izgaraBoyu;

                    if (layout != null)
                    {
                        cell.NesneEni = nesne_eni;
                        cell.NesneBoyu = nesne_boyu;
                        cell.NesneYuksekligi = nesne_yuksekligi;

                        if (cell.NesneYuksekligi * 2 < DepoAlaniYuksekligi)
                        {
                            asama1_Yuksekligi = cell.NesneYuksekligi * 2;
                            asama2_Yuksekligi = DepoAlaniYuksekligi;
                            asama1_ItemSayisi = 2;
                            asama2_ToplamItemSayisi = DepoAlaniYuksekligi / cell.NesneYuksekligi;
                            currentStage = 1;
                        }
                        else if (cell.NesneYuksekligi <= (DepoAlaniYuksekligi) / 2)
                        {
                            asama1_Yuksekligi = cell.NesneYuksekligi;
                            asama2_Yuksekligi = DepoAlaniYuksekligi;
                            currentStage = 1;
                        }
                        else if (cell.NesneYuksekligi >= (DepoAlaniYuksekligi) / 2)
                        {
                            asama1_Yuksekligi = cell.NesneYuksekligi;
                            asama2_Yuksekligi = DepoAlaniYuksekligi;
                            asama1_ItemSayisi = 1;
                            asama2_ToplamItemSayisi = DepoAlaniYuksekligi / cell.NesneYuksekligi;
                            currentStage = 1;
                        }
                        else if (cell.NesneYuksekligi == DepoAlaniYuksekligi)
                        {
                            asama1_Yuksekligi = cell.NesneYuksekligi;
                            asama2_Yuksekligi = cell.NesneYuksekligi;
                            asama1_ItemSayisi = 1;
                            asama2_ToplamItemSayisi = 1;
                            currentStage = 1;
                        }
                        else if (cell.NesneYuksekligi > DepoAlaniYuksekligi)
                        {
                            asama1_Yuksekligi = 0;
                            asama2_Yuksekligi = 0;
                            asama1_ItemSayisi = 0;
                            asama2_ToplamItemSayisi = 0;
                            currentStage = 1;
                        }
                    }

                    var Location = ConvertRectanglesLocationtoCMInsideParentRectangle(cell.Rectangle,
                   Parent.OriginalRectangle, Parent.AmbarEni, Parent.AmbarBoyu, true);

                    cell.cell_Cm_X = Location.Item1;
                    cell.cell_Cm_Y = Location.Item2;

                    cell.DikeyKenarBoslugu = dikey_kenar_boslugu;
                    cell.YatayKenarBoslugu = yatay_kenar_boslugu;
                    cell.CellYuksekligi = DepoAlaniYuksekligi;
                    cell.CellMalSayisi = 0;
                    cell.ItemSayisi = 0;
                    cell.toplam_Nesne_Yuksekligi = 0;
                    cell.Row = row + 1;
                    cell.Column = col + 1;
                    cell.LocationofRect = new Point((int)cell.Rectangle.X, (int)cell.Rectangle.Y);
                    string tag = rowLetter + (col + 1).ToString();
                    cell.CellEtiketi = tag;
                    gridmaps.Add(cell);
                    counter++;
                }
            }

            foreach (var cell1 in gridmaps)
            {
                float kenarBosluguX = OriginalRectangle.Width - (columnCount * cell1.OriginalRectangle.Width);
                float kenarBosluguY = OriginalRectangle.Height - (rowCount * cell1.OriginalRectangle.Height);

                cell1.MoveRectangle(kenarBosluguX / 2, kenarBosluguY / 2);
                cell1.ChangeOriginalRectangleLocation(kenarBosluguX / 2, kenarBosluguY / 2);

                cell1.KareX = cell1.Rectangle.X;
                cell1.KareY = cell1.Rectangle.Y;
                cell1.OriginalKareX = cell1.OriginalRectangle.X;
                cell1.OriginalKareY = cell1.OriginalRectangle.Y;

                //Cell grid = new Cell(DepoId, cell1.CellEtiketi, cell1.CellEni, cell1.CellBoyu,
                //cell1.CellYuksekligi, cell1.CellMalSayisi, cell1.KareX, cell1.KareY, cell1.KareEni,
                //cell1.KareBoyu, cell1.OriginalKareX, cell1.OriginalKareY, cell1.OriginalKareEni,
                //cell1.OriginalKareBoyu, cell1.Zoomlevel, cell1.ItemSayisi);

                //context.Cells.Add(grid);
                //context.SaveChanges();
                //cell1.CellId = grid.CellId;
            }
        }
        private string GetRowLetter(int rowIndex)
        {
            return ((char)('A' + rowIndex)).ToString();
        }
        public RectangleF MoveRectangleToPoint(RectangleF rectangle, float RectX, float RectY)
        {
            rectangle = new RectangleF(RectX, RectY, rectangle.Width, rectangle.Height);
            return rectangle;
        }
        public RectangleF SnapRectangles(RectangleF rectangle, RectangleF refRectangle)
        {
            Point rectangleRightMiddle = GVisual.GetMiddleOfRightEdge(rectangle);
            Point rectangleLeftMiddle = GVisual.GetMiddleOfLeftEdge(rectangle);
            Point rectangleTopMiddle = GVisual.GetMiddleOfTopEdge(rectangle);
            Point rectangleBotMiddle = GVisual.GetMiddleOfBottomEdge(rectangle);
            Point rectangleTopLeftCorner = GVisual.GetTopLeftCorner(rectangle);
            Point rectangleTopRightCorner = GVisual.GetTopRightCorner(rectangle);
            Point rectangleBotLeftCorner = GVisual.GetBottomLeftCorner(rectangle);
            Point rectangleBotRightCorner = GVisual.GetBottomRightCorner(rectangle);


            Point refRectangleRightMiddle = GVisual.GetMiddleOfRightEdge(refRectangle);
            Point refRectangleLeftMiddle = GVisual.GetMiddleOfLeftEdge(refRectangle);
            Point refRectangleTopMiddle = GVisual.GetMiddleOfTopEdge(refRectangle);
            Point refRectangleBotMiddle = GVisual.GetMiddleOfBottomEdge(refRectangle);
            Point refRectangleTopLeftCorner = GVisual.GetTopLeftCorner(refRectangle);
            Point refRectangleTopRightCorner = GVisual.GetTopRightCorner(refRectangle);
            Point refRectangleBotLeftCorner = GVisual.GetBottomLeftCorner(refRectangle);
            Point refRectangleBotRightCorner = GVisual.GetBottomRightCorner(refRectangle);

            int snapping = 10;

            double TopLeftCornerSnapDistance = GVisual.CalculateDistance(rectangleBotLeftCorner.X,
                rectangleBotLeftCorner.Y, refRectangleTopLeftCorner.X,
                refRectangleTopLeftCorner.Y);
            double LeftSideSnapDistance = GVisual.CalculateDistance(rectangleRightMiddle.X,
                rectangleRightMiddle.Y, refRectangleLeftMiddle.X, refRectangleLeftMiddle.Y);
            double BotLeftCornerSnapDistance = GVisual.CalculateDistance(rectangleTopLeftCorner.X,
                rectangleTopLeftCorner.Y, refRectangleBotLeftCorner.X,
                refRectangleBotLeftCorner.Y);


            double TopRightCornerSnapDistance = GVisual.CalculateDistance(rectangleBotRightCorner.X,
                rectangleBotRightCorner.Y, refRectangleTopRightCorner.X, refRectangleTopRightCorner.Y);
            double RightSideSnapDistance = GVisual.CalculateDistance(rectangleLeftMiddle.X,
                rectangleLeftMiddle.Y, refRectangleRightMiddle.X, refRectangleRightMiddle.Y);
            double BotRightCornerSnapDistance = GVisual.CalculateDistance(rectangleTopRightCorner.X,
                rectangleTopRightCorner.Y, refRectangleBotRightCorner.X, refRectangleBotRightCorner.Y);


            double TopSideSnapDistance = GVisual.CalculateDistance(rectangleBotMiddle.X,
                rectangleBotMiddle.Y, refRectangleTopMiddle.X, refRectangleTopMiddle.Y);
            double BotSideSnapDistance = GVisual.CalculateDistance(rectangleTopMiddle.X,
                rectangleTopMiddle.Y, refRectangleBotMiddle.X, refRectangleBotMiddle.Y);

            double RightBotSnapDistance = GVisual.CalculateDistance(rectangleBotLeftCorner.X,
                rectangleBotLeftCorner.Y, refRectangleBotRightCorner.X, refRectangleBotRightCorner.Y);
            double RightTopSnapDistance = GVisual.CalculateDistance(rectangleTopLeftCorner.X,
                rectangleTopLeftCorner.Y, refRectangleTopRightCorner.X, refRectangleTopRightCorner.Y);

            double LeftBotSnapDistance = GVisual.CalculateDistance(rectangleBotRightCorner.X,
                rectangleBotRightCorner.Y, refRectangleBotLeftCorner.X, refRectangleBotLeftCorner.Y);
            double LeftTopSnapDistance = GVisual.CalculateDistance(rectangleTopRightCorner.X,
                rectangleTopRightCorner.Y, refRectangleTopLeftCorner.X, refRectangleTopLeftCorner.Y);

            if (TopSideSnapDistance < snapping)
            {
                rectangle = GVisual.MoveRectangleToPoint(rectangle,
               refRectangle.X + (refRectangle.Width / 2 - rectangle.Width / 2),
               refRectangle.Y - rectangle.Height);
                return rectangle;
            }
            else if (BotSideSnapDistance < snapping)
            {
                rectangle = MoveRectangleToPoint(rectangle,
               refRectangle.X + (refRectangle.Width / 2 - rectangle.Width / 2),
               refRectangle.Bottom);
                return rectangle;
            }
            else if (LeftSideSnapDistance < snapping)
            {
                rectangle = GVisual.MoveRectangleToPoint(rectangle,
               refRectangle.X - rectangle.Width,
               refRectangle.Y + (refRectangle.Height / 2 - rectangle.Height / 2));
                return rectangle;
            }
            else if (RightSideSnapDistance < snapping)
            {
                rectangle = GVisual.MoveRectangleToPoint(rectangle,
               refRectangle.X + refRectangle.Width,
               refRectangle.Y + (refRectangle.Height / 2 - rectangle.Height / 2));
                return rectangle;
            }
            else if (TopRightCornerSnapDistance < snapping)
            {
                rectangle = GVisual.MoveRectangleToPoint(rectangle,
               refRectangle.X + refRectangle.Width - rectangle.Width,
               refRectangle.Y - rectangle.Height);
                return rectangle;
            }
            else if (BotRightCornerSnapDistance < snapping)
            {
                rectangle = GVisual.MoveRectangleToPoint(rectangle,
               refRectangle.X + refRectangle.Width - rectangle.Width,
               refRectangle.Y + refRectangle.Height);
                return rectangle;
            }
            else if (TopLeftCornerSnapDistance < snapping)
            {
                rectangle = GVisual.MoveRectangleToPoint(rectangle,
               refRectangle.X,
               refRectangle.Y - rectangle.Height);
                return rectangle;
            }
            else if (BotLeftCornerSnapDistance < snapping)
            {
                rectangle = GVisual.MoveRectangleToPoint(rectangle,
               refRectangle.X,
               refRectangle.Y + refRectangle.Height);
                return rectangle;
            }
            else if (RightBotSnapDistance < snapping)
            {
                rectangle = GVisual.MoveRectangleToPoint(rectangle,
               refRectangle.X + refRectangle.Width,
               refRectangle.Y + refRectangle.Height - rectangle.Height);
                return rectangle;
            }
            else if (RightTopSnapDistance < snapping)
            {
                rectangle = GVisual.MoveRectangleToPoint(rectangle,
               refRectangle.X + refRectangle.Width,
               refRectangle.Y);
                return rectangle;
            }
            else if (LeftTopSnapDistance < snapping)
            {
                rectangle = GVisual.MoveRectangleToPoint(rectangle,
               refRectangle.X - rectangle.Width,
               refRectangle.Y);
                return rectangle;
            }
            else if (LeftBotSnapDistance < snapping)
            {
                rectangle = GVisual.MoveRectangleToPoint(rectangle,
               refRectangle.X - rectangle.Width,
               refRectangle.Y + refRectangle.Height - rectangle.Height);
                return rectangle;
            }
            else
            {
                if (LeftSideSnapDistance < RightSideSnapDistance &&
                   LeftSideSnapDistance < TopSideSnapDistance &&
                   LeftSideSnapDistance < BotSideSnapDistance)
                {
                    rectangle = GVisual.MoveRectangleToPoint(rectangle,
                       refRectangle.X - rectangle.Width,
                       rectangle.Y);
                }
                else if (RightSideSnapDistance < LeftSideSnapDistance &&
                   RightSideSnapDistance < TopSideSnapDistance &&
                   RightSideSnapDistance < BotSideSnapDistance)
                {
                    rectangle = GVisual.MoveRectangleToPoint(rectangle,
                       refRectangle.X + refRectangle.Width,
                       rectangle.Y);
                }
                else if (TopSideSnapDistance < LeftSideSnapDistance &&
                   TopSideSnapDistance < RightSideSnapDistance &&
                   TopSideSnapDistance < BotSideSnapDistance)
                {
                    rectangle = GVisual.MoveRectangleToPoint(rectangle,
                       rectangle.X,
                       refRectangle.Y - rectangle.Height);
                }
                else if (BotSideSnapDistance < LeftSideSnapDistance &&
                   BotSideSnapDistance < RightSideSnapDistance &&
                   BotSideSnapDistance < TopSideSnapDistance)
                {
                    rectangle = GVisual.MoveRectangleToPoint(rectangle,
                       rectangle.X,
                       refRectangle.Y + refRectangle.Height);
                }
                else if (LeftSideSnapDistance < RightSideSnapDistance &&
                   LeftSideSnapDistance < TopSideSnapDistance &&
                   LeftSideSnapDistance < BotSideSnapDistance)
                {
                    rectangle = GVisual.MoveRectangleToPoint(rectangle,
                       refRectangle.X - rectangle.Width,
                       rectangle.Y);
                }
                return rectangle;
            }
        }
        
    }
}
