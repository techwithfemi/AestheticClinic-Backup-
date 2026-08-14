using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hTreatEye")]
public partial class hTreatEye
{
    public long ID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string pno { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string consultID { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime TDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime TTime { get; set; }

    [StringLength(1000)]
    public string? VisualAcuity { get; set; }

    [StringLength(1000)]
    public string? Aided { get; set; }

    [StringLength(1000)]
    public string? PrevSpecRX { get; set; }

    [StringLength(1000)]
    public string? SubjectiveRefraction { get; set; }

    [StringLength(1000)]
    public string? ExtExamOD { get; set; }

    [StringLength(1000)]
    public string? ExtExamOS { get; set; }

    [StringLength(1000)]
    public string? IntExamOD { get; set; }

    [StringLength(1000)]
    public string? IntExamOS { get; set; }

    [StringLength(1000)]
    public string? Remarks { get; set; }

    [StringLength(50)]
    public string? conID { get; set; }

    [StringLength(1000)]
    public string? Retino { get; set; }

    [StringLength(1000)]
    public string? Refraction { get; set; }

    [StringLength(1000)]
    public string? Ophthal { get; set; }

    [StringLength(1000)]
    public string? FSPrescRE { get; set; }

    [StringLength(1000)]
    public string? FSPrescLE { get; set; }

    [StringLength(1000)]
    public string? Tonometry { get; set; }

    [StringLength(4000)]
    [Unicode(false)]
    public string? RemarksEye { get; set; }
}
