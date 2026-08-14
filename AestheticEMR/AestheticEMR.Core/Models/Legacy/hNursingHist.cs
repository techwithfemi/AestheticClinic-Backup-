using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hNursingHist")]
public partial class hNursingHist
{
    [Column(TypeName = "datetime")]
    public DateTime? dtDate { get; set; }

    [StringLength(50)]
    public string? pNo { get; set; }

    [StringLength(50)]
    public string? consultID { get; set; }

    [StringLength(50)]
    public string? empID { get; set; }

    [StringLength(50)]
    public string? Informnt { get; set; }

    [StringLength(500)]
    public string? Present { get; set; }

    [StringLength(500)]
    public string? Past { get; set; }

    [StringLength(500)]
    public string? Nutrition { get; set; }

    [StringLength(500)]
    public string? Elimin { get; set; }

    [StringLength(500)]
    public string? Exercise { get; set; }

    [StringLength(500)]
    public string? Sleep { get; set; }

    [StringLength(500)]
    public string? Comm { get; set; }

    [StringLength(500)]
    public string? Perception { get; set; }

    [StringLength(500)]
    public string? SocialStat { get; set; }

    [StringLength(500)]
    public string? Sexuality { get; set; }

    [StringLength(500)]
    public string? Stress { get; set; }

    [StringLength(500)]
    public string? Beliefs { get; set; }

    [StringLength(500)]
    public string? Habits { get; set; }

    [StringLength(500)]
    public string? valuables { get; set; }

    [StringLength(500)]
    public string? Urinalysis { get; set; }

    [StringLength(500)]
    public string? genInspec { get; set; }

    [StringLength(50)]
    public string? palpation { get; set; }

    [StringLength(50)]
    public string? percussion { get; set; }

    [StringLength(50)]
    public string? Auscultation { get; set; }

    [StringLength(500)]
    public string? LabResult { get; set; }

    [StringLength(500)]
    public string? NurseDiag { get; set; }

    [StringLength(50)]
    public string? Allergies { get; set; }

    public bool? BCG { get; set; }

    public bool? Polio { get; set; }

    public bool? Tetanus { get; set; }

    public bool? Whooping { get; set; }

    public bool? Dipthe { get; set; }

    public bool? Measles { get; set; }

    [StringLength(150)]
    public string? Others { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LMP { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EDD { get; set; }

    [StringLength(2000)]
    public string? NurAssess { get; set; }

    public long ID { get; set; }

    [StringLength(5000)]
    [Unicode(false)]
    public string? Objectives { get; set; }

    [StringLength(5000)]
    [Unicode(false)]
    public string? NurOrders { get; set; }

    [StringLength(5000)]
    [Unicode(false)]
    public string? Evaluation { get; set; }
}
