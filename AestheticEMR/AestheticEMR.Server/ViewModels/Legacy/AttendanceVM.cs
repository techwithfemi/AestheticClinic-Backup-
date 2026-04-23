using System.ComponentModel.DataAnnotations;

namespace AestheticEMR.Server.ViewModels.Legacy;

public class AttendanceVM
{
    public string? ConsultId { get; set; }

    public int RecId { get; set; }

    [Required]
    public DateTime RecDate { get; set; } = DateTime.Today;

    [Required]
    [StringLength(50)]
    public string PNo { get; set; } = null!;

    [StringLength(50)]
    public string? ClientCat { get; set; }

    [Required]
    [StringLength(50)]
    public string ClinicType { get; set; } = null!;

    public DateTime? Htime { get; set; }

    public byte? PatVal { get; set; }

    public bool? Suppres { get; set; }

    public DateOnly? ExitDate { get; set; }

    [StringLength(50)]
    public string? ExitDateComment { get; set; }

    [StringLength(50)]
    public string? Coyname { get; set; }

    public DateTime? BillDate { get; set; }

    [StringLength(50)]
    public string? AttndStatus { get; set; }

    public bool? AttendedToByImmume { get; set; }

    [StringLength(100)]
    public string? HmoRef { get; set; }

    public DateTime? EntryDate { get; set; }

    public DateTime? EntryTime { get; set; }
}
