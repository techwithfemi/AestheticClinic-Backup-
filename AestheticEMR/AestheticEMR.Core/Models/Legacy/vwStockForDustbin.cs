using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwStockForDustbin
{
    public long SNo { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? StockItem { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Qty { get; set; }

    [StringLength(50)]
    public string? LocID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ExpiryDate { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? BatchNo { get; set; }

    public bool? Suppres { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string ReceivedBy { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? Remarks { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EntryDate { get; set; }

    [StringLength(150)]
    public string? Location { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? UnitCost { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? PostDate { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? DeptID { get; set; }
}
