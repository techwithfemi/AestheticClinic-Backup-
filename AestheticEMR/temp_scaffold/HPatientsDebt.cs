using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HPatientsDebt
{
    public DateTime? EntryDate { get; set; }

    public double? Debt { get; set; }

    public string? Pno { get; set; }
}
