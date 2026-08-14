using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhPreconsultChart
{
    public long ID { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    public double? Temp { get; set; }

    public double? pressure1 { get; set; }

    public double? pressure2 { get; set; }

    public double? pulse { get; set; }

    public double? RespRatio { get; set; }

    [StringLength(1001)]
    public string Fullname { get; set; } = null!;

    public double? weight { get; set; }

    public double? height { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime preDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? preTime { get; set; }

    [StringLength(50)]
    public string pNO { get; set; } = null!;
}
