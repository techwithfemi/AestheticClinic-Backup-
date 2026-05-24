using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryEmpAttendanceParam
{
    public string ShiftJob { get; set; } = null!;

    public DateTime ResumptionTime { get; set; }

    public DateTime ClosingTime { get; set; }

    public string EarlyResumptionRemarks { get; set; } = null!;

    public string LateResumptionRemarks { get; set; } = null!;

    public string NormalClosingRemarks { get; set; } = null!;

    public string AbnormalClosingRemarks { get; set; } = null!;

    public string EvalTo { get; set; } = null!;
}
