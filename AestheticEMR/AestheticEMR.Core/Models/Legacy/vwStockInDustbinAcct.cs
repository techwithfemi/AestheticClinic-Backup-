using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwStockInDustbinAcct
{
    public long SNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EntryDate { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? Stock { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? UnitsInStock { get; set; }

    [StringLength(50)]
    public string? LocID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ExpiryDate { get; set; }

    public bool? Suppres { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? UnitCost { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EntryTime { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? ClientName { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EntryDate2 { get; set; }

    public bool? isPost { get; set; }

    public bool? reversed { get; set; }

    [Column(TypeName = "decimal(37, 4)")]
    public decimal? Amount { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? AppName { get; set; }

    [StringLength(328)]
    [Unicode(false)]
    public string? Remarks { get; set; }
}
