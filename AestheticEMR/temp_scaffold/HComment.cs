using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HComment
{
    public long Sno { get; set; }

    public string Comment { get; set; } = null!;

    public long? Id { get; set; }

    public string? ConsultId { get; set; }

    public string AttendedTo { get; set; } = null!;

    public DateTime Cdate { get; set; }

    public DateTime Ctime { get; set; }

    public string Dept { get; set; } = null!;
}
