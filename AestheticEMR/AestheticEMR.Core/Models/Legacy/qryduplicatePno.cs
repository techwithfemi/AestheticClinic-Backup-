using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryduplicatePno
{
    [StringLength(50)]
    public string pNo { get; set; } = null!;

    public int? tot { get; set; }
}
