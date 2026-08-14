using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class Ebony_hConsulting
{
    public long ConID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime cDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CTime { get; set; }

    [StringLength(50)]
    public string pNo { get; set; } = null!;

    [StringLength(8000)]
    [Unicode(false)]
    public string? Treatment { get; set; }

    [StringLength(3000)]
    [Unicode(false)]
    public string? complaints { get; set; }

    [StringLength(3000)]
    [Unicode(false)]
    public string? diagnosis { get; set; }

    [StringLength(3000)]
    [Unicode(false)]
    public string? HPC { get; set; }

    [StringLength(3000)]
    [Unicode(false)]
    public string? PMH { get; set; }

    [StringLength(3000)]
    [Unicode(false)]
    public string? DrugHx { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EntryTime { get; set; }
}
