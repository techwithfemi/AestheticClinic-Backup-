using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwBillingProcessDetailsAll
{
    public DateTime Date { get; set; }

    public string BillNo { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public string? Coyname { get; set; }

    public string Service { get; set; } = null!;

    public double Price { get; set; }

    public double Qty { get; set; }

    public decimal? SubTotal { get; set; }

    public string? BillType { get; set; }

    public string? Capitated { get; set; }

    public bool? IsProcess { get; set; }

    public long Sno { get; set; }

    public string? RptHead { get; set; }

    public string? BillHead { get; set; }

    public string? Mth { get; set; }

    public string? Yr { get; set; }

    public string? BatchVal { get; set; }

    public string? BatchNo { get; set; }

    public string Company { get; set; } = null!;
}
