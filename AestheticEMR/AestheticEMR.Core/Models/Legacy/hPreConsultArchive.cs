using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hPreConsultArchive")]
public partial class hPreConsultArchive
{
    public int ID { get; set; }

    [StringLength(50)]
    public string PNO { get; set; } = null!;

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime preDate { get; set; }

    [StringLength(250)]
    public string? Remarks { get; set; }

    [StringLength(50)]
    public string examinedBy { get; set; } = null!;

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
    public string clientCat { get; set; } = null!;

    public bool? attendedTo { get; set; }

    [StringLength(50)]
    public string? stool { get; set; }

    [StringLength(50)]
    public string? urine { get; set; }

    [StringLength(50)]
    public string? sdrainage { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? status { get; set; }

    [StringLength(250)]
    public string? comment { get; set; }
}
