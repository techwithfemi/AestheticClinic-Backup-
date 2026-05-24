using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class SuppliersArchive
{
    public long Id { get; set; }

    public string SupplierId { get; set; } = null!;

    public string SupplierName { get; set; } = null!;

    public string? ContactName { get; set; }

    public string? ContactTitle { get; set; }

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Category { get; set; }

    public double? Credit { get; set; }
}
