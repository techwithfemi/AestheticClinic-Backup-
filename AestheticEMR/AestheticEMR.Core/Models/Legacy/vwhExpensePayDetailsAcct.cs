using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhExpensePayDetailsAcct
{
    public long SNo { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string VouchNo { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime PayDate { get; set; }

    [StringLength(550)]
    [Unicode(false)]
    public string ItemCode { get; set; } = null!;

    public double Amount { get; set; }

    [StringLength(3550)]
    [Unicode(false)]
    public string Description { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string payType { get; set; } = null!;

    [StringLength(3550)]
    [Unicode(false)]
    public string? Remarks2 { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string AcctNoCreit { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string AcctNoDebit { get; set; } = null!;

    public bool? isPost { get; set; }

    public bool? suppres { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EntryDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EntryTime { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? ClientName { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? AppName { get; set; }

    [StringLength(3614)]
    [Unicode(false)]
    public string Remarks { get; set; } = null!;

    public double Qty { get; set; }

    public double Price { get; set; }
}
