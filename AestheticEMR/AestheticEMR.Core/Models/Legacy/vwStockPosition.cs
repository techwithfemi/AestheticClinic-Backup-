using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwStockPosition
{
    public long? EntryID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EntryDate { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? ItemID { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? Drug { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Qty { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? PriceLast { get; set; }

    [StringLength(50)]
    public string? ReceivedBy { get; set; }

    [StringLength(200)]
    public string? Comments { get; set; }

    [StringLength(255)]
    public string? Category { get; set; }

    public double? DrugPriceLast { get; set; }

    public bool? Suppres { get; set; }

    public double? BulkUnit { get; set; }

    [StringLength(50)]
    public string? LocID { get; set; }

    [StringLength(50)]
    public string? POID { get; set; }

    [StringLength(50)]
    public string? BatchNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ExpiryDate { get; set; }
}
