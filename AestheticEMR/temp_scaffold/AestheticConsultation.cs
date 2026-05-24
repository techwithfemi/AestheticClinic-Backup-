using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class AestheticConsultation
{
    public int Id { get; set; }

    public int PatientId { get; set; }

    public DateTime ConsultationDate { get; set; }

    public string ProcedureType { get; set; } = null!;

    public string? Provider { get; set; }

    public bool? ConsentGiven { get; set; }

    public bool InformationAccepted { get; set; }

    public string? ProcedureDescription { get; set; }

    public string? RisksAndComplications { get; set; }

    public string? PostTreatmentInstructions { get; set; }

    public string? SkinAssessment { get; set; }

    public string? TreatmentPlan { get; set; }

    public string? CurrentMedications { get; set; }

    public string? Allergies { get; set; }

    public string? DeviceSettings { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime UpdatedDate { get; set; }

    public DateTime CreatedDate { get; set; }

    public string? AreaTreated { get; set; }

    public string? BrandUsed { get; set; }

    public DateTime? ConsentDate { get; set; }

    public string? ConsentNotes { get; set; }

    public string? CoolingMethod { get; set; }

    public string? DeviceUsed { get; set; }

    public string? Dilution { get; set; }

    public string? Fluence { get; set; }

    public string? FollowUpReview { get; set; }

    public string? Indication { get; set; }

    public string? InjectionMapping { get; set; }

    public string? LotNumber { get; set; }

    public DateTime? NextSessionDate { get; set; }

    public int? NumberOfShots { get; set; }

    public string? PulseDuration { get; set; }

    public string? SkinReaction { get; set; }

    public string? SpotSize { get; set; }

    public decimal? UnitsUsed { get; set; }

    public string? Wavelength { get; set; }

    public virtual ICollection<AestheticFollowUp> AestheticFollowUps { get; set; } = new List<AestheticFollowUp>();

    public virtual ICollection<AestheticPhoto> AestheticPhotos { get; set; } = new List<AestheticPhoto>();

    public virtual ICollection<AppProcedureProductUsage> AppProcedureProductUsages { get; set; } = new List<AppProcedureProductUsage>();

    public virtual AestheticPatient Patient { get; set; } = null!;
}
