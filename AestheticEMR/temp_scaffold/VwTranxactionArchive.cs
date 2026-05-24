using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwTranxactionArchive
{
    public long Sno { get; set; }

    public DateTime AttndDate { get; set; }

    public DateTime? BillDate { get; set; }

    public string? Pno { get; set; }

    public string? BillNo { get; set; }

    public decimal Bill { get; set; }

    public decimal AmountPaid { get; set; }

    public decimal RunningTotal { get; set; }

    public string? Remarks { get; set; }

    public string Comapany { get; set; } = null!;

    public int? Seed { get; set; }
}
