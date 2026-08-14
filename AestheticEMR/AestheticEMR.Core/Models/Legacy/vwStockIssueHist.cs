using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwStockIssueHist
{
    [StringLength(50)]
    public string ItemID { get; set; } = null!;

    [StringLength(50)]
    public string? Category { get; set; }

    [StringLength(50)]
    public string? locid { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime IssueDate { get; set; }

    public int? Qty { get; set; }
}
