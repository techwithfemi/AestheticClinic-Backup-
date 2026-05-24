using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhDischarged
{
    public DateTime? DischDate { get; set; }

    public DateTime DischTime { get; set; }

    public string Fullname { get; set; } = null!;

    public string Pno { get; set; } = null!;

    public string? WardId { get; set; }

    public string? Reason { get; set; }

    public string? ApprvBy { get; set; }

    public string? Remarks { get; set; }

    public string? ApprovedBy { get; set; }

    public string? CoyName { get; set; }

    public string ClientCat { get; set; } = null!;

    public string ConsultId { get; set; } = null!;

    public string? Company { get; set; }

    public string? Recommend { get; set; }

    public string? ResponseToDrug { get; set; }

    public string? DrugsGiven { get; set; }

    public string? DiagnosisFindings { get; set; }

    public string? ChiefCompaints { get; set; }

    public string RetainCode { get; set; } = null!;

    public string RetainName { get; set; } = null!;
}
