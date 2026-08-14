using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hRetainership_260325")]
public partial class hRetainership_260325
{
    [Column(TypeName = "smalldatetime")]
    public DateTime? retainDate { get; set; }

    [StringLength(50)]
    public string retainID { get; set; } = null!;

    [StringLength(50)]
    public string RetainCode { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string RetainName { get; set; } = null!;

    [StringLength(50)]
    public string? clientCatID { get; set; }

    [StringLength(50)]
    public string? ClientType { get; set; }

    [StringLength(50)]
    public string? Address { get; set; }

    [StringLength(50)]
    public string? PhoneNo { get; set; }

    [StringLength(50)]
    public string? email { get; set; }

    [StringLength(50)]
    public string? Contact { get; set; }

    public double? ProfFee { get; set; }

    public double? Debt { get; set; }

    [StringLength(50)]
    public string? AcctID { get; set; }

    [StringLength(50)]
    public string? DebtType { get; set; }

    [StringLength(50)]
    public string? Active { get; set; }

    [StringLength(50)]
    public string? UseTariff { get; set; }

    public double? PCent { get; set; }

    public int? BillEndDate { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? RegAmount { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? ConAmount { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? CardRenewAmount { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EntryDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EntryTime { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? ClientName { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? AppName { get; set; }
}
