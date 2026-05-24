using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwPaymentDetailsSummGrid
{
    public decimal? AmountPaid { get; set; }

    public DateTime ReceiptDate { get; set; }

    public string AccountToCredit { get; set; } = null!;

    public string RevType { get; set; } = null!;

    public bool IsPost { get; set; }
}
