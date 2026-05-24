using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HNotify
{
    public long Sno { get; set; }

    public string NotifyDept { get; set; } = null!;

    public string? NotifyFrom { get; set; }

    public string ConsultId { get; set; } = null!;

    public string? Remarks { get; set; }

    public string AttendedTo { get; set; } = null!;

    public DateTime? Ndate { get; set; }

    public DateTime? Ntime { get; set; }

    public string? ClientName { get; set; }

    public string? AppName { get; set; }

    public DateTime? EntryDate { get; set; }

    public DateTime? EntryTime { get; set; }
}
