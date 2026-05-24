using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhFullname1
{
    public string PNo { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public string? OldpNo { get; set; }

    public string? CoyType { get; set; }

    public string? CoyName { get; set; }

    public string? PolicyType { get; set; }
}
