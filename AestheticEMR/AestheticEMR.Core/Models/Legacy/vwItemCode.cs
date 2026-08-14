using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwItemCode
{
    [StringLength(3)]
    public string catCode { get; set; } = null!;

    [StringLength(7)]
    public string ItemCode { get; set; } = null!;
}
