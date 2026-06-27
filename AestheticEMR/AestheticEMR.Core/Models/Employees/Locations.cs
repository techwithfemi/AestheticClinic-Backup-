namespace AestheticEMR.Core.Models.Employees;

public partial class Locations
{
    public string LocID { get; set; }  // pri key

    public string? LocName { get; set; }

    //public string? LocationDesc { get; set; }

    public string? OrgId { get; set; }
}
