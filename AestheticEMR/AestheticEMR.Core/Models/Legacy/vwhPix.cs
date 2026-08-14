using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhPix
{
    public long SNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime dtDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime dtTime { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string pSurName { get; set; } = null!;

    [StringLength(150)]
    [Unicode(false)]
    public string? pFirstname { get; set; }

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
