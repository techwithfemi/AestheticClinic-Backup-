using AestheticEMR.Core.Models;

namespace AestheticEMR.Core.Models.Aesthetic
{
    public class AestheticSignedConsent : BaseEntity
    {
        public int? PatientId { get; set; }
        public AestheticPatient? Patient { get; set; }
        public required int ConsentTemplateId { get; set; }
        public required AestheticConsentTemplate ConsentTemplate { get; set; }
        public required string ConsultId { get; set; }
        public required string PNo { get; set; }
        public required string ProcedureType { get; set; }
        public DateTime SignedDate { get; set; }
        public string? SignedBy { get; set; }
        public string? WitnessedBy { get; set; }
        public string? SignatureName { get; set; }
        public string? Notes { get; set; }
        public required string ConsentContent { get; set; }
        public byte[]? SignatureImage { get; set; }
        public string? SignatureImagePath { get; set; }
        public string? DoctorViewedBy { get; set; }
        public DateTime? DoctorViewedDate { get; set; }
        public bool IsVoided { get; set; }
        public string? VoidReason { get; set; }
    }
}
