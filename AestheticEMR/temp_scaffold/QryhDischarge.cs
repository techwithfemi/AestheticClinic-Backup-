using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhDischarge
{
    public long Id { get; set; }

    public DateTime? DischDate { get; set; }

    public DateTime DischTime { get; set; }

    public string? WardId { get; set; }

    public string Fullname { get; set; } = null!;

    public string? ChiefCompaints { get; set; }

    public string? DiagnosisFindings { get; set; }

    public string? DrugsGiven { get; set; }

    public string? ResponseToDrug { get; set; }

    public string? Recommend { get; set; }

    public string? StaffNo { get; set; }

    public string Pno { get; set; } = null!;

    public string StaffName { get; set; } = null!;

    public DateTime? ApptDate { get; set; }

    public string? Reason { get; set; }

    public string? Remarks { get; set; }

    public string ConsultId { get; set; } = null!;

    public string ClientCat { get; set; } = null!;
}
