using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("EmployeeStatusOLD")]
public partial class EmployeeStatusOLD
{
    public long SNO { get; set; }

    [StringLength(50)]
    public string statID { get; set; } = null!;

    [StringLength(50)]
    public string statName { get; set; } = null!;
}
