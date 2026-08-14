using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhDispensingOffline
{
    [StringLength(50)]
    public string ConsultID { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? dtDate { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string drgName { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string drgCatName { get; set; } = null!;

    public double Qty { get; set; }

    [StringLength(50)]
    public string? Fullname { get; set; }

    [StringLength(350)]
    public string? usage { get; set; }

    public bool? attendedTo { get; set; }

    [StringLength(50)]
    public string? pNO { get; set; }

    [StringLength(101)]
    public string? EmpName { get; set; }

    public double? Price { get; set; }

    public double? Amount { get; set; }

    public double? Cost { get; set; }

    [Column("Qty/Unit")]
    [StringLength(1)]
    [Unicode(false)]
    public string Qty_Unit { get; set; } = null!;

    [StringLength(7)]
    [Unicode(false)]
    public string ClientCatID { get; set; } = null!;

    [StringLength(7)]
    [Unicode(false)]
    public string retainName { get; set; } = null!;

    [StringLength(4)]
    [Unicode(false)]
    public string RetainCode { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? dtTime { get; set; }

    public long ID { get; set; }

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

    public bool? isPost { get; set; }
}
