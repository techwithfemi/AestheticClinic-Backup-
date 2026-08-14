using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwDiagnosisSummaryUnion
{
    [Column(TypeName = "datetime")]
    public DateTime? Date { get; set; }

    [StringLength(12)]
    [Unicode(false)]
    public string Diagnosis { get; set; } = null!;

    [StringLength(30)]
    public string? MthName { get; set; }

    [StringLength(4)]
    [Unicode(false)]
    public string? Yr { get; set; }

    public int? Mth { get; set; }

    [StringLength(8)]
    public string? Period { get; set; }
}
