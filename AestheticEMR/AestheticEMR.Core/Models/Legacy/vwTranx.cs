using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwTranx
{
    [Column(TypeName = "datetime")]
    public DateTime TrDate { get; set; }

    [StringLength(50)]
    public string AcctID { get; set; } = null!;

    [StringLength(100)]
    public string CatHead { get; set; } = null!;

    [StringLength(100)]
    public string? SubCat { get; set; }

    [StringLength(2)]
    public string DrCr { get; set; } = null!;

    public double Amount { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ValueDate { get; set; }

    [StringLength(50)]
    public string? ChequeNo { get; set; }
}
