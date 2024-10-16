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
    public partial class LayoutNaming : Form
    {
        public LayoutNaming()
        {
            InitializeComponent();
        }

        private void LayoutNaming_Load(object sender, EventArgs e)
        {

        }

        private void btn_Layout_Onayla_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();
            string layout_aciklama = txt_Layout_Aciklama.Text;
            string layout_isim = txt_Layout_Isim.Text;

            if (string.IsNullOrWhiteSpace(layout_isim))
            {
                errorProvider.SetError(txt_Layout_Isim, "Bu alan boş bırakılamaz.");
            }
            if (string.IsNullOrWhiteSpace(layout_aciklama))
            {
                errorProvider.SetError(txt_Layout_Aciklama, "Bu alan boş bırakılamaz.");
            }

            if (layout_isim.Length > 30)
            {
                errorProvider.SetError(txt_Layout_Isim, "Layout'un ismi 30 karakterden uzun olamaz.");
            }

            if (!errorProvider.HasErrors)
            {
                this.DialogResult = DialogResult.OK;
            }
        }
    }
}
