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
        public string? ProcedureDescription { get; set; }
        public string? RisksAndComplications { get; set; }
        public string? PostTreatmentInstructions { get; set; }
        public string? SkinAssessment { get; set; }
        public string? TreatmentPlan { get; set; }
        public string? CurrentMedications { get; set; }
        public string? Allergies { get; set; }
        public string? DeviceSettings { get; set; }

        public ICollection<AestheticPhoto> Photos { get; } = [];
    }
}
