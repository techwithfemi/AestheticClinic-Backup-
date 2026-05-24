using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwhPayment
{
    public DateTime Date { get; set; }

    public string ReceiptNo { get; set; } = null!;

    public string BillNo { get; set; } = null!;

    public string PNo { get; set; } = null!;

    public decimal AmountPaid { get; set; }

    public string PaymentFor { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public string? CoyName { get; set; }

    public string? Remarks { get; set; }
}
