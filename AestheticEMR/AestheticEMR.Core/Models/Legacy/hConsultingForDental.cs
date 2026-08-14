using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hConsultingForDental")]
public partial class hConsultingForDental
{
    public long ID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime cDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? cTime { get; set; }

    [StringLength(50)]
    public string treatedBy { get; set; } = null!;

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [StringLength(50)]
    public string pNo { get; set; } = null!;

    [StringLength(50)]
    public string clientCat { get; set; } = null!;

    [StringLength(50)]
    public string? WardID { get; set; }

    [StringLength(1000)]
    public string? symptoms { get; set; }

    [StringLength(2000)]
    public string? prescription { get; set; }

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

    [StringLength(1000)]
    public string? HPC { get; set; }

    [StringLength(1000)]
    public string? PMH { get; set; }

    [StringLength(1000)]
    public string? DrugHx { get; set; }

    [StringLength(50)]
    public string? referto { get; set; }

    public bool? attendedToByPharm { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? remarks { get; set; }

    public bool? isAlarm { get; set; }

    public bool? isReview { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? informt { get; set; }

    public bool? attendedTo { get; set; }

    [StringLength(2000)]
    [Unicode(false)]
    public string? injprescription { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? genSys { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? genPhy { get; set; }

    [StringLength(800)]
    [Unicode(false)]
    public string? treatPlan { get; set; }
}
