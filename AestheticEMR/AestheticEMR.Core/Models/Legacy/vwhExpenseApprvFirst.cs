using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhExpenseApprvFirst
{
    public long ExpID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [StringLength(50)]
    public string ExpName { get; set; } = null!;

    [StringLength(50)]
    public string VouchNo { get; set; } = null!;

    public bool? AttendedTo { get; set; }

    public bool? isApprv { get; set; }

    [StringLength(500)]
    public string? Receivedby { get; set; }

    [StringLength(50)]
    public string? Remarks { get; set; }

    [StringLength(50)]
    public string ExpCat { get; set; } = null!;

    [StringLength(1000)]
    public string? Description { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Qty { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Price { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? SubTotal { get; set; }

    [StringLength(500)]
    public string ItemName { get; set; } = null!;

    [StringLength(225)]
    [Unicode(false)]
    public string Category { get; set; } = null!;

    [StringLength(50)]
    public string? PersNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ExpDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DateLastPurch { get; set; }

    public double? QtyLastPurch { get; set; }

    public double? PriceLastPurch { get; set; }

    [StringLength(101)]
    public string? FirstApprvBy { get; set; }

    [StringLength(225)]
    [Unicode(false)]
    public string CatType { get; set; } = null!;

    [StringLength(50)]
    public string? CatCode { get; set; }

    public long? expID_SNo { get; set; }
}
