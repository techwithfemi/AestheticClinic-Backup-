using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class TariffSpecial
{
    public long Sno { get; set; }

    public string ItemCode { get; set; } = null!;

    public string ItemName { get; set; } = null!;

    public decimal Price { get; set; }

    public decimal Qty { get; set; }

    public decimal Amount { get; set; }

    public string? Remarks { get; set; }
}
