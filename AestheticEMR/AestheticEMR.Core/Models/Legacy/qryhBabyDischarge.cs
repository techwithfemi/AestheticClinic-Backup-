using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhBabyDischarge
{
    [Column(TypeName = "smalldatetime")]
    public DateTime bDate { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? bTime { get; set; }

    [StringLength(50)]
    public string? WardID { get; set; }

    [StringLength(50)]
    public string pNo { get; set; } = null!;

    [StringLength(101)]
    public string Fullname { get; set; } = null!;

    [StringLength(50)]
    public string? sex { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? ProbDischDate { get; set; }

    public int? noOfDays { get; set; }
}
