using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwHdiagnosisList
{
    public string? Disease { get; set; }

    public long Id { get; set; }

    public DateTime? CDate { get; set; }

    public string ConsultId { get; set; } = null!;

    public string Pno { get; set; } = null!;

    public string? ConId { get; set; }
}
