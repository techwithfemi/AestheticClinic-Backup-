using AestheticEMR.Core.Infrastructure;
using AestheticEMR.Core.Models.Legacy;
using AestheticEMR.Core.Services.Account;
using AestheticEMR.Core.Services.Legacy.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace AestheticEMR.Core.Services.Legacy;

public class BillingService(
    ApplicationDbContext context,
    IUserIdAccessor userIdAccessor,
    IBillingCrossDatabaseSyncService billingCrossDatabaseSyncService) : IBillingService
{
    public async Task<IEnumerable<Billing>> GetAllAsync()
    {
        return await context.Billings
            .AsNoTracking()
            .OrderByDescending(x => x.bDate)
            .ThenByDescending(x => x.billNO)
            .ToListAsync();
    }

    public async Task<Billing?> GetByBillNoAsync(string billNo)
    {
        var normalizedBillNo = NormalizeRequired(billNo, "Invoice number is required.");

        return await context.Billings
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.billNO == normalizedBillNo);
    }

    public async Task<IEnumerable<BillingDetail>> GetDetailsAsync(string billNo)
    {
        var normalizedBillNo = NormalizeRequired(billNo, "Invoice number is required.");

        return await context.BillingDetails
            .AsNoTracking()
            .Where(x => x.billNO == normalizedBillNo)
            .OrderBy(x => x.SNO)
            .ToListAsync();
    }

    public async Task<(Billing Billing, IEnumerable<BillingDetail> Details)> CreateAsync(Billing billing, IEnumerable<BillingDetail> details)
    {
        ArgumentNullException.ThrowIfNull(billing);

        var normalizedBilling = PrepareBillingForCreate(billing);
        var normalizedDetails = PrepareDetails(normalizedBilling, details).ToList();

        EnsureNoDuplicateItems(normalizedDetails);
        await EnsurePatientExistsAsync(normalizedBilling.pNo);
        await EnsureBillCanBeModifiedAsync(normalizedBilling, "created");

        RecalculateTotals(normalizedBilling, normalizedDetails);

        await using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            context.Billings.Add(normalizedBilling);
            if (normalizedDetails.Count > 0)
            {
                context.BillingDetails.AddRange(normalizedDetails);
            }

            await context.SaveChangesAsync();

            await billingCrossDatabaseSyncService.SyncCreateOrUpdateAsync(
                context.Database.GetDbConnection(),
                transaction.GetDbTransaction(),
                normalizedBilling,
                normalizedDetails);

            await transaction.CommitAsync();

            return (normalizedBilling, normalizedDetails);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<(Billing Billing, IEnumerable<BillingDetail> Details)> UpdateAsync(string billNo, Billing billing, IEnumerable<BillingDetail> details)
    {
        var normalizedBillNo = NormalizeRequired(billNo, "Invoice number is required.");
        ArgumentNullException.ThrowIfNull(billing);

        var existing = await context.Billings.FirstOrDefaultAsync(x => x.billNO == normalizedBillNo)
            ?? throw new InvalidOperationException($"Invoice '{normalizedBillNo}' was not found.");

        await EnsureBillCanBeModifiedAsync(existing, "updated");

        var normalizedBilling = PrepareBillingForUpdate(existing, billing, normalizedBillNo);
        var normalizedDetails = PrepareDetails(normalizedBilling, details).ToList();

        EnsureNoDuplicateItems(normalizedDetails);
        await EnsurePatientExistsAsync(normalizedBilling.pNo);

        RecalculateTotals(normalizedBilling, normalizedDetails);

        await using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            context.BillingDetails.RemoveRange(context.BillingDetails.Where(x => x.billNO == normalizedBillNo));
            if (normalizedDetails.Count > 0)
            {
                context.BillingDetails.AddRange(normalizedDetails);
            }

            await context.SaveChangesAsync();

            await billingCrossDatabaseSyncService.SyncCreateOrUpdateAsync(
                context.Database.GetDbConnection(),
                transaction.GetDbTransaction(),
                normalizedBilling,
                normalizedDetails);

            await transaction.CommitAsync();

            return (normalizedBilling, normalizedDetails);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task DeleteAsync(string billNo)
    {
        var normalizedBillNo = NormalizeRequired(billNo, "Invoice number is required.");

        var billing = await context.Billings.FirstOrDefaultAsync(x => x.billNO == normalizedBillNo);
        if (billing is null)
        {
            return;
        }

        await EnsureBillCanBeModifiedAsync(billing, "deleted");

        await using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            context.BillingDetails.RemoveRange(context.BillingDetails.Where(x => x.billNO == normalizedBillNo));
            context.Billings.Remove(billing);

            await context.SaveChangesAsync();

            await billingCrossDatabaseSyncService.SyncDeleteAsync(
                context.Database.GetDbConnection(),
                transaction.GetDbTransaction(),
                normalizedBillNo,
                billing.pNo);

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    private Billing PrepareBillingForCreate(Billing billing)
    {
        billing.billNO = NormalizeRequired(billing.billNO, "Invoice number is required.");
        billing.pNo = NormalizeRequired(billing.pNo, "Patient number is required.");
        billing.clientID = NormalizeOptional(billing.clientID);
        billing.billType = NormalizeOptional(billing.billType);
        billing.AmountBilledInWord = NormalizeOptional(billing.AmountBilledInWord);
        billing.BillingMonth = NormalizeOptional(billing.BillingMonth);
        billing.ApprvCode = NormalizeOptional(billing.ApprvCode);

        billing.bDate = billing.bDate == default ? DateOnly.FromDateTime(DateTime.Today) : billing.bDate;
        billing.BillingMonth ??= billing.bDate.ToString("MMMM");
        billing.BillingYear ??= billing.bDate.Year;
        billing.timeVal ??= DateTime.Now;

        billing.DebtBF ??= 0;
        billing.Discount ??= 0;
        billing.AmountPaid ??= 0;
        billing.AmountBilled ??= 0;
        billing.isPaid ??= false;
        billing.isProcess ??= false;
        billing.isPost ??= false;

        return billing;
    }

    private Billing PrepareBillingForUpdate(Billing target, Billing source, string billNo)
    {
        target.billNO = billNo;
        target.bDate = source.bDate == default ? target.bDate : source.bDate;
        target.pNo = NormalizeRequired(source.pNo, "Patient number is required.");
        target.clientID = NormalizeOptional(source.clientID);
        target.DebtBF = source.DebtBF ?? 0;
        target.AmountBilled = source.AmountBilled ?? 0;
        target.Discount = source.Discount ?? 0;
        target.AmountPaid = source.AmountPaid ?? 0;
        target.AmountBilledInWord = NormalizeOptional(source.AmountBilledInWord);
        target.BillingMonth = NormalizeOptional(source.BillingMonth) ?? target.bDate.ToString("MMMM");
        target.BillingYear = source.BillingYear ?? target.bDate.Year;
        target.isPaid = source.isPaid ?? false;
        target.billType = NormalizeOptional(source.billType);
        target.isProcess = source.isProcess ?? false;
        target.AdmDate = source.AdmDate;
        target.DischDate = source.DischDate;
        target.timeVal = source.timeVal ?? DateTime.Now;
        target.ApprvCode = NormalizeOptional(source.ApprvCode);
        target.isPost = source.isPost ?? false;

        return target;
    }

    private IEnumerable<BillingDetail> PrepareDetails(Billing billing, IEnumerable<BillingDetail> details)
    {
        var now = DateTime.Now;

        foreach (var detail in details ?? [])
        {
            detail.billNO = billing.billNO;
            detail.drgName = NormalizeRequired(detail.drgName, "Bill item is required.");
            detail.Price = detail.Price < 0 ? 0 : detail.Price;
            detail.Qty = detail.Qty <= 0 ? 1 : detail.Qty;
            detail.subTotal = Convert.ToDecimal(detail.Price * detail.Qty);
            detail.dtDate = detail.dtDate == default ? now : detail.dtDate;
            detail.billType = NormalizeOptional(detail.billType) ?? billing.billType;
            detail.conID = NormalizeOptional(detail.conID);
            detail.Capitated = NormalizeOptional(detail.Capitated) ?? "NO";
            detail.Dosage = NormalizeOptional(detail.Dosage);
            detail.Category = NormalizeOptional(detail.Category);
            detail.BillTo = NormalizeOptional(detail.BillTo) ?? "Self";
            detail.CoyName = NormalizeOptional(detail.CoyName) ?? detail.BillTo;
            detail.BillHead = NormalizeOptional(detail.BillHead);
            detail.revType = NormalizeOptional(detail.revType);
            detail.DRGCode = NormalizeOptional(detail.DRGCode);
            detail.isRct ??= false;
            detail.BillBy = NormalizeOptional(detail.BillBy) ?? userIdAccessor.GetCurrentUserEmpId();
            detail.treatedBy = NormalizeOptional(detail.treatedBy);
            detail.Dept = NormalizeOptional(detail.Dept);
            detail.isOLD ??= false;
            detail.ClientName = NormalizeOptional(detail.ClientName) ?? Environment.MachineName;
            detail.AppName = NormalizeOptional(detail.AppName) ?? "AestheticEMR";
            detail.RevClinic = NormalizeOptional(detail.RevClinic);
            detail.Reversed ??= false;
            detail.Remarks = NormalizeOptional(detail.Remarks);
            detail.suppres ??= false;
            detail.AppVersion ??= 1;

            yield return detail;
        }
    }

    private void RecalculateTotals(Billing billing, IEnumerable<BillingDetail> details)
    {
        var itemsTotal = details.Sum(x => x.subTotal ?? 0m);
        billing.AmountBilled = itemsTotal;

        var debt = billing.DebtBF ?? 0;
        var discount = billing.Discount ?? 0;
        var paid = billing.AmountPaid ?? 0;
        var due = itemsTotal + debt - discount;

        billing.isPaid = paid >= due && due > 0;
    }

    private async Task EnsurePatientExistsAsync(string pNo)
    {
        var exists = await context.HPatients.AsNoTracking().AnyAsync(x => x.Pno == pNo);
        if (!exists)
        {
            throw new InvalidOperationException($"Patient '{pNo}' was not found.");
        }
    }

    private async Task EnsureBillCanBeModifiedAsync(Billing billing, string operation)
    {
        var latestInvoice = await context.Billings
            .AsNoTracking()
            .Where(x => x.pNo == billing.pNo)
            .OrderByDescending(x => x.bDate)
            .ThenByDescending(x => x.billNO)
            .FirstOrDefaultAsync();

        if (latestInvoice is null)
        {
            return;
        }

        if (IsOlderInvoice(billing, latestInvoice))
        {
            throw new InvalidOperationException(
                $"Invoice '{billing.billNO}' cannot be {operation} because it belongs to a previous visit. Only the latest billNo for patient '{billing.pNo}' can be modified.");
        }
    }

    private static bool IsOlderInvoice(Billing candidate, Billing latest)
    {
        var dateComparison = candidate.bDate.CompareTo(latest.bDate);
        if (dateComparison != 0)
        {
            return dateComparison < 0;
        }

        return string.Compare(candidate.billNO, latest.billNO, StringComparison.OrdinalIgnoreCase) < 0;
    }

    private static string NormalizeRequired(string? value, string errorMessage)
    {
        var normalized = NormalizeOptional(value);
        return !string.IsNullOrWhiteSpace(normalized) ? normalized : throw new InvalidOperationException(errorMessage);
    }

    private static string? NormalizeOptional(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }

    private static void EnsureNoDuplicateItems(List<BillingDetail> details)
    {
        var hasDuplicates = details
            .GroupBy(x => x.drgName, StringComparer.OrdinalIgnoreCase)
            .Any(g => g.Count() > 1);

        if (hasDuplicates)
        {
            throw new InvalidOperationException("Duplicate bill items are not allowed in invoice details.");
        }
    }
}
