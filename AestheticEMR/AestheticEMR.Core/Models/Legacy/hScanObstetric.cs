using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class hScanObstetric
{
    public long SNO { get; set; }

    public long ConID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? InvDate { get; set; }

    [StringLength(50)]
    public string? LabNo { get; set; }

    [StringLength(50)]
    public string? PNo { get; set; }

    [StringLength(50)]
    public string? EmpID { get; set; }

    [StringLength(50)]
    public string? Uterus { get; set; }

    [StringLength(50)]
    public string? NofOfGest { get; set; }

    [StringLength(50)]
    public string? Presentation { get; set; }

    [StringLength(50)]
    public string? LieAsOfToday { get; set; }

    [StringLength(50)]
    public string? FoetalWB { get; set; }

    [StringLength(50)]
    public string? FoetalCP { get; set; }

    [StringLength(50)]
    public string? FoetalLM { get; set; }

    [StringLength(50)]
    public string? PlacentaLocate { get; set; }

    [StringLength(50)]
    public string? PlacentaMG { get; set; }

    [StringLength(50)]
    public string? PlacentaPT { get; set; }

    [StringLength(50)]
    public string? InternalCOS { get; set; }

    [StringLength(50)]
    public string? AmnioticFC { get; set; }

    [StringLength(50)]
    public string? GrossFAD { get; set; }

    [StringLength(50)]
    public string? AdnexalPD { get; set; }

    [StringLength(50)]
    public string? OtherUterineMD { get; set; }

    [StringLength(50)]
    public string? FoetalPWt { get; set; }

    [StringLength(50)]
    public string? LikelyGender { get; set; }

    [StringLength(500)]
    public string? BPD { get; set; }

    [StringLength(500)]
    public string? FL { get; set; }

    [StringLength(500)]
    public string? AC { get; set; }

    [StringLength(500)]
    public string? EDD { get; set; }

    [StringLength(250)]
    public string? Diagnosis { get; set; }

    [StringLength(500)]
    public string? Comments { get; set; }

    [StringLength(500)]
    public string? EGA { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? ImageID { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? CRL { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? FoetalHeartRate { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? GestationalAge { get; set; }
}
