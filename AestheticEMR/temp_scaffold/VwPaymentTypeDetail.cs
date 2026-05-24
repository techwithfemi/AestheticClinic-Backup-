using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwPaymentTypeDetail
{
    public DateTime ReceiptDate { get; set; }

    public string ReceiptNo { get; set; } = null!;

    public decimal AmountPaid { get; set; }

    public string RevType { get; set; } = null!;

    public string BillNo { get; set; } = null!;

    public decimal? AmountToPay { get; set; }

    public string AccountNo { get; set; } = null!;

    public string? RetainCode { get; set; }

    public string PaymentFor { get; set; } = null!;

    public string PayType { get; set; } = null!;

    public string? FullName { get; set; }

    public string RevType2 { get; set; } = null!;
}
