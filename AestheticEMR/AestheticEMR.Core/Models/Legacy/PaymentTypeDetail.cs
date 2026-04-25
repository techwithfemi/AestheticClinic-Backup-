using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Legacy;

public partial class PaymentTypeDetail
{
    public long SNo { get; set; }

    public string ReceiptNo { get; set; } = null!;

    public string billNO { get; set; } = null!;

    public DateTime ReceiptDate { get; set; }

    public decimal AmountPaid { get; set; }

    public string AccountNo { get; set; } = null!;

    public string RevType { get; set; } = null!;

    public string PayType { get; set; } = null!;

    public bool isPost { get; set; }

    public decimal? AmountToPay { get; set; }

    public string? BillItem { get; set; }

    public DateTime? BillDate { get; set; }

    public long? SNoID { get; set; }
}
