using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhPrescriptionInfoForEye
{
    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime cDate { get; set; }

    [StringLength(50)]
    public string pNo { get; set; } = null!;

    [StringLength(3000)]
    public string? prescription { get; set; }

    [StringLength(50)]
    public string treatedBy { get; set; } = null!;

    [StringLength(3000)]
    public string? Preconsult { get; set; }

    [StringLength(101)]
    public string Fullname { get; set; } = null!;

    [StringLength(3000)]
    public string? complaints { get; set; }

    [StringLength(3000)]
    public string? sysReview { get; set; }

    [StringLength(3000)]
    public string? phyExam { get; set; }

    [StringLength(3000)]
    public string? diagnosis { get; set; }

    [StringLength(3000)]
    public string? diffDiagnosis { get; set; }

    [StringLength(3000)]
    public string? investigate { get; set; }

    [StringLength(3000)]
    public string? referto { get; set; }

    [StringLength(3000)]
    public string? HPC { get; set; }

    [StringLength(3000)]
    public string? PMH { get; set; }

    [StringLength(3000)]
    public string? DrugHx { get; set; }

    public bool? attendedToByPharm { get; set; }

    [StringLength(50)]
    public string clientCat { get; set; } = null!;

    [StringLength(2000)]
    [Unicode(false)]
    public string? injprescription { get; set; }

    public long ID { get; set; }

    [StringLength(3000)]
    [Unicode(false)]
    public string? Remarks { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? cTime { get; set; }

    [StringLength(3000)]
    [Unicode(false)]
    public string? genSys { get; set; }

    [StringLength(3000)]
    [Unicode(false)]
    public string? genPhy { get; set; }

    [StringLength(3000)]
    [Unicode(false)]
    public string? treatPlan { get; set; }

    [StringLength(3000)]
    [Unicode(false)]
    public string? treatdone { get; set; }

    [StringLength(3000)]
    [Unicode(false)]
    public string? dentHist { get; set; }

    [StringLength(3000)]
    [Unicode(false)]
    public string? extraOralExam { get; set; }

    [StringLength(3000)]
    [Unicode(false)]
    public string? intraOralExam { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? clinic { get; set; }

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
    public string? RemarksEye { get; set; }

    [StringLength(50)]
    public string? conID { get; set; }

    [StringLength(1000)]
    public string? Retino { get; set; }

    [StringLength(1000)]
    public string? Refraction { get; set; }

    [StringLength(1000)]
    public string? FSPrescRE { get; set; }

    [StringLength(1000)]
    public string? FSPrescLE { get; set; }

    [StringLength(1000)]
    public string? Tonometry { get; set; }
}
