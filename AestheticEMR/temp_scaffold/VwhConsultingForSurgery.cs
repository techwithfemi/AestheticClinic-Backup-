using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwhConsultingForSurgery
{
    public long Id { get; set; }

    public string? MedRpt { get; set; }

    public string ConsultId { get; set; } = null!;

    public long? ConId { get; set; }

    public string? Findings { get; set; }

    public string? Prosedure { get; set; }

    public DateTime SDate { get; set; }
}
