using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwhRevenueType
{
    public long Sno { get; set; }

    public string RevType { get; set; } = null!;

    public string Remarks { get; set; } = null!;

    public DateTime? Date { get; set; }

    public string? PatName { get; set; }

    public double? Subtotal { get; set; }

    public string? AccountNo { get; set; }

    public string? Active { get; set; }
}
