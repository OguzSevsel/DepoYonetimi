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

namespace Balya_Yerleştirme
{
    public partial class SelectLayouts : Form
    {
        public Panel DrawingPanel { get; set; }

        public Pen LayoutPen = new Pen(System.Drawing.Color.Black, 2);
        public PictureBox? SelectedPB { get; set; }
        public MainForm Main { get; set; }
        public int LayoutCount { get; set; }
        public int totalProgress = 0;
        public System.Drawing.Point PointProgressBar = new System.Drawing.Point(18, 76);
        public System.Drawing.Point PointSelectLayoutPanel = new System.Drawing.Point(12, 70);


        public SelectLayouts(Panel drawingPanel, MainForm main)
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Main = main;
            DrawingPanel = drawingPanel;
            GVisual.HideControl(SelectLayoutPanel, this);
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

        public async void CreateLayouts(Layout layout)
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

        private async Task<Ambar> LayoutYükleDatabaseOperation(Layout layout, IProgress<float> progress)
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
                            newReff.OriginalLocationInsideParent = reff.OriginalLocationInsideParent;
                            newReff.FixedPointLocation = reff.FixedPointLocation;
                            newReff.ConveyorId = reff.ConveyorId;
                            newReff.ReferenceId = reff.ReferenceId;
                            newReff.Pointsize = reff.Pointsize;
                            newReff.LocationX = reff.LocationX;
                            newReff.LocationY = reff.LocationY;

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
                    panel.Size = new Size(600, 600);
                    panel.AutoScroll = true;
                    panel.BackColor = System.Drawing.Color.AliceBlue;
                    panel.BorderStyle = BorderStyle.Fixed3D;

                    PictureBox pictureBox = new PictureBox();
                    pictureBox.Size = new Size(400, 380);
                    pictureBox.Location = new System.Drawing.Point(100, 100);
                    pictureBox.BorderStyle = BorderStyle.FixedSingle;
                    GVisual.SetDoubleBuffered(pictureBox);

                    System.Windows.Forms.ToolTip tooltip = new System.Windows.Forms.ToolTip();

                    tooltip.SetToolTip(pictureBox, "Layout'u yüklemek için çift tıklayın.\nSilmek için sağ tıklayın.");

                    tooltip.AutoPopDelay = 5000;
                    tooltip.InitialDelay = 100;
                    tooltip.ReshowDelay = 100;
                    tooltip.ShowAlways = true;

                    Krypton.Toolkit.KryptonWrapLabel LayoutTitle = new Krypton.Toolkit.KryptonWrapLabel();
                    LayoutTitle.MaximumSize = new Size(350, 30);
                    LayoutTitle.MinimumSize = new Size(350, 30);
                    LayoutTitle.TextAlign = ContentAlignment.MiddleCenter;
                    LayoutTitle.AutoSize = true;
                    LayoutTitle.Text = $"{Layout.Name}";
                    LayoutTitle.Location = new System.Drawing.Point(pictureBox.Left + (pictureBox.Width / 2 - LayoutTitle.Width / 2), 10);
                    LayoutTitle.StateCommon.TextColor = System.Drawing.Color.Red;
                    LayoutTitle.StateCommon.Font = new System.Drawing.Font("Arial", 16);

                    Krypton.Toolkit.KryptonWrapLabel LayoutDesc = new Krypton.Toolkit.KryptonWrapLabel();
                    LayoutDesc.MaximumSize = new Size(350, 300);
                    LayoutDesc.MinimumSize = new Size(350, 50);
                    LayoutDesc.TextAlign = ContentAlignment.MiddleCenter;
                    LayoutDesc.Text = $"{Layout.Description}";
                    LayoutDesc.Location = new System.Drawing.Point(25, 25);
                    LayoutDesc.ForeColor = System.Drawing.Color.Black;
                    LayoutDesc.Font = new System.Drawing.Font("Arial", 12);

                    Panel panelDescription = new Panel();
                    panelDescription.Size = new Size(LayoutDesc.Width + 50, LayoutDesc.Height + 50);
                    panelDescription.AutoScroll = true;
                    panelDescription.BackColor = System.Drawing.Color.Azure;
                    panelDescription.BorderStyle = BorderStyle.Fixed3D;
                    panelDescription.Location = new System.Drawing.Point(pictureBox.Left + (pictureBox.Width / 2 - panelDescription.Width / 2), pictureBox.Bottom);

                    RectangleF ambarRect = new RectangleF();
                    ambarRect = ambar.Rectangle;

                    ambar.Rectangle = GVisual.RatioRectangleBetweenTwoParentRectangles(ambar.Rectangle, DrawingPanel.ClientRectangle, pictureBox.ClientRectangle);
                    float BeforeX = ambar.Rectangle.X;

                    ambar.Rectangle = GVisual.CenterRectangletoParentRectangle(ambar.Rectangle, pictureBox.ClientRectangle);
                    float AfterX = ambar.Rectangle.X;
                    float moveX = BeforeX - AfterX;

                    foreach (var conveyor in ambar.conveyors)
                    {
                        conveyor.Rectangle = GVisual.RatioRectangleBetweenTwoParentRectangles(conveyor.Rectangle, DrawingPanel.ClientRectangle, pictureBox.ClientRectangle);
                        conveyor.Rectangle = new RectangleF
                                        (conveyor.Rectangle.X - moveX,
                                    conveyor.Rectangle.Y, conveyor.Rectangle.Width, conveyor.Rectangle.Height);

                        foreach (var reff in conveyor.ConveyorReferencePoints)
                        {
                            reff.Rectangle = GVisual.RatioRectangleBetweenTwoParentRectangles(reff.Rectangle, DrawingPanel.ClientRectangle, pictureBox.ClientRectangle);
                            reff.Rectangle = new RectangleF
                                            (reff.Rectangle.X - moveX,
                                        reff.Rectangle.Y, reff.Rectangle.Width, reff.Rectangle.Height);
                        }
                    }

                    foreach (var depo in ambar.depolar)
                    {
                        depo.Rectangle = GVisual.RatioRectangleBetweenTwoParentRectangles(depo.Rectangle, DrawingPanel.ClientRectangle, pictureBox.ClientRectangle);
                        depo.Rectangle = new RectangleF
                                        (depo.Rectangle.X - moveX,
                                    depo.Rectangle.Y, depo.Rectangle.Width, depo.Rectangle.Height);

                        foreach (var cell in depo.gridmaps)
                        {
                            cell.Rectangle = GVisual.RatioRectangleBetweenTwoParentRectangles(cell.Rectangle, DrawingPanel.ClientRectangle, pictureBox.ClientRectangle);
                            cell.Rectangle = new RectangleF
                                            (cell.Rectangle.X - moveX,
                                        cell.Rectangle.Y, cell.Rectangle.Width, cell.Rectangle.Height);

                            foreach (var item in cell.items)
                            {
                                item.Rectangle = GVisual.RatioRectangleBetweenTwoParentRectangles(item.Rectangle, DrawingPanel.ClientRectangle, pictureBox.ClientRectangle);
                                item.Rectangle = new RectangleF
                                                (item.Rectangle.X - moveX,
                                            item.Rectangle.Y, item.Rectangle.Width, item.Rectangle.Height);

                                foreach (var reff in item.ItemReferencePoints)
                                {
                                    reff.Rectangle = GVisual.RatioRectangleBetweenTwoParentRectangles(reff.Rectangle, DrawingPanel.ClientRectangle, pictureBox.ClientRectangle);
                                    reff.Rectangle = new RectangleF
                                                    (reff.Rectangle.X - moveX,
                                                reff.Rectangle.Y, reff.Rectangle.Width, reff.Rectangle.Height);
                                }
                            }
                        }
                    }
                    panel.Controls.Add(LayoutTitle);
                    panelDescription.Controls.Add(LayoutDesc);
                    panel.Controls.Add(panelDescription);
                    panel.Controls.Add(pictureBox);
                    SelectLayoutPanel.Controls.Add(panel);
                    pictureBox.Paint += (sender, e) => Picture_Box_Paint(sender, e, ambar);
                    pictureBox.Tag = ambar;
                    pictureBox.MouseMove += (sender, e) => Picture_Box_MouseMove(sender, e);
                    pictureBox.MouseEnter += (sender, e) => PictureBox_MouseEnter(sender, e);
                    pictureBox.MouseLeave += (sender, e) => PictureBox_MouseLeave(sender, e);
                    pictureBox.DoubleClick += (sender, e) => PictureBox_MouseDoubleClick(sender, e);
                    pictureBox.MouseDown += (sender, e) => PictureBox_MouseDown(sender, e);
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

        private void PictureBox_MouseLeave(object sender, EventArgs e)
        {
            PictureBox pictureBox = sender as PictureBox;

            if (pictureBox != null)
            {
                LayoutPen.Color = System.Drawing.Color.Black;
                pictureBox.Invalidate();
                pictureBox.Update();
            }
        }

        private void PictureBox_MouseEnter(object sender, EventArgs e)
        {
            PictureBox pictureBox = sender as PictureBox;

            if (pictureBox != null)
            {
                LayoutPen.Color = System.Drawing.Color.Red;
                pictureBox.Invalidate();
            }
        }

        private void Picture_Box_MouseMove(object? sender, MouseEventArgs e)
        {
            PictureBox pictureBox = sender as PictureBox;

            if (pictureBox != null)
            {
                if (pictureBox.ClientRectangle.Contains(e.Location))
                {
                    LayoutPen.Color = System.Drawing.Color.Red;
                    pictureBox.Invalidate();
                    pictureBox.Refresh();
                }
                else
                {
                    LayoutPen.Color = System.Drawing.Color.Black;
                    pictureBox.Invalidate();
                    pictureBox.Refresh();
                }
            }
        }

        private void Picture_Box_Paint(object? sender, PaintEventArgs e, Ambar ambar)
        {
            int counter = 0;
            System.Drawing.Font font = new System.Drawing.Font("Arial", 8);
            SolidBrush brush = new SolidBrush(System.Drawing.Color.Red);
            RectangleF rect = new RectangleF();
            Graphics g = e.Graphics;
            PictureBox pictureBox = sender as PictureBox;
            System.Drawing.Point mousePosition = pictureBox.PointToClient(Cursor.Position);
            if (pictureBox.ClientRectangle.Contains(mousePosition))
            {
                LayoutPen.Color = System.Drawing.Color.Red;
            }
            else
            {
                LayoutPen.Color = System.Drawing.Color.Black;
            }

            g.DrawRectangle(LayoutPen, ambar.Rectangle);

            foreach (var depo in ambar.depolar)
            {
                g.DrawRectangle(LayoutPen, depo.Rectangle);

                foreach (var cell in depo.gridmaps)
                {
                    g.DrawRectangle(LayoutPen, cell.Rectangle);

                    foreach (var item in cell.items)
                    {
                        g.DrawRectangle(LayoutPen, item.Rectangle);
                        counter++;
                        rect = item.Rectangle;
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
                g.DrawRectangle(LayoutPen, conveyor.Rectangle);

                foreach (var reff in conveyor.ConveyorReferencePoints)
                {
                    reff.Draw(g);
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
    }
}
