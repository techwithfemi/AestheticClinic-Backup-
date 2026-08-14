using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("HSERVICENHISCOPY")]
public partial class HSERVICENHISCOPY
{
    [StringLength(255)]
    public string Service { get; set; } = null!;

    [StringLength(255)]
    public string? Category { get; set; }

    [StringLength(255)]
    public string? Company { get; set; }

    public double? Price { get; set; }

    [StringLength(255)]
    public string? Remarks { get; set; }

    public long SNO { get; set; }

    [StringLength(250)]
    public string? CoyName { get; set; }

    [StringLength(50)]
    public string? Capitated { get; set; }

    [StringLength(50)]
    public string? TariffStatus { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? RevType { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? UsersCat { get; set; }
}
