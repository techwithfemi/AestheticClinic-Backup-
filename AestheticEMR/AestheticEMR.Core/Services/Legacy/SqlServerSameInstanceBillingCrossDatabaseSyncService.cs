using AestheticEMR.Core.Models.Legacy;
using AestheticEMR.Core.Services.Legacy.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.Common;

namespace AestheticEMR.Core.Services.Legacy;

public class SqlServerSameInstanceBillingCrossDatabaseSyncService : IBillingCrossDatabaseSyncService
{
    private readonly IBillingCrossDatabaseSyncStrategyProvider strategyProvider;
    private readonly ILogger<SqlServerSameInstanceBillingCrossDatabaseSyncService> logger;
    private readonly IServiceProvider serviceProvider;

    public SqlServerSameInstanceBillingCrossDatabaseSyncService(
        IBillingCrossDatabaseSyncStrategyProvider strategyProvider,
        ILogger<SqlServerSameInstanceBillingCrossDatabaseSyncService> logger,
        IServiceProvider serviceProvider)
    {
        this.strategyProvider = strategyProvider;
        this.logger = logger;
        this.serviceProvider = serviceProvider;
    }

    public BillingCrossDatabaseSyncStatus GetStatus(string primaryConnectionString)
    {
        return strategyProvider.CurrentStatus;
    }

    public async Task SyncCreateOrUpdateAsync(
        DbConnection connection,
        DbTransaction transaction,
        Billing billing,
        IReadOnlyCollection<BillingDetail> details,
        CancellationToken cancellationToken = default)
    {
        using var scope = serviceProvider.CreateScope();
        var appDefaultsService = scope.ServiceProvider.GetRequiredService<IEmrAppDefaultsService>();
        var appDefaults = await appDefaultsService.GetAsync(cancellationToken);
        var values = appDefaults.Values;
        var accountingDb = values["DbName_Acct"];
        var hospitalDb = values["DbName"];
        var targetDatabases = strategyProvider.CurrentStatus.IncludedDatabases;
        if (targetDatabases.Count == 0)
        {
            return;
        }
        foreach (var databaseName in targetDatabases)
        {
            // Skip SmartHR for all direct table operations
            if (string.Equals(databaseName, "SmartHR", StringComparison.OrdinalIgnoreCase))
                continue;

            if (string.Equals(databaseName, accountingDb, StringComparison.OrdinalIgnoreCase))
            {
                // Only post to accounting via stored procs
                await PostToAccountingDbAsync(billing, details, cancellationToken);
            }
            else if (string.Equals(databaseName, hospitalDb, StringComparison.OrdinalIgnoreCase))
            {
                // Only upsert to hospital DB with Billing tables
                if (connection is SqlConnection sqlConnection && transaction is SqlTransaction sqlTransaction)
                {
                    await UpsertBillingAsync(sqlConnection, sqlTransaction, databaseName, billing, cancellationToken);
                    await ReplaceBillingDetailsAsync(sqlConnection, sqlTransaction, databaseName, billing.billNO, details, cancellationToken);
                }
            }
            // else: skip all other DBs
        }
    }

    public async Task SyncDeleteAsync(
        DbConnection connection,
        DbTransaction transaction,
        string billNo,
        string patientNo,
        IReadOnlyCollection<string> tranIds,
        CancellationToken cancellationToken = default)
    {
        using var scope = serviceProvider.CreateScope();
        var appDefaultsService = scope.ServiceProvider.GetRequiredService<IEmrAppDefaultsService>();
        var appDefaults = await appDefaultsService.GetAsync(cancellationToken);
        var values = appDefaults.Values;
        var accountingDb = values["DbName_Acct"];
        var hospitalDb = values["DbName"];
        var targetDatabases = strategyProvider.CurrentStatus.IncludedDatabases;
        if (targetDatabases.Count == 0)
        {
            return;
        }
        foreach (var databaseName in targetDatabases)
        {
            if (string.Equals(databaseName, "SmartHR", StringComparison.OrdinalIgnoreCase))
                continue;

            if (string.Equals(databaseName, accountingDb, StringComparison.OrdinalIgnoreCase))
            {
                await DeleteFromAccountingDbAsync(tranIds, values, cancellationToken);
            }
            else if (string.Equals(databaseName, hospitalDb, StringComparison.OrdinalIgnoreCase))
            {
                if (connection is SqlConnection sqlConnection && transaction is SqlTransaction sqlTransaction)
                {
                    var detailsDelete = $"DELETE FROM {QualifyTable(databaseName, "BillingDetails")} WHERE billNO = @billNo;";
                    await ExecuteNonQueryAsync(sqlConnection, sqlTransaction, detailsDelete, cancellationToken,
                        CreateParameter("@billNo", billNo, SqlDbType.NVarChar, 50));

                    var billingDelete = $"DELETE FROM {QualifyTable(databaseName, "Billing")} WHERE billNO = @billNo AND pNo = @pNo;";
                    await ExecuteNonQueryAsync(sqlConnection, sqlTransaction, billingDelete, cancellationToken,
                        CreateParameter("@billNo", billNo, SqlDbType.NVarChar, 50),
                        CreateParameter("@pNo", patientNo, SqlDbType.NVarChar, 50));
                }
            }
            // else: skip all other DBs
        }
    }

    private async Task UpsertBillingAsync(
        SqlConnection connection,
        SqlTransaction transaction,
        string databaseName,
        Billing billing,
        CancellationToken cancellationToken)
    {
        var sql = $"""
MERGE {QualifyTable(databaseName, "Billing")} AS target
USING (VALUES (@billNO, @bDate, @pNo, @clientID, @debtBF, @amountBilled, @discount, @amountPaid, @tax, @billType, @isPaid, @isProcess, @admDate, @dischDate, @timeVal, @apprvCode, @isPost))
       AS source (billNO, bDate, pNo, clientID, DebtBF, AmountBilled, Discount, AmountPaid, Tax, billType, isPaid, isProcess, AdmDate, DischDate, timeVal, ApprvCode, isPost)
ON target.billNO = source.billNO
WHEN MATCHED THEN
    UPDATE SET
        bDate = source.bDate,
        pNo = source.pNo,
        clientID = source.clientID,
        DebtBF = source.DebtBF,
        AmountBilled = source.AmountBilled,
        Discount = source.Discount,
        AmountPaid = source.AmountPaid,
        Tax = source.Tax,
        billType = source.billType,
        isPaid = source.isPaid,
        isProcess = source.isProcess,
        AdmDate = source.AdmDate,
        DischDate = source.DischDate,
        timeVal = source.timeVal,
        ApprvCode = source.ApprvCode,
        isPost = source.isPost
WHEN NOT MATCHED THEN
    INSERT (billNO, bDate, pNo, clientID, DebtBF, AmountBilled, Discount, AmountPaid, Tax, billType, isPaid, isProcess, AdmDate, DischDate, timeVal, ApprvCode, isPost)
    VALUES (source.billNO, source.bDate, source.pNo, source.clientID, source.DebtBF, source.AmountBilled, source.Discount, source.AmountPaid, source.Tax, source.billType, source.isPaid, source.isProcess, source.AdmDate, source.DischDate, source.timeVal, source.ApprvCode, source.isPost);
""";

        await ExecuteNonQueryAsync(connection, transaction, sql, cancellationToken,
            CreateParameter("@billNO", billing.billNO, SqlDbType.NVarChar, 50),
            CreateParameter("@bDate", billing.bDate, SqlDbType.Date),
            CreateParameter("@pNo", billing.pNo, SqlDbType.NVarChar, 50),
            CreateParameter("@clientID", billing.clientID, SqlDbType.NVarChar, 50),
            CreateParameter("@debtBF", billing.DebtBF, SqlDbType.Decimal),
            CreateParameter("@amountBilled", billing.AmountBilled, SqlDbType.Decimal),
            CreateParameter("@discount", billing.Discount, SqlDbType.Decimal),
            CreateParameter("@amountPaid", billing.AmountPaid, SqlDbType.Decimal),
            CreateParameter("@tax", billing.Tax, SqlDbType.Float),
            CreateParameter("@billType", billing.billType, SqlDbType.NVarChar, 50),
            CreateParameter("@isPaid", billing.isPaid, SqlDbType.Bit),
            CreateParameter("@isProcess", billing.isProcess, SqlDbType.Bit),
            CreateParameter("@admDate", billing.AdmDate, SqlDbType.DateTime2),
            CreateParameter("@dischDate", billing.DischDate, SqlDbType.DateTime2),
            CreateParameter("@timeVal", billing.timeVal, SqlDbType.DateTime2),
            CreateParameter("@apprvCode", billing.ApprvCode, SqlDbType.NVarChar, 150),
            CreateParameter("@isPost", billing.isPost, SqlDbType.Bit));
    }

    private async Task ReplaceBillingDetailsAsync(
        SqlConnection connection,
        SqlTransaction transaction,
        string databaseName,
        string billNo,
        IReadOnlyCollection<BillingDetail> details,
        CancellationToken cancellationToken)
    {
        var deleteSql = $"DELETE FROM {QualifyTable(databaseName, "BillingDetails")} WHERE billNO = @billNo;";
        await ExecuteNonQueryAsync(connection, transaction, deleteSql, cancellationToken,
            CreateParameter("@billNo", billNo, SqlDbType.NVarChar, 50));

        if (details.Count == 0)
        {
            return;
        }

        var insertSql = $"""
INSERT INTO {QualifyTable(databaseName, "BillingDetails")}
    (billNO, SNO, dtDate, drgName, Price, Qty, subTotal, billType, conID, revType, BillTo, CoyName, BillBy, TranID)
VALUES
    (@billNO, @sno, @dtDate, @drgName, @price, @qty, @subTotal, @billType, @conID, @revType, @billTo, @coyName, @billBy, @tranId);
""";

        foreach (var detail in details)
        {
            await ExecuteNonQueryAsync(connection, transaction, insertSql, cancellationToken,
                CreateParameter("@billNO", detail.billNO, SqlDbType.NVarChar, 50),
                CreateParameter("@sno", detail.SNO, SqlDbType.BigInt),
                CreateParameter("@dtDate", detail.dtDate, SqlDbType.DateTime2),
                CreateParameter("@drgName", detail.drgName, SqlDbType.NVarChar, 200),
                CreateParameter("@price", detail.Price, SqlDbType.Float),
                CreateParameter("@qty", detail.Qty, SqlDbType.Float),
                CreateParameter("@subTotal", detail.subTotal, SqlDbType.Decimal),
                CreateParameter("@billType", detail.billType, SqlDbType.NVarChar, 50),
                CreateParameter("@conID", detail.conID, SqlDbType.NVarChar, 50),
                CreateParameter("@revType", detail.revType, SqlDbType.NVarChar, 100),
                CreateParameter("@billTo", detail.BillTo, SqlDbType.NVarChar, 100),
                CreateParameter("@coyName", detail.CoyName, SqlDbType.NVarChar, 100),
                CreateParameter("@billBy", detail.BillBy, SqlDbType.NVarChar, 50),
                CreateParameter("@tranId", detail.TranID, SqlDbType.NVarChar, 100));
        }
    }

    private static async Task ExecuteNonQueryAsync(
        SqlConnection connection,
        SqlTransaction transaction,
        string sql,
        CancellationToken cancellationToken,
        params SqlParameter[] parameters)
    {
        await using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = sql;
        command.CommandType = CommandType.Text;

        if (parameters.Length > 0)
        {
            command.Parameters.AddRange(parameters);
        }

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    private static SqlParameter CreateParameter(string name, object? value, SqlDbType dbType, int? size = null)
    {
        var parameter = new SqlParameter(name, dbType)
        {
            Value = value ?? DBNull.Value
        };

        if (size.HasValue)
        {
            parameter.Size = size.Value;
        }

        if (dbType == SqlDbType.Decimal)
        {
            parameter.Precision = 18;
            parameter.Scale = 2;
        }

        return parameter;
    }

    private static string QualifyTable(string databaseName, string tableName)
    {
        return $"[{EscapeIdentifier(databaseName)}].[dbo].[{EscapeIdentifier(tableName)}]";
    }

    private static string EscapeIdentifier(string value)
    {
        return value.Replace("]", "]]", StringComparison.Ordinal);
    }

    private async Task PostToAccountingDbAsync(Billing billing, IReadOnlyCollection<BillingDetail> details, CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        var appDefaultsService = scope.ServiceProvider.GetRequiredService<IEmrAppDefaultsService>();
        var appDefaults = await appDefaultsService.GetAsync(cancellationToken);
        var values = appDefaults.Values;
        var accountingDb = values["DbName_Acct"];
        var coyID = billing.clientID ?? values["CoyID"];
        var billNo = billing.billNO;
        var pNo = billing.pNo;
        var acctPostOn = values.GetValueOrDefault("AcctPostOn", "false").ToLower() == "true";
        var acctPostType = values.GetValueOrDefault("AcctPostType", "AUTO");
        var acctPostTypeCash = values.GetValueOrDefault("AcctPostType_Cash", "AUTO");
        var acctPostTypeReceivable = values.GetValueOrDefault("AcctPostType_Receivable", "AUTO");
        var strPrivate = values.GetValueOrDefault("PRIVATE", "0001");
        if (!acctPostOn) return;
        if (coyID == strPrivate)
        {
            if (acctPostTypeCash != "AUTO") return;
        }
        else
        {
            if (acctPostTypeReceivable != "AUTO") return;
        }
        // Now connect to Accounting DB and call stored procs as per VB6 logic
        var accountingConnStr = GetAccountingConnectionString();
        await using var conn = new SqlConnection(accountingConnStr);
        await conn.OpenAsync(cancellationToken);
        await using var tran = await conn.BeginTransactionAsync(cancellationToken) as SqlTransaction;
        try
        {
            // Example: call stored proc InsertTranxaction for each detail (simplified)
            foreach (var detail in details)
            {
                if (detail.subTotal == null || detail.subTotal == 0) continue;
                // Call InsertTranxaction for debit (ASSET/RECEIVABLE)
                await CallInsertTranxactionAsync(conn, tran, billing, detail, true, values, cancellationToken);
                // Call InsertTranxaction for credit (SALES)
                await CallInsertTranxactionAsync(conn, tran, billing, detail, false, values, cancellationToken);
            }
            await tran.CommitAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            await tran.RollbackAsync(cancellationToken);
            logger.LogError(ex, "Accounting posting failed for BillNo {BillNo}", billNo);
            throw;
        }
    }

    private async Task DeleteFromAccountingDbAsync(string billNo, CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        var appDefaultsService = scope.ServiceProvider.GetRequiredService<IEmrAppDefaultsService>();
        var appDefaults = await appDefaultsService.GetAsync(cancellationToken);
        var values = appDefaults.Values;
        var accountingDb = values["DbName_Acct"];
        var accountingConnStr = GetAccountingConnectionString();
        await using var conn = new SqlConnection(accountingConnStr);
        await conn.OpenAsync(cancellationToken);
        await using var tran = await conn.BeginTransactionAsync(cancellationToken) as SqlTransaction;
        try
        {
            // Call DeleteTranxaction stored proc for the billNo
            await CallDeleteTranxactionAsync(conn, tran, billNo, values, cancellationToken);
            await tran.CommitAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            await tran.RollbackAsync(cancellationToken);
            logger.LogError(ex, "Accounting delete failed for BillNo {BillNo}", billNo);
            throw;
        }
    }

    private string GetAccountingConnectionString()
    {
        // TODO: Read from config or connection string provider
        // For now, assume it's in appsettings.json as "AccountingConnection"
        var config = serviceProvider.GetRequiredService<IConfiguration>();
        return config.GetConnectionString("AccountingConnection");
    }

    private async Task CallInsertTranxactionAsync(SqlConnection conn, SqlTransaction tran, Billing billing, BillingDetail detail, bool isDebit, IReadOnlyDictionary<string, string> values, CancellationToken cancellationToken)
    {
        // Determine account numbers based on revType, billType, or config
        string acctNo = null;
        if (isDebit)
        {
            acctNo = values.GetValueOrDefault("AcctNo_Receivable") ?? values.GetValueOrDefault("ACCTNo_SUSP_ASSET");
        }
        else
        {
            acctNo = values.GetValueOrDefault("AcctNoSales") ?? values.GetValueOrDefault("ACCTNo_SUSP_SALES");
        }
        if (string.IsNullOrEmpty(acctNo))
            acctNo = isDebit ? values.GetValueOrDefault("ACCTNo_SUSP_ASSET") : values.GetValueOrDefault("ACCTNo_SUSP_SALES");

        var tranId = detail.TranID ?? throw new InvalidOperationException($"Billing detail TranID is required for billNo '{billing.billNO}'.");
        var cmd = conn.CreateCommand();
        cmd.Transaction = tran;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = values.GetValueOrDefault("AcctPostType", "AUTO") == "AUTO" ? "InsertTranxaction" : "InsertTranxactionJournal";
        // Generate period string in MM/YYYY format
        var period = billing.bDate.Month.ToString("D2") + "/" + billing.bDate.Year.ToString();
        // Add parameters in the exact order and names as the sproc
        cmd.Parameters.AddWithValue("@TranID", tranId);
        cmd.Parameters.AddWithValue("@AccountNo", acctNo);
        cmd.Parameters.AddWithValue("@TranNo", tranId);
        cmd.Parameters.AddWithValue("@TranDate", billing.bDate.ToDateTime(TimeOnly.MinValue));
        cmd.Parameters.AddWithValue("@CostCenterID", values["AcctCostCenter"]);
        cmd.Parameters.AddWithValue("@Amount", isDebit ? detail.subTotal.GetValueOrDefault() : -detail.subTotal.GetValueOrDefault());
        cmd.Parameters.AddWithValue("@Description", detail.revType + " (" + detail.drgName + ") (BillNo: " + billing.billNO + ")");
        cmd.Parameters.AddWithValue("@TranCat", "b");
        cmd.Parameters.AddWithValue("@EntryDate", DateTime.Now);
        cmd.Parameters.AddWithValue("@Period", period);
        cmd.Parameters.AddWithValue("@CoyID2", billing.clientID ?? values.GetValueOrDefault("CoyID", "0001"));
        cmd.Parameters.AddWithValue("@UserName", "system");
        cmd.Parameters.AddWithValue("@SNoID", detail.SNO);
        cmd.Parameters.AddWithValue("@BillNO", billing.billNO);
        cmd.Parameters.AddWithValue("@Reversed", detail.Reversed ?? false);
        cmd.Parameters.AddWithValue("@ReversedPair", detail.ReversedPair ?? 0);
        await cmd.ExecuteNonQueryAsync(cancellationToken);
    }

    private async Task DeleteFromAccountingDbAsync(IReadOnlyCollection<string> tranIds, IReadOnlyDictionary<string, string> values, CancellationToken cancellationToken)
    {
        var accountingConnStr = GetAccountingConnectionString();
        await using var conn = new SqlConnection(accountingConnStr);
        await conn.OpenAsync(cancellationToken);
        await using var tran = await conn.BeginTransactionAsync(cancellationToken) as SqlTransaction;
        try
        {
            foreach (var tranId in tranIds.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct(StringComparer.OrdinalIgnoreCase))
            {
                await CallDeleteTranxactionAsync(conn, tran, tranId, values, cancellationToken);
            }
            await tran.CommitAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            await tran.RollbackAsync(cancellationToken);
            logger.LogError(ex, "Accounting delete failed for TranIDs belonging to billing sync.");
            throw;
        }
    }

    private async Task CallDeleteTranxactionAsync(SqlConnection conn, SqlTransaction tran, string tranId, IReadOnlyDictionary<string, string> values, CancellationToken cancellationToken)
    {
        var cmd = conn.CreateCommand();
        cmd.Transaction = tran;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "DeleteTranxaction";
        cmd.Parameters.AddWithValue("@Period", "");
        cmd.Parameters.AddWithValue("@CoyID", values["CoyID"]);
        cmd.Parameters.AddWithValue("@TranNo", tranId);
        cmd.Parameters.AddWithValue("@userName", "system");
        await cmd.ExecuteNonQueryAsync(cancellationToken);
    }
}
