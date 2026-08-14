using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("EmpDeductionCat")]
public partial class EmpDeductionCat
{
    [StringLength(50)]
    public string DedID { get; set; } = null!;

    [StringLength(50)]
    public string DedName { get; set; } = null!;

    public byte SalGrade { get; set; }

    public byte? SalStep { get; set; }

    public double? DedRate { get; set; }
}
