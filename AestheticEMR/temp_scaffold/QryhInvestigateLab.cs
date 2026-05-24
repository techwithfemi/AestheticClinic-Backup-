using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhInvestigateLab
{
    public string PSurname { get; set; } = null!;

    public string PFirstname { get; set; } = null!;

    public DateTime InvDate { get; set; }

    public string ConsultId { get; set; } = null!;

    public bool? AttendedTo { get; set; }

    public string? Capitated { get; set; }
}
