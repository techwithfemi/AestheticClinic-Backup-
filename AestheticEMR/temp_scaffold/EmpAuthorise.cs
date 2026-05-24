using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class EmpAuthorise
{
    public int Sno { get; set; }

    public string EmpId { get; set; } = null!;

    public int EmpAuth { get; set; }
}
