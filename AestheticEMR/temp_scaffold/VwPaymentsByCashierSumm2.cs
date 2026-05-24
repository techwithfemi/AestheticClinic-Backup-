using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwPaymentsByCashierSumm2
{
    public DateTime ReceiptDate { get; set; }

    public string EmpName { get; set; } = null!;

    public string PNo { get; set; } = null!;

    public string BillNo { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public decimal? AmountPaid { get; set; }

    public decimal? Balance { get; set; }

    public decimal? AmountBilled { get; set; }

    public string? PhoneNo { get; set; }
}
