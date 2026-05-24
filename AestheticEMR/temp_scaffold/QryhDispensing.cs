using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhDispensing
{
    public string ConsultId { get; set; } = null!;

    public DateTime? DtDate { get; set; }

    public string DrgName { get; set; } = null!;

    public string DrgCatName { get; set; } = null!;

    public decimal? Qty { get; set; }

    public string? Usage { get; set; }

    public string Fullname { get; set; } = null!;

    public bool? AttendedTo { get; set; }

    public string Pno { get; set; } = null!;

    public string? EmpName { get; set; }

    public decimal? Price { get; set; }

    public decimal? Amount { get; set; }

    public decimal? Cost { get; set; }

    public long Id { get; set; }

    public string? LocId { get; set; }

    public string? QtyUnit { get; set; }

    public string? ClientCatId { get; set; }

    public string RetainName { get; set; } = null!;

    public string RetainCode { get; set; } = null!;

    public DateTime? DtTime { get; set; }

    public DateTime? EntryDate { get; set; }

    public DateTime? EntryTime { get; set; }

    public string? ClientName { get; set; }

    public string? AppName { get; set; }

    public bool? IsPost { get; set; }
}
