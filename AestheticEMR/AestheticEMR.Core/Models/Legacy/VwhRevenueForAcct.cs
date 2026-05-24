using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Legacy;

public partial class VwhRevenueForAcct
{
    public long Sno { get; set; }

    public DateTime BillDate { get; set; }

    public string BillNo { get; set; } = null!;

    public string BillItem { get; set; } = null!;

    public decimal? SubTotal { get; set; }

    public string? RevType { get; set; }

    public string? AccountNo { get; set; }

    public bool? IsRct { get; set; }

    public decimal AmtPaid { get; set; }

    public decimal? AmtDiff { get; set; }

    public int? Serial { get; set; }

    public bool? IsPost { get; set; }

    public string? Active { get; set; }

    public string? InvNo { get; set; }

    public bool? IsProcess { get; set; }
}
