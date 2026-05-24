using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwDocAssignedOnDutyRoster
{
    public string? Doctor { get; set; }

    public string EmpId { get; set; } = null!;

    public string ClinicId { get; set; } = null!;

    public string ClinicName { get; set; } = null!;

    public DateTime? SignIn { get; set; }

    public DateTime RosterDate { get; set; }
}
