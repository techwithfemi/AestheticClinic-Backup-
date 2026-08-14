using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

public partial class AppAestheticConsentTemplate
{
    [Key]
    public int Id { get; set; }

    [StringLength(150)]
    public string Name { get; set; } = null!;

    [StringLength(200)]
    public string Title { get; set; } = null!;

    [StringLength(100)]
    public string? ProcedureType { get; set; }

    public string Content { get; set; } = null!;

    public bool IsActive { get; set; }

    [StringLength(40)]
    public string? CreatedBy { get; set; }

    [StringLength(40)]
    public string? UpdatedBy { get; set; }

    public DateTime UpdatedDate { get; set; }

    public DateTime CreatedDate { get; set; }

    [InverseProperty("ConsentTemplate")]
    public virtual ICollection<AppAestheticSignedConsent> AppAestheticSignedConsents { get; set; } = new List<AppAestheticSignedConsent>();
}
