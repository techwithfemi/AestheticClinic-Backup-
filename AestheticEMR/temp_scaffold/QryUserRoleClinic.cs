using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryUserRoleClinic
{
    public string UserName { get; set; } = null!;

    public string? Fullname { get; set; }

    public string? Doctor { get; set; }

    public string? AccountStatus { get; set; }

    public string ClinicId { get; set; } = null!;

    public string ClinicName { get; set; } = null!;

    public string EmpId { get; set; } = null!;
}
