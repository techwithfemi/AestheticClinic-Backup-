using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hCapitation")]
public partial class hCapitation
{
    public long SNO { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime dtDate { get; set; }

    public int Yr { get; set; }

    [StringLength(2)]
    [Unicode(false)]
    public string Mth { get; set; } = null!;

    [StringLength(50)]
    public string RetainID { get; set; } = null!;

    public double Amount { get; set; }

    [StringLength(100)]
    public string? Remarks { get; set; }
}
