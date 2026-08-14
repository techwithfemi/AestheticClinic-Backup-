using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhFamPlan
{
    public long SNo { get; set; }

    [StringLength(101)]
    public string Fullname { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? Date { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Time { get; set; }

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

    public long ID { get; set; }

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

    [Column(TypeName = "datetime")]
    public DateTime? PlanDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? PlanTime { get; set; }

    [StringLength(101)]
    public string EmpID { get; set; } = null!;
}
