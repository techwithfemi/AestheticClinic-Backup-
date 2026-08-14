using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwIncomeAnalysisByDoctorGrouped
{
    [Column(TypeName = "decimal(38, 0)")]
    public decimal? AmountBilled { get; set; }

    [Column(TypeName = "decimal(38, 0)")]
    public decimal? AmountPaid { get; set; }

    [StringLength(101)]
    public string? DocName { get; set; }

    [StringLength(50)]
    public string? EmpID { get; set; }

    [StringLength(2)]
    public string? Mth { get; set; }

    [StringLength(30)]
    public string? Yr { get; set; }
}
