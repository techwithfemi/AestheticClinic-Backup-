using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("DrugfREQTest")]
public partial class DrugfREQTest
{
    [StringLength(20)]
    [Unicode(false)]
    public string? Freq { get; set; }
}
