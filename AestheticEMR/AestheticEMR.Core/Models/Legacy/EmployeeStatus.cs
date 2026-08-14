using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Table("EmployeeStatus")]
public partial class EmployeeStatus
{
    [Key]
    [StringLength(100)]
    public string statID { get; set; } = null!;

    [StringLength(100)]
    public string statName { get; set; } = null!;
}
