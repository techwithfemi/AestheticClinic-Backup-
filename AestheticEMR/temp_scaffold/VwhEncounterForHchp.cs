using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwhEncounterForHchp
{
    public string? EmpNo { get; set; }

    public string Fullname { get; set; } = null!;

    public string? Sex { get; set; }

    public string? Visittype { get; set; }

    public DateOnly? EncDate { get; set; }

    public DateTime? DischDate { get; set; }

    public string? ConDforDisch { get; set; }

    public string Doctor { get; set; } = null!;

    public string? Diag { get; set; }

    public string? OtherDiag { get; set; }

    public string? Lab { get; set; }

    public string? OtherLab { get; set; }

    public string? Drug { get; set; }

    public string? OtherDrug { get; set; }

    public string? Procd { get; set; }

    public string? OtherProcd { get; set; }

    public string? Remarks { get; set; }

    public string PSurname { get; set; } = null!;

    public string? PFirstname { get; set; }
}
