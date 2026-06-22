using AestheticEMR.Core.Models.Shop;
using AestheticEMR.Core.Services.Legacy.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;

namespace AestheticEMR.Core.Services.Legacy;

/// <summary>
/// Posts inventory transactions to the accounting database.
/// Uses InsertTranxaction and DeleteTranxaction stored procedures to maintain clean accounting entries.
/// 
/// Approach:
/// - When inventory decreases: INSERT into accounting DB, store tranID in Product.LastInventoryTranID
/// - When inventory updated: DELETE old tranID from accounting DB, then INSERT new entry with new tranID
/// - Result: Only one accounting entry per product per billing state (clean, no reversals/clutter)
/// </summary>
public class InventoryAccountingService(
    IConfiguration configuration,
    IEmrAppDefaultsService emrAppDefaultsService,
    ILogger<InventoryAccountingService> logger) : IInventoryAccountingService
{
    public async Task PostInventoryDeductionAsync(string billNo, Product product, int quantityDeducted, CancellationToken cancellationToken = default)
    {
        if (quantityDeducted <= 0)
        {
            return;
        }

        var appDefaults = await emrAppDefaultsService.GetAsync(cancellationToken);
        var values = appDefaults.Values;

        var acctPostOn = string.Equals(values.GetValueOrDefault("AcctPostOn", "false"), "true", StringComparison.OrdinalIgnoreCase);
        var acctPostTypeInventory = values.GetValueOrDefault("AcctPostType_Inventory_Purchase", "AUTO");

        if (!acctPostOn || !string.Equals(acctPostTypeInventory, "AUTO", StringComparison.OrdinalIgnoreCase))
        {
            logger.LogInformation(
                "Inventory deduction for BillNo {BillNo}, Product {ProductName}: accounting posting skipped (AcctPostOn={AcctPostOn}, AcctPostType_Inventory={AcctPostType}).",
                billNo, product.Name, acctPostOn, acctPostTypeInventory);
            return;
        }

        var cogsAcctNo = values.GetValueOrDefault("AcctNo_COGS");
        var inventoryAcctNo = values.GetValueOrDefault("AcctNo_Inventory_Pharmacy");

        if (string.IsNullOrWhiteSpace(cogsAcctNo))
        {
            logger.LogWarning("Inventory deduction for BillNo {BillNo}: no COGS account configured; posting skipped.", billNo);
            return;
        }

        if (string.IsNullOrWhiteSpace(inventoryAcctNo))
        {
            logger.LogWarning("Inventory deduction for BillNo {BillNo}: no Inventory account configured; posting skipped.", billNo);
            return;
        }

        var accountingConnStr = GetAccountingConnectionString();
        if (string.IsNullOrWhiteSpace(accountingConnStr))
        {
            logger.LogWarning("Inventory deduction for BillNo {BillNo}: AccountingConnection is not configured; posting skipped.", billNo);
            return;
        }

        try
        {
            await using var conn = new SqlConnection(accountingConnStr);
            await conn.OpenAsync(cancellationToken);
            await using var tran = (SqlTransaction)await conn.BeginTransactionAsync(cancellationToken);
            try
            {
                var tranId = Guid.NewGuid().ToString();
                var costCenter = values.GetValueOrDefault("AcctCostCenter", "0001");
                var coyId = values.GetValueOrDefault("CoyID", "0001");
                var period = DateTime.Today.Month.ToString("D2") + "/" + DateTime.Today.Year.ToString();
                
                // Calculate COGS amount
                var cogsAmount = product.BuyingPrice * quantityDeducted;
                var description = $"Inventory Deduction: {product.Name} (Qty: {quantityDeducted}) (BillNo: {billNo})";

                // Debit COGS account (positive amount)
                await CallInsertTranxactionAsync(
                    conn, tran, tranId, cogsAcctNo, cogsAmount, description, costCenter, period, coyId, billNo, cancellationToken);

                // Credit Inventory account (negative amount)
                await CallInsertTranxactionAsync(
                    conn, tran, tranId, inventoryAcctNo, -cogsAmount, description, costCenter, period, coyId, billNo, cancellationToken);

                await tran.CommitAsync(cancellationToken);
                
                // Store tranID in product for future reference
                product.LastInventoryTranID = tranId;
                
                logger.LogInformation(
                    "Inventory deduction for BillNo {BillNo}, Product {ProductName} (Qty: {Quantity}): posted to accounting (TranID: {TranID}, debit {CogsAcct}, credit {InventoryAcct}, amount {Amount}).",
                    billNo, product.Name, quantityDeducted, tranId, cogsAcctNo, inventoryAcctNo, cogsAmount);
            }
            catch
            {
                await tran.RollbackAsync(cancellationToken);
                throw;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Inventory deduction for BillNo {BillNo}, Product {ProductName}: accounting posting failed.", billNo, product.Name);
            throw;
        }
    }

    public async Task PostInventoryReversalAsync(string billNo, Product product, int quantityRestored, CancellationToken cancellationToken = default)
    {
        if (quantityRestored <= 0)
        {
            return;
        }

        // If there's no previous transaction, nothing to reverse
        if (string.IsNullOrWhiteSpace(product.LastInventoryTranID))
        {
            logger.LogInformation(
                "Inventory reversal for BillNo {BillNo}, Product {ProductName}: no previous TranID found; reversal skipped.",
                billNo, product.Name);
            return;
        }

        var appDefaults = await emrAppDefaultsService.GetAsync(cancellationToken);
        var values = appDefaults.Values;

        var acctPostOn = string.Equals(values.GetValueOrDefault("AcctPostOn", "false"), "true", StringComparison.OrdinalIgnoreCase);
        var acctPostTypeInventory = values.GetValueOrDefault("AcctPostType_Inventory_Purchase", "AUTO");

        if (!acctPostOn || !string.Equals(acctPostTypeInventory, "AUTO", StringComparison.OrdinalIgnoreCase))
        {
            logger.LogInformation(
                "Inventory reversal for BillNo {BillNo}, Product {ProductName}: accounting posting skipped (AcctPostOn={AcctPostOn}, AcctPostType_Inventory={AcctPostType}).",
                billNo, product.Name, acctPostOn, acctPostTypeInventory);
            return;
        }

        var accountingConnStr = GetAccountingConnectionString();
        if (string.IsNullOrWhiteSpace(accountingConnStr))
        {
            logger.LogWarning("Inventory reversal for BillNo {BillNo}: AccountingConnection is not configured; posting skipped.", billNo);
            return;
        }

        try
        {
            await using var conn = new SqlConnection(accountingConnStr);
            await conn.OpenAsync(cancellationToken);
            await using var tran = (SqlTransaction)await conn.BeginTransactionAsync(cancellationToken);
            try
            {
                var coyId = values.GetValueOrDefault("CoyID", "0001");
                
                // DELETE the old transaction entry from accounting DB
                await CallDeleteTranxactionAsync(
                    conn, tran, product.LastInventoryTranID, coyId, cancellationToken);

                // Clear the stored tranID since we deleted it
                product.LastInventoryTranID = null;

                await tran.CommitAsync(cancellationToken);
                
                logger.LogInformation(
                    "Inventory reversal for BillNo {BillNo}, Product {ProductName} (Qty: {Quantity}): deleted accounting entry (TranID: {TranID}).",
                    billNo, product.Name, quantityRestored, product.LastInventoryTranID);
            }
            catch
            {
                await tran.RollbackAsync(cancellationToken);
                throw;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Inventory reversal for BillNo {BillNo}, Product {ProductName}: accounting deletion failed.", billNo, product.Name);
            throw;
        }
    }

    private async Task CallInsertTranxactionAsync(
        SqlConnection conn,
        SqlTransaction tran,
        string tranId,
        string accountNo,
        decimal amount,
        string description,
        string costCenter,
        string period,
        string coyId,
        string billNo,
        CancellationToken cancellationToken)
    {
        await using var cmd = conn.CreateCommand();
        cmd.Transaction = tran;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "InsertTranxaction";

        var tranDate = DateTime.Today;
        cmd.Parameters.AddWithValue("@TranID", tranId);
        cmd.Parameters.AddWithValue("@AccountNo", accountNo);
        cmd.Parameters.AddWithValue("@TranNo", tranId);
        cmd.Parameters.AddWithValue("@TranDate", tranDate);
        cmd.Parameters.AddWithValue("@CostCenterID", costCenter);
        cmd.Parameters.AddWithValue("@Amount", amount);
        cmd.Parameters.AddWithValue("@Description", description);
        cmd.Parameters.AddWithValue("@TranCat", "b");
        cmd.Parameters.AddWithValue("@EntryDate", DateTime.Now);
        cmd.Parameters.AddWithValue("@Period", period);
        cmd.Parameters.AddWithValue("@CoyID2", coyId);
        cmd.Parameters.AddWithValue("@UserName", "system");
        cmd.Parameters.AddWithValue("@SNoID", 0);
        cmd.Parameters.AddWithValue("@BillNO", billNo);
        cmd.Parameters.AddWithValue("@Reversed", false);
        cmd.Parameters.AddWithValue("@ReversedPair", 0);

        await cmd.ExecuteNonQueryAsync(cancellationToken);
    }

    private async Task CallDeleteTranxactionAsync(
        SqlConnection conn,
        SqlTransaction tran,
        string tranId,
        string coyId,
        CancellationToken cancellationToken)
    {
        await using var cmd = conn.CreateCommand();
        cmd.Transaction = tran;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "DeleteTranxaction";
        cmd.Parameters.AddWithValue("@Period", "");
        cmd.Parameters.AddWithValue("@CoyID", coyId);
        cmd.Parameters.AddWithValue("@TranNo", tranId);
        cmd.Parameters.AddWithValue("@userName", "system");

        await cmd.ExecuteNonQueryAsync(cancellationToken);
    }

    private string? GetAccountingConnectionString()
    {
        return configuration.GetConnectionString("AccountingConnection");
    }
}
