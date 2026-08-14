using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwLabServiceNHI
{
    public long SNO { get; set; }

    [StringLength(255)]
    public string LabItem { get; set; } = null!;

    [StringLength(500)]
    [Unicode(false)]
    public string? Category { get; set; }

    public double? Price { get; set; }

    [StringLength(50)]
    public string CoyID { get; set; } = null!;

    [StringLength(255)]
    public string? Remarks { get; set; }

    [StringLength(255)]
    public string Company { get; set; } = null!;

    [StringLength(50)]
    public string? Capitated { get; set; }

    [StringLength(50)]
    public string? TariffStatus { get; set; }

    [StringLength(50)]
    public string? revType { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Cost { get; set; }

    public long? LabItemSNo { get; set; }
}
