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
        [StringLength(1000)]
        public string? Outcome { get; set; }

        [Range(1, 10)]
        public int? PatientSatisfactionScore { get; set; }

        public bool RepeatPhotosTaken { get; set; }

        [StringLength(1000)]
        public string? NextTreatmentRecommendation { get; set; }

        [StringLength(2000)]
        public string? Notes { get; set; }
    }
}