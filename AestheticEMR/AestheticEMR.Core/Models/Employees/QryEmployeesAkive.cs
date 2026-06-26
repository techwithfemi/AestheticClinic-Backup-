namespace AestheticEMR.Core.Models.Employees;

public partial class QryEmployeesAkive
{
    public string EmpId { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? OtherName { get; set; }

    public string? DeptName { get; set; }

    public string? DesignationName { get; set; }

    public string? EmpCatDesc { get; set; }

    public string? EmpStatusDesc { get; set; }

    public DateTime? HireDate { get; set; }

    public DateTime? Dob { get; set; }

    public string? SalaryScale { get; set; }

    public decimal? GrossSal { get; set; }
}
