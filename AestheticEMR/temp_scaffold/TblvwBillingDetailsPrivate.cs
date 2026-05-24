using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class TblvwBillingDetailsPrivate
{
    public string BillNo { get; set; } = null!;

    public string DrgName { get; set; } = null!;

    public string? BillType { get; set; }

    public double Qty { get; set; }

    public double SubTotal { get; set; }

    public double Price { get; set; }

    public string? BillTo { get; set; }

    public string? CoyName { get; set; }
}
