using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class ChartOfAccountsPreArchive
{
    public long SNo { get; set; }

    public string AccountID { get; set; } = null!;

    public string Period { get; set; } = null!;

    public string AccountNo { get; set; } = null!;

    public string GroupID { get; set; } = null!;

    public string AccountName { get; set; } = null!;

    public decimal AccountOpAmt { get; set; }

    public decimal AccountClAmt { get; set; }

    public string? AccountCat { get; set; }

    public string? AccountDesc { get; set; }

    public string? AccountAddress { get; set; }

    public string? AccountSalesTaxNo { get; set; }

    public bool? Hidden { get; set; }

    public string? RptType { get; set; }
}
