using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("DrugReturned")]
public partial class DrugReturned
{
    public long SNO { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime dtDate { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string DrgName { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Qty { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Cost { get; set; }

    [Column(TypeName = "decimal(37, 4)")]
    public decimal? Amount { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? LocID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Remarks { get; set; }

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

    public bool? Reversed { get; set; }

    public long? SuppID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? AcctID { get; set; }

    [StringLength(8000)]
    [Unicode(false)]
    public string? BatchID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? BatchNo { get; set; }
}
