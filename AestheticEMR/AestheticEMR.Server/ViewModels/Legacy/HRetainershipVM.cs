using System;
using System.ComponentModel.DataAnnotations;

namespace AestheticEMR.Server.ViewModels.Legacy;

public class HRetainershipVM
{
    [StringLength(50)]
    public string? RetainId { get; set; }

    [StringLength(50)]
    public string? RetainCode { get; set; }

    [Required]
    [StringLength(255)]
    public string RetainName { get; set; } = null!;

    [StringLength(50)]
    public string? ClientCatId { get; set; }

    [StringLength(50)]
    public string? ClientType { get; set; }

    [StringLength(200)]
    public string? Address { get; set; }

    [StringLength(30)]
    public string? PhoneNo { get; set; }

    [StringLength(100)]
    public string? Email { get; set; }

    [StringLength(100)]
    public string? Contact { get; set; }

    [Range(0, double.MaxValue)]
    public double? ProfFee { get; set; }

    [Range(0, double.MaxValue)]
    public double? Debt { get; set; }

    [StringLength(50)]
    public string? AcctId { get; set; }

    [StringLength(50)]
    public string? DebtType { get; set; }

    [StringLength(50)]
    public string? Active { get; set; }

    [StringLength(50)]
    public string? UseTariff { get; set; }

    [Range(0, 100)]
    public double? Pcent { get; set; }

    [Range(1, 31)]
    public int? BillEndDate { get; set; } = 31;

    [Range(0, (double)decimal.MaxValue)]
    public decimal? RegAmount { get; set; } = 0;

    [Range(0, (double)decimal.MaxValue)]
    public decimal? ConAmount { get; set; } = 0;

    [Range(0, (double)decimal.MaxValue)]
    public decimal? CardRenewAmount { get; set; } = 0;

    public DateTime? RetainDate { get; set; } = DateTime.Today;

    public DateTime? EntryDate { get; set; }

    public DateTime? EntryTime { get; set; }

    [StringLength(100)]
    public string? ClientName { get; set; }

    [StringLength(100)]
    public string? AppName { get; set; }
}