using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryPayment
{
    public DateTime ReceiptDate { get; set; }

    public string ReceiptNo { get; set; } = null!;

    public string BillNo { get; set; } = null!;

    public string PNo { get; set; } = null!;

    public string PaymentFor { get; set; } = null!;

    public decimal AmountPaid { get; set; }

    public string Fullname { get; set; } = null!;

    public string RetainName { get; set; } = null!;

    public decimal AmountBilled { get; set; }

    public int DebtBf { get; set; }

    public string? Remarks { get; set; }
}
