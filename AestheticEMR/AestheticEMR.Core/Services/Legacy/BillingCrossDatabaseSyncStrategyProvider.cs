using AestheticEMR.Core.Services.Legacy.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AestheticEMR.Core.Services.Legacy;

public class BillingCrossDatabaseSyncStrategyProvider(
    IConfiguration configuration,
    ILogger<BillingCrossDatabaseSyncStrategyProvider> logger) : IBillingCrossDatabaseSyncStrategyProvider
{
    private static readonly StringComparer NameComparer = StringComparer.OrdinalIgnoreCase;
    // Accounting is the supported secondary database
    private static readonly IReadOnlyList<string> SecondaryConnectionNames = ["AccountingConnection"];

    private readonly SemaphoreSlim initializeLock = new(1, 1);
    private BillingCrossDatabaseSyncStatus status = new();
    private bool isInitialized;

    public BillingCrossDatabaseSyncStatus CurrentStatus => status;

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        if (isInitialized)
        {
            return;
        }

        await initializeLock.WaitAsync(cancellationToken);
        try
        {
            if (isInitialized)
            {
                return;
            }

            status = await BuildStatusAsync(cancellationToken);
            isInitialized = true;

            logger.LogInformation(
                "Billing sync strategy initialized. Mode={Mode}, Primary={PrimaryDataSource}/{PrimaryDatabase}, Included={IncludedDatabases}",
                status.EffectiveMode,
                status.PrimaryDataSource,
                status.PrimaryDatabase,
                string.Join(",", status.IncludedDatabases));
        }
        finally
        {
            initializeLock.Release();
        }
    }

    private async Task<BillingCrossDatabaseSyncStatus> BuildStatusAsync(CancellationToken cancellationToken)
    {
        var warnings = new List<string>();
        var sameInstanceDatabases = new HashSet<string>(NameComparer);
        var crossInstanceDatabases = new HashSet<string>(NameComparer);

        var primaryConnection = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(primaryConnection))
        {
            return new BillingCrossDatabaseSyncStatus
            {
                EffectiveMode = "HospitalOnly",
                Warnings = ["Primary DefaultConnection is not configured."]
            };
        }

        SqlConnectionStringBuilder primaryBuilder;
        try
        {
            primaryBuilder = new SqlConnectionStringBuilder(primaryConnection);
        }
        catch
        {
            return new BillingCrossDatabaseSyncStatus
            {
                EffectiveMode = "HospitalOnly",
                Warnings = ["Primary DefaultConnection is invalid."]
            };
        }

        var primaryServerName = await TryGetServerNameAsync(primaryConnection, cancellationToken);
        if (!string.IsNullOrWhiteSpace(primaryServerName))
            warnings.Add($"Primary server verified as '{primaryServerName}'.");
        else
            warnings.Add("Could not verify primary SQL server name using @@SERVERNAME.");

        var primaryDataSource = NormalizeDataSource(primaryBuilder.DataSource);

        foreach (var key in SecondaryConnectionNames)
        {
            var candidateConnectionString = configuration.GetConnectionString(key);
            if (string.IsNullOrWhiteSpace(candidateConnectionString))
            {
                warnings.Add($"{key} is not configured; skipped.");
                continue;
            }

            SqlConnectionStringBuilder candidateBuilder;
            try
            {
                candidateBuilder = new SqlConnectionStringBuilder(candidateConnectionString);
            }
            catch
            {
                warnings.Add($"{key} is invalid; skipped.");
                continue;
            }

            if (string.IsNullOrWhiteSpace(candidateBuilder.InitialCatalog))
            {
                warnings.Add($"{key} has no database/catalog configured; skipped.");
                continue;
            }

            if (NameComparer.Equals(primaryBuilder.InitialCatalog, candidateBuilder.InitialCatalog))
            {
                warnings.Add($"{key} points to the same database as DefaultConnection; skipped.");
                continue;
            }

            var isDifferentServer = !NameComparer.Equals(primaryDataSource, NormalizeDataSource(candidateBuilder.DataSource));

            if (isDifferentServer)
            {
                // Cross-instance: verify with @@SERVERNAME if reachable
                var candidateServerName = await TryGetServerNameAsync(candidateConnectionString, cancellationToken);
                if (!string.IsNullOrWhiteSpace(primaryServerName) &&
                    !string.IsNullOrWhiteSpace(candidateServerName) &&
                    NameComparer.Equals(primaryServerName, candidateServerName))
                {
                    // DataSource string differed but @@SERVERNAME matched — treat as same instance
                    sameInstanceDatabases.Add(candidateBuilder.InitialCatalog);
                    warnings.Add($"{key} ({candidateBuilder.InitialCatalog}) resolved as same-instance via @@SERVERNAME.");
                }
                else
                {
                    // Genuinely on a different server — message bus path
                    crossInstanceDatabases.Add(candidateBuilder.InitialCatalog);
                    warnings.Add($"{key} ({candidateBuilder.InitialCatalog}) is on a different SQL Server instance — will use message bus sync.");
                }
            }
            else
            {
                // Same server/instance — atomic SQL path
                var candidateServerName = await TryGetServerNameAsync(candidateConnectionString, cancellationToken);
                if (!string.IsNullOrWhiteSpace(primaryServerName) &&
                    !string.IsNullOrWhiteSpace(candidateServerName) &&
                    !NameComparer.Equals(primaryServerName, candidateServerName))
                {
                    warnings.Add($"{key} server verification mismatch ({candidateServerName}); treated as cross-instance.");
                    crossInstanceDatabases.Add(candidateBuilder.InitialCatalog);
                }
                else
                {
                    sameInstanceDatabases.Add(candidateBuilder.InitialCatalog);
                }
            }
        }

        // Determine effective mode
        string effectiveMode;
        if (crossInstanceDatabases.Count > 0)
            effectiveMode = "MessageBusEventualSync";
        else if (sameInstanceDatabases.Count > 0)
            effectiveMode = "SameInstanceAtomicSync";
        else
            effectiveMode = "HospitalOnly";

        // IncludedDatabases = all secondaries regardless of path (for status reporting)
        var allIncluded = sameInstanceDatabases.Union(crossInstanceDatabases, NameComparer).ToList();

        return new BillingCrossDatabaseSyncStatus
        {
            EffectiveMode = effectiveMode,
            PrimaryDataSource = primaryBuilder.DataSource,
            PrimaryDatabase = primaryBuilder.InitialCatalog,
            IncludedDatabases = allIncluded,
            SameInstanceDatabases = sameInstanceDatabases.ToList(),
            CrossInstanceDatabases = crossInstanceDatabases.ToList(),
            Warnings = warnings
        };
    }

    private static async Task<string?> TryGetServerNameAsync(string connectionString, CancellationToken cancellationToken)
    {
        try
        {
            await using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync(cancellationToken);

            await using var command = connection.CreateCommand();
            command.CommandText = "SELECT CAST(@@SERVERNAME AS nvarchar(256))";

            var result = await command.ExecuteScalarAsync(cancellationToken);
            return result as string;
        }
        catch
        {
            return null;
        }
    }

    private static string NormalizeDataSource(string? dataSource)
    {
        return (dataSource ?? string.Empty).Trim();
    }
}
