using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryIdgenEmpVal
{
    [StringLength(50)]
    public string empID { get; set; } = null!;

    [StringLength(4)]
    public string? empVal { get; set; }
}
