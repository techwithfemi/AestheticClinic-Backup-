using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class hRecordsMonitorDetail
{
    public long SNo { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string Description { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string ConsultID { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime dtDate { get; set; }

    public int NumCount { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? Remarks { get; set; }
}
