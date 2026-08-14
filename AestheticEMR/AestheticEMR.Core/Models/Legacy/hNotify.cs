using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hNotify")]
public partial class hNotify
{
    public long SNo { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string NotifyDept { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? NotifyFrom { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string ConsultID { get; set; } = null!;

    [StringLength(5000)]
    [Unicode(false)]
    public string? Remarks { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string AttendedTo { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? NDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NTime { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? ClientName { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? AppName { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EntryDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EntryTime { get; set; }
}
