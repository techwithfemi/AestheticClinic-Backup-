using AestheticEMR.Core.Services.Legacy.Interfaces;
using AestheticEMR.Core.Services.Legacy.Messaging.Events;
using MassTransit;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;

namespace AestheticEMR.Core.Services.Legacy.Messaging.Consumers;

/// <summary>
/// Idempotently applies BillingUpsertedEvent to the Accounting database.
/// </summary>
public class AccountingBillingUpsertedConsumer(
    IConfiguration configuration,
    IEmrAppDefaultsService emrAppDefaultsService,
    ILogger<AccountingBillingUpsertedConsumer> logger) : IConsumer<BillingUpsertedEvent>
{
    public async Task Consume(ConsumeContext<BillingUpsertedEvent> context)
    {
        var connStr = configuration.GetConnectionString("AccountingConnection");
        if (string.IsNullOrWhiteSpace(connStr))
        {
            logger.LogWarning("AccountingConnection not configured; skipping Accounting billing sync for {BillNo}.", context.Message.BillNo);
            return;
        }

        await using var conn = new SqlConnection(connStr);
        await conn.OpenAsync(context.CancellationToken);

        var appDefaults = await emrAppDefaultsService.GetAsync(context.CancellationToken);
        var values = appDefaults.Values;

        await UpsertBillingAsync(conn, context.Message, context.CancellationToken);

        var existingTranIds = await GetExistingTranIdsAsync(conn, context.Message.BillNo, context.CancellationToken);
        if (existingTranIds.Count > 0)
        {
            await DeleteSalesTransactionsAsync(conn, existingTranIds, values, context.CancellationToken);
        }

        await ReplaceBillingDetailsAsync(conn, context.Message, context.CancellationToken);
        await PostSalesTransactionsAsync(conn, context.Message, values, context.CancellationToken);

        logger.LogInformation("Accounting billing sync completed for {BillNo}.", context.Message.BillNo);
    }

    private static async Task UpsertBillingAsync(SqlConnection conn, BillingUpsertedEvent msg, CancellationToken ct)
    {
        const string sql = """
            MERGE [dbo].[Billing] AS tgt
            USING (SELECT @BillNo, @BDate, @PNo, @ClientId, @DebtBF, @AmountBilled, @Discount, @AmountPaid, @Tax,
                          @BillType, @IsPaid, @IsProcess, @AdmDate, @DischDate, @TimeVal, @ApprvCode, @IsPost)
                  AS src (billNO, bDate, pNo, clientID, DebtBF, AmountBilled, Discount, AmountPaid, Tax,
                           billType, isPaid, isProcess, AdmDate, DischDate, timeVal, ApprvCode, isPost)
            ON tgt.billNO = src.billNO
            WHEN MATCHED THEN UPDATE SET
                bDate=src.bDate, pNo=src.pNo, clientID=src.clientID,
                DebtBF=src.DebtBF, AmountBilled=src.AmountBilled, Discount=src.Discount, AmountPaid=src.AmountPaid, Tax=src.Tax,
                billType=src.billType, isPaid=src.isPaid, isProcess=src.isProcess,
                AdmDate=src.AdmDate, DischDate=src.DischDate, timeVal=src.timeVal,
                ApprvCode=src.ApprvCode, isPost=src.isPost
            WHEN NOT MATCHED THEN INSERT
                (billNO, bDate, pNo, clientID, DebtBF, AmountBilled, Discount, AmountPaid, Tax,
                 billType, isPaid, isProcess, AdmDate, DischDate, timeVal, ApprvCode, isPost)
            VALUES (src.billNO, src.bDate, src.pNo, src.clientID, src.DebtBF, src.AmountBilled,
                    src.Discount, src.AmountPaid, src.Tax, src.billType, src.isPaid, src.isProcess,
                    src.AdmDate, src.DischDate, src.timeVal, src.ApprvCode, src.isPost);
            """;

        await using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@BillNo", msg.BillNo);
        cmd.Parameters.AddWithValue("@BDate", msg.BDate.ToDateTime(TimeOnly.MinValue));
        cmd.Parameters.AddWithValue("@PNo", msg.PNo);
        cmd.Parameters.AddWithValue("@ClientId", (object?)msg.ClientId ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@DebtBF", msg.DebtBF);
        cmd.Parameters.AddWithValue("@AmountBilled", msg.AmountBilled);
        cmd.Parameters.AddWithValue("@Discount", msg.Discount);
        cmd.Parameters.AddWithValue("@AmountPaid", msg.AmountPaid);
        cmd.Parameters.AddWithValue("@Tax", msg.Tax);
        cmd.Parameters.AddWithValue("@BillType", (object?)msg.BillType ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@IsPaid", msg.IsPaid);
        cmd.Parameters.AddWithValue("@IsProcess", msg.IsProcess);
        cmd.Parameters.AddWithValue("@AdmDate", (object?)msg.AdmDate ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@DischDate", (object?)msg.DischDate ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@TimeVal", (object?)msg.TimeVal ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@ApprvCode", (object?)msg.ApprvCode ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@IsPost", msg.IsPost);
        await cmd.ExecuteNonQueryAsync(ct);
    }

    private static async Task<List<string>> GetExistingTranIdsAsync(SqlConnection conn, string billNo, CancellationToken ct)
    {
        var tranIds = new List<string>();

        const string sql = "SELECT DISTINCT TranID FROM [dbo].[BillingDetail] WHERE billNO = @BillNo AND TranID IS NOT NULL AND LTRIM(RTRIM(TranID)) <> ''";
        await using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@BillNo", billNo);

        await using var reader = await cmd.ExecuteReaderAsync(ct);
        while (await reader.ReadAsync(ct))
        {
            var tranId = reader.GetString(0)?.Trim();
            if (!string.IsNullOrWhiteSpace(tranId))
            {
                tranIds.Add(tranId);
            }
        }

        return tranIds.Distinct(StringComparer.OrdinalIgnoreCase).ToList();
    }

    private static async Task DeleteSalesTransactionsAsync(SqlConnection conn, IReadOnlyCollection<string> tranIds, IReadOnlyDictionary<string, string> values, CancellationToken ct)
    {
        var coyId = values.GetValueOrDefault("CoyID", "0001");

        foreach (var tranId in tranIds.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct(StringComparer.OrdinalIgnoreCase))
        {
            await using var cmd = new SqlCommand("DeleteTranxaction", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Period", "");
            cmd.Parameters.AddWithValue("@CoyID", coyId);
            cmd.Parameters.AddWithValue("@TranNo", tranId);
            cmd.Parameters.AddWithValue("@userName", "system");
            await cmd.ExecuteNonQueryAsync(ct);
        }
    }

    private static async Task ReplaceBillingDetailsAsync(SqlConnection conn, BillingUpsertedEvent msg, CancellationToken ct)
    {
        await using var del = new SqlCommand("DELETE FROM [dbo].[BillingDetail] WHERE billNO = @BillNo", conn);
        del.Parameters.AddWithValue("@BillNo", msg.BillNo);
        await del.ExecuteNonQueryAsync(ct);

        foreach (var d in msg.Details)
        {
            const string ins = """
                INSERT INTO [dbo].[BillingDetail]
                    (billNO, SNO, TranID, dtDate, drgName, Price, Qty, subTotal, billType, conID, revType, BillTo, CoyName, BillBy)
                VALUES
                    (@BillNo, @SNO, @TranID, @DtDate, @DrgName, @Price, @Qty, @SubTotal, @BillType, @ConId, @RevType, @BillTo, @CoyName, @BillBy);
                """;
            await using var ins_cmd = new SqlCommand(ins, conn);
            ins_cmd.Parameters.AddWithValue("@BillNo", d.BillNo);
            ins_cmd.Parameters.AddWithValue("@SNO", d.SNO);
            ins_cmd.Parameters.AddWithValue("@TranID", (object?)d.TranID ?? DBNull.Value);
            ins_cmd.Parameters.AddWithValue("@DtDate", d.DtDate);
            ins_cmd.Parameters.AddWithValue("@DrgName", d.DrgName);
            ins_cmd.Parameters.AddWithValue("@Price", d.Price);
            ins_cmd.Parameters.AddWithValue("@Qty", d.Qty);
            ins_cmd.Parameters.AddWithValue("@SubTotal", (object?)d.SubTotal ?? DBNull.Value);
            ins_cmd.Parameters.AddWithValue("@BillType", (object?)d.BillType ?? DBNull.Value);
            ins_cmd.Parameters.AddWithValue("@ConId", (object?)d.ConId ?? DBNull.Value);
            ins_cmd.Parameters.AddWithValue("@RevType", (object?)d.RevType ?? DBNull.Value);
            ins_cmd.Parameters.AddWithValue("@BillTo", (object?)d.BillTo ?? DBNull.Value);
            ins_cmd.Parameters.AddWithValue("@CoyName", (object?)d.CoyName ?? DBNull.Value);
            ins_cmd.Parameters.AddWithValue("@BillBy", (object?)d.BillBy ?? DBNull.Value);
            await ins_cmd.ExecuteNonQueryAsync(ct);
        }
    }

    private static async Task PostSalesTransactionsAsync(SqlConnection conn, BillingUpsertedEvent msg, IReadOnlyDictionary<string, string> values, CancellationToken ct)
    {
        var acctPostOn = string.Equals(values.GetValueOrDefault("AcctPostOn", "false"), "true", StringComparison.OrdinalIgnoreCase);
        if (!acctPostOn)
        {
            return;
        }

        var strPrivate = values.GetValueOrDefault("PRIVATE", "0001");
        var acctPostTypeCash = values.GetValueOrDefault("AcctPostType_Cash", "AUTO");
        var acctPostTypeReceivable = values.GetValueOrDefault("AcctPostType_Receivable", "AUTO");

        if (string.Equals(msg.ClientId, strPrivate, StringComparison.OrdinalIgnoreCase))
        {
            if (!string.Equals(acctPostTypeCash, "AUTO", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }
        }
        else
        {
            if (!string.Equals(acctPostTypeReceivable, "AUTO", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }
        }

        var debitAccountNo = values.GetValueOrDefault("AcctNo_Receivable") ?? values.GetValueOrDefault("ACCTNo_SUSP_ASSET");
        var creditAccountNo = values.GetValueOrDefault("AcctNoSales") ?? values.GetValueOrDefault("ACCTNo_SUSP_SALES");
        var costCenter = values.GetValueOrDefault("AcctCostCenter", "0001");
        var coyId = msg.ClientId ?? values.GetValueOrDefault("CoyID", "0001");
        var period = msg.BDate.Month.ToString("D2") + "/" + msg.BDate.Year;

        if (string.IsNullOrWhiteSpace(debitAccountNo) || string.IsNullOrWhiteSpace(creditAccountNo))
        {
            return;
        }

        foreach (var d in msg.Details)
        {
            var amount = d.SubTotal ?? (decimal)(d.Price * d.Qty);
            if (amount == 0)
            {
                continue;
            }

            var tranId = !string.IsNullOrWhiteSpace(d.TranID) ? d.TranID! : Guid.NewGuid().ToString();
            var description = $"{d.RevType} ({d.DrgName}) (BillNo: {msg.BillNo})";

            await CallInsertTranxactionAsync(conn, tranId, debitAccountNo, amount, description, msg, d, costCenter, period, coyId, ct);
            await CallInsertTranxactionAsync(conn, tranId, creditAccountNo, -amount, description, msg, d, costCenter, period, coyId, ct);
        }
    }

    private static async Task CallInsertTranxactionAsync(
        SqlConnection conn,
        string tranId,
        string accountNo,
        decimal amount,
        string description,
        BillingUpsertedEvent msg,
        BillingDetailPayload d,
        string costCenter,
        string period,
        string coyId,
        CancellationToken ct)
    {
        await using var cmd = conn.CreateCommand();
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "InsertTranxaction";
        cmd.Parameters.AddWithValue("@TranID", tranId);
        cmd.Parameters.AddWithValue("@AccountNo", accountNo);
        cmd.Parameters.AddWithValue("@TranNo", tranId);
        cmd.Parameters.AddWithValue("@TranDate", msg.BDate.ToDateTime(TimeOnly.MinValue));
        cmd.Parameters.AddWithValue("@CostCenterID", costCenter);
        cmd.Parameters.AddWithValue("@Amount", amount);
        cmd.Parameters.AddWithValue("@Description", description);
        cmd.Parameters.AddWithValue("@TranCat", "b");
        cmd.Parameters.AddWithValue("@EntryDate", DateTime.Now);
        cmd.Parameters.AddWithValue("@Period", period);
        cmd.Parameters.AddWithValue("@CoyID2", coyId);
        cmd.Parameters.AddWithValue("@UserName", "system");
        cmd.Parameters.AddWithValue("@SNoID", d.SNO);
        cmd.Parameters.AddWithValue("@BillNO", msg.BillNo);
        cmd.Parameters.AddWithValue("@Reversed", false);
        cmd.Parameters.AddWithValue("@ReversedPair", 0);

        await cmd.ExecuteNonQueryAsync(ct);
    }
}
