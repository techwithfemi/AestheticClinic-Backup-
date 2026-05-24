using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class DrugMaster
{
    public string DrgName { get; set; } = null!;

    public string? PharmName { get; set; }

    public string? DrgCatName { get; set; }

    public string? QtyUnit { get; set; }

    public double? BulkUnit { get; set; }

    public double? PharmUnit { get; set; }

    public double? UnitsInStock { get; set; }

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

    public double? UnitPrice { get; set; }

    public double? ReOrderLevel { get; set; }

    public string? Brand { get; set; }

    public double? Unit2 { get; set; }

    public double? Unit3 { get; set; }

    public string? Capitated { get; set; }

    public string? PharmCat { get; set; }

    public string? RevType { get; set; }

    public string? DrgCode { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public decimal? LastQty { get; set; }

    public decimal? LastPrice { get; set; }

    public decimal? LastQtyInStock { get; set; }

    public DateTime? LastDatePurch { get; set; }

    public DateTime? EntryDate { get; set; }

    public decimal? LastQtyPurch { get; set; }

    public decimal? LastUnitPrice { get; set; }

    public string? LastPoid { get; set; }

    public decimal? PrevLastQtyInStock { get; set; }

    public decimal? QtyPurch { get; set; }

    public string? Poid { get; set; }

    public DateTime? DatePurch { get; set; }

    public decimal? LastQtyIssued { get; set; }

    public DateTime? LastDateIssued { get; set; }

    public decimal? QtyIssued { get; set; }

    public DateTime? DateIssued { get; set; }

    public decimal? PriceMargin { get; set; }

    public decimal? LeadLevel { get; set; }

    public long Sno { get; set; }

    public double? UnitLevel { get; set; }

    public string? Dept { get; set; }

    public string? BarCode { get; set; }
}
