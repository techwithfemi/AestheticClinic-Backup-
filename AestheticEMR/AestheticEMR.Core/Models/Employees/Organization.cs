namespace AestheticEMR.Core.Models.Employees;

public partial class Organization
{
    public string OrgId { get; set; } = null!;

    public string? OrgName { get; set; }

    public string? OrgDesc { get; set; }

    public bool? Akive { get; set; }
}
