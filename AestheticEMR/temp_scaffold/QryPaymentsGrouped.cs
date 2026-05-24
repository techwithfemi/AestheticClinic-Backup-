using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryPaymentsGrouped
{
    public string PNo { get; set; } = null!;

    public string BillNo { get; set; } = null!;

    public decimal? SumOfAmountPaid { get; set; }
}
