using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class EmpLeaveSchedule
{
    public string SchdId { get; set; } = null!;

    public DateTime? StartDate { get; set; }

    public DateTime? EndDtae { get; set; }

    public string? EmpId { get; set; }

    public string? ApprovedBy { get; set; }
}
