using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhInvestigateResult
{
    public DateTime InvDate { get; set; }

    public string ConsultId { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public string Pno { get; set; } = null!;

    public string? Investigate { get; set; }

    public string ClientCat { get; set; } = null!;

    public string? SympItem { get; set; }

    public string? Result { get; set; }

    public string? Remarks { get; set; }

    public bool? AttendedTo { get; set; }

    public string? Empname { get; set; }

    public string? InvResult { get; set; }
}
