using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("DrugReconcileGen")]
public partial class DrugReconcileGen
{
    public long SNO { get; set; }

    [StringLength(350)]
    public string DrgName { get; set; } = null!;

    [StringLength(150)]
    public string drgcatname { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime RecDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? RecTime { get; set; }

    public double PhyStock { get; set; }

    public double SysStock { get; set; }

    public int Mth { get; set; }

    public int Yr { get; set; }

    [StringLength(50)]
    public string? Remarks { get; set; }
}
