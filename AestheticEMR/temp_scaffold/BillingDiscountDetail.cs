using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class BillingDiscountDetail
{
    public long Sno { get; set; }

    public DateTime DtDate { get; set; }

    public string BillNo { get; set; } = null!;

    public string DrgName { get; set; } = null!;

    public decimal Amount { get; set; }

    public bool? IsPost { get; set; }

    public DateTime? EntryDate { get; set; }

    public DateTime? EntryTime { get; set; }

    public string? ClientName { get; set; }

    public string? AppName { get; set; }

    public bool? Suppres { get; set; }

    public bool? Reversed { get; set; }
}
