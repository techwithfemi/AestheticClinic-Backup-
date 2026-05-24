using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwBillingProcessDetailsGroupedVerify
{
    public string BillNo { get; set; } = null!;

    public double? Subtotal { get; set; }

    public decimal AmountBilled { get; set; }

    public double? Diff { get; set; }

    public string PNo { get; set; } = null!;
}
