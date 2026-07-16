using System.ComponentModel.DataAnnotations;

namespace AestheticEMR.Server.ViewModels.Legacy;

public sealed class RosterLookupsVM
{
    public List<RosterGroupLookupVM> Groups { get; set; } = [];
    public List<RosterStaffLookupVM> SourceStaff { get; set; } = [];
    public List<RosterStaffLookupVM> TargetStaff { get; set; } = [];
    public List<RosterShiftLookupVM> Shifts { get; set; } = [];
}

public sealed class RosterGroupLookupVM
{
    public long GroupId { get; set; }
    public string GroupName { get; set; } = string.Empty;
    public string? DeptId { get; set; }
    public string? DeptName { get; set; }
}

public sealed class RosterStaffLookupVM
{
    public string EmpId { get; set; } = string.Empty;
    public string EmpName { get; set; } = string.Empty;
}

public sealed class RosterShiftLookupVM
{
    public long SNo { get; set; }
    public string ShiftName { get; set; } = string.Empty;
    public string EvalTo { get; set; } = string.Empty;
    public string? DeptId { get; set; }
}

public sealed class RosterGridQueryVM
{
    public string? DeptId { get; set; }
    public long? GroupId { get; set; }
    public DateOnly? FromDate { get; set; }
    public DateOnly? ToDate { get; set; }
    public bool LatestOnly { get; set; } = true;
}

public sealed class RosterEditorQueryVM
{
    public string EmpId { get; set; } = string.Empty;
    public string? DeptId { get; set; }
    public DateOnly? FromDate { get; set; }
    public DateOnly? ToDate { get; set; }
}

public sealed class RosterDaySelectionVM
{
    public DateOnly Date { get; set; }
    public long ShiftId { get; set; }
    public string ShiftAbbrv { get; set; } = string.Empty;
    public string ShiftName { get; set; } = string.Empty;
}

public sealed class RosterSaveVM
{
    public string? DeptId { get; set; }

    [StringLength(200)]
    public string? DeptName { get; set; }

    public long? GroupId { get; set; }

    [StringLength(50)]
    public string? SourceEmpId { get; set; }

    [StringLength(50)]
    public string? TargetEmpId { get; set; }

    [Required, StringLength(200)]
    public string GroupName { get; set; } = string.Empty;

    [MinLength(1)]
    public List<RosterDaySelectionVM> SelectedDays { get; set; } = [];

    public List<RosterDaySelectionVM> UnselectedDays { get; set; } = [];
}

public sealed class RosterDeleteVM
{
    public long SNo { get; set; }
}

public sealed class RosterGridItemVM
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

public sealed class RosterSaveResultVM
{
    public int CreatedCount { get; set; }
    public List<RosterGridItemVM> Items { get; set; } = [];
}
