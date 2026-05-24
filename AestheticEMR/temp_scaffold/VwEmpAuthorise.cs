using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwEmpAuthorise
{
    public int Sno { get; set; }

    public string EmpName { get; set; } = null!;

    public string EmpId { get; set; } = null!;

    public string Dept { get; set; } = null!;
}
