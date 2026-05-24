using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HRest
{
    public DateTime RDate { get; set; }

    public DateTime RTime { get; set; }

    public string PNo { get; set; } = null!;

    public string CertifiedBy { get; set; } = null!;

    public DateTime? MoveDate { get; set; }

    public string? WardId { get; set; }

    public string? Age { get; set; }

    public string? Reason { get; set; }

    public string? Remarks { get; set; }
}
