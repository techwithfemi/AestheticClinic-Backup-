using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("BankRecon")]
public partial class BankRecon
{
    public long SNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DateVal { get; set; }

    [StringLength(50)]
    public string? ItemID { get; set; }

    [StringLength(50)]
    public string? BankCode { get; set; }

    [StringLength(50)]
    public string? AcctID { get; set; }

    [StringLength(50)]
    public string? Remarks { get; set; }

    public double? Amount { get; set; }

    public int? Mth { get; set; }

    public int? Yr { get; set; }
}
