using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwBillingCompare
{
    public DateTime Date { get; set; }

    public string PNo { get; set; } = null!;

    public string BillNo { get; set; } = null!;

    public decimal? Subtotal { get; set; }

    public decimal? AmountBilled { get; set; }

    public decimal? AmountPaid { get; set; }
}
