using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwhComment
{
    public long Sno { get; set; }

    public DateTime Date { get; set; }

    public DateTime Time { get; set; }

    public string Fullname { get; set; } = null!;

    public string Comment { get; set; } = null!;

    public string Dept { get; set; } = null!;

    public string AttendedTo { get; set; } = null!;

    public string? ConsultId { get; set; }
}
