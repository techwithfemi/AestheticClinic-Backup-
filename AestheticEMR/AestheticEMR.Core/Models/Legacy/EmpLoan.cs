using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class EmpLoan
{
    [Column(TypeName = "smalldatetime")]
    public DateTime? LoanDate { get; set; }

    [StringLength(50)]
    public string? EmpID { get; set; }

    [StringLength(50)]
    public string RefNo { get; set; } = null!;

    [Column(TypeName = "decimal(18, 0)")]
    public decimal? Amount { get; set; }

    public int? PayDuration { get; set; }

    [StringLength(50)]
    public string? ApprovedBy { get; set; }

    [StringLength(50)]
    public string? LoanCatID { get; set; }
}
