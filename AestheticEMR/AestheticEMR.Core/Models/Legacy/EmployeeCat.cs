using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Table("EmployeeCat")]
public partial class EmployeeCat
{
    [Key]
    [StringLength(100)]
    public string catID { get; set; } = null!;

    [StringLength(100)]
    public string catName { get; set; } = null!;

    public int? CatLevel { get; set; }
}
