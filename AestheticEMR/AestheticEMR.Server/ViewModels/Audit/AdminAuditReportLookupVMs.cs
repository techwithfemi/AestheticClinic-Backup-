namespace AestheticEMR.Server.ViewModels.Audit;

public class AdminAuditReportUserLookupVM
{
    public string UserName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string DisplayText { get; set; } = string.Empty;
}

public class AdminAuditReportModuleLookupVM
{
    public string Name { get; set; } = string.Empty;
}
