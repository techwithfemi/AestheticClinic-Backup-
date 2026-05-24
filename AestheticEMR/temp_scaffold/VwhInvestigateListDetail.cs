using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwhInvestigateListDetail
{
    public DateTime InvDate { get; set; }

    public string ConId { get; set; } = null!;

    public string ConsultId { get; set; } = null!;

    public string? LabItem { get; set; }

    public string? Category { get; set; }

    public double? SubTotal { get; set; }

    public string Capitated { get; set; } = null!;

    public string Pno { get; set; } = null!;

    public int Id { get; set; }

    public string? Referal { get; set; }

    public string? Clientcat { get; set; }

    public string Fullname { get; set; } = null!;

    public string? TreatedBy { get; set; }

    public string? Remarks { get; set; }

    public string Sex { get; set; } = null!;

    public int? Age { get; set; }

    public string? Company { get; set; }

    public bool? AttendedTo { get; set; }

    public bool? AttendedTobyLab { get; set; }

    public DateTime? CTime { get; set; }
}
