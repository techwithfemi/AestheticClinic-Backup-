using System.ComponentModel.DataAnnotations;

namespace AestheticEMR.Server.ViewModels.Employees;

public class EmployeeVM
{
    public string? EmpId { get; set; }

    [Required, StringLength(100)]
    public required string LastName { get; set; }

    [Required, StringLength(100)]
    public required string FirstName { get; set; }

    [StringLength(50)]
    public string? DesignationId { get; set; }
    public string? DesignationName { get; set; }

    [StringLength(50)]
    public string? DeptId { get; set; }
    public string? DeptName { get; set; }

    public bool Active { get; set; }

    public DateTime? Dob { get; set; }

    [StringLength(10)]
    public string? Sex { get; set; }

    [StringLength(50)]
    public string? EmpStatusCode { get; set; }
}

public class DesignationVM
{
    public required string DesignationId { get; set; }
    public string? DesignationName { get; set; }
}

public class DepartmentVM
{
    public required string DeptId { get; set; }
    public string? DeptName { get; set; }
}

public class EmpDepartmentVM
{
    public required string DeptId { get; set; }
    public string? DeptName { get; set; }
}
