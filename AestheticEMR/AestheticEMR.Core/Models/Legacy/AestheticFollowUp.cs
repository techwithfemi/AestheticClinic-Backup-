using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Table("AestheticFollowUp")]
[Index("ConsultationId", Name = "IX_AestheticFollowUp_ConsultationId")]
public partial class AestheticFollowUp
{
    [Key]
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

    public string? PatientSatisfactionPNo { get; set; }

    public DateTime? PatientSatisfactionSubmittedOn { get; set; }

    [StringLength(40)]
    public string? CreatedBy { get; set; }

    [StringLength(40)]
    public string? UpdatedBy { get; set; }

    public DateTime UpdatedDate { get; set; }

    public DateTime CreatedDate { get; set; }

    [ForeignKey("ConsultationId")]
    [InverseProperty("AestheticFollowUps")]
    public virtual AestheticConsultation Consultation { get; set; } = null!;
}
