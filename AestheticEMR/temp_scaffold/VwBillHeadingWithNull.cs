using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwBillHeadingWithNull
{
    public string? CoyName { get; set; }

    public string? BillHead { get; set; }

    public string? BatchNo { get; set; }

    public string Company { get; set; } = null!;

    public DateTime Date { get; set; }

    public string BillNo { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public string Service { get; set; } = null!;

    public double Price { get; set; }

    public double Qty { get; set; }

    public string? BillType { get; set; }

    public int? Sno { get; set; }

    public double SubTotal { get; set; }
}
