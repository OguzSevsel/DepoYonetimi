using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Balya_Yerleştirme.Models;

namespace Balya_Yerleştirme
{
    public partial class LayoutNaming : Form
    {
        Ambar Ambar { get; set; }
        Isletme isletme { get; set; }

        public LayoutNaming(Ambar ambar, Isletme isletme)
        {
            InitializeComponent();
            this.Ambar = ambar;
            this.isletme = isletme;
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

            using (var context = new DBContext())
            {
                var layout = (from x in context.Layout
                              where x.Name == layout_isim && x.IsletmeID == isletme.IsletmeID
                              select x).FirstOrDefault();

                if (layout != null)
                {
                    if (layout.LayoutId != Ambar.LayoutId)
                    {
                        errorProvider.SetError(txt_Layout_Isim, "Aynı isimli bir layout zaten bulunuyor lütfen başka bir isim seçin");
                        txt_Layout_Isim.Clear();
                    }
                }
            }

            if (!errorProvider.HasErrors)
            {
                this.DialogResult = DialogResult.OK;
            }
        }
    }
}
