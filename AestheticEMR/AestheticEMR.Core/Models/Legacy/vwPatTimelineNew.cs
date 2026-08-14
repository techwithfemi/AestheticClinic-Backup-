using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwPatTimelineNew
{
    public int SNo { get; set; }

    [StringLength(1001)]
    public string Fullname { get; set; } = null!;

    [Column("(AttndDate)")]
    [StringLength(15)]
    [Unicode(false)]
    public string? _AttndDate_ { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string ServicePoint { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? EntryDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EntryTime { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string Company { get; set; } = null!;

    [StringLength(50)]
    public string ConsultID { get; set; } = null!;

    [StringLength(50)]
    public string? ClientCatID { get; set; }

    public long? conID { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? EntryOrExit { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? AppName { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? ClientName { get; set; }

    [StringLength(5000)]
    [Unicode(false)]
    public string? Remarks { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? CoyID { get; set; }
}
