using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("LabServiceCat1")]
public partial class LabServiceCat1
{
    [StringLength(359)]
    public string? Category { get; set; }

    [StringLength(350)]
    public string? LabType { get; set; }
}
