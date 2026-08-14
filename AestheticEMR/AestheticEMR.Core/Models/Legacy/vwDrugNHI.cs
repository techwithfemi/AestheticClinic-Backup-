using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwDrugNHI
{
    public long SNO { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? Drug { get; set; }

    [StringLength(255)]
    public string? Category { get; set; }

    [StringLength(255)]
    public string? PharmCat { get; set; }

    public double Price { get; set; }

    [StringLength(50)]
    public string CoyID { get; set; } = null!;

    [StringLength(250)]
    public string? Remarks { get; set; }

    [StringLength(150)]
    public string Company { get; set; } = null!;

    [Column("Qty/Unit")]
    [StringLength(43)]
    public string? Qty_Unit { get; set; }

    public double? UnitsInStock { get; set; }

    [StringLength(50)]
    public string? Capitated { get; set; }

    [StringLength(50)]
    public string? TariffStatus { get; set; }

    [StringLength(50)]
    public string? revType { get; set; }

    [StringLength(255)]
    public string? PharmName { get; set; }
}
