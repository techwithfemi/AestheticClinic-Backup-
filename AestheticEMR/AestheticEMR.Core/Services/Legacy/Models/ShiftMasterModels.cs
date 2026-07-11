namespace AestheticEMR.Core.Services.Legacy.Models;

public sealed class DepartmentLookupItem
{
    public string DeptId { get; set; } = string.Empty;
    public string DeptName { get; set; } = string.Empty;
    public string? Location { get; set; }
}

public sealed class ShiftMasterItem
{
    public long ShiftId { get; set; }
    public string ShiftName { get; set; } = string.Empty;
    public int DepartmentCount { get; set; }
}

public sealed class ShiftMasterDetail
{
    public long ShiftId { get; set; }
    public string ShiftName { get; set; } = string.Empty;
    public List<string> DeptIds { get; set; } = [];
}

public sealed class ShiftMasterSaveRequest
{
    public string ShiftName { get; set; } = string.Empty;
    public List<string> DeptIds { get; set; } = [];
}
