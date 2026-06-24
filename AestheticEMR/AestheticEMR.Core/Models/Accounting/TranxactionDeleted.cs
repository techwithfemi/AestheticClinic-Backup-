using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class TranxactionDeleted
{
    public long? SNo { get; set; }

    public string? TranID { get; set; }

    public string? TranNo { get; set; }

    public DateTime? TranDate { get; set; }

    public string? AccountID { get; set; }

    public string? CostCenterID { get; set; }

    public decimal? Amount { get; set; }

    public string? Description { get; set; }

    public string? TranCat { get; set; }

    public DateTime? EntryDate { get; set; }

    public string? Period { get; set; }

    public DateTime? Prd2 { get; set; }

    public string? UserName { get; set; }

    public string? Remarks { get; set; }

    public DateTime EntryDate2 { get; set; }

    public DateTime EntryTime { get; set; }

    public string AppName { get; set; } = null!;

    public string ClientName { get; set; } = null!;
}
