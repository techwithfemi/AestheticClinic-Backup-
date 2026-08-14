using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("EmpAuthorise")]
public partial class EmpAuthorise
{
    public int SNO { get; set; }

    [StringLength(50)]
    public string EmpID { get; set; } = null!;

    public int EmpAuth { get; set; }
}
