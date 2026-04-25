using System.ComponentModel.DataAnnotations;

namespace AestheticEMR.Server.ViewModels.Legacy;

public class AppointmentVM
{
    public long Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Pno { get; set; } = null!;

    [Required]
    public DateTime? ApptDate { get; set; }

    [Required]
    public DateTime? ApptTime { get; set; }

    [Required]
    [StringLength(100)]
    public string ClinicType { get; set; } = null!;

    [StringLength(1000)]
    public string? Remarks { get; set; }

    [StringLength(50)]
    public string? EmpID { get; set; }

    public DateTime? EntryDate { get; set; }
    public DateTime? EntryTime { get; set; }
}
