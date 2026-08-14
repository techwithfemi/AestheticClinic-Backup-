using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("Period")]
public partial class Period
{
    [StringLength(50)]
    public string MthName { get; set; } = null!;

    [StringLength(2)]
    public string Mth { get; set; } = null!;

    [StringLength(4)]
    public string Yr { get; set; } = null!;
}
