using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwhApprvCodeRequest
{
    public long Sno { get; set; }

    public DateTime? RecDate { get; set; }

    public string Fullname { get; set; } = null!;

    public string? ConsultId { get; set; }

    public string? Remarks { get; set; }

    public bool? IsSent { get; set; }

    public string? RetainCode { get; set; }

    public string RetainName { get; set; } = null!;

    public string? EnrolleeNo { get; set; }

    public string? PolicyType { get; set; }
}
