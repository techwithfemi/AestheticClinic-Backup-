using AestheticEMR.Core.Models.Aesthetic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Index("ConsentTemplateId", Name = "IX_AppAestheticSignedConsents_ConsentTemplateId")]
[Index("ConsultId", "ProcedureType", "IsVoided", Name = "IX_AppAestheticSignedConsents_ConsultId_ProcedureType_IsVoided")]
[Index("PNo", Name = "IX_AppAestheticSignedConsents_PNo")]
[Index("PatientId", Name = "IX_AppAestheticSignedConsents_PatientId")]
public partial class AppAestheticSignedConsent
{
    [Key]
    public int Id { get; set; }

    public int? PatientId { get; set; }

    public int ConsentTemplateId { get; set; }

    [StringLength(50)]
    public string ConsultId { get; set; } = null!;

    [StringLength(100)]
    public string PNo { get; set; } = null!;

    [StringLength(100)]
    public string ProcedureType { get; set; } = null!;

    public DateTime SignedDate { get; set; }

    [StringLength(150)]
    public string? SignedBy { get; set; }

    [StringLength(150)]
    public string? WitnessedBy { get; set; }

    [StringLength(150)]
    public string SignatureName { get; set; } = null!;

    public string? Notes { get; set; }

    public string ConsentContent { get; set; } = null!;

    public byte[]? SignatureImage { get; set; }

    public string? SignatureImagePath { get; set; }

    [StringLength(150)]
    public string? DoctorViewedBy { get; set; }

    public DateTime? DoctorViewedDate { get; set; }

    public bool IsVoided { get; set; }

    [StringLength(500)]
    public string? VoidReason { get; set; }

    [StringLength(40)]
    public string? CreatedBy { get; set; }

    [StringLength(40)]
    public string? UpdatedBy { get; set; }

    public DateTime UpdatedDate { get; set; }

    public DateTime CreatedDate { get; set; }

    [ForeignKey("ConsentTemplateId")]
    [InverseProperty("AppAestheticSignedConsents")]
    public virtual AppAestheticConsentTemplate ConsentTemplate { get; set; } = null!;

    [ForeignKey("PatientId")]
    [InverseProperty("AppAestheticSignedConsents")]
    public virtual AestheticPatient? Patient { get; set; }
}
