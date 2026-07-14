namespace AestheticEMR.Core.Services.Legacy.Models;

public sealed class RosterGroupGridItem
{
    public string GroupName { get; set; } = string.Empty;
    public string StaffName { get; set; } = string.Empty;
    public string DeptName { get; set; } = string.Empty;
    public string Assigned { get; set; } = string.Empty;
    public long GroupID { get; set; }
    public string EmpID { get; set; } = string.Empty;
}

public sealed class RosterGroupItem
{
    public long RosterGrpId { get; set; }
    public string RosterGrpName { get; set; } = string.Empty;
    public string? DeptId { get; set; }
    public string? DeptName { get; set; }
    public string? Exempted { get; set; }
    public int? EmployeeCount { get; set; }
}

public sealed class RosterGroupDepartmentItem
{
    public string DeptId { get; set; } = string.Empty;
    public string DeptName { get; set; } = string.Empty;
}

public sealed class RosterGroupAvailableStaffItem
{
    public string EmpId { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? DeptId { get; set; }
    public long? RosterGrpId { get; set; }
    public string? RosterGrpName { get; set; }
}

public sealed class RosterGroupSaveRequest
{
    public string DeptId { get; set; } = string.Empty;
    public string RosterGrpName { get; set; } = string.Empty;
    public string Exempted { get; set; } = "NO";
    public List<string> EmpIds { get; set; } = [];
}
