using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwTranxactionArchive
{
    public long SNO { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime AttndDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? BillDate { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? PNo { get; set; }

    [StringLength(50)]
    public string? BillNo { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Bill { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal AmountPaid { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal RunningTotal { get; set; }

    [StringLength(200)]
    public string? Remarks { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string Comapany { get; set; } = null!;

    public int? seed { get; set; }
}
