using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryStockIssuedForGrid
{
    public long IssueID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime IssueDate { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? ItemCode { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string ItemName { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string? Category { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal QtyIssued { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? PharmacyQty { get; set; }

    [StringLength(101)]
    public string? IssuedBy { get; set; }

    [StringLength(200)]
    public string? Comments { get; set; }

    [StringLength(50)]
    public string? reverseID { get; set; }

    [StringLength(3)]
    public string? reversal { get; set; }
}
