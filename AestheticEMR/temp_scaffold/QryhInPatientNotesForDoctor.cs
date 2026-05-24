using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhInPatientNotesForDoctor
{
    public int Sno { get; set; }

    public string Pno { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public string ClientCat { get; set; } = null!;

    public string ConsultId { get; set; } = null!;

    public DateTime NDate { get; set; }

    public DateTime? NTime { get; set; }

    public string Notes { get; set; } = null!;

    public bool? IsDischarged { get; set; }

    public string? Prescription { get; set; }

    public string Empname { get; set; } = null!;

    public string EmpId { get; set; } = null!;
}
