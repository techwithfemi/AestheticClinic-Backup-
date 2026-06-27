namespace AestheticEMR.Core.Models.Employees;

public partial class Organization
{
    public string OrgId { get; set; } = null!;

    public string? OrgName { get; set; }

    public string? OrgAddress1 { get; set; }

    public string? OrgAddress2 { get; set; }
    public string? OrgAddress3 { get; set; }
    public string? OrgLogo { get; set; }
}
