using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwPaymentDetailsSumm
{
    public string ReceiptNo { get; set; } = null!;

    public decimal? AmountPaid { get; set; }

    public DateTime ReceiptDate { get; set; }

    public string BillNo { get; set; } = null!;
}
