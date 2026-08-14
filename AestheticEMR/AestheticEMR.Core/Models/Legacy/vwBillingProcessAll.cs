using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwBillingProcessAll
{
    [Column(TypeName = "datetime")]
    public DateTime AttdDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? BillDate { get; set; }

    [StringLength(50)]
    public string billNO { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? RetainCode { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string CoyName { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? AmountBilled { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? AmountPaid { get; set; }

    public int? YearCode { get; set; }

    [StringLength(50)]
    public string? clientCat { get; set; }

    public bool? isProcess { get; set; }

    public string? InvNo { get; set; }

    [StringLength(1001)]
    public string Fullname { get; set; } = null!;

    [StringLength(100)]
    public string? Remarks { get; set; }

    public long ID { get; set; }

    [StringLength(2)]
    public string? Mth { get; set; }

    [StringLength(30)]
    public string? Yr { get; set; }

    [StringLength(32)]
    public string? BatchVal { get; set; }

    [StringLength(7)]
    public string? BatchNo { get; set; }

    [StringLength(50)]
    public string clinicType { get; set; } = null!;

    [StringLength(2)]
    public string? MonthCode { get; set; }

    [StringLength(50)]
    public string pNO { get; set; } = null!;

    public string? diagnosis { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? retainID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? AttndBillDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? AdmDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DischDate { get; set; }

    [StringLength(50)]
    public string? BillingMonth { get; set; }

    public int? BillingYear { get; set; }

    public bool isSigned { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ExitDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? htime { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? PhoneNo { get; set; }

    public int? BillEndDate { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string? Title { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Discount { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? DebtBF { get; set; }
}
