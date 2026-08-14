using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class hFamPlanDetail
{
    public long SNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DtDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DtTime { get; set; }

    [StringLength(50)]
    public string? PNo { get; set; }

    [StringLength(50)]
    public string? ConsultID { get; set; }

    [StringLength(3)]
    public string? MtdChange { get; set; }

    [StringLength(50)]
    public string? MtdSupplied { get; set; }

    [StringLength(50)]
    public string? Qty { get; set; }

    [StringLength(50)]
    public string? BP { get; set; }

    [StringLength(50)]
    public string? Wt { get; set; }

    [StringLength(250)]
    public string? Observe { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NextAppt { get; set; }

    [StringLength(50)]
    public string? empID { get; set; }
}
