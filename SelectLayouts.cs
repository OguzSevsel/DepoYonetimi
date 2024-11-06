using Balya_Yerleştirme.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GUI_Library;
using String_Library;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Microsoft.Web.WebView2.Core;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Diagnostics.Metrics;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office.CustomUI;
using DocumentFormat.OpenXml.Presentation;
using Microsoft.Identity.Client.NativeInterop;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using DocumentFormat.OpenXml.Drawing.Charts;
using System.Security.Policy;
using static Balya_Yerleştirme.Utilities.Utils;
using Microsoft.Identity.Client;
using CustomNotification;
using Krypton.Toolkit;
using DocumentFormat.OpenXml.Drawing.Diagrams;

namespace Balya_Yerleştirme
{
    public partial class SelectLayouts : Form
    {
        public Panel DrawingPanel { get; set; }

        public System.Drawing.Point rightPanelLocation = new System.Drawing.Point(1258, 70);
        public System.Drawing.Size MainPanelSmallSize { get; set; } = new System.Drawing.Size(1240, 768);
        public System.Drawing.Size MainPanelLargeSize { get; set; } = new System.Drawing.Size(1560, 768);

        public Pen LayoutPen = new Pen(System.Drawing.Color.Black, 2);
        public PictureBox? SelectedPB { get; set; }
        public MainForm Main { get; set; }
        public int LayoutCount { get; set; }

        public int totalProgress = 0;

        public System.Drawing.Point PointProgressBar = new System.Drawing.Point(18, 76);

        public System.Drawing.Point PointSelectLayoutPanel = new System.Drawing.Point(12, 70);

        public System.Drawing.Point Btn_ChangeLayoutDescLocation = new System.Drawing.Point(242, 217);

        public System.Drawing.Point Btn_ChangeLayoutNameLocation = new System.Drawing.Point(242, 18);

        public System.Drawing.Point SmallTitleLocation = new System.Drawing.Point(426, 9);

        public System.Drawing.Point LargeTitleLocation = new System.Drawing.Point(595, 9);

        public Ambar BeforeAmbar = new Ambar();

        public SelectLayouts(Panel drawingPanel, MainForm main)
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Main = main;
            DrawingPanel = drawingPanel;
            GVisual.HideControl(SelectLayoutPanel, this);
            GVisual.HideControl(btn_ChangeLayoutName, InnerPanel1);
            GVisual.HideControl(btn_ChangeLayoutDescription, InnerPanel1);
            GVisual.HideControl(panel_LayoutMenu, this);
            GVisual.ShowControl(progressBarPanel, this, PointProgressBar);
            GetLayoutsFromDB();
        }

        public void GetLayoutsFromDB()
        {
            using (var context = new DBContext())
            {
                var layouts = (from x in context.Layout
                               select x).ToList();

                LayoutCount = layouts.Count;
                timer.Start();

                if (layouts.Count == 0)
                {
                    MessageBox.Show("Kayıtlı layout bulunamadı, lütfen layout oluşturduktan sonra tekrar deneyin.", "Layout bulunamadı.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.DialogResult = DialogResult.Cancel;
                }

                foreach (var layout in layouts)
                {
                    CreateLayouts(layout);
                }
            }
        }

        public async void CreateLayouts(Balya_Yerleştirme.Models.Layout layout)
        {

            var progress = new Progress<float>(value =>
            {
                Main.progressBar.Value = (int)value;
            });

            Ambar ambar = await Task.Run(() => LayoutYükleDatabaseOperation(layout, progress));

            if (ambar != null)
            {
                CreatePictureBox(ambar);
            }
        }

        private async Task<Ambar> LayoutYükleDatabaseOperation(Balya_Yerleştirme.Models.Layout layout, IProgress<float> progress)
        {
            using (var context = new DBContext())
            {
                int ProgressBarValue = Main.progressBar.Value;


                var ambar = await (from x in context.Ambars
                                   where x.LayoutId == layout.LayoutId
                                   select x).FirstOrDefaultAsync();

                if (ambar != null)
                {
                    Main.opcount += 0.1f;

                    if (Main.progressBar.Value < 100 && Main.opcount < 100)
                    {
                        progress.Report(Main.opcount);
                    }

                    Ambar newAmbar = new Ambar(ambar.OriginalKareX, ambar.OriginalKareY, ambar.OriginalKareEni, ambar.OriginalKareBoyu, Main, null);

                    newAmbar.AmbarId = ambar.AmbarId;
                    newAmbar.LayoutId = ambar.LayoutId;

                    newAmbar.AmbarEni = ambar.AmbarEni;
                    newAmbar.AmbarBoyu = ambar.AmbarBoyu;
                    newAmbar.AmbarName = ambar.AmbarName;
                    newAmbar.AmbarDescription = ambar.AmbarDescription;
                    newAmbar.KareX = ambar.KareX;
                    newAmbar.KareY = ambar.KareY;
                    newAmbar.KareEni = ambar.KareEni;
                    newAmbar.KareBoyu = ambar.KareBoyu;
                    newAmbar.OriginalKareX = ambar.OriginalKareX;
                    newAmbar.OriginalKareY = ambar.OriginalKareY;
                    newAmbar.OriginalKareEni = ambar.OriginalKareEni;
                    newAmbar.OriginalKareBoyu = ambar.OriginalKareBoyu;
                    newAmbar.Zoomlevel = ambar.Zoomlevel;

                    var conveyors = await (from x in context.Conveyors
                                           where x.AmbarId == newAmbar.AmbarId
                                           select x).ToListAsync();


                    foreach (var conveyor in conveyors)
                    {
                        Main.opcount += 0.1f;

                        if (Main.progressBar.Value < 100 && Main.opcount < 100)
                        {
                            progress.Report(Main.opcount);
                        }

                        Conveyor newConveyor = new Conveyor(conveyor.OriginalKareX, conveyor.OriginalKareY, conveyor.OriginalKareEni,
                            conveyor.OriginalKareBoyu, Main, null, newAmbar);

                        newConveyor.KareX = conveyor.KareX;
                        newConveyor.KareY = conveyor.KareY;
                        newConveyor.KareEni = conveyor.KareEni;
                        newConveyor.KareBoyu = conveyor.KareBoyu;
                        newConveyor.OriginalKareX = conveyor.OriginalKareX;
                        newConveyor.OriginalKareY = conveyor.OriginalKareY;
                        newConveyor.OriginalKareEni = conveyor.OriginalKareEni;
                        newConveyor.OriginalKareBoyu = conveyor.OriginalKareBoyu;
                        newConveyor.Parent = newAmbar;
                        newConveyor.ConveyorEni = conveyor.ConveyorEni;
                        newConveyor.ConveyorBoyu = conveyor.ConveyorBoyu;
                        newConveyor.ConveyorId = conveyor.ConveyorId;
                        newConveyor.AmbarId = conveyor.AmbarId;
                        newConveyor.Zoomlevel = conveyor.Zoomlevel;

                        newAmbar.conveyors.Add(newConveyor);

                        var reffPoints = await (from x in context.ConveyorReferencePoints
                                                where x.ConveyorId == newConveyor.ConveyorId
                                                select x).ToListAsync();

                        foreach (var reff in reffPoints)
                        {
                            Main.opcount += 0.1f;

                            if (Main.progressBar.Value < 100 && Main.opcount < 100)
                            {
                                progress.Report(Main.opcount);
                            }

                            ConveyorReferencePoint newReff = new ConveyorReferencePoint(reff.OriginalKareX, reff.OriginalKareY, reff.OriginalKareEni,
                                reff.OriginalKareBoyu, reff.Zoomlevel, Main, newConveyor, null);

                            newReff.KareX = reff.KareX;
                            newReff.KareY = reff.KareY;
                            newReff.KareEni = reff.KareEni;
                            newReff.KareBoyu = reff.KareBoyu;
                            newReff.OriginalKareX = reff.OriginalKareX;
                            newReff.OriginalKareY = reff.OriginalKareY;
                            newReff.OriginalKareEni = reff.OriginalKareEni;
                            newReff.OriginalKareBoyu = reff.OriginalKareBoyu;
                            newReff.FixedPointLocation = reff.FixedPointLocation;
                            newReff.ConveyorId = reff.ConveyorId;
                            newReff.ReferenceId = reff.ReferenceId;
                            newReff.Pointsize = reff.Pointsize;
                            newReff.LocationX = reff.LocationX;
                            newReff.LocationY = reff.LocationY;
                            newReff.OriginalLocationInsideParent = new PointF(reff.OriginalLocationInsideParentX, reff.OriginalLocationInsideParentY);

                            newConveyor.ConveyorReferencePoints.Add(newReff);
                        }
                    }

                    var depos = await (from x in context.Depos
                                       where x.AmbarId == newAmbar.AmbarId
                                       select x).ToListAsync();

                    foreach (var depo in depos)
                    {
                        Main.opcount += 0.1f;

                        if (Main.progressBar.Value < 100 && Main.opcount < 100)
                        {
                            progress.Report(Main.opcount);
                        }

                        Depo newDepo = new Depo(depo.OriginalKareX, depo.OriginalKareY, depo.OriginalKareEni, depo.OriginalKareBoyu,
                            depo.Zoomlevel, Main, null, newAmbar);

                        newDepo.DepoId = depo.DepoId;
                        newDepo.depo_alani_x = depo.depo_alani_x;
                        newDepo.depo_alani_y = depo.depo_alani_y;
                        newDepo.DepoAlaniEni = depo.DepoAlaniEni;
                        newDepo.DepoAlaniBoyu = depo.DepoAlaniBoyu;
                        newDepo.DepoAlaniYuksekligi = depo.DepoAlaniYuksekligi;
                        newDepo.DepoName = depo.DepoName;
                        newDepo.DepoDescription = depo.DepoDescription;
                        newDepo.AmbarId = depo.AmbarId;
                        newDepo.Cm_Height = depo.DepoAlaniBoyu * 100;
                        newDepo.Cm_Width = depo.DepoAlaniEni * 100;

                        newDepo.itemDrop_LeftRight = depo.itemDrop_LeftRight;
                        newDepo.asama1_Yuksekligi = depo.asama1_Yuksekligi;
                        newDepo.asama2_Yuksekligi = depo.asama2_Yuksekligi;
                        newDepo.itemDrop_StartLocation = depo.itemDrop_StartLocation;
                        newDepo.itemDrop_UpDown = depo.itemDrop_UpDown;
                        newDepo.Yerlestirilme_Sirasi = depo.Yerlestirilme_Sirasi;
                        newDepo.currentRow = depo.currentRow;
                        newDepo.currentColumn = depo.currentColumn;
                        newDepo.ColumnCount = depo.ColumnCount;
                        newDepo.RowCount = depo.RowCount;
                        newDepo.Depo_Alani_Eni_Cm = depo.Depo_Alani_Eni_Cm;
                        newDepo.Depo_Alani_Boyu_Cm = depo.Depo_Alani_Boyu_Cm;
                        newDepo.asama1_ItemSayisi = depo.asama1_ItemSayisi;
                        newDepo.asama2_ToplamItemSayisi = depo.asama2_ToplamItemSayisi;
                        newDepo.ItemTuru = depo.ItemTuru;
                        newDepo.currentStage = depo.currentStage;


                        newDepo.KareX = depo.KareX;
                        newDepo.KareY = depo.KareY;
                        newDepo.KareEni = depo.KareEni;
                        newDepo.KareBoyu = depo.KareBoyu;
                        newDepo.OriginalKareX = depo.OriginalKareX;
                        newDepo.OriginalKareY = depo.OriginalKareY;
                        newDepo.OriginalKareEni = depo.OriginalKareEni;
                        newDepo.OriginalKareBoyu = depo.OriginalKareBoyu;
                        newDepo.OriginalDepoSizeWidth = depo.OriginalDepoSizeWidth;
                        newDepo.OriginalDepoSizeHeight = depo.OriginalDepoSizeHeight;
                        newDepo.OriginalDepoSize = new SizeF(depo.OriginalDepoSizeWidth, depo.OriginalDepoSizeHeight);

                        newAmbar.depolar.Add(newDepo);

                        var cells = await (from x in context.Cells
                                           where x.DepoId == depo.DepoId
                                           select x).ToListAsync();

                        foreach (var cell in cells)
                        {
                            Main.opcount += 0.1f;

                            if (Main.progressBar.Value < 100 && Main.opcount < 100)
                            {
                                progress.Report(Main.opcount);
                            }

                            Models.Cell newCell = new Models.Cell(cell.OriginalKareX, cell.OriginalKareY, cell.OriginalKareEni, cell.OriginalKareBoyu,
                                Main, newDepo, null);

                            newCell.KareX = cell.KareX;
                            newCell.KareY = cell.KareY;
                            newCell.KareEni = cell.KareEni;
                            newCell.KareBoyu = cell.KareBoyu;
                            newCell.OriginalKareX = cell.OriginalKareX;
                            newCell.OriginalKareY = cell.OriginalKareY;
                            newCell.OriginalKareEni = cell.OriginalKareEni;
                            newCell.OriginalKareBoyu = cell.OriginalKareBoyu;

                            newCell.Parent = newDepo;
                            newCell.CellEni = cell.CellEni;
                            newCell.CellBoyu = cell.CellBoyu;
                            newCell.CellId = cell.CellId;
                            newCell.CellMalSayisi = cell.CellMalSayisi;
                            newCell.CellYuksekligi = cell.CellYuksekligi;
                            newCell.Column = cell.Column;
                            newCell.Row = cell.Row;
                            newCell.CellEtiketi = cell.CellEtiketi;
                            newCell.DepoId = cell.DepoId;
                            newCell.DikeyKenarBoslugu = cell.DikeyKenarBoslugu;
                            newCell.YatayKenarBoslugu = cell.YatayKenarBoslugu;
                            newCell.LocationofRect = cell.LocationofRect;
                            newCell.NesneEni = cell.NesneEni;
                            newCell.NesneBoyu = cell.NesneBoyu;
                            newCell.NesneYuksekligi = cell.NesneYuksekligi;
                            newCell.toplam_Nesne_Yuksekligi = cell.toplam_Nesne_Yuksekligi;
                            newCell.Zoomlevel = cell.Zoomlevel;
                            newCell.cell_Cm_X = cell.cell_Cm_X;
                            newCell.cell_Cm_Y = cell.cell_Cm_Y;

                            newDepo.nesneEni = cell.NesneEni;
                            newDepo.nesneBoyu = cell.NesneBoyu;
                            newDepo.nesneYuksekligi = cell.NesneYuksekligi;

                            newDepo.gridmaps.Add(newCell);

                            var items = await (from x in context.Items
                                               where x.CellId == newCell.CellId
                                               select x).ToListAsync();

                            foreach (var item in items)
                            {
                                Main.opcount += 0.1f;

                                if (Main.progressBar.Value < 100 && Main.opcount < 100)
                                {
                                    progress.Report(Main.opcount);
                                }

                                Models.Item newItem = new Models.Item(item.OriginalKareX, item.OriginalKareY, item.OriginalKareEni, item.OriginalKareBoyu, item.Zoomlevel, Main);

                                newItem.ItemEni = item.ItemEni;
                                newItem.ItemBoyu = item.ItemBoyu;
                                newItem.ItemEtiketi = item.ItemEtiketi;
                                newItem.ItemId = item.ItemId;
                                newItem.ItemTuru = item.ItemTuru;
                                newItem.ItemAciklamasi = item.ItemAciklamasi;
                                newItem.ItemAgirligi = item.ItemAgirligi;
                                newItem.ItemYuksekligi = item.ItemYuksekligi;
                                newItem.KareX = item.KareX;
                                newItem.KareY = item.KareY;
                                newItem.KareEni = item.KareEni;
                                newItem.KareBoyu = item.KareBoyu;
                                newItem.OriginalKareX = item.OriginalKareX;
                                newItem.OriginalKareY = item.OriginalKareY;
                                newItem.OriginalKareEni = item.OriginalKareEni;
                                newItem.OriginalKareBoyu = item.OriginalKareBoyu;
                                newItem.LocationofRect = item.LocationofRect;
                                newItem.CellId = item.CellId;
                                newItem.Cm_X_Axis = item.Cm_X_Axis;
                                newItem.Cm_Y_Axis = item.Cm_Y_Axis;
                                newItem.Cm_Z_Axis = item.Cm_Z_Axis;

                                newCell.items.Add(newItem);

                                var reffPoints = await (from x in context.ItemReferencePoints
                                                        where x.ItemId == newItem.ItemId
                                                        select x).ToListAsync();

                                foreach (var reff in reffPoints)
                                {
                                    Main.opcount += 0.1f;

                                    if (Main.progressBar.Value < 100 && Main.opcount < 100)
                                    {
                                        progress.Report(Main.opcount);
                                    }

                                    ItemReferencePoint newReff = new ItemReferencePoint(reff.OriginalKareX, reff.OriginalKareY, reff.OriginalKareEni,
                                        reff.OriginalKareBoyu, reff.Zoomlevel, Main);

                                    newReff.KareX = reff.KareX;
                                    newReff.KareY = reff.KareY;
                                    newReff.KareEni = reff.KareEni;
                                    newReff.KareBoyu = reff.KareBoyu;
                                    newReff.OriginalKareX = reff.OriginalKareX;
                                    newReff.OriginalKareY = reff.OriginalKareY;
                                    newReff.OriginalKareEni = reff.OriginalKareEni;
                                    newReff.OriginalKareBoyu = reff.OriginalKareBoyu;
                                    newReff.LocationofRect = reff.LocationofRect;
                                    newReff.ReferenceId = reff.ReferenceId;
                                    newReff.Pointsize = reff.Pointsize;

                                    newItem.ItemReferencePoints.Add(newReff);
                                }
                            }
                        }
                    }
                    return newAmbar;
                }
                return null;
            }
        }

        public void CreatePictureBox(Ambar ambar)
        {
            using (var context = new DBContext())
            {
                var Layout = (from x in context.Layout
                              where x.LayoutId == ambar.LayoutId
                              select x).FirstOrDefault();

                if (Layout != null)
                {
                    Panel panel = new Panel();
                    panel.Size = new System.Drawing.Size(750, 750);
                    panel.AutoScroll = true;
                    panel.BackColor = System.Drawing.Color.AliceBlue;
                    panel.BorderStyle = BorderStyle.Fixed3D;

                    PictureBox pictureBox = new PictureBox();
                    pictureBox.Size = new System.Drawing.Size(550, 500);
                    pictureBox.Location = new System.Drawing.Point(100, 50);
                    pictureBox.BorderStyle = BorderStyle.FixedSingle;
                    GVisual.SetDoubleBuffered(pictureBox);

                    System.Windows.Forms.ToolTip tooltip = new System.Windows.Forms.ToolTip();

                    tooltip.SetToolTip(pictureBox, "Layout'u yüklemek için çift tıklayın.\nSilmek için sağ tıklayın.");

                    tooltip.AutoPopDelay = 5000;
                    tooltip.InitialDelay = 100;
                    tooltip.ReshowDelay = 100;
                    tooltip.ShowAlways = true;

                    Krypton.Toolkit.KryptonWrapLabel LayoutTitle = new Krypton.Toolkit.KryptonWrapLabel();
                    LayoutTitle.MaximumSize = new System.Drawing.Size(300, 30);
                    LayoutTitle.MinimumSize = new System.Drawing.Size(100, 30);
                    LayoutTitle.AccessibleName = "Layout Name";
                    LayoutTitle.TextAlign = ContentAlignment.MiddleCenter;
                    LayoutTitle.AutoSize = true;
                    LayoutTitle.Text = $"{Layout.Name}";
                    LayoutTitle.Location = new System.Drawing.Point(pictureBox.Left + (pictureBox.Width / 2 - LayoutTitle.Width / 2), 10);
                    LayoutTitle.StateCommon.TextColor = System.Drawing.Color.Red;
                    LayoutTitle.StateCommon.Font = new System.Drawing.Font("Arial", 16);

                    System.Windows.Forms.RichTextBox LayoutDesc = new System.Windows.Forms.RichTextBox();

                    LayoutDesc.Size = new System.Drawing.Size(450, 180);
                    LayoutDesc.MaximumSize = new System.Drawing.Size(450, 180);
                    LayoutDesc.MinimumSize = new System.Drawing.Size(300, 100);
                    LayoutDesc.AccessibleName = "Layout Desc";
                    LayoutDesc.ReadOnly = true;
                    LayoutDesc.WordWrap = true;
                    LayoutDesc.Dock = DockStyle.Fill;
                    LayoutDesc.BackColor = System.Drawing.Color.Azure;
                    LayoutDesc.Text = $"{Layout.Description}";
                    LayoutDesc.ForeColor = System.Drawing.Color.Black;
                    LayoutDesc.Font = new System.Drawing.Font("Arial", 12);

                    Panel panelDescription = new Panel();
                    panelDescription.Size = new System.Drawing.Size(LayoutDesc.Width, LayoutDesc.Height);
                    //panelDescription.AutoScroll = true;
                    panelDescription.BackColor = System.Drawing.Color.Azure;
                    panelDescription.BorderStyle = BorderStyle.Fixed3D;
                    panelDescription.Location = new System.Drawing.Point(pictureBox.Left + (pictureBox.Width / 2 - panelDescription.Width / 2), pictureBox.Bottom);



                    ambar.SelectLayoutRectangle = GVisual.RatioRectangleBetweenTwoParentRectangles(ambar.Rectangle, DrawingPanel.ClientRectangle, pictureBox.ClientRectangle);
                    float BeforeX = ambar.SelectLayoutRectangle.X;

                    ambar.SelectLayoutRectangle = GVisual.CenterRectangletoParentRectangle(ambar.SelectLayoutRectangle, pictureBox.ClientRectangle);

                    float AfterX = ambar.SelectLayoutRectangle.X;

                    float MoveX = BeforeX - AfterX;

                    foreach (var conveyor in ambar.conveyors)
                    {
                        conveyor.SelectLayoutRectangle = GVisual.RatioRectangleBetweenTwoParentRectangles(conveyor.Rectangle, DrawingPanel.ClientRectangle, pictureBox.ClientRectangle);

                        conveyor.SelectLayoutRectangle = new RectangleF(conveyor.SelectLayoutRectangle.X - MoveX, conveyor.SelectLayoutRectangle.Y, conveyor.SelectLayoutRectangle.Width,
                        conveyor.SelectLayoutRectangle.Height);

                        foreach (var reff in conveyor.ConveyorReferencePoints)
                        {
                            reff.SelectLayoutRectangle = GVisual.RatioRectangleBetweenTwoParentRectangles(reff.Rectangle, DrawingPanel.ClientRectangle, pictureBox.ClientRectangle);

                            reff.SelectLayoutRectangle = new RectangleF(reff.SelectLayoutRectangle.X - MoveX, reff.SelectLayoutRectangle.Y, reff.SelectLayoutRectangle.Width,
                        reff.SelectLayoutRectangle.Height);

                        }
                    }

                    foreach (var depo in ambar.depolar)
                    {
                        depo.SelectLayoutRectangle = GVisual.RatioRectangleBetweenTwoParentRectangles(depo.Rectangle, DrawingPanel.ClientRectangle, pictureBox.ClientRectangle);

                        depo.SelectLayoutRectangle = new RectangleF(depo.SelectLayoutRectangle.X - MoveX, depo.SelectLayoutRectangle.Y, depo.SelectLayoutRectangle.Width,
                        depo.SelectLayoutRectangle.Height);

                        foreach (var cell in depo.gridmaps)
                        {
                            cell.SelectLayoutRectangle = GVisual.RatioRectangleBetweenTwoParentRectangles(cell.Rectangle, DrawingPanel.ClientRectangle, pictureBox.ClientRectangle);

                            cell.SelectLayoutRectangle = new RectangleF(cell.SelectLayoutRectangle.X - MoveX, cell.SelectLayoutRectangle.Y, cell.SelectLayoutRectangle.Width,
                       cell.SelectLayoutRectangle.Height);


                            foreach (var item in cell.items)
                            {
                                item.SelectLayoutRectangle = GVisual.RatioRectangleBetweenTwoParentRectangles(item.Rectangle, DrawingPanel.ClientRectangle, pictureBox.ClientRectangle);

                                item.SelectLayoutRectangle = new RectangleF(item.SelectLayoutRectangle.X - MoveX, item.SelectLayoutRectangle.Y, item.SelectLayoutRectangle.Width,
                       item.SelectLayoutRectangle.Height);

                                item.SelectLayoutRectangle = GVisual.CenterRectangletoParentRectangle(item.SelectLayoutRectangle, cell.SelectLayoutRectangle);
                            }
                        }
                    }



                    panel.Controls.Add(LayoutTitle);
                    panelDescription.Controls.Add(LayoutDesc);
                    panel.Controls.Add(panelDescription);
                    panel.Controls.Add(pictureBox);
                    SelectLayoutPanel.Controls.Add(panel);
                    pictureBox.Paint += (sender, e) => Picture_Box_Paint(sender, e);
                    pictureBox.Tag = ambar;
                    pictureBox.DoubleClick += (sender, e) => PictureBox_MouseDoubleClick(sender, e);
                    pictureBox.MouseDown += (sender, e) => PictureBox_MouseDown(sender, e);
                    pictureBox.MouseEnter += (sender, e) => PictureBox_MouseEnter(sender, e);
                    pictureBox.MouseLeave += (sender, e) => PictureBox_MouseLeave(sender, e);
                }
            }
        }

        private void PictureBox_MouseLeave(object sender, EventArgs e)
        {
            PictureBox pb = sender as PictureBox;

            if (pb != null)
            {
                if (SelectedPB == null)
                {
                    pb.BackColor = System.Drawing.Color.White;
                }
            }
        }

        private void PictureBox_MouseEnter(object sender, EventArgs e)
        {
            PictureBox pb = sender as PictureBox;

            if (pb != null)
            {
                if (SelectedPB == null)
                {
                    pb.BackColor = System.Drawing.Color.LightCyan;
                }
            }
        }

        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            PictureBox pictureBox = sender as PictureBox;

            if (pictureBox != null)
            {
                if (e.Button == MouseButtons.Right)
                {
                    contextMenuStrip.Show(Cursor.Position);
                    SelectedPB = pictureBox;
                }
                if (e.Button == MouseButtons.Left)
                {
                    MakePBsBackColorsWhite();
                    
                    SelectedPB = pictureBox;
                    SelectedPB.BackColor = System.Drawing.Color.LightCyan;

                    SetLayoutDescandNametoTextBoxes(SelectLayoutPanel, pictureBox, lbl_LayoutMenu_Title, txt_ChangeLayoutName, txt_ChangeLayoutDescription);

                    string layoutName = getLayoutNameFromPanel(SelectLayoutPanel, pictureBox, true);
                    
                    OpenRightSide((Ambar)pictureBox.Tag);
                    ScrollPanelIntoView(pictureBox, SelectLayoutPanel);
                    
                    SelectLayoutPanel.ScrollControlIntoView(SelectedPB);
                    
                    lbl_Layout_Sec_Title.Location = SmallTitleLocation;
                    lbl_Layout_Sec_Title.Text = $"{layoutName}";
                }
            }
        }

        private void MakePBsBackColorsWhite()
        {
            foreach (var control in SelectLayoutPanel.Controls)
            {
                if (control is Panel)
                {
                    Panel panel = (Panel)control;

                    foreach (var control1 in panel.Controls)
                    {
                        if (control1 is PictureBox)
                        {
                            PictureBox pb = (PictureBox)control1;

                            pb.BackColor = System.Drawing.Color.White;
                        }
                    }
                }
            }
        }

        private void ScrollPanelIntoView(PictureBox pb, FlowLayoutPanel FlowPanel)
        {
            foreach (var control in FlowPanel.Controls)
            {
                if (control is Panel)
                {
                    Panel panel = (Panel)control;

                    if (panel.Controls.Contains(pb))
                    {
                        FlowPanel.ScrollControlIntoView(panel);
                    }
                }
            }
        }

        private void SetLayoutDescandNametoTextBoxes(FlowLayoutPanel FlowPanel, PictureBox pictureBox, KryptonWrapLabel? lbl_ToWriteLayoutName, System.Windows.Forms.TextBox? txt_ToWriteLayoutName, System.Windows.Forms.TextBox? txt_ToWriteLayoutDesc)
        {
            foreach (var panel in FlowPanel.Controls)
            {
                if (panel is Panel)
                {
                    Panel panel1 = (Panel)panel;

                    if (panel1.Controls.Contains(pictureBox))
                    {
                        foreach (var control in panel1.Controls)
                        {
                            if (control is Krypton.Toolkit.KryptonWrapLabel)
                            {
                                Krypton.Toolkit.KryptonWrapLabel txt = (KryptonWrapLabel)(control);

                                if (txt.AccessibleName == "Layout Name")
                                {
                                    if (lbl_ToWriteLayoutName != null && txt_ToWriteLayoutName != null)
                                    {
                                        lbl_ToWriteLayoutName.Text = txt.Text;
                                        txt_ToWriteLayoutName.Text = txt.Text;
                                    }
                                    else if (lbl_ToWriteLayoutName != null && txt_ToWriteLayoutName == null)
                                    {
                                        lbl_ToWriteLayoutName.Text = txt.Text;
                                    }
                                    else if (txt_ToWriteLayoutName != null && lbl_ToWriteLayoutName == null)
                                    {
                                        txt_ToWriteLayoutName.Text = txt.Text;
                                    }
                                }
                            }
                            else if (control is Panel)
                            {
                                Panel descPanel = (Panel)control;

                                foreach (var txt in descPanel.Controls)
                                {
                                    if (txt is System.Windows.Forms.RichTextBox)
                                    {
                                        System.Windows.Forms.RichTextBox txt1 = (System.Windows.Forms.RichTextBox)txt;

                                        if (txt1.AccessibleName == "Layout Desc")
                                        {
                                            if (txt_ToWriteLayoutDesc != null)
                                            {
                                                txt_ToWriteLayoutDesc.Text = txt1.Text;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void PictureBox_MouseDoubleClick(object sender, EventArgs e)
        {
            PictureBox pictureBox = sender as PictureBox;

            if (pictureBox != null)
            {
                SelectedPB = new PictureBox();
                SelectedPB = pictureBox;
                this.DialogResult = DialogResult.OK;
            }
        }

        private void Picture_Box_Paint(object? sender, PaintEventArgs e)
        {
            int counter = 0;
            System.Drawing.Font font = new System.Drawing.Font("Arial", 8);
            SolidBrush brush = new SolidBrush(System.Drawing.Color.Red);
            RectangleF rect = new RectangleF();
            Graphics g = e.Graphics;
            PictureBox pictureBox = sender as PictureBox;
            Ambar ambar = (Ambar)pictureBox.Tag;
            System.Drawing.Point mousePosition = pictureBox.PointToClient(Cursor.Position);
            System.Drawing.Point point = new System.Drawing.Point(pictureBox.ClientRectangle.Left, pictureBox.ClientRectangle.Top);

            g.DrawRectangle(LayoutPen, ambar.SelectLayoutRectangle);

            System.Drawing.Font font1 = new System.Drawing.Font("Arial", 8);
            SolidBrush brush1 = new SolidBrush(System.Drawing.Color.Red);

            string layoutRectangle = $"ambar SelectLayoutRectangle: {ambar.SelectLayoutRectangle}";

            g.DrawString(layoutRectangle, font1, brush1, new System.Drawing.Point(point.X, point.Y + 20));

            foreach (var depo in ambar.depolar)
            {
                g.DrawRectangle(LayoutPen, depo.SelectLayoutRectangle);

                foreach (var cell in depo.gridmaps)
                {
                    g.DrawRectangle(LayoutPen, cell.SelectLayoutRectangle);

                    foreach (var item in cell.items)
                    {
                        g.DrawRectangle(LayoutPen, item.SelectLayoutRectangle);
                        counter++;
                        rect = item.SelectLayoutRectangle;
                    }

                    if (cell.items.Count > 0)
                    {
                        string text = $"{counter}";
                        SizeF textSize = g.MeasureString(text, font);
                        g.DrawString(text, font, brush, new PointF(rect.X +
                            (rect.Width / 2 - textSize.Width / 2),
                            rect.Y + (rect.Height / 2 - textSize.Height / 2)));
                        counter = 0;
                    }
                }
            }

            foreach (var conveyor in ambar.conveyors)
            {
                g.DrawRectangle(LayoutPen, conveyor.SelectLayoutRectangle);

                foreach (var reff in conveyor.ConveyorReferencePoints)
                {
                    g.DrawRectangle(new Pen(System.Drawing.Color.Red), reff.SelectLayoutRectangle);
                    g.FillRectangle(new SolidBrush(System.Drawing.Color.Red), reff.SelectLayoutRectangle);
                }
            }
        }

        private void SelectLayoutPanel_Scroll(object sender, ScrollEventArgs e)
        {
            foreach (var pictureBox in SelectLayoutPanel.Controls)
            {
                if (pictureBox is PictureBox)
                {
                    PictureBox pb = new PictureBox();
                    pb = (PictureBox)pictureBox;
                    pb.Invalidate();
                }
            }
        }

        private void layoutuSilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<System.Windows.Forms.Control> panels = new List<System.Windows.Forms.Control>();
            using (var context = new DBContext())
            {
                Ambar ambar = (Ambar)SelectedPB.Tag;
                var layout = (from x in context.Layout
                              where x.LayoutId == ambar.LayoutId
                              select x).FirstOrDefault();

                var firstResult = MessageBox.Show($"{layout.Name} isimli Layout'u silmek istiyor musunuz?",
                    "Devam etmek istiyor musunuz?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (firstResult == DialogResult.Yes)
                {
                    if (ambar != null)
                    {
                        if (Main.ambar != null)
                        {
                            if (ambar.AmbarId == Main.ambar.AmbarId)
                            {
                                var result = MessageBox.Show("Şu an yüklü olan Layout'u silmek istiyor musunuz?",
                                    "Devam etmek istiyor musunuz?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                                if (result == DialogResult.Yes)
                                {
                                    if (layout != null)
                                    {
                                        context.Layout.Remove(layout);
                                        context.SaveChanges();
                                    }

                                    foreach (Panel panel in SelectLayoutPanel.Controls)
                                    {
                                        foreach (var pb in panel.Controls)
                                        {
                                            if (pb == SelectedPB)
                                            {
                                                panels.Add(panel);
                                            }
                                        }
                                    }
                                    foreach (var panel in panels)
                                    {
                                        SelectLayoutPanel.Controls.Remove(panel);
                                        SelectedPB = null;
                                        Main.ambar = null;
                                        if (Main.infopanel.Visible)
                                        {
                                            GVisual.HideControl(Main.infopanel, Main.DrawingPanel);
                                        }
                                        Main.DrawingPanel.Invalidate();
                                    }
                                }
                            }
                            else
                            {
                                if (layout != null)
                                {
                                    context.Layout.Remove(layout);
                                    context.SaveChanges();
                                }

                                foreach (Panel panel in SelectLayoutPanel.Controls)
                                {
                                    foreach (var pb in panel.Controls)
                                    {
                                        if (pb == SelectedPB)
                                        {
                                            panels.Add(panel);
                                        }
                                    }
                                }
                                foreach (var panel in panels)
                                {
                                    SelectLayoutPanel.Controls.Remove(panel);
                                    SelectedPB = null;
                                }
                            }
                        }
                        else
                        {
                            if (layout != null)
                            {
                                context.Layout.Remove(layout);
                                context.SaveChanges();
                            }

                            foreach (Panel panel in SelectLayoutPanel.Controls)
                            {
                                foreach (var pb in panel.Controls)
                                {
                                    if (pb == SelectedPB)
                                    {
                                        panels.Add(panel);
                                    }
                                }
                            }
                            foreach (var panel in panels)
                            {
                                SelectLayoutPanel.Controls.Remove(panel);
                                SelectedPB = null;
                            }
                        }
                    }
                }
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (LayoutCount > 0)
            {
                int step = 7;

                totalProgress += step;

                if (progressBar.Value < 100 - step)
                {
                    progressBar.Value = totalProgress;
                }

                int pbCount = 0;
                foreach (System.Windows.Forms.Control PB in SelectLayoutPanel.Controls)
                {
                    if (PB is Panel)
                    {
                        pbCount++;
                    }
                }

                if (pbCount == LayoutCount)
                {
                    progressBar.Value = 100;
                    SelectLayoutPanel.Controls.Remove(progressBarPanel);
                    timer.Stop();
                    progressBar.Value = 0;
                    GVisual.HideControl(progressBarPanel, this);
                    GVisual.ShowControl(SelectLayoutPanel, this, PointSelectLayoutPanel);
                }
            }
        }

        private void OpenRightSide(Ambar? ambar)
        {
            SelectLayoutPanel.Size = MainPanelSmallSize;
            GVisual.ShowControl(panel_LayoutMenu, this, rightPanelLocation);

            foreach (var pb in SelectLayoutPanel.Controls)
            {
                if (pb is Panel)
                {
                    Panel pb1 = pb as Panel;

                    if (pb1 != null)
                    {
                        System.Drawing.Size size = new System.Drawing.Size(pb1.Width,
                            pb1.Height);

                        pb1.Size = new System.Drawing.Size(600, 600);

                        if (ambar != null)
                        {
                            EnlargeorShrinkInsidePanel(pb1, size, pb1.Size, ambar);
                        }
                    }
                }
            }
        }

        private void CloseRightSide(Ambar? ambar)
        {
            SelectLayoutPanel.Size = MainPanelLargeSize;
            GVisual.HideControl(panel_LayoutMenu, this);

            foreach (var pb in SelectLayoutPanel.Controls)
            {
                if (pb is Panel)
                {
                    Panel pb1 = pb as Panel;

                    if (pb1 != null)
                    {
                        System.Drawing.Size size = new System.Drawing.Size(pb1.Width,
                            pb1.Height);

                        pb1.Size = new System.Drawing.Size(750, 750);

                        if (ambar != null)
                        {
                            EnlargeorShrinkInsidePanel(pb1, size, pb1.Size, ambar);
                        }
                    }
                }
            }
        }

        private void SelectLayoutPanel_MouseDown(object sender, MouseEventArgs e)
        {
            bool isinside = false;
            foreach (var pb in SelectLayoutPanel.Controls)
            {
                if (pb is Panel)
                {
                    Panel pb1 = pb as Panel;

                    if (!pb1.ClientRectangle.Contains(e.Location))
                    {
                        isinside = true;
                    }

                    foreach (var control in pb1.Controls)
                    {
                        if (control is PictureBox)
                        {
                            PictureBox picture = control as PictureBox;

                            Ambar ambar = (Ambar)picture.Tag;

                            if (isinside)
                            {
                                CloseRightSide(ambar);
                                lbl_Layout_Sec_Title.Location = LargeTitleLocation;
                                lbl_Layout_Sec_Title.Text = $"Layout Seçin";
                                isinside = false;
                            }
                            picture.BackColor = System.Drawing.Color.White;
                            SelectedPB = null;
                            picture.Invalidate();
                            picture.Update();
                        }
                    }
                }
            }
        }

        private void EnlargeorShrinkInsidePanel(Panel panel, System.Drawing.Size before, System.Drawing.Size after, Ambar? ambar)
        {
            float widthRatio = (float)after.Width / before.Width;
            float heightRatio = (float)after.Height / before.Height;

            foreach (System.Windows.Forms.Control control in panel.Controls)
            {
                int newWidth = (int)(control.Width * widthRatio);
                int newHeight = (int)(control.Height * heightRatio);

                control.Size = new System.Drawing.Size(newWidth, newHeight);

                // Optional: Adjust control position based on new panel size
                int newX = (int)(control.Location.X * widthRatio);
                int newY = (int)(control.Location.Y * heightRatio);

                control.Location = new System.Drawing.Point(newX, newY);

                if (control is PictureBox)
                {
                    PictureBox picture = control as PictureBox;

                    Ambar ambar1 = (Ambar)picture.Tag;

                    ScaleAmbar(ambar1, widthRatio, heightRatio);
                    picture.Invalidate();
                    picture.Update();
                }
            }
        }

        private void ScaleAmbar(Ambar? ambar, float widthRatio, float heightRatio)
        {
            if (ambar != null)
            {
                ambar.SelectLayoutRectangle = new System.Drawing.RectangleF(ambar.SelectLayoutRectangle.X * widthRatio, ambar.SelectLayoutRectangle.Y * heightRatio, ambar.SelectLayoutRectangle.Width * widthRatio, ambar.SelectLayoutRectangle.Height * heightRatio);

                foreach (var depo in ambar.depolar)
                {
                    depo.SelectLayoutRectangle = new System.Drawing.RectangleF(depo.SelectLayoutRectangle.X * widthRatio, depo.SelectLayoutRectangle.Y * heightRatio, depo.SelectLayoutRectangle.Width * widthRatio, depo.SelectLayoutRectangle.Height * heightRatio);

                    foreach (var cell in depo.gridmaps)
                    {
                        cell.SelectLayoutRectangle = new System.Drawing.RectangleF(cell.SelectLayoutRectangle.X * widthRatio, cell.SelectLayoutRectangle.Y * heightRatio, cell.SelectLayoutRectangle.Width * widthRatio, cell.SelectLayoutRectangle.Height * heightRatio);

                        foreach (var item in cell.items)
                        {
                            item.SelectLayoutRectangle = new System.Drawing.RectangleF(item.SelectLayoutRectangle.X * widthRatio, item.SelectLayoutRectangle.Y * heightRatio, item.SelectLayoutRectangle.Width * widthRatio, item.SelectLayoutRectangle.Height * heightRatio);
                        }
                    }
                }
                foreach (var conveyor in ambar.conveyors)
                {
                    conveyor.SelectLayoutRectangle = new System.Drawing.RectangleF(conveyor.SelectLayoutRectangle.X * widthRatio, conveyor.SelectLayoutRectangle.Y * heightRatio, conveyor.SelectLayoutRectangle.Width * widthRatio, conveyor.SelectLayoutRectangle.Height * heightRatio);

                    foreach (var reff in conveyor.ConveyorReferencePoints)
                    {
                        reff.SelectLayoutRectangle = new System.Drawing.RectangleF(reff.SelectLayoutRectangle.X * widthRatio, reff.SelectLayoutRectangle.Y * heightRatio, reff.SelectLayoutRectangle.Width * widthRatio, reff.SelectLayoutRectangle.Height * heightRatio);
                    }
                }
            }
        }

        private async void btn_Layout_Duzenle_Click(object sender, EventArgs e)
        {
            bool isDepoEmpty = true;

            if (SelectedPB != null)
            {
                Ambar ambar = (Ambar)SelectedPB.Tag;

                if (ambar != null)
                {
                    BeforeAmbar = ambar.Clone();
                    LayoutOlusturma ?layout = null;
                    

                    foreach (var depo in ambar.depolar)
                    {
                        foreach (var cell in depo.gridmaps)
                        {
                            if (cell.items.Count > 0)
                            {
                                isDepoEmpty = false;
                            }
                        }
                    }

                    

                    if (isDepoEmpty)
                    {
                        using (var context = new DBContext())
                        {
                            var layout1 = (from x in context.Layout
                                           where x.LayoutId == ambar.LayoutId
                                           select x).FirstOrDefault();

                            if (layout1 != null)
                            {
                                layout = new LayoutOlusturma(Main, ambar, layout1);

                                ambar.layout = layout;
                                layout.AlanNode.Tag = ambar;
                                foreach (var depo in ambar.depolar)
                                {
                                    depo.layout = layout;
                                    depo.layout.izgaraHaritasiOlustur += depo.Depo_IzgaraHaritasiOlustur;
                                    layout.AddDepoNode(depo);
                                    foreach (var cell in depo.gridmaps)
                                    {
                                        cell.Layout = layout;
                                    }
                                }
                                foreach (var conveyor in ambar.conveyors)
                                {
                                    conveyor.layout = layout;
                                    layout.AddConveyorNode(conveyor);
                                    foreach (var reff in conveyor.ConveyorReferencePoints)
                                    {
                                        reff.Layout = layout;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        CustomNotifyIcon notify = new CustomNotifyIcon();
                        notify.showAlert("İçinde Nesneler olan bir layout'u değiştiremezsiniz.", CustomNotifyIcon.enmType.Warning);
                    }

                    if (layout != null)
                    {
                        if (layout.ShowDialog() == DialogResult.OK)
                        {
                            // Proceed with applying the layout changes
                            ambar.SelectLayoutRectangle = GVisual.RatioRectangleBetweenTwoParentRectangles(ambar.Rectangle, layout.drawingPanel.ClientRectangle, SelectedPB.ClientRectangle);
                            float BeforeX = ambar.SelectLayoutRectangle.X;
                            ambar.SelectLayoutRectangle = GVisual.CenterRectangletoParentRectangle(ambar.SelectLayoutRectangle, SelectedPB.ClientRectangle);
                            float AfterX = ambar.SelectLayoutRectangle.X;
                            float MoveX = BeforeX - AfterX;

                            foreach (var conveyor in ambar.conveyors)
                            {
                                conveyor.SelectLayoutRectangle = GVisual.RatioRectangleBetweenTwoParentRectangles(conveyor.Rectangle, layout.drawingPanel.ClientRectangle, SelectedPB.ClientRectangle);
                                conveyor.SelectLayoutRectangle = new RectangleF(conveyor.SelectLayoutRectangle.X - MoveX, conveyor.SelectLayoutRectangle.Y, conveyor.SelectLayoutRectangle.Width, conveyor.SelectLayoutRectangle.Height);

                                foreach (var reff in conveyor.ConveyorReferencePoints)
                                {
                                    reff.SelectLayoutRectangle = GVisual.RatioRectangleBetweenTwoParentRectangles(reff.Rectangle, layout.drawingPanel.ClientRectangle, SelectedPB.ClientRectangle);
                                    reff.SelectLayoutRectangle = new RectangleF(reff.SelectLayoutRectangle.X - MoveX, reff.SelectLayoutRectangle.Y, reff.SelectLayoutRectangle.Width, reff.SelectLayoutRectangle.Height);
                                }
                            }

                            foreach (var depo in ambar.depolar)
                            {
                                depo.SelectLayoutRectangle = GVisual.RatioRectangleBetweenTwoParentRectangles(depo.Rectangle, layout.drawingPanel.ClientRectangle, SelectedPB.ClientRectangle);
                                depo.SelectLayoutRectangle = new RectangleF(depo.SelectLayoutRectangle.X - MoveX, depo.SelectLayoutRectangle.Y, depo.SelectLayoutRectangle.Width, depo.SelectLayoutRectangle.Height);

                                foreach (var cell in depo.gridmaps)
                                {
                                    cell.SelectLayoutRectangle = GVisual.RatioRectangleBetweenTwoParentRectangles(cell.Rectangle, layout.drawingPanel.ClientRectangle, SelectedPB.ClientRectangle);
                                    cell.SelectLayoutRectangle = new RectangleF(cell.SelectLayoutRectangle.X - MoveX, cell.SelectLayoutRectangle.Y, cell.SelectLayoutRectangle.Width, cell.SelectLayoutRectangle.Height);

                                    foreach (var item in cell.items)
                                    {
                                        item.SelectLayoutRectangle = GVisual.RatioRectangleBetweenTwoParentRectangles(item.Rectangle, layout.drawingPanel.ClientRectangle, SelectedPB.ClientRectangle);
                                        item.SelectLayoutRectangle = new RectangleF(item.SelectLayoutRectangle.X - MoveX, item.SelectLayoutRectangle.Y, item.SelectLayoutRectangle.Width, item.SelectLayoutRectangle.Height);
                                    }
                                }
                            }

                            using (var context = new DBContext())
                            {
                                var layout1 = (from x in context.Layout
                                               where x.LayoutId == ambar.LayoutId
                                               select x).FirstOrDefault();

                                if (layout1 != null)
                                {
                                    if (layout.LayoutName != layout1.Name)
                                    {
                                        layout1.Name = layout.LayoutName;
                                    }
                                    if (layout.LayoutDescription != layout1.Description)
                                    {
                                        layout1.Description = layout.LayoutDescription;
                                    }
                                    context.SaveChanges();
                                    setLayoutNameDesc(SelectLayoutPanel, SelectedPB, true, layout1.Name);
                                    setLayoutNameDesc(SelectLayoutPanel, SelectedPB, false, layout1.Description);

                                    var progress1 = new Progress<int>(value =>
                                    {
                                        progressBar.Value = value;
                                    });

                                    await Main.LayoutOlusturSecondDatabaseOperation(progress1, layout1.Name, layout1.Description, layout1, ambar);
                                }
                            }
                            
                            SelectedPB.Invalidate();
                        }
                        else if(layout.DialogResult == DialogResult.Cancel)
                        {
                            ambar = BeforeAmbar.Clone();
                            SelectedPB.Tag = null;
                            SelectedPB.Tag = ambar;
                            SelectedPB.Invalidate();
                        }
                    }
                }
            }
        }

        private void btn_Sil_Click(object sender, EventArgs e)
        {
            List<System.Windows.Forms.Control> panels = new List<System.Windows.Forms.Control>();
            using (var context = new DBContext())
            {
                Ambar ambar = (Ambar)SelectedPB.Tag;
                var layout = (from x in context.Layout
                              where x.LayoutId == ambar.LayoutId
                              select x).FirstOrDefault();

                var firstResult = MessageBox.Show($"{layout.Name} isimli Layout'u silmek istiyor musunuz?",
                    "Devam etmek istiyor musunuz?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (firstResult == DialogResult.Yes)
                {
                    if (ambar != null)
                    {
                        if (Main.ambar != null)
                        {
                            if (ambar.AmbarId == Main.ambar.AmbarId)
                            {
                                var result = MessageBox.Show("Şu an yüklü olan Layout'u silmek istiyor musunuz?",
                                    "Devam etmek istiyor musunuz?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                                if (result == DialogResult.Yes)
                                {
                                    if (layout != null)
                                    {
                                        context.Layout.Remove(layout);
                                        context.SaveChanges();
                                    }

                                    foreach (Panel panel in SelectLayoutPanel.Controls)
                                    {
                                        foreach (var pb in panel.Controls)
                                        {
                                            if (pb == SelectedPB)
                                            {
                                                panels.Add(panel);
                                            }
                                        }
                                    }
                                    foreach (var panel in panels)
                                    {
                                        SelectLayoutPanel.Controls.Remove(panel);
                                        SelectedPB = null;
                                        Main.ambar = null;
                                        if (Main.infopanel.Visible)
                                        {
                                            GVisual.HideControl(Main.infopanel, Main.DrawingPanel);
                                        }
                                        Main.DrawingPanel.Invalidate();
                                    }
                                    CloseRightSide(ambar);
                                }
                            }
                            else
                            {
                                if (layout != null)
                                {
                                    context.Layout.Remove(layout);
                                    context.SaveChanges();
                                }

                                foreach (Panel panel in SelectLayoutPanel.Controls)
                                {
                                    foreach (var pb in panel.Controls)
                                    {
                                        if (pb == SelectedPB)
                                        {
                                            panels.Add(panel);
                                        }
                                    }
                                }
                                foreach (var panel in panels)
                                {
                                    SelectLayoutPanel.Controls.Remove(panel);
                                    SelectedPB = null;
                                }
                                CloseRightSide(ambar);
                            }
                        }
                        else
                        {
                            if (layout != null)
                            {
                                context.Layout.Remove(layout);
                                context.SaveChanges();
                            }

                            foreach (Panel panel in SelectLayoutPanel.Controls)
                            {
                                foreach (var pb in panel.Controls)
                                {
                                    if (pb == SelectedPB)
                                    {
                                        panels.Add(panel);
                                    }
                                }
                            }
                            foreach (var panel in panels)
                            {
                                SelectLayoutPanel.Controls.Remove(panel);
                                SelectedPB = null;
                            }
                            CloseRightSide(ambar);
                        }
                    }
                }
            }
        }

        private void txt_ChangeLayoutName_TextChanged(object sender, EventArgs e)
        {
            if (this.Controls.Contains(panel_LayoutMenu))
            {
                GVisual.ShowControl(btn_ChangeLayoutName, InnerPanel1);
            }

            if (txt_ChangeLayoutName.Text.Length == 0)
            {
                GVisual.HideControl(btn_ChangeLayoutName, InnerPanel1);
            }
        }

        private void txt_ChangeLayoutDescription_TextChanged(object sender, EventArgs e)
        {
            if (this.Controls.Contains(panel_LayoutMenu))
            {
                GVisual.ShowControl(btn_ChangeLayoutDescription, InnerPanel1);
            }

            if (txt_ChangeLayoutDescription.Text.Length == 0)
            {
                GVisual.HideControl(btn_ChangeLayoutDescription, InnerPanel1);
            }
        }

        private void btn_ChangeLayoutName_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();

            string layout_name = txt_ChangeLayoutName.Text;

            if (string.IsNullOrWhiteSpace(layout_name))
            {
                errorProvider.SetError(txt_ChangeLayoutName, "Bu alan boş bırakılamaz.");
            }
            if (layout_name.Length > 30)
            {
                errorProvider.SetError(txt_ChangeLayoutName, "Layout'un ismi 30 karakterden uzun olamaz.");
            }

            using (var context = new DBContext())
            {
                var layout = (from x in context.Layout
                              where x.Name == layout_name
                              select x).FirstOrDefault();

                if (layout != null)
                {
                    errorProvider.SetError(txt_ChangeLayoutName, "Aynı isimli bir layout zaten bulunuyor lütfen başka bir isim seçin");

                    txt_ChangeLayoutName.Clear();
                    txt_ChangeLayoutName.Focus();
                }
            }

            if (!errorProvider.HasErrors)
            {
                string old_LayoutName = getLayoutNameFromPanel(SelectLayoutPanel, SelectedPB, true);

                using (var context = new DBContext())
                {
                    var layout = (from x in context.Layout
                                  where x.Name == old_LayoutName
                                  select x).FirstOrDefault();

                    if (layout != null)
                    {
                        layout.Name = layout_name;
                        context.SaveChanges();
                        GVisual.HideControl(btn_ChangeLayoutName, InnerPanel1);
                        setLayoutNameDesc(SelectLayoutPanel, SelectedPB, true, layout_name);
                        lbl_LayoutMenu_Title.Text = layout_name;

                        CustomNotifyIcon notify = new CustomNotifyIcon();
                        notify.showAlert("Layout ismi başarıyla değiştirildi", CustomNotifyIcon.enmType.Success);
                    }
                    else
                    {
                        CustomNotifyIcon notify = new CustomNotifyIcon();
                        notify.showAlert("Layout ismi değiştirilemedi", CustomNotifyIcon.enmType.Error);
                    }
                }
            }

        }

        private void btn_ChangeLayoutDescription_Click(object sender, EventArgs e)
        {
            string layout_Description = txt_ChangeLayoutDescription.Text;

            if (string.IsNullOrWhiteSpace(layout_Description))
            {
                errorProvider.SetError(txt_ChangeLayoutDescription, "Bu alan boş bırakılamaz.");
            }

            if (!errorProvider.HasErrors)
            {
                string old_LayoutName = getLayoutNameFromPanel(SelectLayoutPanel, SelectedPB, true);

                using (var context = new DBContext())
                {
                    var layout = (from x in context.Layout
                                  where x.Name == old_LayoutName
                                  select x).FirstOrDefault();

                    if (layout != null)
                    {
                        layout.Description = layout_Description;
                        context.SaveChanges();
                        GVisual.HideControl(btn_ChangeLayoutDescription, InnerPanel1);
                        setLayoutNameDesc(SelectLayoutPanel, SelectedPB, false, layout_Description);
                        CustomNotifyIcon notify = new CustomNotifyIcon();
                        notify.showAlert("Layout açıklaması başarıyla değiştirildi", CustomNotifyIcon.enmType.Success);
                    }
                    else
                    {
                        CustomNotifyIcon notify = new CustomNotifyIcon();
                        notify.showAlert("Layout açıklaması değiştirilemedi", CustomNotifyIcon.enmType.Error);
                    }
                }
            }
        }

        private void setLayoutNameDesc(FlowLayoutPanel FlowPanel, PictureBox pictureBox, bool isName, string NameDesc)
        {
            foreach (var panel in FlowPanel.Controls)
            {
                if (panel is Panel)
                {
                    Panel panel1 = (Panel)panel;

                    if (panel1.Controls.Contains(pictureBox))
                    {
                        foreach (var control in panel1.Controls)
                        {
                            if (isName)
                            {
                                if (control is Krypton.Toolkit.KryptonWrapLabel)
                                {
                                    Krypton.Toolkit.KryptonWrapLabel txt = (KryptonWrapLabel)(control);

                                    if (txt.AccessibleName == "Layout Name")
                                    {
                                        txt.Text = NameDesc;
                                    }
                                }
                            }
                            else
                            {
                                if (control is Panel)
                                {
                                    Panel descPanel = (Panel)control;

                                    foreach (var txt in descPanel.Controls)
                                    {
                                        if (txt is System.Windows.Forms.RichTextBox)
                                        {
                                            System.Windows.Forms.RichTextBox txt1 = (System.Windows.Forms.RichTextBox)txt;

                                            if (txt1.AccessibleName == "Layout Desc")
                                            {
                                                txt1.Text = NameDesc;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private string getLayoutNameFromPanel(FlowLayoutPanel FlowPanel, PictureBox pictureBox, bool isName)
        {
            foreach (var panel in FlowPanel.Controls)
            {
                if (panel is Panel)
                {
                    Panel panel1 = (Panel)panel;

                    if (panel1.Controls.Contains(pictureBox))
                    {
                        foreach (var control in panel1.Controls)
                        {
                            if (isName)
                            {
                                if (control is Krypton.Toolkit.KryptonWrapLabel)
                                {
                                    Krypton.Toolkit.KryptonWrapLabel txt = (KryptonWrapLabel)(control);

                                    if (txt.AccessibleName == "Layout Name")
                                    {
                                        return txt.Text;
                                    }
                                }
                            }
                            else
                            {
                                if (control is Panel)
                                {
                                    Panel descPanel = (Panel)control;

                                    foreach (var txt in descPanel.Controls)
                                    {
                                        if (txt is System.Windows.Forms.RichTextBox)
                                        {
                                            System.Windows.Forms.RichTextBox txt1 = (System.Windows.Forms.RichTextBox)txt;

                                            if (txt1.AccessibleName == "Layout Desc")
                                            {
                                                return txt1.Text;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return string.Empty;
        }

        private void btn_Layout_Yukle_Click(object sender, EventArgs e)
        {
            if (SelectedPB != null)
            {
                this.DialogResult = DialogResult.OK;
            }
        }
    }
}
