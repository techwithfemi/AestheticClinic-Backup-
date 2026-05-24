using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HEncounterForHchp
{
    public long Sno { get; set; }

    public long? ConId { get; set; }

    public string? ConsultId { get; set; }

    public string? Pno { get; set; }

    public DateOnly? EncDate { get; set; }

    public string? DischCond { get; set; }

    public string? EncType { get; set; }

    public string? Diag { get; set; }

    public string? OtherDiag { get; set; }

    public string? Lab { get; set; }

    public string? OtherLab { get; set; }

    public string? Drug { get; set; }

    public string? OtherDrug { get; set; }

    public string? Procd { get; set; }

    public string? OtherProcd { get; set; }

    public string? Remarks { get; set; }
}
