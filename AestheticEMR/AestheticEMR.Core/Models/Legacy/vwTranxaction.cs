using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwTranxaction
{
    public long SNO { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime AttndDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? BillDate { get; set; }

    [StringLength(1001)]
    public string FullName { get; set; } = null!;

    [StringLength(50)]
    public string PNo { get; set; } = null!;

    [StringLength(50)]
    public string billNO { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Bill { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? AmountBilled { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? DebtBF { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Discount { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? AmountPaid { get; set; }

    [Column(TypeName = "decimal(20, 2)")]
    public decimal? Balance { get; set; }

    public int RunningTotal { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string Remarks { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string Comapany { get; set; } = null!;

    [Column(TypeName = "decimal(20, 2)")]
    public decimal? Debt { get; set; }

    [StringLength(150)]
    public string? clientID { get; set; }

    [StringLength(8000)]
    [Unicode(false)]
    public string? diagnosis { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? PhoneNo { get; set; }

    [StringLength(50)]
    public string retainCode { get; set; } = null!;
}
