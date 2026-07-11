namespace AestheticEMR.Core.Services.Legacy.Models;

public sealed class RosterLookups
{
    public List<RosterGroupLookup> Groups { get; set; } = [];
    public List<RosterStaffLookup> SourceStaff { get; set; } = [];
    public List<RosterStaffLookup> TargetStaff { get; set; } = [];
    public List<RosterShiftLookup> Shifts { get; set; } = [];
}

public sealed class RosterGroupLookup
{
    public long GroupId { get; set; }
    public string GroupName { get; set; } = string.Empty;
    public string? DeptId { get; set; }
}

public sealed class RosterStaffLookup
{
    public string EmpId { get; set; } = string.Empty;
    public string EmpName { get; set; } = string.Empty;
}

public sealed class RosterShiftLookup
{
    public long SNo { get; set; }
    public string ShiftName { get; set; } = string.Empty;
    public string EvalTo { get; set; } = string.Empty;
}

public sealed class RosterGridQuery
{
    public string? DeptId { get; set; }
    public long? GroupId { get; set; }
    public DateOnly? FromDate { get; set; }
    public DateOnly? ToDate { get; set; }
    public bool LatestOnly { get; set; } = true;
}

public sealed class RosterEditorQuery
{
    public string EmpId { get; set; } = string.Empty;
    public string? DeptId { get; set; }
    public DateOnly? FromDate { get; set; }
    public DateOnly? ToDate { get; set; }
}

public sealed class RosterDaySelection
{
    public DateOnly Date { get; set; }
    public long ShiftId { get; set; }
    public string ShiftAbbrv { get; set; } = string.Empty;
    public string ShiftName { get; set; } = string.Empty;
}

public sealed class RosterSaveRequest
{
    public string DeptId { get; set; } = string.Empty;
    public long? GroupId { get; set; }
    public string? SourceEmpId { get; set; }
    public string? TargetEmpId { get; set; }
    public string GroupName { get; set; } = string.Empty;
    public List<RosterDaySelection> SelectedDays { get; set; } = [];
}

public sealed class RosterDeleteRequest
{
    public long SNo { get; set; }
}

public sealed class RosterSaveResult
{
    public int CreatedCount { get; set; }
    public List<RosterGridItem> Items { get; set; } = [];
}

public sealed class RosterGridItem
{
    public long SNo { get; set; }
    public DateTime Date { get; set; }
    public string? StaffName { get; set; }
    public string? ClockIn { get; set; }
    public string? ClockOut { get; set; }
    public string? Status { get; set; }
    public decimal? Fine { get; set; }
    public string? ShiftName { get; set; }
    public string? GroupName { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? DeptName { get; set; }
    public string? Exempted { get; set; }
    public string? GroupID { get; set; }
    public long? RosterGrpShiftID { get; set; }
    public string? EmpID { get; set; }
    public string? ShiftAbbrv { get; set; }
}
