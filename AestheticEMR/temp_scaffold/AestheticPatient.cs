using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class AestheticPatient
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string? Gender { get; set; }

    public string? SkinType { get; set; }

    public string? Allergies { get; set; }

    public string? MedicalHistory { get; set; }

    public string? CurrentMedications { get; set; }

    public string? Notes { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime UpdatedDate { get; set; }

    public DateTime CreatedDate { get; set; }

    public string? Pno { get; set; }

    public virtual ICollection<AestheticConsultation> AestheticConsultations { get; set; } = new List<AestheticConsultation>();

    public virtual ICollection<AppAestheticSignedConsent> AppAestheticSignedConsents { get; set; } = new List<AppAestheticSignedConsent>();
}
