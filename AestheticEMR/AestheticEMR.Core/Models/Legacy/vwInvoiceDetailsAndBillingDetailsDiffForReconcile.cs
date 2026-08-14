using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwInvoiceDetailsAndBillingDetailsDiffForReconcile
{
    public long SNO { get; set; }

    [StringLength(1001)]
    public string Fullname { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string Company { get; set; } = null!;

    [StringLength(50)]
    public string? RevType { get; set; }

    [StringLength(50)]
    public string billNO { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime AttndDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? BillDate { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string ServiceRev { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal subTotalRev { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string ServiceInv { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? subTotalInv { get; set; }

    [StringLength(50)]
    public string RetainCode { get; set; } = null!;

    [StringLength(50)]
    public string coyCode { get; set; } = null!;
}
