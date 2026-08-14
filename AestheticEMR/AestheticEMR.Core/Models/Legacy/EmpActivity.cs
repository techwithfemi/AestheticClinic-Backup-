using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class EmpActivity
{
    public int ActID { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? ActDate { get; set; }

    [StringLength(50)]
    public string? Nature { get; set; }

    [Column(TypeName = "money")]
    public decimal? Income { get; set; }

    [Column(TypeName = "money")]
    public decimal? Expense { get; set; }

    [StringLength(50)]
    public string? ApprovedBy { get; set; }

    [StringLength(150)]
    public string? Remarks { get; set; }

    [StringLength(50)]
    public string? EmpID { get; set; }

    [StringLength(50)]
    public string? ActCatID { get; set; }
}
