using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwTranxCashOpenBal
{
    public long SNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime TranDate { get; set; }

    [StringLength(50)]
    public string AcctID { get; set; } = null!;

    public double? Balance { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ValueDate { get; set; }
}
