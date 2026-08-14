using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

public partial class hExpenseItem
{
    public long SNO { get; set; }

    [StringLength(3)]
    public string CatCode { get; set; } = null!;

    [Key]
    [StringLength(7)]
    public string ItemCode { get; set; } = null!;

    [StringLength(255)]
    public string ItemName { get; set; } = null!;

    [StringLength(250)]
    public string? Description { get; set; }

    [StringLength(50)]
    public string? Status { get; set; }
}
