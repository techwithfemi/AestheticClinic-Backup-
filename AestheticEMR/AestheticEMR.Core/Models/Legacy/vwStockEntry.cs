using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwStockEntry
{
    [Column(TypeName = "datetime")]
    public DateTime? EntryDate { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? ItemID { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? ItemName { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Qty { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Cost { get; set; }

    [StringLength(200)]
    public string? Comments { get; set; }

    [StringLength(101)]
    public string? ReceivedBy { get; set; }

    [StringLength(150)]
    public string? Category { get; set; }

    [StringLength(50)]
    public string? LocID { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? PrevBal { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? PrevPOID { get; set; }

    [StringLength(43)]
    public string? QtyPerUnit { get; set; }

    [StringLength(50)]
    public string? SuppID { get; set; }

    [StringLength(50)]
    public string? BatchNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ExpiryDate { get; set; }

    [StringLength(50)]
    public string? POID { get; set; }

    [StringLength(250)]
    public string? Supplier { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DateLastPurch { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? QtyLastPurch { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? UnitPriceLast { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? QtyUsed { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? QtyInStockAsAtEntry { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? UnitPrice { get; set; }

    public long EntryID { get; set; }

    public long? ApprvID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? InvoiceNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime EntryDateAcct { get; set; }

    [StringLength(3)]
    public string? reversal { get; set; }
}
