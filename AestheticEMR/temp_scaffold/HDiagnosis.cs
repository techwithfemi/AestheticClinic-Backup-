using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HDiagnosis
{
    public long Id { get; set; }

    public DateTime? CDate { get; set; }

    public string ConsultId { get; set; } = null!;

    public string Pno { get; set; } = null!;

    public string Disease { get; set; } = null!;

    public string? ConId { get; set; }

    public DateTime? EntryDate { get; set; }

    public DateTime? EntryTime { get; set; }

    public string? ClientName { get; set; }

    public string? AppName { get; set; }

    public string? Code { get; set; }
}
