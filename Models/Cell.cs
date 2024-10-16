using System;
using System.Collections.Generic;

namespace Balya_Yerleştirme.Models;

public partial class Cell
{
    public Cell() { }

    public Cell(int depo_id, string cell_etiketi, float cell_eni, float cell_boyu, int cell_yuksekligi,
        int cell_mal_sayisi, float kare_x, float kare_y, float kare_eni, float kare_boyu,
        float original_kare_x, float original_kare_y, float original_kare_eni, float original_kare_boyu,
        float zoomlevel, int item_sayisi, float dikey_kenar_boslugu,
        float yatay_kenar_boslugu, float nesne_eni, float nesne_boyu, int nesne_yuksekligi,
        int column, int row, int total_item_z, float cell_Cm_X, float cell_Cm_Y) 
    { 
        DepoId = depo_id;
        CellEtiketi = cell_etiketi;
        CellEni = cell_eni;
        CellBoyu = cell_boyu;
        CellYuksekligi = cell_yuksekligi;
        CellMalSayisi = cell_mal_sayisi;
        KareX = kare_x;
        KareY = kare_y;
        KareEni = kare_eni;
        KareBoyu = kare_boyu;
        OriginalKareX = original_kare_x;
        OriginalKareY = original_kare_y;
        OriginalKareEni = original_kare_eni;
        OriginalKareBoyu = original_kare_boyu;
        Zoomlevel = zoomlevel;
        ItemSayisi = item_sayisi;
        DikeyKenarBoslugu = dikey_kenar_boslugu;
        YatayKenarBoslugu = yatay_kenar_boslugu;
        NesneEni = nesne_eni;
        NesneBoyu = nesne_boyu;
        NesneYuksekligi = nesne_yuksekligi;
        Column = column;
        Row = row;
        toplam_Nesne_Yuksekligi = total_item_z;
        this.cell_Cm_X = cell_Cm_X;
        this.cell_Cm_Y = cell_Cm_Y;
    }
    public int CellId { get; set; }

    public int DepoId { get; set; }

    public string CellEtiketi { get; set; } = null!;

    public float CellEni { get; set; }

    public float CellBoyu { get; set; }

    public int CellYuksekligi { get; set; }

    public int CellMalSayisi { get; set; }

    public float KareX { get; set; }

    public float KareY { get; set; }

    public float KareEni { get; set; }

    public float KareBoyu { get; set; }

    public float OriginalKareX { get; set; }

    public float OriginalKareY { get; set; }

    public float OriginalKareEni { get; set; }

    public float OriginalKareBoyu { get; set; }

    public float Zoomlevel { get; set; }

    public int ItemSayisi { get; set; }

    public float DikeyKenarBoslugu {  get; set; }

    public float YatayKenarBoslugu { get; set; }

    public float NesneEni { get; set; }

    public float NesneBoyu { get; set; }

    public int NesneYuksekligi { get; set; }

    public int Column {  get; set; }

    public int Row { get; set; }

    public int toplam_Nesne_Yuksekligi { get; set; }

    public float cell_Cm_X { get; set; }

    public float cell_Cm_Y{ get; set; }


    public virtual Depo Depo { get; set; } = null!;

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
}
