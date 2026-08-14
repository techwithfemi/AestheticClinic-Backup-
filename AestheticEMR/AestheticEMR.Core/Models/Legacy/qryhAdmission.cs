using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhAdmission
{
    public long SNO { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime AdmDate { get; set; }

    [StringLength(1001)]
    public string Fullname { get; set; } = null!;

    [StringLength(550)]
    [Unicode(false)]
    public string? WardID { get; set; }

    [StringLength(5500)]
    [Unicode(false)]
    public string? Reason { get; set; }

    [StringLength(5500)]
    [Unicode(false)]
    public string? Remarks { get; set; }

    [StringLength(50)]
    public string pNO { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? aTime { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [StringLength(50)]
    public string? clientCat { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Coyname { get; set; }

    public bool? isDischarged { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DOB { get; set; }

    public int? Age { get; set; }

    [StringLength(50)]
    public string? Sex { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? policyType { get; set; }

    [StringLength(100)]
    public string? empNo { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string Company { get; set; } = null!;

    [StringLength(50)]
    public string retainID { get; set; } = null!;

    [StringLength(150)]
    [Unicode(false)]
    public string? Title { get; set; }

    [StringLength(50)]
    public string? AdmitBy { get; set; }

    [StringLength(101)]
    public string? AdmitedBy { get; set; }

    [StringLength(101)]
    public string? DoctorName { get; set; }

    public int? NoOfDays { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? AdmitingDoc { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DischDate { get; set; }

    [StringLength(8000)]
    [Unicode(false)]
    public string? Diagnosis { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? DocInCharge { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ExtendAdmissionLimitTo { get; set; }

    public int? NoOfDaysAdmission { get; set; }

    [StringLength(8000)]
    [Unicode(false)]
    public string? Comment { get; set; }
}
