using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("EmployeeCatOLD")]
public partial class EmployeeCatOLD
{
    [StringLength(50)]
    public string? catID { get; set; }

    [StringLength(50)]
    public string catName { get; set; } = null!;

    public long SNO { get; set; }
}
