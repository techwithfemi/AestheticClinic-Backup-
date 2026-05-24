using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HAntenatalIncident
{
    public long Sno { get; set; }

    public string IncidentType { get; set; } = null!;

    public short ValueType { get; set; }

    public string Remarks { get; set; } = null!;

    public DateTime IncDate { get; set; }

    public DateTime? EntryDate { get; set; }

    public string Pno { get; set; } = null!;

    public string EntryBy { get; set; } = null!;
}
