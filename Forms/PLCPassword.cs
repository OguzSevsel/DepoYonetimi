using Balya_Yerleştirme.Models;
using Microsoft.EntityFrameworkCore;
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
    public partial class PLCPassword : Form
    {
        public PLCPassword()
        {
            InitializeComponent();
        }

        private void btn_PLC_Password_Onayla_Click(object sender, EventArgs e)
        {
            using (var context = new DBContext())
            {
                var PLC = (from x in context.LogicControllers
                           select x).FirstOrDefault();

                if (PLC != null)
                {
                    if (PLC.PLC_Password == txt_PLC_Password.Text)
                    {
                        using (var dialog = new PLC())
                        {
                            if (dialog.ShowDialog() == DialogResult.OK)
                            {
                                string depo_in = dialog.txt_PLC_DepoIn_DB_Address.Text;
                                string depo_out = dialog.txt_PLC_DepoOut_DB_Address.Text;
                                string balya_onay = dialog.txt_PLC_BalyaOnay_DB_Address.Text;
                                string balya_beklet = dialog.txt_PLC_BalyaBeklet_DB_Address.Text;
                                string balya_hazir = dialog.txt_PLC_BalyaHazir_DB_Address.Text;
                                string vinc_balya_al = dialog.txt_PLC_VincBalyaAl_DB_Address.Text;
                                string plc_ip_address = dialog.txt_PLC_IP_Address.Text;

                                PLC.DepoIn_DBAddress = depo_in;
                                PLC.DepoOut_DBAddress = depo_out;
                                PLC.BalyaOnay_DBAddress = balya_onay;
                                PLC.BalyaBeklet_DBAddress = balya_beklet;
                                PLC.BalyaHazir_DBAddress = balya_hazir;
                                PLC.VincBalyaAl_DBAddress = vinc_balya_al;
                                PLC.PLC_IPAddress = plc_ip_address;

                                context.SaveChanges();

                                this.DialogResult = DialogResult.OK;
                            }
                        }
                    }
                }
            }
        }

        private void btn_PLC_Password_Vazgec_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
