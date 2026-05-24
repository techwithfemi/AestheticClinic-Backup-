using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwPaymentTypesNoReversal
{
    public long Sno { get; set; }

    public string ReceiptNo { get; set; } = null!;

    public decimal AmountPaid { get; set; }

    public string PayType { get; set; } = null!;

    public bool? IsPost { get; set; }

    public DateTime? ReceiptDate { get; set; }

    public string? AccountNo { get; set; }

    public string? TranId { get; set; }
}
