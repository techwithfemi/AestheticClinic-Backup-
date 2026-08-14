using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("paymentDeposit")]
public partial class paymentDeposit
{
    public long ID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime dtDate { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [StringLength(50)]
    public string pno { get; set; } = null!;

    public double Amount { get; set; }

    [StringLength(250)]
    public string? Remarks { get; set; }
}
