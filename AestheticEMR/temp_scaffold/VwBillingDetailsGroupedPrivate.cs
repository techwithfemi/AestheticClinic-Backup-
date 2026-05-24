using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwBillingDetailsGroupedPrivate
{
    public string BillNo { get; set; } = null!;

    public string DrgName { get; set; } = null!;

    public string? BillType { get; set; }

    public double? SubTotal { get; set; }

    public double Price { get; set; }

    public double Qty { get; set; }
}
