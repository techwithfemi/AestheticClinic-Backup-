using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class EmployeeStatusQry
{
    [StringLength(50)]
    public string statID { get; set; } = null!;

    [StringLength(50)]
    public string statName { get; set; } = null!;
}
