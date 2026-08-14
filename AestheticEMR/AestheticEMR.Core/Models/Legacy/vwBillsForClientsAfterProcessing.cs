using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwBillsForClientsAfterProcessing
{
    [StringLength(50)]
    public string? BatchNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime InvDate { get; set; }

    [StringLength(50)]
    public string InvNo { get; set; } = null!;

    [StringLength(50)]
    public string? CoyCode { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime dtDate { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string Service { get; set; } = null!;

    public double Price { get; set; }

    public double Qty { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? subTotal { get; set; }

    [StringLength(101)]
    public string? Period { get; set; }

    [StringLength(50)]
    public string? BillYear { get; set; }

    [StringLength(50)]
    public string? BillMonth { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? AmountInvoiced { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? AmountBilled { get; set; }

    [StringLength(50)]
    public string billNO { get; set; } = null!;

    public bool? isPost { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? BillHead { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? AdmDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DischDate { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Discount { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? DebtBF { get; set; }

    [StringLength(8000)]
    [Unicode(false)]
    public string? diagnosis { get; set; }
}
