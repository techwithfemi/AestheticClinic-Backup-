using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwInvoiceDetails2
{
    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [StringLength(50)]
    public string pNO { get; set; } = null!;

    [StringLength(50)]
    public string? BatchNo { get; set; }

    [StringLength(269)]
    [Unicode(false)]
    public string Company { get; set; } = null!;

    [StringLength(1001)]
    public string Fullname { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime InvDate { get; set; }

    [StringLength(50)]
    public string InvNo { get; set; } = null!;

    [StringLength(50)]
    public string CoyCode { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime dtDate { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string Service { get; set; } = null!;

    public double Price { get; set; }

    public double Qty { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? subTotal { get; set; }

    [StringLength(50)]
    public string? clientCat { get; set; }

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

    [StringLength(255)]
    [Unicode(false)]
    public string RetainName { get; set; } = null!;

    [StringLength(500)]
    [Unicode(false)]
    public string? BillHead { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Discount { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? DebtBF { get; set; }

    [StringLength(2)]
    public string? Mth { get; set; }

    [StringLength(30)]
    public string? Yr { get; set; }

    [StringLength(32)]
    public string? BatchVal { get; set; }

    [StringLength(8000)]
    [Unicode(false)]
    public string? diagnosis { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? BillDate { get; set; }

    [StringLength(7)]
    public string? BatchNo2 { get; set; }

    [StringLength(50)]
    public string RetainCode { get; set; } = null!;

    public long SNO { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? CoyID { get; set; }
}
