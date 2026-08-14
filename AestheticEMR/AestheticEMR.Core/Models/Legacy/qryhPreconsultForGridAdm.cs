using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhPreconsultForGridAdm
{
    public long ID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [StringLength(15)]
    [Unicode(false)]
    public string? Time { get; set; }

    [StringLength(1001)]
    public string Fullname { get; set; } = null!;

    [StringLength(50)]
    public string clinicType { get; set; } = null!;

    [StringLength(50)]
    public string PNO { get; set; } = null!;

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
    public string? RespRatio { get; set; }

    [StringLength(50)]
    public string? clientCat { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [StringLength(250)]
    [Unicode(false)]
    public string? Details { get; set; }

    [StringLength(50)]
    public string? empID { get; set; }

    [StringLength(50)]
    public string? stool { get; set; }

    [StringLength(50)]
    public string? urine { get; set; }

    [StringLength(50)]
    public string? sdrainage { get; set; }

    public bool? attendedTo { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? comment { get; set; }

    [StringLength(101)]
    public string? examinedby { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? UrineAlb { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? UrineSug { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? status { get; set; }

    [StringLength(101)]
    public string? Nurse { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? SPO2 { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? RBS { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Positioning { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? TB { get; set; }
}
