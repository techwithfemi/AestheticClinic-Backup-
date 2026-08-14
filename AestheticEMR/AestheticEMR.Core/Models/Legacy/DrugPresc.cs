using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Table("DrugPresc")]
public partial class DrugPresc
{
    public long SNo { get; set; }

    [Key]
    [StringLength(520)]
    [Unicode(false)]
    public string Description { get; set; } = null!;

    public long? Qty { get; set; }
}
