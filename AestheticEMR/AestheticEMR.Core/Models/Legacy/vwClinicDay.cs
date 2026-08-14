using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwClinicDay
{
    public long SNo { get; set; }

    public int SNoID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string ClinicDay { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Clinic { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime ClinicTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EndTime { get; set; }

    public int PatLimit { get; set; }
}
