using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwBillingProcess
{
    public DateTime AttdDate { get; set; }

    public DateTime? BillDate { get; set; }

    public string BillNo { get; set; } = null!;

    public string RetainCode { get; set; } = null!;

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

    public string RetainId { get; set; } = null!;

    public DateTime? AttndBillDate { get; set; }
}
