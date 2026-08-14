using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hRetainershipOLD")]
public partial class hRetainershipOLD
{
    [Column(TypeName = "smalldatetime")]
    public DateTime? retainDate { get; set; }

    [StringLength(50)]
    public string? retainID { get; set; }

    [StringLength(50)]
    public string? RetainCode { get; set; }

    [StringLength(150)]
    public string? retainName { get; set; }

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
}
