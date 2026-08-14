using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhConsultingEye
{
    public long ID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime cDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? cTime { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [StringLength(50)]
    public string pNo { get; set; } = null!;

    [StringLength(50)]
    public string? WardID { get; set; }

    [StringLength(1000)]
    public string? symptoms { get; set; }

    [StringLength(2000)]
    public string prescription { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? nextApptDate { get; set; }

    [StringLength(1000)]
    public string? Preconsult { get; set; }

    [StringLength(1000)]
    public string? complaints { get; set; }

    [StringLength(1000)]
    public string? sysReview { get; set; }

    [StringLength(1000)]
    public string? phyExam { get; set; }

    [StringLength(1000)]
    public string? diagnosis { get; set; }

    [StringLength(1000)]
    public string? diffDiagnosis { get; set; }

    [StringLength(1000)]
    public string? investigate { get; set; }

    [StringLength(50)]
    public string? referto { get; set; }

    [StringLength(101)]
    public string? Fullname { get; set; }

    [StringLength(101)]
    public string? treatedBy { get; set; }

    [StringLength(2000)]
    public string? result { get; set; }

    public int? Age { get; set; }

    [StringLength(50)]
    public string Sex { get; set; } = null!;

    [StringLength(50)]
    public string? coyNAme { get; set; }

    [StringLength(50)]
    public string? policyType { get; set; }

    [StringLength(154)]
    public string? company { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? genSys { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? genPhy { get; set; }

    [StringLength(800)]
    [Unicode(false)]
    public string? treatPlan { get; set; }

    [StringLength(50)]
    public string? empID { get; set; }

    [StringLength(50)]
    public string? oldpNo { get; set; }

    [StringLength(50)]
    public string? coyType { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? remarks { get; set; }

    [StringLength(2000)]
    [Unicode(false)]
    public string? injprescription { get; set; }

    [StringLength(1000)]
    public string? HPC { get; set; }

    [StringLength(1000)]
    public string? PMH { get; set; }

    [StringLength(1000)]
    public string? DrugHx { get; set; }

    [StringLength(50)]
    public string clientCat { get; set; } = null!;

    [StringLength(200)]
    [Unicode(false)]
    public string? informt { get; set; }

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
    public string? EyeRemarks { get; set; }
}
