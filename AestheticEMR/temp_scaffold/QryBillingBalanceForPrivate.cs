using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryBillingBalanceForPrivate
{
    public DateTime BDate { get; set; }

    public DateTime? ConsultDate { get; set; }

    public string BillNo { get; set; } = null!;

    public string PNo { get; set; } = null!;

    public string ClientName { get; set; } = null!;

    public decimal AmountBilled { get; set; }

    public decimal? ProfFee { get; set; }

    public decimal? AmtBf { get; set; }

    public decimal AmountPaid { get; set; }

    public string Diagnosis { get; set; } = null!;

    public decimal? CurrentDebt { get; set; }

    public string Fullname { get; set; } = null!;

    public string? ClientId { get; set; }

    public string? PCatId { get; set; }

    public string? BillType { get; set; }

    public bool? IsPaid { get; set; }

    public string? AmountBilledInWord { get; set; }

    public string? EmpNo { get; set; }

    public double Debt { get; set; }
}
