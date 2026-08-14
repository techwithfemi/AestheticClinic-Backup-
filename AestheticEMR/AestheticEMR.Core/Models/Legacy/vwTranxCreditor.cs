using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwTranxCreditor
{
    public long SNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime TranDate { get; set; }

    [StringLength(53)]
    public string AcctID { get; set; } = null!;

    [StringLength(50)]
    public string? AcctName { get; set; }

    [StringLength(107)]
    public string? AcctGp { get; set; }

    [StringLength(8)]
    [Unicode(false)]
    public string CatHead { get; set; } = null!;

    [StringLength(107)]
    public string? SubCat { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ValueDate { get; set; }

    [StringLength(200)]
    public string? DrCr { get; set; }

    [StringLength(50)]
    public string ChequeNo { get; set; } = null!;

    [StringLength(1000)]
    public string? Remarks { get; set; }

    public double? OpenBal { get; set; }

    public double Debit { get; set; }

    public double? Credit { get; set; }

    public double? Balance { get; set; }
}
