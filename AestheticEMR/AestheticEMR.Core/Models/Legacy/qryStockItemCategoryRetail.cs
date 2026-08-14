using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryStockItemCategoryRetail
{
    [StringLength(250)]
    public string CategoryCode { get; set; } = null!;

    [StringLength(250)]
    public string CategoryName { get; set; } = null!;

    [StringLength(150)]
    public string? Description { get; set; }
}
