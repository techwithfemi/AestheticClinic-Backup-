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
    private static readonly IReadOnlyList<string> SecondaryConnectionNames = ["SmartHRConnection", "AccountingConnection"];

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
        var includedDatabases = new HashSet<string>(NameComparer);

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
        {
            warnings.Add($"Primary server verified as '{primaryServerName}'.");
        }
        else
        {
            warnings.Add("Could not verify primary SQL server name using @@SERVERNAME.");
        }

        var primaryDataSource = NormalizeDataSource(primaryBuilder.DataSource);

        foreach (var key in SecondaryConnectionNames)
        {
            var candidateConnectionString = configuration.GetConnectionString(key);
            if (string.IsNullOrWhiteSpace(candidateConnectionString))
            {
                warnings.Add($"{key} is not configured.");
                continue;
            }

            SqlConnectionStringBuilder candidateBuilder;
            try
            {
                candidateBuilder = new SqlConnectionStringBuilder(candidateConnectionString);
            }
            catch
            {
                warnings.Add($"{key} is invalid and was skipped.");
                continue;
            }

            if (!NameComparer.Equals(primaryDataSource, NormalizeDataSource(candidateBuilder.DataSource)))
            {
                warnings.Add($"{key} points to a different SQL Server instance and was skipped.");
                continue;
            }

            if (string.IsNullOrWhiteSpace(candidateBuilder.InitialCatalog))
            {
                warnings.Add($"{key} has no database/catalog configured and was skipped.");
                continue;
            }

            if (NameComparer.Equals(primaryBuilder.InitialCatalog, candidateBuilder.InitialCatalog))
            {
                warnings.Add($"{key} points to the same database as DefaultConnection and was skipped.");
                continue;
            }

            var candidateServerName = await TryGetServerNameAsync(candidateConnectionString, cancellationToken);
            if (!string.IsNullOrWhiteSpace(primaryServerName) &&
                !string.IsNullOrWhiteSpace(candidateServerName) &&
                !NameComparer.Equals(primaryServerName, candidateServerName))
            {
                warnings.Add($"{key} server verification mismatch ({candidateServerName}) and was skipped.");
                continue;
            }

            includedDatabases.Add(candidateBuilder.InitialCatalog);
        }

        return new BillingCrossDatabaseSyncStatus
        {
            EffectiveMode = includedDatabases.Count > 0 ? "SameInstanceAtomicSync" : "HospitalOnly",
            PrimaryDataSource = primaryBuilder.DataSource,
            PrimaryDatabase = primaryBuilder.InitialCatalog,
            IncludedDatabases = includedDatabases.ToList(),
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
