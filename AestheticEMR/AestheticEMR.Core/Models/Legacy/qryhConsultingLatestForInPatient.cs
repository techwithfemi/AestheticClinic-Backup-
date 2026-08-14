using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhConsultingLatestForInPatient
{
    public long ID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime cDate { get; set; }

    [StringLength(301)]
    public string Fullname { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? cTime { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [StringLength(50)]
    public string pNo { get; set; } = null!;

    [StringLength(50)]
    public string? WardID { get; set; }

    [StringLength(3000)]
    public string? symptoms { get; set; }

    [StringLength(3000)]
    public string prescription { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? nextApptDate { get; set; }

    [StringLength(3000)]
    public string? Preconsult { get; set; }

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
    public string? referto { get; set; }

    [StringLength(101)]
    public string? treatedBy { get; set; }

    public string? result { get; set; }

    [StringLength(50)]
    public string? RetainID { get; set; }

    [StringLength(50)]
    public string? policyType { get; set; }

    [StringLength(3000)]
    [Unicode(false)]
    public string? genSys { get; set; }

    [StringLength(3000)]
    [Unicode(false)]
    public string? genPhy { get; set; }

    [StringLength(3000)]
    [Unicode(false)]
    public string? treatPlan { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? oldpNo { get; set; }

    [StringLength(3000)]
    [Unicode(false)]
    public string? remarks { get; set; }

    [StringLength(2000)]
    [Unicode(false)]
    public string? injprescription { get; set; }

    [StringLength(3000)]
    public string? HPC { get; set; }

    [StringLength(3000)]
    public string? PMH { get; set; }

    [StringLength(3000)]
    public string? DrugHx { get; set; }

    [StringLength(50)]
    public string clientCat { get; set; } = null!;

    [StringLength(200)]
    [Unicode(false)]
    public string? informt { get; set; }

    [StringLength(3000)]
    public string? services { get; set; }

    public string? investigate { get; set; }

    [StringLength(50)]
    public string? coyType { get; set; }

    [StringLength(50)]
    public string? Sex { get; set; }

    [StringLength(50)]
    public string? EmpID { get; set; }

    [StringLength(50)]
    public string? Company { get; set; }

    public int? Age { get; set; }

    [StringLength(4000)]
    public string? Treatment { get; set; }

    [StringLength(50)]
    public string? empNo { get; set; }

    [StringLength(150)]
    public string? Client { get; set; }

    public string MedRpt { get; set; } = null!;

    public string findings { get; set; } = null!;

    public string prosedure { get; set; } = null!;

    [StringLength(150)]
    public string coyNAme { get; set; } = null!;

    [StringLength(101)]
    public string? PatientCat { get; set; }

    public string? Expr1 { get; set; }

    public bool isLatest { get; set; }
}
