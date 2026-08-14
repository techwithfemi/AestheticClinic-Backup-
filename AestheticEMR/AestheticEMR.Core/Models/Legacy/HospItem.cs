using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class HospItem
{
    [StringLength(50)]
    public string ItemName { get; set; } = null!;

    [StringLength(2)]
    public string? ItemCode { get; set; }
}
