using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class EmpAttendanceParam
{
    public string ShiftType { get; set; } = null!;

    public DateTime ResumTime { get; set; }

    public DateTime CloseTime { get; set; }

    public string ResumRemEarly { get; set; } = null!;

    public string ResumRemLate { get; set; } = null!;

    public string CloseRemNorm { get; set; } = null!;

    public string CloseRemAbNorm { get; set; } = null!;

    public string EvalTo { get; set; } = null!;
}
