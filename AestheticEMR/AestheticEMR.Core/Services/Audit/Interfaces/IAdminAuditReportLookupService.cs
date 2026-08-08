namespace AestheticEMR.Core.Services.Audit.Interfaces;

public interface IAdminAuditReportLookupService
{
    Task<IEnumerable<AdminAuditReportUserLookup>> GetUsersAsync(CancellationToken ct = default);
    Task<IEnumerable<AdminAuditReportModuleLookup>> GetModulesAsync(CancellationToken ct = default);
    Task<IEnumerable<AdminAuditReportRow>> GetReportRowsAsync(DateTime fromDate, DateTime toDate, string filterType, string? filterValue, string? searchTerm, CancellationToken ct = default);
}
