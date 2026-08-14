using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Index("PatientId", Name = "IX_AestheticConsultations_PatientId")]
public partial class AestheticConsultation
{
    [Key]
    public int Id { get; set; }

    public int PatientId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ConsultationDate { get; set; }

    [Unicode(false)]
    public string ProcedureType { get; set; } = null!;

    [Unicode(false)]
    public string? Provider { get; set; }

    [Required]
    public bool? ConsentGiven { get; set; }

    public bool InformationAccepted { get; set; }

    [Unicode(false)]
    public string? ProcedureDescription { get; set; }

    [Unicode(false)]
    public string? RisksAndComplications { get; set; }

    [Unicode(false)]
    public string? PostTreatmentInstructions { get; set; }

    [Unicode(false)]
    public string? SkinAssessment { get; set; }

    [Unicode(false)]
    public string? TreatmentPlan { get; set; }

    [Unicode(false)]
    public string? CurrentMedications { get; set; }

    [Unicode(false)]
    public string? Allergies { get; set; }

    [Unicode(false)]
    public string? DeviceSettings { get; set; }

    [StringLength(40)]
    public string? CreatedBy { get; set; }

    [StringLength(40)]
    public string? UpdatedBy { get; set; }

    public DateTime UpdatedDate { get; set; }

    public DateTime CreatedDate { get; set; }

    [Unicode(false)]
    public string? AreaTreated { get; set; }

    [Unicode(false)]
    public string? BrandUsed { get; set; }

    public DateTime? ConsentDate { get; set; }

    [Unicode(false)]
    public string? ConsentNotes { get; set; }

    [Unicode(false)]
    public string? CoolingMethod { get; set; }

    [Unicode(false)]
    public string? DeviceUsed { get; set; }

    [Unicode(false)]
    public string? Dilution { get; set; }

    [Unicode(false)]
    public string? Fluence { get; set; }

    [Unicode(false)]
    public string? FollowUpReview { get; set; }

    [Unicode(false)]
    public string? Indication { get; set; }

    [Unicode(false)]
    public string? InjectionMapping { get; set; }

    [Unicode(false)]
    public string? LotNumber { get; set; }

    public DateTime? NextSessionDate { get; set; }

    public int? NumberOfShots { get; set; }

    [Unicode(false)]
    public string? PulseDuration { get; set; }

    [Unicode(false)]
    public string? SkinReaction { get; set; }

    [Unicode(false)]
    public string? SpotSize { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? UnitsUsed { get; set; }

    [Unicode(false)]
    public string? Wavelength { get; set; }

    [Unicode(false)]
    public string? ConsultId { get; set; }

    [Unicode(false)]
    public string? PNo { get; set; }

    [Unicode(false)]
    public string? Services { get; set; }

    [InverseProperty("Consultation")]
    public virtual ICollection<AestheticFollowUp> AestheticFollowUps { get; set; } = new List<AestheticFollowUp>();

    [InverseProperty("Consultation")]
    public virtual ICollection<AestheticPhoto> AestheticPhotos { get; set; } = new List<AestheticPhoto>();

    [InverseProperty("Consultation")]
    public virtual ICollection<AppProcedureProductUsage> AppProcedureProductUsages { get; set; } = new List<AppProcedureProductUsage>();

    [ForeignKey("PatientId")]
    [InverseProperty("AestheticConsultations")]
    public virtual AestheticPatient Patient { get; set; } = null!;
}
