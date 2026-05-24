using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwDebtByBillNoSumm
{
    public string PNo { get; set; } = null!;

    public decimal? AmountBilled { get; set; }

    public decimal? AmountPaid { get; set; }

    public decimal? Balance { get; set; }

    public string? ClientId { get; set; }

    public string BillNo { get; set; } = null!;
}
