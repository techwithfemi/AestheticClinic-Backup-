using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("EmpSalaryCat")]
public partial class EmpSalaryCat
{
    public byte salGrade { get; set; }

    public byte salStep { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal? Salary { get; set; }
}
