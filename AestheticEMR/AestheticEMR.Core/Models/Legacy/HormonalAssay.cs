using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("HormonalAssay")]
public partial class HormonalAssay
{
    public long ID { get; set; }

    [StringLength(50)]
    public string LABNO { get; set; } = null!;

    [StringLength(50)]
    public string? DESCRIPTION { get; set; }

    [StringLength(50)]
    public string? RESULT { get; set; }

    [StringLength(50)]
    public string? DESC2 { get; set; }

    [StringLength(50)]
    public string? SAMPLE { get; set; }

    [StringLength(50)]
    public string? CLASS { get; set; }

    [StringLength(50)]
    public string? RANGE { get; set; }

    [StringLength(50)]
    public string? REMARKS { get; set; }
}
