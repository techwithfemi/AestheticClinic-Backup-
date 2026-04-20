// ---------------------------------------
// Email: quickapp@ebenmonney.com
// Templates: www.ebenmonney.com/templates
// (c) 2024 www.ebenmonney.com/mit-license
// ---------------------------------------

using FluentValidation;

namespace AestheticEMR.Server.ViewModels.Aesthetic
{
    public class AestheticPatientVM
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? SkinType { get; set; }
        public string? Allergies { get; set; }
        public string? MedicalHistory { get; set; }
        public string? CurrentMedications { get; set; }
        public string? Notes { get; set; }

        public ICollection<AestheticConsultationVM>? Consultations { get; set; }
    }

    public class AestheticPatientViewModelValidator : AbstractValidator<AestheticPatientVM>
    {
        public AestheticPatientViewModelValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("Patient first name is required.");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Patient last name is required.");
        }
    }
}
