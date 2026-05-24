using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwPaymentsByCashier
{
    public DateTime ReceiptDate { get; set; }

    public string ReceiptNo { get; set; } = null!;

    public string PNo { get; set; } = null!;

    public string BillNo { get; set; } = null!;

    public string? Fullname { get; set; }

    public string PaymentFor { get; set; } = null!;

    public decimal AmountBilled { get; set; }

    public decimal AmountPaid { get; set; }

    public string? EmpName { get; set; }

    public string? EmpId { get; set; }

    public decimal? Balance { get; set; }

    public DateTime? RTime { get; set; }

    public string PayType { get; set; } = null!;

    public string? PhoneNo { get; set; }
}
