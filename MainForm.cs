using Balya_Yerleştirme.Models;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Spreadsheet;
using GUI_Library;
using String_Library;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Drawing;
using static Balya_Yerleştirme.Utilities.Utils;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Office.CustomUI;
using Microsoft.EntityFrameworkCore;
using DocumentFormat.OpenXml.CustomProperties;
using S7.Net;
using System.ComponentModel.Design;
using DocumentFormat.OpenXml.Bibliography;
using System.Drawing.Drawing2D;
using DocumentFormat.OpenXml.Office2010.CustomUI;
using DocumentFormat.OpenXml.Math;
using CustomNotification;
using System.Runtime.CompilerServices;



namespace Balya_Yerleştirme
{
    public partial class MainForm : Form
    {
        public Ambar? ambar { get; set; }
        public Depo selectedDepo { get; set; }
        public bool Move { get; set; } = false;
        public float opcount { get; set; } = 0;


        #region PLC Operations
        public Plc PLC { get; set; }
        public int PLCCounter { get; set; } = 0;
        public bool ShowNesneButtons { get; set; } = false;
        public bool NesneKaldır { get; set; } = false;

        #endregion


        #region Custom Events
        //Events
        public event EventHandler? ItemPlacementToolStripButtonClicked;
        public event EventHandler? ItemPlacementContextMenuStripButtonClicked;
        public event EventHandler? ItemPlacementCancel;
        public event EventHandler? ExportToExcel;
        public event EventHandler? AddItemReferencePoint;
        public event EventHandler? PLCBaglantisiniAyarlaButtonClicked;
        public event EventHandler? PLCBaglantisiPaneliniKapat;
        public event EventHandler? ToolStripNesneYerlestirClicked;
        public event EventHandler? MoveRightEvent;
        public event EventHandler? MoveLeftEvent;
        #endregion


        #region User Input Variables for Adding and Removing Items
        //User Input Variables
        public string item_etiketi { get; set; } = string.Empty;
        public string item_aciklamasi { get; set; } = string.Empty;
        public string item_turu { get; set; } = string.Empty;
        public float item_agirligi { get; set; } = 0;
        #endregion


        #region Zooming, Scrolling Event Variables
        //Certain Event Parameters for zooming or scroll
        public int drawingPanelMoveConst = 368 / 2;
        public Balya_Yerleştirme.Models.Cell RightClickCell { get; set; }
        public Conveyor RightClickConveyor { get; set; }
        public Point DragStartPoint { get; set; }
        private PointF scrollStartPoint { get; set; }
        Point scaledPoint { get; set; }
        Point infoPoint { get; set; }
        public float mouseXRelativeToContent { get; set; }
        public float mouseYRelativeToContent { get; set; }
        public float Zoomlevel { get; set; } = 1f;

        private const float zoomStep = 1.1f;
        private const float maxZoom = 3.5f;
        private const float minZoom = 1f;
        #endregion


        #region Variables for PLC Simulations
        public bool yerBul { get; set; } = false;
        public Models.Cell? BlinkingCell { get; set; } = new Models.Cell();
        #endregion


        #region Locations for Panels
        public Point leftSidePanelLocation { get; set; } = new Point(12, 91);
        public Point rightSidePanelLocation { get; set; } = new Point(1528, 91);
        Point ProgressBarPoint = new Point(379, 12);
        Point ProgressBarPointLayoutOlustur = new Point(1423, 8);
        #endregion


        public MainForm()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            SetDoubleBuffered(this);
            SetDoubleBuffered(DrawingPanel);
            SetDoubleBuffered(infopanel);
            Hide_infopanel();
            HideEverything();
            LoadFromDB();
            DrawingPanel.Invalidate();
            DrawingPanel.MouseWheel += DrawingPanel_MouseWheel;
            btn_PLC_ConnectionPanel_Kapat_Click(this, EventArgs.Empty);
        }



        //Add Items to the Depos According to the Parameters choosed by user when creating Layout
        #region Item Adding Algoritm
        public void AddItemtoCell(Models.Cell? cell1, string item_etiketi, string item_aciklamasi,
            float item_agirligi)
        {
            if (cell1 != null)
            {
                Models.Item newItem = new Models.Item(0, 0, cell1.NesneEni, cell1.NesneBoyu, Zoomlevel, this);

                newItem.OriginalRectangle = GVisual.RatioRectangleToParentRectangle(cell1.NesneEni, cell1.NesneBoyu, cell1.CellEni, cell1.CellBoyu, cell1.OriginalRectangle);

                newItem.Rectangle = GVisual.RatioRectangleToParentRectangle(cell1.NesneEni, cell1.NesneBoyu, cell1.CellEni, cell1.CellBoyu, cell1.Rectangle);

                newItem.ItemId = 0;
                newItem.CellId = cell1.CellId;
                newItem.ItemAgirligi = item_agirligi;
                newItem.ItemEtiketi = item_etiketi;
                newItem.ItemAciklamasi = item_aciklamasi;
                newItem.ItemEni = cell1.NesneEni;
                newItem.ItemBoyu = cell1.NesneBoyu;
                newItem.KareX = newItem.Rectangle.X;
                newItem.KareY = newItem.Rectangle.Y;
                newItem.KareEni = newItem.Rectangle.Width;
                newItem.KareBoyu = newItem.Rectangle.Height;
                newItem.OriginalKareX = newItem.OriginalRectangle.X;
                newItem.OriginalKareY = newItem.OriginalRectangle.Y;
                newItem.OriginalKareEni = newItem.OriginalRectangle.Width;
                newItem.OriginalKareBoyu = newItem.OriginalRectangle.Height;
                newItem.ItemYuksekligi = cell1.NesneYuksekligi;
                newItem.ItemTuru = "Nesne";


                var Location = ConvertRectanglesLocationtoCMInsideParentRectangle(newItem.Rectangle,
                    ambar.Rectangle, ambar.AmbarEni, ambar.AmbarBoyu, true);

                newItem.Cm_X_Axis = Location.Item1;
                newItem.Cm_Y_Axis = Location.Item2;

                cell1.items.Add(newItem);

                foreach (var item in cell1.items)
                {
                    int first = cell1.items.IndexOf(item);
                    int index = first + 1;

                    item.Cm_Z_Axis = index * item.ItemYuksekligi;

                    if (item == newItem)
                    {
                        newItem.Cm_Z_Axis = item.Cm_Z_Axis;
                    }
                }

                cell1.toplam_Nesne_Yuksekligi = cell1.NesneYuksekligi * cell1.items.Count;


                if (newItem.ItemId == 0)
                {
                    Models.Item Dbitem = new Models.Item(cell1.CellId, newItem.ItemEtiketi,
                        newItem.ItemTuru, newItem.ItemEni, newItem.ItemBoyu, newItem.ItemYuksekligi,
                        newItem.Rectangle.X, newItem.Rectangle.Y, newItem.Rectangle.Width, newItem.Rectangle.Height,
                        newItem.Rectangle.X, newItem.Rectangle.Y, newItem.OriginalRectangle.Width,
                        newItem.OriginalRectangle.Height, newItem.Zoomlevel, newItem.ItemAgirligi, newItem.ItemAciklamasi,
                        newItem.Cm_X_Axis, newItem.Cm_Y_Axis, newItem.Cm_Z_Axis);

                    using (var context = new DBContext())
                    {
                        context.Items.Add(Dbitem);
                        context.SaveChanges();
                        newItem.ItemId = Dbitem.ItemId;
                    }
                }
                else
                {
                    using (var context = new DBContext())
                    {
                        var item1 = (from x in context.Items
                                     where x.ItemId == newItem.ItemId
                                     select x).FirstOrDefault();

                        if (item1 != null)
                        {
                            Models.Item Dbitem = new Models.Item(cell1.CellId, newItem.ItemEtiketi,
                               newItem.ItemTuru, newItem.ItemEni, newItem.ItemBoyu, newItem.ItemYuksekligi,
                               newItem.Rectangle.X, newItem.Rectangle.Y, newItem.KareEni, newItem.KareBoyu,
                               newItem.Rectangle.X, newItem.Rectangle.Y, newItem.OriginalRectangle.Width,
                               newItem.OriginalRectangle.Height, newItem.Zoomlevel, newItem.ItemAgirligi, newItem.ItemAciklamasi,
                               newItem.Cm_X_Axis, newItem.Cm_Y_Axis, newItem.Cm_Z_Axis);

                            item1.KareX = newItem.Rectangle.X;
                            item1.KareY = newItem.Rectangle.Y;
                            //item1.KareEni = item.KareEni;
                            //item1.KareBoyu = item.KareBoyu;
                            item1.OriginalKareX = newItem.OriginalRectangle.X;
                            item1.OriginalKareY = newItem.OriginalRectangle.Y;
                            //item1.OriginalKareEni = item.OriginalKareEni;
                            //item1.OriginalKareBoyu = item.OriginalKareBoyu;
                            context.SaveChanges();
                        }
                    }
                }
            }
        }
        public void PlaceItem(Depo depo)
        {
            bool isStageChanged = CheckifDepoStageChanged(depo);
            string param = CheckDepoParameters(depo);

            if (param == "Down Up Right")
            {
                bool placeOnce = true;
                SearchForPlace(depo);
                foreach (var cell in depo.gridmaps)
                {
                    if (cell.Row == depo.currentRow &&
                        cell.Column == depo.currentColumn)
                    {
                        if (placeOnce && !yerBul)
                        {
                            AddItemtoCell(cell, item_etiketi, item_aciklamasi, item_agirligi);
                            placeOnce = false;
                            cell.HoverPen = new Pen(System.Drawing.Color.DarkGray);
                            cell.HoverPen.DashStyle = DashStyle.DashDot;
                            nesneTimer.Stop();
                            yerBul = false;
                        }
                        else if (yerBul)
                        {
                            BlinkingCell = cell;
                            lbl_Nesne_Yerlestir_Value_X.Text = $"{cell.cell_Cm_X} cm";
                            lbl_Nesne_Yerlestir_Value_Y.Text = $"{cell.cell_Cm_Y} cm";
                            lbl_Nesne_Yerlestir_Value_Z.Text = $"{cell.toplam_Nesne_Yuksekligi} cm";
                            nesneTimer.Start();
                            yerBul = false;
                        }
                    }
                }
            }
            else if (param == "Down Up Left")
            {
                bool placeOnce = true;
                SearchForPlace(depo);
                foreach (var cell in depo.gridmaps)
                {
                    SearchForPlace(depo);
                    if (cell.Row == depo.currentRow &&
                        cell.Column == depo.currentColumn)
                    {
                        if (placeOnce && !yerBul)
                        {
                            AddItemtoCell(cell, item_etiketi, item_aciklamasi, item_agirligi);
                            placeOnce = false;
                            cell.HoverPen = new Pen(System.Drawing.Color.DarkGray);
                            cell.HoverPen.DashStyle = DashStyle.DashDot;
                            nesneTimer.Stop();
                            yerBul = false;
                        }
                        else
                        {
                            BlinkingCell = cell;
                            lbl_Nesne_Yerlestir_Value_X.Text = $"{cell.cell_Cm_X} cm";
                            lbl_Nesne_Yerlestir_Value_Y.Text = $"{cell.cell_Cm_Y} cm";
                            lbl_Nesne_Yerlestir_Value_Z.Text = $"{cell.toplam_Nesne_Yuksekligi} cm";
                            nesneTimer.Start();
                            yerBul = false;
                        }
                    }
                }
            }
            else if (param == "Up Down Right")
            {
                bool placeOnce = true;
                SearchForPlace(depo);
                foreach (var cell in depo.gridmaps)
                {
                    if (cell.Row == depo.currentRow &&
                        cell.Column == depo.currentColumn)
                    {
                        if (placeOnce && !yerBul)
                        {
                            AddItemtoCell(cell, item_etiketi, item_aciklamasi, item_agirligi);
                            placeOnce = false;
                            cell.HoverPen = new Pen(System.Drawing.Color.DarkGray);
                            cell.HoverPen.DashStyle = DashStyle.DashDot;
                            nesneTimer.Stop();
                            yerBul = false;
                        }
                        else
                        {
                            BlinkingCell = cell;
                            lbl_Nesne_Yerlestir_Value_X.Text = $"{cell.cell_Cm_X} cm";
                            lbl_Nesne_Yerlestir_Value_Y.Text = $"{cell.cell_Cm_Y} cm";
                            lbl_Nesne_Yerlestir_Value_Z.Text = $"{cell.toplam_Nesne_Yuksekligi} cm";
                            nesneTimer.Start();
                            yerBul = false;
                        }
                    }
                }
            }
            else if (param == "Up Down Left")
            {
                bool placeOnce = true;
                SearchForPlace(depo);
                foreach (var cell in depo.gridmaps)
                {
                    if (cell.Row == depo.currentRow &&
                        cell.Column == depo.currentColumn)
                    {
                        if (placeOnce && !yerBul)
                        {
                            AddItemtoCell(cell, item_etiketi, item_aciklamasi, item_agirligi);
                            placeOnce = false;
                            cell.HoverPen = new Pen(System.Drawing.Color.DarkGray);
                            cell.HoverPen.DashStyle = DashStyle.DashDot;
                            nesneTimer.Stop();
                            yerBul = false;
                        }
                        else
                        {
                            BlinkingCell = cell;
                            lbl_Nesne_Yerlestir_Value_X.Text = $"{cell.cell_Cm_X} cm";
                            lbl_Nesne_Yerlestir_Value_Y.Text = $"{cell.cell_Cm_Y} cm";
                            lbl_Nesne_Yerlestir_Value_Z.Text = $"{cell.toplam_Nesne_Yuksekligi} cm";
                            nesneTimer.Start();
                            yerBul = false;
                        }
                    }
                }
            }
            else if (param == "Middle Up Left")
            {
                bool placeOnce = true;
                SearchForPlace(depo);
                foreach (var cell in depo.gridmaps)
                {
                    if (cell.Row == depo.currentRow &&
                        cell.Column == depo.currentColumn)
                    {
                        if (placeOnce && !yerBul)
                        {
                            AddItemtoCell(cell, item_etiketi, item_aciklamasi, item_agirligi);
                            placeOnce = false;
                            cell.HoverPen = new Pen(System.Drawing.Color.DarkGray);
                            cell.HoverPen.DashStyle = DashStyle.DashDot;
                            nesneTimer.Stop();
                            yerBul = false;
                        }
                        else
                        {
                            BlinkingCell = cell;
                            lbl_Nesne_Yerlestir_Value_X.Text = $"{cell.cell_Cm_X} cm";
                            lbl_Nesne_Yerlestir_Value_Y.Text = $"{cell.cell_Cm_Y} cm";
                            lbl_Nesne_Yerlestir_Value_Z.Text = $"{cell.toplam_Nesne_Yuksekligi} cm";
                            nesneTimer.Start();
                            yerBul = false;
                        }
                    }
                }
            }
            else if (param == "Middle Up Right")
            {
                bool placeOnce = true;
                SearchForPlace(depo);
                foreach (var cell in depo.gridmaps)
                {
                    if (cell.Row == depo.currentRow &&
                        cell.Column == depo.currentColumn)
                    {
                        if (placeOnce && !yerBul)
                        {
                            AddItemtoCell(cell, item_etiketi, item_aciklamasi, item_agirligi);
                            placeOnce = false;
                            cell.HoverPen = new Pen(System.Drawing.Color.DarkGray);
                            cell.HoverPen.DashStyle = DashStyle.DashDot;
                            nesneTimer.Stop();
                            yerBul = false;
                        }
                        else
                        {
                            BlinkingCell = cell;
                            lbl_Nesne_Yerlestir_Value_X.Text = $"{cell.cell_Cm_X} cm";
                            lbl_Nesne_Yerlestir_Value_Y.Text = $"{cell.cell_Cm_Y} cm";
                            lbl_Nesne_Yerlestir_Value_Z.Text = $"{cell.toplam_Nesne_Yuksekligi} cm";
                            nesneTimer.Start();
                            yerBul = false;
                        }
                    }
                }
            }
            else if (param == "Middle Down Left")
            {
                bool placeOnce = true;
                SearchForPlace(depo);
                foreach (var cell in depo.gridmaps)
                {
                    if (cell.Row == depo.currentRow &&
                        cell.Column == depo.currentColumn)
                    {
                        if (placeOnce && !yerBul)
                        {
                            AddItemtoCell(cell, item_etiketi, item_aciklamasi, item_agirligi);
                            placeOnce = false;
                            cell.HoverPen = new Pen(System.Drawing.Color.DarkGray);
                            cell.HoverPen.DashStyle = DashStyle.DashDot;
                            nesneTimer.Stop();
                            yerBul = false;
                        }
                        else
                        {
                            BlinkingCell = cell;
                            lbl_Nesne_Yerlestir_Value_X.Text = $"{cell.cell_Cm_X} cm";
                            lbl_Nesne_Yerlestir_Value_Y.Text = $"{cell.cell_Cm_Y} cm";
                            lbl_Nesne_Yerlestir_Value_Z.Text = $"{cell.toplam_Nesne_Yuksekligi} cm";
                            nesneTimer.Start();
                            yerBul = false;
                        }
                    }
                }
            }
            else if (param == "Middle Down Right")
            {
                bool placeOnce = true;
                SearchForPlace(depo);
                foreach (var cell in depo.gridmaps)
                {
                    if (cell.Row == depo.currentRow &&
                        cell.Column == depo.currentColumn)
                    {
                        if (placeOnce && !yerBul)
                        {
                            AddItemtoCell(cell, item_etiketi, item_aciklamasi, item_agirligi);
                            placeOnce = false;
                            cell.HoverPen = new Pen(System.Drawing.Color.DarkGray);
                            cell.HoverPen.DashStyle = DashStyle.DashDot;
                            nesneTimer.Stop();
                            yerBul = false;
                        }
                        else
                        {
                            BlinkingCell = cell;
                            lbl_Nesne_Yerlestir_Value_X.Text = $"{cell.cell_Cm_X} cm";
                            lbl_Nesne_Yerlestir_Value_Y.Text = $"{cell.cell_Cm_Y} cm";
                            lbl_Nesne_Yerlestir_Value_Z.Text = $"{cell.toplam_Nesne_Yuksekligi} cm";
                            nesneTimer.Start();
                            yerBul = false;
                        }
                    }
                }
            }
        }
        public bool CheckifDepoisEmpty(Depo depo)
        {
            bool isEmpty = true;

            foreach (var cell in depo.gridmaps)
            {
                if (cell.items.Count > 0)
                {
                    isEmpty = false;
                }
            }
            if (isEmpty)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool CheckifDepoStageChanged(Depo depo)
        {
            bool isStage1Over = true;

            foreach (var cell in depo.gridmaps)
            {
                if (cell.items.Count != depo.asama1_ItemSayisi)
                {
                    isStage1Over = false;
                }
            }
            if (isStage1Over)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Depo? GetDepotoPlaceItem(string item_turu)
        {
            List<int> Q = new List<int>();
            if (ambar != null)
            {
                foreach (var depo in ambar.depolar)
                {
                    string depo_stage = CheckDepoStage(depo);

                    if (depo.ItemTuru == item_turu && (depo.currentStage == 1 || depo.currentStage == 2))
                    {
                        if (depo_stage != "full")
                        {
                            Q.Add(depo.Yerlestirilme_Sirasi);
                        }
                    }
                }

                int deponumItemTuru = GetSmallestNumber(Q);

                foreach (var depo in ambar.depolar)
                {
                    string depo_stage = CheckDepoStage(depo);

                    if (depo.ItemTuru == item_turu)
                    {
                        if (depo_stage == "stage1")
                        {
                            if (depo.Yerlestirilme_Sirasi == deponumItemTuru)
                            {
                                depo.currentStage = 1;
                                return depo;
                            }
                        }
                        else if (depo_stage == "stage2")
                        {
                            if (depo.Yerlestirilme_Sirasi == deponumItemTuru)
                            {
                                depo.currentStage = 2;
                                return depo;
                            }
                        }
                        else if (depo_stage == "full")
                        {
                            if (depo.Yerlestirilme_Sirasi == deponumItemTuru)
                            {
                                depo.currentStage = 3;
                                return null;
                            }
                        }
                    }
                }
            }
            return null;
        }
        public int GetSmallestNumber(List<int> intList)
        {
            int result = 0;
            foreach (int number in intList)
            {
                if (number < result || result == 0)
                {
                    result = number;
                }
            }
            return result;
        }
        #endregion

        

        //MainPanel's events, that Area rectangles are drawed 
        #region DrawingPanel Events
        private void DrawingPanel_Scroll(object sender, ScrollEventArgs e)
        {
            DrawingPanel.Invalidate();
            DrawingPanel.Refresh();
        }
        private void DrawingPanel_MouseWheel(object? sender, MouseEventArgs e)
        {
            if (System.Windows.Forms.Control.ModifierKeys == Keys.Control)
            {
                float oldZoom = Zoomlevel;
                if (e.Delta > 0)
                {
                    if (Zoomlevel <= maxZoom)
                    {
                        Zoomlevel *= zoomStep;
                    }
                }
                else
                {
                    if (Zoomlevel > minZoom)
                    {
                        Zoomlevel /= zoomStep;
                        if (Zoomlevel < 1.0f)
                        {
                            Zoomlevel = 1.0f;
                        }
                    }
                    if (Zoomlevel <= 1)
                    {
                        DrawingPanel.AutoScrollMinSize = new Size(0, 0);
                    }
                }

                // Calculate the position of the mouse relative to the content before zooming
                mouseXRelativeToContent = (e.X - DrawingPanel.AutoScrollPosition.X) / oldZoom;
                mouseYRelativeToContent = (e.Y - DrawingPanel.AutoScrollPosition.Y) / oldZoom;
                // Apply the zoom to each fabrika
                ambar.ApplyZoom(Zoomlevel);

                // Calculate the new scroll positions to keep the mouse position consistent
                float newScrollX = mouseXRelativeToContent * Zoomlevel - e.X;
                float newScrollY = mouseYRelativeToContent * Zoomlevel - e.Y;

                DrawingPanel.AutoScrollPosition = new Point((int)newScrollX, (int)newScrollY);

                DrawingPanel.Invalidate();
            }
        }
        private void DrawingPanel_Paint(object sender, PaintEventArgs e)
        {
            DrawingPanel.AutoScrollMinSize = new Size((int)(DrawingPanel.ClientRectangle.Width * Zoomlevel), (int)(DrawingPanel.ClientRectangle.Height * Zoomlevel));
            Bitmap bitmap = new Bitmap((int)(DrawingPanel.ClientRectangle.Width * Zoomlevel), (int)(DrawingPanel.ClientRectangle.Height * Zoomlevel));
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(System.Drawing.Color.White);

                int scrollX = DrawingPanel.AutoScrollPosition.X;
                int scrollY = DrawingPanel.AutoScrollPosition.Y;

                g.TranslateTransform(scrollX, scrollY);

                System.Drawing.Font font = new System.Drawing.Font("Arial", 10);
                SolidBrush brush = new SolidBrush(System.Drawing.Color.Red);

                System.Drawing.Point point = new System.Drawing.Point(DrawingPanel.ClientRectangle.Left, DrawingPanel.ClientRectangle.Top);

                if (ambar != null)
                {
                    //string layoutRectangle1 = $"Ambar SelectLayoutRectangle: {ambar.SelectLayoutRectangle}";
                    //string rectangle1 = $" Ambar Rectangle: {ambar.Rectangle}";
                    //string originalRectangle1 = $" Ambar OriginalRectangle: {ambar.OriginalRectangle}";

                    //g.DrawString(rectangle1, font, brush, new System.Drawing.Point(point.X, point.Y + 20));

                    //g.DrawString(originalRectangle1, font, brush, new System.Drawing.Point(point.X, point.Y + 40));

                    //g.DrawString(layoutRectangle1, font, brush, point);

                    foreach (var depo in ambar.depolar)
                    {
                        //System.Drawing.Font font1 = new System.Drawing.Font("Arial", 8);
                        //SolidBrush brush1 = new SolidBrush(System.Drawing.Color.Red);

                        //string layoutRectangle = $"Depo SelectLayoutRectangle: {depo.SelectLayoutRectangle}";
                        //string rectangle = $" Depo Rectangle: {depo.Rectangle}";
                        //string originalRectangle = $" Depo OriginalRectangle: {depo.OriginalRectangle}";

                        //g.DrawString(rectangle, font1, brush1, new System.Drawing.Point(point.X, point.Y + 60));

                        //g.DrawString(originalRectangle, font1, brush1, new System.Drawing.Point(point.X, point.Y + 80));

                        //g.DrawString(layoutRectangle, font1, brush1, new System.Drawing.Point(point.X, point.Y + 100));
                    }
                    //foreach (var depo in ambar.depolar)
                    //{
                    //    string currentRow = $"Current Row: {depo.currentRow}";
                    //    string currentColumn = $"Current Column: {depo.currentColumn}";

                    //    g.DrawString(currentRow, font, brush, new PointF(ambar.Rectangle.X + 5, ambar.Rectangle.Y + 20));
                    //    g.DrawString(currentColumn, font, brush, new PointF(ambar.Rectangle.X + 5, ambar.Rectangle.Y + 40));


                    //    foreach (var cell in depo.gridmaps)
                    //    {
                    //        string cellRow = $"Row: {cell.Row}";
                    //        string cellColumn = $"Col: {cell.Column}";

                    //        g.DrawString(cellRow, font, brush, new PointF(cell.Rectangle.X + 5, cell.Rectangle.Y + 5));
                    //        g.DrawString(cellColumn, font, brush, new PointF(cell.Rectangle.X + 5, cell.Rectangle.Y + 15));
                    //    }
                    //}

                    ambar.Draw(g);
                }
            }
            e.Graphics.DrawImage(bitmap, 0, 0);
        }
        private void DrawingPanel_MouseMove(object sender, MouseEventArgs e)
        {
            Point scaledPoint = new Point((int)((e.X - DrawingPanel.AutoScrollPosition.X)), (int)((e.Y - DrawingPanel.AutoScrollPosition.Y)));
            infoPoint = scaledPoint;
            if (e.Button == MouseButtons.Left && System.Windows.Forms.Control.ModifierKeys == Keys.Shift)
            {
                float offsetX = (e.X - DragStartPoint.X);
                float offsetY = (e.Y - DragStartPoint.Y);

                Point newScrollPosition = new Point((int)(scrollStartPoint.X - offsetX), (int)(scrollStartPoint.Y - (int)offsetY));

                DrawingPanel.AutoScrollPosition = newScrollPosition;
            }
            if (ambar != null)
            {
                ambar.OnMouseMove(e);
            }

        }
        private void DrawingPanel_MouseDown(object sender, MouseEventArgs e)
        {
            bool contains = false;
            scaledPoint = new Point((int)((e.X - DrawingPanel.AutoScrollPosition.X)),
                (int)((e.Y - DrawingPanel.AutoScrollPosition.Y)));
            if (ambar != null)
            {
                foreach (var depo in ambar.depolar)
                {
                    if (depo.Rectangle.Contains(scaledPoint))
                    {
                        selectedDepo = depo;
                    }
                    foreach (var cell in depo.gridmaps)
                    {
                        if (cell.Rectangle.Contains(scaledPoint))
                        {
                            RightClickCell = cell;
                        }
                    }
                }
                foreach (var conveyor in ambar.conveyors)
                {
                    if (conveyor.Rectangle.Contains(scaledPoint))
                    {
                        RightClickConveyor = conveyor;
                    }
                }
                ambar.OnMouseDown(e);

                if (e.Button == MouseButtons.Left && System.Windows.Forms.Control.ModifierKeys == Keys.Shift)
                {
                    DragStartPoint = e.Location;
                    scrollStartPoint = new PointF(-DrawingPanel.AutoScrollPosition.X, -DrawingPanel.AutoScrollPosition.Y);
                }
                DrawingPanel.Invalidate();

                foreach (var depo in ambar.depolar)
                {
                    foreach (var cell in depo.gridmaps)
                    {
                        if (cell.items.Count > 0)
                        {
                            if (cell.Rectangle.Contains(scaledPoint))
                            {
                                contains = true;
                            }
                            foreach (var item in cell.items)
                            {
                                if (item.Rectangle.Contains(scaledPoint))
                                {
                                    contains = true;
                                }
                            }
                        }
                    }
                }

                if (!contains)
                {
                    if (infopanel.Visible)
                    {
                        GVisual.HideControl(infopanel, DrawingPanel);
                    }
                }
            }
        }
        private void DrawingPanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (ambar != null)
            {
                ambar.OnMouseUp(e);
            }
        }
        private void DrawingPanel_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ambar.OnMouseDoubleClick(e);
        }
        #endregion



        //Get Points for Adding Reference Points to Items
        #region Get Points for Rectangles Such as (Center, Middle of top edge, Middle of right edge etc.)
        //Get Points for Rectangles
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

        #endregion



        //Layout Operations
        #region Layout Events

        //Button event that opens dialog for Creating Layouts
        private async void btn_Layout_Olustur_Click(object sender, EventArgs e)
        {
            ProgressBar_Title.Text = "Yükleniyor...";
            toolStripBTN_ExportToExcel.Enabled = false;
            toolStripBTN_Layout_Sec.Enabled = false;
            toolStripButtonShowCellTag.Enabled = false;
            btn_Layout_Olustur.Enabled = false;

            GVisual.ShowControl(ProgressBarPanel, this, ProgressBarPointLayoutOlustur);

            var progress = new Progress<int>(value =>
            {
                progressBar.Value = value;
            });

            await Task.Run(() => LayoutOlusturFirstDatabaseOperation(progress));

            GVisual.HideControl(ProgressBarPanel, this);
            progressBar.Value = 0;

            toolStripBTN_ExportToExcel.Enabled = true;
            toolStripBTN_Layout_Sec.Enabled = true;
            toolStripButtonShowCellTag.Enabled = true;
            btn_Layout_Olustur.Enabled = true;

            using (var dia = new LayoutOlusturma(this, null))
            {
                if (dia.ShowDialog() == DialogResult.OK)
                {
                    if (dia.Ambar != null)
                    {
                        if (ambar != null)
                        {
                            ambar = null;
                        }

                        ambar = dia.Ambar;

                        ambar.KareX = ambar.Rectangle.X;
                        ambar.KareY = ambar.Rectangle.Y;
                        ambar.KareEni = ambar.Rectangle.Width;
                        ambar.KareBoyu = ambar.Rectangle.Height;
                        ambar.OriginalKareX = ambar.OriginalRectangle.X;
                        ambar.OriginalKareY = ambar.OriginalRectangle.Y;
                        ambar.OriginalKareEni = ambar.OriginalRectangle.Width;
                        ambar.OriginalKareBoyu = ambar.OriginalRectangle.Height;
                        ambar.Zoomlevel = Zoomlevel;

                        foreach (var conveyor in ambar.conveyors)
                        {
                            conveyor.layout = null;

                            conveyor.KareX = conveyor.Rectangle.X;
                            conveyor.KareY = conveyor.Rectangle.Y;
                            conveyor.KareEni = conveyor.Rectangle.Width;
                            conveyor.KareBoyu = conveyor.Rectangle.Height;
                            conveyor.OriginalKareX = conveyor.OriginalRectangle.X;
                            conveyor.OriginalKareY = conveyor.OriginalRectangle.Y;
                            conveyor.OriginalKareEni = conveyor.OriginalRectangle.Width;
                            conveyor.OriginalKareBoyu = conveyor.OriginalRectangle.Height;
                            conveyor.Zoomlevel = Zoomlevel;


                            foreach (var reff in conveyor.ConveyorReferencePoints)
                            {

                                reff.KareX = reff.Rectangle.X;
                                reff.KareY = reff.Rectangle.Y;
                                reff.KareEni = reff.Rectangle.Width;
                                reff.KareBoyu = reff.Rectangle.Height;
                                reff.OriginalKareX = reff.OriginalRectangle.X;
                                reff.OriginalKareY = reff.OriginalRectangle.Y;
                                reff.OriginalKareEni = reff.OriginalRectangle.Width;
                                reff.OriginalKareBoyu = reff.OriginalRectangle.Height;
                                reff.Zoomlevel = Zoomlevel;
                                reff.Layout = null;
                            }

                            if (!ambar.conveyors.Contains(conveyor))
                            {
                                ambar.conveyors.Add(conveyor);
                            }
                        }
                        foreach (var depo in ambar.depolar)
                        {
                            
                            depo.OriginalDepoSizeWidth = depo.OriginalRectangle.Width;
                            depo.OriginalDepoSizeHeight = depo.OriginalRectangle.Height;
                            depo.KareX = depo.Rectangle.X;
                            depo.KareY = depo.Rectangle.Y;
                            depo.KareEni = depo.Rectangle.Width;
                            depo.KareBoyu = depo.Rectangle.Height;
                            depo.OriginalKareX = depo.OriginalRectangle.X;
                            depo.OriginalKareY = depo.OriginalRectangle.Y;
                            depo.OriginalKareEni = depo.OriginalRectangle.Width;
                            depo.OriginalKareBoyu = depo.OriginalRectangle.Height;
                            depo.Zoomlevel = Zoomlevel;
                            depo.layout = null;

                            if (!ambar.depolar.Contains(depo))
                            {
                                ambar.depolar.Add(depo);
                            }

                            foreach (var cell in depo.gridmaps)
                            {
                                cell.KareX = cell.Rectangle.X;
                                cell.KareY = cell.Rectangle.Y;
                                cell.KareEni = cell.Rectangle.Width;
                                cell.KareBoyu = cell.Rectangle.Height;
                                cell.OriginalKareX = cell.OriginalRectangle.X;
                                cell.OriginalKareY = cell.OriginalRectangle.Y;
                                cell.OriginalKareEni = cell.OriginalRectangle.Width;
                                cell.OriginalKareBoyu = cell.OriginalRectangle.Height;
                                cell.Zoomlevel = Zoomlevel;
                                cell.Layout = null;

                                if (!depo.gridmaps.Contains(cell))
                                {
                                    depo.gridmaps.Add(cell);
                                }
                            }
                        }

                        ProgressBar_Title.Text = "Layout Yükleniyor...";
                        toolStripBTN_ExportToExcel.Enabled = false;
                        toolStripBTN_Layout_Sec.Enabled = false;
                        toolStripButtonShowCellTag.Enabled = false;
                        btn_Layout_Olustur.Enabled = false;

                        GVisual.ShowControl(ProgressBarPanel, this, ProgressBarPointLayoutOlustur);

                        var progress1 = new Progress<int>(value =>
                        {
                            progressBar.Value = value;
                        });

                        await Task.Run(() => LayoutOlusturSecondDatabaseOperation(progress1, dia.LayoutName, dia.LayoutDescription));
                        GVisual.HideControl(ProgressBarPanel, this);
                        progressBar.Value = 0;

                        toolStripBTN_ExportToExcel.Enabled = true;
                        toolStripBTN_Layout_Sec.Enabled = true;
                        toolStripButtonShowCellTag.Enabled = true;
                        btn_Layout_Olustur.Enabled = true;

                        DrawingPanel.Invalidate();
                    }
                }
            }
        }



        //Button event that opens the dialog for Selecting Layouts
        private async void toolStripBTN_Layout_Sec_Click(object sender, EventArgs e)
        {
            if (ambar != null)
            {
                DrawingPanel.AutoScrollMinSize = new Size(0, 0);
                ambar.ApplyZoom(1f);
                foreach (var depo in ambar.depolar)
                {
                    depo.ApplyZoom(1f);
                    foreach (var cell in depo.gridmaps)
                    {
                        cell.ApplyZoom(1f);
                        foreach (var item in cell.items)
                        {
                            item.ApplyZoom(1f);

                        }
                    }
                }

                foreach (var conveyor in ambar.conveyors)
                {
                    conveyor.ApplyZoom(1f);
                    foreach (var reff in conveyor.ConveyorReferencePoints)
                    {
                        reff.ApplyZoom(1f);
                    }
                }
            }

            ProgressBar_Title.Text = "Layout Yükleniyor...";
            toolStripBTN_ExportToExcel.Enabled = false;
            toolStripBTN_Layout_Sec.Enabled = false;
            toolStripButtonShowCellTag.Enabled = false;
            btn_Layout_Olustur.Enabled = false;

            GVisual.ShowControl(ProgressBarPanel, this, ProgressBarPoint);

            var progress = new Progress<int>(value =>
            {
                if (value < 100)
                {
                    progressBar.Value = value;
                }
            });

            await Task.Run(() => LayoutSecDataBaseOperations(progress));
            GVisual.HideControl(ProgressBarPanel, this);
            progressBar.Value = 0;

            toolStripBTN_ExportToExcel.Enabled = true;
            toolStripBTN_Layout_Sec.Enabled = true;
            toolStripButtonShowCellTag.Enabled = true;
            btn_Layout_Olustur.Enabled = true;

            using (var dialog = new SelectLayouts(DrawingPanel, this))
            {
                if (dialog.DialogResult == DialogResult.Cancel)
                {
                    dialog.Hide();
                    dialog.Close();
                }

                if (!dialog.IsDisposed)
                {
                    if (dialog.ShowDialog() == DialogResult.OK && dialog.DialogResult != DialogResult.Cancel)
                    {
                        Ambar newAmbar = new Ambar();

                        newAmbar = (Ambar)dialog.SelectedPB.Tag;

                        newAmbar.Rectangle = newAmbar.OriginalRectangle;

                        ambar = newAmbar;
                        ambar.AmbarId = newAmbar.AmbarId;

                        foreach (var conveyor in ambar.conveyors)
                        {
                            conveyor.Rectangle = conveyor.OriginalRectangle;
                            foreach (var reff in conveyor.ConveyorReferencePoints)
                            {
                                reff.Rectangle = reff.OriginalRectangle;
                            }
                        }

                        foreach (var depo in ambar.depolar)
                        {
                            depo.Rectangle = depo.OriginalRectangle;
                            foreach (var cell in depo.gridmaps)
                            {
                                cell.Rectangle = cell.OriginalRectangle;
                                foreach (var item in cell.items)
                                {
                                    cell.toplam_Nesne_Yuksekligi = item.ItemYuksekligi * cell.items.Count;
                                    item.Rectangle = item.OriginalRectangle;
                                    foreach (var reff in item.ItemReferencePoints)
                                    {
                                        reff.Rectangle = reff.OriginalRectangle;
                                    }
                                }
                            }
                        }
                        DrawingPanel.Invalidate();
                        opcount = 0;
                    }
                }
            }
        }

        #endregion



        //Async Database Tasks
        #region Async Database Tasks

        //This is for When User clicked to the Select Layout ToolStrip Item it saves the current Layout State to the Database
        private async Task LayoutSecDataBaseOperations(IProgress<int> progress)
        {
            using (var context = new DBContext())
            {
                if (ambar != null)
                {
                    int totalProgress = 0;
                    int progressStep = 0;

                    foreach (var depo in ambar.depolar)
                    {
                        if (ambar.depolar.Count * depo.gridmaps.Count < 100 && depo.gridmaps.Count > 0)
                        {
                            progressStep = (100 / (ambar.depolar.Count * depo.gridmaps.Count));
                        }
                        else
                        {
                            progressStep = 10;
                        }

                        if (progressBar.Value < 100 - (progressStep / 2) && ambar.depolar.Count >= 5)
                        {
                            totalProgress += progressStep;
                            progress.Report(totalProgress);
                        }

                        var depo1 = (from x in context.Depos
                                     where x.DepoId == depo.DepoId
                                     select x).FirstOrDefault();

                        if (depo1 != null)
                        {
                            depo1.currentColumn = depo.currentColumn;
                            depo1.currentRow = depo.currentRow;
                        }

                        foreach (var cell in depo.gridmaps)
                        {
                            if (progressBar.Value < 100 - (progressStep / 2) && ambar.depolar.Count < 5)
                            {
                                totalProgress += progressStep;
                                progress.Report(totalProgress);
                            }

                            var cells = (from x in context.Cells
                                         where x.CellId == cell.CellId
                                         select x).ToList();

                            foreach (var cell1 in cells)
                            {
                                cell1.toplam_Nesne_Yuksekligi = cell.toplam_Nesne_Yuksekligi;
                                await context.SaveChangesAsync();
                            }

                            foreach (var item in cell.items)
                            {
                                var item1 = (from x in context.Items
                                             where x.ItemId == item.ItemId
                                             select x).FirstOrDefault();

                                if (item1 != null)
                                {
                                    Models.Item DBItem = new Models.Item(cell.CellId, item.ItemEtiketi, item.ItemTuru, item.ItemEni,
                                        item.ItemBoyu, item.ItemYuksekligi, item.Rectangle.X, item.Rectangle.Y, item.Rectangle.Width, item.Rectangle.Height,
                                        item.Rectangle.X, item.Rectangle.Y, item.OriginalRectangle.Width, item.OriginalRectangle.Height,
                                        item.Zoomlevel, item.ItemAgirligi, item.ItemAciklamasi, item.Cm_X_Axis, item.Cm_Y_Axis, item.Cm_Z_Axis);

                                    context.Items.Add(DBItem);
                                    context.Items.Remove(item1);
                                    await context.SaveChangesAsync();
                                    item.ItemId = DBItem.ItemId;

                                    foreach (var reff in item.ItemReferencePoints)
                                    {
                                        Models.ItemReferencePoint DBreff = new Models.ItemReferencePoint(item.ItemId, reff.Rectangle.X, reff.Rectangle.Y,
                                            reff.Rectangle.Width, reff.Rectangle.Height, reff.OriginalRectangle.X, reff.OriginalRectangle.Y,
                                            reff.OriginalRectangle.Width, reff.OriginalRectangle.Height, reff.Zoomlevel, reff.Pointsize);

                                        context.ItemReferencePoints.Add(DBreff);
                                        context.SaveChanges();
                                        reff.ReferenceId = DBreff.ReferenceId;
                                    }
                                }
                            }
                        }
                        if (progressBar.Value < 100 && ambar.depolar.Count < 5)
                        {
                            totalProgress = (int)opcount;
                            progress.Report(totalProgress);
                        }
                    }
                }
            }
        }



        //This is for when user clicked to the create layout button it saves the current layout state to the database
        private async Task LayoutOlusturFirstDatabaseOperation(IProgress<int> progress)
        {
            if (ambar != null)
            {
                int totalProgress = 0;
                int progressStep = 0;

                using (var context = new DBContext())
                {
                    foreach (var depo in ambar.depolar)
                    {
                        if (ambar.depolar.Count * depo.gridmaps.Count < 100 && depo.gridmaps.Count > 0)
                        {
                            progressStep = 100 / ambar.depolar.Count * depo.gridmaps.Count;
                        }
                        else
                        {
                            progressStep = 10;
                        }

                        if (progressBar.Value < 100 - progressStep && ambar.depolar.Count >= 5)
                        {
                            totalProgress += progressStep;
                            progress.Report(totalProgress);
                        }

                        var depo1 = await (from x in context.Depos
                                           where x.DepoId == depo.DepoId
                                           select x).FirstOrDefaultAsync();

                        if (depo1 != null)
                        {
                            depo1.currentColumn = depo.currentColumn;
                            depo1.currentRow = depo.currentRow;
                            await context.SaveChangesAsync();
                        }

                        foreach (var cell in depo.gridmaps)
                        {
                            if (progressBar.Value < 100 - progressStep && ambar.depolar.Count < 5)
                            {
                                totalProgress += progressStep;
                                progress.Report(totalProgress);
                            }

                            var cell1 = await (from x in context.Cells
                                               where x.CellId == cell.CellId
                                               select x).FirstOrDefaultAsync();

                            if (cell1 != null)
                            {
                                cell1.toplam_Nesne_Yuksekligi = cell.toplam_Nesne_Yuksekligi;
                                await context.SaveChangesAsync();
                            }

                            foreach (var item in cell.items)
                            {
                                if (item.ItemId == 0)
                                {
                                    Models.Item Dbitem = new Models.Item(cell.CellId, item.ItemEtiketi,
                                        item.ItemTuru, item.ItemEni, item.ItemBoyu, item.ItemYuksekligi,
                                        item.Rectangle.X, item.Rectangle.Y, item.KareEni, item.KareBoyu,
                                        item.OriginalRectangle.X, item.OriginalRectangle.Y, item.OriginalRectangle.Width,
                                        item.OriginalRectangle.Height, item.Zoomlevel, item.ItemAgirligi, item.ItemAciklamasi,
                                        item.Cm_X_Axis, item.Cm_Y_Axis, item.Cm_Z_Axis);

                                    await context.Items.AddAsync(Dbitem);
                                    await context.SaveChangesAsync();

                                    foreach (var reff in item.ItemReferencePoints)
                                    {
                                        Models.ItemReferencePoint DBreff = new Models.ItemReferencePoint(item.ItemId, reff.Rectangle.X, reff.Rectangle.Y,
                                        reff.Rectangle.Width, reff.Rectangle.Height, reff.OriginalRectangle.X, reff.OriginalRectangle.Y,
                                        reff.OriginalRectangle.Width, reff.OriginalRectangle.Height, reff.Zoomlevel, reff.Pointsize);

                                        await context.ItemReferencePoints.AddAsync(DBreff);
                                        await context.SaveChangesAsync();
                                        reff.ReferenceId = DBreff.ReferenceId;
                                    }
                                }
                                else
                                {
                                    var item1 = await (from x in context.Items
                                                       where x.ItemId == item.ItemId
                                                       select x).FirstOrDefaultAsync();

                                    if (item1 != null)
                                    {
                                        Models.Item Dbitem = new Models.Item(cell.CellId, item.ItemEtiketi,
                                            item.ItemTuru, item.ItemEni, item.ItemBoyu, item.ItemYuksekligi,
                                            item.Rectangle.X, item.Rectangle.Y, item.KareEni, item.KareBoyu,
                                            item.OriginalRectangle.X, item.OriginalRectangle.Y, item.OriginalRectangle.Width,
                                            item.OriginalRectangle.Height, item.Zoomlevel, item.ItemAgirligi, item.ItemAciklamasi,
                                            item.Cm_X_Axis, item.Cm_Y_Axis, item.Cm_Z_Axis);

                                        item1.KareX = item.Rectangle.X;
                                        item1.KareY = item.Rectangle.Y;
                                        //item1.KareEni = item.KareEni;
                                        //item1.KareBoyu = item.KareBoyu;
                                        item1.OriginalKareX = item.OriginalRectangle.X;
                                        item1.OriginalKareY = item.OriginalRectangle.Y;
                                        //item1.OriginalKareEni = item.OriginalKareEni;
                                        //item1.OriginalKareBoyu = item.OriginalKareBoyu;
                                        await context.SaveChangesAsync();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }



        //This is for when user clicked to the save and load layout button in the layout creation form it creates the layout in the database
        private async Task LayoutOlusturSecondDatabaseOperation(IProgress<int> progress, string LayoutName, string LayoutDescription)
        {
            if (ambar != null)
            {
                int totalProgress = 0;
                int progressStep = 0;

                using (var context = new DBContext())
                {
                    Models.Layout newLayout = new Models.Layout(LayoutName, LayoutDescription, 0);
                    context.Layout.Add(newLayout);
                    context.SaveChanges();
                    newLayout.LayoutId = newLayout.LayoutId;


                    Ambar newAmbar = new Ambar(newLayout.LayoutId, "Ambar", "Ambar",
                            ambar.KareX, ambar.KareY, ambar.KareEni, ambar.KareBoyu,
                            ambar.OriginalKareX, ambar.OriginalKareY,
                            ambar.OriginalKareEni, ambar.OriginalKareBoyu,
                            ambar.Zoomlevel, ambar.AmbarEni, ambar.AmbarBoyu);

                    await context.Ambars.AddAsync(newAmbar);
                    await context.SaveChangesAsync();
                    ambar.AmbarId = newAmbar.AmbarId;

                    foreach (var depo in ambar.depolar)
                    {
                        if (ambar.depolar.Count * depo.gridmaps.Count < 100)
                        {
                            progressStep = 100 / ambar.depolar.Count * depo.gridmaps.Count;
                        }
                        else
                        {
                            progressStep = 10;
                        }

                        if (progressBar.Value < 100 - progressStep)
                        {
                            totalProgress += progressStep;
                            progress.Report(totalProgress);
                        }

                        Depo newDepo = new Depo(newAmbar.AmbarId,
                            "Depo", "Depo", depo.DepoAlaniEni,
                            depo.DepoAlaniBoyu, depo.DepoAlaniYuksekligi,
                            depo.OriginalDepoSizeWidth, depo.OriginalDepoSizeHeight,
                            depo.Rectangle.X, depo.Rectangle.Y, depo.KareEni, depo.KareBoyu,
                            depo.OriginalKareX, depo.OriginalKareY,
                            depo.OriginalKareEni, depo.OriginalKareBoyu, depo.Zoomlevel,
                            depo.itemDrop_StartLocation, depo.itemDrop_UpDown,
                            depo.itemDrop_LeftRight, depo.asama1_Yuksekligi, depo.asama2_Yuksekligi,
                            depo.Yerlestirilme_Sirasi, depo.DepoAlaniEni * 100, depo.DepoAlaniBoyu * 100,
                            depo.ColumnCount, depo.RowCount, depo.currentColumn, depo.currentRow, depo.ItemTuru,
                            depo.asama1_ItemSayisi, depo.asama2_ToplamItemSayisi, depo.currentStage);

                        await context.Depos.AddAsync(newDepo);
                        await context.SaveChangesAsync();
                        depo.DepoId = newDepo.DepoId;

                        foreach (var cell in depo.gridmaps)
                        {
                            Balya_Yerleştirme.Models.Cell newCell =
                                new Balya_Yerleştirme.Models.Cell(
                                    newDepo.DepoId, cell.CellEtiketi,
                                    cell.CellEni, cell.CellBoyu,
                                    cell.CellYuksekligi, cell.CellMalSayisi,
                                    cell.KareX, cell.KareY, cell.KareEni,
                                    cell.KareBoyu, cell.OriginalKareX,
                                    cell.OriginalKareY, cell.OriginalKareEni,
                                    cell.OriginalKareBoyu, cell.Zoomlevel,
                                    cell.ItemSayisi, cell.DikeyKenarBoslugu,
                                    cell.YatayKenarBoslugu, cell.NesneEni,
                                    cell.NesneBoyu, cell.NesneYuksekligi,
                                    cell.Column, cell.Row, cell.toplam_Nesne_Yuksekligi, cell.cell_Cm_X, cell.cell_Cm_Y);

                            await context.Cells.AddAsync(newCell);
                            await context.SaveChangesAsync();
                            cell.CellId = newCell.CellId;
                        }
                    }
                    foreach (var conveyor in ambar.conveyors)
                    {
                        Conveyor conv = new Conveyor(newAmbar.AmbarId,
                            null, conveyor.KareX, conveyor.KareY,
                            conveyor.KareEni, conveyor.KareBoyu,
                            conveyor.OriginalKareX, conveyor.OriginalKareY,
                            conveyor.OriginalKareEni, conveyor.OriginalKareBoyu,
                            conveyor.Zoomlevel, conveyor.ConveyorEni,
                            conveyor.ConveyorBoyu);

                        await context.Conveyors.AddAsync(conv);
                        await context.SaveChangesAsync();
                        conveyor.ConveyorId = conv.ConveyorId;

                        foreach (var reff in conveyor.ConveyorReferencePoints)
                        {
                            ConveyorReferencePoint point = new ConveyorReferencePoint(
                                conveyor.ConveyorId, ambar.AmbarId, reff.Rectangle.X,
                                reff.Rectangle.Y, reff.Rectangle.Width,
                                reff.Rectangle.Height, reff.OriginalRectangle.X,
                                reff.OriginalRectangle.Y, reff.OriginalRectangle.Width,
                                reff.OriginalRectangle.Height, reff.Zoomlevel,
                                4);

                            await context.ConveyorReferencePoints.AddAsync(point);
                            await context.SaveChangesAsync();
                            reff.ReferenceId = point.ReferenceId;
                        }
                    }
                }
            }
        }

        #endregion



        //Connection to the PLC Operations
        #region Connect to the PLC Events
        //Connecting to the PLC
        private void btn_PLC_ConnectionPanel_Kapat_Click(object sender, EventArgs e)
        {
            if (PLC_Connection_Panel.Controls.Contains(txt_PLC_IP_Address))
            {
                GVisual.HideControl(txt_PLC_IP_Address, PLC_Connection_Panel);
                GVisual.HideControl(lbl_PLC_IP_Address, PLC_Connection_Panel);
                GVisual.HideControl(btn_Connect_to_PLC, PLC_Connection_Panel);
                GVisual.ChangeSize_of_Control(PLC_Connection_Panel, new Size(btn_PLC_ConnectionPanel_Kapat.Width + 5, btn_PLC_ConnectionPanel_Kapat.Height + 5));
                GVisual.Move_RightSide_of_AnotherControl(PLC_Connection_Panel, ToolStrip, 3);
                GVisual.Control_Center(btn_PLC_ConnectionPanel_Kapat, PLC_Connection_Panel);
                btn_PLC_ConnectionPanel_Kapat.Image = Resources.Resource1.Chevron_Right;
            }
            else
            {
                GVisual.ShowControl(txt_PLC_IP_Address, PLC_Connection_Panel, txt_PLC_IP_Address.Location);
                GVisual.ShowControl(lbl_PLC_IP_Address, PLC_Connection_Panel, lbl_PLC_IP_Address.Location);
                GVisual.ShowControl(btn_Connect_to_PLC, PLC_Connection_Panel, btn_Connect_to_PLC.Location);
                GVisual.ChangeSize_of_Control(PLC_Connection_Panel, new Size(312, 73));
                GVisual.Move_RightSide_of_AnotherControl(PLC_Connection_Panel, ToolStrip, 3);
                GVisual.Control_CenterRightEdge(btn_PLC_ConnectionPanel_Kapat, PLC_Connection_Panel, 3);
                btn_PLC_ConnectionPanel_Kapat.Image = Resources.Resource1.Chevron_Left;
            }
        }
        private void btn_PLC_Connection_Click(object sender, EventArgs e)
        {
            if (!CheckPanelVisible(PLC_DB_AdressPanel))
            {
                PLCBaglantisiniAyarlaButtonClicked?.Invoke(sender, e);
            }

            if (!PLC_Connection_Panel.Controls.Contains(txt_PLC_IP_Address))
            {
                btn_PLC_ConnectionPanel_Kapat_Click(this, EventArgs.Empty);
            }
            MainPanelMakeSmaller(DrawingPanel);
            Point LeftPanelLocation = GVisual.Point_Control_to_LeftSide_ofControl(PLC_DB_AdressPanel, DrawingPanel, 3);
            GVisual.ShowControl(PLC_DB_AdressPanel, this, LeftPanelLocation);
            DrawingPanel.Invalidate();
        }
        private void btn_PLC_DB_AddressPanel_Kapat_Click(object sender, EventArgs e)
        {
            GVisual.HideControl(PLC_DB_AdressPanel, this);
            PLCBaglantisiPaneliniKapat?.Invoke(sender, e);
            MainPanelMakeBigger(DrawingPanel);
            if (PLC_Connection_Panel.Controls.Contains(txt_PLC_IP_Address))
            {
                btn_PLC_ConnectionPanel_Kapat_Click(this, EventArgs.Empty);
            }
            DrawingPanel.Invalidate();
        }
        private void btn_DB_Onayla_Click(object sender, EventArgs e)
        {
            GVisual.HideControl(PLC_DB_AdressPanel, this);
            PLCBaglantisiPaneliniKapat?.Invoke(sender, e);
            MainPanelMakeBigger(DrawingPanel);
            if (PLC_Connection_Panel.Controls.Contains(txt_PLC_IP_Address))
            {
                btn_PLC_ConnectionPanel_Kapat_Click(this, EventArgs.Empty);
            }
            DrawingPanel.Invalidate();
        }
        private void btn_DB_Vazgec_Click(object sender, EventArgs e)
        {
            GVisual.HideControl(PLC_DB_AdressPanel, this);
            PLCBaglantisiPaneliniKapat?.Invoke(sender, e);
            MainPanelMakeBigger(DrawingPanel);
            if (PLC_Connection_Panel.Controls.Contains(txt_PLC_IP_Address))
            {
                btn_PLC_ConnectionPanel_Kapat_Click(this, EventArgs.Empty);
            }
            DrawingPanel.Invalidate();
        }
        private void btn_Connect_to_PLC_Click(object sender, EventArgs e)
        {
            try
            {
                PLC = new Plc(CpuType.S71200, txt_PLC_IP_Address.Text, 0, 1);
                PLC.Open();
                StatusBarConnectionSuccess();
            }
            catch (Exception ex)
            {
                if (ambar != null)
                {
                    if (!PLC_Sim_Panel.Visible && !Nesne_Yerlestirme_First_Panel.Visible && !Nesne_Al_First_Panel.Visible)
                    {
                        GVisual.ShowControl(PLC_Sim_Panel, this, rightSidePanelLocation);
                        MainPanelMakeSmaller1(DrawingPanel);
                    }
                    else if (!PLC_Sim_Panel.Visible && Nesne_Yerlestirme_First_Panel.Visible && !Nesne_Al_First_Panel.Visible)
                    {
                        GVisual.HideControl(Nesne_Yerlestirme_First_Panel, this);
                        GVisual.ShowControl(PLC_Sim_Panel, this, rightSidePanelLocation);
                        MainPanelMakeSmaller1(DrawingPanel);
                    }
                    else if (!PLC_Sim_Panel.Visible && !Nesne_Yerlestirme_First_Panel.Visible && Nesne_Al_First_Panel.Visible)
                    {
                        GVisual.HideControl(Nesne_Al_First_Panel, this);
                        GVisual.ShowControl(PLC_Sim_Panel, this, rightSidePanelLocation);
                        MainPanelMakeSmaller1(DrawingPanel);
                    }
                    if (MoveRightEvent != null)
                    {
                        MoveRightEvent.Invoke(sender, e);
                    }
                }
                else
                {
                    MessageBox.Show("PLC Simülasyonunun açılması için bir layout'un yüklü olması gerekiyor.", "Layout Yüklü Değil", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }


                StatusBarConnectionFailed();
            }
        }
        private void StatusBarConnectionSuccess()
        {
            btn_Connect_to_PLC.ForeColor = System.Drawing.Color.Green;
            btn_Connect_to_PLC.Text = "Bağlandı";
            btn_PLC_ConnectionPanel_Kapat.BackColor = System.Drawing.Color.Lime;
        }
        private void StatusBarConnectionFailed()
        {
            btn_Connect_to_PLC.ForeColor = System.Drawing.Color.Red;
            btn_Connect_to_PLC.Text = "Bağlanılamadı";
            btn_PLC_ConnectionPanel_Kapat.BackColor = System.Drawing.Color.Red;
        }
        #endregion



        //PLC Simulation for Item Placement
        #region PLC Sİmulation for Adding Items

        //These are Simulation Panel Button Events (This panel works for both Item Placement and Item Removal Simulations
        //This is the Button that Opens the Simulation Panel for PLC
        private void btn_PLC_Barkod_Oku_Click(object sender, EventArgs e)
        {
            if (ambar != null)
            {
                if (!CheckPanelVisible(PLC_Sim_Panel))
                {
                    GVisual.ShowControl(PLC_Sim_Panel, this, rightSidePanelLocation);
                    MainPanelMakeSmaller1(DrawingPanel);
                    if (MoveLeftEvent != null)
                    {
                        MoveLeftEvent.Invoke(sender, e);
                    }
                }
                else
                {
                    GVisual.ShowControl(PLC_Sim_Panel, this, rightSidePanelLocation);
                    MainPanelMakeSmallest(DrawingPanel);
                    if (MoveLeftEvent != null)
                    {
                        MoveLeftEvent.Invoke(sender, e);
                    }
                }
                DrawingPanel.Invalidate();
            }
            else
            {
                MessageBox.Show("Lütfen önce Layout yükleyin ya da oluşturun.", "Layout Yüklenmemiş", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }
        //This is the Button that Closes the Simulation Panel for PLC
        private void btn_PLC_Sim_Panel_Kapat_Click(object sender, EventArgs e)
        {
            if (!CheckPanelVisible(PLC_Sim_Panel))
            {
                GVisual.HideControl(PLC_Sim_Panel, this);
                MainPanelMakeBigger(DrawingPanel);
                if (MoveRightEvent != null)
                {
                    MoveRightEvent.Invoke(sender, e);
                }
            }
            else
            {
                GVisual.HideControl(PLC_Sim_Panel, this);
                MainPanelMakeSmaller(DrawingPanel);
            }
            EmptyPLCSimPanel();
            nesneTimer.Stop();
            PLC_Timer.Stop();
            DrawingPanel.Invalidate();
        }
        //This is the Button that Starts the Item Placement Simulation
        private void btn_PLC_Sim_Barkod_Oku_Click(object sender, EventArgs e)
        {
            NesneKaldır = false;
            foreach (var depo in ambar.depolar)
            {
                if (!combo_Tur_Kodu.Items.Contains(depo.ItemTuru))
                {
                    combo_Tur_Kodu.Items.Add(depo.ItemTuru);
                }
            }
            if (!CheckPanelVisible(PLC_Sim_Panel))
            {
                Show_ItemPlacementPanel();
                MainPanelMakeSmallest(DrawingPanel);
                GVisual.HideControl(Nesne_Yerlestirme_Second_Panel, Nesne_Yerlestirme_First_Panel);
                if (MoveLeftEvent != null)
                {
                    MoveLeftEvent.Invoke(sender, e);
                }
            }
            else
            {
                HideEverything();
                Show_ItemPlacementPanel();
            }
            Point point = GVisual.Point_Control_to_BottomSide_ofControl(PLC_Sim_YerSoyle_Panel, btn_PLC_Sim_Nesne_Bul, 10);
            GVisual.ShowControl(PLC_Sim_YerSoyle_Panel, PLC_Sim_Panel, point);
            btn_PLC_Sim_Nesne_Yerlestirildi.Text = "Nesne\nYerleştirildi";
            btn_PLC_Sim_Nesne_Yerlestirilemedi.Text = "Nesne\nYerleştirilemedi";
            lbl_YerSoyle.Text = "Yer Söyle...";
            lbl_Yerlestiriliyor.Text = "Yerleştiriliyor...";
            lbl_YerSoyle.StateCommon.TextColor = System.Drawing.Color.Red;
            lbl_Yerlestiriliyor.StateCommon.TextColor = System.Drawing.Color.Red;
            PLC_Timer.Interval = 250;
            PLC_Timer.Start();
            DrawingPanel.Invalidate();
        }


        //These are Item Placement Panel Button Events
        //This is the Button that find the place for Item Placement
        private void btn_Balya_Yerlestir_Devam_Et_Click(object sender, EventArgs e)
        {
            item_etiketi = txt_Item_Etiketi.Text;
            item_aciklamasi = txt_Item_Aciklamasi.Text;
            item_turu = combo_Tur_Kodu.Text;
            item_agirligi = 20;

            errorProvider.SetError(txt_Item_Agirligi, string.Empty);
            errorProvider.SetError(combo_Tur_Kodu, string.Empty);
            errorProvider.SetError(txt_Item_Etiketi, string.Empty);
            errorProvider.SetError(txt_Item_Aciklamasi, string.Empty);
            errorProvider.Clear();

            item_agirligi = StrLib.ReplaceDotWithCommaReturnFloat
                (txt_Item_Agirligi, errorProvider, "Bu alan boş bırakılamaz.",
                "Bu alana bir sayı yazmalısınız.", "Buraya 0 ya da daha küçük bir değer giremezsiniz.");

            if (string.IsNullOrWhiteSpace(txt_Item_Etiketi.Text))
            {
                errorProvider.SetError(txt_Item_Etiketi, "Bu alan boş bırakılamaz.");
            }
            if (string.IsNullOrWhiteSpace(txt_Item_Aciklamasi.Text))
            {
                errorProvider.SetError(txt_Item_Aciklamasi, "Bu alan boş bırakılamaz.");
            }
            if (txt_Item_Etiketi.Text.Length > 50)
            {
                errorProvider.SetError(txt_Item_Etiketi, "Bu alana 50'den fazla karakter giremezsiniz.");
            }

            using (var context = new DBContext())
            {
                var item = (from x in context.Items
                            where x.ItemEtiketi == item_etiketi
                            select x).FirstOrDefault();

                if (item != null)
                {
                    errorProvider.SetError(txt_Item_Etiketi, "Bu etikete sahip bir nesne bulunuyor, Lütfen etiketi değiştirip tekrar deneyin.");
                }
            }
            if (ambar != null)
            {
                foreach (var depo in ambar.depolar)
                {
                    foreach (var cell in depo.gridmaps)
                    {
                        foreach (var item in cell.items)
                        {
                            if (item.ItemEtiketi == item_etiketi)
                            {
                                errorProvider.SetError(txt_Item_Etiketi, "Bu etikete sahip bir nesne bulunuyor, Lütfen etiketi değiştirip tekrar deneyin.");
                            }
                        }
                    }
                }
            }


            if (!errorProvider.HasErrors)
            {
                if (ambar != null)
                {
                    if (ambar.depolar.Count > 0)
                    {
                        Depo newDepo = GetDepotoPlaceItem(item_turu);

                        if (newDepo != null)
                        {
                            bool StageChanged = CheckifDepoStageChanged(newDepo);
                            bool isDepoEmpty = CheckifDepoisEmpty(newDepo);

                            if (StageChanged || isDepoEmpty)
                            {
                                //SetColumnStartPosition(newDepo);
                                //SetRowStartPosition(newDepo);
                            }
                            yerBul = true;
                            PlaceItem(newDepo);
                            Point location = new Point(7, 505);
                            GVisual.ShowControl(Nesne_Yerlestirme_Second_Panel, Nesne_Yerlestirme_First_Panel, location);
                            DrawingPanel.Invalidate();
                        }
                        else
                        {
                            MessageBox.Show
                                ("Tür kodu bulunamadı, lütfen eşleşen bir tür kodu girin ya da boş bırakın.",
                                "Tür kodu bulunamadı.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
        }
        //This is the Button that Places the Item
        private void btn_Nesne_Yerlestir_Click(object sender, EventArgs e)
        {
            Point point = GVisual.Point_Control_to_BottomSide_ofControl(PLC_Sim_Yerlestiriliyor_Panel, PLC_Sim_YerSoyle_Panel, 10);
            GVisual.ShowControl(PLC_Sim_Yerlestiriliyor_Panel, PLC_Sim_Panel, point);
            lbl_YerSoyle.StateCommon.TextColor = System.Drawing.Color.MidnightBlue;
            ShowNesneButtons = true;
        }
        //This is the Button that Closes Item Placement Panel
        private void btn_Balya_Yerlestirme_Paneli_Kapat_Click(object sender, EventArgs e)
        {
            if (PLC_Sim_Panel.Visible)
            {
                Hide_ItemPlacementPanel();
                MainPanelMakeSmaller1(DrawingPanel);
                if (MoveRightEvent != null)
                {
                    MoveRightEvent.Invoke(sender, e);
                }
            }
            else
            {
                Hide_ItemPlacementPanel();
                MainPanelMakeBigger(DrawingPanel);
                if (MoveRightEvent != null)
                {
                    MoveRightEvent.Invoke(sender, e);
                }
            }
            nesneTimer.Stop();
            PLC_Timer.Stop();
            DrawingPanel.Invalidate();
        }
        //This is the Button that Cancels Item Placement Process
        private void btn_Balya_Yerlestir_Vazgec_Click(object sender, EventArgs e)
        {
            if (PLC_Sim_Panel.Visible)
            {
                Hide_ItemPlacementPanel();
                GVisual.HideControl(PLC_Sim_Panel, this);
                MainPanelMakeBigger(DrawingPanel);
                if (MoveRightEvent != null)
                {
                    MoveRightEvent.Invoke(sender, e);
                }
            }
            else
            {
                Hide_ItemPlacementPanel();
                MainPanelMakeBigger(DrawingPanel);
                if (MoveRightEvent != null)
                {
                    MoveRightEvent.Invoke(sender, e);
                }
            }
            DrawingPanel.Invalidate();
        }



        //These are the timers that give information to the users
        private void PLC_Timer_Tick(object sender, EventArgs e)
        {
            if (lbl_YerSoyle.StateCommon.TextColor == System.Drawing.Color.Red)
            {
                lbl_YerSoyle.StateCommon.TextColor = System.Drawing.Color.Yellow;
            }
            else if (lbl_YerSoyle.StateCommon.TextColor == System.Drawing.Color.Yellow)
            {
                lbl_YerSoyle.StateCommon.TextColor = System.Drawing.Color.Red;
            }


            if (lbl_Yerlestiriliyor.StateCommon.TextColor == System.Drawing.Color.Red)
            {
                lbl_Yerlestiriliyor.StateCommon.TextColor = System.Drawing.Color.Yellow;
            }
            else if (lbl_Yerlestiriliyor.StateCommon.TextColor == System.Drawing.Color.Yellow)
            {
                lbl_Yerlestiriliyor.StateCommon.TextColor = System.Drawing.Color.Red;
            }

            if (ShowNesneButtons)
            {
                PLCCounter++;

                if (PLCCounter > 20)
                {
                    Point point = GVisual.Point_Control_to_BottomSide_ofControl(PLC_Sim_Nesne_Buttons_Panel, PLC_Sim_Yerlestiriliyor_Panel, 25);
                    GVisual.ShowControl(PLC_Sim_Nesne_Buttons_Panel, PLC_Sim_Panel, point);
                    //PLC_Sim_Nesne_Buttons_Panel.Controls.Add(btn_PLC_Sim_Nesne_Yerlestirildi);
                    //PLC_Sim_Nesne_Buttons_Panel.Controls.Add(btn_PLC_Sim_Nesne_Yerlestirilemedi);
                    //btn_PLC_Sim_Nesne_Yerlestirildi.Show();
                    //btn_PLC_Sim_Nesne_Yerlestirilemedi.Show();
                    ShowNesneButtons = false;
                    PLCCounter = 0;
                }
            }
        }
        private void nesneTimer_Tick(object sender, EventArgs e)
        {
            if (BlinkingCell != null)
            {
                if (BlinkingCell.HoverPen.Color == System.Drawing.Color.Lime)
                {
                    BlinkingCell.HoverPen = new Pen(System.Drawing.Color.DarkGray);
                    BlinkingCell.HoverPen.DashStyle = DashStyle.DashDot;
                    DrawingPanel.Invalidate();
                }
                else
                {
                    BlinkingCell.HoverPen = new Pen(System.Drawing.Color.Lime, 3);
                    DrawingPanel.Invalidate();
                }
            }
        }
        


        //These are the Buttons that determines the state of the item placement for Simulation Purposes
        //(These are also works for Item Removal Simulation only their text changes between two simulation)
        private void btn_PLC_Sim_Nesne_Yerlestirildi_Click(object sender, EventArgs e)
        {
            if (NesneKaldır)
            {
                Models.Item deletedItem = new Models.Item();
                if (BlinkingCell != null)
                {
                    foreach (var item in BlinkingCell.items)
                    {
                        if (item.ItemEtiketi == item_etiketi)
                        {
                            deletedItem = item;
                        }
                    }
                    BlinkingCell.items.Remove(deletedItem);

                    using (var context = new DBContext())
                    {
                        var item1 = (from x in context.Items
                                     where x.ItemEtiketi == deletedItem.ItemEtiketi
                                     select x).FirstOrDefault();

                        if (item1 != null)
                        {
                            context.Items.Remove(item1);
                            context.SaveChanges();
                        }
                    }
                }
                if (ambar != null)
                {
                    foreach (var depo in ambar.depolar)
                    {
                        foreach (var cell in depo.gridmaps)
                        {
                            if (cell == BlinkingCell)
                            {
                                depo.currentRow = cell.Row;
                                depo.currentColumn = cell.Column;
                            }
                        }
                    }
                }
                NesneKaldır = false;
                EmptyPLCSimPanel();
                PLC_Timer.Stop();
                if (PLC_Sim_Panel.Visible && Nesne_Al_First_Panel.Visible)
                {
                    if (MoveRightEvent != null)
                    {
                        MoveRightEvent.Invoke(sender, e);
                        MoveRightEvent.Invoke(sender, e);
                    }
                }
                else if (PLC_Sim_Panel.Visible && !Nesne_Al_First_Panel.Visible)
                {
                    if (MoveRightEvent != null)
                    {
                        MoveRightEvent.Invoke(sender, e);
                    }
                }
                else
                {
                    if (MoveRightEvent != null)
                    {
                        MoveRightEvent.Invoke(sender, e);
                    }
                }
                GVisual.HideControl(Nesne_Al_First_Panel, this);
                GVisual.HideControl(PLC_Sim_Panel, this);
                MainPanelMakeBigger(DrawingPanel);
                BlinkingCell = null;
                DrawingPanel.Invalidate();
            }
            else
            {
                if (ambar != null)
                {
                    if (ambar.depolar.Count > 0)
                    {
                        Depo newDepo = GetDepotoPlaceItem(item_turu);

                        if (newDepo != null)
                        {
                            bool StageChanged = CheckifDepoStageChanged(newDepo);
                            bool isDepoEmpty = CheckifDepoisEmpty(newDepo);

                            if (StageChanged || isDepoEmpty)
                            {
                                //SetColumnStartPosition(newDepo);
                                //SetRowStartPosition(newDepo);
                            }
                            yerBul = false;
                            PlaceItem(newDepo);
                        }
                    }
                }
                PLC_Timer.Stop();
                if (PLC_Sim_Panel.Visible && Nesne_Yerlestirme_First_Panel.Visible)
                {
                    if (MoveRightEvent != null)
                    {
                        MoveRightEvent.Invoke(sender, e);
                        MoveRightEvent.Invoke(sender, e);
                    }
                }
                else if (PLC_Sim_Panel.Visible && !Nesne_Yerlestirme_First_Panel.Visible)
                {
                    if (MoveRightEvent != null)
                    {
                        MoveRightEvent.Invoke(sender, e);
                    }
                }
                else
                {
                    if (MoveRightEvent != null)
                    {
                        MoveRightEvent.Invoke(sender, e);
                        MoveRightEvent.Invoke(sender, e);
                    }
                }
                GVisual.HideControl(Nesne_Yerlestirme_First_Panel, this);
                GVisual.HideControl(PLC_Sim_Panel, this);
                EmptyPLCSimPanel();
                MainPanelMakeBigger(DrawingPanel);
                BlinkingCell = null;
                DrawingPanel.Invalidate();
            }
        }
        private void btn_PLC_Sim_Nesne_Yerlestirilemedi_Click(object sender, EventArgs e)
        {
            if (NesneKaldır)
            {
                MessageBox.Show("Nesne kaldırılırken bir sorunla karşılaşıldı.", "Nesne Kaldırılamadı.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                NesneKaldır = false;
                nesneTimer.Stop();
                PLC_Timer.Stop();
            }
            else
            {
                MessageBox.Show("Nesne yerleştirilirken bir sorunla karşılaşıldı.", "Nesne Yerleştirilemedi.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                nesneTimer.Stop();
                PLC_Timer.Stop();
            }
        }

        #endregion



        //PLC Simulation for Item Removal
        #region PLC Simulation for Removing Items

        //This is Simulation Panel Button Events
        //This is for Starting Item Removal Simulation
        private void btn_PLC_Sim_Nesne_Bul_Click(object sender, EventArgs e)
        {
            if (!CheckPanelVisible(PLC_Sim_Panel))
            {
                GVisual.ShowControl(Nesne_Al_First_Panel, this, leftSidePanelLocation);
                MainPanelMakeSmallest(DrawingPanel);
                GVisual.HideControl(Nesne_Al_Second_Panel, Nesne_Al_First_Panel);
                if (MoveLeftEvent != null)
                {
                    MoveLeftEvent.Invoke(sender, e);
                }
            }
            else
            {
                HideEverything();
                Show_ItemPlacementPanel();
            }
            Point point = GVisual.Point_Control_to_BottomSide_ofControl(PLC_Sim_YerSoyle_Panel, btn_PLC_Sim_Nesne_Bul, 5);
            GVisual.ShowControl(PLC_Sim_YerSoyle_Panel, PLC_Sim_Panel, point);
            lbl_YerSoyle.Text = "Nesne Aranıyor...";
            lbl_YerSoyle.StateCommon.TextColor = System.Drawing.Color.Red;
            lbl_Yerlestiriliyor.StateCommon.TextColor = System.Drawing.Color.Red;
            PLC_Timer.Interval = 250;
            PLC_Timer.Start();
            DrawingPanel.Invalidate();
        }


        //These are Item Removal Panel Button Events
        //This button is for finding Items to Remove from depo
        private void btn_Nesne_Bul_Click(object sender, EventArgs e)
        {
            item_etiketi = txt_Nesne_Al_Etiket.Text;
            bool nesne_bulunmuyor = true;

            if (ambar != null)
            {
                foreach (var depo in ambar.depolar)
                {
                    foreach (var cell in depo.gridmaps)
                    {
                        if (cell.items.Count > 0)
                        {
                            nesne_bulunmuyor = false;

                            foreach (var item in cell.items)
                            {
                                if (item.ItemEtiketi == item_etiketi)
                                {
                                    if (item == cell.items.Last())
                                    {
                                        BlinkingCell = cell;
                                        lbl_Nesne_Al_Tur_Kodu_Value.Text = $"{depo.ItemTuru}";
                                        lbl_Nesne_Al_Aciklama_Value.Text = $"{item.ItemAciklamasi}";
                                        lbl_Nesne_Al_Agirlik_Value.Text = $"{item.ItemAgirligi}";
                                        lbl_Nesne_Al_Nesne_X_Value.Text = $"{item.Cm_X_Axis} cm";
                                        lbl_Nesne_Al_Nesne_Y_Value.Text = $"{item.Cm_Y_Axis} cm";
                                        lbl_Nesne_Al_Nesne_Z_Value.Text = $"{item.Cm_Z_Axis} cm";
                                        nesneTimer.Start();
                                        Point point = GVisual.Point_Control_to_BottomSide_ofControl(Nesne_Al_Second_Panel, BorderEdge_NesneAl, 5);
                                        GVisual.ShowControl(Nesne_Al_Second_Panel, Nesne_Al_First_Panel, point);
                                        if (!PLC_Sim_YerSoyle_Panel.Visible)
                                        {
                                            Point point1 = GVisual.Point_Control_to_BottomSide_ofControl(PLC_Sim_YerSoyle_Panel, btn_PLC_Sim_Nesne_Bul, 5);
                                            GVisual.ShowControl(PLC_Sim_YerSoyle_Panel, PLC_Sim_Panel, point1);
                                            PLC_Timer.Start();
                                        }
                                        break;
                                    }
                                    else
                                    {
                                        MessageBox.Show("Nesne bulunduğu hücrede en yukarıda değil, lütfen önce üzerindeki nesneleri alın.", "Nesnenin üzeri dolu.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (nesne_bulunmuyor)
            {
                ShowNotification("Layout içinde kaldırılacak nesne bulunmuyor.", CustomNotifyIcon.enmType.Warning);
            }
        }
        //This is for Canceling finding items to remove from depo
        private void btn_Nesne_Bul_Vazgec_Click(object sender, EventArgs e)
        {
            NesneKaldır = false;
            nesneTimer.Stop();
            PLC_Timer.Stop();
            if (PLC_Sim_Panel.Visible)
            {
                GVisual.HideControl(Nesne_Al_First_Panel, this);
                MainPanelMakeSmaller1(DrawingPanel);
                if (MoveRightEvent != null)
                {
                    MoveRightEvent.Invoke(sender, e);
                }
            }
            else if (!PLC_Sim_Panel.Visible)
            {
                GVisual.HideControl(Nesne_Al_First_Panel, this);
                MainPanelMakeBigger(DrawingPanel);
                if (MoveRightEvent != null)
                {
                    MoveRightEvent.Invoke(sender, e);
                }
            }
            else
            {
                GVisual.HideControl(Nesne_Al_First_Panel, this);
                MainPanelMakeBigger(DrawingPanel);
                if (MoveRightEvent != null)
                {
                    MoveRightEvent(sender, e);
                }
            }
            DrawingPanel.Invalidate();
        }
        //This is for removing items from the depo
        private void btn_Nesne_Kaldır_Click(object sender, EventArgs e)
        {
            Point point = GVisual.Point_Control_to_BottomSide_ofControl(PLC_Sim_Yerlestiriliyor_Panel, PLC_Sim_YerSoyle_Panel, 5);
            GVisual.ShowControl(PLC_Sim_Yerlestiriliyor_Panel, PLC_Sim_Panel, point);
            btn_PLC_Sim_Nesne_Yerlestirildi.Text = "Nesne\nGötürüldü";
            btn_PLC_Sim_Nesne_Yerlestirilemedi.Text = "Nesne\nGötürülemedi";
            lbl_Yerlestiriliyor.Text = "Nesne Götürülüyor...";
            lbl_YerSoyle.StateCommon.TextColor = System.Drawing.Color.MidnightBlue;
            ShowNesneButtons = true;
            NesneKaldır = true;
        }
        //This is for canceling removing items from the depo
        private void btn_Nesne_Kaldır_Vazgec_Click(object sender, EventArgs e)
        {
            NesneKaldır = false;
            nesneTimer.Stop();
            PLC_Timer.Stop();
            GVisual.HideControl(Nesne_Al_Second_Panel, Nesne_Al_First_Panel);
            EmptyPLCSimPanel();
        }
        //This is for Closing Item Removal Panel
        private void btn_Nesne_Al_First_Panel_Kapat_Click(object sender, EventArgs e)
        {
            NesneKaldır = false;
            nesneTimer.Stop();
            PLC_Timer.Stop();
            EmptyPLCSimPanel();
            if (PLC_Sim_Panel.Visible)
            {
                GVisual.HideControl(Nesne_Al_First_Panel, this);
                MainPanelMakeSmaller1(DrawingPanel);
                if (MoveRightEvent != null)
                {
                    MoveRightEvent.Invoke(sender, e);
                }
            }
            else if (!PLC_Sim_Panel.Visible)
            {
                GVisual.HideControl(Nesne_Al_First_Panel, this);
                MainPanelMakeBigger(DrawingPanel);
                if (MoveRightEvent != null)
                {
                    MoveRightEvent.Invoke(sender, e);
                }
            }
            else
            {
                GVisual.HideControl(Nesne_Al_First_Panel, this);
                MainPanelMakeBigger(DrawingPanel);
                if (MoveRightEvent != null)
                {
                    MoveRightEvent(sender, e);
                }
            }
            DrawingPanel.Invalidate();
        }

        #endregion



        //Database Operations
        #region Database Operations

        //Save to Database When Closing Form
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ambar != null)
            {
                Zoomlevel = 1f;
                ambar.ApplyZoom(Zoomlevel);

                if ((Nesne_Yerlestirme_First_Panel.Visible || Nesne_Al_First_Panel.Visible) && !PLC_Sim_Panel.Visible)
                {
                    HideEverything();
                    MainPanelMakeBigger(DrawingPanel);
                    if (ItemPlacementCancel != null)
                    {
                        ItemPlacementCancel.Invoke(sender, e);
                    }
                }
                else if (PLC_Sim_Panel.Visible && !Nesne_Yerlestirme_First_Panel.Visible && !Nesne_Al_First_Panel.Visible)
                {
                    GVisual.HideControl(PLC_Sim_Panel, this);
                    MainPanelMakeBigger(DrawingPanel);
                    if (MoveRightEvent != null)
                    {
                        MoveRightEvent.Invoke(sender, e);
                    }
                }
                else if (PLC_Sim_Panel.Visible && (Nesne_Yerlestirme_First_Panel.Visible || Nesne_Al_First_Panel.Visible))
                {
                    HideEverything();
                    MainPanelMakeBigger(DrawingPanel);
                    if (MoveRightEvent != null)
                    {
                        MoveRightEvent.Invoke(sender, e);
                        MoveRightEvent.Invoke(sender, e);
                    }
                }

                foreach (var depo in ambar.depolar)
                {
                    using (var context = new DBContext())
                    {
                        var notlastClosedLayouts = (from x in context.Layout
                                                    where x.LayoutId != ambar.LayoutId
                                                    select x).ToList();

                        foreach (var layout in notlastClosedLayouts)
                        {
                            layout.LastClosedLayout = 0;
                            context.SaveChanges();
                        }

                        var lastClosedLayout = (from x in context.Layout
                                                where x.LayoutId == ambar.LayoutId
                                                select x).FirstOrDefault();

                        if (lastClosedLayout != null)
                        {
                            lastClosedLayout.LastClosedLayout = 1;
                            context.SaveChanges();
                        }
                    }
                    using (var context = new DBContext())
                    {
                        var depo1 = (from x in context.Depos
                                     where x.DepoId == depo.DepoId
                                     select x).FirstOrDefault();

                        if (depo1 != null)
                        {
                            depo1.currentColumn = depo.currentColumn;
                            depo1.currentRow = depo.currentRow;
                            context.SaveChanges();
                        }
                    }
                    foreach (var cell in depo.gridmaps)
                    {
                        using (var context = new DBContext())
                        {
                            var cell1 = (from x in context.Cells
                                         where x.CellId == cell.CellId
                                         select x).FirstOrDefault();

                            if (cell1 != null)
                            {
                                cell1.toplam_Nesne_Yuksekligi = cell.toplam_Nesne_Yuksekligi;
                                context.SaveChanges();
                            }
                        }
                        foreach (var item in cell.items)
                        {
                            if (item.ItemId == 0)
                            {
                                Models.Item Dbitem = new Models.Item(cell.CellId, item.ItemEtiketi,
                                    item.ItemTuru, item.ItemEni, item.ItemBoyu, item.ItemYuksekligi,
                                    item.Rectangle.X, item.Rectangle.Y, item.Rectangle.Width, item.Rectangle.Height,
                                    item.OriginalRectangle.X, item.OriginalRectangle.Y, item.OriginalRectangle.Width,
                                    item.OriginalRectangle.Height, item.Zoomlevel, item.ItemAgirligi, item.ItemAciklamasi,
                                    item.Cm_X_Axis, item.Cm_Y_Axis, item.Cm_Z_Axis);

                                using (var context = new DBContext())
                                {
                                    context.Items.Add(Dbitem);
                                    context.SaveChanges();
                                }
                            }
                            else
                            {
                                using (var context = new DBContext())
                                {
                                    var item1 = (from x in context.Items
                                                 where x.ItemId == item.ItemId
                                                 select x).FirstOrDefault();

                                    if (item1 != null)
                                    {
                                        Models.Item Dbitem = new Models.Item(cell.CellId, item.ItemEtiketi,
                                           item.ItemTuru, item.ItemEni, item.ItemBoyu, item.ItemYuksekligi,
                                           item.Rectangle.X, item.Rectangle.Y, item.KareEni, item.KareBoyu,
                                           item.OriginalRectangle.X, item.OriginalRectangle.Y, item.OriginalRectangle.Width,
                                           item.OriginalRectangle.Height, item.Zoomlevel, item.ItemAgirligi, item.ItemAciklamasi,
                                           item.Cm_X_Axis, item.Cm_Y_Axis, item.Cm_Z_Axis);

                                        item1.KareX = item.Rectangle.X;
                                        item1.KareY = item.Rectangle.Y;
                                        //item1.KareEni = item.KareEni;
                                        //item1.KareBoyu = item.KareBoyu;
                                        item1.OriginalKareX = item.OriginalRectangle.X;
                                        item1.OriginalKareY = item.OriginalRectangle.Y;
                                        //item1.OriginalKareEni = item.OriginalKareEni;
                                        //item1.OriginalKareBoyu = item.OriginalKareBoyu;
                                        context.SaveChanges();
                                    }
                                }
                            }

                            using (var context = new DBContext())
                            {
                                foreach (var reff in item.ItemReferencePoints)
                                {

                                    Models.ItemReferencePoint DBreff = new Models.ItemReferencePoint(item.ItemId, reff.Rectangle.X, reff.Rectangle.Y,
                                        reff.Rectangle.Width, reff.Rectangle.Height, reff.OriginalRectangle.X, reff.OriginalRectangle.Y,
                                        reff.OriginalRectangle.Width, reff.OriginalRectangle.Height, reff.Zoomlevel, reff.Pointsize);

                                    context.ItemReferencePoints.Add(DBreff);
                                    context.SaveChanges();
                                    reff.ReferenceId = DBreff.ReferenceId;
                                }
                            }
                        }
                    }
                }
            }
        }
        //Load Last Closed Layout when opening the Program
        public void LoadFromDB()
        {
            using (var context = new DBContext())
            {
                var layout = (from x in context.Layout
                              where x.LastClosedLayout == 1
                              select x).FirstOrDefault();

                if (layout != null)
                {
                    var Dbambar = (from x in context.Ambars
                                   where x.LayoutId == layout.LayoutId
                                   select x).FirstOrDefault();

                    if (Dbambar != null)
                    {
                        Ambar loadedAmbar = new Ambar(Dbambar.KareX, Dbambar.KareY,
                            Dbambar.KareEni, Dbambar.KareBoyu, this, null);

                        loadedAmbar.AmbarId = Dbambar.AmbarId;
                        loadedAmbar.AmbarName = Dbambar.AmbarName;
                        loadedAmbar.AmbarDescription = Dbambar.AmbarDescription;
                        loadedAmbar.AmbarEni = Dbambar.AmbarEni;
                        loadedAmbar.AmbarBoyu = Dbambar.AmbarBoyu;
                        loadedAmbar.KareX = Dbambar.KareX;
                        loadedAmbar.KareY = Dbambar.KareY;
                        loadedAmbar.KareEni = Dbambar.KareEni;
                        loadedAmbar.KareBoyu = Dbambar.KareBoyu;
                        loadedAmbar.OriginalKareX = Dbambar.OriginalKareX;
                        loadedAmbar.OriginalKareY = Dbambar.OriginalKareY;
                        loadedAmbar.OriginalKareEni = Dbambar.OriginalKareEni;
                        loadedAmbar.OriginalKareBoyu = Dbambar.OriginalKareBoyu;
                        loadedAmbar.Zoomlevel = Zoomlevel;
                        ambar = loadedAmbar;
                        var depos = (from x in context.Depos
                                     where x.AmbarId == Dbambar.AmbarId
                                     select x).ToList();

                        foreach (var depo in depos)
                        {
                            Depo newDepo = new Depo(depo.KareX, depo.KareY,
                                depo.KareEni, depo.KareBoyu, Zoomlevel, this,
                                null, loadedAmbar);

                            newDepo.DepoId = depo.DepoId;
                            newDepo.Yerlestirilme_Sirasi = depo.Yerlestirilme_Sirasi;
                            newDepo.asama1_Yuksekligi = depo.asama1_Yuksekligi;
                            newDepo.asama2_Yuksekligi = depo.asama2_Yuksekligi;
                            newDepo.itemDrop_StartLocation = depo.itemDrop_StartLocation;
                            newDepo.itemDrop_UpDown = depo.itemDrop_UpDown;
                            newDepo.itemDrop_LeftRight = depo.itemDrop_LeftRight;
                            newDepo.ColumnCount = depo.ColumnCount;
                            newDepo.RowCount = depo.RowCount;
                            newDepo.Cm_Width = depo.Depo_Alani_Eni_Cm;
                            newDepo.Cm_Height = depo.Depo_Alani_Boyu_Cm;

                            newDepo.AmbarId = depo.AmbarId;
                            newDepo.DepoName = depo.DepoName;
                            newDepo.DepoDescription = depo.DepoDescription;
                            newDepo.DepoAlaniYuksekligi = depo.DepoAlaniYuksekligi;
                            newDepo.DepoAlaniEni = depo.DepoAlaniEni;
                            newDepo.DepoAlaniBoyu = depo.DepoAlaniBoyu;
                            newDepo.currentStage = depo.currentStage;
                            newDepo.asama1_ItemSayisi = depo.asama1_ItemSayisi;
                            newDepo.asama2_ToplamItemSayisi = depo.asama2_ToplamItemSayisi;
                            newDepo.ItemTuru = depo.ItemTuru;

                            newDepo.nesneEni = depo.nesneEni;
                            newDepo.nesneBoyu = depo.nesneBoyu;
                            newDepo.nesneYuksekligi = depo.nesneYuksekligi;
                            newDepo.KareX = depo.KareX;
                            newDepo.KareY = depo.KareY;
                            newDepo.KareEni = depo.KareEni;
                            newDepo.KareBoyu = depo.KareBoyu;
                            newDepo.OriginalKareX = depo.OriginalKareX;
                            newDepo.OriginalKareY = depo.OriginalKareY;
                            newDepo.OriginalKareEni = depo.OriginalKareEni;
                            newDepo.OriginalKareBoyu = depo.OriginalKareBoyu;
                            newDepo.currentColumn = depo.currentColumn;
                            newDepo.currentRow = depo.currentRow;

                            loadedAmbar.depolar.Add(newDepo);

                            var cells = (from x in context.Cells
                                         where x.DepoId == newDepo.DepoId
                                         select x).ToList();

                            foreach (var cell in cells)
                            {
                                Balya_Yerleştirme.Models.Cell newCell =
                                    new Balya_Yerleştirme.Models.Cell(cell.KareX, cell.KareY, cell.KareEni,
                                    cell.KareBoyu, this, newDepo, null);

                                newCell.Column = cell.Column;
                                newCell.Row = cell.Row;
                                newCell.CellId = cell.CellId;
                                newCell.DepoId = newDepo.DepoId;
                                newCell.CellYuksekligi = cell.CellYuksekligi;
                                newCell.CellEni = cell.CellEni;
                                newCell.CellBoyu = cell.CellBoyu;
                                newCell.CellEtiketi = cell.CellEtiketi;
                                newCell.NesneEni = cell.NesneEni;
                                newCell.NesneBoyu = cell.NesneBoyu;
                                newCell.NesneYuksekligi = cell.NesneYuksekligi;
                                newDepo.nesneYuksekligi = cell.NesneYuksekligi;

                                newCell.CellMalSayisi = cell.CellMalSayisi;
                                newCell.ItemSayisi = cell.ItemSayisi;
                                newCell.DikeyKenarBoslugu = cell.DikeyKenarBoslugu;
                                newCell.YatayKenarBoslugu = cell.YatayKenarBoslugu;

                                newCell.KareX = cell.KareX;
                                newCell.KareY = cell.KareY;
                                newCell.KareEni = cell.KareEni;
                                newCell.KareBoyu = cell.KareBoyu;
                                newCell.OriginalKareX = cell.OriginalKareX;
                                newCell.OriginalKareY = cell.OriginalKareY;
                                newCell.OriginalKareEni = cell.OriginalKareEni;
                                newCell.OriginalKareBoyu = cell.OriginalKareBoyu;
                                newCell.Zoomlevel = Zoomlevel;
                                newCell.toplam_Nesne_Yuksekligi = cell.toplam_Nesne_Yuksekligi;
                                newCell.cell_Cm_X = cell.cell_Cm_X;
                                newCell.cell_Cm_Y = cell.cell_Cm_Y;

                                newDepo.gridmaps.Add(newCell);

                                var items = (from x in context.Items
                                             where x.CellId == newCell.CellId
                                             select x).ToList();

                                foreach (var item in items)
                                {
                                    Models.Item newItem = new Models.Item(item.OriginalKareX, item.OriginalKareY,
                                        item.OriginalKareEni, item.OriginalKareBoyu, item.Zoomlevel, this);

                                    newItem.ItemId = item.ItemId;
                                    newItem.CellId = item.CellId;

                                    newItem.OriginalRectangle = newItem.Rectangle;

                                    newItem.KareX = item.KareX;
                                    newItem.KareY = item.KareY;
                                    newItem.KareEni = item.KareEni;
                                    newItem.KareBoyu = item.KareBoyu;
                                    newItem.OriginalKareX = item.OriginalKareX;
                                    newItem.OriginalKareY = item.OriginalKareY;
                                    newItem.OriginalKareEni = item.OriginalKareEni;
                                    newItem.OriginalKareBoyu = item.OriginalKareBoyu;
                                    newItem.ItemYuksekligi = item.ItemYuksekligi;
                                    newItem.ItemEni = item.ItemEni;
                                    newItem.ItemBoyu = item.ItemBoyu;
                                    newItem.ItemAciklamasi = item.ItemAciklamasi;
                                    newItem.ItemEtiketi = item.ItemEtiketi;
                                    newItem.ItemTuru = item.ItemTuru;
                                    newItem.ItemAgirligi = item.ItemAgirligi;
                                    newItem.Cm_Z_Axis = item.Cm_Z_Axis;
                                    newItem.Cm_X_Axis = item.Cm_X_Axis;
                                    newItem.Cm_Y_Axis = item.Cm_Y_Axis;

                                    newCell.items.Add(newItem);
                                }
                            }
                        }

                        var conveyors = (from x in context.Conveyors
                                         where x.AmbarId == ambar.AmbarId
                                         select x).ToList();

                        foreach (var conveyor in conveyors)
                        {
                            Conveyor newConveyor = new Conveyor(conveyor.KareX,
                                conveyor.KareY, conveyor.KareEni, conveyor.KareBoyu,
                                this, null, loadedAmbar);

                            newConveyor.ConveyorId = conveyor.ConveyorId;
                            newConveyor.AmbarId = loadedAmbar.AmbarId;
                            newConveyor.ConveyorEni = conveyor.ConveyorEni;
                            newConveyor.ConveyorBoyu = conveyor.ConveyorBoyu;
                            newConveyor.KareX = conveyor.KareX;
                            newConveyor.KareY = conveyor.KareY;
                            newConveyor.KareEni = conveyor.KareEni;
                            newConveyor.KareBoyu = conveyor.KareBoyu;
                            newConveyor.OriginalKareX = conveyor.OriginalKareX;
                            newConveyor.OriginalKareY = conveyor.OriginalKareY;
                            newConveyor.OriginalKareEni = conveyor.OriginalKareEni;
                            newConveyor.OriginalKareBoyu = conveyor.OriginalKareBoyu;

                            loadedAmbar.conveyors.Add(newConveyor);

                            var reffPoints = (from x in context.ConveyorReferencePoints
                                              where x.ConveyorId == newConveyor.ConveyorId
                                              select x).ToList();

                            foreach (var reff in reffPoints)
                            {
                                ConveyorReferencePoint point = new ConveyorReferencePoint(reff.KareX,
                                    reff.KareY, reff.KareEni, reff.KareBoyu, reff.Zoomlevel, this,
                                    newConveyor, null);

                                point.ReferenceId = reff.ReferenceId;
                                point.KareX = reff.KareX;
                                point.KareY = reff.KareY;
                                point.KareEni = reff.KareEni;
                                point.KareBoyu = reff.KareBoyu;
                                point.OriginalKareX = reff.OriginalKareX;
                                point.OriginalKareY = reff.OriginalKareY;
                                point.OriginalKareEni = reff.OriginalKareEni;
                                point.OriginalKareBoyu = reff.OriginalKareBoyu;
                                point.ParentConveyor = newConveyor;
                                point.Pointsize = 4;
                                point.AmbarId = ambar.AmbarId;

                                newConveyor.ConveyorReferencePoints.Add(point);
                            }
                        }
                    }
                }
                DrawingPanel.Invalidate();
            }
        }

        #endregion










































        //Methods After This are the ERRORLESS Methods



        //Save Current Items in the whole Layout to Excel Sheet
        #region Save items to Excel
        //Save to Excel Methods
        private void toolStripBTN_ExportToExcel_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                // Set filters to include Excel files, text files, and all files
                saveFileDialog.Filter =
                    "Excel Files (*.xlsx)|*.xlsx|Excel 97-2003 Files (*.xls)|*.xls|Text files (*.txt)|*.txt|All files (*.*)|*.*";

                saveFileDialog.Title = "Save a File";
                saveFileDialog.DefaultExt = "xlsx"; // Set default file extension to .xlsx
                saveFileDialog.AddExtension = true; // Automatically add the extension

                // Show the dialog and check if the user clicked "Save"
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Get the selected file path
                    string filePath = saveFileDialog.FileName;
                    string fileName = Path.GetFileName(filePath);

                    WriteStateToExcel(filePath, fileName);
                }
            }
            if (Nesne_Yerlestirme_First_Panel.Visible)
            {
                ExportToExcel.Invoke(sender, e);
                MainPanelMakeBigger(DrawingPanel);
                Hide_ItemPlacementPanel();
                Hide_infopanel();
            }
        }
        public bool IsFileOpen(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return false;
            }
            try
            {
                using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    stream.Close();
                }
            }
            catch (IOException ex)
            {
                return true;
            }
            catch (UnauthorizedAccessException ex)
            {
                return true;
            }
            catch (Exception ex)
            {
                return true;
            }
            return false;
        }
        public void WriteStateToExcel(string filePath, string ExcelWorkBookName)
        {
            if (IsFileOpen(filePath))
            {
                MessageBox.Show($"Dosya '{ExcelWorkBookName}' kullanımda. Lütfen kapatıp tekrar deneyin.",
                    "Dosya Kullanımda", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(ExcelWorkBookName);

            worksheet.Column(1).Width = 20; // Width for "Balya Etiketi"
            worksheet.Column(2).Width = 25; // Width for "X Ekseni"
            worksheet.Column(3).Width = 25; // Width for "Y Ekseni"
            worksheet.Column(4).Width = 30; // Width for "Z Ekseni"
            worksheet.Column(5).Width = 30; // Width for "Ağırlığı"
            worksheet.Column(6).Width = 30; // Width for "Bulunduğu Depo"
            worksheet.Column(7).Width = 30; // Width for "Bulunduğu Hücre"

            worksheet.Range("B1:D1").Merge().Value = "Malın Eksenlerdeki Konumu";
            worksheet.Range("F1:G1").Merge().Value = "Bulunduğu Yer";

            // Row 1 Headers
            worksheet.Cell(1, 1).Value = "Mal";
            worksheet.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.LightGreen;
            worksheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            worksheet.Cell(1, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
            worksheet.Cell(1, 1).Style.Font.FontSize = 18;
            worksheet.Cell(1, 1).Style.Font.FontColor = XLColor.Red;
            worksheet.Cell(1, 1).Style.Font.Bold = true;

            // Row 1 Headers
            worksheet.Cell(1, 2).Value = "Malın Eksenlerdeki Konumu";
            worksheet.Cell(1, 2).Style.Fill.BackgroundColor = XLColor.LightGray;
            worksheet.Cell(1, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            worksheet.Cell(1, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
            worksheet.Cell(1, 2).Style.Font.FontSize = 18;
            worksheet.Cell(1, 2).Style.Font.FontColor = XLColor.Red;
            worksheet.Cell(1, 2).Style.Font.Bold = true;

            worksheet.Cell(1, 5).Value = "Malın Ağırlığı";
            worksheet.Cell(1, 5).Style.Fill.BackgroundColor = XLColor.LightCyan;
            worksheet.Cell(1, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            worksheet.Cell(1, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
            worksheet.Cell(1, 5).Style.Font.FontSize = 18;
            worksheet.Cell(1, 5).Style.Font.FontColor = XLColor.Red;
            worksheet.Cell(1, 5).Style.Font.Bold = true;

            worksheet.Cell(1, 6).Value = "Bulunduğu Yer";
            worksheet.Cell(1, 6).Style.Fill.BackgroundColor = XLColor.LightBlue;
            worksheet.Cell(1, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            worksheet.Cell(1, 6).Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
            worksheet.Cell(1, 6).Style.Font.FontSize = 18;
            worksheet.Cell(1, 6).Style.Font.FontColor = XLColor.Red;
            worksheet.Cell(1, 6).Style.Font.Bold = true;

            // Row 2 Headers
            worksheet.Cell(2, 1).Value = "Mal Etiketi";
            worksheet.Cell(2, 1).Style.Fill.BackgroundColor = XLColor.LightGreen;

            worksheet.Cell(2, 2).Value = "X Ekseni";
            worksheet.Cell(2, 2).Style.Fill.BackgroundColor = XLColor.LightGray;
            worksheet.Cell(2, 3).Value = "Y Ekseni";
            worksheet.Cell(2, 3).Style.Fill.BackgroundColor = XLColor.LightGray;
            worksheet.Cell(2, 4).Value = "Z Ekseni";
            worksheet.Cell(2, 4).Style.Fill.BackgroundColor = XLColor.LightGray;

            worksheet.Cell(2, 5).Value = "Ağırlık";
            worksheet.Cell(2, 5).Style.Fill.BackgroundColor = XLColor.LightCyan;

            worksheet.Cell(2, 6).Value = "Depo Adı";
            worksheet.Cell(2, 6).Style.Fill.BackgroundColor = XLColor.LightBlue;

            worksheet.Cell(2, 7).Value = "Hücre Etiketi";
            worksheet.Cell(2, 7).Style.Fill.BackgroundColor = XLColor.LightBlue;

            for (int col = 1; col <= 7; col++)
            {
                worksheet.Cell(1, col).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell(1, col).Style.Border.OutsideBorder = XLBorderStyleValues.Medium;

                worksheet.Cell(2, col).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell(2, col).Style.Border.OutsideBorder = XLBorderStyleValues.Medium;

                worksheet.Cell(2, col).Style.Font.FontSize = 14;
                worksheet.Cell(2, col).Style.Font.Bold = true;
                worksheet.Cell(2, col).Style.Font.FontColor = XLColor.Blue;
            }

            int rowIndex = 3;
            int cellIndex = 1;
            foreach (var depo in ambar.depolar)
            {
                foreach (var cell in depo.gridmaps)
                {
                    foreach (var item in cell.items)
                    {
                        worksheet.Cell(rowIndex, cellIndex).Value = item.ItemEtiketi;
                        worksheet.Cell(rowIndex, cellIndex).Style.Fill.BackgroundColor = XLColor.LightGreen;
                        cellIndex++;

                        worksheet.Cell(rowIndex, cellIndex).Value = $"{item.Cm_X_Axis} cm";
                        worksheet.Cell(rowIndex, cellIndex).Style.Fill.BackgroundColor = XLColor.LightGray;
                        cellIndex++;

                        worksheet.Cell(rowIndex, cellIndex).Value = $"{item.Cm_Y_Axis} cm";
                        worksheet.Cell(rowIndex, cellIndex).Style.Fill.BackgroundColor = XLColor.LightGray;
                        cellIndex++;

                        worksheet.Cell(rowIndex, cellIndex).Value = $"{item.Cm_Z_Axis} cm";
                        worksheet.Cell(rowIndex, cellIndex).Style.Fill.BackgroundColor = XLColor.LightGray;
                        cellIndex++;

                        worksheet.Cell(rowIndex, cellIndex).Value = $"{item.ItemAgirligi} kg";
                        worksheet.Cell(rowIndex, cellIndex).Style.Fill.BackgroundColor = XLColor.LightCyan;
                        cellIndex++;

                        worksheet.Cell(rowIndex, cellIndex).Value = $"{depo.DepoName}";
                        worksheet.Cell(rowIndex, cellIndex).Style.Fill.BackgroundColor = XLColor.LightBlue;
                        cellIndex++;

                        if (item.CellId == cell.CellId)
                        {
                            worksheet.Cell(rowIndex, cellIndex).Value = cell.CellEtiketi;
                            worksheet.Cell(rowIndex, cellIndex).Style.Fill.BackgroundColor = XLColor.LightBlue;
                        }
                        rowIndex++;
                        cellIndex = 1;
                    }
                }
            }

            for (int i = 3; i <= rowIndex; i++)
            {
                for (int j = 1; j <= 7; j++)
                {
                    worksheet.Cell(i, j).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(i, j).Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                    worksheet.Cell(i, j).Style.Font.FontSize = 14;
                    worksheet.Cell(i, j).Style.Font.FontColor = XLColor.Black;
                }
            }

            workbook.SaveAs(filePath);
        }
        #endregion



        //UI Operations
        #region Hide and Show
        private void HideEverything()
        {
            GVisual.HideControl(Nesne_Yerlestirme_First_Panel, this);
            GVisual.HideControl(Nesne_Yerlestirme_Second_Panel, Nesne_Yerlestirme_First_Panel);
            GVisual.HideControl(PLC_DB_AdressPanel, this);
            GVisual.HideControl(ProgressBarPanel, this);
            GVisual.HideControl(Nesne_Al_First_Panel, this);
            GVisual.HideControl(PLC_Sim_Panel, this);
            GVisual.HideControl(PLC_Sim_Nesne_Buttons_Panel, PLC_Sim_Panel);
            GVisual.HideControl(PLC_Sim_YerSoyle_Panel, PLC_Sim_Panel);
            GVisual.HideControl(PLC_Sim_Yerlestiriliyor_Panel, PLC_Sim_Panel);
        }
        private void EmptyPLCSimPanel()
        {
            GVisual.HideControl(PLC_Sim_Nesne_Buttons_Panel, PLC_Sim_Panel);
            GVisual.HideControl(PLC_Sim_YerSoyle_Panel, PLC_Sim_Panel);
            GVisual.HideControl(PLC_Sim_Yerlestiriliyor_Panel, PLC_Sim_Panel);
        }
        public void Hide_ItemPlacementPanel()
        {
            Nesne_Yerlestirme_First_Panel.Enabled = false;
            Nesne_Yerlestirme_First_Panel.Visible = false;
            Nesne_Yerlestirme_First_Panel.Hide();
            this.Controls.Remove(Nesne_Yerlestirme_First_Panel);
        }
        public void Show_ItemPlacementPanel()
        {
            Nesne_Yerlestirme_First_Panel.Enabled = true;
            Nesne_Yerlestirme_First_Panel.Visible = true;
            Nesne_Yerlestirme_First_Panel.Show();
            Nesne_Yerlestirme_First_Panel.Location = new Point(12, 88);
            this.Controls.Add(Nesne_Yerlestirme_First_Panel);
            if (Move)
            {
                ItemPlacementToolStripButtonClicked.Invoke(this, EventArgs.Empty);
                DrawingPanel.Invalidate();
                Move = false;
            }
        }
        #endregion
        #region Show Notification
        public void ShowNotification(string message, CustomNotification.CustomNotifyIcon.enmType enmType)
        {
            CustomNotification.CustomNotifyIcon custom = new CustomNotification.CustomNotifyIcon();
            custom.showAlert(message, enmType);
        }

        #endregion
        #region Hide and Show Info Panel
        public void Hide_infopanel()
        {
            infopanel.Enabled = false;
            infopanel.Visible = false;
            infopanel.Hide();
            DrawingPanel.Controls.Remove(infopanel);
        }
        public void Show_infopanel
            (string balya_etiketi, float balya_agirligi, string cell_etiketi, string balya_aciklamasi, string depo_adi)
        {
            infopanel.Enabled = true;
            infopanel.Visible = true;
            infopanel.Show();
            DrawingPanel.Controls.Add(infopanel);
            lbl_Info_Balya_Etiketi_Value.Text = $"{balya_etiketi}";
            lbl_Info_Balya_Aciklamasi_Value.Text = $"{balya_aciklamasi}";
            lbl_Info_Balya_Agirligi_Value.Text = $"{balya_agirligi} kg";
            lbl_Info_Hucre_Etiketi_Value.Text = $"{cell_etiketi}";
            txt_Info_Depo_Value.Text = $"{depo_adi}";
        }
        #endregion
        #region Check if panel is visible
        public bool CheckPanelVisible(Panel panel)
        {
            bool isVisible = false;
            foreach (var panel1 in this.Controls)
            {
                if (panel1 is Panel)
                {
                    Panel newPanel = (Panel)panel1;

                    if (newPanel != DrawingPanel &&
                        newPanel != PLC_Connection_Panel &&
                        newPanel != infopanel &&
                        newPanel != panel)
                    {
                        if (newPanel.Visible)
                        {
                            isVisible = true;
                        }
                    }
                }
            }
            if (isVisible)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion



        //Testing Purposes
        #region For myself to test the program
        //Add Item to Depos but this is for myself to test the algoritm
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (ambar != null)
            {
                if (ambar.depolar.Count > 0)
                {
                    Depo newDepo = GetDepotoPlaceItem("depo");

                    if (newDepo != null)
                    {
                        bool StageChanged = CheckifDepoStageChanged(newDepo);
                        bool isDepoEmpty = CheckifDepoisEmpty(newDepo);

                        if (StageChanged || isDepoEmpty)
                        {
                            //SetColumnStartPosition(newDepo);
                            //SetRowStartPosition(newDepo);
                        }
                        yerBul = false;
                        PlaceItem(newDepo);
                    }
                }
            }
            DrawingPanel.Invalidate();
        }
        //Remove Item from depos but this is for myself to test the algoritm
        private void balyayıAlToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Models.Item deleteItem = new Models.Item();
            foreach (var item in RightClickCell.items)
            {
                if (item == RightClickCell.items.Last())
                {
                    deleteItem = item;
                }
            }
            selectedDepo.currentRow = RightClickCell.Row;
            selectedDepo.currentColumn = RightClickCell.Column;
            RightClickCell.items.Remove(deleteItem);
            DrawingPanel.Invalidate();
        }
        #endregion



        //Show Cell Tag and Display all items in the depo
        #region Useful Info Methods For Users To See
        //Show Cell Tag and Hide Item Count in the Cell
        private void toolStripButtonShowCellTag_Click(object sender, EventArgs e)
        {
            if (ambar != null)
            {
                if (ambar.DrawMeters == false)
                {
                    ambar.DrawMeters = true;
                    DrawingPanel.Invalidate();
                }
                else
                {
                    ambar.DrawMeters = false;
                    DrawingPanel.Invalidate();
                }
                foreach (var depo in ambar.depolar)
                {
                    if (depo.DrawMeters == false)
                    {
                        depo.DrawMeters = true;
                        DrawingPanel.Invalidate();
                    }
                    else
                    {
                        depo.DrawMeters = false;
                        DrawingPanel.Invalidate();
                    }

                    foreach (var cell in depo.gridmaps)
                    {
                        if (cell.drawtag == false)
                        {
                            cell.drawtag = true;
                            DrawingPanel.Invalidate();
                        }
                        else
                        {
                            cell.drawtag = false;
                            DrawingPanel.Invalidate();
                        }
                    }
                }
            }
        }
        //Show all the items in the selected depo
        private void depodakiNesneleriGörüntüleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dialog = new DepodakiMallariGoruntule(selectedDepo))
            {
                dialog.ShowDialog();
            }
        }
        #endregion











        //Add Reference Points to Items (NOT IN USE)
        #region Add Points to Items
        //Add Points to Items
        private void AddCenterPoint(Balya_Yerleştirme.Models.Item item)
        {
            bool isequal = false;
            Point point = GetCenter(item.OriginalRectangle);

            ItemReferencePoint refpoint = new ItemReferencePoint(
                point.X - 2,
                point.Y - 2,
                4, 4, Zoomlevel, this);
            refpoint.Pointsize = 4;
            refpoint.LocationofRect = new Point((int)refpoint.Rectangle.X, (int)refpoint.Rectangle.Y);
            refpoint.ItemId = item.ItemId;
            refpoint.KareX = refpoint.Rectangle.X;
            refpoint.KareY = refpoint.Rectangle.Y;
            refpoint.KareEni = refpoint.Rectangle.Width;
            refpoint.KareBoyu = refpoint.Rectangle.Height;
            refpoint.OriginalKareX = refpoint.OriginalRectangle.X;
            refpoint.OriginalKareY = refpoint.OriginalRectangle.Y;
            refpoint.OriginalKareEni = refpoint.OriginalRectangle.Width;
            refpoint.OriginalKareBoyu = refpoint.OriginalRectangle.Height;
            refpoint.ApplyZoom(Zoomlevel);

            foreach (var reff in item.ItemReferencePoints)
            {
                if (reff.Rectangle.Location == refpoint.Rectangle.Location)
                {
                    isequal = true;
                }
            }
            if (!isequal)
            {
                item.ItemReferencePoints.Add(refpoint);
            }
        }
        private void RemoveCenterPoint(Balya_Yerleştirme.Models.Item item)
        {
            List<ItemReferencePoint> templist = new List<ItemReferencePoint>();
            Point point = GetCenter(item.OriginalRectangle);

            using (var context = new DBContext())
            {
                foreach (var reff in item.ItemReferencePoints)
                {
                    if (reff.OriginalRectangle.Location == new Point(point.X - 2, point.Y - 2))
                    {
                        var reff1 = (from x in context.ItemReferencePoints
                                     where x.OriginalKareX == reff.OriginalKareX && x.OriginalKareY == reff.OriginalKareY
                                     && x.ItemId == reff.ItemId
                                     select x).FirstOrDefault();

                        if (reff1 != null)
                        {
                            context.ItemReferencePoints.Remove(reff1);
                            context.SaveChanges();
                            templist.Add(reff);
                        }
                    }
                }
            }
            foreach (var pts in templist)
            {
                item.ItemReferencePoints.Remove(pts);
            }
        }
        private void AddBottomEdge(Balya_Yerleştirme.Models.Item item)
        {
            bool isequal = false;
            Point point = GetMiddleOfBottomEdge(item.OriginalRectangle);

            ItemReferencePoint refpoint = new ItemReferencePoint(
                point.X - 2,
                point.Y - 2,
                4, 4, Zoomlevel, this);
            refpoint.Pointsize = 4;
            refpoint.LocationofRect = new Point((int)refpoint.Rectangle.X, (int)refpoint.Rectangle.Y);
            refpoint.ItemId = item.ItemId;
            refpoint.KareX = refpoint.Rectangle.X;
            refpoint.KareY = refpoint.Rectangle.Y;
            refpoint.KareEni = refpoint.Rectangle.Width;
            refpoint.KareBoyu = refpoint.Rectangle.Height;
            refpoint.OriginalKareX = refpoint.OriginalRectangle.X;
            refpoint.OriginalKareY = refpoint.OriginalRectangle.Y;
            refpoint.OriginalKareEni = refpoint.OriginalRectangle.Width;
            refpoint.OriginalKareBoyu = refpoint.OriginalRectangle.Height;
            refpoint.ApplyZoom(Zoomlevel);

            foreach (var reff in item.ItemReferencePoints)
            {
                if (reff.Rectangle.Location == refpoint.Rectangle.Location)
                {
                    isequal = true;
                }
            }
            if (!isequal)
            {
                item.ItemReferencePoints.Add(refpoint);
            }
        }
        private void RemoveBottomEdge(Balya_Yerleştirme.Models.Item item)
        {
            List<ItemReferencePoint> templist = new List<ItemReferencePoint>();
            Point point = GetMiddleOfBottomEdge(item.OriginalRectangle);

            using (var context = new DBContext())
            {
                foreach (var reff in item.ItemReferencePoints)
                {
                    if (reff.OriginalRectangle.Location == new Point(point.X - 2, point.Y - 2))
                    {
                        var reff1 = (from x in context.ItemReferencePoints
                                     where x.OriginalKareX == reff.OriginalKareX && x.OriginalKareY == reff.OriginalKareY
                                     && x.ItemId == reff.ItemId
                                     select x).FirstOrDefault();

                        if (reff1 != null)
                        {
                            context.ItemReferencePoints.Remove(reff1);
                            context.SaveChanges();
                            templist.Add(reff);
                        }
                    }
                }
            }
            foreach (var pts in templist)
            {
                item.ItemReferencePoints.Remove(pts);
            }
        }
        private void AddLeftEdge(Balya_Yerleştirme.Models.Item item)
        {
            bool isequal = false;
            Point point = GetMiddleOfLeftEdge(item.OriginalRectangle);

            ItemReferencePoint refpoint = new ItemReferencePoint(
                point.X - 2,
                point.Y - 2,
                4, 4, Zoomlevel, this);
            refpoint.Pointsize = 4;
            refpoint.LocationofRect = new Point((int)refpoint.Rectangle.X, (int)refpoint.Rectangle.Y);
            refpoint.ItemId = item.ItemId;
            refpoint.KareX = refpoint.Rectangle.X;
            refpoint.KareY = refpoint.Rectangle.Y;
            refpoint.KareEni = refpoint.Rectangle.Width;
            refpoint.KareBoyu = refpoint.Rectangle.Height;
            refpoint.OriginalKareX = refpoint.OriginalRectangle.X;
            refpoint.OriginalKareY = refpoint.OriginalRectangle.Y;
            refpoint.OriginalKareEni = refpoint.OriginalRectangle.Width;
            refpoint.OriginalKareBoyu = refpoint.OriginalRectangle.Height;
            refpoint.ApplyZoom(Zoomlevel);

            foreach (var reff in item.ItemReferencePoints)
            {
                if (reff.Rectangle.Location == refpoint.Rectangle.Location)
                {
                    isequal = true;
                }
            }
            if (!isequal)
            {
                item.ItemReferencePoints.Add(refpoint);
            }
        }
        private void RemoveLeftEdge(Balya_Yerleştirme.Models.Item item)
        {
            List<ItemReferencePoint> templist = new List<ItemReferencePoint>();
            Point point = GetMiddleOfLeftEdge(item.OriginalRectangle);

            using (var context = new DBContext())
            {
                foreach (var reff in item.ItemReferencePoints)
                {
                    if (reff.OriginalRectangle.Location == new Point(point.X - 2, point.Y - 2))
                    {
                        var reff1 = (from x in context.ItemReferencePoints
                                     where x.OriginalKareX == reff.OriginalKareX && x.OriginalKareY == reff.OriginalKareY
                                     && x.ItemId == reff.ItemId
                                     select x).FirstOrDefault();

                        if (reff1 != null)
                        {
                            context.ItemReferencePoints.Remove(reff1);
                            context.SaveChanges();
                            templist.Add(reff);
                        }
                    }
                }
            }
            foreach (var pts in templist)
            {
                item.ItemReferencePoints.Remove(pts);
            }
        }
        private void AddRightEdge(Balya_Yerleştirme.Models.Item item)
        {
            bool isequal = false;
            Point point = GetMiddleOfRightEdge(item.OriginalRectangle);

            ItemReferencePoint refpoint = new ItemReferencePoint(
                point.X - 2,
                point.Y - 2,
                4, 4, Zoomlevel, this);
            refpoint.Pointsize = 4;
            refpoint.LocationofRect = new Point((int)refpoint.Rectangle.X, (int)refpoint.Rectangle.Y);
            refpoint.ItemId = item.ItemId;
            refpoint.KareX = refpoint.Rectangle.X;
            refpoint.KareY = refpoint.Rectangle.Y;
            refpoint.KareEni = refpoint.Rectangle.Width;
            refpoint.KareBoyu = refpoint.Rectangle.Height;
            refpoint.OriginalKareX = refpoint.OriginalRectangle.X;
            refpoint.OriginalKareY = refpoint.OriginalRectangle.Y;
            refpoint.OriginalKareEni = refpoint.OriginalRectangle.Width;
            refpoint.OriginalKareBoyu = refpoint.OriginalRectangle.Height;
            refpoint.ApplyZoom(Zoomlevel);

            foreach (var reff in item.ItemReferencePoints)
            {
                if (reff.Rectangle.Location == refpoint.Rectangle.Location)
                {
                    isequal = true;
                }
            }
            if (!isequal)
            {
                item.ItemReferencePoints.Add(refpoint);
            }
        }
        private void RemoveRightEdge(Balya_Yerleştirme.Models.Item item)
        {
            List<ItemReferencePoint> templist = new List<ItemReferencePoint>();
            Point point = GetMiddleOfRightEdge(item.OriginalRectangle);

            using (var context = new DBContext())
            {
                foreach (var reff in item.ItemReferencePoints)
                {
                    if (reff.OriginalRectangle.Location == new Point(point.X - 2, point.Y - 2))
                    {
                        var reff1 = (from x in context.ItemReferencePoints
                                     where x.OriginalKareX == reff.OriginalKareX && x.OriginalKareY == reff.OriginalKareY
                                     && x.ItemId == reff.ItemId
                                     select x).FirstOrDefault();

                        if (reff1 != null)
                        {
                            context.ItemReferencePoints.Remove(reff1);
                            context.SaveChanges();
                            templist.Add(reff);
                        }
                    }
                }
            }
            foreach (var pts in templist)
            {
                item.ItemReferencePoints.Remove(pts);
            }
        }
        private void AddTopEdge(Balya_Yerleştirme.Models.Item item)
        {
            bool isequal = false;
            Point point = GetMiddleOfTopEdge(item.OriginalRectangle);

            ItemReferencePoint refpoint = new ItemReferencePoint(
                point.X - 2,
                point.Y - 2,
                4, 4, Zoomlevel, this);
            refpoint.Pointsize = 4;
            refpoint.LocationofRect = new Point((int)refpoint.Rectangle.X, (int)refpoint.Rectangle.Y);
            refpoint.ItemId = item.ItemId;
            refpoint.KareX = refpoint.Rectangle.X;
            refpoint.KareY = refpoint.Rectangle.Y;
            refpoint.KareEni = refpoint.Rectangle.Width;
            refpoint.KareBoyu = refpoint.Rectangle.Height;
            refpoint.OriginalKareX = refpoint.OriginalRectangle.X;
            refpoint.OriginalKareY = refpoint.OriginalRectangle.Y;
            refpoint.OriginalKareEni = refpoint.OriginalRectangle.Width;
            refpoint.OriginalKareBoyu = refpoint.OriginalRectangle.Height;
            refpoint.ApplyZoom(Zoomlevel);

            foreach (var reff in item.ItemReferencePoints)
            {
                if (reff.Rectangle.Location == refpoint.Rectangle.Location)
                {
                    isequal = true;
                }
            }
            if (!isequal)
            {
                item.ItemReferencePoints.Add(refpoint);
            }
        }
        private void RemoveTopEdge(Balya_Yerleştirme.Models.Item item)
        {
            List<ItemReferencePoint> templist = new List<ItemReferencePoint>();
            Point point = GetMiddleOfTopEdge(item.OriginalRectangle);

            using (var context = new DBContext())
            {
                foreach (var reff in item.ItemReferencePoints)
                {
                    if (reff.OriginalRectangle.Location == new Point(point.X - 2, point.Y - 2))
                    {
                        var reff1 = (from x in context.ItemReferencePoints
                                     where x.OriginalKareX == reff.OriginalKareX && x.OriginalKareY == reff.OriginalKareY
                                     && x.ItemId == reff.ItemId
                                     select x).FirstOrDefault();

                        if (reff1 != null)
                        {
                            context.ItemReferencePoints.Remove(reff1);
                            context.SaveChanges();
                            templist.Add(reff);
                        }
                    }
                }
            }
            foreach (var pts in templist)
            {
                item.ItemReferencePoints.Remove(pts);
            }
        }
        #endregion
        //Commented out sections (KEEPING FOR POTENTIAL FUTURE USE)
        #region Commented Out Sections
        //private void Focus_EnterToTXTs(object sender, EventArgs e)
        //{
        //    Krypton.Toolkit.KryptonTextBox textBox = sender as Krypton.Toolkit.KryptonTextBox;

        //    if (textBox != null)
        //    {
        //        textBox.Clear();
        //    }
        //}

        //private void toolStripBTN_Balya_Yerlestir_Click(object sender, EventArgs e)
        //{
        //    if (!CheckPanelVisible(Nesne_Yerlestirme_First_Panel))
        //    {
        //        Show_ItemPlacementPanel();
        //        MainPanelMakeSmaller(DrawingPanel);
        //        GVisual.HideControl(Nesne_Yerlestirme_Second_Panel, Nesne_Yerlestirme_First_Panel);
        //        if (ToolStripNesneYerlestirClicked != null)
        //        {
        //            ToolStripNesneYerlestirClicked.Invoke(sender, e);
        //        }
        //    }
        //    else
        //    {
        //        if (PLC_Sim_Panel.Visible)
        //        {
        //            MainPanelMakeSmallest(DrawingPanel);
        //            Show_ItemPlacementPanel();
        //            if (MoveLeftEvent != null)
        //            {
        //                MoveLeftEvent.Invoke(sender, e);
        //            }
        //        }
        //        else
        //        {
        //            HideEverything();
        //            Show_ItemPlacementPanel();
        //        }
        //    }
        //    DrawingPanel.Invalidate();
        //}

        //private void toolStripBTN_Balya_Al_Click(object sender, EventArgs e)
        //{
        //    if (!CheckPanelVisible(Nesne_Al_First_Panel))
        //    {
        //        GVisual.ShowControl(Nesne_Al_First_Panel, this, leftSidePanelLocation);
        //        MainPanelMakeSmaller(DrawingPanel);
        //    }
        //    else
        //    {
        //        HideEverything();
        //        GVisual.ShowControl(Nesne_Al_First_Panel, this, leftSidePanelLocation);
        //        MainPanelMakeSmaller(DrawingPanel);
        //    }
        //    DrawingPanel.Invalidate();
        //}

        //private void balyaYerleştirToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    ItemplacementContextMenuStrip = true;
        //    if (!Nesne_Yerlestirme_First_Panel.Visible)
        //    {
        //        Show_ItemPlacementPanel();
        //        MainPanelMakeSmaller(DrawingPanel);
        //        ItemPlacementContextMenuStripButtonClicked.Invoke(sender, e);
        //        DrawingPanel.Invalidate();
        //    }
        //}

        //private void balyayıAlToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    foreach (var depo in ambar.depolar)
        //    {
        //        foreach (var cell in depo.gridmaps)
        //        {
        //            if (cell.Rectangle.Contains(scaledPoint))
        //            {
        //                cell.itemremoval = true;
        //                cell.DeleteItem();
        //            }
        //        }
        //    }
        //}
        #endregion
    }
}
