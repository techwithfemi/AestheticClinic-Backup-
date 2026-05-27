using AestheticEMR.Core.Services.Legacy.Messaging.Events;
using MassTransit;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AestheticEMR.Core.Services.Legacy.Messaging.Consumers;

public class AccountingBillingDeletedConsumer(
    IConfiguration configuration,
    ILogger<AccountingBillingDeletedConsumer> logger) : IConsumer<BillingDeletedEvent>
{
    public async Task Consume(ConsumeContext<BillingDeletedEvent> context)
    {
        var connStr = configuration.GetConnectionString("AccountingConnection");
        if (string.IsNullOrWhiteSpace(connStr))
        {
            logger.LogWarning("AccountingConnection not configured; skipping Accounting delete for {BillNo}.", context.Message.BillNo);
            return;
        }

        await using var conn = new SqlConnection(connStr);
        await conn.OpenAsync(context.CancellationToken);

        foreach (var tranId in context.Message.TranIds.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct(StringComparer.OrdinalIgnoreCase))
        {
            await using var cmd = new SqlCommand("DeleteTranxaction", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Period", "");
            cmd.Parameters.AddWithValue("@CoyID", "0001");
            cmd.Parameters.AddWithValue("@TranNo", tranId);
            cmd.Parameters.AddWithValue("@userName", "system");
            await cmd.ExecuteNonQueryAsync(context.CancellationToken);
        }

        await using var del = new SqlCommand("DELETE FROM [dbo].[BillingDetail] WHERE billNO = @BillNo", conn);
        del.Parameters.AddWithValue("@BillNo", context.Message.BillNo);
        await del.ExecuteNonQueryAsync(context.CancellationToken);

        await using var del2 = new SqlCommand("DELETE FROM [dbo].[Billing] WHERE billNO = @BillNo", conn);
        del2.Parameters.AddWithValue("@BillNo", context.Message.BillNo);
        await del2.ExecuteNonQueryAsync(context.CancellationToken);

        logger.LogInformation("Accounting deleted billing {BillNo}.", context.Message.BillNo);
    }
}
