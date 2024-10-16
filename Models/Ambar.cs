using System;
using System.Collections.Generic;

namespace Balya_Yerleştirme.Models;

public partial class Ambar
{
    public Ambar() { }

    public Ambar(int layout_id, string ambar_name, string ambar_description, float kare_x,
        float kare_y, float kare_eni, float kare_boyu, float original_kare_x, float original_kare_y,
        float original_kare_eni, float original_kare_boyu, float zoomlevel, float ambar_eni, float ambar_boyu) 
    {
        LayoutId = layout_id;
        AmbarName = ambar_name;
        AmbarDescription = ambar_description;
        KareX = kare_x;
        KareY = kare_y;
        KareEni = kare_eni;
        KareBoyu = kare_boyu;
        OriginalKareX = original_kare_x;
        OriginalKareY = original_kare_y;
        OriginalKareEni = original_kare_eni;
        OriginalKareBoyu = original_kare_boyu;
        Zoomlevel = zoomlevel;
        AmbarEni = ambar_eni;
        AmbarBoyu = ambar_boyu;
    }


    public int AmbarId { get; set; }

    public int LayoutId { get; set; }

    public string AmbarName { get; set; } = null!;

    public string AmbarDescription { get; set; } = null!;

    public float KareX { get; set; }

    public float KareY { get; set; }

    public float KareEni { get; set; }

    public float KareBoyu { get; set; }

    public float OriginalKareX { get; set; }

    public float OriginalKareY { get; set; }

    public float OriginalKareEni { get; set; }

    public float OriginalKareBoyu { get; set; }

    public float Zoomlevel { get; set; }
    public float AmbarEni { get; set; }
    public float AmbarBoyu { get; set; }


    public virtual ICollection<Conveyor> Conveyors { get; set; } = new List<Conveyor>();

    public virtual ICollection<Depo> Depos { get; set; } = new List<Depo>();
}
