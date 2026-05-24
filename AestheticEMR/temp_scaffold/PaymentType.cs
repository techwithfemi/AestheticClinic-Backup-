using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class PaymentType
{
    public long Sno { get; set; }

    public string ReceiptNo { get; set; } = null!;

    public decimal AmountPaid { get; set; }

    public string PayType { get; set; } = null!;

    public DateTime? ReceiptDate { get; set; }

    public bool? IsPost { get; set; }

    public string? AccountNo { get; set; }

    public bool? Suppres { get; set; }

    public bool? Reversed { get; set; }

    public DateTime? EntryDate { get; set; }

    public DateTime? EntryTime { get; set; }

    public string? ClientName { get; set; }

    public string? AppName { get; set; }

    public string? TranId { get; set; }
}
