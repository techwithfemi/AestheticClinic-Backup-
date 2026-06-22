using AestheticEMR.Core.Models.Shop;

namespace AestheticEMR.Core.Services.Legacy.Interfaces;

/// <summary>
/// Posts inventory transactions to the accounting database using InsertTranxaction stored procedure.
/// Handles COGS (Cost of Goods Sold) debits and Inventory credits for product usage in billing.
/// </summary>
public interface IInventoryAccountingService
{
    /// <summary>
    /// Posts a debit to COGS and credit to Inventory accounts for product quantity deductions.
    /// Called when bill items with category "Product" are saved to billing.
    /// </summary>
    Task PostInventoryDeductionAsync(string billNo, Product product, int quantityDeducted, CancellationToken cancellationToken = default);

    /// <summary>
    /// Posts a credit to COGS and debit to Inventory accounts for quantity reversals.
    /// Called when billing items are updated or deleted to reverse previous inventory postings.
    /// </summary>
    Task PostInventoryReversalAsync(string billNo, Product product, int quantityRestored, CancellationToken cancellationToken = default);
}
