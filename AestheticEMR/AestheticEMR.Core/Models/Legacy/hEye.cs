using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hEye")]
public partial class hEye
{
    public int SNO { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime dDate { get; set; }

    [StringLength(50)]
    public string pNo { get; set; } = null!;

    [StringLength(100)]
    public string Reason { get; set; } = null!;

    [StringLength(100)]
    public string? Remarks { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? dTime { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [StringLength(50)]
    public string clientCat { get; set; } = null!;

    public bool? isDischarged { get; set; }
}
