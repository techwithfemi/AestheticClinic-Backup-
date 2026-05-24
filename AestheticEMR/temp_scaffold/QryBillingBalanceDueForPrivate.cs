using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryBillingBalanceDueForPrivate
{
    public DateTime Date { get; set; }

    public DateTime BDate { get; set; }

    public DateTime? ConsultDate { get; set; }

    public string ClientName { get; set; } = null!;

    public string BillNo { get; set; } = null!;

    public string PNo { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public decimal AmountBilled { get; set; }

    public decimal? ProfFee { get; set; }

    public decimal? AmtBf { get; set; }

    public decimal AmountPaid { get; set; }

    public string? ClientId { get; set; }

    public string Diagnosis { get; set; } = null!;

    public string? PCatId { get; set; }

    public string? BillType { get; set; }

    public bool? IsPaid { get; set; }

    public string? AmountBilledInWord { get; set; }

    public double Debt { get; set; }

    public decimal? PrevDebt { get; set; }

    public decimal? CurrentDebt { get; set; }

    public int AmountDue { get; set; }
}
