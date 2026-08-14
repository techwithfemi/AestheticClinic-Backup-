using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class QRYAttendanceForToday
{
    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Time { get; set; }

    [StringLength(101)]
    public string Fullname { get; set; } = null!;

    [StringLength(50)]
    public string pNo { get; set; } = null!;

    [StringLength(50)]
    public string? oldpNo { get; set; }

    [StringLength(50)]
    public string clinicType { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? NextApptDate { get; set; }

    [StringLength(100)]
    public string? Remarks { get; set; }
}
