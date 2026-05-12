namespace AestheticEMR.Core.Models.Aesthetic
{
    public class AestheticConsentStatus
    {
        public required string ConsultId { get; set; }
        public required string PNo { get; set; }
        public required string ProcedureType { get; set; }
        public bool AttendanceTaken { get; set; }
        public bool HasValidConsent { get; set; }
        public bool CanSign => AttendanceTaken;
        public AestheticConsentTemplate? ActiveTemplate { get; set; }
        public AestheticSignedConsent? LatestSignedConsent { get; set; }
    }
}
