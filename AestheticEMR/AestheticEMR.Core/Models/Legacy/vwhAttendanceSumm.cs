using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhAttendanceSumm
{
    [StringLength(50)]
    [Unicode(false)]
    public string ItemName { get; set; } = null!;

    public long? numVal { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime dtdate { get; set; }

    [StringLength(30)]
    public string? Yr { get; set; }
}
