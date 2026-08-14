using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwPaymentDetailsSummForCashPosting
{
    [Column(TypeName = "datetime")]
    public DateTime? ReceiptDate { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string AccountNo { get; set; } = null!;

    [Column(TypeName = "decimal(38, 0)")]
    public decimal? AmountPaid { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string RevType { get; set; } = null!;

    public bool? isPost { get; set; }

    [StringLength(6)]
    [Unicode(false)]
    public string Remarks { get; set; } = null!;
}
