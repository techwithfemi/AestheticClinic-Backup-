using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HReferalTest
{
    public long Id { get; set; }

    public DateTime? ApptDate { get; set; }

    public DateTime? ApptTime { get; set; }

    public string? PNo { get; set; }

    public DateTime? RefDate { get; set; }

    public DateTime? RefTime { get; set; }
}
