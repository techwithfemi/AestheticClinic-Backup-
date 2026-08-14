using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("EmpLeaveSchedule")]
public partial class EmpLeaveSchedule
{
    [StringLength(50)]
    public string SchdID { get; set; } = null!;

    [Column(TypeName = "smalldatetime")]
    public DateTime? StartDate { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? EndDtae { get; set; }

    [StringLength(50)]
    public string? EmpID { get; set; }

    [StringLength(50)]
    public string? ApprovedBy { get; set; }
}
