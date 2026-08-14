using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhAttendanceAndDiagnosis1
{
    [Column(TypeName = "datetime")]
    public DateTime recDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? htime { get; set; }

    [StringLength(101)]
    public string Fullname { get; set; } = null!;

    [StringLength(50)]
    public string Status { get; set; } = null!;

    [StringLength(4000)]
    public string prescription { get; set; } = null!;

    [StringLength(2000)]
    public string? LabTest { get; set; }

    [StringLength(50)]
    public string Sex { get; set; } = null!;

    [StringLength(3000)]
    public string? diagnosis { get; set; }

    [StringLength(50)]
    public string? NHISNo { get; set; }

    [StringLength(50)]
    public string? CardNo { get; set; }

    public int? Age { get; set; }
}
