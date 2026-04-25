using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Legacy;

public partial class hServiceNHI
{
    public string Service { get; set; } = null!;

    public string? Category { get; set; }

    public string? Company { get; set; }

    public double? Price { get; set; }

    public string? Remarks { get; set; }

    public long SNO { get; set; }

    public string? CoyName { get; set; }

    public string? Capitated { get; set; }

    public string? TariffStatus { get; set; }

    public string? RevType { get; set; }

    public string? UsersCat { get; set; }
}
