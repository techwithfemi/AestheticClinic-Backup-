using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class ClinicDay
{
    public long SNo { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string ClinicID { get; set; } = null!;

    [Column("ClinicDay")]
    [StringLength(50)]
    [Unicode(false)]
    public string ClinicDay1 { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime StartTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EndTime { get; set; }

    public int NumOfPat { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string? Remarks { get; set; }
}
