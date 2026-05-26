using AestheticEMR.Core.Services.Legacy.Interfaces;
using Microsoft.Extensions.Hosting;

namespace AestheticEMR.Server.Services;

public class BillingAppDefaultsStartupService(
    IServiceScopeFactory scopeFactory,
    ILogger<BillingAppDefaultsStartupService> logger) : IHostedService
{
    public bool Loaded { get; private set; }
    public string? LastError { get; private set; }
    public DateTimeOffset? LastCheckedAtUtc { get; private set; }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        var defaultsService = scope.ServiceProvider.GetRequiredService<IBillingAppDefaultsService>();

        try
        {
            var defaults = await defaultsService.ReloadAsync(cancellationToken);
            Loaded = true;
            LastError = null;
            LastCheckedAtUtc = DateTimeOffset.UtcNow;

            logger.LogInformation(
                "Billing defaults loaded at startup. App={AppName}, EntryDate={EntryDate}, DefaultCount={Count}",
                defaults.AppName,
                defaults.EntryDate,
                defaults.Values.Count);
        }
        catch (Exception ex)
        {
            Loaded = false;
            LastError = ex.GetBaseException().Message;
            LastCheckedAtUtc = DateTimeOffset.UtcNow;
            logger.LogError(ex, "Billing defaults failed to load at startup");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
