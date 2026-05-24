using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class Vwsupplier
{
    public long SupplierId { get; set; }

    public string SupplierName { get; set; } = null!;

    public string? ContactName { get; set; }

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string CatCode { get; set; } = null!;

    public double? Credit { get; set; }

    public string? AcctId { get; set; }

    public string? ContactTitle { get; set; }

    public string Category { get; set; } = null!;
}
