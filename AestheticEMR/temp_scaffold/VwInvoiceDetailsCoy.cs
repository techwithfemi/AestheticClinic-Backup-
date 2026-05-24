using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwInvoiceDetailsCoy
{
    public DateTime? Expr1 { get; set; }

    public string? BatchNo { get; set; }

    public string Company { get; set; } = null!;

    public string? CoyCode { get; set; }

    public string? ClientCat { get; set; }

    public decimal? AmountBilled { get; set; }

    public string BillNo { get; set; } = null!;

    public string InvNo { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public string DrgName { get; set; } = null!;

    public DateTime? AdmDate { get; set; }

    public DateTime? DischDate { get; set; }

    public int? NoOfDays { get; set; }

    public string? BillHead { get; set; }

    public string? Diagnosis { get; set; }

    public DateTime? BillDate { get; set; }

    public string? Mth { get; set; }

    public string? Yr { get; set; }

    public string? BatchVal { get; set; }

    public string? AcctId { get; set; }

    public bool? IsPost { get; set; }

    public string? BillMonth { get; set; }

    public string? BillYear { get; set; }

    public string RetainName { get; set; } = null!;

    public DateTime? Date { get; set; }
}
