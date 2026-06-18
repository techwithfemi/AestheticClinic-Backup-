using System.ComponentModel.DataAnnotations;

namespace AestheticEMR.Server.ViewModels.Legacy;

public class ConsultingDetailsForBillingVM
{
    [Required]
    [StringLength(50)]
    public string ConsultId { get; set; } = null!;

    [StringLength(100)]
    public string? ClinicType { get; set; }

    [StringLength(200)]
    public string? Purpose { get; set; }

    [StringLength(200)]
    public string? Diagnosis { get; set; }

    [StringLength(120)]
    public string? TreatedBy { get; set; }

    public DateTime? CDate { get; set; }

    [StringLength(20)]
    public string? CTime { get; set; }

    public string? Investigate { get; set; }

    public string? Prescription { get; set; }

    public string? Services { get; set; }

    public string? BillRemarks { get; set; }
}
