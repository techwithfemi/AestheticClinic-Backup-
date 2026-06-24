using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class vwTranxForGrid
{
    public DateTime TranDate { get; set; }

    public string AccountName { get; set; } = null!;

    public string AccountNo { get; set; } = null!;

    public decimal Debit { get; set; }

    public decimal? Credit { get; set; }

    public string? Description { get; set; }

    public string TranNo { get; set; } = null!;

    public string? TranCat { get; set; }

    public string? BillNo { get; set; }

    public string CostCenter { get; set; } = null!;

    public DateTime EntryDate { get; set; }

    public string Period { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public long SNo { get; set; }

    public string? Remarks { get; set; }

    public string CoyID { get; set; } = null!;

    public bool isClose { get; set; }
}
