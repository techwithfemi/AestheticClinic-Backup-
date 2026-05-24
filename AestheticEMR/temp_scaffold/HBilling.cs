using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HBilling
{
    public DateTime BDate { get; set; }

    public string BillNo { get; set; } = null!;

    public string PNo { get; set; } = null!;

    public DateTime ConsultDate { get; set; }

    public string? PaymentFor { get; set; }

    public decimal AmountBilled { get; set; }

    public decimal AmountPaid { get; set; }

    public string? AmountInWord { get; set; }

    public string? Receivedby { get; set; }

    public DateTime? BalanceDate { get; set; }

    public string? PayType { get; set; }
}
