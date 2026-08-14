using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hPix")]
public partial class hPix
{
    public long SNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime dtDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime dtTime { get; set; }

    [Column(TypeName = "image")]
    public byte[]? Image { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string PNo { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? ConsultID { get; set; }

    [StringLength(8000)]
    [Unicode(false)]
    public string? Remarks { get; set; }
}
