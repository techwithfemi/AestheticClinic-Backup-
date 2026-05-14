using AestheticEMR.Core.Models;

namespace AestheticEMR.Core.Models.Aesthetic
{
    public class AestheticFollowUp : BaseEntity
    {
        public int ConsultationId { get; set; }
        public required AestheticConsultation Consultation { get; set; }

        public DateTime ScheduledDate { get; set; }
        public bool IsAutoScheduled { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedDate { get; set; }

        public string? Outcome { get; set; }
        public int? PatientSatisfactionScore { get; set; }
        public bool RepeatPhotosTaken { get; set; }
        public string? NextTreatmentRecommendation { get; set; }
        public string? Notes { get; set; }

        public string? PatientSatisfactionConsultId { get; set; }
        public string? PatientSatisfactionPNo { get; set; }
        public DateTime? PatientSatisfactionSubmittedOn { get; set; }
    }
}