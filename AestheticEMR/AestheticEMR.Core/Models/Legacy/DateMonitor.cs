using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("DateMonitor")]
public partial class DateMonitor
{
    [Column(TypeName = "datetime")]
    public DateTime DtBill { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DtOthers { get; set; }

    public byte[] LastUpdate { get; set; } = null!;
}
