using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwTariffAdjust
{
    public long Sno { get; set; }

    public string ItemName { get; set; } = null!;

    public decimal OldPrice { get; set; }

    public decimal NewPrice { get; set; }

    public string RevType { get; set; } = null!;

    public string Company { get; set; } = null!;

    public string CoyName { get; set; } = null!;

    public string BillNo { get; set; } = null!;

    public string? Category { get; set; }
}
