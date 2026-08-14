using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("EmpAllowanceCat")]
public partial class EmpAllowanceCat
{
    [StringLength(50)]
    public string AllwID { get; set; } = null!;

    [StringLength(50)]
    public string AllwName { get; set; } = null!;

    public byte SalGrade { get; set; }

    public byte? SalStep { get; set; }

    public double? AllwRate { get; set; }
}
