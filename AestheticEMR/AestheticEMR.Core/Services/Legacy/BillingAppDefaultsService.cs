using AestheticEMR.Core.Infrastructure;
using AestheticEMR.Core.Services.Legacy.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AestheticEMR.Core.Services.Legacy;

public class BillingAppDefaultsService(
    IServiceScopeFactory scopeFactory,
    IHostEnvironment hostEnvironment,
    IConfiguration configuration) : IBillingAppDefaultsService
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
    private BillingAppDefaults? _cache;

    public async Task<BillingAppDefaults> GetAsync(CancellationToken cancellationToken = default)
    {
        if (_cache is not null)
        {
            return _cache;
        }

        return await ReloadAsync(cancellationToken);
    }

    public async Task<BillingAppDefaults> ReloadAsync(CancellationToken cancellationToken = default)
    {
        await _syncLock.WaitAsync(cancellationToken);
        try
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

            var appName = "Billing";
            var mailAttachmentDirectory = Path.Combine(hostEnvironment.ContentRootPath, "Mail_Attachment", appName);
            Directory.CreateDirectory(mailAttachmentDirectory);

            var defaultPrinter = await GetDefaultPrinterAsync(context, cancellationToken);

            _cache = new BillingAppDefaults
            {
                AppName = appName,
                ClientCategoryPrivate = "PRIVATE",
                EntryDate = DateOnly.FromDateTime(DateTime.Today),
                PriceColumnIndex = 3,
                MailAttachmentDirectory = mailAttachmentDirectory,
                BillHead = billSetting?.Idval?.Trim() ?? string.Empty,
                BillHead2 = billSetting?.Idval2?.Trim() ?? string.Empty,
                BillHead3 = billSetting?.Idval3?.Trim() ?? string.Empty,
                BillHead4 = billSetting?.Idval4?.Trim() ?? string.Empty,
                BillPixPath = Path.Combine(hostEnvironment.ContentRootPath, "RctPix.JPG"),
                BillPixPath2 = Path.Combine(hostEnvironment.ContentRootPath, "RctPixLab.JPG"),
                BillPixPath3 = Path.Combine(hostEnvironment.ContentRootPath, "RctPixDen.JPG"),
                LabHead = labSetting?.Idval?.Trim() ?? string.Empty,
                LabHead2 = labSetting?.Idval2?.Trim() ?? string.Empty,
                LabHead3 = labSetting?.Idval3?.Trim() ?? string.Empty,
                LabAcctNo = labSetting?.Idval5?.Trim() ?? string.Empty,
                LabPixPath = Path.Combine(hostEnvironment.ContentRootPath, "RctPix.JPG"),
                DefaultPrinter = defaultPrinter,
                Values = new Dictionary<string, string>(values, StringComparer.OrdinalIgnoreCase)
            };

            return _cache;
        }
        finally
        {
            _syncLock.Release();
        }
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
        Ensure(values, "DBName", "Hospital");
        Ensure(values, "DbName", "Hospital");
        Ensure(values, "LockPrice", "YES");
        Ensure(values, "RevTypeRpt", "NO");
        Ensure(values, "AcctPostOn", "NO");
        Ensure(values, "AcctPost_After_BillProcessing", "NO");
        Ensure(values, "AcctPostBeyondCurrentCalendarMonth", "NO");
        Ensure(values, "AcctPostBelowLastPeriodClose", "NO");
        Ensure(values, "Rev_Type_Def", "DEPOSIT");
        Ensure(values, "Rev_Type_Def_Desc", "DEPOSIT");
        Ensure(values, "AUTO_TRAN_NO", "YES");
        Ensure(values, "AcctPostType_Expenses_PostAfterPayment", "YES");
        Ensure(values, "FinYear_Create_StartDate", "01-Jan-21");
        Ensure(values, "Use_Receivable_Invoice_Value_For_Payment_Manual_Tick", "NO");
        Ensure(values, "Use_Receivable_Invoice_Value_For_Payment", "YES");
        Ensure(values, "Use_Receivable_Invoice_Value_For_Payment_Start_Date", values["FinYear_Create_StartDate"]);
        Ensure(values, "Use_Payable_Invoice_Value_For_Voucher", "YES");
        Ensure(values, "Use_Payable_Invoice_Value_For_Voucher_Start_Date", values["FinYear_Create_StartDate"]);
        Ensure(values, "NHISFEE", "ALL");
        Ensure(values, "OLDPNo", "NO");
        Ensure(values, "BillTo", "0001");
        Ensure(values, "Coy_Code", "SHORT");
        Ensure(values, "CoyID", "0001");
        Ensure(values, "LockOldVersion", "NO");
        Ensure(values, "LockDiscount", "YES");
        Ensure(values, "LockDebt", "NO");
        Ensure(values, "TranxStartDateForDebt", "01-Jan-20");
        Ensure(values, "AppLocation", "Lagos");
        Ensure(values, "AutoUpdateTariff", "NO");
        Ensure(values, "CallDefaults", "HOURLY");
        Ensure(values, "shutDown", "NO");
        Ensure(values, "RevType_Drug", "TREATMENT");
        Ensure(values, "RevType_Misc", "PROFESSIONAL FEE");
        Ensure(values, "LockSignedBill", "YES");
        Ensure(values, "Capitate_NHIS_ONLY", "NO");
        Ensure(values, "Split_NHIS_Bill_For_Payment", "YES");
        Ensure(values, "Enforce_Saving_In_Collate_Bill", "NO");
        Ensure(values, "Has_Bill_End_Date", "NO");
        Ensure(values, "RevType_Prof_Fee", "PROF FEE");
        Ensure(values, "RevType_NHIS_Fee", "NHIS FEE");
        Ensure(values, "Voucher_ByPass_For_Refund", "NO");
        Ensure(values, "Use_Clinic_Logo_For_Invoice", "NO");
        Ensure(values, "Private_Client_Only", "NO");
        Ensure(values, "Has_BarCode", "NO");
        Ensure(values, "Barcode_Length", "13");
        Ensure(values, "Print_From_Small_Printer", "YES");
        Ensure(values, "Print_From_Small_Printer_With_Preview", "YES");
        Ensure(values, "POS_Enabled", "YES");
        Ensure(values, "POS_Auto_Print", "YES");
        Ensure(values, "POS_PayType_Default_Cash", "YES");
        Ensure(values, "POS_No_Debt_Allowed", "YES");
        Ensure(values, "POS_Use_Input_Box_For_Qty", "YES");
        Ensure(values, "AcctPostOn_Consumables", "NO");
        Ensure(values, "StartYear", (DateTime.Today.Year - 1).ToString());
        Ensure(values, "Set_Lock_Down", "YES");
        Ensure(values, "Set_Lock_Down_Prd_Interval_In_Mths", "3");
        Ensure(values, "Tel_contact_No", "234-803-345-2113, 234-909-756-1272");
        Ensure(values, "Mail_Activated", "No");
        Ensure(values, "Mail_Server", "mail5005.smarterasp.net");
        Ensure(values, "Mail_UserFrom", "noreply@logicversiononline.com");
        Ensure(values, "Mail_Password", "logic@123");
        Ensure(values, "Mail_SmtpPort", "8889");
        Ensure(values, "Mail_Subject", "Client Mail");
        Ensure(values, "Enforce_Lock_Bill_After_24hrs", "NO");
        Ensure(values, "SearchValue", "20");
        Ensure(values, "Print_Prescription", "NO");
    }

    private static void ValidateRequired(IReadOnlyDictionary<string, string> values)
    {
        Require(values, "PRIVATE", "System Default Value needed for Private Tariff");
        Require(values, "App_Name", "App_Name needed for this Module");

        if (IsYes(values, "Set_Lock_Down"))
        {
            Require(values, "Set_Lock_Down_Next_Date", "Expiry_Date needed for this Module");
        }

        if (IsYes(values, "AcctPostOn"))
        {
            Require(values, "AcctPeriodType", "AcctPeriodType needed for Posting");
            Require(values, "AcctPostType", "AcctPostType needed for Posting");
            Require(values, "FinYearStart", "FinYearStart needed for Posting");
            Require(values, "FinYearClose", "FinYearClose needed for Posting");
            Require(values, "FinPrdStart", "FinPrdStart needed for Posting");
            Require(values, "AcctPostLastPeriodCloseDate", "AcctPostLastPeriodCloseDate needed for Posting");
            Require(values, "ACCTNo_SUSP_SALES", "ACCTNo_SUSP_SALES missing");
            Require(values, "ACCTNo_SUSP_EXPENSES", "ACCTNo_SUSP_EXPENSES missing");
            Require(values, "ACCTNo_SUSP_ASSET", "ACCTNo_SUSP_ASSET missing");
            Require(values, "ACCTNo_SUSP_LIABILITY", "ACCTNo_SUSP_LIABILITY missing");
            Require(values, "ACCTNo_SUSP_EQUITY", "ACCTNo_SUSP_EQUITY missing");
            Require(values, "AcctNoSales_Return", "AcctNoSales_Return needed for Posting");
            Require(values, "AcctNoPurchase_Return", "AcctNoPurchase_Return needed for Posting");
            Require(values, "AcctNo_Sales_Discount", "AcctNo_Sales_Discount needed for Posting");
            Require(values, "AcctNo_COGS", "AcctNo_COGS_Lab Acct  Required");
            Require(values, "AcctNo_Inventory_Lab", "AcctNo_Inventory_Lab Acct  Required");
            Require(values, "AcctCostCenter", "AcctCostCenter needed for Posting");
            Require(values, "AcctNoPOS", "POS Acct No needed for Posting");
            Require(values, "AcctNoCheque", "Cheque Acct No needed for Posting");
            Require(values, "AcctNoTransfer", "Transfer Acct No needed for Posting");
            Require(values, "AcctNoCash", "Cash Acct No needed for Posting");
            Require(values, "AcctNo_PettyCash", "Petty Cash Acct No needed for Posting");
            Require(values, "Acct_Banks", "Acct_Banks Acct Group for Banks Required");
            Require(values, "Acct_Cash", "Acct_Cash Acct Group for Cash Required");
            Require(values, "Acct_Revenue", "Acct_Revenue Acct Group Required");
            Require(values, "Acct_Expenses", "Acct_Expenses Acct Group Required");
            Require(values, "Acct_Inventory_Purchase", "Acct_Inventory_Purchase Acct Group Required");
            Require(values, "Acct_Payable", "Acct_Payable Acct Group Required");
            Require(values, "Acct_Receivable", "Acct_Receivable Acct Group Required");
            Require(values, "AcctPostType_Cash", "AcctPostType_Cash Required");
            Require(values, "AcctPostType_COGS", "AcctPostType_COGS Required");
            Require(values, "AcctPostType_Expenses", "AcctPostType_Expenses Required");
            Require(values, "AcctPostType_Inventory_Purchase", "AcctPostType_Inventory_Purchase Required");
            Require(values, "AcctPostType_Payable", "AcctPostType_Payable Required");
            Require(values, "AcctPostType_Receivable", "AcctPostType_Receivable Required");
            Require(values, "AcctPostType_Revenue_Cash", "AcctPostType_Revenue_Cash Required");
            Require(values, "AcctNo_Inventory_Pharmacy", "AcctNo_Inventory_Pharmacy Acct  Required");
            Require(values, "CashAccountIndex", "CashAccountIndex needed for Posting");
            Require(values, "ARAccountIndex", "Cash Acct Index No needed for Posting");
            Require(values, "AcctNoSales", "Sales Acct No needed for Posting");
            Require(values, "AcctNoSalesInv", "Sales Invoice Acct No needed for Posting");

            RequireNonZeroInteger(values, "CashAccountIndex", "Cash Acct Index No needed for Posting");
            RequireNonZeroInteger(values, "ARAccountIndex", "ARAccountIndex needed for Posting");
        }
    }

    private static bool IsYes(IReadOnlyDictionary<string, string> values, string key)
    {
        return values.TryGetValue(key, out var value)
            && string.Equals(value?.Trim(), "YES", StringComparison.OrdinalIgnoreCase);
    }

    private static void RequireNonZeroInteger(IReadOnlyDictionary<string, string> values, string key, string message)
    {
        if (!values.TryGetValue(key, out var value)
            || !int.TryParse(value, out var parsed)
            || parsed == 0)
        {
            throw new InvalidOperationException(message);
        }
    }

    private static void Require(IReadOnlyDictionary<string, string> values, string key, string message)
    {
        if (!values.TryGetValue(key, out var value) || string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidOperationException(message);
        }
    }

    private static void Ensure(IDictionary<string, string> values, string key, string defaultValue)
    {
        if (!values.TryGetValue(key, out var current) || string.IsNullOrWhiteSpace(current))
        {
            values[key] = defaultValue;
        }
    }

    private static async Task<string> GetDefaultPrinterAsync(ApplicationDbContext context, CancellationToken cancellationToken)
    {
        var printer = await context.Database
            .SqlQueryRaw<string>("SELECT TOP (1) LTRIM(RTRIM([PrtName])) AS [Value] FROM [PrintName]")
            .FirstOrDefaultAsync(cancellationToken);

        return printer?.Trim() ?? string.Empty;
    }
}
