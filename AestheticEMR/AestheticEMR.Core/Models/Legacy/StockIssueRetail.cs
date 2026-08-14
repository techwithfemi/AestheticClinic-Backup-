using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("StockIssueRetail")]
public partial class StockIssueRetail
{
    public int IssueID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime IssueDate { get; set; }

    [StringLength(350)]
    public string ItemID { get; set; } = null!;

    [StringLength(350)]
    public string? Category { get; set; }

    [StringLength(50)]
    public string? BatchNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ExpiryDate { get; set; }

    public int Qty { get; set; }

    public int? stockQtyIn { get; set; }

    [StringLength(50)]
    public string IssuedBy { get; set; } = null!;

    [StringLength(50)]
    public string? POID { get; set; }

    [StringLength(200)]
    public string? Comments { get; set; }

    [StringLength(50)]
    public string? invType { get; set; }
}
