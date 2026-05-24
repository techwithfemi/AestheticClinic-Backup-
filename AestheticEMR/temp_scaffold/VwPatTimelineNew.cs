using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwPatTimelineNew
{
    public int Sno { get; set; }

    public string Fullname { get; set; } = null!;

    public string? AttndDate { get; set; }

    public string ServicePoint { get; set; } = null!;

    public DateTime? EntryDate { get; set; }

    public DateTime? EntryTime { get; set; }

    public string Company { get; set; } = null!;

    public string ConsultId { get; set; } = null!;

    public string? ClientCatId { get; set; }

    public long? ConId { get; set; }

    public string? EntryOrExit { get; set; }

    public string? AppName { get; set; }

    public string? ClientName { get; set; }

    public string? Remarks { get; set; }

    public string? CoyId { get; set; }
}
