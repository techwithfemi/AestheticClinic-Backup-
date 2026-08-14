using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwTranxForRpt
{
    public long SNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime TranDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ValueDate { get; set; }

    [StringLength(50)]
    public string AcctID { get; set; } = null!;

    [StringLength(50)]
    public string AcctNAme { get; set; } = null!;

    [StringLength(50)]
    public string? ChequeNo { get; set; }

    [StringLength(100)]
    public string? Remarks { get; set; }

    public double Debit { get; set; }

    public double Credit { get; set; }

    public double? Balance { get; set; }
}
