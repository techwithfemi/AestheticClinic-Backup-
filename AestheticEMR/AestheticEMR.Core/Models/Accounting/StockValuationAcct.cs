using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class StockValuationAcct
{
    public long SNo { get; set; }

    public string ItemName { get; set; } = null!;

    public decimal AmtOpBal { get; set; }

    public decimal AmtPurch { get; set; }

    public decimal AmtAvailBal { get; set; }

    public string Period { get; set; } = null!;

    public string CoyID { get; set; } = null!;

    public string? Remarks { get; set; }

    public DateTime EntryDate { get; set; }

    public DateTime EntryTime { get; set; }

    public string AppName { get; set; } = null!;

    public string ClientName { get; set; } = null!;
}
