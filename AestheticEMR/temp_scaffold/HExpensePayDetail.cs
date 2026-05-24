using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HExpensePayDetail
{
    public long Sno { get; set; }

    public string VouchNo { get; set; } = null!;

    public DateTime PayDate { get; set; }

    public DateTime PayTime { get; set; }

    public string ItemCode { get; set; } = null!;

    public double Qty { get; set; }

    public double Price { get; set; }

    public double Amount { get; set; }

    public string Description { get; set; } = null!;

    public string PayType { get; set; } = null!;

    public string? Remarks { get; set; }

    public string? ChequeNo { get; set; }

    public DateTime? ValueDate { get; set; }

    public string AcctNoCreit { get; set; } = null!;

    public string AcctNoDebit { get; set; } = null!;

    public DateTime? ChequeDate { get; set; }

    public bool? IsPost { get; set; }

    public DateTime? EntryDate { get; set; }

    public DateTime? EntryTime { get; set; }

    public string? ClientName { get; set; }

    public string? AppName { get; set; }

    public bool? Suppres { get; set; }

    public long? ExpIdSno { get; set; }
}
