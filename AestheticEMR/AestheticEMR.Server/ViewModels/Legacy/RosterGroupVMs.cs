using System.ComponentModel.DataAnnotations;

namespace AestheticEMR.Server.ViewModels.Legacy;

public sealed class RosterGroupGridItemVM
{
    public string GroupName { get; set; } = string.Empty;
    public string StaffName { get; set; } = string.Empty;
    public string DeptName { get; set; } = string.Empty;
    public string Assigned { get; set; } = string.Empty;
    public long GroupID { get; set; }
    public string EmpID { get; set; } = string.Empty;
}

public sealed class RosterGroupItemVM
{
    public long RosterGrpId { get; set; }
    public string RosterGrpName { get; set; } = string.Empty;
    public string? DeptId { get; set; }
    public string? DeptName { get; set; }
    public string? Exempted { get; set; }
    public int? EmployeeCount { get; set; }
}

public sealed class RosterGroupDepartmentItemVM
{
    public string DeptId { get; set; } = string.Empty;
    public string DeptName { get; set; } = string.Empty;
}

public sealed class RosterGroupAvailableStaffItemVM
{
    public string EmpId { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? DeptId { get; set; }
    public long? RosterGrpId { get; set; }
    public string? RosterGrpName { get; set; }
}

public sealed class RosterGroupSaveVM
{
    [Required, StringLength(50)]
    public string DeptId { get; set; } = string.Empty;

    [Required, StringLength(200)]
    public string RosterGrpName { get; set; } = string.Empty;

    [StringLength(10)]
    public string Exempted { get; set; } = "NO";

    [MinLength(1)]
    public List<string> EmpIds { get; set; } = [];
}
