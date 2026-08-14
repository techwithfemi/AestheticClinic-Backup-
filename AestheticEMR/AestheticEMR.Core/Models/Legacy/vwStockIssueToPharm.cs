using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwStockIssueToPharm
{
    public int IssueID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime IssueDate { get; set; }

    [StringLength(50)]
    public string ItemID { get; set; } = null!;

    public int Qty { get; set; }

    [StringLength(50)]
    public string IssuedBy { get; set; } = null!;

    [StringLength(101)]
    public string? empFullname { get; set; }

    public bool? attendedTo { get; set; }

    public bool? Suppres { get; set; }

    [StringLength(50)]
    public string? Category { get; set; }

    public double? UnitLevel { get; set; }

    [Column("Qty/Unit")]
    [StringLength(43)]
    public string? Qty_Unit { get; set; }
}
