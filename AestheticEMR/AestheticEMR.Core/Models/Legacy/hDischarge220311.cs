using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hDischarge220311")]
public partial class hDischarge220311
{
    public long ID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime dischDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime dischTime { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string pno { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string consultID { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string ClientCat { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string WardID { get; set; } = null!;

    [StringLength(250)]
    [Unicode(false)]
    public string? Reason { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? Remarks { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ApptDate { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string ApprvBy { get; set; } = null!;
}
