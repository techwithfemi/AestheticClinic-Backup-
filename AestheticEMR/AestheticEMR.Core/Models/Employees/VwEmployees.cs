namespace AestheticEMR.Core.Models.Employees;

public partial class VwEmployees
{
    public string EmpId { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? OtherName { get; set; }

    public string? DeptName { get; set; }

    public string? desName { get; set; }

    public string? catName { get; set; }

    public string? statName { get; set; }

    public DateTime? HireDate { get; set; }

    public DateTime? Dob { get; set; }

    public string? SalaryScale { get; set; }

    public decimal? GrossSal { get; set; }

    public string? NhsNo { get; set; }

    public string? NsitfNo { get; set; }

    public string? JobDesc { get; set; }

    public string? Sex { get; set; }
}
