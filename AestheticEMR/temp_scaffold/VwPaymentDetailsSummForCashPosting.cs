using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwPaymentDetailsSummForCashPosting
{
    public DateTime? ReceiptDate { get; set; }

    public string AccountNo { get; set; } = null!;

    public decimal? AmountPaid { get; set; }

    public string RevType { get; set; } = null!;

    public bool? IsPost { get; set; }

    public string Remarks { get; set; } = null!;
}
