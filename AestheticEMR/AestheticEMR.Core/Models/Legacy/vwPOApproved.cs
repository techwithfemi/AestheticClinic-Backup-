using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwPOApproved
{
    public long SNO { get; set; }

    public long ID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? OrderDate { get; set; }

    [StringLength(50)]
    public string OrderNo { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string? Drug { get; set; }

    [StringLength(550)]
    public string? Category { get; set; }

    public double? QtyInStock { get; set; }

    [Column(TypeName = "decimal(38, 2)")]
    public decimal Qty { get; set; }

    public double? QtyApprv { get; set; }

    public double? UnitPrice { get; set; }

    public double? Amount { get; set; }

    public long? ApprvID { get; set; }

    public long SnoPO { get; set; }

    public bool? AttendedTo { get; set; }

    [StringLength(5)]
    [Unicode(false)]
    public string SupplierName { get; set; } = null!;

    [StringLength(50)]
    public string POID { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string? ItemName { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string Address { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? ExpiryDate { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? LastQty { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? LastPrice { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? LastQtyInStock { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LastDatePurch { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EntryDate { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? LastQtyPurch { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? LastUnitPrice { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? LastPOID { get; set; }

    [Column(TypeName = "decimal(19, 2)")]
    public decimal? QtyUsed { get; set; }

    [StringLength(50)]
    public string? LocID { get; set; }
}
