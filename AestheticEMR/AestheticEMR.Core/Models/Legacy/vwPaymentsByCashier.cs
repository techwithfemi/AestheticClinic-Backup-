using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwPaymentsByCashier
{
    [Column(TypeName = "datetime")]
    public DateTime ReceiptDate { get; set; }

    [StringLength(50)]
    public string ReceiptNo { get; set; } = null!;

    [StringLength(50)]
    public string pNO { get; set; } = null!;

    [StringLength(50)]
    public string billNO { get; set; } = null!;

    [StringLength(1001)]
    public string? Fullname { get; set; }

    [StringLength(1000)]
    public string paymentFor { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal AmountBilled { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal AmountPaid { get; set; }

    [StringLength(101)]
    public string? EmpName { get; set; }

    [StringLength(50)]
    public string? empID { get; set; }

    [Column(TypeName = "decimal(19, 2)")]
    public decimal? Balance { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? rTime { get; set; }

    [StringLength(50)]
    public string payType { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string? PhoneNo { get; set; }
}
