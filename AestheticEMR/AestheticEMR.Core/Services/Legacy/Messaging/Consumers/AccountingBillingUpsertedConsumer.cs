using AestheticEMR.Core.Services.Legacy.Messaging.Events;
using MassTransit;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AestheticEMR.Core.Services.Legacy.Messaging.Consumers;

/// <summary>
/// Idempotently applies BillingUpsertedEvent to the Accounting database.
/// </summary>
public class AccountingBillingUpsertedConsumer(
    IConfiguration configuration,
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

        await UpsertBillingAsync(conn, context.Message, context.CancellationToken);
        await ReplaceBillingDetailsAsync(conn, context.Message, context.CancellationToken);

        logger.LogInformation("Accounting billing sync completed for {BillNo}.", context.Message.BillNo);
    }

    private static async Task UpsertBillingAsync(SqlConnection conn, BillingUpsertedEvent msg, CancellationToken ct)
    {
        const string sql = """
            MERGE [dbo].[Billing] AS tgt
            USING (SELECT @BillNo, @BDate, @PNo, @ClientId, @DebtBF, @AmountBilled, @Discount, @AmountPaid,
                          @BillType, @IsPaid, @IsProcess, @AdmDate, @DischDate, @TimeVal, @ApprvCode, @IsPost)
                  AS src (billNO, bDate, pNo, clientID, DebtBF, AmountBilled, Discount, AmountPaid,
                           billType, isPaid, isProcess, AdmDate, DischDate, timeVal, ApprvCode, isPost)
            ON tgt.billNO = src.billNO
            WHEN MATCHED THEN UPDATE SET
                bDate=src.bDate, pNo=src.pNo, clientID=src.clientID,
                DebtBF=src.DebtBF, AmountBilled=src.AmountBilled, Discount=src.Discount, AmountPaid=src.AmountPaid,
                billType=src.billType, isPaid=src.isPaid, isProcess=src.isProcess,
                AdmDate=src.AdmDate, DischDate=src.DischDate, timeVal=src.timeVal,
                ApprvCode=src.ApprvCode, isPost=src.isPost
            WHEN NOT MATCHED THEN INSERT
                (billNO, bDate, pNo, clientID, DebtBF, AmountBilled, Discount, AmountPaid,
                 billType, isPaid, isProcess, AdmDate, DischDate, timeVal, ApprvCode, isPost)
            VALUES (src.billNO, src.bDate, src.pNo, src.clientID, src.DebtBF, src.AmountBilled,
                    src.Discount, src.AmountPaid, src.billType, src.isPaid, src.isProcess,
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
}
