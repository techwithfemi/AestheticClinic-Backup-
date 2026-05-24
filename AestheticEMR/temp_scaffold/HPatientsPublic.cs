using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HPatientsPublic
{
    public string Pno { get; set; } = null!;

    public string PSurname { get; set; } = null!;

    public string PFirstName { get; set; } = null!;

    public string Maturity { get; set; } = null!;

    public int? Age { get; set; }

    public string? Sex { get; set; }

    public double? Debt { get; set; }

    public string CoyName { get; set; } = null!;

    public string? PPhoneNo { get; set; }
}
