using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class PaymentDeposit
{
    public long Id { get; set; }

    public DateTime DtDate { get; set; }

    public string ConsultId { get; set; } = null!;

    public string Pno { get; set; } = null!;

    public double Amount { get; set; }

    public string? Remarks { get; set; }
}
