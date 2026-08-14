using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class hWard
{
    [StringLength(50)]
    public string WardID { get; set; } = null!;

    [StringLength(50)]
    public string WardName { get; set; } = null!;

    [StringLength(50)]
    public string Location { get; set; } = null!;
}
