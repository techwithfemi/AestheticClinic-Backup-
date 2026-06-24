using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class vwAccountInfoPandL
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

    public string CatID { get; set; } = null!;

    public string Expr1 { get; set; } = null!;

    public string CatMasterID { get; set; } = null!;

    public string CatMasterName { get; set; } = null!;

    public string AccountID { get; set; } = null!;

    public bool? HiddenGp { get; set; }

    public decimal Debit { get; set; }

    public decimal Credit { get; set; }

    public decimal? AmountAbs { get; set; }

    public int? Yr { get; set; }

    public decimal? AmountRev { get; set; }

    public string GroupID { get; set; } = null!;
}
