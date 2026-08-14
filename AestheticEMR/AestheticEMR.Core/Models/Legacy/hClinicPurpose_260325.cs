using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hClinicPurpose_260325")]
public partial class hClinicPurpose_260325
{
    [StringLength(50)]
    public string? Purpose { get; set; }

    public long SNo { get; set; }
}
