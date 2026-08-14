using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hServicesMay")]
public partial class hServicesMay
{
    [StringLength(10)]
    [Unicode(false)]
    public string ServiceID { get; set; } = null!;

    [Column(TypeName = "money")]
    public decimal Private { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string? Service { get; set; }

    [StringLength(250)]
    public string? Category { get; set; }
}
