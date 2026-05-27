using AestheticEMR.Core.Services.Legacy;

namespace AestheticEMR.Core.Services.Legacy.Interfaces;

public interface IEmrAppDefaultsService
{
    Task<EmrAppDefaults> GetAsync(CancellationToken cancellationToken = default);
    Task<EmrAppDefaults> ReloadAsync(CancellationToken cancellationToken = default);
}
