using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("StockInDustbin")]
public partial class StockInDustbin
{
    public long SNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EntryDate { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? ItemID { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? UnitsInStock { get; set; }

    [StringLength(50)]
    public string? LocID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ExpiryDate { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? BatchNo { get; set; }

    public bool? Suppres { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? ReceivedBy { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? Remarks { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? PostDate { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? UnitCost { get; set; }

    public bool? isPost { get; set; }

    public bool? reversed { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EntryDate2 { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EntryTime { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? ClientName { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? AppName { get; set; }
}
