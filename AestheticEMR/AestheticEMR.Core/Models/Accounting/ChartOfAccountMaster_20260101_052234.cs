using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class ChartOfAccountMaster_20260101_052234
{
    public long SNo { get; set; }

    public string AccountID { get; set; } = null!;

    public string AccountNo { get; set; } = null!;

    public string GroupID { get; set; } = null!;

    public string AccountName { get; set; } = null!;

    public decimal AccountOpAmt { get; set; }

    public decimal AccountClAmt { get; set; }

    public string? AccountDesc { get; set; }

    public bool? Hidden { get; set; }

    public string? AccountCat { get; set; }

    public string? RptType { get; set; }

    public string? AccountAddress { get; set; }

    public string? AccountSalesTaxNo { get; set; }

    public DateTime? DeprStartDate { get; set; }

    public DateTime? DeprEndDate { get; set; }

    public DateTime? DeprNextDate { get; set; }

    public decimal? DeprAmount { get; set; }

    public int? DeprCount { get; set; }

    public bool? isDummy { get; set; }

    public string? ExtID { get; set; }

    public string? ExtIDType { get; set; }

    public bool isContra { get; set; }

    public bool? isPerm { get; set; }

    public DateTime? EntryDate { get; set; }

    public DateTime? EntryTime { get; set; }

    public string? ClientName { get; set; }

    public string? AppName { get; set; }
}
