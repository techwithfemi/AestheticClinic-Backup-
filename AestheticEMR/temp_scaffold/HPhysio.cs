using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HPhysio
{
    public string ConsultId { get; set; } = null!;

    public string Pno { get; set; } = null!;

    public string? Activity { get; set; }

    public string? Coping { get; set; }

    public string? Limitation { get; set; }

    public string? Tests { get; set; }

    public string? Impression { get; set; }

    public string? Goals { get; set; }

    public string? Means { get; set; }

    public DateTime? ReviewDate { get; set; }

    public DateTime? ApptDate { get; set; }

    public string? ConId { get; set; }

    public int Id { get; set; }
}
