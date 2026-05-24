using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwPaymentsSumm
{
    public string BillNo { get; set; } = null!;

    public decimal? AmountBilled { get; set; }

    public decimal? AmountPaid { get; set; }
}
