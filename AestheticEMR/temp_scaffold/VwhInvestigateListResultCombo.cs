using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwhInvestigateListResultCombo
{
    public DateTime InvDate { get; set; }

    public string? ConId { get; set; }

    public string ConsultId { get; set; } = null!;

    public string? LabItem { get; set; }

    public string? Category { get; set; }

    public string Capitated { get; set; } = null!;

    public string PNo { get; set; } = null!;

    public long Id { get; set; }

    public string? Referal { get; set; }

    public string? Clientcat { get; set; }

    public string Fullname { get; set; } = null!;

    public string? Sex { get; set; }

    public int? Age { get; set; }

    public string? CoyCode { get; set; }

    public bool? AttendedTo { get; set; }

    public bool? AttendedTobyLab { get; set; }

    public string? LabNum { get; set; }

    public string? InvRemarks { get; set; }
}
