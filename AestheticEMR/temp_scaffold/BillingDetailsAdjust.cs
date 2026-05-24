using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class BillingDetailsAdjust
{
    public long Sno { get; set; }

    public DateTime AdjustDate { get; set; }

    public DateTime AdjustTime { get; set; }

    public string BillNo { get; set; } = null!;

    public string BillItem { get; set; } = null!;

    public decimal OldQty { get; set; }

    public decimal NewQty { get; set; }

    public decimal OldPrice { get; set; }

    public decimal NewPrice { get; set; }

    public string AdjustBy { get; set; } = null!;

    public string? Remarks { get; set; }
}
