namespace AestheticEMR.Server.ViewModels.Legacy;

public class BillingSyncStatusVM
{
    public string EffectiveMode { get; set; } = "HospitalOnly";
    public string? PrimaryDataSource { get; set; }
    public string? PrimaryDatabase { get; set; }
    public List<string> IncludedDatabases { get; set; } = [];
    public List<string> Warnings { get; set; } = [];
}
