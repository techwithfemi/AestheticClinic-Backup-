using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhPrescriptionInfo
{
    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime cDate { get; set; }

    [StringLength(50)]
    public string pNO { get; set; } = null!;

    [StringLength(3000)]
    [Unicode(false)]
    public string? prescription { get; set; }

    [StringLength(50)]
    public string treatedBy { get; set; } = null!;

    [StringLength(8000)]
    [Unicode(false)]
    public string? Preconsult { get; set; }

    [StringLength(1001)]
    public string Fullname { get; set; } = null!;

    [StringLength(3000)]
    [Unicode(false)]
    public string? complaints { get; set; }

    [StringLength(3000)]
    [Unicode(false)]
    public string? sysReview { get; set; }

    [StringLength(3000)]
    [Unicode(false)]
    public string? phyExam { get; set; }

    [Unicode(false)]
    public string? diagnosis { get; set; }

    [Unicode(false)]
    public string? diffDiagnosis { get; set; }

    [StringLength(3000)]
    [Unicode(false)]
    public string? investigate { get; set; }

    [StringLength(3000)]
    [Unicode(false)]
    public string? referto { get; set; }

    [StringLength(3000)]
    [Unicode(false)]
    public string? HPC { get; set; }

    [Unicode(false)]
    public string? PMH { get; set; }

    [StringLength(3000)]
    [Unicode(false)]
    public string? DrugHx { get; set; }

    public bool? attendedToByPharm { get; set; }

    [StringLength(50)]
    public string? clientCat { get; set; }

    [StringLength(2000)]
    [Unicode(false)]
    public string? injprescription { get; set; }

    public long ID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ANdate { get; set; }

    [StringLength(50)]
    public string? gestAge { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Fundus { get; set; }

    [StringLength(100)]
    public string? presentation { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? FH { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Oedema { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? HB { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? PCV { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? TT { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? TCA { get; set; }

    [StringLength(5000)]
    [Unicode(false)]
    public string? MO { get; set; }

    [StringLength(3000)]
    [Unicode(false)]
    public string? treatPlan { get; set; }

    [StringLength(3000)]
    [Unicode(false)]
    public string? TreatType { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? clinic { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? informt { get; set; }

    [StringLength(50)]
    public string? LastName { get; set; }

    [StringLength(50)]
    public string? FirstName { get; set; }

    [StringLength(15)]
    [Unicode(false)]
    public string? cTime { get; set; }

    [StringLength(3000)]
    [Unicode(false)]
    public string? services { get; set; }

    [StringLength(3000)]
    [Unicode(false)]
    public string? BillRemarks { get; set; }

    [Unicode(false)]
    public string? ClinicRemarks { get; set; }

    [StringLength(50)]
    public string? UrineAlb { get; set; }

    [StringLength(50)]
    public string? UrineSug { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? BP { get; set; }

    [StringLength(50)]
    public string? wt { get; set; }

    [StringLength(3000)]
    [Unicode(false)]
    public string? remarks { get; set; }

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

    [Unicode(false)]
    public string? MedRpt { get; set; }

    public int? Age { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DOB { get; set; }

    public int? AgeThen { get; set; }

    [StringLength(1000)]
    public string? refReason { get; set; }

    [StringLength(500)]
    public string? Comments { get; set; }

    [StringLength(1573)]
    public string? Referal { get; set; }

    [StringLength(101)]
    public string Doctor { get; set; } = null!;

    [StringLength(8000)]
    [Unicode(false)]
    public string? Treatment { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string Company { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? EntryDate { get; set; }

    [StringLength(36)]
    [Unicode(false)]
    public string? DateAndTime { get; set; }

    [StringLength(4000)]
    public string? RESULT { get; set; }

    [StringLength(50)]
    public string? Sex { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? policyType { get; set; }

    [StringLength(100)]
    public string? empNo { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string? Title { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? occupation { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? bloodGroup { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? genotype { get; set; }

    [StringLength(100)]
    public string? Purpose { get; set; }
}
