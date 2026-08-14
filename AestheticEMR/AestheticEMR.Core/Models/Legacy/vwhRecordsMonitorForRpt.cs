using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhRecordsMonitorForRpt
{
    [Column(TypeName = "datetime")]
    public DateTime date { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string Description { get; set; } = null!;

    public int NumCount { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? Remarks { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EntryDate { get; set; }
}
