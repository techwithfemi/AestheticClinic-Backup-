using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class EmployeeCatQry
{
    [StringLength(50)]
    public string catID { get; set; } = null!;

    [StringLength(50)]
    public string catName { get; set; } = null!;
}
