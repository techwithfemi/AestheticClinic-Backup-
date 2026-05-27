using AestheticEMR.Core.Infrastructure;
using AestheticEMR.Core.Services.Legacy.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json;

namespace AestheticEMR.Core.Services.Legacy;

public class EmrAppDefaultsService(
    IServiceScopeFactory scopeFactory,
    IHostEnvironment hostEnvironment,
    IConfiguration configuration) : IEmrAppDefaultsService
{
    private static readonly HashSet<string> AccountingDefaultKeys =
    [
        "AcctPeriodType",
        "FinYearStart",
        "FinYearClose",
        "FinPrdStart",
        "AcctPostLastPeriodCloseDate",
        "AcctPostBeyondCurrentCalendarMonth",
        "AcctPostBelowLastPeriodClose",
        "ACCTNo_SUSP_SALES",
        "ACCTNo_SUSP_EXPENSES",
        "ACCTNo_SUSP_ASSET",
        "ACCTNo_SUSP_LIABILITY",
        "ACCTNo_SUSP_EQUITY",
        "AcctNoSales_Return",
        "AcctNoPurchase_Return",
        "AcctNo_Sales_Discount",
        "Enforce_Lock_Bill_After_24hrs"
    ];

    private readonly SemaphoreSlim _syncLock = new(1, 1);
    private EmrAppDefaults? _cache;

    public async Task<EmrAppDefaults> GetAsync(CancellationToken cancellationToken = default)
    {
        if (_cache is not null)
        {
            return _cache;
        }

        return await ReloadAsync(cancellationToken);
    }

    public async Task<EmrAppDefaults> ReloadAsync(CancellationToken cancellationToken = default)
    {
        await _syncLock.WaitAsync(cancellationToken);
        try
        {
            var jsonPath = GetSnapshotPath();
            var fromSnapshot = await TryLoadSnapshotAsync(jsonPath, cancellationToken);
            if (fromSnapshot is not null)
            {
                _cache = fromSnapshot;
                return _cache;
            }

            var defaultsFromDatabase = await LoadFromDatabaseAsync(cancellationToken);

            if (!File.Exists(jsonPath))
            {
                await WriteJsonSnapshotAsync(defaultsFromDatabase, jsonPath, cancellationToken);
            }

            _cache = defaultsFromDatabase;
            return _cache;
        }
        finally
        {
            _syncLock.Release();
        }
    }

    private string GetSnapshotPath()
    {
        return Path.Combine(hostEnvironment.ContentRootPath, "emrAppDefaults.json");
    }

    private async Task<EmrAppDefaults?> TryLoadSnapshotAsync(string jsonPath, CancellationToken cancellationToken)
    {
        if (!File.Exists(jsonPath))
        {
            return null;
        }

        try
        {
            var json = await File.ReadAllTextAsync(jsonPath, cancellationToken);
            var snapshot = JsonSerializer.Deserialize<EmrAppDefaultsJsonSnapshot>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (snapshot is null)
            {
                return null;
            }

            var publicVariables = new Dictionary<string, string>(snapshot.PublicVariables ?? new Dictionary<string, string>(), StringComparer.OrdinalIgnoreCase);
            var values = new Dictionary<string, string>(snapshot.Values ?? new Dictionary<string, string>(), StringComparer.OrdinalIgnoreCase);

            var entryDate = DateOnly.TryParse(snapshot.EntryDate, out var parsedDate)
                ? parsedDate
                : DateOnly.FromDateTime(DateTime.Today);

            return new EmrAppDefaults
            {
                AppName = snapshot.AppName?.Trim() ?? "Billing",
                ClientCategoryPrivate = snapshot.ClientCategoryPrivate?.Trim() ?? "PRIVATE",
                EntryDate = entryDate,
                PriceColumnIndex = snapshot.PriceColumnIndex <= 0 ? 3 : snapshot.PriceColumnIndex,
                BillHead = GetOrDefault(publicVariables, "BillHead"),
                BillHead2 = GetOrDefault(publicVariables, "BillHead2"),
                BillHead3 = GetOrDefault(publicVariables, "BillHead3"),
                BillHead4 = GetOrDefault(publicVariables, "BillHead4"),
                LabHead = GetOrDefault(publicVariables, "LabHead"),
                LabHead2 = GetOrDefault(publicVariables, "LabHead2"),
                LabHead3 = GetOrDefault(publicVariables, "LabHead3"),
                LabAcctNo = GetOrDefault(publicVariables, "LabAcctNo"),
                Values = values
            };
        }
        catch
        {
            return null;
        }
    }

    private async Task<EmrAppDefaults> LoadFromDatabaseAsync(CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var values = await context.AppDefaults
            .AsNoTracking()
            .ToDictionaryAsync(
                x => x.Id.Trim(),
                x => (x.Idval ?? string.Empty).Trim(),
                StringComparer.OrdinalIgnoreCase,
                cancellationToken);

        await MergeAccountingDefaultsAsync(values, cancellationToken);

        ApplyDefaults(values);
        ValidateRequired(values);

        var billSetting = await context.AppSettings
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == "BILL", cancellationToken);

        var labSetting = await context.AppSettings
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == "LAB", cancellationToken);

        return new EmrAppDefaults
        {
            AppName = "Billing",
            ClientCategoryPrivate = "PRIVATE",
            EntryDate = DateOnly.FromDateTime(DateTime.Today),
            PriceColumnIndex = 3,
            BillHead = billSetting?.Idval?.Trim() ?? string.Empty,
            BillHead2 = billSetting?.Idval2?.Trim() ?? string.Empty,
            BillHead3 = billSetting?.Idval3?.Trim() ?? string.Empty,
            BillHead4 = billSetting?.Idval4?.Trim() ?? string.Empty,
            LabHead = labSetting?.Idval?.Trim() ?? string.Empty,
            LabHead2 = labSetting?.Idval2?.Trim() ?? string.Empty,
            LabHead3 = labSetting?.Idval3?.Trim() ?? string.Empty,
            LabAcctNo = labSetting?.Idval5?.Trim() ?? string.Empty,
            Values = new Dictionary<string, string>(values, StringComparer.OrdinalIgnoreCase)
        };
    }

    private async Task WriteJsonSnapshotAsync(EmrAppDefaults defaults, string jsonPath, CancellationToken cancellationToken)
    {
        // Remove path-like keys from Values
        var filteredValues = new Dictionary<string, string>(defaults.Values, StringComparer.OrdinalIgnoreCase);
        filteredValues.Remove("ACCPATH");
        filteredValues.Remove("Encr");

        var payload = new EmrAppDefaultsJsonSnapshot
        {
            AppName = defaults.AppName,
            ClientCategoryPrivate = defaults.ClientCategoryPrivate,
            EntryDate = defaults.EntryDate.ToString("yyyy-MM-dd"),
            PriceColumnIndex = defaults.PriceColumnIndex,
            PublicVariables = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["BillHead"] = defaults.BillHead,
                ["BillHead2"] = defaults.BillHead2,
                ["BillHead3"] = defaults.BillHead3,
                ["BillHead4"] = defaults.BillHead4,
                ["LabHead"] = defaults.LabHead,
                ["LabHead2"] = defaults.LabHead2,
                ["LabHead3"] = defaults.LabHead3,
                ["LabAcctNo"] = defaults.LabAcctNo
            },
            Values = filteredValues
        };

        var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        await File.WriteAllTextAsync(jsonPath, json, cancellationToken);
    }

    private static string GetOrDefault(IReadOnlyDictionary<string, string> source, string key)
    {
        return source.TryGetValue(key, out var value) ? value : string.Empty;
    }

    private sealed class EmrAppDefaultsJsonSnapshot
    {
        public string? AppName { get; set; }
        public string? ClientCategoryPrivate { get; set; }
        public string? EntryDate { get; set; }
        public int PriceColumnIndex { get; set; }
        public Dictionary<string, string>? PublicVariables { get; set; }
        public Dictionary<string, string>? Values { get; set; }
    }
// ...existing code...
