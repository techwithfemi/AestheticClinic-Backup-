using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HApptTest
{
    public long Id { get; set; }

    public string Pno { get; set; } = null!;

    public DateTime? EntryDate { get; set; }

    public DateTime? EntryTime { get; set; }

    public DateTime? ApptDate { get; set; }

    public DateTime? ApptTime { get; set; }
}
