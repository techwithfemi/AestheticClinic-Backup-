using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhPreConsultForChart
{
    public long ID { get; set; }

    [StringLength(1001)]
    public string Fullname { get; set; } = null!;

    [StringLength(50)]
    public string pNO { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime preDate { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? Remarks { get; set; }

    [StringLength(50)]
    public string? examinedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? preTime { get; set; }

    [StringLength(50)]
    public string? Temp { get; set; }

    [StringLength(50)]
    public string? pressure { get; set; }

    [StringLength(50)]
    public string? pulse { get; set; }

    [StringLength(50)]
    public string? weight { get; set; }

    [StringLength(50)]
    public string? height { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? UrineAlb { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? UrineSug { get; set; }

    [StringLength(50)]
    public string? RespRatio { get; set; }

    [StringLength(50)]
    public string? clientCat { get; set; }

    public bool? attendedTo { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [StringLength(50)]
    public string? Sex { get; set; }

    public int? Age { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? occupation { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? bloodGroup { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? genotype { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? status { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? Company { get; set; }

    [StringLength(50)]
    public string? stool { get; set; }

    [StringLength(50)]
    public string? urine { get; set; }

    [StringLength(50)]
    public string? sdrainage { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? comment { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Ref { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? SPO2 { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? RBS { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Positioning { get; set; }

    [StringLength(101)]
    public string? Nurse { get; set; }
}
