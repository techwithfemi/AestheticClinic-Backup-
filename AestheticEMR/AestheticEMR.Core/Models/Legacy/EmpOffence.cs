using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("EmpOffence")]
public partial class EmpOffence
{
    public int SNo { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? offDate { get; set; }

    [Column(TypeName = "ntext")]
    public string? offDetails { get; set; }

    [StringLength(50)]
    public string? EmpID { get; set; }

    [StringLength(50)]
    public string? OffCat { get; set; }

    [Column(TypeName = "ntext")]
    public string? AssoQuery { get; set; }

    [StringLength(50)]
    public string? IssuedBy { get; set; }
}
