using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("EmpOffenceCat")]
public partial class EmpOffenceCat
{
    [StringLength(50)]
    public string? OffCatID { get; set; }

    [StringLength(50)]
    public string? OffCatName { get; set; }
}
