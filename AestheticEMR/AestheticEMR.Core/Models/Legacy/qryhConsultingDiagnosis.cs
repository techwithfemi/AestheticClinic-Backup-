using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhConsultingDiagnosis
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

    [StringLength(3000)]
    public string? symptoms { get; set; }

    [StringLength(4000)]
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
    public string? investigate { get; set; }

    [StringLength(3000)]
    public string? referto { get; set; }

    [StringLength(301)]
    public string Fullname { get; set; } = null!;

    [StringLength(101)]
    public string? treatedBy { get; set; }

    public string? result { get; set; }

    public int? Age { get; set; }

    [StringLength(50)]
    public string? Sex { get; set; }

    [StringLength(50)]
    public string? coyNAme { get; set; }

    [StringLength(50)]
    public string? policyType { get; set; }

    [StringLength(254)]
    public string? company { get; set; }

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
    public string? EmpID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? OldPNo { get; set; }

    [StringLength(3000)]
    [Unicode(false)]
    public string? remarks { get; set; }

    [StringLength(1000)]
    [Unicode(false)]
    public string diagnosis { get; set; } = null!;

    [StringLength(50)]
    public string? empNo { get; set; }
}
