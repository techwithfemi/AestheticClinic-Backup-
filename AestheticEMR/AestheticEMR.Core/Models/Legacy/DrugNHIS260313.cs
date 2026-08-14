using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("DrugNHIS260313")]
public partial class DrugNHIS260313
{
    [StringLength(255)]
    [Unicode(false)]
    public string DrgName { get; set; } = null!;

    [StringLength(255)]
    public string Company { get; set; } = null!;

    [StringLength(255)]
    public string? PharmCat { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? DrgCatName { get; set; }

    public double Price { get; set; }

    [StringLength(250)]
    public string? Remarks { get; set; }

    public long SNO { get; set; }

    [StringLength(255)]
    public string? CoyName { get; set; }

    [StringLength(50)]
    public string? Capitated { get; set; }

    [StringLength(50)]
    public string? TariffStatus { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? DRGCode { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? RevType { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? DrgMaster { get; set; }
}
