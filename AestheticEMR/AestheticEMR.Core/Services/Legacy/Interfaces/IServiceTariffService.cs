using AestheticEMR.Core.Models.Legacy;

namespace AestheticEMR.Core.Services.Legacy.Interfaces;

public interface IServiceTariffService
{
    IEnumerable<VwCoyAndNhi> GetCompanies();
    IEnumerable<VwCoyAndNhi> GetCompaniesWithTariffs(string? category = null);
    Task<IEnumerable<VwServiceNhi>> GetAllAsync(string? coyId, string? searchText);
    Task<hServiceNHI?> GetByIdAsync(long sno);
    Task<hServiceNHI> CreateAsync(hServiceNHI serviceTariff);
    Task<hServiceNHI> UpdateAsync(hServiceNHI serviceTariff);
    Task DeleteAsync(long sno);
    Task<int> UploadAsync(string coyId, Stream fileStream, string fileName, bool deleteExisting, string? category = null, string? sheetName = null);
    Task<int> CopyFromCompanyAsync(string targetCoyId, string sourceCoyId, bool deleteExisting, string? category = null);
}
