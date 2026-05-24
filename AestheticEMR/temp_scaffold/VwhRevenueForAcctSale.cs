using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwhRevenueForAcctSale
{
    public long Sno { get; set; }

    public DateTime? EntryDate { get; set; }

    public string DrgName { get; set; } = null!;

    public string CoyId { get; set; } = null!;

    public string Company { get; set; } = null!;

    public string BillNo { get; set; } = null!;

    public decimal? SubTotal { get; set; }

    public decimal AmtPaid { get; set; }

    public decimal? AmtDiff { get; set; }

    public string? RevType { get; set; }

    public string? AcctDebit { get; set; }

    public string? AcctCredit { get; set; }

    public string? Active { get; set; }

    public bool IsPost { get; set; }

    public bool? Suppres { get; set; }

    public string? Remarks { get; set; }

    public bool? Reversed { get; set; }

    public string? TranId { get; set; }

    public long? ReversedPair { get; set; }
}
