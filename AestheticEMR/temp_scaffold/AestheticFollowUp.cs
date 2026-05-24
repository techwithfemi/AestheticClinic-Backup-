using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class AestheticFollowUp
{
    public int Id { get; set; }

    public int ConsultationId { get; set; }

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

    public string? PatientSatisfactionPno { get; set; }

    public DateTime? PatientSatisfactionSubmittedOn { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime UpdatedDate { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual AestheticConsultation Consultation { get; set; } = null!;
}
