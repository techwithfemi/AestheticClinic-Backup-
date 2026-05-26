namespace AestheticEMR.Core.Services.Legacy;

public sealed class BillingAppDefaults
{
    public string AppName { get; init; } = "Billing";
    public string ClientCategoryPrivate { get; init; } = "PRIVATE";
    public DateOnly EntryDate { get; init; } = DateOnly.FromDateTime(DateTime.Today);
    public int PriceColumnIndex { get; init; } = 3;
    public string MailAttachmentDirectory { get; init; } = string.Empty;

    public string BillHead { get; init; } = string.Empty;
    public string BillHead2 { get; init; } = string.Empty;
    public string BillHead3 { get; init; } = string.Empty;
    public string BillHead4 { get; init; } = string.Empty;
    public string BillPixPath { get; init; } = string.Empty;
    public string BillPixPath2 { get; init; } = string.Empty;
    public string BillPixPath3 { get; init; } = string.Empty;

    public string LabHead { get; init; } = string.Empty;
    public string LabHead2 { get; init; } = string.Empty;
    public string LabHead3 { get; init; } = string.Empty;
    public string LabAcctNo { get; init; } = string.Empty;
    public string LabPixPath { get; init; } = string.Empty;

    public string DefaultPrinter { get; init; } = string.Empty;

    public IReadOnlyDictionary<string, string> Values { get; init; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

    public string Get(string key, string defaultValue = "")
    {
        return Values.TryGetValue(key, out var value) ? value : defaultValue;
    }
}
