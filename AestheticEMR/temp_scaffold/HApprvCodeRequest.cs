using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HApprvCodeRequest
{
    public long Sno { get; set; }

    public DateTime? ApprvDate { get; set; }

    public string? ConsultId { get; set; }

    public string? Remarks { get; set; }

    public bool? IsSent { get; set; }

    public string? EnrolleeNo { get; set; }
}
