using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class Hospital
{
    [StringLength(3)]
    public string GroupCode { get; set; } = null!;

    [StringLength(225)]
    public string GroupName { get; set; } = null!;

    [StringLength(250)]
    public string? Address { get; set; }
}
