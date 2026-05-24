using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhDispensingOffline
{
    public string ConsultId { get; set; } = null!;

    public DateTime? DtDate { get; set; }

    public string DrgName { get; set; } = null!;

    public string DrgCatName { get; set; } = null!;

    public double Qty { get; set; }

    public string? Fullname { get; set; }

    public string? Usage { get; set; }

    public bool? AttendedTo { get; set; }

    public string? PNo { get; set; }

    public string? EmpName { get; set; }

    public double? Price { get; set; }

    public double? Amount { get; set; }

    public double? Cost { get; set; }

    public string QtyUnit { get; set; } = null!;

    public string ClientCatId { get; set; } = null!;

    public string RetainName { get; set; } = null!;

    public string RetainCode { get; set; } = null!;

    public DateTime? DtTime { get; set; }

    public long Id { get; set; }

    public DateTime? EntryDate { get; set; }

    public DateTime? EntryTime { get; set; }

    public string? ClientName { get; set; }

    public string? AppName { get; set; }

    public bool? IsPost { get; set; }
}
