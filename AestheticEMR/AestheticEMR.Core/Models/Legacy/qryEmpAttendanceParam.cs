using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryEmpAttendanceParam
{
    [Column("Shift/Job")]
    [StringLength(50)]
    [Unicode(false)]
    public string Shift_Job { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime ResumptionTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ClosingTime { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string EarlyResumptionRemarks { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string LateResumptionRemarks { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string NormalClosingRemarks { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string AbnormalClosingRemarks { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string EvalTo { get; set; } = null!;
}
