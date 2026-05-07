using System.ComponentModel.DataAnnotations;

namespace AestheticEMR.Server.ViewModels.Legacy;

public class ServiceTariffVM
{
    public long Sno { get; set; }

    [Required]
    [StringLength(255)]
    public string Service { get; set; } = null!;

    [StringLength(500)]
    public string? Category { get; set; }

    [Range(0, double.MaxValue)]
    public double? Price { get; set; }

    [Required]
    [StringLength(255)]
    public string Company { get; set; } = null!;

    [StringLength(50)]
    public string? CoyId { get; set; }

    [StringLength(255)]
    public string? Remarks { get; set; }

    [StringLength(250)]
    public string? CoyName { get; set; }

    [StringLength(50)]
    public string? Capitated { get; set; }

    [StringLength(50)]
    public string? TariffStatus { get; set; }

    [StringLength(200)]
    public string? RevType { get; set; }

    [StringLength(50)]
    public string? UsersCat { get; set; }
}
