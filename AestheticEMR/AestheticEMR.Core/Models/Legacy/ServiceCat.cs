using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("ServiceCat")]
public partial class ServiceCat
{
    [StringLength(100)]
    [Unicode(false)]
    public string Category { get; set; } = null!;
}
