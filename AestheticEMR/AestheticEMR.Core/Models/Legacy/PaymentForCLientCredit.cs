using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("PaymentForCLientCredit")]
public partial class PaymentForCLientCredit
{
    [Column(TypeName = "smalldatetime")]
    public DateTime ReceiptDate { get; set; }

    [StringLength(50)]
    public string ReceiptNo { get; set; } = null!;

    [StringLength(50)]
    public string ClientID { get; set; } = null!;

    [Column(TypeName = "decimal(18, 0)")]
    public decimal AmountCredit { get; set; }

    public bool isUsed { get; set; }
}
