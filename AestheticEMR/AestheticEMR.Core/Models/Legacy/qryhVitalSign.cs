using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhVitalSign
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

    [StringLength(1001)]
    public string Fullname { get; set; } = null!;

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [StringLength(250)]
    [Unicode(false)]
    public string? Remarks { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? AdmDate { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? SPO2 { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? RBS { get; set; }
}
