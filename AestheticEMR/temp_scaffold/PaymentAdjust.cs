using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class PaymentAdjust
{
    public long Sno { get; set; }

    public string ReceiptNo { get; set; } = null!;

    public decimal AmountOriginal { get; set; }

    public decimal AmountNew { get; set; }

    public string AdjustType { get; set; } = null!;

    public DateTime EntryDate { get; set; }

    public DateTime AdjustTime { get; set; }

    public string? AdjustBy { get; set; }
}
