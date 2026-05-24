using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class BillingDetailsArchive
{
    public string BillNo { get; set; } = null!;

    public int? Sno { get; set; }

    public DateTime DtDate { get; set; }

    public string DrgName { get; set; } = null!;

    public double Price { get; set; }

    public double Qty { get; set; }

    public double SubTotal { get; set; }

    public string? BillType { get; set; }

    public string? ConId { get; set; }

    public string? Capitated { get; set; }

    public string? Category { get; set; }

    public string? BillTo { get; set; }

    public string? CoyName { get; set; }

    public string? BillBy { get; set; }
}
