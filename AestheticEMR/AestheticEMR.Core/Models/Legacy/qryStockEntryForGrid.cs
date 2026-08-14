using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryStockEntryForGrid
{
    public long EntryID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EntryDate { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? ItemCode { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string ItemName { get; set; } = null!;

    [StringLength(150)]
    public string? Category { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ExpiryDate { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? UnitPrice { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? QtyEntered { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? QtyInStockAsAtEntry { get; set; }

    [Column(TypeName = "decimal(19, 2)")]
    public decimal? Total { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? PrevQtyUsed { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? UnitsInStore { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DateLastPurch { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? QtyLastPurch { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? UnitPriceLast { get; set; }

    [StringLength(50)]
    public string? BatchNo { get; set; }

    [StringLength(200)]
    public string? Comments { get; set; }

    [StringLength(250)]
    public string? Supplier { get; set; }

    [StringLength(101)]
    public string? ReceivedBy { get; set; }

    [StringLength(50)]
    public string? empID { get; set; }

    [StringLength(50)]
    public string? reverseID { get; set; }

    [StringLength(3)]
    public string? reversal { get; set; }
}
