using Balya_Yerleştirme.Models;
using GUI_Library;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Balya_Yerleştirme.Forms
{
    public partial class PLC : Form
    {
        public PLC()
        {
            InitializeComponent();
            GVisual.HideControl(panel_New_Password, this);
            using (var context = new DBContext())
            {
                var PLC = (from x in context.LogicControllers
                           select x).FirstOrDefault();

                if (PLC != null)
                {
                    txt_PLC_IP_Address.Text = PLC.PLC_IPAddress;
                    txt_PLC_DepoIn_DB_Address.Text = PLC.DepoIn_DBAddress;
                    txt_PLC_DepoOut_DB_Address.Text = PLC.DepoOut_DBAddress;
                    txt_PLC_BalyaOnay_DB_Address.Text = PLC.BalyaOnay_DBAddress;
                    txt_PLC_BalyaBeklet_DB_Address.Text = PLC.BalyaBeklet_DBAddress;
                    txt_PLC_BalyaHazir_DB_Address.Text = PLC.BalyaHazir_DBAddress;
                    txt_PLC_VincBalyaAl_DB_Address.Text = PLC.VincBalyaAl_DBAddress;
                }
            }
        }

        private void btn_PLC_Sifre_Degistir_Click(object sender, EventArgs e)
        {
            GVisual.ShowControl(panel_New_Password, this);
            GVisual.HideControl(btn_PLC_Sifre_Degistir, this);
        }

        private void ResetPasswordPanel()
        {
            GVisual.ShowControl(btn_PLC_Sifre_Degistir, this);
            GVisual.HideControl(panel_New_Password, this);
            txt_New_Password.Text = "Lütfen Şifre Girin";
        }

        private void txt_New_Password_Enter(object sender, EventArgs e)
        {
            txt_New_Password.Clear();
        }

        private void btn_New_Password_Onayla_Click(object sender, EventArgs e)
        {
            using (var context = new DBContext())
            {
                var PLC = (from x in context.LogicControllers
                           select x).FirstOrDefault();

                if (PLC != null)
                {
                    string password = txt_New_Password.Text;

                    if (string.IsNullOrWhiteSpace(password))
                    {
                        errorProvider.SetError(txt_New_Password, "Şifre boşluktan oluşamaz, lütfen geçerli bir şifre girin");
                    }

                    if (!errorProvider.HasErrors)
                    {
                        PLC.PLC_Password = password;
                        context.SaveChanges();
                        ResetPasswordPanel();
                    }
                }
            }
        }

        private void btn_New_Password_Vazgec_Click(object sender, EventArgs e)
        {
            ResetPasswordPanel();
        }
    }
}
