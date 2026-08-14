using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwDrugReconcile
{
    public long SNO { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime RecDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? RecTime { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? DrgName { get; set; }

    [Column(TypeName = "decimal(19, 2)")]
    public decimal? QtyDiff { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Cost { get; set; }

    [Column(TypeName = "decimal(38, 4)")]
    public decimal? Amount { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? LocID { get; set; }

    public bool? isPost { get; set; }

    public bool? suppres { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EntryDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EntryTime { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? ClientName { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? AppName { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? PhyStock { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? SysStock { get; set; }

    public int Mth { get; set; }

    public int Yr { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? AcctDebit { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? AcctCredit { get; set; }

    [StringLength(405)]
    [Unicode(false)]
    public string? Remarks { get; set; }
}
