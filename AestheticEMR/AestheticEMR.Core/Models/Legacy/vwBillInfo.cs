using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwBillInfo
{
    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [Column(TypeName = "decimal(38, 2)")]
    public decimal AmountGen { get; set; }

    [Column(TypeName = "decimal(38, 2)")]
    public decimal AmountBilled { get; set; }

    [Column(TypeName = "decimal(38, 2)")]
    public decimal AmountBilled2 { get; set; }

    [Column(TypeName = "decimal(38, 2)")]
    public decimal? AmountPayable { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal AmountPaid { get; set; }

    [Column(TypeName = "decimal(38, 2)")]
    public decimal? AmountBal { get; set; }

    [Column(TypeName = "decimal(38, 2)")]
    public decimal? AmountCap { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Discount { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Debt { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Debt2 { get; set; }
}
