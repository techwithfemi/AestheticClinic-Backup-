using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class vwGL
{
    public long? SNo { get; set; }

    public long SNoCOA { get; set; }

    public string AccountID { get; set; } = null!;

    public string AccountNo { get; set; } = null!;

    public string AccountName { get; set; } = null!;

    public decimal AccountOpAmt { get; set; }

    public decimal AccountClAmt { get; set; }

    public decimal Amount { get; set; }

    public string Period { get; set; } = null!;

    public string CoyID { get; set; } = null!;

    public string GroupID { get; set; } = null!;

    public string CatID { get; set; } = null!;

    public string CatMasterID { get; set; } = null!;

    public string LedgerCode { get; set; } = null!;

    public string GroupName { get; set; } = null!;

    public long GroupMin { get; set; }

    public long GroupMax { get; set; }

    public string? Remarks { get; set; }

    public string CatName { get; set; } = null!;

    public string CatMasterName { get; set; } = null!;

    public string? TranID { get; set; }

    public DateTime? TranDate { get; set; }

    public decimal Debit { get; set; }

    public decimal? Credit { get; set; }

    public string? Description { get; set; }

    public string? DebitDescription { get; set; }

    public string? CreditDescription { get; set; }

    public string? CostCenterID { get; set; }

    public string? UserName { get; set; }

    public DateTime? EntryDate { get; set; }

    public DateTime? EntryTime { get; set; }

    public string? AppName { get; set; }

    public string? ClientName { get; set; }

    public string? RptType { get; set; }

    public bool? Hidden { get; set; }

    public string? RptLevel { get; set; }

    public bool? Suppres { get; set; }

    public short? RptSerial { get; set; }

    public string? RptTitle { get; set; }

    public string? PeriodVal { get; set; }
}
