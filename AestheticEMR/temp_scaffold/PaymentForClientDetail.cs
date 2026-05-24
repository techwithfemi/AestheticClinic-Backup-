using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class PaymentForClientDetail
{
    public DateTime ReceiptDate { get; set; }

    public string ReceiptNo { get; set; } = null!;

    public string ClientId { get; set; } = null!;

    public string BillingMonth { get; set; } = null!;

    public int? BillingYear { get; set; }

    public decimal Amount { get; set; }
}
