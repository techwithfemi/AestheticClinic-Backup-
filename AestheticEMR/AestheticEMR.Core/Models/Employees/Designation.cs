namespace AestheticEMR.Core.Models.Employees;

public partial class Designation
{
    public string DesignationId { get; set; } = null!; // pri key

    public string? DesignationName { get; set; }

    public string? DesignationDesc { get; set; }
}
