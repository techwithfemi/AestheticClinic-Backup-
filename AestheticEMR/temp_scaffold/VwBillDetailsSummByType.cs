using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwBillDetailsSummByType
{
    public string BillNo { get; set; } = null!;

    public string DrgName { get; set; } = null!;

    public decimal? AmountAccum { get; set; }

    public string Billtype { get; set; } = null!;

    public decimal? Amount { get; set; }

    public decimal Price { get; set; }

    public decimal Qty { get; set; }

    public string? ConId { get; set; }

    public string? RevType { get; set; }

    public string? BillType2 { get; set; }
}
