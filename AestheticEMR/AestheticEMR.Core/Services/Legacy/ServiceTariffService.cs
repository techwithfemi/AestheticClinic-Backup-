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

    public IEnumerable<VwCoyAndNhi> GetCompaniesWithTariffs(string? category = null)
    {
        var cat = category?.Trim().ToUpperInvariant();

        // Query the correct table based on category to return companies that have data
        if (cat == "DRUG")
        {
            return context.DrugNhis
                .AsNoTracking()
                .GroupBy(x => new { x.Company, x.CoyName })
                .Select(x => new VwCoyAndNhi
                {
                    CoyId = x.Key.Company,
                    Company = x.Key.CoyName ?? x.Key.Company,
                    Remarks = "HMO"
                })
                .OrderBy(x => x.Company)
                .ThenBy(x => x.CoyId)
                .ToList();
        }

        if (cat == "INVESTIGATION")
        {
            return context.LabServiceNhis
                .AsNoTracking()
                .GroupBy(x => new { x.Company, x.CoyName })
                .Select(x => new VwCoyAndNhi
                {
                    CoyId = x.Key.Company,
                    Company = x.Key.CoyName ?? x.Key.Company,
                    Remarks = "HMO"
                })
                .OrderBy(x => x.Company)
                .ThenBy(x => x.CoyId)
                .ToList();
        }

        if (cat == "PRODUCT")
        {
            return context.ProductTariffs
                .AsNoTracking()
                .GroupBy(x => new { x.Company, x.CoyName })
                .Select(x => new VwCoyAndNhi
                {
                    CoyId = x.Key.Company,
                    Company = x.Key.CoyName ?? x.Key.Company,
                    Remarks = "HMO"
                })
                .OrderBy(x => x.Company)
                .ThenBy(x => x.CoyId)
                .ToList();
        }

        // Default: SERVICE â€” read from VwServiceNhis
        return context.VwServiceNhis
            .AsNoTracking()
            .GroupBy(x => new { x.CoyId, x.Company, x.Remarks })
            .Select(x => new VwCoyAndNhi
            {
                CoyId = x.Key.CoyId,
                Company = x.Key.Company,
                Remarks = x.Key.Remarks ?? string.Empty
            })
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

    public async Task<int> UploadAsync(string coyId, Stream fileStream, string fileName, bool deleteExisting, string? category = null, string? sheetName = null)
    {
        var normalizedCoyId = (coyId ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(normalizedCoyId))
            throw new InvalidOperationException("Company code is required.");

        var extension = Path.GetExtension(fileName)?.ToLowerInvariant();
        if (extension is not ".csv" and not ".xls" and not ".xlsx")
            throw new InvalidOperationException("Only .xls, .xlsx and .csv files are supported.");

        var cat = category?.Trim().ToUpperInvariant() ?? "SERVICE";
        var companyName = await ResolveCompanyNameAsync(normalizedCoyId);

        var items = extension == ".csv" ? ParseCsv(fileStream) : ParseXlsx(fileStream, sheetName);

        if (cat == "DRUG")
            return await UploadDrugAsync(normalizedCoyId, companyName, items, deleteExisting);

        if (cat == "INVESTIGATION")
            return await UploadInvestigationAsync(normalizedCoyId, companyName, items, deleteExisting);

        if (cat == "PRODUCT")
            return await UploadProductAsync(normalizedCoyId, companyName, items, deleteExisting);

        // SERVICE (default)
        return await UploadServiceAsync(normalizedCoyId, companyName, items, deleteExisting);
    }

    private async Task<int> UploadDrugAsync(string coyId, string companyName, List<UploadRow> items, bool deleteExisting)
    {
        if (deleteExisting)
        {
            context.DrugNhis.RemoveRange(context.DrugNhis.Where(x => x.Company == coyId));
            await context.SaveChangesAsync();
        }

        var inserted = 0;
        foreach (var row in items)
        {
            var name = row.Service?.Trim();
            if (string.IsNullOrWhiteSpace(name)) continue;

            context.DrugNhis.Add(new DrugNhi
            {
                DrgName = name,
                Company = coyId,
                CoyName = companyName,
                Price = row.Price,
                Remarks = "HMO",
                Capitated = "NO",
                TariffStatus = "FIXED"
            });
            inserted++;
        }

        if (inserted > 0) await context.SaveChangesAsync();
        return inserted;
    }

    private async Task<int> UploadInvestigationAsync(string coyId, string companyName, List<UploadRow> items, bool deleteExisting)
    {
        if (deleteExisting)
        {
            context.LabServiceNhis.RemoveRange(context.LabServiceNhis.Where(x => x.Company == coyId));
            await context.SaveChangesAsync();
        }

        var inserted = 0;
        foreach (var row in items)
        {
            var name = row.Service?.Trim();
            if (string.IsNullOrWhiteSpace(name)) continue;

            context.LabServiceNhis.Add(new LabServiceNhi
            {
                DrgName = name,
                Company = coyId,
                CoyName = companyName,
                Price = row.Price,
                Remarks = "HMO",
                Capitated = "NO",
                TariffStatus = "FIXED"
            });
            inserted++;
        }

        if (inserted > 0) await context.SaveChangesAsync();
        return inserted;
    }

    private async Task<int> UploadProductAsync(string coyId, string companyName, List<UploadRow> items, bool deleteExisting)
    {
        if (deleteExisting)
        {
            context.ProductTariffs.RemoveRange(context.ProductTariffs.Where(x => x.Company == coyId));
            await context.SaveChangesAsync();
        }

        var inserted = 0;
        foreach (var row in items)
        {
            var name = row.Service?.Trim();
            if (string.IsNullOrWhiteSpace(name)) continue;

            context.ProductTariffs.Add(new ProductTariff
            {
                PdtName = name,
                Company = coyId,
                CoyName = companyName,
                Price = row.Price,
                Remarks = "HMO",
                Capitated = "NO",
                TariffStatus = "FIXED"
            });
            inserted++;
        }

        if (inserted > 0) await context.SaveChangesAsync();
        return inserted;
    }

    private async Task<int> UploadServiceAsync(string coyId, string companyName, List<UploadRow> items, bool deleteExisting)
    {
        if (deleteExisting)
        {
            context.hServiceNHIs.RemoveRange(context.hServiceNHIs.Where(x => x.Company == coyId));
            await context.SaveChangesAsync();
        }

        var inserted = 0;
        foreach (var row in items)
        {
            var name = row.Service?.Trim();
            if (string.IsNullOrWhiteSpace(name)) continue;

            var entity = new hServiceNHI
            {
                Service = name,
                Price = row.Price,
                Category = string.IsNullOrWhiteSpace(row.Category) ? null : row.Category.Trim(),
                Company = coyId,
                CoyName = companyName,
                Remarks = "HMO",
                Capitated = "NO",
                TariffStatus = "FIXED",
                UsersCat = "SERVICE"
            };
            NormalizeAndValidate(entity);
            context.hServiceNHIs.Add(entity);
            inserted++;
        }

        if (inserted > 0) await context.SaveChangesAsync();
        return inserted;
    }

    public async Task<int> CopyFromCompanyAsync(string targetCoyId, string sourceCoyId, bool deleteExisting, string? category = null)
    {
        var normalizedTargetCoyId = (targetCoyId ?? string.Empty).Trim();
        var normalizedSourceCoyId = (sourceCoyId ?? string.Empty).Trim();

        if (string.IsNullOrWhiteSpace(normalizedTargetCoyId))
            throw new InvalidOperationException("Target company code is required.");
        if (string.IsNullOrWhiteSpace(normalizedSourceCoyId))
            throw new InvalidOperationException("Source company code is required.");
        if (string.Equals(normalizedTargetCoyId, normalizedSourceCoyId, StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("Source and target company cannot be the same.");

        var cat = category?.Trim().ToUpperInvariant() ?? "SERVICE";
        var targetCompanyName = await ResolveCompanyNameAsync(normalizedTargetCoyId);

        if (cat == "DRUG")
            return await CopyDrugAsync(normalizedTargetCoyId, normalizedSourceCoyId, targetCompanyName, deleteExisting);
        if (cat == "INVESTIGATION")
            return await CopyInvestigationAsync(normalizedTargetCoyId, normalizedSourceCoyId, targetCompanyName, deleteExisting);
        if (cat == "PRODUCT")
            return await CopyProductAsync(normalizedTargetCoyId, normalizedSourceCoyId, targetCompanyName, deleteExisting);

        return await CopyServiceAsync(normalizedTargetCoyId, normalizedSourceCoyId, targetCompanyName, deleteExisting);
    }

    private async Task<int> CopyDrugAsync(string targetCoyId, string sourceCoyId, string targetCompanyName, bool deleteExisting)
    {
        var source = await context.DrugNhis.AsNoTracking().Where(x => x.Company == sourceCoyId).ToListAsync();
        if (source.Count == 0)
            throw new InvalidOperationException("The selected source company has no Drug tariff records.");

        if (deleteExisting)
        {
            context.DrugNhis.RemoveRange(context.DrugNhis.Where(x => x.Company == targetCoyId));
            await context.SaveChangesAsync();
        }

        foreach (var item in source)
        {
            context.DrugNhis.Add(new DrugNhi
            {
                DrgName = item.DrgName,
                Company = targetCoyId,
                CoyName = targetCompanyName,
                Price = item.Price,
                Remarks = item.Remarks ?? "HMO",
                Capitated = item.Capitated ?? "NO",
                TariffStatus = item.TariffStatus ?? "FIXED",
                RevType = item.RevType,
                PharmCat = item.PharmCat,
                DrgCatName = item.DrgCatName
            });
        }

        await context.SaveChangesAsync();
        return source.Count;
    }

    private async Task<int> CopyInvestigationAsync(string targetCoyId, string sourceCoyId, string targetCompanyName, bool deleteExisting)
    {
        var source = await context.LabServiceNhis.AsNoTracking().Where(x => x.Company == sourceCoyId).ToListAsync();
        if (source.Count == 0)
            throw new InvalidOperationException("The selected source company has no Investigation tariff records.");

        if (deleteExisting)
        {
            context.LabServiceNhis.RemoveRange(context.LabServiceNhis.Where(x => x.Company == targetCoyId));
            await context.SaveChangesAsync();
        }

        foreach (var item in source)
        {
            context.LabServiceNhis.Add(new LabServiceNhi
            {
                DrgName = item.DrgName,
                Company = targetCoyId,
                CoyName = targetCompanyName,
                Price = item.Price,
                Remarks = item.Remarks ?? "HMO",
                Capitated = item.Capitated ?? "NO",
                TariffStatus = item.TariffStatus ?? "FIXED",
                RevType = item.RevType,
                DrgCatName = item.DrgCatName
            });
        }

        await context.SaveChangesAsync();
        return source.Count;
    }

    private async Task<int> CopyProductAsync(string targetCoyId, string sourceCoyId, string targetCompanyName, bool deleteExisting)
    {
        var source = await context.ProductTariffs.AsNoTracking().Where(x => x.Company == sourceCoyId).ToListAsync();
        if (source.Count == 0)
            throw new InvalidOperationException("The selected source company has no Product tariff records.");

        if (deleteExisting)
        {
            context.ProductTariffs.RemoveRange(context.ProductTariffs.Where(x => x.Company == targetCoyId));
            await context.SaveChangesAsync();
        }

        foreach (var item in source)
        {
            context.ProductTariffs.Add(new ProductTariff
            {
                PdtName = item.PdtName,
                Company = targetCoyId,
                CoyName = targetCompanyName,
                Price = item.Price,
                Remarks = item.Remarks ?? "HMO",
                Capitated = item.Capitated ?? "NO",
                TariffStatus = item.TariffStatus ?? "FIXED",
                RevType = item.RevType,
                Category = item.Category,
                UsersCat = item.UsersCat
            });
        }

        await context.SaveChangesAsync();
        return source.Count;
    }

    private async Task<int> CopyServiceAsync(string targetCoyId, string sourceCoyId, string targetCompanyName, bool deleteExisting)
    {
        var source = await context.hServiceNHIs.AsNoTracking().Where(x => x.Company == sourceCoyId).ToListAsync();
        if (source.Count == 0)
            throw new InvalidOperationException("The selected source company has no Service tariff records.");

        if (deleteExisting)
        {
            context.hServiceNHIs.RemoveRange(context.hServiceNHIs.Where(x => x.Company == targetCoyId));
            await context.SaveChangesAsync();
        }

        foreach (var item in source)
        {
            context.hServiceNHIs.Add(new hServiceNHI
            {
                Service = item.Service,
                Category = item.Category,
                Company = targetCoyId,
                Price = item.Price,
                Remarks = item.Remarks ?? "HMO",
                CoyName = targetCompanyName,
                Capitated = item.Capitated ?? "NO",
                TariffStatus = item.TariffStatus ?? "FIXED",
                RevType = item.RevType,
                UsersCat = item.UsersCat ?? "SERVICE"
            });
        }

        await context.SaveChangesAsync();
        return source.Count;
    }

    private async Task<string> ResolveCompanyNameAsync(string coyId)
    {
        var retainership = await context.HRetainerships
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.RetainId == coyId);
        return retainership?.RetainName ?? coyId;
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

    private static List<UploadRow> ParseXlsx(Stream stream, string? sheetName)
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

        DataTable? table = null;
        if (!string.IsNullOrWhiteSpace(sheetName))
        {
            table = result.Tables.Cast<DataTable>().FirstOrDefault(x => x.TableName.Equals(sheetName, StringComparison.OrdinalIgnoreCase));
        }

        table ??= result.Tables.Count > 0 ? result.Tables[0] : null;
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
