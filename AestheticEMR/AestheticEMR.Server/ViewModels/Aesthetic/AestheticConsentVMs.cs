using System.ComponentModel.DataAnnotations;

namespace AestheticEMR.Server.ViewModels.Aesthetic
{
    public class AestheticConsentTemplateVM
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string? Name { get; set; }

        [Required]
        [StringLength(200)]
        public string? Title { get; set; }

        [StringLength(100)]
        public string? ProcedureType { get; set; }

        [Required]
        public string? Content { get; set; }

        public bool IsActive { get; set; }
    }

    public class AestheticSignedConsentVM
    {
        public int Id { get; set; }
        public int? PatientId { get; set; }
        public int ConsentTemplateId { get; set; }
        public string? ConsultId { get; set; }
        public string? PNo { get; set; }
        public string? ProcedureType { get; set; }
        public DateTime SignedDate { get; set; }
        public string? SignedBy { get; set; }
        public string? WitnessedBy { get; set; }
        public string? SignatureName { get; set; }
        public string? Notes { get; set; }
        public string? ConsentContent { get; set; }
        public string? SignatureImageBase64 { get; set; }
        public string? SignatureImagePath { get; set; }
        public string? DoctorViewedBy { get; set; }
        public DateTime? DoctorViewedDate { get; set; }
        public bool IsVoided { get; set; }
        public string? VoidReason { get; set; }
    }

    public class AestheticConsentStatusVM
    {
        public string? ConsultId { get; set; }
        public string? PNo { get; set; }
        public string? ProcedureType { get; set; }
        public bool AttendanceTaken { get; set; }
        public bool HasValidConsent { get; set; }
        public bool CanSign { get; set; }
        public AestheticConsentTemplateVM? ActiveTemplate { get; set; }
        public AestheticSignedConsentVM? LatestSignedConsent { get; set; }
    }

    public class SignAestheticConsentVM
    {
        public int? PatientId { get; set; }

        [Required]
        [StringLength(50)]
        public string ConsultId { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string PNo { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string ProcedureType { get; set; } = string.Empty;

        [Required]
        public int ConsentTemplateId { get; set; }

        [Required]
        [StringLength(150)]
        public string SignatureName { get; set; } = string.Empty;

        [StringLength(150)]
        public string? WitnessedBy { get; set; }

        [StringLength(150)]
        public string? SignedBy { get; set; }

        public string? Notes { get; set; }
        public string? SignatureImageBase64 { get; set; }
    }

    public class VoidAestheticConsentVM
    {
        [Required]
        [StringLength(500)]
        public string VoidReason { get; set; } = string.Empty;
    }

    public class UpdateAestheticConsentVM
    {
        public int? PatientId { get; set; }

        [Required]
        [StringLength(150)]
        public string SignatureName { get; set; } = string.Empty;

        [StringLength(150)]
        public string? WitnessedBy { get; set; }

        public string? Notes { get; set; }

        public string? SignatureImageBase64 { get; set; }

        public int? ConsentTemplateId { get; set; }
    }
}
