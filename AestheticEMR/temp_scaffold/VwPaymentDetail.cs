using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwPaymentDetail
{
    public DateTime ReceiptDate { get; set; }

    public string ReceiptNo { get; set; } = null!;

    public decimal AmountPaid { get; set; }

    public string RevType { get; set; } = null!;

    public string BillNo { get; set; } = null!;

    public decimal? AmountToPay { get; set; }

    public string AccountNo { get; set; } = null!;

    public string? RetainCode { get; set; }

    public string? ClientType { get; set; }
}
