namespace AestheticEMR.Core.Services.Audit;

public sealed class AdminAuditReportUserLookup
{
    public string UserName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string DisplayText { get; set; } = string.Empty;
}

public sealed class AdminAuditReportModuleLookup
{
    public string Name { get; set; } = string.Empty;
}
