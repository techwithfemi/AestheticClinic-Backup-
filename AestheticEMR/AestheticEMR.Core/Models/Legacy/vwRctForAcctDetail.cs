using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwRctForAcctDetail
{
    public long SNO { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime BillDate { get; set; }

    [StringLength(50)]
    public string billNO { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string BillItem { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? subTotal { get; set; }

    [StringLength(50)]
    public string? revType { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string AccountNo { get; set; } = null!;

    public bool? isRct { get; set; }
}
