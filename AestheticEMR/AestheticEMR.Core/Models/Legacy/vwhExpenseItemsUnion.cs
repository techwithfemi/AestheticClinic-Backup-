using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhExpenseItemsUnion
{
    [StringLength(255)]
    public string ItemName { get; set; } = null!;

    public long ItemCode { get; set; }

    [StringLength(50)]
    public string? CatCode { get; set; }
}
