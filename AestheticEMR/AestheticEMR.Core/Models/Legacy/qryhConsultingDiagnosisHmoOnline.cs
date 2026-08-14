using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhConsultingDiagnosisHmoOnline
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

    [StringLength(50)]
    public string? empNo { get; set; }

    [StringLength(150)]
    public string company { get; set; } = null!;

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
    public string? empID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? OldPNo { get; set; }

    [StringLength(3000)]
    [Unicode(false)]
    public string? remarks { get; set; }

    [StringLength(3000)]
    public string? diagnosis { get; set; }

    [StringLength(50)]
    public string? branch { get; set; }

    [StringLength(50)]
    public string? status { get; set; }

    [StringLength(150)]
    public string retainName { get; set; } = null!;

    [StringLength(50)]
    public string PatCat { get; set; } = null!;

    [Column(TypeName = "decimal(18, 0)")]
    public decimal? AmountBilled { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal? AmountPaid { get; set; }
}
