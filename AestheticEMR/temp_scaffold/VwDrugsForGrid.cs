using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwDrugsForGrid
{
    public string Drug { get; set; } = null!;

    public string? Category { get; set; }

    public decimal? UnitsInStock { get; set; }

    public string? StdPresc { get; set; }

    public double? StdQty { get; set; }

    public double? Private { get; set; }

    public double? Nhis { get; set; }

    public double? Hmo { get; set; }

    public double? Gene { get; set; }

    public double? _3mthly { get; set; }

    public double? _6mthly { get; set; }

    public double? Cbn { get; set; }

    public double? Nepa { get; set; }

    public decimal? UnitPrice { get; set; }

    public decimal? ReOrderLevel { get; set; }

    public decimal? BulkUnit { get; set; }

    public int? UnitLevel { get; set; }

    public int? Unit2 { get; set; }

    public int? Unit3 { get; set; }

    public string? Capitated { get; set; }

    public string? Remarks { get; set; }

    public string? PharmName { get; set; }

    public string? Location { get; set; }

    public string? DrgCode { get; set; }

    public string? LocId { get; set; }

    public string? RevType { get; set; }

    public DateTime? ExpiredDate { get; set; }

    public string? QtyUnit { get; set; }

    public string? DeptId2 { get; set; }

    public decimal? PcentMargin { get; set; }

    public decimal? SellingPrice { get; set; }

    public string? Brand { get; set; }

    public string? StockDept { get; set; }

    public string? BarCode { get; set; }

    public string? DeptId { get; set; }
}
