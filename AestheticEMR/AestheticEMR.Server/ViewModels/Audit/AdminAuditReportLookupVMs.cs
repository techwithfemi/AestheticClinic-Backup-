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

public class AdminUsersReportRowVM
{
    public string JobTitle { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? Configuration { get; set; }
    public bool IsEnabled { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public bool EmailConfirmed { get; set; }
    public bool PhoneNumberConfirmed { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}
