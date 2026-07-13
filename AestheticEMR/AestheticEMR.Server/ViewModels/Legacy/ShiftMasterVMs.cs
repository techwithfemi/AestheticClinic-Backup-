using System.ComponentModel.DataAnnotations;

namespace AestheticEMR.Server.ViewModels.Legacy;

public sealed class DepartmentLookupVM
{
    public string DeptId { get; set; } = string.Empty;
    public string DeptName { get; set; } = string.Empty;
    public string? Location { get; set; }
}

public sealed class ShiftMasterItemVM
{
    public long ShiftId { get; set; }
    public string ShiftName { get; set; } = string.Empty;
    public int DepartmentCount { get; set; }
    public string Departments { get; set; } = string.Empty;
}

public sealed class ShiftMasterDetailVM
{
    public long ShiftId { get; set; }

    [Required, StringLength(200)]
    public string ShiftName { get; set; } = string.Empty;

    [MinLength(1)]
    public List<string> DeptIds { get; set; } = [];
}
