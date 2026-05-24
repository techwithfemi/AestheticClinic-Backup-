using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwhNurseAllergy
{
    public long Sno { get; set; }

    public string ConsultId { get; set; } = null!;

    public string Pno { get; set; } = null!;

    public DateTime Date { get; set; }

    public string Event { get; set; } = null!;

    public DateTime ReactionTime { get; set; }

    public DateTime TimeSeenByDoctor { get; set; }

    public string SignsAndSymptoms { get; set; } = null!;

    public string? Others { get; set; }

    public string? Comments { get; set; }

    public string Note { get; set; } = null!;

    public string? Nurse { get; set; }

    public string? Doctor { get; set; }

    public string Fullname { get; set; } = null!;

    public string NurseId { get; set; } = null!;

    public string DocId { get; set; } = null!;
}
