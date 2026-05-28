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

            ApplyConnectionStringDatabaseNames(values);

            var entryDate = !string.IsNullOrWhiteSpace(snapshot.EntryDate)
                ? (DateOnly.TryParse(snapshot.EntryDate, out var parsedDate) ? parsedDate : DateOnly.FromDateTime(DateTime.Today))
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
                Taxes = new TaxDefaults
                {
                    TaxName = snapshot.Taxes?.TaxName?.Trim() ?? "VAT",
                    Pcent = snapshot.Taxes?.Pcent ?? 0,
                    Desc = snapshot.Taxes?.Desc?.Trim() ?? string.Empty
                },
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
        ApplyConnectionStringDatabaseNames(values);
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
            Taxes = new TaxDefaults
            {
                TaxName = "VAT",
                Pcent = 0,
                Desc = "Value Added Tax"
            },
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
            Taxes = new TaxDefaultsSnapshot
            {
                TaxName = defaults.Taxes.TaxName,
                Pcent = defaults.Taxes.Pcent,
                Desc = defaults.Taxes.Desc
            },
            Values = filteredValues
        };

        var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        await File.WriteAllTextAsync(jsonPath, json, cancellationToken);
    }

    private void ApplyConnectionStringDatabaseNames(IDictionary<string, string> values)
    {
        void SetDatabaseName(string key, string? connectionString)
        {
            var databaseName = GetDatabaseName(connectionString);
            if (string.IsNullOrWhiteSpace(databaseName))
            {
                return;
            }

            values[key] = databaseName;
        }

        SetDatabaseName("DbName", configuration.GetConnectionString("DefaultConnection"));
        SetDatabaseName("DbName_Acct", configuration.GetConnectionString("AccountingConnection"));
    }

    private static string GetDatabaseName(string? connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return string.Empty;
        }

        var builder = new SqlConnectionStringBuilder(connectionString);
        return builder.InitialCatalog?.Trim() ?? string.Empty;
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
        public TaxDefaultsSnapshot? Taxes { get; set; }
        public Dictionary<string, string>? Values { get; set; }
    }

    private sealed class TaxDefaultsSnapshot
    {
        public string? TaxName { get; set; }
        public double Pcent { get; set; }
        public string? Desc { get; set; }
    }

    private async Task MergeAccountingDefaultsAsync(IDictionary<string, string> values, CancellationToken cancellationToken)
    {
        var accountingConnectionString = configuration.GetConnectionString("AccountingConnection");
        if (string.IsNullOrWhiteSpace(accountingConnectionString))
        {
            return;
        }

        await using var connection = new SqlConnection(accountingConnectionString);
        await connection.OpenAsync(cancellationToken);

        await using var command = connection.CreateCommand();
        command.CommandText = "SELECT [ID], [IDVal] FROM [AppDefaults]";

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            var key = reader.IsDBNull(0) ? string.Empty : reader.GetString(0).Trim();
            if (string.IsNullOrWhiteSpace(key) || !AccountingDefaultKeys.Contains(key))
            {
                continue;
            }

            var value = reader.IsDBNull(1) ? string.Empty : reader.GetString(1).Trim();
            values[key] = value;
        }
    }

    private static void ApplyDefaults(IDictionary<string, string> values)
    {
        void Ensure(string key, string defaultValue)
        {
            if (!values.TryGetValue(key, out var current) || string.IsNullOrWhiteSpace(current))
            {
                values[key] = defaultValue;
            }
        }
        Ensure("DBName", "Hospital");
        Ensure("DbName", "Hospital");
        Ensure("LockPrice", "YES");
        Ensure("RevTypeRpt", "NO");
        Ensure("AcctPostOn", "NO");
        Ensure("AcctPost_After_BillProcessing", "NO");
        Ensure("AcctPostBeyondCurrentCalendarMonth", "NO");
        Ensure("AcctPostBelowLastPeriodClose", "NO");
        Ensure("Rev_Type_Def", "DEPOSIT");
        Ensure("Rev_Type_Def_Desc", "DEPOSIT");
        Ensure("AUTO_TRAN_NO", "YES");
        Ensure("AcctPostType_Expenses_PostAfterPayment", "YES");
        Ensure("FinYear_Create_StartDate", "01-Jan-21");
        Ensure("Use_Receivable_Invoice_Value_For_Payment_Manual_Tick", "NO");
        Ensure("Use_Receivable_Invoice_Value_For_Payment", "YES");
        Ensure("Use_Receivable_Invoice_Value_For_Payment_Start_Date", values["FinYear_Create_StartDate"]);
        Ensure("Use_Payable_Invoice_Value_For_Voucher", "YES");
        Ensure("Use_Payable_Invoice_Value_For_Voucher_Start_Date", values["FinYear_Create_StartDate"]);
        Ensure("NHISFEE", "ALL");
        Ensure("OLDPNo", "NO");
        Ensure("BillTo", "0001");
        Ensure("Coy_CODE", "SHORT");
        Ensure("CoyID", "0001");
        Ensure("LockOldVersion", "NO");
        Ensure("LockDiscount", "YES");
        Ensure("LockDebt", "NO");
        Ensure("TranxStartDateForDebt", "01-Jan-20");
        Ensure("AppLocation", "Lagos");
        Ensure("AutoUpdateTariff", "NO");
        Ensure("CallDefaults", "HOURLY");
        Ensure("shutDown", "NO");
        Ensure("RevType_Drug", "TREATMENT");
        Ensure("RevType_Misc", "PROFESSIONAL FEE");
        Ensure("LockSignedBill", "YES");
        Ensure("Capitate_NHIS_ONLY", "NO");
        Ensure("Split_NHIS_Bill_For_Payment", "YES");
        Ensure("Enforce_Saving_In_Collate_Bill", "NO");
        Ensure("Has_Bill_End_Date", "NO");
        Ensure("RevType_Prof_Fee", "PROF FEE");
        Ensure("RevType_NHIS_Fee", "NHIS FEE");
        Ensure("Voucher_ByPass_For_Refund", "NO");
        Ensure("Use_Clinic_Logo_For_Invoice", "NO");
        Ensure("Private_Client_Only", "NO");
        Ensure("Has_BarCode", "NO");
        Ensure("Barcode_Length", "13");
        Ensure("Print_From_Small_Printer", "YES");
        Ensure("Print_From_Small_Printer_With_Preview", "YES");
        Ensure("POS_Enabled", "YES");
        Ensure("POS_Auto_Print", "YES");
        Ensure("POS_PayType_Default_Cash", "YES");
        Ensure("POS_No_Debt_Allowed", "YES");
        Ensure("POS_Use_Input_Box_For_Qty", "YES");
        Ensure("AcctPostOn_Consumables", "NO");
        Ensure("StartYear", (DateTime.Today.Year - 1).ToString());
        Ensure("Set_Lock_Down", "YES");
        Ensure("Set_Lock_Down_Prd_Interval_In_Mths", "3");
        Ensure("Tel_contact_No", "234-803-345-2113, 234-909-756-1272");
        Ensure("Mail_Activated", "No");
        Ensure("Mail_Server", "mail5005.smarterasp.net");
        Ensure("Mail_UserFrom", "noreply@logicversiononline.com");
        Ensure("Mail_Password", "logic@123");
        Ensure("Mail_SmtpPort", "8889");
        Ensure("Mail_Subject", "Client Mail");
        Ensure("Enforce_Lock_Bill_After_24hrs", "NO");
        Ensure("SearchValue", "20");
        Ensure("Print_Prescription", "NO");
    }

    private static void ValidateRequired(IReadOnlyDictionary<string, string> values)
    {
        void Require(string key, string message)
        {
            if (!values.TryGetValue(key, out var value) || string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidOperationException(message);
            }
        }
        void RequireNonZeroInteger(string key, string message)
        {
            if (!values.TryGetValue(key, out var value)
                || !int.TryParse(value, out var parsed)
                || parsed == 0)
            {
                throw new InvalidOperationException(message);
            }
        }
        Require("PRIVATE", "System Default Value needed for Private Tariff");
        Require("App_Name", "App_Name needed for this Module");
        if (values.TryGetValue("Set_Lock_Down", out var lockDown) && lockDown.Trim().Equals("YES", StringComparison.OrdinalIgnoreCase))
        {
            Require("Set_Lock_Down_Next_Date", "Expiry_Date needed for this Module");
        }
        if (values.TryGetValue("AcctPostOn", out var acctPostOn) && acctPostOn.Trim().Equals("YES", StringComparison.OrdinalIgnoreCase))
        {
            Require("AcctPeriodType", "AcctPeriodType needed for Posting");
            Require("AcctPostType", "AcctPostType needed for Posting");
            Require("FinYearStart", "FinYearStart needed for Posting");
            Require("FinYearClose", "FinYearClose needed for Posting");
            Require("FinPrdStart", "FinPrdStart needed for Posting");
            Require("AcctPostLastPeriodCloseDate", "AcctPostLastPeriodCloseDate needed for Posting");
            Require("ACCTNo_SUSP_SALES", "ACCTNo_SUSP_SALES missing");
            Require("ACCTNo_SUSP_EXPENSES", "ACCTNo_SUSP_EXPENSES missing");
            Require("ACCTNo_SUSP_ASSET", "ACCTNo_SUSP_ASSET missing");
            Require("ACCTNo_SUSP_LIABILITY", "ACCTNo_SUSP_LIABILITY missing");
            Require("ACCTNo_SUSP_EQUITY", "ACCTNo_SUSP_EQUITY missing");
            Require("AcctNoSales_Return", "AcctNoSales_Return needed for Posting");
            Require("AcctNoPurchase_Return", "AcctNoPurchase_Return needed for Posting");
            Require("AcctNo_Sales_Discount", "AcctNo_Sales_Discount needed for Posting");
            Require("AcctNo_COGS", "AcctNo_COGS_Lab Acct  Required");
            Require("AcctNo_Inventory_Lab", "AcctNo_Inventory_Lab Acct  Required");
            Require("AcctCostCenter", "AcctCostCenter needed for Posting");
            Require("AcctNoPOS", "POS Acct No needed for Posting");
            Require("AcctNoCheque", "Cheque Acct No needed for Posting");
            Require("AcctNoTransfer", "Transfer Acct No needed for Posting");
            Require("AcctNoCash", "Cash Acct No needed for Posting");
            Require("AcctNo_PettyCash", "Petty Cash Acct No needed for Posting");
            Require("Acct_Banks", "Acct_Banks Acct Group for Banks Required");
            Require("Acct_Cash", "Acct_Cash Acct Group for Cash Required");
            Require("Acct_Revenue", "Acct_Revenue Acct Group Required");
            Require("Acct_Expenses", "Acct_Expenses Acct Group Required");
            Require("Acct_Inventory_Purchase", "Acct_Inventory_Purchase Acct Group Required");
            Require("Acct_Payable", "Acct_Payable Acct Group Required");
            Require("Acct_Receivable", "Acct_Receivable Acct Group Required");
            Require("AcctPostType_Cash", "AcctPostType_Cash Required");
            Require("AcctPostType_COGS", "AcctPostType_COGS Required");
            Require("AcctPostType_Expenses", "AcctPostType_Expenses Required");
            Require("AcctPostType_Inventory_Purchase", "AcctPostType_Inventory_Purchase Required");
            Require("AcctPostType_Payable", "AcctPostType_Payable Required");
            Require("AcctPostType_Receivable", "AcctPostType_Receivable Required");
            Require("AcctPostType_Revenue_Cash", "AcctPostType_Revenue_Cash Required");
            Require("AcctNo_Inventory_Pharmacy", "AcctNo_Inventory_Pharmacy Acct  Required");
            Require("CashAccountIndex", "CashAccountIndex needed for Posting");
            Require("ARAccountIndex", "Cash Acct Index No needed for Posting");
            Require("AcctNoSales", "Sales Acct No needed for Posting");
            Require("AcctNoSalesInv", "Sales Invoice Acct No needed for Posting");
            RequireNonZeroInteger("CashAccountIndex", "Cash Acct Index No needed for Posting");
            RequireNonZeroInteger("ARAccountIndex", "ARAccountIndex needed for Posting");
        }
    }
}
