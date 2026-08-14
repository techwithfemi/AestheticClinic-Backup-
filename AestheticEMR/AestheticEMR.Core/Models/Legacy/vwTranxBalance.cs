using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwTranxBalance
{
    [Column(TypeName = "datetime")]
    public DateTime TranDate { get; set; }

    [StringLength(50)]
    public string AcctName { get; set; } = null!;

    [StringLength(103)]
    public string? AcctGp { get; set; }

    [StringLength(2)]
    public string DrCr { get; set; } = null!;

    public double? Balance { get; set; }
}
