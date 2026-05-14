using System.ComponentModel.DataAnnotations;

namespace AestheticEMR.Server.ViewModels.Aesthetic
{
    public class AestheticFollowUpVM
    {
        public int Id { get; set; }
        public int ConsultationId { get; set; }
        public int PatientId { get; set; }
        public string? PatientName { get; set; }
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

    public class ScheduleAestheticFollowUpVM
    {
        [Range(1, int.MaxValue)]
        public int ConsultationId { get; set; }

        [Range(1, 365)]
        public int DaysAhead { get; set; } = 14;

        [StringLength(2000)]
        public string? Notes { get; set; }
    }

    public class CompleteAestheticFollowUpVM
    {
        [Required]
        [StringLength(1000)]
        public string? Outcome { get; set; }

        [Required]
        [Range(1, 10)]
        public int? PatientSatisfactionScore { get; set; }

        public bool RepeatPhotosTaken { get; set; }

        [Required]
        [StringLength(1000)]
        public string? NextTreatmentRecommendation { get; set; }

        [StringLength(2000)]
        public string? Notes { get; set; }
    }

    public class SendPatientSatisfactionRequestVM
    {
        [Required]
        [EmailAddress]
        [StringLength(256)]
        public string? RecipientEmail { get; set; }

        [StringLength(150)]
        public string? RecipientName { get; set; }
    }

    public class PublicPatientSatisfactionSurveyVM
    {
        public int FollowUpId { get; set; }
        public int ConsultationId { get; set; }
        public string? ConsultId { get; set; }
        public string? PNo { get; set; }
        public string? PatientName { get; set; }
        public DateTime? ScheduledDate { get; set; }
    }

    public class SubmitPatientSatisfactionVM
    {
        [Required]
        [Range(1, 10)]
        public int? PatientSatisfactionScore { get; set; }

        [StringLength(1000)]
        public string? Outcome { get; set; }
    }
}