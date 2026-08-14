using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hhScreeningAttnd")]
public partial class hhScreeningAttnd
{
    public long SNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime RecDate { get; set; }

    [StringLength(50)]
    public string ConsultID { get; set; } = null!;

    [StringLength(50)]
    public string Remarks { get; set; } = null!;
}
