using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwDocName
{
    public string EmpId { get; set; } = null!;

    public string DocName { get; set; } = null!;

    public string? Designation { get; set; }

    public string? LoginRole { get; set; }
}
