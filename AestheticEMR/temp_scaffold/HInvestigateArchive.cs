using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HInvestigateArchive
{
    public int Id { get; set; }

    public DateTime InvDate { get; set; }

    public string ConsultId { get; set; } = null!;

    public string Pno { get; set; } = null!;

    public string? Investigate { get; set; }

    public string? InvResult { get; set; }

    public string ClientCat { get; set; } = null!;

    public bool? AttendedTo { get; set; }

    public string? ConId { get; set; }

    public string? Capitated { get; set; }
}
