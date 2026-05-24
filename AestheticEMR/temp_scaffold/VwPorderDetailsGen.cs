using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwPorderDetailsGen
{
    public long Sno { get; set; }

    public string? OrderNo { get; set; }

    public string? Drug { get; set; }

    public string? Category { get; set; }

    public DateTime? LastPurchaseDate { get; set; }

    public double? QtyLastPurchased { get; set; }

    public double? LastUnitPrice { get; set; }

    public double? QtyUsed { get; set; }

    public double? QtyInStock { get; set; }

    public double? QtyNeeded { get; set; }

    public double? UnitPrice { get; set; }

    public double? Amount { get; set; }

    public string IsApprv { get; set; } = null!;
}
