using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("DrugsCompat")]
public partial class DrugsCompat
{
    public long SNO { get; set; }

    [StringLength(50)]
    public string DrugCode { get; set; } = null!;

    [StringLength(50)]
    public string DrugCodeIncompat { get; set; } = null!;
}
