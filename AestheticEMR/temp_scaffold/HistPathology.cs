using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HistPathology
{
    public string LabNo { get; set; } = null!;

    public string? PathNo { get; set; }

    public string? Clinician { get; set; }

    public string? EtnicGroup { get; set; }

    public string? Ward { get; set; }

    public string Diagnosis { get; set; } = null!;

    public string? Test { get; set; }

    public string? Maternal { get; set; }

    public DateTime DtDate { get; set; }

    public string Report { get; set; } = null!;

    public long? SnoId { get; set; }

    public long Id { get; set; }
}
