using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwBankRecon
{
    public long SNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Date { get; set; }

    [StringLength(50)]
    public string? Item { get; set; }

    [StringLength(50)]
    public string? AcctID { get; set; }

    public double? Amount { get; set; }

    public int? Mth { get; set; }

    public int? Yr { get; set; }
}
