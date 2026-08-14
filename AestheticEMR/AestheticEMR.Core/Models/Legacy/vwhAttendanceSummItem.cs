using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhAttendanceSummItem
{
    [StringLength(50)]
    [Unicode(false)]
    public string ItemNAme { get; set; } = null!;

    public long NumVal { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DtDAte { get; set; }
}
