using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class vwTranxArchive
{
    public string TranID { get; set; } = null!;

    public string TranNo { get; set; } = null!;

    public DateTime TranDate { get; set; }

    public string AccountID { get; set; } = null!;

    public string CostCenterID { get; set; } = null!;

    public decimal Amount { get; set; }

    public string TranCat { get; set; } = null!;

    public DateTime EntryDate { get; set; }

    public string Period { get; set; } = null!;

    public DateTime? Prd2 { get; set; }

    public string UserName { get; set; } = null!;

    public string? Remarks { get; set; }

    public string AccountNo { get; set; } = null!;

    public bool? isClose { get; set; }
}
