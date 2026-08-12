namespace AestheticEMR.Core.Models.Employees;

public partial class QryEmployees
{
    public string EmpId { get; set; } = null!;

    public string? Fullname { get; set; }

    public string? Dept { get; set; }

    public string? Designation { get; set; }

    public string? Phone { get; set; }

    public DateTime? Dob { get; set; }

    public int? Age { get; set; }
}
