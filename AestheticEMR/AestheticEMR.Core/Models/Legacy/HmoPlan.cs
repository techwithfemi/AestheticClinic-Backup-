using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("HmoPlan")]
public partial class HmoPlan
{
    public long SNo { get; set; }

    [StringLength(70)]
    public string? PlanID { get; set; }

    [StringLength(150)]
    public string PlanName { get; set; } = null!;

    [StringLength(50)]
    public string CoyCode { get; set; } = null!;

    [StringLength(150)]
    public string? Remarks { get; set; }

    public double? Limit { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? LimitPeriod { get; set; }
}
