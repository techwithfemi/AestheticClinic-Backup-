using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhVitalSignsBP
{
    [StringLength(50)]
    public string PNO { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime preDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? preTime { get; set; }

    [StringLength(50)]
    public string? Temp { get; set; }

    [StringLength(50)]
    public string? pressure { get; set; }

    [StringLength(50)]
    public string? examinedBy { get; set; }

    [StringLength(50)]
    public string? pulse { get; set; }

    [StringLength(50)]
    public string? weight { get; set; }

    [StringLength(50)]
    public string? height { get; set; }

    [StringLength(50)]
    public string? RespRatio { get; set; }

    public bool? attendedTo { get; set; }

    [StringLength(355)]
    public string Fullname { get; set; } = null!;

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    public double? pressure1 { get; set; }

    public double? pressure2 { get; set; }
}
