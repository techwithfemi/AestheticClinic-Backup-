using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwBatchNo
{
    [StringLength(8000)]
    [Unicode(false)]
    public string? BatchNo { get; set; }
}
