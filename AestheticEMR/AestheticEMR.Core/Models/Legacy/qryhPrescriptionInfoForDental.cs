using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhPrescriptionInfoForDental
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
    public DateTime tDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime tTime { get; set; }

    public bool? AULI1 { get; set; }

    public bool? AULI2 { get; set; }

    public bool? AULC { get; set; }

    public bool? AULPM1 { get; set; }

    public bool? AULPM2 { get; set; }

    public bool? AULM1 { get; set; }

    public bool? AULM2 { get; set; }

    public bool? AULM3 { get; set; }

    public bool? AURI1 { get; set; }

    public bool? AURI2 { get; set; }

    public bool? AURC { get; set; }

    public bool? AURPM1 { get; set; }

    public bool? AURPM2 { get; set; }

    public bool? AURM1 { get; set; }

    public bool? AURM2 { get; set; }

    public bool? AURM3 { get; set; }

    public bool? ALLI1 { get; set; }

    public bool? ALLI2 { get; set; }

    public bool? ALLC { get; set; }

    public bool? ALLPM1 { get; set; }

    public bool? ALLPM2 { get; set; }

    public bool? ALLM1 { get; set; }

    public bool? ALLM2 { get; set; }

    public bool? ALLM3 { get; set; }

    public bool? ALRI1 { get; set; }

    public bool? ALRI2 { get; set; }

    public bool? ALRC { get; set; }

    public bool? ALRPM1 { get; set; }

    public bool? ALRPM2 { get; set; }

    public bool? ALRM1 { get; set; }

    public bool? ALRM2 { get; set; }

    public bool? ALRM3 { get; set; }

    public bool? CULI1 { get; set; }

    public bool? CULI2 { get; set; }

    public bool? CULC { get; set; }

    public bool? CULPM1 { get; set; }

    public bool? CULPM2 { get; set; }

    public bool? CURI1 { get; set; }

    public bool? CURI2 { get; set; }

    public bool? CURC { get; set; }

    public bool? CURPM1 { get; set; }

    public bool? CURPM2 { get; set; }

    public bool? CLLI1 { get; set; }

    public bool? CLLI2 { get; set; }

    public bool? CLLC { get; set; }

    public bool? CLLPM1 { get; set; }

    public bool? CLLPM2 { get; set; }

    public bool? CLRI1 { get; set; }

    public bool? CLRI2 { get; set; }

    public bool? CLRC { get; set; }

    public bool? CLRPM1 { get; set; }

    public bool? CLRPM2 { get; set; }

    [StringLength(2000)]
    [Unicode(false)]
    public string? aRem { get; set; }

    [StringLength(2000)]
    [Unicode(false)]
    public string? cRem { get; set; }

    [StringLength(1)]
    public string? DType { get; set; }
}
