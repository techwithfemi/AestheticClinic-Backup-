using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhInvestigateListOffline
{
    public int Id { get; set; }

    public DateTime InvDate { get; set; }

    public string ConsultId { get; set; } = null!;

    public string Pno { get; set; } = null!;

    public string? Remarks { get; set; }

    public string ClientCat { get; set; } = null!;

    public string PSurname { get; set; } = null!;

    public string PFirstname { get; set; } = null!;

    public string TreatedBy { get; set; } = null!;

    public int? Age { get; set; }

    public string? CoyName { get; set; }

    public DateTime? CTime { get; set; }

    public string? Company { get; set; }

    public string PCatId { get; set; } = null!;

    public string? Investigate { get; set; }

    public bool? AttendedTobyLab { get; set; }

    public string? ConId { get; set; }

    public bool? AttendedTo { get; set; }

    public string? InvResult { get; set; }
}
