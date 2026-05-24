using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwhInvestigateListResultCombo2
{
    public DateTime InvDate { get; set; }

    public string? ConId { get; set; }

    public string ConsultId { get; set; } = null!;

    public string PNo { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public DateTime ResultDate { get; set; }

    public string? ClientCat { get; set; }

    public int? Age { get; set; }

    public string? Coyname { get; set; }

    public string Company { get; set; } = null!;
}
