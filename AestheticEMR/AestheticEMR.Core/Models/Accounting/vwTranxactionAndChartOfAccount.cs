using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class vwTranxactionAndChartOfAccount
{
    public string AccountID { get; set; } = null!;

    public string Period { get; set; } = null!;

    public string CoyID { get; set; } = null!;

    public string AccountNo { get; set; } = null!;

    public string AccountName { get; set; } = null!;

    public decimal AccountOpAmt { get; set; }

    public decimal AccountClAmt { get; set; }

    public decimal? TranxAmount { get; set; }

    public string GroupName { get; set; } = null!;

    public string CatName { get; set; } = null!;

    public bool? Hidden { get; set; }

    public string GroupID { get; set; } = null!;

    public string CatID { get; set; } = null!;

    public string CatMasterID { get; set; } = null!;

    public string CatMasterName { get; set; } = null!;

    public string? RptType { get; set; }

    public string? RptLevel { get; set; }

    public bool? Suppres { get; set; }

    public short? RptSerial { get; set; }

    public string? RptTitle { get; set; }

    public string? Periodval { get; set; }
}
