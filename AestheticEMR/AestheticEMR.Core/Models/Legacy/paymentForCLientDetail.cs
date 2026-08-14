using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class paymentForCLientDetail
{
    [Column(TypeName = "smalldatetime")]
    public DateTime ReceiptDate { get; set; }

    [StringLength(50)]
    public string ReceiptNo { get; set; } = null!;

    [StringLength(50)]
    public string ClientID { get; set; } = null!;

    [StringLength(50)]
    public string BillingMonth { get; set; } = null!;

    public int? BillingYear { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal Amount { get; set; }
}
