using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hretainershipExempt")]
public partial class hretainershipExempt
{
    [StringLength(50)]
    [Unicode(false)]
    public string retainID { get; set; } = null!;
}
