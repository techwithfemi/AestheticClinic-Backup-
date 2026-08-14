using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("DrugDurTest")]
public partial class DrugDurTest
{
    [StringLength(20)]
    [Unicode(false)]
    public string? Dur { get; set; }
}
