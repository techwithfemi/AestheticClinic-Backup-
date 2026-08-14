using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwClinicDaysForRpt
{
    [StringLength(50)]
    [Unicode(false)]
    public string ClinicDay { get; set; } = null!;

    [StringLength(50)]
    public string? Clinic { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ClinicTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EndTime { get; set; }

    public int? PatLimit { get; set; }

    public long? SNo { get; set; }
}
