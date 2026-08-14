using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwInvoiceDetailsCoyBeforeProcessing
{
    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [StringLength(7)]
    public string? BatchNo { get; set; }

    [StringLength(269)]
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

    [StringLength(1001)]
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

    [Column(TypeName = "datetime")]
    public DateTime? BillDate { get; set; }
}
