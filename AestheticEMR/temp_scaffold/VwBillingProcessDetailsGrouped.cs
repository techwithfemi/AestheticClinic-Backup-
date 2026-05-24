using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwBillingProcessDetailsGrouped
{
    public string BillNo { get; set; } = null!;

    public decimal? Subtotal { get; set; }

    public string Pno { get; set; } = null!;

    public DateTime RecDate { get; set; }

    public DateTime BillDate { get; set; }

    public string RetainId { get; set; } = null!;
}
