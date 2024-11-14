using System;
using System.Collections.Generic;

namespace Balya_Yerleştirme.Models;

public partial class Depo
{
    public Depo() { }
    public Depo(int ambar_id, string depo_name, string depo_description, float depo_alani_eni,
        float depo_alani_boyu, int depo_alani_yuksekligi, float original_depo_size_width,
        float original_depo_size_height, float kare_x, float kare_y, float kare_eni, float kare_boyu,
        float original_kare_x, float original_kare_y, float original_kare_eni, float original_kare_boyu,
        float zoomlevel, string itemDrop_StartLocation, string itemDrop_UpDown, 
        string itemDrop_LeftRight, int itemDrop_Stage1, int itemDrop_Stage2,
        int yerlestirilme_Sirasi, float depo_alani_eni_cm, float depo_alani_boyu_cm,
        int column_count, int row_count, int current_column, int current_row, string item_turu,
        int asama1_ItemSayisi, int asama2_ToplamItemSayisi, int current_stage, string item_turu_secondary)
    {
        AmbarId = ambar_id;
        DepoName = depo_name;
        DepoDescription = depo_description;
        DepoAlaniEni = depo_alani_eni;
        DepoAlaniBoyu = depo_alani_boyu;
        DepoAlaniYuksekligi = depo_alani_yuksekligi;
        OriginalDepoSizeWidth = original_depo_size_width;
        OriginalDepoSizeHeight = original_depo_size_height;
        KareX = kare_x;
        KareY = kare_y;
        KareEni = kare_eni;
        KareBoyu = kare_boyu;
        OriginalKareX = original_kare_x;
        OriginalKareY = original_kare_y;
        OriginalKareEni = original_kare_eni;
        OriginalKareBoyu = original_kare_boyu;
        Zoomlevel = zoomlevel;
        this.itemDrop_StartLocation = itemDrop_StartLocation;
        this.itemDrop_UpDown = itemDrop_UpDown;
        this.itemDrop_LeftRight = itemDrop_LeftRight;
        asama1_Yuksekligi = itemDrop_Stage1;
        asama2_Yuksekligi = itemDrop_Stage2;
        Yerlestirilme_Sirasi = yerlestirilme_Sirasi;
        Depo_Alani_Eni_Cm = depo_alani_eni_cm;
        Depo_Alani_Boyu_Cm = depo_alani_boyu_cm;
        ColumnCount = column_count;
        RowCount = row_count;
        currentColumn = current_column;
        currentRow = current_row;
        ItemTuru = item_turu;
        ItemTuruSecondary = item_turu_secondary;
        this.asama1_ItemSayisi = asama1_ItemSayisi;
        this.asama2_ToplamItemSayisi = asama2_ToplamItemSayisi;
        this.currentStage = current_stage;
    }

    public int DepoId { get; set; }

    public int AmbarId { get; set; }

    public string DepoName { get; set; } = null!;

    public string DepoDescription { get; set; } = null!;

    public float DepoAlaniEni { get; set; }

    public float DepoAlaniBoyu { get; set; }

    public int DepoAlaniYuksekligi { get; set; }

    public float OriginalDepoSizeWidth { get; set; }

    public float OriginalDepoSizeHeight { get; set; }

    public float KareX { get; set; }

    public float KareY { get; set; }

    public float KareEni { get; set; }

    public float KareBoyu { get; set; }

    public float OriginalKareX { get; set; }

    public float OriginalKareY { get; set; }

    public float OriginalKareEni { get; set; }

    public float OriginalKareBoyu { get; set; }

    public float Zoomlevel { get; set; }

    public string itemDrop_StartLocation { get; set; }

    public string itemDrop_UpDown { get; set; }

    public string itemDrop_LeftRight { get; set; }

    public int asama1_Yuksekligi { get; set; }

    public int asama2_Yuksekligi { get; set; }

    public int Yerlestirilme_Sirasi { get; set; }

    public float Depo_Alani_Eni_Cm { get; set; }

    public float Depo_Alani_Boyu_Cm { get; set; }

    public int RowCount { get; set; }

    public int ColumnCount { get; set; }

    public int currentColumn { get; set; }

    public int currentRow { get; set; }

    public string ItemTuru { get; set; }

    public string ItemTuruSecondary { get; set; }

    public int asama1_ItemSayisi { get; set; }

    public int asama2_ToplamItemSayisi { get; set; }

    public int currentStage { get; set; }


    public virtual Ambar Ambar { get; set; } = null!;

    public virtual ICollection<Cell> Cells { get; set; } = new List<Cell>();
}
