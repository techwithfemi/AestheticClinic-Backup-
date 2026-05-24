using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class Billing
{
    public long Id { get; set; }

    public DateOnly BDate { get; set; }

    public string BillNo { get; set; } = null!;

    public string PNo { get; set; } = null!;

    public string? ClientId { get; set; }

    public decimal? AmountBilled { get; set; }

    public string? AmountBilledInWord { get; set; }

    public decimal? AmountPaid { get; set; }

    public string? BillingMonth { get; set; }

    public int? BillingYear { get; set; }

    public bool? IsPaid { get; set; }

    public string? BillType { get; set; }

    public bool? IsProcess { get; set; }

    public DateTime? AdmDate { get; set; }

    public DateTime? DischDate { get; set; }

    public DateTime? TimeVal { get; set; }

    public decimal? Discount { get; set; }

    public decimal? DebtBf { get; set; }

    public string? ApprvCode { get; set; }

    public bool? IsPost { get; set; }

    public string? InvNo { get; set; }

    public string? Diagnosis { get; set; }

    public bool? IsSigned { get; set; }
}
