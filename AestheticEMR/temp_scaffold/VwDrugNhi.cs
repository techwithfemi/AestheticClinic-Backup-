using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwDrugNhi
{
    public long Sno { get; set; }

    public string? Drug { get; set; }

    public string? Category { get; set; }

    public string? PharmCat { get; set; }

    public double Price { get; set; }

    public string CoyId { get; set; } = null!;

    public string? Remarks { get; set; }

    public string Company { get; set; } = null!;

    public string? QtyUnit { get; set; }

    public double? UnitsInStock { get; set; }

    public string? Capitated { get; set; }

    public string? TariffStatus { get; set; }

    public string? RevType { get; set; }

    public string? PharmName { get; set; }
}
