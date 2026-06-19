using AestheticEMR.Core.Services.Legacy.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;

namespace AestheticEMR.Core.Services.Legacy;

/// <summary>
/// Same-instance Accounting posting for receipts. Converted from the VB6
/// receipt "PostToAccounts" routine (frmBillingVerify): debit the cash/bank
/// account, credit the patient's receivable account, verify the period books
/// balance, then commit. Uses the same <c>InsertTranxaction</c> stored procedure
/// as invoice billing.
/// </summary>
public class ReceiptAccountingPostingService(
    IConfiguration configuration,
    IEmrAppDefaultsService emrAppDefaultsService,
    ILogger<ReceiptAccountingPostingService> logger) : IReceiptAccountingPostingService
{
    public async Task<bool> PostReceiptAsync(ReceiptAccountingPostRequest request, CancellationToken cancellationToken = default)
    {
        var appDefaults = await emrAppDefaultsService.GetAsync(cancellationToken);
        var values = appDefaults.Values;

        // ── Gate: VB6 "If AcctPostOn = True And AcctPostType_Cash = "AUTO"" ──
        var acctPostOn = string.Equals(values.GetValueOrDefault("AcctPostOn", "false"), "true", StringComparison.OrdinalIgnoreCase);
        var acctPostTypeCash = values.GetValueOrDefault("AcctPostType_Cash", "AUTO");
        if (!acctPostOn || !string.Equals(acctPostTypeCash, "AUTO", StringComparison.OrdinalIgnoreCase))
        {
            logger.LogInformation(
                "Receipt {ReceiptNo}: accounting posting skipped (AcctPostOn={AcctPostOn}, AcctPostType_Cash={AcctPostTypeCash}).",
                request.ReceiptNo, acctPostOn, acctPostTypeCash);
            return false;
        }

        // ── Resolve accounts ────────────────────────────────────────────────
        var payType = (request.PayType ?? string.Empty).Trim().ToUpperInvariant();

        // Credit leg target: patient receivable account, else configured default.
        var receivableAcct = FirstNonEmpty(request.ReceivableAccountNo, values.GetValueOrDefault("Acct_Receivable"));
        if (string.IsNullOrWhiteSpace(receivableAcct))
        {
            logger.LogWarning("Receipt {ReceiptNo}: no receivable account resolved; accounting posting skipped.", request.ReceiptNo);
            return false;
        }

        // Debit leg target + TranCat by pay type (VB6: CASH -> "h", others -> "i").
        // For non-cash, prefer the bank account selected in the dialog, else the configured default.
        string? debitAcct;
        string tranCat;
        switch (payType)
        {
            case "CASH":
                debitAcct = values.GetValueOrDefault("AcctNoCash");
                tranCat = "h";
                break;
            case "POS":
                debitAcct = FirstNonEmpty(request.BankAccountNo, values.GetValueOrDefault("AcctNoPOS"));
                tranCat = "i";
                break;
            case "CHEQUE":
                debitAcct = FirstNonEmpty(request.BankAccountNo, values.GetValueOrDefault("AcctNoCheque"));
                tranCat = "i";
                break;
            case "TRANSFER":
                debitAcct = FirstNonEmpty(request.BankAccountNo, values.GetValueOrDefault("AcctNoTransfer"));
                tranCat = "i";
                break;
            default:
                logger.LogWarning("Receipt {ReceiptNo}: unsupported pay type '{PayType}'; accounting posting skipped.", request.ReceiptNo, request.PayType);
                return false;
        }

        if (string.IsNullOrWhiteSpace(debitAcct))
        {
            logger.LogWarning("Receipt {ReceiptNo}: no debit account resolved for pay type '{PayType}'; accounting posting skipped.", request.ReceiptNo, request.PayType);
            return false;
        }

        var costCenter = values.GetValueOrDefault("AcctCostCenter", "0001");
        var coyId = string.IsNullOrWhiteSpace(request.CoyId) ? values.GetValueOrDefault("CoyID", "0001") : request.CoyId;
        // Period in MM/YYYY (same convention as the invoice posting path).
        var period = request.EntryDate.Month.ToString("D2") + "/" + request.EntryDate.Year.ToString();

        var patientName = string.IsNullOrWhiteSpace(request.PatientName) ? "Patient" : request.PatientName!.Trim();
        var debitDescription = payType switch
        {
            "CASH" => $"Paid Cash:( Rct No: {request.ReceiptNo})",
            "POS" => $"Paid Via POS:( Rct No: {request.ReceiptNo})",
            "CHEQUE" => $"Paid via Cheque ( Rct No: {request.ReceiptNo})",
            "TRANSFER" => $"Paid via Transfer. Rct No: ({request.ReceiptNo})",
            _ => $"Payment ( Rct No: {request.ReceiptNo})"
        };
        var creditDescription = $"Payment by {patientName} Rct No: ({request.ReceiptNo})";

        var accountingConnStr = GetAccountingConnectionString();
        if (string.IsNullOrWhiteSpace(accountingConnStr))
        {
            logger.LogWarning("Receipt {ReceiptNo}: AccountingConnection is not configured; accounting posting skipped.", request.ReceiptNo);
            return false;
        }

        try
        {
            await using var conn = new SqlConnection(accountingConnStr);
            await conn.OpenAsync(cancellationToken);
            await using var tran = (SqlTransaction)await conn.BeginTransactionAsync(cancellationToken);
            try
            {
                // Debit: cash/bank account receives money (positive amount).
                await CallInsertTranxactionAsync(conn, tran, request, debitAcct!, request.Amount, debitDescription, tranCat, costCenter, period, coyId, cancellationToken);

                // Credit: patient's receivable is reduced (negative amount), VB6 TranCat "h".
                await CallInsertTranxactionAsync(conn, tran, request, receivableAcct!, -request.Amount, creditDescription, "h", costCenter, period, coyId, cancellationToken);

                // ── Confirm Dr = Cr for the period (VB6 dbo.TranBalance check) ──
                var balance = await GetTranBalanceAsync(conn, tran, period, coyId, cancellationToken);
                if (balance != 0m)
                {
                    await tran.RollbackAsync(cancellationToken);
                    logger.LogWarning(
                        "Receipt {ReceiptNo}: accounting period {Period} did not balance (TranBalance={Balance}); posting rolled back, receipt left unposted.",
                        request.ReceiptNo, period, balance);
                    return false;
                }

                await tran.CommitAsync(cancellationToken);
                logger.LogInformation("Receipt {ReceiptNo}: posted to accounting (debit {DebitAcct}, credit {CreditAcct}, amount {Amount}).",
                    request.ReceiptNo, debitAcct, receivableAcct, request.Amount);
                return true;
            }
            catch
            {
                await tran.RollbackAsync(cancellationToken);
                throw;
            }
        }
        catch (Exception ex)
        {
            // Resilience (per VB6): never lose the receipt over an accounting failure.
            // The receipt stays saved with isPost = false and can be re-posted later.
            logger.LogError(ex, "Receipt {ReceiptNo}: accounting posting failed; receipt left unposted.", request.ReceiptNo);
            return false;
        }
    }

    private async Task CallInsertTranxactionAsync(
        SqlConnection conn,
        SqlTransaction tran,
        ReceiptAccountingPostRequest request,
        string accountNo,
        decimal amount,
        string description,
        string tranCat,
        string costCenter,
        string period,
        string coyId,
        CancellationToken cancellationToken)
    {
        await using var cmd = conn.CreateCommand();
        cmd.Transaction = tran;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "InsertTranxaction";

        var tranDate = request.EntryDate.Date;
        cmd.Parameters.AddWithValue("@TranID", request.TranId);
        cmd.Parameters.AddWithValue("@AccountNo", accountNo);
        cmd.Parameters.AddWithValue("@TranNo", request.TranId);
        cmd.Parameters.AddWithValue("@TranDate", tranDate);
        cmd.Parameters.AddWithValue("@CostCenterID", costCenter);
        cmd.Parameters.AddWithValue("@Amount", amount);
        cmd.Parameters.AddWithValue("@Description", description);
        cmd.Parameters.AddWithValue("@TranCat", tranCat);
        cmd.Parameters.AddWithValue("@EntryDate", DateTime.Now);
        cmd.Parameters.AddWithValue("@Period", period);
        cmd.Parameters.AddWithValue("@CoyID2", coyId);
        cmd.Parameters.AddWithValue("@UserName", "system");
        cmd.Parameters.AddWithValue("@SNoID", 0);
        cmd.Parameters.AddWithValue("@BillNO", request.BillNo);
        cmd.Parameters.AddWithValue("@Reversed", false);
        cmd.Parameters.AddWithValue("@ReversedPair", 0);

        await cmd.ExecuteNonQueryAsync(cancellationToken);
    }

    private static async Task<decimal> GetTranBalanceAsync(
        SqlConnection conn,
        SqlTransaction tran,
        string period,
        string coyId,
        CancellationToken cancellationToken)
    {
        await using var cmd = conn.CreateCommand();
        cmd.Transaction = tran;
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = "SELECT dbo.TranBalance(@Period, @CoyID) AS Amount";
        cmd.Parameters.AddWithValue("@Period", period);
        cmd.Parameters.AddWithValue("@CoyID", coyId);

        var result = await cmd.ExecuteScalarAsync(cancellationToken);
        if (result is null || result == DBNull.Value)
        {
            // VB6 treated a missing balance row as a failure (rollback).
            return decimal.MinValue;
        }
        return Convert.ToDecimal(result);
    }

    private string? GetAccountingConnectionString()
    {
        return configuration.GetConnectionString("AccountingConnection");
    }

    private static string? FirstNonEmpty(params string?[] candidates)
    {
        foreach (var candidate in candidates)
        {
            if (!string.IsNullOrWhiteSpace(candidate))
            {
                return candidate;
            }
        }
        return null;
    }
}
