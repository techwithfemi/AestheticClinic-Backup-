namespace AestheticEMR.Core.Services.Legacy;

public class BillingCrossDatabaseSyncStatus
{
    public string EffectiveMode { get; init; } = "HospitalOnly";
    public string? PrimaryDataSource { get; init; }
    public string? PrimaryDatabase { get; init; }
    public IReadOnlyCollection<string> IncludedDatabases { get; init; } = [];
    public IReadOnlyCollection<string> SameInstanceDatabases { get; init; } = [];
    public IReadOnlyCollection<string> CrossInstanceDatabases { get; init; } = [];
    public IReadOnlyCollection<string> Warnings { get; init; } = [];
}
