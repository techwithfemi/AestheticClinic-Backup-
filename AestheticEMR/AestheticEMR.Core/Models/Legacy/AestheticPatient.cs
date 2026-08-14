using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

public partial class AestheticPatient
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string FirstName { get; set; } = null!;

    [StringLength(100)]
    public string LastName { get; set; } = null!;

    [StringLength(100)]
    public string? Email { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string? PhoneNumber { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string? Gender { get; set; }

    [StringLength(50)]
    public string? SkinType { get; set; }

    public string? Allergies { get; set; }

    [StringLength(4000)]
    [Unicode(false)]
    public string? MedicalHistory { get; set; }

    public string? CurrentMedications { get; set; }

    public string? Notes { get; set; }

    [StringLength(40)]
    public string? CreatedBy { get; set; }

    [StringLength(40)]
    public string? UpdatedBy { get; set; }

    public DateTime UpdatedDate { get; set; }

    public DateTime CreatedDate { get; set; }

    public string? Pno { get; set; }

    [InverseProperty("Patient")]
    public virtual ICollection<AestheticConsultation> AestheticConsultations { get; set; } = new List<AestheticConsultation>();

    [InverseProperty("Patient")]
    public virtual ICollection<AppAestheticSignedConsent> AppAestheticSignedConsents { get; set; } = new List<AppAestheticSignedConsent>();
}
