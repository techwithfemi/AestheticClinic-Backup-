using AestheticEMR.Core.Models.Legacy;
using AestheticEMR.Core.Services.Legacy.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.Common;

namespace AestheticEMR.Core.Services.Legacy;

public class SqlServerSameInstanceBillingCrossDatabaseSyncService(
    IBillingCrossDatabaseSyncStrategyProvider strategyProvider,
    ILogger<SqlServerSameInstanceBillingCrossDatabaseSyncService> logger) : IBillingCrossDatabaseSyncService
{
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
        ArgumentNullException.ThrowIfNull(connection);
        ArgumentNullException.ThrowIfNull(transaction);
        ArgumentNullException.ThrowIfNull(billing);
        ArgumentNullException.ThrowIfNull(details);

        if (connection is not SqlConnection sqlConnection || transaction is not SqlTransaction sqlTransaction)
        {
            return;
        }

        var targetDatabases = strategyProvider.CurrentStatus.IncludedDatabases;
        if (targetDatabases.Count == 0)
        {
            return;
        }

        foreach (var databaseName in targetDatabases)
        {
            await UpsertBillingAsync(sqlConnection, sqlTransaction, databaseName, billing, cancellationToken);
            await ReplaceBillingDetailsAsync(sqlConnection, sqlTransaction, databaseName, billing.billNO, details, cancellationToken);
        }
    }

    public async Task SyncDeleteAsync(
        DbConnection connection,
        DbTransaction transaction,
        string billNo,
        string patientNo,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(billNo);
        ArgumentException.ThrowIfNullOrWhiteSpace(patientNo);
        ArgumentNullException.ThrowIfNull(connection);
        ArgumentNullException.ThrowIfNull(transaction);

        if (connection is not SqlConnection sqlConnection || transaction is not SqlTransaction sqlTransaction)
        {
            return;
        }

        var targetDatabases = strategyProvider.CurrentStatus.IncludedDatabases;
        if (targetDatabases.Count == 0)
        {
            return;
        }

        foreach (var databaseName in targetDatabases)
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

    private async Task UpsertBillingAsync(
        SqlConnection connection,
        SqlTransaction transaction,
        string databaseName,
        Billing billing,
        CancellationToken cancellationToken)
    {
        var sql = $"""
MERGE {QualifyTable(databaseName, "Billing")} AS target
USING (VALUES (@billNO, @bDate, @pNo, @clientID, @debtBF, @amountBilled, @discount, @amountPaid, @billType, @isPaid, @isProcess, @admDate, @dischDate, @timeVal, @apprvCode, @isPost))
       AS source (billNO, bDate, pNo, clientID, DebtBF, AmountBilled, Discount, AmountPaid, billType, isPaid, isProcess, AdmDate, DischDate, timeVal, ApprvCode, isPost)
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
        billType = source.billType,
        isPaid = source.isPaid,
        isProcess = source.isProcess,
        AdmDate = source.AdmDate,
        DischDate = source.DischDate,
        timeVal = source.timeVal,
        ApprvCode = source.ApprvCode,
        isPost = source.isPost
WHEN NOT MATCHED THEN
    INSERT (billNO, bDate, pNo, clientID, DebtBF, AmountBilled, Discount, AmountPaid, billType, isPaid, isProcess, AdmDate, DischDate, timeVal, ApprvCode, isPost)
    VALUES (source.billNO, source.bDate, source.pNo, source.clientID, source.DebtBF, source.AmountBilled, source.Discount, source.AmountPaid, source.billType, source.isPaid, source.isProcess, source.AdmDate, source.DischDate, source.timeVal, source.ApprvCode, source.isPost);
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
    (billNO, SNO, dtDate, drgName, Price, Qty, subTotal, billType, conID, revType, BillTo, CoyName, BillBy)
VALUES
    (@billNO, @sno, @dtDate, @drgName, @price, @qty, @subTotal, @billType, @conID, @revType, @billTo, @coyName, @billBy);
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
                CreateParameter("@billBy", detail.BillBy, SqlDbType.NVarChar, 50));
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
}
