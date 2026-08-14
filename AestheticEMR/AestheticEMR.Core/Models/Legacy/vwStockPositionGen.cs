using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwStockPositionGen
{
    public long EntryID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime EntryDate { get; set; }

    [StringLength(50)]
    public string ItemID { get; set; } = null!;

    [StringLength(50)]
    public string Drug { get; set; } = null!;

    public int Qty { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Cost { get; set; }

    [StringLength(50)]
    public string? ReceivedBy { get; set; }

    [StringLength(200)]
    public string? Comments { get; set; }

    public double? PriceLast { get; set; }

    public bool? Suppres { get; set; }

    public double? QtyUsed { get; set; }

    public double? BulkUnit { get; set; }

    public double? ReOrderLevel { get; set; }

    [Column("Qty/Unit")]
    [StringLength(43)]
    public string? Qty_Unit { get; set; }

    [StringLength(50)]
    public string? BatchNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ExpiryDate { get; set; }

    [StringLength(50)]
    public string? Category { get; set; }

    [StringLength(150)]
    public string DrgCatName { get; set; } = null!;
}
