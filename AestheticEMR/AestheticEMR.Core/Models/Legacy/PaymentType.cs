using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Legacy;

public partial class PaymentType
{
    public long SNo { get; set; }

    public string ReceiptNo { get; set; } = null!;

    public decimal AmountPaid { get; set; }

    public string PayType { get; set; } = null!;

    public DateTime? ReceiptDate { get; set; }

    public bool? isPost { get; set; }

    public string? AccountNo { get; set; }

    public bool? suppres { get; set; }

    public bool? reversed { get; set; }

    public DateTime? EntryDate { get; set; }

    public DateTime? EntryTime { get; set; }

    public string? ClientName { get; set; }

    public string? AppName { get; set; }

    public string? TranID { get; set; }
}
