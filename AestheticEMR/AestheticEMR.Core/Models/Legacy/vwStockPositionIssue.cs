using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwStockPositionIssue
{
    public int? IssueID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? IssueDate { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? ItemID { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? Drug { get; set; }

    public int? Qty { get; set; }

    [StringLength(50)]
    public string? IssuedBy { get; set; }

    [StringLength(200)]
    public string? Comments { get; set; }

    [StringLength(150)]
    public string? Category { get; set; }

    public double? DrugPriceLast { get; set; }

    public bool? Suppres { get; set; }

    public double? BulkUnit { get; set; }

    public double? PrevBal { get; set; }

    [StringLength(50)]
    public string? LocID { get; set; }

    [StringLength(50)]
    public string? POID { get; set; }

    [StringLength(50)]
    public string? Expr1 { get; set; }
}
