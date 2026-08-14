using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("TranxactionOLD")]
public partial class TranxactionOLD
{
    public long SNO { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime TrDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? TrTime { get; set; }

    [StringLength(50)]
    public string AcctID { get; set; } = null!;

    [StringLength(100)]
    public string CatHead { get; set; } = null!;

    [StringLength(100)]
    public string? SubCat { get; set; }

    [StringLength(2)]
    public string DrCr { get; set; } = null!;

    public double Amount { get; set; }

    public double? Balance { get; set; }

    [StringLength(100)]
    public string? Remarks { get; set; }

    [StringLength(50)]
    public string? ChequeNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ValueDate { get; set; }

    [StringLength(50)]
    public string? BankCode { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ChequeDate { get; set; }

    [StringLength(50)]
    public string? EntryBy { get; set; }

    [StringLength(50)]
    public string? DeptID { get; set; }

    [StringLength(50)]
    public string? Period { get; set; }

    [StringLength(2)]
    public string? Mth { get; set; }

    [StringLength(4)]
    public string? Yr { get; set; }
}
