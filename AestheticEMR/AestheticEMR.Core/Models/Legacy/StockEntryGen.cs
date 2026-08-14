using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("StockEntryGen")]
public partial class StockEntryGen
{
    public long EntryID { get; set; }

    public long? ApprvID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime EntryDate { get; set; }

    [StringLength(50)]
    public string ItemID { get; set; } = null!;

    [StringLength(50)]
    public string? Category { get; set; }

    [StringLength(50)]
    public string? BatchNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ExpiryDate { get; set; }

    public int Qty { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Cost { get; set; }

    public int? stockQtyOut { get; set; }

    [StringLength(50)]
    public string? ReceivedBy { get; set; }

    [StringLength(50)]
    public string? POID { get; set; }

    [StringLength(200)]
    public string? Comments { get; set; }

    [StringLength(50)]
    public string? invType { get; set; }

    public bool? AttendedTo { get; set; }

    public bool? Suppres { get; set; }

    [StringLength(250)]
    public string? Supplier { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DateLastPurch { get; set; }

    public double? QtyLastPurch { get; set; }

    public double? UnitPriceLast { get; set; }

    public double? QtyUsed { get; set; }

    public double? QtyInStockAsAtEntry { get; set; }

    public double? UnitPrice { get; set; }

    [StringLength(50)]
    public string? reverseID { get; set; }

    [StringLength(3)]
    public string? reversal { get; set; }
}
