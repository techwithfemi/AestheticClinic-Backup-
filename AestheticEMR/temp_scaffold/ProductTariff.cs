using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class ProductTariff
{
    public long Sno { get; set; }

    public string PdtName { get; set; } = null!;

    public string? Category { get; set; }

    public string? Company { get; set; }

    public decimal? Price { get; set; }

    public string? Remarks { get; set; }

    public string? CoyName { get; set; }

    public string? Capitated { get; set; }

    public string? TariffStatus { get; set; }

    public string? RevType { get; set; }

    public string? UsersCat { get; set; }
}
