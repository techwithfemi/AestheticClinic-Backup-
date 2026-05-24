using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwBillingDetailsAdjust
{
    public DateTime AdjustDate { get; set; }

    public DateTime RecDate { get; set; }

    public string PNo { get; set; } = null!;

    public string BillNo { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public string BillItem { get; set; } = null!;

    public string? EmpName { get; set; }

    public string? EmpId { get; set; }

    public string Company { get; set; } = null!;

    public decimal OldQty { get; set; }

    public decimal NewQty { get; set; }

    public decimal OldPrice { get; set; }

    public decimal NewPrice { get; set; }

    public decimal? OldAmount { get; set; }

    public decimal? NewAmount { get; set; }

    public string? Remarks { get; set; }
}
