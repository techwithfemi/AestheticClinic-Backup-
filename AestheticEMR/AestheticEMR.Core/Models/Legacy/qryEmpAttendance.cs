using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryEmpAttendance
{
    public long recID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [StringLength(101)]
    public string Fullname { get; set; } = null!;

    [StringLength(50)]
    public string Dept { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Shift { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? Resumption { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ResumedTime { get; set; }

    [Column("R/Status")]
    [StringLength(50)]
    [Unicode(false)]
    public string? R_Status { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Remarks { get; set; } = null!;

    [Column("C/Time", TypeName = "datetime")]
    public DateTime? C_Time { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? SignOffTime { get; set; }

    [Column("C/Status")]
    [StringLength(50)]
    [Unicode(false)]
    public string? C_Status { get; set; }

    [Column("C/Remarks")]
    [StringLength(50)]
    [Unicode(false)]
    public string? C_Remarks { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string StaffNo { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? OVT { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string? isLock { get; set; }
}
