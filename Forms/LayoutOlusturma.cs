using Balya_Yerleştirme.Models;
using CustomNotification;
using DocumentFormat.OpenXml.Spreadsheet;
using GUI_Library;
using String_Library;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;
using static Balya_Yerleştirme.Utilities.Utils;

namespace Balya_Yerleştirme
{
    public partial class LayoutOlusturma : Form
    {
        //Variables for Moving Areas with MouseMove Events and Moving Areas for UI Operations
        #region Variables for Moving Rectangles

        public float drawingPanelMoveConstant = 330 / 2;
        public bool isMoving { get; set; } = false;
        public bool MovingParameter { get; set; } = false;
        public bool snapped { get; set; } = false;


        #endregion



        //Variables for creation of depo, conveyor, gridmaps, reference points
        #region Creation Variables

        public float cell_eni { get; set; } = 0f;
        public float cell_boyu { get; set; } = 0f;
        public float yatay_kenar_boslugu { get; set; } = 0f;
        public float dikey_kenar_boslugu { get; set; } = 0f;
        public float nesne_eni { get; set; } = 0f;
        public float nesne_boyu { get; set; } = 0f;
        public Depo? selDepo { get; set; }
        public Conveyor? selConveyor { get; set; }
        public bool ToolStripIzgara { get; set; } = false;
        public float nesne_Eni { get; set; }
        public float nesne_Boyu { get; set; }
        public int nesne_Yuksekligi { get; set; }
        public float total_Cell_Width { get; set; }
        public float total_Cell_Height { get; set; }
        public int nesne_sayisi { get; set; }
        public EventHandler izgaraHaritasiOlustur { get; set; }
        public bool AddReferencePoint { get; set; } = false;

        #endregion



        //Manual Move Variables
        #region Changing Size and Location Variables

        public float UnchangedDepoAlaniEni { get; set; }
        public float UnchangedDepoAlaniBoyu { get; set; }
        public float UnchangedConveyorEni { get; set; }
        public float UnchangedConveyorBoyu { get; set; }
        public bool Manuel_Move { get; set; } = false;
        public float txt_width { get; set; }
        public float txt_height { get; set; }

        public bool ambar_Boyut_Degistir = false;

        #endregion



        //Item Placement Sequence Variables
        #region Item Placement Sequence Variables

        public string Parameter = String.Empty;
        public int rowCount { get; set; }
        public int currentRow { get; set; } = 0;
        public int colCount { get; set; }
        public int currentColumn { get; set; } = 0;
        public int toplam_nesne_yuksekligi { get; set; }
        public int toplam_nesne_yuksekligi_asama2 { get; set; }

        public bool askOnce = true;
        public int Ware_Counter { get; set; } = 1;
        public bool Fill_WareHouse { get; set; } = false;

        public System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        public List<int> CountList { get; set; } = new List<int>();

        List<RectangleF> rectangles = new List<RectangleF>();

        #endregion



        //Panel Location Variables for UI Operations 
        #region Panel Locations and Sizes
        public System.Drawing.Point drawingPanelLeftLocation { get; set; } = new System.Drawing.Point(10, 102);
        public System.Drawing.Point drawingPanelMiddleLocation { get; set; } = new System.Drawing.Point(343, 102);
        public System.Drawing.Point leftSidePanelLocation { get; set; } = new System.Drawing.Point(10, 102);
        public System.Drawing.Point rightSidePanelLocation { get; set; } = new System.Drawing.Point(1569, 102);
        public System.Drawing.Size drawingPanelSmallSize { get; set; } = new System.Drawing.Size(1220, 909);
        public System.Drawing.Size drawingPanelMiddleSize { get; set; } = new System.Drawing.Size(1550, 909);
        public System.Drawing.Size drawingPanelLargeSize { get; set; } = new System.Drawing.Size(1880, 909);
        public System.Drawing.Size SmallPanelSize { get; set; } = new System.Drawing.Size(310, 62);

        #endregion



        //Rectangle Variables for Various use cases such as Manuel Move with textboxes etc.
        #region PlaceHolder Rectangles
        public RectangleF selectedConveyorRectangle { get; set; } = new RectangleF();
        public RectangleF UnchangedselectedConveyorRectangle { get; set; } = new RectangleF();
        public RectangleF ProxRectRight { get; set; } = new RectangleF();
        public RectangleF ProxRectLeft { get; set; } = new RectangleF();
        public RectangleF ProxRectTop { get; set; } = new RectangleF();
        public RectangleF ProxRectBottom { get; set; } = new RectangleF();
        public RectangleF ManuelDepoRectangle { get; set; } = new RectangleF();
        public RectangleF ManuelConveyorRectangle { get; set; } = new RectangleF();
        public RectangleF selectedDepoRectangle { get; set; } = new RectangleF();
        public RectangleF UnchangedselectedDepoRectangle { get; set; } = new RectangleF();
        #endregion



        //Custom Object Variables for Selecting an area or Copy Pasting
        #region Custom Objects (for Selecting and Copy Pasting)

        public MainForm Main { get; set; }
        public Ambar? Ambar { get; set; }
        public Ambar? SelectedAmbar { get; set; }
        public Conveyor? selectedConveyor { get; set; } = null;
        public Depo? selectedDepo { get; set; } = null;

        #endregion



        //Variables for Copy and Paste Depos and Conveyors
        #region Copy Paste Variables

        public Models.Cell DataCell { get; set; } = new Models.Cell();
        public Depo? CopyDepo { get; set; }
        public Conveyor? CopyConveyor { get; set; }
        public PointF CopyPoint { get; set; }

        #endregion



        //Pens
        #region Pens

        public Pen SelectedAmbarPen { get; set; } = new Pen(System.Drawing.Color.Black);
        public Pen SelectedDepoPen { get; set; } = new Pen(System.Drawing.Color.Black);
        public Pen SelectedConveyorPen { get; set; } = new Pen(System.Drawing.Color.Black);
        public Pen SelectedDepoEdgePen { get; set; } = new Pen(System.Drawing.Color.Red);
        public Pen SelectedConveyorEdgePen { get; set; } = new Pen(System.Drawing.Color.Red);
        public Pen TransparentPen { get; set; } = new Pen(System.Drawing.Color.Black, 2);

        #endregion



        //Variables for Saving the Layout when closing this
        #region Layout Save
        public string LayoutName { get; set; }
        public string LayoutDescription { get; set; }
        #endregion



        //Variables for TreeNode
        #region TreeNode Variables
        public TreeNode AlanNode { get; set; }
        public TreeNode DepoNode { get; set; }
        public TreeNode ConveyorNode { get; set; }
        #endregion



        public bool menuProcess { get; set; } = false;
        public Layout? layout { get; set; }



        public LayoutOlusturma(MainForm main, Ambar? ambar, Layout? layout)
        {
            InitializeComponent();
            System.Windows.Forms.ToolTip toolTip = new System.Windows.Forms.ToolTip();
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
            drawingPanel.TabStop = true;
            drawingPanel.Click += (s, e) => drawingPanel.Focus();
            AlanTreeView.AfterSelect += AlanTreeView_AfterSelect;
            AlanTreeView.BeforeSelect += AlanTreeView_BeforeSelect;
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
            CopyDepo = new Depo();
            CopyConveyor = new Conveyor();
            GVisual.SetDoubleBuffered(drawingPanel);
            Main = main;
            Ambar = ambar;
            this.layout = layout;

            timer.Tick += Timer_Tick;

            AlanNode = new TreeNode("Alan");
            AlanNode.ForeColor = System.Drawing.Color.Blue;
            AlanTreeView.Nodes.Add(AlanNode);

            DepoNode = new TreeNode("Depolar");
            AlanNode.Nodes.Add(DepoNode);

            ConveyorNode = new TreeNode("Conveyorlar");
            AlanNode.Nodes.Add(ConveyorNode);


            HideEverything();
            MainPanelCloseBothSides(LeftSide_LayoutPanel, RightSide_LayoutPanel, this);
        }



        //TreeView Events and Methods for adding and deleting nodes according to the depos, conveyors in the area
        #region TreeView Events and Methods
        private void AlanTreeView_BeforeSelect(object? sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Tag == null)
            {
                e.Cancel = true;
            }
        }

        private void AlanTreeView_AfterSelect(object? sender, TreeViewEventArgs e)
        {
            var selectedObject = e.Node.Tag;
            TreeNode node = e.Node;

            if (selectedObject is Ambar ambar)
            {
                if (selectedConveyor != null)
                {
                    selectedConveyor = null;
                    SelectedConveyorPen.Width = 1;
                    SelectedConveyorPen.Color = System.Drawing.Color.Black;
                }

                if (selectedDepo != null)
                {
                    selectedDepo = null;
                    SelectedDepoPen.Width = 1;
                    SelectedDepoPen.Color = System.Drawing.Color.Black;
                }

                SelectedAmbar = ambar;
                SelectedAmbarPen.Width = 3;
                SelectedAmbarPen.Color = System.Drawing.Color.Blue;

                SortFlowLayoutPanel(layoutPanel_Ambar);
                AlanTreeView.ExpandAll();

                drawingPanel.Invalidate();
            }
            else if (selectedObject is Depo depo)
            {
                foreach (var depo1 in Ambar.depolar)
                {
                    if (depo1 == selectedObject)
                    {
                        if (selectedConveyor != null)
                        {
                            selectedConveyor = null;
                            SelectedConveyorPen.Width = 1;
                            SelectedConveyorPen.Color = System.Drawing.Color.Black;
                        }

                        if (SelectedAmbar != null)
                        {
                            SelectedAmbar = null;
                            SelectedAmbarPen.Width = 1;
                            SelectedAmbarPen.Color = System.Drawing.Color.Black;
                        }

                        selectedDepo = depo1;
                        SelectedDepoPen.Width = 3;
                        SelectedDepoPen.Color = System.Drawing.Color.Blue;

                        ManuelDepoRectangle = selectedDepo.Rectangle;
                        UnchangedDepoAlaniBoyu = selectedDepo.DepoAlaniBoyu;
                        UnchangedDepoAlaniEni = selectedDepo.DepoAlaniEni;
                        UnchangedselectedDepoRectangle = selectedDepo.Rectangle;

                        SortFlowLayoutPanel(LayoutPanel_SelectedDepo);
                        AlanTreeView.ExpandAll();


                        drawingPanel.Invalidate();
                    }
                }
            }
            else if (selectedObject is Conveyor conveyor)
            {
                foreach (var conv in Ambar.conveyors)
                {
                    if (conv == selectedObject)
                    {
                        if (selectedDepo != null)
                        {
                            selectedDepo = null;
                            SelectedDepoPen.Width = 1;
                            SelectedDepoPen.Color = System.Drawing.Color.Black;
                        }

                        if (SelectedAmbar != null)
                        {
                            SelectedAmbar = null;
                            SelectedAmbarPen.Width = 1;
                            SelectedAmbarPen.Color = System.Drawing.Color.Black;
                        }

                        selectedConveyor = conv;
                        SelectedConveyorPen.Width = 3;
                        SelectedConveyorPen.Color = System.Drawing.Color.Blue;

                        ManuelConveyorRectangle = selectedConveyor.Rectangle;
                        UnchangedConveyorEni = selectedConveyor.ConveyorEni;
                        UnchangedConveyorBoyu = selectedConveyor.ConveyorBoyu;
                        UnchangedselectedConveyorRectangle = selectedConveyor.Rectangle;

                        SortFlowLayoutPanel(layoutPanel_SelectedConveyor);
                        AlanTreeView.ExpandAll();

                        drawingPanel.Invalidate();
                    }
                }
            }
            else
            {
                if (node != DepoNode && node != ConveyorNode)
                {
                    if (selectedConveyor != null)
                    {
                        selectedConveyor = null;
                        SelectedConveyorPen.Width = 1;
                        SelectedConveyorPen.Color = System.Drawing.Color.Black;
                    }

                    if (selectedDepo != null)
                    {
                        selectedDepo = null;
                        SelectedDepoPen.Width = 1;
                        SelectedDepoPen.Color = System.Drawing.Color.Black;
                    }

                    if (SelectedAmbar != null)
                    {
                        SelectedAmbar = null;
                        SelectedAmbarPen.Width = 1;
                        SelectedAmbarPen.Color = System.Drawing.Color.Black;
                    }

                    if (LeftSide_LayoutPanel.Visible)
                    {
                        Clear_AddControltoLeftSidePanel(LayoutPanel_Alan_Hierarchy);
                    }

                    drawingPanel.Invalidate();
                }
            }
        }

        private void DeleteDepoNode(Depo depo)
        {
            TreeNode deleteNode = new TreeNode();
            foreach (TreeNode node in DepoNode.Nodes)
            {
                if (node.Tag == depo)
                {
                    deleteNode = node;
                }
            }
            DepoNode.Nodes.Remove(deleteNode);
        }

        private void DeleteConveyorNode(Conveyor conveyor)
        {
            TreeNode deleteNode = new TreeNode();
            foreach (TreeNode node in ConveyorNode.Nodes)
            {
                if (node.Tag == conveyor)
                {
                    deleteNode = node;
                }
            }
            ConveyorNode.Nodes.Remove(deleteNode);
        }

        public void AddDepoNode(Depo depo)
        {
            TreeNode depoNode = new TreeNode($"{depo.DepoName}");
            depoNode.Tag = depo;
            depoNode.ForeColor = System.Drawing.Color.Blue;
            DepoNode.Nodes.Add(depoNode);
        }

        public void AddConveyorNode(Conveyor conveyor)
        {
            TreeNode conveyorNode = new TreeNode($"Conveyor");
            conveyorNode.Tag = conveyor;
            conveyorNode.ForeColor = System.Drawing.Color.Blue;
            ConveyorNode.Nodes.Add(conveyorNode);
        }

        public void SelectNode(Ambar? ambar, Conveyor? conveyor, Depo? depo)
        {
            AlanTreeView.Focus();
            if (ambar != null)
            {
                foreach (TreeNode node in AlanTreeView.Nodes)
                {
                    if (node.Tag == ambar)
                    {
                        AlanTreeView.SelectedNode = node;
                    }
                }
            }

            if (conveyor != null)
            {
                foreach (TreeNode node in ConveyorNode.Nodes)
                {
                    if (node.Tag == conveyor)
                    {
                        AlanTreeView.SelectedNode = node;
                    }
                }
            }

            if (depo != null)
            {
                foreach (TreeNode node in DepoNode.Nodes)
                {
                    if (node.Tag == depo)
                    {
                        AlanTreeView.SelectedNode = node;
                    }
                }
            }
            AlanTreeView.Refresh();
        }

        public void UnselectNodes()
        {
            AlanTreeView.SelectedNode = null;
            AlanTreeView.CollapseAll();
        }

        #endregion



        //Delete, Copy, Paste depo's and conveyor's with shortcuts 
        #region Delete, Copy, Paste

        //Delete, Copy, Paste
        private void KeyDownEventHandler(object? sender, KeyEventArgs e)
        {
            if (drawingPanel.Focused)
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
                            float x = 0f;
                            float y = 0f;

                            string deponame = CopyDepo.DepoName;

                            PointF checkPoint = new PointF(CopyPoint.X, CopyPoint.Y);

                            if (Ambar.Rectangle.Contains(checkPoint))
                            {
                                if (checkPoint.X + CopyDepo.Rectangle.Width > Ambar.Rectangle.Right)
                                {
                                    x = Ambar.Rectangle.Right - CopyDepo.Rectangle.Width;
                                }
                                else if (checkPoint.X - CopyDepo.Rectangle.Width < Ambar.Rectangle.Left)
                                {
                                    x = Ambar.Rectangle.Left;
                                }
                                else
                                {
                                    x = CopyPoint.X - CopyDepo.Rectangle.Width / 2;
                                }

                                if (checkPoint.Y + CopyDepo.Rectangle.Height > Ambar.Rectangle.Bottom)
                                {
                                    y = Ambar.Rectangle.Bottom - CopyDepo.Rectangle.Height;
                                }
                                else if (checkPoint.Y - CopyDepo.Rectangle.Height < Ambar.Rectangle.Top)
                                {
                                    y = Ambar.Rectangle.Top;
                                }
                                else
                                {
                                    y = CopyPoint.Y - CopyDepo.Rectangle.Height / 2;
                                }


                                Depo CopiedDepo = new Depo(x, y, CopyDepo.Rectangle.Width, CopyDepo.Rectangle.Height, CopyDepo.Zoomlevel, Main, this, Ambar);

                                CopiedDepo.DepoId = 0;
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
                                    CopiedCell.CellId = 0;
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
                            PointF checkPoint = new PointF(CopyPoint.X, CopyPoint.Y);

                            float x = 0f;
                            float y = 0f;

                            if (Ambar.Rectangle.Contains(checkPoint))
                            {

                                if (checkPoint.X + CopyConveyor.Rectangle.Width > Ambar.Rectangle.Right)
                                {
                                    x = Ambar.Rectangle.Right - CopyConveyor.Rectangle.Width;
                                }
                                else if (checkPoint.X - CopyConveyor.Rectangle.Width < Ambar.Rectangle.Left)
                                {
                                    x = Ambar.Rectangle.Left;
                                }
                                else
                                {
                                    x = CopyPoint.X - CopyConveyor.Rectangle.Width / 2;
                                }

                                if (checkPoint.Y + CopyConveyor.Rectangle.Height > Ambar.Rectangle.Bottom)
                                {
                                    y = Ambar.Rectangle.Bottom - CopyConveyor.Rectangle.Height;
                                }
                                else if (checkPoint.Y - CopyConveyor.Rectangle.Height < Ambar.Rectangle.Top)
                                {
                                    y = Ambar.Rectangle.Top;
                                }
                                else
                                {
                                    y = CopyPoint.Y - CopyConveyor.Rectangle.Height / 2;
                                }

                                Conveyor conveyor = new Conveyor(x, y, CopyConveyor.Rectangle.Width, CopyConveyor.Rectangle.Height,
                                Main, this, Ambar);

                                conveyor.OriginalRectangle = conveyor.Rectangle;
                                conveyor.ConveyorId = 0;
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

                                    newReff.ReferenceId = 0;
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
                    if (Ambar != null)
                    {
                        if (selectedDepo != null)
                        {
                            Ambar.deletedDepos.Add(selectedDepo);
                            Ambar.depolar.Remove(selectedDepo);
                            if (LeftSide_LayoutPanel.Visible)
                            {
                                MainPanelCloseLeftSide(LeftSide_LayoutPanel, this);
                                menuProcess = false;
                            }

                            if (RightSide_LayoutPanel.Visible)
                            {
                                if (Depo_Olusturma_Paneli.Visible)
                                {
                                    RightSide_LayoutPanel.ScrollControlIntoView(Depo_Olusturma_Paneli);
                                }
                                else
                                {
                                    GVisual.ShowControl(Depo_Olusturma_Paneli, RightSide_LayoutPanel);
                                    RightSide_LayoutPanel.ScrollControlIntoView(Depo_Olusturma_Paneli);
                                }
                            }
                        }

                        if (selectedConveyor != null)
                        {
                            Ambar.deletedConveyors.Add(selectedConveyor);
                            Ambar.conveyors.Remove(selectedConveyor);
                            if (LeftSide_LayoutPanel.Visible)
                            {
                                MainPanelCloseLeftSide(LeftSide_LayoutPanel, this);
                                menuProcess = false;
                                AddReferencePoint = false;
                            }
                            if (RightSide_LayoutPanel.Visible)
                            {
                                if (Conveyor_Olusturma_Paneli.Visible)
                                {
                                    RightSide_LayoutPanel.ScrollControlIntoView(Conveyor_Olusturma_Paneli);
                                }
                                else
                                {
                                    GVisual.ShowControl(Conveyor_Olusturma_Paneli, RightSide_LayoutPanel);
                                    RightSide_LayoutPanel.ScrollControlIntoView(Conveyor_Olusturma_Paneli);
                                }
                            }
                        }
                    }
                    drawingPanel.Invalidate();
                }
            }
        }

        #endregion



        //Item Placement Sequence events for placing items to the Depo in main program form
        #region Item Placement Sequence Algorithm
        //Item Placement Sequence Start Button Events
        private void btn_Depo_Menu_Nesne_Yerlestirme_Siralamasi_Click(object sender, EventArgs e)
        {
            if (selectedDepo != null)
            {
                if (selectedDepo.gridmaps.Count > 0)
                {
                    GVisual.HideControl(panel_Depo_Menu, groupBox_SelectedDepo);
                    GVisual.ShowControl(Asama1_Yukseklik_Panel, groupBox_SelectedDepo);
                    GVisual.Control_Center(Asama1_Yukseklik_Panel, groupBox_SelectedDepo);

                    TransparentPen.Color = System.Drawing.Color.FromArgb(0, System.Drawing.Color.Black);
                    MovingParameter = true;
                    lbl_Placement_Yukseklik_Depo_Alani_Yuksekligi_Value.Text = $"{selectedDepo.DepoAlaniYuksekligi} cm";
                    lbl_Placement_Yukseklik_Nesne_Yuksekligi_Value.Text = $"{selectedDepo.nesneYuksekligi} cm";
                    lbl_Placement_Yukseklik_upDown_Nesne_Yuksekligi_Value.Text = "0 cm";
                    menuProcess = true;
                }
                else
                {
                    CustomNotifyIcon notify = new CustomNotifyIcon();
                    notify.showAlert("Lütfen önce ızgara haritası oluşturun.", CustomNotifyIcon.enmType.Warning);
                }
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


        //Height Stage 1 Events
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

                    if (menuProcess)
                    {
                        GVisual.HideControl(Asama1_Yukseklik_Panel, groupBox_SelectedDepo);
                        GVisual.HideControl(btn_Depo_Menu_Go_Back, LayoutPanel_SelectedDepo);
                        GVisual.ShowControl(Asama2_Yukseklik_Panel, groupBox_SelectedDepo);
                        GVisual.Control_Center(Asama2_Yukseklik_Panel, groupBox_SelectedDepo);
                    }
                    else
                    {
                        CenterControltoLeftSideofRectangleVertically(selectedDepo.Rectangle,
                            Asama2_Yukseklik_Panel, drawingPanel);
                    }
                    askOnce = true;
                }
            }
        }

        //Height Stage 2 Events
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

                            if (menuProcess)
                            {
                                GVisual.HideControl(Asama2_Yukseklik_Panel, groupBox_SelectedDepo);
                                GVisual.ShowControl(Placement_StartLocation_Panel, groupBox_SelectedDepo);
                                GVisual.Control_Center(Placement_StartLocation_Panel, groupBox_SelectedDepo);
                            }
                            else
                            {
                                GVisual.HideControl(Asama2_Yukseklik_Panel, drawingPanel);
                                CenterControltoLeftSideofRectangleVertically(selectedDepo.Rectangle, Placement_StartLocation_Panel, drawingPanel);
                            }
                        }
                        else
                        {
                            MovingParameter = false;
                            TransparentPen.Color = System.Drawing.Color.FromArgb(255, System.Drawing.Color.Black);

                            if (menuProcess)
                            {
                                GVisual.HideControl(Asama2_Yukseklik_Panel, groupBox_SelectedDepo);
                                Show_DepoMenus("Depo");
                                menuProcess = false;
                            }
                            else
                            {
                                GVisual.HideControl(Asama2_Yukseklik_Panel, drawingPanel);
                            }
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

                        if (menuProcess)
                        {
                            GVisual.HideControl(Asama2_Yukseklik_Panel, groupBox_SelectedDepo);
                            GVisual.ShowControl(Placement_StartLocation_Panel, groupBox_SelectedDepo);
                            GVisual.Control_Center(Placement_StartLocation_Panel, groupBox_SelectedDepo);
                        }
                        else
                        {
                            GVisual.HideControl(Asama2_Yukseklik_Panel, drawingPanel);
                            CenterControltoLeftSideofRectangleVertically(selectedDepo.Rectangle, Placement_StartLocation_Panel, drawingPanel);
                        }
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
                                if (menuProcess)
                                {
                                    GVisual.HideControl(Asama2_Yukseklik_Panel, groupBox_SelectedDepo);
                                    GVisual.ShowControl(Placement_StartLocation_Panel, groupBox_SelectedDepo);
                                    GVisual.Control_Center(Placement_StartLocation_Panel, groupBox_SelectedDepo);
                                }
                                else
                                {
                                    GVisual.HideControl(Asama2_Yukseklik_Panel, drawingPanel);
                                    CenterControltoLeftSideofRectangleVertically(selectedDepo.Rectangle, Placement_StartLocation_Panel, drawingPanel);
                                }
                            }
                            else
                            {
                                MovingParameter = false;
                                TransparentPen.Color = System.Drawing.Color.FromArgb(255, System.Drawing.Color.Black);

                                if (menuProcess)
                                {
                                    GVisual.HideControl(Asama2_Yukseklik_Panel, groupBox_SelectedDepo);
                                    Show_DepoMenus("Depo");
                                    menuProcess = false;
                                }
                                else
                                {
                                    GVisual.HideControl(Asama2_Yukseklik_Panel, drawingPanel);
                                }
                            }
                        }
                        else if (toplam_nesne_yuksekligi_asama2 >
                            selectedDepo.DepoAlaniYuksekligi - selectedDepo.nesneYuksekligi &&
                            toplam_nesne_yuksekligi_asama2 <= selectedDepo.DepoAlaniYuksekligi)
                        {
                            if (menuProcess)
                            {
                                GVisual.HideControl(Asama2_Yukseklik_Panel, groupBox_SelectedDepo);
                                GVisual.ShowControl(Placement_StartLocation_Panel, groupBox_SelectedDepo);
                                GVisual.Control_Center(Placement_StartLocation_Panel, groupBox_SelectedDepo);
                            }
                            else
                            {
                                GVisual.HideControl(Asama2_Yukseklik_Panel, drawingPanel);
                                CenterControltoLeftSideofRectangleVertically(selectedDepo.Rectangle, Placement_StartLocation_Panel, drawingPanel);
                            }
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
            if (menuProcess)
            {
                GVisual.HideControl(Asama2_Yukseklik_Panel, groupBox_SelectedDepo);
                Show_DepoMenus("Depo");
                menuProcess = false;
            }
            else
            {
                GVisual.HideControl(Asama2_Yukseklik_Panel, drawingPanel);
            }
            upDown_1Asama_NesneSayisi.Value = 0;
            upDown_2Asama_NesneSayisi.Value = 0;
        }


        //Item Placement Sequence Choose Start Location Event
        private void btn_Placement_StartLocation_NextPage_Click(object sender, EventArgs e)
        {
            if (selectedDepo != null)
            {
                if (radio_Start_From_Middle.Checked)
                {
                    if (menuProcess)
                    {
                        GVisual.HideControl(Placement_StartLocation_Panel, groupBox_SelectedDepo);
                        GVisual.ShowControl(Placement_UpDown_Panel, groupBox_SelectedDepo);
                        GVisual.Control_Center(Placement_UpDown_Panel, groupBox_SelectedDepo);
                    }
                    else
                    {
                        GVisual.HideControl(Placement_StartLocation_Panel, drawingPanel);
                        CenterControltoLeftSideofRectangleVertically(selectedDepo.Rectangle,
                            Placement_UpDown_Panel, drawingPanel);
                    }
                }
                else if (radio_Start_From_Top.Checked)
                {
                    if (menuProcess)
                    {
                        GVisual.HideControl(Placement_StartLocation_Panel, groupBox_SelectedDepo);
                        GVisual.ShowControl(Placement_LeftRight_Panel, groupBox_SelectedDepo);
                        GVisual.Control_Center(Placement_LeftRight_Panel, groupBox_SelectedDepo);
                    }
                    else
                    {
                        GVisual.HideControl(Placement_StartLocation_Panel, drawingPanel);
                        CenterControltoLeftSideofRectangleVertically(selectedDepo.Rectangle,
                            Placement_LeftRight_Panel, drawingPanel);
                    }

                    currentRow = 0;
                    rectangles.Clear();
                    timer.Interval = 300;
                    timer.Start();
                    Parameter = "Aşağı Doğru";
                }
                else if (radio_Start_From_Bottom.Checked)
                {
                    if (menuProcess)
                    {
                        GVisual.HideControl(Placement_StartLocation_Panel, groupBox_SelectedDepo);
                        GVisual.ShowControl(Placement_LeftRight_Panel, groupBox_SelectedDepo);
                        GVisual.Control_Center(Placement_LeftRight_Panel, groupBox_SelectedDepo);
                    }
                    else
                    {
                        GVisual.HideControl(Placement_StartLocation_Panel, drawingPanel);
                        CenterControltoLeftSideofRectangleVertically(selectedDepo.Rectangle,
                            Placement_LeftRight_Panel, drawingPanel);
                    }

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


        //Item Placement Sequence choose if items will be placed from middle to top or vice versa
        //(Because if user select the start location from top or bottom only where to go is down or up)
        private void btn_Placement_UpDown_NextPage_Click(object sender, EventArgs e)
        {
            if (selectedDepo != null)
            {
                if (radio_To_Up.Checked || radio_To_Down.Checked)
                {
                    if (menuProcess)
                    {
                        GVisual.HideControl(Placement_UpDown_Panel, groupBox_SelectedDepo);
                        GVisual.ShowControl(Placement_LeftRight_Panel, groupBox_SelectedDepo);
                        GVisual.Control_Center(Placement_LeftRight_Panel, groupBox_SelectedDepo);
                    }
                    else
                    {
                        GVisual.HideControl(Placement_UpDown_Panel, drawingPanel);
                        CenterControltoLeftSideofRectangleVertically(selectedDepo.Rectangle,
                            Placement_LeftRight_Panel, drawingPanel);
                    }
                }
                else
                {
                    CustomNotifyIcon notify = new CustomNotifyIcon();
                    notify.showAlert("Bir seçim yapmak zorundasınız.", CustomNotifyIcon.enmType.Warning);
                }
            }
        }


        //Item Placement Sequence choose if items will be placed from left to right or vice versa
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

                    if (menuProcess)
                    {
                        GVisual.HideControl(Placement_LeftRight_Panel, groupBox_SelectedDepo);
                        GVisual.ShowControl(panel_Depo_Menu, groupBox_SelectedDepo);
                        GVisual.Control_Center(panel_Depo_Menu, groupBox_SelectedDepo);
                    }
                    else
                    {
                        GVisual.HideControl(Placement_LeftRight_Panel, drawingPanel);
                    }

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
            rectangles.Clear();
            TransparentPen.Color = System.Drawing.Color.FromArgb(255, System.Drawing.Color.Black);
            if (menuProcess)
            {
                GVisual.HideControl(Placement_LeftRight_Panel, groupBox_SelectedDepo);
                GVisual.ShowControl(panel_Depo_Menu, groupBox_SelectedDepo);
                GVisual.Control_Center(panel_Depo_Menu, groupBox_SelectedDepo);
                menuProcess = false;
            }
            else
            {
                GVisual.HideControl(Placement_LeftRight_Panel, drawingPanel);
            }
            Parameter = string.Empty;
            MovingParameter = false;
            upDown_1Asama_NesneSayisi.Value = 0;
            upDown_2Asama_NesneSayisi.Value = 0;
            drawingPanel.Invalidate();
        }





        #region Simulation of Item Placement Sequence to Show the User Methods
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
                    currentColumn = 1;
                    rectangles.Clear();
                }
            }
            else if (Parameter == "Yukarı Doğru")
            {
                drawingPanel.Invalidate();
                currentRow--;
                if (currentRow == -1)
                {
                    currentRow = rowCount;
                    rectangles.Clear();
                }
            }
            else if (Parameter == "Ortadan Yukarı Doğru")
            {
                drawingPanel.Invalidate();
                currentRow--;
                if (currentRow == -1)
                {
                    if (rowCount % 2 == 0)
                    {
                        currentRow = rowCount / 2;
                    }
                    else
                    {
                        currentRow = rowCount / 2 + 1;
                    }

                    rectangles.Clear();
                }
            }
            else if (Parameter == "Aşağı Doğru")
            {
                drawingPanel.Invalidate();
                currentRow++;
                if (currentRow == rowCount + 2)
                {
                    currentRow = 1;
                    rectangles.Clear();
                }
            }
            else if (Parameter == "Ortadan Aşağı Doğru")
            {
                drawingPanel.Invalidate();
                currentRow++;
                if (currentRow == rowCount + 2)
                {
                    currentRow = (rowCount / 2 + 1);
                    rectangles.Clear();
                }
            }
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
        private void radio_To_Right_CheckedChanged(object sender, EventArgs e)
        {
            timer.Stop();
            rectangles.Clear();
            if (radio_To_Right.Checked)
            {
                currentColumn = 1;
                timer.Interval = 300;
                timer.Start();
                Parameter = "Sağa Doğru";
            }
        }
        private void radio_To_Left_CheckedChanged(object sender, EventArgs e)
        {
            timer.Stop();
            rectangles.Clear();
            if (radio_To_Left.Checked)
            {
                currentColumn = colCount;
                timer.Interval = 300;
                timer.Start();
                Parameter = "Sola Doğru";
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
        private void radio_To_Up_CheckedChanged(object sender, EventArgs e)
        {
            timer.Stop();
            rectangles.Clear();
            if (radio_To_Up.Checked)
            {
                if (rowCount % 2 == 0)
                {
                    currentRow = rowCount / 2;
                }
                else
                {
                    currentRow = rowCount / 2 + 1;
                }
                drawingPanel.Invalidate();
                timer.Interval = 300;
                timer.Start();
                Parameter = "Ortadan Yukarı Doğru";
            }
        }
        private void radio_To_Down_CheckedChanged(object sender, EventArgs e)
        {
            timer.Stop();
            rectangles.Clear();
            if (radio_To_Down.Checked)
            {
                currentRow = rowCount / 2 + 1;
                drawingPanel.Invalidate();
                timer.Interval = 300;
                timer.Start();
                Parameter = "Ortadan Aşağı Doğru";
            }
        }
        private void radio_Start_From_Bottom_CheckedChanged(object sender, EventArgs e)
        {
            timer.Stop();
            rectangles.Clear();
            if (radio_Start_From_Bottom.Checked)
            {
                currentRow = rowCount + 1;
                timer.Interval = 300;
                timer.Start();
                Parameter = "Yukarı Doğru";
            }
        }
        private void radio_Start_From_Top_CheckedChanged(object sender, EventArgs e)
        {
            timer.Stop();
            rectangles.Clear();
            if (radio_Start_From_Top.Checked)
            {
                currentRow = 0;
                timer.Interval = 300;
                timer.Start();
                Parameter = "Aşağı Doğru";
            }
        }
        #endregion

        #endregion



        //Mainpanel events that Areas are drawed, MouseUp, MouseDown, MouseMove, Paint, Scroll
        #region DrawingPanel Events and Methods

        private void DrawInfoStrings(Graphics g, object obje)
        {
            SolidBrush BlueBrush = new SolidBrush(System.Drawing.Color.Blue);
            SolidBrush RedBrush = new SolidBrush(System.Drawing.Color.Red);

            string rightClick = "Sağ tık: ";
            string Del = "Del: ";
            string Copy = "Ctrl + C: ";
            string Paste = "Ctrl + V: ";

            string rightClickInfo = "Menüyü Aç";
            string DelInfo = "Sil";
            string CopyInfo = "Kopyala";
            string PasteInfo = "Yapıştır";

            SizeF textSizeRightClick = g.MeasureString(rightClick, Font);
            SizeF textSizeDel = g.MeasureString(Del, Font);
            SizeF textSizeCopy = g.MeasureString(Copy, Font);
            SizeF textSizePaste = g.MeasureString(Paste, Font);

            SizeF textSizeRightClickInfo = g.MeasureString(rightClickInfo, Font);
            SizeF textSizeDelInfo = g.MeasureString(DelInfo, Font);
            SizeF textSizeCopyInfo = g.MeasureString(CopyInfo, Font);
            SizeF textSizePasteInfo = g.MeasureString(PasteInfo, Font);

            PointF pointRightClick = new PointF(10, drawingPanel.ClientRectangle.Height - textSizeRightClick.Height);
            PointF pointRightClickInfo = new PointF(10 + textSizeRightClick.Width, drawingPanel.ClientRectangle.Height - textSizeRightClickInfo.Height);

            PointF pointDel = new PointF(10, drawingPanel.ClientRectangle.Height - textSizeRightClick.Height - textSizeDel.Height);
            PointF pointDelInfo = new PointF(10 + textSizeDel.Width, drawingPanel.ClientRectangle.Height - textSizeRightClickInfo.Height - textSizeDel.Height);

            PointF pointPaste = new PointF(10, drawingPanel.ClientRectangle.Height - textSizeRightClick.Height - textSizeDel.Height - textSizeCopy.Height);
            PointF pointPasteInfo = new PointF(10 + textSizeCopy.Width, drawingPanel.ClientRectangle.Height - textSizeRightClickInfo.Height - textSizeDelInfo.Height - textSizeCopyInfo.Height);

            PointF pointCopy = new PointF(10, drawingPanel.ClientRectangle.Height - textSizeRightClick.Height - textSizeDel.Height - textSizeCopy.Height - textSizePaste.Height);
            PointF pointCopyInfo = new PointF(10 + textSizePaste.Width, drawingPanel.ClientRectangle.Height - textSizeRightClickInfo.Height - textSizeDelInfo.Height - textSizeCopyInfo.Height - textSizePasteInfo.Height);

            if (obje is Ambar)
            {
                g.DrawString(rightClick, Font, RedBrush, pointRightClick);
                g.DrawString(rightClickInfo, Font, BlueBrush, pointRightClickInfo);
            }
            else if (obje is Depo || obje is Conveyor)
            {
                g.DrawString(rightClick, Font, RedBrush, pointRightClick);
                g.DrawString(rightClickInfo, Font, BlueBrush, pointRightClickInfo);

                g.DrawString(Del, Font, RedBrush, pointDel);
                g.DrawString(DelInfo, Font, BlueBrush, pointDelInfo);

                g.DrawString(Paste, Font, RedBrush, pointPaste);
                g.DrawString(PasteInfo, Font, BlueBrush, pointPasteInfo);

                g.DrawString(Copy, Font, RedBrush, pointCopy);
                g.DrawString(CopyInfo, Font, BlueBrush, pointCopyInfo);
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

                if (SelectedAmbar != null && Fill_WareHouse == false)
                {
                    g.DrawRectangle(SelectedAmbarPen, Ambar.Rectangle);

                    DrawInfoStrings(g, SelectedAmbar);
                }

                foreach (var conv in Ambar.conveyors)
                {
                    g.DrawRectangle(TransparentPen, conv.Rectangle);

                    if (selectedConveyor != null && selectedConveyor == conv &&
                        Fill_WareHouse == false)
                    {
                        g.DrawRectangle(SelectedConveyorPen, conv.Rectangle);

                        SelectedConveyorEdgePen.DashStyle = DashStyle.Dash;
                        DrawLeftLines(SelectedConveyorEdgePen, g, rects, conv.Rectangle,
                            Ambar.Rectangle);
                        DrawRightLines(SelectedConveyorEdgePen, g, rects, conv.Rectangle,
                            Ambar.Rectangle);
                        DrawTopLines(SelectedConveyorEdgePen, g, rects, conv.Rectangle,
                            Ambar.Rectangle);
                        DrawBottomLines(SelectedConveyorEdgePen, g, rects, conv.Rectangle,
                            Ambar.Rectangle);

                        DrawInfoStrings(g, selectedConveyor);
                    }
                    foreach (var reff in conv.ConveyorReferencePoints)
                    {
                        reff.Draw(g);
                    }
                }

                foreach (var depo in Ambar.depolar)
                {
                    g.DrawRectangle(TransparentPen, depo.Rectangle);

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
                        SelectedDepoEdgePen.DashStyle = DashStyle.Dash;

                        DrawLeftLines(SelectedDepoEdgePen, g, rects, depo.Rectangle, Ambar.Rectangle);
                        DrawRightLines(SelectedDepoEdgePen, g, rects, depo.Rectangle, Ambar.Rectangle);
                        DrawTopLines(SelectedDepoEdgePen, g, rects, depo.Rectangle, Ambar.Rectangle);
                        DrawBottomLines(SelectedDepoEdgePen, g, rects, depo.Rectangle, Ambar.Rectangle);

                        DrawInfoStrings(g, selectedDepo);
                    }
                }
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
                                        rect = GetSimulationRectangle(cell);
                                        rectangles.Add(rect);
                                    }

                                    if ((currentColumn >= 0 || currentColumn <= depo.ColumnCount) &&
                                        Parameter == "Sağa Doğru" && cell.Column + 1 == currentColumn)
                                    {
                                        rect = GetSimulationRectangle(cell);
                                        rectangles.Add(rect);
                                    }

                                    if ((currentRow >= 0 || currentRow <= depo.RowCount) &&
                                        Parameter == "Yukarı Doğru" && cell.Row == currentRow)
                                    {
                                        rect = GetSimulationRectangle(cell);
                                        rectangles.Add(rect);
                                    }

                                    if ((currentRow >= 0 || currentRow <= depo.RowCount) &&
                                        Parameter == "Ortadan Yukarı Doğru" && cell.Row == currentRow)
                                    {
                                        rect = GetSimulationRectangle(cell);
                                        rectangles.Add(rect);
                                    }


                                    if ((currentRow >= 0 || currentRow <= depo.RowCount) &&
                                        Parameter == "Aşağı Doğru" && cell.Row == currentRow)
                                    {
                                        rect = GetSimulationRectangle(cell);
                                        rectangles.Add(rect);
                                    }

                                    if ((currentRow >= 0 || currentRow <= depo.RowCount) &&
                                        Parameter == "Ortadan Aşağı Doğru" && cell.Row == currentRow)
                                    {
                                        rect = GetSimulationRectangle(cell);
                                        rectangles.Add(rect);
                                    }


                                    if ((Parameter == "Sağa Doğru" ||
                                            Parameter == "Sola Doğru") &&
                                            cell.Column == currentColumn)
                                    {
                                        DrawArrowInCell(g, cell.Rectangle);
                                    }

                                    if ((Parameter == "Aşağı Doğru" || Parameter == "Ortadan Aşağı Doğru") && cell.Row - 1 == currentRow)
                                    {
                                        DrawArrowInCell(g, cell.Rectangle);
                                    }

                                    if ((Parameter == "Yukarı Doğru" || Parameter == "Ortadan Yukarı Doğru") && cell.Row + 1 == currentRow)
                                    {
                                        DrawArrowInCell(g, cell.Rectangle);
                                    }
                                }
                            }
                            using (Pen pen = new Pen(System.Drawing.Color.Gray, 3))
                            {
                                foreach (var rect1 in rectangles)
                                {
                                    g.DrawRectangle(pen, rect1);
                                }
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
        private static RectangleF GetSimulationRectangle(Models.Cell cell)
        {
            RectangleF rect;
            float x = cell.Rectangle.X;
            float y = cell.Rectangle.Y;
            float width = (float)
                (cell.Rectangle.Width / 2 * 1.5);
            float height = (float)
                (cell.Rectangle.Height / 2 * 1.5);

            rect = new RectangleF(cell.Rectangle.X +
                (cell.Rectangle.Width / 2 - width / 2),
                cell.Rectangle.Y + (cell.Rectangle.Height / 2 -
                height / 2), width, height);
            return rect;
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
        private void drawingPanel_MouseDown(object sender, MouseEventArgs e)
        {
            System.Drawing.Point scaledPoint = new System.Drawing.Point((int)((e.X - drawingPanel.AutoScrollPosition.X)),
                    (int)((e.Y - drawingPanel.AutoScrollPosition.Y)));
            bool deponull = false;
            bool conveyornull = false;
            bool ambarnull = false;

            if (Ambar != null)
            {
                if (!Manuel_Move && !AddReferencePoint)
                {
                    if (Ambar.Rectangle.Contains(scaledPoint))
                    {
                        ambarnull = true;
                    }

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

                            if (CountList.Count > 0)
                            {
                                foreach (var depo in Ambar.depolar)
                                {
                                    if (depo.Yerlestirilme_Sirasi == 0)
                                    {
                                        int num = CountList.FirstOrDefault();
                                        depo.Yerlestirilme_Sirasi = num;

                                        CountList.Remove(num);
                                    }
                                }
                            }
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

                    Ambar.OnMouseDown(e);

                    if (!ambarnull || (deponull || conveyornull))
                    {
                        SelectedAmbar = null;
                        SelectedAmbarPen.Width = 2;
                        SelectedAmbarPen.Color = System.Drawing.Color.Black;
                    }

                    if (!conveyornull && !deponull && SelectedAmbar == null)
                    {
                        menuProcess = false;
                        if (LeftSide_LayoutPanel.Visible)
                        {
                            MainPanelCloseLeftSide(LeftSide_LayoutPanel, this);
                            SortFlowLayoutPanel(LayoutPanel_Alan_Hierarchy);
                            UnselectNodes();
                        }
                    }
                }
                drawingPanel.Invalidate();
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

        #endregion



        //Snapping and Drawing Lines when left clicked to the depos or conveyors
        #region Snap and Drawing Methods

        public RectangleF SnapRectangles(RectangleF rectangle, RectangleF refRectangle)
        {
            System.Drawing.PointF rectangleRightMiddle = GVisual.GetMiddleOfRightEdgeF(rectangle);
            System.Drawing.PointF rectangleLeftMiddle = GVisual.GetMiddleOfLeftEdgeF(rectangle);
            System.Drawing.PointF rectangleTopMiddle = GVisual.GetMiddleOfTopEdgeF(rectangle);
            System.Drawing.PointF rectangleBotMiddle = GVisual.GetMiddleOfBottomEdgeF(rectangle);
            System.Drawing.PointF rectangleTopLeftCorner = GVisual.GetTopLeftCornerF(rectangle);
            System.Drawing.PointF rectangleTopRightCorner = GVisual.GetTopRightCornerF(rectangle);
            System.Drawing.PointF rectangleBotLeftCorner = GVisual.GetBottomLeftCornerF(rectangle);
            System.Drawing.PointF rectangleBotRightCorner = GVisual.GetBottomRightCornerF(rectangle);


            System.Drawing.PointF refRectangleRightMiddle = GVisual.GetMiddleOfRightEdgeF(refRectangle);
            System.Drawing.PointF refRectangleLeftMiddle = GVisual.GetMiddleOfLeftEdgeF(refRectangle);
            System.Drawing.PointF refRectangleTopMiddle = GVisual.GetMiddleOfTopEdgeF(refRectangle);
            System.Drawing.PointF refRectangleBotMiddle = GVisual.GetMiddleOfBottomEdgeF(refRectangle);
            System.Drawing.PointF refRectangleTopLeftCorner = GVisual.GetTopLeftCornerF(refRectangle);
            System.Drawing.PointF refRectangleTopRightCorner = GVisual.GetTopRightCornerF(refRectangle);
            System.Drawing.PointF refRectangleBotLeftCorner = GVisual.GetBottomLeftCornerF(refRectangle);
            System.Drawing.PointF refRectangleBotRightCorner = GVisual.GetBottomRightCornerF(refRectangle);


            //Distance Between Bottom Left of the Rectangle and Top Left of the Reference Rectangle
            double TopLeftCornerSnapDistance = GVisual.CalculateDistance(rectangleBotLeftCorner.X,
                rectangleBotLeftCorner.Y, refRectangleTopLeftCorner.X,
                refRectangleTopLeftCorner.Y);

            //Distance Between Middle Right of the Rectangle and Middle Left of the Reference Rectangle
            double LeftSideSnapDistance = GVisual.CalculateDistance(rectangleRightMiddle.X,
                rectangleRightMiddle.Y, refRectangleLeftMiddle.X, refRectangleLeftMiddle.Y);

            //Distance Between Top Left of the Rectangle and Bottom Left of the Reference Rectangle
            double BotLeftCornerSnapDistance = GVisual.CalculateDistance(rectangleTopLeftCorner.X,
                rectangleTopLeftCorner.Y, refRectangleBotLeftCorner.X,
                refRectangleBotLeftCorner.Y);

            //Distance Between Bottom Right of the Rectangle and Top Right of the Reference Rectangle
            double TopRightCornerSnapDistance = GVisual.CalculateDistance(rectangleBotRightCorner.X,
                rectangleBotRightCorner.Y, refRectangleTopRightCorner.X, refRectangleTopRightCorner.Y);

            //Distance Between Middle Left of the Rectangle and Middle Right of the Reference Rectangle
            double RightSideSnapDistance = GVisual.CalculateDistance(rectangleLeftMiddle.X,
                rectangleLeftMiddle.Y, refRectangleRightMiddle.X, refRectangleRightMiddle.Y);

            //Distance Between Top Right of the Rectangle and Bot Right of the Reference Rectangle
            double BotRightCornerSnapDistance = GVisual.CalculateDistance(rectangleTopRightCorner.X,
                rectangleTopRightCorner.Y, refRectangleBotRightCorner.X, refRectangleBotRightCorner.Y);

            //Distance Between Bottom Middle of the Rectangle and Top Middle of the Reference Rectangle
            double TopSideSnapDistance = GVisual.CalculateDistance(rectangleBotMiddle.X,
                rectangleBotMiddle.Y, refRectangleTopMiddle.X, refRectangleTopMiddle.Y);

            //Distance Between Top Middle of the Rectangle and Bottom Middle of the Reference Rectangle
            double BotSideSnapDistance = GVisual.CalculateDistance(rectangleTopMiddle.X,
                rectangleTopMiddle.Y, refRectangleBotMiddle.X, refRectangleBotMiddle.Y);

            //Distance Between Bottom Left of the Rectangle and Bottom Right of the Reference Rectangle
            double RightBotSnapDistance = GVisual.CalculateDistance(rectangleBotLeftCorner.X,
                rectangleBotLeftCorner.Y, refRectangleBotRightCorner.X, refRectangleBotRightCorner.Y);

            //Distance Between Top Left of the Rectangle and Top Right of the Reference Rectangle
            double RightTopSnapDistance = GVisual.CalculateDistance(rectangleTopLeftCorner.X,
                rectangleTopLeftCorner.Y, refRectangleTopRightCorner.X, refRectangleTopRightCorner.Y);

            //Distance Between Bottom Right of the Rectangle and Bottom Left of the Reference Rectangle
            double LeftBotSnapDistance = GVisual.CalculateDistance(rectangleBotRightCorner.X,
                rectangleBotRightCorner.Y, refRectangleBotLeftCorner.X, refRectangleBotLeftCorner.Y);

            //Distance Between Top Right of the Rectangle and Top Left of the Reference Rectangle
            double LeftTopSnapDistance = GVisual.CalculateDistance(rectangleTopRightCorner.X,
                rectangleTopRightCorner.Y, refRectangleTopLeftCorner.X, refRectangleTopLeftCorner.Y);

            int snapping = 6;

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
                double rightTopBotLeftDistance = GVisual.CalculateDistance(rectangleTopRightCorner.X, rectangleTopRightCorner.Y,
                    refRectangleBotLeftCorner.X, refRectangleBotLeftCorner.Y);

                double leftTopBotRightDistance = GVisual.CalculateDistance(rectangleTopLeftCorner.X, rectangleTopLeftCorner.Y,
                    refRectangleBotRightCorner.X, refRectangleBotRightCorner.Y);

                double rightBotTopLeftDistance = GVisual.CalculateDistance(rectangleBotRightCorner.X, rectangleBotRightCorner.Y,
                    refRectangleTopLeftCorner.X, refRectangleTopLeftCorner.Y);

                double leftBotTopRightDistance = GVisual.CalculateDistance(rectangleBotLeftCorner.X, rectangleBotLeftCorner.Y,
                    refRectangleTopRightCorner.X, refRectangleTopRightCorner.Y);

                PointF refRectangleBottomSideLeftCorner = GVisual.GetCloseBottomSideLeftCornerF(refRectangle);
                PointF refRectangleBottomLeftSideCorner = GVisual.GetCloseBottomLeftSideCornerF(refRectangle);

                PointF refRectangleBottomSideRightCorner = GVisual.GetCloseBottomSideRightCornerF(refRectangle);
                PointF refRectangleBottomRightSideCorner = GVisual.GetCloseBottomRightSideCornerF(refRectangle);

                PointF refRectangleTopSideRightCorner = GVisual.GetCloseTopSideRightCornerF(refRectangle);
                PointF refRectangleTopRightSideCorner = GVisual.GetCloseTopRightSideCornerF(refRectangle);

                PointF refRectangleTopSideLeftCorner = GVisual.GetCloseTopSideLeftCornerF(refRectangle);
                PointF refRectangleTopLeftSideCorner = GVisual.GetCloseTopLeftSideCornerF(refRectangle);


                if (refRectangle.Contains(rectangleTopRightCorner) &&
                    !rectangle.Contains(refRectangleBottomSideRightCorner) &&
                    !refRectangle.Contains(rectangleTopLeftCorner))
                {
                    if (rightTopBotLeftDistance <= snapping)
                    {
                        rectangle = GVisual.MoveRectangleToPoint(rectangle,
                        refRectangle.X - rectangle.Width,
                        refRectangle.Y + refRectangle.Height);
                        snapped = true;
                        return rectangle;
                    }
                    else
                    {
                        if (rectangle.X <= refRectangle.X && rectangle.Y >= refRectangle.Y && rectangle.Y <= refRectangle.Bottom)
                        {
                            rectangle = GVisual.MoveRectangleToPoint(rectangle, refRectangle.X - rectangle.Width, rectangle.Y);
                            return rectangle;
                        }
                        else if (rectangle.Y >= refRectangle.Bottom)
                        {
                            rectangle = GVisual.MoveRectangleToPoint(rectangle, rectangle.X,
                            refRectangle.Y + refRectangle.Height);
                            return rectangle;
                        }
                    }
                }

                else if (refRectangle.Contains(rectangleTopLeftCorner) &&
                    !rectangle.Contains(refRectangleBottomSideRightCorner) &&
                    !refRectangle.Contains(rectangleTopRightCorner))
                {
                    if (leftTopBotRightDistance <= snapping)
                    {
                        rectangle = GVisual.MoveRectangleToPoint(rectangle,
                        refRectangle.X + refRectangle.Width,
                        refRectangle.Y + refRectangle.Height);
                        snapped = true;
                        return rectangle;
                    }
                    else
                    {
                        if (rectangle.X >= refRectangle.Right - snapping && rectangle.Y >= refRectangle.Y && rectangle.Y <= refRectangle.Bottom + snapping)
                        {
                            rectangle = GVisual.MoveRectangleToPoint(rectangle, refRectangle.X + refRectangle.Width, rectangle.Y);
                            return rectangle;
                        }
                        else if (rectangle.Y >= refRectangle.Bottom - snapping)
                        {
                            rectangle = GVisual.MoveRectangleToPoint(rectangle, rectangle.X,
                            refRectangle.Y + refRectangle.Height);
                            return rectangle;
                        }
                    }
                }

                else if (refRectangle.Contains(rectangleTopMiddle) &&
                    (refRectangle.Contains(rectangleTopRightCorner) ||
                    refRectangle.Contains(rectangleTopLeftCorner)))
                {

                    rectangle = GVisual.MoveRectangleToPoint(rectangle, rectangle.X, refRectangle.Y + refRectangle.Height);
                    return rectangle;
                }

                else if (refRectangle.Contains(rectangleBotRightCorner) &&
                    !rectangle.Contains(refRectangleTopSideLeftCorner) &&
                    !refRectangle.Contains(rectangleBotLeftCorner))
                {
                    if (rightBotTopLeftDistance <= snapping)
                    {
                        rectangle = GVisual.MoveRectangleToPoint(rectangle,
                        refRectangle.X - rectangle.Width,
                        refRectangle.Y - rectangle.Height);
                        snapped = true;
                        return rectangle;
                    }
                    else
                    {
                        if (rectangle.X <= refRectangle.X && rectangle.Y <= refRectangle.Y && rectangle.Bottom >= refRectangle.Top)
                        {
                            rectangle = GVisual.MoveRectangleToPoint(rectangle, refRectangle.X - rectangle.Width, rectangle.Y);
                            return rectangle;
                        }
                        else if (rectangle.Bottom <= refRectangle.Y + snapping)
                        {
                            rectangle = GVisual.MoveRectangleToPoint(rectangle, rectangle.X,
                            refRectangle.Y - rectangle.Height);
                            return rectangle;
                        }
                    }
                }

                else if (refRectangle.Contains(rectangleBotLeftCorner) &&
                    !rectangle.Contains(refRectangleTopSideRightCorner) &&
                    !refRectangle.Contains(rectangleBotRightCorner))
                {
                    if (leftBotTopRightDistance <= snapping)
                    {
                        rectangle = GVisual.MoveRectangleToPoint(rectangle,
                        refRectangle.X + refRectangle.Width,
                        refRectangle.Y - rectangle.Height);
                        snapped = true;
                        return rectangle;
                    }
                    else
                    {
                        if (rectangle.X >= refRectangle.X && rectangle.Y <= refRectangle.Y && rectangle.Bottom >= refRectangle.Top)
                        {
                            rectangle = GVisual.MoveRectangleToPoint(rectangle, refRectangle.X + refRectangle.Width, rectangle.Y);
                            return rectangle;
                        }
                        else if (rectangle.Bottom <= refRectangle.Y + snapping)
                        {
                            rectangle = GVisual.MoveRectangleToPoint(rectangle, rectangle.X,
                            refRectangle.Y - rectangle.Height);
                            return rectangle;
                        }
                    }
                }

                else if (refRectangle.Contains(rectangleBotMiddle) ||
                    refRectangle.Contains(rectangleBotLeftCorner) ||
                    refRectangle.Contains(rectangleBotRightCorner) ||
                    rectangle.Contains(refRectangleTopSideRightCorner) ||
                    rectangle.Contains(refRectangleTopSideLeftCorner))
                {

                    rectangle = GVisual.MoveRectangleToPoint(rectangle, rectangle.X, refRectangle.Y - rectangle.Height);
                    return rectangle;
                }

                else if (refRectangle.Contains(rectangleTopMiddle) ||
                    refRectangle.Contains(rectangleTopLeftCorner) ||
                    refRectangle.Contains(rectangleTopRightCorner) ||
                    rectangle.Contains(refRectangleBottomSideRightCorner) ||
                    rectangle.Contains(refRectangleBottomSideLeftCorner))
                {
                    rectangle = GVisual.MoveRectangleToPoint(rectangle, rectangle.X, refRectangle.Y + refRectangle.Height);
                    return rectangle;
                }

                else if (refRectangle.Contains(rectangleLeftMiddle) ||
                    refRectangle.Contains(rectangleTopLeftCorner) ||
                    refRectangle.Contains(rectangleBotLeftCorner) ||
                    rectangle.Contains(refRectangleBottomLeftSideCorner) ||
                    rectangle.Contains(refRectangleTopLeftSideCorner))
                {
                    rectangle = GVisual.MoveRectangleToPoint(rectangle, refRectangle.X - rectangle.Width, rectangle.Y);
                    return rectangle;
                }

                else if (refRectangle.Contains(rectangleRightMiddle) ||
                    refRectangle.Contains(rectangleTopRightCorner) ||
                    refRectangle.Contains(rectangleBotRightCorner) ||
                    rectangle.Contains(refRectangleBottomRightSideCorner) ||
                    rectangle.Contains(refRectangleTopRightSideCorner))
                {
                    rectangle = GVisual.MoveRectangleToPoint(rectangle, refRectangle.X + refRectangle.Width, rectangle.Y);
                    return rectangle;
                }
                return rectangle;
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

        #endregion



        //Change Size and Location events
        #region Change Size and Location Algorithm
        //Yerini ve Boyutunu Değiştir Textbox Events and Methods
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
                                nesne_Yuksekligi = DataCell.NesneYuksekligi;
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
                                yatay_kenar_boslugu, nesne_eni, nesne_boyu, nesne_Yuksekligi);

                            colCount = selectedDepo.ColumnCount;
                            rowCount = selectedDepo.RowCount;

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
                                nesne_Yuksekligi = DataCell.NesneYuksekligi;
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
                               yatay_kenar_boslugu, nesne_eni, nesne_boyu, nesne_Yuksekligi);

                            colCount = selectedDepo.ColumnCount;
                            rowCount = selectedDepo.RowCount;

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
                                DataCell.NesneBoyu, DataCell.NesneYuksekligi);
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
                                DataCell.NesneBoyu, DataCell.NesneYuksekligi);
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
                                DataCell.NesneBoyu, DataCell.NesneYuksekligi);
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
                                DataCell.NesneBoyu, DataCell.NesneYuksekligi);
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



        //Change Size and Location of Depo and Conveyor Button Events
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
                    SelectedDepoPen.Color = System.Drawing.Color.Black;
                    SelectedDepoPen.Width = 1;
                    SelectedDepoEdgePen.Width = 1;
                    Manuel_Move = false;
                    if (menuProcess)
                    {
                        SortFlowLayoutPanel(LayoutPanel_SelectedDepo);
                        Show_DepoMenus("Depo");
                        menuProcess = false;
                    }
                    else
                    {
                        MainPanelCloseLeftSide(LeftSide_LayoutPanel, this);
                        Clear_AddControltoLeftSidePanel(LayoutPanel_Alan_Hierarchy);
                        selectedDepo = null;
                    }
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

                    if (menuProcess)
                    {
                        SortFlowLayoutPanel(LayoutPanel_SelectedDepo);
                        Show_DepoMenus("Depo");
                        menuProcess = false;
                    }
                    else
                    {
                        MainPanelCloseLeftSide(LeftSide_LayoutPanel, this);
                        Clear_AddControltoLeftSidePanel(LayoutPanel_Alan_Hierarchy);
                        selectedDepo = null;
                    }

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
                    SelectedConveyorPen.Color = System.Drawing.Color.Black;
                    SelectedConveyorPen.Width = 1;
                    SelectedConveyorEdgePen.Width = 1;
                    Manuel_Move = false;

                    if (menuProcess)
                    {
                        SortFlowLayoutPanel(layoutPanel_SelectedConveyor);
                        Show_ConveyorMenus("Conveyor");
                        menuProcess = false;
                    }
                    else
                    {
                        MainPanelCloseLeftSide(LeftSide_LayoutPanel, this);
                        Clear_AddControltoLeftSidePanel(LayoutPanel_Alan_Hierarchy);
                        selectedConveyor = null;
                    }

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
                    SelectedConveyorPen.Color = System.Drawing.Color.Black;
                    SelectedConveyorPen.Width = 1;
                    SelectedConveyorEdgePen.Width = 1;

                    if (menuProcess)
                    {
                        SortFlowLayoutPanel(LayoutPanel_SelectedDepo);
                        Show_DepoMenus("Depo");
                        menuProcess = false;
                    }
                    else
                    {
                        MainPanelCloseLeftSide(LeftSide_LayoutPanel, this);
                        Clear_AddControltoLeftSidePanel(LayoutPanel_Alan_Hierarchy);
                        selectedConveyor = null;
                    }

                    drawingPanel.Invalidate();
                }
            }
        }
        private void btn_Padding_Vazgeç_Click(object sender, EventArgs e)
        {
            if (selectedDepo != null)
            {
                selectedDepo.Rectangle = UnchangedselectedDepoRectangle;
                selectedDepo.DepoAlaniEni = UnchangedDepoAlaniEni;
                selectedDepo.DepoAlaniBoyu = UnchangedDepoAlaniBoyu;
                selectedDepo.Cm_Width = UnchangedDepoAlaniEni * 100;
                selectedDepo.Cm_Height = UnchangedDepoAlaniBoyu * 100;
                selectedDepo.OriginalRectangle = selectedDepo.Rectangle;
                selectedDepo.LocationofRect = new System.Drawing.Point((int)selectedDepo.Rectangle.X, (int)selectedDepo.Rectangle.Y);
                SelectedDepoPen.Color = System.Drawing.Color.Black;
                SelectedDepoPen.Width = 1;
                SelectedDepoEdgePen.Width = 1;
                Manuel_Move = false;

                if (menuProcess)
                {
                    SortFlowLayoutPanel(LayoutPanel_SelectedDepo);
                    Show_DepoMenus("Depo SubMenu");
                    menuProcess = false;
                }
                else
                {
                    Clear_AddControltoLeftSidePanel(LayoutPanel_Alan_Hierarchy);
                    MainPanelCloseLeftSide(LeftSide_LayoutPanel, this);
                    selectedDepo = null;
                }
                drawingPanel.Invalidate();
            }
            else if (selectedConveyor != null)
            {
                selectedConveyor.Rectangle = UnchangedselectedConveyorRectangle;
                selectedConveyor.ConveyorEni = UnchangedConveyorEni;
                selectedConveyor.ConveyorBoyu = UnchangedConveyorBoyu;
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
                Manuel_Move = false;

                if (menuProcess)
                {
                    SortFlowLayoutPanel(layoutPanel_SelectedConveyor);
                    Show_ConveyorMenus("Conveyor SubMenu");
                    menuProcess = false;
                }
                else
                {
                    Clear_AddControltoLeftSidePanel(LayoutPanel_Alan_Hierarchy);
                    MainPanelCloseLeftSide(LeftSide_LayoutPanel, this);
                    selectedConveyor = null;
                }
                drawingPanel.Invalidate();
            }
        }
        private void btn_PaddingPanel_Kapat_Click(object sender, EventArgs e)
        {
            if (LeftSide_LayoutPanel.Visible && LeftSide_LayoutPanel.Controls.Contains(PaddingPanel) &&
                selectedConveyor != null)
            {
                SortFlowLayoutPanel(layoutPanel_SelectedConveyor);
                Show_ConveyorMenus("Conveyor");
                Manuel_Move = false;
                menuProcess = false;
            }
            else if (LeftSide_LayoutPanel.Visible && LeftSide_LayoutPanel.Controls.Contains(PaddingPanel) &&
                selectedDepo != null)
            {
                SortFlowLayoutPanel(LayoutPanel_SelectedDepo);
                Show_DepoMenus("Depo");
                Manuel_Move = false;
                menuProcess = false;
            }
            else if (this.Controls.Contains(PaddingPanel))
            {
                GVisual.HideControl(PaddingPanel, this);
                MainPanelMakeBigger(drawingPanel);
                MoveRight();
            }
        }



        //Change Size and Location of Depo Events
        private void deponunYeriniVeBoyutunuDeğiştirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!LeftSide_LayoutPanel.Visible)
            {
                MainPanelOpenLeftSide(LeftSide_LayoutPanel, this, leftSidePanelLocation);
                Clear_AddControltoLeftSidePanel(PaddingPanel);
            }
            else
            {
                Clear_AddControltoLeftSidePanel(PaddingPanel);
            }

            ShowTextboxes(txt_Width.Location, txt_Height.Location,
                    txt_Left_Padding.Location, txt_Right_Padding.Location,
                    txt_Top_Padding.Location, txt_Bottom_Padding.Location);
            ShowMoveCheckButton();
            Manuel_Move = true;

            if (selectedDepo != null)
            {
                UnchangedselectedDepoRectangle = selectedDepo.Rectangle;
                ManuelDepoRectangle = selectedDepo.Rectangle;
                UnchangedDepoAlaniBoyu = selectedDepo.DepoAlaniBoyu;
                UnchangedDepoAlaniEni = selectedDepo.DepoAlaniEni;
                //txt_width.text = $"{unchangeddepoalanieni}";
                //txt_height.text = $"{unchangeddepoalaniboyu}";
            }
        }
        private void btn_Depo_SubMenu_Yerini_Boyutunu_Degistir_Click(object sender, EventArgs e)
        {
            if (selectedDepo != null)
            {
                menuProcess = true;
                Clear_AddControltoLeftSidePanel(PaddingPanel);
                btn_PaddingPanel_Kapat.Image = Resources.Resource1.Go_Back;

                ShowTextboxes(txt_Width.Location, txt_Height.Location,
                    txt_Left_Padding.Location, txt_Right_Padding.Location,
                    txt_Top_Padding.Location, txt_Bottom_Padding.Location);
                ShowMoveCheckButton();
                Manuel_Move = true;

                UnchangedselectedDepoRectangle = selectedDepo.Rectangle;
                ManuelDepoRectangle = selectedDepo.Rectangle;
                UnchangedDepoAlaniBoyu = selectedDepo.DepoAlaniBoyu;
                UnchangedDepoAlaniEni = selectedDepo.DepoAlaniEni;
                //txt_width.text = $"{unchangeddepoalanieni}";
                //txt_height.text = $"{unchangeddepoalaniboyu}";
                drawingPanel.Invalidate();
            }
        }



        //Change Size and Location of Conveyor Events
        private void Conveyor_yeriniVeBoyutunuDeğiştirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!LeftSide_LayoutPanel.Visible)
            {
                MainPanelOpenLeftSide(LeftSide_LayoutPanel, this, leftSidePanelLocation);
                Clear_AddControltoLeftSidePanel(PaddingPanel);
            }
            else
            {
                Clear_AddControltoLeftSidePanel(PaddingPanel);
            }

            ShowTextboxes(txt_Width.Location, txt_Height.Location,
                    txt_Left_Padding.Location, txt_Right_Padding.Location,
                    txt_Top_Padding.Location, txt_Bottom_Padding.Location);
            ShowMoveCheckButton();
            Manuel_Move = true;
            if (selectedConveyor != null)
            {
                UnchangedselectedConveyorRectangle = selectedConveyor.Rectangle;
                ManuelConveyorRectangle = selectedConveyor.Rectangle;
                UnchangedConveyorEni = selectedConveyor.ConveyorEni;
                UnchangedConveyorBoyu = selectedConveyor.ConveyorBoyu;
                //txt_Width.Text = $"{UnchangedConveyorEni}";
                //txt_Height.Text = $"{UnchangedConveyorBoyu}";
            }
        }
        private void btn_Conveyor_SubMenu_Yerini_Boyutunu_Degistir_Click(object sender, EventArgs e)
        {
            if (selectedConveyor != null)
            {
                menuProcess = true;
                Clear_AddControltoLeftSidePanel(PaddingPanel);
                btn_PaddingPanel_Kapat.Image = Resources.Resource1.Go_Back;

                ShowTextboxes(txt_Width.Location, txt_Height.Location,
                    txt_Left_Padding.Location, txt_Right_Padding.Location,
                    txt_Top_Padding.Location, txt_Bottom_Padding.Location);
                ShowMoveCheckButton();
                Manuel_Move = true;

                if (selectedConveyor != null)
                {
                    UnchangedselectedConveyorRectangle = selectedConveyor.Rectangle;
                    ManuelConveyorRectangle = selectedConveyor.Rectangle;
                    UnchangedConveyorEni = selectedConveyor.ConveyorEni;
                    UnchangedConveyorBoyu = selectedConveyor.ConveyorBoyu;
                    //txt_Width.Text = $"{UnchangedConveyorEni}";
                    //txt_Height.Text = $"{UnchangedConveyorBoyu}";
                }
                drawingPanel.Invalidate();
            }
        }


        #endregion



        //Area Creation events
        #region Area Creation Button Events

        //RightSide Area Creation Panel Button Events
        private void btn_Alan_Click(object sender, EventArgs e)
        {
            if (RightSide_LayoutPanel.Visible)
            {
                if (!Alan_Olusturma_Paneli.Visible)
                {
                    ShowControl(RightSide_LayoutPanel, Alan_Olusturma_Paneli);
                }
                RightSide_LayoutPanel.ScrollControlIntoView(Alan_Olusturma_Paneli);
            }
            else
            {
                MainPanelOpenRightSide(RightSide_LayoutPanel, this, rightSidePanelLocation);
                if (!Alan_Olusturma_Paneli.Visible)
                {
                    ShowControl(RightSide_LayoutPanel, Alan_Olusturma_Paneli);
                }
                RightSide_LayoutPanel.ScrollControlIntoView(Alan_Olusturma_Paneli);
            }
        }
        private void btn_Alan_Olustur_Click(object sender, EventArgs e)
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

                        if (layout != null)
                        {
                            ambar.LayoutId = layout.LayoutId;
                        }

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
                        MainPanelCloseRightSide(RightSide_LayoutPanel, this);
                        AlanNode.Tag = ambar;
                        drawingPanel.Invalidate();
                    }
                }
                else
                {
                    MainPanelCloseRightSide(RightSide_LayoutPanel, this);
                    drawingPanel.Invalidate();
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

                    if (layout != null)
                    {
                        ambar.LayoutId = layout.LayoutId;
                    }

                    ambar.Rectangle = GVisual.RatioRectangleToPanel(drawingPanel, ambar.Rectangle);
                    System.Drawing.Point point = GVisual.GetCenter(drawingPanel.ClientRectangle);
                    ambar.Rectangle = new RectangleF(point.X - ambar.Rectangle.Width / 2,
                        point.Y - ambar.Rectangle.Height / 2, ambar.Rectangle.Width, ambar.Rectangle.Height);
                    ambar.OriginalRectangle = ambar.Rectangle;
                    ambar.AmbarEni = alan_eni;
                    ambar.AmbarBoyu = alan_boyu;
                    ambar.LocationofRect = new System.Drawing.Point((int)ambar.Rectangle.X, (int)ambar.Rectangle.Y);
                    Ambar = ambar;
                    MainPanelCloseRightSide(RightSide_LayoutPanel, this);
                    AlanNode.Tag = ambar;
                    drawingPanel.Invalidate();
                }
            }
        }
        private void btn_Alan_Olustur_Vazgec_Click(object sender, EventArgs e)
        {
            MainPanelCloseRightSide(RightSide_LayoutPanel, this);
            ambar_Boyut_Degistir = false;
        }
        private void btn_Alan_Olusturma_Panelini_Kapat_Click(object sender, EventArgs e)
        {
            HideControl(RightSide_LayoutPanel, Alan_Olusturma_Paneli);
            ambar_Boyut_Degistir = false;
        }




        //LeftSide Area Change Size Panel Button Events
        private void btn_Left_Alan_Boyut_Degistir_Click(object sender, EventArgs e)
        {
            if (!menuProcess && Ambar != null && SelectedAmbar != null)
            {
                errorProvider.SetError(txt_Left_Alan_Eni, string.Empty);
                errorProvider.SetError(txt_Left_Alan_Boyu, string.Empty);
                errorProvider.Clear();

                float alan_eni = StrLib.ReplaceDotWithCommaReturnFloat(txt_Left_Alan_Eni, errorProvider,
                    "Bu alan boş bırakılamaz.", "Lütfen buraya bir sayı giriniz.", "Buraya 0 ya da daha küçük bir değer giremezsiniz.");
                float alan_boyu = StrLib.ReplaceDotWithCommaReturnFloat(txt_Left_Alan_Boyu, errorProvider,
                    "Bu alan boş bırakılamaz.", "Lütfen buraya bir sayı giriniz.", "Buraya 0 ya da daha küçük bir değer giremezsiniz.");

                if (!errorProvider.HasErrors)
                {
                    Ambar ambar = new Ambar(0, 0, alan_eni, alan_boyu, Main, this);

                    if (layout != null)
                    {
                        ambar.LayoutId = layout.LayoutId;
                    }

                    ambar.Rectangle = GVisual.RatioRectangleToPanel(drawingPanel, ambar.Rectangle);
                    System.Drawing.Point point = GVisual.GetCenter(drawingPanel.ClientRectangle);
                    ambar.Rectangle = new RectangleF(point.X - ambar.Rectangle.Width / 2,
                        point.Y - ambar.Rectangle.Height / 2, ambar.Rectangle.Width, ambar.Rectangle.Height);
                    ambar.OriginalRectangle = ambar.Rectangle;
                    ambar.AmbarEni = alan_eni;
                    ambar.AmbarBoyu = alan_boyu;
                    ambar.LocationofRect = new System.Drawing.Point((int)ambar.Rectangle.X, (int)ambar.Rectangle.Y);

                    foreach (var depo in Ambar.depolar)
                    {
                        depo.Parent = ambar;
                        depo.Ambar = ambar;
                        ambar.depolar.Add(depo);
                    }
                    foreach (var conveyor in Ambar.conveyors)
                    {
                        conveyor.Parent = ambar;
                        conveyor.Ambar = ambar;
                        ambar.conveyors.Add(conveyor);
                    }

                    Ambar = ambar;
                    ambar_Boyut_Degistir = false;

                    if (!LeftSide_LayoutPanel.Visible)
                    {
                        MainPanelOpenLeftSide(LeftSide_LayoutPanel, this, leftSidePanelLocation);
                        SortFlowLayoutPanel(layoutPanel_Ambar);
                        Show_AreaMenus("Alan SubMenu");
                    }
                    else
                    {
                        SortFlowLayoutPanel(layoutPanel_Ambar);
                        Show_AreaMenus("Alan SubMenu");
                    }

                    AlanNode.Tag = ambar;

                    drawingPanel.Invalidate();
                }
            }
            else if (menuProcess && SelectedAmbar != null && Ambar != null)
            {
                errorProvider.SetError(txt_Alan_Eni, string.Empty);
                errorProvider.SetError(txt_Alan_Boyu, string.Empty);
                errorProvider.Clear();

                float alan_eni = StrLib.ReplaceDotWithCommaReturnFloat(txt_Left_Alan_Eni, errorProvider,
                    "Bu alan boş bırakılamaz.", "Lütfen buraya bir sayı giriniz.", "Buraya 0 ya da daha küçük bir değer giremezsiniz.");
                float alan_boyu = StrLib.ReplaceDotWithCommaReturnFloat(txt_Left_Alan_Boyu, errorProvider,
                    "Bu alan boş bırakılamaz.", "Lütfen buraya bir sayı giriniz.", "Buraya 0 ya da daha küçük bir değer giremezsiniz.");

                if (!errorProvider.HasErrors)
                {
                    Ambar ambar = new Ambar(0, 0, alan_eni, alan_boyu, Main, this);

                    if (layout != null)
                    {
                        ambar.LayoutId = layout.LayoutId;
                    }

                    ambar.Rectangle = GVisual.RatioRectangleToPanel(drawingPanel, ambar.Rectangle);
                    System.Drawing.Point point = GVisual.GetCenter(drawingPanel.ClientRectangle);
                    ambar.Rectangle = new RectangleF(point.X - ambar.Rectangle.Width / 2,
                        point.Y - ambar.Rectangle.Height / 2, ambar.Rectangle.Width, ambar.Rectangle.Height);
                    ambar.OriginalRectangle = ambar.Rectangle;
                    ambar.AmbarEni = alan_eni;
                    ambar.AmbarBoyu = alan_boyu;
                    ambar.LocationofRect = new System.Drawing.Point((int)ambar.Rectangle.X, (int)ambar.Rectangle.Y);

                    foreach (var depo in Ambar.depolar)
                    {
                        depo.Parent = ambar;
                        depo.Ambar = ambar;
                        ambar.depolar.Add(depo);
                    }
                    foreach (var conveyor in Ambar.conveyors)
                    {
                        conveyor.Parent = ambar;
                        conveyor.Ambar = ambar;
                        ambar.conveyors.Add(conveyor);
                    }

                    Ambar = ambar;

                    if (!LeftSide_LayoutPanel.Visible)
                    {
                        MainPanelOpenLeftSide(LeftSide_LayoutPanel, this, leftSidePanelLocation);
                        SortFlowLayoutPanel(layoutPanel_Ambar);
                        Show_AreaMenus("Alan SubMenu");
                    }
                    else
                    {
                        SortFlowLayoutPanel(layoutPanel_Ambar);
                        Show_AreaMenus("Alan SubMenu");
                    }

                    menuProcess = false;
                    AlanNode.Tag = ambar;
                    drawingPanel.Invalidate();
                }
            }
        }
        private void btn_Left_Alan_Boyut_Degistirme_Paneli_Kapat_Click(object sender, EventArgs e)
        {
            if (SelectedAmbar != null && menuProcess)
            {
                SortFlowLayoutPanel(layoutPanel_Ambar);
                Show_AreaMenus("Alan SubMenu");
                menuProcess = false;
            }
            else if (Ambar != null && !menuProcess)
            {
                if (LeftSide_LayoutPanel.Visible)
                {
                    MainPanelCloseLeftSide(LeftSide_LayoutPanel, this);
                }
            }
        }
        private void btn_Left_Alan_Boyut_Degistir_Vazgec_Click(object sender, EventArgs e)
        {
            if (SelectedAmbar != null && menuProcess)
            {
                SortFlowLayoutPanel(layoutPanel_Ambar);
                Show_AreaMenus("Alan SubMenu");
                menuProcess = false;
            }
            else if (Ambar != null && !menuProcess)
            {
                if (LeftSide_LayoutPanel.Visible)
                {
                    MainPanelCloseLeftSide(LeftSide_LayoutPanel, this);
                }
            }
        }
        #endregion
        //Conveyor Creation events
        #region Conveyor Creation Button Events
        private void btn_Conveyor_Click(object sender, EventArgs e)
        {
            if (Ambar != null)
            {
                if (RightSide_LayoutPanel.Visible)
                {
                    if (!Conveyor_Olusturma_Paneli.Visible)
                    {
                        ShowControl(RightSide_LayoutPanel, Conveyor_Olusturma_Paneli);
                    }
                    RightSide_LayoutPanel.ScrollControlIntoView(Conveyor_Olusturma_Paneli);
                }
                else
                {
                    MainPanelOpenRightSide(RightSide_LayoutPanel, this, rightSidePanelLocation);
                    if (!Conveyor_Olusturma_Paneli.Visible)
                    {
                        ShowControl(RightSide_LayoutPanel, Conveyor_Olusturma_Paneli);
                    }
                    RightSide_LayoutPanel.ScrollControlIntoView(Conveyor_Olusturma_Paneli);
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
            errorProvider.Clear();
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
                    MainPanelCloseRightSide(RightSide_LayoutPanel, this);
                    AddConveyorNode(conveyor);
                    drawingPanel.Invalidate();
                }
            }
        }
        private void btn_Conveyor_Olustur_Vazgec_Click(object sender, EventArgs e)
        {
            MainPanelCloseRightSide(RightSide_LayoutPanel, this);
        }
        private void btn_Conveyor_Olusturma_Kapat_Click(object sender, EventArgs e)
        {
            HideControl(RightSide_LayoutPanel, Conveyor_Olusturma_Paneli);
        }
        #endregion
        //Depo Creation events
        #region Depo Creation Button Events
        private void btn_Depo_Click(object sender, EventArgs e)
        {
            if (Ambar != null)
            {
                if (RightSide_LayoutPanel.Visible)
                {
                    if (!Depo_Olusturma_Paneli.Visible)
                    {
                        ShowControl(RightSide_LayoutPanel, Depo_Olusturma_Paneli);
                    }
                    RightSide_LayoutPanel.ScrollControlIntoView(Depo_Olusturma_Paneli);
                }
                else
                {
                    MainPanelOpenRightSide(RightSide_LayoutPanel, this, rightSidePanelLocation);
                    if (!Depo_Olusturma_Paneli.Visible)
                    {
                        ShowControl(RightSide_LayoutPanel, Depo_Olusturma_Paneli);
                    }
                    RightSide_LayoutPanel.ScrollControlIntoView(Depo_Olusturma_Paneli);
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
                string depo_item_kind_secondary = txt_Depo_Item_2_Tur_Kodu.Text;

                if (depo_eni > Ambar.AmbarEni)
                {
                    errorProvider.SetError(txt_Depo_Eni, "Deponun eni alanın eninden büyük olamaz.");
                }
                if (depo_boyu > Ambar.AmbarBoyu)
                {
                    errorProvider.SetError(txt_Depo_Boyu, "Deponun boyu alanın boyundan büyük olamaz.");
                }
                if (depo_item_kind.Length > 15)
                {
                    errorProvider.SetError(txt_Depo_Item_Turu, "Tür kodu 15 karakterden uzun olamaz.");
                }
                if (depo_item_kind_secondary.Length > 15)
                {
                    errorProvider.SetError(txt_Depo_Item_2_Tur_Kodu, "İkinci tür kodu 15 karakterden uzun olamaz.");
                }

                if (depo_item_kind.Length == 0 && depo_item_kind_secondary.Length > 0)
                {
                    errorProvider.SetError(txt_Depo_Item_Turu, "Eğer ikinci tür kodunu girecekseniz ilkini de girmek zorundasınız");
                }

                if (!errorProvider.HasErrors)
                {
                    Depo depo = new Depo(0, 0, depo_eni, depo_boyu, 1f, Main, this, Ambar);

                    depo.Rectangle = GVisual.RatioRectangleToParentRectangle(depo_eni,
                        depo_boyu, Ambar.AmbarEni, Ambar.AmbarBoyu, Ambar.Rectangle);

                    depo.DepoId = 0;
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
                    depo.ItemTuruSecondary = depo_item_kind_secondary;
                    depo.Yerlestirilme_Sirasi = Ware_Counter;
                    depo.itemDrop_LeftRight = "Sağa Doğru";
                    depo.itemDrop_UpDown = "Yukarı Doğru";
                    depo.itemDrop_StartLocation = "Aşağıdan";

                    SearchForLocationtoPlace(depo.Rectangle, null, depo, Ambar);
                    MainPanelCloseRightSide(RightSide_LayoutPanel, this);
                    Ware_Counter++;
                    drawingPanel.Invalidate();
                    AdjustTextboxesText($"{depo.DepoAlaniEni}", $"{depo.DepoAlaniBoyu}",
                        null, null, null, null);
                    AddDepoNode(depo);
                }
            }
        }
        private void btn_Depo_Olustur_Vazgec_Click(object sender, EventArgs e)
        {
            MainPanelCloseRightSide(RightSide_LayoutPanel, this);
        }
        private void btn_Depo_Olusturma_Kapat_Click(object sender, EventArgs e)
        {
            HideControl(RightSide_LayoutPanel, Depo_Olusturma_Paneli);
        }

        #endregion
        //Depo Gridmaps Creation events
        #region Gridmaps Creation Button Events

        //RightSide Gridmaps Creation Button Events
        private void btn_Izgara_Haritasi_Click(object sender, EventArgs e)
        {
            if (Ambar != null)
            {
                if (Ambar.depolar.Count > 0)
                {
                    ToolStripIzgara = true;
                    if (RightSide_LayoutPanel.Visible)
                    {
                        if (!Izgara_Olusturma_Paneli.Visible)
                        {
                            ShowControl(RightSide_LayoutPanel, Izgara_Olusturma_Paneli);
                        }
                        RightSide_LayoutPanel.ScrollControlIntoView(Izgara_Olusturma_Paneli);
                    }
                    else
                    {
                        MainPanelOpenRightSide(RightSide_LayoutPanel, this, rightSidePanelLocation);
                        if (!Izgara_Olusturma_Paneli.Visible)
                        {
                            ShowControl(RightSide_LayoutPanel, Izgara_Olusturma_Paneli);
                        }
                        RightSide_LayoutPanel.ScrollControlIntoView(Izgara_Olusturma_Paneli);
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
        private void btn_Izgara_Olustur_Kapat_Click(object sender, EventArgs e)
        {
            HideControl(RightSide_LayoutPanel, Izgara_Olusturma_Paneli);
        }
        private void btn_Izgara_Olustur_Click(object sender, EventArgs e)
        {
            errorProvider.SetError(txt_Dikey_Kenar_Boslugu, string.Empty);
            errorProvider.SetError(txt_Yatay_Kenar_Boslugu, string.Empty);
            errorProvider.Clear();

            nesne_Yuksekligi = StrLib.CheckIntTextbox(txt_Nesnenin_Yuksekligi, errorProvider, "Bu alan boş bırakılamaz.", "Buraya lütfen bir sayı giriniz.", "Buraya 0 ya da daha küçük bir değer giremezsiniz.");

            nesne_Eni = StrLib.ReplaceDotWithCommaReturnFloat(txt_Nesnenin_Eni, errorProvider, "Bu alan boş bırakılamaz.", "Buraya lütfen bir sayı giriniz.", "Buraya 0 ya da daha küçük bir değer giremezsiniz.");

            nesne_Boyu = StrLib.ReplaceDotWithCommaReturnFloat(txt_Nesnenin_Boyu, errorProvider, "Bu alan boş bırakılamaz.", "Buraya lütfen bir sayı giriniz.", "Buraya 0 ya da daha küçük bir değer giremezsiniz.");

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
                            hucre_Dikey_Bosluk, hucre_Yatay_Bosluk, nesne_Eni, nesne_Boyu, nesne_Yuksekligi);
                        MainPanelCloseRightSide(RightSide_LayoutPanel, this);
                        colCount = selectedDepo.ColumnCount;
                        rowCount = selectedDepo.RowCount;
                    }
                    else
                    {
                        selectedDepo.nesneEni = nesne_Eni;
                        selectedDepo.nesneBoyu = nesne_Boyu;
                        selectedDepo.nesneYuksekligi = nesne_Yuksekligi;
                        selectedDepo.CreateGridMapMenuItem(total_Cell_Width, total_Cell_Height,
                            hucre_Dikey_Bosluk, hucre_Yatay_Bosluk, nesne_Eni, nesne_Boyu, nesne_Yuksekligi);
                        MainPanelCloseRightSide(RightSide_LayoutPanel, this);
                        colCount = selectedDepo.ColumnCount;
                        rowCount = selectedDepo.RowCount;
                    }
                }
                else if (ToolStripIzgara)
                {
                    foreach (var depo in Ambar.depolar)
                    {
                        depo.gridmaps.Clear();
                    }
                    if (izgaraHaritasiOlustur != null)
                    {
                        izgaraHaritasiOlustur.Invoke(sender, EventArgs.Empty);
                    }
                    MainPanelCloseRightSide(RightSide_LayoutPanel, this);
                    ToolStripIzgara = false;
                }
            }
            drawingPanel.Invalidate();
        }
        private void btn_Izgara_Olustur_Iptal_Et_Click(object sender, EventArgs e)
        {
            MainPanelCloseRightSide(RightSide_LayoutPanel, this);
        }


        //LeftSide Gridmaps Creation Button Events
        private void btn_Left_Izgara_Olusturma_Go_Back_Click(object sender, EventArgs e)
        {
            if (menuProcess && selectedDepo != null)
            {
                SortFlowLayoutPanel(LayoutPanel_SelectedDepo);
                Show_DepoMenus("Izgara Haritasi");
                menuProcess = false;
            }
            else if (menuProcess && SelectedAmbar != null)
            {
                SortFlowLayoutPanel(layoutPanel_Ambar);
                Show_AreaMenus("Alan SubMenu");
                menuProcess = false;
            }
            else
            {
                MainPanelCloseLeftSide(LeftSide_LayoutPanel, this);
            }
        }
        private void btn_Left_Izgara_Haritasi_Olustur_Click(object sender, EventArgs e)
        {
            errorProvider.SetError(txt_Left_Dikey_Kenar_Boslugu, string.Empty);
            errorProvider.SetError(txt_Left_Yatay_Kenar_Boslugu, string.Empty);
            errorProvider.Clear();

            nesne_Yuksekligi = StrLib.CheckIntTextbox(txt_Left_Nesne_Yuksekligi, errorProvider, "Bu alan boş bırakılamaz.", "Buraya lütfen bir sayı giriniz.", "Buraya 0 ya da daha küçük bir değer giremezsiniz.");

            nesne_Eni = StrLib.ReplaceDotWithCommaReturnFloat(txt_Left_Nesne_Eni, errorProvider, "Bu alan boş bırakılamaz.", "Buraya lütfen bir sayı giriniz.", "Buraya 0 ya da daha küçük bir değer giremezsiniz.");

            nesne_Boyu = StrLib.ReplaceDotWithCommaReturnFloat(txt_Left_Nesne_Boyu, errorProvider, "Bu alan boş bırakılamaz.", "Buraya lütfen bir sayı giriniz.", "Buraya 0 ya da daha küçük bir değer giremezsiniz.");

            float hucre_Dikey_Bosluk =
                StrLib.ReplaceDotWithCommaReturnFloat(txt_Left_Dikey_Kenar_Boslugu, errorProvider,
                "Bu alan boş bırakılamaz.", "Buraya lütfen bir sayı giriniz.", "Buraya 0 ya da daha küçük bir değer giremezsiniz.");

            float hucre_Yatay_Bosluk =
                StrLib.ReplaceDotWithCommaReturnFloat(txt_Left_Yatay_Kenar_Boslugu, errorProvider,
                "Bu alan boş bırakılamaz.", "Buraya lütfen bir sayı giriniz.", "Buraya 0 ya da daha küçük bir değer giremezsiniz.");

            total_Cell_Width = nesne_Eni + hucre_Yatay_Bosluk;
            total_Cell_Height = nesne_Boyu + hucre_Dikey_Bosluk;


            if (!errorProvider.HasErrors)
            {
                if (selectedDepo != null)
                {
                    if (selectedDepo.gridmaps.Count > 0)
                    {
                        selectedDepo.gridmaps.Clear();

                        selectedDepo.nesneEni = nesne_Eni;
                        selectedDepo.nesneBoyu = nesne_Boyu;
                        selectedDepo.nesneYuksekligi = nesne_Yuksekligi;
                        selectedDepo.CreateGridMapMenuItem(total_Cell_Width, total_Cell_Height,
                            hucre_Dikey_Bosluk, hucre_Yatay_Bosluk, nesne_Eni, nesne_Boyu, nesne_Yuksekligi);


                        if (menuProcess)
                        {
                            SortFlowLayoutPanel(LayoutPanel_SelectedDepo);
                            Show_DepoMenus("Depo");
                            menuProcess = false;
                        }
                        else
                        {
                            MainPanelCloseLeftSide(LeftSide_LayoutPanel, this);
                        }

                        SelectNode(null, null, selectedDepo);

                        colCount = selectedDepo.ColumnCount;
                        rowCount = selectedDepo.RowCount;
                    }
                    else
                    {
                        selectedDepo.nesneEni = nesne_Eni;
                        selectedDepo.nesneBoyu = nesne_Boyu;
                        selectedDepo.nesneYuksekligi = nesne_Yuksekligi;
                        selectedDepo.CreateGridMapMenuItem(total_Cell_Width, total_Cell_Height,
                            hucre_Dikey_Bosluk, hucre_Yatay_Bosluk, nesne_Eni, nesne_Boyu, nesne_Yuksekligi);

                        if (menuProcess)
                        {
                            SortFlowLayoutPanel(LayoutPanel_SelectedDepo);
                            Show_DepoMenus("Depo");
                            menuProcess = false;
                        }
                        else
                        {
                            MainPanelCloseLeftSide(LeftSide_LayoutPanel, this);
                        }

                        SelectNode(null, null, selectedDepo);

                        colCount = selectedDepo.ColumnCount;
                        rowCount = selectedDepo.RowCount;
                    }
                }
                else if (selectedDepo == null && SelectedAmbar != null)
                {
                    if (menuProcess)
                    {
                        foreach (var depo in SelectedAmbar.depolar)
                        {
                            if (depo.gridmaps.Count > 0)
                            {
                                depo.gridmaps.Clear();
                            }

                            depo.nesneEni = nesne_Eni;
                            depo.nesneBoyu = nesne_Boyu;
                            depo.nesneYuksekligi = nesne_Yuksekligi;
                            depo.CreateGridMapMenuItem(total_Cell_Width, total_Cell_Height,
                                hucre_Dikey_Bosluk, hucre_Yatay_Bosluk, nesne_Eni, nesne_Boyu, nesne_Yuksekligi);

                            SortFlowLayoutPanel(layoutPanel_Ambar);
                            Show_AreaMenus("Alan");
                            menuProcess = false;

                            SelectNode(SelectedAmbar, null, null);

                            colCount = depo.ColumnCount;
                            rowCount = depo.RowCount;
                        }
                    }
                }
            }
            drawingPanel.Invalidate();
        }
        private void btn_Left_Izgara_Haritasi_Vazgec_Click(object sender, EventArgs e)
        {
            if (menuProcess && selectedDepo != null)
            {
                SortFlowLayoutPanel(LayoutPanel_SelectedDepo);
                Show_DepoMenus("Depo");
                menuProcess = false;
            }
            else if (menuProcess && SelectedAmbar != null)
            {
                SortFlowLayoutPanel(layoutPanel_Ambar);
                Show_AreaMenus("Alan");
                menuProcess = false;
            }
            else
            {
                MainPanelCloseLeftSide(LeftSide_LayoutPanel, this);
            }
        }

        #endregion



        //Area events
        #region Area Events

        //Delete Area (SubMenu and Context Menu)
        private void silToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Ambar != null)
            {
                using (var context = new DBContext())
                {
                    var ambar = (from x in context.Ambars
                                 where x.AmbarId == Ambar.AmbarId
                                 select x).FirstOrDefault();

                    if (ambar != null)
                    {
                        context.Ambars.Remove(ambar);
                        context.SaveChanges();
                    }
                }
                Ambar = null;
                AlanNode.Tag = null;
                drawingPanel.Invalidate();
            }
        }
        private void btn_Alan_SubMenu_Sil_Click(object sender, EventArgs e)
        {
            if (Ambar != null && SelectedAmbar != null)
            {
                using (var context = new DBContext())
                {
                    var ambar = (from x in context.Ambars
                                 where x.AmbarId == Ambar.AmbarId
                                 select x).FirstOrDefault();

                    if (ambar != null)
                    {
                        context.Ambars.Remove(ambar);
                        context.SaveChanges();
                    }
                }

                Ambar = null;
                SelectedAmbar = null;
                AlanNode.Tag = null;
                drawingPanel.Invalidate();
            }
        }



        //Empty Area (SubMenu and Context Menu)
        private void boşaltToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Ambar != null)
            {
                foreach (var depo in Ambar.depolar)
                {
                    Ambar.deletedDepos.Add(depo);
                }

                foreach (var conveyor in Ambar.conveyors)
                {
                    Ambar.deletedConveyors.Add(conveyor);
                }

                Ambar.depolar.Clear();
                Ambar.conveyors.Clear();
                DepoNode.Nodes.Clear();
                ConveyorNode.Nodes.Clear();
                drawingPanel.Invalidate();
            }
        }
        private void btn_Alan_SubMenu_Bosalt_Click(object sender, EventArgs e)
        {
            if (Ambar != null && SelectedAmbar != null)
            {
                foreach (var depo in Ambar.depolar)
                {
                    Ambar.deletedDepos.Add(depo);
                }

                foreach (var conveyor in Ambar.conveyors)
                {
                    Ambar.deletedConveyors.Add(conveyor);
                }

                Ambar.depolar.Clear();
                Ambar.conveyors.Clear();
                DepoNode.Nodes.Clear();
                ConveyorNode.Nodes.Clear();
                drawingPanel.Invalidate();
            }
        }



        //Area Item Placement Queue (SubMenu and Context Menu)
        private void depolarınDoldurulmaSirasiniAyarla_Click(object sender, EventArgs e)
        {
            TransparentPen.Color = System.Drawing.Color.FromArgb(30, System.Drawing.Color.Black);
            CustomNotifyIcon notify = new CustomNotifyIcon();
            notify.showAlert("Depoların sırasını üzerlerine sol tıklayarak seçebilirsiniz.(Sağ tıklayarak sırasını kaldırabilirsiniz.)", CustomNotifyIcon.enmType.Info);
            Fill_WareHouse = true;
            drawingPanel.Invalidate();
        }
        private void btn_Alan_SubMenu_Depo_Siralamasi_Click(object sender, EventArgs e)
        {
            if (SelectedAmbar != null)
            {
                if (SelectedAmbar.depolar.Count > 0)
                {
                    TransparentPen.Color = System.Drawing.Color.FromArgb(30, System.Drawing.Color.Black);
                    CustomNotifyIcon notify = new CustomNotifyIcon();
                    notify.showAlert("Depoların sırasını üzerilerine sol tıklayarak seçebilirsiniz.\n(Sağ tıklayarak sırasını kaldırabilirsiniz.)", CustomNotifyIcon.enmType.Info);
                    Fill_WareHouse = true;
                    drawingPanel.Invalidate();
                }
                else
                {
                    CustomNotifyIcon notify = new CustomNotifyIcon();
                    notify.showAlert("Lütfen önce depo oluşturun.", CustomNotifyIcon.enmType.Warning);
                }
            }
        }



        //Area Change Size Events (SubMenu and Context Menu)
        private void boyutunuDeğiştirToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (Ambar != null)
            {
                ambar_Boyut_Degistir = true;
                menuProcess = false;
                if (!LeftSide_LayoutPanel.Visible)
                {
                    MainPanelOpenLeftSide(LeftSide_LayoutPanel, this, leftSidePanelLocation);
                    SortFlowLayoutPanel(LeftSide_Alan_Boyut_Degistirme_Paneli);
                }
                else
                {
                    SortFlowLayoutPanel(LeftSide_Alan_Boyut_Degistirme_Paneli);
                }
            }
            else
            {
                MessageBox.Show("Lütfen önce alan oluşturun.", "Alan yok.", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }
        private void btn_Alan_SubMenu_Boyut_Degistir_Click(object sender, EventArgs e)
        {
            if (Ambar != null && SelectedAmbar != null)
            {
                ambar_Boyut_Degistir = true;
                menuProcess = true;
                SortFlowLayoutPanel(LeftSide_Alan_Boyut_Degistirme_Paneli);
            }
            else
            {
                MessageBox.Show("Lütfen önce alan oluşturun.", "Alan yok.", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }



        //Add GridMaps to All the Depos in the Area SubMenu Button Event
        private void btn_Alan_SubMenu_Depolara_Izgara_Ekle_Click(object sender, EventArgs e)
        {
            if (Ambar != null && SelectedAmbar != null)
            {
                if (SelectedAmbar.depolar.Count > 0)
                {
                    menuProcess = true;
                    Clear_AddControltoLeftSidePanel(LeftPanel_Izgara_Olusturma);
                }
                else
                {
                    CustomNotifyIcon notify = new CustomNotifyIcon();
                    notify.showAlert("Lütfen önce depo oluşturun.", CustomNotifyIcon.enmType.Warning);
                }
            }
            else
            {
                MessageBox.Show("Lütfen alan seçin.", "Alan seçin.", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        #endregion
        //Depo events
        #region Depo Events
        //Delete Depo
        private void depoyuSilToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            TreeNode DeletedNode = new TreeNode();
            if (Ambar != null)
            {
                if (selectedDepo != null)
                {
                    Ambar.deletedDepos.Add(selectedDepo);
                    Ambar.depolar.Remove(selectedDepo);
                    DeleteDepoNode(selectedDepo);
                }
            }
            selectedDepo = null;
            CopyDepo = null;
            drawingPanel.Invalidate();
        }
        private void btn_Depo_SubMenu_Sil_Click(object sender, EventArgs e)
        {
            if (selectedDepo != null)
            {
                if (Ambar != null)
                {
                    Ambar.deletedDepos.Add(selectedDepo);
                    Ambar.depolar.Remove(selectedDepo);
                    Show_DepoMenus("Depo");
                    MainPanelCloseLeftSide(LeftSide_LayoutPanel, this);
                    if (Ambar.depolar.Count == 0)
                    {
                        GVisual.HideControl(LayoutPanel_SelectedDepo, LeftSide_LayoutPanel);
                    }
                    DeleteDepoNode(selectedDepo);
                    drawingPanel.Invalidate();
                }
            }
        }
        #endregion
        //Depo Gridmap events
        #region GridMaps Events
        //Create Gridmaps
        private void ızgaraHaritasıOluşturToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripIzgara = false;
            menuProcess = false;
            MainPanelOpenLeftSide(LeftSide_LayoutPanel, this, leftSidePanelLocation);
            Clear_AddControltoLeftSidePanel(LeftPanel_Izgara_Olusturma);
        }
        private void btn_Depo_SubMenu_Izgara_Haritasi_Olustur_Click(object sender, EventArgs e)
        {
            menuProcess = true;
            if (RightSide_LayoutPanel.Visible)
            {
                MainPanelCloseRightSide(RightSide_LayoutPanel, this);
            }
            Clear_AddControltoLeftSidePanel(LeftPanel_Izgara_Olusturma);
        }


        //Change the Size of Gridmap Cells
        private void btn_Depo_SubMenu_Izgara_Haritasi_Boyut_Degistir_Click(object sender, EventArgs e)
        {
            ToolStripIzgara = false;
            if (selectedDepo != null)
            {
                if (selectedDepo.gridmaps.Count > 0)
                {
                    menuProcess = true;
                    if (RightSide_LayoutPanel.Visible)
                    {
                        MainPanelCloseRightSide(RightSide_LayoutPanel, this);
                    }
                    Clear_AddControltoLeftSidePanel(LeftPanel_Izgara_Olusturma);
                }
                else
                {
                    CustomNotifyIcon notify = new CustomNotifyIcon();
                    notify.showAlert("Lütfen önce ızgara haritası oluşturun.", CustomNotifyIcon.enmType.Warning);
                }
            }
        }
        private void boyutunuDeğiştirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripIzgara = false;
            if (selectedDepo != null)
            {
                if (selectedDepo.gridmaps.Count > 0)
                {
                    if (!LeftSide_LayoutPanel.Visible)
                    {
                        MainPanelOpenLeftSide(LeftSide_LayoutPanel, this, leftSidePanelLocation);
                        Clear_AddControltoLeftSidePanel(LeftPanel_Izgara_Olusturma);
                    }
                    else
                    {
                        Clear_AddControltoLeftSidePanel(LeftPanel_Izgara_Olusturma);
                    }
                }
                else
                {
                    CustomNotifyIcon notify = new CustomNotifyIcon();
                    notify.showAlert("Lütfen önce ızgara haritası oluşturun.", CustomNotifyIcon.enmType.Warning);
                }
            }
        }


        //Delete Gridmaps
        private void silToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (Ambar != null)
            {
                if (selectedDepo != null)
                {
                    selectedDepo.gridmaps.Clear();
                }
            }
            drawingPanel.Invalidate();
        }
        private void btn_Depo_SubMenu_Izgara_Haritasini_Sil_Click(object sender, EventArgs e)
        {
            if (selectedDepo != null)
            {
                if (selectedDepo.gridmaps.Count > 0)
                {
                    Show_DepoMenus("Depo");
                    selectedDepo.gridmaps.Clear();
                    drawingPanel.Invalidate();
                }
                else
                {
                    CustomNotifyIcon notify = new CustomNotifyIcon();
                    notify.showAlert("Izgara haritası yok", CustomNotifyIcon.enmType.Error);
                }
            }
        }
        #endregion
        //Conveyor events
        #region Conveyor Events
        //Delete Conveyor Events
        private void Conveyor_silToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Ambar != null)
            {
                if (selectedConveyor != null)
                {
                    Ambar.deletedConveyors.Add(selectedConveyor);
                    Ambar.conveyors.Remove(selectedConveyor);
                    DeleteConveyorNode(selectedConveyor);
                }
            }
            selectedConveyor = null;
            CopyConveyor = null;
            drawingPanel.Invalidate();
        }
        private void btn_Conveyor_SubMenu_Conveyor_Sil_Click(object sender, EventArgs e)
        {
            if (Ambar != null)
            {
                if (selectedConveyor != null)
                {
                    Ambar.deletedConveyors.Add(selectedConveyor);
                    Ambar.conveyors.Remove(selectedConveyor);
                    Show_ConveyorMenus("Conveyor");
                    MainPanelCloseLeftSide(LeftSide_LayoutPanel, this);
                    if (Ambar.conveyors.Count == 0)
                    {
                        GVisual.HideControl(layoutPanel_SelectedConveyor, LeftSide_LayoutPanel);
                    }
                    DeleteConveyorNode(selectedConveyor);
                }
            }
            selectedConveyor = null;
            CopyConveyor = null;
            drawingPanel.Invalidate();
        }
        #endregion
        //Conveyor Reference events
        #region Conveyor Reference Events
        //Add Reference Point to Conveyor Events
        private void Referans_ekleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedConveyor != null)
            {
                AddReferencePoint = true;
                CenterControltoLeftSideofRectangleVertically
                    (selectedConveyor.Rectangle, Conveyor_Reference_FixedorManuel_Panel, drawingPanel);
            }
        }
        private void btn_Conveyor_SubMenu_Referans_Ekle_Click(object sender, EventArgs e)
        {
            menuProcess = true;
            if (selectedConveyor != null)
            {
                AddReferencePoint = true;
                GVisual.ShowControl(Conveyor_Reference_FixedorManuel_Panel, groupBox_SelectedConveyor);
                GVisual.Control_Center(Conveyor_Reference_FixedorManuel_Panel, groupBox_SelectedConveyor);
                GVisual.HideControl(panel_Conveyor_SubMenu_Referans, groupBox_SelectedConveyor);
            }
        }



        //Delete Conveyor Reference Point Events
        private void Referans_silToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedConveyor != null)
            {
                selectedConveyor.ConveyorReferencePoints.Clear();
                drawingPanel.Invalidate();
            }
        }
        private void btn_Conveyor_SubMenu_Referans_Sil_Click(object sender, EventArgs e)
        {
            if (selectedConveyor != null)
            {
                selectedConveyor.ConveyorReferencePoints.Clear();
                drawingPanel.Invalidate();
            }
        }



        //Change Locations of Conveyor Reference Point Events
        private void Referans_yerleriniDeğiştirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedConveyor != null)
            {
                float reffX = 0;
                float reffY = 0;
                using (var dialog = new ConveyorReffYerDegistir(selectedConveyor))
                {
                    dialog.errorProvider.Clear();
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
                                        if (textbox is System.Windows.Forms.TextBox)
                                        {
                                            System.Windows.Forms.TextBox textBox = new System.Windows.Forms.TextBox();
                                            textBox = (System.Windows.Forms.TextBox)textbox;

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
        private void btn_Conveyor_SubMenu_Referans_Yerlerini_Degistir_Click(object sender, EventArgs e)
        {
            if (selectedConveyor != null)
            {
                float reffX = 0;
                float reffY = 0;
                using (var dialog = new ConveyorReffYerDegistir(selectedConveyor))
                {
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        menuProcess = true;
                        foreach (var reff in selectedConveyor.ConveyorReferencePoints)
                        {
                            foreach (Panel panel in dialog.Panel.Controls)
                            {
                                if (reff == (ConveyorReferencePoint)panel.Tag)
                                {
                                    foreach (var textbox in panel.Controls)
                                    {
                                        if (textbox is System.Windows.Forms.TextBox)
                                        {
                                            System.Windows.Forms.TextBox textBox = new System.Windows.Forms.TextBox();
                                            textBox = (System.Windows.Forms.TextBox)textbox;

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
                                        menuProcess = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }



        //Adding Conveyor Reference Point Logic
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
            AddReferencePoint = false;
        }



        //Accept Conveyor Reference Points
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
            if (menuProcess)
            {
                Show_ConveyorMenus("Referans SubMenu");
                menuProcess = false;
            }
            else
            {
                GVisual.HideControl(Conveyor_Reference_Fixed_Panel, drawingPanel);
            }
            drawingPanel.Invalidate();
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

                    if (menuProcess)
                    {
                        Show_ConveyorMenus("Referans SubMenu");
                        menuProcess = false;
                    }
                    else
                    {
                        GVisual.HideControl(Conveyor_Reference_Sayisi_Paneli, drawingPanel);
                    }
                    AddReferencePoint = false;
                }
            }
        }



        //Select if Manual or Fixed Reference Points
        private void btn_Select_Fixed_Conveyor_Reference_Point_Click(object sender, EventArgs e)
        {
            if (menuProcess)
            {
                GVisual.HideControl(Conveyor_Reference_FixedorManuel_Panel, groupBox_SelectedConveyor);
                GVisual.ShowControl(Conveyor_Reference_Fixed_Panel, groupBox_SelectedConveyor);
                GVisual.Control_Center(Conveyor_Reference_Fixed_Panel, groupBox_SelectedConveyor);


                chk_Conveyor_Reference_Bottom.ForeColor = System.Drawing.Color.Black;
                chk_Conveyor_Reference_Bottom.Font = new System.Drawing.Font("Segoe UI", 9);

                chk_Conveyor_Reference_Top.ForeColor = System.Drawing.Color.Black;
                chk_Conveyor_Reference_Top.Font = new System.Drawing.Font("Segoe UI", 9);

                chk_Conveyor_Reference_Right.ForeColor = System.Drawing.Color.Black;
                chk_Conveyor_Reference_Right.Font = new System.Drawing.Font("Segoe UI", 9);

                chk_Conveyor_Reference_Left.ForeColor = System.Drawing.Color.Black;
                chk_Conveyor_Reference_Left.Font = new System.Drawing.Font("Segoe UI", 9);

                chk_Conveyor_Reference_Center.ForeColor = System.Drawing.Color.Black;
                chk_Conveyor_Reference_Center.Font = new System.Drawing.Font("Segoe UI", 9);
            }
            else
            {
                GVisual.HideControl(Conveyor_Reference_FixedorManuel_Panel, drawingPanel);
                CenterControltoLeftSideofRectangleVertically(selectedConveyor.Rectangle,
                    Conveyor_Reference_Fixed_Panel, drawingPanel);
            }
        }
        private void btn_Manuel_Reference_Point_Click(object sender, EventArgs e)
        {
            if (menuProcess)
            {
                GVisual.HideControl(Conveyor_Reference_FixedorManuel_Panel, groupBox_SelectedConveyor);
                GVisual.ShowControl(Conveyor_Reference_Sayisi_Paneli, groupBox_SelectedConveyor);
                GVisual.Control_Center(Conveyor_Reference_Sayisi_Paneli, groupBox_SelectedConveyor);
            }
            else
            {
                GVisual.HideControl(Conveyor_Reference_FixedorManuel_Panel, drawingPanel);
                CenterControltoLeftSideofRectangleVertically(selectedConveyor.Rectangle, Conveyor_Reference_Sayisi_Paneli,
                    drawingPanel);
            }
        }
        #endregion



        //Alan SubMenu events
        #region Alan SubMenu Events
        //Open LeftSide Menu Context Menu Item Event for Ambar, Area
        private void alanMenuAcBTN_Click(object sender, EventArgs e)
        {
            SortFlowLayoutPanel(layoutPanel_Ambar);
            if (!LeftSide_LayoutPanel.Visible)
            {
                MainPanelOpenLeftSide(LeftSide_LayoutPanel, this, leftSidePanelLocation);
                Show_AreaMenus("Alan");
            }
        }


        private void btn_Alan_SubMenu_Click(object sender, EventArgs e)
        {
            Show_AreaMenus("Alan SubMenu");
        }

        private void btn_Alan_Menu_Go_Back_Click(object sender, EventArgs e)
        {
            if (groupBox_Ambar.Controls.Contains(panel_Alan_SubMenu))
            {
                Show_AreaMenus("Alan");
            }
            menuProcess = false;
        }

        public void Show_AreaMenus(string whichMenu)
        {
            GVisual.HideControl(panel_Alan_Menu, groupBox_Ambar);
            GVisual.HideControl(panel_Alan_SubMenu, groupBox_Ambar);
            GVisual.HideControl(btn_Alan_Menu_Go_Back, groupBox_Ambar);

            if (whichMenu == "Alan")
            {
                GVisual.ShowControl(panel_Alan_Menu, groupBox_Ambar);
                GVisual.Control_Center(panel_Alan_Menu, groupBox_Ambar);
            }
            else if (whichMenu == "Alan SubMenu")
            {
                GVisual.ShowControl(panel_Alan_SubMenu, groupBox_Ambar, new System.Drawing.Point(62, 28));
                GVisual.ShowControl(btn_Alan_Menu_Go_Back, groupBox_Ambar);
            }
        }



        #endregion
        //Depo SubMenu events
        #region Depo SubMenu Events
        private void depoMenuAcBTN_Click(object sender, EventArgs e)
        {
            if (!LeftSide_LayoutPanel.Visible)
            {
                MainPanelOpenLeftSide(LeftSide_LayoutPanel, this, leftSidePanelLocation);
            }
            Show_DepoMenus("Depo");
            SortFlowLayoutPanel(LayoutPanel_SelectedDepo);
        }
        private void btn_Depo_SubMenu_Click(object sender, EventArgs e)
        {
            Show_DepoMenus("Depo SubMenu");
        }
        private void btn_Depo_SubMenu_Izgara_Haritasi_Click(object sender, EventArgs e)
        {
            Show_DepoMenus("Izgara Haritasi");
        }
        private void btn_Depo_Menu_Go_Back_Click(object sender, EventArgs e)
        {
            Show_DepoMenus("Depo");

            if (panel_Depo_SubMenu_Izgara_Haritasi.Visible)
            {
                GVisual.HideControl(panel_Depo_SubMenu_Izgara_Haritasi, groupBox_SelectedDepo);
            }
            else if (panel_Depo_SubMenu.Visible)
            {
                GVisual.HideControl(panel_Depo_SubMenu, groupBox_SelectedDepo);
            }
            else if (Asama1_Yukseklik_Panel.Visible)
            {
                GVisual.HideControl(Asama1_Yukseklik_Panel, groupBox_SelectedDepo);
            }
            menuProcess = false;
        }
        public void Show_DepoMenus(string whichMenu)
        {
            GVisual.HideControl(panel_Depo_Menu, groupBox_SelectedDepo);
            GVisual.HideControl(panel_Depo_SubMenu, groupBox_SelectedDepo);
            GVisual.HideControl(panel_Depo_SubMenu_Izgara_Haritasi, groupBox_SelectedDepo);
            GVisual.HideControl(btn_Depo_Menu_Go_Back, groupBox_SelectedDepo);

            if (whichMenu == "Izgara Haritasi")
            {
                GVisual.ShowControl(panel_Depo_SubMenu_Izgara_Haritasi, groupBox_SelectedDepo);
                GVisual.Control_Center(panel_Depo_SubMenu_Izgara_Haritasi, groupBox_SelectedDepo);
                GVisual.ShowControl(btn_Depo_Menu_Go_Back, groupBox_SelectedDepo);
            }
            else if (whichMenu == "Depo SubMenu")
            {
                GVisual.ShowControl(panel_Depo_SubMenu, groupBox_SelectedDepo);
                GVisual.Control_Center(panel_Depo_SubMenu, groupBox_SelectedDepo);
                GVisual.ShowControl(btn_Depo_Menu_Go_Back, groupBox_SelectedDepo);
            }
            else if (whichMenu == "Depo")
            {
                GVisual.ShowControl(panel_Depo_Menu, groupBox_SelectedDepo);
                GVisual.Control_Center(panel_Depo_Menu, groupBox_SelectedDepo);
            }
        }
        #endregion
        //Conveyor SubMenu events
        #region Conveyor SubMenu Events
        private void conveyorMenuAcBTN_C1ick(object sender, EventArgs e)
        {
            SortFlowLayoutPanel(layoutPanel_SelectedConveyor);
            Show_ConveyorMenus("Conveyor");
            if (!LeftSide_LayoutPanel.Visible)
            {
                MainPanelOpenLeftSide(LeftSide_LayoutPanel, this, leftSidePanelLocation);
            }
        }
        private void btn_Conveyor_SubMenu_Referans_Click(object sender, EventArgs e)
        {
            Show_ConveyorMenus("Referans SubMenu");
        }
        private void btn_Conveyor_SubMenu_Click(object sender, EventArgs e)
        {
            Show_ConveyorMenus("Conveyor SubMenu");
        }
        private void btn_Conveyor_Menu_Go_Back_Click(object sender, EventArgs e)
        {
            Show_ConveyorMenus("Conveyor");

            if (panel_Conveyor_Submenu.Visible)
            {
                GVisual.HideControl(panel_Conveyor_Submenu, groupBox_SelectedConveyor);
            }
            else if (panel_Conveyor_SubMenu_Referans.Visible)
            {
                GVisual.HideControl(panel_Conveyor_SubMenu_Referans, groupBox_SelectedConveyor);
            }
        }
        public void Show_ConveyorMenus(string whichMenu)
        {
            groupBox_SelectedConveyor.Controls.Clear();

            if (whichMenu == "Conveyor")
            {
                GVisual.ShowControl(panel_Conveyor_Menu, groupBox_SelectedConveyor);
                GVisual.Control_Center(panel_Conveyor_Menu, groupBox_SelectedConveyor);
            }
            else if (whichMenu == "Conveyor SubMenu")
            {
                GVisual.ShowControl(panel_Conveyor_Submenu, groupBox_SelectedConveyor);
                GVisual.Control_Center(panel_Conveyor_Submenu, groupBox_SelectedConveyor);
                GVisual.ShowControl(btn_Conveyor_Menu_Go_Back, groupBox_SelectedConveyor);
            }
            else if (whichMenu == "Referans SubMenu")
            {
                GVisual.ShowControl(panel_Conveyor_SubMenu_Referans, groupBox_SelectedConveyor);
                GVisual.Control_Center(panel_Conveyor_SubMenu_Referans, groupBox_SelectedConveyor);
                GVisual.ShowControl(btn_Conveyor_Menu_Go_Back, groupBox_SelectedConveyor);
            }
        }
        #endregion



        //All UI Operations Events and Methods (clearing textboxes, hide and show panels, hide and show textboxes, adjust locations of the areas and panels etc) 
        #region UI Operations
        public void HideEverything()
        {
            GVisual.HideControl(Alan_Olusturma_Paneli, this);
            GVisual.HideControl(Conveyor_Olusturma_Paneli, this);
            GVisual.HideControl(Depo_Olusturma_Paneli, this);
            GVisual.HideControl(Izgara_Olusturma_Paneli, this);
            GVisual.HideControl(Izgara_Olusturma_Paneli, this);
            GVisual.HideControl(PaddingPanel, this);
            GVisual.HideControl(Depo_Olusturma_Paneli, this);
            GVisual.HideControl(LeftSide_LayoutPanel, this);
            GVisual.HideControl(RightSide_LayoutPanel, this);
            GVisual.HideControl(LeftPanel_Izgara_Olusturma, this);
            GVisual.HideControl(layoutPanel_Ambar, this);
            GVisual.HideControl(LeftSide_Alan_Boyut_Degistirme_Paneli, this);
            GVisual.HideControl(LayoutPanel_Alan_Hierarchy, this);



            GVisual.HideControl(txt_Width, drawingPanel);
            GVisual.HideControl(txt_Height, drawingPanel);
            GVisual.HideControl(txt_Left_Padding, drawingPanel);
            GVisual.HideControl(txt_Right_Padding, drawingPanel);
            GVisual.HideControl(txt_Top_Padding, drawingPanel);
            GVisual.HideControl(txt_Bottom_Padding, drawingPanel);
            GVisual.HideControl(btn_Yer_Onayla, drawingPanel);
            GVisual.HideControl(Placement_LeftRight_Panel, drawingPanel);
            GVisual.HideControl(Placement_StartLocation_Panel, drawingPanel);
            GVisual.HideControl(Placement_UpDown_Panel, drawingPanel);
            GVisual.HideControl(Asama1_Yukseklik_Panel, drawingPanel);
            GVisual.HideControl(Asama2_Yukseklik_Panel, drawingPanel);
            GVisual.HideControl(Conveyor_Reference_Sayisi_Paneli, drawingPanel);
            GVisual.HideControl(Conveyor_Reference_FixedorManuel_Panel, drawingPanel);
            GVisual.HideControl(Conveyor_Reference_Fixed_Panel, drawingPanel);



            GVisual.HideControl(panel_Depo_SubMenu, groupBox_SelectedDepo);
            GVisual.HideControl(panel_Depo_SubMenu_Izgara_Haritasi, groupBox_SelectedDepo);



            GVisual.HideControl(panel_Conveyor_Submenu, groupBox_SelectedConveyor);
            GVisual.HideControl(panel_Conveyor_SubMenu_Referans, groupBox_SelectedConveyor);



            GVisual.HideControl(panel_Alan_SubMenu, groupBox_Ambar);
        }
        public void MoveLeft()
        {
            if (Ambar != null)
            {
                Ambar.Rectangle = new RectangleF(Ambar.Rectangle.X - drawingPanelMoveConstant, Ambar.Rectangle.Y, Ambar.Rectangle.Width, Ambar.Rectangle.Height);
                Ambar.OriginalRectangle = Ambar.Rectangle;
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
                        cell.OriginalRectangle = cell.Rectangle;
                        cell.LocationofRect = new System.Drawing.Point((int)cell.Rectangle.X, (int)cell.Rectangle.Y);

                    }
                }
                foreach (var conveyor in Ambar.conveyors)
                {
                    conveyor.Rectangle = new RectangleF(conveyor.Rectangle.X - drawingPanelMoveConstant, conveyor.Rectangle.Y, conveyor.Rectangle.Width, conveyor.Rectangle.Height);
                    conveyor.OriginalRectangle = conveyor.Rectangle;

                    conveyor.LocationofRect = new System.Drawing.Point((int)conveyor.Rectangle.X, (int)conveyor.Rectangle.Y);
                    selectedConveyorRectangle = conveyor.Rectangle;
                    UnchangedselectedConveyorRectangle = conveyor.Rectangle;

                    foreach (var reff in conveyor.ConveyorReferencePoints)
                    {
                        reff.Rectangle = new RectangleF(reff.Rectangle.X - drawingPanelMoveConstant,
                            reff.Rectangle.Y, reff.Rectangle.Width, reff.Rectangle.Height);
                        reff.OriginalRectangle = reff.Rectangle;

                        reff.LocationofRect = new System.Drawing.Point((int)reff.Rectangle.X, (int)reff.Rectangle.Y);
                    }
                }
                drawingPanel.Invalidate();
            }
        }
        public void MoveRight()
        {
            if (Ambar != null)
            {
                Ambar.Rectangle = new RectangleF(Ambar.Rectangle.X + drawingPanelMoveConstant, Ambar.Rectangle.Y, Ambar.Rectangle.Width, Ambar.Rectangle.Height);
                Ambar.OriginalRectangle = Ambar.Rectangle;
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
                        cell.OriginalRectangle = cell.Rectangle;
                        cell.LocationofRect = new System.Drawing.Point((int)cell.Rectangle.X, (int)cell.Rectangle.Y);
                    }
                }
                foreach (var conveyor in Ambar.conveyors)
                {
                    conveyor.Rectangle = new RectangleF(conveyor.Rectangle.X + drawingPanelMoveConstant, conveyor.Rectangle.Y, conveyor.Rectangle.Width, conveyor.Rectangle.Height);
                    conveyor.OriginalRectangle = conveyor.Rectangle;

                    conveyor.LocationofRect = new System.Drawing.Point((int)conveyor.Rectangle.X, (int)conveyor.Rectangle.Y);
                    selectedConveyorRectangle = conveyor.Rectangle;
                    UnchangedselectedConveyorRectangle = conveyor.Rectangle;
                    foreach (var reff in conveyor.ConveyorReferencePoints)
                    {
                        reff.Rectangle = new RectangleF(reff.Rectangle.X + drawingPanelMoveConstant,
                            reff.Rectangle.Y, reff.Rectangle.Width, reff.Rectangle.Height);
                        reff.OriginalRectangle = reff.Rectangle;

                        reff.LocationofRect = new System.Drawing.Point((int)reff.Rectangle.X, (int)reff.Rectangle.Y);
                    }
                }
                drawingPanel.Invalidate();
            }
        }
        public void btn_openClose_LeftSide_Click(object sender, EventArgs e)
        {
            if (LeftSide_LayoutPanel.Visible)
            {
                MainPanelCloseLeftSide(LeftSide_LayoutPanel, this);

                if (selectedDepo != null)
                {
                    SortFlowLayoutPanel(LayoutPanel_SelectedDepo);
                }
                else if (SelectedAmbar != null)
                {
                    SortFlowLayoutPanel(layoutPanel_Ambar);
                }
                else if (selectedConveyor != null)
                {
                    SortFlowLayoutPanel(layoutPanel_SelectedConveyor);
                }
                else
                {
                    Clear_AddControltoLeftSidePanel(LayoutPanel_Alan_Hierarchy);
                }

                menuProcess = false;
                Manuel_Move = false;
                AddReferencePoint = false;
            }
            else
            {
                MainPanelOpenLeftSide(LeftSide_LayoutPanel, this, leftSidePanelLocation);

                if (selectedDepo != null)
                {
                    SortFlowLayoutPanel(LayoutPanel_SelectedDepo);
                }
                else if (SelectedAmbar != null)
                {
                    SortFlowLayoutPanel(layoutPanel_Ambar);
                }
                else if (selectedConveyor != null)
                {
                    SortFlowLayoutPanel(layoutPanel_SelectedConveyor);
                }
                else
                {
                    Clear_AddControltoLeftSidePanel(LayoutPanel_Alan_Hierarchy);
                }
            }
        }
        public void btn_openClose_RightSide_Click(object sender, EventArgs e)
        {
            if (RightSide_LayoutPanel.Visible)
            {
                MainPanelCloseRightSide(RightSide_LayoutPanel, this);
            }
            else
            {
                MainPanelOpenRightSide(RightSide_LayoutPanel, this, rightSidePanelLocation);
            }
        }
        public void MainPanelOpenLeftSide(System.Windows.Forms.Control showControlLeft,
            System.Windows.Forms.Control parentControl, System.Drawing.Point childControlLocationLeft)
        {
            if (RightSide_LayoutPanel.Visible)
            {
                GVisual.ShowControl(showControlLeft, parentControl, childControlLocationLeft);
                GVisual.ChangeSize_of_Control(drawingPanel, drawingPanelSmallSize);
                drawingPanel.Location = drawingPanelMiddleLocation;
                MoveLeft();
                GVisual.Control_TopRightCorner(btn_openClose_RightSide, drawingPanel, 3);
                btn_OpenClose_LeftSide.Image = Resources.Resource1.Chevron_Left;
            }
            else
            {
                GVisual.ShowControl(showControlLeft, parentControl, childControlLocationLeft);
                GVisual.ChangeSize_of_Control(drawingPanel, drawingPanelMiddleSize);
                drawingPanel.Location = drawingPanelMiddleLocation;
                MoveLeft();
                GVisual.Control_TopRightCorner(btn_openClose_RightSide, drawingPanel, 3);
                btn_OpenClose_LeftSide.Image = Resources.Resource1.Chevron_Left;
            }
        }
        public void MainPanelOpenRightSide(System.Windows.Forms.Control showControlRight,
            System.Windows.Forms.Control parentControl, System.Drawing.Point childControlLocationRight)
        {
            if (LeftSide_LayoutPanel.Visible)
            {
                GVisual.ShowControl(showControlRight, parentControl, childControlLocationRight);
                GVisual.ChangeSize_of_Control(drawingPanel, drawingPanelSmallSize);
                drawingPanel.Location = drawingPanelMiddleLocation;
                MoveLeft();
                GVisual.Control_TopRightCorner(btn_openClose_RightSide, drawingPanel, 3);
                btn_openClose_RightSide.Image = Resources.Resource1.Chevron_Right;
            }
            else
            {
                GVisual.ShowControl(showControlRight, parentControl, childControlLocationRight);
                GVisual.ChangeSize_of_Control(drawingPanel, drawingPanelMiddleSize);
                drawingPanel.Location = drawingPanelLeftLocation;
                MoveLeft();
                GVisual.Control_TopRightCorner(btn_openClose_RightSide, drawingPanel, 3);
                btn_openClose_RightSide.Image = Resources.Resource1.Chevron_Right;
            }
        }
        public void MainPanelOpenBothSides(System.Windows.Forms.Control showControlLeft, System.Windows.Forms.Control showControlRight,
            System.Windows.Forms.Control parentControl, System.Drawing.Point childControlLocationLeft, System.Drawing.Point childControlLocationRight)
        {
            GVisual.ShowControl(showControlLeft, parentControl, childControlLocationLeft);
            GVisual.ShowControl(showControlRight, parentControl, childControlLocationRight);
            GVisual.ChangeSize_of_Control(drawingPanel, drawingPanelSmallSize);
            drawingPanel.Location = drawingPanelMiddleLocation;
            GVisual.Control_TopRightCorner(btn_openClose_RightSide, drawingPanel, 3);
            btn_OpenClose_LeftSide.Image = Resources.Resource1.Chevron_Left;
            btn_openClose_RightSide.Image = Resources.Resource1.Chevron_Right;
        }
        public void MainPanelCloseLeftSide(System.Windows.Forms.Control hideControlLeft,
            System.Windows.Forms.Control parentControl)
        {
            if (RightSide_LayoutPanel.Visible)
            {
                GVisual.HideControl(hideControlLeft, parentControl);
                GVisual.ChangeSize_of_Control(drawingPanel, drawingPanelMiddleSize);
                drawingPanel.Location = drawingPanelLeftLocation;
                MoveRight();
                GVisual.Control_TopRightCorner(btn_openClose_RightSide, drawingPanel, 3);
                btn_OpenClose_LeftSide.Image = Resources.Resource1.Chevron_Right;
            }
            else
            {
                GVisual.HideControl(hideControlLeft, parentControl);
                GVisual.ChangeSize_of_Control(drawingPanel, drawingPanelLargeSize);
                drawingPanel.Location = drawingPanelLeftLocation;
                MoveRight();
                GVisual.Control_TopRightCorner(btn_openClose_RightSide, drawingPanel, 3);
                btn_OpenClose_LeftSide.Image = Resources.Resource1.Chevron_Right;
            }

        }
        public void MainPanelCloseRightSide(System.Windows.Forms.Control hideControlRight,
            System.Windows.Forms.Control parentControl)
        {
            if (LeftSide_LayoutPanel.Visible)
            {
                GVisual.HideControl(hideControlRight, parentControl);
                GVisual.ChangeSize_of_Control(drawingPanel, drawingPanelMiddleSize);
                drawingPanel.Location = drawingPanelMiddleLocation;
                MoveRight();
                GVisual.Control_TopRightCorner(btn_openClose_RightSide, drawingPanel, 3);
                btn_openClose_RightSide.Image = Resources.Resource1.Chevron_Left;
            }
            else
            {
                GVisual.HideControl(hideControlRight, parentControl);
                GVisual.ChangeSize_of_Control(drawingPanel, drawingPanelLargeSize);
                drawingPanel.Location = drawingPanelLeftLocation;
                MoveRight();
                GVisual.Control_TopRightCorner(btn_openClose_RightSide, drawingPanel, 3);
                btn_openClose_RightSide.Image = Resources.Resource1.Chevron_Left;
            }

        }
        public void MainPanelCloseBothSides(System.Windows.Forms.Control hideControlLeft, System.Windows.Forms.Control hideControlRight,
            System.Windows.Forms.Control parentControl)
        {
            GVisual.HideControl(hideControlLeft, parentControl);
            GVisual.HideControl(hideControlRight, parentControl);
            GVisual.ChangeSize_of_Control(drawingPanel, drawingPanelLargeSize);
            drawingPanel.Location = drawingPanelLeftLocation;
            GVisual.Control_TopRightCorner(btn_openClose_RightSide, drawingPanel, 3);
            btn_openClose_RightSide.Image = Resources.Resource1.Chevron_Left;
            btn_OpenClose_LeftSide.Image = Resources.Resource1.Chevron_Right;
        }
        public void ShowControl(System.Windows.Forms.Control parentControl, System.Windows.Forms.Control childControl)
        {
            GVisual.ShowControl(childControl, parentControl);
        }
        public void HideControl(System.Windows.Forms.Control parentControl, System.Windows.Forms.Control childControl)
        {
            GVisual.HideControl(childControl, parentControl);
            if (parentControl.Controls.Count == 0)
            {
                MainPanelCloseRightSide(RightSide_LayoutPanel, this);
            }
        }
        public void SortFlowLayoutPanel(System.Windows.Forms.Control ShowControl)
        {
            List<System.Windows.Forms.Control> controls = new List<System.Windows.Forms.Control>();
            foreach (System.Windows.Forms.Control control in LeftSide_LayoutPanel.Controls)
            {
                controls.Add(control);
            }

            foreach (System.Windows.Forms.Control control in controls)
            {
                if (control != LayoutPanel_Alan_Hierarchy)
                {
                    LeftSide_LayoutPanel.Controls.Remove(control);
                }
            }
            GVisual.ShowControl(ShowControl, LeftSide_LayoutPanel);
            GVisual.ShowControl(LayoutPanel_Alan_Hierarchy, LeftSide_LayoutPanel);
            LeftSide_LayoutPanel.Controls.SetChildIndex(LayoutPanel_Alan_Hierarchy, 1);
        }
        public void Clear_AddControltoLeftSidePanel(System.Windows.Forms.Control ShowControl)
        {
            LeftSide_LayoutPanel.Controls.Clear();
            GVisual.ShowControl(ShowControl, LeftSide_LayoutPanel);
        }
        #endregion
        #region Less Frequent UI Operations
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
        #endregion
        #region TextBox Events
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
        #endregion



        //Get Cm Values of the Location of the areas
        #region Convert to CM Methods
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
        #endregion



        private async void btn_Layout_Kaydet_Click(object sender, EventArgs e)
        {
            if (RightSide_LayoutPanel.Visible && !LeftSide_LayoutPanel.Visible)
            {
                MainPanelCloseRightSide(RightSide_LayoutPanel, this);
            }
            else if (!RightSide_LayoutPanel.Visible && LeftSide_LayoutPanel.Visible)
            {
                MainPanelCloseLeftSide(LeftSide_LayoutPanel, this);
            }
            else if (RightSide_LayoutPanel.Visible && LeftSide_LayoutPanel.Visible)
            {
                MainPanelCloseBothSides(LeftSide_LayoutPanel, RightSide_LayoutPanel, this);
            }

            if (Ambar != null)
            {
                using (var dialog = new LayoutNaming(Ambar))
                {
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        LayoutDescription = dialog.txt_Layout_Aciklama.Text;
                        LayoutName = dialog.txt_Layout_Isim.Text;

                        if (Ambar != null)
                        {
                            if (Main.ambar != null)
                            {
                                Main.ambar = null;
                            }

                            Main.ambar = Ambar;

                            Main.ambar.KareX = Ambar.Rectangle.X;
                            Main.ambar.KareY = Ambar.Rectangle.Y;
                            Main.ambar.KareEni = Ambar.Rectangle.Width;
                            Main.ambar.KareBoyu = Ambar.Rectangle.Height;
                            Main.ambar.OriginalKareX = Ambar.OriginalRectangle.X;
                            Main.ambar.OriginalKareY = Ambar.OriginalRectangle.Y;
                            Main.ambar.OriginalKareEni = Ambar.OriginalRectangle.Width;
                            Main.ambar.OriginalKareBoyu = Ambar.OriginalRectangle.Height;
                            Main.ambar.Zoomlevel = Main.Zoomlevel;

                            foreach (var conveyor in Main.ambar.conveyors)
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
                                conveyor.Zoomlevel = Main.Zoomlevel;

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
                                    reff.Zoomlevel = Main.Zoomlevel;
                                    reff.Layout = null;
                                }
                            }
                            foreach (var depo in Main.ambar.depolar)
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
                                depo.Zoomlevel = Main.Zoomlevel;
                                depo.layout = null;

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
                                    cell.Zoomlevel = Main.Zoomlevel;
                                    cell.Layout = null;
                                }
                            }

                            await Task.Run(() => Main.LayoutOlusturSecondDatabaseOperation(LayoutName, LayoutDescription, layout, Main.ambar));

                            Main.DrawingPanel.Invalidate();
                            this.Hide();
                            this.Close();
                        }
                    }
                }
            }
            else
            {
                this.DialogResult = DialogResult.Cancel;
            }
        }



        private void PanelWrap(Panel panel, System.Windows.Forms.Control HideControl, System.Windows.Forms.Control ShowControl, bool wrap)
        {
            if (wrap)
            {
                GVisual.HideControl(HideControl, panel);
                GVisual.ShowControl(ShowControl, panel);
            }
            else
            {
                GVisual.ShowControl(ShowControl, panel);
                GVisual.ShowControl(HideControl, panel);
            }
        }
        private void btn_OpenClose_Alan_Olusturma_Paneli_Click(object sender, EventArgs e)
        {
            if (Alan_Olusturma_Paneli.Size == SmallPanelSize)
            {
                GVisual.ChangeSize_of_Control(Alan_Olusturma_Paneli, new Size(310, 388));
                RightSide_LayoutPanel.ScrollControlIntoView(Alan_Olusturma_Paneli);
                GVisual.HideControl(lbl_Small_Alan_Title, SubPanel_Alan_Olusturma_Paneli_Controls);
                btn_OpenClose_Alan_Olusturma_Paneli.Image = Resources.Resource1.Chevron_Up;
                PanelWrap(Alan_Olusturma_Paneli, SubPanel_Alan_Olusturma_Paneli, SubPanel_Alan_Olusturma_Paneli_Controls, false);
            }
            else
            {
                GVisual.ChangeSize_of_Control(Alan_Olusturma_Paneli, SmallPanelSize);
                GVisual.ShowControl(lbl_Small_Alan_Title, SubPanel_Alan_Olusturma_Paneli_Controls);
                btn_OpenClose_Alan_Olusturma_Paneli.Image = Resources.Resource1.Chevron_Down;
                PanelWrap(Alan_Olusturma_Paneli, SubPanel_Alan_Olusturma_Paneli, SubPanel_Alan_Olusturma_Paneli_Controls, true);
            }
        }
        private void btn_OpenClose_Izgara_Haritasi_Olusturma_Paneli_Click(object sender, EventArgs e)
        {
            if (Izgara_Olusturma_Paneli.Size == SmallPanelSize)
            {
                GVisual.ChangeSize_of_Control(Izgara_Olusturma_Paneli, new Size(310, 883));
                RightSide_LayoutPanel.ScrollControlIntoView(Izgara_Olusturma_Paneli);
                GVisual.HideControl(lbl_Small_Izgara_Title, SubPanel_Izgara_Haritasi_Olusturma_Paneli_Controls);
                btn_OpenClose_Izgara_Haritasi_Olusturma_Paneli.Image = Resources.Resource1.Chevron_Up;
                PanelWrap(Izgara_Olusturma_Paneli, SubPanel_Izgara_Haritasi_Olusturma_Paneli, SubPanel_Izgara_Haritasi_Olusturma_Paneli_Controls, false);
            }
            else
            {
                GVisual.ChangeSize_of_Control(Izgara_Olusturma_Paneli, SmallPanelSize);
                GVisual.ShowControl(lbl_Small_Izgara_Title, SubPanel_Izgara_Haritasi_Olusturma_Paneli_Controls);
                btn_OpenClose_Izgara_Haritasi_Olusturma_Paneli.Image = Resources.Resource1.Chevron_Down;
                PanelWrap(Izgara_Olusturma_Paneli, SubPanel_Izgara_Haritasi_Olusturma_Paneli, SubPanel_Izgara_Haritasi_Olusturma_Paneli_Controls, true);
            }
        }
        private void btn_OpenClose_Depo_Olusturma_Paneli_Click(object sender, EventArgs e)
        {
            if (Depo_Olusturma_Paneli.Size == SmallPanelSize)
            {
                GVisual.ChangeSize_of_Control(Depo_Olusturma_Paneli, new Size(310, 883));
                RightSide_LayoutPanel.ScrollControlIntoView(Depo_Olusturma_Paneli);
                GVisual.HideControl(lbl_Small_Depo_Title, SubPanel_Depo_Olusturma_Paneli_Controls);
                btn_OpenClose_Depo_Olusturma_Paneli.Image = Resources.Resource1.Chevron_Up;
                PanelWrap(Depo_Olusturma_Paneli, SubPanel_Depo_Olusturma_Paneli, SubPanel_Depo_Olusturma_Paneli_Controls, false);
            }
            else
            {
                GVisual.ChangeSize_of_Control(Depo_Olusturma_Paneli, SmallPanelSize);
                GVisual.ShowControl(lbl_Small_Depo_Title, SubPanel_Depo_Olusturma_Paneli_Controls);
                btn_OpenClose_Depo_Olusturma_Paneli.Image = Resources.Resource1.Chevron_Down;
                PanelWrap(Depo_Olusturma_Paneli, SubPanel_Depo_Olusturma_Paneli, SubPanel_Depo_Olusturma_Paneli_Controls, true);
            }
        }
        private void btn_OpenClose_Conveyor_Olusturma_Paneli_Click(object sender, EventArgs e)
        {
            if (Conveyor_Olusturma_Paneli.Size == SmallPanelSize)
            {
                GVisual.ChangeSize_of_Control(Conveyor_Olusturma_Paneli, new Size(310, 420));
                RightSide_LayoutPanel.ScrollControlIntoView(Conveyor_Olusturma_Paneli);
                GVisual.HideControl(lbl_Small_Conveyor_Title, SubPanel_Conveyor_Olusturma_Paneli_Controls);
                btn_OpenClose_Conveyor_Olusturma_Paneli.Image = Resources.Resource1.Chevron_Up;
                PanelWrap(Conveyor_Olusturma_Paneli, SubPanel_Conveyor_Olusturma_Paneli, SubPanel_Conveyor_Olusturma_Paneli_Controls, false);
            }
            else
            {
                GVisual.ChangeSize_of_Control(Conveyor_Olusturma_Paneli, SmallPanelSize);
                GVisual.ShowControl(lbl_Small_Conveyor_Title, SubPanel_Conveyor_Olusturma_Paneli_Controls);
                btn_OpenClose_Conveyor_Olusturma_Paneli.Image = Resources.Resource1.Chevron_Down;
                PanelWrap(Conveyor_Olusturma_Paneli, SubPanel_Conveyor_Olusturma_Paneli, SubPanel_Conveyor_Olusturma_Paneli_Controls, true);
            }
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
        /*//public void AdjustTextBoxLocations(RectangleF rectangle, RectangleF parentRectangle,
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
        //}*/
    }
}
