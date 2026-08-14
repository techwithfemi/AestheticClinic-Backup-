using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwIncomeAnalysisByDoctor
{
    [StringLength(50)]
    public string ClinicID { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime recDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Time { get; set; }

    [StringLength(301)]
    public string Fullname { get; set; } = null!;

    [StringLength(50)]
    public string clinicType { get; set; } = null!;

    [StringLength(100)]
    public string? Remarks { get; set; }

    [StringLength(50)]
    public string? clientCat { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal? AmountBilled { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal? AmountPaid { get; set; }

    [StringLength(150)]
    public string CoyNAme { get; set; } = null!;

    [StringLength(101)]
    public string? DocName { get; set; }

    [StringLength(50)]
    public string? EmpID { get; set; }

    public int Target { get; set; }
}
