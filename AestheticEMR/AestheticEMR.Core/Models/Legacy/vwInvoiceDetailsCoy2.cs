using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwInvoiceDetailsCoy2
{
    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [StringLength(50)]
    public string? BatchNo { get; set; }

    [StringLength(269)]
    [Unicode(false)]
    public string Company { get; set; } = null!;

    [StringLength(50)]
    public string? CoyCode { get; set; }

    [Column(TypeName = "decimal(38, 2)")]
    public decimal? aubTotal { get; set; }

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

    [Column(TypeName = "datetime")]
    public DateTime? BillDate { get; set; }

    [Column(TypeName = "decimal(38, 2)")]
    public decimal? AmountBilled { get; set; }

    public bool? isPost { get; set; }

    [StringLength(50)]
    public string? BillYear { get; set; }

    [StringLength(50)]
    public string? BillMonth { get; set; }

    [StringLength(101)]
    public string? Period { get; set; }

    [StringLength(50)]
    public string? AcctID { get; set; }
}
