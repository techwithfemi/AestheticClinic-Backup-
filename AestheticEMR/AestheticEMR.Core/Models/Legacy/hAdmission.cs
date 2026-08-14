using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Table("hAdmission")]
public partial class hAdmission
{
    public long SNO { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime AdmDate { get; set; }

    [StringLength(50)]
    public string pNo { get; set; } = null!;

    [StringLength(550)]
    [Unicode(false)]
    public string? WardID { get; set; }

    [StringLength(5500)]
    [Unicode(false)]
    public string? Reason { get; set; }

    [StringLength(5500)]
    [Unicode(false)]
    public string? Remarks { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? aTime { get; set; }

    [Key]
    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [StringLength(50)]
    public string clientCat { get; set; } = null!;

    public bool? isDischarged { get; set; }

    [StringLength(50)]
    public string? AdmitBy { get; set; }

    public bool? isDischargedByDoc { get; set; }

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

    public long? ConID { get; set; }

    public int? NoOfDaysAdmission { get; set; }

    public int? AdmissionDaysAllowed { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ExtendAdmissionLimitTo { get; set; }

    [StringLength(8000)]
    [Unicode(false)]
    public string? Comment { get; set; }
}
