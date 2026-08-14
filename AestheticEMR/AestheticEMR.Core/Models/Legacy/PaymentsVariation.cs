using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("PaymentsVariation")]
public partial class PaymentsVariation
{
    [Column(TypeName = "datetime")]
    public DateTime ReceiptDate { get; set; }

    [StringLength(50)]
    public string ReceiptNo { get; set; } = null!;

    [StringLength(50)]
    public string billNO { get; set; } = null!;

    [StringLength(50)]
    public string pNO { get; set; } = null!;

    [StringLength(50)]
    public string? clientID { get; set; }

    public double BF { get; set; }

    public double Discount { get; set; }

    public double CF { get; set; }

    public double? RctBillBal { get; set; }
}
