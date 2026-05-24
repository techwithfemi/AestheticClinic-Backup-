using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HDebtReview
{
    public long Sno { get; set; }

    public DateTime DtDate { get; set; }

    public string BillNo { get; set; } = null!;

    public double Debt { get; set; }

    public double AdjustTo { get; set; }

    public bool? Attendedto { get; set; }

    public string? Pno { get; set; }

    public string? Remarks { get; set; }

    public DateTime? EntryDate { get; set; }

    public DateTime? EntryTime { get; set; }

    public string? ClientName { get; set; }

    public string? AppName { get; set; }
}
