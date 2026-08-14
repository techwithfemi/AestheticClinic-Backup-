using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("DrugUnitTest")]
public partial class DrugUnitTest
{
    [StringLength(50)]
    [Unicode(false)]
    public string? Unit { get; set; }
}
