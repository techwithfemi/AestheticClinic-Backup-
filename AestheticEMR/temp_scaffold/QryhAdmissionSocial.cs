using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhAdmissionSocial
{
    public long Sno { get; set; }

    public DateTime Date { get; set; }

    public string Fullname { get; set; } = null!;

    public string Ward { get; set; } = null!;

    public string Diagnosis { get; set; } = null!;

    public string? SocialDiagnosis { get; set; }

    public string PNo { get; set; } = null!;

    public DateTime? Time { get; set; }

    public string ConsultId { get; set; } = null!;

    public string ClientCat { get; set; } = null!;

    public string? CoyName { get; set; }

    public bool? IsDischarged { get; set; }

    public DateTime? Dob { get; set; }

    public string Sex { get; set; } = null!;

    public string? EmpNo { get; set; }

    public string? PolicyType { get; set; }

    public int? Age { get; set; }
}
