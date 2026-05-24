using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class DrugReconcileGen
{
    public long Sno { get; set; }

    public string DrgName { get; set; } = null!;

    public string Drgcatname { get; set; } = null!;

    public DateTime RecDate { get; set; }

    public DateTime? RecTime { get; set; }

    public double PhyStock { get; set; }

    public double SysStock { get; set; }

    public int Mth { get; set; }

    public int Yr { get; set; }

    public string? Remarks { get; set; }
}
