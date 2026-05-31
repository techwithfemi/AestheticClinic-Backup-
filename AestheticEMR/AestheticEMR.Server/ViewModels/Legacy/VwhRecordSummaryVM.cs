namespace AestheticEMR.Server.ViewModels.Legacy;

public class VwhRecordSummaryVM
{
    public string ConsultId { get; set; } = string.Empty;
    public string PNo { get; set; } = string.Empty;
    public string? ClientCat { get; set; }
    public string ClinicType { get; set; } = string.Empty;
    public string? Coyname { get; set; }
    public string RetainName { get; set; } = string.Empty;
    public string Fullname { get; set; } = string.Empty;
    public DateTime? Dob { get; set; }
    public int? Age { get; set; }
    public string? PhoneNo { get; set; }
    public string? RetainCode { get; set; }
    public string? RetainId { get; set; }
}
