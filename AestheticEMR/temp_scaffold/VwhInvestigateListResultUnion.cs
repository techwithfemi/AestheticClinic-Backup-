using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwhInvestigateListResultUnion
{
    public string? ConId { get; set; }

    public string Fullname { get; set; } = null!;

    public string ConsultId { get; set; } = null!;

    public DateTime InvDate { get; set; }
}
