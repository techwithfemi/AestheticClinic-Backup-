using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Table("hPatientArea")]
public partial class hPatientArea
{
    [Key]
    [StringLength(250)]
    public string AreaName { get; set; } = null!;
}
