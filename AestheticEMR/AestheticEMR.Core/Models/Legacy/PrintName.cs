using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Table("PrintName")]
public partial class PrintName
{
    public long SNo { get; set; }

    [Key]
    [StringLength(500)]
    [Unicode(false)]
    public string PrtName { get; set; } = null!;
}
