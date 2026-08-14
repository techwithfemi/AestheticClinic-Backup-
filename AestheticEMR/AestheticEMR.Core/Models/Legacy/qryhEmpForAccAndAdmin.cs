using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhEmpForAccAndAdmin
{
    [StringLength(50)]
    public string empID { get; set; } = null!;

    [StringLength(101)]
    public string empFullName { get; set; } = null!;

    [StringLength(50)]
    public string DeptID { get; set; } = null!;
}
