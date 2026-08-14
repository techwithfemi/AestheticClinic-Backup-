using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class hBirth
{
    public int SNO { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime bDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? bTime { get; set; }

    [StringLength(50)]
    public string? WardID { get; set; }

    [StringLength(200)]
    public string? supervisedby { get; set; }

    [StringLength(50)]
    public string? sex { get; set; }

    [StringLength(50)]
    public string? weight { get; set; }

    [StringLength(50)]
    public string? height { get; set; }

    [StringLength(50)]
    public string? pNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ProbDischDate { get; set; }

    [StringLength(100)]
    public string? Remarks { get; set; }
}
