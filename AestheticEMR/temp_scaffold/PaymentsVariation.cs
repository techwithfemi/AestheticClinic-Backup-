using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class PaymentsVariation
{
    public DateTime ReceiptDate { get; set; }

    public string ReceiptNo { get; set; } = null!;

    public string BillNo { get; set; } = null!;

    public string PNo { get; set; } = null!;

    public string? ClientId { get; set; }

    public double Bf { get; set; }

    public double Discount { get; set; }

    public double Cf { get; set; }

    public double? RctBillBal { get; set; }
}
