using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwInvoiceDetailsCoy2ForUnProcessed
{
    [Column(TypeName = "datetime")]
    public DateTime? BillDate { get; set; }

    [StringLength(50)]
    public string? BatchNo { get; set; }

    [StringLength(269)]
    [Unicode(false)]
    public string Company { get; set; } = null!;

    [Column(TypeName = "decimal(38, 2)")]
    public decimal? AmountBilled { get; set; }

    [StringLength(50)]
    public string InvNo { get; set; } = null!;

    [StringLength(1001)]
    public string Fullname { get; set; } = null!;

    [StringLength(50)]
    public string pNO { get; set; } = null!;

    [StringLength(50)]
    public string billNO { get; set; } = null!;

    [StringLength(8000)]
    [Unicode(false)]
    public string? diagnosis { get; set; }

    [StringLength(7)]
    public string? BatchNo2 { get; set; }

    [StringLength(50)]
    public string CoyCode { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? Date { get; set; }
}
