using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HScreeningResult
{
    public long Id { get; set; }

    public string Labno { get; set; } = null!;

    public DateTime Invdate { get; set; }

    public string Empid { get; set; } = null!;

    public bool? Attendedto { get; set; }
}
