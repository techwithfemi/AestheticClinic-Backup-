using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwPaymentTypesSummGrid
{
    public DateTime? ReceiptDate { get; set; }

    public string AccountToDebit { get; set; } = null!;

    public decimal? AmountPaid { get; set; }

    public string PayType { get; set; } = null!;

    public bool? IsPost { get; set; }
}
