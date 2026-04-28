// ---------------------------------------
// Email: quickapp@ebenmonney.com
// Templates: www.ebenmonney.com/templates
// (c) 2024 www.ebenmonney.com/mit-license
// ---------------------------------------

using AestheticEMR.Core.Models;

namespace AestheticEMR.Core.Models.Aesthetic
{
    public class AestheticConsultation : BaseEntity
    {
        public required int PatientId { get; set; }
        public required AestheticPatient Patient { get; set; }
        public DateTime ConsultationDate { get; set; }
        public string ProcedureType { get; set; } = "Aesthetics";
        public string? Provider { get; set; }
        public bool ConsentGiven { get; set; }
        public bool InformationAccepted { get; set; }
        public DateTime? ConsentDate { get; set; }
        public string? ConsentNotes { get; set; }
        public string? ProcedureDescription { get; set; }
        public string? RisksAndComplications { get; set; }
        public string? PostTreatmentInstructions { get; set; }
        public string? SkinAssessment { get; set; }
        public string? TreatmentPlan { get; set; }
        public string? CurrentMedications { get; set; }
        public string? Allergies { get; set; }
        public string? DeviceSettings { get; set; }

        public string? AreaTreated { get; set; }

        // Laser session details
        public string? DeviceUsed { get; set; }
        public string? Wavelength { get; set; }
        public string? SpotSize { get; set; }
        public string? Fluence { get; set; }
        public string? PulseDuration { get; set; }
        public string? CoolingMethod { get; set; }
        public int? NumberOfShots { get; set; }
        public string? SkinReaction { get; set; }
        public DateTime? NextSessionDate { get; set; }

        // Botox session details
        public string? Indication { get; set; }
        public string? BrandUsed { get; set; }
        public string? Dilution { get; set; }
        public decimal? UnitsUsed { get; set; }
        public string? InjectionMapping { get; set; }
        public string? LotNumber { get; set; }
        public string? FollowUpReview { get; set; }

        public ICollection<AestheticPhoto> Photos { get; } = [];
    }
}
