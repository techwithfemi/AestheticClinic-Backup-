// ---------------------------------------
// Email: quickapp@ebenmonney.com
// Templates: www.ebenmonney.com/templates
// (c) 2024 www.ebenmonney.com/mit-license
// ---------------------------------------

using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace AestheticEMR.Server.ViewModels.Aesthetic
{
    public class AestheticConsultationVM
    {
        public int Id { get; set; }

        public int PatientId { get; set; }

        public string? PatientName { get; set; }

        [Required]
        public DateTime ConsultationDate { get; set; }

        [Required]
        [StringLength(100)]
        public string? ProcedureType { get; set; }

        // Always set server-side from the logged-in user's EmpId
        [ValidateNever]
        [StringLength(150)]
        public string? Provider { get; set; }

        [StringLength(50)]
        public string? ConsultId { get; set; }

        [Required]
        [StringLength(100)]
        public string? PNo { get; set; }

        [Required]
        [StringLength(2000)]
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
            RuleFor(x => x.PNo).NotEmpty().WithMessage("Patient number (PNo) is required.");
            RuleFor(x => x.ProcedureType).NotEmpty().WithMessage("Procedure type is required.");
            RuleFor(x => x.Services).NotEmpty().WithMessage("Services are required.");
            RuleFor(x => x.ConsultationDate)
                .Must(date => date.Date <= DateTime.Today)
                .WithMessage("Consultation date cannot be in the future.");
        }
    }
}
