using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hServiceCatMayDup")]
public partial class hServiceCatMayDup
{
    [StringLength(100)]
    [Unicode(false)]
    public string CatName { get; set; } = null!;

    [StringLength(100)]
    public string? Clinic { get; set; }
}
