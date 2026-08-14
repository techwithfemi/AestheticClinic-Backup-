using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("ClinicTime")]
public partial class ClinicTime
{
    public long SNo { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string TimeOfDay { get; set; } = null!;
}
