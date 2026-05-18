using AestheticEMR.Server.Attributes;
using System.ComponentModel.DataAnnotations;

namespace AestheticEMR.Server.ViewModels.Legacy;

public class BillingDetailVM
{
    public long SNO { get; set; }

    [Required]
    [StringLength(200)]
    public string DrgName { get; set; } = null!;

    [Range(0, double.MaxValue)]
    public double Price { get; set; }

    [Range(1, double.MaxValue)]
    public double Qty { get; set; } = 1;

    [StringLength(50)]
    public string? BillType { get; set; }

    [StringLength(50)]
    public string? ConID { get; set; }

    [StringLength(10)]
    public string? Capitated { get; set; }

    [StringLength(100)]
    public string? Dosage { get; set; }

    [StringLength(100)]
    public string? Category { get; set; }

    [StringLength(200)]
    public string? BillTo { get; set; }

    [StringLength(200)]
    public string? CoyName { get; set; }

    [StringLength(100)]
    public string? BillHead { get; set; }

    [StringLength(100)]
    public string? RevType { get; set; }

    [StringLength(100)]
    public string? DRGCode { get; set; }

    public bool IsPost { get; set; }

    public bool? IsRct { get; set; }

    [StringLength(50)]
    public string? BillBy { get; set; }

    [StringLength(100)]
    public string? TreatedBy { get; set; }

    [StringLength(100)]
    public string? Dept { get; set; }

    public bool? IsOLD { get; set; }

    [StringLength(200)]
    public string? ClientName { get; set; }

    [StringLength(100)]
    public string? AppName { get; set; }

    [StringLength(100)]
    public string? RevClinic { get; set; }

    public bool? Reversed { get; set; }

    [StringLength(500)]
    public string? Remarks { get; set; }

    public bool? Suppres { get; set; }

    [Range(1, int.MaxValue)]
    public int? AppVersion { get; set; } = 1;
}

public class BillingVM
{
    [Required]
    [StringLength(50)]
    public string BillNo { get; set; } = null!;

    [Required]
    public DateOnly BDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    [Required]
    [StringLength(50)]
    public string PNo { get; set; } = null!;

    [StringLength(50)]
    public string? ClientID { get; set; }

    [Range(0, (double)decimal.MaxValue)]
    public decimal? DebtBF { get; set; } = 0;

    [Range(0, (double)decimal.MaxValue)]
    public decimal? AmountBilled { get; set; }

    [Range(0, (double)decimal.MaxValue)]
    public decimal? Discount { get; set; } = 0;

    [Range(0, (double)decimal.MaxValue)]
    public decimal? AmountPaid { get; set; } = 0;

    [StringLength(500)]
    public string? AmountBilledInWord { get; set; }

    [StringLength(50)]
    public string? BillingMonth { get; set; }

    public int? BillingYear { get; set; }

    public bool? IsPaid { get; set; }

    [StringLength(50)]
    public string? BillType { get; set; }

    public bool? IsProcess { get; set; }

    public DateTime? AdmDate { get; set; }

    public DateTime? DischDate { get; set; }

    public DateTime? TimeVal { get; set; }

    [StringLength(100)]
    public string? ApprvCode { get; set; }

    public bool? IsPost { get; set; }

    [MinimumCount(1)]
    public List<BillingDetailVM> Details { get; set; } = [];
}
