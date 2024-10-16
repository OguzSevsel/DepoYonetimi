using System;
using System.Collections.Generic;

namespace Balya_Yerleştirme.Models;

public partial class Conveyor
{
    public Conveyor() { }

    public Conveyor(int ambarId, float? conveyorAraligi, float kareX, float kareY, 
        float kareEni, float kareBoyu, float originalKareX, float originalKareY, float originalKareEni, 
        float originalKareBoyu, float zoomlevel, float conveyor_eni, float conveyor_boyu)
    {
        AmbarId = ambarId;
        ConveyorAraligi = conveyorAraligi;
        KareX = kareX;
        KareY = kareY;
        KareEni = kareEni;
        KareBoyu = kareBoyu;
        OriginalKareX = originalKareX;
        OriginalKareY = originalKareY;
        OriginalKareEni = originalKareEni;
        OriginalKareBoyu = originalKareBoyu;
        Zoomlevel = zoomlevel;
        ConveyorEni = conveyor_eni;
        ConveyorBoyu = conveyor_boyu;
    }

    public int ConveyorId { get; set; }

    public int AmbarId { get; set; }

    public float? ConveyorAraligi { get; set; }

    public float KareX { get; set; }

    public float KareY { get; set; }

    public float KareEni { get; set; }

    public float KareBoyu { get; set; }

    public float OriginalKareX { get; set; }

    public float OriginalKareY { get; set; }

    public float OriginalKareEni { get; set; }

    public float OriginalKareBoyu { get; set; }

    public float Zoomlevel { get; set; }

    public float ConveyorEni { get; set; }

    public float ConveyorBoyu { get; set; }


    public virtual Ambar Ambar { get; set; } = null!;

    public virtual ICollection<ConveyorReferencePoint> ConveyorReferencePoints { get; set; } = new List<ConveyorReferencePoint>();
}
