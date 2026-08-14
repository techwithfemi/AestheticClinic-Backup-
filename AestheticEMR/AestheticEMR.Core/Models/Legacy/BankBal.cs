using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("BankBal")]
public partial class BankBal
{
    [Column(TypeName = "datetime")]
    public DateTime? EndDate { get; set; }

    [StringLength(20)]
    public string? AcctID { get; set; }

    public double? CloseBal { get; set; }
}
