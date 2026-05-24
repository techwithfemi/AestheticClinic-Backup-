using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryEmpNo
{
    public string EmpId { get; set; } = null!;

    public string? EmpNo { get; set; }

    public string LastName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string Dept { get; set; } = null!;
}
