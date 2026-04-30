// ---------------------------------------
// Email: quickapp@ebenmonney.com
// Templates: www.ebenmonney.com/templates
// (c) 2024 www.ebenmonney.com/mit-license
// ---------------------------------------

using FluentValidation;

namespace AestheticEMR.Server.ViewModels.Aesthetic
{
    public class AestheticConsultationVM
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string? PatientName { get; set; }
        public DateTime ConsultationDate { get; set; }
        public string? ProcedureType { get; set; }
        public string? Provider { get; set; }
        public string? ConsultId { get; set; }
        public string? PNo { get; set; }
        public string? Services { get; set; }
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

        public string? DeviceUsed { get; set; }
        public string? Wavelength { get; set; }
        public string? SpotSize { get; set; }
        public string? Fluence { get; set; }
        public string? PulseDuration { get; set; }
        public string? CoolingMethod { get; set; }
        public int? NumberOfShots { get; set; }
        public string? SkinReaction { get; set; }
        public DateTime? NextSessionDate { get; set; }

        public string? Indication { get; set; }
        public string? BrandUsed { get; set; }
        public string? Dilution { get; set; }
        public decimal? UnitsUsed { get; set; }
        public string? InjectionMapping { get; set; }
        public string? LotNumber { get; set; }
        public string? FollowUpReview { get; set; }

        public ICollection<AestheticPhotoVM>? Photos { get; set; }
    }

    public class AestheticConsultationViewModelValidator : AbstractValidator<AestheticConsultationVM>
    {
        public AestheticConsultationViewModelValidator()
        {
            RuleFor(x => x.PatientId).GreaterThan(0).WithMessage("Consultation must be linked to a patient.");
            RuleFor(x => x.ProcedureType).NotEmpty().WithMessage("Procedure type is required.");
            RuleFor(x => x.ConsultationDate).LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Consultation date cannot be in the future.");
        }
    }
}
