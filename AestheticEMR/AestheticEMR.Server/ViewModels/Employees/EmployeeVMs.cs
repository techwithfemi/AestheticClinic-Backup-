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
    /// <summary>
    /// Designation id (e.g. "01", "02", "99"). The server auto-generates this on
    /// create and ignores any client value, so it is optional in POST payloads.
    /// Required on PUT because the id is the route segment and PK.
    /// </summary>
    [StringLength(50)]
    public string? DesignationId { get; set; }

    [Required(AllowEmptyStrings = false), StringLength(150)]
    public string? DesignationName { get; set; }

    /// <summary>
    /// Populated by the list endpoint. The number of employees currently
    /// using this designation. Lets the UI warn before delete and disable it
    /// when &gt; 0. Not part of any create/update payload.
    /// </summary>
    public int? InUseCount { get; set; }
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
