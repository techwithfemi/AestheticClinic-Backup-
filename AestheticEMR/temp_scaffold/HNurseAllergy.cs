using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HNurseAllergy
{
    public long Sno { get; set; }

    public string ConsultId { get; set; } = null!;

    public string Pno { get; set; } = null!;

    public DateTime Adate { get; set; }

    public string Event { get; set; } = null!;

    public DateTime TReaction { get; set; }

    public DateTime TDoc { get; set; }

    public string Symptoms { get; set; } = null!;

    public string? Others { get; set; }

    public string? Comments { get; set; }

    public string Note { get; set; } = null!;

    public string Nurse { get; set; } = null!;

    public string Doctor { get; set; } = null!;
}
