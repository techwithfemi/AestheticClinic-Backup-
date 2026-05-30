using AestheticEMR.Server.Attributes;
using System.ComponentModel.DataAnnotations;

namespace AestheticEMR.Server.ViewModels.Legacy;

public class BillingDetailVM
{
    public long SNO { get; set; }

    [StringLength(100)]
    public string? TranID { get; set; }

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

    [StringLength(100)]
    public string? RevenueType { get; set; }

    [StringLength(100)]
    public string? Category { get; set; }

    [StringLength(100)]
    public string? BillTo { get; set; }

    [StringLength(100)]
    public string? CoyName { get; set; }
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

    [Range(0, double.MaxValue)]
    public double? Tax { get; set; } = 0;

    [StringLength(50)]
    public string? BillType { get; set; }

    public bool? IsPaid { get; set; }

    [MinimumCount(1)]
    public List<BillingDetailVM> Details { get; set; } = [];
}
