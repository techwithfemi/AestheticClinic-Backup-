using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class Drug
{
    public string DrgName { get; set; } = null!;

    public string LocId { get; set; } = null!;

    public string? PharmName { get; set; }

    public string? DrgCatName { get; set; }

    public string? QtyUnit { get; set; }

    public decimal? BulkUnit { get; set; }

    public decimal? PharmUnit { get; set; }

    public decimal? UnitsInStock { get; set; }

    public decimal? UnitPrice { get; set; }

    public decimal? ReOrderLevel { get; set; }

    public string? Brand { get; set; }

    public int? Unit2 { get; set; }

    public int? Unit3 { get; set; }

    public string? Capitated { get; set; }

    public string? PharmCat { get; set; }

    public string? StdPresc { get; set; }

    public int? StdQty { get; set; }

    public double? Private { get; set; }

    public double? Nhis { get; set; }

    public double? Hmo { get; set; }

    /// <summary>
    /// 0
    /// </summary>
    public double? Gene { get; set; }

    /// <summary>
    /// 0
    /// </summary>
    public double? _3mthly { get; set; }

    /// <summary>
    /// 0
    /// </summary>
    public double? _6mthly { get; set; }

    /// <summary>
    /// 0
    /// </summary>
    public double? Cbn { get; set; }

    /// <summary>
    /// 0
    /// </summary>
    public double? Nepa { get; set; }

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

    public decimal? PcentMargin { get; set; }

    public decimal? SellingPrice { get; set; }

    public decimal? PriceMargin { get; set; }

    public decimal? LeadLevel { get; set; }

    public decimal? BulkUnit2 { get; set; }

    public decimal? QtyUsed { get; set; }

    public int? UnitLevel { get; set; }

    public decimal? BulkCost { get; set; }

    public decimal? Amount { get; set; }
}
