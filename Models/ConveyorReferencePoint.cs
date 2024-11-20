using System;
using System.Collections.Generic;

namespace Balya_Yerleştirme.Models;

public partial class ConveyorReferencePoint
{
    public ConveyorReferencePoint() { }

    public ConveyorReferencePoint(int conveyorId, int ambarId, float kareX, float kareY, 
        float kareEni, float kareBoyu, float originalKareX, float originalKareY, float originalKareEni, 
        float originalKareBoyu, float zoomlevel, float pointsize, float originalLocationInsideParentX, float originalLocationInsideParentY, float locationX, float locationY, string fixedPointLocation)
    {
        ConveyorId = conveyorId;
        AmbarId = ambarId;
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
        this.OriginalLocationInsideParentX = originalLocationInsideParentX;
        this.OriginalLocationInsideParentY = originalLocationInsideParentY;
        this.LocationX = locationX;
        this.LocationY = locationY;
        this.FixedPointLocation = fixedPointLocation;
    }

public int ReferenceId { get; set; }

    public int ConveyorId { get; set; }

    public int AmbarId { get; set; }

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

    public float OriginalLocationInsideParentX { get; set; }

    public float OriginalLocationInsideParentY { get; set; }

    public float LocationX { get; set; }

    public float LocationY { get; set; }

    public string FixedPointLocation { get; set; }

    public virtual Conveyor Conveyor { get; set; } = null!;
}
