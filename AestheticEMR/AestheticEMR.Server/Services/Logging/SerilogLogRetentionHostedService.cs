using AestheticEMR.Core.Infrastructure;
using AestheticEMR.Server.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AestheticEMR.Server.Services.Logging
{
    public class SerilogLogRetentionHostedService(
        IServiceScopeFactory scopeFactory,
        IOptions<AppSettings> appSettings,
        ILogger<SerilogLogRetentionHostedService> logger) : BackgroundService
    {
        private readonly LogRetentionConfig _config = appSettings.Value.LogRetentionConfig ?? new LogRetentionConfig();

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!_config.Enabled)
            {
                logger.LogInformation("Serilog log retention cleanup is disabled by configuration.");
                return;
            }

            var retentionDays = Math.Max(1, _config.RetentionDays);
            var cleanupIntervalHours = Math.Max(1, _config.CleanupIntervalHours);

            logger.LogInformation(
                "Serilog log retention cleanup enabled. Keeping last {RetentionDays} days; cleanup interval {CleanupIntervalHours} hour(s).",
                retentionDays,
                cleanupIntervalHours);

            await CleanupOldLogsAsync(retentionDays, stoppingToken);

            using var timer = new PeriodicTimer(TimeSpan.FromHours(cleanupIntervalHours));
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                await CleanupOldLogsAsync(retentionDays, stoppingToken);
            }
        }

        private async Task CleanupOldLogsAsync(int retentionDays, CancellationToken cancellationToken)
        {
            try
            {
                var cutoffUtc = DateTime.UtcNow.AddDays(-retentionDays);

                using var scope = scopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var deletedRows = await dbContext.Database.ExecuteSqlInterpolatedAsync(
                    $"DELETE FROM [dbo].[Logs] WHERE [TimeStamp] < {cutoffUtc}",
                    cancellationToken);

                logger.LogInformation(
                    "Serilog log retention cleanup complete. Deleted {DeletedRows} rows older than {CutoffUtc:O}.",
                    deletedRows,
                    cutoffUtc);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to execute Serilog log retention cleanup.");
            }
        }
    }
}
