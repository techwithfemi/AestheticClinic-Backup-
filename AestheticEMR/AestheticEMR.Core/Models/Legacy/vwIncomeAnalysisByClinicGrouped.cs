using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwIncomeAnalysisByClinicGrouped
{
    [StringLength(2)]
    public string? Mth { get; set; }

    [StringLength(30)]
    public string? Yr { get; set; }

    [StringLength(100)]
    public string Clinic { get; set; } = null!;

    [StringLength(50)]
    public string clinicType { get; set; } = null!;

    [Column(TypeName = "decimal(38, 0)")]
    public decimal? AmountBilled { get; set; }

    [Column(TypeName = "decimal(38, 0)")]
    public decimal? AmountPaid { get; set; }

    public long ClinicID { get; set; }
}
