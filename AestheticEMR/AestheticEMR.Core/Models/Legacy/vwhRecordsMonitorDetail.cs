using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhRecordsMonitorDetail
{
    public long SNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [StringLength(251)]
    public string FullName { get; set; } = null!;

    [StringLength(500)]
    [Unicode(false)]
    public string ItemName { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string ConsultID { get; set; } = null!;
}
