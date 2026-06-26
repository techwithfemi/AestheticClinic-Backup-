namespace AestheticEMR.Core.Models.Employees;

public partial class Locations
{
    public string LocationId { get; set; }  // pri key

    public string? LocationName { get; set; }

    public string? LocationDesc { get; set; }

    public string? OrgId { get; set; }
}
