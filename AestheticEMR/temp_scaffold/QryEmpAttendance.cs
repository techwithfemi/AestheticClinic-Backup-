using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryEmpAttendance
{
    public long RecId { get; set; }

    public DateTime Date { get; set; }

    public string Fullname { get; set; } = null!;

    public string Dept { get; set; } = null!;

    public string Shift { get; set; } = null!;

    public DateTime? Resumption { get; set; }

    public DateTime ResumedTime { get; set; }

    public string? RStatus { get; set; }

    public string Remarks { get; set; } = null!;

    public DateTime? CTime { get; set; }

    public DateTime? SignOffTime { get; set; }

    public string? CStatus { get; set; }

    public string? CRemarks { get; set; }

    public string StaffNo { get; set; } = null!;

    public string? Ovt { get; set; }

    public string? IsLock { get; set; }
}
