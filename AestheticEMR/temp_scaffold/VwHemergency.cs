using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwHemergency
{
    public long Sno { get; set; }

    public DateTime Date { get; set; }

    public DateTime TimeIn { get; set; }

    public DateTime TimeOut { get; set; }

    public string Fullname { get; set; } = null!;

    public string Company { get; set; } = null!;

    public string Complaint { get; set; } = null!;

    public string Diagnosis { get; set; } = null!;

    public string CareGiven { get; set; } = null!;

    public string ItemsUsed { get; set; } = null!;

    public string? Remarks { get; set; }

    public string? EntryBy { get; set; }

    public string ConsultId { get; set; } = null!;

    public string? EmpId { get; set; }

    public string? ClientCat { get; set; }
}
