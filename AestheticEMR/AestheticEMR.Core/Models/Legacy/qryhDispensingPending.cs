using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhDispensingPending
{
    [StringLength(50)]
    public string ConsultID { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime dtDate { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string drgName { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string drgCatName { get; set; } = null!;

    public double Qty { get; set; }

    [StringLength(550)]
    public string? usage { get; set; }

    [StringLength(355)]
    public string Fullname { get; set; } = null!;

    public bool? attendedTo { get; set; }

    [StringLength(355)]
    public string pno { get; set; } = null!;

    [StringLength(101)]
    public string? EmpName { get; set; }

    public double? Price { get; set; }

    public double? Amount { get; set; }

    public double? Cost { get; set; }

    public long ID { get; set; }

    [StringLength(50)]
    public string? LocID { get; set; }

    [Column("Qty/Unit")]
    [StringLength(43)]
    public string? Qty_Unit { get; set; }

    [StringLength(50)]
    public string? clientCatID { get; set; }

    [StringLength(150)]
    public string retainName { get; set; } = null!;

    [StringLength(50)]
    public string? RetainCode { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? dtTime { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string? Pending { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DueDate { get; set; }
}
