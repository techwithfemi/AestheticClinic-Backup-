using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HPatientsTimeline
{
    public long Sno { get; set; }

    public string ConsultId { get; set; } = null!;

    public string ServicePoint { get; set; } = null!;

    public string? Remarks { get; set; }

    public string? AppName { get; set; }

    public string? ClientName { get; set; }

    public DateTime? EntryDate { get; set; }

    public DateTime? EntryTime { get; set; }

    public long? ConId { get; set; }

    public string? EntryOrExit { get; set; }
}
