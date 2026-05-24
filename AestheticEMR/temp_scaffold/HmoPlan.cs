using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HmoPlan
{
    public long Sno { get; set; }

    public string? PlanId { get; set; }

    public string PlanName { get; set; } = null!;

    public string CoyCode { get; set; } = null!;

    public string? Remarks { get; set; }

    public double? Limit { get; set; }

    public string? LimitPeriod { get; set; }
}
