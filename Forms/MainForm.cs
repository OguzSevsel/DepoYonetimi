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
using System.Net.Security;
using DocumentFormat.OpenXml.Wordprocessing;
using Balya_Yerleştirme.Forms;



namespace Balya_Yerleştirme
{
    public partial class MainForm : Form
    {
        public Ambar? ambar { get; set; }
        public Depo selectedDepo { get; set; }
        public Isletme? Isletme { get; set; }
        public bool Move { get; set; } = false;
        public float opcount { get; set; } = 0;
        public Size PanelSize { get; set; }
        public string InputPath { get; set; }
        public string OutputPath { get; set; }
        public string FailedPath { get; set; }




        #region PLC Operations
        public Plc PLC { get; set; }
        public int PLCCounter { get; set; } = 0;
        public bool ShowNesneButtons { get; set; } = false;
        public bool NesneKaldır { get; set; } = false;

        #endregion


        #region User Input Variables for Adding and Removing Items
        //User Input Variables
        public string item_etiketi { get; set; } = string.Empty;
        public string item_aciklamasi { get; set; } = string.Empty;
        public string item_turu { get; set; } = string.Empty;
        public string item_turu_secondary { get; set; } = string.Empty;
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

        public System.Drawing.Point drawingPanelLeftLocation { get; set; } = new System.Drawing.Point(12, 91);
        public System.Drawing.Point drawingPanelMiddleLocation { get; set; } = new System.Drawing.Point(373, 91);

        public System.Drawing.Size drawingPanelSmallSize { get; set; } = new System.Drawing.Size(1164, 909);
        public System.Drawing.Size drawingPanelMiddleSize { get; set; } = new System.Drawing.Size(1522, 909);
        public System.Drawing.Size drawingPanelLargeSize { get; set; } = new System.Drawing.Size(1880, 909);
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
            var paths = CreateFolders();
            InputPath = paths.Item1;
            OutputPath = paths.Item2;
            FailedPath = paths.Item3;
            DrawingPanel.Invalidate();
            DrawingPanel.MouseWheel += DrawingPanel_MouseWheel;
            PanelSize = new Size(DrawingPanel.ClientRectangle.Width, DrawingPanel.ClientRectangle.Height);
            btn_PLC_ConnectionPanel_Kapat_Click(this, EventArgs.Empty);
        }



        //Add Items to the Depos According to the Parameters choosed by user when creating Layout
        #region Item Adding Algoritm
        public void AddItemtoCell(Ambar ambar, Models.Cell? cell1, string item_etiketi, string item_aciklamasi,
            float item_agirligi)
        {
            if (cell1 != null)
            {
                Models.Item newItem = new Models.Item(0, 0, cell1.NesneEni, cell1.NesneBoyu, Zoomlevel, this);

                newItem.OriginalRectangle = GVisual.RatioRectangleToParentRectangle(cell1.NesneEni, cell1.NesneBoyu, cell1.CellEni, cell1.CellBoyu, cell1.OriginalRectangle);

                newItem.Rectangle = GVisual.RatioRectangleToParentRectangle(cell1.NesneEni, cell1.NesneBoyu, cell1.CellEni, cell1.CellBoyu, cell1.Rectangle);

                newItem.SelectLayoutRectangle = GVisual.RatioRectangleToParentRectangle(cell1.NesneEni, cell1.NesneBoyu, cell1.CellEni, cell1.CellBoyu, cell1.OriginalRectangle);

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

                using (var context = new DBContext())
                {
                    Models.Item Dbitem = new Models.Item(cell1.CellId, newItem.ItemEtiketi,
                    newItem.ItemTuru, newItem.ItemEni, newItem.ItemBoyu, newItem.ItemYuksekligi,
                    newItem.Rectangle.X, newItem.Rectangle.Y, newItem.Rectangle.Width, newItem.Rectangle.Height,
                    newItem.OriginalRectangle.X, newItem.OriginalRectangle.Y, newItem.OriginalRectangle.Width,
                    newItem.OriginalRectangle.Height, newItem.Zoomlevel, newItem.ItemAgirligi, newItem.ItemAciklamasi,
                    newItem.Cm_X_Axis, newItem.Cm_Y_Axis, newItem.Cm_Z_Axis);

                    context.Items.Add(Dbitem);
                    context.SaveChanges();
                    newItem.ItemId = Dbitem.ItemId;
                }
            }
        }
        public void PlaceItem(Depo depo, string item_etiketi, string item_aciklamasi, float item_agirligi)
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
                            AddItemtoCell(depo.Parent, cell, item_etiketi, item_aciklamasi, item_agirligi);
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
                            AddItemtoCell(depo.Parent, cell, item_etiketi, item_aciklamasi, item_agirligi);
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
                            AddItemtoCell(depo.Parent, cell, item_etiketi, item_aciklamasi, item_agirligi);
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
                            AddItemtoCell(depo.Parent, cell, item_etiketi, item_aciklamasi, item_agirligi);
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
                            AddItemtoCell(depo.Parent, cell, item_etiketi, item_aciklamasi, item_agirligi);
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
                            AddItemtoCell(depo.Parent, cell, item_etiketi, item_aciklamasi, item_agirligi);
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
                            AddItemtoCell(depo.Parent, cell, item_etiketi, item_aciklamasi, item_agirligi);
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
                            AddItemtoCell(depo.Parent, cell, item_etiketi, item_aciklamasi, item_agirligi);
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
        public Depo? GetDepotoPlaceItem(string item_turu, string item_turu_secondary)
        {
            List<int> Q = new List<int>();
            if (ambar != null)
            {
                foreach (var depo in ambar.depolar)
                {
                    string depo_stage = CheckDepoStage(depo);

                    if (depo.ItemTuru == item_turu && depo.ItemTuruSecondary == item_turu_secondary && (depo.currentStage == 1 || depo.currentStage == 2))
                    {
                        //MessageBox.Show
                        //        ("item turu ve secondary item turu tuttu", "item turu ve secondary item turu tuttu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        if (depo_stage != "full")
                        {
                            Q.Add(depo.Yerlestirilme_Sirasi);
                        }
                    }
                    else if (depo.ItemTuru == item_turu &&
                        depo.ItemTuru.Length > 0 &&
                        depo.ItemTuruSecondary == item_turu_secondary &&
                        depo.ItemTuruSecondary.Length == 0 &&
                        (depo.currentStage == 1 || depo.currentStage == 2))
                    {
                        //MessageBox.Show
                        //       ("item turu tuttu ve secondary item turu bos", "item turu tuttu ve secondary item turu bos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                if (ambar != null)
                {
                    ambar.ApplyZoom(Zoomlevel);
                }

                // Calculate the new scroll positions to keep the mouse position consistent
                float newScrollX = mouseXRelativeToContent * Zoomlevel - e.X;
                float newScrollY = mouseYRelativeToContent * Zoomlevel - e.Y;

                DrawingPanel.AutoScrollPosition = new Point((int)newScrollX, (int)newScrollY);

                DrawingPanel.Invalidate();
            }
        }
        private void DrawingPanel_Paint(object sender, PaintEventArgs e)
        {
            DrawingPanel.AutoScrollMinSize = new Size((int)((DrawingPanel.ClientRectangle.Width * Zoomlevel)), (int)((DrawingPanel.ClientRectangle.Height * Zoomlevel)));

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
                    ambar.Draw(g);
                }
            }
            e.Graphics.DrawImage(bitmap, new Point(0, 0));
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
        private void btn_Layout_Olustur_Click(object sender, EventArgs e)
        {
            if (Isletme != null)
            {
                LayoutOlusturma dia = new LayoutOlusturma(this, null, null, Isletme);
                dia.Show();
            }
            else
            {
                MessageBox.Show("Lütfen önce bir işletme seçin.", "Lütfen İşletme Seçin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }



        //Button event that opens the dialog for Selecting Layouts
        private void toolStripBTN_Layout_Sec_Click(object sender, EventArgs e)
        {
            if (ambar != null)
            {
                Zoomlevel = 1f;
                ambar.ApplyZoom(Zoomlevel);
                foreach (var depo in ambar.depolar)
                {
                    depo.ApplyZoom(Zoomlevel);
                    foreach (var cell in depo.gridmaps)
                    {
                        cell.ApplyZoom(Zoomlevel);
                        foreach (var item in cell.items)
                        {
                            item.ApplyZoom(Zoomlevel);
                        }
                    }
                }

                foreach (var conveyor in ambar.conveyors)
                {
                    conveyor.ApplyZoom(Zoomlevel);
                    foreach (var reff in conveyor.ConveyorReferencePoints)
                    {
                        reff.ApplyZoom(Zoomlevel);
                    }
                }
                DrawingPanel.AutoScrollMinSize = new Size(0, 0);
            }
            SelectLayouts dialog = new SelectLayouts(DrawingPanel, this, Isletme);
            if (dialog.DialogResult != DialogResult.Cancel)
            {
                dialog.Show();
            }
        }


        #endregion



        //Async Database Tasks
        #region Async Database Tasks

        //This is for when user clicked to the save and load layout button in the layout creation form it creates the layout in the database
        public async Task LayoutOlusturSecondDatabaseOperation(string LayoutName, string LayoutDescription, Layout? layout, Ambar ambar)
        {
            if (ambar != null)
            {
                using (var context = new DBContext())
                {
                    if (layout == null)
                    {
                        if (Isletme != null)
                        {
                            Models.Layout newLayout = new Models.Layout(Isletme.IsletmeID, LayoutName, LayoutDescription, 0);
                            await context.Layout.AddAsync(newLayout);
                            await context.SaveChangesAsync();
                            ambar.LayoutId = newLayout.LayoutId;

                            Ambar newAmbar = new Ambar(newLayout.LayoutId, "ambar", "ambar",
                                    ambar.KareX, ambar.KareY, ambar.KareEni, ambar.KareBoyu,
                                    ambar.OriginalKareX, ambar.OriginalKareY,
                                    ambar.OriginalKareEni, ambar.OriginalKareBoyu,
                                    ambar.Zoomlevel, ambar.AmbarEni, ambar.AmbarBoyu);

                            await context.Ambars.AddAsync(newAmbar);
                            await context.SaveChangesAsync();
                            ambar.AmbarId = newAmbar.AmbarId;

                            foreach (var depo in ambar.deletedDepos)
                            {
                                if (depo.DepoId != 0)
                                {
                                    var depo1 = (from x in context.Depos
                                                 where x.DepoId == depo.DepoId
                                                 select x).FirstOrDefault();

                                    if (depo1 != null)
                                    {
                                        context.Depos.Remove(depo1);
                                        context.SaveChanges();
                                    }
                                }
                            }

                            foreach (var conveyor in ambar.deletedConveyors)
                            {
                                if (conveyor.ConveyorId != 0)
                                {
                                    var conveyor1 = (from x in context.Conveyors
                                                     where x.ConveyorId == conveyor.ConveyorId
                                                     select x).FirstOrDefault();

                                    if (conveyor1 != null)
                                    {
                                        context.Conveyors.Remove(conveyor1);
                                        context.SaveChanges();
                                    }
                                }
                            }

                            foreach (var depo in ambar.depolar)
                            {
                                Depo newDepo = new Depo(newAmbar.AmbarId,
                                    depo.DepoName, depo.DepoDescription, depo.DepoAlaniEni,
                                    depo.DepoAlaniBoyu, depo.DepoAlaniYuksekligi,
                                    depo.OriginalDepoSizeWidth, depo.OriginalDepoSizeHeight,
                                    depo.Rectangle.X, depo.Rectangle.Y, depo.KareEni, depo.KareBoyu,
                                    depo.OriginalKareX, depo.OriginalKareY,
                                    depo.OriginalKareEni, depo.OriginalKareBoyu, depo.Zoomlevel,
                                    depo.itemDrop_StartLocation, depo.itemDrop_UpDown,
                                    depo.itemDrop_LeftRight, depo.asama1_Yuksekligi, depo.asama2_Yuksekligi,
                                    depo.Yerlestirilme_Sirasi, depo.DepoAlaniEni * 100, depo.DepoAlaniBoyu * 100,
                                    depo.ColumnCount, depo.RowCount, depo.currentColumn, depo.currentRow, depo.ItemTuru,
                                    depo.asama1_ItemSayisi, depo.asama2_ToplamItemSayisi, depo.currentStage, depo.ItemTuruSecondary, depo.isYerlestirilme);

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
                                        4, reff.OriginalLocationInsideParent.X, reff.OriginalLocationInsideParent.Y);

                                    await context.ConveyorReferencePoints.AddAsync(point);
                                    await context.SaveChangesAsync();
                                    reff.ReferenceId = point.ReferenceId;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (ambar.AmbarId == 0)
                        {
                            Ambar newAmbar = new Ambar(layout.LayoutId, "ambar", "ambar",
                                ambar.Rectangle.X, ambar.Rectangle.Y, ambar.Rectangle.Width, ambar.Rectangle.Height,
                                ambar.OriginalRectangle.X, ambar.OriginalRectangle.Y,
                                ambar.OriginalRectangle.Width, ambar.OriginalRectangle.Height,
                                ambar.Zoomlevel, ambar.AmbarEni, ambar.AmbarBoyu);

                            await context.Ambars.AddAsync(newAmbar);
                            await context.SaveChangesAsync();
                            ambar.AmbarId = newAmbar.AmbarId;
                        }
                        else
                        {
                            var dbAmbar = await (from x in context.Ambars
                                                 where x.LayoutId == layout.LayoutId
                                                 select x).FirstOrDefaultAsync();

                            if (dbAmbar != null)
                            {
                                dbAmbar.KareX = ambar.Rectangle.X;
                                dbAmbar.KareY = ambar.Rectangle.Y;
                                dbAmbar.KareEni = ambar.Rectangle.Width;
                                dbAmbar.KareBoyu = ambar.Rectangle.Height;
                                dbAmbar.OriginalKareX = ambar.OriginalRectangle.X;
                                dbAmbar.OriginalKareY = ambar.OriginalRectangle.Y;
                                dbAmbar.OriginalKareEni = ambar.OriginalRectangle.Width;
                                dbAmbar.OriginalKareBoyu = ambar.OriginalRectangle.Height;

                                await context.SaveChangesAsync();
                            }
                        }

                        foreach (var depo in ambar.deletedDepos)
                        {
                            if (depo.DepoId != 0)
                            {
                                var depo1 = (from x in context.Depos
                                             where x.DepoId == depo.DepoId
                                             select x).FirstOrDefault();

                                if (depo1 != null)
                                {
                                    context.Depos.Remove(depo1);
                                    context.SaveChanges();
                                }
                            }
                        }

                        foreach (var conveyor in ambar.deletedConveyors)
                        {
                            if (conveyor.ConveyorId != 0)
                            {
                                var conveyor1 = (from x in context.Conveyors
                                                 where x.ConveyorId == conveyor.ConveyorId
                                                 select x).FirstOrDefault();

                                if (conveyor1 != null)
                                {
                                    context.Conveyors.Remove(conveyor1);
                                    context.SaveChanges();
                                }
                            }
                        }

                        foreach (var depo in ambar.depolar)
                        {
                            if (depo.DepoId == 0 && !ambar.deletedDepos.Contains(depo))
                            {
                                Depo newDepo = new Depo(ambar.AmbarId,
                                    depo.DepoName, depo.DepoDescription, depo.DepoAlaniEni,
                                    depo.DepoAlaniBoyu, depo.DepoAlaniYuksekligi,
                                    depo.OriginalDepoSizeWidth, depo.OriginalDepoSizeHeight,
                                    depo.Rectangle.X, depo.Rectangle.Y, depo.Rectangle.Width, depo.Rectangle.Height,
                                    depo.OriginalRectangle.X, depo.OriginalRectangle.Y,
                                    depo.OriginalRectangle.Width, depo.OriginalRectangle.Height, depo.Zoomlevel,
                                    depo.itemDrop_StartLocation, depo.itemDrop_UpDown,
                                    depo.itemDrop_LeftRight, depo.asama1_Yuksekligi, depo.asama2_Yuksekligi,
                                    depo.Yerlestirilme_Sirasi, depo.DepoAlaniEni * 100, depo.DepoAlaniBoyu * 100,
                                    depo.ColumnCount, depo.RowCount, depo.currentColumn, depo.currentRow, depo.ItemTuru,
                                    depo.asama1_ItemSayisi, depo.asama2_ToplamItemSayisi, depo.currentStage, depo.ItemTuruSecondary, depo.isYerlestirilme);

                                await context.Depos.AddAsync(newDepo);
                                await context.SaveChangesAsync();
                                depo.DepoId = newDepo.DepoId;

                                foreach (var cell in depo.gridmaps)
                                {
                                    if (cell.CellId == 0)
                                    {
                                        Balya_Yerleştirme.Models.Cell newCell =
                                  new Balya_Yerleştirme.Models.Cell(
                                      depo.DepoId, cell.CellEtiketi,
                                      cell.CellEni, cell.CellBoyu,
                                      cell.CellYuksekligi, cell.CellMalSayisi,
                                      cell.Rectangle.X, cell.Rectangle.Y, cell.Rectangle.Width,
                                      cell.Rectangle.Height, cell.OriginalRectangle.X,
                                      cell.OriginalRectangle.Y, cell.OriginalRectangle.Width,
                                      cell.OriginalRectangle.Height, cell.Zoomlevel,
                                      cell.ItemSayisi, cell.DikeyKenarBoslugu,
                                      cell.YatayKenarBoslugu, cell.NesneEni,
                                      cell.NesneBoyu, cell.NesneYuksekligi,
                                      cell.Column, cell.Row, cell.toplam_Nesne_Yuksekligi, cell.cell_Cm_X, cell.cell_Cm_Y);

                                        await context.Cells.AddAsync(newCell);
                                        await context.SaveChangesAsync();
                                        cell.CellId = newCell.CellId;
                                    }
                                    else
                                    {
                                        var cell1 = await (from x in context.Cells
                                                           where x.CellId == cell.CellId
                                                           select x).FirstOrDefaultAsync();

                                        if (cell1 != null)
                                        {
                                            cell1.KareX = cell.Rectangle.X;
                                            cell1.KareY = cell.Rectangle.Y;
                                            cell1.KareEni = cell.Rectangle.Width;
                                            cell1.KareBoyu = cell.Rectangle.Height;
                                            cell1.OriginalKareX = cell.OriginalRectangle.X;
                                            cell1.OriginalKareY = cell.OriginalRectangle.Y;
                                            cell1.OriginalKareEni = cell.OriginalRectangle.Width;
                                            cell1.OriginalKareBoyu = cell.OriginalRectangle.Height;
                                            cell1.CellEni = cell.CellEni;
                                            cell1.CellBoyu = cell.CellBoyu;
                                            cell1.CellYuksekligi = cell.CellYuksekligi;
                                            cell1.CellMalSayisi = cell.CellMalSayisi;
                                            cell1.ItemSayisi = cell.ItemSayisi;
                                            cell1.DikeyKenarBoslugu = cell.DikeyKenarBoslugu;
                                            cell1.YatayKenarBoslugu = cell.YatayKenarBoslugu;
                                            cell1.NesneEni = cell.NesneEni;
                                            cell1.NesneBoyu = cell.NesneBoyu;
                                            cell1.NesneYuksekligi = cell.NesneYuksekligi;
                                            cell1.Column = cell.Column;
                                            cell1.Row = cell.Row;
                                            cell1.toplam_Nesne_Yuksekligi = cell.toplam_Nesne_Yuksekligi;
                                            cell1.cell_Cm_X = cell.cell_Cm_X;
                                            cell1.cell_Cm_Y = cell.cell_Cm_Y;
                                            cell1.CellEtiketi = cell.CellEtiketi;

                                            await context.SaveChangesAsync();
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (!ambar.deletedDepos.Contains(depo))
                                {
                                    var depo1 = await (from x in context.Depos
                                                       where x.DepoId == depo.DepoId
                                                       select x).FirstOrDefaultAsync();

                                    if (depo1 != null)
                                    {
                                        depo1.KareX = depo.Rectangle.X;
                                        depo1.KareY = depo.Rectangle.Y;
                                        depo1.KareEni = depo.Rectangle.Width;
                                        depo1.KareBoyu = depo.Rectangle.Height;
                                        depo1.OriginalKareX = depo.OriginalRectangle.X;
                                        depo1.OriginalKareY = depo.OriginalRectangle.Y;
                                        depo1.OriginalKareEni = depo.OriginalRectangle.Width;
                                        depo1.OriginalKareBoyu = depo.OriginalRectangle.Height;
                                        depo1.ItemTuru = depo.ItemTuru;
                                        depo1.ItemTuruSecondary = depo.ItemTuruSecondary;
                                        depo1.itemDrop_LeftRight = depo.itemDrop_LeftRight;
                                        depo1.itemDrop_Stage1 = depo.itemDrop_Stage1;
                                        depo1.itemDrop_Stage2 = depo.itemDrop_Stage2;
                                        depo1.itemDrop_StartLocation = depo.itemDrop_StartLocation;
                                        depo1.itemDrop_UpDown = depo.itemDrop_UpDown;
                                        depo1.asama1_ItemSayisi = depo.asama1_ItemSayisi;
                                        depo1.asama2_ToplamItemSayisi = depo.asama2_ToplamItemSayisi;
                                        depo1.currentStage = depo.currentStage;
                                        depo1.currentRow = depo.currentRow;
                                        depo1.currentColumn = depo.currentColumn;
                                        depo1.Yerlestirilme_Sirasi = depo.Yerlestirilme_Sirasi;
                                        depo1.OriginalDepoSizeHeight = depo.OriginalDepoSizeHeight;
                                        depo1.OriginalDepoSizeWidth = depo.OriginalDepoSizeWidth;
                                        depo1.DepoAlaniEni = depo.DepoAlaniEni;
                                        depo1.DepoAlaniBoyu = depo.DepoAlaniBoyu;
                                        depo1.DepoAlaniYuksekligi = depo.DepoAlaniYuksekligi;
                                        depo1.DepoDescription = depo.DepoDescription;
                                        depo1.DepoName = depo.DepoName;
                                        depo1.Depo_Alani_Eni_Cm = depo.Depo_Alani_Eni_Cm;
                                        depo1.Depo_Alani_Boyu_Cm = depo.Depo_Alani_Boyu_Cm;
                                        depo1.ColumnCount = depo.ColumnCount;
                                        depo1.RowCount = depo.RowCount;

                                        await context.SaveChangesAsync();

                                        foreach (var cell in depo.gridmaps)
                                        {
                                            if (cell.CellId == 0)
                                            {
                                                Balya_Yerleştirme.Models.Cell newCell =
                                             new Balya_Yerleştirme.Models.Cell(
                                                 depo.DepoId, cell.CellEtiketi,
                                                 cell.CellEni, cell.CellBoyu,
                                                 cell.CellYuksekligi, cell.CellMalSayisi,
                                                 cell.Rectangle.X, cell.Rectangle.Y, cell.Rectangle.Width,
                                                 cell.Rectangle.Height, cell.OriginalRectangle.X,
                                                 cell.OriginalRectangle.Y, cell.OriginalRectangle.Width,
                                                 cell.OriginalRectangle.Height, cell.Zoomlevel,
                                                 cell.ItemSayisi, cell.DikeyKenarBoslugu,
                                                 cell.YatayKenarBoslugu, cell.NesneEni,
                                                 cell.NesneBoyu, cell.NesneYuksekligi,
                                                 cell.Column, cell.Row, cell.toplam_Nesne_Yuksekligi, cell.cell_Cm_X, cell.cell_Cm_Y);

                                                await context.Cells.AddAsync(newCell);
                                                await context.SaveChangesAsync();
                                                cell.CellId = newCell.CellId;
                                            }
                                            else
                                            {
                                                var cell1 = await (from x in context.Cells
                                                                   where x.CellId == cell.CellId
                                                                   select x).FirstOrDefaultAsync();

                                                if (cell1 != null)
                                                {
                                                    cell1.KareX = cell.Rectangle.X;
                                                    cell1.KareY = cell.Rectangle.Y;
                                                    cell1.KareEni = cell.Rectangle.Width;
                                                    cell1.KareBoyu = cell.Rectangle.Height;
                                                    cell1.OriginalKareX = cell.OriginalRectangle.X;
                                                    cell1.OriginalKareY = cell.OriginalRectangle.Y;
                                                    cell1.OriginalKareEni = cell.OriginalRectangle.Width;
                                                    cell1.OriginalKareBoyu = cell.OriginalRectangle.Height;
                                                    cell1.CellEni = cell.CellEni;
                                                    cell1.CellBoyu = cell.CellBoyu;
                                                    cell1.CellYuksekligi = cell.CellYuksekligi;
                                                    cell1.CellMalSayisi = cell.CellMalSayisi;
                                                    cell1.ItemSayisi = cell.ItemSayisi;
                                                    cell1.DikeyKenarBoslugu = cell.DikeyKenarBoslugu;
                                                    cell1.YatayKenarBoslugu = cell.YatayKenarBoslugu;
                                                    cell1.NesneEni = cell.NesneEni;
                                                    cell1.NesneBoyu = cell.NesneBoyu;
                                                    cell1.NesneYuksekligi = cell.NesneYuksekligi;
                                                    cell1.Column = cell.Column;
                                                    cell1.Row = cell.Row;
                                                    cell1.toplam_Nesne_Yuksekligi = cell.toplam_Nesne_Yuksekligi;
                                                    cell1.cell_Cm_X = cell.cell_Cm_X;
                                                    cell1.cell_Cm_Y = cell.cell_Cm_Y;
                                                    cell1.CellEtiketi = cell.CellEtiketi;

                                                    await context.SaveChangesAsync();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        foreach (var conveyor in ambar.conveyors)
                        {
                            if (conveyor.ConveyorId == 0 && !ambar.deletedConveyors.Contains(conveyor))
                            {
                                Conveyor conv = new Conveyor(ambar.AmbarId,
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
                                        4, reff.OriginalLocationInsideParent.X, reff.OriginalLocationInsideParent.Y);

                                    await context.ConveyorReferencePoints.AddAsync(point);
                                    await context.SaveChangesAsync();
                                    reff.ReferenceId = point.ReferenceId;
                                }
                            }
                            else
                            {
                                if (!ambar.deletedConveyors.Contains(conveyor))
                                {
                                    var conv = await (from x in context.Conveyors
                                                      where x.ConveyorId == conveyor.ConveyorId
                                                      select x).FirstOrDefaultAsync();

                                    if (conv != null)
                                    {
                                        conv.KareX = conveyor.Rectangle.X;
                                        conv.KareY = conveyor.Rectangle.Y;
                                        conv.KareEni = conveyor.Rectangle.Width;
                                        conv.KareBoyu = conveyor.Rectangle.Height;
                                        conv.OriginalKareX = conveyor.OriginalRectangle.X;
                                        conv.OriginalKareY = conveyor.OriginalRectangle.Y;
                                        conv.OriginalKareEni = conveyor.OriginalRectangle.Width;
                                        conv.OriginalKareBoyu = conveyor.OriginalRectangle.Height;
                                        conv.ConveyorEni = conveyor.ConveyorEni;
                                        conv.ConveyorBoyu = conveyor.ConveyorBoyu;
                                        conv.LocationofRect = conveyor.LocationofRect;
                                        conv.ConveyorAraligi = conveyor.ConveyorAraligi;
                                        conv.Zoomlevel = conveyor.Zoomlevel;

                                        await context.SaveChangesAsync();
                                    }
                                    foreach (var reff in conveyor.ConveyorReferencePoints)
                                    {
                                        var reff1 = await (from x in context.ConveyorReferencePoints
                                                           where x.ReferenceId == reff.ReferenceId
                                                           select x).FirstOrDefaultAsync();

                                        if (reff1 != null)
                                        {
                                            reff1.KareX = reff.Rectangle.X;
                                            reff1.KareY = reff.Rectangle.Y;
                                            reff1.KareEni = reff.Rectangle.Width;
                                            reff1.KareBoyu = reff.Rectangle.Height;
                                            reff1.OriginalKareX = reff.OriginalRectangle.X;
                                            reff1.OriginalKareY = reff.OriginalRectangle.Y;
                                            reff1.OriginalKareEni = reff.OriginalRectangle.Width;
                                            reff1.OriginalKareBoyu = reff.OriginalRectangle.Height;
                                            reff1.LocationofRect = reff.LocationofRect;
                                            reff1.Zoomlevel = reff.Zoomlevel;
                                            reff1.OriginalLocationInsideParentX = reff.OriginalLocationInsideParentX;
                                            reff1.OriginalLocationInsideParentY = reff.OriginalLocationInsideParentY;
                                            reff1.Pointsize = reff.Pointsize;
                                            reff1.ReferenceId = reff.ReferenceId;

                                            await context.SaveChangesAsync();
                                        }
                                    }
                                }
                            }
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
                IsletmeInfoPanel.Location = new Point(730, 4);
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
                IsletmeInfoPanel.Location = new Point(886, 4);
            }
        }
        private void btn_PLC_Connection_Click(object sender, EventArgs e)
        {
            MainPanelOpenLeftSide(leftLayoutPanel, this, leftSidePanelLocation, PLC_DB_AdressPanel);
            GVisual.ShowControl(PLC_DB_AdressPanel, leftLayoutPanel);
            DrawingPanel.Invalidate();
        }
        private void btn_PLC_DB_AddressPanel_Kapat_Click(object sender, EventArgs e)
        {
            MainPanelCloseLeftSide(leftLayoutPanel, this);
            GVisual.HideControl(PLC_DB_AdressPanel, leftLayoutPanel);

            if (PLC_Connection_Panel.Controls.Contains(txt_PLC_IP_Address))
            {
                btn_PLC_ConnectionPanel_Kapat_Click(this, EventArgs.Empty);
            }
            DrawingPanel.Invalidate();
        }
        private void btn_DB_Onayla_Click(object sender, EventArgs e)
        {
            MainPanelCloseLeftSide(leftLayoutPanel, this);
            GVisual.HideControl(PLC_DB_AdressPanel, leftLayoutPanel);

            if (PLC_Connection_Panel.Controls.Contains(txt_PLC_IP_Address))
            {
                btn_PLC_ConnectionPanel_Kapat_Click(this, EventArgs.Empty);
            }
            DrawingPanel.Invalidate();
        }
        private void btn_DB_Vazgec_Click(object sender, EventArgs e)
        {
            MainPanelCloseLeftSide(leftLayoutPanel, this);
            GVisual.HideControl(PLC_DB_AdressPanel, leftLayoutPanel);

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
                    if (leftLayoutPanel.Visible && !rightLayoutPanel.Visible)
                    {
                        MainPanelCloseLeftSide(leftLayoutPanel, this);
                        MainPanelOpenRightSide(rightLayoutPanel, this, rightSidePanelLocation, PLC_Sim_Panel);
                    }
                    else if (!leftLayoutPanel.Visible && rightLayoutPanel.Visible)
                    {
                        EmptyPLCSimPanel();
                    }
                    else if (!leftLayoutPanel.Visible && !rightLayoutPanel.Visible)
                    {
                        MainPanelOpenRightSide(rightLayoutPanel, this, rightSidePanelLocation, PLC_Sim_Panel);
                    }
                    else if (leftLayoutPanel.Visible && rightLayoutPanel.Visible)
                    {
                        MainPanelCloseLeftSide(leftLayoutPanel, this);
                        EmptyPLCSimPanel();
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
        #region PLC Simulation for Adding Items

        //These are Simulation Panel Button Events (This panel works for both Item Placement and Item Removal Simulations
        //This is the Button that Opens the Simulation Panel for PLC
        private void btn_PLC_Barkod_Oku_Click(object sender, EventArgs e)
        {
            if (ambar != null)
            {
                MainPanelOpenRightSide(rightLayoutPanel, this, rightSidePanelLocation, PLC_Sim_Panel);
            }
            else
            {
                MessageBox.Show("Lütfen önce Layout yükleyin ya da oluşturun.", "Layout Yüklenmemiş", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }
        //This is the Button that Closes the Simulation Panel for PLC
        private void btn_PLC_Sim_Panel_Kapat_Click(object sender, EventArgs e)
        {
            MainPanelCloseRightSide(rightLayoutPanel, this);

            BlinkingCell = null;
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
                if (!combo_Tur_Kodu_2_Etiket.Items.Contains(depo.ItemTuruSecondary))
                {
                    combo_Tur_Kodu_2_Etiket.Items.Add(depo.ItemTuruSecondary);
                }
            }

            MainPanelOpenLeftSide(leftLayoutPanel, this, leftSidePanelLocation, Nesne_Yerlestirme_First_Panel);

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
            item_turu_secondary = combo_Tur_Kodu_2_Etiket.Text;
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

            bool isinlayout = CheckifItemisinLayout(item_etiketi);

            if (isinlayout)
            {
                errorProvider.SetError(txt_Item_Etiketi, "Bu etikete sahip bir nesne bulunuyor, Lütfen etiketi değiştirip tekrar deneyin.");
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
                        Depo newDepo = GetDepotoPlaceItem(item_turu, item_turu_secondary);

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
                            PlaceItem(newDepo, item_etiketi, item_aciklamasi, item_agirligi);
                            Point location = new Point(3, 548);
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
            MainPanelCloseLeftSide(leftLayoutPanel, this);
            EmptyItemPlacementPanel();
            EmptyPLCSimPanel();
            nesneTimer.Stop();
            PLC_Timer.Stop();
            DrawingPanel.Invalidate();
        }
        //This is the Button that Cancels Item Placement Process
        private void btn_Balya_Yerlestir_Vazgec_Click(object sender, EventArgs e)
        {
            if (rightLayoutPanel.Visible)
            {
                MainPanelCloseLeftSide(leftLayoutPanel, this);
                MainPanelCloseRightSide(rightLayoutPanel, this);
            }
            else
            {
                MainPanelCloseLeftSide(leftLayoutPanel, this);
            }
            nesneTimer.Stop();
            PLC_Timer.Stop();
            EmptyPLCSimPanel();
            DrawingPanel.Invalidate();
        }
        //This is the Button that Returns to the First Item Placement Process Step
        private void btn_Nesne_Yerlestir_Vazgec_Click(object sender, EventArgs e)
        {
            GVisual.HideControl(PLC_Sim_Yerlestiriliyor_Panel, PLC_Sim_Panel);
            GVisual.HideControl(Nesne_Yerlestirme_Second_Panel, Nesne_Yerlestirme_First_Panel);
            if (BlinkingCell != null)
            {
                BlinkingCell.HoverPen = new Pen(System.Drawing.Color.DarkGray);
                BlinkingCell.HoverPen.DashStyle = DashStyle.DashDot;
                BlinkingCell = null;
                DrawingPanel.Invalidate();
            }
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

                if (PLCCounter > 3)
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
            else
            {
                if (ambar != null)
                {
                    foreach (var depo in ambar.depolar)
                    {
                        foreach (var cell in depo.gridmaps)
                        {
                            if (cell.HoverPen.Color == System.Drawing.Color.Lime)
                            {
                                cell.HoverPen = new Pen(System.Drawing.Color.DarkGray);
                                cell.HoverPen.DashStyle = DashStyle.DashDot;
                                DrawingPanel.Invalidate();
                            }
                        }
                    }
                }
                else
                {
                    nesneTimer.Stop();
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
                EmptyItemRemovePanel();
                PLC_Timer.Stop();
                MainPanelCloseLeftSide(leftLayoutPanel, this);
                MainPanelCloseRightSide(rightLayoutPanel, this);
                if (BlinkingCell != null)
                {
                    BlinkingCell.HoverPen = new Pen(System.Drawing.Color.DarkGray);
                    BlinkingCell.HoverPen.DashStyle = DashStyle.DashDot;
                    BlinkingCell = null;
                    DrawingPanel.Invalidate();
                }
            }
            else
            {
                if (leftLayoutPanel.Visible && rightLayoutPanel.Visible)
                {
                    MainPanelCloseLeftSide(leftLayoutPanel, this);
                    MainPanelCloseRightSide(rightLayoutPanel, this);
                }
                else if (!leftLayoutPanel.Visible && rightLayoutPanel.Visible)
                {
                    MainPanelCloseRightSide(rightLayoutPanel, this);
                }
                else if (leftLayoutPanel.Visible && !rightLayoutPanel.Visible)
                {
                    MainPanelCloseLeftSide(leftLayoutPanel, this);
                }

                if (ambar != null)
                {
                    if (ambar.depolar.Count > 0)
                    {
                        Depo newDepo = GetDepotoPlaceItem(item_turu, item_turu_secondary);

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
                            PlaceItem(newDepo, item_etiketi, item_aciklamasi, item_agirligi);
                        }
                    }
                }
                PLC_Timer.Stop();
                EmptyPLCSimPanel();
                EmptyItemPlacementPanel();
                if (BlinkingCell != null)
                {
                    BlinkingCell.HoverPen = new Pen(System.Drawing.Color.DarkGray);
                    BlinkingCell.HoverPen.DashStyle = DashStyle.DashDot;
                    BlinkingCell = null;
                    DrawingPanel.Invalidate();
                }
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
        private void btn_PLC_Sim_Nesne_Bul_Click(object sender, EventArgs e)
        {
            MainPanelOpenLeftSide(leftLayoutPanel, this, leftSidePanelLocation, Nesne_Al_First_Panel);

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
        private void btn_Nesne_Bul_Click(object sender, EventArgs e)
        {
            item_etiketi = txt_Nesne_Al_Etiket.Text;
            bool nesne_bulunmuyor = true;
            bool nesne_etiketi = false;

            Models.Cell? AlternateCell = null;
            Models.Item? AlternateItem = null;
            List<Models.Item> Items = new List<Models.Item>();


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
                                if (item.ItemEtiketi == item_etiketi && item == cell.items.Last())
                                {
                                    nesne_etiketi = true;
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
                                else if (item.ItemEtiketi == item_etiketi && item != cell.items.Last())
                                {
                                    nesne_etiketi = true;
                                    foreach (var item1 in cell.items)
                                    {
                                        if (item1 == cell.items.Last() && item1 != item)
                                        {
                                            nesne_etiketi = true;
                                            BlinkingCell = cell;
                                            lbl_Nesne_Al_Tur_Kodu_Value.Text = $"{depo.ItemTuru}";
                                            lbl_Nesne_Al_Aciklama_Value.Text = $"{item.ItemAciklamasi}";
                                            lbl_Nesne_Al_Agirlik_Value.Text = $"{item.ItemAgirligi}";
                                            lbl_Nesne_Al_Nesne_X_Value.Text = $"{item.Cm_X_Axis} cm";
                                            lbl_Nesne_Al_Nesne_Y_Value.Text = $"{item.Cm_Y_Axis} cm";
                                            lbl_Nesne_Al_Nesne_Z_Value.Text = $"{item.Cm_Z_Axis} cm";
                                            nesneTimer.Start();
                                            Point point = GVisual.Point_Control_to_BottomSide_ofControl
                                                (Nesne_Al_Second_Panel, BorderEdge_NesneAl, 5);
                                            GVisual.ShowControl(Nesne_Al_Second_Panel, Nesne_Al_First_Panel,
                                                point);
                                            if (!PLC_Sim_YerSoyle_Panel.Visible)
                                            {
                                                Point point1 = GVisual.Point_Control_to_BottomSide_ofControl(PLC_Sim_YerSoyle_Panel, btn_PLC_Sim_Nesne_Bul, 5);
                                                GVisual.ShowControl(PLC_Sim_YerSoyle_Panel, PLC_Sim_Panel, point1);
                                                PLC_Timer.Start();
                                            }

                                            (AlternateItem, AlternateCell) = FindEmptyCellstoPlace(cell, depo, item1);
                                            if (AlternateItem != null)
                                            {
                                                Items.Add(AlternateItem);
                                            }
                                        }
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
            if (!nesne_etiketi)
            {
                MessageBox.Show("Aradığınız etikete sahip bir nesne bulunamadı.", "Nesne Bulunamadı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            if (AlternateCell != null && BlinkingCell != null)
            {
                var result = MessageBox.Show("Nesne bulunduğu hücrede en yukarıda değil, Üzerini boşaltıp almak istiyor musunuz?", "Üzerini boşaltmak istiyor musunuz?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    foreach (var item2 in Items)
                    {
                        BlinkingCell.items.Remove(item2);
                        using (var context = new DBContext())
                        {
                            var item = (from x in context.Items
                                        where x.ItemId == item2.ItemId
                                        select x).FirstOrDefault();

                            if (item != null)
                            {
                                context.Items.Remove(item);
                                context.SaveChanges();
                            }

                        }
                        item2.ItemId = 0;

                        MainPanelCloseLeftSide(leftLayoutPanel, this);
                        MainPanelCloseRightSide(rightLayoutPanel, this);

                        AddItemtoCell(ambar, AlternateCell, item2.ItemEtiketi, item2.ItemAciklamasi, item2.ItemAgirligi);

                        MainPanelOpenRightSide(rightLayoutPanel, this, rightSidePanelLocation, PLC_Sim_Panel);
                        MainPanelOpenLeftSide(leftLayoutPanel, this, leftSidePanelLocation, Nesne_Al_First_Panel);
                    }
                }
                else
                {
                    BlinkingCell = null;
                    MainPanelCloseLeftSide(leftLayoutPanel, this);
                    MainPanelCloseRightSide(rightLayoutPanel, this);
                    EmptyPLCSimPanel();
                }
            }
            DrawingPanel.Invalidate();
        }
        private (Models.Item?, Models.Cell?) FindEmptyCellstoPlace(Models.Cell cell, Depo depo, Models.Item item)
        {
            Models.Cell? AlternateCell = null;

            if (depo.itemDrop_StartLocation == "Ortadan")
            {
                if (depo.itemDrop_UpDown == "Yukarı Doğru")
                {
                    if (depo.itemDrop_LeftRight == "Sağa Doğru")
                    {
                        AlternateCell = SearchThroughRightReturnCell(true, true, depo, cell);
                    }
                    else if (depo.itemDrop_LeftRight == "Sola Doğru")
                    {
                        AlternateCell = SearchThroughLeftReturnCell(true, true, depo, cell);
                    }
                }
                else if (depo.itemDrop_UpDown == "Aşağı Doğru")
                {
                    if (depo.itemDrop_LeftRight == "Sağa Doğru")
                    {
                        AlternateCell = SearchThroughRightReturnCell(true, false, depo, cell);
                    }
                    else if (depo.itemDrop_LeftRight == "Sola Doğru")
                    {
                        AlternateCell = SearchThroughLeftReturnCell(true, false, depo, cell);
                    }
                }
            }
            else if (depo.itemDrop_StartLocation == "Yukarıdan")
            {
                if (depo.itemDrop_LeftRight == "Sağa Doğru")
                {
                    AlternateCell = SearchThroughRightReturnCell(false, false, depo, cell);
                }
                else if (depo.itemDrop_LeftRight == "Sola Doğru")
                {
                    AlternateCell = SearchThroughLeftReturnCell(false, false, depo, cell);
                }
            }
            else if (depo.itemDrop_StartLocation == "Aşağıdan")
            {
                if (depo.itemDrop_LeftRight == "Sağa Doğru")
                {
                    AlternateCell = SearchThroughRightReturnCell(false, true, depo, cell);
                }
                else if (depo.itemDrop_LeftRight == "Sola Doğru")
                {
                    AlternateCell = SearchThroughLeftReturnCell(false, true, depo, cell);
                }
            }

            if (AlternateCell != null)
            {
                foreach (var item1 in cell.items)
                {
                    if (item1 == item)
                    {
                        return (item1, AlternateCell);
                    }
                }
            }
            return (null, null);
        }







        //This is for Canceling finding items to remove from depo
        private void btn_Nesne_Bul_Vazgec_Click(object sender, EventArgs e)
        {
            NesneKaldır = false;
            nesneTimer.Stop();
            PLC_Timer.Stop();
            EmptyPLCSimPanel();

            if (rightLayoutPanel.Visible)
            {
                MainPanelCloseLeftSide(leftLayoutPanel, this);
                MainPanelCloseRightSide(leftLayoutPanel, this);
            }
            else
            {
                MainPanelCloseLeftSide(leftLayoutPanel, this);
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
            if (BlinkingCell != null)
            {
                BlinkingCell.HoverPen = new Pen(System.Drawing.Color.DarkGray);
                BlinkingCell.HoverPen.DashStyle = DashStyle.DashDot;
                BlinkingCell = null;
                DrawingPanel.Invalidate();
            }

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
            MainPanelCloseLeftSide(leftLayoutPanel, this);
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
            }

            if (rightLayoutPanel.Visible && leftLayoutPanel.Visible)
            {
                MainPanelCloseLeftSide(leftLayoutPanel, this);
                MainPanelCloseRightSide(rightLayoutPanel, this);
            }
            else if (rightLayoutPanel.Visible && !leftLayoutPanel.Visible)
            {
                MainPanelCloseRightSide(rightLayoutPanel, this);
            }
            else if (leftLayoutPanel.Visible && !rightLayoutPanel.Visible)
            {
                MainPanelCloseLeftSide(leftLayoutPanel, this);
            }

            using (var context = new DBContext())
            {
                if (Isletme != null)
                {
                    var notlastClosedIsletme = (from x in context.Isletme
                                                where x.IsletmeID != Isletme.IsletmeID
                                                select x).ToList();

                    foreach (var isletme1 in notlastClosedIsletme)
                    {
                        isletme1.LastClosedIsletme = 0;
                        context.SaveChanges();
                    }
                }


                if (ambar != null)
                {
                    var notlastClosedLayouts = (from x in context.Layout
                                                where x.LayoutId != ambar.LayoutId && x.IsletmeID == Isletme.IsletmeID
                                                select x).ToList();

                    foreach (var layout in notlastClosedLayouts)
                    {
                        layout.LastClosedLayout = 0;
                        context.SaveChanges();
                    }
                }

                if (Isletme != null)
                {
                    var isletme = (from x in context.Isletme
                                   where x.IsletmeID == Isletme.IsletmeID
                                   select x).FirstOrDefault();

                    if (isletme != null)
                    {
                        isletme.LastClosedIsletme = 1;
                        context.SaveChanges();

                        if (ambar != null)
                        {
                            var lastClosedLayout = (from x in context.Layout
                                                    where x.LayoutId == ambar.LayoutId && isletme.IsletmeID == x.IsletmeID
                                                    select x).FirstOrDefault();

                            if (lastClosedLayout != null)
                            {
                                lastClosedLayout.LastClosedLayout = 1;
                                context.SaveChanges();
                            }
                        }
                    }
                }
            }

            if (ambar != null)
            {
                foreach (var depo in ambar.depolar)
                {
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
                    }
                }
            }
        }
        //Load Last Closed Layout when opening the Program
        public void LoadFromDB()
        {
            using (var context = new DBContext())
            {
                var isletme = (from x in context.Isletme
                               where x.LastClosedIsletme == 1
                               select x).FirstOrDefault();

                if (isletme != null)
                {
                    Isletme = isletme;
                    lbl_SelectedIsletme_Value.Text = Isletme.Name;
                    lbl_SelectedLayout_Value.Text = "Seçilmedi";

                    var layout = (from x in context.Layout
                                  where x.LastClosedLayout == 1 && x.IsletmeID == isletme.IsletmeID
                                  select x).FirstOrDefault();

                    if (layout != null)
                    {
                        lbl_SelectedLayout_Value.Text = layout.Name;
                        var Dbambar = (from x in context.Ambars
                                       where x.LayoutId == layout.LayoutId
                                       select x).FirstOrDefault();

                        if (Dbambar != null)
                        {
                            Ambar loadedAmbar = new Ambar(Dbambar.KareX, Dbambar.KareY,
                                Dbambar.KareEni, Dbambar.KareBoyu, this, null);

                            loadedAmbar.AmbarId = Dbambar.AmbarId;
                            loadedAmbar.LayoutId = Dbambar.LayoutId;
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
                                newDepo.ItemTuruSecondary = depo.ItemTuruSecondary;

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
                                newDepo.isYerlestirilme = depo.isYerlestirilme;

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
                                    point.OriginalLocationInsideParent = new PointF(reff.OriginalLocationInsideParentX, reff.OriginalLocationInsideParentY);

                                    newConveyor.ConveyorReferencePoints.Add(point);
                                }
                            }
                        }
                    }
                }
                else
                {
                    lbl_SelectedIsletme_Value.Text = "Seçilmedi";
                }
                DrawingPanel.Invalidate();
            }
        }

        #endregion



        //Add Items From Orders
        #region AddItemsFromOrders
        public string GetDesktopPath()
        {
            // Get the path to the Desktop folder
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            return desktopPath;
        }
        public string CreateAndGetDirectoryPath(string subfolderName)
        {
            string rootPath = GetDesktopPath();

            string fullPath = Path.Combine(rootPath, subfolderName);

            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }

            return fullPath;
        }
        public (string, string, string) CreateFolders()
        {
            string rootPath = CreateAndGetDirectoryPath("İş Emirleri");

            var comingWorkOrders = Path.Combine(rootPath, "Gelen iş emirleri");
            var doneWorkOrders = Path.Combine(rootPath, "Biten iş emirleri");
            var failedWorkOrders = Path.Combine(rootPath, "Başarısız iş emirleri");

            if (!Directory.Exists(comingWorkOrders))
            {
                Directory.CreateDirectory(comingWorkOrders);
            }

            if (!Directory.Exists(doneWorkOrders))
            {
                Directory.CreateDirectory(doneWorkOrders);
            }

            if (!Directory.Exists(failedWorkOrders))
            {
                Directory.CreateDirectory(failedWorkOrders);
            }
            return (comingWorkOrders, doneWorkOrders, failedWorkOrders);
        }




        private void toolstripBTN_addItemFromOrders_Click(object sender, EventArgs e)
        {
            ProcessCsvFiles(InputPath, OutputPath, FailedPath);
        }

        public void ProcessCsvFiles(string inputFolderPath, string outputFolderPath, string failedFolderPath)
        {
            // Get all CSV files (txt files) in the specified folder
            string[] files = Directory.GetFiles(inputFolderPath, "*.txt");
            List<List<string>> fileData = new List<List<string>>();
            bool isfilenametoolong = false;

            foreach (var file in files)
            {
                string filename = Path.GetFileNameWithoutExtension(file);
                if (filename.Length > 22)
                {
                    isfilenametoolong = true;
                }
            }

            if (isfilenametoolong)
            {
                MessageBox.Show("Gelen iş emirlerindeki dosyaların isimlerinin uzunluğu 22 karakterden uzun olamaz lütfen kısaltıp tekrar deneyin", "Dosya ismi çok uzun", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isfilenametoolong = false;
            }
            else
            {
                foreach (var file in files)
                {
                    var lineData = ReadCsvFile(file);
                    fileData.Add(lineData);

                    ProcessData(fileData, inputFolderPath, outputFolderPath, failedFolderPath, file);

                    // Delete the processed CSV file
                    //file.delete(file);
                    fileData.Clear();
                }
            }
        }

        public List<string> ReadCsvFile(string filePath)
        {
            var data = new List<string>();

            using (var reader = new StreamReader(filePath))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (line != null)
                    {
                        data.Add(line);
                    }
                }
            }
            return data;
        }
        public int ReturnLargestInt(List<int> list)
        {
            int num = 0;
            foreach (int number in list)
            {
                if (number > num)
                {
                    num = number;
                }
            }
            return num;
        }
        private bool CheckifItemisinLayout(string item_etiketi)
        {
            bool isinlayout = false;
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
                                isinlayout = true;
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Öncelikle bir Layout oluşturmanız ya da yüklemeniz gerekiyor", "Layout yüklü değil", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }

            if (isinlayout)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void ProcessData(List<List<string>> data, string inputFolderPath, string outputFolderPath, string failedFolderPath, string file)
        {
            bool firstLine = true;
            List<string> lines = new List<string>();
            List<string> faultyLines = new List<string>();
            List<Models.Item> items = new List<Models.Item>();

            foreach (var dataLine in data)
            {
                foreach (var line in dataLine)
                {
                    var values = line.Split('/'); // Adjust delimiter if needed

                    string item_etiketi = values[0];
                    string tur_kodu = values[1];
                    string tur_kodu_2 = values[2];
                    string item_aciklamasi = values[3];
                    string item_agirligi_string = values[4];
                    float item_agirligi = 0;

                    if (float.TryParse(item_agirligi_string, out float agirlik))
                    {
                        item_agirligi = agirlik;
                    }
                    if (item_etiketi != "Etiket" && tur_kodu != "Tur Kodu" && tur_kodu_2 != "Tur Kodu 2" && item_aciklamasi != "Aciklama" && item_agirligi_string != "Agirlik")
                    {
                        Depo newDepo = GetDepotoPlaceItem(tur_kodu, tur_kodu_2);

                        if (newDepo != null)
                        {
                            bool isiteminlayout = CheckifItemisinLayout(item_etiketi);

                            if (isiteminlayout)
                            {
                                yerBul = false;
                                //PlaceItem(newDepo, item_etiketi, item_aciklamasi, item_agirligi);

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
                                                    items.Add(item);
                                                }
                                            }
                                            foreach (var item in items)
                                            {
                                                cell.items.Remove(item);
                                            }
                                        }
                                    }
                                }
                                DrawingPanel.Invalidate();
                                lines.Add(line);
                            }
                            else
                            {
                                faultyLines.Add(line);
                            }
                        }
                        else
                        {
                            faultyLines.Add(line);
                        }
                    }
                }
            }

            if (lines.Count > 0)
            {
                string excelFileName = Path.GetFileNameWithoutExtension(file);
                string excelFileName1 = NameSuccessFiles(outputFolderPath, excelFileName);

                WriteToExcel(lines, outputFolderPath, excelFileName1);
            }

            if (faultyLines.Count > 0)
            {
                string excelFileName = Path.GetFileNameWithoutExtension(file);
                string excelFileName1 = NameFailedFiles(failedFolderPath, excelFileName);

                WriteToExcel(faultyLines, failedFolderPath, excelFileName1);
            }
        }
        public string NameFailedFiles(string outputFolderPath, string fileNameForNoFiles)
        {
            string[] files = Directory.GetFiles(outputFolderPath, "*.xlsx");
            List<int> numberlist = new List<int>();
            foreach (var path in files)
            {
                string filename = Path.GetFileNameWithoutExtension(path);
                string compareFilename = filename.Remove(1);
                string beforeParentheses = string.Empty;
                string afterParentheses = string.Empty;
                string beforeParenthese = string.Empty;
                string result = string.Empty;
                int startindex = 0;
                bool isbraced = false;

                if (compareFilename == fileNameForNoFiles)
                {
                    foreach (var item in files)
                    {
                        string filename1 = Path.GetFileNameWithoutExtension(item);

                        if (filename1.Contains('('))
                        {
                            int startIndex = filename1.IndexOf('(');
                            int endIndex = filename1.IndexOf(')');

                            if (startIndex != -1 && endIndex != -1 && endIndex > startIndex + 1)
                            {
                                beforeParenthese = filename1.Substring(0, startIndex);

                                string numberinsideParentheses = filename1.Substring(startIndex + 1, (endIndex - (startIndex + 1)));
                                int resultNumber = Convert.ToInt32(numberinsideParentheses);


                                if (beforeParenthese == fileNameForNoFiles + "---")
                                {
                                    beforeParentheses = filename1.Substring(0, startIndex + 1);
                                    afterParentheses = filename1.Substring(endIndex);
                                    startindex = filename1.IndexOf('(');

                                    isbraced = true;
                                    numberlist.Add(resultNumber);
                                }
                            }
                        }
                    }

                    if (isbraced)
                    {
                        isbraced = false;
                        int resultNumber = ReturnLargestInt(numberlist);
                        resultNumber++;

                        result = beforeParentheses + $"{resultNumber}" + afterParentheses;

                        beforeParenthese = filename.Substring(0, startindex);

                        if (beforeParenthese == fileNameForNoFiles + "---")
                        {
                            filename = result + ".xlsx";
                            numberlist.Clear();
                            return filename;
                        }
                    }
                    else
                    {
                        if (compareFilename == fileNameForNoFiles)
                        {
                            filename = filename + "(1)" + ".xlsx";
                            return filename;
                        }
                    }
                }
            }
            return fileNameForNoFiles + "---" + ".xlsx";
        }
        public string NameSuccessFiles(string outputFolderPath, string fileNameForNoFiles)
        {
            string[] files = Directory.GetFiles(outputFolderPath, "*.xlsx");
            List<int> numberlist = new List<int>();
            foreach (var path in files)
            {
                string filename = Path.GetFileNameWithoutExtension(path);

                string beforeParentheses = string.Empty;
                string afterParentheses = string.Empty;
                string beforeParenthese = string.Empty;
                string result = string.Empty;
                int startindex = 0;
                bool isbraced = false;
                if (filename == fileNameForNoFiles)
                {
                    foreach (var item in files)
                    {
                        string filename1 = Path.GetFileNameWithoutExtension(item);

                        if (filename1.Contains('('))
                        {
                            int startIndex = filename1.IndexOf('(');
                            int endIndex = filename1.IndexOf(')');

                            if (startIndex != -1 && endIndex != -1 && endIndex > startIndex + 1)
                            {
                                beforeParenthese = filename1.Substring(0, startIndex);

                                string numberinsideParentheses = filename1.Substring(startIndex + 1, (endIndex - (startIndex + 1)));
                                int resultNumber = Convert.ToInt32(numberinsideParentheses);


                                if (beforeParenthese == fileNameForNoFiles)
                                {
                                    beforeParentheses = filename1.Substring(0, startIndex + 1);
                                    afterParentheses = filename1.Substring(endIndex);
                                    startindex = filename1.IndexOf('(');

                                    isbraced = true;
                                    numberlist.Add(resultNumber);
                                }
                            }
                        }
                    }

                    if (isbraced)
                    {
                        isbraced = false;
                        int resultNumber = ReturnLargestInt(numberlist);
                        resultNumber++;

                        result = beforeParentheses + $"{resultNumber}" + afterParentheses;

                        beforeParenthese = filename.Substring(0, startindex);

                        if (beforeParenthese == fileNameForNoFiles)
                        {
                            filename = result + ".xlsx";
                            numberlist.Clear();
                            return filename;
                        }
                    }
                    else
                    {
                        if (filename == fileNameForNoFiles)
                        {
                            filename = filename + "(1)" + ".xlsx";
                            return filename;
                        }
                    }
                }
            }
            return fileNameForNoFiles + ".xlsx";
        }
        public void WriteToExcel(List<string> data, string outputPath, string fileName)
        {
            var wb = new XLWorkbook();
            var ws = wb.AddWorksheet($"{fileName}");

            int row = 1;
            int col = 1;
            foreach (var line in data)
            {
                var values = line.Split('/'); // Adjust delimiter if needed
                foreach (var value in values)
                {
                    ws.Cell(row, col).Value = value;
                    col++;
                }
                row++;
                col = 1;
            }
            string path = Path.Combine(outputPath, fileName);
            wb.SaveAs(path);
        }
        #endregion


































        #region ERRORLESS METHODS

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
            if (leftLayoutPanel.Visible && rightLayoutPanel.Visible)
            {
                MainPanelCloseLeftSide(leftLayoutPanel, this);
                MainPanelCloseRightSide(rightLayoutPanel, this);
            }
            else if (leftLayoutPanel.Visible && !rightLayoutPanel.Visible)
            {
                MainPanelCloseLeftSide(leftLayoutPanel, this);
            }
            else if (!leftLayoutPanel.Visible && rightLayoutPanel.Visible)
            {
                MainPanelCloseRightSide(rightLayoutPanel, this);
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
        private void MainPanelOpenLeftSide(System.Windows.Forms.Control showControlLeft,
           System.Windows.Forms.Control parentControl, System.Drawing.Point childControlLocationLeft, System.Windows.Forms.Control controltoShow)
        {
            showControlLeft.Controls.Clear();
            GVisual.ShowControl(controltoShow, showControlLeft);
            if (!showControlLeft.Visible)
            {
                if (rightLayoutPanel.Visible)
                {
                    GVisual.ShowControl(showControlLeft, parentControl, childControlLocationLeft);
                    GVisual.ChangeSize_of_Control(DrawingPanel, drawingPanelSmallSize);
                    DrawingPanel.Location = drawingPanelMiddleLocation;
                    MoveLeft();
                }
                else
                {
                    GVisual.ShowControl(showControlLeft, parentControl, childControlLocationLeft);
                    GVisual.ChangeSize_of_Control(DrawingPanel, drawingPanelMiddleSize);
                    DrawingPanel.Location = drawingPanelMiddleLocation;
                    MoveLeft();
                }
            }
        }
        public void MainPanelOpenRightSide(System.Windows.Forms.Control showControlRight,
            System.Windows.Forms.Control parentControl, System.Drawing.Point childControlLocationRight, System.Windows.Forms.Control controltoShow)
        {
            showControlRight.Controls.Clear();
            GVisual.ShowControl(controltoShow, showControlRight);

            if (!showControlRight.Visible)
            {
                if (leftLayoutPanel.Visible)
                {
                    GVisual.ShowControl(showControlRight, parentControl, childControlLocationRight);
                    GVisual.ChangeSize_of_Control(DrawingPanel, drawingPanelSmallSize);
                    DrawingPanel.Location = drawingPanelMiddleLocation;
                    MoveLeft();
                }
                else
                {
                    GVisual.ShowControl(showControlRight, parentControl, childControlLocationRight);
                    GVisual.ChangeSize_of_Control(DrawingPanel, drawingPanelMiddleSize);
                    DrawingPanel.Location = drawingPanelLeftLocation;
                    MoveLeft();
                }
            }
        }
        public void MainPanelCloseRightSide(System.Windows.Forms.Control hideControlRight,
            System.Windows.Forms.Control parentControl)
        {
            if (hideControlRight.Visible)
            {
                if (leftLayoutPanel.Visible)
                {
                    GVisual.HideControl(hideControlRight, parentControl);
                    GVisual.ChangeSize_of_Control(DrawingPanel, drawingPanelMiddleSize);
                    DrawingPanel.Location = drawingPanelMiddleLocation;
                    MoveRight();
                }
                else
                {
                    GVisual.HideControl(hideControlRight, parentControl);
                    GVisual.ChangeSize_of_Control(DrawingPanel, drawingPanelLargeSize);
                    DrawingPanel.Location = drawingPanelLeftLocation;
                    MoveRight();
                }
            }
        }
        public void MainPanelCloseLeftSide(System.Windows.Forms.Control hideControlLeft,
            System.Windows.Forms.Control parentControl)
        {
            if (hideControlLeft.Visible)
            {
                if (rightLayoutPanel.Visible)
                {
                    GVisual.HideControl(hideControlLeft, parentControl);
                    GVisual.ChangeSize_of_Control(DrawingPanel, drawingPanelMiddleSize);
                    DrawingPanel.Location = drawingPanelLeftLocation;
                    MoveRight();
                }
                else
                {
                    GVisual.HideControl(hideControlLeft, parentControl);
                    GVisual.ChangeSize_of_Control(DrawingPanel, drawingPanelLargeSize);
                    DrawingPanel.Location = drawingPanelLeftLocation;
                    MoveRight();
                }
            }
        }
        public void MoveLeft()
        {
            if (ambar != null)
            {
                ambar.Rectangle = new RectangleF(ambar.Rectangle.X - drawingPanelMoveConst, ambar.Rectangle.Y, ambar.Rectangle.Width, ambar.Rectangle.Height);
                ambar.OriginalRectangle = new RectangleF(ambar.OriginalRectangle.X - drawingPanelMoveConst, ambar.OriginalRectangle.Y, ambar.OriginalRectangle.Width, ambar.OriginalRectangle.Height);
                foreach (var depo in ambar.depolar)
                {
                    depo.Rectangle = new RectangleF(depo.Rectangle.X - drawingPanelMoveConst, depo.Rectangle.Y, depo.Rectangle.Width, depo.Rectangle.Height);
                    depo.OriginalRectangle = new RectangleF(depo.OriginalRectangle.X - drawingPanelMoveConst, depo.OriginalRectangle.Y, depo.OriginalRectangle.Width, depo.OriginalRectangle.Height);

                    depo.LocationofRect = new System.Drawing.Point((int)depo.Rectangle.X, (int)depo.Rectangle.Y);
                    foreach (var cell in depo.gridmaps)
                    {
                        cell.Rectangle = new RectangleF(cell.Rectangle.X - drawingPanelMoveConst, cell.Rectangle.Y, cell.Rectangle.Width, cell.Rectangle.Height);
                        cell.OriginalRectangle = new RectangleF(cell.OriginalRectangle.X - drawingPanelMoveConst, cell.OriginalRectangle.Y, cell.OriginalRectangle.Width, cell.OriginalRectangle.Height);

                        cell.LocationofRect = new System.Drawing.Point((int)cell.Rectangle.X, (int)cell.Rectangle.Y);

                        foreach (var item in cell.items)
                        {
                            item.Rectangle = new RectangleF(item.Rectangle.X - drawingPanelMoveConst, item.Rectangle.Y, item.Rectangle.Width, item.Rectangle.Height);
                            item.OriginalRectangle = new RectangleF(item.OriginalRectangle.X - drawingPanelMoveConst, item.OriginalRectangle.Y, item.OriginalRectangle.Width, item.OriginalRectangle.Height);
                            item.LocationofRect = new System.Drawing.Point((int)item.Rectangle.X, (int)item.Rectangle.Y);
                        }
                    }
                }
                foreach (var conveyor in ambar.conveyors)
                {
                    conveyor.Rectangle = new RectangleF(conveyor.Rectangle.X - drawingPanelMoveConst, conveyor.Rectangle.Y, conveyor.Rectangle.Width, conveyor.Rectangle.Height);
                    conveyor.OriginalRectangle = new RectangleF(conveyor.OriginalRectangle.X - drawingPanelMoveConst, conveyor.OriginalRectangle.Y, conveyor.OriginalRectangle.Width, conveyor.OriginalRectangle.Height);

                    conveyor.LocationofRect = new System.Drawing.Point((int)conveyor.Rectangle.X, (int)conveyor.Rectangle.Y);

                    foreach (var reff in conveyor.ConveyorReferencePoints)
                    {
                        reff.Rectangle = new RectangleF(reff.Rectangle.X - drawingPanelMoveConst,
                            reff.Rectangle.Y, reff.Rectangle.Width, reff.Rectangle.Height);
                        reff.OriginalRectangle = new RectangleF(reff.OriginalRectangle.X - drawingPanelMoveConst, reff.OriginalRectangle.Y, reff.OriginalRectangle.Width, reff.OriginalRectangle.Height);


                        reff.LocationofRect = new System.Drawing.Point((int)reff.Rectangle.X, (int)reff.Rectangle.Y);
                    }
                }
                DrawingPanel.Invalidate();
            }
        }
        public void MoveRight()
        {
            if (ambar != null)
            {
                ambar.Rectangle = new RectangleF(ambar.Rectangle.X + drawingPanelMoveConst, ambar.Rectangle.Y, ambar.Rectangle.Width, ambar.Rectangle.Height);

                ambar.OriginalRectangle = new RectangleF(ambar.OriginalRectangle.X + drawingPanelMoveConst, ambar.OriginalRectangle.Y, ambar.OriginalRectangle.Width, ambar.OriginalRectangle.Height);

                foreach (var depo in ambar.depolar)
                {
                    depo.Rectangle = new RectangleF(depo.Rectangle.X + drawingPanelMoveConst, depo.Rectangle.Y, depo.Rectangle.Width, depo.Rectangle.Height);


                    depo.OriginalRectangle = new RectangleF(depo.OriginalRectangle.X + drawingPanelMoveConst, depo.OriginalRectangle.Y, depo.OriginalRectangle.Width, depo.OriginalRectangle.Height);

                    depo.LocationofRect = new System.Drawing.Point((int)depo.Rectangle.X, (int)depo.Rectangle.Y);
                    foreach (var cell in depo.gridmaps)
                    {
                        cell.Rectangle = new RectangleF(cell.Rectangle.X + drawingPanelMoveConst, cell.Rectangle.Y, cell.Rectangle.Width, cell.Rectangle.Height);
                        cell.OriginalRectangle = new RectangleF(cell.OriginalRectangle.X + drawingPanelMoveConst, cell.OriginalRectangle.Y, cell.OriginalRectangle.Width, cell.OriginalRectangle.Height);

                        cell.LocationofRect = new System.Drawing.Point((int)cell.Rectangle.X, (int)cell.Rectangle.Y);

                        foreach (var item in cell.items)
                        {
                            item.Rectangle = new RectangleF(item.Rectangle.X + drawingPanelMoveConst, item.Rectangle.Y, item.Rectangle.Width, item.Rectangle.Height);
                            item.OriginalRectangle = new RectangleF(item.OriginalRectangle.X + drawingPanelMoveConst, item.OriginalRectangle.Y, item.OriginalRectangle.Width, item.OriginalRectangle.Height);

                            item.LocationofRect = new System.Drawing.Point((int)item.Rectangle.X, (int)item.Rectangle.Y);
                        }
                    }
                }
                foreach (var conveyor in ambar.conveyors)
                {
                    conveyor.Rectangle = new RectangleF(conveyor.Rectangle.X + drawingPanelMoveConst, conveyor.Rectangle.Y, conveyor.Rectangle.Width, conveyor.Rectangle.Height);
                    conveyor.OriginalRectangle = new RectangleF(conveyor.OriginalRectangle.X + drawingPanelMoveConst, conveyor.OriginalRectangle.Y, conveyor.OriginalRectangle.Width, conveyor.OriginalRectangle.Height);

                    conveyor.LocationofRect = new System.Drawing.Point((int)conveyor.Rectangle.X, (int)conveyor.Rectangle.Y);
                    foreach (var reff in conveyor.ConveyorReferencePoints)
                    {
                        reff.Rectangle = new RectangleF(reff.Rectangle.X + drawingPanelMoveConst,
                            reff.Rectangle.Y, reff.Rectangle.Width, reff.Rectangle.Height);

                        reff.OriginalRectangle = new RectangleF(reff.OriginalRectangle.X + drawingPanelMoveConst, reff.OriginalRectangle.Y, reff.OriginalRectangle.Width, reff.OriginalRectangle.Height);

                        reff.LocationofRect = new System.Drawing.Point((int)reff.Rectangle.X, (int)reff.Rectangle.Y);
                    }
                }
                DrawingPanel.Invalidate();
            }
        }

        private void EmptyItemPlacementPanel()
        {
            GVisual.HideControl(Nesne_Yerlestirme_Second_Panel, Nesne_Yerlestirme_First_Panel);
        }
        private void EmptyItemRemovePanel()
        {
            GVisual.HideControl(Nesne_Al_Second_Panel, Nesne_Al_First_Panel);
        }

        #region Hide and Show
        private void HideEverything()
        {
            GVisual.HideControl(Nesne_Yerlestirme_First_Panel, this);
            GVisual.HideControl(Nesne_Yerlestirme_Second_Panel, Nesne_Yerlestirme_First_Panel);
            GVisual.HideControl(PLC_DB_AdressPanel, this);
            GVisual.HideControl(ProgressBarPanel, this);
            GVisual.HideControl(Nesne_Al_First_Panel, this);
            GVisual.HideControl(PLC_Sim_Panel, this);
            GVisual.HideControl(rightLayoutPanel, this);
            GVisual.HideControl(leftLayoutPanel, this);
            GVisual.HideControl(DepoInfoPanel, this);
            GVisual.HideControl(PLC_Sim_Nesne_Buttons_Panel, PLC_Sim_Panel);
            GVisual.HideControl(PLC_Sim_YerSoyle_Panel, PLC_Sim_Panel);
            GVisual.HideControl(PLC_Sim_Yerlestiriliyor_Panel, PLC_Sim_Panel);
            EmptyPLCSimPanel();
            EmptyItemPlacementPanel();
            EmptyItemRemovePanel();
        }
        private void EmptyPLCSimPanel()
        {
            GVisual.HideControl(PLC_Sim_Nesne_Buttons_Panel, PLC_Sim_Panel);
            GVisual.HideControl(PLC_Sim_YerSoyle_Panel, PLC_Sim_Panel);
            GVisual.HideControl(PLC_Sim_Yerlestiriliyor_Panel, PLC_Sim_Panel);
        }
        private void ShowOnLeftOrRightPanel(System.Windows.Forms.Control ShowControl, System.Windows.Forms.Control ParentControl)
        {
            ParentControl.Controls.Clear();

            GVisual.ShowControl(ShowControl, ParentControl);
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
                    Depo newDepo = GetDepotoPlaceItem("depo", "depo");

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
                        PlaceItem(newDepo, item_etiketi, item_aciklamasi, item_agirligi);
                    }
                }
            }
            DrawingPanel.Invalidate();
        }
        //Remove Item from depos but this is for myself to test the algoritm

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




        private void toolStripBTN_Isletme_Sec_Click(object sender, EventArgs e)
        {
            SelectBusiness business = new SelectBusiness(this);
            business.Show();
        }


        private void PopulateDepoInfoPanel(Depo depo)
        {
            string izgara_eni = string.Empty;
            string izgara_boyu = string.Empty;
            string nesne_eni = string.Empty;
            string nesne_boyu = string.Empty;
            string nesne_yuksekligi = string.Empty;

            lbl_DepoInfoMenu_DepoDescription_Value.Text = depo.DepoDescription;
            lbl_DepoInfoMenu_DepoName_Value.Text = depo.DepoName;
            lbl_DepoInfoMenu_DepoItemKind_Value.Text = depo.ItemTuru;
            lbl_DepoInfoMenu_DepoItemKindSecondary_Value.Text = depo.ItemTuruSecondary;
            lbl_DepoInfoMenu_DepoWidth_Value.Text = $"{depo.DepoAlaniEni} Metre";
            lbl_DepoInfoMenu_DepoHeight_Value.Text = $"{depo.DepoAlaniBoyu} Metre";
            lbl_DepoInfoMenu_DepoYuksekligi_Value.Text = $"{depo.DepoAlaniYuksekligi} Cm";

            if (depo.gridmaps != null)
            {
                foreach (var cell in depo.gridmaps)
                {
                    izgara_eni = $"{cell.CellEni} Cm";
                    izgara_boyu = $"{cell.CellBoyu} Cm";
                    nesne_eni = $"{cell.NesneEni} Cm";
                    nesne_boyu = $"{cell.NesneBoyu} Cm";
                    nesne_yuksekligi = $"{cell.NesneYuksekligi} Cm";
                }
                if (izgara_eni != string.Empty)
                {
                    lbl_IzgaraInfoMenu_IzgaraWidth_Value.Text = izgara_eni;
                }
                if (izgara_boyu != string.Empty)
                {
                    lbl_IzgaraInfoMenu_IzgaraHeight_Value.Text = izgara_boyu;
                }
                if (nesne_eni != string.Empty)
                {
                    lbl_NesneInfoMenu_NesneWidth_Value.Text = nesne_eni;
                }
                if (nesne_boyu != string.Empty)
                {
                    lbl_NesneInfoMenu_NesneHeight_Value.Text = nesne_boyu;
                }
                if (nesne_yuksekligi != string.Empty)
                {
                    lbl_NesneInfoMenu_NesneYuksekligi_Value.Text = nesne_yuksekligi;
                }
            }
            else
            {
                lbl_IzgaraInfoMenu_IzgaraHeight_Value.Text = "Izgara Haritası Oluşturulmamış.";
                lbl_IzgaraInfoMenu_IzgaraWidth_Value.Text = "Izgara Haritası Oluşturulmamış.";
                lbl_NesneInfoMenu_NesneHeight_Value.Text = "Izgara Haritası Oluşturulmamış.";
                lbl_NesneInfoMenu_NesneWidth_Value.Text = "Izgara Haritası Oluşturulmamış.";
                lbl_NesneInfoMenu_NesneYuksekligi_Value.Text = "Izgara Haritası Oluşturulmamış.";
            }
        }
        private void ToolStripBTN_OzellikleriGoruntule_Click(object sender, EventArgs e)
        {
            if (selectedDepo != null)
            {
                if (!leftLayoutPanel.Visible)
                {
                    MainPanelOpenLeftSide(leftLayoutPanel, this, leftSidePanelLocation, DepoInfoPanel);
                }
                PopulateDepoInfoPanel(selectedDepo);
            }
        }

        private void btn_Close_DepoInfoPanel_Click(object sender, EventArgs e)
        {
            MainPanelCloseLeftSide(leftLayoutPanel, this);
        }
    }
}
