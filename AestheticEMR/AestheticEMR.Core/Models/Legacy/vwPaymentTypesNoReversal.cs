using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwPaymentTypesNoReversal
{
    public long SNo { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string ReceiptNo { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal AmountPaid { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string PayType { get; set; } = null!;

    public bool? isPost { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ReceiptDate { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? AccountNo { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? TranID { get; set; }
}
