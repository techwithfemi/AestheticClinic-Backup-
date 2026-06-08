namespace AestheticEMR.Server.ViewModels.Legacy;

public class QryhvisitsForTodayVM
{
    public DateTime RecDate { get; set; }
    public string PNo { get; set; } = string.Empty;
    public string Fullname { get; set; } = string.Empty;
    public string ClinicType { get; set; } = string.Empty;
    public string? ClientCat { get; set; }
    public string ConsultId { get; set; } = string.Empty;
    public string CoyName { get; set; } = string.Empty;
    public string? RetainName { get; set; }
}
