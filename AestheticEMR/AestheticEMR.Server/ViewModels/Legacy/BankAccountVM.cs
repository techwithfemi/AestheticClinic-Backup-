namespace AestheticEMR.Server.ViewModels.Legacy;

/// <summary>Lightweight projection of vwAccountsInfo for bank-account selection on receipts.</summary>
public class BankAccountVM
{
    public string AccountId { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
}
