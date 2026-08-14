using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwDocName
{
    [StringLength(50)]
    public string EmpID { get; set; } = null!;

    [StringLength(116)]
    public string DocName { get; set; } = null!;

    [StringLength(100)]
    public string? Designation { get; set; }

    [StringLength(18)]
    public string? LoginRole { get; set; }
}
