using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhComment
{
    public long SNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Time { get; set; }

    [StringLength(406)]
    public string Fullname { get; set; } = null!;

    [StringLength(5000)]
    [Unicode(false)]
    public string Comment { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Dept { get; set; } = null!;

    [StringLength(3)]
    [Unicode(false)]
    public string AttendedTo { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? ConsultID { get; set; }
}
