using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class EmployeeStatusOld
{
    public long Sno { get; set; }

    public string StatId { get; set; } = null!;

    public string StatName { get; set; } = null!;
}
