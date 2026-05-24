using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class EmpAttendanceArchive2
{
    public long RecId { get; set; }

    public DateTime AttDate { get; set; }

    public DateTime AttTime { get; set; }

    public string EmpId { get; set; } = null!;

    public string Remarks { get; set; } = null!;

    public string Shift { get; set; } = null!;

    public DateTime? Rtime { get; set; }

    public string? Status { get; set; }

    public int? TimeVal { get; set; }

    public DateTime? CTime { get; set; }

    public string? CStatus { get; set; }

    public int? CTimeVal { get; set; }

    public DateTime? SignOffTime { get; set; }

    public string? CRemarks { get; set; }

    public string? OvtDesc { get; set; }

    public string? IsLock { get; set; }
}
