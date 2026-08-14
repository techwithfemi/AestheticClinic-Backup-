using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class EmpSalary
{
    [Column(TypeName = "smalldatetime")]
    public DateTime SalDate { get; set; }

    [StringLength(50)]
    public string EmpID { get; set; } = null!;

    [StringLength(50)]
    public string? AllwType { get; set; }

    [StringLength(50)]
    public string? DedType { get; set; }

    public byte? SalGrade { get; set; }

    public byte? SalStep { get; set; }
}
