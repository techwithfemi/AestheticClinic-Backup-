namespace AestheticEMR.Core.Models.Employees;

public partial class VwEmpName
{
    public string EmpId { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? OtherName { get; set; }

    public string? FullName { get; set; }
}
