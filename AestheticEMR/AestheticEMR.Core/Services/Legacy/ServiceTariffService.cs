using AestheticEMR.Core.Infrastructure;
using AestheticEMR.Core.Models.Legacy;
using AestheticEMR.Core.Services.Legacy.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Globalization;
using System.Text;
using ExcelDataReader;

namespace AestheticEMR.Core.Services.Legacy;

public class ServiceTariffService(ApplicationDbContext context) : IServiceTariffService
{
    public IEnumerable<VwCoyAndNhi> GetCompanies()
    {
        return context.VwCoyAndNhis
            .AsNoTracking()
            .OrderBy(x => x.Company)
            .ThenBy(x => x.CoyId)
            .ToList();
    }

    public async Task<IEnumerable<VwServiceNhi>> GetAllAsync(string? coyId, string? searchText)
    {
        var query = context.VwServiceNhis.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(coyId))
        {
            var normalizedCoyId = coyId.Trim();
            query = query.Where(x => x.CoyId == normalizedCoyId);
        }

        if (!string.IsNullOrWhiteSpace(searchText))
        {
            var term = searchText.Trim();
            query = query.Where(x => x.Service.Contains(term));
        }

        return await query
            .OrderBy(x => x.Service)
            .ThenBy(x => x.Category)
            .ToListAsync();
    }

    public async Task<hServiceNHI?> GetByIdAsync(long sno)
    {
        return await context.hServiceNHIs.FirstOrDefaultAsync(x => x.SNO == sno);
    }

    public async Task<hServiceNHI> CreateAsync(hServiceNHI serviceTariff)
    {
        NormalizeAndValidate(serviceTariff);
        await PopulateCompanyNameAsync(serviceTariff);

        context.hServiceNHIs.Add(serviceTariff);
        await context.SaveChangesAsync();
        return serviceTariff;
    }

    public async Task<hServiceNHI> UpdateAsync(hServiceNHI serviceTariff)
    {
        NormalizeAndValidate(serviceTariff);
        await PopulateCompanyNameAsync(serviceTariff);

        await context.SaveChangesAsync();
        return serviceTariff;
    }

    public async Task DeleteAsync(long sno)
    {
        var existing = await GetByIdAsync(sno);
        if (existing is null)
        {
            return;
        }

        context.hServiceNHIs.Remove(existing);
        await context.SaveChangesAsync();
    }

    public async Task<int> UploadAsync(string coyId, Stream fileStream, string fileName, bool deleteExisting)
    {
        var normalizedCoyId = (coyId ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(normalizedCoyId))
        {
            throw new InvalidOperationException("Company code is required.");
        }

        var extension = Path.GetExtension(fileName)?.ToLowerInvariant();
        if (extension is not ".csv" and not ".xlsx")
        {
            throw new InvalidOperationException("Only .csv and .xlsx files are supported.");
        }

        var retainership = await context.HRetainerships
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.RetainId == normalizedCoyId);

        var companyName = retainership?.RetainName ?? normalizedCoyId;

        if (deleteExisting)
        {
            var existing = context.hServiceNHIs.Where(x => x.Company == normalizedCoyId);
            context.hServiceNHIs.RemoveRange(existing);
            await context.SaveChangesAsync();
        }

        var items = extension == ".csv"
            ? ParseCsv(fileStream)
            : ParseXlsx(fileStream);

        var inserted = 0;

        foreach (var row in items)
        {
            var service = row.Service?.Trim();
            if (string.IsNullOrWhiteSpace(service))
            {
                continue;
            }

            var entity = new hServiceNHI
            {
                Service = service,
                Price = row.Price,
                Category = string.IsNullOrWhiteSpace(row.Category) ? null : row.Category.Trim(),
                Company = normalizedCoyId,
                CoyName = companyName,
                Remarks = "HMO",
                Capitated = "NO",
                TariffStatus = "FIXED"
            };

            NormalizeAndValidate(entity);
            context.hServiceNHIs.Add(entity);
            inserted++;
        }

        if (inserted > 0)
        {
            await context.SaveChangesAsync();
        }

        return inserted;
    }

    private static List<UploadRow> ParseCsv(Stream stream)
    {
        var rows = new List<UploadRow>();
        using var reader = new StreamReader(stream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true, leaveOpen: true);

        var lineNumber = 0;
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            lineNumber++;
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            var columns = line.Split(',');
            if (lineNumber == 1 && LooksLikeHeader(columns))
            {
                continue;
            }

            var service = columns.ElementAtOrDefault(0)?.Trim();
            var priceRaw = columns.ElementAtOrDefault(1)?.Trim();
            var category = columns.ElementAtOrDefault(2)?.Trim();

            if (string.IsNullOrWhiteSpace(service))
            {
                continue;
            }

            rows.Add(new UploadRow
            {
                Service = service,
                Price = ParsePrice(priceRaw),
                Category = category
            });
        }

        return rows;
    }

    private static List<UploadRow> ParseXlsx(Stream stream)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        using var excelReader = ExcelReaderFactory.CreateReader(stream);
        var result = excelReader.AsDataSet(new ExcelDataSetConfiguration
        {
            ConfigureDataTable = _ => new ExcelDataTableConfiguration
            {
                UseHeaderRow = false
            }
        });

        var table = result.Tables.Count > 0 ? result.Tables[0] : null;
        if (table is null)
        {
            return [];
        }

        var startRow = table.Rows.Count > 0 && LooksLikeHeader(table.Rows[0]) ? 1 : 0;
        var rows = new List<UploadRow>();

        for (var i = startRow; i < table.Rows.Count; i++)
        {
            var row = table.Rows[i];
            var service = row.ItemArray.ElementAtOrDefault(0)?.ToString()?.Trim();
            var priceRaw = row.ItemArray.ElementAtOrDefault(1)?.ToString()?.Trim();
            var category = row.ItemArray.ElementAtOrDefault(2)?.ToString()?.Trim();

            if (string.IsNullOrWhiteSpace(service))
            {
                continue;
            }

            rows.Add(new UploadRow
            {
                Service = service,
                Price = ParsePrice(priceRaw),
                Category = category
            });
        }

        return rows;
    }

    private static bool LooksLikeHeader(string[] columns)
    {
        var first = columns.ElementAtOrDefault(0)?.Trim().ToLowerInvariant() ?? string.Empty;
        var second = columns.ElementAtOrDefault(1)?.Trim().ToLowerInvariant() ?? string.Empty;
        return first.Contains("service") || second.Contains("price");
    }

    private static bool LooksLikeHeader(DataRow row)
    {
        var first = row.ItemArray.ElementAtOrDefault(0)?.ToString()?.Trim().ToLowerInvariant() ?? string.Empty;
        var second = row.ItemArray.ElementAtOrDefault(1)?.ToString()?.Trim().ToLowerInvariant() ?? string.Empty;
        return first.Contains("service") || second.Contains("price");
    }

    private static double ParsePrice(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
        {
            return 0;
        }

        var normalized = raw.Replace(",", string.Empty).Trim();
        return double.TryParse(normalized, NumberStyles.Any, CultureInfo.InvariantCulture, out var value)
            ? value
            : 0;
    }

    private static void NormalizeAndValidate(hServiceNHI serviceTariff)
    {
        serviceTariff.Service = (serviceTariff.Service ?? string.Empty).Trim();
        serviceTariff.Category = string.IsNullOrWhiteSpace(serviceTariff.Category) ? null : serviceTariff.Category.Trim();
        serviceTariff.Company = (serviceTariff.Company ?? string.Empty).Trim();
        serviceTariff.Remarks = string.IsNullOrWhiteSpace(serviceTariff.Remarks) ? null : serviceTariff.Remarks.Trim();
        serviceTariff.RevType = string.IsNullOrWhiteSpace(serviceTariff.RevType) ? null : serviceTariff.RevType.Trim();
        serviceTariff.Capitated = string.IsNullOrWhiteSpace(serviceTariff.Capitated) ? "NO" : serviceTariff.Capitated.Trim().ToUpperInvariant();
        serviceTariff.TariffStatus = string.IsNullOrWhiteSpace(serviceTariff.TariffStatus) ? "FIXED" : serviceTariff.TariffStatus.Trim().ToUpperInvariant();

        if (string.IsNullOrWhiteSpace(serviceTariff.Service))
        {
            throw new InvalidOperationException("Service name is required.");
        }

        if (string.IsNullOrWhiteSpace(serviceTariff.Company))
        {
            throw new InvalidOperationException("Company code is required.");
        }

        if (serviceTariff.Price is null || serviceTariff.Price < 0)
        {
            throw new InvalidOperationException("Price must be zero or greater.");
        }
    }

    private async Task PopulateCompanyNameAsync(hServiceNHI serviceTariff)
    {
        if (!string.IsNullOrWhiteSpace(serviceTariff.CoyName))
        {
            serviceTariff.CoyName = serviceTariff.CoyName.Trim();
            return;
        }

        var tariffCompany = await context.VwCoyAndNhis
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.CoyId == serviceTariff.Company);

        serviceTariff.CoyName = tariffCompany?.Company ?? serviceTariff.Company;
        serviceTariff.Remarks ??= tariffCompany?.Remarks ?? "HMO";
    }

    private sealed class UploadRow
    {
        public string Service { get; set; } = string.Empty;
        public double Price { get; set; }
        public string? Category { get; set; }
    }
}
