using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class hRecordsMonitorForRpt
{
    [Column(TypeName = "datetime")]
    public DateTime? date { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    public int NumCount { get; set; }

    [StringLength(500)]
    public string? Remarks { get; set; }
}
