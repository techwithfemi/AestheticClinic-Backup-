using AestheticEMR.Core.Models;

namespace AestheticEMR.Core.Models.Aesthetic
{
    public class AestheticConsentTemplate : BaseEntity
    {
        public required string Name { get; set; }
        public required string Title { get; set; }
        public string? ProcedureType { get; set; }
        public required string Content { get; set; }
        public bool IsActive { get; set; } = true;

        public ICollection<AestheticSignedConsent> SignedConsents { get; } = [];
    }
}
