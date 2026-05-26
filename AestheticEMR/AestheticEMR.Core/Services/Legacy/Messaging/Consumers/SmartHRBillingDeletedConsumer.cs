using AestheticEMR.Core.Services.Legacy.Messaging.Events;
using MassTransit;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AestheticEMR.Core.Services.Legacy.Messaging.Consumers;

public class SmartHRBillingDeletedConsumer(
    IConfiguration configuration,
    ILogger<SmartHRBillingDeletedConsumer> logger) : IConsumer<BillingDeletedEvent>
{
    public async Task Consume(ConsumeContext<BillingDeletedEvent> context)
    {
        var connStr = configuration.GetConnectionString("SmartHRConnection");
        if (string.IsNullOrWhiteSpace(connStr))
        {
            logger.LogWarning("SmartHRConnection not configured; skipping SmartHR delete for {BillNo}.", context.Message.BillNo);
            return;
        }

        await using var conn = new SqlConnection(connStr);
        await conn.OpenAsync(context.CancellationToken);

        await using var del = new SqlCommand("DELETE FROM [dbo].[BillingDetail] WHERE billNO = @BillNo", conn);
        del.Parameters.AddWithValue("@BillNo", context.Message.BillNo);
        await del.ExecuteNonQueryAsync(context.CancellationToken);

        await using var del2 = new SqlCommand("DELETE FROM [dbo].[Billing] WHERE billNO = @BillNo", conn);
        del2.Parameters.AddWithValue("@BillNo", context.Message.BillNo);
        await del2.ExecuteNonQueryAsync(context.CancellationToken);

        logger.LogInformation("SmartHR deleted billing {BillNo}.", context.Message.BillNo);
    }
}
