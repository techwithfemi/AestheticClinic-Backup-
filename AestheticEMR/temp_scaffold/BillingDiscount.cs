using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class BillingDiscount
{
    public long Sno { get; set; }

    public DateTime DtDate { get; set; }

    public DateTime? DtTime { get; set; }

    public string BillNo { get; set; } = null!;

    public decimal Amount { get; set; }

    public decimal Pcent { get; set; }

    public decimal AmountBilled { get; set; }

    public string DrgName { get; set; } = null!;

    public string? Remarks { get; set; }

    public DateTime? EntryDate { get; set; }

    public DateTime? EntryTime { get; set; }

    public string? ClientName { get; set; }

    public string? AppName { get; set; }
}
