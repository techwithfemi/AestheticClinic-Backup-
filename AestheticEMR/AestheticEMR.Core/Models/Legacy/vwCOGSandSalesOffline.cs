using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwCOGSandSalesOffline
{
    public long SNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime dtDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EntryDate { get; set; }

    [StringLength(50)]
    public string ConsultID { get; set; } = null!;

    [StringLength(1001)]
    public string Fullname { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string drgName { get; set; } = null!;

    public double Qty { get; set; }

    public double? Cost { get; set; }

    public double? Price { get; set; }

    public double? CostAmount { get; set; }

    public double? SalesAmount { get; set; }

    public bool? isPost { get; set; }

    [StringLength(50)]
    public string? AcctID { get; set; }

    public bool? suppres { get; set; }

    [StringLength(1334)]
    public string Remarks { get; set; } = null!;

    public double? Margin { get; set; }
}
