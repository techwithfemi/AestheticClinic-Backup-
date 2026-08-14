using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhExpense
{
    public long ExpID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [StringLength(50)]
    public string? Remarks { get; set; }

    [StringLength(50)]
    public string ExpName { get; set; } = null!;

    [StringLength(50)]
    public string VouchNo { get; set; } = null!;

    public bool? AttendedTo { get; set; }

    public bool? isApprv { get; set; }

    [StringLength(101)]
    public string? Receivedby { get; set; }

    [StringLength(50)]
    public string ExpCat { get; set; } = null!;

    [StringLength(100)]
    public string? Description { get; set; }

    public double Qty { get; set; }

    public double Price { get; set; }

    public double? SubTotal { get; set; }

    [StringLength(255)]
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
}
