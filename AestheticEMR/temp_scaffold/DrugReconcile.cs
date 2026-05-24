using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class DrugReconcile
{
    public long Sno { get; set; }

    public string? DrgName { get; set; }

    public string Drgcatname { get; set; } = null!;

    public DateTime RecDate { get; set; }

    public DateTime? RecTime { get; set; }

    public decimal? PhyStock { get; set; }

    public decimal? SysStock { get; set; }

    public int Mth { get; set; }

    public int Yr { get; set; }

    public string? Remarks { get; set; }

    public string? Drgcode { get; set; }

    public decimal? Cost { get; set; }

    public decimal? Amount { get; set; }

    public string? LocId { get; set; }

    public DateTime? EntryDate { get; set; }

    public DateTime? EntryTime { get; set; }

    public string? ClientName { get; set; }

    public string? AppName { get; set; }

    public bool? IsPost { get; set; }

    public bool? Suppres { get; set; }

    public string? AcctIdinv { get; set; }

    public string? AcctIdrecon { get; set; }
}
