using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwhpaymentsSumm
{
    public string Fullname { get; set; } = null!;

    public string PNo { get; set; } = null!;

    public string BillNo { get; set; } = null!;

    public decimal? AmountPaid { get; set; }
}
