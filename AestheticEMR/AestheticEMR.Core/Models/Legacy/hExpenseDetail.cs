using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class hExpenseDetail
{
    public long ExpID { get; set; }

    [StringLength(50)]
    public string VouchNo { get; set; } = null!;

    [StringLength(50)]
    public string ExpName { get; set; } = null!;

    [StringLength(50)]
    public string ExpCat { get; set; } = null!;

    [StringLength(100)]
    public string? Description { get; set; }

    public double Qty { get; set; }

    public double Price { get; set; }

    public double? SubTotal { get; set; }

    public long? AcctID { get; set; }

    public bool? isApprv { get; set; }

    public bool? AttendedTo { get; set; }

    [StringLength(1000)]
    public string? Remarks { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ExpDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DateLastPurch { get; set; }

    public double? QtyLastPurch { get; set; }

    public double? PriceLastPurch { get; set; }
}
