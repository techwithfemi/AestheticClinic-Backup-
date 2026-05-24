using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwPaymentDetailsSumm2
{
    public string ReceiptNo { get; set; } = null!;

    public decimal AmountPaid { get; set; }

    public decimal? AmountPaidDetail { get; set; }

    public DateTime ReceiptDate { get; set; }

    public DateTime ReceiptDateDetail { get; set; }

    public decimal? Diff { get; set; }

    public string BillNo { get; set; } = null!;

    public string PaymentFor { get; set; } = null!;
}
