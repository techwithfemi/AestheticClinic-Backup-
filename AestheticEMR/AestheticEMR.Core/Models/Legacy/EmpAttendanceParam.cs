using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("EmpAttendanceParam")]
public partial class EmpAttendanceParam
{
    [StringLength(50)]
    [Unicode(false)]
    public string ShiftType { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime ResumTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CloseTime { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string ResumRemEarly { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string ResumRemLate { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string CloseRemNorm { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string CloseRemAbNorm { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string evalTo { get; set; } = null!;
}
