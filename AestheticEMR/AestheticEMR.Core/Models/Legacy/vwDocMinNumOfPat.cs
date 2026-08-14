using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwDocMinNumOfPat
{
    public bool? attendedToByDoc { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [StringLength(65)]
    public string? Doctor { get; set; }

    [StringLength(50)]
    public string clinicID { get; set; } = null!;

    [StringLength(50)]
    public string EmpID { get; set; } = null!;

    public int? NumOfPat { get; set; }
}
