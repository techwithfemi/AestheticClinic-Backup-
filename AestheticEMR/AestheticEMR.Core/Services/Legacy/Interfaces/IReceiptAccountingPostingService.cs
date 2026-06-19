namespace AestheticEMR.Core.Services.Legacy.Interfaces;

/// <summary>
/// Posts a saved receipt (payment) to the Accounting database using the same
/// <c>InsertTranxaction</c> stored procedure that invoice billing uses.
/// Mirrors the VB6 receipt "PostToAccounts" logic: debit the cash/bank account,
/// credit the patient's receivable account, then verify the period books balance.
/// </summary>
public interface IReceiptAccountingPostingService
{
    /// <summary>
    /// Posts the receipt's double-entry to Accounting.
    /// </summary>
    /// <returns>
    /// <c>true</c> when the entries were posted and committed (caller should mark
    /// the receipt rows <c>isPost = true</c>); <c>false</c> when posting is disabled
    /// by configuration or could not be completed (receipt stays unposted/re-postable).
    /// </returns>
    Task<bool> PostReceiptAsync(ReceiptAccountingPostRequest request, CancellationToken cancellationToken = default);
}

/// <summary>
/// Immutable inputs required to post a single receipt to Accounting.
/// </summary>
public sealed record ReceiptAccountingPostRequest
{
    /// <summary>Receipt number (used in transaction descriptions and as the bill reference).</summary>
    public required string ReceiptNo { get; init; }

    /// <summary>Bill number the receipt was raised against.</summary>
    public required string BillNo { get; init; }

    /// <summary>Single transaction id shared by the debit and credit legs (the receipt's voucher no).</summary>
    public required string TranId { get; init; }

    /// <summary>Pay type: Cash, Cheque, Transfer or POS.</summary>
    public required string PayType { get; init; }

    /// <summary>Amount paid on this receipt (always posted as a positive value on the debit leg).</summary>
    public required decimal Amount { get; init; }

    /// <summary>Receipt/entry date — drives the accounting transaction date and period.</summary>
    public required DateTime EntryDate { get; init; }

    /// <summary>Company / retainership id (coyID) — "0001" for private.</summary>
    public required string CoyId { get; init; }

    /// <summary>Patient's receivable GL account (VwhRecord.AcctId); falls back to the configured Acct_Receivable.</summary>
    public string? ReceivableAccountNo { get; init; }

    /// <summary>Bank GL account selected in the dialog for non-cash pay types; falls back to the configured default.</summary>
    public string? BankAccountNo { get; init; }

    /// <summary>Patient name for the credit-leg description.</summary>
    public string? PatientName { get; init; }
}
