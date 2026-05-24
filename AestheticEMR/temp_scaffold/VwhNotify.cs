using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwhNotify
{
    public long Sno { get; set; }

    public DateTime? Date { get; set; }

    public DateTime? Time { get; set; }

    public string Fullname { get; set; } = null!;

    public string NotifyDept { get; set; } = null!;

    public string? NotifyFrom { get; set; }

    public string ConsultId { get; set; } = null!;

    public string? Remarks { get; set; }

    public string AttendedTo { get; set; } = null!;
}
