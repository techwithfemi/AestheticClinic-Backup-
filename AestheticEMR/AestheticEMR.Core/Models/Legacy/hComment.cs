using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class hComment
{
    public long SNo { get; set; }

    [StringLength(5000)]
    [Unicode(false)]
    public string Comment { get; set; } = null!;

    public long? ID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? ConsultID { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string AttendedTo { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime CDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CTime { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Dept { get; set; } = null!;
}
