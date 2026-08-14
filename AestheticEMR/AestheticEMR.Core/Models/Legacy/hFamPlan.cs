using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hFamPlan")]
public partial class hFamPlan
{
    public long ID { get; set; }

    [StringLength(50)]
    public string ConsultID { get; set; } = null!;

    [StringLength(50)]
    public string PNo { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? PlanDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? PlanTime { get; set; }

    [StringLength(250)]
    public string Education { get; set; } = null!;

    [StringLength(50)]
    public string? NoPregToDate { get; set; }

    [StringLength(3)]
    public string? NoOfChildBornAlive { get; set; }

    [StringLength(3)]
    public string? NoOfChildStillAlive { get; set; }

    [StringLength(3)]
    public string? NoOfMiscar { get; set; }

    [StringLength(50)]
    public string? MthYrLastPregEnded { get; set; }

    [StringLength(50)]
    public string? ResOfLastPreg { get; set; }

    [StringLength(50)]
    public string? MoreChild { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DtLastMenstru { get; set; }

    [StringLength(3)]
    public string? Smoker { get; set; }

    [StringLength(500)]
    public string? MedHist { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DtPreg1 { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DtPreg2 { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DtPreg3 { get; set; }
}
