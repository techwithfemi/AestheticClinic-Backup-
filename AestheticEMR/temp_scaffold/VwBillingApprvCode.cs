using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwBillingApprvCode
{
    public DateTime AttdDate { get; set; }

    public DateTime BillDate { get; set; }

    public string BillNo { get; set; } = null!;

    public string? RetainCode { get; set; }

    public string CoyName { get; set; } = null!;

    public decimal AmountBilled { get; set; }

    public decimal AmountPaid { get; set; }

    public string? ApprvCode { get; set; }

    public int? MonthCode { get; set; }

    public int? YearCode { get; set; }

    public string? ClientCat { get; set; }

    public bool? IsProcess { get; set; }

    public string? InvNo { get; set; }

    public string Fullname { get; set; } = null!;

    public string? Remarks { get; set; }

    public long Id { get; set; }
}
