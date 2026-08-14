using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhRecordsAndBill
{
    public int recID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime recDate { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [StringLength(50)]
    public string pNO { get; set; } = null!;

    [StringLength(50)]
    public string? clientCat { get; set; }

    [StringLength(100)]
    public string? Remarks { get; set; }

    [StringLength(50)]
    public string? empID { get; set; }

    [StringLength(50)]
    public string clinicType { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? htime { get; set; }

    [StringLength(2)]
    public string? Mth { get; set; }

    [StringLength(30)]
    public string? Yr { get; set; }

    [StringLength(7)]
    public string? BatchNo { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Coyname { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? BillDate { get; set; }

    [StringLength(50)]
    public string RetainCode { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string RetainName { get; set; } = null!;

    [StringLength(50)]
    public string? clientCatID { get; set; }

    [StringLength(50)]
    public string? ClientType { get; set; }

    [StringLength(1001)]
    public string Fullname { get; set; } = null!;

    [StringLength(30)]
    public string? MonthNAme { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? AmountBilled { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? AmountPaid { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? PhoneNo { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Discount { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? DebtBF { get; set; }
}
