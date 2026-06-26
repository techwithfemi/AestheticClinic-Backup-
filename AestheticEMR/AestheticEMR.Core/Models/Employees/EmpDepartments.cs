namespace AestheticEMR.Core.Models.Employees;

public partial class EmpDepartments
{
    public string DeptId { get; set; } = null!;

    public string? DeptName { get; set; }

    public string? DeptDesc { get; set; }

    public string? OrgId { get; set; }
}
