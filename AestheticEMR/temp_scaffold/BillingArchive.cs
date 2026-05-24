using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class BillingArchive
{
    public long Id { get; set; }

    public DateTime BDate { get; set; }

    public DateTime? ConsultDate { get; set; }

    public string BillNo { get; set; } = null!;

    public string PNo { get; set; } = null!;

    public string? ClientId { get; set; }

    public decimal AmountBilled { get; set; }

    public decimal ProfFee { get; set; }

    public decimal? AmtBf { get; set; }

    public string? AmountBilledInWord { get; set; }

    public decimal AmountPaid { get; set; }

    public string? BillingMonth { get; set; }

    public int? BillingYear { get; set; }

    public string Diagnosis { get; set; } = null!;

    public bool? IsPaid { get; set; }

    public string? BillType { get; set; }

    public string? BillTo { get; set; }

    public string? CoyName { get; set; }
}
