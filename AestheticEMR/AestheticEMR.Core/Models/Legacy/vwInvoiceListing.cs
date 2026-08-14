using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwInvoiceListing
{
    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [StringLength(7)]
    public string? BatchNo { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string Company { get; set; } = null!;

    [StringLength(50)]
    public string CoyCode { get; set; } = null!;

    [StringLength(50)]
    public string? clientCat { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? AmountBilled { get; set; }

    [StringLength(50)]
    public string billNO { get; set; } = null!;

    [StringLength(50)]
    public string? InvNo { get; set; }

    [StringLength(406)]
    public string Fullname { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string drgName { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? AdmDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DischDate { get; set; }

    public int? NoOfDays { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? BillHead { get; set; }

    [StringLength(8000)]
    [Unicode(false)]
    public string? diagnosis { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? BillDate { get; set; }

    [StringLength(2)]
    public string? Mth { get; set; }

    [StringLength(30)]
    public string? Yr { get; set; }

    [StringLength(32)]
    public string? BatchVal { get; set; }

    [StringLength(50)]
    public string? AcctID { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string RetainName { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? AmountPaid { get; set; }
}
