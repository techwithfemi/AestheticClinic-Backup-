using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class AppAestheticSignedConsent
{
    public int Id { get; set; }

    public int? PatientId { get; set; }

    public int ConsentTemplateId { get; set; }

    public string ConsultId { get; set; } = null!;

    public string Pno { get; set; } = null!;

    public string ProcedureType { get; set; } = null!;

    public DateTime SignedDate { get; set; }

    public string? SignedBy { get; set; }

    public string? WitnessedBy { get; set; }

    public string SignatureName { get; set; } = null!;

    public string? Notes { get; set; }

    public string ConsentContent { get; set; } = null!;

    public byte[]? SignatureImage { get; set; }

    public string? SignatureImagePath { get; set; }

    public string? DoctorViewedBy { get; set; }

    public DateTime? DoctorViewedDate { get; set; }

    public bool IsVoided { get; set; }

    public string? VoidReason { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime UpdatedDate { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual AppAestheticConsentTemplate ConsentTemplate { get; set; } = null!;

    public virtual AestheticPatient? Patient { get; set; }
}
