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
        public bool ConsentGiven { get; set; }
        public bool InformationAccepted { get; set; }
        public string? ProcedureDescription { get; set; }
        public string? RisksAndComplications { get; set; }
        public string? PostTreatmentInstructions { get; set; }
        public string? SkinAssessment { get; set; }
        public string? TreatmentPlan { get; set; }
        public string? CurrentMedications { get; set; }
        public string? Allergies { get; set; }
        public string? DeviceSettings { get; set; }

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
