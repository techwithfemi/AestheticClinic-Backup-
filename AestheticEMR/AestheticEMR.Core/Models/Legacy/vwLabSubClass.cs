using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwLabSubClass
{
    [StringLength(520)]
    [Unicode(false)]
    public string SubClass { get; set; } = null!;

    [StringLength(500)]
    [Unicode(false)]
    public string ClassName { get; set; } = null!;

    [StringLength(350)]
    public string Category { get; set; } = null!;
}
