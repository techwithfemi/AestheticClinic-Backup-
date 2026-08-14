using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("EmpActivityCat")]
public partial class EmpActivityCat
{
    [StringLength(50)]
    public string ActCatID { get; set; } = null!;

    [StringLength(50)]
    public string? ActCatName { get; set; }
}
