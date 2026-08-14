using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryIdgen
{
    [StringLength(50)]
    public string pNo { get; set; } = null!;

    [StringLength(9)]
    public string? idVal { get; set; }
}
