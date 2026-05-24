using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class PaymentForClientCredit
{
    public DateTime ReceiptDate { get; set; }

    public string ReceiptNo { get; set; } = null!;

    public string ClientId { get; set; } = null!;

    public decimal AmountCredit { get; set; }

    public bool IsUsed { get; set; }
}
