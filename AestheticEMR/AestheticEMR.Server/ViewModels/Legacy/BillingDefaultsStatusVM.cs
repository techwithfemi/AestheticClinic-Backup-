namespace AestheticEMR.Server.ViewModels.Legacy;

public class BillingDefaultsStatusVM
{
    public bool Loaded { get; set; }
    public string? Error { get; set; }
    public DateTimeOffset? LastCheckedAtUtc { get; set; }
}
