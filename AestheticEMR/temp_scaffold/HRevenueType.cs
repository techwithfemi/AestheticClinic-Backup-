using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HRevenueType
{
    public long Sno { get; set; }

    public string RevType { get; set; } = null!;

    public string CatRemarks { get; set; } = null!;

    public DateTime? RevDateDumm { get; set; }

    public string? PatNameDumm { get; set; }

    public double? Subtotal { get; set; }

    public string? AccountNo { get; set; }

    public string? Active { get; set; }

    public int? Serial { get; set; }
}
