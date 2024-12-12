using System;
using System.Collections.Generic;

namespace Balya_Yerleştirme.Models;

public partial class LogicController
{
    public LogicController() { }

    public LogicController(string plc_ipAddress, string depo_in_db_address, string depo_out_db_address, string balya_onay_db_address, string balya_beklet_db_address, string balya_hazir_db_address, string vinc_balya_al_db_address)
    {
        PLC_IPAddress = plc_ipAddress;
        DepoIn_DBAddress = depo_in_db_address;
        DepoOut_DBAddress = depo_out_db_address;
        BalyaOnay_DBAddress = balya_onay_db_address;
        BalyaBeklet_DBAddress = balya_beklet_db_address;
        BalyaHazir_DBAddress = balya_hazir_db_address;
        VincBalyaAl_DBAddress = vinc_balya_al_db_address;
    }

    public int PLC_id { get; set; }

    public string PLC_IPAddress { get; set; }

    public string DepoIn_DBAddress { get; set; }

    public string DepoOut_DBAddress { get; set; }

    public string BalyaOnay_DBAddress{ get; set; }

    public string BalyaBeklet_DBAddress { get; set; }

    public string BalyaHazir_DBAddress { get; set; }

    public string VincBalyaAl_DBAddress { get; set; }

    public string PLC_Password { get; set; }
}