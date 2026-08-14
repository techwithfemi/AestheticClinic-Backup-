using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwCOGSandInventoryAcctNo
{
    [StringLength(500)]
    public string IDVal { get; set; } = null!;
}
