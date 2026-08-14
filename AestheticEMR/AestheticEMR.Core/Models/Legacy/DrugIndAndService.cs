using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class DrugIndAndService
{
    [StringLength(550)]
    public string? service { get; set; }

    [StringLength(550)]
    public string? Category { get; set; }

    [StringLength(50)]
    public string? revtype { get; set; }
}
