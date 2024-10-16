using Balya_Yerleştirme.Models;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Balya_Yerleştirme.Utilities.Utils;

namespace Balya_Yerleştirme
{
    public partial class InfoForm : Form
    {
        public List<Item> items;
        public Cell cell;

        public InfoForm(List<Item> Items, Cell Cell)
        {
            InitializeComponent();
            this.MaximizeBox = false;
            items = Items;
            cell = Cell;
            Hide_InfoPanel();
            MakeGridViewPanelBigger();
            PopulateDataGrid(items, cell);
        }

        private void CellDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = CellDataGridView.CurrentRow;

            if (row != null)
            {
                var cells = row.Cells;
                string balya_etiketi = (string)cells["Balya Etiketi"].Value;

                foreach (var item in items)
                {
                    float total_item_yuksekligi = 0;
                    MakeGridViewPanelSmaller();
                    foreach (var x in items)
                    {
                        total_item_yuksekligi += x.ItemYuksekligi;
                    }
                    Show_InfoPanel(item.ItemEtiketi, item.ItemEni, item.ItemBoyu,
                        item.ItemYuksekligi, item.ItemAgirligi, cell.CellEtiketi, cell.Parent.DepoName,
                        total_item_yuksekligi, cell.CellYuksekligi * 100);
                }            
            }
        }

        public void PopulateDataGrid(List<Item> items, Cell cell)
        {
            CellDataGridView.DataSource = null;
            CellDataGridView.Columns.Clear();
            CellDataGridView.Rows.Clear();

            BindingSource bindingSource = new BindingSource();

            DataTable cellDataTable = new DataTable();

            cellDataTable.Columns.Add("Balya Etiketi", typeof(string));
            cellDataTable.Columns.Add("Hücre Numarası", typeof(string));
            cellDataTable.Columns.Add("Hücrede Kaçıncı Sırada", typeof(string));

            CellDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            CellDataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            CellDataGridView.AllowUserToAddRows = false;
            CellDataGridView.AllowUserToResizeColumns = false;
            CellDataGridView.AllowUserToResizeRows = false;
            CellDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            CellDataGridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;


            foreach (var item in items)
            {
                int index = items.FindIndex(i => i == item);

                DataRow dataRowuser = cellDataTable.NewRow();
                dataRowuser["Balya Etiketi"] = item.ItemEtiketi;
                dataRowuser["Hücre Numarası"] = cell.CellEtiketi;
                if (item == items.Last())
                {
                    dataRowuser["Hücrede Kaçıncı Sırada"] = $"{index + 1}. Sırada (En Tepede)";
                }
                else if (item == items.First())
                {
                    dataRowuser["Hücrede Kaçıncı Sırada"] = $"{index + 1}. Sırada (En Aşağıda)";
                }
                else
                {
                    dataRowuser["Hücrede Kaçıncı Sırada"] = $"{index + 1}. Sırada";
                }
                cellDataTable.Rows.Add(dataRowuser);
            }
            bindingSource.DataSource = cellDataTable;
            CellDataGridView.DataSource = bindingSource;

            AdjustCellDataGridViewSize();
        }
        private void AdjustCellDataGridViewSize()
        {
            int totalHeight = CellDataGridView.Rows.GetRowsHeight(DataGridViewElementStates.Visible) + CellDataGridView.ColumnHeadersHeight;
            int totalWidth = CellDataGridView.Columns.GetColumnsWidth(DataGridViewElementStates.Visible) + CellDataGridView.RowHeadersWidth;

            if (totalHeight < CellDataPanel.Height)
            {
                CellDataGridView.ClientSize = new Size(totalWidth, totalHeight);
                CellDataGridView.Location = MoveControlInsidePanel(CellDataGridView,
                    CellDataPanel, "Merkez");
                lbl_Cell_Data_Grid_Title.Location = CenterControlInsidePanelX(
                    lbl_Cell_Data_Grid_Title, GridViewPanel);
                CellDataPanel.Location = MoveControltoSidesofAnotherControl(CellDataPanel,
                    lbl_Cell_Data_Grid_Title, 10, "Alt");
            }
            else
            {
                CellDataGridView.ClientSize = new Size(totalWidth, CellDataPanel.Height);
                CellDataGridView.Location = MoveControlInsidePanel(CellDataGridView,
                    CellDataPanel, "Merkez");
                lbl_Cell_Data_Grid_Title.Location = CenterControlInsidePanelX(
                    lbl_Cell_Data_Grid_Title, GridViewPanel);
                CellDataPanel.Location = MoveControltoSidesofAnotherControl(CellDataPanel,
                    lbl_Cell_Data_Grid_Title, 10, "Alt");
            }
        }
        private void Show_InfoPanel(string balya_etiketi, float balyanin_eni,
            float balyanin_boyu, float balyanin_yuksekligi, float balyanin_agirligi,
            string hucre_etiketi, string depo_adi, float total_balya_yuksekligi, float cell_yuksekligi)
        {
            InfoPanel.Enabled = true;
            InfoPanel.Visible = true;
            InfoPanel.Location = new Point(12, 12);
            InfoPanel.Show();
            this.Controls.Add(InfoPanel);
            lbl_Info_Balya_Etiketi_Value.Text = $"{balya_etiketi}";
            lbl_Info_Balya_Eni_Value.Text = $"{balyanin_eni} cm";
            lbl_Info_Balya_Boyu_Value.Text = $"{balyanin_boyu} cm";
            lbl_Info_Balya_Yuksekligi_Value.Text = $"{balyanin_yuksekligi}  cm";
            lbl_Info_Balya_Agirligi_Value.Text = $"{balyanin_agirligi} kg";
            lbl_Info_Hucre_Etiketi_Value.Text = $"{hucre_etiketi}";
            lbl_Info_Depo_Adi_Value.Text = $"{depo_adi}";
            lbl_Cell_Yuksekligi_Value.Text = $"{cell_yuksekligi} cm";
            lbl_Balya_Yuksekligi_Value.Text = $"{total_balya_yuksekligi} cm";
        }

        private void Hide_InfoPanel()
        {
            InfoPanel.Enabled = false;
            InfoPanel.Visible = false;
            InfoPanel.Hide();
            this.Controls.Remove(InfoPanel);
        }

        private void MakeGridViewPanelSmaller()
        {
            GridViewPanel.Size = new Size(888, 657);
            GridViewPanel.Location = new Point(364, 12);
            CellDataPanel.Location = MoveControlInsidePanel(CellDataPanel, GridViewPanel, "Merkez");
            lbl_Cell_Data_Grid_Title.Location = MoveControltoSidesofAnotherControl(
                lbl_Cell_Data_Grid_Title, CellDataPanel, 10, "Üst");
        }
        private void MakeGridViewPanelBigger()
        {
            GridViewPanel.Size = new Size(1240, 657);
            GridViewPanel.Location = new Point(12, 12);
            CellDataPanel.Location = MoveControlInsidePanel(CellDataPanel, GridViewPanel, "Merkez");
            lbl_Cell_Data_Grid_Title.Location = MoveControltoSidesofAnotherControl(
                lbl_Cell_Data_Grid_Title, CellDataPanel, 10, "Üst");
        }

        private void btn_kapat_Click(object sender, EventArgs e)
        {
            MakeGridViewPanelBigger();
            Hide_InfoPanel();
        }
    }
}
