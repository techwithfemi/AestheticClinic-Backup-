using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhDispensingPending
{
    public string ConsultId { get; set; } = null!;

    public DateTime DtDate { get; set; }

    public string DrgName { get; set; } = null!;

    public string DrgCatName { get; set; } = null!;

    public double Qty { get; set; }

    public string? Usage { get; set; }

    public string Fullname { get; set; } = null!;

    public bool? AttendedTo { get; set; }

    public string Pno { get; set; } = null!;

    public string? EmpName { get; set; }

    public double? Price { get; set; }

    public double? Amount { get; set; }

    public double? Cost { get; set; }

    public long Id { get; set; }

    public string? LocId { get; set; }

    public string? QtyUnit { get; set; }

    public string? ClientCatId { get; set; }

    public string RetainName { get; set; } = null!;

    public string? RetainCode { get; set; }

    public DateTime? DtTime { get; set; }

    public string? Pending { get; set; }

    public DateTime? DueDate { get; set; }
}
