using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwDrugMaster
{
    public string DrgName { get; set; } = null!;

    public string? PharmName { get; set; }

    public string? DrgCatName { get; set; }

    public string? QtyUnit { get; set; }

    public double? BulkUnit { get; set; }

    public double? PharmUnit { get; set; }

    public double? UnitsInStock { get; set; }

    public double? UnitLevel { get; set; }

    public double? UnitPrice { get; set; }

    public double? ReOrderLevel { get; set; }

    public string? Brand { get; set; }

    public double? Unit2 { get; set; }

    public double? Unit3 { get; set; }

    public string? Capitated { get; set; }

    public string? PharmCat { get; set; }

    public string? RevType { get; set; }

    public long Sno { get; set; }

    public string? DrgCode { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public decimal? LastQty { get; set; }

    public decimal? LastPrice { get; set; }

    public decimal? LastQtyInStock { get; set; }

    public DateTime? LastDatePurch { get; set; }

    public DateTime? EntryDate { get; set; }

    public string? DeptId { get; set; }
}
