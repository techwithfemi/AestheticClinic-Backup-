using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("empAttendanceArchive2")]
public partial class empAttendanceArchive2
{
    public long recID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime AttDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime AttTime { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string EmpID { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Remarks { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Shift { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? RTime { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Status { get; set; }

    public int? timeVal { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? cTime { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? cStatus { get; set; }

    public int? cTimeVal { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? SignOffTime { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? cRemarks { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? OvtDesc { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string? isLock { get; set; }
}
