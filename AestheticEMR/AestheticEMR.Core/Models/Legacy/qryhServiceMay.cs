using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhServiceMay
{
    [StringLength(10)]
    [Unicode(false)]
    public string ServiceID { get; set; } = null!;

    [StringLength(150)]
    [Unicode(false)]
    public string? Service { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Category { get; set; } = null!;

    [Column(TypeName = "money")]
    public decimal Private { get; set; }
}
