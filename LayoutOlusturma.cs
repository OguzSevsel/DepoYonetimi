using Balya_Yerleştirme.Models;
using CustomNotification;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Drawing.Wordprocessing;
using DocumentFormat.OpenXml.Office2013.Drawing.Chart;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using GUI_Library;
using Krypton.Toolkit;
using SixLabors.Fonts;
using String_Library;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Balya_Yerleştirme.Utilities.Utils;

namespace Balya_Yerleştirme
{
    public partial class LayoutOlusturma : Form
    {
        public float drawingPanelMoveConstant = 152.5f;
        public float Cm_Degeri { get; set; } = 0f;
        public float pxX { get; set; } = 0f;
        public float txt_left { get; set; } = 0f;
        public bool isMoving { get; set; } = false;
        public bool ToolStripIzgara { get; set; } = false;
        public bool MovingParameter { get; set; } = false;
        public float UnchangedDepoAlaniEni { get; set; }
        public float UnchangedDepoAlaniBoyu { get; set; }
        public float UnchangedConveyorEni { get; set; }
        public float UnchangedConveyorBoyu { get; set; }

        List<RectangleF> rectangles = new List<RectangleF>();

        public string Parameter = String.Empty;
        public int rowCount { get; set; }
        public int currentRow { get; set; } = 0;
        public int colCount { get; set; }
        public int currentColumn { get; set; } = 0;
        public bool snapped { get; set; } = false;
        public bool Manuel_Move { get; set; } = false;
        public bool Fill_WareHouse { get; set; } = false;
        public bool AddReferencePoint { get; set; } = false;
        public int Ware_Counter { get; set; } = 1;
        public List<int> CountList { get; set; } = new List<int>();

        public List<Panel> SidePanels;
        public System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

        public System.Drawing.Point drawingPanelLargeLocation { get; set; } = new System.Drawing.Point(12, 102);
        public System.Drawing.Point drawingPanelSmallLocation { get; set; } = new System.Drawing.Point(323, 102);
        public System.Drawing.Point leftSidePanelLocation { get; set; } = new System.Drawing.Point(12, 102);
        public System.Drawing.Size drawingPanelSmallSize { get; set; } = new System.Drawing.Size(1462, 690);
        public System.Drawing.Size drawingPanelLargeSize { get; set; } = new System.Drawing.Size(1771, 690);


        public Depo? selDepo { get; set; }
        public Conveyor? selConveyor { get; set; }


        public MainForm Main { get; set; }
        public Ambar? Ambar { get; set; }
        public Conveyor? selectedConveyor { get; set; } = null;
        public RectangleF selectedConveyorRectangle { get; set; } = new RectangleF();
        public RectangleF UnchangedselectedConveyorRectangle { get; set; } = new RectangleF();
        public RectangleF ProxRectRight { get; set; } = new RectangleF();
        public RectangleF ProxRectLeft { get; set; } = new RectangleF();
        public RectangleF ProxRectTop { get; set; } = new RectangleF();
        public RectangleF ProxRectBottom { get; set; } = new RectangleF();
        public RectangleF ManuelDepoRectangle { get; set; } = new RectangleF();
        public RectangleF ManuelConveyorRectangle { get; set; } = new RectangleF();



        public Depo? selectedDepo { get; set; } = null;
        public RectangleF selectedDepoRectangle { get; set; } = new RectangleF();
        public RectangleF UnchangedselectedDepoRectangle { get; set; } = new RectangleF();


        public float nesne_Eni { get; set; }
        public float nesne_Boyu { get; set; }
        public int nesne_Yuksekligi { get; set; }
        public float total_Cell_Width { get; set; }
        public float total_Cell_Height { get; set; }
        public int toplam_nesne_yuksekligi { get; set; }
        public int toplam_nesne_yuksekligi_asama2 { get; set; }
        public int nesne_sayisi { get; set; }
        public float txt_width { get; set; }
        public float txt_height { get; set; }


        public Pen SelectedDepoPen { get; set; } = new Pen(System.Drawing.Color.Black);
        public Pen SelectedConveyorPen { get; set; } = new Pen(System.Drawing.Color.Black);
        public Pen SelectedDepoEdgePen { get; set; } = new Pen(System.Drawing.Color.Red);
        public Pen SelectedConveyorEdgePen { get; set; } = new Pen(System.Drawing.Color.Red);
        public Pen TransparentPen { get; set; } = new Pen(System.Drawing.Color.Black, 2);
        public EventHandler izgaraHaritasiOlustur { get; set; }

        public Depo? CopyDepo { get; set; }
        public Conveyor? CopyConveyor { get; set; }
        public PointF CopyPoint { get; set; }
        public string LayoutName { get; set; }

        public string LayoutDescription { get; set; }
        public bool ambar_Boyut_Degistir = false;
        public bool askOnce = true;
        public Models.Cell DataCell { get; set; } = new Models.Cell();

        public float cell_eni = 0f;
        public float cell_boyu = 0f;
        public float yatay_kenar_boslugu = 0f;
        public float dikey_kenar_boslugu = 0f;
        public float nesne_eni = 0f;
        public float nesne_boyu = 0f;

        public LayoutOlusturma(MainForm main)
        {
            InitializeComponent();
            ToolTip toolTip = new ToolTip();
            toolTip.AutoPopDelay = 2500;
            toolTip.InitialDelay = 100;
            toolTip.ReshowDelay = 250;
            toolTip.ShowAlways = true;
            toolTip.SetToolTip(this.btn_Alan, "Alan oluştur.");
            toolTip.SetToolTip(this.btn_Conveyor, "Conveyor oluştur.");
            toolTip.SetToolTip(this.btn_Depo, "Depo oluştur.");
            toolTip.SetToolTip(this.btn_Izgara_Haritasi, "Tüm Depolara ızgara haritası ekle.");


            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.KeyDown += KeyDownEventHandler;
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
            CopyDepo = new Depo();
            CopyConveyor = new Conveyor();
            GVisual.SetDoubleBuffered(drawingPanel);
            izgaraHaritasiOlustur += izgaraHaritasiOlusturEventHandler;
            Main = main;
            SidePanels = new List<Panel>();
            SidePanels.Add(Alan_Olusturma_Paneli);
            SidePanels.Add(Izgara_Mal_Paneli);
            SidePanels.Add(Izgara_Olusturma_Paneli);
            SidePanels.Add(Depo_Olusturma_Paneli);
            SidePanels.Add(Conveyor_Olusturma_Paneli);
            HideEverything();
            DrawingPanelShrink(Alan_Olusturma_Paneli, this, leftSidePanelLocation);
            timer.Tick += Timer_Tick;
        }

        private void KeyDownEventHandler(object? sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                if (Ambar != null)
                {
                    if (selectedDepo != null)
                    {
                        CopyConveyor = null;
                        CopyDepo = new Depo(selectedDepo.Rectangle.X, selectedDepo.Rectangle.Y,
                            selectedDepo.Rectangle.Width, selectedDepo.Rectangle.Height,
                            selectedDepo.Zoomlevel, null, this, Ambar);

                        CopyDepo.OriginalRectangle = selectedDepo.OriginalRectangle;

                        CopyDepo.KareX = CopyDepo.Rectangle.X;
                        CopyDepo.KareY = CopyDepo.Rectangle.Y;
                        CopyDepo.KareEni = CopyDepo.Rectangle.Width;
                        CopyDepo.KareBoyu = CopyDepo.Rectangle.Height;
                        CopyDepo.OriginalKareX = CopyDepo.OriginalRectangle.X;
                        CopyDepo.OriginalKareY = CopyDepo.OriginalRectangle.Y;
                        CopyDepo.OriginalKareEni = CopyDepo.OriginalRectangle.Width;
                        CopyDepo.OriginalKareBoyu = CopyDepo.OriginalRectangle.Height;
                        CopyDepo.OriginalDepoSizeWidth = CopyDepo.OriginalRectangle.Width;
                        CopyDepo.OriginalDepoSizeHeight = CopyDepo.OriginalRectangle.Height;
                        CopyDepo.OriginalDepoSize = new SizeF(CopyDepo.OriginalRectangle.Width, CopyDepo.OriginalRectangle.Height);

                        CopyDepo.DepoAlaniEni = selectedDepo.DepoAlaniEni;
                        CopyDepo.DepoAlaniBoyu = selectedDepo.DepoAlaniBoyu;
                        CopyDepo.DepoAlaniYuksekligi = selectedDepo.DepoAlaniYuksekligi;

                        CopyDepo.nesneYuksekligi = selectedDepo.nesneYuksekligi;
                        CopyDepo.nesneEni = selectedDepo.nesneEni;
                        CopyDepo.nesneBoyu = selectedDepo.nesneBoyu;

                        CopyDepo.DepoName = selectedDepo.DepoName;
                        CopyDepo.DepoDescription = selectedDepo.DepoDescription;

                        CopyDepo.RowCount = selectedDepo.RowCount;
                        CopyDepo.ColumnCount = selectedDepo.ColumnCount;
                        CopyDepo.currentColumn = selectedDepo.currentColumn;
                        CopyDepo.currentRow = selectedDepo.currentRow;
                        CopyDepo.asama1_ItemSayisi = selectedDepo.asama1_ItemSayisi;
                        CopyDepo.asama2_ToplamItemSayisi = selectedDepo.asama2_ToplamItemSayisi;
                        CopyDepo.ItemTuru = selectedDepo.ItemTuru;
                        CopyDepo.currentStage = selectedDepo.currentStage;

                        CopyDepo.Yerlestirilme_Sirasi = selectedDepo.Yerlestirilme_Sirasi;
                        CopyDepo.asama1_Yuksekligi = selectedDepo.asama1_Yuksekligi;
                        CopyDepo.asama2_Yuksekligi = selectedDepo.asama2_Yuksekligi;
                        CopyDepo.itemDrop_LeftRight = selectedDepo.itemDrop_LeftRight;
                        CopyDepo.itemDrop_UpDown = selectedDepo.itemDrop_UpDown;
                        CopyDepo.itemDrop_StartLocation = selectedDepo.itemDrop_StartLocation;


                        foreach (var cell in selectedDepo.gridmaps)
                        {
                            float cell_start_X = selectedDepo.Rectangle.X - cell.Rectangle.X;
                            float cell_start_Y = selectedDepo.Rectangle.Y - cell.Rectangle.Y;
                            float cellX = CopyDepo.Rectangle.X + cell_start_X;
                            float cellY = CopyDepo.Rectangle.Y + cell_start_Y;

                            Models.Cell CopiedCell = new Models.Cell(cellX, cellY, cell.Rectangle.Width, cell.Rectangle.Height, Main, CopyDepo, this);

                            CopiedCell.OriginalRectangle = cell.OriginalRectangle;

                            CopiedCell.KareX = CopiedCell.Rectangle.X;
                            CopiedCell.KareY = CopiedCell.Rectangle.Y;
                            CopiedCell.KareEni = CopiedCell.Rectangle.Width;
                            CopiedCell.KareBoyu = CopiedCell.Rectangle.Height;
                            CopiedCell.OriginalKareX = CopiedCell.OriginalRectangle.X;
                            CopiedCell.OriginalKareY = CopiedCell.OriginalRectangle.Y;
                            CopiedCell.OriginalKareEni = CopiedCell.OriginalRectangle.Width;
                            CopiedCell.OriginalKareBoyu = CopiedCell.OriginalRectangle.Height;
                            CopiedCell.CellEni = cell.CellEni;
                            CopiedCell.CellBoyu = cell.CellBoyu;
                            CopiedCell.CellEtiketi = cell.CellEtiketi;
                            CopiedCell.CellMalSayisi = cell.CellMalSayisi;
                            CopiedCell.CellYuksekligi = cell.CellYuksekligi;
                            CopiedCell.DikeyKenarBoslugu = cell.DikeyKenarBoslugu;
                            CopiedCell.YatayKenarBoslugu = cell.YatayKenarBoslugu;
                            CopiedCell.NesneYuksekligi = cell.NesneYuksekligi;
                            CopiedCell.NesneEni = cell.NesneEni;
                            CopiedCell.NesneBoyu = cell.NesneBoyu;
                            CopiedCell.Parent = CopyDepo;
                            CopiedCell.Row = cell.Row;
                            CopiedCell.Column = cell.Column;
                            CopiedCell.toplam_Nesne_Yuksekligi = cell.toplam_Nesne_Yuksekligi;
                            CopiedCell.Zoomlevel = cell.Zoomlevel;
                            CopiedCell.cell_Cm_X = cell.cell_Cm_X;
                            CopiedCell.cell_Cm_Y = cell.cell_Cm_Y;

                            CopyDepo.gridmaps.Add(CopiedCell);
                        }
                    }
                    if (selectedConveyor != null)
                    {
                        CopyDepo = null;
                        CopyConveyor = new Conveyor(selectedConveyor.Rectangle.X, selectedConveyor.Rectangle.Y,
                            selectedConveyor.Rectangle.Width, selectedConveyor.Rectangle.Height, null, this, Ambar);

                        CopyConveyor.OriginalRectangle = CopyConveyor.Rectangle;

                        CopyConveyor.KareX = CopyConveyor.Rectangle.X;
                        CopyConveyor.KareY = CopyConveyor.Rectangle.Y;
                        CopyConveyor.KareEni = CopyConveyor.Rectangle.Width;
                        CopyConveyor.KareBoyu = CopyConveyor.Rectangle.Height;
                        CopyConveyor.OriginalKareX = CopyConveyor.OriginalRectangle.X;
                        CopyConveyor.OriginalKareY = CopyConveyor.OriginalRectangle.Y;
                        CopyConveyor.OriginalKareEni = CopyConveyor.OriginalRectangle.Width;
                        CopyConveyor.OriginalKareBoyu = CopyConveyor.OriginalRectangle.Height;
                        CopyConveyor.Parent = Ambar;
                        CopyConveyor.ConveyorEni = selectedConveyor.ConveyorEni;
                        CopyConveyor.ConveyorBoyu = selectedConveyor.ConveyorBoyu;
                        CopyConveyor.Zoomlevel = selectedConveyor.Zoomlevel;

                        foreach (var reff in selectedConveyor.ConveyorReferencePoints)
                        {
                            float reff_start_X = selectedConveyor.Rectangle.X - reff.Rectangle.X;
                            float reff_start_Y = selectedConveyor.Rectangle.Y - reff.Rectangle.Y;
                            float reffX = CopyConveyor.Rectangle.X + reff_start_X;
                            float reffY = CopyConveyor.Rectangle.Y + reff_start_Y;

                            ConveyorReferencePoint newReff = new ConveyorReferencePoint(reffX, reffY, reff.Rectangle.Width, reff.Rectangle.Height, reff.Zoomlevel, null, CopyConveyor, this);

                            newReff.KareX = newReff.Rectangle.X;
                            newReff.KareY = newReff.Rectangle.Y;
                            newReff.KareEni = newReff.Rectangle.Width;
                            newReff.KareBoyu = newReff.Rectangle.Height;
                            newReff.OriginalKareX = newReff.OriginalRectangle.X;
                            newReff.OriginalKareY = newReff.OriginalRectangle.Y;
                            newReff.OriginalKareEni = newReff.OriginalRectangle.Width;
                            newReff.OriginalKareBoyu = newReff.OriginalRectangle.Height;
                            newReff.ParentConveyor = CopyConveyor;
                            newReff.FixedPointLocation = reff.FixedPointLocation;
                            newReff.Info = reff.Info;
                            newReff.Pointsize = 4;

                            float X = newReff.Rectangle.X - CopyConveyor.Rectangle.X;
                            float Y = newReff.Rectangle.Y - CopyConveyor.Rectangle.Y;

                            newReff.OriginalLocationInsideParent = new PointF(X, Y);

                            CopyConveyor.ConveyorReferencePoints.Add(newReff);
                        }
                    }
                }
            }

            if (e.Control && e.KeyCode == Keys.V)
            {
                if (Ambar != null)
                {
                    if (CopyDepo != null)
                    {
                        string deponame = CopyDepo.DepoName;

                        PointF checkPoint = new PointF(CopyPoint.X + CopyDepo.Rectangle.Width / 2, CopyPoint.Y + CopyDepo.Rectangle.Height / 2);
                        if (Ambar.Rectangle.Contains(checkPoint))
                        {
                            Depo CopiedDepo = new Depo(CopyPoint.X - CopyDepo.Rectangle.Width / 2,
                                CopyPoint.Y - CopyDepo.Rectangle.Height / 2, CopyDepo.Rectangle.Width,
                            CopyDepo.Rectangle.Height, CopyDepo.Zoomlevel, Main, this, Ambar);

                            CopiedDepo.OriginalRectangle = CopiedDepo.Rectangle;

                            CopiedDepo.KareX = CopiedDepo.Rectangle.X;
                            CopiedDepo.KareY = CopiedDepo.Rectangle.Y;
                            CopiedDepo.KareEni = CopiedDepo.Rectangle.Width;
                            CopiedDepo.KareBoyu = CopiedDepo.Rectangle.Height;
                            CopiedDepo.OriginalKareX = CopiedDepo.OriginalRectangle.X;
                            CopiedDepo.OriginalKareY = CopiedDepo.OriginalRectangle.Y;
                            CopiedDepo.OriginalKareEni = CopiedDepo.OriginalRectangle.Width;
                            CopiedDepo.OriginalKareBoyu = CopiedDepo.OriginalRectangle.Height;

                            CopiedDepo.DepoAlaniEni = CopyDepo.DepoAlaniEni;
                            CopiedDepo.DepoAlaniBoyu = CopyDepo.DepoAlaniBoyu;
                            CopiedDepo.DepoAlaniYuksekligi = CopyDepo.DepoAlaniYuksekligi;

                            CopiedDepo.nesneYuksekligi = CopyDepo.nesneYuksekligi;
                            CopiedDepo.nesneEni = CopyDepo.nesneEni;
                            CopiedDepo.nesneBoyu = CopyDepo.nesneBoyu;

                            CopiedDepo.DepoName = deponame;
                            CopiedDepo.DepoDescription = CopyDepo.DepoDescription;

                            CopiedDepo.RowCount = CopyDepo.RowCount;
                            CopiedDepo.ColumnCount = CopyDepo.ColumnCount;
                            CopiedDepo.currentColumn = CopyDepo.currentColumn;
                            CopiedDepo.currentRow = CopyDepo.currentRow;


                            CopiedDepo.OriginalDepoSizeWidth = CopyDepo.OriginalRectangle.Width;
                            CopiedDepo.OriginalDepoSizeHeight = CopyDepo.OriginalRectangle.Height;
                            CopiedDepo.OriginalDepoSize = new SizeF(CopiedDepo.OriginalDepoSizeWidth, CopiedDepo.OriginalDepoSizeHeight);

                            int count = 0;
                            foreach (var depo in Ambar.depolar)
                            {
                                if (depo.Yerlestirilme_Sirasi > count || count == 0)
                                {
                                    count = depo.Yerlestirilme_Sirasi;
                                }
                            }

                            int number = count + 1;
                            CopiedDepo.Yerlestirilme_Sirasi = number;
                            CopiedDepo.asama1_Yuksekligi = CopyDepo.DepoAlaniYuksekligi / 2;
                            CopiedDepo.asama2_Yuksekligi = CopyDepo.DepoAlaniYuksekligi;
                            CopiedDepo.itemDrop_LeftRight = CopyDepo.itemDrop_LeftRight;
                            CopiedDepo.itemDrop_UpDown = CopyDepo.itemDrop_UpDown;
                            CopiedDepo.itemDrop_StartLocation = CopyDepo.itemDrop_StartLocation;
                            CopiedDepo.currentStage = CopyDepo.currentStage;
                            CopiedDepo.asama1_ItemSayisi = CopyDepo.asama1_ItemSayisi;
                            CopiedDepo.asama2_ToplamItemSayisi = CopyDepo.asama2_ToplamItemSayisi;
                            CopiedDepo.ItemTuru = CopyDepo.ItemTuru;

                            foreach (var cell in CopyDepo.gridmaps)
                            {
                                float cell_start_X = CopyDepo.Rectangle.X - cell.Rectangle.X;
                                float cell_start_Y = CopyDepo.Rectangle.Y - cell.Rectangle.Y;
                                float cellX = CopiedDepo.Rectangle.X + cell_start_X;
                                float cellY = CopiedDepo.Rectangle.Y + cell_start_Y;

                                Models.Cell CopiedCell = new Models.Cell(cellX, cellY, cell.Rectangle.Width, cell.Rectangle.Height, Main, CopiedDepo, this);

                                CopiedCell.OriginalRectangle = CopiedCell.Rectangle;

                                CopiedCell.KareX = CopiedCell.Rectangle.X;
                                CopiedCell.KareY = CopiedCell.Rectangle.Y;
                                CopiedCell.KareEni = CopiedCell.Rectangle.Width;
                                CopiedCell.KareBoyu = CopiedCell.Rectangle.Height;
                                CopiedCell.OriginalKareX = CopiedCell.OriginalRectangle.X;
                                CopiedCell.OriginalKareY = CopiedCell.OriginalRectangle.Y;
                                CopiedCell.OriginalKareEni = CopiedCell.OriginalRectangle.Width;
                                CopiedCell.OriginalKareBoyu = CopiedCell.OriginalRectangle.Height;
                                CopiedCell.CellEni = cell.CellEni;
                                CopiedCell.CellBoyu = cell.CellBoyu;
                                CopiedCell.CellEtiketi = cell.CellEtiketi;
                                CopiedCell.CellMalSayisi = cell.CellMalSayisi;
                                CopiedCell.CellYuksekligi = cell.CellYuksekligi;
                                CopiedCell.DikeyKenarBoslugu = cell.DikeyKenarBoslugu;
                                CopiedCell.YatayKenarBoslugu = cell.YatayKenarBoslugu;
                                CopiedCell.NesneYuksekligi = cell.NesneYuksekligi;
                                CopiedCell.NesneEni = cell.NesneEni;
                                CopiedCell.NesneBoyu = cell.NesneBoyu;
                                CopiedCell.Parent = CopiedDepo;
                                CopiedCell.Row = cell.Row;
                                CopiedCell.Column = cell.Column;
                                CopiedCell.toplam_Nesne_Yuksekligi = cell.toplam_Nesne_Yuksekligi;
                                CopiedCell.Zoomlevel = cell.Zoomlevel;
                                CopiedCell.cell_Cm_X = cell.cell_Cm_X;
                                CopiedCell.cell_Cm_Y = cell.cell_Cm_Y;

                                CopiedDepo.gridmaps.Add(CopiedCell);
                            }

                            bool ifintersects = CheckifIntersects(CopiedDepo.Rectangle, RectangleF.Empty, CopiedDepo, null);

                            if (ifintersects)
                            {
                                CustomNotifyIcon notify = new CustomNotifyIcon();
                                notify.showAlert("Başka bir alana yapıştıramazsınız.", CustomNotifyIcon.enmType.Info);
                            }
                            else
                            {
                                Ambar.depolar.Add(CopiedDepo);
                                int num = 0;
                                foreach (var depo in Ambar.depolar)
                                {
                                    if (depo.DepoName.Contains('('))
                                    {
                                        int startIndex = deponame.IndexOf('(');
                                        int endIndex = deponame.IndexOf(')');

                                        if (startIndex != -1 && endIndex != -1 && endIndex > startIndex + 1)
                                        {
                                            if (depo.DepoName == deponame)
                                            {
                                                string beforeParentheses = deponame.Substring(0, startIndex + 1);
                                                string afterParentheses = deponame.Substring(endIndex);

                                                string numberinsideParentheses = depo.DepoName.Substring(startIndex + 1, (endIndex - (startIndex + 1)));

                                                int resultNumber = Convert.ToInt32(numberinsideParentheses);

                                                resultNumber++;

                                                string result = beforeParentheses + $"{resultNumber}" + afterParentheses;
                                                deponame = result;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (depo.DepoName == deponame)
                                        {
                                            deponame = $"{deponame} (1)";
                                        }
                                    }
                                }
                                CopiedDepo.DepoName = deponame;
                                drawingPanel.Invalidate();
                            }
                        }
                        else
                        {
                            CustomNotifyIcon notify = new CustomNotifyIcon();
                            notify.showAlert("Depoyu sadece alanın içerisine yapıştırabilirsiniz.", CustomNotifyIcon.enmType.Info);
                        }
                    }

                    if (CopyConveyor != null)
                    {
                        PointF checkPoint = new PointF(CopyPoint.X + CopyConveyor.Rectangle.Width / 2, CopyPoint.Y + CopyConveyor.Rectangle.Height / 2);

                        if (Ambar.Rectangle.Contains(checkPoint))
                        {
                            Conveyor conveyor = new Conveyor(CopyPoint.X - CopyConveyor.Rectangle.Width / 2, CopyPoint.Y - CopyConveyor.Rectangle.Height / 2, CopyConveyor.Rectangle.Width, CopyConveyor.Rectangle.Height,
                            Main, this, Ambar);

                            conveyor.OriginalRectangle = conveyor.Rectangle;

                            conveyor.KareX = conveyor.Rectangle.X;
                            conveyor.KareY = conveyor.Rectangle.Y;
                            conveyor.KareEni = conveyor.Rectangle.Width;
                            conveyor.KareBoyu = conveyor.Rectangle.Height;
                            conveyor.OriginalKareX = conveyor.OriginalRectangle.X;
                            conveyor.OriginalKareY = conveyor.OriginalRectangle.Y;
                            conveyor.OriginalKareEni = conveyor.OriginalRectangle.Width;
                            conveyor.OriginalKareBoyu = conveyor.OriginalRectangle.Height;
                            conveyor.ConveyorEni = CopyConveyor.ConveyorEni;
                            conveyor.ConveyorBoyu = CopyConveyor.ConveyorBoyu;

                            foreach (var reff in CopyConveyor.ConveyorReferencePoints)
                            {
                                float reff_start_X = CopyConveyor.Rectangle.X - reff.Rectangle.X;
                                float reff_start_Y = CopyConveyor.Rectangle.Y - reff.Rectangle.Y;
                                float reffX = conveyor.Rectangle.X + reff_start_X;
                                float reffY = conveyor.Rectangle.Y + reff_start_Y;

                                ConveyorReferencePoint newReff = new ConveyorReferencePoint(reffX, reffY, reff.Rectangle.Width, reff.Rectangle.Height, reff.Zoomlevel, Main, conveyor, this);

                                newReff.KareX = newReff.Rectangle.X;
                                newReff.KareY = newReff.Rectangle.Y;
                                newReff.KareEni = newReff.Rectangle.Width;
                                newReff.KareBoyu = newReff.Rectangle.Height;
                                newReff.OriginalKareX = newReff.OriginalRectangle.X;
                                newReff.OriginalKareY = newReff.OriginalRectangle.Y;
                                newReff.OriginalKareEni = newReff.OriginalRectangle.Width;
                                newReff.OriginalKareBoyu = newReff.OriginalRectangle.Height;
                                newReff.ParentConveyor = conveyor;
                                newReff.FixedPointLocation = reff.FixedPointLocation;
                                newReff.Info = reff.Info;
                                newReff.Pointsize = 4;

                                float X = newReff.Rectangle.X - conveyor.Rectangle.X;
                                float Y = newReff.Rectangle.Y - conveyor.Rectangle.Y;

                                newReff.OriginalLocationInsideParent = new PointF(X, Y);

                                conveyor.ConveyorReferencePoints.Add(newReff);
                            }


                            bool ifintersects = CheckifIntersects(RectangleF.Empty, conveyor.Rectangle, null, conveyor);

                            if (ifintersects)
                            {
                                CustomNotifyIcon notify = new CustomNotifyIcon();
                                notify.showAlert("Başka bir alana yapıştıramazsınız.", CustomNotifyIcon.enmType.Info);
                            }
                            else
                            {
                                Ambar.conveyors.Add(conveyor);
                                drawingPanel.Invalidate();
                            }
                        }
                        else
                        {
                            CustomNotifyIcon notify = new CustomNotifyIcon();
                            notify.showAlert("Conveyor'u sadece alanın içerisine yapıştırabilirsiniz.", CustomNotifyIcon.enmType.Info);
                        }
                    }
                }
            }

            if (e.KeyCode == Keys.Delete)
            {
                //List<Depo> depos = new List<Depo>();
                //List<Conveyor> conveyors = new List<Conveyor>();

                //if (selectedDepo != null)
                //    depos.Add(selectedDepo);
                //}
                //if (Ambar != null)
                //{
                //    foreach (var depo in depos)
                //    {
                //        Ambar.depolar.Remove(depo);
                //    }
                //}

                //if (selectedConveyor != null)
                //{
                //    conveyors.Add(selectedConveyor);
                //}

                //if (Ambar != null)
                //{
                //    foreach (var conv in conveyors)
                //    {
                //        Ambar.conveyors.Remove(conv);
                //    }
                //}
                if (Ambar != null)
                {
                    if (selectedDepo != null)
                    {
                        Ambar.depolar.Remove(selectedDepo);
                    }

                    if (selectedConveyor != null)
                    {
                        Ambar.conveyors.Remove(selectedConveyor);
                    }
                }
                drawingPanel.Invalidate();
            }
        }
        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (Parameter == "Sola Doğru")
            {
                drawingPanel.Invalidate();
                currentColumn--;
                if (currentColumn == -1)
                {
                    currentColumn = colCount;
                    rectangles.Clear();
                }
            }
            else if (Parameter == "Sağa Doğru")
            {
                drawingPanel.Invalidate();
                currentColumn++;
                if (currentColumn == colCount + 2)
                {
                    currentColumn = 0;
                    rectangles.Clear();
                }
            }
            else if (Parameter == "Yukarı Doğru")
            {
                drawingPanel.Invalidate();
                currentRow--;
                if (currentRow == -1)
                {
                    currentRow = (rowCount);
                    rectangles.Clear();
                }
            }
            else if (Parameter == "Ortadan Yukarı Doğru")
            {
                drawingPanel.Invalidate();
                currentRow--;
                if (currentRow == -1)
                {
                    currentRow = (rowCount / 2);
                    rectangles.Clear();
                }
            }
            else if (Parameter == "Aşağı Doğru")
            {
                drawingPanel.Invalidate();
                currentRow++;
                if (currentRow == rowCount + 2)
                {
                    currentRow = 0;
                    rectangles.Clear();
                }
            }
            else if (Parameter == "Ortadan Aşağı Doğru")
            {
                drawingPanel.Invalidate();
                currentRow++;
                if (currentRow == rowCount + 2)
                {
                    currentRow = (rowCount / 2 + 2);
                    rectangles.Clear();
                }
            }
        }
        private void radio_To_Right_CheckedChanged(object sender, EventArgs e)
        {
            currentColumn = 0;
            timer.Stop();
            if (radio_To_Right.Checked)
            {
                rectangles.Clear();
                timer.Interval = 300;
                timer.Start();
                Parameter = "Sağa Doğru";
            }
        }
        private void radio_To_Left_CheckedChanged(object sender, EventArgs e)
        {
            currentColumn = colCount;
            timer.Stop();
            if (radio_To_Left.Checked)
            {
                rectangles.Clear();
                timer.Interval = 300;
                timer.Start();
                Parameter = "Sola Doğru";
            }
        }
        private void radio_To_Up_CheckedChanged(object sender, EventArgs e)
        {
            currentRow = rowCount / 2 + 1;
            timer.Stop();
            if (radio_To_Up.Checked)
            {
                rectangles.Clear();
                timer.Interval = 300;
                timer.Start();
                Parameter = "Ortadan Yukarı Doğru";
            }
        }
        private void radio_To_Down_CheckedChanged(object sender, EventArgs e)
        {
            currentRow = (rowCount / 2 + 1);
            timer.Stop();
            if (radio_To_Down.Checked)
            {
                rectangles.Clear();
                timer.Interval = 300;
                timer.Start();
                Parameter = "Ortadan Aşağı Doğru";
            }
        }
        private void radio_Start_From_Bottom_CheckedChanged(object sender, EventArgs e)
        {
            currentRow = rowCount;
            timer.Stop();
            if (radio_Start_From_Bottom.Checked)
            {
                rectangles.Clear();
                timer.Interval = 300;
                timer.Start();
                Parameter = "Yukarı Doğru";
            }
        }
        private void radio_Start_From_Top_CheckedChanged(object sender, EventArgs e)
        {
            currentRow = 0;
            timer.Stop();
            if (radio_Start_From_Top.Checked)
            {
                rectangles.Clear();
                timer.Interval = 300;
                timer.Start();
                Parameter = "Aşağı Doğru";
            }
        }



        private void SimulationPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            List<RectangleF> rects = new List<RectangleF>();

            if (Ambar != null)
            {
                //Ambar.Draw(g);
                foreach (var item in Ambar.conveyors)
                {
                    rects.Add(item.Rectangle);
                }
                foreach (var item in Ambar.depolar)
                {
                    rects.Add(item.Rectangle);
                }
            }


            if (Ambar != null)
            {
                g.DrawRectangle(TransparentPen, Ambar.Rectangle);

                PointF AmbarTop = GVisual.GetMiddleOfTopEdgeF(Ambar.Rectangle);
                PointF AmbarBottom = GVisual.GetMiddleOfBottomEdgeF(Ambar.Rectangle);

                Pen pen2 = new Pen(System.Drawing.Color.Yellow, 2);
                pen2.DashStyle = DashStyle.DashDot;

                g.DrawLine(pen2, AmbarTop, AmbarBottom);

                foreach (var conv in Ambar.conveyors)
                {
                    g.DrawRectangle(TransparentPen, conv.Rectangle);

                    if (selectedConveyor != null && selectedConveyor == conv &&
                        Fill_WareHouse == false)
                    {
                        g.DrawRectangle(SelectedConveyorPen, conv.Rectangle);


                        //g.FillRectangle(new SolidBrush(System.Drawing.Color.AliceBlue),
                        //    conv.Rectangle);
                        SelectedConveyorEdgePen.DashStyle = DashStyle.Dash;
                        DrawLeftLines(SelectedConveyorEdgePen, g, rects, conv.Rectangle,
                            Ambar.Rectangle);
                        DrawRightLines(SelectedConveyorEdgePen, g, rects, conv.Rectangle,
                            Ambar.Rectangle);
                        DrawTopLines(SelectedConveyorEdgePen, g, rects, conv.Rectangle,
                            Ambar.Rectangle);
                        DrawBottomLines(SelectedConveyorEdgePen, g, rects, conv.Rectangle,
                            Ambar.Rectangle);

                        string CopyPaste = "Ctrl + C: Conveyor'u Kopyala\nCtrl + V: Conveyor'u Yapıştır";
                        SizeF textSize = g.MeasureString(CopyPaste, Font);
                        SolidBrush brush = new SolidBrush(System.Drawing.Color.Blue);
                        g.DrawString(CopyPaste, Font, brush, new PointF(10, drawingPanel.ClientRectangle.Height - textSize.Height));
                    }
                    foreach (var reff in conv.ConveyorReferencePoints)
                    {
                        reff.Draw(g);
                    }
                }

                foreach (var depo in Ambar.depolar)
                {
                    g.DrawRectangle(TransparentPen, depo.Rectangle);
                    g.DrawRectangle(TransparentPen, depo.OriginalRectangle);

                    string textDepoAdi = $"{depo.DepoName}";

                    System.Drawing.Font font = new System.Drawing.Font("Arial", 8);
                    SolidBrush brush = new SolidBrush(System.Drawing.Color.Red);

                    g.DrawString(textDepoAdi, font, brush, depo.Rectangle.Location);

                    foreach (var cell in depo.gridmaps)
                    {
                        Pen pen1 = new Pen(System.Drawing.Color.LightGray);
                        pen1.DashStyle = DashStyle.Dash;
                        g.DrawRectangle(pen1, cell.Rectangle);
                    }

                    if (selectedDepo != null && selectedDepo == depo &&
                        Fill_WareHouse == false)
                    {
                        g.DrawRectangle(SelectedDepoPen, depo.Rectangle);

                        //g.FillRectangle(new SolidBrush(System.Drawing.Color.AliceBlue),
                        //    depo.Rectangle);
                        SelectedDepoEdgePen.DashStyle = DashStyle.Dash;
                        DrawLeftLines(SelectedDepoEdgePen, g, rects, depo.Rectangle, Ambar.Rectangle);
                        DrawRightLines(SelectedDepoEdgePen, g, rects, depo.Rectangle, Ambar.Rectangle);
                        DrawTopLines(SelectedDepoEdgePen, g, rects, depo.Rectangle, Ambar.Rectangle);
                        DrawBottomLines(SelectedDepoEdgePen, g, rects, depo.Rectangle, Ambar.Rectangle);

                        string CopyPaste = "Ctrl + C: Depoyu Kopyala\nCtrl + V: Depoyu Yapıştır";
                        SizeF textSize = g.MeasureString(CopyPaste, Font);
                        SolidBrush brush1 = new SolidBrush(System.Drawing.Color.Blue);
                        g.DrawString(CopyPaste, Font, brush1, new PointF(10, drawingPanel.ClientRectangle.Height - textSize.Height));
                    }
                }
                bool isfull = false;
                RectangleF rect = new RectangleF();
                if (MovingParameter)
                {
                    if (Parameter != String.Empty)
                    {
                        foreach (var depo in Ambar.depolar)
                        {
                            if (depo == selectedDepo)
                            {
                                foreach (var cell in depo.gridmaps)
                                {
                                    if ((currentColumn >= 0 || currentColumn <= depo.ColumnCount) &&
                                        Parameter == "Sola Doğru" && cell.Column - 1 == currentColumn)
                                    {
                                        float x = cell.Rectangle.X;
                                        float y = cell.Rectangle.Y;
                                        float width = (float)(cell.Rectangle.Width / 2 * 1.5);
                                        float height = (float)(cell.Rectangle.Height / 2 * 1.5);

                                        rect = new RectangleF(cell.Rectangle.X +
                                            (cell.Rectangle.Width / 2 - width / 2),
                                            cell.Rectangle.Y + (cell.Rectangle.Height / 2 -
                                            height / 2), width, height);

                                        rectangles.Add(rect);
                                    }

                                    if ((currentColumn >= 0 || currentColumn <= depo.ColumnCount) &&
                                        Parameter == "Sağa Doğru" && cell.Column + 1 == currentColumn)
                                    {
                                        float x = cell.Rectangle.X;
                                        float y = cell.Rectangle.Y;
                                        float width = (float)(cell.Rectangle.Width / 2 * 1.5);
                                        float height = (float)(cell.Rectangle.Height / 2 * 1.5);

                                        rect = new RectangleF(cell.Rectangle.X +
                                            (cell.Rectangle.Width / 2 - width / 2),
                                            cell.Rectangle.Y + (cell.Rectangle.Height / 2 -
                                            height / 2), width, height);

                                        rectangles.Add(rect);
                                    }

                                    if ((currentRow >= 0 || currentRow <= depo.RowCount + 2) &&
                                        Parameter == "Yukarı Doğru" && cell.Row - 1 == currentRow)
                                    {
                                        float x = cell.Rectangle.X;
                                        float y = cell.Rectangle.Y;
                                        float width = (float)(cell.Rectangle.Width / 2 * 1.5);
                                        float height = (float)(cell.Rectangle.Height / 2 * 1.5);

                                        rect = new RectangleF(cell.Rectangle.X +
                                            (cell.Rectangle.Width / 2 - width / 2),
                                            cell.Rectangle.Y + (cell.Rectangle.Height / 2 -
                                            height / 2), width, height);

                                        rectangles.Add(rect);
                                    }

                                    if ((currentRow >= 0 || currentRow <= depo.RowCount + 2) &&
                                        Parameter == "Ortadan Yukarı Doğru" && cell.Row - 1 == currentRow)
                                    {
                                        float x = cell.Rectangle.X;
                                        float y = cell.Rectangle.Y;
                                        float width = (float)(cell.Rectangle.Width / 2 * 1.5);
                                        float height = (float)(cell.Rectangle.Height / 2 * 1.5);

                                        rect = new RectangleF(cell.Rectangle.X +
                                            (cell.Rectangle.Width / 2 - width / 2),
                                            cell.Rectangle.Y + (cell.Rectangle.Height / 2 -
                                            height / 2), width, height);

                                        rectangles.Add(rect);
                                    }


                                    if ((currentRow >= 0 || currentRow <= depo.RowCount + 2) &&
                                        Parameter == "Aşağı Doğru" && cell.Row + 1 == currentRow)
                                    {
                                        float x = cell.Rectangle.X;
                                        float y = cell.Rectangle.Y;
                                        float width = (float)(cell.Rectangle.Width / 2 * 1.5);
                                        float height = (float)(cell.Rectangle.Height / 2 * 1.5);

                                        rect = new RectangleF(cell.Rectangle.X +
                                            (cell.Rectangle.Width / 2 - width / 2),
                                            cell.Rectangle.Y + (cell.Rectangle.Height / 2 -
                                            height / 2), width, height);

                                        rectangles.Add(rect);
                                    }

                                    if ((currentRow >= 0 || currentRow <= depo.RowCount + 2) &&
                                        Parameter == "Ortadan Aşağı Doğru" && cell.Row + 1 == currentRow)
                                    {
                                        float x = cell.Rectangle.X;
                                        float y = cell.Rectangle.Y;
                                        float width = (float)(cell.Rectangle.Width / 2 * 1.5);
                                        float height = (float)(cell.Rectangle.Height / 2 * 1.5);

                                        rect = new RectangleF(cell.Rectangle.X +
                                            (cell.Rectangle.Width / 2 - width / 2),
                                            cell.Rectangle.Y + (cell.Rectangle.Height / 2 -
                                            height / 2), width, height);

                                        rectangles.Add(rect);
                                    }


                                    if ((Parameter == "Sağa Doğru" ||
                                            Parameter == "Sola Doğru") &&
                                            cell.Column == currentColumn)
                                    {
                                        DrawArrowInCell(g, cell.Rectangle);
                                        //string Colc = $"Column Count: {colCount}";
                                        //string currentCol = $"Current Column: {currentColumn}";
                                        //g.DrawString(Colc, Font, new SolidBrush(System.Drawing.Color.Black), new PointF(Ambar.Rectangle.X, Ambar.Rectangle.Y));
                                        //g.DrawString(currentCol, Font, new SolidBrush(System.Drawing.Color.Black), new PointF(Ambar.Rectangle.X, Ambar.Rectangle.Y + 20));
                                    }
                                    if ((Parameter == "Yukarı Doğru" ||
                                        Parameter == "Aşağı Doğru" ||
                                        Parameter == "Ortadan Yukarı Doğru" ||
                                        Parameter == "Ortadan Aşağı Doğru") &&
                                        cell.Row == currentRow)
                                    {
                                        DrawArrowInCell(g, cell.Rectangle);
                                        //string Colc = $"Column Count: {colCount}";
                                        //string currentCol = $"Current Column: {currentColumn}";
                                        //g.DrawString(Colc, Font, new SolidBrush(System.Drawing.Color.Black), new PointF(Ambar.Rectangle.X, Ambar.Rectangle.Y));
                                        //g.DrawString(currentCol, Font, new SolidBrush(System.Drawing.Color.Black), new PointF(Ambar.Rectangle.X, Ambar.Rectangle.Y + 20));
                                    }
                                }
                            }
                            using (Pen pen = new Pen(System.Drawing.Color.Gray, 3))
                            {
                                foreach (var rect1 in rectangles)
                                {
                                    g.DrawRectangle(pen, rect1);
                                }

                                //if (currentColumn == colCount &&
                                //    Parameter == "Sağa Doğru")
                                //{
                                //    string Colc = $"Sağ Column Count: {colCount}";
                                //    string currentCol = $"Sağ Current Column: {currentColumn}";
                                //    g.DrawString(Colc, Font, new SolidBrush(System.Drawing.Color.Black), new PointF(Ambar.Rectangle.X, Ambar.Rectangle.Y + 40));
                                //    g.DrawString(currentCol, Font, new SolidBrush(System.Drawing.Color.Black), new PointF(Ambar.Rectangle.X, Ambar.Rectangle.Y + 60));
                                //    rectangles.Clear();
                                //}
                                //if (currentRow == rowCount &&
                                //    Parameter == "Aşağı Doğru")
                                //{
                                //    rectangles.Clear();
                                //}
                            }
                        }
                    }
                }

                if (Fill_WareHouse)
                {
                    foreach (var depo in Ambar.depolar)
                    {
                        System.Drawing.Font font = new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold);

                        string queue = $"{depo.Yerlestirilme_Sirasi}";

                        if (depo.Yerlestirilme_Sirasi == 0)
                        {
                            queue = string.Empty;
                        }
                        System.Drawing.SizeF textsize = g.MeasureString(queue, font);

                        g.DrawString(queue, font, new SolidBrush(System.Drawing.Color.Red),
                               new PointF(depo.Rectangle.X + (depo.Rectangle.Width / 2 - textsize.Width / 2),
                               depo.Rectangle.Y + (depo.Rectangle.Height / 2 - textsize.Height / 2)));
                    }
                }
            }
        }
        private void drawingPanel_MouseClick(object sender, MouseEventArgs e)
        {
            if (Fill_WareHouse)
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (Ambar != null)
                    {
                        System.Drawing.Point scaledPoint =
                        new System.Drawing.Point((int)((e.X - drawingPanel.AutoScrollPosition.X)),
                        (int)((e.Y - drawingPanel.AutoScrollPosition.Y)));

                        foreach (var depo in Ambar.depolar)
                        {
                            if (depo.Rectangle.Contains(scaledPoint))
                            {
                                if (depo.Yerlestirilme_Sirasi == 0)
                                {
                                    int count = 0;

                                    foreach (int num in CountList)
                                    {
                                        if (num < count || count == 0)
                                        {
                                            count = num;
                                        }
                                    }
                                    depo.Yerlestirilme_Sirasi = count;
                                    CountList.Remove(count);
                                }
                            }
                        }
                        bool iszero = false;
                        foreach (var depo in Ambar.depolar)
                        {
                            if (depo.Yerlestirilme_Sirasi == 0)
                            {
                                iszero = true;
                            }
                        }
                        if (!iszero)
                        {
                            CustomNotifyIcon notify = new CustomNotifyIcon();
                            notify.showAlert("Çıkmak için depo olmayan bir yere tıklamanız yeterli", CustomNotifyIcon.enmType.Warning);
                        }
                    }
                }
                if (e.Button == MouseButtons.Right)
                {
                    if (Ambar != null)
                    {
                        System.Drawing.Point scaledPoint =
                        new System.Drawing.Point((int)((e.X - drawingPanel.AutoScrollPosition.X)),
                        (int)((e.Y - drawingPanel.AutoScrollPosition.Y)));
                        foreach (var depo in Ambar.depolar)
                        {
                            if (depo.Rectangle.Contains(scaledPoint))
                            {
                                CountList.Add(depo.Yerlestirilme_Sirasi);
                                depo.Yerlestirilme_Sirasi = 0;
                            }
                        }
                    }
                }
            }
            drawingPanel.Invalidate();
        }
        private void depolarınDoldurulmaSirasiniAyarla_Click(object sender, EventArgs e)
        {
            TransparentPen.Color = System.Drawing.Color.FromArgb(30, System.Drawing.Color.Black);
            CustomNotifyIcon notify = new CustomNotifyIcon();
            notify.showAlert("Depoların sırasını üzerlerine sol tıklayarak seçebilirsiniz.(Sağ tıklayarak sırasını kaldırabilirsiniz.)", CustomNotifyIcon.enmType.Info);
            Fill_WareHouse = true;
        }
        private void DrawArrowInCell(Graphics g, RectangleF cell)
        {
            if (MovingParameter)
            {
                if (Parameter == "Aşağı Doğru")
                {
                    PointF start = GVisual.GetMiddleOfTopEdge(cell);
                    PointF end = GVisual.GetMiddleOfBottomEdge(cell);

                    Pen pen = new Pen(Brushes.Green, 5);
                    pen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;

                    g.DrawLine(pen, start, end);
                }
                else if (Parameter == "Yukarı Doğru")
                {
                    PointF start = GVisual.GetMiddleOfBottomEdge(cell);
                    PointF end = GVisual.GetMiddleOfTopEdge(cell);

                    Pen pen = new Pen(Brushes.Green, 5);
                    pen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;

                    g.DrawLine(pen, start, end);
                }
                else if (Parameter == "Ortadan Yukarı Doğru")
                {
                    PointF start = GVisual.GetMiddleOfBottomEdge(cell);
                    PointF end = GVisual.GetMiddleOfTopEdge(cell);

                    Pen pen = new Pen(Brushes.Green, 5);
                    pen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;

                    g.DrawLine(pen, start, end);
                }
                else if (Parameter == "Ortadan Aşağı Doğru")
                {
                    PointF start = GVisual.GetMiddleOfTopEdge(cell);
                    PointF end = GVisual.GetMiddleOfBottomEdge(cell);

                    Pen pen = new Pen(Brushes.Green, 5);
                    pen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;

                    g.DrawLine(pen, start, end);
                }
                else if (Parameter == "Sağa Doğru")
                {
                    PointF start = GVisual.GetMiddleOfLeftEdge(cell);
                    PointF end = GVisual.GetMiddleOfRightEdge(cell);

                    Pen pen = new Pen(Brushes.Green, 5);
                    pen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;

                    g.DrawLine(pen, start, end);
                }
                else if (Parameter == "Sola Doğru")
                {
                    PointF start = GVisual.GetMiddleOfRightEdge(cell);
                    PointF end = GVisual.GetMiddleOfLeftEdge(cell);

                    Pen pen = new Pen(Brushes.Green, 5);
                    pen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;

                    g.DrawLine(pen, start, end);
                }
            }
        }
        private void drawingPanel_MouseDown(object sender, MouseEventArgs e)
        {
            System.Drawing.Point scaledPoint = new System.Drawing.Point((int)((e.X - drawingPanel.AutoScrollPosition.X)),
                    (int)((e.Y - drawingPanel.AutoScrollPosition.Y)));
            bool deponull = false;
            bool conveyornull = false;
            if (Ambar != null)
            {
                Ambar.OnMouseDown(e);
                drawingPanel.Invalidate();

                if (!Manuel_Move && !AddReferencePoint)
                {
                    foreach (var depo in Ambar.depolar)
                    {
                        if (depo.Rectangle.Contains(scaledPoint))
                        {
                            deponull = true;
                        }
                    }

                    if (!deponull)
                    {
                        if (MovingParameter)
                        {
                            GVisual.HideControl(Asama1_Yukseklik_Panel, drawingPanel);
                            GVisual.HideControl(Asama2_Yukseklik_Panel, drawingPanel);
                            GVisual.HideControl(Placement_StartLocation_Panel, drawingPanel);
                            GVisual.HideControl(Placement_UpDown_Panel, drawingPanel);
                            GVisual.HideControl(Placement_LeftRight_Panel, drawingPanel);
                            upDown_1Asama_NesneSayisi.Value = 0;
                            upDown_2Asama_NesneSayisi.Value = 0;
                            MovingParameter = false;
                            TransparentPen.Color = System.Drawing.Color.FromArgb(255, System.Drawing.Color.Black);
                        }
                        if (Fill_WareHouse)
                        {
                            CustomNotifyIcon notify = new CustomNotifyIcon();
                            notify.showAlert("Sıralama Kaydedildi.", CustomNotifyIcon.enmType.Success);
                            TransparentPen.Color = System.Drawing.Color.FromArgb(255, System.Drawing.Color.Black);
                            Fill_WareHouse = false;
                        }
                        selectedDepo = null;
                        SelectedDepoEdgePen.Width = 2;
                        SelectedDepoPen.Width = 2;
                        SelectedDepoPen.Color = System.Drawing.Color.Black;
                    }

                    foreach (var conveyor in Ambar.conveyors)
                    {
                        if (conveyor.Rectangle.Contains(scaledPoint))
                        {
                            conveyornull = true;
                        }
                    }

                    if (!conveyornull)
                    {
                        selectedConveyor = null;
                        SelectedConveyorEdgePen.Width = 2;
                        SelectedConveyorPen.Width = 2;
                        SelectedConveyorPen.Color = System.Drawing.Color.Black;
                    }
                }
            }
        }
        private void drawingPanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (Ambar != null)
            {
                Ambar.OnMouseUp(e);
                drawingPanel.Invalidate();
            }
        }
        private void drawingPanel_MouseMove(object sender, MouseEventArgs e)
        {
            System.Drawing.Point scaledPoint = new System.Drawing.Point((int)((e.X - drawingPanel.AutoScrollPosition.X)),
                    (int)((e.Y - drawingPanel.AutoScrollPosition.Y)));
            CopyPoint = scaledPoint;
            if (Ambar != null)
            {
                Ambar.OnMouseMove(e);

                if (selectedDepo != null)
                {
                    foreach (var depo in Ambar.depolar)
                    {
                        if (selectedDepo != depo)
                        {
                            if (selectedDepo.Rectangle.IntersectsWith(depo.Rectangle))
                            {
                                float x = selectedDepo.Rectangle.X;
                                float y = selectedDepo.Rectangle.Y;

                                selectedDepo.Rectangle = SnapRectangles(selectedDepo.Rectangle,
                                    depo.Rectangle);
                                selectedDepo.OriginalRectangle = selectedDepo.Rectangle;

                                if (snapped)
                                {
                                    selectedDepo.isDragging = false;
                                    snapped = false;
                                }

                                float changedX = selectedDepo.Rectangle.X - x;
                                float changedY = selectedDepo.Rectangle.Y - y;

                                foreach (var cell in selectedDepo.gridmaps)
                                {
                                    cell.Rectangle = GVisual.MoveRectangle(cell.Rectangle, changedX, changedY);
                                    cell.OriginalRectangle = GVisual.MoveRectangle(cell.OriginalRectangle, changedX, changedY);
                                    cell.LocationofRect = new System.Drawing.Point((int)cell.Rectangle.X, (int)cell.Rectangle.Y);
                                }
                            }
                        }
                    }
                    foreach (var conveyor in Ambar.conveyors)
                    {
                        if (selectedDepo.Rectangle.IntersectsWith(conveyor.Rectangle))
                        {
                            float x = selectedDepo.Rectangle.X;
                            float y = selectedDepo.Rectangle.Y;

                            selectedDepo.Rectangle = SnapRectangles(selectedDepo.Rectangle,
                                conveyor.Rectangle);
                            selectedDepo.OriginalRectangle = selectedDepo.Rectangle;

                            if (snapped)
                            {
                                selectedDepo.isDragging = false;
                                snapped = false;
                            }

                            float changedX = selectedDepo.Rectangle.X - x;
                            float changedY = selectedDepo.Rectangle.Y - y;

                            foreach (var cell in selectedDepo.gridmaps)
                            {
                                cell.Rectangle = GVisual.MoveRectangle(cell.Rectangle, changedX, changedY);
                                cell.OriginalRectangle = GVisual.MoveRectangle(cell.OriginalRectangle, changedX, changedY);
                                cell.LocationofRect = new System.Drawing.Point((int)cell.Rectangle.X, (int)cell.Rectangle.Y);
                            }
                        }
                    }
                }

                if (selectedConveyor != null)
                {
                    foreach (var depo in Ambar.depolar)
                    {
                        if (selectedConveyor.Rectangle.IntersectsWith(depo.Rectangle))
                        {
                            selectedConveyor.Rectangle = SnapRectangles(selectedConveyor.Rectangle, depo.Rectangle);
                            selectedConveyor.OriginalRectangle = selectedConveyor.Rectangle;
                            if (snapped)
                            {
                                selectedConveyor.isDragging = false;
                                snapped = false;
                            }
                            foreach (var reff in selectedConveyor.ConveyorReferencePoints)
                            {
                                reff.MoveRectangleExact
                                    (selectedConveyor.Rectangle.X + reff.OriginalLocationInsideParent.X,
                                    selectedConveyor.Rectangle.Y + reff.OriginalLocationInsideParent.Y);
                            }
                        }
                    }

                    foreach (var conv in Ambar.conveyors)
                    {
                        if (selectedConveyor != conv)
                        {
                            if (selectedConveyor.Rectangle.IntersectsWith(conv.Rectangle))
                            {
                                selectedConveyor.Rectangle = SnapRectangles(selectedConveyor.Rectangle, conv.Rectangle);
                                selectedConveyor.OriginalRectangle = selectedConveyor.Rectangle;
                                if (snapped)
                                {
                                    selectedConveyor.isDragging = false;
                                    snapped = false;
                                }
                                foreach (var reff in selectedConveyor.ConveyorReferencePoints)
                                {
                                    reff.MoveRectangleExact
                                        (selectedConveyor.Rectangle.X + reff.OriginalLocationInsideParent.X,
                                        selectedConveyor.Rectangle.Y + reff.OriginalLocationInsideParent.Y);
                                }
                            }
                        }
                    }
                }
                drawingPanel.Invalidate();
            }
        }



        private void txt_Width_TextChanged(object sender, EventArgs e)
        {
            errorProvider.Clear();
            var pxCM = ConvertPixelToCmGivenRectangle(Ambar.Rectangle, Ambar.AmbarEni, Ambar.AmbarBoyu);
            float pxX = pxCM.Item1;
            float pxY = pxCM.Item2;

            txt_width = StrLib.ReplaceDotWithCommaReturnFloat(txt_Width, errorProvider,
               "Bu alan boş bırakılamaz.", "Lütfen buraya bir sayı giriniz.", "Buraya 0 ya da daha küçük bir değer giremezsiniz.");

            if (Manuel_Move)
            {
                if (selectedDepo != null)
                {
                    if (!errorProvider.HasErrors)
                    {
                        RectangleF rect = new RectangleF(selectedDepo.Rectangle.X, selectedDepo.Rectangle.Y,
                            txt_width * pxX * 100, selectedDepo.Rectangle.Height);

                        if (rect.Right > Ambar.Rectangle.Right)
                        {
                            CustomNotifyIcon notify = new CustomNotifyIcon();
                            notify.showAlert("Alanın dışına çıkamazsınız.", CustomNotifyIcon.enmType.Error);
                            txt_Width.Text = $"{selectedDepo.DepoAlaniEni}";
                        }
                        else
                        {
                            DataCell = selectedDepo.gridmaps.FirstOrDefault();

                            if (DataCell != null)
                            {
                                cell_eni = DataCell.CellEni;
                                cell_boyu = DataCell.CellBoyu;
                                dikey_kenar_boslugu = DataCell.DikeyKenarBoslugu;
                                yatay_kenar_boslugu = DataCell.YatayKenarBoslugu;
                                nesne_eni = DataCell.NesneEni;
                                nesne_boyu = DataCell.NesneBoyu;
                            }

                            selectedDepo.Rectangle = new RectangleF(selectedDepo.Rectangle.X,
                                selectedDepo.Rectangle.Y, txt_width * pxX * 100,
                                selectedDepo.Rectangle.Height);

                            selectedDepo.OriginalRectangle = new RectangleF(selectedDepo.Rectangle.X,
                                selectedDepo.Rectangle.Y, txt_width * pxX * 100,
                                selectedDepo.Rectangle.Height);

                            selectedDepo.OriginalDepoSize = new SizeF(selectedDepo.OriginalRectangle.Width,
                               selectedDepo.OriginalRectangle.Height);

                            selectedDepo.DepoAlaniEni = txt_width;

                            selectedDepo.Cm_Width = txt_width * 100;

                            selectedDepo.CreateGridMapMenuItem(cell_eni, cell_boyu, dikey_kenar_boslugu,
                                yatay_kenar_boslugu, nesne_eni, nesne_boyu);

                            //txt_Width.Text = $"{txt_width}";
                        }
                        drawingPanel.Invalidate();
                    }
                }
                if (selectedConveyor != null)
                {
                    if (!errorProvider.HasErrors)
                    {
                        RectangleF rect = new RectangleF(selectedConveyor.Rectangle.X,
                            selectedConveyor.Rectangle.Y, txt_width * pxX * 100,
                            selectedConveyor.Rectangle.Height);

                        if (rect.Right > Ambar.Rectangle.Right)
                        {
                            CustomNotifyIcon notify = new CustomNotifyIcon();
                            notify.showAlert("Alanın dışına çıkamazsınız.", CustomNotifyIcon.enmType.Error);
                            txt_Width.Text = $"{selectedConveyor.ConveyorEni}";
                        }
                        else
                        {
                            selectedConveyor.Rectangle = new RectangleF(selectedConveyor.Rectangle.X,
                                selectedConveyor.Rectangle.Y, txt_width * pxX * 100,
                                selectedConveyor.Rectangle.Height);
                            selectedConveyor.OriginalRectangle = new RectangleF(selectedConveyor.Rectangle.X, selectedConveyor.Rectangle.Y, selectedConveyor.Rectangle.Width, selectedConveyor.Rectangle.Height);

                            AdjustFixedConveyorReferencePoints(selectedConveyor);

                            //txt_Width.Text = $"{txt_width}";
                        }
                        drawingPanel.Invalidate();
                    }
                }
            }
        }
        private void txt_Height_TextChanged(object sender, EventArgs e)
        {
            errorProvider.Clear();

            var pxCM = ConvertPixelToCmGivenRectangle(Ambar.Rectangle, Ambar.AmbarEni, Ambar.AmbarBoyu);
            float pxX = pxCM.Item1;
            float pxY = pxCM.Item2;
            txt_height = StrLib.ReplaceDotWithCommaReturnFloat(txt_Height, errorProvider,
               "Bu alan boş bırakılamaz.", "Lütfen buraya bir sayı giriniz.", "Buraya 0 ya da daha küçük bir değer giremezsiniz.");
            if (Manuel_Move)
            {
                if (selectedDepo != null)
                {
                    if (!errorProvider.HasErrors)
                    {
                        RectangleF rect = new RectangleF(selectedDepo.Rectangle.X, selectedDepo.Rectangle.Y,
                        selectedDepo.Rectangle.Width, txt_height * pxY * 100);

                        if (rect.Bottom > Ambar.Rectangle.Bottom)
                        {
                            CustomNotifyIcon notify = new CustomNotifyIcon();
                            notify.showAlert("Alanın dışına çıkamazsınız.", CustomNotifyIcon.enmType.Error);
                            txt_Height.Text = $"{selectedDepo.DepoAlaniBoyu}";
                        }
                        else
                        {
                            DataCell = selectedDepo.gridmaps.FirstOrDefault();

                            if (DataCell != null)
                            {
                                cell_eni = DataCell.CellEni;
                                cell_boyu = DataCell.CellBoyu;
                                dikey_kenar_boslugu = DataCell.DikeyKenarBoslugu;
                                yatay_kenar_boslugu = DataCell.YatayKenarBoslugu;
                                nesne_eni = DataCell.NesneEni;
                                nesne_boyu = DataCell.NesneBoyu;
                            }

                            selectedDepo.Rectangle = new RectangleF(selectedDepo.Rectangle.X, selectedDepo.Rectangle.Y,
                            selectedDepo.Rectangle.Width, txt_height * pxY * 100);

                            selectedDepo.OriginalRectangle =
                                new RectangleF(selectedDepo.Rectangle.X,
                                selectedDepo.Rectangle.Y,
                            selectedDepo.Rectangle.Width, txt_height * pxY * 100);

                            selectedDepo.OriginalDepoSize = new SizeF(selectedDepo.OriginalRectangle.Width,
                               selectedDepo.OriginalRectangle.Height);

                            selectedDepo.DepoAlaniBoyu = txt_height;

                            selectedDepo.Cm_Height = txt_height * 100;

                            selectedDepo.CreateGridMapMenuItem(cell_eni, cell_boyu, dikey_kenar_boslugu,
                               yatay_kenar_boslugu, nesne_eni, nesne_boyu);

                            //txt_Height.Text = $"{txt_height}";
                        }
                        drawingPanel.Invalidate();
                    }
                }
                if (selectedConveyor != null)
                {
                    if (!errorProvider.HasErrors)
                    {
                        RectangleF rect = new RectangleF(selectedConveyor.Rectangle.X,
                            selectedConveyor.Rectangle.Y, selectedConveyor.Rectangle.Width,
                            txt_height * pxY * 100);

                        if (rect.Bottom >= Ambar.Rectangle.Bottom + 1)
                        {
                            CustomNotifyIcon notify = new CustomNotifyIcon();
                            notify.showAlert("Alanın dışına çıkamazsınız.", CustomNotifyIcon.enmType.Error);
                            txt_Height.Text = $"{selectedConveyor.ConveyorBoyu}";
                        }
                        else
                        {
                            selectedConveyor.Rectangle = new RectangleF(selectedConveyor.Rectangle.X,
                                selectedConveyor.Rectangle.Y,
                            selectedConveyor.Rectangle.Width, txt_height * pxY * 100);

                            selectedConveyor.OriginalRectangle = new RectangleF(selectedConveyor.Rectangle.X,
                                selectedConveyor.Rectangle.Y,
                            selectedConveyor.Rectangle.Width, txt_height * pxY * 100);

                            AdjustFixedConveyorReferencePoints(selectedConveyor);

                            //txt_Height.Text = $"{txt_height}";
                        }
                        drawingPanel.Invalidate();
                    }
                }
            }
        }
        private void AdjustFixedConveyorReferencePoints(Conveyor selectedConveyor)
        {
            foreach (var reff in selectedConveyor.ConveyorReferencePoints)
            {
                if (reff.FixedPointLocation == "Center")
                {
                    PointF point = GVisual.GetCenterF(selectedConveyor.Rectangle);

                    reff.Rectangle = new RectangleF(point.X - reff.Rectangle.Width / 2,
                        point.Y - reff.Rectangle.Height / 2, reff.Rectangle.Width,
                        reff.Rectangle.Height);
                    reff.OriginalLocationInsideParent = new PointF(reff.Rectangle.X - selectedConveyor.Rectangle.X,
                        reff.Rectangle.Y - selectedConveyor.Rectangle.Y);
                }

                if (reff.FixedPointLocation == "Top")
                {
                    PointF point = GVisual.GetMiddleOfTopEdgeF(selectedConveyor.Rectangle);

                    reff.Rectangle = new RectangleF(point.X - reff.Rectangle.Width / 2,
                        point.Y - reff.Rectangle.Height / 2, reff.Rectangle.Width,
                        reff.Rectangle.Height);
                    reff.OriginalLocationInsideParent = new PointF(reff.Rectangle.X - selectedConveyor.Rectangle.X,
                        reff.Rectangle.Y - selectedConveyor.Rectangle.Y);
                }

                if (reff.FixedPointLocation == "Bottom")
                {
                    PointF point = GVisual.GetMiddleOfBottomEdgeF(selectedConveyor.Rectangle);

                    reff.Rectangle = new RectangleF(point.X - reff.Rectangle.Width / 2,
                        point.Y - reff.Rectangle.Height / 2, reff.Rectangle.Width,
                        reff.Rectangle.Height);
                    reff.OriginalLocationInsideParent = new PointF(reff.Rectangle.X - selectedConveyor.Rectangle.X,
                        reff.Rectangle.Y - selectedConveyor.Rectangle.Y);
                }

                if (reff.FixedPointLocation == "Left")
                {
                    PointF point = GVisual.GetMiddleOfLeftEdgeF(selectedConveyor.Rectangle);

                    reff.Rectangle = new RectangleF(point.X - reff.Rectangle.Width / 2,
                        point.Y - reff.Rectangle.Height / 2, reff.Rectangle.Width,
                        reff.Rectangle.Height);
                    reff.OriginalLocationInsideParent = new PointF(reff.Rectangle.X - selectedConveyor.Rectangle.X,
                        reff.Rectangle.Y - selectedConveyor.Rectangle.Y);
                }

                if (reff.FixedPointLocation == "Right")
                {
                    PointF point = GVisual.GetMiddleOfRightEdgeF(selectedConveyor.Rectangle);

                    reff.Rectangle = new RectangleF(point.X - reff.Rectangle.Width / 2,
                        point.Y - reff.Rectangle.Height / 2, reff.Rectangle.Width,
                        reff.Rectangle.Height);
                    reff.OriginalLocationInsideParent = new PointF(reff.Rectangle.X - selectedConveyor.Rectangle.X,
                        reff.Rectangle.Y - selectedConveyor.Rectangle.Y);
                }
            }
        }
        private void txt_Left_Padding_TextChanged(object sender, EventArgs e)
        {
            errorProvider.Clear();
            var pxCM = ConvertPixelToCmGivenRectangle(Ambar.Rectangle, Ambar.AmbarEni, Ambar.AmbarBoyu);
            float pxX = pxCM.Item1;
            float proxRectLoc = 0f;

            float txt_left = StrLib.ReplaceDotWithCommaReturnFloatButAllowZeros(txt_Left_Padding, errorProvider,
               "Bu alan boş bırakılamaz.", "Lütfen buraya bir sayı giriniz.", "Buraya 0 ya da daha küçük bir değer giremezsiniz.");

            if (!ProxRectLeft.IsEmpty)
            {
                proxRectLoc = ProxRectLeft.Right - Ambar.Rectangle.Left;
            }
            else
            {
                proxRectLoc = 0;
            }

            if (selectedDepo != null)
            {
                if (!errorProvider.HasErrors)
                {
                    float ambar_eni_CM = (Ambar.AmbarEni * 100) - selectedDepo.Cm_Width;

                    if (txt_left >= 0 && txt_left <= ambar_eni_CM)
                    {
                        selectedDepo.Rectangle =
                        new RectangleF(Ambar.Rectangle.X + proxRectLoc + txt_left * pxX,
                        selectedDepo.Rectangle.Y,
                        selectedDepo.Rectangle.Width, selectedDepo.Rectangle.Height);

                        selectedDepo.OriginalRectangle =
                        new RectangleF(Ambar.Rectangle.X + proxRectLoc + txt_left * pxX,
                        selectedDepo.Rectangle.Y,
                        selectedDepo.Rectangle.Width, selectedDepo.Rectangle.Height);

                        if (selectedDepo.gridmaps.Count > 0)
                        {
                            DataCell = selectedDepo.gridmaps.FirstOrDefault();

                            selectedDepo.gridmaps.Clear();

                            selectedDepo.CreateGridMapMenuItem(DataCell.CellEni, DataCell.CellBoyu,
                                DataCell.DikeyKenarBoslugu, DataCell.YatayKenarBoslugu, DataCell.NesneEni,
                                DataCell.NesneBoyu);
                        }

                        drawingPanel.Invalidate();
                    }
                    else
                    {
                        CustomNotifyIcon notify = new CustomNotifyIcon();
                        notify.showAlert("Alanın dışına çıkamazsınız.", CustomNotifyIcon.enmType.Error);
                        drawingPanel.Invalidate();
                    }
                }
            }
            if (selectedConveyor != null)
            {
                if (!errorProvider.HasErrors)
                {
                    float ambar_eni_CM =
                        (Ambar.AmbarEni * 100) - selectedConveyor.ConveyorEni * 100;

                    if (txt_left >= 0 && txt_left <= ambar_eni_CM)
                    {
                        selectedConveyor.Rectangle = new RectangleF
                            (Ambar.Rectangle.X + proxRectLoc + txt_left * pxX,
                            selectedConveyor.Rectangle.Y,
                            selectedConveyor.Rectangle.Width,
                            selectedConveyor.Rectangle.Height);

                        selectedConveyor.OriginalRectangle = new RectangleF
                            (Ambar.Rectangle.X + proxRectLoc + txt_left * pxX,
                            selectedConveyor.Rectangle.Y,
                            selectedConveyor.Rectangle.Width,
                            selectedConveyor.Rectangle.Height);

                        foreach (var reff in selectedConveyor.ConveyorReferencePoints)
                        {
                            reff.Rectangle = new RectangleF
                                (selectedConveyor.Rectangle.X + reff.OriginalLocationInsideParent.X,
                                selectedConveyor.Rectangle.Y + reff.OriginalLocationInsideParent.Y,
                                reff.Rectangle.Width, reff.Rectangle.Height);
                        }

                        drawingPanel.Invalidate();
                    }
                    else
                    {
                        CustomNotifyIcon notify = new CustomNotifyIcon();
                        notify.showAlert("Alanın dışına çıkamazsınız.", CustomNotifyIcon.enmType.Error);
                        drawingPanel.Invalidate();
                    }
                }
            }
        }
        private void txt_Right_Padding_TextChanged(object sender, EventArgs e)
        {
            errorProvider.Clear();
            var pxCM = ConvertPixelToCmGivenRectangle(Ambar.Rectangle, Ambar.AmbarEni, Ambar.AmbarBoyu);
            float pxX = pxCM.Item1;
            float pxY = pxCM.Item2;
            float right_padding = 0f;
            float ProxRectWidthCM = 0f;

            float txt_right =
                StrLib.ReplaceDotWithCommaReturnFloatButAllowZeros(txt_Right_Padding, errorProvider,
               "Bu alan boş bırakılamaz.", "Lütfen buraya bir sayı giriniz.", "Buraya 0 ya da daha küçük bir değer giremezsiniz.");

            if (!ProxRectRight.IsEmpty)
            {
                ProxRectWidthCM = Ambar.Rectangle.Right - ProxRectRight.X;
            }
            else
            {
                ProxRectWidthCM = 0f;
            }

            if (selectedDepo != null)
            {
                if (!errorProvider.HasErrors)
                {
                    float ambar_eni_CM = (Ambar.AmbarEni * 100) - selectedDepo.Cm_Width;

                    right_padding = txt_right * pxX + selectedDepo.Rectangle.Width + ProxRectWidthCM;

                    if (txt_right >= 0 && txt_right <= ambar_eni_CM)
                    {
                        selectedDepo.Rectangle = new RectangleF(Ambar.Rectangle.Right - right_padding,
                            selectedDepo.Rectangle.Y, selectedDepo.Rectangle.Width,
                            selectedDepo.Rectangle.Height);

                        selectedDepo.OriginalRectangle = new RectangleF(Ambar.Rectangle.Right - right_padding,
                            selectedDepo.Rectangle.Y, selectedDepo.Rectangle.Width,
                            selectedDepo.Rectangle.Height);

                        if (selectedDepo.gridmaps.Count > 0)
                        {
                            DataCell = selectedDepo.gridmaps.FirstOrDefault();

                            selectedDepo.gridmaps.Clear();

                            selectedDepo.CreateGridMapMenuItem(DataCell.CellEni, DataCell.CellBoyu,
                                DataCell.DikeyKenarBoslugu, DataCell.YatayKenarBoslugu, DataCell.NesneEni,
                                DataCell.NesneBoyu);
                        }

                        drawingPanel.Invalidate();
                    }
                    else
                    {
                        CustomNotifyIcon notify = new CustomNotifyIcon();
                        notify.showAlert("Alanın dışına çıkamazsınız.", CustomNotifyIcon.enmType.Error);
                        drawingPanel.Invalidate();
                    }
                }
            }
            if (selectedConveyor != null)
            {
                if (!errorProvider.HasErrors)
                {
                    float ambar_eni_CM =
                        (Ambar.AmbarEni * 100) - selectedConveyor.ConveyorEni * 100;

                    right_padding = txt_right * pxX +
                        selectedConveyor.Rectangle.Width + ProxRectWidthCM;

                    if (txt_right >= 0 && txt_right <= ambar_eni_CM)
                    {
                        selectedConveyor.Rectangle = new RectangleF
                            (Ambar.Rectangle.Right - right_padding,
                            selectedConveyor.Rectangle.Y,
                            selectedConveyor.Rectangle.Width,
                            selectedConveyor.Rectangle.Height);

                        selectedConveyor.OriginalRectangle = new RectangleF
                            (Ambar.Rectangle.Right - right_padding,
                            selectedConveyor.Rectangle.Y,
                            selectedConveyor.Rectangle.Width,
                            selectedConveyor.Rectangle.Height);

                        foreach (var reff in selectedConveyor.ConveyorReferencePoints)
                        {
                            reff.Rectangle = new RectangleF
                                (selectedConveyor.Rectangle.X + reff.OriginalLocationInsideParent.X,
                                selectedConveyor.Rectangle.Y + reff.OriginalLocationInsideParent.Y,
                                reff.Rectangle.Width, reff.Rectangle.Height);
                        }

                        drawingPanel.Invalidate();
                    }
                    else
                    {
                        CustomNotifyIcon notify = new CustomNotifyIcon();
                        notify.showAlert("Alanın dışına çıkamazsınız.", CustomNotifyIcon.enmType.Error);
                        drawingPanel.Invalidate();
                    }
                }
            }
        }
        private void txt_Bottom_Padding_TextChanged(object sender, EventArgs e)
        {
            errorProvider.Clear();
            var pxCM = ConvertPixelToCmGivenRectangle(Ambar.Rectangle, Ambar.AmbarEni, Ambar.AmbarBoyu);
            float pxX = pxCM.Item1;
            float pxY = pxCM.Item2;
            float proxRectBotLoc = 0f;
            float bot_padding = 0f;
            float ambar_boyu_CM = 0f;

            float txt_bot =
                StrLib.ReplaceDotWithCommaReturnFloatButAllowZeros(txt_Bottom_Padding, errorProvider,
                "Bu alan boş bırakılamaz.", "Lütfen buraya bir sayı giriniz.", "Buraya 0 ya da daha küçük bir değer giremezsiniz.");

            if (!ProxRectBottom.IsEmpty)
            {
                proxRectBotLoc = Ambar.Rectangle.Bottom - ProxRectBottom.Top;
            }
            else
            {
                proxRectBotLoc = 0f;
            }

            if (selectedDepo != null)
            {
                if (!errorProvider.HasErrors)
                {
                    ambar_boyu_CM = (Ambar.AmbarBoyu * 100) - selectedDepo.Cm_Height;

                    bot_padding = txt_bot * pxY + selectedDepo.Rectangle.Height + proxRectBotLoc;

                    if (txt_bot >= 0 && txt_bot <= ambar_boyu_CM)
                    {
                        selectedDepo.Rectangle = new RectangleF(selectedDepo.Rectangle.X,
                            Ambar.Rectangle.Bottom - bot_padding,
                            selectedDepo.Rectangle.Width,
                            selectedDepo.Rectangle.Height);

                        selectedDepo.OriginalRectangle = new RectangleF(selectedDepo.Rectangle.X,
                            Ambar.Rectangle.Bottom - bot_padding,
                            selectedDepo.Rectangle.Width,
                            selectedDepo.Rectangle.Height);

                        if (selectedDepo.gridmaps.Count > 0)
                        {
                            DataCell = selectedDepo.gridmaps.FirstOrDefault();

                            selectedDepo.gridmaps.Clear();

                            selectedDepo.CreateGridMapMenuItem(DataCell.CellEni, DataCell.CellBoyu,
                                DataCell.DikeyKenarBoslugu, DataCell.YatayKenarBoslugu, DataCell.NesneEni,
                                DataCell.NesneBoyu);
                        }

                        drawingPanel.Invalidate();
                    }
                    else
                    {
                        CustomNotifyIcon notify = new CustomNotifyIcon();
                        notify.showAlert("Alanın dışına çıkamazsınız.", CustomNotifyIcon.enmType.Error);
                        drawingPanel.Invalidate();
                    }
                }
            }
            if (selectedConveyor != null)
            {
                if (!errorProvider.HasErrors)
                {
                    ambar_boyu_CM = (Ambar.AmbarBoyu * 100);

                    bot_padding = txt_bot * pxY +
                        selectedConveyor.Rectangle.Height + proxRectBotLoc;

                    Debug.WriteLine($"{txt_bot}");

                    if (txt_bot >= 0 && txt_bot <= ambar_boyu_CM)
                    {
                        selectedConveyor.Rectangle =
                            new RectangleF(selectedConveyor.Rectangle.X,
                            Ambar.Rectangle.Y + Ambar.Rectangle.Height - bot_padding,
                            selectedConveyor.Rectangle.Width,
                            selectedConveyor.Rectangle.Height);

                        selectedConveyor.OriginalRectangle =
                            new RectangleF(selectedConveyor.Rectangle.X,
                            Ambar.Rectangle.Y + Ambar.Rectangle.Height - bot_padding,
                            selectedConveyor.Rectangle.Width,
                            selectedConveyor.Rectangle.Height);

                        foreach (var reff in selectedConveyor.ConveyorReferencePoints)
                        {
                            reff.Rectangle = new RectangleF
                                (selectedConveyor.Rectangle.X + reff.OriginalLocationInsideParent.X,
                                selectedConveyor.Rectangle.Y + reff.OriginalLocationInsideParent.Y,
                                reff.Rectangle.Width, reff.Rectangle.Height);
                        }
                        drawingPanel.Invalidate();
                    }
                    else
                    {
                        CustomNotifyIcon notify = new CustomNotifyIcon();
                        notify.showAlert("Alanın dışına çıkamazsınız.", CustomNotifyIcon.enmType.Error);
                        drawingPanel.Invalidate();
                    }
                }
            }
        }
        private void txt_Top_Padding_TextChanged(object sender, EventArgs e)
        {
            errorProvider.Clear();
            var pxCM = ConvertPixelToCmGivenRectangle(Ambar.Rectangle, Ambar.AmbarEni, Ambar.AmbarBoyu);
            float pxX = pxCM.Item1;
            float pxY = pxCM.Item2;
            float proxRectTopLoc = 0f;
            float top_padding = 0f;
            float txt_top =
                StrLib.ReplaceDotWithCommaReturnFloatButAllowZeros(txt_Top_Padding, errorProvider,
                "Bu alan boş bırakılamaz.", "Lütfen buraya bir sayı giriniz.", "Buraya 0 ya da daha küçük bir değer giremezsiniz.");

            if (!ProxRectTop.IsEmpty)
            {
                proxRectTopLoc = ProxRectTop.Bottom - Ambar.Rectangle.Top;
            }
            else
            {
                proxRectTopLoc = 0f;
            }

            if (selectedDepo != null)
            {
                if (!errorProvider.HasErrors)
                {
                    float ambar_boyu_CM = (Ambar.AmbarBoyu * 100) - selectedDepo.Cm_Height;

                    top_padding = txt_top * pxY + proxRectTopLoc;

                    if (txt_top >= 0 && txt_top <= ambar_boyu_CM)
                    {
                        selectedDepo.Rectangle = new RectangleF(selectedDepo.Rectangle.X,
                            Ambar.Rectangle.Y + top_padding, selectedDepo.Rectangle.Width,
                            selectedDepo.Rectangle.Height);

                        selectedDepo.OriginalRectangle = new RectangleF(selectedDepo.Rectangle.X,
                            Ambar.Rectangle.Y + top_padding, selectedDepo.Rectangle.Width,
                            selectedDepo.Rectangle.Height);

                        if (selectedDepo.gridmaps.Count > 0)
                        {
                            DataCell = selectedDepo.gridmaps.FirstOrDefault();

                            selectedDepo.gridmaps.Clear();

                            selectedDepo.CreateGridMapMenuItem(DataCell.CellEni, DataCell.CellBoyu,
                                DataCell.DikeyKenarBoslugu, DataCell.YatayKenarBoslugu, DataCell.NesneEni,
                                DataCell.NesneBoyu);
                        }

                        drawingPanel.Invalidate();
                    }
                    else
                    {
                        CustomNotifyIcon notify = new CustomNotifyIcon();
                        notify.showAlert("Alanın dışına çıkamazsınız.", CustomNotifyIcon.enmType.Error);
                        drawingPanel.Invalidate();
                    }
                }
            }
            if (selectedConveyor != null)
            {
                if (!errorProvider.HasErrors)
                {
                    float ambar_boyu_CM = (Ambar.AmbarBoyu * 100) -
                        selectedConveyor.ConveyorBoyu * 100;

                    top_padding = txt_top * pxY + proxRectTopLoc;

                    if (txt_top >= 0 && txt_top <= ambar_boyu_CM)
                    {
                        selectedConveyor.Rectangle =
                            new RectangleF(selectedConveyor.Rectangle.X,
                            Ambar.Rectangle.Y + top_padding,
                            selectedConveyor.Rectangle.Width,
                            selectedConveyor.Rectangle.Height);

                        selectedConveyor.OriginalRectangle =
                            new RectangleF(selectedConveyor.Rectangle.X,
                            Ambar.Rectangle.Y + top_padding,
                            selectedConveyor.Rectangle.Width,
                            selectedConveyor.Rectangle.Height);

                        foreach (var reff in selectedConveyor.ConveyorReferencePoints)
                        {
                            reff.Rectangle = new RectangleF
                                (selectedConveyor.Rectangle.X + reff.OriginalLocationInsideParent.X,
                                selectedConveyor.Rectangle.Y + reff.OriginalLocationInsideParent.Y,
                                reff.Rectangle.Width, reff.Rectangle.Height);
                        }

                        drawingPanel.Invalidate();
                    }
                    else
                    {
                        CustomNotifyIcon notify = new CustomNotifyIcon();
                        notify.showAlert("Alanın dışına çıkamazsınız.", CustomNotifyIcon.enmType.Error);
                        drawingPanel.Invalidate();
                    }
                }
            }
        }
        private void btn_Yer_Onayla_Click(object sender, EventArgs e)
        {
            if (selectedDepo != null)
            {
                bool intersect = CheckifIntersects(selectedDepo.Rectangle, RectangleF.Empty,
                        selectedDepo, null);
                if (intersect)
                {
                    CustomNotifyIcon notify = new CustomNotifyIcon();
                    notify.showAlert("Bu alan başka bir alanın sınırlarına giremez.",
                        CustomNotifyIcon.enmType.Error);
                    selectedDepo.Rectangle = UnchangedselectedDepoRectangle;
                    selectedDepo.OriginalRectangle = selectedDepo.Rectangle;
                    selectedDepo.LocationofRect = new System.Drawing.Point((int)selectedDepo.Rectangle.X, (int)selectedDepo.Rectangle.Y);
                    selectedDepo = null;
                    SelectedDepoPen.Color = System.Drawing.Color.Black;
                    SelectedDepoPen.Width = 1;
                    SelectedDepoEdgePen.Width = 1;
                    Manuel_Move = false;
                    GVisual.HideControl(btn_Manuel_Move, drawingPanel);
                    GVisual.HideControl(PaddingPanel, drawingPanel);
                    DrawingPanelEnlarge(PaddingPanel, this);
                    drawingPanel.Invalidate();
                }
                else
                {
                    selectedDepo.DepoAlaniEni = txt_width;
                    selectedDepo.DepoAlaniBoyu = txt_height;
                    selectedDepo.Cm_Width = selectedDepo.DepoAlaniEni * 100;
                    selectedDepo.Cm_Height = selectedDepo.DepoAlaniBoyu * 100;

                    selectedDepo.OriginalRectangle =
                        new RectangleF(selectedDepo.Rectangle.X,
                        selectedDepo.Rectangle.Y, selectedDepo.Rectangle.Width,
                        selectedDepo.Rectangle.Height);

                    selectedDepo.OriginalDepoSizeWidth = selectedDepo.OriginalRectangle.Width;
                    selectedDepo.OriginalDepoSizeHeight = selectedDepo.OriginalRectangle.Height;

                    selectedDepo.OriginalDepoSize = new SizeF(selectedDepo.OriginalDepoSizeWidth, selectedDepo.OriginalDepoSizeHeight);

                    selectedDepo.LocationofRect = new System.Drawing.Point((int)selectedDepo.Rectangle.X, (int)selectedDepo.Rectangle.Y);
                    SelectedDepoPen.Color = System.Drawing.Color.Black;
                    SelectedDepoPen.Width = 1;
                    SelectedDepoEdgePen.Width = 1;
                    Manuel_Move = false;
                    GVisual.HideControl(btn_Manuel_Move, drawingPanel);
                    GVisual.HideControl(PaddingPanel, drawingPanel);
                    DrawingPanelEnlarge(PaddingPanel, this);
                    selectedDepo = null;
                    drawingPanel.Invalidate();
                }
            }
            if (selectedConveyor != null)
            {
                bool intersect = CheckifIntersects(RectangleF.Empty,
                    selectedConveyor.Rectangle, null, selectedConveyor);
                if (intersect)
                {
                    CustomNotifyIcon notify = new CustomNotifyIcon();
                    notify.showAlert("Bu alan başka bir alanın sınırlarına giremez.",
                        CustomNotifyIcon.enmType.Error);
                    selectedConveyor.Rectangle = UnchangedselectedConveyorRectangle;
                    selectedConveyor.OriginalRectangle = selectedConveyor.Rectangle;
                    selectedConveyor.LocationofRect = new System.Drawing.Point((int)selectedConveyor.Rectangle.X,
                        (int)selectedConveyor.Rectangle.Y);
                    selectedConveyor = null;
                    SelectedConveyorPen.Color = System.Drawing.Color.Black;
                    SelectedConveyorPen.Width = 1;
                    SelectedConveyorEdgePen.Width = 1;
                    Manuel_Move = false;
                    GVisual.HideControl(btn_Manuel_Move, drawingPanel);
                    GVisual.HideControl(PaddingPanel, drawingPanel);
                    DrawingPanelEnlarge(PaddingPanel, this);
                    drawingPanel.Invalidate();
                }
                else
                {
                    selectedConveyor.ConveyorBoyu = float.Parse(txt_Height.Text);
                    selectedConveyor.ConveyorEni = float.Parse(txt_Width.Text);

                    selectedConveyor.OriginalRectangle =
                        new RectangleF(selectedConveyor.Rectangle.X,
                        selectedConveyor.Rectangle.Y,
                        selectedConveyor.Rectangle.Width,
                        selectedConveyor.Rectangle.Height);

                    selectedConveyor.LocationofRect = new System.Drawing.Point((int)selectedConveyor.Rectangle.X,
                        (int)selectedConveyor.Rectangle.Y);
                    Manuel_Move = false;
                    selectedConveyor = null;
                    SelectedConveyorPen.Color = System.Drawing.Color.Black;
                    SelectedConveyorPen.Width = 1;
                    SelectedConveyorEdgePen.Width = 1;
                    GVisual.HideControl(btn_Manuel_Move, drawingPanel);
                    GVisual.HideControl(PaddingPanel, drawingPanel);
                    DrawingPanelEnlarge(PaddingPanel, this);
                    drawingPanel.Invalidate();
                }
            }
        }
        private void btn_Padding_Vazgeç_Click(object sender, EventArgs e)
        {
            if (selectedDepo != null)
            {
                selectedDepo.Rectangle = UnchangedselectedDepoRectangle;
                selectedDepo.OriginalRectangle = selectedDepo.Rectangle;
                selectedDepo.LocationofRect = new System.Drawing.Point((int)selectedDepo.Rectangle.X, (int)selectedDepo.Rectangle.Y);
                selectedDepo = null;
                SelectedDepoPen.Color = System.Drawing.Color.Black;
                SelectedDepoPen.Width = 1;
                SelectedDepoEdgePen.Width = 1;
                Manuel_Move = false;
                GVisual.HideControl(btn_Manuel_Move, drawingPanel);
                GVisual.HideControl(PaddingPanel, drawingPanel);
                drawingPanel.Invalidate();
            }
            if (selectedConveyor != null)
            {
                selectedConveyor.Rectangle = UnchangedselectedConveyorRectangle;
                selectedConveyor.OriginalRectangle = selectedConveyor.Rectangle;
                selectedConveyor.LocationofRect = new System.Drawing.Point((int)selectedConveyor.Rectangle.X,
                        (int)selectedConveyor.Rectangle.Y);
                SelectedConveyorPen.Color = System.Drawing.Color.Black;
                SelectedConveyorPen.Width = 1;
                SelectedConveyorEdgePen.Width = 1;
                foreach (var reff in selectedConveyor.ConveyorReferencePoints)
                {
                    reff.Rectangle = new RectangleF
                        (selectedConveyor.Rectangle.X + reff.OriginalLocationInsideParent.X,
                        selectedConveyor.Rectangle.Y + reff.OriginalLocationInsideParent.Y,
                        reff.Rectangle.Width, reff.Rectangle.Height);
                }
                AdjustFixedConveyorReferencePoints(selectedConveyor);
                selectedConveyor = null;
                Manuel_Move = false;
                GVisual.HideControl(btn_Manuel_Move, drawingPanel);
                GVisual.HideControl(PaddingPanel, drawingPanel);
                drawingPanel.Invalidate();
            }
            DrawingPanelEnlarge(PaddingPanel, this);
        }
        private bool CheckifIntersects(RectangleF DepoRect, RectangleF ConveyorRect,
            Depo? selectedDepo, Conveyor? selectedConveyor)
        {
            if (Ambar != null)
            {
                foreach (var depo in Ambar.depolar)
                {
                    if (!DepoRect.IsEmpty &&
                        depo.Rectangle.IntersectsWith(DepoRect) &&
                        DepoRect != depo.Rectangle &&
                        selectedDepo != null &&
                        selectedDepo != depo)
                    {
                        return true;
                    }
                    if (!ConveyorRect.IsEmpty &&
                        depo.Rectangle.IntersectsWith(ConveyorRect) &&
                        selectedConveyor != null)
                    {
                        return true;
                    }
                }
                foreach (var conveyor in Ambar.conveyors)
                {
                    if (!DepoRect.IsEmpty &&
                        conveyor.Rectangle.IntersectsWith(DepoRect) &&
                        selectedDepo != null)
                    {
                        return true;
                    }
                    if (!ConveyorRect.IsEmpty &&
                                conveyor.Rectangle.IntersectsWith(ConveyorRect) &&
                                ConveyorRect != conveyor.Rectangle &&
                                selectedConveyor != null &&
                                selectedConveyor != conveyor)
                    {
                        return true;
                    }
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        public bool CheckifIntersectsWithArea(RectangleF rect)
        {
            if (Ambar != null)
            {
                foreach (var depo in Ambar.depolar)
                {
                    if (rect.IntersectsWith(depo.Rectangle))
                    {
                        return true;
                    }
                }
                foreach (var conveyor in Ambar.conveyors)
                {
                    if (rect.IntersectsWith(conveyor.Rectangle))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        //Alan Button Events
        private void btn_Alan_Click(object sender, EventArgs e)
        {
            if (!CheckPanelVisible(Alan_Olusturma_Paneli))
            {
                HideEverything();
                DrawingPanelShrink(Alan_Olusturma_Paneli, this, leftSidePanelLocation);
            }
            else
            {
                HideEverything();
                GVisual.ShowControl(Alan_Olusturma_Paneli, this, leftSidePanelLocation);
                GVisual.ChangeSize_of_Control(drawingPanel, drawingPanelSmallSize);
                GVisual.Move_RightSide_of_AnotherControl(drawingPanel, Alan_Olusturma_Paneli, 7);
            }
        }
        private void btn_Alan_Olustur_Click(object sender, EventArgs e)
        {
            if (ambar_Boyut_Degistir && Ambar != null)
            {
                errorProvider.SetError(txt_Alan_Eni, string.Empty);
                errorProvider.SetError(txt_Alan_Boyu, string.Empty);
                errorProvider.Clear();

                float alan_eni = StrLib.ReplaceDotWithCommaReturnFloat(txt_Alan_Eni, errorProvider,
                    "Bu alan boş bırakılamaz.", "Lütfen buraya bir sayı giriniz.", "Buraya 0 ya da daha küçük bir değer giremezsiniz.");
                float alan_boyu = StrLib.ReplaceDotWithCommaReturnFloat(txt_Alan_Boyu, errorProvider,
                    "Bu alan boş bırakılamaz.", "Lütfen buraya bir sayı giriniz.", "Buraya 0 ya da daha küçük bir değer giremezsiniz.");

                if (!errorProvider.HasErrors)
                {
                    Ambar.Rectangle = GVisual.RatioRectangleToPanel(drawingPanel, Ambar.Rectangle);
                    System.Drawing.Point point = GVisual.GetCenter(drawingPanel.ClientRectangle);
                    Ambar.Rectangle = new RectangleF(point.X - Ambar.Rectangle.Width / 2,
                        point.Y - Ambar.Rectangle.Height / 2, Ambar.Rectangle.Width, Ambar.Rectangle.Height);
                    Ambar.OriginalRectangle = Ambar.Rectangle;
                    Ambar.AmbarEni = alan_eni;
                    Ambar.AmbarBoyu = alan_boyu;
                    Ambar.LocationofRect = new System.Drawing.Point((int)Ambar.Rectangle.X, (int)Ambar.Rectangle.Y);
                    Ambar = Ambar;
                    drawingPanel.Invalidate();
                    DrawingPanelEnlarge(Alan_Olusturma_Paneli, this);
                    ambar_Boyut_Degistir = false;
                }
            }
            else
            {
                if (Ambar != null)
                {
                    var result = MessageBox.Show
                    ("Halihazırda bulunan alan silinecektir, devam etmek istiyor musunuz?",
                     "Devam Etmek İstiyor Musunuz?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        Ambar = null;
                        errorProvider.SetError(txt_Alan_Eni, string.Empty);
                        errorProvider.SetError(txt_Alan_Boyu, string.Empty);
                        errorProvider.Clear();

                        float alan_eni = StrLib.ReplaceDotWithCommaReturnFloat(txt_Alan_Eni, errorProvider,
                            "Bu alan boş bırakılamaz.", "Lütfen buraya bir sayı giriniz.", "Buraya 0 ya da daha küçük bir değer giremezsiniz.");
                        float alan_boyu = StrLib.ReplaceDotWithCommaReturnFloat(txt_Alan_Boyu, errorProvider,
                            "Bu alan boş bırakılamaz.", "Lütfen buraya bir sayı giriniz.", "Buraya 0 ya da daha küçük bir değer giremezsiniz.");


                        if (!errorProvider.HasErrors)
                        {
                            Ambar ambar = new Ambar(0, 0, alan_eni, alan_boyu, Main, this);

                            ambar.Rectangle = GVisual.RatioRectangleToPanel(drawingPanel, ambar.Rectangle);
                            System.Drawing.Point point = GVisual.GetCenter(drawingPanel.ClientRectangle);
                            ambar.Rectangle = new RectangleF(point.X - ambar.Rectangle.Width / 2,
                                point.Y - ambar.Rectangle.Height / 2, ambar.Rectangle.Width, ambar.Rectangle.Height);
                            ambar.OriginalRectangle = ambar.Rectangle;
                            ambar.AmbarEni = alan_eni;
                            ambar.AmbarBoyu = alan_boyu;
                            ambar.LocationofRect = new System.Drawing.Point((int)ambar.Rectangle.X, (int)ambar.Rectangle.Y);
                            Ambar = ambar;
                            selDepo = new Depo(10, 10, 10, 10, 1f, Main, this, Ambar);
                            selConveyor = new Conveyor(10, 10, 10, 10, Main, this, Ambar);
                            drawingPanel.Invalidate();
                            DrawingPanelEnlarge(Alan_Olusturma_Paneli, this);
                        }
                    }
                    else
                    {
                        DrawingPanelEnlarge(Alan_Olusturma_Paneli, this);
                    }
                }
                else
                {
                    errorProvider.SetError(txt_Alan_Eni, string.Empty);
                    errorProvider.SetError(txt_Alan_Boyu, string.Empty);
                    errorProvider.Clear();

                    float alan_eni = StrLib.ReplaceDotWithCommaReturnFloat(txt_Alan_Eni, errorProvider,
                        "Bu alan boş bırakılamaz.", "Lütfen buraya bir sayı giriniz.", "Buraya 0 ya da daha küçük bir değer giremezsiniz.");
                    float alan_boyu = StrLib.ReplaceDotWithCommaReturnFloat(txt_Alan_Boyu, errorProvider,
                        "Bu alan boş bırakılamaz.", "Lütfen buraya bir sayı giriniz.", "Buraya 0 ya da daha küçük bir değer giremezsiniz.");

                    if (!errorProvider.HasErrors)
                    {
                        Ambar ambar = new Ambar(0, 0, alan_eni, alan_boyu, Main, this);

                        ambar.Rectangle = GVisual.RatioRectangleToPanel(drawingPanel, ambar.Rectangle);
                        System.Drawing.Point point = GVisual.GetCenter(drawingPanel.ClientRectangle);
                        ambar.Rectangle = new RectangleF(point.X - ambar.Rectangle.Width / 2,
                            point.Y - ambar.Rectangle.Height / 2, ambar.Rectangle.Width, ambar.Rectangle.Height);
                        ambar.OriginalRectangle = ambar.Rectangle;
                        ambar.AmbarEni = alan_eni;
                        ambar.AmbarBoyu = alan_boyu;
                        ambar.LocationofRect = new System.Drawing.Point((int)ambar.Rectangle.X, (int)ambar.Rectangle.Y);
                        Ambar = ambar;
                        drawingPanel.Invalidate();
                        DrawingPanelEnlarge(Alan_Olusturma_Paneli, this);
                    }
                }
            }
        }
        private void btn_Alan_Olustur_Vazgec_Click(object sender, EventArgs e)
        {
            DrawingPanelEnlarge(Alan_Olusturma_Paneli, this);
            ambar_Boyut_Degistir = false;
        }
        private void boyutunuDeğiştirToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (Ambar != null)
            {
                ambar_Boyut_Degistir = true;
                if (!CheckPanelVisible(Alan_Olusturma_Paneli))
                {
                    HideEverything();
                    DrawingPanelShrink(Alan_Olusturma_Paneli, this, leftSidePanelLocation);
                }
                else
                {
                    HideEverything();
                    GVisual.ShowControl(Alan_Olusturma_Paneli, this, leftSidePanelLocation);
                    GVisual.ChangeSize_of_Control(drawingPanel, drawingPanelSmallSize);
                    GVisual.Move_RightSide_of_AnotherControl(drawingPanel, Alan_Olusturma_Paneli, 7);
                }
            }
            else
            {
                MessageBox.Show("Lütfen önce alan oluşturun.", "Alan yok.", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }



        //Conveyor Button Events
        private void btn_Conveyor_Click(object sender, EventArgs e)
        {
            if (Ambar != null)
            {
                if (!CheckPanelVisible(Conveyor_Olusturma_Paneli))
                {
                    HideEverything();
                    DrawingPanelShrink(Conveyor_Olusturma_Paneli, this, leftSidePanelLocation);
                }
                else
                {
                    HideEverything();
                    GVisual.ShowControl(Conveyor_Olusturma_Paneli, this, leftSidePanelLocation);
                    GVisual.ChangeSize_of_Control(drawingPanel, drawingPanelSmallSize);
                    GVisual.Move_RightSide_of_AnotherControl(drawingPanel, Conveyor_Olusturma_Paneli, 7);
                }
            }
            else
            {
                MessageBox.Show("Lütfen önce alan oluşturun.", "Alan yok.", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }

        }
        private void btn_Conveyor_Olustur_Click(object sender, EventArgs e)
        {
            if (Ambar != null)
            {
                float conveyor_eni = StrLib.ReplaceDotWithCommaReturnFloat(txt_Conveyor_Eni,
                    errorProvider, "Bu alan boş bırakılamaz.", "Lütfen buraya bir sayı giriniz.", "Buraya 0 ya da daha küçük bir değer giremezsiniz.");

                float conveyor_boyu = StrLib.ReplaceDotWithCommaReturnFloat(txt_Conveyor_Boyu,
                    errorProvider, "Bu alan boş bırakılamaz.", "Lütfen buraya bir sayı giriniz.", "Buraya 0 ya da daha küçük bir değer giremezsiniz.");

                if (conveyor_eni > Ambar.AmbarEni)
                {
                    errorProvider.SetError(txt_Conveyor_Eni,
                        "Conveyor'un eni alanın eninden büyük olamaz.");
                }
                if (conveyor_boyu > Ambar.AmbarBoyu)
                {
                    errorProvider.SetError(txt_Conveyor_Eni,
                        "Conveyor'un boyu alanın boyundan büyük olamaz.");
                }

                if (!errorProvider.HasErrors)
                {
                    Conveyor conveyor = new Conveyor(0, 0, conveyor_eni, conveyor_boyu, null, this, Ambar);
                    conveyor.Rectangle = GVisual.RatioRectangleToParentRectangle(conveyor_eni,
                        conveyor_boyu, Ambar.AmbarEni, Ambar.AmbarBoyu, Ambar.Rectangle);
                    conveyor.ConveyorEni = conveyor_eni;
                    conveyor.ConveyorBoyu = conveyor_boyu;

                    SearchForLocationtoPlace(conveyor.Rectangle, conveyor, null, Ambar);

                    drawingPanel.Invalidate();
                    DrawingPanelEnlarge(Conveyor_Olusturma_Paneli, this);
                }
            }
        }
        private void btn_Conveyor_Olustur_Vazgec_Click(object sender, EventArgs e)
        {
            DrawingPanelEnlarge(Conveyor_Olusturma_Paneli, this);
        }
        private void btn_Conveyor_Olusturma_Kapat_Click(object sender, EventArgs e)
        {
            DrawingPanelEnlarge(Conveyor_Olusturma_Paneli, this);
        }


        public bool CheckPanelVisible(Panel panel)
        {
            bool isVisible = false;
            foreach (var panel1 in this.Controls)
            {
                if (panel1 is Panel)
                {
                    Panel newPanel = (Panel)panel1;

                    if (newPanel != drawingPanel && newPanel != ButtonPanel)
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



        private void SearchForLocationtoPlace(RectangleF rectangle,
            Conveyor? conveyor, Depo? depo,
            Ambar Ambar)
        {
            Random random = new Random();
            bool foundEmptySpace = false;
            int maxAttempts = 1000;
            int attempts = 0;
            RectangleF newRectangle = new RectangleF();

            float minY = Ambar.Rectangle.Top;
            float minX = Ambar.Rectangle.Left;
            float maxX = Ambar.Rectangle.Right - rectangle.Width;
            float maxY = Ambar.Rectangle.Bottom - rectangle.Height;

            while (!foundEmptySpace && attempts < maxAttempts)
            {
                attempts++;

                float randomX = (float)(random.NextDouble() * (maxX - minX) + minX);
                float randomY = (float)(random.NextDouble() * (maxY - minY) + minY);

                newRectangle = new RectangleF(randomX, randomY, rectangle.Width, rectangle.Height);

                bool isIntersecting = CheckifIntersectsWithArea(newRectangle);

                if (!isIntersecting)
                {
                    foundEmptySpace = true;
                    if (conveyor != null)
                    {
                        conveyor.Rectangle = newRectangle;
                        conveyor.OriginalRectangle = conveyor.Rectangle;
                        Ambar.conveyors.Add(conveyor);
                    }

                    if (depo != null)
                    {
                        depo.Rectangle = newRectangle;
                        depo.OriginalRectangle = depo.Rectangle;
                        Ambar.depolar.Add(depo);
                    }
                }
            }
            if (!foundEmptySpace)
            {
                CustomNotifyIcon notify = new CustomNotifyIcon();
                notify.showAlert("Bu alanda yer kalmadı lütfen yer açıp tekrar deneyin.", CustomNotifyIcon.enmType.Error);
            }
        }



        //Depo Button Events
        private void btn_Depo_Click(object sender, EventArgs e)
        {
            if (Ambar != null)
            {
                if (!CheckPanelVisible(Depo_Bilgi_Panel))
                {
                    HideEverything();
                    DrawingPanelShrink(Depo_Bilgi_Panel, this, leftSidePanelLocation);
                }
                else
                {
                    HideEverything();
                    GVisual.ShowControl(Depo_Bilgi_Panel, this, leftSidePanelLocation);
                    GVisual.ChangeSize_of_Control(drawingPanel, drawingPanelSmallSize);
                    GVisual.Move_RightSide_of_AnotherControl(drawingPanel, Depo_Bilgi_Panel, 7);
                }
            }
            else
            {
                MessageBox.Show("Lütfen önce alan oluşturun.", "Alan yok.", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }

        }
        private void btn_Depo_Olustur_Click(object sender, EventArgs e)
        {
            if (Ambar != null)
            {
                errorProvider.SetError(txt_Depo_Eni, string.Empty);
                errorProvider.SetError(txt_Depo_Boyu, string.Empty);
                errorProvider.SetError(txt_Depo_Yuksekligi, string.Empty);
                errorProvider.Clear();

                int depo_Yuksekligi = StrLib.CheckIntTextbox(txt_Depo_Yuksekligi,
                    errorProvider, "Bu alan boş bırakılamaz.", "Buraya lütfen bir tam sayı giriniz.", "Buraya 0 ya da daha küçük bir değer giremezsiniz.");
                float depo_eni = StrLib.ReplaceDotWithCommaReturnFloat(txt_Depo_Eni,
                    errorProvider, "Bu alan boş bırakılamaz.", "Buraya lütfen bir sayı giriniz.", "Buraya 0 ya da daha küçük bir değer giremezsiniz.");
                float depo_boyu = StrLib.ReplaceDotWithCommaReturnFloat(txt_Depo_Boyu,
                    errorProvider, "Bu alan boş bırakılamaz.", "Buraya lütfen bir sayı giriniz.", "Buraya 0 ya da daha küçük bir değer giremezsiniz.");
                string depo_name = txt_Depo_Adi.Text;
                string depo_description = txt_Depo_Aciklamasi.Text;
                string depo_item_kind = txt_Depo_Item_Turu.Text;

                if (depo_eni > Ambar.AmbarEni)
                {
                    errorProvider.SetError(txt_Depo_Eni, "Deponun eni alanın eninden büyük olamaz.");
                }
                if (depo_boyu > Ambar.AmbarBoyu)
                {
                    errorProvider.SetError(txt_Depo_Boyu, "Deponun boyu alanın boyundan büyük olamaz.");
                }

                if (!errorProvider.HasErrors)
                {
                    Depo depo = new Depo(0, 0, depo_eni, depo_boyu, 1f, Main, this, Ambar);

                    depo.Rectangle = GVisual.RatioRectangleToParentRectangle(depo_eni,
                        depo_boyu, Ambar.AmbarEni, Ambar.AmbarBoyu, Ambar.Rectangle);

                    depo.OriginalRectangle = depo.Rectangle;
                    depo.LocationofRect = new System.Drawing.Point((int)depo.Rectangle.X, (int)depo.Rectangle.Y);
                    depo.OriginalDepoSize = depo.Rectangle.Size;
                    depo.KareX = depo.Rectangle.X;
                    depo.KareY = depo.Rectangle.Y;
                    depo.KareEni = depo.Rectangle.Width;
                    depo.KareBoyu = depo.Rectangle.Height;
                    depo.DepoAlaniEni = depo_eni;
                    depo.DepoAlaniBoyu = depo_boyu;
                    depo.DepoAlaniYuksekligi = depo_Yuksekligi;
                    depo.OriginalKareX = depo.OriginalRectangle.X;
                    depo.OriginalKareY = depo.OriginalRectangle.Y;
                    depo.OriginalKareEni = depo.OriginalRectangle.Width;
                    depo.OriginalKareBoyu = depo.OriginalRectangle.Height;
                    depo.OriginalDepoSizeWidth = depo.OriginalRectangle.Width;
                    depo.OriginalDepoSizeHeight = depo.OriginalRectangle.Height;
                    depo.DepoName = depo_name;
                    depo.DepoDescription = depo_description;
                    depo.ItemTuru = depo_item_kind;
                    depo.Yerlestirilme_Sirasi = Ware_Counter;
                    depo.itemDrop_LeftRight = "Sağa Doğru";
                    depo.itemDrop_UpDown = "Yukarı Doğru";
                    depo.itemDrop_StartLocation = "Aşağıdan";

                    SearchForLocationtoPlace(depo.Rectangle, null, depo, Ambar);

                    Ware_Counter++;
                    drawingPanel.Invalidate();
                    DrawingPanelEnlarge(Depo_Olusturma_Paneli, this);

                    AdjustTextboxesText($"{depo.DepoAlaniEni}", $"{depo.DepoAlaniBoyu}",
                        null, null, null, null);
                }
            }
        }
        private void btn_Depo_Olustur_Vazgec_Click(object sender, EventArgs e)
        {
            DrawingPanelEnlarge(Depo_Olusturma_Paneli, this);
        }
        private void btn_Depo_Olusturma_Kapat_Click(object sender, EventArgs e)
        {
            DrawingPanelEnlarge(Depo_Olusturma_Paneli, this);
        }
        private void btn_Depo_Bilgi_Panel_Sonraki_Sayfa_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();

            string depo_name = txt_Depo_Adi.Text;
            string depo_description = txt_Depo_Aciklamasi.Text;
            string depo_item_kind = txt_Depo_Item_Turu.Text;

            if (Ambar != null)
            {
                foreach (var depo in Ambar.depolar)
                {
                    if (depo.DepoName == depo_name)
                    {
                        errorProvider.SetError(txt_Depo_Adi, "Bu ad ile bir depo zaten var, lütfen değiştirip tekrar deneyin.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Lütfen önce alan oluşturun.", "Alan yok."
                    , MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


            if (string.IsNullOrWhiteSpace(depo_name))
            {
                errorProvider.SetError(txt_Depo_Adi, "Bu alan boş bırakılamaz.");
            }

            if (string.IsNullOrWhiteSpace(depo_description))
            {
                errorProvider.SetError(txt_Depo_Aciklamasi, "Bu alan boş bırakılamaz.");
            }

            if (depo_name.Length > 50)
            {
                errorProvider.SetError(txt_Depo_Adi, "Deponun 50 karakterden uzun bir isimi olamaz.");
            }
            if (depo_item_kind.Length > 50)
            {
                errorProvider.SetError(txt_Depo_Item_Turu, "Nesne türü 50 karakterden uzun olamaz.");
            }

            if (!errorProvider.HasErrors)
            {
                GVisual.HideControl(Depo_Bilgi_Panel, this);
                GVisual.ShowControl(Depo_Olusturma_Paneli, this, leftSidePanelLocation);
            }

        }
        private void btn_Depo_Bilgi_Panel_Vazgec_Click(object sender, EventArgs e)
        {
            DrawingPanelEnlarge(Depo_Bilgi_Panel, this);
        }
        private void btn_Depo_Bilgi_Panel_Kapat_Click(object sender, EventArgs e)
        {
            DrawingPanelEnlarge(Depo_Bilgi_Panel, this);
        }


        private void izgaraHaritasiOlusturEventHandler(object? sender, EventArgs e)
        {

        }
        private void btn_Izgara_Haritasi_Click(object sender, EventArgs e)
        {
            if (Ambar != null)
            {
                if (Ambar.depolar.Count > 0)
                {
                    ToolStripIzgara = true;
                    if (!CheckPanelVisible(Izgara_Mal_Paneli))
                    {
                        HideEverything();
                        DrawingPanelShrink(Izgara_Mal_Paneli, this, leftSidePanelLocation);
                    }
                    else
                    {
                        HideEverything();
                        GVisual.ShowControl(Izgara_Mal_Paneli, this, leftSidePanelLocation);
                        GVisual.ChangeSize_of_Control(drawingPanel, drawingPanelSmallSize);
                        GVisual.Move_RightSide_of_AnotherControl(drawingPanel, Izgara_Mal_Paneli, 7);
                    }
                }
                else
                {
                    CustomNotifyIcon notify = new CustomNotifyIcon();
                    notify.showAlert("Izgara haritası oluşturmak için öncelikle depo alanı oluşturmalısınız", CustomNotifyIcon.enmType.Error);
                }
            }
            else
            {
                MessageBox.Show("Lütfen önce alan oluşturun", "Alan yok", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }
        private void btn_Izgara_Mal_Kapat_Click(object sender, EventArgs e)
        {
            DrawingPanelEnlarge(Izgara_Mal_Paneli, this);
            GVisual.Clear_Krypton_TextBoxes(Izgara_Mal_Paneli, null);
        }
        private void btn_Izgara_Mal_Devam_Et_Click(object sender, EventArgs e)
        {
            errorProvider.SetError(txt_Nesnenin_Boyu, string.Empty);
            errorProvider.SetError(txt_Nesnenin_Eni, string.Empty);
            errorProvider.SetError(txt_Nesnenin_Yuksekligi, string.Empty);
            errorProvider.Clear();

            nesne_Yuksekligi = StrLib.CheckIntTextbox(txt_Nesnenin_Yuksekligi,
                errorProvider, "Bu alan boş bırakılamaz.", "Buraya lütfen bir tam sayı giriniz.", "Buraya 0 ya da daha küçük bir değer giremezsiniz.");
            nesne_Eni = StrLib.ReplaceDotWithCommaReturnFloat(txt_Nesnenin_Eni,
                errorProvider, "Bu alan boş bırakılamaz.", "Buraya lütfen bir sayı giriniz.", "Buraya 0 ya da daha küçük bir değer giremezsiniz.");
            nesne_Boyu = StrLib.ReplaceDotWithCommaReturnFloat(txt_Nesnenin_Boyu,
                errorProvider, "Bu alan boş bırakılamaz.", "Buraya lütfen bir sayı giriniz.", "Buraya 0 ya da daha küçük bir değer giremezsiniz.");

            if (!errorProvider.HasErrors)
            {
                GVisual.HideControl(Izgara_Mal_Paneli, this);
                GVisual.ShowControl(Izgara_Olusturma_Paneli, this, leftSidePanelLocation);
            }
        }
        private void btn_Izgara_Mal_Vazgec_Click(object sender, EventArgs e)
        {
            DrawingPanelEnlarge(Izgara_Mal_Paneli, this);
            GVisual.Clear_Krypton_TextBoxes(Izgara_Mal_Paneli, null);
        }
        private void btn_Izgara_Olustur_Kapat_Click(object sender, EventArgs e)
        {
            DrawingPanelEnlarge(Izgara_Olusturma_Paneli, this);
            GVisual.Clear_Krypton_TextBoxes(Izgara_Mal_Paneli, null);
            GVisual.Clear_Krypton_TextBoxes(Izgara_Olusturma_Paneli, null);
        }
        private void btn_Izgara_Olustur_Click(object sender, EventArgs e)
        {
            errorProvider.SetError(txt_Dikey_Kenar_Boslugu, string.Empty);
            errorProvider.SetError(txt_Yatay_Kenar_Boslugu, string.Empty);
            errorProvider.Clear();

            float hucre_Dikey_Bosluk =
                StrLib.ReplaceDotWithCommaReturnFloat(txt_Dikey_Kenar_Boslugu, errorProvider,
                "Bu alan boş bırakılamaz.", "Buraya lütfen bir sayı giriniz.", "Buraya 0 ya da daha küçük bir değer giremezsiniz.");

            float hucre_Yatay_Bosluk =
                StrLib.ReplaceDotWithCommaReturnFloat(txt_Yatay_Kenar_Boslugu, errorProvider,
                "Bu alan boş bırakılamaz.", "Buraya lütfen bir sayı giriniz.", "Buraya 0 ya da daha küçük bir değer giremezsiniz.");

            total_Cell_Width = nesne_Eni + hucre_Yatay_Bosluk;
            total_Cell_Height = nesne_Boyu + hucre_Dikey_Bosluk;

            if (!errorProvider.HasErrors)
            {
                if (selectedDepo != null && ToolStripIzgara == false)
                {
                    if (selectedDepo.gridmaps.Count > 0)
                    {
                        selectedDepo.gridmaps.Clear();

                        selectedDepo.nesneEni = nesne_Eni;
                        selectedDepo.nesneBoyu = nesne_Boyu;
                        selectedDepo.nesneYuksekligi = nesne_Yuksekligi;
                        selectedDepo.CreateGridMapMenuItem(total_Cell_Width, total_Cell_Height,
                            hucre_Dikey_Bosluk, hucre_Yatay_Bosluk, nesne_Eni, nesne_Boyu);
                        DrawingPanelEnlarge(Izgara_Olusturma_Paneli, this);
                    }
                    else
                    {
                        selectedDepo.nesneEni = nesne_Eni;
                        selectedDepo.nesneBoyu = nesne_Boyu;
                        selectedDepo.nesneYuksekligi = nesne_Yuksekligi;
                        selectedDepo.CreateGridMapMenuItem(total_Cell_Width, total_Cell_Height,
                            hucre_Dikey_Bosluk, hucre_Yatay_Bosluk, nesne_Eni, nesne_Boyu);
                        DrawingPanelEnlarge(Izgara_Olusturma_Paneli, this);
                    }
                }
                else if (ToolStripIzgara)
                {
                    foreach (var depo in Ambar.depolar)
                    {
                        depo.gridmaps.Clear();
                    }
                    izgaraHaritasiOlustur.Invoke(sender, EventArgs.Empty);
                    DrawingPanelEnlarge(Izgara_Olusturma_Paneli, this);
                    ToolStripIzgara = false;
                }
            }
            drawingPanel.Invalidate();
        }
        private void btn_Izgara_Olustur_Iptal_Et_Click(object sender, EventArgs e)
        {
            DrawingPanelEnlarge(Izgara_Olusturma_Paneli, this);
            GVisual.Clear_Krypton_TextBoxes(Izgara_Mal_Paneli, null);
            GVisual.Clear_Krypton_TextBoxes(Izgara_Olusturma_Paneli, null);
        }
        


        private bool isPanelVisible(Panel depo_Panel, Panel izgara_Mal_Panel, Panel izgara_Panel,
            Panel alan_Panel, Panel conveyor_Panel, Panel padding_Panel)
        {
            if (depo_Panel.Visible)
            {
                return true;
            }
            else if (izgara_Panel.Visible)
            {
                return true;
            }
            else if (izgara_Mal_Panel.Visible)
            {
                return true;
            }
            else if (alan_Panel.Visible)
            {
                return true;
            }
            else if (conveyor_Panel.Visible)
            {
                return true;
            }
            else if (padding_Panel.Visible)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void DrawLeftLines(Pen pen, Graphics g,
           List<RectangleF> ProximityRectangles, RectangleF rectangle,
           RectangleF ParentRectangle)
        {
            float rectRight = 0f;
            float rectLeft = 0f;
            float rectBottom = 0f;
            float rectTop = 0f;

            float x = 0f;

            float rectangleRight = (float)Math.Round(rectangle.Right, 1);
            float rectangleLeft = (float)Math.Round(rectangle.Left, 1);
            float rectangleBottom = (float)Math.Round(rectangle.Bottom, 1);
            float rectangleTop = (float)Math.Round(rectangle.Top, 1);

            RectangleF proxRectLeft = new RectangleF();

            foreach (var rect in ProximityRectangles)
            {
                rectRight = (float)Math.Round(rect.Right, 1);
                rectLeft = (float)Math.Round(rect.Left, 1);
                rectBottom = (float)Math.Round(rect.Bottom, 1);
                rectTop = (float)Math.Round(rect.Top, 1);
                System.Drawing.Point rectrightMiddle = GVisual.GetMiddleOfRightEdge(rect);

                if (rect != ParentRectangle && rect != rectangle)
                {
                    if ((rectTop <= rectangleBottom && rectBottom >= rectangleTop) ||
                        (rectTop >= rectangleTop && rectBottom <= rectangleBottom))
                    {
                        if (rectRight <= rectangleLeft)
                        {
                            if (rectRight > x)
                            {
                                x = rectRight;
                                proxRectLeft = rect;
                            }
                        }
                    }
                }
            }
            if (!proxRectLeft.IsEmpty)
            {
                // Calculate the vertical midpoint of the overlapping area
                float overlapTop = Math.Max(proxRectLeft.Top, rectangle.Top);
                float overlapBottom = Math.Min(proxRectLeft.Bottom, rectangle.Bottom);
                float midpointY = overlapTop + (overlapBottom - overlapTop) / 2;

                float startX = proxRectLeft.Right;
                float endX = rectangle.Left;

                System.Drawing.Point startPoint = new System.Drawing.Point((int)startX, (int)midpointY);
                System.Drawing.Point endPoint = new System.Drawing.Point((int)endX, (int)midpointY);

                g.DrawLine(pen, startPoint, endPoint);

                float pxCM =
                    ConvertTwoRectanglesDistancetoCMHorizontally
                    (Ambar.AmbarEni, Ambar.AmbarBoyu, Ambar.Rectangle, proxRectLeft, rectangle, true);

                string widthCm = $"{(int)(pxCM)} cm";

                ProxRectLeft = proxRectLeft;

                if (Manuel_Move == true)
                {
                    AdjustTextboxesText(null, null, $"{(int)(pxCM)}", null, null, null);
                }
                else
                {
                    HideTextboxes();
                    GVisual.HideControl(btn_Yer_Onayla, drawingPanel);
                }

                SizeF textSize = g.MeasureString(widthCm, new System.Drawing.Font("Arial", 10));
                g.DrawString(widthCm, new System.Drawing.Font("Arial", 10),
                    new SolidBrush(System.Drawing.Color.Red),
                    new PointF(startX - textSize.Width, midpointY));
            }
            else
            {
                PointF pointRectangle = new PointF(rectangle.Left, rectangle.Y);
                PointF pointParentRect = new PointF((ParentRectangle.Left),
                    rectangle.Y);

                //string RectangleX =
                //$"Selected Rectangle: {rectangle}";

                //string parentRectangle =
                //$"Ambar Rectangle: {ParentRectangle}";

                //string PointsDiffX =
                //$"Difference Between Rectangle to Parent: {pointRectangle.X - pointParentRect.X}";

                //g.DrawString
                //    (RectangleX, new System.Drawing.Font("Arial", 8),
                //    new SolidBrush(System.Drawing.Color.Red),
                //    new PointF(Ambar.Rectangle.X, Ambar.Rectangle.Y + 20));
                //g.DrawString
                //    (parentRectangle, new System.Drawing.Font("Arial", 8),
                //    new SolidBrush(System.Drawing.Color.Red),
                //    new PointF(Ambar.Rectangle.X, Ambar.Rectangle.Y + 40));
                //g.DrawString
                //    (PointsDiffX, new System.Drawing.Font("Arial", 8),
                //    new SolidBrush(System.Drawing.Color.Red),
                //    new PointF(Ambar.Rectangle.X, Ambar.Rectangle.Y + 60));

                float pxCM = ConvertTwoRectanglesDistancetoCMHorizontally(Ambar.AmbarEni, Ambar.AmbarBoyu, Ambar.Rectangle, ParentRectangle, rectangle, false);
                string widthCm = $"{(int)(pxCM)} cm";

                ProxRectLeft = RectangleF.Empty;

                if (Manuel_Move)
                {
                    AdjustTextboxesText(null, null, $"{(int)(pxCM)}", null, null, null);
                }
                else
                {
                    HideTextboxes();
                    GVisual.HideControl(btn_Yer_Onayla, drawingPanel);
                }

                SizeF textSize = g.MeasureString(widthCm, new System.Drawing.Font("Arial", 10));

                g.DrawLine(pen, pointRectangle, pointParentRect);
                //g.DrawString(widthCm, new System.Drawing.Font("Arial", 8), 
                //    new SolidBrush(System.Drawing.Color.Red), 
                //    new PointF(pointRectangle.X - textSize.Width, pointRectangle.Y));

                g.DrawString(widthCm, new System.Drawing.Font("Arial", 10),
                    new SolidBrush(System.Drawing.Color.Red),
                    new PointF(pointParentRect.X - textSize.Width, pointParentRect.Y));
            }
        }
        private void DrawRightLines(Pen pen, Graphics g,
           List<RectangleF> ProximityRectangles, RectangleF rectangle,
           RectangleF ParentRectangle)
        {
            float rectRight = 0f;
            float rectLeft = 0f;
            float rectBottom = 0f;
            float rectTop = 0f;

            float x = 0f;

            float rectangleRight = (float)Math.Round(rectangle.Right, 1);
            float rectangleLeft = (float)Math.Round(rectangle.Left, 1);
            float rectangleBottom = (float)Math.Round(rectangle.Bottom, 1);
            float rectangleTop = (float)Math.Round(rectangle.Top, 1);

            RectangleF proxRectRight = new RectangleF();

            foreach (var rect in ProximityRectangles)
            {
                rectRight = (float)Math.Round(rect.Right, 1);
                rectLeft = (float)Math.Round(rect.Left, 1);
                rectBottom = (float)Math.Round(rect.Bottom, 1);
                rectTop = (float)Math.Round(rect.Top, 1);

                if (rect != ParentRectangle && rect != rectangle)
                {
                    if ((rectTop <= rectangleBottom && rectBottom >= rectangleTop) ||
                        (rectTop >= rectangleTop && rectBottom <= rectangleBottom))
                    {
                        if (rectLeft >= rectangleRight)
                        {
                            if (rectLeft < x || x == 0)
                            {
                                x = rectLeft;
                                proxRectRight = rect;
                            }
                        }
                    }
                }
            }
            if (!proxRectRight.IsEmpty)
            {
                float overlapTop = Math.Max(proxRectRight.Top, rectangle.Top);
                float overlapBottom = Math.Min(proxRectRight.Bottom, rectangle.Bottom);
                float midpointY = overlapTop + (overlapBottom - overlapTop) / 2;

                float startX = proxRectRight.Left;
                float endX = rectangle.Right;

                System.Drawing.Point startPoint = new System.Drawing.Point((int)startX, (int)midpointY);
                System.Drawing.Point endPoint = new System.Drawing.Point((int)endX, (int)midpointY);

                g.DrawLine(pen, startPoint, endPoint);

                float pxCM = ConvertTwoRectanglesDistancetoCMHorizontally(Ambar.AmbarEni, Ambar.AmbarBoyu,
                    Ambar.Rectangle, rectangle, proxRectRight, true);

                string widthCm = $"{(int)(pxCM)} cm";

                ProxRectRight = proxRectRight;

                if (Manuel_Move)
                {
                    AdjustTextboxesText(null, null, null, $"{(int)(pxCM)}", null, null);
                }
                else
                {
                    HideTextboxes();
                }

                SizeF textSize = g.MeasureString(widthCm, new System.Drawing.Font("Arial", 10));

                if (midpointY > ParentRectangle.Height - textSize.Height - 20)
                {
                    g.DrawString(widthCm, new System.Drawing.Font("Arial", 10),
                    new SolidBrush(System.Drawing.Color.Red),
                    new PointF(startX + 2,
                    midpointY - textSize.Height));
                }
                else
                {
                    g.DrawString(widthCm, new System.Drawing.Font("Arial", 10),
                    new SolidBrush(System.Drawing.Color.Red),
                    new PointF(startX + 2,
                    midpointY - textSize.Height));
                }
            }
            else
            {
                PointF pointRectangle = new PointF(rectangle.Right, rectangle.Bottom);
                PointF pointParentRect = new PointF((ParentRectangle.Right), rectangle.Bottom);

                float pxCM = ConvertTwoRectanglesDistancetoCMHorizontally
                    (Ambar.AmbarEni, Ambar.AmbarBoyu, Ambar.Rectangle,
                    rectangle, ParentRectangle, false);

                string widthCm = $"{(int)(pxCM)} cm";
                ProxRectRight = RectangleF.Empty;

                if (Manuel_Move)
                {
                    AdjustTextboxesText(null, null, null, $"{(int)(pxCM)}", null, null);
                }
                else
                {
                    HideTextboxes();
                }

                SizeF textSize = g.MeasureString(widthCm, new System.Drawing.Font("Arial", 10));

                g.DrawLine(pen, pointRectangle, pointParentRect);

                if (pointRectangle.Y > ParentRectangle.Height - textSize.Height - 20)
                {
                    //g.DrawString(widthCm, new System.Drawing.Font("Arial", 8),
                    //new SolidBrush(System.Drawing.Color.Red),
                    //new PointF(pointRectangle.X + 2,
                    //pointRectangle.Y - textSize.Height));

                    g.DrawString(widthCm, new System.Drawing.Font("Arial", 10),
                    new SolidBrush(System.Drawing.Color.Red),
                    new PointF(pointParentRect.X + 2,
                    pointParentRect.Y - textSize.Height));
                }
                else
                {
                    //g.DrawString(widthCm, new System.Drawing.Font("Arial", 8),
                    //new SolidBrush(System.Drawing.Color.Red),
                    //new PointF(pointRectangle.X + 2,
                    //pointRectangle.Y - textSize.Height));

                    g.DrawString(widthCm, new System.Drawing.Font("Arial", 10),
                    new SolidBrush(System.Drawing.Color.Red),
                    new PointF(pointParentRect.X + 2,
                    pointParentRect.Y - textSize.Height));
                }
            }
        }
        private void DrawTopLines(Pen pen, Graphics g,
            List<RectangleF> ProximityRectangles, RectangleF rectangle,
            RectangleF ParentRectangle)
        {
            float rectRight = 0f;
            float rectBottom = 0f;
            float rectLeft = 0f;
            float rectTop = 0f;

            float rectangleRight = (float)Math.Round(rectangle.Right, 1);
            float rectangleLeft = (float)Math.Round(rectangle.Left, 1);
            float rectangleBottom = (float)Math.Round(rectangle.Bottom, 1);
            float rectangleTop = (float)Math.Round(rectangle.Top, 1);

            RectangleF proxRectTop = new RectangleF();

            float x = 0f;

            foreach (var rect in ProximityRectangles)
            {
                rectRight = (float)Math.Round(rect.Right, 1);
                rectLeft = (float)Math.Round(rect.Left, 1);
                rectBottom = (float)Math.Round(rect.Bottom, 1);
                rectTop = (float)Math.Round(rect.Top, 1);

                if (rect != ParentRectangle && rect != rectangle)
                {
                    if ((rectLeft <= rectangleRight && rectRight >= rectangleLeft) ||
                        (rectRight >= rectangleLeft && rectLeft <= rectangleRight))
                    {
                        if (rectBottom <= rectangleTop)
                        {
                            if (rectBottom > x || x == 0)
                            {
                                x = rectBottom;
                                proxRectTop = rect;
                            }
                        }
                    }
                }
            }

            if (!proxRectTop.IsEmpty)
            {
                float overlapLeft = Math.Max(proxRectTop.Left, rectangle.Left);
                float overlapRight = Math.Min(proxRectTop.Right, rectangle.Right);
                float midpointX = overlapLeft + (overlapRight - overlapLeft) / 2;

                float startY = proxRectTop.Bottom;
                float endY = rectangle.Top;

                System.Drawing.Point startPoint = new System.Drawing.Point((int)midpointX, (int)startY);
                System.Drawing.Point endPoint = new System.Drawing.Point((int)midpointX, (int)endY);

                g.DrawLine(pen, startPoint, endPoint);

                float pxCM = ConvertTwoRectanglesDistancetoCMVertically
                    (Ambar.AmbarEni, Ambar.AmbarBoyu, Ambar.Rectangle, rectangle,
                    proxRectTop, true);

                string widthCm = $"{(int)(pxCM)} cm";

                ProxRectTop = proxRectTop;

                if (Manuel_Move)
                {
                    AdjustTextboxesText(null, null, null, null, $"{(int)(pxCM)}", null);
                }
                else
                {
                    HideTextboxes();
                }

                SizeF textSize = g.MeasureString(widthCm,
                    new System.Drawing.Font("Arial", 10));

                g.DrawString(widthCm, new System.Drawing.Font("Arial", 10),
                   new SolidBrush(System.Drawing.Color.Red),
                   new PointF(midpointX + 2, startY - textSize.Height));
            }
            else
            {
                PointF pointRectangle = new PointF(rectangle.X, rectangle.Y);
                PointF pointParentRect = new PointF((rectangle.X),
                    ParentRectangle.Top);

                float pxCM = ConvertTwoRectanglesDistancetoCMVertically
                    (Ambar.AmbarEni, Ambar.AmbarBoyu, Ambar.Rectangle, rectangle,
                    ParentRectangle, false);

                string widthCm = $"{(int)(pxCM)} cm";

                ProxRectTop = RectangleF.Empty;

                if (Manuel_Move)
                {
                    AdjustTextboxesText(null, null, null, null, $"{(int)(pxCM)}", null);
                }
                else
                {
                    HideTextboxes();
                }

                SizeF textSize = g.MeasureString(widthCm, new System.Drawing.Font("Arial", 10));

                g.DrawLine(pen, pointRectangle, pointParentRect);

                if (pointRectangle.Y < textSize.Height)
                {
                    // g.DrawString(widthCm, new System.Drawing.Font("Arial", 8),
                    //new SolidBrush(System.Drawing.Color.Red),
                    //new PointF(pointRectangle.X + 2, pointRectangle.Y));

                    g.DrawString(widthCm, new System.Drawing.Font("Arial", 10),
                   new SolidBrush(System.Drawing.Color.Red),
                   new PointF(pointParentRect.X + 2, pointParentRect.Y));
                }
                else
                {
                    // g.DrawString(widthCm, new System.Drawing.Font("Arial", 8),
                    //new SolidBrush(System.Drawing.Color.Red),
                    //new PointF(pointRectangle.X + 2, pointRectangle.Y - textSize.Height));

                    g.DrawString(widthCm, new System.Drawing.Font("Arial", 10),
                   new SolidBrush(System.Drawing.Color.Red),
                   new PointF(pointParentRect.X + 2, pointParentRect.Y - textSize.Height));
                }
            }
        }
        private void DrawBottomLines(Pen pen, Graphics g,
            List<RectangleF> ProximityRectangles, RectangleF rectangle,
            RectangleF ParentRectangle)
        {
            float rectRight = 0f;
            float rectBottom = 0f;
            float rectLeft = 0f;
            float rectTop = 0f;

            float rectangleRight = (float)Math.Round(rectangle.Right, 1);
            float rectangleLeft = (float)Math.Round(rectangle.Left, 1);
            float rectangleBottom = (float)Math.Round(rectangle.Bottom, 1);
            float rectangleTop = (float)Math.Round(rectangle.Top, 1);

            RectangleF proxRectBot = new RectangleF();

            float x = 0f;

            foreach (var rect in ProximityRectangles)
            {
                if (rect != ParentRectangle && rect != rectangle)
                {
                    rectRight = (float)Math.Round(rect.Right, 1);
                    rectLeft = (float)Math.Round(rect.Left, 1);
                    rectBottom = (float)Math.Round(rect.Bottom, 1);
                    rectTop = (float)Math.Round(rect.Top, 1);

                    if ((rectLeft <= rectangleRight && rectRight >= rectangleLeft) ||
                        (rectRight >= rectangleLeft && rectLeft <= rectangleRight))
                    {
                        if (rectTop >= rectangleBottom)
                        {
                            if (rectTop < x || x == 0)
                            {
                                x = rectTop;
                                proxRectBot = rect;
                            }
                        }
                    }
                }
            }


            if (!proxRectBot.IsEmpty)
            {
                float overlapLeft = Math.Max(proxRectBot.Left, rectangle.Left);
                float overlapRight = Math.Min(proxRectBot.Right, rectangle.Right);
                float midpointX = overlapLeft + (overlapRight - overlapLeft) / 2;

                float startY = proxRectBot.Top;
                float endY = rectangle.Bottom;

                System.Drawing.Point startPoint = new System.Drawing.Point((int)midpointX, (int)startY);
                System.Drawing.Point endPoint = new System.Drawing.Point((int)midpointX, (int)endY);

                g.DrawLine(pen, startPoint, endPoint);

                float pxCM = ConvertTwoRectanglesDistancetoCMVertically
                    (Ambar.AmbarEni, Ambar.AmbarBoyu, Ambar.Rectangle, proxRectBot,
                    rectangle, true);

                string widthCm = $"{(int)(pxCM)} cm";

                ProxRectBottom = proxRectBot;

                if (Manuel_Move)
                {
                    AdjustTextboxesText(null, null, null, null, null, $"{(int)(pxCM)}");
                }
                else
                {
                    HideTextboxes();
                }

                SizeF textSize = g.MeasureString(widthCm,
                    new System.Drawing.Font("Arial", 10));

                g.DrawString(widthCm, new System.Drawing.Font("Arial", 10),
                   new SolidBrush(System.Drawing.Color.Red),
                   new PointF(midpointX + 2, startY));
            }
            else
            {
                System.Drawing.Point pointRectangle = new System.Drawing.Point((int)rectangle.Right, (int)rectangle.Bottom);
                System.Drawing.Point pointParentRect = new System.Drawing.Point((int)(rectangle.Right),
                    (int)ParentRectangle.Bottom);

                float pxCM = ConvertTwoRectanglesDistancetoCMVertically
                    (Ambar.AmbarEni, Ambar.AmbarBoyu, Ambar.Rectangle, ParentRectangle,
                    rectangle, false);

                string widthCm = $"{(int)(pxCM)} cm";

                ProxRectBottom = RectangleF.Empty;

                if (Manuel_Move)
                {
                    AdjustTextboxesText(null, null, null, null, null, $"{(int)(pxCM)}");
                }
                else
                {
                    HideTextboxes();
                }

                SizeF textSize = g.MeasureString(widthCm,
                    new System.Drawing.Font("Arial", 10));

                g.DrawLine(pen, pointRectangle, pointParentRect);

                if (pointRectangle.Y > ParentRectangle.Height - textSize.Height)
                {
                    // g.DrawString(widthCm, new System.Drawing.Font("Arial", 8),
                    //new SolidBrush(System.Drawing.Color.Red),
                    //new PointF(pointRectangle.X - textSize.Width,
                    //pointRectangle.Y - textSize.Height));

                    g.DrawString(widthCm, new System.Drawing.Font("Arial", 10),
                    new SolidBrush(System.Drawing.Color.Red),
                    new PointF(pointParentRect.X - textSize.Width,
                    pointParentRect.Y - textSize.Height));
                }
                else
                {
                    // g.DrawString(widthCm, new System.Drawing.Font("Arial", 8),
                    //new SolidBrush(System.Drawing.Color.Red),
                    //new PointF(pointRectangle.X - textSize.Width,
                    //pointRectangle.Y));

                    g.DrawString(widthCm, new System.Drawing.Font("Arial", 10),
                   new SolidBrush(System.Drawing.Color.Red),
                   new PointF(pointParentRect.X - textSize.Width,
                   pointParentRect.Y));
                }
            }
        }
        //public void AdjustTextBoxLocations(RectangleF rectangle, RectangleF parentRectangle,
        //    RectangleF RefRectangle, Point startPoint, Point endPoint, string side)
        //{
        //    if (!parentRectangle.IsEmpty && startPoint.IsEmpty && endPoint.IsEmpty)
        //    {
        //        Point topEdge = GVisual.GetMiddleOfTopEdge(rectangle);
        //        Point leftEdge = GVisual.GetMiddleOfLeftEdge(rectangle);
        //        Point leftPadding = GVisual.GetMiddleOfLeftEdge(parentRectangle);
        //        Point rightPadding = GVisual.GetMiddleOfRightEdge(parentRectangle);
        //        Point topPadding = GVisual.GetMiddleOfTopEdge(parentRectangle);
        //        Point bottomPadding = GVisual.GetMiddleOfBottomEdge(parentRectangle);
        //        Point rectangleRight = GVisual.GetMiddleOfRightEdge(rectangle);
        //        Point rectangleLocation = new Point((int)rectangle.Location.X, (int)rectangle.Location.Y);
        //        Point rectangleBottom = GVisual.GetBottomRightCorner(rectangle);
        //        Point rectangleCenter = GVisual.GetCenter(rectangle);

        //        //Point txt_width_loc = new Point(topEdge.X - txt_Width.Width / 2 + 20,
        //        //    topEdge.Y);
        //        //Point txt_height_loc = new Point(leftEdge.X,
        //        //    leftEdge.Y - txt_Height.Height / 2);

        //        Point txt_width_loc = new Point(rectangleCenter.X - txt_Width.Width / 2,
        //            rectangleCenter.Y - txt_Width.Height - 5);
        //        Point txt_height_loc =
        //            GVisual.Point_Control_to_BottomSide_ofControl
        //            (txt_Height, txt_Width, 10);

        //        txt_Width.Location = txt_width_loc;
        //        txt_Height.Location = txt_height_loc;

        //        if (side == "Left")
        //        {
        //            txt_Left_Padding.Location =
        //            GVisual.Center_Control_To_Horizontal_Padding(txt_Left_Padding, leftPadding,
        //            rectangleLocation, rectangleLocation.Y + 3);
        //        }
        //        else if (side == "Right")
        //        {
        //            txt_Right_Padding.Location =
        //            GVisual.Center_Control_To_Horizontal_Padding(txt_Right_Padding, rectangleRight,
        //            rightPadding, (int)(rectangle.Location.Y +
        //            rectangle.Height - txt_Right_Padding.Height - 3));
        //        }
        //        else if (side == "Top")
        //        {
        //            txt_Top_Padding.Location = GVisual.Center_Control_To_Vertical_Padding(txt_Top_Padding,
        //                topPadding, rectangleLocation, rectangleLocation.X + 3);
        //        }
        //        else if (side == "Bottom")
        //        {
        //            txt_Bottom_Padding.Location = GVisual.Center_Control_To_Vertical_Padding(txt_Bottom_Padding,
        //            rectangleBottom, bottomPadding, rectangleBottom.X - txt_Bottom_Padding.Width - 3);
        //        }
        //    }

        //    if (!RefRectangle.IsEmpty && !startPoint.IsEmpty && !endPoint.IsEmpty)
        //    {
        //        Point topEdge = GVisual.GetMiddleOfTopEdge(rectangle);
        //        Point leftEdge = GVisual.GetMiddleOfLeftEdge(rectangle);
        //        Point leftPadding = GVisual.GetMiddleOfRightEdge(RefRectangle);
        //        Point rightPadding = GVisual.GetMiddleOfLeftEdge(RefRectangle);
        //        Point topPadding = GVisual.GetMiddleOfBottomEdge(RefRectangle);
        //        Point bottomPadding = GVisual.GetMiddleOfTopEdge(RefRectangle);
        //        Point rectangleRight = GVisual.GetMiddleOfRightEdge(rectangle);
        //        Point rectangleLocation = new Point((int)rectangle.Location.X, (int)rectangle.Location.Y);
        //        Point rectangleBottom = GVisual.GetBottomRightCorner(rectangle);
        //        Point rectangleCenter = GVisual.GetCenter(rectangle);

        //        //Point txt_width_loc = new Point(topEdge.X - txt_Width.Width / 2 + 20,
        //        //    topEdge.Y);
        //        //Point txt_height_loc = new Point(leftEdge.X,
        //        //    leftEdge.Y - txt_Height.Height / 2);

        //        Point txt_width_loc = new Point(rectangleCenter.X - txt_Width.Width / 2,
        //           rectangleCenter.Y - txt_Width.Height - 5);
        //        Point txt_height_loc =
        //            GVisual.Point_Control_to_BottomSide_ofControl
        //            (txt_Height, txt_Width, 10);

        //        txt_Width.Location = txt_width_loc;
        //        txt_Height.Location = txt_height_loc;

        //        if (side == "Left")
        //        {
        //            txt_Left_Padding.Location =
        //            GVisual.Center_Control_To_Horizontal_Padding(txt_Left_Padding, startPoint,
        //            endPoint, endPoint.Y);
        //        }
        //        else if (side == "Right")
        //        {
        //            txt_Right_Padding.Location =
        //                GVisual.Center_Control_To_Horizontal_Padding(txt_Right_Padding, startPoint,
        //                endPoint, endPoint.Y);
        //        }
        //        else if (side == "Top")
        //        {
        //            txt_Top_Padding.Location = GVisual.Center_Control_To_Vertical_Padding(txt_Top_Padding,
        //                startPoint, endPoint, endPoint.X + 3);
        //        }
        //        else if (side == "Bottom")
        //        {
        //            txt_Bottom_Padding.Location = GVisual.Center_Control_To_Vertical_Padding(txt_Bottom_Padding,
        //                startPoint, endPoint, endPoint.X);
        //        }
        //        else
        //        {
        //            txt_Left_Padding.Location =
        //            GVisual.Center_Control_To_Horizontal_Padding(txt_Left_Padding, leftPadding,
        //            rectangleLocation, rectangleLocation.Y + 3);

        //            txt_Right_Padding.Location =
        //                GVisual.Center_Control_To_Horizontal_Padding(txt_Right_Padding, rectangleRight,
        //                rightPadding, (int)(rectangle.Location.Y +
        //                rectangle.Height - txt_Right_Padding.Height - 3));

        //            txt_Top_Padding.Location = GVisual.Center_Control_To_Vertical_Padding(txt_Top_Padding,
        //                topPadding, rectangleLocation, rectangleLocation.X + 3);

        //            txt_Bottom_Padding.Location = GVisual.Center_Control_To_Vertical_Padding(txt_Bottom_Padding,
        //                rectangleBottom, bottomPadding, rectangleBottom.X - txt_Bottom_Padding.Width - 3);
        //        }
        //    }

        //    if (!txt_Width.Visible)
        //    {
        //        ShowTextboxes(txt_Width.Location, txt_Height.Location, txt_Left_Padding.Location,
        //       txt_Right_Padding.Location, txt_Top_Padding.Location, txt_Bottom_Padding.Location);
        //    }
        //}



        public static float ConvertTwoRectanglesDistancetoCMVertically
            (float areaWidthMeters, float areaHeightMeters, RectangleF parentRectangle,
            RectangleF downRectangle, RectangleF upRectangle, bool inBetween)
        {
            float areaWidthCm = areaWidthMeters * 100;

            float widthCmPx = areaWidthCm / parentRectangle.Width;

            if (parentRectangle == upRectangle)
            {
                if (downRectangle.Top >= upRectangle.Top && !inBetween)
                {
                    float diffX = downRectangle.Top - upRectangle.Top;
                    float PX = diffX * widthCmPx;
                    double x = Math.Round(PX);
                    int y = (int)x;
                    return y;
                }
                else
                {
                    return 0f;
                }
            }
            else if (parentRectangle == downRectangle)
            {
                if (downRectangle.Bottom >= upRectangle.Bottom && !inBetween)
                {
                    float diffX = downRectangle.Bottom - upRectangle.Bottom;
                    float PX = diffX * widthCmPx;
                    double x = Math.Round(PX);
                    int y = (int)x;
                    return y;
                }
                else
                {
                    return 0f;
                }
            }
            else
            {
                if (downRectangle.Top >= upRectangle.Bottom && inBetween)
                {
                    float diffX = downRectangle.Top - upRectangle.Bottom;
                    float PX = diffX * widthCmPx;
                    double x = Math.Round(PX);
                    int y = (int)x;
                    return y;
                }
                else if (downRectangle.Bottom <= upRectangle.Bottom && inBetween)
                {
                    float diffX = upRectangle.Bottom - downRectangle.Bottom;
                    float PX = diffX * widthCmPx;
                    double x = Math.Round(PX);
                    int y = (int)x;
                    return y;
                }
                else
                {
                    return 0f;
                }
            }
        }

        public (float, float) ConvertPixelToCmGivenRectangle(RectangleF Rect,
            float rectAreaWidthMeters, float rectAreaHeightMeters)
        {
            float areaWidthCm = rectAreaWidthMeters * 100;
            float areaHeightCm = rectAreaHeightMeters * 100;

            float widthCmPx = Rect.Width / areaWidthCm;
            float heightCmPx = Rect.Height / areaHeightCm;

            return (widthCmPx, heightCmPx);
        }



        public float ConvertTwoRectanglesDistancetoCMHorizontally(float areaWidthMeters,
            float areaHeightMeters, RectangleF parentRectangle, RectangleF leftRectangle,
            RectangleF rightRectangle, bool inBetween)
        {
            float areaWidthCm = areaWidthMeters * 100;

            float widthCmPx = areaWidthCm / parentRectangle.Width;

            if (parentRectangle == leftRectangle)
            {
                if (leftRectangle.Left <= rightRectangle.X && !inBetween)
                {
                    float diffX = rightRectangle.Left - leftRectangle.Left;
                    float PX = diffX * widthCmPx;
                    double x = Math.Round(PX);
                    int y = (int)x;
                    return y;
                }
                else
                {
                    return 0f;
                }
            }
            else if (parentRectangle == rightRectangle)
            {
                if (leftRectangle.Right <= rightRectangle.Right && !inBetween)
                {
                    float diffX = rightRectangle.Right - leftRectangle.Right;
                    float PX = diffX * widthCmPx;
                    double x = Math.Round(PX);
                    int y = (int)x;
                    return y;
                }
                else
                {
                    return 0f;
                }
            }
            else
            {
                if (leftRectangle.Right <= rightRectangle.X && inBetween)
                {
                    float diffX = rightRectangle.X - leftRectangle.Right;
                    float PX = diffX * widthCmPx;
                    double x = Math.Round(PX);
                    int y = (int)x;
                    return y;
                }
                else if (leftRectangle.X <= rightRectangle.X && inBetween)
                {
                    float diffX = rightRectangle.Left - leftRectangle.Left;
                    float PX = diffX * widthCmPx;
                    double x = Math.Round(PX);
                    int y = (int)x;
                    return y;
                }
                else
                {
                    return 0f;
                }
            }
        }



        private void MoveLeft()
        {
            if (Ambar != null)
            {
                Ambar.Rectangle = new RectangleF(Ambar.Rectangle.X - drawingPanelMoveConstant, Ambar.Rectangle.Y, Ambar.Rectangle.Width, Ambar.Rectangle.Height);
                foreach (var depo in Ambar.depolar)
                {
                    depo.Rectangle = new RectangleF(depo.Rectangle.X - drawingPanelMoveConstant, depo.Rectangle.Y, depo.Rectangle.Width, depo.Rectangle.Height);
                    depo.OriginalRectangle = depo.Rectangle;
                    depo.LocationofRect = new System.Drawing.Point((int)depo.Rectangle.X, (int)depo.Rectangle.Y);
                    selectedDepoRectangle = depo.Rectangle;
                    UnchangedselectedDepoRectangle = depo.Rectangle;
                    foreach (var cell in depo.gridmaps)
                    {
                        cell.Rectangle = new RectangleF(cell.Rectangle.X - drawingPanelMoveConstant, cell.Rectangle.Y, cell.Rectangle.Width, cell.Rectangle.Height);
                        cell.LocationofRect = new System.Drawing.Point((int)cell.Rectangle.X, (int)cell.Rectangle.Y);

                    }
                }
                foreach (var conveyor in Ambar.conveyors)
                {
                    conveyor.Rectangle = new RectangleF(conveyor.Rectangle.X - drawingPanelMoveConstant, conveyor.Rectangle.Y, conveyor.Rectangle.Width, conveyor.Rectangle.Height);
                    conveyor.LocationofRect = new System.Drawing.Point((int)conveyor.Rectangle.X, (int)conveyor.Rectangle.Y);
                    selectedConveyorRectangle = conveyor.Rectangle;
                    UnchangedselectedConveyorRectangle = conveyor.Rectangle;

                    foreach (var reff in conveyor.ConveyorReferencePoints)
                    {
                        reff.Rectangle = new RectangleF(reff.Rectangle.X - drawingPanelMoveConstant,
                            reff.Rectangle.Y, reff.Rectangle.Width, reff.Rectangle.Height);
                        reff.LocationofRect = new System.Drawing.Point((int)reff.Rectangle.X, (int)reff.Rectangle.Y);
                    }
                }
                drawingPanel.Invalidate();
            }
        }
        private void MoveRight()
        {
            if (Ambar != null)
            {
                Ambar.Rectangle = new RectangleF(Ambar.Rectangle.X + drawingPanelMoveConstant, Ambar.Rectangle.Y, Ambar.Rectangle.Width, Ambar.Rectangle.Height);
                foreach (var depo in Ambar.depolar)
                {
                    depo.Rectangle = new RectangleF(depo.Rectangle.X + drawingPanelMoveConstant, depo.Rectangle.Y, depo.Rectangle.Width, depo.Rectangle.Height);
                    depo.OriginalRectangle = depo.Rectangle;
                    depo.LocationofRect = new System.Drawing.Point((int)depo.Rectangle.X, (int)depo.Rectangle.Y);
                    selectedDepoRectangle = depo.Rectangle;
                    UnchangedselectedDepoRectangle = depo.Rectangle;
                    foreach (var cell in depo.gridmaps)
                    {
                        cell.Rectangle = new RectangleF(cell.Rectangle.X + drawingPanelMoveConstant, cell.Rectangle.Y, cell.Rectangle.Width, cell.Rectangle.Height);
                        cell.LocationofRect = new System.Drawing.Point((int)cell.Rectangle.X, (int)cell.Rectangle.Y);
                    }
                }
                foreach (var conveyor in Ambar.conveyors)
                {
                    conveyor.Rectangle = new RectangleF(conveyor.Rectangle.X + drawingPanelMoveConstant, conveyor.Rectangle.Y, conveyor.Rectangle.Width, conveyor.Rectangle.Height);
                    conveyor.LocationofRect = new System.Drawing.Point((int)conveyor.Rectangle.X, (int)conveyor.Rectangle.Y);
                    selectedConveyorRectangle = conveyor.Rectangle;
                    UnchangedselectedConveyorRectangle = conveyor.Rectangle;
                    foreach (var reff in conveyor.ConveyorReferencePoints)
                    {
                        reff.Rectangle = new RectangleF(reff.Rectangle.X + drawingPanelMoveConstant,
                            reff.Rectangle.Y, reff.Rectangle.Width, reff.Rectangle.Height);
                        reff.LocationofRect = new System.Drawing.Point((int)reff.Rectangle.X, (int)reff.Rectangle.Y);
                    }
                }
                drawingPanel.Invalidate();
            }
        }



        private void ShowTextboxes(System.Drawing.Point txt_Width_Location, System.Drawing.Point txt_Height_Location,
            System.Drawing.Point txt_Left_Padding_Location, System.Drawing.Point txt_Right_Padding_Location,
            System.Drawing.Point txt_Top_Padding_Location, System.Drawing.Point txt_Bottom_Padding_Location)
        {
            GVisual.ShowControl(txt_Width, PaddingPanel, txt_Width_Location);
            GVisual.ShowControl(txt_Height, PaddingPanel, txt_Height_Location);
            GVisual.ShowControl(txt_Left_Padding, PaddingPanel, txt_Left_Padding_Location);
            GVisual.ShowControl(txt_Right_Padding, PaddingPanel, txt_Right_Padding_Location);
            GVisual.ShowControl(txt_Top_Padding, PaddingPanel, txt_Top_Padding_Location);
            GVisual.ShowControl(txt_Bottom_Padding, PaddingPanel, txt_Bottom_Padding_Location);
        }
        private void HideTextboxes()
        {
            GVisual.HideControl(txt_Width, PaddingPanel);
            GVisual.HideControl(txt_Height, PaddingPanel);
            GVisual.HideControl(txt_Left_Padding, PaddingPanel);
            GVisual.HideControl(txt_Right_Padding, PaddingPanel);
            GVisual.HideControl(txt_Top_Padding, PaddingPanel);
            GVisual.HideControl(txt_Bottom_Padding, PaddingPanel);
        }
        
        public void AdjustTextboxesText(string? txt_width, string? txt_height, string? txt_left_padding,
            string? txt_right_padding, string? txt_top_padding, string? txt_bottom_padding)
        {
            if (isMoving == false)
            {
                if (txt_left_padding != null)
                {
                    txt_Left_Padding.Text = txt_left_padding;
                }
                if (txt_right_padding != null)
                {
                    txt_Right_Padding.Text = txt_right_padding;
                }
                if (txt_bottom_padding != null)
                {
                    txt_Bottom_Padding.Text = txt_bottom_padding;
                }
                if (txt_top_padding != null)
                {
                    txt_Top_Padding.Text = txt_top_padding;
                }
                if (txt_width != null)
                {
                    txt_Width.Text = txt_width;
                }
                if (txt_height != null)
                {
                    txt_Height.Text = txt_height;
                }
            }
        }



        private void HideEverything()
        {
            GVisual.HideControl(Alan_Olusturma_Paneli, this);
            GVisual.HideControl(Conveyor_Olusturma_Paneli, this);
            GVisual.HideControl(Depo_Olusturma_Paneli, this);
            GVisual.HideControl(Izgara_Mal_Paneli, this);
            GVisual.HideControl(Izgara_Olusturma_Paneli, this);
            GVisual.HideControl(PaddingPanel, this);
            GVisual.HideControl(Depo_Bilgi_Panel, this);
            GVisual.HideControl(txt_Width, drawingPanel);
            GVisual.HideControl(txt_Height, drawingPanel);
            GVisual.HideControl(txt_Left_Padding, drawingPanel);
            GVisual.HideControl(txt_Right_Padding, drawingPanel);
            GVisual.HideControl(txt_Top_Padding, drawingPanel);
            GVisual.HideControl(txt_Bottom_Padding, drawingPanel);
            GVisual.HideControl(btn_Manuel_Move, drawingPanel);
            GVisual.HideControl(btn_Yer_Onayla, drawingPanel);
            GVisual.HideControl(Placement_LeftRight_Panel, drawingPanel);
            GVisual.HideControl(Placement_StartLocation_Panel, drawingPanel);
            GVisual.HideControl(Placement_UpDown_Panel, drawingPanel);
            GVisual.HideControl(Asama1_Yukseklik_Panel, drawingPanel);
            GVisual.HideControl(Asama2_Yukseklik_Panel, drawingPanel);
            GVisual.HideControl(Conveyor_Reference_Sayisi_Paneli, drawingPanel);
            GVisual.HideControl(Conveyor_Reference_FixedorManuel_Panel, drawingPanel);
            GVisual.HideControl(Conveyor_Reference_Fixed_Panel, drawingPanel);
        }
        private void DrawingPanelEnlarge(System.Windows.Forms.Control hideControl,
            System.Windows.Forms.Control parentControl)
        {
            GVisual.HideControl(hideControl, parentControl);
            GVisual.ChangeSize_of_Control(drawingPanel, drawingPanelLargeSize);
            drawingPanel.Location = drawingPanelLargeLocation;
            MoveRight();
        }
        private void DrawingPanelShrink(System.Windows.Forms.Control showControl,
            System.Windows.Forms.Control parentControl, System.Drawing.Point childControlLocation)
        {
            GVisual.ShowControl(showControl, parentControl, childControlLocation);
            GVisual.ChangeSize_of_Control(drawingPanel, drawingPanelSmallSize);
            drawingPanel.Location = drawingPanelSmallLocation;
            MoveLeft();
        }



        private void btn_Manuel_Move_Click(object sender, EventArgs e)
        {
            if (Manuel_Move)
            {
                Manuel_Move = false;
            }
            else
            {
                ShowTextboxes(txt_Width.Location, txt_Height.Location,
                    txt_Left_Padding.Location, txt_Right_Padding.Location,
                    txt_Top_Padding.Location, txt_Bottom_Padding.Location);
                ShowMoveCheckButton();
                Manuel_Move = true;
                if (selectedDepo != null)
                {
                    ManuelDepoRectangle = selectedDepo.Rectangle;
                    UnchangedDepoAlaniBoyu = selectedDepo.DepoAlaniBoyu;
                    UnchangedDepoAlaniEni = selectedDepo.DepoAlaniEni;
                    //txt_width.text = $"{unchangeddepoalanieni}";
                    //txt_height.text = $"{unchangeddepoalaniboyu}";
                }
                if (selectedConveyor != null)
                {
                    ManuelConveyorRectangle = selectedConveyor.Rectangle;
                    UnchangedConveyorEni = selectedConveyor.ConveyorEni;
                    UnchangedConveyorBoyu = selectedConveyor.ConveyorBoyu;
                    //txt_Width.Text = $"{UnchangedConveyorEni}";
                    //txt_Height.Text = $"{UnchangedConveyorBoyu}";
                }
            }
        }
        private void ShowMoveCheckButton()
        {
            if (Ambar != null)
            {
                GVisual.ShowControl(btn_Yer_Onayla, PaddingPanel, new System.Drawing.Point(10, 10));
                GVisual.Move_LeftSide_of_AnotherControl(btn_Yer_Onayla,
                    btn_Padding_Vazgeç, 10);
            }
        }



        private void CenterControltoLeftSideofRectangleVertically(RectangleF rectangle,
            System.Windows.Forms.Control control,
            System.Windows.Forms.Control ParentControl)
        {
            System.Drawing.Point selectedPoint = GVisual.GetMiddleOfLeftEdge(rectangle);
            System.Drawing.Point point = new System.Drawing.Point(selectedPoint.X - control.Width,
                selectedPoint.Y - control.Height / 2);

            GVisual.ShowControl(control, ParentControl, point);

            if (control.Top < ParentControl.ClientRectangle.Top)
            {
                control.Location =
                    new System.Drawing.Point(control.Location.X,
                    ParentControl.ClientRectangle.Top);
            }
            else if (control.Bottom > ParentControl.ClientRectangle.Bottom)
            {
                control.Location =
                    new System.Drawing.Point(control.Location.X,
                    ParentControl.ClientRectangle.Bottom - control.Height);
            }
        }



        private void nesneKoyulmaSıralamasınıAyarlaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedDepo != null)
            {
                if (selectedDepo.gridmaps.Count > 0)
                {
                    TransparentPen.Color = System.Drawing.Color.FromArgb(0, System.Drawing.Color.Black);
                    MovingParameter = true;
                    lbl_Placement_Yukseklik_Depo_Alani_Yuksekligi_Value.Text = $"{selectedDepo.DepoAlaniYuksekligi} cm";
                    lbl_Placement_Yukseklik_Nesne_Yuksekligi_Value.Text = $"{selectedDepo.nesneYuksekligi} cm";
                    lbl_Placement_Yukseklik_upDown_Nesne_Yuksekligi_Value.Text = "0 cm";

                    CenterControltoLeftSideofRectangleVertically(selectedDepo.Rectangle,
                        Asama1_Yukseklik_Panel, drawingPanel);
                }
                else
                {
                    CustomNotifyIcon notify = new CustomNotifyIcon();
                    notify.showAlert("Önce ızgara haritası oluşturmanız gerekiyor.", CustomNotifyIcon.enmType.Warning);
                }
            }
        }
        private void btn_Placement_StartLocation_NextPage_Click(object sender, EventArgs e)
        {
            if (selectedDepo != null)
            {
                if (radio_Start_From_Middle.Checked)
                {
                    GVisual.HideControl(Placement_StartLocation_Panel, drawingPanel);
                    CenterControltoLeftSideofRectangleVertically(selectedDepo.Rectangle,
                        Placement_UpDown_Panel, drawingPanel);
                }
                else if (radio_Start_From_Top.Checked)
                {
                    GVisual.HideControl(Placement_StartLocation_Panel, drawingPanel);
                    CenterControltoLeftSideofRectangleVertically(selectedDepo.Rectangle,
                        Placement_LeftRight_Panel, drawingPanel);

                    currentRow = 0;
                    rectangles.Clear();
                    timer.Interval = 300;
                    timer.Start();
                    Parameter = "Aşağı Doğru";
                }
                else if (radio_Start_From_Bottom.Checked)
                {
                    GVisual.HideControl(Placement_StartLocation_Panel, drawingPanel);
                    CenterControltoLeftSideofRectangleVertically(selectedDepo.Rectangle,
                        Placement_LeftRight_Panel, drawingPanel);

                    currentRow = rowCount;
                    rectangles.Clear();
                    timer.Interval = 300;
                    timer.Start();
                    Parameter = "Yukarı Doğru";
                }
                else
                {
                    CustomNotifyIcon notify = new CustomNotifyIcon();
                    notify.showAlert("Bir seçim yapmak zorundasınız.", CustomNotifyIcon.enmType.Warning);
                }
            }
        }
        private void btn_Placement_UpDown_NextPage_Click(object sender, EventArgs e)
        {
            if (selectedDepo != null)
            {
                if (radio_To_Up.Checked || radio_To_Down.Checked)
                {
                    GVisual.HideControl(Placement_UpDown_Panel, drawingPanel);
                    CenterControltoLeftSideofRectangleVertically(selectedDepo.Rectangle,
                        Placement_LeftRight_Panel, drawingPanel);
                }
                else
                {
                    CustomNotifyIcon notify = new CustomNotifyIcon();
                    notify.showAlert("Bir seçim yapmak zorundasınız.", CustomNotifyIcon.enmType.Warning);
                }
            }
        }
        private void btn_Placement_LeftRight_OK_Click(object sender, EventArgs e)
        {
            if (selectedDepo != null)
            {
                if (radio_To_Left.Checked || radio_To_Right.Checked)
                {
                    int rowcount = selectedDepo.RowCount + 1;
                    int currentRow = 0;
                    if (rowcount % 2 == 0)
                    {
                        currentRow = rowcount / 2;
                    }
                    else if (rowcount % 2 != 0)
                    {
                        currentRow = (rowcount / 2) + 1;
                    }

                    timer.Stop();
                    if (radio_To_Down.Checked)
                    {
                        selectedDepo.itemDrop_UpDown = "Aşağı Doğru";
                        if (rowcount % 2 == 0)
                        {
                            selectedDepo.currentRow = currentRow + 1;
                        }
                        else
                        {
                            selectedDepo.currentRow = currentRow;
                        }
                    }
                    else
                    {
                        selectedDepo.itemDrop_UpDown = "Yukarı Doğru";
                        selectedDepo.currentRow = currentRow;
                    }

                    if (radio_To_Left.Checked)
                    {
                        selectedDepo.itemDrop_LeftRight = "Sola Doğru";
                        selectedDepo.currentColumn = selectedDepo.ColumnCount;
                    }
                    else
                    {
                        selectedDepo.itemDrop_LeftRight = "Sağa Doğru";
                        selectedDepo.currentColumn = 1;
                    }

                    if (radio_Start_From_Bottom.Checked)
                    {
                        selectedDepo.itemDrop_StartLocation = "Aşağıdan";
                        selectedDepo.currentRow = rowcount;
                    }
                    else if (radio_Start_From_Middle.Checked)
                    {
                        selectedDepo.itemDrop_StartLocation = "Ortadan";
                    }
                    else
                    {
                        selectedDepo.itemDrop_StartLocation = "Yukarıdan";
                        selectedDepo.currentRow = 1;
                    }
                    TransparentPen.Color = System.Drawing.Color.FromArgb(255, System.Drawing.Color.Black);
                    selectedDepo.asama1_Yuksekligi = toplam_nesne_yuksekligi;
                    selectedDepo.asama2_Yuksekligi = toplam_nesne_yuksekligi_asama2;
                    selectedDepo.asama1_ItemSayisi = (int)upDown_1Asama_NesneSayisi.Value;
                    selectedDepo.asama2_ToplamItemSayisi = (int)upDown_2Asama_NesneSayisi.Value + (int)upDown_1Asama_NesneSayisi.Value;
                    MovingParameter = false;
                    Parameter = string.Empty;
                    rectangles.Clear();
                    GVisual.HideControl(Placement_LeftRight_Panel, drawingPanel);
                    CustomNotifyIcon notify = new CustomNotifyIcon();
                    notify.showAlert("Yerleştirilme Parametreleri Kaydedildi.", CustomNotifyIcon.enmType.Success);
                    drawingPanel.Invalidate();
                    upDown_1Asama_NesneSayisi.Value = 0;
                    upDown_2Asama_NesneSayisi.Value = 0;
                    toplam_nesne_yuksekligi = 0;
                    toplam_nesne_yuksekligi_asama2 = 0;
                }
                else
                {
                    CustomNotifyIcon notify = new CustomNotifyIcon();
                    notify.showAlert("Bir seçim yapmak zorundasınız.", CustomNotifyIcon.enmType.Warning);
                }
            }
        }
        private void btn_Placement_LeftRight_Cancel_Click(object sender, EventArgs e)
        {
            TransparentPen.Color = System.Drawing.Color.FromArgb(255, System.Drawing.Color.Black);
            GVisual.HideControl(Placement_LeftRight_Panel, drawingPanel);
            Parameter = string.Empty;
            MovingParameter = false;
            upDown_1Asama_NesneSayisi.Value = 0;
            upDown_2Asama_NesneSayisi.Value = 0;
        }
        private void btn_Placement_Yukseklik_Asama2Ac_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();

            if (upDown_1Asama_NesneSayisi.Value == 0)
            {
                errorProvider.SetError(upDown_1Asama_NesneSayisi, "Bu alana 0 giremezsiniz.");
            }
            if (!errorProvider.HasErrors)
            {
                if (selectedDepo != null)
                {
                    lbl_Placement_Yukseklik_Toplam_Nesne_Yuksekligi_Value.Text = $"{toplam_nesne_yuksekligi} cm";
                    int kalan_depo_yuksekligi = selectedDepo.DepoAlaniYuksekligi - toplam_nesne_yuksekligi;

                    int konulabilecek_nesne_sayisi = kalan_depo_yuksekligi / selectedDepo.nesneYuksekligi;

                    lbl_Placement_Yukseklik_Kalan_Depo_Yuksekligi_Value.Text = $"{kalan_depo_yuksekligi} cm";
                    lbl_Placement_Yukseklik_Toplam_Nesne_Yuksekligi_Value.Text = $"{toplam_nesne_yuksekligi} cm";

                    if (konulabilecek_nesne_sayisi >= 1)
                    {
                        lbl_Placement_Yukseklik_Konulabilecek_NesneSayisi_Value.Text = $"{(int)konulabilecek_nesne_sayisi} Adet";
                    }

                    GVisual.HideControl(Asama1_Yukseklik_Panel, drawingPanel);
                    CenterControltoLeftSideofRectangleVertically(selectedDepo.Rectangle,
                            Asama2_Yukseklik_Panel, drawingPanel);

                    askOnce = true;
                }
            }
        }
        private void upDown_1Asama_NesneSayisi_ValueChanged(object sender, EventArgs e)
        {
            if (selectedDepo != null)
            {
                int depo_alani_yuksekligi = selectedDepo.DepoAlaniYuksekligi;
                nesne_sayisi = (int)upDown_1Asama_NesneSayisi.Value;
                toplam_nesne_yuksekligi = nesne_sayisi * selectedDepo.nesneYuksekligi;


                if (toplam_nesne_yuksekligi <= (depo_alani_yuksekligi / 2))
                {
                    lbl_Placement_Yukseklik_upDown_Nesne_Yuksekligi_Value.Text = $"{toplam_nesne_yuksekligi} cm";
                }
                else if (toplam_nesne_yuksekligi > depo_alani_yuksekligi / 2 && toplam_nesne_yuksekligi <= depo_alani_yuksekligi && askOnce)
                {
                    var result = MessageBox.Show("Nesne yüksekliğinin toplamı depo alanının yüksekliğinin yarısından fazla devam etmek istediğinize emin misiniz? ", "Devam etmek istiyor musunuz?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        lbl_Placement_Yukseklik_upDown_Nesne_Yuksekligi_Value.Text = $"{toplam_nesne_yuksekligi} cm";
                    }
                    else
                    {
                        upDown_1Asama_NesneSayisi.Value = nesne_sayisi - 1;
                    }
                    askOnce = false;
                }
                else if (toplam_nesne_yuksekligi > depo_alani_yuksekligi)
                {
                    upDown_1Asama_NesneSayisi.Value = nesne_sayisi - 1;
                    CustomNotifyIcon notify = new CustomNotifyIcon();
                    notify.showAlert("Toplam nesne yüksekliği deponun yüksekliğinden fazla olamaz.", CustomNotifyIcon.enmType.Error);
                }
                else
                {
                    lbl_Placement_Yukseklik_upDown_Nesne_Yuksekligi_Value.Text = $"{toplam_nesne_yuksekligi} cm";
                }
            }
        }
        private void btn_Placement_Yukseklik_Onayla_Click(object sender, EventArgs e)
        {
            if (selectedDepo != null)
            {
                int nesne_sayisi = (int)upDown_2Asama_NesneSayisi.Value;
                int asama2_Nesne_Yuksekligi = nesne_sayisi * selectedDepo.nesneYuksekligi;

                toplam_nesne_yuksekligi_asama2 = toplam_nesne_yuksekligi + asama2_Nesne_Yuksekligi;

                if (toplam_nesne_yuksekligi >= selectedDepo.DepoAlaniYuksekligi - selectedDepo.nesneYuksekligi
                    && toplam_nesne_yuksekligi <= selectedDepo.DepoAlaniYuksekligi)
                {
                    if (toplam_nesne_yuksekligi_asama2 <= selectedDepo.DepoAlaniYuksekligi - selectedDepo.nesneYuksekligi)
                    {
                        var result =
                            MessageBox.Show("Depoda yükseklik açısından yer var. " +
                            "Eğer bunu onaylarsanız, depo alanını dolduracak kadar nesne koyulmayacak. " +
                            "Devam etmek istiyor musunuz?", "Emin misiniz?",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                        if (result == DialogResult.Yes)
                        {
                            GVisual.HideControl(Asama2_Yukseklik_Panel, drawingPanel);
                            CenterControltoLeftSideofRectangleVertically(selectedDepo.Rectangle, Placement_StartLocation_Panel, drawingPanel);
                        }
                        else
                        {
                            MovingParameter = false;
                            TransparentPen.Color = System.Drawing.Color.FromArgb(255, System.Drawing.Color.Black);
                            GVisual.HideControl(Asama2_Yukseklik_Panel, drawingPanel);
                        }
                    }
                    else if (toplam_nesne_yuksekligi_asama2 >
                        selectedDepo.DepoAlaniYuksekligi - selectedDepo.nesneYuksekligi &&
                        toplam_nesne_yuksekligi_asama2 <= selectedDepo.DepoAlaniYuksekligi)
                    {
                        if (toplam_nesne_yuksekligi == selectedDepo.nesneYuksekligi &&
                            toplam_nesne_yuksekligi * 2 > selectedDepo.DepoAlaniYuksekligi)
                        {
                            toplam_nesne_yuksekligi_asama2 = selectedDepo.DepoAlaniYuksekligi;
                        }

                        GVisual.HideControl(Asama2_Yukseklik_Panel, drawingPanel);
                        CenterControltoLeftSideofRectangleVertically(selectedDepo.Rectangle, Placement_StartLocation_Panel, drawingPanel);
                    }
                }
                else
                {
                    if (nesne_sayisi == 0)
                    {
                        if (toplam_nesne_yuksekligi == selectedDepo.nesneYuksekligi &&
                            toplam_nesne_yuksekligi * 2 > selectedDepo.DepoAlaniYuksekligi)
                        {
                            toplam_nesne_yuksekligi_asama2 = selectedDepo.DepoAlaniYuksekligi;
                        }
                        else
                        {
                            CustomNotifyIcon notify = new CustomNotifyIcon();
                            notify.showAlert("Girdiğiniz nesne sayısı 0'dan büyük olmalı", CustomNotifyIcon.enmType.Error);
                        }
                    }
                    else
                    {
                        if (toplam_nesne_yuksekligi_asama2 <= selectedDepo.DepoAlaniYuksekligi - selectedDepo.nesneYuksekligi)
                        {
                            var result =
                                MessageBox.Show("Depoda yükseklik açısından yer var. " +
                                "Eğer bunu onaylarsanız, depo alanını dolduracak kadar nesne koyulmayacak. " +
                                "Devam etmek istiyor musunuz?", "Emin misiniz?",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                            if (result == DialogResult.Yes)
                            {
                                GVisual.HideControl(Asama2_Yukseklik_Panel, drawingPanel);
                                CenterControltoLeftSideofRectangleVertically(selectedDepo.Rectangle, Placement_StartLocation_Panel, drawingPanel);
                            }
                            else
                            {
                                MovingParameter = false;
                                TransparentPen.Color = System.Drawing.Color.FromArgb(255, System.Drawing.Color.Black);
                                GVisual.HideControl(Asama2_Yukseklik_Panel, drawingPanel);
                            }
                        }
                        else if (toplam_nesne_yuksekligi_asama2 >
                            selectedDepo.DepoAlaniYuksekligi - selectedDepo.nesneYuksekligi &&
                            toplam_nesne_yuksekligi_asama2 <= selectedDepo.DepoAlaniYuksekligi)
                        {
                            GVisual.HideControl(Asama2_Yukseklik_Panel, drawingPanel);
                            CenterControltoLeftSideofRectangleVertically(selectedDepo.Rectangle, Placement_StartLocation_Panel, drawingPanel);
                        }
                    }
                }
            }
        }
        private void btn_Placement_Yukseklik_Vazgec_Click(object sender, EventArgs e)
        {
            TransparentPen.Color = System.Drawing.Color.FromArgb(255, System.Drawing.Color.Black);
            MovingParameter = false;
            Parameter = string.Empty;
            GVisual.HideControl(Asama2_Yukseklik_Panel, drawingPanel);
            upDown_1Asama_NesneSayisi.Value = 0;
            upDown_2Asama_NesneSayisi.Value = 0;
        }
        private void upDown_2Asama_NesneSayisi_ValueChanged(object sender, EventArgs e)
        {
            if (selectedDepo != null)
            {
                int depo_alani_yuksekligi = selectedDepo.DepoAlaniYuksekligi;
                int nesne_sayisi1 = (int)upDown_2Asama_NesneSayisi.Value;
                int asama2_Nesne_Yuksekligi = nesne_sayisi1 * selectedDepo.nesneYuksekligi;

                int kalan_depo_yuksekligi = selectedDepo.DepoAlaniYuksekligi - toplam_nesne_yuksekligi;
                int kalan_toplam_nesne_sayisi = depo_alani_yuksekligi / selectedDepo.nesneYuksekligi - nesne_sayisi;

                int toplam_nesne_height = toplam_nesne_yuksekligi + asama2_Nesne_Yuksekligi;

                if (asama2_Nesne_Yuksekligi <= kalan_depo_yuksekligi)
                {
                    lbl_Placement_Yukseklik_Kalan_Depo_Yuksekligi_Value.Text = $"{kalan_depo_yuksekligi - asama2_Nesne_Yuksekligi} cm";
                    lbl_Placement_Yukseklik_Toplam_Nesne_Yuksekligi_Value.Text = $"{toplam_nesne_height} cm";
                    lbl_Placement_Yukseklik_upDown_Nesne_Yuksekligi_Value2.Text = $"{asama2_Nesne_Yuksekligi} cm";
                    lbl_Placement_Yukseklik_Konulabilecek_NesneSayisi_Value.Text = $"{kalan_toplam_nesne_sayisi - nesne_sayisi1} Adet";
                    toplam_nesne_yuksekligi_asama2 = toplam_nesne_yuksekligi + asama2_Nesne_Yuksekligi;
                }
                else
                {
                    upDown_2Asama_NesneSayisi.Value = nesne_sayisi1 - 1;
                    CustomNotifyIcon notify = new CustomNotifyIcon();
                    notify.showAlert("Toplam nesne yüksekliği deponun yüksekliğinden fazla olamaz.", CustomNotifyIcon.enmType.Error);
                }
            }
        }
        private void radio_Start_From_Middle_CheckedChanged(object sender, EventArgs e)
        {
            if (radio_Start_From_Middle.Checked)
            {
                Parameter = string.Empty;
                rectangles.Clear();
                timer.Stop();
                drawingPanel.Invalidate();
            }
        }
        private void textBoxFloat_TextChanged(object sender, EventArgs e)
        {
            Krypton.Toolkit.KryptonTextBox textBox = sender as Krypton.Toolkit.KryptonTextBox;

            if (textBox != null)
            {

                if (textBox.Text.Contains('.'))
                {
                    int decimalIndex = textBox.Text.IndexOf('.');

                    if (textBox.Text.Length - decimalIndex - 1 > 3)
                    {
                        textBox.Text = textBox.Text.Substring(0, decimalIndex + 2);

                        textBox.SelectionStart = textBox.Text.Length;
                    }
                }

                if (textBox.Text.Contains(','))
                {
                    int decimalIndex = textBox.Text.IndexOf(',');

                    if (textBox.Text.Length - decimalIndex - 1 > 3)
                    {
                        textBox.Text = textBox.Text.Substring(0, decimalIndex + 2);

                        textBox.SelectionStart = textBox.Text.Length;
                    }
                }

                if (textBox.Text != "cm girin" && textBox.Text != "metre girin" &&
                    textBox.Text != "Açıklama girin" && textBox.Text != "Depo adı girin" &&
                    textBox.Text != "Tür kodu girin")
                {
                    textBox.StateCommon.Content.Color1 = System.Drawing.Color.Black;
                }
                else
                {
                    textBox.StateCommon.Content.Color1 = System.Drawing.Color.LightGray;
                }
            }
        }
        private void textBoxFloat_KeyPress(object sender, KeyPressEventArgs e)
        {
            Krypton.Toolkit.KryptonTextBox textBox = sender as Krypton.Toolkit.KryptonTextBox;

            if (char.IsControl(e.KeyChar)) return;

            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            if (e.KeyChar == '.' && textBox.Text.Contains('.') || e.KeyChar == ',' && textBox.Text.Contains(","))
            {
                e.Handled = true;
            }

            if (textBox.Text.Contains('.'))
            {
                int decimalIndex = textBox.Text.IndexOf('.');

                if (textBox.SelectionStart > decimalIndex && textBox.Text.Length - decimalIndex > 3)
                {
                    e.Handled = true;
                }
            }
            if (textBox.Text.Contains(','))
            {
                int decimalIndex = textBox.Text.IndexOf(',');

                if (textBox.SelectionStart > decimalIndex && textBox.Text.Length - decimalIndex > 3)
                {
                    e.Handled = true;
                }
            }
        }


        private void FocusEntertoTXTs(object sender, EventArgs eventArgs)
        {
            Krypton.Toolkit.KryptonTextBox textBox = sender as Krypton.Toolkit.KryptonTextBox;

            if (textBox != null)
            {
                if (textBox.Text != "cm girin" && textBox.Text != "metre girin" && 
                    textBox.Text != "Açıklama girin" && textBox.Text != "Depo adı girin" &&
                    textBox.Text != "Tür kodu girin")
                {
                    textBox.StateCommon.Content.Color1 = System.Drawing.Color.Black;
                }
                else
                {
                    textBox.StateCommon.Content.Color1 = System.Drawing.Color.LightGray;
                    textBox.Clear();
                }
            }
        }
        private void TextChangedTXTs(object sender, EventArgs eventArgs)
        {
            Krypton.Toolkit.KryptonTextBox textBox = sender as Krypton.Toolkit.KryptonTextBox;

            if (textBox != null)
            {
                if (textBox.Text != "cm girin" && textBox.Text != "metre girin" &&
                    textBox.Text != "Açıklama girin" && textBox.Text != "Depo adı girin" &&
                    textBox.Text != "Tür kodu girin")
                {
                    textBox.StateCommon.Content.Color1 = System.Drawing.Color.Black;
                }
                else
                {
                    textBox.StateCommon.Content.Color1 = System.Drawing.Color.LightGray;
                }
            }
        }

        private void btn_Alan_Olusturma_Panelini_Kapat_Click(object sender, EventArgs e)
        {
            DrawingPanelEnlarge(Alan_Olusturma_Paneli, this);
            ambar_Boyut_Degistir = false;
        }
        public RectangleF SnapRectangles(RectangleF rectangle, RectangleF refRectangle)
        {
            System.Drawing.Point rectangleRightMiddle = GVisual.GetMiddleOfRightEdge(rectangle);
            System.Drawing.Point rectangleLeftMiddle = GVisual.GetMiddleOfLeftEdge(rectangle);
            System.Drawing.Point rectangleTopMiddle = GVisual.GetMiddleOfTopEdge(rectangle);
            System.Drawing.Point rectangleBotMiddle = GVisual.GetMiddleOfBottomEdge(rectangle);
            System.Drawing.Point rectangleTopLeftCorner = GVisual.GetTopLeftCorner(rectangle);
            System.Drawing.Point rectangleTopRightCorner = GVisual.GetTopRightCorner(rectangle);
            System.Drawing.Point rectangleBotLeftCorner = GVisual.GetBottomLeftCorner(rectangle);
            System.Drawing.Point rectangleBotRightCorner = GVisual.GetBottomRightCorner(rectangle);


            System.Drawing.Point refRectangleRightMiddle = GVisual.GetMiddleOfRightEdge(refRectangle);
            System.Drawing.Point refRectangleLeftMiddle = GVisual.GetMiddleOfLeftEdge(refRectangle);
            System.Drawing.Point refRectangleTopMiddle = GVisual.GetMiddleOfTopEdge(refRectangle);
            System.Drawing.Point refRectangleBotMiddle = GVisual.GetMiddleOfBottomEdge(refRectangle);
            System.Drawing.Point refRectangleTopLeftCorner = GVisual.GetTopLeftCorner(refRectangle);
            System.Drawing.Point refRectangleTopRightCorner = GVisual.GetTopRightCorner(refRectangle);
            System.Drawing.Point refRectangleBotLeftCorner = GVisual.GetBottomLeftCorner(refRectangle);
            System.Drawing.Point refRectangleBotRightCorner = GVisual.GetBottomRightCorner(refRectangle);

            int snapping = 12;

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
                snapped = true;
                return rectangle;
            }
            else if (BotSideSnapDistance < snapping)
            {
                rectangle = GVisual.MoveRectangleToPoint(rectangle,
               refRectangle.X + (refRectangle.Width / 2 - rectangle.Width / 2),
               refRectangle.Bottom);
                snapped = true;
                return rectangle;
            }
            else if (LeftSideSnapDistance < snapping)
            {
                rectangle = GVisual.MoveRectangleToPoint(rectangle,
               refRectangle.X - rectangle.Width,
               refRectangle.Y + (refRectangle.Height / 2 - rectangle.Height / 2));
                snapped = true;
                return rectangle;
            }
            else if (RightSideSnapDistance < snapping)
            {
                rectangle = GVisual.MoveRectangleToPoint(rectangle,
               refRectangle.X + refRectangle.Width,
               refRectangle.Y + (refRectangle.Height / 2 - rectangle.Height / 2));
                snapped = true;
                return rectangle;
            }
            else if (TopRightCornerSnapDistance < snapping)
            {
                rectangle = GVisual.MoveRectangleToPoint(rectangle,
               refRectangle.X + refRectangle.Width - rectangle.Width,
               refRectangle.Y - rectangle.Height);
                snapped = true;
                return rectangle;
            }
            else if (BotRightCornerSnapDistance < snapping)
            {
                rectangle = GVisual.MoveRectangleToPoint(rectangle,
               refRectangle.X + refRectangle.Width - rectangle.Width,
               refRectangle.Y + refRectangle.Height);
                snapped = true;
                return rectangle;
            }
            else if (TopLeftCornerSnapDistance < snapping)
            {
                rectangle = GVisual.MoveRectangleToPoint(rectangle,
               refRectangle.X,
               refRectangle.Y - rectangle.Height);
                snapped = true;
                return rectangle;
            }
            else if (BotLeftCornerSnapDistance < snapping)
            {
                rectangle = GVisual.MoveRectangleToPoint(rectangle,
               refRectangle.X,
               refRectangle.Y + refRectangle.Height);
                snapped = true;
                return rectangle;
            }
            else if (RightBotSnapDistance < snapping)
            {
                rectangle = GVisual.MoveRectangleToPoint(rectangle,
               refRectangle.X + refRectangle.Width,
               refRectangle.Y + refRectangle.Height - rectangle.Height);
                snapped = true;
                return rectangle;
            }
            else if (RightTopSnapDistance < snapping)
            {
                rectangle = GVisual.MoveRectangleToPoint(rectangle,
               refRectangle.X + refRectangle.Width,
               refRectangle.Y);
                snapped = true;
                return rectangle;
            }
            else if (LeftTopSnapDistance < snapping)
            {
                rectangle = GVisual.MoveRectangleToPoint(rectangle,
               refRectangle.X - rectangle.Width,
               refRectangle.Y);
                snapped = true;
                return rectangle;
            }
            else if (LeftBotSnapDistance < snapping)
            {
                rectangle = GVisual.MoveRectangleToPoint(rectangle,
               refRectangle.X - rectangle.Width,
               refRectangle.Y + refRectangle.Height - rectangle.Height);
                snapped = true;
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



        private void btn_Conveyor_Reference_Sayisi_Onayla_Click(object sender, EventArgs e)
        {
            if (selectedConveyor != null)
            {
                errorProvider.Clear();
                float Conveyor_X = StrLib.ReplaceDotWithCommaReturnFloat(txt_Conveyor_Reference_X,
                    errorProvider, "Bu alan boş bırakılamaz.", "Buraya bir sayı girmelisiniz", "Buraya 0 ya da daha küçük bir değer giremezsiniz.");

                float Conveyor_Y = StrLib.ReplaceDotWithCommaReturnFloat(txt_Conveyor_Reference_Y,
                    errorProvider, "Bu alan boş bırakılamaz.", "Buraya bir sayı girmelisiniz", "Buraya 0 ya da daha küçük bir değer giremezsiniz.");


                if (Conveyor_X > selectedConveyor.ConveyorEni * 100)
                {
                    errorProvider.SetError(txt_Conveyor_Reference_X,
                        "Girdiğiniz değer conveyor'un eninden küçük olmalı.");
                }
                if (Conveyor_Y > selectedConveyor.ConveyorBoyu * 100)
                {
                    errorProvider.SetError(txt_Conveyor_Reference_Y,
                        "Girdiğiniz değer conveyor'un eninden küçük olmalı.");
                }

                if (Conveyor_X <= 0)
                {
                    errorProvider.SetError(txt_Conveyor_Reference_X,
                        "0 ya da daha küçük bir değer giremezsiniz.");
                }

                if (Conveyor_Y <= 0)
                {
                    errorProvider.SetError(txt_Conveyor_Reference_Y,
                        "0 ya da daha küçük bir değer giremezsiniz.");
                }

                if (!errorProvider.HasErrors)
                {
                    ConveyorReferencePoint point = new ConveyorReferencePoint(10, 10, 6, 6, 1f, null, selectedConveyor, this);

                    PointF newpoint = ResizeandMoveChildRectangleInsideParent(selectedConveyor.Rectangle,
                        Conveyor_X, Conveyor_Y, selectedConveyor.ConveyorEni * 100,
                        selectedConveyor.ConveyorBoyu * 100, point.Rectangle);

                    point.MoveRectangleExact(newpoint.X, newpoint.Y);

                    point.Pointsize = 4;
                    point.LocationofRect = new System.Drawing.Point((int)point.Rectangle.X, (int)point.Rectangle.Y);
                    point.KareX = point.Rectangle.X;
                    point.KareY = point.Rectangle.Y;
                    point.KareEni = point.Rectangle.Width;
                    point.KareBoyu = point.Rectangle.Height;
                    point.OriginalKareX = point.OriginalRectangle.X;
                    point.OriginalKareY = point.OriginalRectangle.Y;
                    point.OriginalKareEni = point.OriginalRectangle.Width;
                    point.OriginalKareBoyu = point.OriginalRectangle.Height;
                    point.LocationX = Conveyor_X;
                    point.LocationY = Conveyor_Y;
                    point.FixedPointLocation = string.Empty;

                    float X = point.Rectangle.X - selectedConveyor.Rectangle.X;
                    float Y = point.Rectangle.Y - selectedConveyor.Rectangle.Y;

                    point.OriginalLocationInsideParent = new PointF(X, Y);

                    selectedConveyor.ConveyorReferencePoints.Add(point);
                    drawingPanel.Invalidate();
                    GVisual.HideControl(Conveyor_Reference_Sayisi_Paneli, drawingPanel);
                    AddReferencePoint = false;
                }
            }
        }
        private void btn_Select_Fixed_Conveyor_Reference_Point_Click(object sender, EventArgs e)
        {
            GVisual.HideControl(Conveyor_Reference_FixedorManuel_Panel, drawingPanel);
            CenterControltoLeftSideofRectangleVertically(selectedConveyor.Rectangle,
                Conveyor_Reference_Fixed_Panel, drawingPanel);

        }
        private void btn_Manuel_Reference_Point_Click(object sender, EventArgs e)
        {
            GVisual.HideControl(Conveyor_Reference_FixedorManuel_Panel, drawingPanel);
            CenterControltoLeftSideofRectangleVertically(selectedConveyor.Rectangle, Conveyor_Reference_Sayisi_Paneli,
                drawingPanel);
        }
        private void AddConveyorRefPoint(float Conveyor_X, float Conveyor_Y, string fixedPointLocation)
        {
            ConveyorReferencePoint point = new ConveyorReferencePoint(Conveyor_X,
                Conveyor_Y, 6, 6, 1f, null, selectedConveyor, this);

            point.MoveRectangleExact(Conveyor_X - point.Rectangle.Width / 2,
                Conveyor_Y - point.Rectangle.Height / 2);

            var cm = ConvertRectanglesLocationtoCMInsideParentRectangle(point.Rectangle,
                selectedConveyor.Rectangle, selectedConveyor.ConveyorEni,
                selectedConveyor.ConveyorBoyu, true);

            point.Pointsize = 4;
            point.LocationofRect = new System.Drawing.Point((int)point.Rectangle.X, (int)point.Rectangle.Y);
            point.KareX = point.Rectangle.X;
            point.KareY = point.Rectangle.Y;
            point.KareEni = point.Rectangle.Width;
            point.KareBoyu = point.Rectangle.Height;
            point.OriginalKareX = point.OriginalRectangle.X;
            point.OriginalKareY = point.OriginalRectangle.Y;
            point.OriginalKareEni = point.OriginalRectangle.Width;
            point.OriginalKareBoyu = point.OriginalRectangle.Height;
            point.LocationX = cm.Item1;
            point.LocationY = cm.Item2;
            point.FixedPointLocation = fixedPointLocation;

            float X = point.Rectangle.X - selectedConveyor.Rectangle.X;
            float Y = point.Rectangle.Y - selectedConveyor.Rectangle.Y;

            point.OriginalLocationInsideParent = new PointF(X, Y);

            selectedConveyor.ConveyorReferencePoints.Add(point);
            drawingPanel.Invalidate();
            GVisual.HideControl(Conveyor_Reference_Fixed_Panel, drawingPanel);
            AddReferencePoint = false;
        }
        private void btn_Conveyor_Reference_Fixed_Onayla_Click(object sender, EventArgs e)
        {
            List<ConveyorReferencePoint> tempRef = new List<ConveyorReferencePoint>();

            foreach (var reff in selectedConveyor.ConveyorReferencePoints)
            {
                tempRef.Add(reff);
            }

            if (chk_Conveyor_Reference_Top.Checked)
            {
                PointF Top = GVisual.GetMiddleOfTopEdgeF(selectedConveyor.Rectangle);

                if (tempRef.Count > 0)
                {
                    foreach (var reff in tempRef)
                    {
                        if (reff.FixedPointLocation != "Top")
                        {
                            AddConveyorRefPoint(Top.X, Top.Y, "Top");
                        }
                    }
                }
                else
                {
                    AddConveyorRefPoint(Top.X, Top.Y, "Top");
                }
            }
            else
            {
                foreach (var reff in tempRef)
                {
                    if (reff.FixedPointLocation == "Top")
                    {
                        selectedConveyor.ConveyorReferencePoints.Remove(reff);
                    }
                }
            }
            if (chk_Conveyor_Reference_Bottom.Checked)
            {
                PointF Bottom = GVisual.GetMiddleOfBottomEdgeF(selectedConveyor.Rectangle);

                if (tempRef.Count > 0)
                {
                    foreach (var reff in tempRef)
                    {
                        if (reff.FixedPointLocation != "Bottom")
                        {
                            AddConveyorRefPoint(Bottom.X, Bottom.Y, "Bottom");
                        }
                    }
                }
                else
                {
                    AddConveyorRefPoint(Bottom.X, Bottom.Y, "Bottom");
                }
            }
            else
            {
                foreach (var reff in tempRef)
                {
                    if (reff.FixedPointLocation == "Bottom")
                    {
                        selectedConveyor.ConveyorReferencePoints.Remove(reff);
                    }
                }
            }
            if (chk_Conveyor_Reference_Left.Checked)
            {
                PointF Left = GVisual.GetMiddleOfLeftEdgeF(selectedConveyor.Rectangle);

                if (tempRef.Count > 0)
                {
                    foreach (var reff in tempRef)
                    {
                        if (reff.FixedPointLocation != "Left")
                        {
                            AddConveyorRefPoint(Left.X, Left.Y, "Left");
                        }
                    }
                }
                else
                {
                    AddConveyorRefPoint(Left.X, Left.Y, "Left");
                }
            }
            else
            {
                foreach (var reff in tempRef)
                {
                    if (reff.FixedPointLocation == "Left")
                    {
                        selectedConveyor.ConveyorReferencePoints.Remove(reff);
                    }
                }
            }
            if (chk_Conveyor_Reference_Right.Checked)
            {
                PointF Right = GVisual.GetMiddleOfRightEdgeF(selectedConveyor.Rectangle);

                if (tempRef.Count > 0)
                {
                    foreach (var reff in tempRef)
                    {
                        if (reff.FixedPointLocation != "Right")
                        {
                            AddConveyorRefPoint(Right.X, Right.Y, "Right");
                        }
                    }
                }
                else
                {
                    AddConveyorRefPoint(Right.X, Right.Y, "Right");
                }
            }
            else
            {
                foreach (var reff in tempRef)
                {
                    if (reff.FixedPointLocation == "Right")
                    {
                        selectedConveyor.ConveyorReferencePoints.Remove(reff);
                    }
                }
            }
            if (chk_Conveyor_Reference_Center.Checked)
            {
                PointF Center = GVisual.GetCenterF(selectedConveyor.Rectangle);

                if (tempRef.Count > 0)
                {
                    foreach (var reff in tempRef)
                    {
                        if (reff.FixedPointLocation != "Center")
                        {
                            AddConveyorRefPoint(Center.X, Center.Y, "Center");
                        }
                    }
                }
                else
                {
                    AddConveyorRefPoint(Center.X, Center.Y, "Center");
                }
            }
            else
            {
                foreach (var reff in tempRef)
                {
                    if (reff.FixedPointLocation == "Center")
                    {
                        selectedConveyor.ConveyorReferencePoints.Remove(reff);
                    }
                }
            }
            GVisual.HideControl(Conveyor_Reference_Fixed_Panel, drawingPanel);
            drawingPanel.Invalidate();
        }
        private void btn_Layout_Kaydet_Click(object sender, EventArgs e)
        {
            using (var dialog = new LayoutNaming())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    LayoutDescription = dialog.txt_Layout_Aciklama.Text;
                    LayoutName = dialog.txt_Layout_Isim.Text;

                    this.DialogResult = DialogResult.OK;
                }
            }
        }




        //Deleted Depo Events
        private void alanınYeriniVeBoyutunuDeğiştirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DrawingPanelShrink(PaddingPanel, this, leftSidePanelLocation);
            ShowTextboxes(txt_Width.Location, txt_Height.Location,
                    txt_Left_Padding.Location, txt_Right_Padding.Location,
                    txt_Top_Padding.Location, txt_Bottom_Padding.Location);
            ShowMoveCheckButton();
            Manuel_Move = true;
            if (selectedDepo != null)
            {
                ManuelDepoRectangle = selectedDepo.Rectangle;
                UnchangedDepoAlaniBoyu = selectedDepo.DepoAlaniBoyu;
                UnchangedDepoAlaniEni = selectedDepo.DepoAlaniEni;
                //txt_width.text = $"{unchangeddepoalanieni}";
                //txt_height.text = $"{unchangeddepoalaniboyu}";
            }
        }
        private void depoyuSilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Depo> depos = new List<Depo>();
            if (Ambar != null)
            {
                if (selectedDepo != null)
                {
                    foreach (var depo in Ambar.depolar)
                    {
                        if (depo == selectedDepo)
                        {
                            depos.Add(depo);
                        }
                    }

                    foreach (var depo1 in depos)
                    {
                        Ambar.depolar.Remove(depo1);
                    }
                }
            }
            selectedDepo = null;
            CopyDepo = null;
            drawingPanel.Invalidate();
        }
        private void izgaraHaritasıOluşturToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DrawingPanelShrink(Izgara_Mal_Paneli, this, leftSidePanelLocation);
        }
        private void izgaraHaritasiniSilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Balya_Yerleştirme.Models.Cell> cells = new List<Balya_Yerleştirme.Models.Cell>();
            if (Ambar != null)
            {
                if (selectedDepo != null)
                {
                    foreach (var cell in selectedDepo.gridmaps)
                    {
                        cells.Add(cell);
                    }

                    foreach (var cell1 in cells)
                    {
                        selectedDepo.gridmaps.Remove(cell1);
                    }
                }
            }
            drawingPanel.Invalidate();
        }




        //Deleted Conveyor Events
        private void conveyorYerDegistirMenuStripItem_Click(object sender, EventArgs e)
        {
            DrawingPanelShrink(PaddingPanel, this, leftSidePanelLocation);
            ShowTextboxes(txt_Width.Location, txt_Height.Location,
                    txt_Left_Padding.Location, txt_Right_Padding.Location,
                    txt_Top_Padding.Location, txt_Bottom_Padding.Location);
            ShowMoveCheckButton();
            Manuel_Move = true;
            if (selectedConveyor != null)
            {
                ManuelConveyorRectangle = selectedConveyor.Rectangle;
                UnchangedConveyorEni = selectedConveyor.ConveyorEni;
                UnchangedConveyorBoyu = selectedConveyor.ConveyorBoyu;
                //txt_Width.Text = $"{UnchangedConveyorEni}";
                //txt_Height.Text = $"{UnchangedConveyorBoyu}";
            }
        }
        private void conveyoruSilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Conveyor> conveyors = new List<Conveyor>();
            if (Ambar != null)
            {
                if (selectedConveyor != null)
                {
                    foreach (var conveyor in Ambar.conveyors)
                    {
                        if (selectedConveyor == conveyor)
                        {
                            conveyors.Add(conveyor);
                        }
                    }

                    foreach (var conv in conveyors)
                    {
                        Ambar.conveyors.Remove(conv);
                    }
                }
            }
            selectedConveyor = null;
            CopyConveyor = null;
            drawingPanel.Invalidate();
        }
        private void referansNoktasıEkleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedConveyor != null)
            {
                AddReferencePoint = true;
                CenterControltoLeftSideofRectangleVertically
                    (selectedConveyor.Rectangle, Conveyor_Reference_FixedorManuel_Panel, drawingPanel);
            }
        }
        private void referansNoktalarınıSilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedConveyor != null)
            {
                List<ConveyorReferencePoint> tempList = new List<ConveyorReferencePoint>();

                foreach (var reff in selectedConveyor.ConveyorReferencePoints)
                {
                    tempList.Add(reff);
                }

                foreach (var ref1 in tempList)
                {
                    selectedConveyor.ConveyorReferencePoints.Remove(ref1);
                }
                drawingPanel.Invalidate();
            }
        }
        private void referansNoktalarınınYerleriniDeğiştirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedConveyor != null)
            {
                float reffX = 0;
                float reffY = 0;
                using (var dialog = new ConveyorReffYerDegistir(selectedConveyor))
                {
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        foreach (var reff in selectedConveyor.ConveyorReferencePoints)
                        {
                            foreach (Panel panel in dialog.Panel.Controls)
                            {
                                if (reff == (ConveyorReferencePoint)panel.Tag)
                                {
                                    foreach (var textbox in panel.Controls)
                                    {
                                        if (textbox is TextBox)
                                        {
                                            TextBox textBox = new TextBox();
                                            textBox = (TextBox)textbox;

                                            if (textBox.Name == "textBoxLocationX")
                                            {
                                                reffX = float.Parse(textBox.Text);
                                            }
                                            if (textBox.Name == "textBoxLocationY")
                                            {
                                                reffY = float.Parse(textBox.Text);
                                            }
                                        }

                                        PointF newpoint = ResizeandMoveChildRectangleInsideParent(selectedConveyor.Rectangle,
                                                 reffX, reffY, selectedConveyor.ConveyorEni * 100,
                                                 selectedConveyor.ConveyorBoyu * 100, reff.Rectangle);

                                        reff.MoveRectangleExact(newpoint.X, newpoint.Y);

                                        var cm = ConvertRectanglesLocationtoCMInsideParentRectangle(reff.Rectangle,
                                            selectedConveyor.Rectangle, selectedConveyor.ConveyorEni,
                                            selectedConveyor.ConveyorBoyu, true);

                                        reff.Pointsize = 4;
                                        reff.LocationofRect = new System.Drawing.Point((int)reff.Rectangle.X, (int)reff.Rectangle.Y);
                                        reff.KareX = reff.Rectangle.X;
                                        reff.KareY = reff.Rectangle.Y;
                                        reff.KareEni = reff.Rectangle.Width;
                                        reff.KareBoyu = reff.Rectangle.Height;
                                        reff.OriginalKareX = reff.OriginalRectangle.X;
                                        reff.OriginalKareY = reff.OriginalRectangle.Y;
                                        reff.OriginalKareEni = reff.OriginalRectangle.Width;
                                        reff.OriginalKareBoyu = reff.OriginalRectangle.Height;
                                        reff.LocationX = cm.Item1;
                                        reff.LocationY = cm.Item2;
                                        reff.FixedPointLocation = reff.FixedPointLocation;

                                        float X = reff.Rectangle.X - selectedConveyor.Rectangle.X;
                                        float Y = reff.Rectangle.Y - selectedConveyor.Rectangle.Y;

                                        reff.OriginalLocationInsideParent = new PointF(X, Y);

                                        drawingPanel.Invalidate();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }




        //Alan ToolStripMenu Events
        private void silToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Ambar != null)
            {
                Ambar = null;
                drawingPanel.Invalidate();
            }
        }
        private void boşaltToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Ambar != null)
            {
                List<Depo> Depos = new List<Depo>();
                List<Conveyor> Conveyors = new List<Conveyor>();

                foreach (var depo in Ambar.depolar)
                {
                    Depos.Add(depo);
                }
                foreach (var conveyor in Ambar.conveyors)
                {
                    Conveyors.Add(conveyor);
                }

                foreach (var depo in Depos)
                {
                    Ambar.depolar.Remove(depo);
                }
                foreach (var conveyor in Conveyors)
                {
                    Ambar.conveyors.Remove(conveyor);
                }
                drawingPanel.Invalidate();
            }
        }




        //Depo ToolStripMenu Events
        //Izgara Haritası ToolStripMenuItems Events
        private void ızgaraHaritasıOluşturToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripIzgara = false;
            DrawingPanelShrink(Izgara_Mal_Paneli, this, leftSidePanelLocation);
        }
        private void silToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            List<Balya_Yerleştirme.Models.Cell> cells = new List<Balya_Yerleştirme.Models.Cell>();
            if (Ambar != null)
            {
                if (selectedDepo != null)
                {
                    foreach (var cell in selectedDepo.gridmaps)
                    {
                        cells.Add(cell);
                    }

                    foreach (var cell1 in cells)
                    {
                        selectedDepo.gridmaps.Remove(cell1);
                    }
                }
            }
            drawingPanel.Invalidate();
        }
        //Depo ToolStripMenuItems Events
        private void deponunYeriniVeBoyutunuDeğiştirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DrawingPanelShrink(PaddingPanel, this, leftSidePanelLocation);
            ShowTextboxes(txt_Width.Location, txt_Height.Location,
                    txt_Left_Padding.Location, txt_Right_Padding.Location,
                    txt_Top_Padding.Location, txt_Bottom_Padding.Location);
            ShowMoveCheckButton();
            Manuel_Move = true;
            if (selectedDepo != null)
            {
                ManuelDepoRectangle = selectedDepo.Rectangle;
                UnchangedDepoAlaniBoyu = selectedDepo.DepoAlaniBoyu;
                UnchangedDepoAlaniEni = selectedDepo.DepoAlaniEni;
                //txt_width.text = $"{unchangeddepoalanieni}";
                //txt_height.text = $"{unchangeddepoalaniboyu}";
            }
        }
        private void depoyuSilToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            List<Depo> depos = new List<Depo>();
            if (Ambar != null)
            {
                if (selectedDepo != null)
                {
                    foreach (var depo in Ambar.depolar)
                    {
                        if (depo == selectedDepo)
                        {
                            depos.Add(depo);
                        }
                    }

                    foreach (var depo1 in depos)
                    {
                        Ambar.depolar.Remove(depo1);
                    }
                }
            }
            selectedDepo = null;
            CopyDepo = null;
            drawingPanel.Invalidate();
        }




        //Conveyor ToolStripMenu Events
        //Referans ToolStripMenuItems Events
        private void Referans_ekleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedConveyor != null)
            {
                AddReferencePoint = true;
                CenterControltoLeftSideofRectangleVertically
                    (selectedConveyor.Rectangle, Conveyor_Reference_FixedorManuel_Panel, drawingPanel);
            }
        }
        private void Referans_silToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedConveyor != null)
            {
                List<ConveyorReferencePoint> tempList = new List<ConveyorReferencePoint>();

                foreach (var reff in selectedConveyor.ConveyorReferencePoints)
                {
                    tempList.Add(reff);
                }

                foreach (var ref1 in tempList)
                {
                    selectedConveyor.ConveyorReferencePoints.Remove(ref1);
                }
                drawingPanel.Invalidate();
            }
        }
        private void Referans_yerleriniDeğiştirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedConveyor != null)
            {
                float reffX = 0;
                float reffY = 0;
                using (var dialog = new ConveyorReffYerDegistir(selectedConveyor))
                {
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        foreach (var reff in selectedConveyor.ConveyorReferencePoints)
                        {
                            foreach (Panel panel in dialog.Panel.Controls)
                            {
                                if (reff == (ConveyorReferencePoint)panel.Tag)
                                {
                                    foreach (var textbox in panel.Controls)
                                    {
                                        if (textbox is TextBox)
                                        {
                                            TextBox textBox = new TextBox();
                                            textBox = (TextBox)textbox;

                                            if (textBox.Name == "textBoxLocationX")
                                            {
                                                reffX = float.Parse(textBox.Text);
                                            }
                                            if (textBox.Name == "textBoxLocationY")
                                            {
                                                reffY = float.Parse(textBox.Text);
                                            }
                                        }

                                        PointF newpoint = ResizeandMoveChildRectangleInsideParent(selectedConveyor.Rectangle,
                                                 reffX, reffY, selectedConveyor.ConveyorEni * 100,
                                                 selectedConveyor.ConveyorBoyu * 100, reff.Rectangle);

                                        reff.MoveRectangleExact(newpoint.X, newpoint.Y);

                                        var cm = ConvertRectanglesLocationtoCMInsideParentRectangle(reff.Rectangle,
                                            selectedConveyor.Rectangle, selectedConveyor.ConveyorEni,
                                            selectedConveyor.ConveyorBoyu, true);

                                        reff.Pointsize = 4;
                                        reff.LocationofRect = new System.Drawing.Point((int)reff.Rectangle.X, (int)reff.Rectangle.Y);
                                        reff.KareX = reff.Rectangle.X;
                                        reff.KareY = reff.Rectangle.Y;
                                        reff.KareEni = reff.Rectangle.Width;
                                        reff.KareBoyu = reff.Rectangle.Height;
                                        reff.OriginalKareX = reff.OriginalRectangle.X;
                                        reff.OriginalKareY = reff.OriginalRectangle.Y;
                                        reff.OriginalKareEni = reff.OriginalRectangle.Width;
                                        reff.OriginalKareBoyu = reff.OriginalRectangle.Height;
                                        reff.LocationX = cm.Item1;
                                        reff.LocationY = cm.Item2;
                                        reff.FixedPointLocation = reff.FixedPointLocation;

                                        float X = reff.Rectangle.X - selectedConveyor.Rectangle.X;
                                        float Y = reff.Rectangle.Y - selectedConveyor.Rectangle.Y;

                                        reff.OriginalLocationInsideParent = new PointF(X, Y);

                                        drawingPanel.Invalidate();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        //Conveyor ToolStripMenuItems Events
        private void Conveyor_yeriniVeBoyutunuDeğiştirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DrawingPanelShrink(PaddingPanel, this, leftSidePanelLocation);
            ShowTextboxes(txt_Width.Location, txt_Height.Location,
                    txt_Left_Padding.Location, txt_Right_Padding.Location,
                    txt_Top_Padding.Location, txt_Bottom_Padding.Location);
            ShowMoveCheckButton();
            Manuel_Move = true;
            if (selectedConveyor != null)
            {
                ManuelConveyorRectangle = selectedConveyor.Rectangle;
                UnchangedConveyorEni = selectedConveyor.ConveyorEni;
                UnchangedConveyorBoyu = selectedConveyor.ConveyorBoyu;
                //txt_Width.Text = $"{UnchangedConveyorEni}";
                //txt_Height.Text = $"{UnchangedConveyorBoyu}";
            }
        }
        private void Conveyor_silToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Conveyor> conveyors = new List<Conveyor>();
            if (Ambar != null)
            {
                if (selectedConveyor != null)
                {
                    foreach (var conveyor in Ambar.conveyors)
                    {
                        if (selectedConveyor == conveyor)
                        {
                            conveyors.Add(conveyor);
                        }
                    }

                    foreach (var conv in conveyors)
                    {
                        Ambar.conveyors.Remove(conv);
                    }
                }
            }
            selectedConveyor = null;
            CopyConveyor = null;
            drawingPanel.Invalidate();
        }

        private void boyutunuDeğiştirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripIzgara = false;
            DrawingPanelShrink(Izgara_Mal_Paneli, this, leftSidePanelLocation);
        }







        //private void DrawParameters(Graphics g)
        //{
        //    if (selectedDepo != null)
        //    {
        //        Brush brush = new SolidBrush(System.Drawing.Color.Black);
        //        System.Drawing.Font font = new System.Drawing.Font("Arial", 8);
        //        g.DrawString(selectedDepo.itemDrop_StartLocation, font, brush, new Point(drawingPanel.Left, drawingPanel.Top));
        //        g.DrawString(selectedDepo.itemDrop_Stage1, font, brush, new Point(drawingPanel.Left, drawingPanel.Top + 20));
        //        g.DrawString(selectedDepo.itemDrop_Stage2, font, brush, new Point(drawingPanel.Left, drawingPanel.Top + 40));
        //        g.DrawString(selectedDepo.itemDrop_UpDown, font, brush, new Point(drawingPanel.Left, drawingPanel.Top + 60));
        //        g.DrawString(selectedDepo.itemDrop_LeftRight, font, brush, new Point(drawingPanel.Left, drawingPanel.Top + 80));
        //        g.DrawString(selectedDepo.asama1_Yuksekligi.ToString(), font, brush, new Point(drawingPanel.Left, drawingPanel.Top + 100));
        //        g.DrawString(selectedDepo.asama2_Yuksekligi.ToString(), font, brush, new Point(drawingPanel.Left, drawingPanel.Top + 120));
        //    }
        //}
    }
}
