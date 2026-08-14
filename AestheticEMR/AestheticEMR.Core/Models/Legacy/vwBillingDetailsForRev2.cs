using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwBillingDetailsForRev2
{
    public long? SNO { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? BillDate { get; set; }

    [StringLength(1001)]
    public string Fullname { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string Company { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string? pCatID { get; set; }

    [StringLength(50)]
    public string? RevType { get; set; }

    [StringLength(50)]
    public string? billNO { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? Service { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal subTotal { get; set; }

    [StringLength(50)]
    public string? clientCatID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? retainID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? RetainCode { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? BillBy { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? treatedBy { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? billType { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string Consultant { get; set; } = null!;

    [StringLength(50)]
    public string? ClientType { get; set; }

    [StringLength(50)]
    public string pNO { get; set; } = null!;

    [StringLength(50)]
    public string? clientCat { get; set; }

    [StringLength(50)]
    public string PatCode { get; set; } = null!;

    [StringLength(13)]
    public string? PatCode2 { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? AmountBilled { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? AmountPaid { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? coyCode { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string Processed { get; set; } = null!;

    [StringLength(3)]
    [Unicode(false)]
    public string Locked { get; set; } = null!;

    [StringLength(32)]
    public string? BatchVal { get; set; }

    [StringLength(7)]
    public string? BatchNo { get; set; }

    [StringLength(50)]
    public string clinicType { get; set; } = null!;

    public string? InvNo { get; set; }

    public bool? isProcess { get; set; }
}
