using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwEmpAuthorise
{
    public int SNO { get; set; }

    [StringLength(101)]
    public string EmpName { get; set; } = null!;

    [StringLength(50)]
    public string empID { get; set; } = null!;

    [StringLength(100)]
    public string Dept { get; set; } = null!;
}
