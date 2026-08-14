using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hPreConsultChart")]
public partial class hPreConsultChart
{
    public long ID { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime preDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? preTime { get; set; }

    public double? Temp { get; set; }

    public double? pressure1 { get; set; }

    public double? pressure2 { get; set; }

    public double? pulse { get; set; }

    public double? weight { get; set; }

    public double? height { get; set; }

    public double? RespRatio { get; set; }
}
