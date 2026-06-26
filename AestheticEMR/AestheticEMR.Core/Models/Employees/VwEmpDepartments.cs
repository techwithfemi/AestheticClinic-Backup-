namespace AestheticEMR.Core.Models.Employees;

public partial class VwEmpDepartments
{
    public string DeptId { get; set; } = null!;

    public string? DeptName { get; set; }

    public string? DeptDesc { get; set; }

    public string? OrgName { get; set; }

    public bool? Akive { get; set; }
}
