namespace AestheticEMR.Server.ViewModels.Legacy;

/// <summary>
/// Lightweight view of an employee for dropdown/lookup controls.
/// Property names are intentionally camelCase to match the Angular <c>VwEmpName</c> model
/// so ng-select bindLabel/bindValue resolve correctly without per-request mapping.
/// </summary>
public class EmployeeLookupVM
{
    public string empID { get; set; } = null!;
    public string empName { get; set; } = null!;
    public string dept { get; set; } = null!;
    public string designation { get; set; } = null!;
}
