using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class hPatientClinic
{
    public long SNO { get; set; }

    [StringLength(50)]
    public string PNo { get; set; } = null!;

    [StringLength(50)]
    public string Clinic { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime RegDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ExpireDate { get; set; }

    public bool? Active { get; set; }

    [StringLength(50)]
    public string? Remarks { get; set; }
}
