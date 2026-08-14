using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwReqAndPurchaseAnalysis
{
    [Column(TypeName = "datetime")]
    public DateTime? OrderDate { get; set; }

    [StringLength(50)]
    public string? OrderNo { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? ItemID { get; set; }

    [StringLength(250)]
    public string? Category { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DateLastPurch { get; set; }

    public double? QtyLastPurch { get; set; }

    public double? UnitPriceLast { get; set; }

    public double? QtyUsed { get; set; }

    public double? QtyInStock { get; set; }

    public double? QtyRqst { get; set; }

    public double? UnitPriceRqst { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? OrderDateApprv { get; set; }

    public double? QtyApprv { get; set; }

    public double? UnitPrice { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EntryDate { get; set; }

    [StringLength(50)]
    public string? SuppID { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? QtyPurch { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? PricePurch { get; set; }

    public double? AmountLastPurch { get; set; }

    public double? AmountRqst { get; set; }

    public double? AmountApprv { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? AmountPurch { get; set; }

    public long? EntryID { get; set; }

    public long? ApprvID { get; set; }

    public long? IDNum { get; set; }
}
