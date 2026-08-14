using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hPatientsReg")]
public partial class hPatientsReg
{
    [StringLength(50)]
    public string RegType { get; set; } = null!;

    public double Amount { get; set; }
}
