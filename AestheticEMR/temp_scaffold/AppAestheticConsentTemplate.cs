using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class AppAestheticConsentTemplate
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string? ProcedureType { get; set; }

    public string Content { get; set; } = null!;

    public bool IsActive { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime UpdatedDate { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual ICollection<AppAestheticSignedConsent> AppAestheticSignedConsents { get; set; } = new List<AppAestheticSignedConsent>();
}
