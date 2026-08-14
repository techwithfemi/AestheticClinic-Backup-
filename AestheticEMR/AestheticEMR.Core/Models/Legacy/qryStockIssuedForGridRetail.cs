using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryStockIssuedForGridRetail
{
    public int IssueID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime IssueDate { get; set; }

    [StringLength(50)]
    public string ItemCode { get; set; } = null!;

    [StringLength(350)]
    public string ItemName { get; set; } = null!;

    public int Qty { get; set; }

    [StringLength(101)]
    public string? IssuedBy { get; set; }

    [StringLength(200)]
    public string? Comments { get; set; }

    [StringLength(50)]
    public string empID { get; set; } = null!;
}
