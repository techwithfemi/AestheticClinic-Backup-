using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class vwtranxJournal
{
    public long SNo { get; set; }

    public string TranID { get; set; } = null!;

    public string TranNo { get; set; } = null!;

    public DateTime TranDate { get; set; }

    public string CenterName { get; set; } = null!;

    public decimal Amount { get; set; }

    public string? Description { get; set; }

    public string TranCat { get; set; } = null!;

    public DateTime EntryDate { get; set; }

    public string Period { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string? Remarks { get; set; }

    public DateTime? Prd2 { get; set; }

    public string AccountNo { get; set; } = null!;

    public string AccountName { get; set; } = null!;

    public string GroupName { get; set; } = null!;

    public string? CatName { get; set; }

    public string? CatName2 { get; set; }

    public string DeptName { get; set; } = null!;

    public string DivName { get; set; } = null!;

    public decimal? AcctBal { get; set; }

    public string? CoyID { get; set; }

    public string CostCenterID { get; set; } = null!;

    public string? CatID { get; set; }

    public bool? isPost { get; set; }
}
