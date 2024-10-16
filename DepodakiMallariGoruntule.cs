using Balya_Yerleştirme.Models;
using DocumentFormat.OpenXml.Spreadsheet;
using GUI_Library;
using Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Balya_Yerleştirme
{
    public partial class DepodakiMallariGoruntule : Form
    {
        public Depo Depo { get; set; }

        public DepodakiMallariGoruntule(Depo depo)
        {
            InitializeComponent();
            Depo = depo;

            DepoDataGridView.DataSource = null;
            DepoDataGridView.Columns.Clear();
            DepoDataGridView.Rows.Clear();

            BindingSource bindingSource = new BindingSource();

            DataTable cellDataTable = new DataTable();

            cellDataTable.Columns.Add("Depo Adı", typeof(string));
            cellDataTable.Columns.Add("Nesne Etiketi", typeof(string));
            cellDataTable.Columns.Add("Hücre Numarası", typeof(string));

            DepoDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            DepoDataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            DepoDataGridView.AllowUserToAddRows = false;
            DepoDataGridView.AllowUserToResizeColumns = false;
            DepoDataGridView.AllowUserToResizeRows = false;
            DepoDataGridView.AllowUserToDeleteRows = false;
            DepoDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            DepoDataGridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;

            foreach (var cell in Depo.gridmaps)
            {
                PopulateDataGrid(cell, bindingSource, cellDataTable);
            }

            GVisual.AdjustDataGridViewSize(DepoDataGridView, DepoDataPanel);

            GVisual.Move_TopSide_of_AnotherControl(lbl_Nesne_Gotuntule_Title, DepoDataPanel, 10);
        }

        private void DataGridView_VisibleChanged(object sender, EventArgs e)
        {
            DataGridView_PaintCells(Depo);
        }
        private void DataGridView_PaintCells(Depo depo)
        {
            int nrRows = DepoDataGridView.Rows.Count;
            int nrColumns = DepoDataGridView.Columns.Count;
            System.Drawing.Color blue = System.Drawing.Color.AliceBlue;
            System.Drawing.Color coral = System.Drawing.Color.Coral;
            System.Drawing.Color selectedColor = System.Drawing.Color.Coral;

            foreach (var cell in depo.gridmaps)
            {
                for (int i = 0; i < nrRows; i++)
                {
                    string cell_etiketi = (string)DepoDataGridView.Rows[i].Cells[2].Value;
                    if (cell_etiketi == cell.CellEtiketi)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            if (selectedColor == System.Drawing.Color.Coral)
                            {
                                DepoDataGridView.Rows[i].Cells[j].Style.BackColor = blue;
                            }
                            else
                            {
                                DepoDataGridView.Rows[i].Cells[j].Style.BackColor = coral;
                            }
                        }
                    }
                }
                if (selectedColor == System.Drawing.Color.Coral)
                {
                    selectedColor = blue;
                }
                else
                {
                    selectedColor = coral;
                }
            }
        }

        public void PopulateDataGrid(Models.Cell cell, BindingSource bindingSource, DataTable cellDataTable)
        {
            foreach (var item in cell.items)
            {
                int index = cell.items.FindIndex(i => i == item);

                    DataRow dataRowuser = cellDataTable.NewRow();
                dataRowuser["Depo Adı"] = Depo.DepoName;
                dataRowuser["Nesne Etiketi"] = item.ItemEtiketi;
                dataRowuser["Hücre Numarası"] = cell.CellEtiketi;
                
                cellDataTable.Rows.Add(dataRowuser);

                bindingSource.DataSource = cellDataTable;
                DepoDataGridView.DataSource = bindingSource;
            }
        }
    }
}
