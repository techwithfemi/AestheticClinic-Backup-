using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HInPatientNotesForDoctor
{
    public int Sno { get; set; }

    public string Pno { get; set; } = null!;

    public string ClientCat { get; set; } = null!;

    public string ConsultId { get; set; } = null!;

    public DateTime NDate { get; set; }

    public DateTime? NTime { get; set; }

    public string Notes { get; set; } = null!;

    public string? Prescription { get; set; }

    public string? EmpId { get; set; }
}
