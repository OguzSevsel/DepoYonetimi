using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Balya_Yerleştirme.Models;

public partial class Item
{
    public Item() { }

    public Item(int cell_id, string item_etiketi, string item_turu, float item_eni, float item_boyu, int item_yuksekligi,
        float kare_x, float kare_y, float kare_eni, float kare_boyu, float original_kare_x, float original_kare_y,
        float original_kare_eni, float original_kare_boyu, float zoomlevel, float item_agirligi, string item_aciklamasi,
        float cm_x_axis, float cm_y_axis, float cm_z_axis)
    {
        CellId = cell_id;
        ItemEtiketi = item_etiketi;
        ItemTuru = item_turu;
        ItemEni = item_eni;
        ItemBoyu = item_boyu;
        ItemYuksekligi = item_yuksekligi;
        KareX = kare_x;
        KareY = kare_y;
        KareEni = kare_eni;
        KareBoyu = kare_boyu;
        OriginalKareX = original_kare_x;
        OriginalKareY = original_kare_y;
        OriginalKareEni = original_kare_eni;
        OriginalKareBoyu = original_kare_boyu;
        Zoomlevel = zoomlevel;
        ItemAgirligi = item_agirligi;
        ItemAciklamasi = item_aciklamasi;
        Cm_X_Axis = cm_x_axis;
        Cm_Y_Axis = cm_y_axis;
        Cm_Z_Axis = cm_z_axis;
    }

    public int ItemId { get; set; }

    public int CellId { get; set; }

    public string ItemEtiketi { get; set; } = null!;

    public string ItemTuru { get; set; } = null!;

    public float ItemEni { get; set; }

    public float ItemBoyu { get; set; }

    public int ItemYuksekligi { get; set; }

    public float KareX { get; set; }

    public float KareY { get; set; }

    public float KareEni { get; set; }

    public float KareBoyu { get; set; }

    public float OriginalKareX { get; set; }

    public float OriginalKareY { get; set; }

    public float OriginalKareEni { get; set; }

    public float OriginalKareBoyu { get; set; }

    public float Zoomlevel { get; set; }

    public float ItemAgirligi { get; set; }

    public string ItemAciklamasi { get; set; }

    public float Cm_X_Axis { get; set; }

    public float Cm_Y_Axis { get; set; }

    public float Cm_Z_Axis { get; set; }

    public virtual Cell Cell { get; set; } = null!;

    public virtual ICollection<ItemReferencePoint> ItemReferencePoints { get; set; } = new List<ItemReferencePoint>();
}
