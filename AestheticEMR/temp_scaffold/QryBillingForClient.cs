using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryBillingForClient
{
    public string BillNo { get; set; } = null!;

    public string PNo { get; set; } = null!;

    public string Clientname { get; set; } = null!;

    public string? OldpNo { get; set; }

    public string Fullname { get; set; } = null!;

    public DateTime BDate { get; set; }

    public DateTime? ConsultDate { get; set; }

    public string? ClientId { get; set; }

    public decimal AmountBilled { get; set; }

    public string? AmountBilledInWord { get; set; }

    public decimal AmountPaid { get; set; }

    public string? BillingMonth { get; set; }

    public int? BillingYear { get; set; }

    public string Diagnosis { get; set; } = null!;

    public bool? IsPaid { get; set; }
}
