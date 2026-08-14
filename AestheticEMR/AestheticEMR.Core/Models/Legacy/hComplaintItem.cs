using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class hComplaintItem
{
    public long SNo { get; set; }

    [StringLength(250)]
    public string ItmName { get; set; } = null!;
}
