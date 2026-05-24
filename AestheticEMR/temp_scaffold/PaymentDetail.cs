using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class PaymentDetail
{
    public long Sno { get; set; }

    public string ReceiptNo { get; set; } = null!;

    public string BillNo { get; set; } = null!;

    public DateTime ReceiptDate { get; set; }

    public decimal AmountPaid { get; set; }

    public string AccountNo { get; set; } = null!;

    public string RevType { get; set; } = null!;

    public bool IsPost { get; set; }

    public decimal? AmountToPay { get; set; }

    public string? BillItem { get; set; }

    public DateTime? BillDate { get; set; }

    public long? SnoId { get; set; }

    public bool? Suppres { get; set; }
}
