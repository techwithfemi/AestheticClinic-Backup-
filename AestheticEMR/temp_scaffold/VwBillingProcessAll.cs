using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwBillingProcessAll
{
    public DateTime AttdDate { get; set; }

    public DateTime? BillDate { get; set; }

    public string BillNo { get; set; } = null!;

    public string? RetainCode { get; set; }

    public string CoyName { get; set; } = null!;

    public decimal? AmountBilled { get; set; }

    public decimal? AmountPaid { get; set; }

    public int? YearCode { get; set; }

    public string? ClientCat { get; set; }

    public bool? IsProcess { get; set; }

    public string? InvNo { get; set; }

    public string Fullname { get; set; } = null!;

    public string? Remarks { get; set; }

    public long Id { get; set; }

    public string? Mth { get; set; }

    public string? Yr { get; set; }

    public string? BatchVal { get; set; }

    public string? BatchNo { get; set; }

    public string ClinicType { get; set; } = null!;

    public string? MonthCode { get; set; }

    public string PNo { get; set; } = null!;

    public string? Diagnosis { get; set; }

    public string? RetainId { get; set; }

    public DateTime? AttndBillDate { get; set; }

    public DateTime? AdmDate { get; set; }

    public DateTime? DischDate { get; set; }

    public string? BillingMonth { get; set; }

    public int? BillingYear { get; set; }

    public bool IsSigned { get; set; }

    public DateTime? ExitDate { get; set; }

    public DateTime? Htime { get; set; }

    public string? PhoneNo { get; set; }

    public int? BillEndDate { get; set; }

    public string? Title { get; set; }

    public decimal? Discount { get; set; }

    public decimal? DebtBf { get; set; }
}
