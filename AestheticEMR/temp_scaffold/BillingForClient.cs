using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class BillingForClient
{
    public DateTime BDate { get; set; }

    public DateTime? ConsultDate { get; set; }

    public string BillNo { get; set; } = null!;

    public string PNo { get; set; } = null!;

    public string ClientId { get; set; } = null!;

    public decimal AmountBilled { get; set; }

    public string? AmountBilledInWord { get; set; }

    public string BillingMonth { get; set; } = null!;

    public int BillingYear { get; set; }

    public string Diagnosis { get; set; } = null!;

    public bool IsPaid { get; set; }
}
