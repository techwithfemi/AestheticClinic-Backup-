using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hConRoomAssign")]
public partial class hConRoomAssign
{
    public long SNO { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime SchdDate { get; set; }

    [StringLength(50)]
    public string ConRoomNo { get; set; } = null!;

    [StringLength(50)]
    public string DocNo { get; set; } = null!;

    [StringLength(50)]
    public string? Remarks { get; set; }

    public bool? IsOff { get; set; }
}
