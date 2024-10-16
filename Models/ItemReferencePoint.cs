using System;
using System.Collections.Generic;

namespace Balya_Yerleştirme.Models;

public partial class ItemReferencePoint
{
    public ItemReferencePoint() { }

    public ItemReferencePoint(int ıtemId, float kareX, float kareY, float kareEni, float kareBoyu, 
        float originalKareX, float originalKareY, float originalKareEni, float originalKareBoyu, 
        float zoomlevel, float pointsize)
    {
        ItemId = ıtemId;
        KareX = kareX;
        KareY = kareY;
        KareEni = kareEni;
        KareBoyu = kareBoyu;
        OriginalKareX = originalKareX;
        OriginalKareY = originalKareY;
        OriginalKareEni = originalKareEni;
        OriginalKareBoyu = originalKareBoyu;
        Zoomlevel = zoomlevel;
        Pointsize = pointsize;
    }

    public int ReferenceId { get; set; }

    public int ItemId { get; set; }

    public float KareX { get; set; }

    public float KareY { get; set; }

    public float KareEni { get; set; }

    public float KareBoyu { get; set; }

    public float OriginalKareX { get; set; }

    public float OriginalKareY { get; set; }

    public float OriginalKareEni { get; set; }

    public float OriginalKareBoyu { get; set; }

    public float Zoomlevel { get; set; }

    public float Pointsize { get; set; }

    public virtual Item Item { get; set; } = null!;
}
