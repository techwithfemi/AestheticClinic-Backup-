using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Table("Designation")]
public partial class Designation
{
    [Key]
    [StringLength(100)]
    public string desID { get; set; } = null!;

    [StringLength(200)]
    public string desName { get; set; } = null!;
}
