using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwLabServiceNhi
{
    public long Sno { get; set; }

    public string LabItem { get; set; } = null!;

    public string? Category { get; set; }

    public double? Price { get; set; }

    public string CoyId { get; set; } = null!;

    public string? Remarks { get; set; }

    public string Company { get; set; } = null!;

    public string? Capitated { get; set; }

    public string? TariffStatus { get; set; }

    public string? RevType { get; set; }

    public decimal Cost { get; set; }

    public long? LabItemSno { get; set; }
}
