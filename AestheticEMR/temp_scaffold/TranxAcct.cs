using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class TranxAcct
{
    public long Sno { get; set; }

    public string TranId { get; set; } = null!;

    public string AccountId { get; set; } = null!;

    public string TranNo { get; set; } = null!;

    public DateTime TranDate { get; set; }

    public string CostCenterId { get; set; } = null!;

    public decimal Amount { get; set; }

    public string? Description { get; set; }

    public string TranCat { get; set; } = null!;

    public DateTime EntryDate { get; set; }

    public string Period { get; set; } = null!;

    public DateTime? Prd2 { get; set; }

    public string UserName { get; set; } = null!;

    public string? Remarks { get; set; }

    public string? CoyId { get; set; }
}
