using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryStockissued
{
    public long IssueID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime IssueDate { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? ItemID { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? ItemName { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Qty { get; set; }

    [StringLength(200)]
    public string? Comments { get; set; }

    [StringLength(101)]
    public string? IssuedBy { get; set; }

    [StringLength(50)]
    public string? Category { get; set; }

    [StringLength(50)]
    public string? LocID { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? PrevBal { get; set; }

    [StringLength(43)]
    public string? QtyPerUnit { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? DeptID { get; set; }
}
